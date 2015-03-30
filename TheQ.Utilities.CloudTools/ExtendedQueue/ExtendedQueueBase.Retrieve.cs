using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
		public async virtual Task<T> DecodeMessageAsync<T>(QueueMessageWrapper wrapper, CancellationToken token) 
		{
			var msgBytes = wrapper.ActualMessage.AsBytes;
			var overflownId = (wrapper.OverflowId = this.GetOverflownMessageId(wrapper.ActualMessage));
			var wasOverflown = (wrapper.WasOverflown = !string.IsNullOrWhiteSpace(overflownId));

			msgBytes = await (wasOverflown
				? this.GetOverflownMessageContentsAsync(wrapper.ActualMessage, overflownId, token)
				: this.GetNonOverflownMessageContentsAsync(wrapper.ActualMessage, token)).ConfigureAwait(false);

			var serialized = await this.ByteArrayToSerializedMessageContents(msgBytes).ConfigureAwait(false);

			return typeof(T) == typeof(string) ? (T) (object)serialized : this.DeserializeToObject<T>(serialized);



			//	this.LogService.QuickLogDebug("QueueMessageWrapper", "Attempting to identify a queue message with ID '{0}' on queue '{1}'", this.ActualMessage.Id, this._queueName);
			//	var amsg = this.ActualMessage.AsBytes;
			//	var sisOverflown = true;

			//	for (var i = 0; i < this.OverflownMessagePrefix.Length; i++)
			//	{
			//		if (msg[i] != QueueMessageWrapper.OverflownMessagePrefix[i])
			//		{
			//			isOverflown = false;
			//			break;
			//		}
			//	}

			//	if (isOverflown)
			//	{
			//		this.LogService.QuickLogDebug(
			//			"QueueMessageWrapper",
			//			"The message with ID '{0}' on queue '{1}' was an overflown message; proceeding to download data from the respective BLOB",
			//			this.ActualMessage.Id,
			//			this._queueName);
			//		this.WasOverflown = true;
			//		this.OverflowId = this.ActualMessage.AsString.Replace(QueueMessageWrapper.OverflownMessagePrefix, string.Empty);
			//		var oc = this.OverflowContainer;
			//		var or = oc.GetBlobReference(string.Format(CultureInfo.InvariantCulture, QueueMessageWrapper.OverflownBlobNameFormat, this._queueName, this.OverflowId));

			//		if (!await or.ExistsAsync()) throw new InvalidOperationException("Overflown message contents could not be located; was the blob deleted?");

			//		await or.FetchAttributesAsync();
			//		var buffer = new byte[or.Properties.Length];
			//		or.DownloadToByteArray(buffer, 0);

			//		return await DataCompression.DecompressAsync(buffer);
			//	}

			//	this.LogService.QuickLogDebug(
			//		"QueueMessageWrapper",
			//		"Successfully retrieved data for message with ID '{0}' on queue '{1}'. Procedding to decompression",
			//		this.ActualMessage.Id,
			//		this._queueName);
			//}
			//catch (DecoderFallbackException)
			//{
			//	this.LogService.QuickLogDebug(
			//		"QueueMessageWrapper",
			//		"The message with ID '{0}' on queue '{1}' was not overflown; procedding to decompress normally",
			//		this.ActualMessage.Id,
			//		this._queueName);
			//}

			//return await DataCompression.DecompressAsync(this.ActualMessage.AsBytes);
		}



		protected internal virtual async Task<string> ByteArrayToSerializedMessageContents(byte[] messageBytes)
		{
			using (var converter = new MemoryStream(messageBytes))
			using (var decoratedConverter = this.GetByteDecoder(converter))
			using (var reader = new StreamReader(decoratedConverter))
				return await reader.ReadToEndAsync().ConfigureAwait(false);
		}



		protected internal abstract Stream GetByteDecoder(Stream originalConverter);

		protected internal abstract string GetOverflownMessageId(IQueueMessage message);


		protected internal abstract Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token);


		protected internal abstract Task<byte[]> GetNonOverflownMessageContentsAsync(IQueueMessage message, CancellationToken token);


		protected internal abstract T DeserializeToObject<T>(string serializedContents);
		
		protected internal abstract Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token);
	}
}