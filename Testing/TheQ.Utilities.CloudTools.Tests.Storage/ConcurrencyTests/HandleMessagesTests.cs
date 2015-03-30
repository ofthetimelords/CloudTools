// <copyright file="HandleMessagesTests.cs" company="nett">
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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheQ.Utilities.AzureTools.Tests.Storage.Models;
using TheQ.Utilities.CloudTools.Azure;
using TheQ.Utilities.CloudTools.Azure.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators;
using TheQ.Utilities.CloudTools.Storage.Models;



namespace TheQ.Utilities.CloudTools.Tests.Storage.ConcurrencyTests
{
	[TestClass]
	public class HandleMessagesTests
	{
		[TestMethod]
		public void TestSerial_NormalProcessing()
		{
			// Arrange
			const int runCount = 300;
			var client = new CloudEnvironment();
			var overflow = client.BlobClient.GetContainerReference("overflownqueues-1");
			var queue = client.QueueClient.GetQueueReference("test1");
			var result = string.Empty;
			var expected = string.Empty;
			var sw = new Stopwatch();
			var factory = new AzureExtendedQueueFactory(ExceptionPolicy.LogAndThrow, new AzureBlobContainer(overflow), new ConsoleLogService());
			var equeue = factory.Create(new AzureQueue(queue));

			for (var i = 0; i < runCount; i++) expected += i.ToString(CultureInfo.InvariantCulture);

			using (var mre = new ManualResetEvent(false))
			{
				var options = new HandleSerialMessageOptions(
					TimeSpan.FromSeconds(0),
					TimeSpan.FromMinutes(2),
					TimeSpan.FromSeconds(30),
					5,
					new CancellationToken(),
					message =>
					{
						if (message.GetMessageContents<string>() == "END")
						{
							mre.Set();
							return true;
						}

						result += message.GetMessageContents<string>();
						return true;
					},
					null,
					ex => { throw ex; });

				// Act
				sw.Start();
				queue.CreateIfNotExists();
				overflow.CreateIfNotExists();
				queue.Clear();
				for (var i = 0; i < runCount; i++) equeue.AddMessageEntity(i.ToString(CultureInfo.InvariantCulture));
				equeue.AddMessageEntity("END");
				equeue.HandleMessagesAsync(options);

				// Assert
				mre.WaitOne();
				sw.Stop();
				Trace.WriteLine("Total execution time (in seconds): " + sw.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));
				Assert.AreEqual(expected, result);
			}
		}



		//[TestMethod]
		//public void TestSerial_ParallelProcessing()
		//{
		//	// Arrange
		//	const int runCount = 300;
		//	var client = new CloudEnvironment();
		//	var queue = client.QueueClient.GetQueueReference("test2");
		//	var overflow = client.BlobClient.GetContainerReference("overflownqueues-2");
		//	var locking = new object();
		//	var result = string.Empty;
		//	var expected = string.Empty;
		//	var sw = new Stopwatch();
		//	long actuallyRun = 0;
		//	for (var i = 1; i < runCount + 1; i++) expected += ((char) (i)).ToString(CultureInfo.InvariantCulture);

		//	using (var mre = new ManualResetEvent(false))
		//	{
		//		var options = new HandleParallelMessageOptions(
		//			TimeSpan.FromSeconds(0),
		//			TimeSpan.FromMinutes(2),
		//			TimeSpan.FromSeconds(30),
		//			5,
		//			50,
		//			new ConsoleLogService(),
		//			new CancellationToken(),
		//			(AzureBlobContainer) overflow,
		//			message =>
		//			{
		//				lock (locking)
		//				{
		//					var character = message.GetMessageContents<string>();
		//					result += character;
		//				}

		//				if (Interlocked.Increment(ref actuallyRun) == runCount) mre.Set();

		//				return true;
		//			},
		//			null,
		//			ex => { throw ex; });

		//		// Act
		//		sw.Start();
		//		queue.CreateIfNotExists();
		//		overflow.CreateIfNotExists();
		//		queue.Clear();
		//		for (var i = 1; i < runCount + 1; i++) queue.AddSafeMessage(((char) (i)).ToString(CultureInfo.InvariantCulture), overflow);
		//		queue.HandleMessagesInParallel(options);

		//		// Assert
		//		mre.WaitOne();
		//		sw.Stop();
		//		Trace.WriteLine("Total execution time (in seconds): " + sw.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));
		//		Assert.IsTrue(expected.All(c => result.Contains(c)));
		//	}
		//}



		//[TestMethod]
		//public void TestSerial_BatchProcessing()
		//{
		//	// Arrange
		//	const int runCount = 300;
		//	var client = new CloudEnvironment();
		//	var queue = client.QueueClient.GetQueueReference("test5");
		//	var overflow = client.BlobClient.GetContainerReference("overflownqueues-5");
		//	var locking = new object();
		//	var result = string.Empty;
		//	var expected = string.Empty;
		//	var sw = new Stopwatch();
		//	long actuallyRun = 0;
		//	for (var i = 1; i < runCount + 1; i++) expected += ((char) (i)).ToString(CultureInfo.InvariantCulture);

		//	using (var mre = new ManualResetEvent(false))
		//	{
		//		var options = new HandleBatchMessageOptions(
		//			TimeSpan.FromSeconds(0),
		//			TimeSpan.FromMinutes(2),
		//			TimeSpan.FromSeconds(30),
		//			5,
		//			50,
		//			new ConsoleLogService(),
		//			new CancellationToken(),
		//			(AzureBlobContainer) overflow,
		//			messages =>
		//			{
		//				lock (locking)
		//				{
		//					foreach (var message in messages)
		//					{
		//						var character = message.GetMessageContents<string>();
		//						result += character;

		//						if (Interlocked.Increment(ref actuallyRun) == runCount) mre.Set();
		//					}
		//				}

		//				return messages;
		//			},
		//			null,
		//			ex => { throw ex; });

		//		// Act
		//		sw.Start();
		//		queue.CreateIfNotExists();
		//		overflow.CreateIfNotExists();
		//		queue.Clear();
		//		for (var i = 1; i < runCount + 1; i++) queue.AddSafeMessage(((char) (i)).ToString(CultureInfo.InvariantCulture), overflow);
		//		queue.HandleMessagesInBatch(options);

		//		// Assert
		//		mre.WaitOne();
		//		sw.Stop();
		//		Trace.WriteLine("Total execution time (in seconds): " + sw.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));
		//		Assert.IsTrue(expected.All(c => result.Contains(c)));
		//	}
		//}



		//[TestMethod]
		//public void TestOverflownMessages()
		//{
		//	// Arrange
		//	var client = new CloudEnvironment();
		//	var queue = client.QueueClient.GetQueueReference("test3");
		//	var overflow = client.BlobClient.GetContainerReference("overflownqueues-3");
		//	var result = string.Empty;
		//	var rnd = new Random();
		//	var expected = new string(Enumerable.Range(1, 128*1024).Select(r => (char) rnd.Next(1024, 4096)).ToArray());
		//	var sw = new Stopwatch();

		//	using (var mre = new ManualResetEvent(false))
		//	{
		//		var options = new HandleSerialMessageOptions(
		//			TimeSpan.FromSeconds(0),
		//			TimeSpan.FromMinutes(2),
		//			TimeSpan.FromSeconds(30),
		//			5,
		//			new ConsoleLogService(),
		//			new CancellationToken(),
		//			(AzureBlobContainer) overflow,
		//			message =>
		//			{
		//				result = message.GetMessageContents<string>();
		//				mre.Set();
		//				return true;
		//			},
		//			null,
		//			ex => { throw ex; });

		//		// Act
		//		sw.Start();
		//		queue.CreateIfNotExists();
		//		overflow.CreateIfNotExists();
		//		queue.Clear();
		//		queue.AddSafeMessage(expected, overflow);
		//		queue.HandleMessages(options);

		//		// Assert
		//		mre.WaitOne();
		//		sw.Stop();
		//		Trace.WriteLine("Total execution time (in seconds): " + sw.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));
		//		Assert.AreEqual(expected, result);
		//	}
		//}



		//[TestMethod]
		//public void TestSerializedMessages()
		//{
		//	// Arrange
		//	var client = new CloudEnvironment();
		//	var queue = client.QueueClient.GetQueueReference("test4");
		//	var overflow = client.BlobClient.GetContainerReference("overflownqueues-4");
		//	ComplexModel result = null;
		//	var expected = new ComplexModel {Name = "Test"};
		//	var sw = new Stopwatch();

		//	using (var mre = new ManualResetEvent(false))
		//	{
		//		var options = new HandleSerialMessageOptions(
		//			TimeSpan.FromSeconds(0),
		//			TimeSpan.FromMinutes(2),
		//			TimeSpan.FromSeconds(30),
		//			5,
		//			new ConsoleLogService(),
		//			new CancellationToken(),
		//			(AzureBlobContainer) overflow,
		//			message =>
		//			{
		//				result = message.GetMessageContents<ComplexModel>();
		//				mre.Set();
		//				return true;
		//			},
		//			null,
		//			ex => { throw ex; });

		//		// Act
		//		sw.Start();
		//		queue.CreateIfNotExists();
		//		overflow.CreateIfNotExists();
		//		queue.Clear();
		//		queue.AddSafeMessage(expected, overflow);
		//		queue.HandleMessages(options);

		//		// Assert
		//		mre.WaitOne();
		//		sw.Stop();
		//		Trace.WriteLine("Total execution time (in seconds): " + sw.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));
		//		Assert.AreEqual(expected.Name, result.Name);
		//		Assert.AreEqual(expected.ADictionary.First().Key, result.ADictionary.First().Key);
		//		Assert.AreEqual(expected.ADictionary.First().Value, result.ADictionary.First().Value);
		//		Assert.AreEqual(expected.AList.First(), result.AList.First());
		//	}
		//}
	}
}