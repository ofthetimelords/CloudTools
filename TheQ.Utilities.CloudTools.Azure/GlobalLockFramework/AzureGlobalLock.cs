// <copyright file="AzureGlobalLock.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.GlobalLockFramework;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Azure.GlobalLockFramework
{
	public class AzureGlobalLock : GlobalLockBase<AzureLockState>
	{
		/// <summary>
		///     <para>Initializes a new instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <exception cref="ArgumentNullException">container;Parameter <paramref name="container" /> was <see langword="null" /></exception>
		public AzureGlobalLock(AzureLockStateProvider lockStateProvider, AzureLockState initialState) : base(lockStateProvider, initialState) { }



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="cancelToken">The cancellation token.</param>
		/// <exception cref="ArgumentNullException">container;Parameter <paramref name="container" /> was <see langword="null" /></exception>
		public AzureGlobalLock(CancellationToken cancelToken, AzureLockStateProvider lockStateProvider, AzureLockState initialState)
			: base(cancelToken, lockStateProvider, initialState) { }



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <exception cref="ArgumentNullException">container;Parameter <paramref name="container" /> was <see langword="null" /></exception>
		public AzureGlobalLock(AzureLockStateProvider lockStateProvider, AzureLockState initialState, [CanBeNull] ILogService logService)
			: base(lockStateProvider, initialState, logService) { }



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="TheQ.Utilities.CloudTools.Storage.GlobalLockFramework.GlobalLockBase`1" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="container">The container on which to select a blob to apply a lock on.</param>
		/// <param name="cancelToken">The cancellation token.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <exception cref="ArgumentNullException">container;Parameter <paramref name="container" /> was <see langword="null" /></exception>
		public AzureGlobalLock(CancellationToken cancelToken, AzureLockStateProvider lockStateProvider, AzureLockState initialState, [CanBeNull] ILogService logService)
			: base(cancelToken, lockStateProvider, initialState, logService) { }
	}
}