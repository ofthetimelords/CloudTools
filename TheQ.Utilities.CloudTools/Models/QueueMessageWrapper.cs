// <copyright file="QueueMessageWrapper.cs" company="nett">
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
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TheQ.Utilities.CloudTools.Storage.Infrastructure;
using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.Models
{
	/// <summary>Represents a message with built-in support for (de)compression and (de)serialisation of messages and automatic handling of overflows.</summary>
	/// <remarks>
	///     The string concatenation strategy for checking whether a message is "overflown" (that is, its actual contents have been expanded to a BLOB) compared to another approach, such as XML
	///     serialisation, was to avoid needless resource wasting, which, compared to the generated strings would be deemed more costly.
	/// </remarks>
	public class QueueMessageWrapper
	{
		/// <summary>
		///     Used as a
		///     <see cref="string.Format(System.IFormatProvider,string,object[])" />
		///     template of the BLOB's name that will contain the overflown queue data.
		/// </summary>
		public const string OverflownBlobNameFormat = "Overflown-{0}-{1}";

		/// <summary>Used as the prefix of an overflown message.</summary>
		public static readonly string OverflownMessagePrefix = "*Overflown*";

		internal static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings {PreserveReferencesHandling = PreserveReferencesHandling.All};
		private readonly string _queueName;
		private object _data;
		private bool _initialized;
		private string _messageContentsRaw;



		/// <summary>
		///     Initializes a new instance of the
		///     <see cref="QueueMessageWrapper" />
		///     class.
		/// </summary>
		/// <param name="queue">The queue the message belongs to.</param>
		/// <param name="message">The underlying message.</param>
		/// <param name="overflowContainer">The BLOB container containing potential data from an overflow of the queue message.</param>
		/// <param name="logService">The logging service to use.</param>
		public QueueMessageWrapper([NotNull] IQueue queue, [NotNull] IQueueMessage message, [NotNull] IBlobContainer overflowContainer, [CanBeNull] ILogService logService)
		{
			Guard.NotNull(queue, "queue");
			Guard.NotNull(message, "message");
			Guard.NotNull(overflowContainer, "overflowContainer");

			this._queueName = queue.Name;
			this.ActualMessage = message;
			this.OverflowContainer = overflowContainer;
			this.LogService = logService;
		}



		/// <summary>
		///     Initializes a new instance of the
		///     <see cref="QueueMessageWrapper" />
		///     class.
		/// </summary>
		/// <param name="queue">The queue the message belongs to.</param>
		/// <param name="message">The underlying message.</param>
		/// <param name="overflowContainer">The BLOB container containing potential data from an overflow of the queue message.</param>
		public QueueMessageWrapper([NotNull] IQueue queue, [NotNull] IQueueMessage message, [NotNull] IBlobContainer overflowContainer)
			: this(queue, message, overflowContainer, null) { }



		/// <summary>Gets or sets the overflow queue message data BLOB container.</summary>
		[NotNull]
		private IBlobContainer OverflowContainer { get; set; }


		/// <summary>Gets or sets the logging service to use.</summary>
		[CanBeNull]
		private ILogService LogService { get; set; }


		/// <summary>
		///     Gets the underlying
		///     <see cref="IQueueMessage" />
		///     instance for raw processing.
		/// </summary>
		/// <value>
		///     The actual
		///     <see cref="IQueueMessage" />
		///     instance.
		/// </value>
		[NotNull]
		public IQueueMessage ActualMessage { get; private set; }


		/// <summary>Gets the message contents as a decompressed JSON string.</summary>
		[NotNull]
		public string MessageContentsRaw
		{
			get
			{
				if (!this._initialized)
				{
					this._messageContentsRaw = this.RetrieveMessageContents().Result;
					this._initialized = true;
				}

				return this._messageContentsRaw;
			}
		}


		/// <summary>Gets a value indicating whether the message's contents were overflown to a BLOB.</summary>
		/// <value>
		///     <c>true</c>
		///     if the message's contents were overflown to a BLOB; otherwise,
		///     <c>false</c>
		///     .
		/// </value>
		public bool WasOverflown { get; private set; }


		/// <summary>
		///     Gets an identifier that links an overflown
		///     <see cref="IQueueMessage" />
		///     with the respective BLOB name, after it gets formatted with
		///     <see cref="OverflownBlobNameFormat" />
		///     .
		/// </summary>
		/// <value>
		///     A unique BLOB-safe identifier (preferrably a proper
		///     <see cref="Guid" />
		///     .
		/// </value>
		[CanBeNull]
		public string OverflowId { get; private set; }



		/// <summary>Lazily attempts to read the message from the queue, retrieve The contents from the BLOB if required</summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetMessageContents<T>()
		{
			if (!this._initialized)
			{
				this.LogService.QuickLogDebug("QueueMessageWrapper", "Attempting to read and initialise the contents of message with ID '{0}' on queue '{1}'", this.ActualMessage.Id, this._queueName);
				if (typeof (T) == typeof (string)) this._data = this.MessageContentsRaw;
				else this._data = JsonConvert.DeserializeObject<T>(this.MessageContentsRaw, JsonSettings);

				this._initialized = true;
			}

			return (T) this._data;
		}



		/// <summary>Retrieves the contents of the message as a string, handling overflown cases if needed.</summary>
		/// <returns>The contents of the message as a string.</returns>
		/// <exception cref="System.InvalidOperationException">Overflown message contents could not be located; was the blob deleted?</exception>
		[NotNull]
		private async Task<string> RetrieveMessageContents()
		{
			try
			{
				this.LogService.QuickLogDebug("QueueMessageWrapper", "Attempting to identify a queue message with ID '{0}' on queue '{1}'", this.ActualMessage.Id, this._queueName);
				var msg = this.ActualMessage.AsBytes;
				var isOverflown = true;

				for (var i = 0; i < OverflownMessagePrefix.Length; i++)
				{
					if (msg[i] != OverflownMessagePrefix[i])
					{
						isOverflown = false;
						break;
					}
				}

				if (isOverflown)
				{
					this.LogService.QuickLogDebug(
						"QueueMessageWrapper",
						"The message with ID '{0}' on queue '{1}' was an overflown message; procedding to decode download data from the respective BLOB",
						this.ActualMessage.Id,
						this._queueName);
					this.WasOverflown = true;
					this.OverflowId = this.ActualMessage.AsString.Replace(OverflownMessagePrefix, string.Empty);
					var oc = this.OverflowContainer;
					var or = oc.GetBlobReference(string.Format(CultureInfo.InvariantCulture, OverflownBlobNameFormat, this._queueName, this.OverflowId));

					if (!await or.ExistsAsync()) throw new InvalidOperationException("Overflown message contents could not be located; was the blob deleted?");

					await or.FetchAttributesAsync();
					var buffer = new byte[or.Properties.Length];
					or.DownloadToByteArray(buffer, 0);

					return await DataCompression.DecompressAsync(buffer);
				}

				this.LogService.QuickLogDebug("QueueMessageWrapper", "Successfully retrieved data for message with ID '{0}' on queue '{1}'. Procedding to decompression", this.ActualMessage.Id, this._queueName);
			}
			catch (DecoderFallbackException)
			{
				this.LogService.QuickLogDebug("QueueMessageWrapper", "The message with ID '{0}' on queue '{1}' was not overflown; procedding to decompress normally", this.ActualMessage.Id, this._queueName);
			}

			return await DataCompression.DecompressAsync(this.ActualMessage.AsBytes);
		}
	}
}