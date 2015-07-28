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
		public Task HandleMessagesInParallelAsync([NotNull] HandleMessagesParallelOptions messageOptions)
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
		internal async Task HandleMessagesInParallelAsync([NotNull] HandleMessagesParallelOptions messageOptions, ExtendedQueueBase invoker = null)
		{
			Guard.NotNull(messageOptions, "messageOptions");


			this.Statistics.IncreaseListeners();
			this.Statistics.IncreaseAllMessageSlots(messageOptions.MaximumCurrentMessages);
			
			// Used to allow to pass it as a reference
			long[] activeMessageSlots = { 0 };

			// The amount of time to wait to poll the internal queue for free threads
			var internalZeroThreadsWait = TimeSpan.FromSeconds(1);

			while (true)
			{
				try
				{
					if (messageOptions.CancelToken.IsCancellationRequested)
					{
						this.Statistics.DecreaseListeners();
						this.Statistics.DecreaseAllMessageSlots(messageOptions.MaximumCurrentMessages);
						return;
					}


					this.LogAction(LogSeverity.Debug, "Attempting to retrieve new messages from a queue", "Queue: {0}", this.Name);
					var busySlots = (int) Interlocked.Read(ref activeMessageSlots[0]);
					var freeMessageSlots = messageOptions.MaximumCurrentMessages - busySlots;

					this.Statistics.IncreaseBusyMessageSlots(busySlots);


					if (await this.DelayOnNoParallelMessages(freeMessageSlots, messageOptions.MaximumCurrentMessages, internalZeroThreadsWait).ConfigureAwait(false)) continue;

					this.LogAction(LogSeverity.Debug, "Attempting to read from a queue", "Queue: {0}", this.Name);
					var messages = await this.This(invoker).GetMessagesAsync(messageOptions, freeMessageSlots, this.This(invoker)).ConfigureAwait(false);

					if (messages.Count == 0)
					{
						this.LogAction(LogSeverity.Debug,
							"No new messages are available",
							"No new messages are available for processing in queue '{0}', will try again on {1}",
							this.Name,
							internalZeroThreadsWait.ToString("g", CultureInfo.InvariantCulture));
						await Task.Delay(messageOptions.PollFrequency, messageOptions.CancelToken).ConfigureAwait(false);
						continue;
					}

					this.LogAction(LogSeverity.Debug, "Messages were retrieved", "A total of '{0}' messages were retrieved on this batch from queue '{1}'", messages.Count.ToString(), this.Name);

					foreach (var message in messages)
						this.This(invoker).ProcessOneParallelMessage(messageOptions, message, activeMessageSlots, this.This(invoker));
				}
				catch (TaskCanceledException)
				{
					if (messageOptions.CancelToken.IsCancellationRequested)
					{
						this.Statistics.DecreaseListeners();
						this.Statistics.DecreaseAllMessageSlots(messageOptions.MaximumCurrentMessages);
						this.Statistics.DecreaseBusyMessageSlots((int)activeMessageSlots[0]);
						return;
					}
				}
				catch (Exception ex)
				{
					this.This(invoker).HandleGeneralExceptions(messageOptions, ex);
				}
			}
		}



		protected internal async Task<bool> DelayOnNoParallelMessages(int freeMessageSlots, int totalMessageSlots, TimeSpan internalZeroThreadsWait)
		{
			if (freeMessageSlots <= 0)
			{
				this.LogAction(LogSeverity.Debug,
					"No free threads for incoming messages are available",
					"No free threads are available for processing messages in queue '{0}' (0 out of '{1}'), will try again on {2}",
					this.Name,
					totalMessageSlots.ToString(),
					internalZeroThreadsWait.ToString("g", CultureInfo.InvariantCulture));
				await Task.Delay(internalZeroThreadsWait).ConfigureAwait(false);
				return true;
			}
			return false;
		}



		protected internal async Task ProcessOneParallelMessage(HandleMessagesParallelOptions messageOptions, IQueueMessage message, long[] activeMessageSlots, ExtendedQueueBase invoker)
		{
			var messageSpecificCancellationTokenSource = new CancellationTokenSource();
			var currentMessage = message;

			if (currentMessage == null)
				return;
			Interlocked.Increment(ref activeMessageSlots[0]);

			Task keepAliveTask = null;

			this.LogAction(LogSeverity.Debug,
				"Started processing a message",
				"Started processing queue's '{0}' message with ID '{1}' ({2} slots remaining)",
				this.Name,
				currentMessage.Id,
				(messageOptions.MaximumCurrentMessages - Interlocked.Read(ref activeMessageSlots[0])).ToString());

			try
			{
				keepAliveTask =
					await
						this.This(invoker).ProcessMessageInternal(new QueueMessageWrapper(this, currentMessage), messageOptions, messageSpecificCancellationTokenSource, invoker).ConfigureAwait(false);
			}
			catch (TaskCanceledException)
			{
				throw;
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
				this.This(invoker).ParallelFinallyHandler(messageOptions, activeMessageSlots, keepAliveTask, currentMessage, messageSpecificCancellationTokenSource);
			}

			this.Statistics.DecreaseBusyMessageSlots();
		}



		protected internal async Task<List<IQueueMessage>> GetMessagesAsync(HandleMessagesParallelOptions messageOptions, int freeMessageSlots, ExtendedQueueBase invoker)
		{
			return (await this.This(invoker).GetMessagesAsync(freeMessageSlots > this.This(invoker).MaximumMessagesProvider.MaximumMessagesPerRequest
				? this.This(invoker).MaximumMessagesProvider.MaximumMessagesPerRequest
				: freeMessageSlots,
				messageOptions.MessageLeaseTime,
				messageOptions.CancelToken).ConfigureAwait(false)).ToList();
		}



		private void ParallelFinallyHandler(
			[CanBeNull] HandleMessagesParallelOptions messageOptions,
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
					this.LogAction(LogSeverity.Warning,
						"Message processing was faulted; cancelling",
						"Queue's '{0}' message '{1}', processing faulted; cancelling related jobs for this message",
						this.Name,
						currentMessage.Id);
				}
				messageSpecificCancellationTokenSource.Cancel();
			}
		}
	}
}