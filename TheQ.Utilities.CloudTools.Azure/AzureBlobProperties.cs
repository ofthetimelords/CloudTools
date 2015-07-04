// <copyright file="AzureBlobProperties.cs" company="nett">
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

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     An implementation of <see cref="IBlobProperties" /> for Windows Azure.
	/// </summary>
	public class AzureBlobProperties : IBlobProperties
	{
		/// <summary>
		///     The underlying implementation.
		/// </summary>
		private readonly BlobProperties _blobPropertiesReference;



		/// <summary>
		///     Initializes a new instance of the <see cref="AzureBlobProperties" /> class.
		/// </summary>
		/// <param name="properties">The actual <see cref="BlobProperties" /> instance.</param>
		public AzureBlobProperties(BlobProperties properties)
		{
			Guard.NotNull(properties, "properties");

			this._blobPropertiesReference = properties;
		}



		/// <summary>
		///     Gets or sets the cache-control value stored for the blob.
		/// </summary>
		/// <value>
		///     A string containing the blob's cache-control value.
		/// </value>
		public string CacheControl
		{
			get
			{
				try
				{
					return this._blobPropertiesReference.CacheControl;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
			set
			{
				try
				{
					this._blobPropertiesReference.CacheControl = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


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
		public string ContentDisposition
		{
			get
			{
				return this._blobPropertiesReference.ContentDisposition;
			}
			set
			{
				this._blobPropertiesReference.ContentDisposition = value;
			}
		}


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
		public string ContentEncoding
		{
			get { return this._blobPropertiesReference.ContentEncoding; }
			set { this._blobPropertiesReference.ContentEncoding = value; }
		}


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
		public string ContentLanguage
		{
			get
			{
				return this._blobPropertiesReference.ContentLanguage;
			}
			set
			{
				this._blobPropertiesReference.ContentLanguage = value;
			}
		}


		/// <summary>
		///     Gets the size of the blob, in bytes.
		/// </summary>
		/// <value>
		///     A long value containing the blob's size in bytes.
		/// </value>
		public long Length
		{
			get
			{
				try
				{
					return this._blobPropertiesReference.Length;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets or sets the content-MD5 value stored for the blob.
		/// </summary>
		/// <value>
		///     A string containing the blob's content-MD5 hash.
		/// </value>
		public string ContentMD5
		{
			get
			{
				return this._blobPropertiesReference.ContentMD5;
			}
			set
			{
				this._blobPropertiesReference.ContentMD5 = value;
			}
		}


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
		public string ContentType
		{
			get
			{
				return this._blobPropertiesReference.ContentType;
			}
			set
			{
				this._blobPropertiesReference.ContentType = value;
			}
		}


		/// <summary>
		///     Gets the blob's <see cref="TheQ.Utilities.CloudTools.Azure.AzureBlobProperties.ETag" /> value.
		/// </summary>
		/// <value>
		///     A string containing the blob's <see cref="TheQ.Utilities.CloudTools.Azure.AzureBlobProperties.ETag" /> value.
		/// </value>
		public string ETag
		{
			get
			{
				return this._blobPropertiesReference.ETag;
			}
		}


		/// <summary>
		///     Gets the the last-modified time for the blob, expressed as a UTC value.
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>containing the blob's last-modified time, in UTC format.</para>
		/// </value>
		public DateTimeOffset? LastModified
		{
			get
			{
				return this._blobPropertiesReference.LastModified;
			}
		}



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="AzureBlobProperties" /> to <see cref="BlobProperties" /> .
		/// </summary>
		/// <param name="properties">The <see cref="AzureBlobProperties" /> instance.</param>
		/// <returns>
		///     The underlying <see cref="BlobProperties" /> instance.
		/// </returns>
		public static implicit operator BlobProperties(AzureBlobProperties properties) { return properties._blobPropertiesReference; }



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="BlobProperties" /> to <see cref="AzureBlobProperties" /> .
		/// </summary>
		/// <param name="properties">The <see cref="BlobProperties" /> instance.</param>
		/// <returns>
		///     A <see cref="AzureBlobProperties" /> wrapper.
		/// </returns>
		public static implicit operator AzureBlobProperties(BlobProperties properties) { return new AzureBlobProperties(properties); }
	}
}