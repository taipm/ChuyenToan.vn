using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.DateTimes
{
    public static class DateTimeExtension
    {
        #region Is<...>

        public static bool IsStartDay(this DateTime value)
        {
            if (value.Hour == 0 && value.Minute == 0 && value.Second == 0) return true;
            return false;
        }
        public static bool IsWeekday(this DateTime value)
        {
            return !value.IsWeekend();
        }
        public static bool IsWeekend(this DateTime value)
        {
            return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
        }

        public static bool IsToday(this DateTime value)
        {
            if (value.Date == DateTime.Now.Date) return true;
            return false;
        }
        public static bool IsYesterday(this DateTime value)
        {
            if (value.Date == DateTime.Now.AddDays(-1).Date) return true;
            return false;
        }
        public static bool IsTomorrow(this DateTime value)
        {
            if (value.Date == DateTime.Now.AddDays(1).Date) return true;
            return false;
        }
        //public static bool IsInThisWeek(this DateTime value)
        //{
        //    if (value.Date == DateTime.Now.Date) return true;
        //    return false;
        //}
        //public static bool IsInLastWeek(this DateTime value)
        //{
        //    int _firstDay
        //    if (value.Date == DateTime.Now.Add(-1).Date) return true;
        //    return false;
        //}
        //public static bool IsInNextWeek(this DateTime value)
        //{
        //    if (value.Date == DateTime.Now.AddDays(1).Date) return true;
        //    return false;
        //}
        public static bool IsInRange(this DateTime currentDate, DateTime beginDate, DateTime endDate)
        {
            return (currentDate >= beginDate && currentDate <= endDate);
        }
        public static Boolean IsLeapDay(this DateTime date)
        {
            return (date.Month == 2 && date.Day == 29);
        }
        public static bool IsLeapYear(this DateTime value)
        {
            return (System.DateTime.DaysInMonth(value.Year, 2) == 29);
        }
        #endregion

        #region Next <....>
        public static DateTime NextDayOfWeek(this DateTime dt, DayOfWeek day)
        {
            var d = new GregorianCalendar().AddDays(dt, -((int)dt.DayOfWeek) + (int)day);
            return (d.Day < dt.Day) ? d.AddDays(7) : d;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns>
        ///     result like 00:00:03
        /// </returns>
        public static TimeSpan TimeElapsed(this DateTime date)
        {
            return DateTime.Now - date;
        }
        /// <summary>
        /// Get the elapsed time since the input DateTime
        /// </summary>
        /// <param name="input">Input DateTime</param>
        /// <returns>Returns a TimeSpan value with the elapsed time since the input DateTime</returns>
        /// <example>
        /// TimeSpan elapsed = dtStart.Elapsed();
        /// </example>
        /// <seealso cref="ElapsedSeconds()"/>
        public static TimeSpan Elapsed(this DateTime input)
        {
            return DateTime.Now.Subtract(input);
        }
        public static int Age(this DateTime dateOfBirth)
        {
            if (DateTime.Today.Month < dateOfBirth.Month ||
            DateTime.Today.Month == dateOfBirth.Month &&
             DateTime.Today.Day < dateOfBirth.Day)
            {
                return DateTime.Today.Year - dateOfBirth.Year - 1;
            }
            else
                return DateTime.Today.Year - dateOfBirth.Year;
        }

        

        /// <summary>
        /// return the length of time between the start and current date
        /// Details
        ///  Author: Sanction10
        /// Submitted on: 9/2/2010 2:09:44 PM
        ///Language: C#
        ///Type: System.DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string LengthOfTime(this DateTime date)
        {
            TimeSpan lengthOfTime = DateTime.Now.Subtract(date);
            if (lengthOfTime.Minutes == 0)
                return lengthOfTime.Seconds.ToString() + "s";
            else if (lengthOfTime.Hours == 0)
                return lengthOfTime.Minutes.ToString() + "m";
            else if (lengthOfTime.Days == 0)
                return lengthOfTime.Hours.ToString() + "h";
            else
                return lengthOfTime.Days.ToString() + "d";
        }

        

        #region 5 useful methods
        public static DateTime AddWeekdays(this DateTime date, int days)
        {
            var sign = days < 0 ? -1 : 1;
            var unsignedDays = Math.Abs(days);
            var weekdaysAdded = 0;
            while (weekdaysAdded < unsignedDays)
            {
                date = date.AddDays(sign);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    weekdaysAdded++;
            }
            return date;
        }
        public static DateTime SetTime(this DateTime date, int hour)
        {
            return date.SetTime(hour, 0, 0, 0);
        }
        public static DateTime SetTime(this DateTime date, int hour, int minute)
        {
            return date.SetTime(hour, minute, 0, 0);
        }
        public static DateTime SetTime(this DateTime date, int hour, int minute, int second)
        {
            return date.SetTime(hour, minute, second, 0);
        }
        public static DateTime SetTime(this DateTime date, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second, millisecond);
        }
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }
        public static string ToString(this DateTime? date)
        {
            return date.ToString(null, DateTimeFormatInfo.CurrentInfo);
        }
        public static string ToString(this DateTime? date, string format)
        {
            return date.ToString(format, DateTimeFormatInfo.CurrentInfo);
        }
        public static string ToString(this DateTime? date, IFormatProvider provider)
        {
            return date.ToString(null, provider);
        }
        public static string ToString(this DateTime? date, string format, IFormatProvider provider)
        {
            if (date.HasValue)
                return date.Value.ToString(format, provider);
            else
                return string.Empty;
        }
        public static string ToRelativeDateString(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.Now);
        }
        public static string ToRelativeDateStringUtc(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.UtcNow);
        }
        private static string GetRelativeDateValue(DateTime date, DateTime comparedTo)
        {
            TimeSpan diff = comparedTo.Subtract(date);
            if (diff.TotalDays >= 365)
                return string.Concat("on ", date.ToString("MMMM d, yyyy"));
            if (diff.TotalDays >= 7)
                return string.Concat("on ", date.ToString("MMMM d"));
            else if (diff.TotalDays > 1)
                return string.Format("{0:N0} days ago", diff.TotalDays);
            else if (diff.TotalDays == 1)
                return "yesterday";
            else if (diff.TotalHours >= 2)
                return string.Format("{0:N0} hours ago", diff.TotalHours);
            else if (diff.TotalMinutes >= 60)
                return "more than an hour ago";
            else if (diff.TotalMinutes >= 5)
                return string.Format("{0:N0} minutes ago", diff.TotalMinutes);
            if (diff.TotalMinutes >= 1)
                return "a few minutes ago";
            else
                return "less than a minute ago";
        }

        #endregion

        #region Calculator for datetime
        private static int DateValue(this DateTime dt)
        {
            return dt.Year * 372 + (dt.Month - 1) * 31 + dt.Day - 1;
        }

        public static int YearsBetween(this DateTime dt, DateTime dt2)
        {
            return dt.MonthsBetween(dt2) / 12;
        }

        public static int YearsBetween(this DateTime dt, DateTime dt2, bool includeLastDay)
        {
            return dt.MonthsBetween(dt2, includeLastDay) / 12;
        }

        public static int YearsBetween(this DateTime dt, DateTime dt2, bool includeLastDay, out int excessMonths)
        {
            int months = dt.MonthsBetween(dt2, includeLastDay);
            excessMonths = months % 12;
            return months / 12;
        }

        public static int MonthsBetween(this DateTime dt, DateTime dt2)
        {
            int months = (dt2.DateValue() - dt.DateValue()) / 31;
            return Math.Abs(months);
        }

        public static int MonthsBetween(this DateTime dt, DateTime dt2, bool includeLastDay)
        {
            if (!includeLastDay) return dt.MonthsBetween(dt2);
            int days;
            if (dt2 >= dt)
                days = dt2.AddDays(1).DateValue() - dt.DateValue();
            else
                days = dt.AddDays(1).DateValue() - dt2.DateValue();
            return days / 31;
        }

        public static int WeeksBetween(this DateTime dt, DateTime dt2)
        {
            return dt.DaysBetween(dt2) / 7;
        }

        public static int WeeksBetween(this DateTime dt, DateTime dt2, bool includeLastDay)
        {
            return dt.DaysBetween(dt2, includeLastDay) / 7;
        }

        public static int WeeksBetween(this DateTime dt, DateTime dt2, bool includeLastDay, out int excessDays)
        {
            int days = dt.DaysBetween(dt2, includeLastDay);
            excessDays = days % 7;
            return days / 7;
        }

        public static int DaysBetween(this DateTime dt, DateTime dt2)
        {
            return (dt2.Date - dt.Date).Duration().Days;
        }

        public static int DaysBetween(this DateTime dt, DateTime dt2, bool includeLastDay)
        {
            int days = dt.DaysBetween(dt2);
            if (!includeLastDay) return days;
            return days + 1;
        }
        #endregion

        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        public static DateTime GetFirstDayOfQuarter(DateTime originalDate)
        {
            return AddQuarters(new DateTime(originalDate.Year, 1, 1), GetQuarter(originalDate) - 1);
        }

        public static DateTime GetLastDayOfQuarter(DateTime originalDate)
        {
            return AddQuarters(new DateTime(originalDate.Year, 1, 1), GetQuarter(originalDate)).AddDays(-1);
        }
        public static DateTime AddQuarters(DateTime originalDate, int quarters)
        {
            return originalDate.AddMonths(quarters * 3);
        }
        public static int GetQuarter(DateTime fromDate)
        {
            int month = fromDate.Month - 1;
            int month2 = Math.Abs(month / 3) + 1;
            return month2;
        }
        public static DateTime TruncateToYearStart(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }
        public static DateTime TruncateToMonthStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
        public static DateTime TruncateToDayStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }
        public static DateTime TruncateToHourStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }
        public static DateTime TruncateToMinuteStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }
        public static DateTime TruncateToSecondStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}
