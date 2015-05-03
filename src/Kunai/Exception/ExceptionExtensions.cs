using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kunai.ExceptionExt
{
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Gets the most inner (deepest) exception of a given Exception object
		/// </summary>
		/// <param name="ex">Source Exception</param>
		/// <returns></returns>
		public static Exception GetDeepInner(this System.Exception ex)
		{
			System.Exception ActualInnerEx = ex;

			while (ActualInnerEx != null)
			{
				ActualInnerEx = ActualInnerEx.InnerException;
				if (ActualInnerEx != null)
					ex = ActualInnerEx;
			}
			return ex;
		}

		#region Exception.ToLogString
		/// <summary>
		/// <para>Creates a log-string from the Exception.</para>
		/// <para>The result includes the stacktrace, innerexception et cetera, separated by <seealso cref="Environment.NewLine"/>.</para>
		/// </summary>
		/// <param name="ex">The exception to create the string from.</param>
		/// <param name="additionalMessage">Additional message to place at the top of the string, maybe be empty or null.</param>
		/// <returns></returns>
		public static string ToLogString(this Exception ex, string additionalMessage)
		{
			StringBuilder msg = new StringBuilder();

			if (!string.IsNullOrEmpty(additionalMessage))
			{
				msg.Append(additionalMessage);
				msg.Append(Environment.NewLine);
			}

			if (ex != null)
			{
				try
				{
					Exception orgEx = ex;

					msg.Append("Exception:");
					msg.Append(Environment.NewLine);
					while (orgEx != null)
					{
						msg.Append(orgEx.Message);
						msg.Append(Environment.NewLine);
						orgEx = orgEx.InnerException;
					}

					if (ex.Data != null)
					{
						foreach (object i in ex.Data)
						{
							msg.Append("Data :");
							msg.Append(i.ToString());
							msg.Append(Environment.NewLine);
						}
					}

					if (ex.StackTrace != null)
					{
						msg.Append("StackTrace:");
						msg.Append(Environment.NewLine);
						msg.Append(ex.StackTrace.ToString());
						msg.Append(Environment.NewLine);
					}

					if (ex.Source != null)
					{
						msg.Append("Source:");
						msg.Append(Environment.NewLine);
						msg.Append(ex.Source);
						msg.Append(Environment.NewLine);
					}

					if (ex.TargetSite != null)
					{
						msg.Append("TargetSite:");
						msg.Append(Environment.NewLine);
						msg.Append(ex.TargetSite.ToString());
						msg.Append(Environment.NewLine);
					}

					Exception baseException = ex.GetBaseException();
					if (baseException != null)
					{
						msg.Append("BaseException:");
						msg.Append(Environment.NewLine);
						msg.Append(ex.GetBaseException());
					}
				}
				finally
				{
				}
			}
			return msg.ToString();
		}
		#endregion Exception.ToLogString
	}
}
