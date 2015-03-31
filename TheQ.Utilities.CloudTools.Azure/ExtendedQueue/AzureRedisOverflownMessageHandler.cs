// <copyright file="AzureRedisOverflownMessageHandler.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/31</date>
// <summary>
// 
// </summary>

using System.Linq;



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