using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	/// <summary>
	/// Provides the maximum amount of an Azure queue's message in bytes. Used in order to allow for easier IoC scenarios.
	/// </summary>
	public class AzureMaximumMessageSizeProvider : IMaximumMessageSizeProvider
	{
		/// <summary>
		/// Gets the maximum size of a message an Azure queue supports. Currently hard-code to 64K.
		/// </summary>
		/// <value>
		/// An integer representing the maximum size of the message.
		/// </value>
		/// <remarks>The size of 65K is multiplied by 3/4, which is the size of a message after it's been Base64 encoded.</remarks>
		public int MaximumMessageSize
		{
			get { return 65535 * 3 / 4; }
		}
	}
}
