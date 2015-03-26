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

using TheQ.Utilities.CloudTools.Storage.GlobalLockFramework;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Azure.GlobalLockFramework
{
	public class AzureLockStateProvider : ILockStateProvider<AzureLockState>
	{
		public AzureLockStateProvider(ILogService logService) { this.LogService = logService; }


		private ILogService LogService { get; set; }


		internal const int DefaultLeaseTimeInSeconds = 60;



		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() { if (!this.IsDisposed) this.IsDisposed = true; }



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



		public async Task<AzureLockState> BreakLockAsync(AzureLockState state, CancellationToken cancelToken)
		{
			Guard.NotNull(state, "state");


			// TODO: What if does not exist already, Exceptions
			if (state.LeaseId != null) await this.UnregisterLockAsync(state, cancelToken);
			else await this.BreakLockInternal(state, cancelToken);

			return state;
		}



		public async Task<AzureLockState> UnregisterLockAsync(AzureLockState state, CancellationToken cancelToken)
		{
			Guard.NotNull(state, "state");

			if (state.LockingBlob != null)
			{
				await state.LockingBlob.ReleaseLeaseAsync(new AzureAccessCondition(AccessCondition.GenerateLeaseCondition(state.LeaseId)), cancelToken);
				state.LockingBlob = null;
				state.LeaseId = null;
			}

			return state;
		}



		public async Task<AzureLockState> RegisterLockAsync(AzureLockState state, string newLockName, TimeSpan? leaseTime, bool isDefaultLeaseTime, CancellationToken cancelToken)
		{
			Guard.NotNull(state, "state");

			await state.LockingBlobContainer.CreateIfNotExistsAsync(cancelToken);

			state.LockingBlob = state.LockingBlobContainer.GetBlobReference(newLockName);

			if (!await state.LockingBlob.ExistsAsync(cancelToken)) await state.LockingBlob.UploadFromByteArrayAsync(new byte[0], 0, 0, cancelToken);

			state.LeaseId = await state.LockingBlob.AcquireLeaseAsync(isDefaultLeaseTime && !leaseTime.HasValue ? TimeSpan.FromSeconds(AzureLockStateProvider.DefaultLeaseTimeInSeconds) : leaseTime, null, cancelToken);
			state.LockName = newLockName;

			return state;
		}



		public async Task<AzureLockState> RenewLockAsync(AzureLockState state, CancellationToken cancelToken)
		{
			Guard.NotNull(state, "state");

			if (state.LockingBlob != null) await state.LockingBlob.RenewLeaseAsync(new AzureAccessCondition(AccessCondition.GenerateLeaseCondition(state.LeaseId)), cancelToken);

			return state;
		}

		/// <summary>
		///     Ensures that a lease time is between 15 and 60 seconds (inclusive).
		/// </summary>
		/// <param name="leaseTime">The lease time to validate.</param>
		/// <returns>
		///     True if the specified lease time is between 15 and 60 seconds (inclusive)
		/// </returns>
		public bool IsValidLeaseTime(TimeSpan? leaseTime)
		{
			return !leaseTime.HasValue || (leaseTime.Value.TotalSeconds >= 15 && leaseTime.Value.TotalSeconds <= 60);
		}


		private async Task<AzureLockState> BreakLockInternal(AzureLockState state, CancellationToken cancelToken)
		{
			if (state.LockingBlob != null)
			{
				await state.LockingBlob.BreakLeaseAsync(null, cancelToken);
				state.LockingBlob = null;
				state.LeaseId = null;
			}

			return state;
		}
	}
}