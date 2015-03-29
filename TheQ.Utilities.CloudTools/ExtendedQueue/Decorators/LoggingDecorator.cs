using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	/// <summary>
	/// Defines the exception handling policy that will be used by the <see cref="LoggingDecorator"/> instance.
	/// </summary>
	public enum ExceptionPolicy
	{
		LogAndThrow = 0,
		LogOnly = 1,
		ThrowOnly = 2
	}

	public class LoggingDecorator : ExtendedQueueBase
	{
		private ExtendedQueueBase DecoratedQueue { get; set; }

		private ILogService LogService { get; set; }

		public ExceptionPolicy PolicyForExceptions { get; set; }



		public LoggingDecorator(ExtendedQueueBase decoratedQueue, ExceptionPolicy policy, ILogService logService)
		{
			this.DecoratedQueue = decoratedQueue;
			this.LogService = logService ?? new NullLogService();
		}



		protected internal override string SerializeMessageEntity(object messageEntity)
		{
			try
			{
				return this.DecoratedQueue.SerializeMessageEntity(messageEntity);
			}
			catch (CloudToolsStorageException ex)
			{
				if (this.PolicyForExceptions != ExceptionPolicy.ThrowOnly)
					this.LogService.QuickLogError("Queue", ex, "Unexpected exception occurred while adding a message to the queue (during serialisation)");

				if (this.PolicyForExceptions != ExceptionPolicy.LogOnly) throw;
			}

			return string.Empty;
		}



		protected internal override Stream GetByteConverter(Stream originalConverter)
		{
			try
			{
				return this.DecoratedQueue.GetByteConverter(originalConverter);
			}
			catch (CloudToolsStorageException ex)
			{
				if (this.PolicyForExceptions != ExceptionPolicy.ThrowOnly)
					this.LogService.QuickLogError("Queue", ex, "Unexpected exception occurred while adding a message to the queue (during the retrieval of a byte converter)");

				if (this.PolicyForExceptions != ExceptionPolicy.LogOnly) throw;
			}

			return Stream.Null;
		}



		protected internal override byte[] PostProcessMessage(byte[] originalContents)
		{
			try
			{
				return this.DecoratedQueue.PostProcessMessage(originalContents);
			}
			catch (CloudToolsStorageException ex)
			{
				if (this.PolicyForExceptions != ExceptionPolicy.ThrowOnly)
					this.LogService.QuickLogError("Queue", ex, "Unexpected exception occurred while adding a message to the queue (during post-processing of the mesasge)");

				if (this.PolicyForExceptions != ExceptionPolicy.LogOnly) throw;
			}

			return null;
		}



		protected internal override Task AddNonOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			try
			{
				return this.DecoratedQueue.AddNonOverflownMessage(messageContents, token);
			}
			catch (CloudToolsStorageException ex)
			{
				if (this.PolicyForExceptions != ExceptionPolicy.ThrowOnly)
					this.LogService.QuickLogError("Queue", ex, "Unexpected exception occurred while adding a message to the queue (adding the message; not-overflown)");

				if (this.PolicyForExceptions != ExceptionPolicy.LogOnly) throw;
			}

			return Task.FromResult(false);
		}



		protected internal override Task AddOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			try
			{
				return this.DecoratedQueue.AddOverflownMessage(messageContents, token);
			}
			catch (CloudToolsStorageException ex)
			{
				if (this.PolicyForExceptions != ExceptionPolicy.ThrowOnly)
					this.LogService.QuickLogError("Queue", ex, "Unexpected exception occurred while adding a message to the queue (adding the message; overflown)");

				if (this.PolicyForExceptions != ExceptionPolicy.LogOnly) throw;
			}

			return Task.FromResult(false);
		}



		public override Task AddMessageEntityAsync(object entity)
		{
			try
			{
				return base.AddMessageEntityAsync(entity);
			}
			catch (CloudToolsStorageException ex)
			{
				if (this.PolicyForExceptions != ExceptionPolicy.ThrowOnly)
					this.LogService.QuickLogError("Queue", ex, "Unexpected exception occurred while adding a message to the queue.");

				if (this.PolicyForExceptions != ExceptionPolicy.LogOnly) throw;
			}

			return Task.FromResult(false);
		}



		protected override Task<byte[]> MessageContentsToByteArray(string serializedContents)
		{
			try
			{
				return base.MessageContentsToByteArray(serializedContents);
			}
			catch (CloudToolsStorageException ex)
			{
				if (this.PolicyForExceptions != ExceptionPolicy.ThrowOnly)
					this.LogService.QuickLogError("Queue", ex, "Unexpected exception occurred while adding a message to the queue (converting the message's contents to a byte array)");

				if (this.PolicyForExceptions != ExceptionPolicy.LogOnly) throw;
			}

			return Task.FromResult<byte[]>(null);
		}
	}
}