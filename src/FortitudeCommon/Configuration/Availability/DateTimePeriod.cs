namespace FortitudeCommon.Configuration.Availability;

public class DateTimePeriod : IDateTimePeriod
{
    public DateTimeOffset Date { get; }
    public IntraDayPeriod IntraDayAvailability { get; }

    public bool DateTimeContained(DateTimeOffset checkDateTime)
    {
        var convertedTime = checkDateTime.UtcDateTime + Date.Offset;
        if (convertedTime.Date != Date.Date) return false;
        return convertedTime.TimeOfDay >= IntraDayAvailability.StartTime &&
               (IntraDayAvailability.EndTime == default || convertedTime.TimeOfDay < IntraDayAvailability.EndTime);
    }

    public TimeSpan TimeToPeriod(DateTimeOffset fromNow)
    {
        var convertedTime = fromNow.UtcDateTime + Date.Offset;
        var dateDiff = Date - convertedTime;
        if (IntraDayAvailability.StartTime == default) return TimeSpan.FromDays(365);
        var startTime = IntraDayAvailability.StartTime;
        return convertedTime.TimeOfDay - startTime + dateDiff;
    }
}
