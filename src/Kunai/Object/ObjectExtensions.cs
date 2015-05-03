using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kunai.ObjectExt
{
	public static class ObjectExtensions
	{
		//public static T ChangeType<T>(this object obj)
		//{
		//	return (T)Convert.ChangeType(obj, typeof(T));
		//}

		// TODO : test
		public static U ChangeType<U>(this object source)
		{
			if (source is U)
				return (U)source;

			var destinationType = typeof(U);
			if (destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
				destinationType = new NullableConverter(destinationType).UnderlyingType;

			return (U)Convert.ChangeType(source, destinationType);
		}

		public static Dictionary<string, object> GetPropertyDictionary(this object source)
		{
			var properties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

			var result = properties.ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(source));

			return result;
		}

		public static T Or<T>(this T t, params T[] args)
		{
			foreach (var item in args)
				if (item != null) return item;
			return default(T);
		}


		public static bool HasValueAndEquals<T>(this T? source, T target) where T : struct
		{
			return source.HasValue && source.Value.Equals(target);
		}

		public static bool IsDefault<T>(this T val)
		{
			return val.Equals(default(T));
		}

		// move to enumerable ?
		public static bool IsIn<T>(this T value, IEnumerable<T> values)
		{
			return values.Contains(value);
		}
		public static string Format(this object value, string format)
		{
			return string.Format(format, value);
		}

		public static T ConstrainToRange<T>(this T d, T min, T max) where T : IComparable
		{
			if (d.CompareTo(min) < 0) return min;
			if (d.CompareTo(max) > 0) return max;
			return d;
		}



		public static bool IsBetween<T>(this T me, T lower, T upper) where T : IComparable<T>
		{
			return me.CompareTo(lower) >= 0 && me.CompareTo(upper) < 0;
		}

		public static void IfType<T>(this object item, Action<T> action) where T : class
		{
			if (item is T)
			{
				action(item as T);
			}
		}


		public static T DeepClone<T>(this T input) where T : ISerializable
		{
			using (var stream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, input);
				stream.Position = 0;
				return (T)formatter.Deserialize(stream);
			}
		}

	}
}
