// <copyright file="AzureQueueMessageProvider.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using System.Linq;

using Microsoft.WindowsAzure.Storage.Queue;

using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     An implementation of <see cref="IQueueMessageProvider" /> for Windows Azure.
	/// </summary>
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