// <copyright file="IOverflownMessageHandler.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/28</date>
// <summary>
// 
// </summary>


using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;

namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel
{
	/// <summary>
	/// Implementations of this class will handle messages that need to be written on an overflown store.
	/// </summary>
	public interface IOverflownMessageHandler
	{
		/// <summary>
		/// Stores an overflown message, asynchronously.
		/// </summary>
		/// <param name="originalMessage">The original converted message.</param>
		/// <param name="messagePointer">An identifier that joins a message on the overflown messages store and the queue.</param>
		/// <param name="queueName">The name of the queue.</param>
		/// <param name="token">A cancellation token.</param>
		/// <returns>A <see cref="Task"/> representing the current proccess.</returns>
		Task StoreOverflownMessageAsync([NotNull] byte[] originalMessage, string messagePointer, string queueName, CancellationToken token);


		/// <summary>
		/// Creates a message pointer from a message ID that joins the message on the overflown messages store and the queue.
		/// </summary>
		/// <param name="id">The identifier to generate a pointer from.</param>
		/// <returns>The resulting message pointer which will be stored in the queue and will identify a message in the overflown messages store.</returns>
		string CreateMessagePointerFromId(string id);


		/// <summary>
		/// Gets the identifier from the message pointer.
		/// </summary>
		/// <param name="pointerRaw">The raw pointer data.</param>
		/// <returns>The resulting message ID.</returns>
		string GetIdFromMessagePointer(byte[] pointerRaw);


		/// <summary>
		/// Removes the overflown message contents, asynchronously.
		/// </summary>
		/// <param name="id">The message identifier.</param>
		/// <param name="queueName">The name of the queue.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>A <see cref="Task"/> representing the current proccess.</returns>
		Task RemoveOverflownContentsAsync(string id, string queueName, CancellationToken token);


		/// <summary>
		/// Retrieves the overflown message contents, asynchronously.
		/// </summary>
		/// <param name="id">The message identifier.</param>
		/// <param name="queueName">the name of the queue.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>A byte array with the overflown message contents.</returns>
		Task<byte[]> RetrieveOverflownMessageAsync(string id, string queueName, CancellationToken token);
	}
}