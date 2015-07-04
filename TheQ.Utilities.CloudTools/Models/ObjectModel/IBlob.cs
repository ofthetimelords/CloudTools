// <copyright file="IBlob.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	/// <summary>
	///     Defines a BLOB, with the least required functionality for CloudTools
	/// </summary>
	public interface IBlob
	{
		/// <summary>
		///     Gets the name of the blob.
		/// </summary>
		/// <value>
		///     A string containing the name of the blob.
		/// </value>
		string Name { get; }


		/// <summary>
		///     <para>Gets a <see cref="IBlobContainer" /></para>
		///     <para>object representing the blob's container.</para>
		/// </summary>
		/// <value>
		///     <para>A <see cref="IBlobContainer" /></para>
		///     <para>object.</para>
		/// </value>
		IBlobContainer Container { get; }


		/// <summary>
		///     Gets the properties of this BLOB.
		/// </summary>
		/// <value>
		///     <para>A <see cref="IBlobProperties" /></para>
		///     <para>object.</para>
		/// </value>
		IBlobProperties Properties { get; }



		/// <summary>
		///     Initiates an asynchronous operation to open a stream for reading from the blob.
		/// </summary>
		Task<Stream> OpenReadAsync();



		/// <summary>
		///     Initiates an asynchronous operation to open a stream for reading from the blob.
		/// </summary>
		Task<Stream> OpenReadAsync(CancellationToken cancellationToken);



		/// <summary>
		///     Initiates an asynchronous operation to populate the blob's properties and metadata.
		/// </summary>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task FetchAttributesAsync();



		/// <summary>
		///     Initiates an asynchronous operation to populate the blob's properties and metadata.
		/// </summary>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task FetchAttributesAsync(CancellationToken cancellationToken);



		/// <summary>
		///     Initiates an asynchronous operation to download the contents of a blob to a <see langword="byte" /> array.
		/// </summary>
		/// <param name="target">The target <see langword="byte" /> array.</param>
		/// <param name="index">The starting offset in the <see langword="byte" /> array.</param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object of type <c>int</c></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		Task<int> DownloadToByteArrayAsync(byte[] target, int index);



		/// <summary>
		///     Initiates an asynchronous operation to download the contents of a blob to a <see langword="byte" /> array.
		/// </summary>
		/// <param name="target">The target <see langword="byte" /> array.</param>
		/// <param name="index">The starting offset in the <see langword="byte" /> array.</param>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object of type <c>int</c></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		Task<int> DownloadToByteArrayAsync(byte[] target, int index, CancellationToken cancellationToken);



		/// <summary>
		///     Initiates an asynchronous operation to upload a stream to a block blob.
		/// </summary>
		/// <param name="source">
		///     <para>A <see cref="Stream" /></para>
		///     <para>object providing the blob content.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task UploadFromStreamAsync(Stream source);



		/// <summary>
		///     Initiates an asynchronous operation to upload a stream to a block blob.
		/// </summary>
		/// <param name="source">
		///     <para>A <see cref="Stream" /></para>
		///     <para>object providing the blob content.</para>
		/// </param>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task UploadFromStreamAsync(Stream source, CancellationToken cancellationToken);



		/// <summary>
		///     Initiates an asynchronous operation to upload the contents of a <see langword="byte" /> array to a blob.
		/// </summary>
		/// <param name="buffer">An array of bytes.</param>
		/// <param name="index">The zero-based <see langword="byte" /> offset in <paramref name="buffer" /> at which to begin uploading bytes to the blob.</param>
		/// <param name="count">The number of bytes to be written to the blob.</param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task UploadFromByteArrayAsync(byte[] buffer, int index, int count);



		/// <summary>
		///     Initiates an asynchronous operation to upload the contents of a <see langword="byte" /> array to a blob.
		/// </summary>
		/// <param name="buffer">An array of bytes.</param>
		/// <param name="index">The zero-based <see langword="byte" /> offset in <paramref name="buffer" /> at which to begin uploading bytes to the blob.</param>
		/// <param name="count">The number of bytes to be written to the blob.</param>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken);



		/// <summary>
		///     Initiates an asynchronous operation to check existence of the blob.
		/// </summary>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object of type <c>bool</c></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		Task<bool> ExistsAsync();



		/// <summary>
		///     Initiates an asynchronous operation to check existence of the blob.
		/// </summary>
		/// <param name="cancellationToken">
		///     <para>A <see cref="CancellationToken" /></para>
		///     <para>to observe while waiting for a task to complete.</para>
		/// </param>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object of type <c>bool</c></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		Task<bool> ExistsAsync(CancellationToken cancellationToken);



		/// <summary>
		///     Downloads the contents of a blob to a <see langword="byte" /> array.
		/// </summary>
		/// <param name="target">The target <see langword="byte" /> array.</param>
		/// <param name="index">The starting offset in the <see langword="byte" /> array.</param>
		/// <returns>
		///     The total number of bytes read into the buffer.
		/// </returns>
		int DownloadToByteArray(byte[] target, int index);



		/// <summary>
		///     Deletes the current BLOB if it already exists.
		/// </summary>
		bool DeleteIfExists();



		/// <summary>
		///     Deletes the current BLOB if it already exists.
		/// </summary>
		Task<bool> DeleteIfExistsAsync();
	}
}