// <copyright file="ILockStateProvider.cs" company="nett">
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
using System.Threading;
using System.Threading.Tasks;



namespace TheQ.Utilities.CloudTools.Storage.GlobalLockFramework
{
	public interface ILockStateProvider<T> : IDisposable where T : class, ILockState
	{
		/// <summary>
		///     Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		///     <para>
		///         <c>true</c>
		///     </para>
		///     <para>if this instance is disposed; otherwise, <c>false</c></para>
		///     <para>.</para>
		/// </value>
		bool IsDisposed { get; }


		Task<T> BreakLockAsync(T state, CancellationToken cancelToken);


		Task<T> UnregisterLockAsync(T state, CancellationToken cancelToken);


		Task<T> RegisterLockAsync(T state, string newLockName, TimeSpan? leaseTime, bool isDefaultLeaseTime, CancellationToken cancelToken);


		Task<T> RenewLockAsync(T state, CancellationToken cancelToken);


		bool IsValidLeaseTime(TimeSpan? leaseTime);
	}
}