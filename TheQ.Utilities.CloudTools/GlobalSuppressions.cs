// <copyright file="GlobalSuppressions.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using System.Diagnostics.CodeAnalysis;
using System.Linq;



[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "TheQ", Scope = "namespace", Target = "TheQ.Utilities.CloudTools.Storage.Models")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "TheQ")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "TheQ", Scope = "namespace", Target = "TheQ.Utilities.CloudTools.Storage.Infrastructure")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "TheQ", Scope = "namespace", Target = "TheQ.Utilities.CloudTools.Storage")]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "HandleMessageOptions", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions.#.ctor(System.TimeSpan,System.TimeSpan,System.TimeSpan,System.Int32,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService,System.Threading.CancellationToken,System.Func`2<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage,System.Boolean>,System.Func`2<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage,System.Boolean>,System.Action`1<System.Exception>)"
		)]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TheQ", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions.#.ctor(System.TimeSpan,System.TimeSpan,System.TimeSpan,System.Int32,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService,System.Threading.CancellationToken,System.Func`2<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage,System.Boolean>,System.Func`2<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage,System.Boolean>,System.Action`1<System.Exception>)"
		)]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CloudTools", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions.#.ctor(System.TimeSpan,System.TimeSpan,System.TimeSpan,System.Int32,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService,System.Threading.CancellationToken,System.Func`2<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage,System.Boolean>,System.Func`2<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage,System.Boolean>,System.Action`1<System.Exception>)"
		)]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Scope = "member",
		Target = "TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService.#Error(System.String,System.Exception,System.String)")]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Scope = "member",
		Target = "TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService.#Error(System.String,System.String)")]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Scope = "member",
		Target = "TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService.#Error(System.Exception,System.String)")]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Scope = "member", Target = "TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryLock(System.String,System.Boolean&)")]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Scope = "member",
		Target = "TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryLock(System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&)")]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "4#", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)")]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)")]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.GlobalMutex.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.QueueExtensions.#ProcessMessageInternal(Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage,Microsoft.WindowsAzure.Storage.Queue.CloudQueue,TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions,System.Threading.CancellationTokenSource,System.Threading.Tasks.Task&)"
		)]
[assembly:
	SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member",
		Target = "TheQ.Utilities.CloudTools.Storage.Infrastructure.DataCompression.#Compress(System.String)")]
[assembly:
	SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member",
		Target = "TheQ.Utilities.CloudTools.Storage.Infrastructure.DataCompression.#Decompress(System.Byte[])")]
[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "GlobalMutexBase{TLockState}.#Dispose(System.Boolean)")]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Boolean&,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)")
]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)")
]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Queues.QueueExtensions.#HandleGeneralExceptions(Microsoft.WindowsAzure.Storage.Queue.CloudQueue,TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions,System.Exception)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Queues.QueueExtensions.#HandleStorageExceptions(Microsoft.WindowsAzure.Storage.Queue.CloudQueue,TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions,StorageException)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "GlobalMutexBase{TLockState}.#Dispose(System.Boolean,System.Boolean)")]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Queues.QueueExtensions.#ProcessMessageInternal(TheQ.Utilities.CloudTools.Storage.Models.QueueMessageWrapper,Microsoft.WindowsAzure.Storage.Queue.CloudQueue,TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptions,System.Threading.CancellationTokenSource,System.Threading.Tasks.Task&)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Queues.QueueExtensions.#HandleGeneralExceptions(Microsoft.WindowsAzure.Storage.Queue.CloudQueue,TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptionsBase,System.Exception)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Queues.QueueExtensions.#HandleStorageExceptions(Microsoft.WindowsAzure.Storage.Queue.CloudQueue,TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptionsBase,StorageException)"
		)]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "TheQ", Scope = "namespace", Target = "TheQ.Utilities.CloudTools.Storage.Queues")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "TheQ", Scope = "namespace", Target = "TheQ.Utilities.CloudTools.Storage.Blob")]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target = "GlobalMutexBase{TLockState}.#CreateRenewalThreadIfApplicable(System.String,System.Nullable`1<System.TimeSpan>)")]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "TheQ", Scope = "namespace", Target = "TheQ.Utilities.CloudTools.Storage.Models.ObjectModel")]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Queues.QueueExtensions.#HandleGeneralExceptions(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IQueue,TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptionsBase,System.Exception,System.Boolean)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Queues.QueueExtensions.#HandleStorageExceptions(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IQueue,TheQ.Utilities.CloudTools.Storage.Models.HandleMessageOptionsBase,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.CloudToolsStorageException)"
		)]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "NumberLess", Scope = "member",
		Target = "TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition.#IfSequenceNumberLessThanOrEqual")]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "NumberLess", Scope = "member",
		Target = "TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition.#IfSequenceNumberLessThan")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IQueue")]
[assembly:
	SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.MessageUpdateFieldsEnum")]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.CloudTools.Storage.Infrastructure.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
		Target =
			"TheQ.Utilities.CloudTools.Storage.Queues.QueueExtensions.#ProcessMessageInternalBatch(System.Collections.Generic.IList`1<TheQ.Utilities.CloudTools.Storage.Models.QueueMessageWrapper>,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IQueue,System.Threading.Tasks.Task&,System.Threading.CancellationTokenSource,TheQ.Utilities.CloudTools.Storage.Models.HandleBatchMessageOptions)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#TryCreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Boolean&,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLockAsync(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Threading.CancellationToken,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]
[assembly:
	SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member",
		Target =
			"GlobalMutexBase{TLockState}.#CreateLock(TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IBlobContainer,System.String,System.Nullable`1<System.TimeSpan>,System.Func`2<System.String,TheQ.Utilities.CloudTools.Storage.Models.ObjectModel.IAccessCondition>,TheQ.Utilities.Shared.ILogService)"
		)]