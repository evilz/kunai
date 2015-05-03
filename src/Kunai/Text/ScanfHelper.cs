using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Kunai.TextExt
{

	// TODO : scanf specifier : %[*][width][length]specifier 


	public static class ScanfHelper
	{
		static readonly Dictionary<string, string> _typePatterns;
		static ScanfHelper()
		{
			_typePatterns = new Dictionary<string, string>
			{
				{"String", @"[\w\d\S]+"},
				{"Int16", @"-[0-9]+|[0-9]+"},
				{"UInt16", @"[0-9]+"},
				{"Int32", @"-[0-9]+|[0-9]+"},
				{"UInt32", @"[0-9]+"},
				{"Int64", @"-[0-9]+|[0-9]+"},
				{"UInt64", @"[0-9]+"},
				{"Single", @"[-+]?[0-9]*\.?[0-9]+"},
				{"Double", @"[-+]?[0-9]*\.?[0-9]+"},
				{"Boolean", @"true|false"},
				{"Byte", @"[0-9]{1,3}"},
				{"SByte", @"-[0-9]{1,3}|[0-9]{1,3}"},
				{"Char", @"[\w\S]{1}"},
				{"Decimal", @"[-+]?[0-9]*\.?[0-9]+"}
			};

		}

		public static object[] ScanFormat(this string text, string format)
		{
			var targets = new List<object>();
			try
			{
				// position, type
				var targetMatchGroups = new List<Tuple<int, string>>();

				var masterPattern = AddGroupParens(format);

				//store the group location of the format tags so that we can select the correct group values later.
				const string MATCHING_PATTERN = @"(\([\w\d\S]+\))";
				var reggie = new Regex(MATCHING_PATTERN);
				var matches = reggie.Matches(masterPattern);
				for (var i = 0; i < matches.Count; i++)
				{
					var m = matches[i];
					var sVal = m.Groups[1].Captures[0].Value;

					//is this value a {n} value. We will determine this by checking for {
					if (sVal.IndexOf('{') >= 0)
					{
						const string p = @"\(\{(\w*)\}\)"; //pull out the type
						sVal = Regex.Replace(sVal, p, "$1");
						targetMatchGroups.Add(new Tuple<int, string>(i, sVal));

					}
				}

				//Replace all of the types with the pattern that matches that type
				foreach (var typePattern in _typePatterns)
				{
					masterPattern = Regex.Replace(masterPattern, @"\{" + typePattern.Key + @"\}", typePattern.Value);
				}

				masterPattern = WhiteSpaceToWhiteSpacePattern(masterPattern);

				//run our generated pattern against the original text.
				reggie = new Regex(masterPattern);
				matches = reggie.Matches(text);
				//PrintMatches(matches);

				for (var x = 0; x < targetMatchGroups.Count; x++)
				{
					var i = targetMatchGroups[x].Item1;
					var tName = targetMatchGroups[x].Item2;
					var t = Type.GetType("System." + tName);

					if (i < matches[0].Groups.Count)
					{
						//add 1 to i because i is a result of serveral matches each resulting in one group.
						//this query is one match resulting in serveral groups.
						var sValue = matches[0].Groups[i + 1].Captures[0].Value;
						targets.Add(ReturnValue(t, sValue));
					}
				}
			}
			catch (Exception ex)
			{
				throw new ScanFormatExeption("Scanf exception", ex);
			}

			return targets.ToArray();
		}

		private static string AddGroupParens(string format)
		{
			var masterPattern = format.Trim();
			var matchingPattern = @"(\S+)";
			masterPattern = Regex.Replace(masterPattern, matchingPattern, "($1)"); //insert grouping parens
			return masterPattern;
		}

		public static void ScanFormat(this string text, string format, params object[] outputs)
		{
			try
			{
				var targetMatchGroups = new List<int>();

				//masterPattern is going to hold a "big" regex pattern that will be ran against the original text
				var masterPattern = EscapeSpecialChar(format);
				masterPattern = AddGroupParens(masterPattern);

				//store the group location of the format tags so that we can select the correct group values later.
				var matchingPattern = @"(\([\w\d\S]+\))";
				var reggie = new Regex(matchingPattern);
				var matches = reggie.Matches(masterPattern);
				for (var i = 0; i < matches.Count; i++)
				{
					var m = matches[i];
					var sVal = m.Groups[1].Captures[0].Value;

					//is this value a {n} value. We will determine this by checking for {
					if (sVal.IndexOf('{') >= 0)
					{
						targetMatchGroups.Add(i);
					}
				}

				matchingPattern = @"(\{\S+\})";	//match each paramter tag of the format {n} where n is a digit
				reggie = new Regex(matchingPattern);
				matches = reggie.Matches(masterPattern);

				for (var i = 0; i < outputs.Length && i < matches.Count; i++)
				{
					//var groupID = String.Format("${0}",(i+1));

					var t = outputs[i].GetType();
					var innerPattern = _typePatterns[t.Name];

					//replace the {n} with the type's pattern
					var groupPattern = "\\{" + i + "\\}";
					masterPattern = Regex.Replace(masterPattern, groupPattern, innerPattern);
				}

				masterPattern = WhiteSpaceToWhiteSpacePattern(masterPattern);

				//run our generated pattern against the original text.
				reggie = new Regex(masterPattern);
				matches = reggie.Matches(text);
				for (var x = 0; x < targetMatchGroups.Count; x++)
				{
					var i = targetMatchGroups[x];
					if (i >= matches[0].Groups.Count) continue;
					//add 1 to i because i is a result of serveral matches each resulting in one group.
					//this query is one match resulting in serveral groups.
					var sValue = matches[0].Groups[i + 1].Captures[0].Value;
					var t = outputs[x].GetType();
					outputs[x] = ReturnValue(t, sValue);
				}
			}
			catch (Exception ex)
			{
				throw new ScanFormatExeption("Scan exception", ex);
			}
		}

		private static string EscapeSpecialChar(string format)
		{
			format = Regex.Escape(format); //insert grouping parens
			format = format.Replace(@"\{", "{");
			format = format.Replace(@"}\", "}");
			format = format.Replace(@"|\", "|");
			format = format.Replace(@"\ ", " ");
			return format;
		}

		private static string WhiteSpaceToWhiteSpacePattern(string masterPattern)
		{
			masterPattern = Regex.Replace(masterPattern, @"\s+", "\\s+"); //replace white space with the whitespace pattern
			return masterPattern;
		}

		#region Generic ScanFormat

		public static Tuple<T, T2, T3, T4, T5, T6, T7> ScanFormat<T, T2, T3, T4, T5, T6, T7>(this string text, string format)
		{
			var o = new object[7];

			o[0] = DefaultAndStringEmpty<T>();
			o[1] = DefaultAndStringEmpty<T2>();
			o[2] = DefaultAndStringEmpty<T3>();
			o[3] = DefaultAndStringEmpty<T4>();
			o[4] = DefaultAndStringEmpty<T5>();
			o[5] = DefaultAndStringEmpty<T6>();
			o[6] = DefaultAndStringEmpty<T7>();
			text.ScanFormat(format, o);

			return new Tuple<T, T2, T3, T4, T5, T6, T7>((T)o[0], (T2)o[1], (T3)o[2], (T4)o[3], (T5)o[4], (T6)o[5], (T7)o[6]);
		}

		public static Tuple<T, T2, T3, T4, T5, T6> ScanFormat<T, T2, T3, T4, T5, T6>(this string text, string format)
		{
			var o = new object[6];

			o[0] = DefaultAndStringEmpty<T>();
			o[1] = DefaultAndStringEmpty<T2>();
			o[2] = DefaultAndStringEmpty<T3>();
			o[3] = DefaultAndStringEmpty<T4>();
			o[4] = DefaultAndStringEmpty<T5>();
			o[5] = DefaultAndStringEmpty<T6>();
			text.ScanFormat(format, o);

			return new Tuple<T, T2, T3, T4, T5, T6>((T)o[0], (T2)o[1], (T3)o[2], (T4)o[3], (T5)o[4], (T6)o[5]);
		}

		public static Tuple<T, T2, T3, T4, T5> ScanFormat<T, T2, T3, T4, T5>(this string text, string format)
		{
			var o = new object[5];

			o[0] = DefaultAndStringEmpty<T>();
			o[1] = DefaultAndStringEmpty<T2>();
			o[2] = DefaultAndStringEmpty<T3>();
			o[3] = DefaultAndStringEmpty<T4>();
			o[4] = DefaultAndStringEmpty<T5>();
			text.ScanFormat(format, o);

			return new Tuple<T, T2, T3, T4, T5>((T)o[0], (T2)o[1], (T3)o[2], (T4)o[3], (T5)o[4]);
		}

		public static Tuple<T, T2, T3, T4> ScanFormat<T, T2, T3, T4>(this string text, string format)
		{
			var o = new object[5];

			o[0] = DefaultAndStringEmpty<T>();
			o[1] = DefaultAndStringEmpty<T2>();
			o[2] = DefaultAndStringEmpty<T3>();
			o[3] = DefaultAndStringEmpty<T4>();
			text.ScanFormat(format, o);

			return new Tuple<T, T2, T3, T4>((T)o[0], (T2)o[1], (T3)o[2], (T4)o[3]);
		}

		public static Tuple<T, T2, T3> ScanFormat<T, T2, T3>(this string text, string format)
		{
			var o = new object[5];

			o[0] = DefaultAndStringEmpty<T>();
			o[1] = DefaultAndStringEmpty<T2>();
			o[2] = DefaultAndStringEmpty<T3>();
			text.ScanFormat(format, o);

			return new Tuple<T, T2, T3>((T)o[0], (T2)o[1], (T3)o[2]);
		}

		public static Tuple<T, T2> ScanFormat<T, T2>(this string text, string format)
		{
			var o = new object[5];

			o[0] = DefaultAndStringEmpty<T>();
			o[1] = DefaultAndStringEmpty<T2>();
			text.ScanFormat(format, o);

			return new Tuple<T, T2>((T)o[0], (T2)o[1]);
		}

		public static T ScanFormat<T>(this string text, string format)
		{
			var o = new object[5];

			o[0] = DefaultAndStringEmpty<T>();
			text.ScanFormat(format, o);

			return (T)o[0];
		}

		private static object DefaultAndStringEmpty<T>()
		{
			return typeof(T) == typeof(string) ? (object)string.Empty : default(T);
		}

		#endregion


		private static object ReturnValue(Type type, string sValue)
		{
			if (type == typeof(string))
				return sValue;

			var m = type.GetMethod("Parse", new[] { typeof(string) });
			return m.Invoke(null, new object[] { sValue });
		}


	}

	/// <summary>
	/// Exceptions that are thrown by this namespace and the Scanner Class
	/// </summary>
	class ScanFormatExeption : Exception
	{
		public ScanFormatExeption() : base()
		{
		}

		public ScanFormatExeption(string message) : base(message)
		{
		}

		public ScanFormatExeption(string message, Exception inner) : base(message, inner)
		{
		}

		public ScanFormatExeption(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
