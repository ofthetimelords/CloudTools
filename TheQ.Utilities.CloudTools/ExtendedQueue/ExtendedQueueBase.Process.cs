// <copyright file="ExtendedQueueBase.Process.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/29</date>
// <summary>
// 
// </summary>

using System;
using System.Collections.Generic;
using System.Globalization;
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
		/// <param name="queue">The queue the <paramref name="message" /> belongs to.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <param name="syncToken">A synchronisation token.</param>
		/// <param name="messageSpecificCancellationTokenSource">The message-specific cancellation token.</param>
		/// <param name="loggingService">An object used for logging.</param>
		/// <returns>
		///     True if the <paramref name="message" /> was deleted; <see langword="false" /> if it should be requeued and <see langword="checked" /> again.
		/// </returns>
		private bool WasPoisonMessageAndRemoved(
			[NotNull] HandleSerialMessageOptions messageOptions,
			[NotNull] QueueMessageWrapper message,
			[NotNull] object syncToken,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(messageOptions, "messageOptions");
			Guard.NotNull(message, "message");
			Guard.NotNull(syncToken, "syncToken");
			Guard.NotNull(messageSpecificCancellationTokenSource, "messageSpecificCancellationTokenSource");

			if (message.ActualMessage.DequeueCount <= messageOptions.PoisonMessageThreshold) return false;

			//messageOptions.QuickLogDebug(
			//	"HandleMessages",
			//	"Queue's '{0}' message '{1}' was identified as a poison message ({2} out of {3} dequeue counts) ",
			//	queue.Name,
			//	message.ActualMessage.Id,
			//	message.ActualMessage.DequeueCount,
			//	messageOptions.PoisonMessageThreshold);

			if (messageOptions.PoisonHandler != null && !messageOptions.PoisonHandler(message)) return false;

//			messageOptions.QuickLogDebug("HandleMessages", "Deleting queue's '{0}' poison message '{1}'", queue.Name, message.ActualMessage.Id);
			this.Get(invoker).SyncDeleteMessage(syncToken, message, messageSpecificCancellationTokenSource, this.Get(invoker));

			return true;
		}



		/// <summary>
		///     <para>Deletes an <see cref="IQueueMessage" /></para>
		///     <para>under a synchronisation token and cancels related jobs (i.e. keep-alive operations).</para>
		/// </summary>
		/// <param name="syncToken">The synchronization token.</param>
		/// <param name="message">The message to delete.</param>
		/// <param name="messageSpecificCancellationTokenSource">The message-specific cancellation token.</param>
		private void SyncDeleteMessage(
			[NotNull] object syncToken,
			[NotNull] QueueMessageWrapper message,
			[CanBeNull] CancellationTokenSource messageSpecificCancellationTokenSource,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(message, "message");
			Guard.NotNull(syncToken, "syncToken");

			lock (syncToken)
			{
				// Cancel all other waiting operations before deleting.
				//				loggingService.QuickLogDebug("HandleMessages", "Queue's '{0}' message '{1}' scheduled for deletion, cancelling other related tasks prior to deletion", queue.Name, message.ActualMessage.Id);

				if (messageSpecificCancellationTokenSource != null) messageSpecificCancellationTokenSource.Cancel();
				this.Get(invoker).DeleteMessage(message.ActualMessage);

				if (message.WasOverflown)
				{
					//loggingService.QuickLogDebug(
					//	"HandleMessages",
					//	"Queue's '{0}' message '{1}' was overflown. Attempting to remove the related BLOB with overflow ID '{2}'",
					//	queue.Name,
					//	message.ActualMessage.Id,
					//	message.OverflowId);

					try
					{
						this.Get(invoker).RemoveOverflownContentsAsync(message, messageSpecificCancellationTokenSource.Token).Wait(messageSpecificCancellationTokenSource.Token);
					}
					catch (CloudToolsStorageException ex)
					{
						if (ex.StatusCode != 404 && ex.StatusCode != 409 && ex.StatusCode != 412)
							throw;
					}
				}

				//loggingService.QuickLogDebug("HandleMessages", "Message {0}, deleted on {1}", message.ActualMessage.Id, DateTimeOffset.Now.ToString("O", CultureInfo.InvariantCulture));
			}
		}



		/// <summary>
		///     Processes a <paramref name="queue" /> message.
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		/// <param name="queue">The queue the <paramref name="message" /> belongs to.</param>
		/// <param name="messageOptions">Initialisation options for the method that handles the messages.</param>
		/// <param name="messageSpecificCancellationTokenSource">A cancellation token source that's specific to this message.</param>
		/// <param name="keepAliveTask">A task that is responsible for keeping a <paramref name="message" /> alive while being processed.</param>
		private void ProcessMessageInternal(
			[NotNull] QueueMessageWrapper message,
			[NotNull] HandleSerialMessageOptions messageOptions,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource,
			[CanBeNull] ref Task keepAliveTask,
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
				//messageOptions.QuickLogDebug(
				//	"HandleMessages",
				//	"Queue's '{0}' message '{1}' exceeded the allowed time window and will be deleted (was inserted on '{2}' and the maximum allowed time window was '{3}'",
				//	queue.Name,
				//	message.ActualMessage.Id,
				//	message.ActualMessage.InsertionTime.HasValue ? message.ActualMessage.InsertionTime.Value.ToString("O", CultureInfo.InvariantCulture) : "<unknown>",
				//	messageOptions.TimeWindow.ToString("g", CultureInfo.InvariantCulture));

				this.Get(invoker).SyncDeleteMessage(syncToken, message, messageSpecificCancellationTokenSource, invoker);
				return;
			}

			// Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
			if (this.Get(invoker).WasPoisonMessageAndRemoved(messageOptions, message, syncToken, messageSpecificCancellationTokenSource, invoker)) return;

			// Starts the background thread which ensures message leases stay fresh.
			keepAliveTask = this.KeepMessageAlive(message.ActualMessage, messageOptions.MessageLeaseTime, messageSpecificCancellationTokenSource.Token, syncToken, this.Get(invoker));
			using (var comboCancelToken = CancellationTokenSource.CreateLinkedTokenSource(messageSpecificCancellationTokenSource.Token, messageOptions.CancelToken))
			{
				//messageOptions.QuickLogDebug("HandleMessages", "Calling handler ('{2}') for message queue's '{1}' message '{0}'", message.ActualMessage.Id, queue.Name, messageOptions.MessageHandler.Method.Name);

				// Execute the provided action and if successful, delete the message.
				if (messageOptions.MessageHandler(message)) this.Get(invoker).SyncDeleteMessage(syncToken, message, comboCancelToken, invoker);
			}
		}



		/// <summary>
		///     Handles poison <paramref name="messages" /> by either delegating it to a handler or deleting them if no handler is provided.
		/// </summary>
		/// <param name="messages">The list of messages to operate on.</param>
		/// <param name="queue">The queue the message belongs to.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <param name="syncToken">A synchronisation token.</param>
		/// <param name="loggingService">An object used for logging.</param>
		/// <returns>
		///     Returns a list of <paramref name="messages" /> that were not poison <paramref name="messages" /> and should be further processed.
		/// </returns>
		private IList<QueueMessageWrapper> WerePoisonMessagesAndRemovedBatch(
			[NotNull] HandleBatchMessageOptions messageOptions,
			[NotNull] IList<QueueMessageWrapper> messages,
			[NotNull] object syncToken,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(messageOptions, "messageOptions");
			Guard.NotNull(messages, "messages");
			Guard.NotNull(syncToken, "syncToken");

			var poisonMessages = messages.Where(p => p.ActualMessage.DequeueCount > messageOptions.PoisonMessageThreshold).ToList();

			if (poisonMessages.Count == 0) return messages;

			//messageOptions.QuickLogDebug("HandleBatchMessages", "Queue's '{0}' {1} messages were identified as poison message(s)", queue.Name, poisonMessages.Count);

			var handledMessages = messageOptions.PoisonHandler != null ? messageOptions.PoisonHandler(poisonMessages) : new List<QueueMessageWrapper>(poisonMessages);

			//messageOptions.QuickLogDebug("HandleMessages", "Deleting queue's '{0}' poison messages", queue.Name);

			foreach (var message in handledMessages) this.Get(invoker).SyncDeleteMessage(syncToken, message, null, invoker);

			return messages.Except(handledMessages).ToList();
		}



		/// <summary>
		///     Processes <paramref name="queue" /> <paramref name="messages" /> in batch.
		/// </summary>
		/// <param name="messages">The messages to be processed.</param>
		/// <param name="queue">The queue the <paramref name="messages" /> belong to.</param>
		/// <param name="keepAliveTask">A task that is responsible for keeping a message alive while being processed.</param>
		/// <param name="batchCancellationToken">A cancellation token that's responsible for all tasks used in keep-alive.</param>
		/// <param name="messageOptions">Initialisation options for the method that handles the messages.</param>
		private void ProcessMessageInternalBatch(
			[NotNull] IList<QueueMessageWrapper> messages,
			[CanBeNull] ref Task keepAliveTask,
			[NotNull] CancellationTokenSource batchCancellationToken,
			[NotNull] HandleBatchMessageOptions messageOptions,
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
//				messageOptions.QuickLogDebug("HandleBatchMessages", "Queue's '{0}' {1} messages exceeded the allowed time window and will be deleted", queue.Name, oldMessages.Count);

				foreach (var message in messages) this.Get(invoker).SyncDeleteMessage(syncToken, message, null, invoker);
			}

			// Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
			var toBeProcessedMessages = this.Get(invoker).WerePoisonMessagesAndRemovedBatch(messageOptions, timeValidMessages, syncToken, invoker);

			if (toBeProcessedMessages.Count == 0) return;

			var generalCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(batchCancellationToken.Token, messageOptions.CancelToken).Token;
			keepAliveTask = this.Get(invoker).KeepMessageAlive(messages, messageOptions.MessageLeaseTime, generalCancellationToken, syncToken, invoker);

			//messageOptions.QuickLogDebug("HandleMessages", "Calling handler ('{1}') for message queue's {0} messages", toBeProcessedMessages.Count, queue.Name);
			var handledMessages = messageOptions.MessageHandler(messages);

			batchCancellationToken.Cancel();

			// Execute the provided action and if successful, delete the message.
			foreach (var message in handledMessages)
				this.Get(invoker).SyncDeleteMessage(syncToken, message, null, invoker);
		}
	}
}