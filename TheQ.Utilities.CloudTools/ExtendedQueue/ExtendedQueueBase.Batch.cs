using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
		public Task HandleMessagesInBatchAsync([NotNull] HandleBatchMessageOptions messageOptions) { return this.HandleMessagesInBatchAsync(messageOptions, this); }


		/// <summary>
		///     Handles messages from the <paramref name="queue" /> in a serial manner, in an endless loop.
		/// </summary>
		/// <param name="queue">The queue to check for messages.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <exception cref="ArgumentNullException">The messageOps parameter is null.</exception>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>.</para>
		/// </returns>
		[NotNull]
		internal async Task HandleMessagesInBatchAsync([NotNull] HandleBatchMessageOptions messageOptions, ExtendedQueueBase invoker)
		{
			Guard.NotNull(messageOptions, "messageOptions");

			while (true)
			{
				if (messageOptions.CancelToken.IsCancellationRequested) return;

				// When set to true, the queue won't wait before it requests another message
				var shouldDelayNextRequest = false;

				// Used to prevent a message operation from running on a specific message
				var rawMessages = new List<IQueueMessage>();
				var convertedMessages = new List<QueueMessageWrapper>();
				var batchCancellationToken = new CancellationTokenSource();
				Task keepAliveTask = null;

				while (true)
				{
					try
					{
						//messageOptions.QuickLogDebug("HandleMessages", "Attempting to listen for a new message in queue '{0}'", queue.Name);

						var howManyMoreFit = messageOptions.MaximumCurrentMessages - rawMessages.Count;
						var retrievedMessages = (await this.Get(invoker).GetMessagesAsync(howManyMoreFit > this.MaximumMessagesProvider.MaximumMessagesPerRequest
							? this.MaximumMessagesProvider.MaximumMessagesPerRequest
							: howManyMoreFit, messageOptions.MessageLeaseTime, messageOptions.CancelToken).ConfigureAwait(false)).ToList();

						if (retrievedMessages.Count == 0 && rawMessages.Count == 0)
						{
							// No buffered messages, and none were retrieved from the cache.
							//messageOptions.QuickLogDebug(
							//	"HandleBatchMessages",
							//	"No message available in queue '{0}'. Will wait for '{1}' before retrying.",
							//	queue.Name,
							//	messageOptions.PollFrequency.ToString("g", CultureInfo.InvariantCulture));

							shouldDelayNextRequest = true;
						}
						else
						{
							// Keep trying to retrieve messages until there are no more or the quota is reached
							if (retrievedMessages.Count > 0)
							{
								rawMessages.AddRange(retrievedMessages);

								if (rawMessages.Count < messageOptions.MaximumCurrentMessages) continue;
							}

							// Have buffered messages and optionally some were retrieved from the cache. Proceed normally.
							convertedMessages.AddRange(rawMessages.Select(m => new QueueMessageWrapper(this.Get(invoker), m)));
							//messageOptions.QuickLogDebug("HandleBatchMessages", "Started processing queue's '{0}' {1} messages", queue.Name, rawMessages.Count);

							this.ProcessMessageInternalBatch(convertedMessages, ref keepAliveTask, batchCancellationToken, messageOptions, invoker);

							break;
						}
					}
					catch (TaskCanceledException)
					{
						//messageOptions.QuickLogDebug("HandleMessages", "The message checking task was cancelled on queue '{0}'", queue.Name);
					}
					catch (CloudToolsStorageException ex)
					{
						this.Get(invoker).HandleStorageExceptions(messageOptions, ex);
					}
					catch (Exception ex)
					{
						this.Get(invoker).HandleGeneralExceptions(messageOptions, ex);
					}
					finally
					{
						this.Get(invoker).BatchFinallyHandler(messageOptions, keepAliveTask, batchCancellationToken);
					}

					// Delay the next polling attempt for a new message, since no messages were received last time.
					if (shouldDelayNextRequest) await Task.Delay(messageOptions.PollFrequency, messageOptions.CancelToken).ConfigureAwait(false);
				}
			}
		}



		private void BatchFinallyHandler(
			[CanBeNull] HandleBatchMessageOptions messageOptions,
			[CanBeNull] Task keepAliveTask,
			[NotNull] CancellationTokenSource batchCancelToken)
		{
			if (Guard.IsAnyNull(messageOptions)) return;
			Guard.NotNull(batchCancelToken, "generalCancelToken");

			// Cancel any outstanding jobs. Since we don't have separate threads per message as in the serial processing, this indicates no faulted processing.
			if (keepAliveTask != null && !keepAliveTask.IsCompleted)
			{
				//messageOptions.QuickLogDebug("HandleBatchMessages", "Queue's '{0}' batch messages' processing faulted; cancelling related jobs", queue.Name);
				batchCancelToken.Cancel();
			}
		}
	}
}