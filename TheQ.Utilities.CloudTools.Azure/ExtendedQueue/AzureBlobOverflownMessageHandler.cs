using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Azure.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Blob;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;

namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	/// <summary>
	///     Implements <see cref="IOverflownMessageHandler" /> for use with Azure BLOBs.
	/// </summary>
	public class AzureBlobOverflownMessageHandler : AzureOverflownMessageHandlerBase
	{
		/// <summary>
		/// The name format for BLOBs storing overflown messages.
		/// </summary>
		protected const string OverflownBlobNameFormat = "Overflown-{0}-{1}";

	
		
		/// <summary>
		///     Initializes a new instance of the <see cref="AzureBlobOverflownMessageHandler" /> class.
		/// </summary>
		/// <param name="container">The BLOB container that will hold the overflown messages.</param>
		public AzureBlobOverflownMessageHandler(IBlobContainer container)
		{
			this.OverflowContainer = container;
		}



		/// <summary>
		/// Gets or sets the overflow container.
		/// </summary>
		/// <value>
		/// An <see cref="IBlobContainer"/> instance.
		/// </value>
		private IBlobContainer OverflowContainer { get; set; }



		/// <summary>
		/// Stores an overflown message, asynchronously.
		/// </summary>
		/// <param name="originalMessage">The original converted message.</param>
		/// <param name="messagePointer">An identifier that joins a message on the overflown messages store and the queue.</param>
		/// <param name="queueName">The name of the queue.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>
		/// A <see cref="Task" /> representing the current proccess.
		/// </returns>
		public override Task StoreOverflownMessageAsync(byte[] originalMessage, string messagePointer, string queueName, CancellationToken token)
		{
			Guard.NotNull(originalMessage, "originalMessage");
			Guard.NotNull(messagePointer, "messagePointer");
			Guard.NotNull(queueName, "queueName");


			var blob = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, AzureBlobOverflownMessageHandler.OverflownBlobNameFormat, queueName, messagePointer));

			return blob.UploadFromByteArrayAsync(originalMessage, 0, originalMessage.Length, token);
		}



		/// <summary>
		/// Removes the overflown message contents, asynchronously.
		/// </summary>
		/// <param name="id">The message identifier.</param>
		/// <param name="queueName">The name of the queue.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>
		/// A <see cref="Task" /> representing the current proccess
		/// </returns>
		public override async Task RemoveOverflownContentsAsync(string id, string queueName, CancellationToken token)
		{
			try
			{
				var or = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, AzureBlobOverflownMessageHandler.OverflownBlobNameFormat, queueName, id));
				await or.DeleteIfExistsAsync().ConfigureAwait(false);
			}
			catch (CloudToolsStorageException ex)
			{
				if (ex.StatusCode != 404 && ex.StatusCode != 409 && ex.StatusCode != 412)
					throw;
			}
		}



		/// <summary>
		/// Retrieves the overflown message contents, asynchronously.
		/// </summary>
		/// <param name="id">The message identifier.</param>
		/// <param name="queueName">the name of the queue.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>
		/// A byte array with the overflown message contents.
		/// </returns>
		public override Task<byte[]> RetrieveOverflownMessageAsync(string id, string queueName, CancellationToken token)
		{
			var blobName = string.Format(CultureInfo.InvariantCulture, AzureBlobOverflownMessageHandler.OverflownBlobNameFormat, queueName, id);

			return this.OverflowContainer.DownloadByteArrayAsync(blobName, token);
		}
	}
}