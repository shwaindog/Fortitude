namespace FortitudeIO.TimeSeries.FileSystem.File.Reading;

public struct PeriodRange
{
    public PeriodRange(DateTime? fromTime, DateTime? toTime)
    {
        FromTime = fromTime;
        ToTime = toTime;
    }

    public DateTime? FromTime { get; }
    public DateTime? ToTime { get; }
}

public static class PeriodRangeExtensions
{
    public static bool IntersectsWith(this PeriodRange? periodRange, TimeSeriesPeriod timeSeriesPeriod, DateTime periodStartTime)
    {
        if (periodRange == null) return true;
        var range = periodRange.Value;
        return (range.FromTime < timeSeriesPeriod.PeriodEnd(periodStartTime) || (range.FromTime == null && range.ToTime != null))
               && (range.ToTime > periodStartTime || (range.ToTime == null && range.FromTime != null));
    }
}
