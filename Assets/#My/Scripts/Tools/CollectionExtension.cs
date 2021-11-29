using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSouls.Tools
{
	public static class CollectionExtension
	{
		/// <summary>
		/// Returns a random element from a collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static T Random<T>(this ICollection<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("The passed collection was null.");
			else if (collection.Count == 0)
				throw new IndexOutOfRangeException("The collection was empty.");
			return collection.ElementAt(new Random().Next(0, collection.Count));
		}
	}
}
