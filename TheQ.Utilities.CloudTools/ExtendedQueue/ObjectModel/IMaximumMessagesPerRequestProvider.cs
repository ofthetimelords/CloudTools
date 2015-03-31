namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel
{
	/// <summary>
	/// Provides the maximum amount of messages that can be retrieved at once. Used to allow for easier IoC scenarios.
	/// </summary>
	public interface IMaximumMessagesPerRequestProvider
	{
		/// <summary>
		/// Gets the maximum amount of messages that can be retrieved per request.
		/// </summary>
		/// <value>
		/// An integer specifying the maximum messages that can be retrieved per request.
		/// </value>
		int MaximumMessagesPerRequest { get; }
	}
}