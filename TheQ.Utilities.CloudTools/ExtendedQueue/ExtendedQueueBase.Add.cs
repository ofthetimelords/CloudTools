using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public abstract partial class ExtendedQueueBase
	{
		/// <summary>
		/// Adds an object (message entity) to the list.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public virtual void AddMessageEntity(object entity) { this.AddMessageEntityAsync(entity, CancellationToken.None).Wait(); }



		/// <summary>
		/// Adds an object (message entity) to the list, asynchronously.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public virtual Task AddMessageEntityAsync(object entity) { return this.AddMessageEntityAsync(entity, CancellationToken.None); }

		/// <summary>
		/// Adds an object (message entity) to the list.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns></returns>
		public virtual async Task AddMessageEntityAsync(object entity, CancellationToken token)
		{
			Guard.NotNull(entity, "entity");

			this.LogAction(LogSeverity.Info, "Added a message", "Queue name: {0}, Message payload: {1}", this.Name, entity.ToString());
			var maxSize = this.MaximumSizeProvider.MaximumMessageSize;

			var stringSource = entity as string;
			var serialized = stringSource ?? await this.Top.SerializeMessageEntity(entity).ConfigureAwait(false);

			var messageAsBytes = await this.Top.MessageContentsToByteArray(serialized).ConfigureAwait(false);
			messageAsBytes = await this.Top.PostProcessMessage(messageAsBytes).ConfigureAwait(false);

			if (messageAsBytes.Length < maxSize)
				await this.Top.AddNonOverflownMessageAsync(messageAsBytes, token).ConfigureAwait(false);
			else
			{
				this.LogAction(LogSeverity.Debug, "Message will be overflown", "Queue name: {0}, Message payload: {1}", this.Name, entity.ToString());
				await this.Top.AddOverflownMessageAsync(messageAsBytes, token).ConfigureAwait(false);
			}
		}



		/// <summary>
		/// Converts the contents of a serialised message to a byte array.
		/// </summary>
		/// <param name="serializedContents">The serialized message contents source.</param>
		/// <returns>A byte array representation of the serialised message contents.</returns>
		protected internal virtual async Task<byte[]> MessageContentsToByteArray(string serializedContents)
		{
			using (var converter = new MemoryStream(serializedContents.Length))
			{
				using (var decoratedConverter = await this.Top.GetByteEncoder(converter).ConfigureAwait(false))
				using (var writer = new StreamWriter(decoratedConverter))
				{
					await writer.WriteAsync(serializedContents).ConfigureAwait(false);
					await writer.FlushAsync().ConfigureAwait(false);
				}

				return converter.ToArray();
			}
		}



		/// <summary>
		/// Serializes a message entity to string.
		/// </summary>
		/// <param name="messageEntity">The message entity to serialise.</param>
		/// <returns>A string representation of the entity.</returns>
		protected internal abstract Task<string> SerializeMessageEntity(object messageEntity);



		/// <summary>
		/// Gets an additional that applies changes on the byte encoding stream.
		/// </summary>
		/// <param name="originalConverter">The original stream.</param>
		/// <returns>The new stream.</returns>
		protected internal abstract Task<Stream> GetByteEncoder(Stream originalConverter);



		/// <summary>
		/// Posts-processes a message's byte array content.
		/// </summary>
		/// <param name="originalContents">The original byte array contents of the serialised message.</param>
		/// <returns>The post-processed byte array message contents.</returns>
		protected internal abstract Task<byte[]> PostProcessMessage(byte[] originalContents);



		/// <summary>
		/// Adds a non-overflown message to the queue, asynchronously.
		/// </summary>
		/// <param name="messageContents">The message contents to add.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous process.</returns>
		protected internal abstract Task AddNonOverflownMessageAsync(byte[] messageContents, CancellationToken token);



		/// <summary>
		/// Adds an overflown message to the queue, asynchronously.
		/// </summary>
		/// <param name="messageContents">The message contents to add.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous process.</returns>
		protected internal abstract Task AddOverflownMessageAsync(byte[] messageContents, CancellationToken token);
	}
}