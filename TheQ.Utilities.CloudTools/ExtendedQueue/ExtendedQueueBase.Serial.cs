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
		public Task HandleMessagesInSerialAsync(HandleMessagesSerialOptions messageOptions)
		{
			return this.HandleMessagesInSerialAsync(messageOptions, this);
		}



		/// <summary>
		///     Begins a task that receives messages serially and automatically manages their lifetime.
		/// </summary>
		/// <param name="messageOptions">An options object used to initialise the procedure.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		/// <returns>
		///     A cancellable task representing the message processing procedure.
		/// </returns>
		internal async Task HandleMessagesInSerialAsync(HandleMessagesSerialOptions messageOptions, ExtendedQueueBase invoker)
		{
			Guard.NotNull(messageOptions, "messageOptions");

			this.Statistics.IncreaseListeners();
			this.Statistics.IncreaseAllMessageSlots();

			while (true)
			{
				if (messageOptions.CancelToken.IsCancellationRequested)
				{
					this.Statistics.DecreaseListeners();
					this.Statistics.DecreaseAllMessageSlots();
					return;
				}

				// When set to true, the queue won't wait before it requests another message
				var receivedMessage = false;

				// Used to prevent a message operation from running on a specific message
				var messageSpecificCancellationTokenSource = new CancellationTokenSource();

				Task keepAliveTask = null;
				IQueueMessage message = null;

				try
				{
					this.LogAction(LogSeverity.Debug, "Attempting to read from a queue", "Queue: {0}", this.Name);

					message = await this.This(invoker).GetMessageFromQueue(messageOptions, messageSpecificCancellationTokenSource).ConfigureAwait(false);
					if (message != null)
					{
						this.Statistics.IncreaseBusyMessageSlots();
						this.LogAction(LogSeverity.Debug, "One message found in the queue and will be processed", "Queue: {0}, Message ID: {1}", this.Name, message.Id);
						receivedMessage = true;

						keepAliveTask = await this.This(invoker).ProcessMessageInternal(
							new QueueMessageWrapper(this.This(invoker), message),
							messageOptions,
							messageSpecificCancellationTokenSource,
							this.This(invoker)).ConfigureAwait(false);
					}
					else this.LogAction(LogSeverity.Debug, "No messages found when an attempt was made to read from a queue", "Queue: {0}", this.Name);
				}
				catch (TaskCanceledException)
				{
					if (this.This(invoker).HandleTaskCancelled(messageOptions))
					{
						this.Statistics.DecreaseListeners();
						this.Statistics.DecreaseAllMessageSlots();
						this.Statistics.DecreaseBusyMessageSlots();
						return;
					}
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
					this.This(invoker).SerialFinallyHandler(messageOptions, keepAliveTask, message, messageSpecificCancellationTokenSource);
				}

				this.Statistics.DecreaseBusyMessageSlots();

				// Delay the next polling attempt for a new message, since no messages were received last time.
				if (!receivedMessage)
					await Task.Delay(messageOptions.PollFrequency, messageOptions.CancelToken).ConfigureAwait(false);
			}
		}



		/// <summary>
		///     Error handler for cancelled task exception during serial message processing.
		/// </summary>
		/// <param name="messageOptions">The message options object.</param>
		protected internal virtual bool HandleTaskCancelled(HandleMessagesSerialOptions messageOptions)
		{
			this.LogAction(LogSeverity.Info, "Attempting to cancel No messages found when an attempt was made to read from a queue", "Queue: {0}", this.Name);
			if (messageOptions.CancelToken.IsCancellationRequested)
				return true;

			return false;
		}



		/// <summary>
		///     Gets the next message from the queue, asynchronously.
		/// </summary>
		/// <param name="messageOptions">The message options object.</param>
		/// <param name="messageSpecificCancellationTokenSource">The message specific cancellation token source.</param>
		/// <returns>An <see cref="IQueueMessage" /> instance.</returns>
		protected internal virtual async Task<IQueueMessage> GetMessageFromQueue(HandleMessagesSerialOptions messageOptions, CancellationTokenSource messageSpecificCancellationTokenSource)
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
			[CanBeNull] HandleMessagesSerialOptions messageOptions,
			[CanBeNull] Task keepAliveTask,
			[CanBeNull] IQueueMessage message,
			[NotNull] CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			if (Guard.IsAnyNull(messageOptions, messageSpecificCancellationTokenSource))
				return;

			// Cancel any outstanding jobs due to the faulted operation (the keepalive task should have been cancelled)
			if (keepAliveTask != null && !keepAliveTask.IsCompleted)
				if (message != null)
				{
					this.LogAction(LogSeverity.Warning, "Message was not processed successfully", "Queue's '{0}' message '{1}', processing faulted; cancelling related jobs", this.Name, message.Id);
					messageSpecificCancellationTokenSource.Cancel();
				}
		}
	}
}