using System;
using System.Collections.Generic;
using System.Linq;

namespace Kunai.Cache;

/// <summary>
/// Extension methods for caching enumerables.
/// </summary>
public static class CacheExtensions
{
	/// <summary>
	/// Lazily caches the elements of an enumerable so that the source is only iterated once.
	/// Subsequent iterations use the cached values.
	/// </summary>
	/// <typeparam name="T">The type of elements in the sequence.</typeparam>
	/// <param name="source">The source enumerable to cache.</param>
	/// <returns>A new enumerable that caches elements as they are first requested.</returns>
	public static IEnumerable<T> Cache<T>(this IEnumerable<T> source) =>
		CacheHelper(source.GetEnumerator());

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
