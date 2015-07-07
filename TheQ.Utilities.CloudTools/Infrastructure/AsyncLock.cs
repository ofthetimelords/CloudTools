// <copyright file="AsyncLock.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>Stephen Toub</author>
// <email></email>
// <date>2015/06/21</date>
// <summary>
// Based on the work of Stephen Toub, http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266988.aspx
// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TheQ.Utilities.CloudTools.Storage.Infrastructure
{
	// http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266983.aspx
	[Obsolete]
	internal class AsyncSemaphore
	{
		private static readonly Task Completed = Task.FromResult(true);
		private readonly Queue<TaskCompletionSource<bool>> _waiters = new Queue<TaskCompletionSource<bool>>();
		private int _currentCount;



		public AsyncSemaphore(int initialCount)
		{
			if (initialCount < 0)
				throw new ArgumentOutOfRangeException("initialCount");
			this._currentCount = initialCount;
		}



		public Task WaitAsync()
		{
			lock (this._waiters)
			{
				if (this._currentCount > 0)
				{
					--this._currentCount;
					return AsyncSemaphore.Completed;
				}
				var waiter = new TaskCompletionSource<bool>();
				this._waiters.Enqueue(waiter);
				return waiter.Task;
			}
		}



		public void Release()
		{
			TaskCompletionSource<bool> toRelease = null;
			lock (this._waiters)
			{
				if (this._waiters.Count > 0)
					toRelease = this._waiters.Dequeue();
				else
					++this._currentCount;
			}
			if (toRelease != null)
				toRelease.SetResult(true);
		}
	}



	[Obsolete]
	public class AsyncLock
	{
		private readonly Task<Releaser> _releaser;
		private readonly AsyncSemaphore _semaphore;



		public AsyncLock()
		{
			this._semaphore = new AsyncSemaphore(1);
			this._releaser = Task.FromResult(new Releaser(this));
		}



		public ConfiguredTaskAwaitable<Releaser> LockAsync()
		{
			var wait = this._semaphore.WaitAsync();
			return
				(wait.IsCompleted
					? this._releaser
					: wait.ContinueWith((_, state) => new Releaser((AsyncLock) state), this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default))
					.ConfigureAwait(false);
		}



		public ConfiguredTaskAwaitable<Releaser> LockAsync(CancellationToken token)
		{
			var wait = this._semaphore.WaitAsync();
			return
				(wait.IsCompleted
					? this._releaser
					: wait.ContinueWith((_, state) => new Releaser((AsyncLock) state), this, token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)).ConfigureAwait(false);
		}



		[Obsolete]
		public struct Releaser : IDisposable
		{
			private readonly AsyncLock _toRelease;



			internal Releaser(AsyncLock toRelease)
			{
				this._toRelease = toRelease;
			}



			public void Dispose()
			{
				if (this._toRelease != null)
					this._toRelease._semaphore.Release();
			}
		}
	}
}