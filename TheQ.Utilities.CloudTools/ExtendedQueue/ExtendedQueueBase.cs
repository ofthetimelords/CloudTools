using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	/// <summary>
	/// A wrapper around an <see cref="IQueue"/> implementation that provides extended functionality (through decoration).
	/// </summary>
	public abstract partial class ExtendedQueueBase : IExtendedQueue
	{
		protected ExtendedQueueBase()
		{
			this.Statistics = new StatisticsContainer();
		}

		/// <summary>
		/// Used internally to mark the severity of a logged event.
		/// </summary>
		protected internal enum LogSeverity
		{
			Debug = 0,
			Critical = 1,
			Error = 2,
			Warning = 3,
			Info = 4
		}

	
		/// <summary>
		/// Gets or sets the maximum message size provider.
		/// </summary>
		/// <value>
		/// An <see cref="IMaximumMessageSizeProvider"/> instance.
		/// </value>
		protected internal virtual IMaximumMessageSizeProvider MaximumSizeProvider { get; set; }

		/// <summary>
		/// Gets or sets the maximum messages per-request provider.
		/// </summary>
		/// <value>
		/// An <see cref="IMaximumMessagesPerRequestProvider"/> instance.
		/// </value>
		protected internal virtual IMaximumMessagesPerRequestProvider MaximumMessagesProvider { get; set; }


		/// <summary>
		/// Gets or sets the <see cref="IQueue"/> instance to be wrapped.
		/// </summary>
		/// <value>
		/// An <see cref="IQueue"/> instance.
		/// </value>
		public virtual IQueue OriginalQueue { get; protected internal set; }


		/// <summary>
		/// Gets or sets the queue message provider.
		/// </summary>
		/// <value>
		/// An <see cref="IQueueMessageProvider"/> instance.
		/// </value>
		protected internal virtual IQueueMessageProvider MessageProvider { get; set; }


		/// <summary>
		///     Gets the name of the queue.
		/// </summary>
		/// <value>
		///     A string containing the name of the queue.
		/// </value>
		[NotNull]
		public virtual string Name
		{
			get { return this.OriginalQueue.Name; }
		}


		/// <summary>
		/// Gets the original <see cref="IQueue"/> that this implementation extends.
		/// </summary>
		/// <value>
		/// The underlying <see cref="IQueue"/> object.
		/// </value>
		public IQueue Original
		{
			get { return this.OriginalQueue; }
		}


		/// <summary>
		/// Gets the statistics of this queue, for all its lifetime.
		/// </summary>
		/// <value>
		/// An object containing certain queue statistics (current and aggregate).
		/// </value>
		public StatisticsContainer Statistics { get; protected internal set; }


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
		public virtual Task AddMessageAsync(IQueueMessage message)
		{
			return this.OriginalQueue.AddMessageAsync(message);
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
		public virtual Task AddMessageAsync(IQueueMessage message, CancellationToken cancellationToken)
		{
			return this.OriginalQueue.AddMessageAsync(message, cancellationToken);
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
		public virtual Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount)
		{
			return this.OriginalQueue.GetMessagesAsync(messageCount);
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
		public virtual Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount, CancellationToken cancellationToken)
		{
			return this.OriginalQueue.GetMessagesAsync(messageCount, cancellationToken);
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
		public virtual Task<IQueueMessage> GetMessageAsync(TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
		{
			return this.OriginalQueue.GetMessageAsync(visibilityTimeout, cancellationToken);
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
		public virtual Task<IEnumerable<IQueueMessage>> GetMessagesAsync(int messageCount, TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
		{
			return this.OriginalQueue.GetMessagesAsync(messageCount, visibilityTimeout, cancellationToken);
		}



		/// <summary>
		///     Deletes a message.
		/// </summary>
		/// <param name="message">
		///     <para>An <see cref="IQueueMessage" /></para>
		///     <para>object.</para>
		/// </param>
		public virtual void DeleteMessage(IQueueMessage message)
		{
			this.OriginalQueue.DeleteMessage(message);
		}



		/// <summary>
		///     Deletes the specified message from the queue.
		/// </summary>
		/// <param name="messageId">A string specifying the message ID.</param>
		/// <param name="popReceipt">A string specifying the pop receipt value.</param>
		public virtual void DeleteMessage(string messageId, string popReceipt)
		{
			this.OriginalQueue.DeleteMessage(messageId, popReceipt);
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
		public virtual Task UpdateMessageAsync(IQueueMessage message, TimeSpan visibilityTimeout, QueueMessageUpdateFields updateFields)
		{
			return this.OriginalQueue.UpdateMessageAsync(message, visibilityTimeout, updateFields);
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
		public virtual Task UpdateMessageAsync(IQueueMessage message, TimeSpan visibilityTimeout, QueueMessageUpdateFields updateFields, CancellationToken cancellationToken)
		{
			return this.OriginalQueue.UpdateMessageAsync(message, visibilityTimeout, updateFields, cancellationToken);
		}



		/// <summary>
		/// Logs an <see cref="Exception"/>.
		/// </summary>
		/// <param name="severity">The severity of the exception.</param>
		/// <param name="exception">The <see cref="Exception"/> itself.</param>
		/// <param name="details">The details of the log message.</param>
		/// <param name="formatArguments">The string formatting arguments of the details message.</param>
		[StringFormatMethod("details")]
		protected internal abstract void LogException(LogSeverity severity, Exception exception, string details = null, params string[] formatArguments);



		/// <summary>
		/// Logs a message.
		/// </summary>
		/// <param name="severity">The severity of the message.</param>
		/// <param name="message">The message's text.</param>
		/// <param name="details">The details of the log message.</param>
		/// <param name="formatArguments">The string formatting arguments of the details message.</param>
		[StringFormatMethod("details")]
		protected internal abstract void LogAction(LogSeverity severity, string message = null, string details = null, params string[] formatArguments);
	}
}