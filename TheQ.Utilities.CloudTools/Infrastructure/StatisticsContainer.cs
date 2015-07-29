// <copyright file="NullLogService.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/31</date>
// <summary>
// 
// </summary>


using System.Threading;

namespace TheQ.Utilities.CloudTools.Storage.Infrastructure
{
	/// <summary>
	///     An object that's used to monitor a queue's statistics over its lifetime.
	/// </summary>
	public class StatisticsContainer
	{
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return string.Format("Queue Statistics | All slots: {0}, Busy Slots: {1}, Listeners: {2}, # of Successful M.: {3}, # of Reenqueued M.: {4}, Total Reenqueues: {5}, # of Poison M.: {6}, # of Faulted: {7}",
				this.AllMessageSlots,
				this.BusyMessageSlots,
				this.CurrentListeners,
				this.TotalSuccessfulMessages,
				this.TotalReenqueuedMessages,
				this.TotalReenqueuesCount,
				this.TotalPoisonMessages,
				this.TotalFaultedMessages);
		}



		private int _busyMessageSlots;
		private int _allMessageSlots;
		private int _currentListeners;
		private long _totalSuccessfulMessages;
		private long _totalReenqueuedMessages;
		private long _totalReenqueuesCount;
		private long _totalPoisonMessages;
		private long _totalFaultedMessages;


		public int AllMessageSlots
		{
			get { return this._allMessageSlots; }
		}


		public int BusyMessageSlots
		{
			get { return this._busyMessageSlots; }
		}


		public int FreeMessageSlots
		{
			get { return this._allMessageSlots - this._busyMessageSlots; }
		}


		public int CurrentListeners
		{
			get { return this._currentListeners; }
		}



		public long TotalSuccessfulMessages
		{
			get { return this._totalSuccessfulMessages; }
		}


		public long TotalReenqueuedMessages
		{
			get { return this._totalReenqueuedMessages; }
		}


		public long TotalReenqueuesCount
		{
			get { return this._totalReenqueuesCount; }
		}


		public long TotalPoisonMessages
		{
			get { return this._totalPoisonMessages; }
		}


		public long TotalFaultedMessages
		{
			get { return this._totalFaultedMessages; }
		}



		public void IncreaseAllMessageSlots(int count)
		{
			Interlocked.Add(ref this._allMessageSlots, count);
		}



		public void IncreaseAllMessageSlots()
		{
			Interlocked.Increment(ref this._allMessageSlots);
		}



		public void DecreaseAllMessageSlots(int count)
		{
			Interlocked.Add(ref this._allMessageSlots, -count);
		}



		public void DecreaseAllMessageSlots()
		{
			Interlocked.Decrement(ref this._allMessageSlots);
		}






		public void IncreaseBusyMessageSlots(int count)
		{
			Interlocked.Add(ref this._busyMessageSlots, count);
		}



		public void IncreaseBusyMessageSlots()
		{
			Interlocked.Increment(ref this._busyMessageSlots);
		}



		public void DecreaseBusyMessageSlots(int count)
		{
			Interlocked.Add(ref this._busyMessageSlots, -count);
		}



		public void DecreaseBusyMessageSlots()
		{
			Interlocked.Decrement(ref this._busyMessageSlots);
		}









		public void IncreaseListeners(int count)
		{
			Interlocked.Add(ref this._currentListeners, count);
		}



		public void IncreaseListeners()
		{
			Interlocked.Increment(ref this._currentListeners);
		}



		public void DecreaseListeners(int count)
		{
			Interlocked.Add(ref this._currentListeners, -count);
		}



		public void DecreaseListeners()
		{
			Interlocked.Decrement(ref this._currentListeners);
		}







		public void IncreaseSuccessfulMessages(int count)
		{
			Interlocked.Add(ref this._totalSuccessfulMessages, count);
		}



		public void IncreaseSuccessfulMessages()
		{
			Interlocked.Increment(ref this._totalSuccessfulMessages);
		}






		public void IncreaseReenqueuedMessages(int count)
		{
			Interlocked.Add(ref this._totalReenqueuedMessages, count);
		}



		public void IncreaseReenqueuedMessages()
		{
			Interlocked.Increment(ref this._totalReenqueuedMessages);
		}






		public void IncreaseReenqueuesCount(int count)
		{
			Interlocked.Add(ref this._totalReenqueuesCount, count);
		}



		public void IncreaseReenqueuesCount()
		{
			Interlocked.Increment(ref this._totalReenqueuesCount);
		}






		public void IncreasePoisonMessages(int count)
		{
			Interlocked.Add(ref this._totalPoisonMessages, count);
		}



		public void IncreasePoisonMessages()
		{
			Interlocked.Increment(ref this._totalPoisonMessages);
		}






		public void IncreaseCriticallyFaultedMessages(int count)
		{
			Interlocked.Add(ref this._totalFaultedMessages, count);
		}



		public void IncreaseCriticallyFaultedMessages()
		{
			Interlocked.Increment(ref this._totalFaultedMessages);
		}
	}
}