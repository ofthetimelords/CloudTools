using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Queue;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	public class AzureQueueMessageProvider : IQueueMessageProvider
	{
		/// <summary>
		///     Creates a <see cref="IQueueMessage" /> instance.
		/// </summary>
		/// <param name="message">The message as a string.</param>
		/// <returns>
		///     An <see cref="IQueueMessage" /> instance with the specified contents.
		/// </returns>
		public IQueueMessage Create(string message) { return new AzureQueueMessage(new CloudQueueMessage(message)); }



		/// <summary>
		///     Creates a <see cref="IQueueMessage" /> instance.
		/// </summary>
		/// <param name="message">The message as a <see langword="byte" /> array.</param>
		/// <returns>
		///     An <see cref="IQueueMessage" /> instance with the specified contents.
		/// </returns>
		public IQueueMessage Create(byte[] message) { return new AzureQueueMessage(new CloudQueueMessage(message)); }
	}
}
