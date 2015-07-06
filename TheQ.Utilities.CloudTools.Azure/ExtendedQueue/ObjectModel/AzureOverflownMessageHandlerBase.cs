using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue.ObjectModel
{
	/// <summary>
	/// A base implementation
	/// </summary>
	public abstract class AzureOverflownMessageHandlerBase : IOverflownMessageHandler
	{
		/// <summary>
		/// The message prefix for overflown message pointers.
		/// </summary>
		protected const string OverflownMessagePrefix = "*Overflown*";


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
		public abstract Task StoreOverflownMessageAsync(byte[] originalMessage, string messagePointer, string queueName, CancellationToken token);



		/// <summary>
		/// Creates a message pointer from a message ID that joins the message on the overflown messages store and the queue.
		/// </summary>
		/// <param name="id">The identifier to generate a pointer from.</param>
		/// <returns>The resulting message pointer which will be stored in the queue and will identify a message in the overflown messages store.</returns>
		public string CreateMessagePointerFromId(string id)
		{
			Guard.NotNull(id, "id");

			return string.Concat(AzureOverflownMessageHandlerBase.OverflownMessagePrefix, id);
		}



		/// <summary>
		/// Gets the identifier from the message pointer.
		/// </summary>
		/// <param name="pointerRaw">The raw pointer data.</param>
		/// <returns>The resulting message ID.</returns>
		public string GetIdFromMessagePointer(byte[] pointerRaw)
		{
			Guard.NotNull(pointerRaw, "pointerRaw");
			var isOverflown = true;

			var targetLength = AzureOverflownMessageHandlerBase.OverflownMessagePrefix.Length;

			if (pointerRaw.Length < targetLength)
				return string.Empty;

			for (var i = 0; i < targetLength; i++)
				if (pointerRaw[i] != AzureOverflownMessageHandlerBase.OverflownMessagePrefix[i])
				{
					isOverflown = false;
					break;
				}

			if (!isOverflown) return string.Empty;


			var asString = Encoding.UTF8.GetString(pointerRaw);
			return asString.Replace(AzureOverflownMessageHandlerBase.OverflownMessagePrefix, string.Empty);
		}



		/// <summary>
		/// Removes the overflown message contents, asynchronously.
		/// </summary>
		/// <param name="id">The message identifier.</param>
		/// <param name="queueName">The name of the queue.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>A <see cref="Task"/> representing the current proccess</returns>
		public abstract Task RemoveOverflownContentsAsync(string id, string queueName, CancellationToken token);



		/// <summary>
		/// Retrieves the overflown message contents, asynchronously.
		/// </summary>
		/// <param name="id">The message identifier.</param>
		/// <param name="queueName">the name of the queue.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>A byte array with the overflown message contents.</returns>
		public abstract Task<byte[]> RetrieveOverflownMessageAsync(string id, string queueName, CancellationToken token);
	}
}
