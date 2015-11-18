// <copyright file="JsonSerialiserDecorator.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/03/29</date>
// <summary>
// 
// </summary>

using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	/// <summary>
	/// An <see cref="ExtendedQueueBase"/> decorator that adds JSON serialisation capabilities.
	/// </summary>
	public class JsonSerialiserDecorator : DecoratorBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonSerialiserDecorator"/> class.
		/// </summary>
		/// <param name="decoratedQueue">The queue to decorate.</param>
		public JsonSerialiserDecorator(ExtendedQueueBase decoratedQueue) : base(decoratedQueue)
		{ }



		/// <summary>
		/// Serializes a message entity to string.
		/// </summary>
		/// <param name="messageEntity">The message entity to serialise.</param>
		/// <returns>A string representation of the entity.</returns>
		protected internal override Task<string> SerializeMessageEntity(object messageEntity)
		{
			this.Top.LogAction(LogSeverity.Debug, "Calling JsonSerialiserDecorator.SerializeMessageEntity");
			return Task.FromResult(JsonConvert.SerializeObject(messageEntity, Formatting.None));
		}


		protected internal override T DeserializeToObject<T>(string serializedContents)
		{
			this.Top.LogAction(LogSeverity.Debug, "Calling JsonSerialiserDecorator.DeserializeToObject");
			return JsonConvert.DeserializeObject<T>(serializedContents);
		}
	}
}