// <copyright file="IExtendedQueue.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/31</date>
// <summary>
// Wraps an <see cref="IQueue" /> object with advancaed message adding and retrieving capabilities.
// </summary>

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel
{
	/// <summary>
	///     <para>Wraps an <see cref="IQueue" /> object with advancaed message adding and retrieving capabilities.</para>
	///     <para>
	///         The purpose of <see cref="IExtendedQueue" /> is to allow for common patterns when dealing with queues.
	///         Common problems when dealing with queues are:
	///     </para>
	///     <list type="bullet">
	///         <item>Managing leases.</item>
	///         <item>Dealing with messages in a parallel or batch fashion.</item>
	///         <item>Compressing messages to reduce storage usage.</item>
	///         <item>Support overflowing a message's contents to another store if it won't fit the underlying queue mechanism.</item>
	///     </list>
	///     <para>As with <see cref="IQueue" />, all implementations should be agnostic in regards to mechanism.</para>
	/// </summary>
	public interface IExtendedQueue : IQueue
	{
		/// <summary>
		/// Gets the statistics of this queue, for all its lifetime.
		/// </summary>
		/// <value>
		/// An object containing certain queue statistics (current and aggregate).
		/// </value>
		StatisticsContainer Statistics { get; }


		/// <summary>
		/// Gets the name of the queue
		/// </summary>
		/// <value>
		/// The name of the underlying queue.
		/// </value>
		string Name { get; }


		/// <summary>
		/// Gets the underlying <see cref="IQueue"/> implementation.
		/// </summary>
		IQueue OriginalQueue { get; }



		/// <summary>
		///     Adds an object (message entity) to the list asynchronously.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		/// <returns>A task representing the asynchronous procedure.</returns>
		Task AddMessageEntityAsync([NotNull] object entity);



		/// <summary>
		///     Adds an object (message entity) to the list asynchronously.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		/// <param name="token">An optional cancellation token.</param>
		/// <returns>
		///     A task representing the asynchronous procedure.
		/// </returns>
		Task AddMessageEntityAsync([NotNull] object entity, CancellationToken token);



		/// <summary>
		///     Adds an object (message entity) to the list.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		void AddMessageEntity([NotNull] object entity);



		/// <summary>
		///     Begins a task that receives messages in a batch and automatically manages their lifetime.
		/// </summary>
		/// <param name="messageOptions">An options object used to initialise the procedure.</param>
		/// <returns>A cancellable task representing the message processing procedure.</returns>
		Task HandleMessagesInBatchAsync([NotNull] HandleMessagesBatchOptions messageOptions);



		/// <summary>
		///     Begins a task that receives messages in parallel and automatically manages their lifetime.
		/// </summary>
		/// <param name="messageOptions">An options object used to initialise the procedure.</param>
		/// <returns>A cancellable task representing the message processing procedure.</returns>
		Task HandleMessagesInParallelAsync([NotNull] HandleMessagesParallelOptions messageOptions);



		/// <summary>
		///     Begins a task that receives messages serially and automatically manages their lifetime.
		/// </summary>
		/// <param name="messageOptions">An options object used to initialise the procedure.</param>
		/// <returns>A cancellable task representing the message processing procedure.</returns>
		Task HandleMessagesInSerialAsync([NotNull] HandleMessagesSerialOptions messageOptions);



		/// <summary>
		///     This member is intended for <c>internal</c> usage only. Converts an incoming message to an entity.
		/// </summary>
		/// <typeparam name="T">The type of the object to attempt to deserialise to.</typeparam>
		/// <param name="message">The original <paramref name="message"/>.</param>
		/// <param name="token">An optional cancellation token.</param>
		/// <returns>The contents of the message as an instance of type <typeparamref name="T" />.</returns>
		Task<T> DecodeMessageAsync<T>([NotNull] QueueMessageWrapper message, CancellationToken token);
	}
}