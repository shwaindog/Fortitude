using System;

namespace FortitudeCommon.Configuration.Availability
{
    public class DateTimePeriod : IDateTimePeriod
    {
        public DateTimeOffset Date { get; private set; }
        public IntraDayPeriod IntraDayAvailability { get; private set; }
        public bool DateTimeContained(DateTimeOffset checkDateTime)
        {
            DateTime convertedTime = checkDateTime.UtcDateTime + Date.Offset;
            if (convertedTime.Date != Date.Date) return false;
            return convertedTime.TimeOfDay >= IntraDayAvailability.StartTime &&
                   (IntraDayAvailability.EndTime == null || convertedTime.TimeOfDay < IntraDayAvailability.EndTime);
        }

        public TimeSpan TimeToPeriod(DateTimeOffset fromNow)
        {
            DateTime convertedTime = fromNow.UtcDateTime + Date.Offset;
            TimeSpan dateDiff = Date - convertedTime;
            if (IntraDayAvailability.StartTime == null)
            {
                return TimeSpan.FromDays(365);
            }
            TimeSpan startTime = IntraDayAvailability.StartTime.Value;
            return convertedTime.TimeOfDay - startTime + dateDiff;
        }
    }
}