// <copyright file="AzureGlobalMutexFactory.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.GlobalMutexFramework;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.GlobalMutexFramework
{
	public class AzureGlobalMutexFactory : IGlobalMutexFactory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AzureGlobalMutexFactory"/> class.
		/// </summary>
		/// <param name="lockContainer">The BLOB that will contain the lease.</param>
		/// <param name="lockStateProvider">The lock state provider.</param>
		/// <param name="logService">The logging service to use.</param>
		public AzureGlobalMutexFactory(AzureBlobContainer lockContainer, AzureLockStateProvider lockStateProvider, ILogService logService)
		{
			this.LockStateProvider = lockStateProvider ?? new AzureLockStateProvider(logService);
			this.LogService = logService;
			this.LockContainer = lockContainer;
		}



		/// <summary>
		/// Gets or sets the lock state provider instance.
		/// </summary>
		/// <value>
		/// The Azure lock state provider.
		/// </value>
		private AzureLockStateProvider LockStateProvider { get; set; }


		/// <summary>
		/// Gets or sets the logging service of this factory instance.
		/// </summary>
		/// <value>
		/// The log service.
		/// </value>
		private ILogService LogService { get; set; }


		/// <summary>
		/// Gets or sets the BLOB that will contain the lock lease.
		/// </summary>
		/// <value>
		/// An <see cref="AzureBlobContainer"/> instance.
		/// </value>
		private AzureBlobContainer LockContainer { get; set; }



		/// <summary>
		///     <para>Creates an <see cref="AzureGlobalMutex" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     <para>A <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public IGlobalMutex TryCreateLock(string lockName, out bool success)
		{
			return new AzureGlobalMutex(new CancellationToken(), this.LockStateProvider, new AzureLockState {LockingBlobContainer = this.LockContainer}, this.LogService).TryLock(lockName, out success);
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     <para>A <see cref="AzureGlobalMutex" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public IGlobalMutex TryCreateLock(string lockName, TimeSpan? leaseTime, out bool success)
		{
			return new AzureGlobalMutex(new CancellationToken(), this.LockStateProvider, new AzureLockState {LockingBlobContainer = this.LockContainer}, this.LogService).TryLock(
				lockName,
				leaseTime,
				out success);
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">A cancellation token.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     <para>A <see cref="AzureGlobalMutex" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public IGlobalMutex TryCreateLock(string lockName, CancellationToken cancelToken, out bool success)
		{
			return new AzureGlobalMutex(cancelToken, this.LockStateProvider, new AzureLockState { LockingBlobContainer = this.LockContainer }, this.LogService).TryLock(lockName, out success);
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it with the specified name and will return instantly.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <returns>
		///     <para>A <see cref="AzureGlobalMutex" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public IGlobalMutex TryCreateLock(string lockName, TimeSpan? leaseTime, CancellationToken cancelToken, out bool success)
		{
			return new AzureGlobalMutex(cancelToken, this.LockStateProvider, new AzureLockState {LockingBlobContainer = this.LockContainer}, this.LogService).TryLock(lockName, leaseTime, out success);
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <returns>
		///     <para>A <see cref="AzureGlobalMutex" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public IGlobalMutex CreateLock(string lockName)
		{
			return new AzureGlobalMutex(new CancellationToken(), this.LockStateProvider, new AzureLockState { LockingBlobContainer = this.LockContainer }, this.LogService).LockAsync(lockName).Result;
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <returns>
		///     <para>A <see cref="AzureGlobalMutex" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public IGlobalMutex CreateLock(string lockName, TimeSpan? leaseTime)
		{
			return new AzureGlobalMutex(new CancellationToken(), this.LockStateProvider, new AzureLockState {LockingBlobContainer = this.LockContainer}, this.LogService).LockAsync(lockName, leaseTime).Result;
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>
		///     <para>A <see cref="AzureGlobalMutex" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public IGlobalMutex CreateLock(string lockName, CancellationToken cancelToken)
		{
			return new AzureGlobalMutex(cancelToken, this.LockStateProvider, new AzureLockState { LockingBlobContainer = this.LockContainer }, this.LogService).LockAsync(lockName).Result;
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>
		///     <para>A <see cref="AzureGlobalMutex" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public IGlobalMutex CreateLock(string lockName, TimeSpan? leaseTime, CancellationToken cancelToken)
		{
			return new AzureGlobalMutex(cancelToken, this.LockStateProvider, new AzureLockState {LockingBlobContainer = this.LockContainer}, this.LogService).LockAsync(lockName, leaseTime).Result;
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <returns>
		///     <para>A <see cref="Task{AzureGlobalMutex}" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public Task<IGlobalMutex> CreateLockAsync(string lockName)
		{
			return new AzureGlobalMutex(new CancellationToken(), this.LockStateProvider, new AzureLockState { LockingBlobContainer = this.LockContainer }, this.LogService).LockAsync(lockName);
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <returns>
		///     <para>A <see cref="Task{AzureGlobalMutex}" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public Task<IGlobalMutex> CreateLockAsync(string lockName, TimeSpan? leaseTime)
		{
			return new AzureGlobalMutex(new CancellationToken(), this.LockStateProvider, new AzureLockState {LockingBlobContainer = this.LockContainer}, this.LogService).LockAsync(lockName, leaseTime);
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>
		///     <para>A <see cref="Task{AzureGlobalMutex}" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public Task<IGlobalMutex> CreateLockAsync(string lockName, CancellationToken cancelToken)
		{
			return new AzureGlobalMutex(cancelToken, this.LockStateProvider, new AzureLockState { LockingBlobContainer = this.LockContainer }, this.LogService).LockAsync(lockName);
		}



		/// <summary>
		///     <para>Creates a <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>instance and attempts to create a lock on it. The method will not return before a lock can be retrieved.</para>
		/// </summary>
		/// <remarks>
		///     <para>Warning! Unless you are aware of the implications an infinite lock has, you should probably avoid providing a <see langword="null" /> value in <paramref name="leaseTime" /></para>
		///     <para>.</para>
		/// </remarks>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>
		///     <para>A <see cref="Task{AzureGlobalMutex}" /></para>
		///     <para>instance with an activated lock (if successful).</para>
		/// </returns>
		public Task<IGlobalMutex> CreateLockAsync(string lockName, TimeSpan? leaseTime, CancellationToken cancelToken)
		{
			return new AzureGlobalMutex(cancelToken, this.LockStateProvider, new AzureLockState {LockingBlobContainer = this.LockContainer}, this.LogService).LockAsync(lockName, leaseTime);
		}
	}
}