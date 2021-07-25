using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.DTO
{
	public class ListOfIds<T>
	{
		public List<T> Ids { get; set; } = new List<T>();
		public ListOfIds() { }
		public ListOfIds(List<T> list)
		{
			Ids = list;
		}
		
		public NameValueCollection GetNameValue(string itemName)
		{
			NameValueCollection collection = new NameValueCollection();
			foreach (var item in Ids)
			{
				collection.Add(itemName, item.ToString());

			}
			return collection;
		}
	}
}
