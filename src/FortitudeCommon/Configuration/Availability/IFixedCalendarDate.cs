using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IFixedCalendarDate : IDateTimeContained
    {
        TimeZoneInfo TimeZone { get; }
        Month Month { get; }
        int DayOfMonth { get; }
        ITimeZoneTimePeriod TimeZoneTimePeriod { get; }
        TimeSpan TimeToNextPeriod(DateTimeOffset fromNow);
    }
}