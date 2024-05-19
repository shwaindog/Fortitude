#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public abstract unsafe class DataBucket<TEntry, TBucket> : IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    protected readonly IMutableBucketContainer BucketContainer;
    protected readonly IBucketTrackingTimeSeriesFile ContainingFile;
    protected ShiftableMemoryMappedFileView? BucketAppenderFileView;
    protected BucketIndexInfo* BucketContainerIndexEntry;
    protected ShiftableMemoryMappedFileView? BucketHeaderFileView;
    private BucketHeader cacheBucketHeader;
    protected IMessageSerializer? EntrySerializer;
    private MemoryMappedFileBuffer? fileBuffer;
    protected long HeaderRealignmentDelta;
    private BucketHeader* mappedFileBucketInfo;
    private long requiredViewFileCursorOffset;
    private byte* requiredViewVirtualMemoryLocation;
    protected bool Writable;

    protected DataBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, IMutableBucketContainer? parentBucket = null)
    {
        BucketContainer = bucketContainer;
        ContainingFile = bucketContainer.ContainingTimeSeriesFile;
        ContainingFile.CurrentlyOpenBucket = this;
        HeaderRealignmentDelta = bucketFileCursorOffset % 8;
        FileCursorOffset = bucketFileCursorOffset + HeaderRealignmentDelta;
        // ReSharper disable VirtualMemberCallInConstructor
        OpenBucket(asWritable: writable);
        // ReSharper restore VirtualMemberCallInConstructor
    }

    protected virtual long EndOfBucketHeaderSectionOffset => FileCursorOffset + sizeof(BucketHeader);

    protected bool CanUseWritableBufferInfo =>
        IsOpen && mappedFileBucketInfo != null
               && BucketHeaderFileView!.LowerHalfViewVirtualMemoryAddress == requiredViewVirtualMemoryLocation
               && BucketHeaderFileView!.LowerViewFileCursorOffset == requiredViewFileCursorOffset;

    public BucketFactory<TBucket> BucketFactory { get; set; }

    public TBucket? MoveNext =>
        NextSiblingBucketDeltaFileOffset != 0 ?
            BucketFactory.OpenExistingBucket(BucketContainer.ContainingTimeSeriesFile, FileCursorOffset + NextSiblingBucketDeltaFileOffset
                , Writable) :
            default;

    public virtual TBucket? CloseAndCreateNextBucket()
    {
        if (!Writable) return null;
        var nextStartPeriod = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
        var nextPeriodResult = CheckTimeSupported(nextStartPeriod);
        if (nextPeriodResult == StorageAttemptResult.NextBucketPeriod)
        {
            BucketFlags = BucketFlags.BucketClosedGracefully | BucketFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            // TODO SubBucketOnlyBucket needs to add up all sizes of containing buckets when closing.
            var bucketEndCursor = BucketFactory.AppendCloseBucketDelimiter(BucketAppenderFileView!, CalculateBucketEndFileOffset());
            var nextBucket = BucketFactory.CreateNewBucket(BucketContainer, bucketEndCursor, nextStartPeriod, true);
            CloseFileView();
            return nextBucket;
        }

        return null;
    }

    public virtual uint BucketHeaderSizeBytes => (uint)(HeaderRealignmentDelta + (EndOfBucketHeaderSectionOffset - FileCursorOffset));

    public Type ExpectedEntryType => typeof(TEntry);

    public uint BucketId
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.BucketId;
            cacheBucketHeader.BucketId = mappedFileBucketInfo->BucketId;
            return cacheBucketHeader.BucketId;
        }
        set
        {
            if (value == cacheBucketHeader.BucketId || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketId = value;
            cacheBucketHeader.BucketId = value;
        }
    }

    public uint ParentBucketId
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.ParentBucketId;
            cacheBucketHeader.ParentBucketId = mappedFileBucketInfo->ParentBucketId;
            return cacheBucketHeader.ParentBucketId;
        }
        set
        {
            if (value == cacheBucketHeader.ParentBucketId || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->ParentBucketId = value;
            cacheBucketHeader.ParentBucketId = value;
        }
    }

    public long ParentDeltaFileOffset
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.ParentDeltaFileOffset;
            cacheBucketHeader.ParentDeltaFileOffset = mappedFileBucketInfo->ParentDeltaFileOffset;
            return cacheBucketHeader.ParentDeltaFileOffset;
        }
        set
        {
            if (value == cacheBucketHeader.ParentDeltaFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->ParentDeltaFileOffset = value;
            cacheBucketHeader.ParentDeltaFileOffset = value;
        }
    }

    public long FileCursorOffset { get; }

    public BucketFlags BucketFlags
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.BucketFlags;
            cacheBucketHeader.BucketFlags = mappedFileBucketInfo->BucketFlags;
            return cacheBucketHeader.BucketFlags;
        }
        set
        {
            if (value == cacheBucketHeader.BucketFlags || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketFlags = value;
            cacheBucketHeader.BucketFlags = value;
        }
    }

    public abstract TimeSeriesPeriod TimeSeriesPeriod { get; }

    public bool Intersects(DateTime? fromTime = null, DateTime? toTime = null) =>
        (PeriodStartTime < toTime || (toTime == null && fromTime != null))
        && (TimeSeriesPeriod.PeriodEnd(PeriodStartTime) > fromTime || (fromTime == null && toTime != null));

    public DateTime PeriodStartTime
    {
        get
        {
            if (!CanUseWritableBufferInfo)
                return DateTime.FromBinary(cacheBucketHeader.PeriodStartTime * BucketHeader.LowestBucketGranularityTickDivisor);
            cacheBucketHeader.PeriodStartTime = mappedFileBucketInfo->PeriodStartTime;
            return DateTime.FromBinary(cacheBucketHeader.PeriodStartTime * BucketHeader.LowestBucketGranularityTickDivisor);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.PeriodStartTime || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->PeriodStartTime = (uint)(value.Ticks / BucketHeader.LowestBucketGranularityTickDivisor);
            cacheBucketHeader.PeriodStartTime = (uint)(value.Ticks / BucketHeader.LowestBucketGranularityTickDivisor);
        }
    }

    public long PreviousSiblingBucketDeltaFileOffset
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.PreviousSiblingDeltaFileOffset;
            cacheBucketHeader.PreviousSiblingDeltaFileOffset = mappedFileBucketInfo->PreviousSiblingDeltaFileOffset;
            return cacheBucketHeader.PreviousSiblingDeltaFileOffset;
        }

        set
        {
            if (value == cacheBucketHeader.PreviousSiblingDeltaFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->PreviousSiblingDeltaFileOffset = value;
            cacheBucketHeader.PreviousSiblingDeltaFileOffset = value;
        }
    }

    public long NextSiblingBucketDeltaFileOffset
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.NextSiblingBucketDeltaFileOffset;
            cacheBucketHeader.NextSiblingBucketDeltaFileOffset = mappedFileBucketInfo->NextSiblingBucketDeltaFileOffset;
            return cacheBucketHeader.NextSiblingBucketDeltaFileOffset;
        }

        set
        {
            if (value == cacheBucketHeader.NextSiblingBucketDeltaFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->NextSiblingBucketDeltaFileOffset = value;
            cacheBucketHeader.NextSiblingBucketDeltaFileOffset = value;
        }
    }

    public DateTime CreatedDateTime
    {
        get
        {
            if (!CanUseWritableBufferInfo) return DateTime.FromBinary(cacheBucketHeader.CreatedDateTime);
            cacheBucketHeader.CreatedDateTime = mappedFileBucketInfo->CreatedDateTime;
            return DateTime.FromBinary(cacheBucketHeader.CreatedDateTime);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.CreatedDateTime || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->CreatedDateTime = value.Ticks;
            cacheBucketHeader.CreatedDateTime = value.Ticks;
        }
    }

    public DateTime LastAmendedDateTime
    {
        get
        {
            if (!CanUseWritableBufferInfo) return DateTime.FromBinary(cacheBucketHeader.LastAmendedDateTime);
            cacheBucketHeader.LastAmendedDateTime = mappedFileBucketInfo->LastAmendedDateTime;
            return DateTime.FromBinary(cacheBucketHeader.LastAmendedDateTime);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.LastAmendedDateTime || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->LastAmendedDateTime = value.Ticks;
            cacheBucketHeader.LastAmendedDateTime = value.Ticks;
        }
    }

    public uint DataEntriesCount
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.DataEntriesCount;
            cacheBucketHeader.DataEntriesCount = mappedFileBucketInfo->DataEntriesCount;
            return cacheBucketHeader.DataEntriesCount;
        }

        set
        {
            if (value == cacheBucketHeader.DataEntriesCount || !Writable || !CanUseWritableBufferInfo) return;
            if (BucketContainerIndexEntry != null)
            {
                var deltaEntries = value - mappedFileBucketInfo->DataEntriesCount;
                BucketContainerIndexEntry->NumEntries += deltaEntries;
                BucketContainer.DataEntriesCount += deltaEntries;
            }

            mappedFileBucketInfo->DataEntriesCount = value;
            cacheBucketHeader.DataEntriesCount = value;
        }
    }

    public virtual long BucketDataStartFileOffset => EndOfBucketHeaderSectionOffset;

    public ulong DataSizeBytes
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.DataSizeBytes;
            cacheBucketHeader.DataSizeBytes = mappedFileBucketInfo->DataSizeBytes;
            return cacheBucketHeader.DataSizeBytes;
        }

        set
        {
            if (value == cacheBucketHeader.DataSizeBytes || !Writable || !CanUseWritableBufferInfo) return;
            if (BucketContainerIndexEntry != null)
            {
                var deltaBytes = value - mappedFileBucketInfo->DataSizeBytes;
                BucketContainerIndexEntry->DataSizeBytes += deltaBytes;
                BucketContainer.DataSizeBytes += deltaBytes;
            }

            mappedFileBucketInfo->DataSizeBytes = value;
            cacheBucketHeader.DataSizeBytes = value;
        }
    }

    public uint NonDataSizeBytes
    {
        get => BucketHeaderSizeBytes;

        set
        {
            if (value > BucketHeaderSizeBytes || !Writable || !CanUseWritableBufferInfo) return;
            if (BucketContainerIndexEntry == null) return;
            var additionalNonBucketDataSize = value - NonDataSizeBytes;
            BucketContainer.NonDataSizeBytes = additionalNonBucketDataSize;
        }
    }

    public IStorageTimeResolver<TEntry>? StorageTimeResolver { get; set; }

    public virtual uint CreateBucketId(uint previousHighestBucketId) => previousHighestBucketId + 1;

    public StorageAttemptResult CheckTimeSupported(DateTime storageDateTime)
    {
        if (!IsOpen) OpenBucket(asWritable: Writable);
        if (storageDateTime == default || storageDateTime == DateTimeConstants.UnixEpoch) return StorageAttemptResult.StorageTimeNotSupported;
        var bucketPeriod = TimeSeriesPeriod;
        var bucketEndTime = bucketPeriod.PeriodEnd(PeriodStartTime);
        if (storageDateTime >= PeriodStartTime && storageDateTime < bucketEndTime)
            return BucketFlags.HasBucketCurrentAppendingFlag() ? StorageAttemptResult.PeriodRangeMatched : StorageAttemptResult.BucketClosedForAppend;

        if (storageDateTime >= bucketEndTime && storageDateTime < bucketPeriod.PeriodEnd(bucketEndTime))
            return StorageAttemptResult.NextBucketPeriod;
        if (storageDateTime >= bucketPeriod.PreviousPeriodStart(PeriodStartTime) && storageDateTime < PeriodStartTime)
            return StorageAttemptResult.BucketClosedForAppend;
        var filePeriod = ((ITimeSeriesFile)BucketContainer.ContainingTimeSeriesFile).TimeSeriesPeriod;
        var fileStartTime = ((ITimeSeriesFile)BucketContainer.ContainingTimeSeriesFile).PeriodStartTime;
        if (storageDateTime >= fileStartTime && storageDateTime < filePeriod.PeriodEnd(fileStartTime)) return StorageAttemptResult.BucketSearchRange;
        return StorageAttemptResult.NextFilePeriod;
    }

    public bool IsOpen => BucketAppenderFileView != null && BucketHeaderFileView != null;

    public bool IsOpenForAppend => BucketFlags.HasBucketCurrentAppendingFlag();

    public IBuffer? DataWriterAtAppendLocation
    {
        get
        {
            if (!IsOpenForAppend || BucketAppenderFileView == null) return null;
            fileBuffer ??= new MemoryMappedFileBuffer(BucketAppenderFileView, false);
            fileBuffer.SetFileWriterAt(BucketAppenderFileView, BucketDataStartFileOffset + (long)DataSizeBytes);
            return fileBuffer;
        }
    }

    public virtual IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        BucketHeaderFileView = alternativeHeaderAndDataFileView ?? SelectBucketHeaderFileView();
        alternativeHeaderAndDataFileView ??= BucketContainer.ContainingTimeSeriesFile.ActiveBucketDataFileView;
        alternativeHeaderAndDataFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, 0, asWritable);
        BucketAppenderFileView = alternativeHeaderAndDataFileView;
        Writable = asWritable;
        BucketHeaderFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, 0, asWritable);
        requiredViewFileCursorOffset = BucketHeaderFileView.LowerViewFileCursorOffset;
        requiredViewVirtualMemoryLocation = BucketHeaderFileView.LowerHalfViewVirtualMemoryAddress;
        mappedFileBucketInfo = (BucketHeader*)BucketHeaderFileView.FileCursorBufferPointer(FileCursorOffset, shouldGrow: asWritable);
        cacheBucketHeader = *mappedFileBucketInfo;
        BucketContainerIndexEntry = BucketContainer.BucketIndexes.GetBucketIndexInfo(BucketId);
        return this;
    }

    public virtual void InitializeNewBucket(DateTime containingTime)
    {
        var bucketStartTime = TimeSeriesPeriod.ContainingPeriodBoundaryStart(containingTime);
        PeriodStartTime = bucketStartTime;
        var bucketId = BucketContainer.CreateBucketId();
        if (BucketFactory.IsFileRootBucketType) BucketContainer.ContainingTimeSeriesFile.Header.HighestBucketId = bucketId;

        BucketId = bucketId;
        BucketFlags = BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending;
        CreatedDateTime = DateTime.UtcNow;
        LastAmendedDateTime = DateTime.UtcNow;
        DataEntriesCount = 0;
        DataSizeBytes = 0;
        var thisEndBucketTime = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
        var fileEndTime = BucketContainer.TimeSeriesPeriod.PeriodEnd(BucketContainer.PeriodStartTime);
        if (fileEndTime == thisEndBucketTime) BucketFlags |= BucketFlags.IsLastPossibleSibling;
        ParentBucketId = 0;
        ParentDeltaFileOffset = 0;
        BucketContainer.AddNewBucket(this);
        BucketContainerIndexEntry->NumIndexEntries += 1;
        BucketContainer.NonDataSizeBytes += BucketHeaderSizeBytes;
    }

    public virtual void Dispose()
    {
        CloseFileView();
    }

    public virtual void CloseFileView()
    {
        BucketAppenderFileView = null;
        BucketHeaderFileView = null;
        mappedFileBucketInfo = null;
        BucketContainerIndexEntry = null;
        Writable = false;
    }

    public virtual long CalculateBucketEndFileOffset() => BucketDataStartFileOffset + (long)DataSizeBytes;

    public abstract IEnumerable<TEntry> AllBucketEntriesFrom(long? fromFileCursorOffset = null);

    public virtual IEnumerable<TEntry> EntriesBetween(DateTime? fromTime = null, DateTime? toTime = null)
    {
        foreach (var timeSeriesEntry in AllBucketEntriesFrom(BucketDataStartFileOffset))
            if (EntryIntersects(timeSeriesEntry, fromTime, toTime))
                yield return timeSeriesEntry;
    }

    public virtual IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromTime = null
        , DateTime? toTime = null) where TM : class, IVersionedMessage =>
        EntriesBetween(BucketDataStartFileOffset, usingMessageDeserializer, fromTime, toTime);

    public abstract int CopyTo(List<TEntry> destination, DateTime? fromDateTime = null, DateTime? toDateTime = null);

    public void SetEntrySerializer(IMessageSerializer useSerializer)
    {
        EntrySerializer = useSerializer;
    }

    public abstract StorageAttemptResult AppendEntry(TEntry entry);


    public virtual IEnumerable<TEntry> EntriesBetween(long fileCursorOffset, DateTime? fromTime = null, DateTime? toTime = null)
    {
        foreach (var timeSeriesEntry in AllBucketEntriesFrom(fileCursorOffset))
            if (EntryIntersects(timeSeriesEntry, fromTime, toTime))
                yield return timeSeriesEntry;
    }

    public abstract IEnumerable<TM> EntriesBetween<TM>(long fileCursorOffset, IMessageDeserializer<TM> usingMessageDeserializer
        , DateTime? fromTime = null
        , DateTime? toTime = null) where TM : class, IVersionedMessage;


    ~DataBucket()
    {
        Dispose();
    }

    protected bool EntryIntersects(TEntry checkEntry, DateTime? fromTime = null, DateTime? toTime = null)
    {
        var entryStorageTime = checkEntry.StorageTime(StorageTimeResolver);
        return (entryStorageTime < toTime || (toTime == null && fromTime != null))
               && (entryStorageTime > fromTime || (fromTime == null && toTime != null));
    }

    protected virtual ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => ContainingFile.ActiveBucketHeaderFileView;
}
