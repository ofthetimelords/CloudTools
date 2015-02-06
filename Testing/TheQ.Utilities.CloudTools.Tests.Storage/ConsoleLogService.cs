// <copyright file="ConsoleLogService.cs" company="nett">
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
using System.Diagnostics;
using System.Linq;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;



namespace TheQ.Utilities.CloudTools.Tests.Storage
{
	internal class ConsoleLogService : ILogService
	{
		/// <summary>
		///     Logs a critical error.
		/// </summary>
		/// <param name="message">The error's message.</param>
		/// <param name="details">The error's additional details.</param>
		/// <param name="category">The error's category.</param>
		public void Critical(Exception exception, string category = null, string details = null) { Trace.WriteLine("Error: " + exception.Message + Environment.NewLine, category); }



		/// <summary>
		///     Registers a <paramref name="message" /> to the log as an error message.
		/// </summary>
		/// <param name="message">The message to register.</param>
		/// <param name="category">The category this <paramref name="message" /> belongs to (to differentiate the source of messages).</param>
		/// <returns>
		///     True if the <paramref name="message" /> was registered successfully; <see langword="false" /> if an error occurred.
		/// </returns>
		public void Error(string message, string category = null, string details = null) { Trace.WriteLine("Error: " + message + Environment.NewLine, category); }



		/// <summary>
		///     Registers an <paramref name="exception" /> to the log as an error message.
		/// </summary>
		/// <param name="exception">The exception to register.</param>
		/// <param name="category">The category this message belongs to (to differentiate the source of messages).</param>
		/// <returns>
		///     True if the message was registered successfully; <see langword="false" /> if an error occurred.
		/// </returns>
		public void Error(Exception exception, string category = null, string details = null) { Trace.WriteLine("Error: " + exception + Environment.NewLine, category); }



		/// <summary>
		///     Logs a warning.
		/// </summary>
		/// <param name="message">The warning's message.</param>
		/// <param name="category">The warning's category.</param>
		/// <param name="details">The warning's additional details.</param>
		public void Warning(string message, string category = null, string details = null) { Trace.WriteLine("Warning: " + message + Environment.NewLine, category); }



		/// <summary>
		///     Registers a <paramref name="message" /> to the log as an informational message.
		/// </summary>
		/// <param name="message">The message to register.</param>
		/// <param name="category">The category this <paramref name="message" /> belongs to (to differentiate the source of messages).</param>
		/// <returns>
		///     True if the <paramref name="message" /> was registered successfully; <see langword="false" /> if an error occurred.
		/// </returns>
		public void Info(string message, string category = null, string details = null) { Trace.WriteLine("Info: " + message + Environment.NewLine, category); }



		/// <summary>
		///     Registers a debug <paramref name="message" /> to the log as an informational message.
		/// </summary>
		/// <param name="message">The message to register.</param>
		/// <param name="category">The category this <paramref name="message" /> belongs to (to differentiate the source of messages).</param>
		/// <returns>
		///     True if the <paramref name="message" /> was registered successfully; <see langword="false" /> if an error occurred.
		/// </returns>
		public void Debug(string message, string category = null, string details = null) { Trace.WriteLine("Debug: " + message + Environment.NewLine, category); }



		/// <summary>
		///     Registers an <paramref name="exception" /> with a custom error <paramref name="message" /> to the log.
		/// </summary>
		/// <param name="message">The message to register.</param>
		/// <param name="exception">The exception to register.</param>
		/// <param name="category">The category this <paramref name="message" /> belongs to (to differentiate the source of messages).</param>
		/// <returns>
		///     True if the <paramref name="message" /> was registered successfully; <see langword="false" /> if an error occurred.
		/// </returns>
		public void Error(string message, Exception exception, string category) { Trace.WriteLine("Error: " + message + " | " + exception + Environment.NewLine, category); }
	}
}