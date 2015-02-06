// <copyright file="IQueueMessageProvider.cs" company="nett">
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



namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	/// <summary>
	///     Allows the generation of a <see cref="IQueueMessage" /> instance.
	/// </summary>
	public interface IQueueMessageProvider
	{
		/// <summary>
		///     Creates a <see cref="IQueueMessage" /> instance.
		/// </summary>
		/// <param name="message">The message as a string.</param>
		/// <returns>
		///     An <see cref="IQueueMessage" /> instance with the specified contents.
		/// </returns>
		IQueueMessage Create(string message);



		/// <summary>
		///     Creates a <see cref="IQueueMessage" /> instance.
		/// </summary>
		/// <param name="message">The message as a <see langword="byte" /> array.</param>
		/// <returns>
		///     An <see cref="IQueueMessage" /> instance with the specified contents.
		/// </returns>
		IQueueMessage Create(byte[] message);
	}
}