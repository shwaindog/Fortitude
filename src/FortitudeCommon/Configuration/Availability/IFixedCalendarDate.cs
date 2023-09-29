using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IFixedCalendarDate : IDateTimeContained
    {
        TimeZone TimeZone { get; }
        Month Month { get; }
        int DayOfMonth { get; }
        ITimeZoneTimePeriod TimeZoneTimePeriod { get; }
        TimeSpan TimeToNextPeriod(DateTimeOffset fromNow);
    }
}