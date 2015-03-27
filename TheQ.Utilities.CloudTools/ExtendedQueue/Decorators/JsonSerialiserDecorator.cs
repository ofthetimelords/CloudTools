using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceStack;
using ServiceStack.Text;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public class JsonSerialiserDecorator : ExtendedQueueBase
	{
		private ExtendedQueueBase DecoratedQueue { get; set; }

		public JsonSerialiserDecorator(ExtendedQueueBase decoratedQueue) { this.DecoratedQueue = decoratedQueue; }



		protected internal override string SerializeMessageEntity(object messageEntity) { return messageEntity.ToJsv(); }


		protected internal override Stream GetByteConverter(Stream originalConverter) { return this.DecoratedQueue.GetByteConverter(originalConverter); }


		protected internal override byte[] PostProcessMessage(byte[] originalContents) { return this.DecoratedQueue.PostProcessMessage(originalContents); }


		protected internal override Task AddNonOverflownMessage(byte[] messageContents) { return this.DecoratedQueue.AddNonOverflownMessage(messageContents); }


		protected internal override Task AddOverflownMessage(byte[] messageContents) { return this.DecoratedQueue.AddOverflownMessage(messageContents); }
	}
}