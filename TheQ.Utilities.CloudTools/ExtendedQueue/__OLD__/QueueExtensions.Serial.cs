//// <copyright file="QueueExtensions.Serial.cs" company="nett">
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
//		public static async Task HandleMessages([NotNull] this IQueue queue, [NotNull] HandleSerialMessageOptions messageOptions)
//		{
//			Guard.NotNull(queue, "queue");
//			Guard.NotNull(messageOptions, "messageOptions");

//			while (true)
//			{
//				if (messageOptions.CancelToken.IsCancellationRequested) return;

//				// When set to true, the queue won't wait before it requests another message
//				var receivedMessage = false;

//				// Used to prevent a message operation from running on a specific message
//				var messageSpecificCancellationTokenSource = new CancellationTokenSource();

//				Task keepAliveTask = null;
//				IQueueMessage message = null;

//				try
//				{
//					messageOptions.QuickLogDebug("HandleMessages", "Attempting to listen for a new message in queue '{0}'", queue.Name);
//					message = await queue.GetMessageAsync(messageOptions.MessageLeaseTime, messageOptions.CancelToken);
//					if (message == null)
//					{
//						messageOptions.QuickLogDebug(
//							"HandleMessages",
//							"No message available in queue '{0}'. Will wait for '{1}' before retrying.",
//							queue.Name,
//							messageOptions.PollFrequency.ToString("g", CultureInfo.InvariantCulture));
//					}
//					else
//					{
//						messageOptions.QuickLogDebug("HandleMessages", "Started processing queue's '{0}' message's '{1}' metadata", queue.Name, message.Id);
//						receivedMessage = true;

//						ProcessMessageInternal(new QueueMessageWrapper(message, messageOptions.OverflowMessageContainer), queue, messageOptions, messageSpecificCancellationTokenSource, ref keepAliveTask);
//					}
//				}
//				catch (TaskCanceledException)
//				{
//					messageOptions.QuickLogDebug("HandleMessages", "The message checking task was cancelled on queue '{0}'", queue.Name);
//					throw;
//				}
//				catch (CloudToolsStorageException ex)
//				{
//					HandleStorageExceptions(queue, messageOptions, ex);
//				}
//				catch (Exception ex)
//				{
//					HandleGeneralExceptions(queue, messageOptions, ex);
//				}
//				finally
//				{
//					SerialFinallyHandler(queue, messageOptions, keepAliveTask, message, messageSpecificCancellationTokenSource);
//				}

//				// Delay the next polling attempt for a new message, since no messages were received last time.
//				if (!receivedMessage) await Task.Delay(messageOptions.PollFrequency);
//			}
//		}



//		private static void SerialFinallyHandler(
//			[CanBeNull] IQueue queue,
//			[CanBeNull] HandleSerialMessageOptions messageOptions,
//			[CanBeNull] Task keepAliveTask,
//			[CanBeNull] IQueueMessage message,
//			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource)
//		{
//			if (Guard.IsAnyNull(queue, messageOptions, messageSpecificCancellationTokenSource)) return;

//			// Cancel any outstanding jobs due to the faulted operation (the keepalive task should have been cancelled)
//			if (keepAliveTask != null && !keepAliveTask.IsCompleted)
//			{
//				if (message != null) messageOptions.QuickLogDebug("HandleMessages", "Queue's '{0}' message '{1}', processing faulted; cancelling related jobs", queue.Name, message.Id);

//				messageSpecificCancellationTokenSource.Cancel();
//			}
//		}
//	}
//}