using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAC.Models.Extensions
{
    public static class DateExtensions
    {
        public static string ToPrettyDate(this DateTime date)
        {
            if (date > DateTime.Now)
            {
                var timeSince = date.Subtract(DateTime.Now);
                if (timeSince.TotalMinutes < 30) return "in a few minutes";
                if (timeSince.TotalMinutes < 120) return "in one hour";
                if (timeSince.TotalHours < 24) return string.Format("in {0} hours", timeSince.Hours);
                if (timeSince.TotalDays == 1) return "tomorrow";
                if (timeSince.TotalDays < 7) return string.Format("in {0} day(s)", timeSince.Days);
                if (timeSince.TotalDays < 14) return "next week";
                if (timeSince.TotalDays < 21) return "in two weeks";
                if (timeSince.TotalDays < 28) return "in three weeks";
                if (timeSince.TotalDays < 60) return "next month";
                if (timeSince.TotalDays < 365) return string.Format("in {0} months", Math.Round(timeSince.TotalDays / 30));
                if (timeSince.TotalDays < 730) return "next year";
                return string.Format("in {0} years", Math.Round(timeSince.TotalDays / 365));
            }
            else
            {
                var timeSince = DateTime.Now.Subtract(date);
                if (timeSince.TotalMinutes < 1) return "a few seconds ago";
                if (timeSince.TotalMinutes < 30) return "a few minutes ago";
                if (timeSince.TotalMinutes < 60) return "less than one hour";
                if (timeSince.TotalMinutes < 120) return "one hour ago";
                if (timeSince.TotalHours < 24) return string.Format("{0} hours ago", timeSince.Hours);
                if (timeSince.TotalDays == 1) return "yesterday";
                if (timeSince.TotalDays < 7) return string.Format("{0} day(s) ago", timeSince.Days);
                if (timeSince.TotalDays < 14) return "last week";
                if (timeSince.TotalDays < 21) return "two week ago";
                if (timeSince.TotalDays < 28) return "three weeks ago";
                if (timeSince.TotalDays < 60) return "one month ago";
                if (timeSince.TotalDays < 365) return string.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
                if (timeSince.TotalDays < 730) return "one year ago";
                return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));
            }
        }



        public static string ToDateString(this DateTime date)
        {
            return date.ToString("dddd, dd MMMM yyyy");
        }
    }

}