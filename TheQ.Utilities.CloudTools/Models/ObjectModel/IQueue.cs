// <copyright file="IQueue.cs" company="nett">
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

using Microsoft.WindowsAzure.Storage.Queue;


namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	/// <summary>
	///     Indicates the kind of update that should be performed on a UpdateMessageAsync call.
	/// </summary>
	[Flags]
	public enum QueueMessageUpdateFields
	{
		/// <summary>
		///     Change the visibility of the message.
		/// </summary>
		Visibility = 1,


		/// <summary>
		///     Change the message contents.
		/// </summary>
		Content = 2
	}



	/// <summary>
	///     Defines a queue, with the minimum required functionality for CloudTools.
	/// </summary>
	public interface IQueue
	{
		/// <summary>
		///     Gets the name of the queue.
		/// </summary>
		/// <value>
		///     A string containing the name of the queue.
		/// </value>
		string Name { get; }



		/// <summary>
		///     Initiates an asynchronous operation to add a <paramref name="message" /> to the queue.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="CloudQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task AddMessageAsync(IQueueMessage message);



		/// <summary>
		///     Initiates an asynchronous operation to add a <paramref name="message" /> to the queue.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="IQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task AddMessageAsync(IQueueMessage message, CancellationToken cancellationToken);



		/// <summary>
		///     Initiates an asynchronous operation to get messages from the queue.
		/// </summary>
		/// <param name="messageCount">The number of messages to retrieve.</param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that is an enumerable collection of type <see cref="IQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount);



		/// <summary>
		///     Initiates an asynchronous operation to get messages from the queue.
		/// </summary>
		/// <param name="messageCount">The number of messages to retrieve.</param>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that is an enumerable collection of type <see cref="IQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount, CancellationToken cancellationToken);



		/// <summary>
		///     Initiates an asynchronous operation to get a single message from the queue, and specifies how long the message should be reserved before it becomes visible, and therefore available for deletion.
		/// </summary>
		/// <param name="visibilityTimeout">
		///     <para>A <see cref="TimeSpan" /></para>
		///     <para>specifying the visibility timeout interval.</para>
		/// </param>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object of type <see cref="IQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		Task<IQueueMessage> GetMessageAsync(TimeSpan? visibilityTimeout, CancellationToken cancellationToken);



		/// <summary>
		///     Initiates an asynchronous operation to get the specified number of messages from the queue using the specified request options and operation context. This operation marks the retrieved messages
		///     as invisible in the queue for the default visibility timeout period.
		/// </summary>
		/// <param name="messageCount">The number of messages to retrieve.</param>
		/// <param name="visibilityTimeout">
		///     <para>A <see cref="TimeSpan" /></para>
		///     <para>specifying the visibility timeout interval.</para>
		/// </param>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that is an enumerable collection of type <see cref="IQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount, TimeSpan? visibilityTimeout, CancellationToken cancellationToken);



		/// <summary>
		///     Deletes a <paramref name="message"/>.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="CloudQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		void DeleteMessage(IQueueMessage message);



		/// <summary>
		///     Deletes the specified message from the queue.
		/// </summary>
		/// <param name="messageId">A string specifying the message ID.</param>
		/// <param name="popReceipt">A string specifying the pop receipt value.</param>
		void DeleteMessage(string messageId, string popReceipt);


		/// <summary>
		///     Initiates an asynchronous operation to update the visibility timeout and optionally the content of a message.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="IQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		/// <param name="visibilityTimeout">
		///     <para>A <see cref="TimeSpan" /></para>
		///     <para>specifying the visibility timeout interval.</para>
		/// </param>
		/// <param name="updateFields">
		///     <para>A set of <see cref="QueueMessageUpdateFields" /></para>
		///     <para>values that specify which parts of the <paramref name="message" /> are to be updated.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task UpdateMessageAsync(IQueueMessage message, TimeSpan visibilityTimeout, QueueMessageUpdateFields updateFields);



		/// <summary>
		///     Initiates an asynchronous operation to update the visibility timeout and optionally the content of a message.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="IQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		/// <param name="visibilityTimeout">
		///     <para>A <see cref="TimeSpan" /></para>
		///     <para>specifying the visibility timeout interval.</para>
		/// </param>
		/// <param name="updateFields">
		///     <para>A set of <see cref="QueueMessageUpdateFields" /></para>
		///     <para>values that specify which parts of the <paramref name="message" /> are to be updated.</para>
		/// </param>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task UpdateMessageAsync(IQueueMessage message, TimeSpan visibilityTimeout, QueueMessageUpdateFields updateFields, CancellationToken cancellationToken);
	}
}