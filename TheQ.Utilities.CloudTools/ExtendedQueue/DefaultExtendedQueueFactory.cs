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

		public IList<Func<ExtendedQueueBase, DecoratorBase>> Decorators { get; private set; }



		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultExtendedQueueFactory" /> class.
		/// </summary>
		/// <param name="messageProvider">An object that will generate a proper instance of <see cref="IQueueMessage" />.</param>
		/// <param name="maximumMessageSizeProvider">An object that will report the maximum size of a message.</param>
		/// <param name="maximumMessagePerRequestProvider">An object that will report the maximum amount of messages that can be retrieved per request.</param>
		/// <param name="overflownMessageHandler">An object that handles messages that are overflown and should be recorded in a temporary storage.</param>
		/// <param name="logService">The logging service to use.</param>
		public DefaultExtendedQueueFactory(
			[NotNull] IQueueMessageProvider messageProvider,
			[NotNull] IMaximumMessageSizeProvider maximumMessageSizeProvider,
			[NotNull] IMaximumMessagesPerRequestProvider maximumMessagePerRequestProvider,
			[NotNull] IOverflownMessageHandler overflownMessageHandler,
			[NotNull] ILogService logService)
		{
			Guard.NotNull(messageProvider, "messageProvider");
			Guard.NotNull(maximumMessageSizeProvider, "maximumMessageSizeProvider");
			Guard.NotNull(maximumMessagePerRequestProvider, "maximumMessagePerRequestProvider");
			Guard.NotNull(overflownMessageHandler, "overflownMessageHandler");
			Guard.NotNull(logService, "logService");


			this.MessageProvider = messageProvider;
			this.MaximumMessageSizeProvider = maximumMessageSizeProvider;
			this.MaximumMessagesProvider = maximumMessagePerRequestProvider;
			this.OverflownMessageHandler = overflownMessageHandler;
			this.LogService = logService;

			this.Decorators = new List<Func<ExtendedQueueBase, DecoratorBase>>
			{
				q => new CompressionDecorator(q),
				q => new JsonSerialiserDecorator(q),
				q => new OverflowHandlingDecorator(q, this.OverflownMessageHandler),
				q => new LoggingDecorator(q, this.LogService)
			};
		}



		/// <summary>
		/// Creates an <see cref="IExtendedQueue" /> instance from a <see cref="IQueue" /> instance.
		/// </summary>
		/// <param name="original">The <see cref="IQueue" /> instance to wrap into an <see cref="IExtendedQueue" /> instance.</param>
		/// <returns>
		/// An <see cref="IExtendedQueue" /> instance backed by the <paramref name="original" /> instance.
		/// </returns>
		public IExtendedQueue Create(IQueue original)
		{

			var baseQueue = new ExtendedQueue(original, this.MessageProvider, this.MaximumMessageSizeProvider, this.MaximumMessagesProvider);

			ExtendedQueueBase decoratedQueue = baseQueue;
			foreach (var action in this.Decorators)
				decoratedQueue = action(decoratedQueue);


			decoratedQueue.LogAction(ExtendedQueueBase.LogSeverity.Info,
				"Created a new IExtendedQueue instance using the DefaultExtendedQueueFactory instance",
				"Queue name: {0}, Actual type of the factory: {1}",
				original.Name,
				this.GetType().FullName);

			return decoratedQueue;
		}
	}
}