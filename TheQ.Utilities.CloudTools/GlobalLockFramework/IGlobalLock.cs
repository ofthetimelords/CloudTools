// <copyright file="IGlobalLock.cs" company="nett">
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
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Storage.GlobalLockFramework
{
	/// <summary>
	/// Represents a Global Lock, a MutEx that can be used to synchronise operations between different machines and/or processes.
	/// </summary>
	public interface IGlobalLock : IDisposable
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
		/// Gets the name of the current lock.
		/// </summary>
		/// <value>
		/// A string value with the name of the current lock.
		/// </value>
		string CurrentLockName { get; }


		/// <summary>
		/// Gets the current lease identifier.
		/// </summary>
		/// <value>
		/// A string value with the current lease identifier.
		/// </value>
		string CurrentLeaseId { get; }



		/// <summary>
		///     Attempts to release a global lock asynchronously
		/// </summary>
		Task UnlockAsync();



		/// <summary>
		///     Attempts to release a global lock.
		/// </summary>
		void Unlock();



		/// <summary>
		///     Force-unlocks a lock even if it was acquired by another instance, asynchronously.
		/// </summary>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		Task<IGlobalLock> ForceUnlockAsync();



		/// <summary>
		///     Force-unlocks a lock even if it was acquired by another instance.
		/// </summary>
		/// <remarks>
		///     Note: Implementations SHOULD NOT <see langword="throw" /> on error when using this overload.
		/// </remarks>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		IGlobalLock ForceUnlock();



		/// <summary>
		///     Force-unlocks a lock even if it was acquired by another instance.
		/// </summary>
		/// <param name="throwOnError">
		///     <para>if set to <c>true</c></para>
		///     <para>and an exception occurs it will be thrown back to the caller.</para>
		/// </param>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		IGlobalLock ForceUnlock(bool throwOnError);



		/// <summary>
		///     Force-unlocks a lock even if it was acquired by another instance, asynchronously.
		/// </summary>
		/// <param name="throwOnError">
		///     <para>if set to <c>true</c></para>
		///     <para>and an exception occurs it will be thrown back to the caller.</para>
		/// </param>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		Task<IGlobalLock> ForceUnlockAsync(bool throwOnError);



		/// <summary>
		///     Attempts to create a lock with the specified name and returns instantly.
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		IGlobalLock TryLock([NotNull] string lockName, out bool success);



		/// <summary>
		///     Attempts to create a lock with the specified name and returns instantly.
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <exception cref="ArgumentException">
		///     Parameter <paramref name="lockName" /> is <see langword="null" /> or empty;lockName or Parameter <paramref name="leaseTime" /> must be either <see langword="null" /> or between 15 and 60
		///     seconds;leaseTime
		/// </exception>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		IGlobalLock TryLock([NotNull] string lockName, TimeSpan? leaseTime, out bool success);



		/// <summary>
		///     Attempts to create a lock with the specified name and will not return before a lock can be retrieved.
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <exception cref="ArgumentException">Parameter <paramref name="lockName" /> is <see langword="null" /> or empty;lockName</exception>
		/// <exception cref="OperationCanceledException">A wait for a global lock retrieval was cancelled</exception>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		Task<IGlobalLock> LockAsync([NotNull] string lockName);




		/// <summary>
		///     Attempts to create a lock with the specified name and will not return before a lock can be retrieved.
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <exception cref="OperationCanceledException">A wait for a global lock retrieval was cancelled</exception>
		/// <exception cref="ArgumentException">
		///     Parameter <paramref name="lockName" /> is <see langword="null" /> or empty;lockName or Parameter <paramref name="leaseTime" /> must be either <see langword="null" /> or between 15 and 60
		///     seconds;leaseTime
		/// </exception>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		Task<IGlobalLock> LockAsync([NotNull] string lockName, TimeSpan? leaseTime);



		/// <summary>
		///     Attempts to create a lock with the specified name and will not return before a lock can be retrieved.
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="timeBetweenAttempts">The time between lock retrieval attempts.</param>
		/// <exception cref="OperationCanceledException">A wait for a global lock retrieval was cancelled</exception>
		/// <exception cref="ArgumentException">
		///     Parameter <paramref name="lockName" /> is <see langword="null" /> or empty;lockName or Parameter <paramref name="leaseTime" /> must be either <see langword="null" /> or between 15 and 60
		///     seconds;leaseTime
		/// </exception>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		Task<IGlobalLock> LockAsync([NotNull] string lockName, TimeSpan? leaseTime, TimeSpan timeBetweenAttempts);
	}
}