using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace BibleVerses.Common
{
    public static class DateTimeParser
    {
        private static String[] months = { "january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december" };
        public static DateTime? LogDateTime(string longDateTime)
        {
            if (string.IsNullOrWhiteSpace(longDateTime))
                return null;
            string dtstring = longDateTime.Trim();
            if (dtstring.EndsWith(")") && dtstring.Contains("(")) //format: August 28 2018, 8:00:00 am (MST-AZ)
                return parseEmailDateTime(dtstring);

            if (dtstring.EndsWith("Z")) // utc format: 2019-01-16T21:25:22.629Z
                return parseUTCDateTime(dtstring);
            DateTime parsedDateTime;
            if (DateTime.TryParse(longDateTime, out parsedDateTime))
                return parsedDateTime;
            return null;
        }
        private static DateTime parseUTCDateTime(string dt)
        {
            DateTime localTime = DateTime.Parse(dt).ToLocalTime();
            return localTime;
        }
        private static DateTime parseEmailDateTime(string dt)
        {
            String[] dateParts = dt.Split(' ');

            int month = Array.IndexOf(months, dateParts[0].ToLower()) + 1;
            int day = int.Parse(dateParts[1]);
            int year = int.Parse(dateParts[2].Replace(",", ""));
            String hour = dateParts[3];
            String AmPm = dateParts[4];
            String TimeZone = dateParts[5];

            StringBuilder sb = new StringBuilder();
            sb.Append(month.ToString());
            sb.Append("/");
            sb.Append(day.ToString());
            sb.Append("/");
            sb.Append(year.ToString());
            sb.Append(" ");
            sb.Append(hour);
            sb.Append(" ");
            sb.Append(AmPm);
            //sb.Append(" ");
            //sb.Append(TimeZone);

            return DateTime.Parse(sb.ToString());
        }
        public static TimeSpan GetTimeSpan(string time)
        {
            if (string.IsNullOrWhiteSpace(time))
                return new TimeSpan();
            TimeSpan ts = TimeSpan.ParseExact(time, new string[] { "hhmmss", @"hh\:mm\:ss" }, CultureInfo.InvariantCulture);
            return ts;
        }
    }
}
