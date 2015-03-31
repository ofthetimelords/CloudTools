using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Azure.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Blob;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	public class AzureBlobOverflownMessageHandler : AzureOverflownMessageHandlerBase
	{
		private IBlobContainer OverflowContainer { get; set; }



		/// <summary>
		/// Initializes a new instance of the <see cref="AzureBlobOverflownMessageHandler"/> class.
		/// </summary>
		/// <param name="container">The BLOB container that will hold the overflown messages.</param>
		public AzureBlobOverflownMessageHandler(IBlobContainer container) { this.OverflowContainer = container; }



		public override Task Serialize(byte[] originalMessage, string messageId, string queueName, CancellationToken token)
		{
			Guard.NotNull(originalMessage, "originalMessage");
			Guard.NotNull(messageId, "messageId");
			Guard.NotNull(queueName, "queueName");


			var blob = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, AzureOverflownMessageHandlerBase.OverflownBlobNameFormat, queueName, messageId));

			return blob.UploadFromByteArrayAsync(originalMessage, 0, originalMessage.Length, token);
		}



		public override async Task RemoveOverflownContentsAsync(string id, string queueName, CancellationToken token)
		{
			try
			{
				var or = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, AzureOverflownMessageHandlerBase.OverflownBlobNameFormat, queueName, id));
				or.DeleteIfExists();
			}
			catch (CloudToolsStorageException ex)
			{
				if (ex.StatusCode != 404 && ex.StatusCode != 409 && ex.StatusCode != 412)
					throw;
			}
		}



		public override Task<byte[]> GetOverflownMessageContents(string id, string queueName, CancellationToken token)
		{
			var blobName = string.Format(CultureInfo.InvariantCulture, AzureOverflownMessageHandlerBase.OverflownBlobNameFormat, queueName, id);

			return this.OverflowContainer.DownloadByteArrayAsync(blobName, token);
		}
	}
}
