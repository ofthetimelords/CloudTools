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



namespace TheQ.Utilities.CloudTools.Storage.GlobalLockFramework
{
	public interface ILockState
	{
		string LockName { get; set; }


		string LeaseId { get; set; }


		TimeSpan? LeaseTime { get; set; }
	}
}