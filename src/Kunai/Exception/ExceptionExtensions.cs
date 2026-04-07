using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kunai.ExceptionExt;

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
			msg.AppendLine(additionalMessage);
		}

		if (ex != null)
		{
			Exception orgEx = ex;

			msg.AppendLine("Exception:");
			while (orgEx != null)
			{
				msg.AppendLine(orgEx.Message);
				orgEx = orgEx.InnerException;
			}

			if (ex.Data != null)
			{
				foreach (object i in ex.Data)
				{
					msg.Append("Data :");
					msg.AppendLine(i.ToString());
				}
			}

			if (ex.StackTrace != null)
			{
				msg.AppendLine("StackTrace:");
				msg.AppendLine(ex.StackTrace.ToString());
			}

			if (ex.Source != null)
			{
				msg.AppendLine("Source:");
				msg.AppendLine(ex.Source);
			}

			if (ex.TargetSite != null)
			{
				msg.AppendLine("TargetSite:");
				msg.AppendLine(ex.TargetSite.ToString());
			}

			Exception baseException = ex.GetBaseException();
			if (baseException != null)
			{
				msg.AppendLine("BaseException:");
				msg.Append(baseException);
			}
		}
		return msg.ToString();
	}
	#endregion Exception.ToLogString
}
