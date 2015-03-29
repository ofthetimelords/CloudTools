// <copyright file="CloudToolsStorageException.cs" company="nett">
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
using System.Runtime.Serialization;



namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	/// <summary>
	///     Wraps a storage exception to one that is compatible with CloudTools.
	/// </summary>
	[Serializable]
	public class CloudToolsStorageException : Exception
	{
		/// <summary>
		///     <para>Initializes a new instance of the <see cref="CloudToolsStorageException" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="baseException">The base exception.</param>
		/// <param name="statusCode">The exception's status code.</param>
		/// <param name="errorCode">The specific error code.</param>
		public CloudToolsStorageException(Exception baseException, int? statusCode, string errorCode)
			: base(baseException.Message, baseException)
		{
			this.StatusCode = statusCode;
			this.ErrorCode = errorCode;
		}



		/// <summary>
		///     Initializes a new instance of the <see cref="CloudToolsStorageException" /> class.
		/// </summary>
		public CloudToolsStorageException() { }



		/// <summary>
		///     Initializes a new instance of the <see cref="CloudToolsStorageException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public CloudToolsStorageException(string message)
			: base(message) { }



		/// <summary>
		///     Initializes a new instance of the <see cref="CloudToolsStorageException" /> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">
		///     The exception that is the cause of the current exception, or a <see langword="null" /> reference (Nothing in Visual Basic) if no inner exception is specified.
		/// </param>
		public CloudToolsStorageException(string message, Exception innerException)
			: base(message, innerException) { }



		/// <summary>
		///     <para>Initializes a new instance of the <see cref="CloudToolsStorageException" /></para>
		///     <para>class.</para>
		/// </summary>
		/// <param name="info">
		///     <para>The <see cref="SerializationInfo" /></para>
		///     <para>that holds the serialized object data about the exception being thrown.</para>
		/// </param>
		/// <param name="context">
		///     <para>The <see cref="StreamingContext" /></para>
		///     <para>that contains contextual information about the source or destination.</para>
		/// </param>
		protected CloudToolsStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.StatusCode = info.GetValue("StatusCode", typeof (int?)) as int?;
			this.ErrorCode = info.GetString("ErrorCode");
		}



		/// <summary>
		///     Gets the status code of the exception if valid.
		/// </summary>
		/// <value>
		///     An integer representing a valid status code.
		/// </value>
		public int? StatusCode { get; private set; }


		/// <summary>
		///     Gets the error code.
		/// </summary>
		/// <value>
		///     A code representing the cause of the exception.
		/// </value>
		public string ErrorCode { get; private set; }



		/// <summary>
		///     <para>When overridden in a derived class, sets the <see cref="SerializationInfo" /></para>
		///     <para>with information about the exception.</para>
		/// </summary>
		/// <param name="info">
		///     <para>The <see cref="SerializationInfo" /></para>
		///     <para>that holds the serialized object data about the exception being thrown.</para>
		/// </param>
		/// <param name="context">
		///     <para>The <see cref="StreamingContext" /></para>
		///     <para>that contains contextual information about the source or destination.</para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <para>The <paramref name="info" /></para>
		///     <para>parameter is a <see langword="null" /> reference (Nothing in Visual Basic).</para>
		/// </exception>
		/// <PermissionSet>
		///     <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*"
		///         PathDiscovery="*AllFiles*" />
		///     <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
		/// </PermissionSet>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("StatusCode", this.StatusCode);
			info.AddValue("ErrorCode", this.ErrorCode);

			base.GetObjectData(info, context);
		}



		/// <summary>
		/// Creates and returns a string representation of the current exception.
		/// </summary>
		/// <returns>
		/// A string representation of the current exception.
		/// </returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
		public override string ToString() { return base.ToString() + Environment.NewLine + "[Status Code: " + (this.StatusCode) + "][Error Code:" + this.ErrorCode + "]"; }
	}
}