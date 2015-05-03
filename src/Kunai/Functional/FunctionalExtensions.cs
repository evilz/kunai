using System;
using System.Collections.Generic;

namespace Kunai.FunctionalExt
{
	public static class FunctionalExtensions
	{
		public static void Times(this Action<int> action, int repeatCount)
		{
			for (int i = 1; i <= repeatCount; i++)
			{
				action(i);
			}
		}

		public static R Pipe<T, R>(this T o, Func<T, R> func)
		{
			// guard
			if (func == null) throw new ArgumentNullException("func", "'func' can not be null.");
			return func(o);
		}
		public static T Pipe<T>(this T o, Action<T> action)
		{
			// guard
			if (action == null) throw new ArgumentNullException("action", "'action' can not be null.");
			action(o);
			return o;
		}

		public static T If<T>(this T val, Func<T, bool> predicate, Func<T, T> func)
		{
			if (predicate(val))
			{
				return func(val);
			}
			return val;
		}

		// CHECK this
		public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
		{
			var t = new Dictionary<T, TResult>();
			return n =>
			{
				if (t.ContainsKey(n)) return t[n];
				else
				{
					var result = func(n);
					t.Add(n, result);
					return result;
				}
			};
		}
	}
}
