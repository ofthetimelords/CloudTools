using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	/// <summary>
	///     An <see cref="ExtendedQueueBase" /> decorator that adds compression capabilities.
	/// </summary>
	public class CompressionDecorator : DecoratorBase
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CompressionDecorator" /> class.
		/// </summary>
		/// <param name="decoratedQueue">The queue to decorate.</param>
		public CompressionDecorator(ExtendedQueueBase decoratedQueue) : base(decoratedQueue)
		{
		}



		protected internal override Task<Stream> GetByteEncoder(Stream originalConverter)
		{
			this.LogAction(LogSeverity.Debug, "Calling CompressionDecorator.GetByteEncoder");
			return Task.FromResult<Stream>(new DeflateStream(originalConverter, CompressionMode.Compress, true));
		}



		protected internal override Task<Stream> GetByteDecoder(Stream originalConverter)
		{
			this.LogAction(LogSeverity.Debug, "Calling CompressionDecorator.GetByteDecoder");
			return Task.FromResult<Stream>(new DeflateStream(originalConverter, CompressionMode.Decompress, true));
		}
	}
}