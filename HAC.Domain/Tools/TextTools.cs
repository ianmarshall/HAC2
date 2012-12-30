using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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


    }
}
