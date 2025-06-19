#region

using FortitudeMarkets.Storage.TimeSeriesData.Requests;
using FortitudeMarkets.Storage.TimeSeriesData.Responses;

#endregion

namespace FortitudeMarkets.Storage.TimeSeriesData;

public enum TimeSeriesStorageType
{
    Unknown
    , CommaSeparatedFile
    , JsonFile
    , FortitudeIndexedFlatFile
    , FortitudeDirectoryHierarchy
    , RelationalDatabase
    , TimeSeriesDatabase
    , RemoteServiceRequest
    , AggregatedFeeds
}

public enum TimeSeriesFeedType
{
    SingleDataPoint
    , DoubleDataPoint
    , TripleDataPoint
    , Level0Quote
    , Level1Quote
    , Level2Quote
    , Level3Quote
    , Orders
    , Trades
    , Annotations
}

public enum TimestampType
{
    DefaultRecordTimestamp
    , SourceTimestamp
    , AdapterTimestamp
    , ClientReceivedTimestamp
}

public interface IHistoricalDataFeed
{
    string FeedName { get; }
    TimeSeriesStorageType StorageType { get; }
    TimestampType StorageTimestampType { get; }
    DateTime EarliestRecordedEntry { get; }
}

public interface IRetrievableHistoricalDataFeed<T>
{
    IEnumerable<HistoricalEntry<T>> TimeSeriesAsEnumerable(IHistoricalDataRequest request);
    IPagedCollectionResult<HistoricalEntry<T>> TimeSeriesAsPageCollection(IHistoricalDataRequest request);
    IObservable<HistoricalEntry<T>> TimeSeriesAsObservable(IHistoricalDataRequest request, ReplyResultsType replyResultsType);
    HistoricalEntry<T>? FindClosestEntryAt(DateTime atOrGreaterThan);
}

public interface IEditableHistoricalDataFeed<T> : IRetrievableHistoricalDataFeed<T>
{
    bool AppendNewEntry(EditableEntry<T> entry);
    HistoricalEntry<T> EditExisting(HistoricalEntry<T> toEdit, EditableEntry<T> updatedEntry);
    HistoricalEntry<T> EditExistingQualityFlags(HistoricalEntry<T> toEdit, QualityFlags updatedQualityFlags);
}
