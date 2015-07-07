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
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	/// <summary>
	///     Represents the base class for <see cref="ExtendedQueueBase" /> decorators.
	/// </summary>
	public abstract class DecoratorBase : ExtendedQueueBase
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="DecoratorBase" /> class.
		/// </summary>
		/// <param name="decoratedQueue">The queue to decorate.</param>
		protected DecoratorBase(ExtendedQueueBase decoratedQueue)
		{
			this.DecoratedQueue = decoratedQueue;
		}



		/// <summary>
		///     Gets the decorated queue.
		/// </summary>
		/// <value>
		///     The decorated queue.
		/// </value>
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


		protected internal override IMaximumMessageSizeProvider MaximumSizeProvider
		{
			get { return this.DecoratedQueue.MaximumSizeProvider; }
			set { this.DecoratedQueue.MaximumSizeProvider = value; }
		}


		protected internal override IMaximumMessagesPerRequestProvider MaximumMessagesProvider
		{
			get { return this.DecoratedQueue.MaximumMessagesProvider; }
			set { this.DecoratedQueue.MaximumMessagesProvider = value; }
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



		/// <summary>
		///     Adds an object (message entity) to the list.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public override void AddMessageEntity(object entity)
		{
			this.DecoratedQueue.AddMessageEntity(entity, this);
		}



		internal override Task AddMessageEntityAsync(object entity, CancellationToken token, ExtendedQueueBase invoker)
		{
			return this.DecoratedQueue.AddMessageEntityAsync(entity, token, invoker);
		}



		internal override void AddMessageEntity(object entity, ExtendedQueueBase invoker)
		{
			this.DecoratedQueue.AddMessageEntity(entity, invoker);
		}



		public override Task AddMessageEntityAsync(object entity)
		{
			return this.DecoratedQueue.AddMessageEntityAsync(entity, CancellationToken.None, this);
		}



		public override Task AddMessageEntityAsync(object entity, CancellationToken token)
		{
			return this.DecoratedQueue.AddMessageEntityAsync(entity, token, this);
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
		///     <para>object of type <see cref="IQueueMessage" /></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public override Task<IQueueMessage> GetMessageAsync(TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
		{
			return this.DecoratedQueue.GetMessageAsync(visibilityTimeout, cancellationToken);
		}



		protected internal override Task<IQueueMessage> GetMessageFromQueue(HandleMessagesSerialOptions messageOptions, CancellationTokenSource messageSpecificCancellationTokenSource)
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



		protected internal override void HandleGeneralExceptions(HandleMessagesOptionsBase messageOptions, Exception ex)
		{
			this.DecoratedQueue.HandleGeneralExceptions(messageOptions, ex);
		}



		protected internal override void HandleStorageExceptions(HandleMessagesOptionsBase messageOptions, CloudToolsStorageException ex)
		{
			this.DecoratedQueue.HandleStorageExceptions(messageOptions, ex);
		}



		protected internal override bool HandleTaskCancelled(HandleMessagesSerialOptions messageOptions)
		{
			return this.DecoratedQueue.HandleTaskCancelled(messageOptions);
		}



		protected internal override Task<byte[]> MessageContentsToByteArray(string serializedContents, ExtendedQueueBase invoker)
		{
			return this.DecoratedQueue.MessageContentsToByteArray(serializedContents, invoker);
		}



		protected internal override void SerialFinallyHandler(
			HandleMessagesSerialOptions messageOptions,
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



		protected internal override Task<string> ByteArrayToSerializedMessageContents(byte[] messageBytes, ExtendedQueueBase invoker)
		{
			return this.DecoratedQueue.ByteArrayToSerializedMessageContents(messageBytes, invoker);
		}



		public override Task<T> DecodeMessageAsync<T>(QueueMessageWrapper wrapper, CancellationToken token)
		{
			return this.DecoratedQueue.DecodeMessageAsync<T>(wrapper, token, this);
		}



		/// <summary>
		///     Deletes the specified message from the queue.
		/// </summary>
		/// <param name="messageId">A string specifying the message ID.</param>
		/// <param name="popReceipt">A string specifying the pop receipt value.</param>
		public override void DeleteMessage(string messageId, string popReceipt)
		{
			this.DecoratedQueue.DeleteMessage(messageId, popReceipt);
		}



		protected internal override Task<string> SerializeMessageEntity(object messageEntity)
		{
			return this.DecoratedQueue.SerializeMessageEntity(messageEntity);
		}



		protected internal override Task<Stream> GetByteEncoder(Stream originalConverter)
		{
			return this.DecoratedQueue.GetByteEncoder(originalConverter);
		}



		protected internal override Task<Stream> GetByteDecoder(Stream originalConverter)
		{
			return this.DecoratedQueue.GetByteDecoder(originalConverter);
		}



		protected internal override Task<string> GetOverflownMessageId(IQueueMessage message)
		{
			return this.DecoratedQueue.GetOverflownMessageId(message);
		}



		protected internal override Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token)
		{
			return this.DecoratedQueue.GetOverflownMessageContentsAsync(message, id, token);
		}



		protected internal override Task<byte[]> GetNonOverflownMessageContentsAsync(IQueueMessage message, CancellationToken token)
		{
			return this.DecoratedQueue.GetNonOverflownMessageContentsAsync(message, token);
		}



		protected internal override T DeserializeToObject<T>(string serializedContents)
		{
			return this.DecoratedQueue.DeserializeToObject<T>(serializedContents);
		}



		protected internal override Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token)
		{
			return this.DecoratedQueue.RemoveOverflownContentsAsync(message, token);
		}



		protected internal override Task<byte[]> PostProcessMessage(byte[] originalContents)
		{
			return this.DecoratedQueue.PostProcessMessage(originalContents);
		}



		protected internal override Task AddNonOverflownMessageAsync(byte[] messageContents, CancellationToken token)
		{
			return this.DecoratedQueue.AddNonOverflownMessageAsync(messageContents, token);
		}



		protected internal override Task AddOverflownMessageAsync(byte[] messageContents, CancellationToken token)
		{
			return this.DecoratedQueue.AddOverflownMessageAsync(messageContents, token);
		}



		protected internal override void LogException(LogSeverity severity, Exception exception, string details = null, params string[] formatArguments)
		{
			this.DecoratedQueue.LogException(severity, exception, details, formatArguments);
		}



		protected internal override void LogAction(LogSeverity severity, string message = null, string details = null, params string[] formatArguments)
		{
			this.DecoratedQueue.LogAction(severity, message, details, formatArguments);
		}
	}
}