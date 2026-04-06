using System;
using System.Collections.Generic;

namespace Kunai.FunctionalExt;

/// <summary>
/// Functional programming extension methods for types.
/// </summary>
public static class FunctionalExtensions
{
	/// <summary>
	/// Repeats an action a specified number of times, passing the current iteration (1-based) as an argument.
	/// </summary>
	/// <param name="action">The action to repeat. Receives the current iteration number.</param>
	/// <param name="repeatCount">The number of times to repeat the action.</param>
	public static void Times(this Action<int> action, int repeatCount)
	{
		for (int i = 1; i <= repeatCount; i++)
		{
			action(i);
		}
	}

	/// <summary>
	/// Pipes the value through a function and returns the result.
	/// </summary>
	/// <typeparam name="T">The input type.</typeparam>
	/// <typeparam name="R">The output type.</typeparam>
	/// <param name="o">The input value.</param>
	/// <param name="func">The function to apply.</param>
	/// <returns>The result of applying <paramref name="func"/> to <paramref name="o"/>.</returns>
	public static R Pipe<T, R>(this T o, Func<T, R> func)
	{
		// guard
		if (func == null) throw new ArgumentNullException(nameof(func), "'func' can not be null.");
		return func(o);
	}
	/// <summary>
	/// Pipes the value through an action and returns the original value, enabling fluent chaining.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="o">The input value.</param>
	/// <param name="action">The action to apply to the value.</param>
	/// <returns>The original value after applying the action.</returns>
	public static T Pipe<T>(this T o, Action<T> action)
	{
		// guard
		if (action == null) throw new ArgumentNullException(nameof(action), "'action' can not be null.");
		action(o);
		return o;
	}

	/// <summary>
	/// Applies a transformation function to the value if the predicate returns <see langword="true"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="val">The input value.</param>
	/// <param name="predicate">The condition to test.</param>
	/// <param name="func">The transformation to apply if the condition is met.</param>
	/// <returns>The transformed value if the predicate is true; otherwise the original value.</returns>
	public static T If<T>(this T val, Func<T, bool> predicate, Func<T, T> func)
	{
		if (predicate(val))
		{
			return func(val);
		}
		return val;
	}

	/// <summary>
	/// Returns a memoized version of the function that caches results by input value.
	/// Subsequent calls with the same argument return the cached result without invoking the function again.
	/// </summary>
	/// <typeparam name="T">The input type.</typeparam>
	/// <typeparam name="TResult">The output type.</typeparam>
	/// <param name="func">The function to memoize.</param>
	/// <returns>A memoized version of <paramref name="func"/>.</returns>
	public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
	{
		var cache = new Dictionary<T, TResult>();
		return n =>
		{
			if (cache.TryGetValue(n, out var cached)) return cached;
			var result = func(n);
			cache.Add(n, result);
			return result;
		};
	}
}
