// <copyright file="BlobExtensions.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.Blob
{
	/// <summary>
	///     BLOB container and BLOB extension methods.
	/// </summary>
	public static class BlobExtensions
	{
		/// <summary>
		///     A more refined approach to download a BLOB to a <see langword="byte" /> array.
		/// </summary>
		/// <param name="container">The BLOB container to operate on.</param>
		/// <param name="blobName">The name of the BLOB to retrieve.</param>
		/// <exception cref="System.ArgumentNullException">Parameter <paramref name="container" /> or <paramref name="blobName" /> is null.</exception>
		/// <returns>
		///     A <see langword="byte" /> array of the retrieved data.
		/// </returns>
		[NotNull]
		public static async Task<byte[]> DownloadByteArrayAsync([NotNull] this IBlobContainer container, [NotNull] string blobName, CancellationToken token)
		{
			Guard.NotNull(container, "container");
			Guard.NotNull(blobName, "blobName");

			var blobData = container.GetBlobReference(blobName);
			await blobData.FetchAttributesAsync(token).ConfigureAwait(false);

			var blobDataArray = new byte[blobData.Properties.Length];
			await blobData.DownloadToByteArrayAsync(blobDataArray, 0, token).ConfigureAwait(false);

			return blobDataArray;
		}



		/// <summary>
		///     A more refined approach to download a BLOB to a <see langword="byte" /> array.
		/// </summary>
		/// <param name="blob">The BLOB reference to operate on.</param>
		/// <exception cref="System.ArgumentNullException">Parameter <paramref name="blob" /> is null.</exception>
		/// <returns>
		///     A <see langword="byte" /> array of the retrieved data.
		/// </returns>
		[NotNull]
		public static async Task<byte[]> DownloadByteArrayAsync([NotNull] this IBlob blob, CancellationToken token)
		{
			Guard.NotNull(blob, "blob");

			await blob.FetchAttributesAsync(token).ConfigureAwait(false);

			var blobDataArray = new byte[blob.Properties.Length];
			await blob.DownloadToByteArrayAsync(blobDataArray, 0, token).ConfigureAwait(false);

			return blobDataArray;
		}



		/// <summary>
		///     Serialises and uploads an object to a BLOB reference.
		/// </summary>
		/// <param name="blob">The BLOB to upload the object to.</param>
		/// <param name="instance">The object that will be serialised and then uploaded.</param>
		public static async void UploadObjectAsync([NotNull] this IBlob blob, [NotNull] object instance, CancellationToken token)
		{
			Guard.NotNull(blob, "blob");
			Guard.NotNull(instance, "instance");

			using (var ms = new MemoryStream())
			{
				new BinaryFormatter().Serialize(ms, instance);
				ms.Seek(0, SeekOrigin.Begin);
				await blob.UploadFromStreamAsync(ms, token).ConfigureAwait(false);
			}
		}
	}
}