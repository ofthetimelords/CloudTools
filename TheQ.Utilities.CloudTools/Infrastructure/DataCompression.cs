// <copyright file="DataCompression.cs" company="nett">
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
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Storage.Infrastructure
{
	internal static class DataCompression
	{
		[NotNull]
		public static async Task<byte[]> CompressAsync([NotNull] string source)
		{
			if (source == null) return new byte[0];

			using (var ms = new MemoryStream(source.Length)) // Hardcoded...
			{
				using (var ds = new DeflateStream(ms, CompressionMode.Compress, true)) using (var sw = new StreamWriter(ds)) await sw.WriteAsync(source);

				return ms.ToArray();
			}
		}



		[CanBeNull]
		public static async Task<string> DecompressAsync([NotNull] byte[] source)
		{
			if (source == null) return null;

			using (var ms = new MemoryStream(source)) using (var ds = new DeflateStream(ms, CompressionMode.Decompress, true)) using (var sr = new StreamReader(ds)) return await sr.ReadToEndAsync();
		}
	}
}