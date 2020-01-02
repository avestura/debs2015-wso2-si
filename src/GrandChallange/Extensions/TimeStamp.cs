using System;

namespace GrandChallange.Extensions
{
    public class TimeStamp
    {
        public DateTime dateTime { get; set; }

        public TimeStamp(DateTime newDateTime) 
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(newDateTime));
            }

            dateTime = newDateTime;
        }

        public long GetUnixTime() 
        {          
            var dateTimeOffset = new DateTimeOffset(dateTime).ToUniversalTime();
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }
    }
}
