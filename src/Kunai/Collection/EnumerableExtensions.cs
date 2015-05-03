using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Kunai.CollectionExt
{
	public static class EnumerableExtensions
	{
		private static void Swap(ref char a, ref char b)
		{
			a ^= b;
			b ^= a;
			a ^= b;
		}

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

		public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> obj, T value)
		{
			return (from i in Enumerable.Range(0, obj.Count())
					where obj.ElementAt(i).Equals(value)
					select i);
		}

		public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> obj, IEnumerable<T> value)
		{
			return (from i in Enumerable.Range(0, obj.Count())
					where value.Contains(obj.ElementAt(i))
					select i);
		}

		public static IEnumerable<T> Slice<T>(this IEnumerable<T> collection, int start, int end)
		{
			return collection.Skip(start).Take(end - start);
		}


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
			T selected = default(T);

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


		// CHECK THIS !
		public static T[] Shuffle<T>(this T[] list)
		{
			var r = new Random((int)DateTime.Now.Ticks);
			for (int i = list.Length - 1; i > 0; i--)
			{
				int j = r.Next(0, i - 1);
				var e = list[i];
				list[i] = list[j];
				list[j] = e;
			}
			return list;
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

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || !source.Any();
		}

		public static T Last<T>(this IList<T> list)
		{
			return list[list.Count - 1];
		}

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

		public static IEnumerable<T> Reverse<T>(this IEnumerable<T> items)
		{
			var enumerable = items as IList<T> ?? items.ToList();
			for (int i = enumerable.Count() - 1; i >= 0; i--)
			{
				yield return enumerable[i];
			}
		}

		/// <summary>
		/// Continues processing items in a collection until the end condition is true.
		/// </summary>
		/// <typeparam name="T">The type of the collection.</typeparam>
		/// <param name="collection">The collection to iterate.</param>
		/// <param name="endCondition">The condition that returns true if iteration should stop.</param>
		/// <returns>Iterator of sub-list.</returns>
		public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> collection, Predicate<T> endCondition)
		{
			return collection.TakeWhile(item => !endCondition(item));
		}

		public static IList<T> AddElement<T>(this IList<T> list, T item)
		{
			list.Add(item);
			return list;
		}

		public static IList<T> AddElementIf<T>(this IList<T> list, bool condition, T item)
		{
			if (condition)
			{
				list.Add(item);
			}

			return list;
		}

		public static IList<T> AddElementRange<T>(this IList<T> list, IEnumerable<T> items)
		{
			items.Each(list.Add);
			return list;
		}

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


		public static void AddRange<T>(this ObservableCollection<T> oc, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (var item in collection)
			{
				oc.Add(item);
			}


		}
	}
}
