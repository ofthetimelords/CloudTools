using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	//public class AzureRedisOverflownMessageHandler : IOverflownMessageHandler
	//{
	//	private const string OverflownBlobNameFormat = "Overflown-{0}-{1}";
	//	private const string OverflownMessagePrefix = "*Overflown*";




	//	/// <summary>
	//	/// Initializes a new instance of the <see cref="AzureBlobOverflownMessageHandler"/> class.
	//	/// </summary>
	//	/// <param name="container">The BLOB container that will hold the overflown messages.</param>
	//	public AzureRedisOverflownMessageHandler() {  }



	//	public Task Serialize(byte[] originalMessage, string messageId, string queueName, CancellationToken token)
	//	{
	//		var blob = this.OverflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, AzureRedisOverflownMessageHandler.OverflownBlobNameFormat, queueName, messageId));

	//		return blob.UploadFromByteArrayAsync(originalMessage, 0, originalMessage.Length, token);
	//	}



	//	public string CreateMessagePointerFromId(string id)
	//	{
	//		Guard.NotNull(id, "id");
	//		return string.Concat(AzureRedisOverflownMessageHandler.OverflownMessagePrefix, id);
	//	}



	//	public string GetIdFromMessagePointer(string pointer)
	//	{
	//		Guard.NotNull(pointer, "pointer");

	//		return pointer.Replace(AzureRedisOverflownMessageHandler.OverflownMessagePrefix, string.Empty);
	//	}
	//}
}
