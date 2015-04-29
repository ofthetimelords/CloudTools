// <copyright file="LoggingDecorator.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/30</date>
// <summary>
// 
// </summary>

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public class LoggingDecorator : DecoratorBase
	{
		public LoggingDecorator(ExtendedQueueBase decoratedQueue, ILogService logService) : base(decoratedQueue)
		{
			this.LogService = logService ?? new NullLogService();
		}



		private ILogService LogService { get; set; }


		protected internal override string SerializeMessageEntity(object messageEntity)
		{
			return this.LogAction(
				() => this.DecoratedQueue.SerializeMessageEntity(messageEntity),
				"Unexpected exception occurred while adding a message to the queue (during serialisation)");
		}



		protected internal override Stream GetByteEncoder(Stream originalConverter)
		{
			return this.LogAction(
				() => this.DecoratedQueue.GetByteEncoder(originalConverter),
				"Unexpected exception occurred while adding a message to the queue (during the retrieval of a byte encoder)");
		}



		protected internal override Stream GetByteDecoder(Stream originalConverter)
		{
			return this.LogAction(
				() => this.DecoratedQueue.GetByteDecoder(originalConverter),
				"Unexpected exception occurred while retrieving a message from the queue (during the retrieval of a byte decoder)");
		}



		protected internal override byte[] PostProcessMessage(byte[] originalContents)
		{
			return this.LogAction(
				() => this.DecoratedQueue.PostProcessMessage(originalContents),
				"Unexpected exception occurred while adding a message to the queue (during post-processing of the mesasge)");
		}



		protected internal override Task AddNonOverflownMessageAsync(byte[] messageContents, CancellationToken token)
		{
			return this.LogAction(
				() => this.DecoratedQueue.AddNonOverflownMessageAsync(messageContents, token),
				"Unexpected exception occurred while adding a message to the queue (adding the message; not-overflown)");
		}



		protected internal override Task AddOverflownMessageAsync(byte[] messageContents, CancellationToken token)
		{
			return this.LogAction(
				() => this.DecoratedQueue.AddOverflownMessageAsync(messageContents, token), "Unexpected exception occurred while adding a message to the queue (adding the message; overflown)");
		}



		public override Task AddMessageEntityAsync(object entity)
		{
			return this.LogAction(
				() => this.DecoratedQueue.AddMessageEntityAsync(entity), "Unexpected exception occurred while adding a message to the queue.");
		}



		protected internal override Task<byte[]> MessageContentsToByteArray(string serializedContents, ExtendedQueueBase invoker)
		{
			return this.LogAction(
				() => this.DecoratedQueue.MessageContentsToByteArray(serializedContents, invoker),
				"Unexpected exception occurred while adding a message to the queue (converting the message's contents to a byte array)");
		}



		public override Task AddMessageEntityAsync(object entity, CancellationToken token)
		{
			return this.LogAction(
				() => this.DecoratedQueue.AddMessageEntityAsync(entity, token), "Unexpected exception occurred while adding a message to the queue");
		}



		protected internal override Task<string> ByteArrayToSerializedMessageContents(byte[] messageBytes, ExtendedQueueBase invoker)
		{
			return this.LogAction(
				() => this.DecoratedQueue.ByteArrayToSerializedMessageContents(messageBytes, invoker),
				"Unexpected exception occurred while retrieving a message from the queue (converting the the byte array representation of message's contents to a serialized string)");
		}



		public override Task<T> DecodeMessageAsync<T>(QueueMessageWrapper wrapper, CancellationToken token)
		{
			return this.LogAction(
				() => this.DecoratedQueue.DecodeMessageAsync<T>(wrapper, token),
				"Unexpected exception occurred while retrieving a message from the queue (decoding the message)");
		}



		protected internal override T DeserializeToObject<T>(string serializedContents)
		{
			return this.LogAction(
				() => this.DecoratedQueue.DeserializeToObject<T>(serializedContents),
				"Unexpected exception occurred while retrieving a message from the queue (deserializing the message to an object)");
		}



		protected internal override Task<IQueueMessage> GetMessageFromQueue(HandleSerialMessageOptions messageOptions, CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			return this.LogAction(
				() => this.DecoratedQueue.GetMessageFromQueue(messageOptions, messageSpecificCancellationTokenSource),
				"Unexpected exception occurred while retrieving a message from the queue");
		}



		protected internal override Task<byte[]> GetNonOverflownMessageContentsAsync(IQueueMessage message, CancellationToken token)
		{
			return this.LogAction(
				() => this.DecoratedQueue.GetNonOverflownMessageContentsAsync(message, token),
				"");
		}



		protected internal override Task<byte[]> GetOverflownMessageContentsAsync(IQueueMessage message, string id, CancellationToken token)
		{
			return this.LogAction(
				() => this.DecoratedQueue.GetOverflownMessageContentsAsync(message, id, token),
				"");
		}



		protected internal override string GetOverflownMessageId(IQueueMessage message)
		{
			return this.LogAction(
				() => this.DecoratedQueue.GetOverflownMessageId(message),
				"");
		}



		protected internal override void HandleGeneralExceptions(HandleMessageOptionsBase messageOptions, Exception ex, bool parallelYetExternal = false)
		{
			this.LogAction(
				() => this.DecoratedQueue.HandleGeneralExceptions(messageOptions, ex, parallelYetExternal),
				"");
		}

		protected internal override void HandleStorageExceptions(HandleMessageOptionsBase messageOptions, CloudToolsStorageException ex)
		{
			this.LogAction(
				() => this.DecoratedQueue.HandleStorageExceptions(messageOptions, ex),
				"");
		}



		protected internal override bool HandleTaskCancelled(HandleSerialMessageOptions messageOptions)
		{
			return this.LogAction(
				() => this.DecoratedQueue.HandleTaskCancelled(messageOptions),
				"");
		}



		protected internal override Task RemoveOverflownContentsAsync(QueueMessageWrapper message, CancellationToken token)
		{
			return this.LogAction(
				() => this.DecoratedQueue.RemoveOverflownContentsAsync(message, token),
				"");
		}



		protected internal override void SerialFinallyHandler(
			HandleSerialMessageOptions messageOptions,
			Task keepAliveTask,
			IQueueMessage message,
			CancellationTokenSource messageSpecificCancellationTokenSource)
		{
			this.LogAction(
				() => this.DecoratedQueue.SerialFinallyHandler(messageOptions, keepAliveTask, message, messageSpecificCancellationTokenSource),
				"");
		}



		[DebuggerStepThrough]
		private T LogAction<T>(Func<T> action, string message)
		{
			try
			{
				return action();
			}
			catch (CloudToolsStorageException ex)
			{
				this.LogService.QuickLogError("Queue", ex, message);
				throw;
			}
		}



		[DebuggerStepThrough]
		private void LogAction(Action action, string message)
		{
			try
			{
				action();
			}
			catch (CloudToolsStorageException ex)
			{
				this.LogService.QuickLogError("Queue", ex, message);
				throw;
			}
		}
	}
}