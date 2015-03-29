using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	public class AzureOverflownMessageHandler : IOverflownMessageHandler
	{
		private const string OverflownBlobNameFormat = "Overflown-{0}-{1}";
		private const string OverflownMessagePrefix = "*Overflown*";


		private IBlobContainer OverflowContainer { get; set; }



		public AzureOverflownMessageHandler(IBlobContainer container) { this.OverflowContainer = container; }



		public Task Serialize(byte[] originalMessage, string messageId, string queueName, CancellationToken token)
		{
			var blob = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, AzureOverflownMessageHandler.OverflownBlobNameFormat, queueName, messageId));

			return blob.UploadFromByteArrayAsync(originalMessage, 0, originalMessage.Length, token);
		}
	}
}
