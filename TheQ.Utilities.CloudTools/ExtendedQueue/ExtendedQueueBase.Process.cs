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
		//private readonly AsyncLock _lock = new AsyncLock();

		/// <summary>
		///     Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
		/// </summary>
		/// <param name="message">The message to operate on.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <param name="syncToken">A synchronisation token.</param>
		/// <param name="messageSpecificCancellationTokenSource">The message-specific cancellation token.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		/// <returns>
		///     True if the <paramref name="message" /> was deleted; <see langword="false" /> if it should be requeued and <see langword="checked" /> again.
		/// </returns>
		private async Task<bool> WasPoisonMessageAndRemoved(
			[NotNull] HandleMessagesSerialOptions messageOptions,
			[NotNull] QueueMessageWrapper message,
			[NotNull] object syncToken,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(messageOptions, "messageOptions");
			Guard.NotNull(message, "message");
			Guard.NotNull(syncToken, "syncToken");
			Guard.NotNull(messageSpecificCancellationTokenSource, "messageSpecificCancellationTokenSource");

			if (message.ActualMessage.DequeueCount <= messageOptions.PoisonMessageThreshold)
				return false;

			if (messageOptions.PoisonHandler != null && !(await messageOptions.PoisonHandler(message).ConfigureAwait(false)))
				return false;

			await this.This(invoker).SyncDeleteMessage(syncToken, message, messageSpecificCancellationTokenSource, this.This(invoker)).ConfigureAwait(false);

			return true;
		}



		/// <summary>
		///     <para>Deletes an <see cref="IQueueMessage" /></para>
		///     <para>under a synchronisation token and cancels related jobs (i.e. keep-alive operations).</para>
		/// </summary>
		/// <param name="syncToken">The synchronization token.</param>
		/// <param name="message">The message to delete.</param>
		/// <param name="messageSpecificCancellationTokenSource">The message-specific cancellation token.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		private async Task SyncDeleteMessage(
			[NotNull] object syncToken,
			[NotNull] QueueMessageWrapper message,
			[CanBeNull] CancellationTokenSource messageSpecificCancellationTokenSource,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(message, "message");
			Guard.NotNull(syncToken, "syncToken");

			if (messageSpecificCancellationTokenSource != null)
				messageSpecificCancellationTokenSource.Cancel();

			//using (await this._lock.LockAsync(messageSpecificCancellationTokenSource != null ? messageSpecificCancellationTokenSource.Token : CancellationToken.None))
			{
				// Cancel all other waiting operations before deleting.
				this.This(invoker).DeleteMessage(message.ActualMessage);

				if (await message.GetWasOverflownAsync().ConfigureAwait(false))
				{
					try
					{
						await this.This(invoker).RemoveOverflownContentsAsync(message, messageSpecificCancellationTokenSource.Token).ConfigureAwait(false);
					}
					catch (CloudToolsStorageException ex)
					{
						if (ex.StatusCode != 404 && ex.StatusCode != 409 && ex.StatusCode != 412)
							throw;
					}
				}
			}
		}



		/// <summary>
		///     Processes a queue message.
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		/// <param name="messageOptions">Initialisation options for the method that handles the messages.</param>
		/// <param name="messageSpecificCancellationTokenSource">A cancellation token source that's specific to this message.</param>
		/// <param name="keepAliveTask">A task that is responsible for keeping a <paramref name="message" /> alive while being processed.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		private async Task<Task> ProcessMessageInternal(
			[NotNull] QueueMessageWrapper message,
			[NotNull] HandleMessagesSerialOptions messageOptions,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(message, "message");
			Guard.NotNull(messageOptions, "messageOptions");
			Guard.NotNull(messageSpecificCancellationTokenSource, "messageSpecificCancellationTokenSource");

			var syncToken = new object();

			// Very old message; delete it and move to the next one
			if (messageOptions.TimeWindow.TotalSeconds > 0
				&& (!message.ActualMessage.InsertionTime.HasValue || message.ActualMessage.InsertionTime.Value.UtcDateTime.Add(messageOptions.TimeWindow) < DateTime.UtcNow))
			{
				await this.This(invoker).SyncDeleteMessage(syncToken, message, messageSpecificCancellationTokenSource, invoker).ConfigureAwait(false);
				return Task.FromResult<Task>(null);
			}

			// Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
			if (await this.This(invoker).WasPoisonMessageAndRemoved(messageOptions, message, syncToken, messageSpecificCancellationTokenSource, invoker).ConfigureAwait(false))
				return Task.FromResult<Task>(null);

			// Starts the background thread which ensures message leases stay fresh.
			var keepAliveTask = this.KeepMessageAlive(message.ActualMessage, messageOptions.MessageLeaseTime, messageSpecificCancellationTokenSource.Token, syncToken, this.This(invoker));
			using (var comboCancelToken = CancellationTokenSource.CreateLinkedTokenSource(messageSpecificCancellationTokenSource.Token, messageOptions.CancelToken))
			{
				// Execute the provided action and if successful, delete the message.
				if (await messageOptions.MessageHandler(message).ConfigureAwait(false))
					await this.This(invoker).SyncDeleteMessage(syncToken, message, comboCancelToken, invoker).ConfigureAwait(false);
			}

			return keepAliveTask;
		}



		/// <summary>
		///     Handles poison <paramref name="messages" /> by either delegating it to a handler or deleting them if no handler is provided.
		/// </summary>
		/// <param name="messages">The list of messages to operate on.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <param name="syncToken">A synchronisation token.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		/// <returns>
		///     Returns a list of <paramref name="messages" /> that were not poison <paramref name="messages" /> and should be further processed.
		/// </returns>
		private async Task<IList<QueueMessageWrapper>> WerePoisonMessagesAndRemovedBatch(
			[NotNull] HandleMessagesBatchOptions messageOptions,
			[NotNull] IList<QueueMessageWrapper> messages,
			[NotNull] object syncToken,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(messageOptions, "messageOptions");
			Guard.NotNull(messages, "messages");
			Guard.NotNull(syncToken, "syncToken");

			var poisonMessages = messages.Where(p => p.ActualMessage.DequeueCount > messageOptions.PoisonMessageThreshold).ToList();

			if (poisonMessages.Count == 0)
				return messages;

			var handledMessages = messageOptions.PoisonHandler != null ? await messageOptions.PoisonHandler(poisonMessages).ConfigureAwait(false) : new List<QueueMessageWrapper>(poisonMessages);


			foreach (var message in handledMessages)
				await this.This(invoker).SyncDeleteMessage(syncToken, message, null, invoker).ConfigureAwait(false);

			return messages.Except(handledMessages).ToList();
		}



		/// <summary>
		///     Processes <paramref name="messages" /> in batch.
		/// </summary>
		/// <param name="messages">The messages to be processed.</param>
		/// <param name="keepAliveTask">A task that is responsible for keeping a message alive while being processed.</param>
		/// <param name="batchCancellationToken">A cancellation token that's responsible for all tasks used in keep-alive.</param>
		/// <param name="messageOptions">Initialisation options for the method that handles the messages.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		private async Task<Task> ProcessMessageInternalBatch(
			[NotNull] IList<QueueMessageWrapper> messages,
			[NotNull] CancellationTokenSource batchCancellationToken,
			[NotNull] HandleMessagesBatchOptions messageOptions,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(messages, "messages");
			Guard.NotNull(messageOptions, "messageOptions");

			var syncToken = new object();

			var oldMessages = messageOptions.TimeWindow.TotalSeconds <= 0
				? new List<QueueMessageWrapper>()
				: messages.Where(m => !m.ActualMessage.InsertionTime.HasValue || m.ActualMessage.InsertionTime.Value.UtcDateTime.Add(messageOptions.TimeWindow) < DateTime.UtcNow).ToList();
			var timeValidMessages = messages.Except(oldMessages).ToList();

			// Very old messages; delete them and move to the next one
			if (oldMessages.Count > 0)
			{
				foreach (var message in messages)
					await this.This(invoker).SyncDeleteMessage(syncToken, message, null, invoker).ConfigureAwait(false);
			}

			// Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
			var toBeProcessedMessages = await this.This(invoker).WerePoisonMessagesAndRemovedBatch(messageOptions, timeValidMessages, syncToken, invoker).ConfigureAwait(false);

			if (toBeProcessedMessages.Count == 0)
				return Task.FromResult<Task>(null);

			var generalCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(batchCancellationToken.Token, messageOptions.CancelToken).Token;
			var keepAliveTask = this.This(invoker).KeepMessageAlive(messages, messageOptions.MessageLeaseTime, generalCancellationToken, syncToken, invoker);

			var handledMessages = await messageOptions.MessageHandler(messages).ConfigureAwait(false);

			batchCancellationToken.Cancel();

			// Execute the provided action and if successful, delete the message.
			foreach (var message in handledMessages)
				await this.This(invoker).SyncDeleteMessage(syncToken, message, null, invoker).ConfigureAwait(false);

			return keepAliveTask;
		}
	}
}