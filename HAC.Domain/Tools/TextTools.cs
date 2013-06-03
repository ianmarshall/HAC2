using System;
using System.Globalization;
using System.Web.Configuration;

namespace HAC.Domain.Tools
{
    public static class TextTools
    {
        public static string CreateDateSuffix(this DateTime date)
        {
            // Get day...
            var day = date.Day;

            // Get day modulo...
            var dayModulo = day % 10;

            // Convert day to string...
            var suffix = day.ToString(CultureInfo.InvariantCulture);

            // Combine day with correct suffix...
            suffix += (day == 11 || day == 12 || day == 13) ? "th" :
                (dayModulo == 1) ? "st" :
                (dayModulo == 2) ? "nd" :
                (dayModulo == 3) ? "rd" :
                "th";

            // Return result...
            return suffix;
        }

        public static DateTime CorrectTimeZone(this DateTime date)
        {
            return Convert.ToDateTime(date.ToString("d", new CultureInfo(WebConfigurationManager.AppSettings["Culture"])));
        }

        public static string StripHtmlTagByCharArray(string htmlString)
        {
            if (string.IsNullOrEmpty(htmlString))
                return htmlString;

            char[] array = new char[htmlString.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < htmlString.Length; i++)
            {
                char let = htmlString[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

    }
}
