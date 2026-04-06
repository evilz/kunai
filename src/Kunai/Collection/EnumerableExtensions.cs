using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kunai.CollectionExt;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/> and related collection types.
/// </summary>
public static class EnumerableExtensions
{
	private static void Swap(ref char a, ref char b)
	{
		a ^= b;
		b ^= a;
		a ^= b;
	}

	/// <summary>
	/// Creates an enumerable of characters from <paramref name="first"/> to <paramref name="last"/> (inclusive).
	/// Supports both ascending and descending ranges.
	/// </summary>
	/// <param name="first">The starting character.</param>
	/// <param name="last">The ending character.</param>
	/// <returns>An enumerable of characters between <paramref name="first"/> and <paramref name="last"/>.</returns>
	public static IEnumerable<char> To(this char first, char last)
	{
		bool reverseRequired = (first > last);

		if (reverseRequired)
			Swap(ref first, ref last);

		var result = Enumerable.Range(first, last - first + 1).Select(charCode => (char)charCode);

		if (reverseRequired)
			result = result.Reverse();

		return result;
	}

	/// <summary>
	///     Applies an Action <typeparamref name="T"/> to all elements
	///     of an array.
	/// </summary>
	/// <typeparam name="T">
	///     Type of elements in the array
	/// </typeparam>
	/// <param name="elements">
	///     Array of elements
	/// </param>
	/// <param name="action">
	///     The <see cref="Action{TProperty}"/> to be performed in all
	///     elements.
	/// </param>
	public static void Each<T>(this IEnumerable<T> elements, Action<T> action)
	{
		if (elements == null) return;

		var cached = elements;

		foreach (var item in cached)
			action(item);
	}

	/// <summary>
	/// Returns indices of all elements in the sequence that equal <paramref name="value"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="obj">The source sequence.</param>
	/// <param name="value">The value to search for.</param>
	/// <returns>An enumerable of zero-based indices of matching elements.</returns>
	public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> obj, T value)
	{
		return (from i in Enumerable.Range(0, obj.Count())
				where obj.ElementAt(i).Equals(value)
				select i);
	}

	/// <summary>
	/// Returns indices of all elements in the sequence that are contained in <paramref name="value"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="obj">The source sequence.</param>
	/// <param name="value">The values to search for.</param>
	/// <returns>An enumerable of zero-based indices of matching elements.</returns>
	public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> obj, IEnumerable<T> value)
	{
		return (from i in Enumerable.Range(0, obj.Count())
				where value.Contains(obj.ElementAt(i))
				select i);
	}

	/// <summary>
	/// Returns a sub-sequence of elements from index <paramref name="start"/> (inclusive)
	/// to index <paramref name="end"/> (exclusive).
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="collection">The source collection.</param>
	/// <param name="start">The zero-based start index (inclusive).</param>
	/// <param name="end">The zero-based end index (exclusive).</param>
	/// <returns>A sub-sequence of the collection.</returns>
	public static IEnumerable<T> Slice<T>(this IEnumerable<T> collection, int start, int end) =>
		collection.Skip(start).Take(end - start);


	public static T GetRandomItem<T>(this IList<T> input)
	{
		var rand = new Random();
		if (input != null)
		{
			if (input.Count == 1)
				return input[0];

			int n = rand.Next(input.Count() + 1);

			return input[n];
		}
		return (T)input;
	}


	
	// CHECK THIS
	public static IEnumerable<T> RandomElements<T>(this IEnumerable<T> collection, int count = 0)
	{
		if (count > collection.Count() || count <= 0)
			count = collection.Count();

		List<int> usedIndices = new List<int>();
		Random random = new Random((int)DateTime.Now.Ticks);
		while (count > 0)
		{
			int index = random.Next(collection.Count());
			if (!usedIndices.Contains(index))
			{
				yield return collection.ElementAt(index);
				usedIndices.Add(index);
				count--;
			}
		}
	}


	private static Random random = new Random();

	public static T SelectRandom<T>(this IEnumerable<T> sequence)
	{
		if (sequence == null)
		{
			throw new ArgumentNullException();
		}

		if (!sequence.Any())
		{
			throw new ArgumentException("The sequence is empty.");
		}

		//optimization for ICollection<T>
		if (sequence is ICollection<T>)
		{
			ICollection<T> col = (ICollection<T>)sequence;
			return col.ElementAt(random.Next(col.Count));
		}

		int count = 1;
		T selected = default;

		foreach (T element in sequence)
		{
			if (random.Next(count++) == 0)
			{
				//Select the current element with 1/count probability
				selected = element;
			}
		}

		return selected;
	}


	// TODO : REFECTO !
	public static string ToHtmlTable<T>(this IEnumerable<T> list, string tableSyle = "table table-bordered")
	{

		var result = new StringBuilder();
		
			result.Append("<table id=\"" + typeof(T).Name + "Table\" class=\"" + tableSyle + "\">");
		
		var propertyArray = typeof(T).GetProperties();
		foreach (var prop in propertyArray)
		{
			result.AppendFormat("<th>{0}</th>", prop.Name);
			
		}

		for (int i = 0; i < list.Count(); i++)
		{
			
				result.AppendFormat("<tr>");
			
			foreach (var prop in propertyArray)
			{
				object value = prop.GetValue(list.ElementAt(i), null);
				result.AppendFormat("<td>{0}</td>", value ?? String.Empty);
			}
			result.AppendLine("</tr>");
		}
		result.Append("</table>");
		return result.ToString();
	}

	/// <summary>
	/// Returns <see langword="true"/> if the sequence is <see langword="null"/> or contains no elements.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="source">The sequence to check.</param>
	/// <returns><see langword="true"/> if <paramref name="source"/> is null or empty; otherwise <see langword="false"/>.</returns>
	public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) =>
		source == null || !source.Any();

	/// <summary>
	/// Removes duplicate elements from the collection based on a key produced by <paramref name="Predicate"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="list">The source collection.</param>
	/// <param name="Predicate">A function that returns an integer key for each element.</param>
	/// <returns>A sequence of elements with duplicate keys removed.</returns>
	public static IEnumerable<T> RemoveDuplicates<T>(this ICollection<T> list, Func<T, int> Predicate)
	{
		var dict = new Dictionary<int, T>();

		foreach (var item in list)
		{
			if (!dict.ContainsKey(Predicate(item)))
			{
				dict.Add(Predicate(item), item);
			}
		}

		return dict.Values.AsEnumerable();
	}

	/// <summary>
	/// Continues processing items in a collection until the end condition is true.
	/// </summary>
	/// <typeparam name="T">The type of the collection.</typeparam>
	/// <param name="collection">The collection to iterate.</param>
	/// <param name="endCondition">The condition that returns true if iteration should stop.</param>
	/// <returns>Iterator of sub-list.</returns>
	public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> collection, Predicate<T> endCondition) =>
		collection.TakeWhile(item => !endCondition(item));

	/// <summary>
	/// Adds an element to the list and returns the list for fluent chaining.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="list">The list to add to.</param>
	/// <param name="item">The item to add.</param>
	/// <returns>The list after adding the item.</returns>
	public static IList<T> AddElement<T>(this IList<T> list, T item)
	{
		list.Add(item);
		return list;
	}

	/// <summary>
	/// Conditionally adds an element to the list and returns the list for fluent chaining.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="list">The list to add to.</param>
	/// <param name="condition">If <see langword="true"/>, the item is added.</param>
	/// <param name="item">The item to conditionally add.</param>
	/// <returns>The list after optionally adding the item.</returns>
	public static IList<T> AddElementIf<T>(this IList<T> list, bool condition, T item)
	{
		if (condition)
		{
			list.Add(item);
		}

		return list;
	}

	/// <summary>
	/// Adds a range of elements to the list and returns the list for fluent chaining.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="list">The list to add to.</param>
	/// <param name="items">The items to add.</param>
	/// <returns>The list after adding all items.</returns>
	public static IList<T> AddElementRange<T>(this IList<T> list, IEnumerable<T> items)
	{
		items.Each(list.Add);
		return list;
	}

	/// <summary>
	/// Conditionally adds a range of elements to the list and returns the list for fluent chaining.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="list">The list to add to.</param>
	/// <param name="condition">If <see langword="true"/>, the items are added.</param>
	/// <param name="items">The items to conditionally add.</param>
	/// <returns>The list after optionally adding the items.</returns>
	public static IList<T> AddElementRangeIf<T>(this IList<T> list, bool condition, IEnumerable<T> items)
	{
		if (condition)
		{
			list.AddElementRange(items);
		}
		return list;
	}


	// TEST this !!
	public static void SetAllValues<T>(this IEnumerable<T> source, T value)
	{
		source.Each(obj => obj = value);
	}

	/// <summary>
	/// Converts an <see cref="IEnumerator{T}"/> to an <see cref="IEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="enumerator">The enumerator to convert.</param>
	/// <returns>An enumerable that iterates the enumerator.</returns>
	public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
	{
		while (enumerator.MoveNext())
			yield return enumerator.Current;
	}


	/// <summary>
	///   Returns all combinations of a chosen amount of selected elements in the sequence.
	/// </summary>
	/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
	/// <param name = "source">The source for this extension method.</param>
	/// <param name = "select">The amount of elements to select for every combination.</param>
	/// <param name = "repetition">True when repetition of elements is allowed.</param>
	/// <returns>All combinations of a chosen amount of selected elements in the sequence.</returns>
	public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int select, bool repetition = false)
	{
		

		return select == 0
			? new[] { new T[0] }
			: source.SelectMany((element, index) =>
			   source
				   .Skip(repetition ? index : index + 1)
				   .Combinations(select - 1, repetition)
				   .Select(c => new[] { element }.Concat(c)));
	}


	/// <summary>
	/// Concatenates a specified separator String between each element of a specified enumeration, yielding a single concatenated string.
	/// </summary>
	/// <typeparam name="T">any object</typeparam>
	/// <param name="list">The enumeration</param>
	/// <param name="separator">A String</param>
	/// <returns>A String consisting of the elements of value interspersed with the separator string.</returns>
	public static string ToString<T>(this IEnumerable<T> list, string separator)
	{
		StringBuilder sb = new StringBuilder();
		foreach (var obj in list)
		{
			if (sb.Length > 0)
			{
				sb.Append(separator);
			}
			sb.Append(obj);
		}
		return sb.ToString();
	}


	/// <summary>
	/// transposes the rows and columns of its argument
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="values"></param>
	/// <returns></returns>
	public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> values)
	{
		if (values.Count() == 0)
			return values;
		if (values.First().Count() == 0)
			return Transpose(values.Skip(1));

		var x = values.First().First();
		var xs = values.First().Skip(1);
		var xss = values.Skip(1);
		return
		 new[] {new[] {x}
	   .Concat(xss.Select(ht => ht.First()))}
		   .Concat(new[] { xs }
		   .Concat(xss.Select(ht => ht.Skip(1)))
		   .Transpose());
	}


	/// <summary>
	/// Adds a bulk range of items to an <see cref="ObservableCollection{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="oc">The observable collection to add items to.</param>
	/// <param name="collection">The items to add.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> is null.</exception>
	public static void AddRange<T>(this ObservableCollection<T> oc, IEnumerable<T> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException(nameof(collection));
		}
		foreach (var item in collection)
		{
			oc.Add(item);
		}


	}
}
