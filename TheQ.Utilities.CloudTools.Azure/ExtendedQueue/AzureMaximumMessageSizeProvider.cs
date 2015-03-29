using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	public class AzureMaximumMessageSizeProvider : IMaximumMessageSizeProvider
	{
		public int MaximumMessageSize
		{
			get { return 65535; }
		}
	}
}
