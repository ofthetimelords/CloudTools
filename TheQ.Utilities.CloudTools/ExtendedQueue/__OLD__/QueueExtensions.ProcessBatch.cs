//// <copyright file="QueueExtensions.ProcessBatch.cs" company="nett">
////      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
////      Please see the License.txt file for more information. All other rights reserved.
//// </copyright>
//// <author>James Kavakopoulos</author>
//// <email>ofthetimelords@gmail.com</email>
//// <date>2015/02/06</date>
//// <summary>
//// 
//// </summary>

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
//using TheQ.Utilities.CloudTools.Storage.Infrastructure;
//using TheQ.Utilities.CloudTools.Storage.Internal;
//using TheQ.Utilities.CloudTools.Storage.Models;
//using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



//namespace TheQ.Utilities.CloudTools.Storage.Queues
//{
//	/// <summary>
//	///     Provides helper methods for Azure Queues that enable leasing autoupdating.
//	/// </summary>
//	public static partial class QueueExtensions
//	{
//		/// <summary>
//		///     Handles poison <paramref name="messages" /> by either delegating it to a handler or deleting them if no handler is provided.
//		/// </summary>
//		/// <param name="messages">The list of messages to operate on.</param>
//		/// <param name="queue">The queue the message belongs to.</param>
//		/// <param name="messageOptions">Initialisation options for this method.</param>
//		/// <param name="syncToken">A synchronisation token.</param>
//		/// <param name="loggingService">An object used for logging.</param>
//		/// <returns>
//		///     Returns a list of <paramref name="messages" /> that were not poison <paramref name="messages" /> and should be further processed.
//		/// </returns>
//		private static IList<QueueMessageWrapper> WerePoisonMessagesAndRemovedBatch(
//			[NotNull] IQueue queue,
//			[NotNull] HandleBatchMessageOptions messageOptions,
//			[NotNull] IList<QueueMessageWrapper> messages,
//			[NotNull] object syncToken,
//			[CanBeNull] ILogService loggingService)
//		{
//			Guard.NotNull(queue, "queue");
//			Guard.NotNull(messageOptions, "messageOptions");
//			Guard.NotNull(messages, "messages");
//			Guard.NotNull(syncToken, "syncToken");

//			var poisonMessages = messages.Where(p => p.ActualMessage.DequeueCount > messageOptions.PoisonMessageThreshold).ToList();

//			if (poisonMessages.Count == 0) return messages;

//			messageOptions.QuickLogDebug("HandleBatchMessages", "Queue's '{0}' {1} messages were identified as poison message(s)", queue.Name, poisonMessages.Count);

//			var handledMessages = messageOptions.PoisonHandler != null ? messageOptions.PoisonHandler(poisonMessages) : new List<QueueMessageWrapper>(poisonMessages);

//			messageOptions.QuickLogDebug("HandleMessages", "Deleting queue's '{0}' poison messages", queue.Name);

//			foreach (var message in handledMessages) SyncDeleteMessage(queue, syncToken, message, messageOptions.OverflowMessageContainer, null, loggingService);

//			return messages.Except(handledMessages).ToList();
//		}



//		/// <summary>
//		///     Processes <paramref name="queue" /> <paramref name="messages" /> in batch.
//		/// </summary>
//		/// <param name="messages">The messages to be processed.</param>
//		/// <param name="queue">The queue the <paramref name="messages" /> belong to.</param>
//		/// <param name="keepAliveTask">A task that is responsible for keeping a message alive while being processed.</param>
//		/// <param name="batchCancellationToken">A cancellation token that's responsible for all tasks used in keep-alive.</param>
//		/// <param name="messageOptions">Initialisation options for the method that handles the messages.</param>
//		private static void ProcessMessageInternalBatch(
//			[NotNull] IList<QueueMessageWrapper> messages,
//			[NotNull] IQueue queue,
//			[CanBeNull] ref Task keepAliveTask,
//			[NotNull] CancellationTokenSource batchCancellationToken,
//			[NotNull] HandleBatchMessageOptions messageOptions)
//		{
//			Guard.NotNull(queue, "queue");
//			Guard.NotNull(messages, "messages");
//			Guard.NotNull(messageOptions, "messageOptions");

//			var syncToken = new object();

//			var oldMessages = messageOptions.TimeWindow.TotalSeconds <= 0
//				? new List<QueueMessageWrapper>()
//				: messages.Where(m => !m.ActualMessage.InsertionTime.HasValue || m.ActualMessage.InsertionTime.Value.UtcDateTime.Add(messageOptions.TimeWindow) < DateTime.UtcNow).ToList();
//			var timeValidMessages = messages.Except(oldMessages).ToList();

//			// Very old messages; delete them and move to the next one
//			if (oldMessages.Count > 0)
//			{
//				messageOptions.QuickLogDebug("HandleBatchMessages", "Queue's '{0}' {1} messages exceeded the allowed time window and will be deleted", queue.Name, oldMessages.Count);

//				foreach (var message in messages) SyncDeleteMessage(queue, syncToken, message, messageOptions.OverflowMessageContainer, null, messageOptions.LogService);
//			}

//			// Handles poison messages by either delegating it to a handler or deleting it if no handler is provided.
//			var toBeProcessedMessages = WerePoisonMessagesAndRemovedBatch(queue, messageOptions, timeValidMessages, syncToken, messageOptions.LogService);

//			if (toBeProcessedMessages.Count == 0) return;

//			var generalCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(batchCancellationToken.Token, messageOptions.CancelToken).Token;
//			keepAliveTask = messages.KeepMessageAlive(queue, messageOptions.MessageLeaseTime, generalCancellationToken, syncToken, messageOptions.LogService);

//			messageOptions.QuickLogDebug("HandleMessages", "Calling handler ('{1}') for message queue's {0} messages", toBeProcessedMessages.Count, queue.Name);
//			var handledMessages = messageOptions.MessageHandler(messages);

//			batchCancellationToken.Cancel();

//			// Execute the provided action and if successful, delete the message.
//			foreach (var message in handledMessages) SyncDeleteMessage(queue, syncToken, message, messageOptions.OverflowMessageContainer, null, messageOptions.LogService);
//		}
//	}
//}