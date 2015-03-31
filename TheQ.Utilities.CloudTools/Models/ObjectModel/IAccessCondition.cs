//// <copyright file="IAccessCondition.cs" company="nett">
////      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
////      Please see the License.txt file for more information. All other rights reserved.
//// </copyright>
//// <author>James Kavakopoulos</author>
//// <email>ofthetimelords@gmail.com</email>
//// <date>2015/02/06</date>
//// <summary>
//// 
//// </summary>

//using System;
//using System.Linq;



//namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
//{
//	/// <summary>
//	///     Defines an access condition when working with cloud objects that can be used to filter results.
//	/// </summary>
//	/// <remarks>
//	///     Currently only used for BLOB leasing.
//	/// </remarks>
//	public interface IAccessCondition
//	{
//		/// <summary>
//		///     Gets or sets an ETag value for a condition specifying that the given ETag must match the ETag of the specified resource.
//		/// </summary>
//		/// <value>
//		///     <para>A string containing an ETag value, or <c>"*"</c></para>
//		///     <para>to match any ETag. If <c>null</c></para>
//		///     <para>, no condition exists.</para>
//		/// </value>
//		string IfMatchETag { get; set; }


//		/// <summary>
//		///     Gets or sets an ETag value for a condition specifying that the given ETag must not match the ETag of the specified resource.
//		/// </summary>
//		/// <value>
//		///     <para>A string containing an ETag value, or <c>"*"</c></para>
//		///     <para>to match any ETag. If <c>null</c></para>
//		///     <para>, no condition exists.</para>
//		/// </value>
//		string IfNoneMatchETag { get; set; }


//		/// <summary>
//		///     <para>Gets or sets a <see cref="DateTimeOffset" /></para>
//		///     <para>value for a condition specifying a time since which a resource has been modified.</para>
//		/// </summary>
//		/// <value>
//		///     <para>A <see cref="DateTimeOffset" /></para>
//		///     <para>value specified in UTC, or <c>null</c></para>
//		///     <para>if no condition exists.</para>
//		/// </value>
//		DateTimeOffset? IfModifiedSinceTime { get; set; }


//		/// <summary>
//		///     <para>Gets or sets a <see cref="DateTimeOffset" /></para>
//		///     <para>value for a condition specifying a time since which a resource has not been modified.</para>
//		/// </summary>
//		/// <value>
//		///     <para>A <see cref="DateTimeOffset" /></para>
//		///     <para>value specified in UTC, or <c>null</c></para>
//		///     <para>if no condition exists.</para>
//		/// </value>
//		DateTimeOffset? IfNotModifiedSinceTime { get; set; }


//		/// <summary>
//		///     Gets or sets a value for a condition specifying that the current sequence number must be less than or equal to the specified value.
//		/// </summary>
//		/// <remarks>
//		///     This condition only applies to page blobs.
//		/// </remarks>
//		/// <value>
//		///     <para>A sequence number, or <c>null</c></para>
//		///     <para>if no condition exists.</para>
//		/// </value>
//		long? IfSequenceNumberLessThanOrEqual { get; set; }


//		/// <summary>
//		///     Gets or sets a value for a condition specifying that the current sequence number must be less than the specified value.
//		/// </summary>
//		/// <remarks>
//		///     This condition only applies to page blobs.
//		/// </remarks>
//		/// <value>
//		///     <para>A sequence number, or <c>null</c></para>
//		///     <para>if no condition exists.</para>
//		/// </value>
//		long? IfSequenceNumberLessThan { get; set; }


//		/// <summary>
//		///     Gets or sets a value for a condition specifying that the current sequence number must be equal to the specified value.
//		/// </summary>
//		/// <remarks>
//		///     This condition only applies to page blobs.
//		/// </remarks>
//		/// <value>
//		///     <para>A sequence number, or <c>null</c></para>
//		///     <para>if no condition exists.</para>
//		/// </value>
//		long? IfSequenceNumberEqual { get; set; }


//		/// <summary>
//		///     Gets or sets a lease ID that must match the lease on a resource.
//		/// </summary>
//		/// <value>
//		///     <para>A string containing a lease ID, or <c>null</c></para>
//		///     <para>if no condition exists.</para>
//		/// </value>
//		string LeaseId { get; set; }
//	}
//}