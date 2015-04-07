// <copyright file="AzureLockState.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.GlobalMutexFramework;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.GlobalMutexFramework
{
	/// <summary>
	///     An <see cref="ILockState" /> implementation for Windows Azure locks using BLOBs.
	/// </summary>
	public class AzureLockState : ILockState
	{
		/// <summary>
		///     Gets or sets the BLOB container that offers the lockign capabilities.
		/// </summary>
		/// <value>
		///     An <see cref="IBlobContainer" /> instance.
		/// </value>
		public AzureBlobContainer LockingBlobContainer { get; set; }


		/// <summary>
		/// Gets or sets the BLOB that will hold the lock.
		/// </summary>
		/// <value>
		/// An <see cref="IBlob"/> instance.
		/// </value>
		public IBlob LockingBlob { get; set; }


		/// <summary>
		/// Gets or sets the name of the lock.
		/// </summary>
		/// <value>
		/// A string value representing the name of the lock.
		/// </value>
		public string LockName { get; set; }


		/// <summary>
		/// Gets or sets the lease identifier used to validate ownership of a lock.
		/// </summary>
		/// <value>
		/// A string value representing the lease identifier.
		/// </value>
		public string LeaseId { get; set; }
	}
}