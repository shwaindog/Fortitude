using System;

namespace FortitudeCommon.Configuration.Availability
{
    public struct IntraDayPeriod
    {
        public IntraDayPeriod(TimeSpan? startTime, TimeSpan? duration) : this()
        {
            StartTime = startTime;
            EndTime = duration;
        }

        public TimeSpan? StartTime { get; private set; } // if null carries on from the previous day
        public TimeSpan? EndTime { get; private set; } // if null goes until the next day
    }
}