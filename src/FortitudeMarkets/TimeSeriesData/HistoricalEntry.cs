namespace FortitudeMarkets.TimeSeriesData;

public enum QualityFlags : byte
{
    Unknown = 0
    , Restart = 1
    , Initializing = 2
    , StartUnexpectedGap = 3
    , ResumeFromUnexpectedGap = 4
    , ContainsGaps = 5
    , Reliable = 6
    , SourceClose = 7
    , Stale = 8
}

[Flags]
public enum ResultFlags : byte
{
    Unknown = 0
    , QualityFlagsMask = 0x0F
    , FromNewBucket = 0x20
    , FromDelta = 0x40
    , ExpectMoreResults = 0x80
}

public struct HistoricalEntry<T>
{
    public QualityFlags QualityFlags => (QualityFlags)(ResultFlags & ResultFlags.QualityFlagsMask);
    public ResultFlags ResultFlags { get; set; }
    public DateTime RequestDateTime { get; set; }
    public T Entry { get; set; }
}

public struct EditableEntry<T>
{
    public QualityFlags QualityFlags { get; set; }
    public TimestampType RecordedTimestampType { get; set; }
    public DateTime RecordedDateTime { get; set; }
    public T Entry { get; set; }
}
