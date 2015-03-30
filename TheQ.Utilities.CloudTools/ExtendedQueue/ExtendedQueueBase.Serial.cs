using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
		public Task HandleMessagesAsync(HandleSerialMessageOptions messageOptions)
		{
			return this.HandleMessagesAsync(messageOptions, this);
		}

		internal async Task HandleMessagesAsync(HandleSerialMessageOptions messageOptions, ExtendedQueueBase invoker)
		{
			Guard.NotNull(messageOptions, "messageOptions");

			while (true)
			{
				if (messageOptions.CancelToken.IsCancellationRequested) return;

				// When set to true, the queue won't wait before it requests another message
				var receivedMessage = false;

				// Used to prevent a message operation from running on a specific message
				var messageSpecificCancellationTokenSource = new CancellationTokenSource();

				Task keepAliveTask = null;
				IQueueMessage message = null;

				try
				{
					message = await this.Get(invoker).GetMessageFromQueue(messageOptions, messageSpecificCancellationTokenSource).ConfigureAwait(false);
					if (message != null)
					{
						receivedMessage = true;

						this.Get(invoker).ProcessMessageInternal(
							new QueueMessageWrapper(this.Get(invoker), message),
							messageOptions,
							messageSpecificCancellationTokenSource,
							ref keepAliveTask, this.Get(invoker));
					}
				}
				catch (TaskCanceledException)
				{
					this.Get(invoker).HandleTaskCancelled(messageOptions);
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
					this.Get(invoker).SerialFinallyHandler(messageOptions, keepAliveTask, message, messageSpecificCancellationTokenSource);
				}

				// Delay the next polling attempt for a new message, since no messages were received last time.
				if (!receivedMessage) await Task.Delay(messageOptions.PollFrequency, messageOptions.CancelToken).ConfigureAwait(false);
			}
		}



		protected internal virtual void HandleTaskCancelled(HandleSerialMessageOptions messageOptions)
		{
			//messageOptions.QuickLogDebug("HandleMessages", "The message checking task was cancelled on queue '{0}'", queue.Name);
		}



		protected internal virtual async Task<IQueueMessage> GetMessageFromQueue(HandleSerialMessageOptions messageOptions, CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			return await this.GetMessageAsync(messageOptions.MessageLeaseTime, messageSpecificCancellationTokenSource.Token).ConfigureAwait(false);
		}



		protected internal virtual void SerialFinallyHandler(
			[CanBeNull] HandleSerialMessageOptions messageOptions,
			[CanBeNull] Task keepAliveTask,
			[CanBeNull] IQueueMessage message,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			if (Guard.IsAnyNull(messageOptions, messageSpecificCancellationTokenSource)) return;

			// Cancel any outstanding jobs due to the faulted operation (the keepalive task should have been cancelled)
			if (keepAliveTask != null && !keepAliveTask.IsCompleted)
				//if (message != null) messageOptions.QuickLogDebug("HandleMessages", "Queue's '{0}' message '{1}', processing faulted; cancelling related jobs", queue.Name, message.Id);
				messageSpecificCancellationTokenSource.Cancel();
		}
	}
}