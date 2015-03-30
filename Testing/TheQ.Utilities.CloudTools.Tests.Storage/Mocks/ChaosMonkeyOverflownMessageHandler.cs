using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;

using TheQ.Utilities.CloudTools.Azure.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.Blob;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Tests.Storage.Mocks
{
	public class ChaosMonkeyOverflownMessageHandler : IOverflownMessageHandler
	{
		public enum FailureMode
		{
			GetIdFromMessagePointer,
			Serialize,
			RemoveOverflownContents,
			GetOverflownContents
		}

		protected const string OverflownBlobNameFormat = "Overflown-{0}-{1}";
		protected const string OverflownMessagePrefix = "*Overflown*";

		private IBlobContainer OverflowContainer { get; set; }


		private FailureMode _failureMode;



		/// <summary>
		/// Initializes a new instance of the <see cref="AzureBlobOverflownMessageHandler"/> class.
		/// </summary>
		/// <param name="container">The BLOB container that will hold the overflown messages.</param>
		public ChaosMonkeyOverflownMessageHandler(IBlobContainer container, FailureMode failureMode)
		{
			this.OverflowContainer = container;
			this._failureMode = failureMode;
		}



		public string CreateMessagePointerFromId(string id)
		{
			Guard.NotNull(id, "id");

			return string.Concat(OverflownMessagePrefix, id);
		}



		public string GetIdFromMessagePointer(byte[] pointer)
		{
			if (this._failureMode == FailureMode.GetIdFromMessagePointer)
				throw new ArgumentNullException();

			Guard.NotNull(pointer, "pointer");
			var isOverflown = true;

			for (var i = 0; i < OverflownMessagePrefix.Length; i++)
				if (pointer[i] != OverflownMessagePrefix[i])
				{
					isOverflown = false;
					break;
				}

			if (!isOverflown) return string.Empty;

			var asString = Encoding.UTF8.GetString(pointer);
			return asString.Replace(OverflownMessagePrefix, string.Empty);
		}



		public Task Serialize(byte[] originalMessage, string messageId, string queueName, CancellationToken token)
		{
			if (this._failureMode == FailureMode.Serialize)
				throw new CloudToolsStorageException(new StorageException(), 404, "Blah");

			Guard.NotNull(originalMessage, "originalMessage");
			Guard.NotNull(messageId, "messageId");
			Guard.NotNull(queueName, "queueName");


			var blob = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, OverflownBlobNameFormat, queueName, messageId));

			return blob.UploadFromByteArrayAsync(originalMessage, 0, originalMessage.Length, token);
		}




		public Task RemoveOverflownContentsAsync(string id, string queueName, CancellationToken token)
		{
			if (this._failureMode == FailureMode.RemoveOverflownContents)
				throw new ArgumentNullException();

			try
			{
				var or = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, OverflownBlobNameFormat, queueName, id));
				or.DeleteIfExists();
			}
			catch (CloudToolsStorageException ex)
			{
				if (ex.StatusCode != 404 && ex.StatusCode != 409 && ex.StatusCode != 412)
					throw;
			}

			return Task.FromResult(false);
		}



		public Task<byte[]> GetOverflownMessageContents(string id, string queueName, CancellationToken token)
		{
			if (this._failureMode == FailureMode.GetOverflownContents)
				throw new CloudToolsStorageException(new StorageException(), 404, "Blah");


			var blobName = string.Format(CultureInfo.InvariantCulture, OverflownBlobNameFormat, queueName, id);

			return this.OverflowContainer.DownloadByteArrayAsync(blobName, token);
		}
	}
}
