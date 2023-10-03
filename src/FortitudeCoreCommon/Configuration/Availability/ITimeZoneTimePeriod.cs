using System;

namespace FortitudeCommon.Configuration.Availability
{
    public static class TimeZoneInfoExtensions
    {
        public static readonly TimeZoneInfo NewYork = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
        public static readonly TimeZoneInfo NewZealand = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
    }

    public interface ITimeZoneTimePeriod : ITimeContained
    {
        TimeZoneInfo TimeZone { get; }
        IntraDayPeriod IntraDayAvailability { get; }

    }

    class TimeZoneTimePeriod : ITimeZoneTimePeriod
    {
        public TimeZoneTimePeriod(TimeZoneInfo timeZone, IntraDayPeriod intraDayAvailability)
        {
            TimeZone = timeZone;
            IntraDayAvailability = intraDayAvailability;
        }

        public bool ActiveTime(DateTime checkDateTime)
        {
            var timeZoneTime = TimeZoneInfo.ConvertTimeFromUtc(checkDateTime.ToUniversalTime(), TimeZone);
            return IntraDayAvailability.WithinStartEndTime(timeZoneTime.TimeOfDay);
        }

        public TimeZoneInfo TimeZone { get; }
        public IntraDayPeriod IntraDayAvailability { get; }

        public static readonly TimeZoneTimePeriod NewYorkCloseRestart =
            new TimeZoneTimePeriod(TimeZoneInfoExtensions.NewYork,
                new IntraDayPeriod(TimeSpan.FromHours(17), TimeSpan.FromHours(17).Add(TimeSpan.FromMinutes(10))));
    }
}