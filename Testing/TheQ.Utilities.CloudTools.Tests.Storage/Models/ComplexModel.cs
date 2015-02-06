// <copyright file="ComplexModel.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;



namespace TheQ.Utilities.AzureTools.Tests.Storage.Models
{
	internal class ComplexModel
	{
		public ComplexModel()
		{
			this.ADictionary = new ConcurrentDictionary<string, int>();
			this.ADictionary.Add("Test", 1);
			this.AList = new List<int>();
			this.AList.Add(45);
		}



		public string Name { get; set; }


		public IDictionary<string, int> ADictionary { get; private set; }


		public IList<int> AList { get; private set; }
	}
}