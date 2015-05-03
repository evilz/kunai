using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kunai.UriExt
{
	public static class UriExtensions
	{
		// GET parma
		public static Dictionary<string, string> Parameters(this Uri self)
		{
			return String.IsNullOrEmpty(self.Query)
			? new Dictionary<string, string>()
			: self.Query.Substring(1).Split('&').ToDictionary(
			p => p.Split('=')[0],
			p => p.Split('=')[1]
			);
		}

		// TODO Use param
	}
}
