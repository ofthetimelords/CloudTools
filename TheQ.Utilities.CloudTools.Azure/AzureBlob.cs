// <copyright file="AzureBlob.cs" company="nett">
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

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     <para>An implementation of <see cref="IBlob" /></para>
	///     <para>for <see cref="CloudBlockBlob" /></para>
	///     <para>.</para>
	/// </summary>
	public class AzureBlob : IBlob
	{
		/// <summary>
		///     <para>A <see cref="CloudBlockBlob" /></para>
		///     <para>instance, used by this instance.</para>
		/// </summary>
		private readonly CloudBlockBlob _blobReference;



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="AzureBlob" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="blob">
		///     <para>The actual <see cref="CloudBlockBlob" /></para>
		///     <para>instance.</para>
		/// </param>
		public AzureBlob(CloudBlockBlob blob)
		{
			Guard.NotNull(blob, "blob");

			this._blobReference = blob;
		}



		/// <summary>
		///     Initiates an asynchronous operation to open a stream for reading from the blob.
		/// </summary>
		public Task<Stream> OpenReadAsync()
		{
			try
			{
				return this._blobReference.OpenReadAsync();
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation to open a stream for reading from the blob.
		/// </summary>
		public Task<Stream> OpenReadAsync(CancellationToken cancellationToken)
		{
			try
			{
				return this._blobReference.OpenReadAsync(cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation to populate the blob's properties and metadata.
		/// </summary>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object that represents the asynchronous operation.</para>
		/// </returns>
		public Task FetchAttributesAsync()
		{
			try
			{
				return this._blobReference.FetchAttributesAsync();
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		public Task FetchAttributesAsync(CancellationToken cancellationToken)
		{
			try
			{
				return this._blobReference.FetchAttributesAsync(cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index)
		{
			try
			{
				return this._blobReference.DownloadToByteArrayAsync(target, index);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index, CancellationToken cancellationToken)
		{
			try
			{
				return this._blobReference.DownloadToByteArrayAsync(target, index, cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		public Task UploadFromStreamAsync(Stream source)
		{
			try
			{
				return this._blobReference.UploadFromStreamAsync(source);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		public Task UploadFromStreamAsync(Stream source, CancellationToken cancellationToken)
		{
			try
			{
				return this._blobReference.UploadFromStreamAsync(source, cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Gets the name of the blob.
		/// </summary>
		/// <value>
		///     A string containing the name of the blob.
		/// </value>
		public string Name
		{
			get
			{
				return this._blobReference.Name;
			}
		}


		/// <summary>
		///     <para>Gets a <see cref="CloudBlobContainer" /></para>
		///     <para>object representing the blob's container.</para>
		/// </summary>
		/// <value>
		///     <para>A <see cref="CloudBlobContainer" /></para>
		///     <para>object.</para>
		/// </value>
		public IBlobContainer Container
		{
			get
			{
				try
				{
					return (AzureBlobContainer) this._blobReference.Container;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets the blob's system properties.
		/// </summary>
		/// <value>
		///     The blob's properties.
		/// </value>
		public IBlobProperties Properties
		{
			get
			{
				try
				{
					return (AzureBlobProperties) this._blobReference.Properties;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}



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
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count)
		{
			try
			{
				return this._blobReference.UploadFromByteArrayAsync(buffer, index, count);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			try
			{
				return this._blobReference.UploadFromByteArrayAsync(buffer, index, count, cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation to check existence of the blob.
		/// </summary>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>object of type <c>bool</c></para>
		///     <para>that represents the asynchronous operation.</para>
		/// </returns>
		public Task<bool> ExistsAsync()
		{
			try
			{
				return this._blobReference.ExistsAsync();
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



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
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			try
			{
				return this._blobReference.ExistsAsync(cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Downloads the contents of a blob to a <see langword="byte" /> array.
		/// </summary>
		/// <param name="target">The target <see langword="byte" /> array.</param>
		/// <param name="index">The starting offset in the <see langword="byte" /> array.</param>
		/// <returns>
		///     The total number of bytes read into the buffer.
		/// </returns>
		public int DownloadToByteArray(byte[] target, int index)
		{
			try
			{
				return this._blobReference.DownloadToByteArray(target, index);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation to delete the current blob if it already exists.
		/// </summary>
		/// <returns>
		///     <para>
		///         <c>true</c>
		///     </para>
		///     <para>if the blob did already exist and was deleted; otherwise <c>false</c></para>
		///     <para>.</para>
		/// </returns>
		public bool DeleteIfExists()
		{
			try
			{
				return this._blobReference.DeleteIfExists();
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Deletes the blob if it already exists, asynchronously
		/// </summary>
		/// <returns>
		///     <para>
		///         <c>true</c>
		///     </para>
		///     <para>if the blob did already exist and was deleted; otherwise <c>false</c></para>
		///     <para>.</para>
		/// </returns>
		public Task<bool> DeleteIfExistsAsync()
		{
			try
			{
				return this._blobReference.DeleteIfExistsAsync();
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     <para>Performs an <see langword="implicit" /> conversion from <see cref="AzureBlob" /></para>
		///     <para>to <see cref="CloudBlockBlob" /></para>
		///     <para>.</para>
		/// </summary>
		/// <param name="blob">
		///     <para>The <see cref="AzureBlob" /></para>
		///     <para>.</para>
		/// </param>
		/// <returns>
		///     <para>The underlying <see cref="CloudBlockBlob" /></para>
		///     <para>instance.</para>
		/// </returns>
		public static implicit operator CloudBlockBlob(AzureBlob blob) { return blob._blobReference; }



		/// <summary>
		///     <para>Performs an <see langword="implicit" /> conversion from <see cref="CloudBlockBlob" /></para>
		///     <para>to <see cref="AzureBlob" /></para>
		///     <para>.</para>
		/// </summary>
		/// <param name="blob">
		///     <para>The original <see cref="CloudBlockBlob" /></para>
		///     <para>.</para>
		/// </param>
		/// <returns>
		///     <para>An <see cref="AzureBlob" /></para>
		///     <para>instance</para>
		/// </returns>
		public static implicit operator AzureBlob(CloudBlockBlob blob) { return new AzureBlob(blob); }
	}
}