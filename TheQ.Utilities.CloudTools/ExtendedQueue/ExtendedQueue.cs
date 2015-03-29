﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel;
using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	/// <summary>
	/// A wrapper around an <see cref="IQueue"/> implementation that provides extended functionality (through decoration).
	/// </summary>
	public class ExtendedQueue : ExtendedQueueBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedQueue"/> class.
		/// </summary>
		/// <param name="original">The original queue to be wrapped.</param>
		/// <exception cref="ArgumentNullException">The parameter was null: + <paramref name="original" /></exception>
		public ExtendedQueue([NotNull] IQueue original, IQueueMessageProvider messageProvider, IMaximumMessageSizeProvider maximumMessageSizeProvider)
		{
			Guard.NotNull(original, "original");
			Guard.NotNull(messageProvider, "messageProvider");

			this.OriginalQueue = original;
			this.MessageProvider = messageProvider;
			this.MaximumMessageProvider = maximumMessageSizeProvider;
		}

		protected internal override string SerializeMessageEntity(object messageEntity)
		{
			// Not a preferrable method!
			using (var ms = new MemoryStream())
			{
				new BinaryFormatter().Serialize(ms, messageEntity);
				return Convert.ToBase64String(ms.ToArray());
			}
		}



		protected internal override Stream GetByteConverter(Stream originalConverter) { return originalConverter; }



		protected internal override byte[] PostProcessMessage(byte[] originalContents) { return originalContents; }



		protected internal override Task AddNonOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			return (this as IQueue).AddMessageAsync(this.MessageProvider.Create(messageContents), token);
		}



		/// <summary>
		/// Adds the overflown message.
		/// </summary>
		/// <param name="messageContents">The message contents.</param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">Message wouldn't fit in the Queue.</exception>
		protected internal override async Task AddOverflownMessage(byte[] messageContents, CancellationToken token)
		{
			throw new ArgumentException("Message wouldn't fit in the Queue. The size of the message was " + messageContents.Length + " bytes", "messageContents");
		}
	}
}