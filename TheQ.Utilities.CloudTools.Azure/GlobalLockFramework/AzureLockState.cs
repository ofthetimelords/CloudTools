// <copyright file="AzureLockState.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.GlobalLockFramework;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.GlobalLockFramework
{
	public class AzureLockState : ILockState
	{
		public IBlobContainer LockingBlobContainer { get; set; }


		public IBlob LockingBlob { get; set; }


		public string LockName { get; set; }


		public string LeaseId { get; set; }


		public TimeSpan? LeaseTime { get; set; }
	}
}