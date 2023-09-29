using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface ITimeZoneTimePeriod : ITimeContained
    {
        TimeZone TimeZone { get; }
        IntraDayPeriod IntraDayAvailability { get; }
    }
}