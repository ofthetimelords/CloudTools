// <copyright file="BlobExtensions.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/31</date>
// <summary>
// 
// </summary>

using System.Linq;
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
	}
}