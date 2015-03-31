// <copyright file="OverflowHandlingDecorator.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/30</date>
// <summary>
// 
// </summary>

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public class OverflowHandlingDecorator : DecoratorBase
	{
		public OverflowHandlingDecorator(ExtendedQueueBase decoratedQueue, IOverflownMessageHandler overflownMessageHandler) : base(decoratedQueue)
		{
			this.OverflownMessageHandler = overflownMessageHandler;
		}



		private IOverflownMessageHandler OverflownMessageHandler { get; set; }



		protected internal override async Task AddOverflownMessageAsync(byte[] messageContents, CancellationToken token)
		{
			var id = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

			await this.OverflownMessageHandler.Serialize(messageContents, id, this.Name, token).ConfigureAwait(false);
			var messagePointer = this.MessageProvider.Create(this.OverflownMessageHandler.CreateMessagePointerFromId(id));
			await this.AddMessageAsync(messagePointer, token).ConfigureAwait(false);
		}



		protected internal override string GetOverflownMessageId(IQueueMessage message) { return this.OverflownMessageHandler.GetIdFromMessagePointer(message.AsBytes); }



		protected internal override Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token)
		{
			return this.OverflownMessageHandler.GetOverflownMessageContents(id, this.Name, token);
		}



		protected internal override Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token)
		{
			return this.OverflownMessageHandler.RemoveOverflownContentsAsync(message.OverflowId, this.Name, token);
		}
	}
}