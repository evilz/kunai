using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Kunai.Reflection;

namespace Kunai.TextExt
{
	public static class TextExtensions
	{
		public static string AsString(this IEnumerable<char> chars) { return new string(chars.ToArray()); }
		public static string AsJoinedString<T>(this IEnumerable<T> enumerable, string separator = " ") { return string.Join(separator, enumerable); }

		public static void AsException(this string message)
		{
			throw new Exception(message);
		}

		public static bool AsBoolean(this string value)
		{
			var trueVal = new[] {"true", "t", "yes", "y", "1"};
			return trueVal.Contains(value.Trim().ToLowerInvariant());
		}
		public static bool IsBoolean(this string value)
		{
			var trueVal = new[] { "false","f","no","n","true", "t", "yes", "y", "1" };
			return trueVal.Contains(value.Trim().ToLowerInvariant());
		}


		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}

		public static bool IsNullOrWhiteSpace(this string s)
		{
			return string.IsNullOrWhiteSpace(s);
		}

		public static string Format(this string format, params object[] args)
		{
			return string.Format(format,args);
		}

		/// <summary>
		/// Formats the string according to the specified mask
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
		/// <returns>The formatted string</returns>
		public static string FormatWithMask(this string input, string mask)
		{
			if (input.IsNullOrEmpty()) return input;
			var output = string.Empty;
			var index = 0;
			foreach (var m in mask)
			{
				if (m == '#')
				{
					if (index < input.Length)
					{
						output += input[index];
						index++;
					}
				}
				else
					output += m;
			}
			return output;
		}


		// TODO !!!! change this regex !
		public static bool IsNumeric(this string value)
		{
			Regex regex = new Regex(@"[0-9]");
			return regex.IsMatch(value);
		}

		/// <summary>
		/// Converts the string representation of a Guid to its Guid 
		/// equivalent. A return value indicates whether the operation 
		/// succeeded. 
		/// </summary>
		/// <param name="s">A string containing a Guid to convert.</param>
		/// <param name="result">
		/// When this method returns, contains the Guid value equivalent to 
		/// the Guid contained in <paramref name="s"/>, if the conversion 
		/// succeeded, or <see cref="Guid.Empty"/> if the conversion failed. 
		/// The conversion fails if the <paramref name="s"/> parameter is a 
		/// <see langword="null" /> reference (<see langword="Nothing" /> in 
		/// Visual Basic), or is not of the correct format. 
		/// </param>
		/// <value>
		/// <see langword="true" /> if <paramref name="s"/> was converted 
		/// successfully; otherwise, <see langword="false" />.
		/// </value>
		/// <exception cref="ArgumentNullException">
		///        Thrown if <pararef name="s"/> is <see langword="null"/>.
		/// </exception>
		/// <remarks>
		/// Original code at https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=94072&wa=wsignin1.0#tabs
		/// 
		/// </remarks>
		public static bool IsGuid(this string s)
		{
			if (s == null)
				throw new ArgumentNullException("s");

			Regex format = new Regex(
				"^[A-Fa-f0-9]{32}$|" +
				"^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
				"^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
			Match match = format.Match(s);

			return match.Success;
		}

		public static string NullToEmpty(this string input)
		{
			if (input == null)
			{
				return string.Empty;
			}

			return input;
		}

		public static T ToEnum<T>(this string value) where T : struct
		{
			if (string.IsNullOrEmpty(value)) return default(T);
			T result;
			return Enum.TryParse<T>(value, true, out result) ? result : default(T);
		}
		
			public static string GetEnumDescription<T>(string value)
			{
				Type type = typeof(T);
				var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

				if (name == null)
				{
					return string.Empty;
				}
				var field = type.GetField(name);
				var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
				return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
			}
	

		public static System.IO.Stream AsStream(this string input)
		{
			var byteArray = Encoding.UTF8.GetBytes(input);
			var stream = new MemoryStream(byteArray);
			return stream;
		}

		public static TextReader AsTextReader(this string input)
		{
			return new StringReader(input);
		}

		public static T Parse<T>(this string sValue) where T : IConvertible 
		{
			var type = typeof(T);
			var m = type.GetMethod("Parse", new[] { typeof(string) });
			return (T)m.Invoke(null, new object[] { sValue });
		}

		public static T Convert<T>(this string input)
		{
			var converter = TypeDescriptor.GetConverter(typeof(T));
			return (T)converter.ConvertFromString(input);
		}

		public static IEnumerable<string> GetMatchValue(this string rawString, string pattern, bool uniqueOnly = false)
		{
			MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(rawString, pattern, RegexOptions.IgnoreCase);
			IEnumerable<string> result = matches.Cast<Match>().Select(m => m.Value);
			if (uniqueOnly) return result.Distinct().ToList<string>();

			return result.ToList<string>();
		}

		/// <summary>
		/// Returns the contents of a string starting with the location of the searchFor
		/// </summary>
		/// <param name="s">The string to search.</param>
		/// <param name="searchFor">The string to search for.</param>
		/// <returns></returns>
		public static string TakeFrom(this string s, string searchFor)
		{
			if (string.IsNullOrEmpty(searchFor)) return s;
			if (!s.Contains(searchFor)) return s;
			var index = s.IndexOf(searchFor,StringComparison.CurrentCulture);
			return s.Substring(index, s.Length - index);
		}

		public static bool ContainsNoSpaces(this string s)
		{
			var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]+$");
			return regex.IsMatch(s);
		}

		public static int ExcelColumnIndex(this string columnName)
		{
			int number = 0;
			int pow = 1;

			for (int i = columnName.Length - 1; i >= 0; i--)
			{
				number += (columnName[i] - 'A' + 1) * pow;
				pow *= 26;
			}

			return number;
		}

		public static String toSlug(this string text)
		{
			String value = text.Normalize(NormalizationForm.FormD).Trim();
			StringBuilder builder = new StringBuilder();

			foreach (char c in text.ToCharArray())
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					builder.Append(c);

			value = builder.ToString();

			byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);

			value = Regex.Replace(Regex.Replace(Encoding.ASCII.GetString(bytes), @"\s{2,}|[^\w]", " ", RegexOptions.ECMAScript).Trim(), @"\s+", "_");

			return value.ToLowerInvariant();
		}

		/// <summary>
		/// Return the remainder of a string s after a separator c.
		/// </summary>
		/// <param name="s">String to search in.</param>
		/// <param name="c">Separator</param>
		/// <returns>The right part of the string after the character c, or the string itself when c isn't found.</returns>
		public static string RightOf(this string s, char c)
		{
			int ndx = s.IndexOf(c);
			if (ndx == -1)
				return s;
			return s.Substring(ndx + 1);
		}


		/// <summary>
		/// Returns characters from right of specified length
		/// </summary>
		/// <param name="value">String value</param>
		/// <param name="length">Max number of charaters to return</param>
		/// <returns>Returns string from right</returns>
		public static string Right(this string value, int length)
		{
			return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
		}

		/// <summary>
		/// Returns characters from left of specified length
		/// </summary>
		/// <param name="value">String value</param>
		/// <param name="length">Max number of charaters to return</param>
		/// <returns>Returns string from left</returns>
		public static string Left(this string value, int length)
		{
			return value != null && value.Length > length ? value.Substring(0, length) : value;
		}

		/// <summary>
		/// Returns the first part of the strings, up until the character c. If c is not found in the
		/// string the whole string is returned.
		/// </summary>
		/// <param name="s">String to truncate</param>
		/// <param name="c">Character to stop at.</param>
		/// <returns>Truncated string</returns>
		public static string LeftOf(this string s, char c)
		{
			int ndx = s.IndexOf(c);
			if (ndx >= 0)
			{
				return s.Substring(0, ndx);
			}

			return s;
		}

		/// <summary>
		/// Truncates the string to a specified length and replace the truncated to a ...
		/// </summary>
		/// <param name="text">string that will be truncated</param>
		/// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
		/// <returns>truncated string</returns>
		public static string Truncate(this string text, int maxLength)
		{
			// replaces the truncated string to a ...
			const string suffix = "...";
			string truncatedString = text;

			if (maxLength <= 0) return truncatedString;
			int strLength = maxLength - suffix.Length;

			if (strLength <= 0) return truncatedString;

			if (text == null || text.Length <= maxLength) return truncatedString;

			truncatedString = text.Substring(0, strLength);
			truncatedString = truncatedString.TrimEnd();
			truncatedString += suffix;
			return truncatedString;
		}

		/// <summary>
		/// Repeat the given char the specified number of times.
		/// </summary>
		/// <param name="input">The char to repeat.</param>
		/// <param name="count">The number of times to repeat the string.</param>
		/// <returns>The repeated char string.</returns>
		public static string Repeat(this char input, int count)
		{
			return new string(input, count);
		}

		public static string Reverse(this string s)
		{
			char[] c = s.ToCharArray();
			Array.Reverse(c);
			return new string(c);
		}


		public static string ToTitleCase(this string text)
		{
			System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
			System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
			return textInfo.ToTitleCase(text);
		}
		// TODO : remake this ! and rename
		public static string ToSentence(this string variableName)
		{
			var builder = new StringBuilder();

			char[] chars = variableName.ToCharArray();

			foreach (char c in chars)
			{
				if (char.IsLetter(c) && char.IsUpper(c))
				{
					builder.Append(" ");
				}

				builder.Append(c);
			}

			variableName = builder.ToString().TrimStart();

			return variableName;
		}

		/// <summary>
		/// Splits a string into a NameValueCollection, where each "namevalue" is separated by
		/// the "OuterSeparator". The parameter "NameValueSeparator" sets the split between Name and Value.
		/// Example: 
		///             String str = "param1=value1;param2=value2";
		///             NameValueCollection nvOut = str.ToNameValueCollection(';', '=');
		///             
		/// The result is a NameValueCollection where:
		///             key[0] is "param1" and value[0] is "value1"
		///             key[1] is "param2" and value[1] is "value2"
		/// </summary>
		/// <param name="str">String to process</param>
		/// <param name="OuterSeparator">Separator for each "NameValue"</param>
		/// <param name="NameValueSeparator">Separator for Name/Value splitting</param>
		/// <returns></returns>
		public static NameValueCollection ToNameValueCollection(this String str, Char OuterSeparator, Char NameValueSeparator)
		{
			NameValueCollection nvText = null;
			str = str.TrimEnd(OuterSeparator);
			if (!String.IsNullOrEmpty(str))
			{
				String[] arrStrings = str.TrimEnd(OuterSeparator).Split(OuterSeparator);

				foreach (String s in arrStrings)
				{
					Int32 posSep = s.IndexOf(NameValueSeparator);
					String name = s.Substring(0, posSep);
					String value = s.Substring(posSep + 1);
					if (nvText == null)
						nvText = new NameValueCollection();
					nvText.Add(name, value);
				}
			}
			return nvText;
		}

		/// <summary>
		/// Count all words in a given string
		/// </summary>
		/// <param name="input">string to begin with</param>
		/// <returns>int</returns>
		public static int WordCount(this string input)
		{
			var count = 0;
			try
			{
				// Exclude whitespaces, Tabs and line breaks
				var re = new Regex(@"[^\s]+");
				var matches = re.Matches(input);
				count = matches.Count;
			}
			catch
			{
			}
			return count;
		}


		// refacto !
		public static string Repeat(this string input, int count)
		{
			if (input == null)
			{
				return null;
			}

			var sb = new StringBuilder();

			for (var repeat = 0; repeat < count; repeat++)
			{
				sb.Append(input);
			}

			return sb.ToString();
		}

		public static string StripHtml(this string input)
		{
			// Will this simple expression replace all tags???
			var tagsExpression = new Regex(@"</?.+?>");
			return tagsExpression.Replace(input, " ");
		}

		public static byte[] ToBytes(this string content)
		{
			byte[] bytes = new byte[content.Length * sizeof(char)];
			System.Buffer.BlockCopy(content.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		public static string ToPlural(this string singular)
		{
			// Multiple words in the form A of B : Apply the plural to the first word only (A)
			int index = singular.LastIndexOf(" of ");
			if (index > 0) return (singular.Substring(0, index)) + singular.Remove(0, index).ToPlural();

			// single Word rules
			//sibilant ending rule
			if (singular.EndsWith("sh")) return singular + "es";
			if (singular.EndsWith("ch")) return singular + "es";
			if (singular.EndsWith("us")) return singular + "es";
			if (singular.EndsWith("ss")) return singular + "es";
			//-ies rule
			if (singular.EndsWith("y")) return singular.Remove(singular.Length - 1, 1) + "ies";
			// -oes rule
			if (singular.EndsWith("o")) return singular.Remove(singular.Length - 1, 1) + "oes";
			// -s suffix rule
			return singular + "s";
		}

		public static bool IsStrongPassword(this string s)
		{
			bool isStrong = Regex.IsMatch(s, @"[\d]");
			if (isStrong) isStrong = Regex.IsMatch(s, @"[a-z]");
			if (isStrong) isStrong = Regex.IsMatch(s, @"[A-Z]");
			if (isStrong) isStrong = Regex.IsMatch(s, @"[\s~!@#\$%\^&\*\(\)\{\}\|\[\]\\:;'?,.`+=<>\/]");
			if (isStrong) isStrong = s.Length > 7;
			return isStrong;
		}

		public static bool IsValidIPAddress(this string s)
		{
			return Regex.IsMatch(s,
					@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
		}

		public static bool IsValidUrl(this string text)
		{
			Regex rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
			return rx.IsMatch(text);
		}

		public static bool IsUnicode(this string value)
		{
			int asciiBytesCount = Encoding.ASCII.GetByteCount(value);
			int unicodBytesCount = Encoding.UTF8.GetByteCount(value);

			if (asciiBytesCount != unicodBytesCount)
			{
				return true;
			}
			return false;
		}

		public static bool IsValidEmailAddress(this string s)
		{
			Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
			return regex.IsMatch(s);
		}

		#region CSV
		private enum CSVSplitState
		{
			Normal,
			InQuotes,
			InQuotesFoundQuote
		}

		public static IEnumerable<string> CSVSplit(this string s)
		{
			CSVSplitState state = CSVSplitState.Normal;
			StringBuilder token = new StringBuilder();
			foreach (char c in s)
			{
				switch (state)
				{
					case CSVSplitState.Normal:
						if (c == ',')
						{
							yield return token.ToString();
							token = new StringBuilder();
						}
						else if (c == '"')
							state = CSVSplitState.InQuotes;
						else
							token.Append(c);
						break;

					case CSVSplitState.InQuotes:
						if (c == '"')
							state = CSVSplitState.InQuotesFoundQuote;
						else
							token.Append(c);
						break;

					case CSVSplitState.InQuotesFoundQuote:
						if (c == '"')
						{
							token.Append(c);
							state = CSVSplitState.InQuotes;
						}
						else
						{
							state = CSVSplitState.Normal;
							goto case CSVSplitState.Normal;
						}
						break;
				}
			}
			yield return token.ToString();
		}

		public static string ToCsv<T>(this IEnumerable<T> instance, bool includeColumnHeader, string[] properties)
		{
			if (instance == null)
				return null;

			var csv = new StringBuilder();

			if (includeColumnHeader)
			{
				var header = new StringBuilder();
				foreach (var property in properties)
					header.AppendFormat("{0},", property);

				csv.AppendLine(header.ToString(0, header.Length - 1));
			}

			foreach (var item in instance)
			{
				var row = new StringBuilder();

				foreach (var property in properties)
					row.AppendFormat("{0},", item.GetPropertyValue<object>(property));

				csv.AppendLine(row.ToString(0, row.Length - 1));
			}

			return csv.ToString();
		}

		public static string ToCsv<T>(this IEnumerable<T> instance, bool includeColumnHeader)
		{
			if (instance == null)
				return null;

			var properties = (from p in typeof(T).GetProperties()
							  select p.Name).ToArray();

			return ToCsv(instance, includeColumnHeader, properties);
		}

		#endregion

		public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string value)
		{
			if (condition) builder.Append(value);
			return builder;
		}

		public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object[] arguments)
		{
			string value = String.Format(format, arguments);

			builder.AppendLine(value);

			return builder;
		}

		#region System.IO.Compression;

		public static string CompressString(this string text)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(text);
			var memoryStream = new MemoryStream();
			using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
			{
				gZipStream.Write(buffer, 0, buffer.Length);
			}

			memoryStream.Position = 0;

			var compressedData = new byte[memoryStream.Length];
			memoryStream.Read(compressedData, 0, compressedData.Length);

			var gZipBuffer = new byte[compressedData.Length + 4];
			Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
			Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
			return System.Convert.ToBase64String(gZipBuffer);
		}

		public static string DecompressString(string compressedText)
		{
			byte[] gZipBuffer = System.Convert.FromBase64String(compressedText);
			using (var memoryStream = new MemoryStream())
			{
				int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
				memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

				var buffer = new byte[dataLength];

				memoryStream.Position = 0;
				using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					gZipStream.Read(buffer, 0, buffer.Length);
				}

				return Encoding.UTF8.GetString(buffer);
			}
		}

		#endregion
	}
}
