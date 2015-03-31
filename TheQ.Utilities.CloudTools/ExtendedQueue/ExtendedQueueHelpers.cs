#region Using directives
using System.Diagnostics;
#endregion



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	/// <summary>
	///     This method is intented to support the API and is not intented for general use.
	/// </summary>
	internal static class ExtendedQueueHelpers
	{
		/// <summary>
		///     Gets the instance
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="invoker">The invoker.</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static ExtendedQueueBase Get(this ExtendedQueueBase source, ExtendedQueueBase invoker)
		{
			return invoker ?? source;
		}
	}
}