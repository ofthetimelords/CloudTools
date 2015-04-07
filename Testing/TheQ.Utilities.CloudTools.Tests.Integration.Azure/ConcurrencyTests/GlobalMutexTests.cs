// <copyright file="GlobalMutexTests.cs" company="nett">
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
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheQ.Utilities.CloudTools.Azure;
using TheQ.Utilities.CloudTools.Azure.GlobalMutexFramework;

namespace TheQ.Utilities.CloudTools.Tests.Integration.Azure.ConcurrencyTests
{
	[TestClass]
	public class GlobalMutexTests
	{
		[TestMethod]
		public void TestLock_Lock_Concurrency()
		{
			// Arrange
			var client = new CloudEnvironment();
			var result = string.Empty;
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests1"), lockProvider, consoleLog);

			var thread1 = new Thread(
				() =>
				{
					using (var globalMutex = factory.CreateLock("test_lock"))
					{
						result += "1";

						Thread.Sleep(5000);
					}
				});
			var thread2 = new Thread(
				() =>
				{
					Thread.Sleep(3000); // Ensure it will enter later than the previous method
					using (var globalMutex = factory.CreateLock("test_lock")) result += "2";
				});

			// Act
			client.BreakAnyLeases("globalmutextests1", "test_lock");
			thread2.Start();
			thread1.Start();
			thread1.Join();
			thread2.Join();

			// Assert
			Assert.AreEqual("12", result);
		}



		[TestMethod]
		public void TestTryLock_TryLock_Concurrency()
		{
			// Arrange
			var client = new CloudEnvironment();
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests2"), lockProvider, consoleLog);
			var result = string.Empty;
			var thread1 = new Thread(
				() =>
				{
					bool isLocked;
					using (var globalMutex = factory.TryCreateLock("test_lock", out isLocked))
					{
						if (isLocked) result += "1";
						Thread.Sleep(2000);
					}
				});
			var thread2 = new Thread(
				() =>
				{
					Thread.Sleep(1000); // Ensure it will enter later than the previous method
					bool isLocked;
					using (var globalMutex = factory.TryCreateLock("test_lock", out isLocked)) if (isLocked) result += "2";
				});

			// Act
			client.BreakAnyLeases("globalmutextests2", "test_lock");
			thread2.Start();
			thread1.Start();
			thread1.Join();
			thread2.Join();

			// Assert
			Assert.AreEqual("1", result);
		}



		[TestMethod]
		public void TestLock_Lock_LockIsReleased()
		{
			// Arrange
			var client = new CloudEnvironment();
			var result = string.Empty;
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests3"), lockProvider, consoleLog);


			// Act
			client.BreakAnyLeases("globalmutextests3", "test_lock");
			using (var globalMutex = factory.CreateLock("test_lock")) result += "1";

			using (var globalMutex = factory.CreateLock("test_lock")) result += "2";

			// Assert
			Assert.AreEqual("12", result);
		}



		[TestMethod]
		public void TestLock_LeaseIsRenewed()
		{
			// Arrange
			var client = new CloudEnvironment();
			var result = string.Empty;
			bool isLocked;
			bool isLockedInner;
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests4"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globalmutextests4", "test_lock");
			using (var globalMutex = factory.TryCreateLock("test_lock", TimeSpan.FromSeconds(15), out isLocked))
			{
				if (isLocked) result += "1";

				Thread.Sleep(TimeSpan.FromSeconds(40));
				using (var globalMutexInner = factory.TryCreateLock("test_lock", out isLockedInner)) if (isLockedInner) result += "2";
			}

			// Assert
			Assert.AreEqual("1", result);
		}



		[TestMethod]
		public void TestTryLock_TryLock_LockIsNotReleased()
		{
			// Arrange
			var client = new CloudEnvironment();
			var result = string.Empty;
			bool isLocked;
			bool isLockedInner;
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests5"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globalmutextests5", "test_lock");
			using (var globalMutex = factory.TryCreateLock("test_lock", out isLocked))
			{
				if (isLocked) result += "1";

				using (var globalMutexInner = factory.TryCreateLock("test_lock", out isLockedInner)) if (isLockedInner) result += "2";
			}

			// Assert
			Assert.AreEqual("1", result);
		}



		[TestMethod]
		public void TestLock_Lock_TryLock_IsNotReleased()
		{
			// Arrange
			var client = new CloudEnvironment();
			var result = string.Empty;
			bool isLocked;
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests6"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globalmutextests6", "test_lock");
			var globalMutex = factory.CreateLock("test_lock");
			result += "1";

			using (var globalMutexInner = factory.TryCreateLock("test_lock", out isLocked)) if (isLocked) result += "2";

			globalMutex.Unlock();

			// Assert
			Assert.AreEqual("1", result);
		}



		[TestMethod]
		public void TestTryLock_TryLock_Maximum60SecondsLockValueIsAllowed()
		{
			// Arrange
			var client = new CloudEnvironment();
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests7"), lockProvider, consoleLog);

			// Act
			using (var globalMutex = factory.CreateLock("test_lock7", TimeSpan.FromSeconds(59)))
			{
			}
			using (var globalMutex = factory.CreateLock("test_lock7", TimeSpan.FromSeconds(60)))
			{
			}

			try
			{
				using (var globalMutex = factory.CreateLock("test_lock7", TimeSpan.FromSeconds(61)))
				{
				}

				// Assert
				Assert.Fail();
			}
			catch
			{
				// Assert
				Assert.IsTrue(true);
			}
		}



		[TestMethod]
		public void TestReacquireLock()
		{
			// Arrange
			var client = new CloudEnvironment();
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests8"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globalmutextests8", "test_lock");
			var globalMutex = factory.CreateLock("test_lock");
			var lease = globalMutex.CurrentLeaseId;
			globalMutex.LockAsync("test_lock").Wait();

			// Assert
			Assert.IsTrue(globalMutex.CurrentLeaseId == lease);
		}



		[TestMethod]
		public void TestReacquireNewLock()
		{
			// Arrange
			var client = new CloudEnvironment();
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests9"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globalmutextests9", "test_lock");
			client.BreakAnyLeases("globalmutextests9", "test_lock_alt");
			var globalMutex = factory.CreateLock("test_lock");
			var lease = globalMutex.CurrentLeaseId;
			globalMutex.LockAsync("test_lock_alt").Wait();

			// Assert II
			Assert.IsTrue(globalMutex.CurrentLeaseId != lease);
		}



		[TestMethod]
		public void TestBrokenLock_Concurrency()
		{
			// Arrange
			var client = new CloudEnvironment();
			var result = string.Empty;
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalMutexFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globalmutextests10"), lockProvider, consoleLog);

			var thread1 = new Thread(
				() =>
				{
					using (var globalMutex = factory.CreateLock("test_lock"))
					{
						result += "1";
						Thread.Sleep(5000);
						client.BreakAnyLeases("globalmutextests10", "test_lock");
					}

					Thread.Sleep(3000);
					result += "3";
				});
			var thread2 = new Thread(
				() =>
				{
					Thread.Sleep(3000); // Ensure it will enter later than the previous method
					using (var globalMutex = factory.CreateLock("test_lock")) result += "2";
				});

			// Act
			client.BreakAnyLeases("globalmutextests10", "test_lock");
			thread2.Start();
			thread1.Start();
			thread1.Join();
			thread2.Join();

			// Assert
			Assert.AreEqual("123", result);
		}
	}
}