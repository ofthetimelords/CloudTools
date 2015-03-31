// <copyright file="HandleParallelMessageOptions.cs" company="nett">
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
using System.Linq;
using System.Threading;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.Models
{
	/// <summary>
	///     <para>Input arguments for the ExtendedQueue framework.</para>
	/// </summary>
	public class HandleParallelMessageOptions : HandleSerialMessageOptions
	{
		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleParallelMessageOptions" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="timeWindow">
		///     <para>The time window within which a message is still valid for processing (older messages will be discarded). Use <see cref="System.TimeSpan.Zero" /></para>
		///     <para>to ignore this check.</para>
		/// </param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="maximumCurrentMessages">The maximum amount of messages that can be processed at the same batch.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="messageHandler">An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.</param>
		/// <param name="poisonHandler">
		///     <para>An action that specifies how a poison message should be handled, which fires before <see cref="TheQ.Utilities.CloudTools.Storage.Models.HandleSerialMessageOptions.MessageHandler" /></para>
		///     <para>. Returns a value indicating whether the message has been handled successfully and should be removed.</para>
		/// </param>
		/// <param name="exceptionHandler">An action that specifies how an exception should be handled.</param>
		/// <exception cref="ArgumentException">Parameter <paramref name="maximumCurrentMessages" /> exceeded 32 which is the maximum allowed value by Azure</exception>
		/// <exception cref="ArgumentNullException">cancelToken;The Cancellation Token parameter is required or messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1.
		/// </exception>
		public HandleParallelMessageOptions(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			int maximumCurrentMessages,
			CancellationToken cancelToken,
			[NotNull] Func<QueueMessageWrapper, bool> messageHandler,
			[CanBeNull] Func<QueueMessageWrapper, bool> poisonHandler = null,
			[CanBeNull] Action<Exception> exceptionHandler = null)
			: base(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, cancelToken, messageHandler, poisonHandler, exceptionHandler)
		{
				this.MaximumCurrentMessages = maximumCurrentMessages;
		}



		/// <summary>
		///     Gets the maximum amount of messages that can be processed at the same batch.
		/// </summary>
		public int MaximumCurrentMessages { get; private set; }
	}
}