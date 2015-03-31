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

using TheQ.Utilities.CloudTools.Storage.GlobalLockFramework;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.GlobalLockFramework
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


		public IBlob LockingBlob { get; set; }


		public string LockName { get; set; }


		public string LeaseId { get; set; }
	}
}