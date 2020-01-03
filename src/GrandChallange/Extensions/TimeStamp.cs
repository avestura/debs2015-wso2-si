using System;

namespace GrandChallange.Extensions
{
    public class TimeStamp
    {
        public long GetUnixTime(string dateTimeString) 
        {
            if (dateTimeString == null)
            {
                throw new ArgumentNullException(nameof(dateTimeString));
            }

            DateTime dateTime = Convert.ToDateTime(dateTimeString);
            var dateTimeOffset = new DateTimeOffset(dateTime).ToUniversalTime();
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }
    }
}
