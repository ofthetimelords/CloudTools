// <copyright file="IGlobalLockFactory.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Storage.GlobalLockFramework
{
	/// <summary>
	///     Represents a Global Lock Factory which can be inherited from to provide implementations for other cloud platforms.
	/// </summary>
	public interface IGlobalLockFactory
	{
		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock TryCreateLock(
			[NotNull] string lockName,
			out bool success);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock TryCreateLock(
			[NotNull] string lockName,
			TimeSpan? leaseTime,
			out bool success);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">A cancellation token.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock TryCreateLock(
			[NotNull] string lockName,
			CancellationToken cancelToken,
			out bool success);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock TryCreateLock(
			[NotNull] string lockName,
			TimeSpan? leaseTime,
			CancellationToken cancelToken,
			out bool success);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock CreateLock([NotNull] string lockName);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock CreateLock([NotNull] string lockName, TimeSpan? leaseTime);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock CreateLock([NotNull] string lockName, CancellationToken cancelToken);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock CreateLock([NotNull] string lockName, TimeSpan? leaseTime, CancellationToken cancelToken);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		Task<IGlobalLock> CreateLockAsync([NotNull] string lockName);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		Task<IGlobalLock> CreateLockAsync([NotNull] string lockName, TimeSpan? leaseTime);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		Task<IGlobalLock> CreateLockAsync([NotNull] string lockName, CancellationToken cancelToken);



		/// <summary>
		///     <para>Creates a <see cref="Blob.GlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="loggingService">An optional logging service.</param>
		/// <returns>
		///     <para>A <see cref="Blob.GlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		Task<IGlobalLock> CreateLockAsync([NotNull] string lockName, TimeSpan? leaseTime, CancellationToken cancelToken);
	}
}