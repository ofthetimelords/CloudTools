// <copyright file="GlobalLockTests.cs" company="nett">
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheQ.Utilities.CloudTools.Azure;
using TheQ.Utilities.CloudTools.Azure.GlobalLockFramework;



namespace TheQ.Utilities.CloudTools.Tests.Storage.ConcurrencyTests
{
	[TestClass]
	public class GlobalLockTests
	{
		[TestMethod]
		public void TestLock_Lock_Concurrency()
		{
			// Arrange
			var client = new CloudEnvironment();
			var result = string.Empty;
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests1"), lockProvider, consoleLog);

			var thread1 = new Thread(
				() =>
				{
					using (var globalLock = factory.CreateLock("test_lock"))
					{
						result += "1";

						Thread.Sleep(5000);
					}
				});
			var thread2 = new Thread(
				() =>
				{
					Thread.Sleep(3000); // Ensure it will enter later than the previous method
					using (var globalLock = factory.CreateLock("test_lock")) result += "2";
				});

			// Act
			client.BreakAnyLeases("globallocktests1", "test_lock");
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
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests2"), lockProvider, consoleLog);
			var result = string.Empty;
			var thread1 = new Thread(
				() =>
				{
					bool isLocked;
					using (var globalLock = factory.TryCreateLock("test_lock", out isLocked))
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
					using (var globalLock = factory.TryCreateLock("test_lock", out isLocked)) if (isLocked) result += "2";
				});

			// Act
			client.BreakAnyLeases("globallocktests2", "test_lock");
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
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests3"), lockProvider, consoleLog);


			// Act
			client.BreakAnyLeases("globallocktests3", "test_lock");
			using (var globalLock = factory.CreateLock("test_lock")) result += "1";

			using (var globalLock = factory.CreateLock("test_lock")) result += "2";

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
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests4"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globallocktests4", "test_lock");
			using (var globalLock = factory.TryCreateLock("test_lock", TimeSpan.FromSeconds(10), out isLocked))
			{
				if (isLocked) result += "1";

				Thread.Sleep(TimeSpan.FromSeconds(35));
				using (var globalLockInner = factory.TryCreateLock("test_lock", out isLockedInner)) if (isLockedInner) result += "2";
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
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests5"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globallocktests5", "test_lock");
			using (var globalLock = factory.TryCreateLock("test_lock", out isLocked))
			{
				if (isLocked) result += "1";

				using (var globalLockInner = factory.TryCreateLock("test_lock", out isLockedInner)) if (isLockedInner) result += "2";
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
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests6"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globallocktests6", "test_lock");
			var globalLock = factory.CreateLock("test_lock");
			result += "1";

			using (var globalLockInner = factory.TryCreateLock("test_lock", out isLocked)) if (isLocked) result += "2";

			globalLock.Unlock();

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
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests7"), lockProvider, consoleLog);

			// Act
			using (var globalLock = factory.CreateLock("test_lock7", TimeSpan.FromSeconds(59)))
			{
			}
			using (var globalLock = factory.CreateLock("test_lock7", TimeSpan.FromSeconds(60)))
			{
			}

			try
			{
				using (var globalLock = factory.CreateLock("test_lock7", TimeSpan.FromSeconds(61)))
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
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests8"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globallocktests8", "test_lock");
			var globalLock = factory.CreateLock("test_lock");
			var lease = globalLock.CurrentLeaseId;
			globalLock.LockAsync("test_lock").Wait();

			// Assert
			Assert.IsTrue(globalLock.CurrentLeaseId == lease);
		}



		[TestMethod]
		public void TestReacquireNewLock()
		{
			// Arrange
			var client = new CloudEnvironment();
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests9"), lockProvider, consoleLog);

			// Act
			client.BreakAnyLeases("globallocktests9", "test_lock");
			client.BreakAnyLeases("globallocktests9", "test_lock_alt");
			var globalLock = factory.CreateLock("test_lock");
			var lease = globalLock.CurrentLeaseId;
			globalLock.LockAsync("test_lock_alt").Wait();

			// Assert II
			Assert.IsTrue(globalLock.CurrentLeaseId != lease);
		}



		[TestMethod]
		public void TestBrokenLock_Concurrency()
		{
			// Arrange
			var client = new CloudEnvironment();
			var result = string.Empty;
			var consoleLog = new ConsoleLogService();
			var lockProvider = new AzureLockStateProvider(consoleLog);
			var factory = new AzureGlobalLockFactory((AzureBlobContainer) client.BlobClient.GetContainerReference("globallocktests10"), lockProvider, consoleLog);

			var thread1 = new Thread(
				() =>
				{
					using (var globalLock = factory.CreateLock("test_lock"))
					{
						result += "1";
						Thread.Sleep(5000);
						client.BreakAnyLeases("globallocktests10", "test_lock");
					}

					Thread.Sleep(3000);
					result += "3";
				});
			var thread2 = new Thread(
				() =>
				{
					Thread.Sleep(3000); // Ensure it will enter later than the previous method
					using (var globalLock = factory.CreateLock("test_lock")) result += "2";
				});

			// Act
			client.BreakAnyLeases("globallocktests10", "test_lock");
			thread2.Start();
			thread1.Start();
			thread1.Join();
			thread2.Join();

			// Assert
			Assert.AreEqual("123", result);
		}
	}
}