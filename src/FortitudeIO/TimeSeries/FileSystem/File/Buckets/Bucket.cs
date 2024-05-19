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
    public static readonly long LowestBucketGranularityTickDivisor = TimeSpan.FromHours(1).Ticks;

    public uint BucketId;
    public uint ParentBucketId;
    public BucketFlags BucketFlags;
    public TimeSeriesPeriod TimeSeriesPeriod;
    public long ParentDeltaFileOffset;
    public long PreviousSiblingDeltaFileOffset;
    public long NextSiblingBucketDeltaFileOffset;
    public long CreatedDateTime;
    public long LastAmendedDateTime;
    public ulong DataSizeBytes;
    public uint DataEntriesCount;
    public uint PeriodStartTime;
}

public enum StorageAttemptResult
{
    EntryNotCompatible
    , NoBucketChecked
    , PeriodRangeMatched
    , NextBucketPeriod
    , StorageTimeNotSupported
    , NextFilePeriod
    , CalculateFilePeriod
    , BucketSearchRange
    , BucketClosedForAppend
}

public interface IBucket : IDisposable
{
    uint BucketId { get; }
    BucketFlags BucketFlags { get; }
    uint ParentBucketId { get; }
    long ParentDeltaFileOffset { get; }
    long FileCursorOffset { get; }
    TimeSeriesPeriod TimeSeriesPeriod { get; }
    DateTime PeriodStartTime { get; }
    uint BucketHeaderSizeBytes { get; }
    bool IsOpen { get; }
    long PreviousSiblingBucketDeltaFileOffset { get; }
    long NextSiblingBucketDeltaFileOffset { get; }
    DateTime CreatedDateTime { get; }
    DateTime LastAmendedDateTime { get; }
    uint DataEntriesCount { get; }
    long BucketDataStartFileOffset { get; }
    ulong DataSizeBytes { get; }
    uint NonDataSizeBytes { get; }
    Type ExpectedEntryType { get; }

    bool Intersects(DateTime? fromTime = null, DateTime? toTime = null);
    StorageAttemptResult CheckTimeSupported(DateTime storageDateTime);
    void CloseFileView();
    IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false);
}

public interface IBucket<TEntry> : IBucket where TEntry : ITimeSeriesEntry<TEntry>
{
    IEnumerable<TEntry> AllBucketEntriesFrom(long? fromFileCursorOffset = null);
    IEnumerable<TEntry> EntriesBetween(DateTime? fromTime = null, DateTime? toTime = null);

    IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromTime = null, DateTime? toTime = null)
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
    new DateTime PeriodStartTime { get; set; }
    new uint ParentBucketId { get; set; }
    new long ParentDeltaFileOffset { get; set; }
    new long PreviousSiblingBucketDeltaFileOffset { get; set; }
    new long NextSiblingBucketDeltaFileOffset { get; set; }
    new uint DataEntriesCount { get; set; }
    new ulong DataSizeBytes { get; set; }
    new uint NonDataSizeBytes { get; set; }
    IBuffer? DataWriterAtAppendLocation { get; }
    uint CreateBucketId(uint previousHighestBucketId);
    void SetEntrySerializer(IMessageSerializer useSerializer);
    void InitializeNewBucket(DateTime containingTime);
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
    TBucket? CloseAndCreateNextBucket();
}
