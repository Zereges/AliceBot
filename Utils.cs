using System;
using System.Text;

namespace AliceBot
{
    internal static class Utils
    {
        public static string ToHumanReadable(TimeSpan span)
        {
            StringBuilder builder = new StringBuilder(40);

            void WithPlural(int value, string desc)
            {
                builder.Append(value);
                builder.Append(" ");
                builder.Append(desc);
                if (value != 1)
                    builder.Append("s");
                builder.Append(" ");
            }

            if (span.Days > 0)
                WithPlural(span.Days, "day");

            if (span.Hours > 0)
                WithPlural(span.Hours, "hour");
            WithPlural(span.Minutes, "minute");
            if (span.Days == 0)
                WithPlural(span.Seconds, "second");

            builder.Length -= 1; // last space
            return builder.ToString();
        }
    }
}
