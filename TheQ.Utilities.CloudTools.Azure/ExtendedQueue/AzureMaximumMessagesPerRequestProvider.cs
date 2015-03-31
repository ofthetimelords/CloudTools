using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;

namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	/// <summary>
	///     Provides the maximum amount of messages that cen be retrieved per request. Used in order to allow for easier IoC scenarios.
	/// </summary>
	public class AzureMaximumMessagesPerRequestProvider : IMaximumMessagesPerRequestProvider
	{
		/// <summary>
		///     Gets the maximum amount of messages that can be retrieved per request.
		/// </summary>
		/// <value>
		///     An integer specifying the maximum messages that can be retrieved per request.
		/// </value>
		public int MaximumMessagesPerRequest
		{
			get { return 32; }
		}
	}
}