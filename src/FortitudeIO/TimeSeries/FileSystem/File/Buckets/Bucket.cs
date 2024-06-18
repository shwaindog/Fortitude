// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BucketHeader
{
    public static readonly long LowestBucketGranularityTickDivisor = TimeSpan.FromHours(1).Ticks;

    public long  CreatedDateTime;
    public long  LastAmendedDateTime;
    public ulong TotalFileDataSizeBytes;
    public uint  TotalHeadersBytes;
    public uint  TotalFileIndexBytes;
    public uint  TotalDataEntriesCount;
    public uint  PeriodStartTime;
    public uint  BucketId;

    public BucketFlags      BucketFlags;
    public TimeSeriesPeriod TimeSeriesPeriod;
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
  , StorageSizeFailure
}

public interface IBucket : IDisposable
{
    uint        BucketId         { get; }
    BucketFlags BucketFlags      { get; }
    long        FileCursorOffset { get; }

    TimeSeriesPeriod TimeSeriesPeriod { get; }

    DateTime PeriodStartTime                { get; }
    uint     BucketHeaderSizeBytes          { get; }
    bool     IsOpen                         { get; }
    DateTime CreatedDateTime                { get; }
    DateTime LastAmendedDateTime            { get; }
    uint     TotalDataEntriesCount          { get; }
    long     EndAllHeadersSectionFileOffset { get; }
    ulong    TotalFileDataSizeBytes         { get; }
    uint     TotalHeadersSizeBytes          { get; }
    uint     TotalFileIndexSizeBytes        { get; }
    Type     ExpectedEntryType              { get; }
    void     RefreshViews(ShiftableMemoryMappedFileView? usingMappedFileView = null);
    bool     Intersects(DateTime? fromTime = null, DateTime? toTime = null);
    bool     BucketIntersects(TimeRange? period = null);

    StorageAttemptResult CheckTimeSupported(DateTime storageDateTime);

    void    CloseBucketFileViews();
    IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false);
    void    VisitChildrenCacheAndClose();
}

public interface IBucket<TEntry> : IBucket where TEntry : ITimeSeriesEntry<TEntry>
{
    IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext);
}

public interface IMutableBucket : IBucket
{
    bool            IsOpenForAppend         { get; }
    new uint        BucketId                { get; set; }
    new BucketFlags BucketFlags             { get; set; }
    new DateTime    CreatedDateTime         { get; set; }
    new DateTime    LastAmendedDateTime     { get; set; }
    new DateTime    PeriodStartTime         { get; set; }
    new uint        TotalDataEntriesCount   { get; set; }
    new ulong       TotalFileDataSizeBytes  { get; set; }
    new uint        TotalHeadersSizeBytes   { get; set; }
    new uint        TotalFileIndexSizeBytes { get; set; }
    uint            CreateBucketId(uint previousHighestBucketId);
    void            SetEntrySerializer(IMessageSerializer useSerializer);
    void            InitializeNewBucket(DateTime containingTime);
    long            CalculateBucketEndFileOffset();
}

public interface IMutableBucket<TEntry> : IBucket<TEntry>, IMutableBucket where TEntry : ITimeSeriesEntry<TEntry>
{
    IStorageTimeResolver<TEntry>? StorageTimeResolver { get; set; }

    AppendResult AppendEntry(IAppendContext<TEntry> entry);
}

public interface IBucketNavigation<TBucket> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket
{
    IBucketFactory<TBucket> BucketFactory { get; set; }

    TBucket? CloseAndCreateNextBucket();
}
