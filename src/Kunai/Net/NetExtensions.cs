using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kunai.NetExt
{
	public static class NetExtensions
	{
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

		public static void ForceDownload(this HttpResponse Response, string fullPathToFile, string outputFileName)
		{
			Response.Clear();
			Response.AddHeader("content-disposition", "attachment; filename=" + outputFileName);
			Response.WriteFile(fullPathToFile);
			Response.ContentType = "";
			Response.End();
		}


		public static string HtmlEncode(this string data)
		{
			return HttpUtility.HtmlEncode(data);
		}

		public static string HtmlDecode(this string data)
		{
			return HttpUtility.HtmlDecode(data);
		}

		public static NameValueCollection ParseQueryString(this string query)
		{
			return HttpUtility.ParseQueryString(query);
		}

		public static string UrlEncode(this string url)
		{
			return HttpUtility.UrlEncode(url);
		}

		public static string UrlDecode(this string url)
		{
			return HttpUtility.UrlDecode(url);
		}

		public static string UrlPathEncode(this string url)
		{
			return HttpUtility.UrlPathEncode(url);
		}
	}
}
