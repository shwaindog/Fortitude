// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session.Retrieval;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.Session;

public interface ITimeSeriesSession : IDisposable
{
    bool IsOpen { get; }
    void Reopen();

    void Close();
}

public interface IReaderSession<TEntry> : ITimeSeriesSession
    where TEntry : ITimeSeriesEntry
{
    IEnumerable<TEntry> StartReaderContext(IReaderContext<TEntry> readerContext);

    void VisitChildrenCacheAndClose();

    IReaderContext<TEntry> AllChronologicalEntriesReader
    (IRecycler resultRecycler, EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject
      , ReaderOptions readerOptions = ReaderOptions.ConsumerControlled
      , Func<TEntry>? createNew = null);

    IReaderContext<TEntry> ChronologicalEntriesBetweenTimeRangeReader
    (IRecycler resultRecycler, UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject
      , ReaderOptions readerOptions = ReaderOptions.ConsumerControlled
      , Func<TEntry>? createNew = null);

    IReaderContext<TEntry> AllChronologicalEntriesReader<TConcreteEntry>
    (IRecycler resultRecycler, EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject
      , ReaderOptions readerOptions = ReaderOptions.ConsumerControlled) where TConcreteEntry : class, TEntry, ITimeSeriesEntry, new();

    IReaderContext<TEntry> ChronologicalEntriesBetweenTimeRangeReader<TConcreteEntry>
    (IRecycler resultRecycler, UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject
      , ReaderOptions readerOptions = ReaderOptions.ConsumerControlled) where TConcreteEntry : class, TEntry, ITimeSeriesEntry, new();


    public IReaderContext<TEntry> AllReverseChronologicalEntriesReader
    (IRecycler resultsRecycler, EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null);

    public IReaderContext<TEntry> ReverseChronologicalEntriesBetweenTimeRangeReader
    (IRecycler resultsRecycler, UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null);

    public IReaderContext<TEntry> AllReverseChronologicalEntriesReader<TConcreteEntry>
        (IRecycler resultsRecycler, EntryResultSourcing entryResultSourcing = EntryResultSourcing.FromRecycler)
        where TConcreteEntry : class, TEntry, ITimeSeriesEntry, new();

    public IReaderContext<TEntry> ReverseChronologicalEntriesBetweenTimeRangeReader<TConcreteEntry>
    (IRecycler resultsRecycler, UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.FromRecycler)
        where TConcreteEntry : class, TEntry, ITimeSeriesEntry, new();
}

public struct AppendResult
{
    public AppendResult(StorageAttemptResult storageAttemptResult) => StorageAttemptResult = storageAttemptResult;

    public StorageAttemptResult StorageAttemptResult { get; set; }

    public int?     SerializedSize { get; set; }
    public uint?    BucketId       { get; set; }
    public long?    FileOffset     { get; set; }
    public DateTime StorageTime    { get; set; }
}

public interface IWriterSession<in TEntry> : ITimeSeriesSession where TEntry : ITimeSeriesEntry
{
    AppendResult AppendEntry(TEntry entry);
}
