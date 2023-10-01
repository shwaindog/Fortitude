using System;

namespace FortitudeCommon.Chronometry
{
    public class TimeContext
    {
        static TimeContext()
        {
            Provider = new HighPrecisionTimeContext();
        }

        public static DateTime UtcNow => Provider.UtcNow;

        public static DateTime LocalTimeNow => Provider.LocalTimeNow;

        public static ITimeContext Provider { get; set; }
    }
}