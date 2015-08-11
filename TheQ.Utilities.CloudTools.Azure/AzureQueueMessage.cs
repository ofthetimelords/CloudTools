// <copyright file="AzureQueueMessage.cs" company="nett">
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
using System.Security.Policy;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     An implementation of <see cref="IQueueMessage" /> for Windows Azure.
	/// </summary>
	public class AzureQueueMessage : IQueueMessage
	{
		private readonly CloudQueueMessage _queueMessageReference;



		/// <summary>
		///     Initializes a new instance of the <see cref="AzureQueueMessage" /> class.
		/// </summary>
		/// <param name="message">The original <see cref="CloudQueueMessage" /> instance.</param>
		public AzureQueueMessage(CloudQueueMessage message)
		{
			Guard.NotNull(message, "message");

			this._queueMessageReference = message;
		}



		/// <summary>
		///     Gets the content of the message as a <see langword="byte" /> array.
		/// </summary>
		/// <value>
		///     The content of the message as a <see langword="byte" /> array.
		/// </value>
		public byte[] AsBytes
		{
			get
			{
				try
				{
					return this._queueMessageReference.AsBytes;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets the message ID.
		/// </summary>
		/// <value>
		///     A string containing the message ID.
		/// </value>
		public string Id
		{
			get
			{
				try
				{
					return this._queueMessageReference.Id;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets the message's pop receipt.
		/// </summary>
		/// <value>
		///     A string containing the pop receipt value.
		/// </value>
		public string PopReceipt
		{
			get
			{
				try
				{
					return this._queueMessageReference.PopReceipt;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets the time that the message was added to the queue.
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>indicating the time that the message was added to the queue.</para>
		/// </value>
		public DateTimeOffset? InsertionTime
		{
			get
			{
				try
				{
					return this._queueMessageReference.InsertionTime;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets the time that the message expires.
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>indicating the time that the message expires.</para>
		/// </value>
		public DateTimeOffset? ExpirationTime
		{
			get
			{
				try
				{
					return this._queueMessageReference.ExpirationTime;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets the time that the message will next be visible.
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>indicating the time that the message will next be visible.</para>
		/// </value>
		public DateTimeOffset? NextVisibleTime
		{
			get
			{
				try
				{
					return this._queueMessageReference.NextVisibleTime;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets the content of the message, as a string.
		/// </summary>
		/// <value>
		///     A string containing the message content.
		/// </value>
		public string AsString
		{
			get
			{
				try
				{
					return this._queueMessageReference.AsString;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}


		/// <summary>
		///     Gets the number of times this message has been dequeued.
		/// </summary>
		/// <value>
		///     The number of times this message has been dequeued.
		/// </value>
		public int DequeueCount
		{
			get
			{
				try
				{
					return this._queueMessageReference.DequeueCount;
				}
				catch (StorageException ex)
				{
					throw ex.Wrap();
				}
			}
		}



		/// <summary>
		///     Sets the <paramref name="content" /> of this message.
		/// </summary>
		/// <param name="content">The content of the message as a <see langword="byte" /> array.</param>
		public void SetMessageContent(byte[] content)
		{
			try
			{
				this._queueMessageReference.SetMessageContent(content);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Sets the <paramref name="content" /> of this message.
		/// </summary>
		/// <param name="content">A string containing the new message content.</param>
		public void SetMessageContent(string content)
		{
			try
			{
				this._queueMessageReference.SetMessageContent(content);
			}
			catch (StorageException ex)
			{
				throw ex.Wrap();
			}
		}



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="AzureQueueMessage" /> to <see cref="CloudQueueMessage" /> .
		/// </summary>
		/// <param name="message">The <see cref="AzureQueueMessage" /> .</param>
		/// <returns>
		///     The underlying <see cref="CloudQueueMessage" /> .
		/// </returns>
		public static implicit operator CloudQueueMessage(AzureQueueMessage message)
		{
			return message != null ? message._queueMessageReference : null;
		}



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="CloudQueueMessage" /> to <see cref="AzureQueueMessage" /> .
		/// </summary>
		/// <param name="message">The <see cref="CloudQueueMessage" /> instance.</param>
		/// <returns>
		///     A <see cref="AzureQueueMessage" /> wrapper.
		/// </returns>
		public static implicit operator AzureQueueMessage(CloudQueueMessage message) { return message != null ? new AzureQueueMessage(message) : null; }



		/// <summary>
		///     Creates an <see cref="AzureQueueMessage"/> from a <see cref="CloudQueueMessage" /> instance.
		/// </summary>
		/// <param name="message">The <see cref="CloudQueueMessage" /> instance.</param>
		/// <returns>
		///     A <see cref="AzureQueueMessage" /> wrapper.
		/// </returns>
		public static AzureQueueMessage FromCloudQueueMessage(CloudQueueMessage message)
		{
			return message;
		}



		/// <summary>
		///     Retrieves the underlying <see cref="CloudQueueMessage" /> instance from this <see cref="AzureQueueMessage"/> instance.
		/// </summary>
		/// <param name="message">The underlying <see cref="CloudQueueMessage" /> instance.</param>
		/// <returns>
		///     An <see cref="AzureQueueMessage" /> wrapper.
		/// </returns>
		public static CloudQueueMessage ToCloudQueueMessage(AzureQueueMessage message)
		{
			return message;
		}
	}
}