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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	/// <summary>
	/// Provides logging capabilities to an ExtendedQueue instance.
	/// </summary>
	public class LoggingDecorator : DecoratorBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingDecorator"/> class.
		/// </summary>
		/// <param name="decoratedQueue">The <see cref="ExtendedQueueBase"/> instance to decorate.</param>
		/// <param name="logService">The logging service to use.</param>
		public LoggingDecorator(ExtendedQueueBase decoratedQueue, ILogService logService) : base(decoratedQueue)
		{
			this.LogService = logService ?? new NullLogService();
		}



		private ILogService LogService { get; set; }


		protected internal override void LogException(LogSeverity severity, Exception exception, string details = null, params string[] formatArguments)
		{
			switch (severity)
			{
				case LogSeverity.Critical:
					this.LogService.Critical(exception, "TheQ.CloudTools.ExtendedQueue", string.Format(CultureInfo.InvariantCulture, details, formatArguments));
					break;
				case LogSeverity.Error:
					this.LogService.Error(exception, "TheQ.CloudTools.ExtendedQueue", string.Format(CultureInfo.InvariantCulture, details, formatArguments));
					break;
				default:
					this.LogService.Debug(exception.Message, "TheQ.CloudTools.ExtendedQueue", string.Format(CultureInfo.InvariantCulture, details, formatArguments));
					break;
			}
		}



		protected internal override void LogAction(LogSeverity severity, string message = null, string details = null, params string[] formatArguments)
		{
			switch (severity)
			{
				case LogSeverity.Error:
					this.LogService.Error(message, "TheQ.CloudTools.ExtendedQueue", string.Format(CultureInfo.InvariantCulture, details, formatArguments));
					break;
				case LogSeverity.Warning:
					this.LogService.Warning(message, "TheQ.CloudTools.ExtendedQueue", string.Format(CultureInfo.InvariantCulture, details, formatArguments));
					break;
				case LogSeverity.Info:
					this.LogService.Info(message, "TheQ.CloudTools.ExtendedQueue", string.Format(CultureInfo.InvariantCulture, details, formatArguments));
					break;
				default:
					this.LogService.Debug(message, "TheQ.CloudTools.ExtendedQueue", string.Format(CultureInfo.InvariantCulture, details, formatArguments));
					break;
			}
		}
	}
}