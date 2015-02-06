// <copyright file="LogHelper.cs" company="nett">
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
using System.Globalization;
using System.Linq;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;



namespace TheQ.Utilities.CloudTools.Storage.Infrastructure
{
	public static class LogHelper
	{
		[StringFormatMethod("message")]
		public static void QuickLogDebug([CanBeNull] this HandleMessageOptionsBase options, [NotNull] string category, [NotNull] string message, [NotNull] params object[] formatArguments)
		{
			(options != null && options.LogService != null ? options.LogService : null).QuickLogDebug(category, message, formatArguments);
		}



		[StringFormatMethod("message")]
		public static void QuickLogDebug([CanBeNull] this ILogService logService, [NotNull] string category, [NotNull] string message, [NotNull] params object[] formatArguments)
		{
			if (logService != null) logService.Debug(string.Format(CultureInfo.InvariantCulture, message, formatArguments), "TheQ/CloudTools/" + category);
		}



		[StringFormatMethod("message")]
		public static void QuickLogError(
			[CanBeNull] this HandleMessageOptionsBase options,
			[NotNull] string category,
			[CanBeNull] Exception exception,
			[NotNull] string message,
			[NotNull] params object[] formatArguments)
		{
			(options != null && options.LogService != null ? options.LogService : null).QuickLogError(category, exception, message, formatArguments);
		}



		[StringFormatMethod("message")]
		public static void QuickLogError(
			[CanBeNull] this ILogService logService,
			[NotNull] string category,
			[CanBeNull] Exception exception,
			[NotNull] string message,
			[NotNull] params object[] formatArguments)
		{
			if (logService != null) logService.Error(exception, string.Format(CultureInfo.InvariantCulture, message, formatArguments), "TheQ/CloudTools/" + category);
		}
	}
}