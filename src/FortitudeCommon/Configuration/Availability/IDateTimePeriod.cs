using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IDateTimePeriod : IDateTimeContained
    {
        DateTimeOffset Date { get; }
        IntraDayPeriod IntraDayAvailability { get; }
        TimeSpan TimeToPeriod(DateTimeOffset fromNow);
    }
}