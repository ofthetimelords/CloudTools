using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public abstract partial class ExtendedQueueBase
	{
		/// <summary>
		///     Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
		/// </summary>
		/// <param name="message">The message to operate on.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <param name="asyncLock">An object that's responsible for synchronising access to shared resources in an asynchronous manner.</param>
		/// <param name="messageSpecificCancellationTokenSource">The message-specific cancellation token.</param>
		/// <returns>
		///     True if the <paramref name="message" /> was deleted; <see langword="false" /> if it should be requeued and <see langword="checked" /> again.
		/// </returns>
		private async Task<bool> WasPoisonMessageAndRemoved(
			[NotNull] HandleMessagesSerialOptions messageOptions,
			[NotNull] QueueMessageWrapper message,
			[NotNull] AsyncLock asyncLock,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			Guard.NotNull(messageOptions, "messageOptions");
			Guard.NotNull(message, "message");
			Guard.NotNull(asyncLock, "asyncLock");
			Guard.NotNull(messageSpecificCancellationTokenSource, "messageSpecificCancellationTokenSource");

			if (message.ActualMessage.DequeueCount <= messageOptions.PoisonMessageThreshold)
				return false;

			if (messageOptions.PoisonHandler != null && !(await messageOptions.PoisonHandler(message).ConfigureAwait(false)))
				return false;

			this.Statistics.IncreasePoisonMessages();

			await this.Top.SyncDeleteMessage(asyncLock, message, messageSpecificCancellationTokenSource).ConfigureAwait(false);

			return true;
		}



		/// <summary>
		///     <para>Deletes an <see cref="IQueueMessage" /></para>
		///     <para>under a synchronisation token and cancels related jobs (i.e. keep-alive operations).</para>
		/// </summary>
		/// <param name="asyncLock">An object that's responsible for synchronising access to shared resources in an asynchronous manner.</param>
		/// <param name="message">The message to delete.</param>
		/// <param name="messageSpecificCancellationTokenSource">The message-specific cancellation token.</param>
		private async Task SyncDeleteMessage(
			[NotNull] AsyncLock asyncLock,
			[NotNull] QueueMessageWrapper message,
			[CanBeNull] CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			Guard.NotNull(message, "message");
			Guard.NotNull(asyncLock, "asyncLock");


			using (await asyncLock.LockAsync())//messageSpecificCancellationTokenSource != null ? messageSpecificCancellationTokenSource.Token : CancellationToken.None))
			{
				// Cancel all other waiting operations before deleting.
				this.Top.DeleteMessage(message.ActualMessage);

				if (await message.GetWasOverflownAsync().ConfigureAwait(false))
				{
					try
					{
						await this.Top.RemoveOverflownContentsAsync(message, messageSpecificCancellationTokenSource.Token).ConfigureAwait(false);
					}
					catch (CloudToolsStorageException ex)
					{
						if (ex.StatusCode != 404 && ex.StatusCode != 409 && ex.StatusCode != 412)
							throw;
					}
				}

				if (messageSpecificCancellationTokenSource != null)
					messageSpecificCancellationTokenSource.Cancel();
			}
		}



		/// <summary>
		///     Processes a queue message.
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		/// <param name="messageOptions">Initialisation options for the method that handles the messages.</param>
		/// <param name="messageSpecificCancellationTokenSource">A cancellation token source that's specific to this message.</param>
		/// <param name="asyncLock">An object that's responsible for synchronising access to shared resources in an asynchronous manner.</param>
		private async Task<Task> ProcessMessageInternal(
			[NotNull] QueueMessageWrapper message,
			[NotNull] HandleMessagesSerialOptions messageOptions,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			Guard.NotNull(message, "message");
			Guard.NotNull(messageOptions, "messageOptions");
			Guard.NotNull(messageSpecificCancellationTokenSource, "messageSpecificCancellationTokenSource");

			var asynclock = new AsyncLock();


			// Very old message; delete it and move to the next one
			if (messageOptions.TimeWindow.TotalSeconds > 0
				&& (!message.ActualMessage.InsertionTime.HasValue || message.ActualMessage.InsertionTime.Value.UtcDateTime.Add(messageOptions.TimeWindow) < DateTime.UtcNow))
			{
				await this.Top.SyncDeleteMessage(asynclock, message, messageSpecificCancellationTokenSource).ConfigureAwait(false);
				return Task.FromResult<Task>(null);
			}

			// Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
			if (await this.Top.WasPoisonMessageAndRemoved(messageOptions, message, asynclock, messageSpecificCancellationTokenSource).ConfigureAwait(false))
				return Task.FromResult<Task>(null);

			// Starts the background thread which ensures message leases stay fresh.
			var keepAliveTask = this.KeepMessageAlive(message.ActualMessage, messageOptions.MessageLeaseTime, messageSpecificCancellationTokenSource.Token, asynclock);
			using (var comboCancelToken = CancellationTokenSource.CreateLinkedTokenSource(messageSpecificCancellationTokenSource.Token, messageOptions.CancelToken))
			{
				// Execute the provided action and if successful, delete the message.
				if (await messageOptions.MessageHandler(message).ConfigureAwait(false))
				{
					this.Statistics.IncreaseSuccessfulMessages();
					await this.Top.SyncDeleteMessage(asynclock, message, comboCancelToken).ConfigureAwait(false);
				} else
					this.Statistics.IncreaseReenqueuesCount();
			}


			messageSpecificCancellationTokenSource.Cancel();
			return keepAliveTask;
		}



		/// <summary>
		///     Handles poison <paramref name="messages" /> by either delegating it to a handler or deleting them if no handler is provided.
		/// </summary>
		/// <param name="messages">The list of messages to operate on.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <param name="asyncLock">An object that's responsible for synchronising access to shared resources in an asynchronous manner.</param>
		/// <returns>
		///     Returns a list of <paramref name="messages" /> that were not poison <paramref name="messages" /> and should be further processed.
		/// </returns>
		private async Task<IList<QueueMessageWrapper>> WerePoisonMessagesAndRemovedBatch(
			[NotNull] HandleMessagesBatchOptions messageOptions,
			[NotNull] IList<QueueMessageWrapper> messages,
			[NotNull] AsyncLock asyncLock)
		{
			Guard.NotNull(messageOptions, "messageOptions");
			Guard.NotNull(messages, "messages");
			Guard.NotNull(asyncLock, "asyncLock");

			var poisonMessages = messages.Where(p => p.ActualMessage.DequeueCount > messageOptions.PoisonMessageThreshold).ToList();

			if (poisonMessages.Count == 0)
				return messages;

			var handledMessages = messageOptions.PoisonHandler != null ? await messageOptions.PoisonHandler(poisonMessages).ConfigureAwait(false) : new List<QueueMessageWrapper>(poisonMessages);


			foreach (var message in handledMessages)
				await this.Top.SyncDeleteMessage(asyncLock, message, null).ConfigureAwait(false);


			this.Statistics.IncreasePoisonMessages(poisonMessages.Count);

			return messages.Except(handledMessages).ToList();
		}



		/// <summary>
		///     Processes <paramref name="messages" /> in batch.
		/// </summary>
		/// <param name="messages">The messages to be processed.</param>
		/// <param name="asyncLock">An object that's responsible for synchronising access to shared resources in an asynchronous manner.</param>
		/// <param name="batchCancellationToken">A cancellation token that's responsible for all tasks used in keep-alive.</param>
		/// <param name="messageOptions">Initialisation options for the method that handles the messages.</param>
		private async Task<Task> ProcessMessageInternalBatch(
			[NotNull] IList<QueueMessageWrapper> messages,
			[NotNull] CancellationTokenSource batchCancellationToken,
			[NotNull] HandleMessagesBatchOptions messageOptions)
		{
			Guard.NotNull(messages, "messages");
			Guard.NotNull(messageOptions, "messageOptions");

			var asynclock = new AsyncLock();

			var oldMessages = messageOptions.TimeWindow.TotalSeconds <= 0
				? new List<QueueMessageWrapper>()
				: messages.Where(m => !m.ActualMessage.InsertionTime.HasValue || m.ActualMessage.InsertionTime.Value.UtcDateTime.Add(messageOptions.TimeWindow) < DateTime.UtcNow).ToList();
			var timeValidMessages = messages.Except(oldMessages).ToList();

			// Very old messages; delete them and move to the next one
			if (oldMessages.Count > 0)
			{
				foreach (var message in messages)
					await this.Top.SyncDeleteMessage(asynclock, message, null).ConfigureAwait(false);
			}

			// Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
			var toBeProcessedMessages = await this.Top.WerePoisonMessagesAndRemovedBatch(messageOptions, timeValidMessages, asynclock).ConfigureAwait(false);

			if (toBeProcessedMessages.Count == 0)
				return Task.FromResult<Task>(null);

			var generalCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(batchCancellationToken.Token, messageOptions.CancelToken).Token;
			var keepAliveTask = this.Top.KeepMessageAlive(messages, messageOptions.MessageLeaseTime, generalCancellationToken, asynclock);

			var handledMessages = await messageOptions.MessageHandler(messages).ConfigureAwait(false);
			this.Statistics.IncreaseSuccessfulMessages(handledMessages.Count);
			batchCancellationToken.Cancel();

			// Execute the provided action and if successful, delete the message.
			foreach (var message in handledMessages)
				await this.Top.SyncDeleteMessage(asynclock, message, null).ConfigureAwait(false);

			var nonHandledMessages = messages.Except(handledMessages).Count();
			this.Statistics.IncreaseReenqueuesCount(nonHandledMessages);


			return keepAliveTask;
		}
	}
}