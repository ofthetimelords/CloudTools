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
	public class OverflowHandlingDecorator : ExtendedQueueBase
	{
		public OverflowHandlingDecorator(ExtendedQueueBase decoratedQueue, IOverflownMessageHandler overflownMessageHandler)
		{
			this.DecoratedQueue = decoratedQueue;
			this.OverflownMessageHandler = overflownMessageHandler;
		}



		private ExtendedQueueBase DecoratedQueue { get; set; }


		private IOverflownMessageHandler OverflownMessageHandler { get; set; }


		protected internal override string SerializeMessageEntity(object messageEntity) { return this.DecoratedQueue.SerializeMessageEntity(messageEntity); }


		protected internal override Stream GetByteConverter(Stream originalConverter) { return this.DecoratedQueue.GetByteConverter(originalConverter); }


		protected internal override byte[] PostProcessMessage(byte[] originalContents) { return this.DecoratedQueue.PostProcessMessage(originalContents); }



		protected internal override Task AddNonOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			return this.DecoratedQueue.AddNonOverflownMessage(messageContents, token);
		}



		protected internal override async Task AddOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			var id = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

			await this.OverflownMessageHandler.Serialize(messageContents, id, this.Name, token).ConfigureAwait(false);
			var messagePointer = this.MessageProvider.Create(string.Concat(QueueMessageWrapper.OverflownMessagePrefix, id));
			await (this as IQueue).AddMessageAsync(messagePointer, token).ConfigureAwait(false);
		}
	}
}