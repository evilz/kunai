using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Kunai.Functional
{
	public static class LinqExtensions
	{
		//	public static ObjectQuery<T> Include<T, T2>(this ObjectQuery<T> data, Expression<Func<T, ICollection<T2>>> property1, Expression<Func<T2, object>> property2)
		//		where T : class
		//		where T2 : class
		//	{
		//		var name1 = (property1.Body as MemberExpression).Member.Name;
		//		var name2 = (property2.Body as MemberExpression).Member.Name;

		//		return data.Include(name1 + "." + name2);
		//	}

		//	public static ObjectQuery<T> Include<T, T2>(this ObjectQuery<T> data, Expression<Func<T, T2>> property1, Expression<Func<T2, object>> property2) where T : class
		//	{
		//		var name1 = (property1.Body as MemberExpression).Member.Name;
		//		var name2 = (property2.Body as MemberExpression).Member.Name;

		//		return data.Include(name1 + "." + name2);
		//	}

		//	public static ObjectQuery<T> Include<T>(this ObjectQuery<T> data, Expression<Func<T, object>> property) where T : class
		//	{
		//		var name = (property.Body as MemberExpression).Member.Name;

		//		return data.Include(name);
		//	}


		public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> @this, Func<T, TKey> keySelector)
		{
			return @this.GroupBy(keySelector).Select(grps => grps).Select(e => e.First());
		}

		/// <summary>
		/// Converts an enumeration of groupings into a Dictionary of those groupings.
		/// </summary>
		/// <typeparam name="TKey">Key type of the grouping and dictionary.</typeparam>
		/// <typeparam name="TValue">Element type of the grouping and dictionary list.</typeparam>
		/// <param name="groupings">The enumeration of groupings from a GroupBy() clause.</param>
		/// <returns>A dictionary of groupings such that the key of the dictionary is TKey type and the value is List of TValue type.</returns>
		public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
		{
			return groupings.ToDictionary(group => group.Key, group => group.ToList());
		}


		public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
		{
			var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

			var l = source.ToLookup(firstKeySelector);
			foreach (var item in l)
			{
				var dict = new Dictionary<TSecondKey, TValue>();
				retVal.Add(item.Key, dict);
				var subdict = item.ToLookup(secondKeySelector);
				foreach (var subitem in subdict)
				{
					dict.Add(subitem.Key, aggregate(subitem));
				}
			}

			return retVal;
		}

		public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortExpression)
		{
			sortExpression += "";
			string[] parts = sortExpression.Split(' ');
			bool descending = false;
			string property = "";

			if (parts.Length > 0 && parts[0] != "")
			{
				property = parts[0];

				if (parts.Length > 1)
				{
					descending = parts[1].ToLower().Contains("esc");
				}

				PropertyInfo prop = typeof(T).GetProperty(property);

				if (prop == null)
				{
					throw new Exception("No property '" + property + "' in + " + typeof(T).Name + "'");
				}

				if (descending)
					return list.OrderByDescending(x => prop.GetValue(x, null));
				else
					return list.OrderBy(x => prop.GetValue(x, null));
			}

			return list;
		}
	}
}
