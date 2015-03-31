// <copyright file="AzureLockStateProvider.cs" company="nett">
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

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using TheQ.Utilities.CloudTools.Storage.GlobalLockFramework;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.GlobalLockFramework
{
	/// <summary>
	/// An implementation of <see cref="ILockStateProvider{T}"/> for Windows Azure, using BLOB storage as the back-end.
	/// </summary>
	public class AzureLockStateProvider : ILockStateProvider<AzureLockState>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AzureLockStateProvider"/> class.
		/// </summary>
		/// <param name="logService">The logging service to use.</param>
		public AzureLockStateProvider(ILogService logService) { this.LogService = logService; }


		/// <summary>
		/// Gets or sets the logging service to use.
		/// </summary>
		/// <value>
		/// An <see cref="ILogService"/> implementation.
		/// </value>
		private ILogService LogService { get; set; }


		/// <summary>
		/// The default lease time in seconds supported by Windows Azure, set to 60 seconds (the maximum amount).
		/// </summary>
		internal const int DefaultLeaseTimeInSeconds = 60;



		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <remarks>This instance is not disposable, but will have to act as one.</remarks>
		public void Dispose()
		{
			if (!this.IsDisposed)
			{
				this.IsDisposed = true;
				GC.SuppressFinalize(this);
			}
		}



		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		public bool IsDisposed { get; private set; }



		/// <summary>
		/// Breaks the lock (forces a lock to unlock) asynchronously.
		/// </summary>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		public async Task<AzureLockState> BreakLockAsync(AzureLockState state, CancellationToken cancelToken)
		{
			Guard.NotNull(state, "state");
			CloudToolsStorageException exception = null;

			try
			{
				// TODO: What if does not exist already, Exceptions
				if (state.LeaseId != null) await this.UnregisterLockAsync(state, cancelToken).ConfigureAwait(false);
				else await this.BreakLockInternal(state, cancelToken).ConfigureAwait(false);
			}
			catch (CloudToolsStorageException ex)
			{
				exception = ex;
			}

			if (exception != null && exception.StatusCode == 409 || exception.StatusCode == 412) 
				this.BreakLockInternal(state, cancelToken).Wait(cancelToken);

			return state;
		}



		/// <summary>
		/// Unregisters a lock asynchronously.
		/// </summary>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		public async Task<AzureLockState> UnregisterLockAsync(AzureLockState state, CancellationToken cancelToken)
		{
			Guard.NotNull(state, "state");

			if (state.LockingBlob != null)
			{
				await ((CloudBlockBlob)(state.LockingBlob as AzureBlob)).ReleaseLeaseAsync(AccessCondition.GenerateLeaseCondition(state.LeaseId), cancelToken).ConfigureAwait(false);
				//await state.LockingBlob.ReleaseLeaseAsync(new AzureAccessCondition(AccessCondition.GenerateLeaseCondition(state.LeaseId)), cancelToken).ConfigureAwait(false);
				
				state.LockingBlob = null;
				state.LeaseId = null;
			}

			return state;
		}



		/// <summary>
		/// Registers a lock asynchronously.
		/// </summary>
		/// <param name="newLockName">The (new) name of the lock. This will override the <see cref="ILockState.LockName"/> currently assigned when the operation is complete.</param>
		/// <param name="leaseTime">The amount of time to lease the lock.</param>
		/// <param name="isDefaultLeaseTime">if set to <c>true</c> then implementations should use a default lease time for locks (in cases where null represents something else).</param>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		public async Task<AzureLockState> RegisterLockAsync(AzureLockState state, string newLockName, TimeSpan? leaseTime, bool isDefaultLeaseTime, CancellationToken cancelToken)
		{
			Guard.NotNull(state, "state");

			await state.LockingBlobContainer.CreateIfNotExistsAsync(cancelToken).ConfigureAwait(false);

			state.LockingBlob = state.LockingBlobContainer.GetBlobReference(newLockName);

			if (!await state.LockingBlob.ExistsAsync(cancelToken).ConfigureAwait(false)) await state.LockingBlob.UploadFromByteArrayAsync(new byte[0], 0, 0, cancelToken).ConfigureAwait(false);

			state.LeaseId =
				await
					state.LockingBlob.AcquireLeaseAsync(
						isDefaultLeaseTime && !leaseTime.HasValue ? TimeSpan.FromSeconds(AzureLockStateProvider.DefaultLeaseTimeInSeconds) : leaseTime,
						null,
						cancelToken).ConfigureAwait(false);
			state.LockName = newLockName;

			return state;
		}



		/// <summary>
		/// Renews an existing lock asynchronously.
		/// </summary>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		public async Task<AzureLockState> RenewLockAsync(AzureLockState state, CancellationToken cancelToken)
		{
			Guard.NotNull(state, "state");

			if (state.LockingBlob != null) 
				await ((CloudBlockBlob)(state.LockingBlob as AzureBlob)).RenewLeaseAsync(AccessCondition.GenerateLeaseCondition(state.LeaseId), cancelToken).ConfigureAwait(false);
			//if (state.LockingBlob != null) await state.LockingBlob.RenewLeaseAsync(new AzureAccessCondition(AccessCondition.GenerateLeaseCondition(state.LeaseId)), cancelToken).ConfigureAwait(false);

			return state;
		}



		/// <summary>
		///     Ensures that a lease time is between 15 and 60 seconds (inclusive; limits set by Windows Azure).
		/// </summary>
		/// <param name="leaseTime">The lease time to validate.</param>
		/// <returns>
		///     True if the specified lease time is between 15 and 60 seconds (inclusive; limits set by Windows Azure)
		/// </returns>
		public bool IsValidLeaseTime(TimeSpan? leaseTime)
		{
			return !leaseTime.HasValue || (leaseTime.Value.TotalSeconds >= 15 && leaseTime.Value.TotalSeconds <= 60);
		}



		/// <summary>
		/// Performs the break-lock operation.
		/// </summary>
		/// <param name="state">The lock state object.</param>
		/// <param name="cancelToken">An optional cancellation token.</param>
		/// <returns>The lock state object, updated after the operation.</returns>
		private async Task<AzureLockState> BreakLockInternal(AzureLockState state, CancellationToken cancelToken)
		{
			if (state.LockingBlob != null)
			{
				await state.LockingBlob.BreakLeaseAsync(null, cancelToken).ConfigureAwait(false);
				state.LockingBlob = null;
				state.LeaseId = null;
			}

			return state;
		}
	}
}