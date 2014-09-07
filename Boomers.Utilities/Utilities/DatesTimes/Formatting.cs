using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.DatesTimes
{
    public class Formatting
    {
        private static readonly DateTime EPOCH = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, 0), DateTimeKind.Utc);
        public static long ToEpochTimestamp(DateTime date)
        {
            TimeSpan diff = date.ToUniversalTime() - EPOCH;
            return (long)diff.TotalSeconds;
        }

        public static DateTime FromIso8601FormattedDateTime(string iso8601DateTime)
        {
            return DateTime.ParseExact(iso8601DateTime, "o", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string ToIso8601FormattedDateTime(DateTime dateTime)
        {
            return dateTime.ToString("o");
        }

        /// <summary>
        /// converts a Epoch time to the Human readable Datetime.
        /// </summary>
        /// <param name="epochTime">Time based in seconds</param>
        /// <returns></returns>
        public static DateTime FromEpochToDateTime(double epochTime)
        {
            return EPOCH.AddSeconds(epochTime);
        }
        public static string Month(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "No Such Month";
            }
        }
        /// <summary>
        /// gets the current day of week from the names
        /// </summary>
        /// <param name="day">Monday-Sunday</param>
        /// <returns>1-7</returns>
        public static int DayOFWeek(string day)
        {
            switch (day.ToLower())
            {
                case "sunday":
                    return 7;
                case "monday":
                    return 1;
                case "tuesday":
                    return 2;
                case "wednesday":
                    return 3;
                case "thursday":
                    return 4;
                case "friday":
                    return 5;
                case "saturday":
                    return 6;
                default:
                    return 0;
            }
        }
        public static int Month(string month)
        {
            switch (month.ToLower())
            {
                case "january":
                    return 1;
                case "february":
                    return 2;
                case "march":
                    return 3;
                case "april":
                    return 4;
                case "may":
                    return 5;
                case "june":
                    return 6;
                case "july":
                    return 7;
                case "august":
                    return 8;
                case "september":
                    return 9;
                case "october":
                    return 10;
                case "november":
                    return 11;
                case "december":
                    return 12;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Gets the number of weeks between each date.
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public static int NumberOfWeeks(System.DateTime dateFrom, System.DateTime dateTo)
        {
            TimeSpan Span = dateTo.Subtract(dateFrom);
            if (Span.Days <= 7)
            {
                if (dateFrom.DayOfWeek > dateTo.DayOfWeek)
                    return 2;
                return 1;
            }

            int Days = Span.Days - 7 + (int)dateFrom.DayOfWeek;
            int WeekCount = 1;
            int DayCount = 0;

            for (WeekCount = 1; DayCount < Days; WeekCount++)
            { DayCount += 7; }

            return WeekCount;
        }
    }
}
