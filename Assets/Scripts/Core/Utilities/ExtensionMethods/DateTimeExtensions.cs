using System;

namespace Utilities.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static string ToHoursMinutesSeconds(this DateTime dateTime)
        {
            return $"{dateTime.Hour}:{dateTime.Minute:00}:{dateTime.Second:00}";
        }

        public static string ToHoursMinutesSeconds(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }
    }
}