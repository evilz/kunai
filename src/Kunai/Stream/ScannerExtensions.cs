using System;
using System.IO;
using System.Text;
using Kunai.TextExt;

namespace Kunai.Stream
{
	/// <summary>
	/// Scanner extensions are inspired from Java scanner. You can read next string, number or array from a TextReader
	/// 
	/// When writing literate programs as Markdown documents, 
	/// you can also include snippets in other languages.
	/// These will not be colorized and processed as F# 
	/// code samples:
	///
	///		[lang=csharp]
	///		Console.WriteLine("Hello world!");
	/// 
	/// </summary>
	public static class ScannerExtensions
	{

		/// <summary>
		/// Get next string
		/// 
		///	 ## Example
		/// 
		///		string input = "    the string  032423";
		///		var reader = new StringReader(input);
		///		var s1 = reader.NextString();
		///
		/// </summary>
		public static string NextString(this TextReader reader)
		{
			var sb = new StringBuilder();
			var lastChar = reader.Read();
			while (lastChar > -1)
			{
				if (lastChar.IsWhiteSpace())
				{
					if (sb.Length > 0)
					{
						break;
					}
				}
				else
				{
					sb.Append((char)lastChar);
				}
				lastChar = reader.Read();
			}

			return sb.ToString();
		}

		public static int NextNumber(this TextReader reader)
		{
			var token = reader.NextString();
			return int.Parse(token);
		}

		public static T NextNumber<T>(this TextReader reader) where T : IConvertible
		{
			var token = reader.NextString();
			return (token.Parse<T>());
		}

		public static T[] NextNumberArray<T>(this TextReader reader, int size) where T : IConvertible
		{
			T[] array = new T[size];
			for (int i = 0; i < size; i++)
			{
				T token = reader.NextNumber<T>();
				array[i] = token;
			}
			return array;
		}

		public static string[] NextStringArray<T>(this TextReader reader, int size)
		{
			string[] array = new string[size];
			for (int i = 0; i < size; i++)
			{
				string token = reader.NextString();
				array[i] = token;
			}
			return array;
		}



		public static bool IsWhiteSpace(this int charcode)
		{
			return Char.IsWhiteSpace((char)charcode);
		}

		public static bool IsWhiteSpace(this char charcode)
		{
			return Char.IsWhiteSpace(charcode);
		}
	}
}
