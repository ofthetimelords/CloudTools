// <copyright file="HandleBatchMessageOptions.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.Models
{
	/// <summary>
	///     <para>Input arguments for the <see cref="QueueExtensions.HandleMessages" /></para>
	///     <para>extension method.</para>
	/// </summary>
	public class HandleBatchMessageOptions : HandleMessageOptionsBase
	{
		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleBatchMessageOptions	" /></para>
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
		/// <param name="logService">The logging service to use.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="messageHandler">An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.</param>
		/// <param name="poisonHandler">
		///     <para>An action that specifies how poison messages should be handled, which fires before <see cref="TheQ.Utilities.CloudTools.Storage.Models.HandleBatchMessageOptions.MessageHandler" /></para>
		///     <para>. Returns a list of messages that have been handled successfully and should be removed. Messages that are not returned will not be removed and will be requeued automatically.</para>
		/// </param>
		/// <param name="exceptionHandler">An action that specifies how an exception should be handled.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		public HandleBatchMessageOptions(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			int maximumCurrentMessages,
			CancellationToken cancelToken,
			[NotNull] Func<IList<QueueMessageWrapper>, IList<QueueMessageWrapper>> messageHandler,
			[CanBeNull] Func<IList<QueueMessageWrapper>, IList<QueueMessageWrapper>> poisonHandler = null,
			[CanBeNull] Action<Exception> exceptionHandler = null)
			: base(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, cancelToken, exceptionHandler)

		{
			if (messageHandler == null) throw new ArgumentNullException("messageHandler", "The Message Handler delegate is required");

			this.MaximumCurrentMessages = maximumCurrentMessages;
			this.MessageHandler = messageHandler;
			this.PoisonHandler = poisonHandler;
		}



		/// <summary>
		///     An action that specifies how a message should be handled. Returns a list of messages that have been handled successfully and should be removed. Messages that are not returned will not be removed
		///     and will be requeued automatically.
		/// </summary>
		[NotNull]
		public Func<IList<QueueMessageWrapper>, IList<QueueMessageWrapper>> MessageHandler { get; private set; }


		/// <summary>
		///     <para>An action that specifies how a poison message should be handled, which fires before <see cref="TheQ.Utilities.CloudTools.Storage.Models.HandleBatchMessageOptions.MessageHandler" /></para>
		///     <para>. Returns a list of messages that have been handled successfully and should be removed. Messages that are not returned will not be removed and will be requeued automatically.</para>
		/// </summary>
		[CanBeNull]
		public Func<IList<QueueMessageWrapper>, IList<QueueMessageWrapper>> PoisonHandler { get; private set; }


		/// <summary>
		///     Gets the maximum amount of messages that can be processed at the same batch.
		/// </summary>
		public int MaximumCurrentMessages { get; private set; }
	}
}