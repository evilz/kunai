using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace Kunai.Reflection
{
	public static class ReflectionExtensions
	{
		public static T GetAttribute<T>(this PropertyInfo pi) where T : Attribute
		{
			return pi.GetAttributes<T>().FirstOrDefault();
		}

		public static IEnumerable<T> GetAttributes<T>(this PropertyInfo pi) where T : Attribute
		{
			return pi.GetCustomAttributes(typeof(T), false).Select(o => (T)o);
		}

		/// <summary>
		/// Loads the custom attributes from the type
		/// </summary>
		/// <typeparam name="T">The type of the custom attribute to find.</typeparam>
		/// <param name="typeWithAttributes">The calling assembly to search.</param>
		/// <returns>The custom attribute of type T, if found.</returns>
		public static T GetAttribute<T>(this Type typeWithAttributes)
			where T : Attribute
		{
			return GetAttributes<T>(typeWithAttributes).FirstOrDefault();
		}

		/// <summary>
		/// Loads the custom attributes from the type
		/// </summary>
		/// <typeparam name="T">The type of the custom attribute to find.</typeparam>
		/// <param name="typeWithAttributes">The calling assembly to search.</param>
		/// <returns>An enumeration of attributes of type T that were found.</returns>
		public static IEnumerable<T> GetAttributes<T>(this Type typeWithAttributes)
			where T : Attribute
		{
			// Try to find the configuration attribute for the default logger if it exists
			object[] configAttributes = Attribute.GetCustomAttributes(typeWithAttributes,
				typeof(T), false);

			if (configAttributes != null)
			{
				foreach (T attribute in configAttributes)
				{
					yield return attribute;
				}
			}
		}


		public static Type FindCommonAncestor(this Type type, Type targetType)
		{
			if (targetType.IsAssignableFrom(type))
				return targetType;

			var baseType = targetType.BaseType;
			while (baseType != null && !baseType.IsPrimitive)
			{
				if (baseType.IsAssignableFrom(type))
					return baseType;
				baseType = baseType.BaseType;
			}
			return null;
		}


		public static string Description(this Enum someEnum)
		{
			var memInfo = someEnum.GetType().GetMember(someEnum.ToString());

			if (memInfo != null && memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

				if (attrs != null && attrs.Length > 0)
					return ((DescriptionAttribute)attrs[0]).Description;
			}
			return someEnum.ToString();
		}

		// TODO USE EXPRESSION !
		//public static string Description(this T obj)
		//{
		//	var memInfo = typeof(T)T.GetType().GetMember(someEnum.ToString());

		//	if (memInfo != null && memInfo.Length > 0)
		//	{
		//		object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

		//		if (attrs != null && attrs.Length > 0)
		//			return ((DescriptionAttribute)attrs[0]).Description;
		//	}
		//	return someEnum.ToString();
		//}

		public static T Extend<T>(this T target, T source) where T : class
		{
			var properties =
				target.GetType().GetProperties().Where(pi => pi.CanRead && pi.CanWrite);

			foreach (var propertyInfo in properties)
			{
				var targetValue = propertyInfo.GetValue(target, null);
				var defaultValue = propertyInfo.PropertyType.GetDefault();

				if (targetValue != null && !targetValue.Equals(defaultValue)) continue;

				var sourceValue = propertyInfo.GetValue(source, null);
				propertyInfo.SetValue(target, sourceValue, null);
			}

			return target;
		}

		public static object GetDefault(this Type type)
		{
			// If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
			if (type == null || !type.IsValueType || type == typeof(void))
				return null;

			// If the supplied Type has generic parameters, its default value cannot be determined
			if (type.ContainsGenericParameters)
			{
				throw new ArgumentException(
					"{" + MethodBase.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
					"> contains generic parameters, so the default value cannot be retrieved");
			}

			// If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
			//  default instance of the value type
			if (type.IsPrimitive || !type.IsNotPublic)
			{
				try
				{
					return Activator.CreateInstance(type);
				}
				catch (Exception e)
				{
					throw new ArgumentException(
						"{" + MethodBase.GetCurrentMethod() +
						"} Error:\n\nThe Activator.CreateInstance method could not " +
						"create a default instance of the supplied value type <" + type +
						"> (Inner Exception message: \"" + e.Message + "\")", e);
				}
			}

			// Fail with exception
			throw new ArgumentException("{" + MethodBase.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" +
										type +
										"> is not a publicly-visible type, so the default value cannot be retrieved");
		}


		public static CompilerResults CSharpCompile(this string code, string dllName = "dynamicCompile", params string[] additionalAssemblies)
		{
			var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
			var parameters = new CompilerParameters
			{
				ReferencedAssemblies = {
		  "mscorlib.dll",
		  "System.Core.dll",
	   },
				OutputAssembly = dllName,
				GenerateExecutable = false,
				GenerateInMemory = true,
			};

			parameters.ReferencedAssemblies.AddRange(additionalAssemblies);

			return csc.CompileAssemblyFromSource(parameters, code);
		}

		public static T GetPropertyValue<T>(this object source, string property)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			var sourceType = source.GetType();
			var sourceProperties = sourceType.GetProperties();

			var propertyValue = (from s in sourceProperties
								 where s.Name.Equals(property)
								 select s.GetValue(source, null)).FirstOrDefault();

			return propertyValue != null ? (T)propertyValue : default(T);
		}

		/// <summary>
		/// Alternative version of <see cref="Type.IsSubclassOf"/> that supports raw generic types (generic types without
		/// any type parameters).
		/// </summary>
		/// <param name="baseType">The base type class for which the check is made.</param>
		/// <param name="toCheck">To type to determine for whether it derives from <paramref name="baseType"/>.</param>
		public static bool IsSubclassOfRawGeneric(this Type toCheck, Type baseType)
		{
			while (toCheck != typeof(object))
			{
				Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (baseType == cur)
				{
					return true;
				}

				toCheck = toCheck.BaseType;
			}

			return false;
		}


		/// <summary>
		/// Return true if the type is a System.Nullable wrapper of a value type
		/// </summary>
		/// <param name="type">The type to check</param>
		/// <returns>True if the type is a System.Nullable wrapper</returns>
		public static bool IsNullable(this Type type)
		{
			return type.IsGenericType
				   && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
		}
	}
}
