using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public interface IExtendedQueue : IQueue
	{
		Task AddMessageEntityAsync([NotNull] object entity);


		Task AddMessageEntityAsync([NotNull] object entity, CancellationToken token);


		void AddMessageEntity([NotNull] object entity);



		Task HandleMessagesInBatchAsync([NotNull] HandleBatchMessageOptions messageOptions);


		Task HandleMessagesInParallelAsync([NotNull] HandleParallelMessageOptions messageOptions);


		Task HandleMessagesAsync([NotNull] HandleSerialMessageOptions messageOptions);
	}
}