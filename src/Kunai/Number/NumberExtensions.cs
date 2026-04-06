using System;
using System.Collections.Generic;
using System.Linq;
using Kunai.ObjectExt;

namespace Kunai.NumberExt;

/// <summary>
/// Extension methods for numeric types.
/// </summary>
public static class NumberExtensions
{
	/// <summary>
		/// Format a double using the local culture currency settings.
		/// </summary>
		/// <param name="value">The double to be formatted.</param>
		/// <returns>The double formatted based on the local culture currency settings.</returns>
		public static string ToLocalCurrencyString(this double value)
		{
			return value.Format("{0:C}");
		}

	/// <summary>
	/// Converts a 1-based column index to an Excel column name (e.g. 1 → "A", 27 → "AA").
	/// </summary>
	/// <param name="index">The 1-based column index.</param>
	/// <returns>The Excel column name string.</returns>
	public static string ExcelColumnName(this int index)
	{
		var chars = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
	'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

		index -= 1;	//adjust so it matches 0-indexed array rather than 1-indexed column

		string columnName;

		var quotient = index / 26;
		if (quotient > 0)
			columnName = ExcelColumnName(quotient) + chars[index % 26];
		else
			columnName = chars[index % 26].ToString();

		return columnName;
	}

	/// <summary>
	/// Returns <see langword="true"/> if the integer is within [<paramref name="start"/>, <paramref name="end"/>] inclusive.
	/// </summary>
	/// <param name="target">The value to test.</param>
	/// <param name="start">The inclusive lower bound.</param>
	/// <param name="end">The inclusive upper bound.</param>
	/// <returns><see langword="true"/> if <paramref name="target"/> is within range; otherwise <see langword="false"/>.</returns>
	public static bool IsInRange(this int target, int start, int end) =>
		target >= start && target <= end;

	/// <summary>
	/// Kilobytes
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static int KB(this int value) =>
		value * 1024;

	/// <summary>
	/// Megabytes
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static int MB(this int value) =>
		value.KB() * 1024;

	/// <summary>
	/// Gigabytes
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static int GB(this int value) =>
		value.MB() * 1024;

	/// <summary>
	/// Terabytes
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static long TB(this int value) =>
		(long)value.GB() * (long)1024;

	// TODO : look at php cake
	public static string ToReadableSize(this long fileSize)
	{
		string[] suffix = { "bytes", "KB", "MB", "GB" };
		long j = 0;

		while (fileSize > 1024 && j < 4)
		{
			fileSize = fileSize / 1024;
			j++;
		}
		return (fileSize + " " + suffix[j]);
	}

	/// <summary>
	/// Returns <see langword="true"/> if <paramref name="number"/> is a prime number.
	/// </summary>
	/// <param name="number">The number to test.</param>
	/// <returns><see langword="true"/> if the number is prime; otherwise <see langword="false"/>.</returns>
	public static bool IsPrime(this int number)
	{
		if ((number % 2) == 0)
		{
			return number == 2;
		}
		int sqrt = (int)Math.Sqrt(number);
		for (int t = 3; t <= sqrt; t = t + 2)
		{
			if (number % t == 0)
			{
				return false;
			}
		}
		return number != 1;
	}

	#region Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough information. Works with int, double, float.

	public static double StdDevP(this IEnumerable<int> source)
	{
		return StdDevLogic(source, 0);
	}

	public static double StdDevP(this IEnumerable<double> source)
	{
		return StdDevLogic(source, 0);
	}

	public static double StdDevP(this IEnumerable<float> source)
	{
		return StdDevLogic(source, 0);
	}

	public static double StdDev(this IEnumerable<int> source)
	{
		return StdDevLogic(source);
	}

	public static double StdDev(this IEnumerable<double> source)
	{
		return StdDevLogic(source);
	}

	public static float StdDev(this IEnumerable<float> source)
	{
		return StdDevLogic(source);
	}

	private static double StdDevLogic(this IEnumerable<double> source, int buffer = 1)
	{
		if (source == null)
		{ throw new ArgumentNullException(nameof(source)); }

		var data = source.ToList();
		var average = data.Average();
		var differences = data.Select(u => Math.Pow(average - u, 2.0)).ToList();
		return Math.Sqrt(differences.Sum() / (differences.Count() - buffer));
	}

	private static double StdDevLogic(this IEnumerable<int> source, int buffer = 1)
	{
		return StdDevLogic(source.Select(x => (double)x));
	}

	private static float StdDevLogic(this IEnumerable<float> source, int buffer = 1)
	{
		if (source == null)
		{ throw new ArgumentNullException(nameof(source)); }
		var data = source.ToList();
		var average = data.Average();
		var differences = data.Select(u => Math.Pow(average - u, 2.0)).ToList();
		return (float)Math.Sqrt(differences.Sum() / (differences.Count() - buffer));
	}
	#endregion


}
