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
		public virtual void AddMessageEntity(object entity) { this.AddMessageEntityAsync(entity).Wait(); }



		internal virtual void AddMessageEntity(object entity, ExtendedQueueBase invoker) { this.AddMessageEntityAsync(entity, CancellationToken.None, invoker).Wait(); }



		public virtual Task AddMessageEntityAsync(object entity) { return this.AddMessageEntityAsync(entity, CancellationToken.None); }



		public virtual Task AddMessageEntityAsync(object entity, CancellationToken token) { return this.AddMessageEntityAsync(entity, token, this); }

		internal virtual async Task AddMessageEntityAsync(object entity, CancellationToken token, ExtendedQueueBase invoker)
		{
			Guard.NotNull(entity, "entity");

			var maxSize = this.MaximumSizeProvider.MaximumMessageSize *3/4;

			var stringSource = entity as string;
			var serialized = stringSource ?? this.Get(invoker).SerializeMessageEntity(entity);

			var messageAsBytes = await this.Get(invoker).MessageContentsToByteArray(serialized, invoker).ConfigureAwait(false);
			messageAsBytes = this.Get(invoker).PostProcessMessage(messageAsBytes);

			if (messageAsBytes.Length < maxSize) await this.Get(invoker).AddNonOverflownMessage(messageAsBytes, token).ConfigureAwait(false);
			else await this.Get(invoker).AddOverflownMessage(messageAsBytes, token).ConfigureAwait(false);
		}



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



		protected internal abstract string SerializeMessageEntity(object messageEntity);



		protected internal abstract Stream GetByteEncoder(Stream originalConverter);



		protected internal abstract byte[] PostProcessMessage(byte[] originalContents);



		protected internal abstract Task AddNonOverflownMessage(byte[] messageContents, CancellationToken token);



		protected internal abstract Task AddOverflownMessage(byte[] messageContents, CancellationToken token);
	}
}