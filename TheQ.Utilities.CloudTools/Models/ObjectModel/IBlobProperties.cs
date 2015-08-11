// <copyright file="IBlobProperties.cs" company="nett">
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
using System.Linq;



namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	/// <summary>
	///     Defines metadata about a BLOB, with the least required functionality for CloudTools.
	/// </summary>
	public interface IBlobProperties
	{
		/// <summary>
		///     Gets or sets the cache-control value stored for the blob.
		/// </summary>
		/// <value>
		///     A string containing the blob's cache-control value.
		/// </value>
		string CacheControl { get; set; }


		/// <summary>
		///     Gets or sets the content-disposition value stored for the blob.
		/// </summary>
		/// <remarks>
		///     <para>If this property has not been set for the blob, it returns <c>null</c></para>
		///     <para>.</para>
		/// </remarks>
		/// <value>
		///     A string containing the blob's content-disposition value.
		/// </value>
		string ContentDisposition { get; set; }


		/// <summary>
		///     Gets or sets the content-encoding value stored for the blob.
		/// </summary>
		/// <remarks>
		///     <para>If this property has not been set for the blob, it returns <c>null</c></para>
		///     <para>.</para>
		/// </remarks>
		/// <value>
		///     A string containing the blob's content-encoding value.
		/// </value>
		string ContentEncoding { get; set; }


		/// <summary>
		///     Gets or sets the content-language value stored for the blob.
		/// </summary>
		/// <remarks>
		///     <para>If this property has not been set for the blob, it returns <c>null</c></para>
		///     <para>.</para>
		/// </remarks>
		/// <value>
		///     A string containing the blob's content-language value.
		/// </value>
		string ContentLanguage { get; set; }


		/// <summary>
		///     Gets the size of the blob, in bytes.
		/// </summary>
		/// <value>
		///     A long value containing the blob's size in bytes.
		/// </value>
		long Length { get; }


		/// <summary>
		///     Gets or sets the content-MD5 value stored for the blob.
		/// </summary>
		/// <value>
		///     A string containing the blob's content-MD5 hash.
		/// </value>
		string ContentMD5 { get; set; }


		/// <summary>
		///     Gets or sets the content-type value stored for the blob.
		/// </summary>
		/// <remarks>
		///     <para>If this property has not been set for the blob, it returns <c>null</c></para>
		///     <para>.</para>
		/// </remarks>
		/// <value>
		///     A string containing the blob's content-type value.
		/// </value>
		string ContentType { get; set; }


		/// <summary>
		///     Gets the blob's <see cref="TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobProperties.ETag" /> value.
		/// </summary>
		/// <value>
		///     A string containing the blob's <see cref="TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobProperties.ETag" /> value.
		/// </value>
		string ETag { get; }


		/// <summary>
		///     Gets the the last-modified time for the blob, expressed as a UTC value.
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>containing the blob's last-modified time, in UTC format.</para>
		/// </value>
		DateTimeOffset? LastModified { get; }
	}
}