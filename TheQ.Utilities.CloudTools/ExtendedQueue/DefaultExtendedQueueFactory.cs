using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators;
using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	/// <summary>
	/// A factory that wraps existing <see cref="IQueue"/> instances to an <see cref="IExtendedQueue"/> instance using the default decorators.
	/// </summary>
	public class DefaultExtendedQueueFactory : IExtendedQueueFactory
	{
		private IQueueMessageProvider MessageProvider { get; set; }


		private IMaximumMessageSizeProvider MaximumMessageSizeProvider { get; set; }


		private IMaximumMessagesPerRequestProvider MaximumMessagesProvider { get; set; }


		private IOverflownMessageHandler OverflownMessageHandler { get; set; }


		private ILogService LogService { get; set; }

		private ExceptionPolicy Policy { get; set; }



		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultExtendedQueueFactory"/> class.
		/// </summary>
		/// <param name="policy">The exception policy that will be used by the logging decorator.</param>
		/// <param name="messageProvider">An object that will generate a proper instance of <see cref="IQueueMessage"/>.</param>
		/// <param name="maximumMessageSizeProvider">An object that will report the maximum size of a message.</param>
		/// <param name="overflownMessageHandler">An object that handles messages that are overflown and should be recorded in a temporary storage.</param>
		/// <param name="logService">The logging service to use.</param>
		public DefaultExtendedQueueFactory(
			ExceptionPolicy policy,
			IQueueMessageProvider messageProvider,
			IMaximumMessageSizeProvider maximumMessageSizeProvider,
			IMaximumMessagesPerRequestProvider maximumMessagePerRequestProvider,
			IOverflownMessageHandler overflownMessageHandler,
			ILogService logService)
		{
			Guard.NotNull(messageProvider, "messageProvider");
			Guard.NotNull(maximumMessageSizeProvider, "maximumMessageSizeProvider");
			Guard.NotNull(overflownMessageHandler, "overflownMessageHandler");
			Guard.NotNull(logService, "logService");


			this.MessageProvider = messageProvider;
			this.MaximumMessageSizeProvider = maximumMessageSizeProvider;
			this.MaximumMessagesProvider = maximumMessagePerRequestProvider;
			this.OverflownMessageHandler = overflownMessageHandler;
			this.LogService = logService;
			this.Policy = policy;
		}



		public IExtendedQueue Create(IQueue original)
		{
			var baseQueue = new ExtendedQueue(original, this.MessageProvider, this.MaximumMessageSizeProvider, this.MaximumMessagesProvider);

			var compressibleQueue = new CompressionDecorator(baseQueue);
			var jsonQueue = new JsonSerialiserDecorator(compressibleQueue);
			var overflowQueue = new OverflowHandlingDecorator(jsonQueue, this.OverflownMessageHandler);
			var loggedQueue = new LoggingDecorator(overflowQueue, this.Policy, this.LogService);

			return loggedQueue;
		}
	}
}