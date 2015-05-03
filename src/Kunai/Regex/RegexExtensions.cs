using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kunai.RegexExt
{
	public static class RegexExtensions
	{
		public static T[] REExtract<T>(this string s, string regex)
		{
			TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
			if (!tc.CanConvertFrom(typeof(string)))
			{
				throw new ArgumentException("Type does not have a TypeConverter from string", "T");
			}
			if (!string.IsNullOrEmpty(s))
			{
				return
					System.Text.RegularExpressions.Regex.Matches(s, regex)
					.Cast<Match>()
					.Select(f => f.ToString())
					.Select(f => (T)tc.ConvertFrom(f))
					.ToArray();
			}
			else
				return new T[0];
		}
	}
}
