// <copyright file="StorageExceptionHelper.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using Microsoft.WindowsAzure.Storage;

using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Azure
{
	/// <summary>
	///     Helper methods for Azure's <see cref="StorageException" /> .
	/// </summary>
	internal static class StorageExceptionHelper
	{
		/// <summary>
		///     Wraps a <see cref="StorageException" /> to a <see cref="CloudToolsStorageException" /> .
		/// </summary>
		/// <param name="sourceException">The source exception to wrap.</param>
		/// <returns>
		///     The exception wrapped as a <see cref="CloudToolsStorageException" /> .
		/// </returns>
		public static CloudToolsStorageException Wrap(this StorageException sourceException)
		{
			if (sourceException == null)
				return null;

			var status = sourceException.RequestInformation != null ? sourceException.RequestInformation.HttpStatusCode : 0;
			var error = sourceException.RequestInformation != null && sourceException.RequestInformation.ExtendedErrorInformation != null
				? sourceException.RequestInformation.ExtendedErrorInformation.ErrorCode
				: null;

			return new CloudToolsStorageException(sourceException, status, error);
		}
	}
}