namespace FortitudeIO.TimeSeries;

public interface ITimeSeriesSummary
{
    DateTime SummaryStartTime { get; }
    TimeSeriesPeriod SummaryPeriod { get; }
    DateTime SummaryEndTime { get; }
}
