﻿// <copyright file="HandleMessageOptionsBase.cs" company="nett">
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



namespace TheQ.Utilities.CloudTools.Storage.Models
{
	/// <summary>
	///     <para>Input arguments for the ExtendedQueue framework.</para>
	/// </summary>
	public abstract class HandleMessagesOptionsBase
	{
		/// <summary>
		///     <para>Initializes a new instance of the <see cref="HandleMessagesOptionsBase" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="timeWindow">
		///     <para>The time window within which a message is still valid for processing (older messages will be discarded). Use <see cref="System.TimeSpan.Zero" /></para>
		///     <para>to ignore this check.</para>
		/// </param>
		/// <param name="messageLeaseTime">The amount of time between periodic refreshes on the lease of a message.</param>
		/// <param name="pollFrequency">The frequency with which the queue is being polled for new messages.</param>
		/// <param name="poisonMessageThreshold">The amount of times a message can be enqueued.</param>
		/// <param name="cancelToken">A cancellation token to allow cancellation of this process.</param>
		/// <param name="exceptionHandler">An action that specifies how an exception should be handled.</param>
		/// <exception cref="ArgumentException">
		///     Message Lease Time cannot be lower than 30 seconds! or Poll Frequency cannot be lower than 1 second! or Poison Message Threshold cannot be lower than 1
		/// </exception>
		protected HandleMessagesOptionsBase(
			TimeSpan timeWindow,
			TimeSpan messageLeaseTime,
			TimeSpan pollFrequency,
			int poisonMessageThreshold,
			CancellationToken cancelToken,
			[CanBeNull] Action<Exception> exceptionHandler = null)
		{
			if (messageLeaseTime.TotalSeconds < TimeSpan.FromSeconds(30).TotalSeconds) throw new ArgumentException("Message Lease Time cannot be lower than 30 seconds!");

			if (pollFrequency.TotalSeconds < TimeSpan.FromSeconds(1).TotalSeconds) throw new ArgumentException("Poll Frequency cannot be lower than 1 second!");

			if (poisonMessageThreshold < 1) throw new ArgumentException("Poison Message Threshold cannot be lower than 1");

			this.TimeWindow = timeWindow;
			this.MessageLeaseTime = messageLeaseTime;
			this.PollFrequency = pollFrequency;
			this.PoisonMessageThreshold = poisonMessageThreshold;
			this.CancelToken = cancelToken;
			this.ExceptionHandler = exceptionHandler;
		}



		/// <summary>
		/// The time window within which a message is still valid for processing (older messages will be discarded).
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
		///     A cancellation token to allow cancellation of this process.
		/// </summary>
		public CancellationToken CancelToken { get; private set; }


		/// <summary>
		///     An action that specifies how an exception should be handled.
		/// </summary>
		[CanBeNull]
		public Action<Exception> ExceptionHandler { get; private set; }
	}
}