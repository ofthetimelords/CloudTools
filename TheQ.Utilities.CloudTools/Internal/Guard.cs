// <copyright file="Guard.cs" company="nett">
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



namespace TheQ.Utilities.CloudTools.Storage.Internal
{
	/// <summary>
	///     Helper methods for common data checks.
	/// </summary>
	public static class Guard
	{
		/// <summary>
		///     <para>Ensures that a parameter is not null, otherwise throws an <see cref="ArgumentException" /></para>
		///     <para>.</para>
		/// </summary>
		/// <param name="argument">The parameter to check for null.</param>
		/// <param name="name">The name of the parameter (used to display on the xception).</param>
		/// <exception cref="ArgumentNullException">The parameter was null: + <paramref name="name" /></exception>
		[DebuggerStepThrough]
		public static void NotNull([CanBeNull] object argument, [NotNull] [InvokerParameterName] string name)
		{
			if (argument == null) throw new ArgumentNullException(name, "A parameter was null: " + name);
		}



		/// <summary>
		///     <para>Ensures that a string parameter is not <see langword="null" /> or a whitespace, otherwise throws an <see cref="ArgumentException" /></para>
		///     <para>.</para>
		/// </summary>
		/// <param name="argument">The parameter to check for <see langword="null" /> & whitespace-only.</param>
		/// <param name="name">The name of the parameter (used to display on the xception).</param>
		/// <exception cref="ArgumentNullException">The parameter was null: + <paramref name="name" /></exception>
		[DebuggerStepThrough]
		public static void NotNull([CanBeNull] string argument, [NotNull] [InvokerParameterName] string name)
		{
			if (string.IsNullOrWhiteSpace(argument)) throw new ArgumentNullException(name, "A parameter was null or an empty string: " + name);
		}



		/// <summary>
		///     Checks if any of the provided objects are not <see langword="null" /> and returns the result for use in if statements. Allows for exiting a method without a thrown exception.
		/// </summary>
		/// <param name="instances">The objects to check for null.</param>
		/// <returns>
		///     True if no object is null, <see langword="false" /> if at least one is.
		/// </returns>
		[DebuggerStepThrough]
		public static bool IsAnyNull([CanBeNull] params object[] instances)
		{
			return instances == null || instances.Any(o => o == null);
		}



		/// <summary>
		///     <para>Ensures that a string parameter is not <see langword="null" /> (but allows empty or whitespace strings), otherwise throws an <see cref="ArgumentException" /></para>
		///     <para>.</para>
		/// </summary>
		/// <param name="argument">The parameter to check for null.</param>
		/// <param name="name">The name of the parameter (used to display on the xception).</param>
		/// <exception cref="ArgumentNullException">The parameter was null: + <paramref name="name" /></exception>
		[DebuggerStepThrough]
		public static void NotNullOnly([CanBeNull] string argument, [NotNull] [InvokerParameterName] string name)
		{
			if (argument == null) throw new ArgumentNullException(name, "A parameter was null: " + name);
		}
	}
}