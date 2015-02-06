// <copyright file="AzureAccessCondition.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     An implementation of <see cref="IAccessCondition" /> for Windows Azure.
	/// </summary>
	public class AzureAccessCondition : IAccessCondition
	{
		private readonly AccessCondition _conditionReference;



		/// <summary>
		///     Initializes a new instance of the <see cref="AzureAccessCondition" /> class.
		/// </summary>
		/// <param name="condition">The original <see cref="AccessCondition" /> instance.</param>
		public AzureAccessCondition(AccessCondition condition)
		{
			Guard.NotNull(condition, "condition");

			this._conditionReference = condition;
		}



		/// <summary>
		///     Gets or sets an ETag value for a condition specifying that the given ETag must match the ETag of the specified resource.
		/// </summary>
		/// <value>
		///     <para>A string containing an ETag value, or <c>"*"</c></para>
		///     <para>to match any ETag. If <c>null</c></para>
		///     <para>, no condition exists.</para>
		/// </value>
		public string IfMatchETag
		{
			get
			{
				try
				{
					return this._conditionReference.IfMatchETag;
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
					this._conditionReference.IfMatchETag = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets or sets an ETag value for a condition specifying that the given ETag must not match the ETag of the specified resource.
		/// </summary>
		/// <value>
		///     <para>A string containing an ETag value, or <c>"*"</c></para>
		///     <para>to match any ETag. If <c>null</c></para>
		///     <para>, no condition exists.</para>
		/// </value>
		public string IfNoneMatchETag
		{
			get
			{
				try
				{
					return this._conditionReference.IfNoneMatchETag;
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
					this._conditionReference.IfNoneMatchETag = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     <para>Gets or sets a <see cref="DateTimeOffset" /></para>
		///     <para>value for a condition specifying a time since which a resource has been modified.</para>
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>value specified in UTC, or <c>null</c></para>
		///     <para>if no condition exists.</para>
		/// </value>
		public DateTimeOffset? IfModifiedSinceTime
		{
			get
			{
				try
				{
					return this._conditionReference.IfModifiedSinceTime;
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
					this._conditionReference.IfModifiedSinceTime = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     <para>Gets or sets a <see cref="DateTimeOffset" /></para>
		///     <para>value for a condition specifying a time since which a resource has not been modified.</para>
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>value specified in UTC, or <c>null</c></para>
		///     <para>if no condition exists.</para>
		/// </value>
		public DateTimeOffset? IfNotModifiedSinceTime
		{
			get
			{
				try
				{
					return this._conditionReference.IfNotModifiedSinceTime;
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
					this._conditionReference.IfNotModifiedSinceTime = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets or sets a value for a condition specifying that the current sequence number must be less than or equal to the specified value.
		/// </summary>
		/// <remarks>
		///     This condition only applies to page blobs.
		/// </remarks>
		/// <value>
		///     <para>A sequence number, or <c>null</c></para>
		///     <para>if no condition exists.</para>
		/// </value>
		public long? IfSequenceNumberLessThanOrEqual
		{
			get
			{
				try
				{
					return this._conditionReference.IfSequenceNumberLessThanOrEqual;
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
					this._conditionReference.IfSequenceNumberLessThanOrEqual = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets or sets a value for a condition specifying that the current sequence number must be less than the specified value.
		/// </summary>
		/// <remarks>
		///     This condition only applies to page blobs.
		/// </remarks>
		/// <value>
		///     <para>A sequence number, or <c>null</c></para>
		///     <para>if no condition exists.</para>
		/// </value>
		public long? IfSequenceNumberLessThan
		{
			get
			{
				try
				{
					return this._conditionReference.IfSequenceNumberLessThan;
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
					this._conditionReference.IfSequenceNumberLessThan = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets or sets a value for a condition specifying that the current sequence number must be equal to the specified value.
		/// </summary>
		/// <remarks>
		///     This condition only applies to page blobs.
		/// </remarks>
		/// <value>
		///     <para>A sequence number, or <c>null</c></para>
		///     <para>if no condition exists.</para>
		/// </value>
		public long? IfSequenceNumberEqual
		{
			get
			{
				try
				{
					return this._conditionReference.IfSequenceNumberEqual;
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
					this._conditionReference.IfSequenceNumberEqual = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets or sets a lease ID that must match the lease on a resource.
		/// </summary>
		/// <value>
		///     <para>A string containing a lease ID, or <c>null</c></para>
		///     <para>if no condition exists.</para>
		/// </value>
		public string LeaseId
		{
			get
			{
				try
				{
					return this._conditionReference.LeaseId;
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
					this._conditionReference.LeaseId = value;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="AzureAccessCondition" /> to <see cref="AccessCondition" /> .
		/// </summary>
		/// <param name="condition">The <see cref="AzureAccessCondition" /> source.</param>
		/// <returns>
		///     The underlying <see cref="AccessCondition" /> target.
		/// </returns>
		public static implicit operator AccessCondition(AzureAccessCondition condition) { return condition._conditionReference; }



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="AccessCondition" /> to <see cref="AzureAccessCondition" /> .
		/// </summary>
		/// <param name="condition">The <see cref="AccessCondition" /> source.</param>
		/// <returns>
		///     A <see cref="AzureAccessCondition" /> wrapper.
		/// </returns>
		public static implicit operator AzureAccessCondition(AccessCondition condition) { return new AzureAccessCondition(condition); }
	}
}