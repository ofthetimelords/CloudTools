// <copyright file="HandleMessageOptionsBase.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Queues;



namespace TheQ.Utilities.CloudTools.Storage.Models
{
	/// <summary>
	///     <para>Input arguments for the <see cref="QueueExtensions.HandleMessages" /></para>
	///     <para>extension method.</para>
	/// </summary>
	public abstract class HandleMessageOptionsBase
	{
		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleMessageOptionsBase" /></para>
		///     <para>class, without a logging service.</para>
		/// </summary>
		/// <param name="timeWindow">The time window within which a message is still valid for processing (older messages will be discarded).</param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <param name="exceptionHandler">An action that specifies how an exception should be handled.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		protected HandleMessageOptionsBase(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer,
			[CanBeNull] Action<Exception> exceptionHandler)
			: this(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, null, cancelToken, overflowContainer, exceptionHandler)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleMessageOptionsBase" /></para>
		///     <para>class, without a logging service or an exception handler.</para>
		/// </summary>
		/// <param name="timeWindow">The time window within which a message is still valid for processing (older messages will be discarded).</param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		protected HandleMessageOptionsBase(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer)
			: this(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, null, cancelToken, overflowContainer, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleMessageOptionsBase" /></para>
		///     <para>class, without a poison mesasge handler or an exception handler.</para>
		/// </summary>
		/// <param name="timeWindow">The time window within which a message is still valid for processing (older messages will be discarded).</param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		protected HandleMessageOptionsBase(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			[CanBeNull] ILogService logService,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer)
			: this(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, logService, cancelToken, overflowContainer, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleMessageOptionsBase" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="timeWindow">
		///     <para>The time window within which a message is still valid for processing (older messages will be discarded). Use <see cref="System.TimeSpan.Zero" /></para>
		///     <para>to ignore this check.</para>
		/// </param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <param name="exceptionHandler">An action that specifies how an exception should be handled.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		protected HandleMessageOptionsBase(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			[CanBeNull] ILogService logService,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer,
			[CanBeNull] Action<Exception> exceptionHandler)
		{
			try
			{
				if (overflowContainer == null) throw new ArgumentNullException("overflowContainer", "The overflow BLOB container is required");

				if (messageLeaseTime.TotalSeconds < TimeSpan.FromSeconds(30).TotalSeconds) throw new ArgumentException("Message Lease Time cannot be lower than 30 seconds!");

				if (pollFrequency.TotalSeconds < TimeSpan.FromSeconds(1).TotalSeconds) throw new ArgumentException("Poll Frequency cannot be lower than 1 second!");

				if (poisonMessageThreshold < 1) throw new ArgumentException("Poison Message Threshold cannot be lower than 1");

				this.TimeWindow = timeWindow;
				this.MessageLeaseTime = messageLeaseTime;
				this.PollFrequency = pollFrequency;
				this.PoisonMessageThreshold = poisonMessageThreshold;
				this.LogService = logService;
				this.CancelToken = cancelToken;
				this.OverflowMessageContainer = overflowContainer;
				this.ExceptionHandler = exceptionHandler;
			}
			catch (Exception ex)
			{
				if (logService != null) logService.Error(ex, "TheQ/CloudTools/HandleMessageOptions", "An error occurred while attempting to create an instance of TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions");

				throw;
			}
		}



		/// <summary>
		///     <list type="bullet">
		///         <item>
		///             <description></description>
		///         </item>
		///         <item>
		///             <description>The time window within which a message is still valid for processing (older messages will be discarded).</description>
		///         </item>
		///         <item>
		///             <description></description>
		///         </item>
		///     </list>
		/// </summary>
		public TimeSpan TimeWindow { get; private set; }


		/// <summary>
		///     The amount of time between periodic refreshes on the lease of a message.
		/// </summary>
		public TimeSpan MessageLeaseTime { get; private set; }


		/// <summary>
		///     The frequency with which the queue is being polled for new messages.
		/// </summary>
		public TimeSpan PollFrequency { get; private set; }


		/// <summary>
		///     The amount of times a message can be enqueued.
		/// </summary>
		public int PoisonMessageThreshold { get; private set; }


		/// <summary>
		///     The logging service to use.
		/// </summary>
		[CanBeNull]
		public ILogService LogService { get; private set; }


		/// <summary>
		///     A cancellation token to allow cancellation of this process.
		/// </summary>
		public CancellationToken CancelToken { get; private set; }


		/// <summary>
		///     Gets the BLOB container which contains queue messages that don't fit in the Queue.
		/// </summary>
		[NotNull]
		public IBlobContainer OverflowMessageContainer { get; private set; }


		/// <summary>
		///     An action that specifies how an exception should be handled.
		/// </summary>
		[CanBeNull]
		public Action<Exception> ExceptionHandler { get; private set; }
	}
}