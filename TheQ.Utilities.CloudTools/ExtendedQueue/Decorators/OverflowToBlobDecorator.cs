using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public class OverflowToBlobDecorator : ExtendedQueueBase
	{
		private ExtendedQueueBase DecoratedQueue { get; set; }

		private IOverflownMessageHandler OverflownMessageHandler { get; set; }


		public OverflowToBlobDecorator(ExtendedQueueBase decoratedQueue, IOverflownMessageHandler overflownMessageHandler)
		{
			this.DecoratedQueue = decoratedQueue;
			this.OverflownMessageHandler = overflownMessageHandler;
		}

		protected internal override string SerializeMessageEntity(object messageEntity) { return this.DecoratedQueue.SerializeMessageEntity(messageEntity); }


		protected internal override Stream GetByteConverter(Stream originalConverter) { return this.DecoratedQueue.GetByteConverter(originalConverter); }


		protected internal override byte[] PostProcessMessage(byte[] originalContents) { return this.DecoratedQueue.PostProcessMessage(originalContents); }


		protected internal override Task AddNonOverflownMessage(byte[] messageContents) { return this.DecoratedQueue.AddNonOverflownMessage(messageContents); }



		protected internal override async Task AddOverflownMessage(byte[] messageContents)
		{
			var id = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

			await this.OverflownMessageHandler.Serialise(messageContents, id).ConfigureAwait(false);
			var messagePointer = this.MessageProvider.Create(string.Concat(QueueMessageWrapper.OverflownMessagePrefix, id));
			await this.AddMessageAsync(messagePointer).ConfigureAwait(false);

		}
	}
}