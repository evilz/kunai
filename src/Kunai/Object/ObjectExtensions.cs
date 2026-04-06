using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Kunai.ObjectExt;

/// <summary>
/// Extension methods for <see cref="object"/> and generic types.
/// </summary>
public static class ObjectExtensions
{
	/// <summary>
	/// Converts the object to the specified type <typeparamref name="U"/>, supporting <see cref="Nullable{T}"/> types.
	/// </summary>
	/// <typeparam name="U">The target type to convert to.</typeparam>
	/// <param name="source">The source object.</param>
	/// <returns>The converted value as <typeparamref name="U"/>.</returns>
	public static U ChangeType<U>(this object source)
	{
		if (source is U)
			return (U)source;

		var destinationType = typeof(U);
		if (destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
			destinationType = new NullableConverter(destinationType).UnderlyingType;

		return (U)Convert.ChangeType(source, destinationType);
	}

	/// <summary>
	/// Returns a dictionary of all public instance property names and their values.
	/// </summary>
	/// <param name="source">The source object.</param>
	/// <returns>A dictionary mapping property names to their values.</returns>
	public static Dictionary<string, object> GetPropertyDictionary(this object source)
	{
		var properties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

		var result = properties.ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(source));

		return result;
	}

	/// <summary>
	/// Returns the first non-null value from the arguments, or the default value of <typeparamref name="T"/> if all are null.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="t">The initial value to consider.</param>
	/// <param name="args">Additional values to consider.</param>
	/// <returns>The first non-null value, or the default value of <typeparamref name="T"/>.</returns>
	public static T Or<T>(this T t, params T[] args)
	{
		foreach (var item in args)
			if (item != null) return item;
		return default;
	}


	/// <summary>
	/// Returns <see langword="true"/> if the nullable value has a value and it equals <paramref name="target"/>.
	/// </summary>
	/// <typeparam name="T">The underlying value type.</typeparam>
	/// <param name="source">The nullable value.</param>
	/// <param name="target">The value to compare against.</param>
	/// <returns><see langword="true"/> if <paramref name="source"/> has a value equal to <paramref name="target"/>.</returns>
	public static bool HasValueAndEquals<T>(this T? source, T target) where T : struct =>
		source.HasValue && source.Value.Equals(target);

	/// <summary>
	/// Returns <see langword="true"/> if the value equals the default value for its type.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="val">The value to check.</param>
	/// <returns><see langword="true"/> if the value is the default; otherwise <see langword="false"/>.</returns>
	public static bool IsDefault<T>(this T val) =>
		val.Equals(default(T));

	/// <summary>
	/// Returns <see langword="true"/> if the value is contained within the specified collection.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="value">The value to search for.</param>
	/// <param name="values">The collection to search in.</param>
	/// <returns><see langword="true"/> if found; otherwise <see langword="false"/>.</returns>
	// move to enumerable ?
	public static bool IsIn<T>(this T value, IEnumerable<T> values) =>
		values.Contains(value);
	/// <summary>
	/// Formats the object using the specified format string.
	/// </summary>
	/// <param name="value">The object to format.</param>
	/// <param name="format">The composite format string.</param>
	/// <returns>The formatted string.</returns>
	public static string Format(this object value, string format) =>
		string.Format(format, value);

	/// <summary>
	/// Returns <see langword="true"/> if the value is within [<paramref name="lower"/>, <paramref name="upper"/>) (lower-inclusive, upper-exclusive).
	/// </summary>
	/// <typeparam name="T">A comparable type.</typeparam>
	/// <param name="me">The value to test.</param>
	/// <param name="lower">The inclusive lower bound.</param>
	/// <param name="upper">The exclusive upper bound.</param>
	/// <returns><see langword="true"/> if <paramref name="me"/> is between the bounds.</returns>
	public static bool IsBetween<T>(this T me, T lower, T upper) where T : IComparable<T> =>
		me.CompareTo(lower) >= 0 && me.CompareTo(upper) < 0;

	/// <summary>
	/// Executes an action on the item if it is of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type to check for.</typeparam>
	/// <param name="item">The object to test.</param>
	/// <param name="action">The action to execute if the type matches.</param>
	public static void IfType<T>(this object item, Action<T> action) where T : class
	{
		if (item is T typedItem)
		{
			action(typedItem);
		}
	}

}
