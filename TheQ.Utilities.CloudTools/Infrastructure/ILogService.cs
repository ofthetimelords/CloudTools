﻿// <copyright file="ILogService.cs" company="nett">
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
using System.Linq;

using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Storage.Infrastructure
{
	/// <summary>
	///     Defines an essential logging service.
	/// </summary>
	public interface ILogService
	{
		/// <summary>
		///     Logs a critical error.
		/// </summary>
		/// <param name="exception">The exception to retrieve the error from.</param>
		/// <param name="details">The error's additional details.</param>
		/// <param name="category">The error's category.</param>
		void Critical([NotNull] Exception exception, [CanBeNull] string category = null, [CanBeNull] string details = null);



		/// <summary>
		///     Logs an error.
		/// </summary>
		/// <param name="message">The error's message.</param>
		/// <param name="details">The error's additional details.</param>
		/// <param name="category">The error's category.</param>
		void Error([NotNull] string message, [CanBeNull] string category = null, [CanBeNull] string details = null);



		/// <summary>
		///     Logs an error, from an exception.
		/// </summary>
		/// <param name="exception">The exception to retrieve the error from.</param>
		/// <param name="details">Additioanl details about the error.</param>
		/// <param name="category">The error's category.</param>
		void Error([NotNull] Exception exception, [CanBeNull] string category = null, [CanBeNull] string details = null);



		/// <summary>
		///     Logs a warning.
		/// </summary>
		/// <param name="message">The warning's message.</param>
		/// <param name="details">The warning's additional details.</param>
		/// <param name="category">The warning's category.</param>
		void Warning([NotNull] string message, [CanBeNull] string category = null, [CanBeNull] string details = null);



		/// <summary>
		///     Logs an informational message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="details">The message's additional details.</param>
		/// <param name="category">The message's category.</param>
		void Info([NotNull] string message, [CanBeNull] string category = null, [CanBeNull] string details = null);



		/// <summary>
		///     Logs a debug message.
		/// </summary>
		/// <param name="message">The debug message.</param>
		/// <param name="details">The message's additional details.</param>
		/// <param name="category">The message's category.</param>
		void Debug([NotNull] string message, [CanBeNull] string category = null, [CanBeNull] string details = null);
	}
}