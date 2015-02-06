// <copyright file="HandleSerialMessageOptions.cs" company="nett">
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
	public class HandleSerialMessageOptions : HandleMessageOptionsBase
	{
		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleSerialMessageOptions" /></para>
		///     <para>class, without a logging service.</para>
		/// </summary>
		/// <param name="timeWindow">The time window within which a message is still valid for processing (older messages will be discarded).</param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <param name="messageHandler">An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.</param>
		/// <param name="poisonHandler">
		///     <para>An action that specifies how a poison message should be handled, which fires before <see cref="TheQ.Utilities.CloudTools.Storage.Models.HandleSerialMessageOptions.MessageHandler" /></para>
		///     <para>. Returns a value indicating whether the message has been handled successfully and should be removed.</para>
		/// </param>
		/// <param name="exceptionHandler">An action that specifies how an exception should be handled.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		public HandleSerialMessageOptions(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer,
			[NotNull] Func<QueueMessageWrapper, bool> messageHandler,
			[CanBeNull] Func<QueueMessageWrapper, bool> poisonHandler,
			[CanBeNull] Action<Exception> exceptionHandler)
			: this(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, null, cancelToken, overflowContainer, messageHandler, poisonHandler, exceptionHandler)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleSerialMessageOptions" /></para>
		///     <para>class, without a logging service or an exception handler.</para>
		/// </summary>
		/// <param name="timeWindow">The time window within which a message is still valid for processing (older messages will be discarded).</param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <param name="messageHandler">An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.</param>
		/// <param name="poisonHandler">
		///     <para>An action that specifies how a poison message should be handled, which fires before <see cref="TheQ.Utilities.CloudTools.Storage.Models.HandleSerialMessageOptions.MessageHandler" /></para>
		///     <para>. Returns a value indicating whether the message has been handled successfully and should be removed.</para>
		/// </param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		public HandleSerialMessageOptions(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer,
			[NotNull] Func<QueueMessageWrapper, bool> messageHandler,
			[CanBeNull] Func<QueueMessageWrapper, bool> poisonHandler)
			: this(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, null, cancelToken, overflowContainer, messageHandler, poisonHandler, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleSerialMessageOptions" /></para>
		///     <para>class, without a poison mesasge handler or an exception handler.</para>
		/// </summary>
		/// <param name="timeWindow">The time window within which a message is still valid for processing (older messages will be discarded).</param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <param name="messageHandler">An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		public HandleSerialMessageOptions(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer,
			[NotNull] Func<QueueMessageWrapper, bool> messageHandler)
			: this(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, null, cancelToken, overflowContainer, messageHandler, null, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleSerialMessageOptions" /></para>
		///     <para>class, without an exception handler.</para>
		/// </summary>
		/// <param name="timeWindow">The time window within which a message is still valid for processing (older messages will be discarded).</param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <param name="messageHandler">An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.</param>
		/// <param name="poisonHandler">
		///     <para>An action that specifies how a poison message should be handled, which fires before <see cref="TheQ.Utilities.CloudTools.Storage.Models.HandleSerialMessageOptions.MessageHandler" /></para>
		///     <para>. Returns a value indicating whether the message has been handled successfully and should be removed.</para>
		/// </param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		public HandleSerialMessageOptions(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			[CanBeNull] ILogService logService,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer,
			[NotNull] Func<QueueMessageWrapper, bool> messageHandler,
			[CanBeNull] Func<QueueMessageWrapper, bool> poisonHandler)
			: this(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, logService, cancelToken, overflowContainer, messageHandler, poisonHandler, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleSerialMessageOptions" /></para>
		///     <para>class, without a poison mesasge handler or an exception handler.</para>
		/// </summary>
		/// <param name="timeWindow">The time window within which a message is still valid for processing (older messages will be discarded).</param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="overflowContainer">A BLOB container that contains messages that don't fit in the queue.</param>
		/// <param name="messageHandler">An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		public HandleSerialMessageOptions(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			[CanBeNull] ILogService logService,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer,
			[NotNull] Func<QueueMessageWrapper, bool> messageHandler)
			: this(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, logService, cancelToken, overflowContainer, messageHandler, null, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleSerialMessageOptions" /></para>
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
		/// <param name="messageHandler">An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.</param>
		/// <param name="poisonHandler">
		///     <para>An action that specifies how a poison message should be handled, which fires before <see cref="TheQ.Utilities.CloudTools.Storage.Models.HandleSerialMessageOptions.MessageHandler" /></para>
		///     <para>. Returns a value indicating whether the message has been handled successfully and should be removed.</para>
		/// </param>
		/// <param name="exceptionHandler">An action that specifies how an exception should be handled.</param>
		/// <exception cref="ArgumentNullException">messageHandler;The Message Handler <see langword="delegate" /> is required</exception>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		public HandleSerialMessageOptions(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			[CanBeNull] ILogService logService,
			CancellationToken cancelToken,
			[NotNull] IBlobContainer overflowContainer,
			[NotNull] Func<QueueMessageWrapper, bool> messageHandler,
			[CanBeNull] Func<QueueMessageWrapper, bool> poisonHandler,
			[CanBeNull] Action<Exception> exceptionHandler)
			: base(timeWindow, messageLeaseTime, pollFrequency, poisonMessageThreshold, logService, cancelToken, overflowContainer, exceptionHandler)

		{
			try
			{
				if (messageHandler == null) throw new ArgumentNullException("messageHandler", "The Message Handler delegate is required");

				if (overflowContainer == null) throw new ArgumentNullException("overflowContainer", "The overflow BLOB container is required");

				this.MessageHandler = messageHandler;
				this.PoisonHandler = poisonHandler;
			}
			catch (Exception ex)
			{
				if (logService != null) logService.Error(ex, "TheQ/CloudTools/HandleMessageOptions", "An error occurred while attempting to create an instance of TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions");

				throw;
			}
		}



		/// <summary>
		///     An action that specifies how a message should be handled. Returns a value indicating whether the message has been handled successfully and should be removed.
		/// </summary>
		[NotNull]
		public Func<QueueMessageWrapper, bool> MessageHandler { get; private set; }


		/// <summary>
		///     <para>An action that specifies how a poison message should be handled, which fires before <see cref="TheQ.Utilities.CloudTools.Storage.Models.HandleSerialMessageOptions.MessageHandler" /></para>
		///     <para>. Returns a value indicating whether the message has been handled successfully and should be removed.</para>
		/// </summary>
		[CanBeNull]
		public Func<QueueMessageWrapper, bool> PoisonHandler { get; private set; }
	}
}