// <copyright file="CloudEnvironment.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>


using System.Configuration;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace TheQ.Utilities.CloudTools.Tests.Integration.Azure
{
	internal class CloudEnvironment
	{
		public CloudEnvironment()
		{
			var cs = ConfigurationManager.ConnectionStrings["TestEnvironmentConnectionString"].ConnectionString;
			this.StorageAccount = CloudStorageAccount.Parse(cs);

			this.TableClient = this.StorageAccount.CreateCloudTableClient();
			this.BlobClient = this.StorageAccount.CreateCloudBlobClient();
			this.QueueClient = this.StorageAccount.CreateCloudQueueClient();
		}



		private CloudStorageAccount StorageAccount { get; set; }


		public CloudTableClient TableClient { get; private set; }


		public CloudBlobClient BlobClient { get; private set; }


		public CloudQueueClient QueueClient { get; private set; }



		public void BreakAnyLeases(string containerName, string blobName)
		{
			try
			{
				var container = this.BlobClient.GetContainerReference(containerName);

				//container.CreateIfNotExists();

				var blobReference = container.GetBlobReferenceFromServer(blobName);
				blobReference.BreakLease();
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 404 && ex.RequestInformation.HttpStatusCode != 409 && ex.RequestInformation.HttpStatusCode != 412) throw;
			}
		}
	}
}