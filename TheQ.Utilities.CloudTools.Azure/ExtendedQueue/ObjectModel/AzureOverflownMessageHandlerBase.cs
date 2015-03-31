using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue.ObjectModel
{
	public abstract class AzureOverflownMessageHandlerBase : IOverflownMessageHandler
	{
		protected const string OverflownBlobNameFormat = "Overflown-{0}-{1}";
		protected const string OverflownMessagePrefix = "*Overflown*";


		public abstract Task Serialize(byte[] originalMessage, string messageId, string queueName, CancellationToken token);



		public string CreateMessagePointerFromId(string id)
		{
			Guard.NotNull(id, "id");

			return string.Concat(AzureOverflownMessageHandlerBase.OverflownMessagePrefix, id);
		}



		public string GetIdFromMessagePointer(byte[] pointer)
		{
			Guard.NotNull(pointer, "pointer");
			var isOverflown = true;

			for (var i = 0; i < AzureOverflownMessageHandlerBase.OverflownMessagePrefix.Length; i++)
				if (pointer[i] != AzureOverflownMessageHandlerBase.OverflownMessagePrefix[i])
				{
					isOverflown = false;
					break;
				}

			if (!isOverflown) return string.Empty;


			//this.LogService.QuickLogDebug(
			//	"QueueMessageWrapper",
			//	"The message with ID '{0}' on queue '{1}' was an overflown message; proceeding to download data from the respective BLOB",
			//	this.ActualMessage.Id,
			//	this._queueName);

			var asString = Encoding.UTF8.GetString(pointer);
			return asString.Replace(AzureOverflownMessageHandlerBase.OverflownMessagePrefix, string.Empty);
		}



		public abstract Task RemoveOverflownContentsAsync(string id, string queueName, CancellationToken token);


		public abstract Task<byte[]> GetOverflownMessageContents(string id, string queueName, CancellationToken token);
	}
}
