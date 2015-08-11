// <copyright file="IQueueMesssage.cs" company="nett">
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



namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	/// <summary>
	///     Defines a message from a queue, with the minimum required implementation for CloudTools.
	/// </summary>
	public interface IQueueMessage
	{
		/// <summary>
		///     Gets the content of the message as a <see langword="byte" /> array.
		/// </summary>
		/// <value>
		///     The content of the message as a <see langword="byte" /> array.
		/// </value>
		byte[] AsBytes { get; }


		/// <summary>
		///     Gets the message ID.
		/// </summary>
		/// <value>
		///     A string containing the message ID.
		/// </value>
		string Id { get; }


		/// <summary>
		///     Gets the message's pop receipt.
		/// </summary>
		/// <value>
		///     A string containing the pop receipt value.
		/// </value>
		string PopReceipt { get; }


		/// <summary>
		///     Gets the time that the message was added to the queue.
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>indicating the time that the message was added to the queue.</para>
		/// </value>
		DateTimeOffset? InsertionTime { get; }


		/// <summary>
		///     Gets the time that the message expires.
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>indicating the time that the message expires.</para>
		/// </value>
		DateTimeOffset? ExpirationTime { get; }


		/// <summary>
		///     Gets the time that the message will next be visible.
		/// </summary>
		/// <value>
		///     <para>A <see cref="DateTimeOffset" /></para>
		///     <para>indicating the time that the message will next be visible.</para>
		/// </value>
		DateTimeOffset? NextVisibleTime { get; }


		/// <summary>
		///     Gets the content of the message, as a string.
		/// </summary>
		/// <value>
		///     A string containing the message content.
		/// </value>
		string AsString { get; }


		/// <summary>
		///     Gets the number of times this message has been dequeued.
		/// </summary>
		/// <value>
		///     The number of times this message has been dequeued.
		/// </value>
		int DequeueCount { get; }



		/// <summary>
		///     Sets the <paramref name="content" /> of this message.
		/// </summary>
		/// <param name="content">The content of the message as a <see langword="byte" /> array.</param>
		void SetMessageContent(byte[] content);



		/// <summary>
		///     Sets the <paramref name="content" /> of this message.
		/// </summary>
		/// <param name="content">A string containing the new message content.</param>
		void SetMessageContent(string content);
	}
}