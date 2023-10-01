using System;

namespace FortitudeCommon.Configuration.Availability
{
    public struct IntraDayPeriod
    {
        public IntraDayPeriod(TimeSpan startTime, TimeSpan duration) : this()
        {
            StartTime = startTime;
            EndTime = duration;
        }

        public bool WithinStartEndTime(TimeSpan timeOfDay) => timeOfDay >= StartTime && timeOfDay < EndTime;

        public TimeSpan StartTime { get; } // if null carries on from the previous day
        public TimeSpan EndTime { get; } // if null goes until the next day
    }
}