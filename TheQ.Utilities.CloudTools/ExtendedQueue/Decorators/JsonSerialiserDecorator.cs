using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public class JsonSerialiserDecorator : ExtendedQueueBase
	{
		private ExtendedQueueBase DecoratedQueue { get; set; }


		public JsonSerialiserDecorator(ExtendedQueueBase decoratedQueue) { this.DecoratedQueue = decoratedQueue; }



		protected internal override string SerializeMessageEntity(object messageEntity) { return JsonConvert.SerializeObject(messageEntity, Formatting.None); }


		protected internal override Stream GetByteConverter(Stream originalConverter) { return this.DecoratedQueue.GetByteConverter(originalConverter); }


		protected internal override byte[] PostProcessMessage(byte[] originalContents) { return this.DecoratedQueue.PostProcessMessage(originalContents); }



		protected internal override Task AddNonOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			return this.DecoratedQueue.AddNonOverflownMessage(messageContents, token);
		}



		protected internal override Task AddOverflownMessage(byte[] messageContents, CancellationToken token) { return this.DecoratedQueue.AddOverflownMessage(messageContents, token); }
	}
}