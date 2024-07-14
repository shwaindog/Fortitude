// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.Session;

public interface ITimeSeriesSession : IDisposable
{
    bool IsOpen { get; }
    void Reopen();

    void Close();
}

public interface IReaderSession<TEntry> : ITimeSeriesSession
    where TEntry : ITimeSeriesEntry<TEntry>
{
    IEnumerable<TEntry> StartReaderContext(IReaderContext<TEntry> readerContext);

    void VisitChildrenCacheAndClose();

    IReaderContext<TEntry> GetAllEntriesReader
    (EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject
      , Func<TEntry>? createNew = null);

    IReaderContext<TEntry> GetEntriesBetweenReader
    (UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null);
}

public struct AppendResult
{
    public AppendResult(StorageAttemptResult storageAttemptResult) => StorageAttemptResult = storageAttemptResult;
    public int?                 SerializedSize       { get; set; }
    public uint?                BucketId             { get; set; }
    public long?                FileOffset           { get; set; }
    public DateTime             StorageTime          { get; set; }
    public StorageAttemptResult StorageAttemptResult { get; set; }
}

public interface IWriterSession<in TEntry> : ITimeSeriesSession where TEntry : ITimeSeriesEntry<TEntry>
{
    AppendResult AppendEntry(TEntry entry);
}
