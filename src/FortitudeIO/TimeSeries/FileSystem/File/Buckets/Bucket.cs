#region

using System.Runtime.InteropServices;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BucketHeader
{
    public uint BucketId;
    public uint ParentBucketId;
    public BucketFlags BucketFlags;
    public TimeSeriesPeriod BucketPeriod;
    public long ParentDeltaFileOffset;
    public long BucketPeriodStart;
    public long PreviousSiblingDeltaFileOffset;
    public long NextSiblingBucketDeltaFileOffset;
    public long CreatedDateTime;
    public long LastAmendedDateTime;
    public uint EntryCount;
    public long SerializedEntriesBytes;
}

public enum StorageAttemptResult
{
    PeriodRangeMatched
    , ForNextTimePeriod
    , ForPreviousTimePeriod
    , StorageTimeNotSupported
    , FileRangeNotSupported
    , BucketSearchRange
    , TypeNotCompatible
    , BucketClosedForAppend
}

public interface IBucket : IDisposable
{
    uint BucketId { get; }
    BucketFlags BucketFlags { get; }
    uint ParentBucketId { get; }
    long ParentDeltaFileOffset { get; }
    long FileCursorOffset { get; }
    TimeSeriesPeriod BucketPeriod { get; }
    DateTime BucketPeriodStart { get; }
    bool IsOpen { get; }
    long PreviousSiblingBucketDeltaFileOffset { get; }
    long NextSiblingBucketDeltaFileOffset { get; }
    DateTime CreatedDateTime { get; }
    DateTime LastAmendedDateTime { get; }
    uint EntryCount { get; }
    long EntriesBufferFileOffset { get; }
    long SerializedEntriesBytes { get; }
    Type ExpectedEntryType { get; }

    bool Intersects(DateTime? fromTime = null, DateTime? toTime = null);
    StorageAttemptResult CheckTimeSupported(DateTime storageDateTime);
    void CloseFileView();
    IBucket OpenBucket(ShiftableMemoryMappedFileView? mappedFileView = null, bool asWritable = false);
}

public interface IBucket<TEntry> : IBucket where TEntry : ITimeSeriesEntry<TEntry>
{
    IEnumerable<TEntry> AllEntries { get; }
    IEnumerable<TEntry> EntriesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null);

    IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromDateTime = null, DateTime? toDateTime = null)
        where TM : class, IVersionedMessage;

    int CopyTo(List<TEntry> destination, DateTime? fromDateTime = null, DateTime? toDateTime = null);
}

public interface IMutableBucket : IBucket
{
    bool IsOpenForAppend { get; }
    new uint BucketId { get; set; }
    new BucketFlags BucketFlags { get; set; }
    new DateTime CreatedDateTime { get; set; }
    new DateTime LastAmendedDateTime { get; set; }
    new DateTime BucketPeriodStart { get; set; }
    new uint ParentBucketId { get; set; }
    new long ParentDeltaFileOffset { get; set; }
    new long PreviousSiblingBucketDeltaFileOffset { get; set; }
    new long NextSiblingBucketDeltaFileOffset { get; set; }
    IBuffer? DataWriterAtAppendLocation { get; }
    uint CreateBucketId(uint previouslyHighestBucketId);
    uint CreateBucketId(uint parentBucketId, uint? previousSiblingBucketId);
    void SetEntrySerializer(IMessageSerializer useSerializer);
    void InitializeNewBucket(DateTime containingTime, IMutableSubBucketContainerBucket? parentBucket = null);
    long CalculateBucketEndFileOffset();
}

public interface IMutableBucket<TEntry> : IBucket<TEntry>, IMutableBucket where TEntry : ITimeSeriesEntry<TEntry>
{
    IStorageTimeResolver<TEntry>? StorageTimeResolver { get; set; }
    StorageAttemptResult AppendEntry(TEntry entry);
}

public interface IBucketNavigation<TBucket> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket
{
    BucketFactory<TBucket> BucketFactory { get; set; }
    TBucket? MoveNext { get; }
    TBucket? CloseAndCreateNextBucket(IMutableSubBucketContainerBucket? parentBucket = null);
}
