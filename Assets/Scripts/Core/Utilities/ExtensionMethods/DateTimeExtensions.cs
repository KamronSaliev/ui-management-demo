using System;

namespace Utilities.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static string ToHoursMinutesSeconds(this DateTime dateTime)
        {
            return $"{dateTime.Hour:00}:{dateTime.Minute:00}:{dateTime.Second:00}";
        }

        public static string ToHoursMinutesSeconds(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }
    }
}