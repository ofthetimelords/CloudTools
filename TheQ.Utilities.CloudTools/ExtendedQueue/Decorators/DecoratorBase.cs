// <copyright file="OverflowToBlobDecorator.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/28</date>
// <summary>
// 
// </summary>

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public abstract class DecoratorBase : ExtendedQueueBase
	{
		protected DecoratorBase(ExtendedQueueBase decoratedQueue) { this.DecoratedQueue = decoratedQueue; }



		protected ExtendedQueueBase DecoratedQueue { get; private set; }


		protected internal override string SerializeMessageEntity(object messageEntity) { return this.DecoratedQueue.SerializeMessageEntity(messageEntity); }


		protected internal override Stream GetByteEncoder(Stream originalConverter) { return this.DecoratedQueue.GetByteEncoder(originalConverter); }


		protected internal override Stream GetByteDecoder(Stream originalConverter) { return this.DecoratedQueue.GetByteDecoder(originalConverter); }



		protected internal override string GetOverflownMessageId(IQueueMessage message) { return this.DecoratedQueue.GetOverflownMessageId(message); }



		protected internal override Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token) { return this.DecoratedQueue.GetOverflownMessageContentsAsync(message, id, token); }


		protected internal override Task<byte[]> GetNonOverflownMessageContentsAsync(IQueueMessage message, CancellationToken token) { return this.DecoratedQueue.GetNonOverflownMessageContentsAsync(message, token); }


		protected internal override T DeserializeToObject<T>(string serializedContents) { return this.DecoratedQueue.DeserializeToObject<T>(serializedContents); }



		protected internal override Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token)
		{
			return this.DecoratedQueue.RemoveOverflownContentsAsync(message, token);
		}


		protected internal override byte[] PostProcessMessage(byte[] originalContents) { return this.DecoratedQueue.PostProcessMessage(originalContents); }



		protected internal override Task AddNonOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			return this.DecoratedQueue.AddNonOverflownMessage(messageContents, token);
		}



		protected internal override Task AddOverflownMessage(byte[] messageContents, CancellationToken token) { return this.DecoratedQueue.AddOverflownMessage(messageContents, token); }
	}
}