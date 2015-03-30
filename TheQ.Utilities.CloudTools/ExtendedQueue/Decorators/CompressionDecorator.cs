using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	/// <summary>
	/// An <see cref="ExtendedQueueBase"/> decorator that adds compression capabilities.
	/// </summary>
	public class CompressionDecorator : DecoratorBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="CompressionDecorator"/> class.
		/// </summary>
		/// <param name="decoratedQueue">The decorated queue.</param>
		public CompressionDecorator(ExtendedQueueBase decoratedQueue) :base(decoratedQueue) { }

		protected internal override Stream GetByteEncoder(Stream originalConverter) { return new DeflateStream(originalConverter, CompressionMode.Compress, true); }

		protected internal override Stream GetByteDecoder(Stream originalConverter) { return new DeflateStream(originalConverter, CompressionMode.Decompress, true); }
	}
}