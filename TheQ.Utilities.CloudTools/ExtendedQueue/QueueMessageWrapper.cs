// <copyright file="QueueMessageWrapper.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/30</date>
// <summary>
// 
// </summary>

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public sealed class QueueMessageWrapper
	{
		private bool _initialized;
		private object _rawContent;



		/// <summary>
		///     Initializes a new instance of the
		///     <see cref="QueueMessageWrapper" />
		///     class.
		/// </summary>
		/// <param name="queue">The queue the message belongs to.</param>
		/// <param name="message">The underlying message.</param>
		public QueueMessageWrapper([NotNull] IExtendedQueue queue, [NotNull] IQueueMessage message)
		{
			Guard.NotNull(queue, "queue");
			Guard.NotNull(message, "message");

			this.ParentQueue = queue;
			this.ActualMessage = message;
		}



		public IExtendedQueue ParentQueue { get; private set; }


		/// <summary>
		///     Gets the underlying
		///     <see cref="IQueueMessage" />
		///     instance for raw processing.
		/// </summary>
		/// <value>
		///     The actual
		///     <see cref="IQueueMessage" />
		///     instance.
		/// </value>
		[NotNull]
		public IQueueMessage ActualMessage { get; private set; }


		public bool WasOverflown { get; protected internal set; }


		[CanBeNull]
		public string OverflowId { get; protected internal set; }


		public void EnsureMetadataIsPopulated() { this.GetMessageContentsAsync<object>().Wait(); }


		public async Task EnsureMetadataIsPopulatedAsync() { await this.GetMessageContentsAsync<object>().ConfigureAwait(false); }



		[NotNull]
		public T GetMessageContents<T>() { return this.GetMessageContentsAsync<T>().Result; }



		[NotNull]
		public Task<T> GetMessageContentsAsync<T>() { return this.GetMessageContentsAsync<T>(CancellationToken.None); }



		[NotNull]
		public async Task<T> GetMessageContentsAsync<T>(CancellationToken token)
		{
			if (!this._initialized)
			{
				this._rawContent = await this.ParentQueue.DecodeMessageAsync<T>(this, token).ConfigureAwait(false);
				this._initialized = true;
			}

			return (T) this._rawContent;
		}
	}
}