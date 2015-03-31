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
		///     Adds an object (message entity) to the list.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public virtual void AddMessageEntity(object entity) { this.AddMessageEntityAsync(entity).Wait(); }



		/// <summary>
		/// Adds an object (message entity) to the list.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		internal virtual void AddMessageEntity(object entity, ExtendedQueueBase invoker) { this.AddMessageEntityAsync(entity, CancellationToken.None, invoker).Wait(); }



		/// <summary>
		/// Adds an object (message entity) to the list, asynchronously.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public virtual Task AddMessageEntityAsync(object entity) { return this.AddMessageEntityAsync(entity, CancellationToken.None); }



		/// <summary>
		/// Adds an object (message entity) to the list asynchronously
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns></returns>
		public virtual Task AddMessageEntityAsync(object entity, CancellationToken token) { return this.AddMessageEntityAsync(entity, token, this); }

		/// <summary>
		/// Adds an object (message entity) to the list.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		/// <param name="token">A cancellation token.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		/// <returns></returns>
		internal virtual async Task AddMessageEntityAsync(object entity, CancellationToken token, ExtendedQueueBase invoker)
		{
			Guard.NotNull(entity, "entity");

			var maxSize = this.MaximumSizeProvider.MaximumMessageSize *3/4;

			var stringSource = entity as string;
			var serialized = stringSource ?? this.Get(invoker).SerializeMessageEntity(entity);

			var messageAsBytes = await this.Get(invoker).MessageContentsToByteArray(serialized, invoker).ConfigureAwait(false);
			messageAsBytes = this.Get(invoker).PostProcessMessage(messageAsBytes);

			if (messageAsBytes.Length < maxSize) await this.Get(invoker).AddNonOverflownMessageAsync(messageAsBytes, token).ConfigureAwait(false);
			else await this.Get(invoker).AddOverflownMessageAsync(messageAsBytes, token).ConfigureAwait(false);
		}



		/// <summary>
		/// Converts the contents of a serialised message to a byte array.
		/// </summary>
		/// <param name="serializedContents">The serialized message contents source.</param>
		/// <param name="invoker">The (optional) decorator that called this method.</param>
		/// <returns>A byte array representation of the serialised message contents.</returns>
		protected internal virtual async Task<byte[]> MessageContentsToByteArray(string serializedContents, ExtendedQueueBase invoker)
		{
			using (var converter = new MemoryStream(serializedContents.Length))
			{
				using (var decoratedConverter = this.Get(invoker).GetByteEncoder(converter))
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
		protected internal abstract string SerializeMessageEntity(object messageEntity);



		/// <summary>
		/// Gets an additional that applies changes on the byte encoding stream.
		/// </summary>
		/// <param name="originalConverter">The original stream.</param>
		/// <returns>The new stream.</returns>
		protected internal abstract Stream GetByteEncoder(Stream originalConverter);



		/// <summary>
		/// Posts-processes a message's byte array content.
		/// </summary>
		/// <param name="originalContents">The original byte array contents of the serialised message.</param>
		/// <returns>The post-processed byte array message contents.</returns>
		protected internal abstract byte[] PostProcessMessage(byte[] originalContents);



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