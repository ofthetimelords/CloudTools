// <copyright file="AzureBlobContainer.cs" company="nett">
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

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     An implementation of <see cref="IBlobContainer" /> for Windows Azure.
	/// </summary>
	public class AzureBlobContainer : IBlobContainer
	{
		private readonly CloudBlobContainer _blobContainerReference;



		/// <summary>
		///     Initializes a new instance of the <see cref="AzureBlobContainer" /> class.
		/// </summary>
		/// <param name="container">The actual <see cref="CloudBlobContainer" /> instance.</param>
		public AzureBlobContainer(CloudBlobContainer container)
		{
			Guard.NotNull(container, "container");

			this._blobContainerReference = container;
		}



		/// <summary>
		///     Initiates an asynchronous operation that creates the container if it does not already exist.
		/// </summary>
		public async Task<bool> CreateIfNotExistsAsync()
		{
			try
			{
				return await this._blobContainerReference.CreateIfNotExistsAsync();
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Initiates an asynchronous operation that creates the container if it does not already exist.
		/// </summary>
		public async Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken)
		{
			try
			{
				return await this._blobContainerReference.CreateIfNotExistsAsync(cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Gets a reference to a blob in this container.
		/// </summary>
		/// <param name="blobName">The name of the blob.</param>
		/// <returns>
		///     A reference to a blob.
		/// </returns>
		public IBlob GetBlobReference(string blobName)
		{
			try
			{
				return (AzureBlob) this._blobContainerReference.GetBlockBlobReference(blobName);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Returns a task that renews a lease on this container.
		/// </summary>
		/// <param name="accessCondition">An <see cref="AccessCondition" /> object that represents the access conditions for the container, including a required lease ID.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for a task to complete.</param>
		/// <returns>
		///     A <see cref="Task" /> object that represents the current operation.
		/// </returns>
		public async Task RenewLeaseAsync(IAccessCondition accessCondition, CancellationToken cancellationToken)
		{
			try
			{
				await this._blobContainerReference.RenewLeaseAsync((AzureAccessCondition) accessCondition, cancellationToken);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Returns a task that renews a lease on this container.
		/// </summary>
		/// <param name="accessCondition">An <see cref="AccessCondition" /> object that represents the access conditions for the container, including a required lease ID.</param>
		/// <returns>
		///     A <see cref="Task" /> object that represents the current operation.
		/// </returns>
		public async Task RenewLeaseAsync(IAccessCondition accessCondition)
		{
			try
			{
				await this._blobContainerReference.RenewLeaseAsync((AzureAccessCondition) accessCondition);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="AzureBlobContainer" /> to <see cref="CloudBlobContainer" /> .
		/// </summary>
		/// <param name="container">The <see cref="AzureBlobContainer" /> instance.</param>
		/// <returns>
		///     The underlying <see cref="CloudBlobContainer" /> instance.
		/// </returns>
		public static implicit operator CloudBlobContainer(AzureBlobContainer container) { return container._blobContainerReference; }



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="CloudBlobContainer" /> to <see cref="AzureBlobContainer" /> .
		/// </summary>
		/// <param name="container">The <see cref="CloudBlobContainer" /> instance.</param>
		/// <returns>
		///     A <see cref="AzureBlobContainer" /> wrapper.
		/// </returns>
		public static implicit operator AzureBlobContainer(CloudBlobContainer container) { return new AzureBlobContainer(container); }
	}
}