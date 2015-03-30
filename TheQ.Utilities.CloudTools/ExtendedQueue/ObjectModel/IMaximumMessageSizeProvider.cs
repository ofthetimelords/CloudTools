using System.Linq;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel
{
	/// <summary>
	/// Provides the maximum amount of a queue's message in bytes. Used to allow for easier IoC scenarios.
	/// </summary>
	public interface IMaximumMessageSizeProvider
	{
		/// <summary>
		/// Gets the maximum size of a message a queue supports.
		/// </summary>
		/// <value>
		/// An integer representing the maximum size of the message.
		/// </value>
		int MaximumMessageSize { get; }
	}
}