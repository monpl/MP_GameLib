using System;

namespace MPGameLib.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime oneOfSunday = new DateTime(2020, 1, 19);

        public static int ReturnWeekCount(this DateTime target)
        {
            var days = (target - oneOfSunday).Days;

            return days / 7;
        }

        public static bool AlreadyPastWeek(this DateTime target, DateTime target2)
        {
            return target.ReturnWeekCount() > target2.ReturnWeekCount();
        }
    }
}