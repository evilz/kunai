using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace Kunai.NetExt
{
	public static class NetExtensions
	{
		/// <summary>
		/// Converts an IP address to its numeric representation.
		/// </summary>
		/// <param name="IPaddress">The IP address to convert.</param>
		/// <returns>The numeric value of the IP address.</returns>
		public static double inet_aton(this IPAddress IPaddress)
		{
			int i;
			double num = 0;
			if (IPaddress.ToString() == "")
			{
				return 0;
			}
			string[] arrDec = IPaddress.ToString().Split('.');
			for (i = arrDec.Length - 1; i >= 0; i--)
			{
				num += ((int.Parse(arrDec[i]) % 256) * Math.Pow(256, (3 - i)));
			}
			return num;
		}

		/// <summary>
		/// HTML-encodes a string.
		/// </summary>
		/// <param name="data">The string to encode.</param>
		/// <returns>The HTML-encoded string.</returns>
		public static string HtmlEncode(this string data)
		{
			return HttpUtility.HtmlEncode(data);
		}

		/// <summary>
		/// HTML-decodes a string.
		/// </summary>
		/// <param name="data">The string to decode.</param>
		/// <returns>The HTML-decoded string.</returns>
		public static string HtmlDecode(this string data)
		{
			return HttpUtility.HtmlDecode(data);
		}

		/// <summary>
		/// Parses a query string into a <see cref="NameValueCollection"/>.
		/// </summary>
		/// <param name="query">The query string to parse.</param>
		/// <returns>A <see cref="NameValueCollection"/> of key-value pairs.</returns>
		public static NameValueCollection ParseQueryString(this string query)
		{
			return HttpUtility.ParseQueryString(query);
		}

		/// <summary>
		/// URL-encodes a string.
		/// </summary>
		/// <param name="url">The string to encode.</param>
		/// <returns>The URL-encoded string.</returns>
		public static string UrlEncode(this string url)
		{
			return HttpUtility.UrlEncode(url);
		}

		/// <summary>
		/// URL-decodes a string.
		/// </summary>
		/// <param name="url">The encoded string to decode.</param>
		/// <returns>The URL-decoded string.</returns>
		public static string UrlDecode(this string url)
		{
			return HttpUtility.UrlDecode(url);
		}

		/// <summary>
		/// URL path-encodes a string.
		/// </summary>
		/// <param name="url">The string to encode.</param>
		/// <returns>The URL path-encoded string.</returns>
		public static string UrlPathEncode(this string url)
		{
			return HttpUtility.UrlPathEncode(url);
		}
	}
}
