// <copyright file="AzureQueue.cs" company="nett">
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

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     An implementation of <see cref="IQueue" /> for Windows Azure.
	/// </summary>
	public class AzureQueue : IQueue
	{
		private readonly CloudQueue _queueReference;



		/// <summary>
		///     Initializes a new instance of the <see cref="AzureQueue" /> class.
		/// </summary>
		/// <param name="queue">The actual <see cref="CloudQueue" /> instance.</param>
		public AzureQueue(CloudQueue queue)
		{
			Guard.NotNull(queue, "queue");

			this._queueReference = queue;
		}



		/// <summary>
		///     Gets the name of the queue.
		/// </summary>
		/// <value>
		///     A string containing the name of the queue.
		/// </value>
		public string Name
		{
			get { return this._queueReference.Name; }
		}



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
		public Task AddMessageAsync(IQueueMessage message)
		{
			try
			{
				return this._queueReference.AddMessageAsync((AzureQueueMessage) message);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation to add a <paramref name="message" /> to the queue.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="CloudQueueMessage" /></para>
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
		public Task AddMessageAsync(IQueueMessage message, CancellationToken cancellationToken)
		{
			try
			{
				return this._queueReference.AddMessageAsync((AzureQueueMessage) message, cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation to get messages from the queue.
		/// </summary>
		/// <param name="messageCount">The number of messages to retrieve.</param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that is an enumerable collection of type <see cref="CloudQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public async Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount)
		{
			try
			{
				return (await this._queueReference.GetMessagesAsync(messageCount).ConfigureAwait(false)).Select(c => (AzureQueueMessage) c);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		///     <para>object that is an enumerable collection of type <see cref="CloudQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public async Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount, CancellationToken cancellationToken)
		{
			try
			{
				return (await this._queueReference.GetMessagesAsync(messageCount, cancellationToken).ConfigureAwait(false)).Select(c => (AzureQueueMessage) c);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		///     <para>object of type <see cref="CloudQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public async Task<IQueueMessage> GetMessageAsync(TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
		{
			try
			{
				return (AzureQueueMessage) (await this._queueReference.GetMessageAsync(cancellationToken).ConfigureAwait(false));
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		///     <para>object that is an enumerable collection of type <see cref="CloudQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public async Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount, TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
		{
			try
			{
				return (await this._queueReference.GetMessagesAsync(messageCount, visibilityTimeout, null, null, cancellationToken).ConfigureAwait(false)).Select(c => (AzureQueueMessage) c);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Deletes a message.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="CloudQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		public void DeleteMessage(IQueueMessage message)
		{
			try
			{
				this._queueReference.DeleteMessage((AzureQueueMessage) message);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Deletes the specified message from the queue.
		/// </summary>
		/// <param name="messageId">A string specifying the message ID.</param>
		/// <param name="popReceipt">A string specifying the pop receipt value.</param>
		public void DeleteMessage(string messageId, string popReceipt)
		{
			try
			{
				this._queueReference.DeleteMessage(messageId, popReceipt);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation to update the visibility timeout and optionally the content of a message.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="CloudQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		/// <param name="visibilityTimeout">
		///     <para>A <see cref="TimeSpan" /></para>
		///     <para>specifying the visibility timeout interval.</para>
		/// </param>
		/// <param name="updateFields">
		///     <para>A set of <see cref="MessageUpdateFields" /></para>
		///     <para>values that specify which parts of the <paramref name="message" /> are to be updated.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		public Task UpdateMessageAsync(IQueueMessage message, TimeSpan visibilityTimeout, QueueMessageUpdateFields updateFields)
		{
			try
			{
				return this._queueReference.UpdateMessageAsync((AzureQueueMessage) message, visibilityTimeout, (MessageUpdateFields) (int) updateFields);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation to update the visibility timeout and optionally the content of a message.
		/// </summary>
		/// <param name="message">
		///     <para>A <see cref="CloudQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		/// <param name="visibilityTimeout">
		///     <para>A <see cref="TimeSpan" /></para>
		///     <para>specifying the visibility timeout interval.</para>
		/// </param>
		/// <param name="updateFields">
		///     <para>A set of <see cref="MessageUpdateFields" /></para>
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
		public Task UpdateMessageAsync(IQueueMessage message, TimeSpan visibilityTimeout, QueueMessageUpdateFields updateFields, CancellationToken cancellationToken)
		{
			try
			{
				return this._queueReference.UpdateMessageAsync((AzureQueueMessage) message, visibilityTimeout, (MessageUpdateFields) (int) updateFields, cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="AzureQueue" /> to <see cref="CloudQueue" /> .
		/// </summary>
		/// <param name="queue">The <see cref="AzureQueue" /> instance.</param>
		/// <returns>
		///     The underlying <see cref="CloudQueue" /> instance.
		/// </returns>
		public static implicit operator CloudQueue(AzureQueue queue) { return queue._queueReference; }



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="CloudQueue" /> to <see cref="AzureQueue" /> .
		/// </summary>
		/// <param name="queue">The <see cref="CloudQueue" /> instance.</param>
		/// <returns>
		///     A <see cref="AzureQueue" /> wrapper.
		/// </returns>
		public static implicit operator AzureQueue(CloudQueue queue) { return new AzureQueue(queue); }



		/// <summary>
		///     Creates an <see cref="AzureQueue"/> from a <see cref="CloudQueue" /> instance.
		/// </summary>
		/// <param name="queue">The <see cref="CloudQueue" /> instance.</param>
		/// <returns>
		///     A <see cref="AzureQueue" /> wrapper.
		/// </returns>
		public static AzureQueue FromCloudQueue(CloudQueue queue)
		{
			return new AzureQueue(queue);
		}



		/// <summary>
		///     Retrieves the underlying <see cref="CloudQueue" /> instance from this <see cref="AzureQueue"/> instance.
		/// </summary>
		/// <param name="queue">The underlying <see cref="CloudQueue" /> instance.</param>
		/// <returns>
		///     An <see cref="AzureQueue" /> wrapper.
		/// </returns>
		public static CloudQueue ToCloudQueueMessage(AzureQueue queue) { return queue._queueReference; }
	}

}