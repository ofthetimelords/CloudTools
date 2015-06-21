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
	/// <summary>
	/// An <see cref="ExtendedQueueBase"/> decorator that handles overflown messages.
	/// </summary>
	public class OverflowHandlingDecorator : DecoratorBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OverflowHandlingDecorator"/> class.
		/// </summary>
		/// <param name="decoratedQueue">The decorated queue.</param>
		/// <param name="overflownMessageHandler">The overflown message handler.</param>
		public OverflowHandlingDecorator(ExtendedQueueBase decoratedQueue, IOverflownMessageHandler overflownMessageHandler) : base(decoratedQueue)
		{
			this.OverflownMessageHandler = overflownMessageHandler;
		}



		/// <summary>
		/// Gets or sets the overflown message handler.
		/// </summary>
		/// <value>
		/// An <see cref="IOverflownMessageHandler"/> instance.
		/// </value>
		private IOverflownMessageHandler OverflownMessageHandler { get; set; }



		/// <summary>
		/// Adds an overflown message to the queue, asynchronously.
		/// </summary>
		/// <param name="messageContents">The message contents to add.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous process.</returns>
		protected internal override async Task AddOverflownMessageAsync(byte[] messageContents, CancellationToken token)
		{
			var id = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

			await this.OverflownMessageHandler.StoreOverflownMessageAsync(messageContents, id, this.Name, token).ConfigureAwait(false);
			var messagePointer = this.MessageProvider.Create(this.OverflownMessageHandler.CreateMessagePointerFromId(id));
			await this.AddMessageAsync(messagePointer, token).ConfigureAwait(false);
		}



		protected internal override Task<string> GetOverflownMessageId(IQueueMessage message) { return Task.FromResult(this.OverflownMessageHandler.GetIdFromMessagePointer(message.AsBytes)); }



		protected internal override Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token)
		{
			return this.OverflownMessageHandler.RetrieveOverflownMessageAsync(id, this.Name, token);
		}



		protected internal override Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token)
		{
			return this.OverflownMessageHandler.RemoveOverflownContentsAsync(message.OverflowId, this.Name, token);
		}
	}
}