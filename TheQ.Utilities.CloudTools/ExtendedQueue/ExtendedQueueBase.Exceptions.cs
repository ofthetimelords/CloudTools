using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public abstract partial class ExtendedQueueBase
	{
		protected internal virtual void HandleStorageExceptions([CanBeNull] HandleMessageOptionsBase messageOptions, [CanBeNull] CloudToolsStorageException ex)
		{
			if (Guard.IsAnyNull(messageOptions, ex)) return;

			//if (string.Equals(ex.ErrorCode, "MessageNotFound", StringComparison.OrdinalIgnoreCase))
			//{
			//	messageOptions.QuickLogError(
			//		"HandleMessages",
			//		ex,
			//		"A 'Message not Found' error occured while attempting to work on a message (this error should not occur under normal circumstances), on queue '{0}'",
			//		queue.Name);
			//}
			//else messageOptions.QuickLogError("HandleMessages", ex, "An error occurred while trying to operate on the queue '{0}'", queue.Name);

			try
			{
				if (messageOptions.ExceptionHandler != null) messageOptions.ExceptionHandler(ex);
			}
			catch (Exception innerEx)
			{
				//if (messageOptions.ExceptionHandler != null) messageOptions.QuickLogError("HandleMessages", innerEx, "An error occurred in the custom exception message handler for queue '{0}'", queue.Name);
			}
		}



		protected internal virtual void HandleGeneralExceptions(
			[CanBeNull] HandleMessageOptionsBase messageOptions,
			[CanBeNull] Exception ex,
			bool parallelYetExternal = false)
		{
			if (Guard.IsAnyNull( messageOptions, ex)) return;

			//if (parallelYetExternal) messageOptions.QuickLogError("HandleMessages", ex, "An unexpected error occurred outside of a parallel operational loop on queue '{0}'", queue.Name);
			//else messageOptions.QuickLogError("HandleMessages", ex, "An error occurred while trying to operate on the queue '{0}'", queue.Name);

			try
			{
				if (messageOptions.ExceptionHandler != null) messageOptions.ExceptionHandler(ex);
			}
			catch (Exception innerEx)
			{
				//if (messageOptions.ExceptionHandler != null) messageOptions.QuickLogError("HandleMessages", innerEx, "An error occurred in the custom exception message handler for queue '{0}'", queue.Name);
			}
		}
	}
}