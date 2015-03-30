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
		public virtual Task<T> DecodeMessageAsync<T>(QueueMessageWrapper wrapper, CancellationToken token) { return this.DecodeMessageAsync<T>(wrapper, token, this); }


		internal virtual async Task<T> DecodeMessageAsync<T>(QueueMessageWrapper wrapper, CancellationToken token, ExtendedQueueBase invoker)
		{
			var msgBytes = wrapper.ActualMessage.AsBytes;
			var overflownId = (wrapper.OverflowId = this.Get(invoker).GetOverflownMessageId(wrapper.ActualMessage));
			var wasOverflown = (wrapper.WasOverflown = !string.IsNullOrWhiteSpace(overflownId));

			msgBytes = await (wasOverflown
				? this.Get(invoker).GetOverflownMessageContentsAsync(wrapper.ActualMessage, overflownId, token)
				: this.Get(invoker).GetNonOverflownMessageContentsAsync(wrapper.ActualMessage, token)).ConfigureAwait(false);

			var serialized = await this.Get(invoker).ByteArrayToSerializedMessageContents(msgBytes, this.Get(invoker)).ConfigureAwait(false);

			return typeof(T) == typeof(string) ? (T)(object)serialized : this.DeserializeToObject<T>(serialized);
		}



		protected internal virtual async Task<string> ByteArrayToSerializedMessageContents(byte[] messageBytes, ExtendedQueueBase invoker)
		{
			using (var converter = new MemoryStream(messageBytes))
			{
				converter.Seek(0, SeekOrigin.Begin);

				using (var decoratedConverter = this.Get(invoker).GetByteDecoder(converter)) 
				using (var reader = new StreamReader(decoratedConverter)) 
					return await reader.ReadToEndAsync().ConfigureAwait(false);
			}
		}



		protected internal abstract Stream GetByteDecoder(Stream originalConverter);


		protected internal abstract string GetOverflownMessageId(IQueueMessage message);


		protected internal abstract Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token);


		protected internal abstract Task<byte[]> GetNonOverflownMessageContentsAsync(IQueueMessage message, CancellationToken token);


		protected internal abstract T DeserializeToObject<T>(string serializedContents);


		protected internal abstract Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token);
	}
}