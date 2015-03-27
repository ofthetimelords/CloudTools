// <copyright file="QueueExtensions.Parallel.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.Queues
{
	/// <summary>
	///     Provides helper methods for Azure Queues that enable leasing autoupdating.
	/// </summary>
	public static partial class QueueExtensions
	{
		/// <summary>
		///     Handles messages from the <paramref name="queue" /> in a parallel manner, in an endless loop.
		/// </summary>
		/// <param name="queue">The queue to check for messages.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <exception cref="ArgumentNullException">queue;Parameter 'queue' was null. or messageOptions;Parameter 'messageOptions' was not provided.</exception>
		/// <exception cref="ArgumentNullException">The messageOps parameter is null.</exception>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>.</para>
		/// </returns>
		[NotNull]
		public static async Task HandleMessagesInParallel([NotNull] this IQueue queue, [NotNull] HandleParallelMessageOptions messageOptions)
		{
			Guard.NotNull(queue, "queue");
			Guard.NotNull(messageOptions, "messageOptions");

			// Slots allowed by Azure
			const int maxSize = 32;

			// Used to allow to pass it as a reference
			long[] activeMessageSlots = {0};

			// The amount of time to wait to poll the internal queue for free threads
			var internalZeroThreadsWait = TimeSpan.FromSeconds(1);

			while (true)
			{
				try
				{
					if (messageOptions.CancelToken.IsCancellationRequested) return;

					messageOptions.QuickLogDebug("HandleMessages", "Ready to contact queue '{0}' for a batch of new messages (maximum '{1}')", queue.Name, messageOptions.MaximumCurrentMessages);
					var freeMessageSlots = messageOptions.MaximumCurrentMessages - (int) Interlocked.Read(ref activeMessageSlots[0]);

					if (freeMessageSlots == 0)
					{
						messageOptions.QuickLogDebug(
							"HandleMessages",
							"No free threads are available for processing messages in queue '{0}' (0 out of '{1}'), will try again on {2}",
							queue.Name,
							messageOptions.MaximumCurrentMessages,
							DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
						await Task.Delay(internalZeroThreadsWait);
						continue;
					}

					messageOptions.QuickLogDebug(
						"HandleMessages",
						"Ready to request a batch of messages on queue '{0}', '{1}' free slots out of {2}",
						queue.Name,
						freeMessageSlots,
						messageOptions.MaximumCurrentMessages);
					var messages = (await queue.GetMessagesAsync(freeMessageSlots > maxSize ? maxSize : freeMessageSlots, messageOptions.MessageLeaseTime, messageOptions.CancelToken)).ToList();

					if (messages.Count == 0)
					{
						messageOptions.QuickLogDebug(
							"HandleMessages",
							"No new messages are available for processing messages in queue '{0}' (0 out of '{1}'), will try again on {2}",
							queue.Name,
							messageOptions.MaximumCurrentMessages,
							DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
						await Task.Delay(messageOptions.PollFrequency);
						continue;
					}

					messageOptions.QuickLogDebug("HandleMessages", "A total of '{0}' messages were retrieved on this batch from queue '{1}'", messages.Count, queue.Name);

					foreach (var message in messages)
					{
						var messageSpecificCancellationTokenSource = new CancellationTokenSource();
						var comboCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(messageSpecificCancellationTokenSource.Token, messageOptions.CancelToken).Token;
						var currentMessage = message;

						if (currentMessage == null) continue;
						Interlocked.Increment(ref activeMessageSlots[0]);

						var actualTask = Task.Factory.StartNew(
							() =>
							{
								Task keepAliveTask = null;
								messageOptions.QuickLogDebug(
									"HandleMessages",
									"Started processing queue's '{0}' message with ID '{1}' ({2} slots remaining)",
									queue.Name,
									currentMessage.Id,
									messageOptions.MaximumCurrentMessages - Interlocked.Read(ref activeMessageSlots[0]));

								try
								{
									ProcessMessageInternal(
										new QueueMessageWrapper(queue, currentMessage, messageOptions.OverflowMessageContainer),
										queue,
										messageOptions,
										messageSpecificCancellationTokenSource,
										ref keepAliveTask);
								}
								catch (TaskCanceledException)
								{
									messageOptions.QuickLogDebug("HandleMessages", "The message checking task was cancelled on queue '{0}'", queue.Name);
									throw;
								}
								catch (CloudToolsStorageException ex)
								{
									HandleStorageExceptions(queue, messageOptions, ex);
								}
								catch (Exception ex)
								{
									HandleGeneralExceptions(queue, messageOptions, ex);
								}
								finally
								{
									ParallelFinallyHandler(queue, messageOptions, activeMessageSlots, keepAliveTask, currentMessage, messageSpecificCancellationTokenSource);
								}
							},
							comboCancellationToken,
							TaskCreationOptions.PreferFairness | TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning,
							TaskScheduler.Current);
					}
				}
				catch (TaskCanceledException)
				{
					throw;
				}
				catch (Exception ex)
				{
					HandleGeneralExceptions(queue, messageOptions, ex, true);
				}
			}
		}



		private static void ParallelFinallyHandler(
			[CanBeNull] IQueue queue,
			[CanBeNull] HandleParallelMessageOptions messageOptions,
			[CanBeNull] long[] activeMessageSlots,
			[CanBeNull] Task keepAliveTask,
			[CanBeNull] IQueueMessage currentMessage,
			[CanBeNull] CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			if (Guard.IsAnyNull(queue, messageOptions, activeMessageSlots, messageSpecificCancellationTokenSource)) return;

			Interlocked.Decrement(ref activeMessageSlots[0]);

			// Cancel any outstanding jobs due to the faulted operation (the keepalive task should have been cancelled)
			if (keepAliveTask != null && !keepAliveTask.IsCompleted)
			{
				if (currentMessage != null)
				{
					messageOptions.QuickLogDebug("HandleMessages", "Queue's '{0}' message '{1}', processing faulted; cancelling related jobs on {1}", queue.Name, currentMessage.Id);
					messageOptions.QuickLogDebug("HandleMessages", "Queue '{0}': currently active threads: {1}", queue.Name, Interlocked.Read(ref activeMessageSlots[0]));
				}
				messageSpecificCancellationTokenSource.Cancel();
			}
			else if (currentMessage != null) messageOptions.QuickLogDebug("HandleMessages", "Queue '{0}': currently active threads: {1}", queue.Name, Interlocked.Read(ref activeMessageSlots[0]));
		}
	}
}