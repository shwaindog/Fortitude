#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public abstract unsafe class DataBucket<TEntry, TBucket> : IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(DataBucket<,>));
    protected readonly IMutableBucketContainer BucketContainer;
    protected readonly IBucketTrackingSession ContainingFile;
    protected ShiftableMemoryMappedFileView? BucketAppenderFileView;
    protected BucketIndexInfo* BucketContainerIndexEntry;
    private BucketFactory<TBucket>? bucketFactory;
    protected ShiftableMemoryMappedFileView? BucketHeaderFileView;
    private BucketHeader cacheBucketHeader;
    protected IMessageSerializer? EntrySerializer;
    private MemoryMappedFileBuffer? fileBuffer;
    protected long HeaderRealignmentDelta;
    private BucketHeader* mappedFileBucketInfo;
    protected long RequiredHeaderViewFileCursorOffset;
    protected byte* RequiredHeaderViewLocation;
    protected bool Writable;

    protected DataBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
    {
        BucketContainer = bucketContainer;
        ContainingFile = bucketContainer.ContainingSession;
        ContainingFile.CurrentlyOpenBucket = this;
        HeaderRealignmentDelta = bucketFileCursorOffset % 8 > 0 ? 8 - bucketFileCursorOffset % 8 : 0;
        FileCursorOffset = bucketFileCursorOffset + HeaderRealignmentDelta;
        // ReSharper disable VirtualMemberCallInConstructor
        OpenBucket(alternativeFileView, writable);
        // ReSharper restore VirtualMemberCallInConstructor
    }

    protected virtual long EndOfBucketHeaderSectionOffset => FileCursorOffset + sizeof(BucketHeader);

    protected bool CanUseWritableBufferInfo =>
        IsOpen && mappedFileBucketInfo != null
               && BucketHeaderFileView!.LowerHalfViewVirtualMemoryAddress == RequiredHeaderViewLocation
               && BucketHeaderFileView!.LowerViewFileCursorOffset == RequiredHeaderViewFileCursorOffset;

    public BucketFactory<TBucket> BucketFactory
    {
        get => bucketFactory ??= new BucketFactory<TBucket>();
        set => bucketFactory = value;
    }

    public TBucket? MoveNext =>
        NextSiblingBucketDeltaFileOffset != 0 ?
            BucketFactory.OpenExistingBucket(BucketContainer.ContainingSession, FileCursorOffset + NextSiblingBucketDeltaFileOffset
                , Writable) :
            default;

    public virtual TBucket? CloseAndCreateNextBucket()
    {
        var nextStartPeriod = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
        var nextPeriodResult = CheckTimeSupported(nextStartPeriod);
        if (nextPeriodResult == StorageAttemptResult.NextBucketPeriod)
        {
            BucketFlags = BucketFlags.BucketClosedGracefully | BucketFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            var bucketEndCursor = BucketFactory.AppendCloseBucketDelimiter(BucketAppenderFileView!, CalculateBucketEndFileOffset());
            CloseFileView();
            var nextBucket = BucketFactory.CreateNewBucket(BucketContainer, bucketEndCursor, nextStartPeriod, true);
            return nextBucket;
        }

        return null;
    }

    public void RefreshViews(ShiftableMemoryMappedFileView? usingMappedFileView = null)
    {
        var isWriter = Writable;
        if (IsOpen) CloseFileView();
        OpenBucket(usingMappedFileView, isWriter);
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
            if (BucketContainerIndexEntry != null) BucketContainerIndexEntry->BucketFlags = value;
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
        var filePeriod = BucketContainer.ContainingSession.TimeSeriesPeriod;
        var fileStartTime = BucketContainer.ContainingSession.PeriodStartTime;
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
        alternativeHeaderAndDataFileView ??= BucketContainer.ContainingSession.ActiveBucketDataFileView;
        alternativeHeaderAndDataFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, 0, asWritable);
        BucketAppenderFileView = alternativeHeaderAndDataFileView;
        Writable = asWritable;
        BucketHeaderFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, 0, asWritable);
        RequiredHeaderViewFileCursorOffset = BucketHeaderFileView.LowerViewFileCursorOffset;
        RequiredHeaderViewLocation = BucketHeaderFileView.LowerHalfViewVirtualMemoryAddress;
        mappedFileBucketInfo = (BucketHeader*)BucketHeaderFileView.FileCursorBufferPointer(FileCursorOffset, shouldGrow: asWritable);
        cacheBucketHeader = *mappedFileBucketInfo;
        if (BucketId != 0) BucketContainerIndexEntry = BucketContainer.BucketIndexes.GetBucketIndexInfo(BucketId);

        return this;
    }

    public virtual void InitializeNewBucket(DateTime containingTime)
    {
        var bucketStartTime = TimeSeriesPeriod.ContainingPeriodBoundaryStart(containingTime);
        PeriodStartTime = bucketStartTime;
        var bucketId = BucketContainer.CreateBucketId();
        if (BucketFactory.IsFileRootBucketType) BucketContainer.ContainingSession.Header.HighestBucketId = bucketId;

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
        // during open
        BucketContainerIndexEntry = BucketContainer.BucketIndexes.GetBucketIndexInfo(BucketId);
        BucketContainer.NonDataSizeBytes += BucketHeaderSizeBytes;
    }

    public virtual void Dispose()
    {
        CloseFileView();
    }

    public virtual void CloseFileView()
    {
        if (!IsOpen) return;
        if (Writable) BucketHeaderFileView!.FlushPtrDataToDisk(mappedFileBucketInfo, sizeof(BucketHeader));
        BucketAppenderFileView = null;
        BucketHeaderFileView = null;
        mappedFileBucketInfo = null;
        BucketContainerIndexEntry = null;
        Writable = false;
    }

    public virtual long CalculateBucketEndFileOffset() => BucketDataStartFileOffset + (long)DataSizeBytes;

    public void Entries(IReaderContext<TEntry> readerContext)
    {
        throw new NotImplementedException();
    }

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
