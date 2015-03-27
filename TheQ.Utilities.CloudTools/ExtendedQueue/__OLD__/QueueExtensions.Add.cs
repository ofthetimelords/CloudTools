// <copyright file="QueueExtensions.Add.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.Queues
{
	/// <summary>
	///     Provides helper methods for Azure Queues that enable leasing autoupdating.
	/// </summary>
	public static partial class QueueExtensions
	{
		/// <summary>
		///     <para>Adds a message to the queue, by compressing it and automatically expanding it to the <paramref name="overflowContainer" /></para>
		///     <para>BLOB if required.</para>
		/// </summary>
		/// <param name="queue">The queue to add the message to.</param>
		/// <param name="source">The object to be saved in the queue.</param>
		/// <param name="queueMessageProvider">An object that allows the generation of a <see cref="IQueueMessage" /> instance.</param>
		/// <param name="maxMessageSize">Maximum size of the message.</param>
		/// <param name="overflowContainer">The BLOB container that will contain overflown messages.</param>
		/// <returns>
		///     An asynchronous task.
		/// </returns>
		public static async Task AddSafeMessage(
			[NotNull] this IQueue queue,
			[NotNull] object source,
			IQueueMessageProvider queueMessageProvider,
			long maxMessageSize,
			[NotNull] IBlobContainer overflowContainer) { await queue.AddSafeMessage(source, overflowContainer, queueMessageProvider, maxMessageSize, null); }



		/// <summary>
		///     <para>Adds a message to the queue, by compressing it and automatically expanding it to the <paramref name="overflowContainer" /></para>
		///     <para>BLOB if required.</para>
		/// </summary>
		/// <param name="queue">The queue to add the message to.</param>
		/// <param name="source">The object to be saved in the queue.</param>
		/// <param name="overflowContainer">The BLOB container that will contain overflown messages.</param>
		/// <param name="queueMessageProvider">An object that allows the generation of a <see cref="IQueueMessage" /> instance.</param>
		/// <param name="maxMessageSize">Maximum size of the message.</param>
		/// <param name="logService">The logging service to use.</param>
		/// <returns>
		///     An asynchronous task.
		/// </returns>
		public static async Task AddSafeMessage(
			[NotNull] this IQueue queue,
			[NotNull] object source,
			[NotNull] IBlobContainer overflowContainer,
			IQueueMessageProvider queueMessageProvider,
			long maxMessageSize,
			[CanBeNull] ILogService logService)
		{
			Guard.NotNull(queue, "queue");
			Guard.NotNull(source, "source");
			Guard.NotNull(overflowContainer, "overflowContainer");

			//			var maxSize = (IQueueMessage.MaxMessageSize * 3) / 4;
			var maxSize = maxMessageSize*3/4;

			var stringSource = source as string;
			var serialized = stringSource ?? JsonConvert.SerializeObject(source, QueueMessageWrapper.JsonSettings);
			var compressed = await DataCompression.CompressAsync(serialized);

			if (compressed.Length < maxSize)
			{
				logService.QuickLogDebug("SafeAddMessage", "The contents of the message will be stored in the queue itself.");
				await queue.AddMessageAsync(queueMessageProvider.Create(compressed));
			}
			else
			{
				logService.QuickLogDebug(
					"SafeAddMessage",
					"Object '{0}' with ID '{1}' was {2} bytes when serialised and the maximum allowed size is {3} bytes. The contents of the message will be stored in the queue itself.",
					queue.Name,
					source,
					compressed.Length.ToString("N1", CultureInfo.InvariantCulture),
					maxSize.ToString("N1", CultureInfo.InvariantCulture));

				// This ID will link a queue message with an overflow BLOB, since we don't have a message ID yet.
				var id = Guid.NewGuid();
				var blob = overflowContainer.GetBlobReference(string.Format(CultureInfo.InvariantCulture, QueueMessageWrapper.OverflownBlobNameFormat, queue.Name, id));

				await blob.UploadFromByteArrayAsync(compressed, 0, compressed.Length);
				await queue.AddMessageAsync(queueMessageProvider.Create(string.Concat(QueueMessageWrapper.OverflownMessagePrefix, id)));
			}
		}
	}
}