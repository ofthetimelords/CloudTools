// <copyright file="ExtendedQueueBase.Retrieve.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/30</date>
// <summary>
// 
// </summary>

using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public abstract partial class ExtendedQueueBase
	{
		/// <summary>
		///     This member is intended for internal usage only. Converts an incoming message to an entity.
		/// </summary>
		/// <typeparam name="T">The type of the object to attempt to deserialise to.</typeparam>
		/// <param name="message">The original message.</param>
		/// <param name="token">An optional cancellation token.</param>
		/// <returns>The contents of the message as an instance of type <typeparamref name="T" />.</returns>
		public virtual async Task<T> DecodeMessageAsync<T>(QueueMessageWrapper message, CancellationToken token)
		{
			var msgBytes = message.ActualMessage.AsBytes;
			var overflownId = (message.SetOverflowId(await this.Top.GetOverflownMessageId(message.ActualMessage).ConfigureAwait(false)));
			var wasOverflown = (message.SetWasOverflown(!string.IsNullOrWhiteSpace(overflownId)));

			msgBytes = await (wasOverflown
				? this.Top.GetOverflownMessageContentsAsync(message.ActualMessage, overflownId, token)
				: this.Top.GetNonOverflownMessageContentsAsync(message.ActualMessage, token)).ConfigureAwait(false);

			var serialized = await this.Top.ByteArrayToSerializedMessageContents(msgBytes).ConfigureAwait(false);

			return typeof(T) == typeof(string) ? (T)(object)serialized : this.Top.DeserializeToObject<T>(serialized);
		}



		/// <summary>
		/// Converts a raw message content to a string representing a serialised entity.
		/// </summary>
		/// <param name="messageBytes">The message as a byte array.</param>
		/// <returns>The original serialised entity as a <see cref="string"/>.</returns>
		protected internal virtual async Task<string> ByteArrayToSerializedMessageContents(byte[] messageBytes)
		{
			using (var converter = new MemoryStream(messageBytes))
			{
				converter.Seek(0, SeekOrigin.Begin);

				using (var decoratedConverter = await this.Top.GetByteDecoder(converter).ConfigureAwait(false)) 
				using (var reader = new StreamReader(decoratedConverter)) 
					return await reader.ReadToEndAsync().ConfigureAwait(false);
			}
		}



		/// <summary>
		/// Processes the object during encoding on the byte level.
		/// </summary>
		/// <param name="originalConverter">The original stream.</param>
		/// <returns>The new stream.</returns>
		protected internal abstract Task<Stream> GetByteDecoder(Stream originalConverter);


		/// <summary>
		/// If supported, gets the ID used as a pointer to the overflown message contents.
		/// </summary>
		/// <param name="message">The message for which to retrieve the ID.</param>
		/// <returns>The Overflow ID of the message as a <see cref="string"/>.</returns>
		protected internal abstract Task<string> GetOverflownMessageId(IQueueMessage message);


		/// <summary>
		/// If supported, gets the contents of an overflown message.
		/// </summary>
		/// <param name="message">The message of which to get the contents.</param>
		/// <param name="id">The Overflow ID.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>The contents of the message as a byte array.</returns>
		protected internal abstract Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token);


		/// <summary>
		/// Gets the contents of an overflown message.
		/// </summary>
		/// <param name="message">The message of which to get the contents.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>The contents of the message as a byte array.</returns>
		protected internal abstract Task<byte[]> GetNonOverflownMessageContentsAsync(IQueueMessage message, CancellationToken token);


		/// <summary>
		/// Attempts to deserialise a message to an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to attempt to deserialise to.</typeparam>
		/// <param name="serializedContents">The serialized contents of the message.</param>
		/// <returns>An object of type <typeparamref name="T"/>.</returns>
		protected internal abstract T DeserializeToObject<T>(string serializedContents);


		/// <summary>
		/// Cleans up the message contents that were stored outside of the message due to the contents being overflown.
		/// </summary>
		/// <param name="message">The message's contents.</param>
		/// <param name="token">An optional cancellation token.</param>
		protected internal abstract Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token);
	}
}