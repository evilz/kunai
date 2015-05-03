using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kunai.Cache
{
	public static class CacheExtensions
	{
		public static IEnumerable<T> Cache<T>(this IEnumerable<T> source)
		{
			return CacheHelper(source.GetEnumerator());
		}

		private static IEnumerable<T> CacheHelper<T>(IEnumerator<T> source)
		{
			var isEmpty = new Lazy<bool>(() => !source.MoveNext());
			var head = new Lazy<T>(() => source.Current);
			var tail = new Lazy<IEnumerable<T>>(() => CacheHelper(source));

			return CacheHelper(isEmpty, head, tail);
		}

		private static IEnumerable<T> CacheHelper<T>(
		Lazy<bool> isEmpty,
		Lazy<T> head,
		Lazy<IEnumerable<T>> tail)
		{
			if (isEmpty.Value)
				yield break;

			yield return head.Value;
			foreach (var value in tail.Value)
				yield return value;
		}
	}
}
