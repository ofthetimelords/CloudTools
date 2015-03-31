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



		public override Task StoreOverflownMessageAsync(byte[] originalMessage, string messagePointer, string queueName, CancellationToken token)
		{
			Guard.NotNull(originalMessage, "originalMessage");
			Guard.NotNull(messagePointer, "messageId");
			Guard.NotNull(queueName, "queueName");


			var blob = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, AzureBlobOverflownMessageHandler.OverflownBlobNameFormat, queueName, messagePointer));

			return blob.UploadFromByteArrayAsync(originalMessage, 0, originalMessage.Length, token);
		}



		public override async Task RemoveOverflownContentsAsync(string id, string queueName, CancellationToken token)
		{
			try
			{
				var or = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, OverflownBlobNameFormat, queueName, id));
				await or.DeleteIfExistsAsync().ConfigureAwait(false);
			}
			catch (CloudToolsStorageException ex)
			{
				if (ex.StatusCode != 404 && ex.StatusCode != 409 && ex.StatusCode != 412)
					throw;
			}
		}



		public override Task<byte[]> RetrieveOverflownMessageAsync(string id, string queueName, CancellationToken token)
		{
			var blobName = string.Format(CultureInfo.InvariantCulture, OverflownBlobNameFormat, queueName, id);

			return this.OverflowContainer.DownloadByteArrayAsync(blobName, token);
		}
	}
}