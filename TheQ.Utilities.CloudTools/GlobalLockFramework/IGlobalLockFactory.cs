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
	///     Represents a Global Lock Factory which can be inherited from to provide implementations for different platforms.
	/// </summary>
	public interface IGlobalLockFactory
	{
		/// <summary>
		///     <para>Creates an <see cref="IGlobalLockFactory" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock TryCreateLock(
			[NotNull] string lockName,
			out bool success);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock TryCreateLock(
			[NotNull] string lockName,
			TimeSpan? leaseTime,
			out bool success);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">A cancellation token.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock TryCreateLock(
			[NotNull] string lockName,
			CancellationToken cancelToken,
			out bool success);



		/// <summary>
		///     <para>Creates a <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock TryCreateLock(
			[NotNull] string lockName,
			TimeSpan? leaseTime,
			CancellationToken cancelToken,
			out bool success);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock CreateLock([NotNull] string lockName);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock CreateLock([NotNull] string lockName, TimeSpan? leaseTime);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock CreateLock([NotNull] string lockName, CancellationToken cancelToken);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		IGlobalLock CreateLock([NotNull] string lockName, TimeSpan? leaseTime, CancellationToken cancelToken);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		Task<IGlobalLock> CreateLockAsync([NotNull] string lockName);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		Task<IGlobalLock> CreateLockAsync([NotNull] string lockName, TimeSpan? leaseTime);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		Task<IGlobalLock> CreateLockAsync([NotNull] string lockName, CancellationToken cancelToken);



		/// <summary>
		///     <para>Creates an <see cref="IGlobalLock" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>
		///     <para>An <see cref="IGlobalLock" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		Task<IGlobalLock> CreateLockAsync([NotNull] string lockName, TimeSpan? leaseTime, CancellationToken cancelToken);
	}
}