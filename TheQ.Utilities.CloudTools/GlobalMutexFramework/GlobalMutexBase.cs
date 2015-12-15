// <copyright file="GlobalMutexBase.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/31</date>
// <summary>
// 
// </summary>

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.GlobalMutexFramework
{
	/// <summary>
	///     Defines a base implementation of a MutEx framework that can span between different processes or even host machines.
	/// </summary>
	public abstract class GlobalMutexBase<TLockState> : IGlobalMutex where TLockState : class, ILockState
	{
		/// <summary>
		///     The default time between locking attempts, set to 3 seconds.
		/// </summary>
		private static TimeSpan DefaultTimeBetweenLockAttemptsValue = TimeSpan.FromSeconds(3);



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="lockStateProvider">The lock state provider to use.</param>
		/// <param name="initialState">The initial state of the lock.</param>
		protected GlobalMutexBase(ILockStateProvider<TLockState> lockStateProvider, TLockState initialState)
			: this(new CancellationToken(), lockStateProvider, initialState, null)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="lockStateProvider">The lock state provider.</param>
		/// <param name="initialState">The initial state.</param>
		/// <param name="logService">The logging service to use.</param>
		protected GlobalMutexBase(ILockStateProvider<TLockState> lockStateProvider, TLockState initialState, [CanBeNull] ILogService logService)
			: this(new CancellationToken(), lockStateProvider, initialState, logService)
		{
		}



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="cancelToken">The cancellation token.</param>
		/// <param name="lockStateProvider">The lock state provider.</param>
		/// <param name="initialState">The initial state.</param>
		/// <param name="logService">The logging service to use.</param>
		protected GlobalMutexBase(CancellationToken cancelToken, ILockStateProvider<TLockState> lockStateProvider, TLockState initialState, [CanBeNull] ILogService logService = null)
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
			get { return DefaultTimeBetweenLockAttemptsValue; }
			set { if (DefaultTimeBetweenLockAttemptsValue.TotalSeconds >= 1) DefaultTimeBetweenLockAttemptsValue = value; }
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
		[CanBeNull]
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


		/// <summary>
		///     Gets the name of the current lock.
		/// </summary>
		/// <value>
		///     A string value with the name of the current lock.
		/// </value>
		[CanBeNull]
		public string CurrentLockName
		{
			get { return this.LockState.LockName; }
		}


		/// <summary>
		///     Gets the current lease identifier.
		/// </summary>
		/// <value>
		///     A string value with current lease identifier.
		/// </value>
		[CanBeNull]
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
			await this.UnlockAsync(true).ConfigureAwait(false);
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
		public async Task<IGlobalMutex> ForceUnlockAsync()
		{
			return await this.ForceUnlockAsync(false).ConfigureAwait(false);
		}



		/// <summary>
		///     Force-unlock a lock even if it was acquired by another thread.
		/// </summary>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		public IGlobalMutex ForceUnlock()
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
		public IGlobalMutex ForceUnlock(bool throwOnError)
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
		/// <exception cref="CloudToolsStorageException">A conflict has occurred while attempting to retrieve the lock and <paramref name="throwOnError" /> was set to <c>true</c>.</exception>
		/// <exception cref="Exception">Any uncaught exception.</exception>
		[NotNull]
		public async Task<IGlobalMutex> ForceUnlockAsync(bool throwOnError)
		{
			try
			{
				if (this.LockState.LeaseId != null) await this.UnlockAsync().ConfigureAwait(false);
				else
				{
					this.LockState = await this.LockStateProvider.BreakLockAsync(this.LockState, this.CancelToken).ConfigureAwait(false);
					this.LogService.QuickLogDebug("GlobalMutex", "Global lock with name '{0}' was force-released", this.LockName);
				}
			}
			catch (CloudToolsStorageException ex)
			{
				if (ex.StatusCode != 404 && ex.StatusCode != 409)
				{
					this.LogService.QuickLogError("GlobalMutex", ex, "Attempting to force-release global lock with name '{0}' failed due to an unexpected exception", this.LockName);
					if (throwOnError) throw;
				}
				else this.LogService.QuickLogDebug("GlobalMutex", "Attempting to force a global lock with name '{0}' failed; the lock must have been released already", this.LockName);
			}
			catch (Exception ex)
			{
				var aggEx = ex as AggregateException;
				if (aggEx != null)
				{
					var ctEx = aggEx.InnerException as CloudToolsStorageException;

					if (ctEx.StatusCode != 404 && ctEx.StatusCode != 409)
					{
						this.LogService.QuickLogError("GlobalMutex", ex, "Attempting to force-release global lock with name '{0}' failed due to an unexpected exception", this.LockName);
						if (throwOnError) throw;
					}
					else this.LogService.QuickLogDebug("GlobalMutex", "Attempting to force a global lock with name '{0}' failed; the lock must have been released already", this.LockName);
				}
				else
				{
					this.LogService.QuickLogError("GlobalMutex", ex, "Attempting to force-release global lock with name '{0}' failed due to an unexpected exception", this.LockName);
					if (throwOnError) throw;
				}
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
		public IGlobalMutex TryLock(string lockName, out bool success)
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
		public IGlobalMutex TryLock(string lockName, TimeSpan? leaseTime, out bool success)
		{
			success = Task.Run(() => this.TryLockInternal(lockName, leaseTime, false), this.CancelToken).Result;
			return this;
		}



		/// <summary>
		///     Attempts to create a lock with the specified name and will not return before a lock can be retrieved.
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <exception cref="ArgumentException">Parameter <paramref name="lockName" /> is <see langword="null" /> or empty;lockName</exception>
		/// <exception cref="ArgumentNullException">The parameter was null: <paramref name="lockName" /></exception>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		[NotNull]
		public Task<IGlobalMutex> LockAsync(string lockName)
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
		/// <exception cref="ArgumentNullException">The parameter was null: <paramref name="lockName" /></exception>
		public Task<IGlobalMutex> LockAsync(string lockName, TimeSpan? leaseTime)
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
		/// <exception cref="ArgumentException">
		///     Parameter <paramref name="lockName" /> is <see langword="null" /> or empty;lockName or Parameter <paramref name="leaseTime" /> must be either <see langword="null" /> or between 15 and 60
		///     seconds;leaseTime
		/// </exception>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		/// <exception cref="ArgumentNullException">The parameter was null: <paramref name="lockName" /></exception>
		public Task<IGlobalMutex> LockAsync(string lockName, TimeSpan? leaseTime, TimeSpan timeBetweenAttempts)
		{
			Guard.NotNull(lockName, "lockName");

			return this.LockInternal(lockName, leaseTime, false, timeBetweenAttempts);
		}



		private async Task<IGlobalMutex> LockInternal(string lockName, TimeSpan? leaseTime, bool isDefaultLeaseTime, TimeSpan timeBetweenAttempts)
		{
			Guard.NotNull(lockName, "lockName");

			var success = await this.TryLockInternal(lockName, leaseTime, isDefaultLeaseTime).ConfigureAwait(false);

			while (!success)
			{
				if (this.CancelToken.IsCancellationRequested) return this;

				await Task.Delay((int) timeBetweenAttempts.TotalSeconds, this.CancelToken).ConfigureAwait(false);
				success = await this.TryLockInternal(lockName, leaseTime, isDefaultLeaseTime).ConfigureAwait(false);
			}

			return this;
		}



		/// <summary>
		///     Handles an attempt to create a lock on a blob.
		/// </summary>
		/// <param name="lockName">The name of the lock to acquire.</param>
		/// <param name="leaseTime">A custom lease time for the lock.</param>
		/// <param name="isDefaultLeaseTime">if set to <c>true</c> [is default lease time].</param>
		/// <returns>
		///     The current instance (to allow fluent usage).
		/// </returns>
		/// <exception cref="ArgumentException">Invalid lease time specified!</exception>
		private async Task<bool> TryLockInternal([NotNull] string lockName, TimeSpan? leaseTime, bool isDefaultLeaseTime)
		{
			Guard.NotNull(lockName, "lockName");
			this.GuardLeaseTime(leaseTime);

			try
			{
				if (this.LockState.LeaseId != null && this.LockName != null && lockName == this.LockName) return true;

				if (this.CancelToken.IsCancellationRequested) return false;

				await this.UnlockAsync().ConfigureAwait(false);

				// Allow exceptions to propagate from here.
				await this.TryAcquireLeaseAsync(lockName, leaseTime, isDefaultLeaseTime).ConfigureAwait(false);

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
		/// <exception cref="ArgumentNullException">The parameter was null: <paramref name="lockName" /></exception>
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

				await Task.Delay(TimeSpan.FromSeconds((leaseTime.TotalSeconds*3)/4), cancelToken).ConfigureAwait(false);

				try
				{
					if (this.LockName != null) this.LockState = await this.LockStateProvider.RenewLockAsync(this.LockState, cancelToken).ConfigureAwait(false);
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex)
				{
					this.LogService.QuickLogError(
						"GlobalMutex",
						ex,
						"Unexpected exception while attempting to renew a global lock (using BLOB leases) with name '{0}' and ID '{1}'",
						lockName,
						this.LockState.LeaseId);
					throw;
				}
			}
		}



		/// <summary>
		/// Attempts to acquire a lease on a BLOB.
		/// </summary>
		/// <param name="lockName">Name of the blob to acquire the lock upon.</param>
		/// <param name="leaseTime">The lease time.</param>
		/// <param name="isDefaultLeaseTime">if set to <c>true</c> then signifies that <paramref name="leaseTime"/> should use the default value.</param>
		/// <returns>An asynchronous task.</returns>
		/// <remarks>Note that a null value for <paramref name="leaseTime"/> doesn't necessarilly indicate a default value (use to fix infinite-lease bugs).</remarks>
		private async Task TryAcquireLeaseAsync([NotNull] string lockName, TimeSpan? leaseTime, bool isDefaultLeaseTime)
		{
			Guard.NotNull(lockName, "lockName");
			this.GuardLeaseTime(leaseTime);

			try
			{
				this.LockState = await this.LockStateProvider.RegisterLockAsync(this.LockState, lockName, leaseTime, isDefaultLeaseTime, this.CancelToken).ConfigureAwait(false);
			}
			catch (CloudToolsStorageException ex)
			{
				// 409 a lease is already present
				if (ex.ErrorCode != "LeaseAlreadyPresent" && ex.StatusCode != (int) HttpStatusCode.Conflict)
				{
					this.LogService.QuickLogError("GlobalMutex", ex, "Unexpected exception while attempting to create a global lock (using BLOB leases) with name '{0}'", lockName);
					throw;
				}

				this.LogService.QuickLogDebug("GlobalMutex", "Attempting to create a global lock with name '{0}' failed; a lock has been placed already", lockName);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				this.LogService.QuickLogError("GlobalMutex", ex, "Unexpected exception while attempting to create a global lock (using BLOB leases) with name '{0}'", lockName);
				throw;
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
				await this.LockStateProvider.UnregisterLockAsync(this.LockState, this.CancelToken).ConfigureAwait(false);

				if (attemptLog) this.LogService.QuickLogDebug("GlobalMutex", "Global lock with name '{0}' was released", this.LockName);
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
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex)
				{
					try
					{
						if (attemptLog) this.LogService.QuickLogError("GlobalMutex", ex, "An error occurred while attempting to dispose a global lock during unlocking, with name '{0}'", this.LockName);
					}
					catch
					{
						// Attempt to ignore exceptions at this level
					}
				}

				try
				{
					if (disposing) this.LockStateProvider.Dispose();
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex)
				{
					try
					{
						if (attemptLog)
						{
							this.LogService.QuickLogError(
								"GlobalMutex",
								ex,
								"An error occurred while attempting to dispose the underlying lock provider during unlocking, with name '{0}'",
								this.LockName);
						}
					}
					catch
					{
						// Attempt to ignore exceptions at this level
					}
				}

				this.IsDisposed = true;
			}
		}



		/// <summary>
		///     <para>Finalizes an instance of the <see cref="GlobalMutexBase{TLockState}" /></para>
		///     <para>class.</para>
		/// </summary>
		~GlobalMutexBase()
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
		/// <exception cref="ArgumentException">Invalid lease time specified!</exception>
		private void GuardLeaseTime(TimeSpan? leaseTime)
		{
			if (!this.LockStateProvider.IsValidLeaseTime(leaseTime)) throw new ArgumentException("Invalid lease time specified!", "leaseTime");
		}
	}
}