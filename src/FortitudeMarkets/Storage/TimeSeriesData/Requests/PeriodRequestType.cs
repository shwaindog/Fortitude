namespace FortitudeMarkets.Storage.TimeSeriesData.Requests;

public enum PeriodRequestType
{
    Relative
    , FixedPeriod
}

public enum RelativePeriodRange
{
    None
    , OneMinute
    , FifteenMinutes
    , OneHour
    , FourHours
    , OneDay
    , OneWeek
    , OneMonth
    , OneYear
    , SpecifiedTimeSpan
    , SpecifiedDateTime
}

public interface IHistoricalDataRequest
{
    PeriodRequestType RequestType { get; }
    IRequestPeriod PeriodRequest { get; }
}

public interface IRequestPeriod
{
    PeriodRequestType PeriodRequestType { get; }
    TimestampType SearchTimestampType { get; }
}

public interface IFixedPeriodRequest
{
    DateTime FromTime { get; }
    DateTime ToTime { get; }
}

public interface IRelativePeriodRequest : IRequestPeriod
{
    RelativePeriodRange RelativePeriodRange { get; }
}

public interface IRelativeSpecifiedTimespanPeriod : IRelativePeriodRequest
{
    TimeSpan StartTimeFromNow { get; }
}

public interface IRelativeSpecifiedDateTimePeriod : IRelativePeriodRequest
{
    TimeSpan StartTime { get; }
}

public enum ReplyResultsType
{
    AsFastAsPossible
    , AtArrivalSpeed
}

public interface IPagedHistoricalDataRequest : IHistoricalDataRequest
{
    int PageSize { get; }
}
