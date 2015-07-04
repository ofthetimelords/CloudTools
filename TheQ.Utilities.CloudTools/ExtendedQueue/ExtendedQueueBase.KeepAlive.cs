// <copyright file="QueueExtensions.KeepAlive.cs" company="nett">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public abstract partial class ExtendedQueueBase
	{
		/// <summary>
		///     Keeps the <paramref name="queue" /> <paramref name="message" /> enqueued by updating its invisible status and avoids accidental dequeuing, without a synchronisation token or a logging service.
		/// </summary>
		/// <param name="message">The message to ensure stays enqueued.</param>
		/// <param name="queue">The queue the <paramref name="message" /> belongs to.</param>
		/// <param name="messageLeaseTime">The amount of time renewals should occur for (note; actual checks will be performed at 50% of this value to ensure latency handling).</param>
		/// <param name="cancelToken">A token used to allow cancellation of this operation.</param>
		/// <exception cref="ArgumentNullException">queue;Parameter 'message' was null. or queue;Parameter 'queue' was not provided.</exception>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>.</para>
		/// </returns>
		[NotNull]
		public Task KeepMessageAlive([NotNull] IQueueMessage message, TimeSpan messageLeaseTime, CancellationToken cancelToken)
		{
			return this.KeepMessageAlive(message, messageLeaseTime, cancelToken);
		}



		/// <summary>
		///     Keeps the <paramref name="queue" /> <paramref name="message" /> enqueued by updating its invisible status and avoids accidental dequeuing.
		/// </summary>
		/// <param name="message">The message to ensure stays enqueued.</param>
		/// <param name="queue">The queue the <paramref name="message" /> belongs to.</param>
		/// <param name="messageLeaseTime">The amount of time renewals should occur for (note; actual checks will be performed at 50% of this value to ensure latency handling).</param>
		/// <param name="cancelToken">A token used to allow cancellation of this operation.</param>
		/// <param name="syncToken">
		///     <para>A simple <see cref="Object" /></para>
		///     <para>instance Used to synchronise delete and update operations.</para>
		/// </param>
		/// <param name="loggingService">The logging service.</param>
		/// <exception cref="ArgumentNullException">queue;Parameter 'message' was null. or queue;Parameter 'queue' was not provided.</exception>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>.</para>
		/// </returns>
		[NotNull]
		internal async Task KeepMessageAlive(
			[NotNull] IQueueMessage message,
			TimeSpan messageLeaseTime,
			CancellationToken cancelToken,
			[CanBeNull] object syncToken,
			ExtendedQueueBase invoker)
		{
			Guard.NotNull(message, "message");

			while (true)
			{
				try
				{
//					loggingService.QuickLogDebug("KeepMessageAlive", "Queue's '{0}' message '{1}' waiting to renew on {2}", queue.Name, message.Id, DateTimeOffset.Now.ToString("O"));
					await Task.Delay(TimeSpan.FromSeconds(messageLeaseTime.TotalSeconds * .75), cancelToken).ConfigureAwait(false);
//					loggingService.QuickLogDebug("KeepMessageAlive", "Queue's '{0}' message '{1}' started renewing on {2}", queue.Name, message.Id, DateTimeOffset.Now.ToString("O"));

					// Attempt to update the expiration of a message.
					if (syncToken != null) lock (syncToken) this.Get(invoker).DoMessageExpirationUpdateAsync(message, messageLeaseTime, cancelToken, this.Get(invoker)).Wait(cancelToken);
					else await this.Get(invoker).DoMessageExpirationUpdateAsync(message, messageLeaseTime, cancelToken, this.Get(invoker)).ConfigureAwait(false);

					//loggingService.QuickLogDebug("KeepMessageAlive", "Queue's '{0}' message '{1}' completed renewing on {2}", queue.Name, message.Id, DateTimeOffset.Now.ToString("O"));
				}
				//catch (CloudToolsStorageException ex)
				//{
				//	if (string.Equals(ex.ErrorCode, "MessageNotFound", StringComparison.OrdinalIgnoreCase))
				//	{
				//		loggingService.QuickLogError(
				//			"KeepMessageAlive",
				//			ex,
				//			"A 'Message not Found' error occured while attempting to work on a message (this error should not occur under normal circumstances), on queue '{0}'.",
				//			queue.Name);
				//		break;
				//	}

				//	loggingService.QuickLogError("KeepMessageAlive", ex, "An error occurred while trying to perform a Keep Alive operation on a Queue message on queue '{0}'.", queue.Name);
				//	break;
				//}
				catch (OperationCanceledException)
				{
					return;
				}
			}
		}



		internal async Task KeepMessageAlive(
			[NotNull] IList<QueueMessageWrapper> messages,
			TimeSpan messageLeaseTime,
			CancellationToken generalCancelToken,
			[CanBeNull] object syncToken,
			ExtendedQueueBase invoker)
		{
			while (true)
			{
				try
				{
					var endingSooner = messages.Min(m => m.ActualMessage.NextVisibleTime.Value);
//					loggingService.QuickLogDebug("KeepMessageAlive", "Queue's '{0}' for {1} batch messages waiting to renew on {2}", queue.Name, messages.Count, DateTimeOffset.Now.ToString("O"));

					await Task.Delay(TimeSpan.FromSeconds(endingSooner.UtcDateTime.Subtract(DateTime.UtcNow).TotalSeconds * .50), generalCancelToken).ConfigureAwait(false);
//					loggingService.QuickLogDebug("KeepMessageAlive", "Queue's '{0}' for {1} batch messages started renewing on {2}", queue.Name, messages.Count, DateTimeOffset.Now.ToString("O"));

					// Attempt to update the expiration of a message.
					if (syncToken != null) lock (syncToken) Parallel.ForEach(messages, async message => await this.DoMessageExpirationUpdateAsync(message.ActualMessage, messageLeaseTime, generalCancelToken, this.Get(invoker)).ConfigureAwait(false));
					else Parallel.ForEach(messages, async message => await this.DoMessageExpirationUpdateAsync(message.ActualMessage, messageLeaseTime, generalCancelToken, this.Get(invoker)).ConfigureAwait(false));

//					loggingService.QuickLogDebug("KeepMessageAlive", "Queue's '{0}' {1} messages completed renewing on {2}", queue.Name, messages.Count, DateTimeOffset.Now.ToString("O"));
				}
				//catch (CloudToolsStorageException ex)
				//{
				//	if (ex.StatusCode != 404 && ex.StatusCode != 409 && ex.StatusCode != 412)
				//		throw;

				//	if (string.Equals(ex.ErrorCode, "MessageNotFound", StringComparison.OrdinalIgnoreCase))
				//	{
				//		loggingService.QuickLogError(
				//			"KeepMessageAlive",
				//			ex,
				//			"A 'Message not Found' error occured while attempting to work on a message (this error should not occur under normal circumstances), on queue '{0}'.",
				//			queue.Name);
				//		break;
				//	}

				//	loggingService.QuickLogError("KeepMessageAlive", ex, "An error occurred while trying to perform a Keep Alive operation on a Queue message on queue '{0}'.", queue.Name);
				//	break;
				//}
				catch (OperationCanceledException)
				{
					return;
				}
			}
		}



		/// <summary>
		///     Performs an update on the expiration time of a <paramref name="message" />
		/// </summary>
		/// <param name="message">The message to operate on.</param>
		/// <param name="queue">The queue the <paramref name="message" /> belongs to.</param>
		/// <param name="messageLeaseTime">The time till the next expiration.</param>
		/// <param name="cancelToken">The cancellation token.</param>
		/// <returns>
		/// </returns>
		private async Task DoMessageExpirationUpdateAsync([NotNull] IQueueMessage message, TimeSpan messageLeaseTime, CancellationToken cancelToken, ExtendedQueueBase invoker)
		{
			Guard.NotNull(message, "message");

			if (cancelToken.IsCancellationRequested) return;

			await this.Get(invoker).UpdateMessageAsync(message, messageLeaseTime, QueueMessageUpdateFields.Visibility, cancelToken).ConfigureAwait(false);
		}
	}
}