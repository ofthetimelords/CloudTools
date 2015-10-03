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
		///     Handles messages from the queue in a serial manner, in an endless loop.
		/// </summary>
		/// <param name="queue">The queue to check for messages.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <exception cref="ArgumentNullException">The messageOps parameter is null.</exception>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>.</para>
		/// </returns>
		[NotNull]
		public async Task HandleMessagesInBatchAsync([NotNull] HandleMessagesBatchOptions messageOptions)
		{
			Guard.NotNull(messageOptions, "messageOptions");

			this.Statistics.IncreaseListeners();
			this.Statistics.IncreaseAllMessageSlots(messageOptions.MaximumCurrentMessages);


			while (true)
			{
				if (messageOptions.CancelToken.IsCancellationRequested)
				{
					this.Statistics.DecreaseListeners();
					this.Statistics.DecreaseAllMessageSlots(messageOptions.MaximumCurrentMessages);
					return;
				}

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
						this.Top.LogAction(LogSeverity.Debug, "Attempting to retrieve new messages from a queue", "Queue: {0}", this.Name);

						var howManyMoreFit = messageOptions.MaximumCurrentMessages - rawMessages.Count;

						IList<IQueueMessage> retrievedMessages;
						
						if (howManyMoreFit > 0)
							retrievedMessages = (await this.Top.GetMessagesAsync(
							Math.Min(this.MaximumMessagesProvider.MaximumMessagesPerRequest, howManyMoreFit),
							messageOptions.MessageLeaseTime,
							messageOptions.CancelToken).ConfigureAwait(false)).ToList();
						else
							retrievedMessages = new List<IQueueMessage>();

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
							convertedMessages.AddRange(rawMessages.Select(m => new QueueMessageWrapper(this.Top, m)));
							//messageOptions.QuickLogDebug("HandleBatchMessages", "Started processing queue's '{0}' {1} messages", queue.Name, rawMessages.Count);

							this.Statistics.IncreaseBusyMessageSlots(convertedMessages.Count);
							keepAliveTask = await this.ProcessMessageInternalBatch(convertedMessages, batchCancellationToken, messageOptions).ConfigureAwait(false);

							break;
						}
					}
					catch (TaskCanceledException)
					{
						if (messageOptions.CancelToken.IsCancellationRequested)
						{
							this.Statistics.DecreaseListeners();
							this.Statistics.DecreaseAllMessageSlots(messageOptions.MaximumCurrentMessages);
							this.Statistics.DecreaseBusyMessageSlots(convertedMessages.Count);
							return;
						}
						else if (batchCancellationToken.IsCancellationRequested)
							break;
					}
					catch (CloudToolsStorageException ex)
					{
						this.Top.HandleStorageExceptions(messageOptions, ex);
					}
					catch (Exception ex)
					{
						this.Top.HandleGeneralExceptions(messageOptions, ex);
					}
					finally
					{
						this.Top.BatchFinallyHandler(messageOptions, keepAliveTask, batchCancellationToken);
					}


					// Delay the next polling attempt for a new message, since no messages were received last time.
					if (shouldDelayNextRequest) await Task.Delay(messageOptions.PollFrequency, messageOptions.CancelToken).ConfigureAwait(false);
				}

				this.Statistics.DecreaseBusyMessageSlots(convertedMessages.Count);
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