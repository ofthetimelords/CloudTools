//// <copyright file="QueueExtensions.Batch.cs" company="nett">
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
//using System.Globalization;
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
//		///     Handles messages from the <paramref name="queue" /> in a serial manner, in an endless loop.
//		/// </summary>
//		/// <param name="queue">The queue to check for messages.</param>
//		/// <param name="messageOptions">Initialisation options for this method.</param>
//		/// <exception cref="ArgumentNullException">The messageOps parameter is null.</exception>
//		/// <returns>
//		///     <para>A <see cref="Task" /></para>
//		///     <para>.</para>
//		/// </returns>
//		[NotNull]
//		public static async Task HandleMessagesInBatch([NotNull] this IQueue queue, [NotNull] HandleBatchMessageOptions messageOptions)
//		{
//			Guard.NotNull(queue, "queue");
//			Guard.NotNull(messageOptions, "messageOptions");

//			const int maxSize = 32;

//			while (true)
//			{
//				if (messageOptions.CancelToken.IsCancellationRequested) return;

//				// When set to true, the queue won't wait before it requests another message
//				var shouldDelayNextRequest = false;

//				// Used to prevent a message operation from running on a specific message
//				var rawMessages = new List<IQueueMessage>();
//				var convertedMessages = new List<QueueMessageWrapper>();
//				var batchCancellationToken = new CancellationTokenSource();
//				Task keepAliveTask = null;

//				while (true)
//				{
//					try
//					{
//						messageOptions.QuickLogDebug("HandleMessages", "Attempting to listen for a new message in queue '{0}'", queue.Name);

//						var howManyMoreFit = messageOptions.MaximumCurrentMessages - rawMessages.Count;
//						var retrievedMessages = (await queue.GetMessagesAsync(howManyMoreFit > maxSize ? maxSize : howManyMoreFit, messageOptions.MessageLeaseTime, messageOptions.CancelToken)).ToList();

//						if (retrievedMessages.Count == 0 && rawMessages.Count == 0)
//						{
//							// No buffered messages, and none were retrieved from the cache.
//							messageOptions.QuickLogDebug(
//								"HandleBatchMessages",
//								"No message available in queue '{0}'. Will wait for '{1}' before retrying.",
//								queue.Name,
//								messageOptions.PollFrequency.ToString("g", CultureInfo.InvariantCulture));

//							shouldDelayNextRequest = true;
//						}
//						else
//						{
//							// Keep trying to retrieve messages until there are no more or the quota is reached
//							if (retrievedMessages.Count > 0)
//							{
//								rawMessages.AddRange(retrievedMessages);

//								if (rawMessages.Count < messageOptions.MaximumCurrentMessages) continue;
//							}

//							// Have buffered messages and optionally some were retrieved from the cache. Proceed normally.
//							convertedMessages.AddRange(rawMessages.Select(m => new QueueMessageWrapper(queue, m, messageOptions.OverflowMessageContainer)));
//							messageOptions.QuickLogDebug("HandleBatchMessages", "Started processing queue's '{0}' {1} messages", queue.Name, rawMessages.Count);

//							ProcessMessageInternalBatch(convertedMessages, queue, ref keepAliveTask, batchCancellationToken, messageOptions);

//							break;
//						}
//					}
//					catch (TaskCanceledException)
//					{
//						messageOptions.QuickLogDebug("HandleMessages", "The message checking task was cancelled on queue '{0}'", queue.Name);
//						throw;
//					}
//					catch (CloudToolsStorageException ex)
//					{
//						HandleStorageExceptions(queue, messageOptions, ex);
//					}
//					catch (Exception ex)
//					{
//						HandleGeneralExceptions(queue, messageOptions, ex);
//					}
//					finally
//					{
//						BatchFinallyHandler(queue, messageOptions, keepAliveTask, batchCancellationToken);
//					}

//					// Delay the next polling attempt for a new message, since no messages were received last time.
//					if (shouldDelayNextRequest) await Task.Delay(messageOptions.PollFrequency);
//				}
//			}
//		}



//		private static void BatchFinallyHandler(
//			[CanBeNull] IQueue queue,
//			[CanBeNull] HandleBatchMessageOptions messageOptions,
//			[CanBeNull] Task keepAliveTask,
//			[NotNull] CancellationTokenSource batchCancelToken)
//		{
//			if (Guard.IsAnyNull(queue, messageOptions)) return;
//			Guard.NotNull(batchCancelToken, "generalCancelToken");

//			// Cancel any outstanding jobs. Since we don't have separate threads per message as in the serial processing, this indicates no faulted processing.
//			if (keepAliveTask != null && !keepAliveTask.IsCompleted)
//			{
//				messageOptions.QuickLogDebug("HandleBatchMessages", "Queue's '{0}' batch messages' processing faulted; cancelling related jobs", queue.Name);
//				batchCancelToken.Cancel();
//			}
//		}
//	}
//}