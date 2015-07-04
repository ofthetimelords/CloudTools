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

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	/// <summary>
	/// Represents a queue message wrapped to allow for <see cref="IExtendedQueue" /> support.
	/// </summary>
	public sealed class QueueMessageWrapper
	{
		private bool _initialized;
		private object _rawContent;
		private bool _wasOverflown;
		private string _overflowId;



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



		/// <summary>
		/// Gets the queue this message belongs to.
		/// </summary>
		/// <value>
		/// An <see cref="IExtendedQueue"/> instance.
		/// </value>
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


		/// <summary>
		/// Gets or sets a value indicating whether this message was overflown to a secondary store.
		/// </summary>
		/// <value>
		///   <c>true</c> if the message's contents were overflown; otherwise, <c>false</c>.
		/// </value>
		public bool WasOverflown
		{
			get
			{
				if (!this._initialized)
					this.EnsureMetadataIsPopulated();

				return this._wasOverflown;
			}
			protected internal set { this._wasOverflown = value; }
		}


		/// <summary>
		/// Gets or sets the overflow identifier, used to link a message's overflown contents from the marker message in the queue.
		/// </summary>
		/// <value>
		/// A string representing the overflow identifier.
		/// </value>
		[CanBeNull]
		public string OverflowId
		{
			get
			{
				if (!this._initialized)
					this.EnsureMetadataIsPopulated();

				return this._overflowId;
			}
			protected internal set { this._overflowId = value; }
		}



		/// <summary>
		/// Ensures that any metadata, such as <see cref="OverflowId"/> are assigned proper values by decoding the message's contents (which might be an expensive operation).
		/// </summary>
		public void EnsureMetadataIsPopulated()
		{
			this.GetMessageContentsAsync<object>().Wait();
		}


		/// <summary>
		/// Ensures that any metadata, such as <see cref="OverflowId"/> are assigned proper values by decoding the message's contents (which might be an expensive operation) asynchronously.
		/// </summary>
		/// <returns>A <see cref="Task"/> representing the asynchronous process.</returns>
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