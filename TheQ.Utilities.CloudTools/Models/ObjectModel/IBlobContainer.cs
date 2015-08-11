// <copyright file="IBlobContainer.cs" company="nett">
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
using System.Threading;
using System.Threading.Tasks;



namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	/// <summary>
	///     Defines a blob container with the least required functionality for CloudTools.
	/// </summary>
	public interface IBlobContainer
	{
		/// <summary>
		///     Initiates an asynchronous operation that creates the container if it does not already exist.
		/// </summary>
		Task<bool> CreateIfNotExistsAsync();



		/// <summary>
		///     Initiates an asynchronous operation that creates the container if it does not already exist.
		/// </summary>
		Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken);



		/// <summary>
		///     Gets a BLOB reference.
		/// </summary>
		/// <param name="blobName">The name of the BLOB.</param>
		/// <returns>
		///     <para>A <see cref="IBlob" /></para>
		///     <para>instance.</para>
		/// </returns>
		IBlob GetBlobReference(string blobName);
	}
}