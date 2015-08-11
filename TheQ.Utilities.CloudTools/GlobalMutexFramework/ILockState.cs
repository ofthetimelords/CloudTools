// <copyright file="ILockState.cs" company="nett">
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



namespace TheQ.Utilities.CloudTools.Storage.GlobalMutexFramework
{
	/// <summary>
	/// Represents the state of a lock for the Global Locking Framework.
	/// </summary>
	public interface ILockState
	{
		/// <summary>
		/// Gets or sets the name of the lock.
		/// </summary>
		/// <value>
		/// A string value representing the name of the lock.
		/// </value>
		string LockName { get; set; }


		/// <summary>
		/// Gets or sets the lease identifier used to validate ownership of a lock.
		/// </summary>
		/// <value>
		/// A string value representing the lease identifier.
		/// </value>
		string LeaseId { get; set; }
	}
}