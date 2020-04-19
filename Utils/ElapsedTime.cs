    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.Utils
{
    public static class ElapsedTime
    {
        public static string GetElapsedTimeHumanReadable(this DateTime datetime)
        {
            TimeSpan ts = DateTime.Now.Subtract(datetime);

            // The trick: make variable contain date and time representing the desired timespan,
            // having +1 in each date component.
            DateTime date = DateTime.MinValue + ts;

            return ProcessPeriod(date.Year - 1, date.Month - 1, "year")
                   ?? ProcessPeriod(date.Month - 1, date.Day - 1, "month")
                   ?? ProcessPeriod(date.Day - 1, date.Hour, "day", "Yesterday")
                   ?? ProcessPeriod(date.Hour, date.Minute, "hour")
                   ?? ProcessPeriod(date.Minute, date.Second, "minute")
                   ?? ProcessPeriod(date.Second, 0, "second")
                   ?? "Right now";
        }

        private static string ProcessPeriod(int value, int subValue, string name, string singularName = null)
        {
            if (value == 0)
            {
                return null;
            }
            if (value == 1)
            {
                if (!String.IsNullOrEmpty(singularName))
                {
                    return singularName;
                }

                string articleSuffix = name[0] == 'h' ? "n" : String.Empty;
                
                return subValue == 0
                    ? String.Format($"A{articleSuffix} {name} ago")
                    : String.Format($"About a{articleSuffix} {name} ago");
            }
            return subValue == 0
                ? String.Format($"{value} {name}s ago")
                : String.Format($"About {value} {name}s ago");
        }
    }
}
