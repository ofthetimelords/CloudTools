// <copyright file="GlobalLockBase.cs" company="nett">



#region Using directives
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
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;
#endregion



namespace TheQ.Utilities.CloudTools.Storage.GlobalLockFramework
{
	/// <summary>
	///     Defines a base implementation of a MutEx framework that can span between different processes or even host machines.
	/// </summary>
	public abstract class GlobalLockBase<TLockState> : IGlobalLock where TLockState : class, ILockState
	{
		/// <summary>
		///     The default time between locking attempts, set to 3 seconds.
		/// </summary>
		private static TimeSpan _defaultTimeBetweenLockAttemptsValue = TimeSpan.FromSeconds(3);



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <exception cref="ArgumentNullException">container;Parameter <paramref name="container" /> was <see langword="null" /></exception>
		public GlobalLockBase(ILockStateProvider<TLockState> lockStateProvider, TLockState initialState)
			: this(new CancellationToken(), lockStateProvider, initialState, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="cancelToken">The cancellation token.</param>
		/// <exception cref="ArgumentNullException">container;Parameter <paramref name="container" /> was <see langword="null" /></exception>
		public GlobalLockBase(CancellationToken cancelToken, ILockStateProvider<TLockState> lockStateProvider, TLockState initialState)
			: this(cancelToken, lockStateProvider, initialState, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <exception cref="ArgumentNullException">container;Parameter <paramref name="container" /> was <see langword="null" /></exception>
		public GlobalLockBase(ILockStateProvider<TLockState> lockStateProvider, TLockState initialState, [CanBeNull] ILogService logService)
			: this(new CancellationToken(), lockStateProvider, initialState, logService)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="cancelToken">The cancellation token.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <exception cref="ArgumentNullException">container;Parameter <paramref name="container" /> was <see langword="null" /></exception>
		public GlobalLockBase(CancellationToken cancelToken, ILockStateProvider<TLockState> lockStateProvider, TLockState initialState, [CanBeNull] ILogService logService)
		{
			Guard.NotNull(lockStateProvider, "lockStateProvider");

			this.LockState = initialState;
			this.LockStateProvider = lockStateProvider;
			this.CancelToken = cancelToken;
			this.LogService = logService;
		}



		/// <summary>
		///     Gets or sets the default time between lock attempts.
		/// </summary>
		/// <value>
		///     The default time between locking attempts, at least 1 second.
		/// </value>
		public static TimeSpan DefaultTimeBetweenLockAttempts
		{
			get { return _defaultTimeBetweenLockAttemptsValue; }
			set { if (_defaultTimeBetweenLockAttemptsValue.TotalSeconds >= 1) _defaultTimeBetweenLockAttemptsValue = value; }
		}


		/// <summary>
		///     Gets or sets the logging service that will be used.
		/// </summary>
		[CanBeNull]
		private ILogService LogService { get; set; }


		/// <summary>
		///     Gets or sets the logging service that will be used.
		/// </summary>
		[NotNull]
		private ILockStateProvider<TLockState> LockStateProvider { get; set; }


		/// <summary>
		///     Gets or sets the logging service that will be used.
		/// </summary>
		[NotNull]
		private TLockState LockState { get; set; }


		//private TLockContainer Container { get; set; }
		/// <summary>
		///     Gets or sets the thread responsible for renewing a lock.
		/// </summary>
		[CanBeNull]
		private Task RenewalThread { get; set; }


		/// <summary>
		///     Used for gracefully aborting the renewal thread.
		/// </summary>
		[NotNull]
		private CancellationTokenSource RenewalThreadCancelToken { get; set; }


		/// <summary>
		///     Gets or sets an optional cancellation token.
		/// </summary>
		private CancellationToken CancelToken { get; set; }


		/// <summary>
		///     Gets the name of the lock, or <c>null</c> if no lease is held.
		/// </summary>
		[CanBeNull]
		public string LockName
		{
			get { return this.LockState.LockName; }
		}


		public string CurrentLockName
		{
			get { return this.LockState.LockName; }
		}


		public string CurrentLeaseId
		{
			get { return this.LockState.LeaseId; }
		}


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
		public bool IsDisposed { get; private set; }



		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true, true);
			GC.SuppressFinalize(this);
		}



		/// <summary>
		///     Attempts to release a global lock.
		/// </summary>
		public async Task UnlockAsync()
		{
			await this.UnlockAsync(true);
		}



		/// <summary>
		///     Attempts to release a global lock.
		/// </summary>
		public void Unlock()
		{
			Task.Run(() => this.UnlockAsync(true), this.CancelToken).Wait(this.CancelToken);
		}



		/// <summary>
		///     Force-unlock a lock even if it was acquired by another thread.
		/// </summary>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		public async Task<IGlobalLock> ForceUnlockAsync()
		{
			return await this.ForceUnlockAsync(false);
		}



		/// <summary>
		///     Force-unlock a lock even if it was acquired by another thread.
		/// </summary>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		public IGlobalLock ForceUnlock()
		{
			return this.ForceUnlock(false);
		}



		/// <summary>
		///     Force-unlock a lock even if it was acquired by another thread.
		/// </summary>
		/// <param name="throwOnError">
		///     <para>if set to <c>true</c></para>
		///     <para>and an exception occurs it will be thrown back to the caller.</para>
		/// </param>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		public IGlobalLock ForceUnlock(bool throwOnError)
		{
			return this.ForceUnlockAsync(throwOnError).Result;
		}



		/// <summary>
		///     Force-unlock a lock even if it was acquired by another thread.
		/// </summary>
		/// <param name="throwOnError">
		///     <para>if set to <c>true</c></para>
		///     <para>and an exception occurs it will be thrown back to the caller.</para>
		/// </param>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		public async Task<IGlobalLock> ForceUnlockAsync(bool throwOnError)
		{
			try
			{
				if (this.LockState.LeaseId != null) await this.UnlockAsync();
				else
				{
					this.LockState = await this.LockStateProvider.BreakLockAsync(this.LockState, this.CancelToken);
					this.LogService.QuickLogDebug("GlobalLock", "Global lock with name '{0}' was force-released", this.LockName);
				}
			}
			catch (CloudToolsStorageException ex)
			{
				if (ex.HttpStatusCode != 404 && ex.HttpStatusCode != 409)
				{
					this.LogService.QuickLogError("GlobalLock", ex, "Attempting to force-release global lock with name '{0}' failed due to an unexpected exception", this.LockName);
					if (throwOnError) throw;
				}
				else this.LogService.QuickLogDebug("GlobalLock", "Attempting to force a global lock with name '{0}' failed; the lock must have been released already", this.LockName);
			}
			catch (Exception ex)
			{
				this.LogService.QuickLogError("GlobalLock", ex, "Attempting to force-release global lock with name '{0}' failed due to an unexpected exception", this.LockName);
				if (throwOnError) throw;
			}

			return this;
		}



		/// <summary>
		///     Attempts to create a lock with the specified name and returns instantly.
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="success">The value is set when the operation completes, indicating if the lock was successfully acquired or not.</param>
		/// <exception cref="ArgumentException">lockName;Parameter <paramref name="lockName" /> is null;</exception>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		public IGlobalLock TryLock(string lockName, out bool success)
		{
			success = Task.Run(() => this.TryLockInternal(lockName, null, true), this.CancelToken).Result;
			return this;
		}



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
		public IGlobalLock TryLock(string lockName, TimeSpan? leaseTime, out bool success)
		{
			success = Task.Run(() => this.TryLockInternal(lockName, leaseTime, false), this.CancelToken).Result;
			return this;
		}



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
		public Task<IGlobalLock> LockAsync(string lockName)
		{
			Guard.NotNull(lockName, "lockName");

			return this.LockInternal(lockName, null, true, DefaultTimeBetweenLockAttempts);
		}




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
		public Task<IGlobalLock> LockAsync(string lockName, TimeSpan? leaseTime)
		{
			Guard.NotNull(lockName, "lockName");

			return this.LockInternal(lockName, leaseTime, false, DefaultTimeBetweenLockAttempts);
		}



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
		public Task<IGlobalLock> LockAsync(string lockName, TimeSpan? leaseTime, TimeSpan timeBetweenAttempts)
		{
			Guard.NotNull(lockName, "lockName");

			return this.LockInternal(lockName, leaseTime, false, timeBetweenAttempts);
		}



		private async Task<IGlobalLock> LockInternal(string lockName, TimeSpan? leaseTime, bool isDefaultLeaseTime, TimeSpan timeBetweenAttempts)
		{
			Guard.NotNull(lockName, "lockName");

			var success = await this.TryLockInternal(lockName, leaseTime, isDefaultLeaseTime);

			while (!success)
			{
				if (this.CancelToken.IsCancellationRequested) return this;

				await Task.Delay((int) timeBetweenAttempts.TotalSeconds, this.CancelToken);
				success = await this.TryLockInternal(lockName, leaseTime, isDefaultLeaseTime);
			}

			return this;
		}



		/// <summary>
		///     Handles an attempt to create a lock on a blob.
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock. The value must be between 15 and 60 seconds, or <see langword="null" /> (in which case an infinite lock is created).</param>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		private async Task<bool> TryLockInternal([NotNull] string lockName, TimeSpan? leaseTime, bool isDefaultLeaseTime)
		{
			Guard.NotNull(lockName, "lockName");
			GuardLeaseTime(leaseTime);
			Guard.NotNull(lockName, "lockName");
			GuardLeaseTime(leaseTime);

			try
			{
				if (this.LockState.LeaseId != null && this.LockName != null && lockName == this.LockName) return true;

				if (this.CancelToken.IsCancellationRequested)
					return false;

				await this.UnlockAsync();

				// Allow exceptions to propagate from here.
				await this.TryAcquireLeaseAsync(lockName, leaseTime, isDefaultLeaseTime);

				if (this.LockState.LeaseId != null) this.CreateRenewalThreadIfApplicable(lockName, leaseTime);
				else return false;

				return true;
			}
			catch (OperationCanceledException)
			{
				return false;
			}
		}



		/// <summary>
		///     If a non-infinite lock has been specified, it will attempt to create a thread responsible for renewing the lock.
		/// </summary>
		/// <param name="lockName">The name of the lock.</param>
		/// <param name="leaseTime">The lease time.</param>
		private void CreateRenewalThreadIfApplicable([NotNull] string lockName, TimeSpan? leaseTime)
		{
			Guard.NotNull(lockName, "lockName");

			if (!leaseTime.HasValue) return;

			this.RenewalThreadCancelToken = new CancellationTokenSource();
			var cancelToken = CancellationTokenSource.CreateLinkedTokenSource(this.CancelToken, this.RenewalThreadCancelToken.Token);
			this.RenewalThread = this.RenewalThreadCoreAsync(lockName, leaseTime.Value, cancelToken.Token);
		}



		/// <summary>
		///     Performs the renewal of a lock.
		/// </summary>
		/// <param name="lockName">The name of the lock.</param>
		/// <param name="leaseTime">The lease time.</param>
		/// <param name="cancelToken">The cancellation token for the thread.</param>
		/// <returns>
		/// </returns>
		[NotNull]
		private async Task RenewalThreadCoreAsync([NotNull] string lockName, TimeSpan leaseTime, CancellationToken cancelToken)
		{
			Guard.NotNull(lockName, "lockName");

			while (true)
			{
				if (cancelToken.IsCancellationRequested) return;

				await Task.Delay(TimeSpan.FromSeconds((leaseTime.TotalSeconds*3)/4), cancelToken);

				try
				{
					if (this.LockName != null) this.LockState = await this.LockStateProvider.RenewLockAsync(this.LockState, cancelToken);
				}
				catch (Exception ex)
				{
					if (!(ex is OperationCanceledException))
					{
						this.LogService.QuickLogError("GlobalLock", ex, "Unexpected exception while attempting to renew a global lock (using BLOB leases) with name '{0}' and ID '{1}'", lockName, this.LockState.LeaseId);
						throw;
					}
				}
			}
		}



		/// <summary>
		///     Attempts to acquire a lease on a BLOB.
		/// </summary>
		/// <param name="lockName">Name of the blob to acquire the lock upon.</param>
		/// <param name="leaseTime">The lease time.</param>
		private async Task TryAcquireLeaseAsync([NotNull] string lockName, TimeSpan? leaseTime, bool isDefaultLeaseTime)
		{
			Guard.NotNull(lockName, "lockName");
			GuardLeaseTime(leaseTime);

			try
			{
				this.LockState = await this.LockStateProvider.RegisterLockAsync(this.LockState, lockName, leaseTime, isDefaultLeaseTime, this.CancelToken);
			}
			catch (CloudToolsStorageException ex)
			{
				// 409 a lease is already present
				if (ex.ErrorCode != "LeaseAlreadyPresent" && ex.HttpStatusCode != (int) HttpStatusCode.Conflict)
				{
					this.LogService.QuickLogError("GlobalLock", ex, "Unexpected exception while attempting to create a global lock (using BLOB leases) with name '{0}'", lockName);
					throw;
				}

				this.LogService.QuickLogDebug("GlobalLock", "Attempting to create a global lock with name '{0}' failed; a lock has been placed already", lockName);
			}
			catch (Exception ex)
			{
				if (!(ex is OperationCanceledException))
				{
					this.LogService.QuickLogError("GlobalLock", ex, "Unexpected exception while attempting to create a global lock (using BLOB leases) with name '{0}'", lockName);
					throw;
				}
			}
		}



		/// <summary>
		///     Attempts to release a global lock.
		/// </summary>
		/// <param name="attemptLog">if set to <c>true</c> it will attempt to log the operation (used when disposing).</param>
		/// <returns>
		///     An asynchronous task.
		/// </returns>
		private async Task UnlockAsync(bool attemptLog)
		{
			if (this.RenewalThread != null)
			{
				this.RenewalThread = null;
				this.RenewalThreadCancelToken.Cancel();
			}

			if (this.LockState.LeaseId != null && this.LockState.LockName != null)
			{
				await this.LockStateProvider.UnregisterLockAsync(this.LockState, this.CancelToken);

				if (attemptLog) this.LogService.QuickLogDebug("GlobalLock", "Global lock with name '{0}' was released", this.LockName);
			}

			this.LockState.LeaseId = null;
			this.LockState.LockName = null;
		}



		/// <summary>
		///     Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing">
		///     <para>
		///         <c>true</c>
		///     </para>
		///     <para>to release both managed and unmanaged resources; <c>false</c></para>
		///     <para>to release only unmanaged resources.</para>
		/// </param>
		/// <param name="attemptLog">
		///     <para>if set to <c>false</c></para>
		///     <para>it won't attempt to log failures (used for finalisers.</para>
		/// </param>
		protected virtual void Dispose(bool disposing, bool attemptLog)
		{
			if (!this.IsDisposed)
			{
				try
				{
					if (disposing) this.ForceUnlockAsync().Wait(TimeSpan.FromMinutes(1));
				}
				catch (Exception ex)
				{
					if (!(ex is OperationCanceledException))
					{
						try
						{
							if (attemptLog) this.LogService.QuickLogError("GlobalLock", ex, "An error occurred while attempting to dispose a global lock during unlocking, with name '{0}'", this.LockName);
						}
						catch
						{
							// Attempt to ignore exceptions at this level
						}
					}
				}

				try
				{
					if (disposing) this.LockStateProvider.Dispose();
				}
				catch (Exception ex)
				{
					if (!(ex is OperationCanceledException))
					{
						try
						{
							if (attemptLog) this.LogService.QuickLogError("GlobalLock", ex, "An error occurred while attempting to dispose the underlying lock provider during unlocking, with name '{0}'", this.LockName);
						}
						catch
						{
							// Attempt to ignore exceptions at this level
						}
					}
				}

				this.IsDisposed = true;
			}
		}



		/// <summary>
		///     <para>Finalizes an instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		~GlobalLockBase()
		{
			this.Dispose(false, false);
		}



		/// <summary>
		///     Ensures that a lease time is between 15 and 60 seconds (inclusive).
		/// </summary>
		/// <param name="leaseTime">The lease time to validate.</param>
		/// <returns>
		///     True if the specified lease time is between 15 and 60 seconds (inclusive)
		/// </returns>
		private void GuardLeaseTime(TimeSpan? leaseTime)
		{
			if (!this.LockStateProvider.IsValidLeaseTime(leaseTime))
				throw new ArgumentException("Invalid lease time specified!", "leaseTime");
		}
	}
}