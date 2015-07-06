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
		/// <summary>
		///     Begins a task that receives messages in a batch and automatically manages their lifetime.
		/// </summary>
		/// <param name="messageOptions">An options object used to initialise the procedure.</param>
		/// <returns>A cancellable task representing the message processing procedure.</returns>
		public Task HandleMessagesInBatchAsync([NotNull] HandleMessagesBatchOptions messageOptions) { return this.HandleMessagesInBatchAsync(messageOptions, this); }


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
		internal async Task HandleMessagesInBatchAsync([NotNull] HandleMessagesBatchOptions messageOptions, ExtendedQueueBase invoker)
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
						this.LogAction(LogSeverity.Debug, "Attempting to retrieve new messages from a queue", "Queue: {0}", this.Name);

						var howManyMoreFit = messageOptions.MaximumCurrentMessages - rawMessages.Count;

						//if (howManyMoreFit <= 0)
						//{
						//	this.LogAction(LogSeverity.Debug, "The batch is full, will try again later", "Queue: {0}", this.Name);
						//	shouldDelayNextRequest = true;
						//}
						//else
						{
							var retrievedMessages = (await this.This(invoker).GetMessagesAsync(
								Math.Min(this.MaximumMessagesProvider.MaximumMessagesPerRequest, howManyMoreFit),
								messageOptions.MessageLeaseTime,
								messageOptions.CancelToken).ConfigureAwait(false)).ToList();

							if (retrievedMessages.Count == 0 && rawMessages.Count == 0)
							{
								shouldDelayNextRequest = true;
							}
							else
							{
								// Keep trying to retrieve messages until there are no more or the quota is reached
								if (retrievedMessages.Count > 0)
								{
									rawMessages.AddRange(retrievedMessages);

									if (rawMessages.Count < messageOptions.MaximumCurrentMessages)
										continue;
								}

								// Have buffered messages and optionally some were retrieved from the cache. Proceed normally.
								convertedMessages.AddRange(rawMessages.Select(m => new QueueMessageWrapper(this.This(invoker), m)));
								//messageOptions.QuickLogDebug("HandleBatchMessages", "Started processing queue's '{0}' {1} messages", queue.Name, rawMessages.Count);

								keepAliveTask = await this.ProcessMessageInternalBatch(convertedMessages, batchCancellationToken, messageOptions, invoker).ConfigureAwait(false);

								break;
							}
						}
					}
					catch (TaskCanceledException)
					{
						if (messageOptions.CancelToken.IsCancellationRequested)
							break;
					}
					catch (CloudToolsStorageException ex)
					{
						this.This(invoker).HandleStorageExceptions(messageOptions, ex);
					}
					catch (Exception ex)
					{
						this.This(invoker).HandleGeneralExceptions(messageOptions, ex);
					}
					finally
					{
						this.This(invoker).BatchFinallyHandler(messageOptions, keepAliveTask, batchCancellationToken);
					}

					// Delay the next polling attempt for a new message, since no messages were received last time.
					if (shouldDelayNextRequest) await Task.Delay(messageOptions.PollFrequency, messageOptions.CancelToken).ConfigureAwait(false);
				}

				if (messageOptions.CancelToken.IsCancellationRequested)
					return;
			}
		}



		/// <summary>
		/// The finally handler in the try/catch/finally statement of HandleMessagesInBatchAsync.
		/// </summary>
		/// <param name="messageOptions">The message options object.</param>
		/// <param name="keepAliveTask">The <see cref="Task"/> that keeps the message "alive".</param>
		/// <param name="batchCancelToken">The cancellation token for the batch.</param>
		private void BatchFinallyHandler(
			[CanBeNull] HandleMessagesBatchOptions messageOptions,
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