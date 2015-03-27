using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public class CompressionDecorator : ExtendedQueueBase
	{
		private ExtendedQueueBase DecoratedQueue { get; set; }

		public CompressionDecorator(ExtendedQueueBase decoratedQueue) { this.DecoratedQueue = decoratedQueue; }


		protected internal override string SerializeMessageEntity(object messageEntity) { return this.DecoratedQueue.SerializeMessageEntity(messageEntity); }


		protected internal override Stream GetByteConverter(Stream originalConverter) { return new DeflateStream(originalConverter, CompressionMode.Compress, true); }


		protected internal override byte[] PostProcessMessage(byte[] originalContents) { return this.DecoratedQueue.PostProcessMessage(originalContents); }


		protected internal override Task AddNonOverflownMessage(byte[] messageContents) { return this.DecoratedQueue.AddNonOverflownMessage(messageContents); }


		protected internal override Task AddOverflownMessage(byte[] messageContents) { return this.DecoratedQueue.AddOverflownMessage(messageContents); }
	}
}