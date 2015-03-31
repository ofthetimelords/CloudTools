using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure.ExtendedQueue
{
	public class AzureExtendedQueueFactory : DefaultExtendedQueueFactory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AzureExtendedQueueFactory"/> class.
		/// </summary>
		/// <param name="policy">The exception policy that will be used by the logging decorator.</param>
		/// <param name="overflowContainer">The overflow container.</param>
		/// <param name="logService">The logging service to use.</param>
		public AzureExtendedQueueFactory(
			IBlobContainer overflowContainer,
			ILogService logService)
			: base(
				new AzureQueueMessageProvider(),
				new AzureMaximumMessageSizeProvider(),
				new AzureMaximumMessagesPerRequestProvider(),
				new AzureBlobOverflownMessageHandler(overflowContainer),
				logService)
		{
		}
	}
}