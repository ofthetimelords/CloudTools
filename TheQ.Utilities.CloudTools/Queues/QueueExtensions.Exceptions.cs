// <copyright file="QueueExtensions.Exceptions.cs" company="nett">
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

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.Queues
{
	/// <summary>
	///     Provides helper methods for Azure Queues that enable leasing autoupdating.
	/// </summary>
	public static partial class QueueExtensions
	{
		private static void HandleStorageExceptions([CanBeNull] IQueue queue, [CanBeNull] HandleMessageOptionsBase messageOptions, [CanBeNull] CloudToolsStorageException ex)
		{
			if (Guard.IsAnyNull(queue, messageOptions, ex)) return;

			if (string.Equals(ex.ErrorCode, "MessageNotFound", StringComparison.OrdinalIgnoreCase))
			{
				messageOptions.QuickLogError(
					"HandleMessages",
					ex,
					"A 'Message not Found' error occured while attempting to work on a message (this error should not occur under normal circumstances), on queue '{0}'",
					queue.Name);
			}
			else messageOptions.QuickLogError("HandleMessages", ex, "An error occurred while trying to operate on the queue '{0}'", queue.Name);

			try
			{
				if (messageOptions.ExceptionHandler != null) messageOptions.ExceptionHandler(ex);
			}
			catch (Exception innerEx)
			{
				if (messageOptions.ExceptionHandler != null) messageOptions.QuickLogError("HandleMessages", innerEx, "An error occurred in the custom exception message handler for queue '{0}'", queue.Name);
			}
		}



		private static void HandleGeneralExceptions([CanBeNull] IQueue queue, [CanBeNull] HandleMessageOptionsBase messageOptions, [CanBeNull] Exception ex, bool parallelYetExternal = false)
		{
			if (Guard.IsAnyNull(queue, messageOptions, ex)) return;

			if (parallelYetExternal) messageOptions.QuickLogError("HandleMessages", ex, "An unexpected error occurred outside of a parallel operational loop on queue '{0}'", queue.Name);
			else messageOptions.QuickLogError("HandleMessages", ex, "An error occurred while trying to operate on the queue '{0}'", queue.Name);

			try
			{
				if (messageOptions.ExceptionHandler != null) messageOptions.ExceptionHandler(ex);
			}
			catch (Exception innerEx)
			{
				if (messageOptions.ExceptionHandler != null) messageOptions.QuickLogError("HandleMessages", innerEx, "An error occurred in the custom exception message handler for queue '{0}'", queue.Name);
			}
		}
	}
}