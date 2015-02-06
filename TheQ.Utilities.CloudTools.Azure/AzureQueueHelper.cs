// <copyright file="AzureQueueHelper.cs" company="nett">
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
using System.Linq;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models;
using TheQ.Utilities.CloudTools.Storage.Queues;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     Provides helper methods for Azure Queues that makes working with CloudTools easier.
	/// </summary>
	public static class AzureQueueHelper
	{
		/// <summary>
		///     Use the same <see cref="AzureQueueMessageProvider" /> instance.
		/// </summary>
		private static readonly AzureQueueMessageProvider AzureQueueMessageProviderInstance = new AzureQueueMessageProvider();



		/// <summary>
		///     <para>Adds a message to the queue, by compressing it and automatically expanding it to the <paramref name="overflowContainer" /></para>
		///     <para>BLOB if required.</para>
		/// </summary>
		/// <param name="queue">The queue to add the message to.</param>
		/// <param name="source">The object to be saved in the queue.</param>
		/// <param name="overflowContainer">The BLOB container that will contain overflown messages.</param>
		[NotNull]
		public static Task AddSafeMessage([NotNull] this CloudQueue queue, [NotNull] object source, [NotNull] CloudBlobContainer overflowContainer)
		{
			return queue.AddSafeMessage(source, overflowContainer, null);
		}



		/// <summary>
		///     <para>Adds a message to the queue, by compressing it and automatically expanding it to the <paramref name="overflowContainer" /></para>
		///     <para>BLOB if required.</para>
		/// </summary>
		/// <param name="queue">The queue to add the message to.</param>
		/// <param name="source">The object to be saved in the queue.</param>
		/// <param name="overflowContainer">The BLOB container that will contain overflown messages.</param>
		/// <param name="logService">The logging service to use.</param>
		[NotNull]
		public static async Task AddSafeMessage([NotNull] this CloudQueue queue, [NotNull] object source, [NotNull] CloudBlobContainer overflowContainer, [CanBeNull] ILogService logService)
		{
			try
			{
				await ((AzureQueue) queue).AddSafeMessage(source, (AzureBlobContainer) overflowContainer, AzureQueueMessageProviderInstance, CloudQueueMessage.MaxMessageSize, logService);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Handles messages from the <paramref name="queue" /> in a serial manner, in an endless loop.
		/// </summary>
		/// <param name="queue">The queue to check for messages.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <exception cref="ArgumentNullException">The messageOps parameter is null.</exception>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>.</para>
		/// </returns>
		public static async Task HandleMessagesInBatch([NotNull] this CloudQueue queue, [NotNull] HandleBatchMessageOptions messageOptions)
		{
			try
			{
				await ((AzureQueue) queue).HandleMessagesInBatch(messageOptions);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Handles messages from the <paramref name="queue" /> in a parallel manner, in an endless loop.
		/// </summary>
		/// <param name="queue">The queue to check for messages.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <exception cref="ArgumentNullException">queue;Parameter 'queue' was null. or messageOptions;Parameter 'messageOptions' was not provided.</exception>
		/// <exception cref="ArgumentNullException">The messageOps parameter is null.</exception>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>.</para>
		/// </returns>
		public static async Task HandleMessagesInParallel([NotNull] this CloudQueue queue, [NotNull] HandleParallelMessageOptions messageOptions)
		{
			try
			{
				await ((AzureQueue) queue).HandleMessagesInParallel(messageOptions);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Handles messages from the <paramref name="queue" /> in a serial manner, in an endless loop.
		/// </summary>
		/// <param name="queue">The queue to check for messages.</param>
		/// <param name="messageOptions">Initialisation options for this method.</param>
		/// <exception cref="ArgumentNullException">The messageOps parameter is null.</exception>
		/// <returns>
		///     <para>A <see cref="Task" /></para>
		///     <para>.</para>
		/// </returns>
		public static async Task HandleMessages([NotNull] this CloudQueue queue, [NotNull] HandleSerialMessageOptions messageOptions)
		{
			try
			{
				await ((AzureQueue) queue).HandleMessages(messageOptions);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}
	}
}