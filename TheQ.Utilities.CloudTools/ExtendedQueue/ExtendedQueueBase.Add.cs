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
		public void AddMessageEntity(object entity) { this.AddMessageEntityAsync(entity).ConfigureAwait(false).GetAwaiter().GetResult(); }



		public virtual Task AddMessageEntityAsync(object entity) { return this.AddMessageEntityAsync(entity, CancellationToken.None); }



		public virtual async Task AddMessageEntityAsync(object entity, CancellationToken token)
		{
			Guard.NotNull(entity, "entity");

			var maxSize = this.MaximumMessageProvider.MaximumMessageSize *3/4;

			var stringSource = entity as string;
			var serialized = stringSource ?? this.SerializeMessageEntity(entity);

			var messageAsBytes = await this.MessageContentsToByteArray(serialized).ConfigureAwait(false);
			messageAsBytes = this.PostProcessMessage(messageAsBytes);

			if (messageAsBytes.Length < maxSize) await this.AddNonOverflownMessage(messageAsBytes, token).ConfigureAwait(false);
			else await this.AddOverflownMessage(messageAsBytes, token).ConfigureAwait(false);
		}



		protected virtual async Task<byte[]> MessageContentsToByteArray(string serializedContents)
		{
			using (var converter = new MemoryStream(serializedContents.Length))
			using (var decoratedConverter = this.GetByteConverter(converter))
			using (var writer = new StreamWriter(decoratedConverter))
			{
				await writer.WriteAsync(serializedContents).ConfigureAwait(false);
				return converter.ToArray();
			}
		}



		protected internal abstract string SerializeMessageEntity(object messageEntity);



		protected internal abstract Stream GetByteConverter(Stream originalConverter);



		protected internal abstract byte[] PostProcessMessage(byte[] originalContents);



		protected internal abstract Task AddNonOverflownMessage(byte[] messageContents, CancellationToken token);



		protected internal abstract Task AddOverflownMessage(byte[] messageContents, CancellationToken token);
	}
}