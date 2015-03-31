using System;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;

namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public abstract partial class ExtendedQueueBase
	{
		protected internal virtual void HandleStorageExceptions([CanBeNull] HandleMessageOptionsBase messageOptions, [CanBeNull] CloudToolsStorageException ex)
		{
			if (Guard.IsAnyNull(messageOptions, ex))
				return;

			try
			{
				if (messageOptions.ExceptionHandler != null)
					messageOptions.ExceptionHandler(ex);
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
			if (Guard.IsAnyNull(messageOptions, ex))
				return;

			try
			{
				if (messageOptions.ExceptionHandler != null)
					messageOptions.ExceptionHandler(ex);
			}
			catch (Exception innerEx)
			{
				//if (messageOptions.ExceptionHandler != null) messageOptions.QuickLogError("HandleMessages", innerEx, "An error occurred in the custom exception message handler for queue '{0}'", queue.Name);
			}
		}
	}
}