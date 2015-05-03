using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kunai.DateTimeExt
{
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Converts a regular DateTime to a RFC822 date string.
		/// </summary>
		/// <returns>The specified date formatted as a RFC822 date string.</returns>
		public static string ToRFC822DateString(this DateTime date)
		{
			int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
			string timeZone = "+" + offset.ToString().PadLeft(2, '0');
			if (offset < 0)
			{
				int i = offset * -1;
				timeZone = "-" + i.ToString().PadLeft(2, '0');
			}
			return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
		}

		/// <summary>
		/// Converts a System.DateTime object to Unix timestamp
		/// </summary>
		/// <returns>The Unix timestamp</returns>
		public static long ToUnixTimestamp(this DateTime date)
		{
			DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
			TimeSpan unixTimeSpan = date - unixEpoch;

			return (long)unixTimeSpan.TotalSeconds;
		}

		/// <summary>
		/// Converts <see cref="TimeSpan"/> objects to a simple human-readable string.  Examples: 3.1 seconds, 2 minutes, 4.23 hours, etc.
		/// </summary>
		/// <param name="span">The timespan.</param>
		/// <param name="significantDigits">Significant digits to use for output.</param>
		/// <returns></returns>
		public static string ToHumanTimeString(this TimeSpan span, int significantDigits = 3)
		{
			var format = "G" + significantDigits;
			return span.TotalMilliseconds < 1000 ? span.TotalMilliseconds.ToString(format) + " milliseconds"
				: (span.TotalSeconds < 60 ? span.TotalSeconds.ToString(format) + " seconds"
					: (span.TotalMinutes < 60 ? span.TotalMinutes.ToString(format) + " minutes"
						: (span.TotalHours < 24 ? span.TotalHours.ToString(format) + " hours"
												: span.TotalDays.ToString(format) + " days")));
		}

		public static WeekSelector Weeks(this int value)
		{
			var ws = new WeekSelector();
			ws.ReferenceValue = value;
			return ws;
		}

		public static YearsSelector Years(this int value)
		{
			var ws = new YearsSelector();
			ws.ReferenceValue = value;
			return ws;
		}

		public static DaysSelector Days(this int value)
		{
			var ws = new DaysSelector();
			ws.ReferenceValue = value;
			return ws;
		}


		public static DateTime January(this int day, int year)
		{
			return new DateTime(year, 1, day);
		}

		public static DateTime February(this int day, int year)
		{
			return new DateTime(year, 2, day);
		}

		public static DateTime March(this int day, int year)
		{
			return new DateTime(year, 3, day);
		}

		public static DateTime April(this int day, int year)
		{
			return new DateTime(year, 4, day);
		}

		public static DateTime May(this int day, int year)
		{
			return new DateTime(year, 5, day);
		}

		public static DateTime June(this int day, int year)
		{
			return new DateTime(year, 6, day);
		}

		public static DateTime July(this int day, int year)
		{
			return new DateTime(year, 7, day);
		}

		public static DateTime August(this int day, int year)
		{
			return new DateTime(year, 8, day);
		}

		public static DateTime September(this int day, int year)
		{
			return new DateTime(year, 9, day);
		}

		public static DateTime October(this int day, int year)
		{
			return new DateTime(year, 10, day);
		}

		public static DateTime November(this int day, int year)
		{
			return new DateTime(year, 11, day);
		}

		public static DateTime December(this int day, int year)
		{
			return new DateTime(year, 12, day);
		}

		public static IEnumerable<DateTime> GetDateRangeTo(this DateTime self, DateTime toDate)
		{
			var days = new TimeSpan(toDate.Ticks - self.Ticks).Days;

			return Enumerable.Range(0, days)
.Select(p => self.Date.AddDays(p));
		}

		public static DateTime LastDayOfMonth(this DateTime date)
		{
			var endOfTheMonth = date
				.FirstDayOfMonth()
				.AddMonths(1)
				.AddDays(-1);

			return endOfTheMonth;
		}

		public static DateTime FirstDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		// TODO make it more generic
		public static DateTime NextSunday(this DateTime dt)
		{
			return new GregorianCalendar().AddDays(dt, -((int)dt.DayOfWeek) + 7);
		}

		public static bool IsWeekend(this DateTime value)
		{
			return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
		}
	}

	public abstract class TimeSelector
	{
		protected TimeSpan myTimeSpan;

		internal int ReferenceValue
		{
			set { myTimeSpan = MyTimeSpan(value); }
		}

		public DateTime Ago { get { return DateTime.Now - myTimeSpan; } }
		public DateTime FromNow { get { return DateTime.Now + myTimeSpan; } }
		public DateTime AgoSince(DateTime dt) { return dt - myTimeSpan; }
		public DateTime From(DateTime dt) { return dt + myTimeSpan; }
		protected abstract TimeSpan MyTimeSpan(int refValue);

	}

	public class WeekSelector : TimeSelector
	{
		protected override TimeSpan MyTimeSpan(int refValue) { return new TimeSpan(7 * refValue, 0, 0, 0); }
	}

	public class DaysSelector : TimeSelector
	{
		protected override TimeSpan MyTimeSpan(int refValue) { return new TimeSpan(refValue, 0, 0, 0); }
	}

	public class YearsSelector : TimeSelector
	{
		protected override TimeSpan MyTimeSpan(int refValue)
		{
			return new TimeSpan(365 * refValue, 0, 0, 0);
		}
	}


}
