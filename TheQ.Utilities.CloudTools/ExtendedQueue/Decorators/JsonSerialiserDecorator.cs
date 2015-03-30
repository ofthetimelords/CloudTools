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

using Newtonsoft.Json;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.Decorators
{
	public class JsonSerialiserDecorator : DecoratorBase
	{
		public JsonSerialiserDecorator(ExtendedQueueBase decoratedQueue) : base(decoratedQueue) { }


		protected internal override string SerializeMessageEntity(object messageEntity) { return JsonConvert.SerializeObject(messageEntity, Formatting.None); }


		protected internal override T DeserializeToObject<T>(string serializedContents) { return JsonConvert.DeserializeObject<T>(serializedContents); }
	}
}