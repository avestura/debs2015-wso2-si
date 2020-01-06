using System;

namespace GrandChallange.Extensions
{
    public static class TimeStampExtensions
    {
        public static long GetUnixTime(this string dateTimeString)
        {
            if (dateTimeString == null)
            {
                throw new ArgumentNullException(nameof(dateTimeString));
            }

            DateTime dateTime = Convert.ToDateTime(dateTimeString);
            var dateTimeOffset = new DateTimeOffset(dateTime).ToUniversalTime();
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }

        public static long GetUnixTime(this DateTime dateTime)
            => new DateTimeOffset(dateTime).ToUniversalTime().ToUnixTimeMilliseconds();


        public static DateTime ThirtyMinAgo(this DateTime dateTime) => dateTime - TimeSpan.FromMinutes(30);

        public static DateTime FiftyMinsAgo(this DateTime dateTime) => dateTime - TimeSpan.FromMinutes(15);

        public static bool Within30Mins(this DateTime dateTime) => DateTime.Now > (dateTime - TimeSpan.FromMinutes(30));

        public static bool Within15Mins(this DateTime dateTime) => DateTime.Now > (dateTime - TimeSpan.FromMinutes(15));
    }
}
