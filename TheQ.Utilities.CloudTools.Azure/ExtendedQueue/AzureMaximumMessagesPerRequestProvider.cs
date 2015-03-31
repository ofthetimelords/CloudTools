using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;


namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	public class AzureMaximumMessagesPerRequestProvider : IMaximumMessagesPerRequestProvider
	{
		public int MaximumMessagesPerRequest
		{
			get { return 32; }
		}
	}
}
