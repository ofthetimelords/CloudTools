// <copyright file="DecoratorBase.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/30</date>
// <summary>
// 
// </summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public abstract class DecoratorBase : ExtendedQueueBase
	{
		protected DecoratorBase(ExtendedQueueBase decoratedQueue) { this.DecoratedQueue = decoratedQueue; }


		protected ExtendedQueueBase DecoratedQueue { get; private set; }


		protected internal override IQueueMessageProvider MessageProvider
		{
			get { return this.DecoratedQueue.MessageProvider; }
			set { this.DecoratedQueue.MessageProvider = value; }
		}


		/// <summary>
		///     Gets the name of the queue.
		/// </summary>
		/// <value>
		///     A string containing the name of the queue.
		/// </value>
		public override string Name
		{
			get { return this.DecoratedQueue.Name; }
		}


		protected internal override IMaximumMessageSizeProvider MaximumMessageProvider
		{
			get { return this.DecoratedQueue.MaximumMessageProvider; }
			set { this.DecoratedQueue.MaximumMessageProvider = value; }
		}


		protected internal override IQueue OriginalQueue
		{
			get { return this.DecoratedQueue.OriginalQueue; }
			set { this.DecoratedQueue.OriginalQueue = value; }
		}



		/// <summary>
		///     Initiates an asynchronous operation to add a <paramref name="message" /> to the queue.
		/// </summary>
		/// <param name="message">
		///     <para>An <see cref="IQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		public override Task AddMessageAsync(IQueueMessage message)
		{
			return this.DecoratedQueue.AddMessageAsync(message);
		}



		/// <summary>
		///     Initiates an asynchronous operation to add a <paramref name="message" /> to the queue.
		/// </summary>
		/// <param name="message">
		///     <para>An <see cref="IQueueMessage" /></para>
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
		public override Task AddMessageAsync(IQueueMessage message, CancellationToken cancellationToken)
		{
			return this.DecoratedQueue.AddMessageAsync(message, cancellationToken);
		}



		public override Task AddMessageEntityAsync(object entity) { return this.DecoratedQueue.AddMessageEntityAsync(entity); }


		public override Task AddMessageEntityAsync(object entity, CancellationToken token) { return this.DecoratedQueue.AddMessageEntityAsync(entity, token); }



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
		public override Task<IQueueMessage> GetMessageAsync(TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
		{
			return this.DecoratedQueue.GetMessageAsync(visibilityTimeout, cancellationToken);
		}



		protected internal override Task<IQueueMessage> GetMessageFromQueue(HandleSerialMessageOptions messageOptions, CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			return this.DecoratedQueue.GetMessageFromQueue(messageOptions, messageSpecificCancellationTokenSource);
		}



		/// <summary>
		///     Initiates an asynchronous operation to get messages from the queue.
		/// </summary>
		/// <param name="messageCount">The number of messages to retrieve.</param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that is an enumerable collection of type <see cref="IQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public override Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount)
		{
			return this.DecoratedQueue.GetMessagesAsync(messageCount);
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
		///     <para>object that is an enumerable collection of type <see cref="IQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public override Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount, CancellationToken cancellationToken)
		{
			return this.DecoratedQueue.GetMessagesAsync(messageCount, cancellationToken);
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
		///     <para>object that is an enumerable collection of type <see cref="IQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public override Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount, TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
		{
			return this.DecoratedQueue.GetMessagesAsync(messageCount, visibilityTimeout, cancellationToken);
		}



		protected internal override void HandleGeneralExceptions(HandleMessageOptionsBase messageOptions, Exception ex, bool parallelYetExternal = false)
		{
			this.DecoratedQueue.HandleGeneralExceptions(messageOptions, ex, parallelYetExternal);
		}



		protected internal override void HandleStorageExceptions(HandleMessageOptionsBase messageOptions, CloudToolsStorageException ex)
		{
			this.DecoratedQueue.HandleStorageExceptions(messageOptions, ex);
		}



		protected internal override void HandleTaskCancelled(HandleSerialMessageOptions messageOptions) { this.DecoratedQueue.HandleTaskCancelled(messageOptions); }


		protected internal override Task<byte[]> MessageContentsToByteArray(string serializedContents) { return this.DecoratedQueue.MessageContentsToByteArray(serializedContents); }



		protected internal override void SerialFinallyHandler(
			HandleSerialMessageOptions messageOptions,
			Task keepAliveTask,
			IQueueMessage message,
			CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			this.DecoratedQueue.SerialFinallyHandler(messageOptions, keepAliveTask, message, messageSpecificCancellationTokenSource);
		}



		/// <summary>
		///     Initiates an asynchronous operation to update the visibility timeout and optionally the content of a message.
		/// </summary>
		/// <param name="message">
		///     <para>An <see cref="IQueueMessage" /></para>
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
		public override Task UpdateMessageAsync(IQueueMessage message, TimeSpan visibilityTimeout, QueueMessageUpdateFields updateFields)
		{
			return this.DecoratedQueue.UpdateMessageAsync(message, visibilityTimeout, updateFields);
		}



		/// <summary>
		///     Initiates an asynchronous operation to update the visibility timeout and optionally the content of a message.
		/// </summary>
		/// <param name="message">
		///     <para>An <see cref="IQueueMessage" /></para>
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
		public override Task UpdateMessageAsync(IQueueMessage message, TimeSpan visibilityTimeout, QueueMessageUpdateFields updateFields, CancellationToken cancellationToken)
		{
			return this.DecoratedQueue.UpdateMessageAsync(message, visibilityTimeout, updateFields, cancellationToken);
		}



		/// <summary>
		///     Deletes a message.
		/// </summary>
		/// <param name="message">
		///     <para>An <see cref="IQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		public override void DeleteMessage(IQueueMessage message)
		{
			this.DecoratedQueue.DeleteMessage(message);
		}



		protected internal override Task<string> ByteArrayToSerializedMessageContents(byte[] messageBytes)
		{
			return this.DecoratedQueue.ByteArrayToSerializedMessageContents(messageBytes);
		}



		public override Task<T> DecodeMessageAsync<T>(QueueMessageWrapper wrapper, CancellationToken token) { return this.DecoratedQueue.DecodeMessageAsync<T>(wrapper, token); }



		/// <summary>
		///     Deletes the specified message from the queue.
		/// </summary>
		/// <param name="messageId">A string specifying the message ID.</param>
		/// <param name="popReceipt">A string specifying the pop receipt value.</param>
		public override void DeleteMessage(string messageId, string popReceipt)
		{
			this.DecoratedQueue.DeleteMessage(messageId, popReceipt);
		}



		protected internal override string SerializeMessageEntity(object messageEntity) { return this.DecoratedQueue.SerializeMessageEntity(messageEntity); }


		protected internal override Stream GetByteEncoder(Stream originalConverter) { return this.DecoratedQueue.GetByteEncoder(originalConverter); }


		protected internal override Stream GetByteDecoder(Stream originalConverter) { return this.DecoratedQueue.GetByteDecoder(originalConverter); }


		protected internal override string GetOverflownMessageId(IQueueMessage message) { return this.DecoratedQueue.GetOverflownMessageId(message); }



		protected internal override Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token)
		{
			return this.DecoratedQueue.GetOverflownMessageContentsAsync(message, id, token);
		}



		protected internal override Task<byte[]> GetNonOverflownMessageContentsAsync(IQueueMessage message, CancellationToken token)
		{
			return this.DecoratedQueue.GetNonOverflownMessageContentsAsync(message, token);
		}



		protected internal override T DeserializeToObject<T>(string serializedContents) { return this.DecoratedQueue.DeserializeToObject<T>(serializedContents); }



		protected internal override Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token)
		{
			return this.DecoratedQueue.RemoveOverflownContentsAsync(message, token);
		}



		protected internal override byte[] PostProcessMessage(byte[] originalContents) { return this.DecoratedQueue.PostProcessMessage(originalContents); }



		protected internal override Task AddNonOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			return this.DecoratedQueue.AddNonOverflownMessage(messageContents, token);
		}



		protected internal override Task AddOverflownMessage(byte[] messageContents, CancellationToken token) { return this.DecoratedQueue.AddOverflownMessage(messageContents, token); }
	}
}