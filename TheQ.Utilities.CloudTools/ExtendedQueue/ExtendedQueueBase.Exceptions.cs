using System;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;

namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public abstract partial class ExtendedQueueBase
	{
		/// <summary>
		/// Defines a handler for storage related exceptions
		/// </summary>
		/// <param name="messageOptions">The options of this extended queue.</param>
		/// <param name="exception">The <see cref="CloudToolsStorageException"/> that occurred.</param>
		protected internal virtual void HandleStorageExceptions([CanBeNull] HandleMessagesOptionsBase messageOptions, [CanBeNull] CloudToolsStorageException exception)
		{
			if (Guard.IsAnyNull(messageOptions, exception))
				return;

			try
			{
				if (messageOptions.ExceptionHandler != null)
				{
					messageOptions.ExceptionHandler(exception);
					this.LogException(LogSeverity.Info, exception, "An unexpected storage exception occurred while processing messages on queue '{0}' and was handled", this.Name);
				}
				{
					this.LogException(LogSeverity.Warning, exception, "An unexpected storage exception occurred while processing messages on queue '{0}' but was not handled!", this.Name);
				}
			}
			catch (Exception innerEx)
			{
				this.LogException(LogSeverity.Error, innerEx, "An unexpected exception occurred within the storage exception handler on queue '{0}'", this.Name);
			}
		}



		protected internal virtual void HandleGeneralExceptions([CanBeNull] HandleMessagesOptionsBase messageOptions, [CanBeNull] Exception exception)
		{
			if (Guard.IsAnyNull(messageOptions, exception))
				return;

			try
			{
				if (messageOptions.ExceptionHandler != null)
				{
					messageOptions.ExceptionHandler(exception);
					this.LogException(LogSeverity.Info, exception, "An unexpected exception occurred while processing messages on queue '{0}' and was handled", this.Name);
				}
				else
				{
					this.LogException(LogSeverity.Error, exception, "An unexpected exception occurred while processing messages on queue '{0}' but was not handled!", this.Name);
				}

			}
			catch (Exception innerEx)
			{
				this.LogException(LogSeverity.Error, innerEx, "An unexpected exception occurred within the general exception handler on queue '{0}'", this.Name);
			}
		}
	}
}