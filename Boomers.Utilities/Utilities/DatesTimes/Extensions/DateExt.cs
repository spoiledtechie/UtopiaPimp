using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Boomers.Utilities.DatesTimes;
using System.Text.RegularExpressions;

namespace Boomers.Utilities.DatesTimes
{
    public static class DateExt
    {
        private static Regex rgxGetQuarterYear = new Regex(@"\d{4}Q\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex rgxNumbers = new Regex(@"\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static List<DateTime> ToDateTimesByQuarter(this string yearInQuarters)
        {
            if (rgxGetQuarterYear.IsMatch(yearInQuarters))
            {
                yearInQuarters = rgxGetQuarterYear.Match(yearInQuarters).Value;

                int year = Convert.ToInt32(rgxNumbers.Match(yearInQuarters).Value);
                int month1 = 0, month2 = 0;

                switch (rgxNumbers.Matches(yearInQuarters)[1].Value)
                {
                    case "1":
                        month1 = 1;
                        month2 = 3;
                        break;
                    case "2":
                        month1 = 4;
                        month2 = 6;
                        break;
                    case "3":
                        month1 = 7;
                        month2 = 9;
                        break;
                    case "4":
                        month1 = 10;
                        month2 = 12;
                        break;
                }
                DateTime dt2 = Last(new DateTime(year, month2, 5));
                DateTime dt1 = new DateTime(year, month1, 1);
                var dtList = new List<DateTime>();
                dtList.Add(dt1);
                dtList.Add(dt2);
                return dtList;
            }
            throw new NullReferenceException("Couldn't Find Date in " + yearInQuarters);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in January
        /// in the specified year.
        /// </summary>
        public static DateTime January(this int day, int year)
        {
            return new DateTime(year, 1, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in February
        /// in the specified year.
        /// </summary>
        public static DateTime February(this int day, int year)
        {
            return new DateTime(year, 2, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in March
        /// in the specified year.
        /// </summary>
        public static DateTime March(this int day, int year)
        {
            return new DateTime(year, 3, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in April
        /// in the specified year.
        /// </summary>
        public static DateTime April(this int day, int year)
        {
            return new DateTime(year, 4, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in May
        /// in the specified year.
        /// </summary>
        public static DateTime May(this int day, int year)
        {
            return new DateTime(year, 5, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in June
        /// in the specified year.
        /// </summary>
        public static DateTime June(this int day, int year)
        {
            return new DateTime(year, 6, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in July
        /// in the specified year.
        /// </summary>
        public static DateTime July(this int day, int year)
        {
            return new DateTime(year, 7, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in August
        /// in the specified year.
        /// </summary>
        public static DateTime August(this int day, int year)
        {
            return new DateTime(year, 8, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in September
        /// in the specified year.
        /// </summary>
        public static DateTime September(this int day, int year)
        {
            return new DateTime(year, 9, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in October
        /// in the specified year.
        /// </summary>
        public static DateTime October(this int day, int year)
        {
            return new DateTime(year, 10, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in November
        /// in the specified year.
        /// </summary>
        public static DateTime November(this int day, int year)
        {
            return new DateTime(year, 11, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in December
        /// in the specified year.
        /// </summary>
        public static DateTime December(this int day, int year)
        {
            return new DateTime(year, 12, day);
        }
        public static string ToShortDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        }
        /// <summary>
        /// Gets the week number of the current Date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekNumber(this System.DateTime date)
        {
            GregorianCalendar cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        /// <summary>
        /// Gets the age in years from the entered Date.
        /// </summary>
        /// <param name="dob">Date of Birth of person.</param>
        /// <returns>difference in years.</returns>
        public static int GetAge(this System.DateTime dob)
        {
            // Get year diff
            int years = System.DateTime.Now.Year - dob.Year;
            // Add year diff to birth day
            dob = dob.AddYears(years);

            // Subtract another year if its one day before the birth day
            if (System.DateTime.Today.CompareTo(dob) < 0) { years--; }
            return years;
        }

        /// <summary>
        /// Gets the weeks in the current year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int WeeksInYear(this System.DateTime date)
        {
            int year = date.Year;
            GregorianCalendar cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
            return cal.GetWeekOfYear(new System.DateTime(year, 12, 28), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static DateTime CurrentTimeToHour(this System.DateTime dateTime)
        {
            return dateTime.AddMinutes((DateTime.Now.Minute * -1)).AddSeconds(DateTime.Now.Second * -1);
        }
        public static TimeSpan TimeLeftInHour(this System.DateTime dateTime)
        {
            return dateTime.AddHours(1).AddMinutes(DateTime.UtcNow.Minute * -1).AddSeconds(DateTime.UtcNow.Second * -1).Subtract(dateTime);
        }
        /// <summary>
        /// Converts DateTime to yyyyMMdd Format
        /// </summary>
        /// <param name="datetime">Date and Time  to convert.</param>
        /// <returns></returns>
        public static string ToyyyyMMdd(this DateTime datetime)
        {
            return datetime.ToString("yyyyMMdd");
        }
        public static string ToyyyyMMddHHmmss(this DateTime datetime)
        {
            return datetime.ToString("yyyyMMddHHmmss");
        }
        public static string ToyyyyMMddHHmmssDashes(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd-HH-mm-ss");
        }
        /// <summary>
        /// gets the relative time from date and time entered.
        /// </summary>
        /// <param name="date">Date to format.</param>
        /// <returns>about date days/hours/minutes ago.</returns>
        public static string ToLongRelativeDate(this DateTime date)
        {
            TimeSpan span = System.DateTime.UtcNow.Subtract(date);
            if (span <= TimeSpan.FromSeconds(60))
                return span.Seconds + " seconds ago";
            else if (span <= TimeSpan.FromMinutes(60))
                if (span.Minutes > 1)
                    return span.Minutes + " minutes ago";
                else
                    return "a minute ago";
            else if (span <= TimeSpan.FromHours(24))
                if (span.Hours > 1)
                    return span.Hours + " hours ago";
                else
                    return "an hour ago";
            else if (span <= TimeSpan.FromDays(31))
                if (span.Days > 1)
                    return span.Days + " days ago";
                else
                    return "a day ago";
            else if (span <= TimeSpan.FromDays(356))
                if (span.Days > 62)
                    return span.Days / 31 + " months ago";
                else
                    return "a month ago";
            else
                if (span.Days > 712)
                    return span.Days / 356 + " years ago";
                else
                    return "a year ago";
        }
        public static string ToLongRelativeDate(this TimeSpan span)
        {
            if (span <= TimeSpan.FromSeconds(60))
                return span.Seconds + " seconds ago";
            else if (span <= TimeSpan.FromMinutes(60))
                if (span.Minutes > 1)
                    return span.Minutes + " minutes ago";
                else
                    return "a minute ago";
            else if (span <= TimeSpan.FromHours(24))
                if (span.Hours > 1)
                    return span.Hours + " hours ago";
                else
                    return "an hour ago";
            else if (span <= TimeSpan.FromDays(31))
                if (span.Days > 1)
                    return span.Days + " days ago";
                else
                    return "a day ago";
            else if (span <= TimeSpan.FromDays(356))
                if (span.Days > 62)
                    return span.Days / 31 + " months ago";
                else
                    return "a month ago";
            else
                if (span.Days > 712)
                    return span.Days / 356 + " years ago";
                else
                    return "a year ago";
        }

        public static string ToRelativeDate(this TimeSpan span)
        {
            if (span <= TimeSpan.FromSeconds(60))
                return span.Seconds + "s";
            else if (span <= TimeSpan.FromMinutes(60))
                return span.Minutes + "m " + span.Seconds + "s";
            else if (span <= TimeSpan.FromHours(24))
                return span.Hours + "h " + span.Minutes + "m";
            else if (span <= TimeSpan.FromDays(31))
                return span.Days + "d " + span.Hours + "h";
            else if (span <= TimeSpan.FromDays(356))
                return span.Days / 31 + "m " + span.Days + "d";
            else
                return span.Days / 356 + "y " + span.Days / 31 + "m";
        }
        public static string ToRelativeDate(this DateTime date)
        {
            TimeSpan span = System.DateTime.UtcNow.Subtract(date);
            if (span <= TimeSpan.FromSeconds(60))
                return span.Seconds + "s";
            else if (span <= TimeSpan.FromMinutes(60))
                return span.Minutes + "m " + span.Seconds + "s";
            else if (span <= TimeSpan.FromHours(24))
                return span.Hours + "h " + span.Minutes + "m";
            else if (span <= TimeSpan.FromDays(31))
                return span.Days + "d " + span.Hours + "h";
            else if (span <= TimeSpan.FromDays(356))
                return span.Days / 31 + "m " + span.Days + "d";
            else
                return span.Days / 356 + "y " + span.Days / 31 + "m";
        }
        public static string ToShortRelativeDate(this DateTime date)
        {
            TimeSpan span = System.DateTime.UtcNow.Subtract(date);
            if (span <= TimeSpan.FromSeconds(60))
                return span.Seconds + "s";
            else if (span <= TimeSpan.FromMinutes(60))
                return span.Minutes + "." + span.Seconds + "m";
            else if (span <= TimeSpan.FromHours(24))
                return span.Hours + ":" + span.Minutes + "h";
            else if (span <= TimeSpan.FromDays(31))
                return span.Days + "." + span.Hours + "d";
            else if (span <= TimeSpan.FromDays(356))
                return span.Days / 31 + "." + span.Days + "m";
            else
                return span.Days / 356 + "." + span.Days / 31 + "y";
        }
        public static string ToShortRelativeDate(this TimeSpan span)
        {
            if (span <= TimeSpan.FromSeconds(60))
                return span.Seconds + "s";
            else if (span <= TimeSpan.FromMinutes(60))
                return span.Minutes + "." + span.Seconds + "m";
            else if (span <= TimeSpan.FromHours(24))
                return span.Hours + ":" + span.Minutes + "h";
            else if (span <= TimeSpan.FromDays(31))
                return span.Days + "." + span.Hours + "d";
            else if (span <= TimeSpan.FromDays(356))
                return span.Days / 31 + "." + span.Days + "m";
            else
                return span.Days / 356 + "." + span.Days / 31 + "y";
        }

        /// <summary>
        /// Gets a DateTime representing the first day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current)
        {
            DateTime first = current.AddDays(1 - current.Day);
            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the first specified day in the current month
        /// </summary>
        /// <param name="current">The current day</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime first = current.First();

            if (first.DayOfWeek != dayOfWeek)
            {
                first = first.Next(dayOfWeek);
            }

            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current)
        {
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);

            DateTime last = current.First().AddDays(daysInMonth - 1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the last specified day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime last = current.Last();

            last = last.AddDays(Math.Abs(dayOfWeek - last.DayOfWeek) * -1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the first date following the current date which falls on the given day of the week
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The day of week for the next date to get</param>
        public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;

            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }

            DateTime result = current.AddDays(offsetDays);
            return result;
        }
    }
}