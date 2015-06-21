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
		/// <summary>
		///     Begins a task that receives messages in parallel and automatically manages their lifetime.
		/// </summary>
		/// <param name="messageOptions">An options object used to initialise the procedure.</param>
		/// <returns>A cancellable task representing the message processing procedure.</returns>
		/// <exception cref="ArgumentNullException">queue;Parameter 'queue' was null. or messageOptions;Parameter 'messageOptions' was not provided.</exception>
		public Task HandleMessagesInParallelAsync([NotNull] HandleParallelMessageOptions messageOptions)
		{
			return this.HandleMessagesInParallelAsync(messageOptions, this);
		}



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
		internal async Task HandleMessagesInParallelAsync([NotNull] HandleParallelMessageOptions messageOptions, ExtendedQueueBase invoker = null)
		{
			Guard.NotNull(messageOptions, "messageOptions");

			// Used to allow to pass it as a reference
			long[] activeMessageSlots = { 0 };

			// The amount of time to wait to poll the internal queue for free threads
			var internalZeroThreadsWait = TimeSpan.FromSeconds(1);

			while (true)
			{
				try
				{
					if (messageOptions.CancelToken.IsCancellationRequested) return;

					//messageOptions.QuickLogDebug("HandleMessages", "Ready to contact queue '{0}' for a batch of new messages (maximum '{1}')", queue.Name, messageOptions.MaximumCurrentMessages);
					var freeMessageSlots = messageOptions.MaximumCurrentMessages - (int)Interlocked.Read(ref activeMessageSlots[0]);

					if (await ExtendedQueueBase.DelayOnNoParallelMessages(freeMessageSlots, internalZeroThreadsWait).ConfigureAwait(false)) continue;

					//messageOptions.QuickLogDebug(
					//	"HandleMessages",
					//	"Ready to request a batch of messages on queue '{0}', '{1}' free slots out of {2}",
					//	queue.Name,
					//	freeMessageSlots,
					//	messageOptions.MaximumCurrentMessages);
					var messages = await this.Get(invoker).GetMessagesAsync(messageOptions, freeMessageSlots, this.Get(invoker)).ConfigureAwait(false);

					if (messages.Count == 0)
					{
						//messageOptions.QuickLogDebug(
						//	"HandleMessages",
						//	"No new messages are available for processing messages in queue '{0}' (0 out of '{1}'), will try again on {2}",
						//	queue.Name,
						//	messageOptions.MaximumCurrentMessages,
						//	DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
						await Task.Delay(messageOptions.PollFrequency, messageOptions.CancelToken).ConfigureAwait(false);
						continue;
					}

					//messageOptions.QuickLogDebug("HandleMessages", "A total of '{0}' messages were retrieved on this batch from queue '{1}'", messages.Count, queue.Name);

					foreach (var message in messages)
						this.Get(invoker).ProcessOneParallelMessage(messageOptions, message, activeMessageSlots, this.Get(invoker));
				}
				catch (TaskCanceledException)
				{
					if (messageOptions.CancelToken.IsCancellationRequested)
						return;
				}
				catch (Exception ex)
				{
					this.Get(invoker).HandleGeneralExceptions(messageOptions, ex, true);
				}
			}
		}



		protected internal static async Task<bool> DelayOnNoParallelMessages(int freeMessageSlots, TimeSpan internalZeroThreadsWait)
		{
			if (freeMessageSlots == 0)
			{
				//messageOptions.QuickLogDebug(
				//	"HandleMessages",
				//	"No free threads are available for processing messages in queue '{0}' (0 out of '{1}'), will try again on {2}",
				//	queue.Name,
				//	messageOptions.MaximumCurrentMessages,
				//	DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
				await Task.Delay(internalZeroThreadsWait).ConfigureAwait(false);
				return true;
			}
			return false;
		}



		protected internal async Task ProcessOneParallelMessage(HandleParallelMessageOptions messageOptions, IQueueMessage message, long[] activeMessageSlots, ExtendedQueueBase invoker)
		{
			var messageSpecificCancellationTokenSource = new CancellationTokenSource();
			var currentMessage = message;

			if (currentMessage == null)
				return;
			Interlocked.Increment(ref activeMessageSlots[0]);

			Task keepAliveTask = null;

			//messageOptions.QuickLogDebug(
			//	"HandleMessages",
			//	"Started processing queue's '{0}' message with ID '{1}' ({2} slots remaining)",
			//	queue.Name,
			//	currentMessage.Id,
			//	messageOptions.MaximumCurrentMessages - Interlocked.Read(ref activeMessageSlots[0]));

			try
			{
				keepAliveTask =
					await
						this.Get(invoker).ProcessMessageInternal(new QueueMessageWrapper(this, currentMessage), messageOptions, messageSpecificCancellationTokenSource, invoker).ConfigureAwait(false);
			}
			catch (TaskCanceledException)
			{
				throw;
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
				this.Get(invoker).ParallelFinallyHandler(messageOptions, activeMessageSlots, keepAliveTask, currentMessage, messageSpecificCancellationTokenSource);
			}
		}



		protected internal async Task<List<IQueueMessage>> GetMessagesAsync(HandleParallelMessageOptions messageOptions, int freeMessageSlots, ExtendedQueueBase invoker)
		{
			return (await this.Get(invoker).GetMessagesAsync(freeMessageSlots > this.Get(invoker).MaximumMessagesProvider.MaximumMessagesPerRequest
				? this.Get(invoker).MaximumMessagesProvider.MaximumMessagesPerRequest
				: freeMessageSlots,
				messageOptions.MessageLeaseTime,
				messageOptions.CancelToken).ConfigureAwait(false)).ToList();
		}



		private void ParallelFinallyHandler(
			[CanBeNull] HandleParallelMessageOptions messageOptions,
			[CanBeNull] long[] activeMessageSlots,
			[CanBeNull] Task keepAliveTask,
			[CanBeNull] IQueueMessage currentMessage,
			[CanBeNull] CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			if (Guard.IsAnyNull(messageOptions, activeMessageSlots, messageSpecificCancellationTokenSource)) return;

			Interlocked.Decrement(ref activeMessageSlots[0]);

			// Cancel any outstanding jobs due to the faulted operation (the keepalive task should have been cancelled)
			if (keepAliveTask != null && !keepAliveTask.IsCompleted)
			{
				if (currentMessage != null)
				{
					//messageOptions.QuickLogDebug("HandleMessages", "Queue's '{0}' message '{1}', processing faulted; cancelling related jobs on {1}", queue.Name, currentMessage.Id);
					//messageOptions.QuickLogDebug("HandleMessages", "Queue '{0}': currently active threads: {1}", queue.Name, Interlocked.Read(ref activeMessageSlots[0]));
				}
				messageSpecificCancellationTokenSource.Cancel();
			}
			//else if (currentMessage != null) messageOptions.QuickLogDebug("HandleMessages", "Queue '{0}': currently active threads: {1}", queue.Name, Interlocked.Read(ref activeMessageSlots[0]));
		}
	}
}