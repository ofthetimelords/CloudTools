#region Using directives
using System;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;
#endregion



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public abstract partial class ExtendedQueueBase
	{
		/// <summary>
		///     Begins a task that receives messages serially and automatically manages their lifetime.
		/// </summary>
		/// <param name="messageOptions">An options object used to initialise the procedure.</param>
		/// <returns>A cancellable task representing the message processing procedure.</returns>
		public Task HandleMessagesAsync(HandleSerialMessageOptions messageOptions)
		{
			return this.HandleMessagesAsync(messageOptions, this);
		}



		/// <summary>
		///     Begins a task that receives messages serially and automatically manages their lifetime.
		/// </summary>
		/// <param name="messageOptions">An options object used to initialise the procedure.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		/// <returns>
		///     A cancellable task representing the message processing procedure.
		/// </returns>
		internal async Task HandleMessagesAsync(HandleSerialMessageOptions messageOptions, ExtendedQueueBase invoker)
		{
			Guard.NotNull(messageOptions, "messageOptions");

			while (true)
			{
				if (messageOptions.CancelToken.IsCancellationRequested)
					return;

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
							ref keepAliveTask,
							this.Get(invoker));
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
				if (!receivedMessage)
					await Task.Delay(messageOptions.PollFrequency, messageOptions.CancelToken).ConfigureAwait(false);
			}
		}



		/// <summary>
		///     Error handler for cancelled task exception during serial message processing.
		/// </summary>
		/// <param name="messageOptions">The message options object.</param>
		protected internal virtual void HandleTaskCancelled(HandleSerialMessageOptions messageOptions)
		{
			//messageOptions.QuickLogDebug("HandleMessages", "The message checking task was cancelled on queue '{0}'", queue.Name);
		}



		/// <summary>
		///     Gets the next message from the queue, asynchronously.
		/// </summary>
		/// <param name="messageOptions">The message options object.</param>
		/// <param name="messageSpecificCancellationTokenSource">The message specific cancellation token source.</param>
		/// <returns>An <see cref="IQueueMessage" /> instance.</returns>
		protected internal virtual async Task<IQueueMessage> GetMessageFromQueue(HandleSerialMessageOptions messageOptions, CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			return await this.GetMessageAsync(messageOptions.MessageLeaseTime, messageSpecificCancellationTokenSource.Token).ConfigureAwait(false);
		}



		/// <summary>
		///     The finally handler in the try/catch/finally statement of HandleMessagesAsync.
		/// </summary>
		/// <param name="messageOptions">The message options object.</param>
		/// <param name="keepAliveTask">The <see cref="Task" /> that keeps the message "alive".</param>
		/// <param name="message">The message currently processing.</param>
		/// <param name="messageSpecificCancellationTokenSource">The cancellation token for the message.</param>
		protected internal virtual void SerialFinallyHandler(
			[CanBeNull] HandleSerialMessageOptions messageOptions,
			[CanBeNull] Task keepAliveTask,
			[CanBeNull] IQueueMessage message,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			if (Guard.IsAnyNull(messageOptions, messageSpecificCancellationTokenSource))
				return;

			// Cancel any outstanding jobs due to the faulted operation (the keepalive task should have been cancelled)
			if (keepAliveTask != null && !keepAliveTask.IsCompleted)
				//if (message != null) messageOptions.QuickLogDebug("HandleMessages", "Queue's '{0}' message '{1}', processing faulted; cancelling related jobs", queue.Name, message.Id);
				messageSpecificCancellationTokenSource.Cancel();
		}
	}
}