// <copyright file="LogHelper.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/31</date>
// <summary>
// 
// </summary>

using System;
using System.Globalization;
using System.Linq;

using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Storage.Infrastructure
{
	/// <summary>
	///     Logging helpers.
	/// </summary>
	internal static class LogHelper
	{
		/// <summary>
		///     Logs a debug operation.
		/// </summary>
		/// <param name="logService">The logging service to use.</param>
		/// <param name="category">The log category.</param>
		/// <param name="message">The log message.</param>
		/// <param name="formatArguments">The arguments passed into <paramref name="message" />.</param>
		[StringFormatMethod("message")]
		public static void QuickLogDebug([CanBeNull] this ILogService logService, [NotNull] string category, [NotNull] string message, [NotNull] params object[] formatArguments)
		{
			if (logService != null) logService.Debug(string.Format(CultureInfo.InvariantCulture, message, formatArguments), "TheQ.CloudTools." + category);
		}



		/// <summary>
		///     Logs an error.
		/// </summary>
		/// <param name="logService">The logging service.</param>
		/// <param name="category">The log category.</param>
		/// <param name="exception">The exception that occurred.</param>
		/// <param name="message">The log message.</param>
		/// <param name="formatArguments">The arguments passed into <paramref name="message" />.</param>
		[StringFormatMethod("message")]
		public static void QuickLogError(
			[CanBeNull] this ILogService logService,
			[NotNull] string category,
			[CanBeNull] Exception exception,
			[NotNull] string message,
			[NotNull] params object[] formatArguments)
		{
			if (logService != null && exception != null) logService.Error(exception, "TheQ.CloudTools." + category, string.Format(CultureInfo.InvariantCulture, message, formatArguments));
		}
	}
}