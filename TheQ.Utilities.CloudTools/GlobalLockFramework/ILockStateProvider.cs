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
	/// <summary>
	/// This class is responsible for performing the actual locking operations on a supported backing store for the Global Locking framework.
	/// </summary>
	/// <typeparam name="T">The type of the object that represents a lock's state.</typeparam>
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


		/// <summary>
		/// Breaks the lock (forces a lock to unlock) asynchronously.
		/// </summary>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		Task<T> BreakLockAsync(T state, CancellationToken cancelToken);


		/// <summary>
		/// Unregisters a lock asynchronously.
		/// </summary>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		Task<T> UnregisterLockAsync(T state, CancellationToken cancelToken);


		/// <summary>
		/// Registers a lock asynchronously.
		/// </summary>
		/// <param name="newLockName">The (new) name of the lock. This will override the <see cref="ILockState.LockName"/> currently assigned when the operation is complete.</param>
		/// <param name="leaseTime">The amount of time to lease the lock.</param>
		/// <param name="isDefaultLeaseTime">if set to <c>true</c> then implementations should use a default lease time for locks (in cases where null represents something else).</param>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		Task<T> RegisterLockAsync(T state, string newLockName, TimeSpan? leaseTime, bool isDefaultLeaseTime, CancellationToken cancelToken);


		/// <summary>
		/// Renews an existing lock asynchronously.
		/// </summary>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		Task<T> RenewLockAsync(T state, CancellationToken cancelToken);


		/// <summary>
		/// Determines whether the underlying backing store supports the provided lease time.
		/// </summary>
		/// <param name="leaseTime">The lease time to validate.</param>
		/// <returns><c>True</c> if the lease time is supported by the underlyinmg implementation; <c>false</c> if otherwise</returns>
		bool IsValidLeaseTime(TimeSpan? leaseTime);
	}
}