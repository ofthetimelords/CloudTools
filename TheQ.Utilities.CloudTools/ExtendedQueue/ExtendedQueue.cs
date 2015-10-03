// <copyright file="ExtendedQueue.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/07/04</date>
// <summary>
// 
// </summary>

using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;

namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	/// <summary>
	///     A wrapper around an <see cref="IQueue" /> implementation that provides extended functionality (through decoration).
	/// </summary>
	public class ExtendedQueue : ExtendedQueueBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedQueue" /> class.
		/// </summary>
		/// <param name="original">The original queue to be wrapped.</param>
		/// <param name="messageProvider">The queue message provider that's used to generate new queue messages.</param>
		/// <param name="maximumSizeSizeProvider">The maximum size of a queue's message provider.</param>
		/// <param name="maximumMessagesPerRequestProvider">The maximum queue messages per request provider.</param>
		/// <exception cref="ArgumentNullException">The parameter was null: + <paramref name="original" /></exception>
		public ExtendedQueue(
			[NotNull] IQueue original,
			IQueueMessageProvider messageProvider,
			IMaximumMessageSizeProvider maximumSizeSizeProvider,
			IMaximumMessagesPerRequestProvider maximumMessagesPerRequestProvider)
		{
			Guard.NotNull(original, "original");
			Guard.NotNull(messageProvider, "messageProvider");

			this.OriginalQueue = original;
			this.MessageProvider = messageProvider;
			this.MaximumSizeProvider = maximumSizeSizeProvider;
			this.MaximumMessagesProvider = maximumMessagesPerRequestProvider;
		}



		/// <summary>
		/// Logs an <see cref="Exception"/> when overriden by a decorator. This does nothing.
		/// </summary>
		/// <param name="severity">The severity of the exception.</param>
		/// <param name="exception">The <see cref="Exception"/> itself.</param>
		/// <param name="details">The details of the log message.</param>
		/// <param name="formatArguments">The string formatting arguments of the details message.</param>
		protected internal override void LogException(LogSeverity severity, Exception exception, string details = null, params string[] formatArguments)
		{
		}



		/// <summary>
		/// Logs a message when overriden by a decorator. This does nothing.
		/// </summary>
		/// <param name="severity">The severity of the message.</param>
		/// <param name="message">The message's text.</param>
		/// <param name="details">The details of the log message.</param>
		/// <param name="formatArguments">The string formatting arguments of the details message.</param>
		protected internal override void LogAction(LogSeverity severity, string message = null, string details = null, params string[] formatArguments)
		{
		}



		/// <summary>
		/// Serializes a message entity to string.
		/// </summary>
		/// <param name="messageEntity">The message entity to serialise.</param>
		/// <returns>A string representation of the entity.</returns>
		protected internal override Task<string> SerializeMessageEntity(object messageEntity)
		{
			// Not a preferrable method; this is meant to be overridden by a decorator
			using (var ms = new MemoryStream())
			{
				new BinaryFormatter().Serialize(ms, messageEntity);
				return Task.FromResult(Convert.ToBase64String(ms.ToArray()));
			}
		}



		/// <summary>
		/// Processes the object during encoding on the byte level.
		/// </summary>
		/// <param name="originalConverter">The original stream.</param>
		/// <returns>The new stream.</returns>
		protected internal override Task<Stream> GetByteEncoder(Stream originalConverter)
		{
			return Task.FromResult(originalConverter);
		}



		/// <summary>
		/// Processes the object during decoding on the byte level.
		/// </summary>
		/// <param name="originalConverter">The original stream.</param>
		/// <returns>The new stream.</returns>
		protected internal override Task<Stream> GetByteDecoder(Stream originalConverter)
		{
			return Task.FromResult(originalConverter);
		}



		/// <summary>
		/// Posts-processes a message's <see langword="byte"/> array content.
		/// </summary>
		/// <param name="originalContents">The original byte array contents of the serialised message.</param>
		/// <returns>The post-processed byte array message contents.</returns>
		protected internal override Task<byte[]> PostProcessMessage(byte[] originalContents)
		{
			return Task.FromResult(originalContents);
		}



		/// <summary>
		/// Adds a non-overflown message to the queue, asynchronously.
		/// </summary>
		/// <param name="messageContents">The message contents to add.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous process.</returns>
		protected internal override Task AddNonOverflownMessageAsync(byte[] messageContents, CancellationToken token)
		{
			return this.AddMessageAsync(this.MessageProvider.Create(messageContents), token);
		}



		/// <summary>
		///     Adds the overflown message.
		/// </summary>
		/// <param name="messageContents">The message contents.</param>
		/// <param name="token">The token.</param>
		/// <exception cref="ArgumentException">Message wouldn't fit in the Queue.</exception>
		/// <returns>
		/// </returns>
		protected internal override Task AddOverflownMessageAsync(byte[] messageContents, CancellationToken token)
		{
			this.Top.LogAction(LogSeverity.Error, "Message wouldn't fit in the Queue. The size of the message was " + messageContents.Length + " bytes.", "Check for misconfigured decorators.");
			throw new NotSupportedException("Message wouldn't fit in the Queue. The size of the message was " + messageContents.Length + " bytes.");
		}



		/// <summary>
		/// If supported, gets the ID used as a pointer to the overflown message contents.
		/// </summary>
		/// <param name="message">The message for which to retrieve the ID.</param>
		/// <returns>The Overflow ID of the message as a <see cref="string"/>.</returns>
		protected internal override Task<string> GetOverflownMessageId(IQueueMessage message)
		{
			// The default implementation does not support overflown messages, so it will have to assume that this is not an overflown message.
			return Task.FromResult(string.Empty);
		}



		/// <summary>
		/// If supported, gets the contents of an overflown message.
		/// </summary>
		/// <param name="message">The message of which to get the contents.</param>
		/// <param name="id">The Overflow ID.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>The contents of the message as a byte array.</returns>
		protected internal override Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token)
		{
			this.Top.LogAction(LogSeverity.Error, "Retrieving overflown messages is not supported by the default ExtendedQueue implementation", "Check for misconfigured decorators.");
			throw new NotSupportedException("Retrieving overflown messages is not supported by the default ExtendedQueue implementation.");
		}



		/// <summary>
		/// Gets the contents of an overflown message.
		/// </summary>
		/// <param name="message">The message of which to get the contents.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>The contents of the message as a byte array.</returns>
		protected internal override Task<byte[]> GetNonOverflownMessageContentsAsync(IQueueMessage message, CancellationToken token)
		{
			return Task.FromResult(message.AsBytes);
		}



		/// <summary>
		/// Attempts to deserialise a message to an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to attempt to deserialise to.</typeparam>
		/// <param name="serializedContents">The serialized contents of the message.</param>
		/// <returns>An object of type <typeparamref name="T"/>.</returns>
		protected internal override T DeserializeToObject<T>(string serializedContents)
		{
			// Not a preferrable method; this is meant to be overriden by a decorator
			using (var ms = new MemoryStream())
			using (var sw = new StreamWriter(ms))
			{
				sw.Write(Convert.FromBase64String(serializedContents));
				ms.Seek(0, SeekOrigin.Begin);

				return (T) new BinaryFormatter().Deserialize(ms);
			}
		}



		/// <summary>
		/// Cleans up the message contents that were stored outside of the message due to the contents being overflown.
		/// </summary>
		/// <param name="message">The message's contents.</param>
		/// <param name="token">An optional cancellation token.</param>
		protected internal override Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token)
		{
			return Task.FromResult(false);
		}
	}
}