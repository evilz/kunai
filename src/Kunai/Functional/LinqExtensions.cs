using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Kunai.Functional;

/// <summary>
/// Additional LINQ-style extension methods for sequences and collections.
/// </summary>
public static class LinqExtensions
{
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
