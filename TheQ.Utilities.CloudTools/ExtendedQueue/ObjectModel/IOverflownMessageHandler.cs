// <copyright file="IOverflownMessageHandler.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/28</date>
// <summary>
// 
// </summary>

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheQ.Utilities.CloudTools.Storage.Internal;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue
{
	public interface IOverflownMessageHandler
	{
		Task Serialize([NotNull] byte[] originalMessage, string messageId, string queueName, CancellationToken token);
	}
}