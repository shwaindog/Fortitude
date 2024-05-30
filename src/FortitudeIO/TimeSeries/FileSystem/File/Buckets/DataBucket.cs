// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public abstract unsafe class DataBucket<TEntry, TBucket> : IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(DataBucket<,>));

    protected readonly IMutableBucketContainer BucketContainer;
    protected readonly IBucketTrackingSession  ContainingFile;

    protected ShiftableMemoryMappedFileView? BucketAppenderDataReaderFileView;
    protected BucketIndexInfo*               BucketContainerIndexEntry;
    private   BucketFactory<TBucket>?        bucketFactory;
    protected ShiftableMemoryMappedFileView? BucketHeaderFileView;
    private   BucketHeader                   cacheBucketHeader;
    protected IMessageSerializer?            EntrySerializer;
    private   MemoryMappedFileBuffer?        fileBuffer;
    protected long                           HeaderRealignmentDelta;
    private   BucketHeader*                  mappedFileBucketInfo;
    protected long                           RequiredHeaderViewFileCursorOffset;
    protected byte*                          RequiredHeaderViewLocation;
    protected bool                           Writable;

    protected DataBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
    {
        BucketContainer                    = bucketContainer;
        ContainingFile                     = bucketContainer.ContainingSession;
        ContainingFile.CurrentlyOpenBucket = this;
        HeaderRealignmentDelta             = bucketFileCursorOffset % 8 > 0 ? 8 - bucketFileCursorOffset % 8 : 0;
        FileCursorOffset                   = bucketFileCursorOffset + HeaderRealignmentDelta;
        // ReSharper disable VirtualMemberCallInConstructor
        OpenBucket(alternativeFileView, writable);
        // ReSharper restore VirtualMemberCallInConstructor
    }

    protected virtual long EndOfBucketHeaderSectionOffset => FileCursorOffset + sizeof(BucketHeader);

    protected virtual bool CanAccessHeaderFromFileView =>
        IsOpen && mappedFileBucketInfo != null
               && BucketHeaderFileView!.StartAddress == RequiredHeaderViewLocation
               && BucketHeaderFileView!.LowerViewFileCursorOffset == RequiredHeaderViewFileCursorOffset;

    public BucketFactory<TBucket> BucketFactory
    {
        get => bucketFactory ??= new BucketFactory<TBucket>();
        set => bucketFactory = value;
    }

    public TBucket? MoveNext =>
        NextSiblingBucketDeltaFileOffset != 0
            ? BucketFactory.OpenExistingBucket(BucketContainer.ContainingSession, FileCursorOffset + NextSiblingBucketDeltaFileOffset
                                             , Writable)
            : default;

    public virtual TBucket? CloseAndCreateNextBucket()
    {
        var nextStartPeriod  = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
        var nextPeriodResult = CheckTimeSupported(nextStartPeriod);
        if (nextPeriodResult == StorageAttemptResult.NextBucketPeriod)
        {
            BucketFlags = BucketFlags.BucketClosedGracefully | BucketFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            var bucketEndCursor = BucketFactory.AppendCloseBucketDelimiter(BucketAppenderDataReaderFileView!, CalculateBucketEndFileOffset());
            CloseBucketFileViews();
            var nextBucket = BucketFactory.CreateNewBucket(BucketContainer, bucketEndCursor, nextStartPeriod, true);
            return nextBucket;
        }

        return null;
    }

    public virtual uint BucketHeaderSizeBytes => (uint)(HeaderRealignmentDelta + (EndOfBucketHeaderSectionOffset - FileCursorOffset));

    public virtual Func<TEntry>? DefaultEntryFactory => null;

    public Type ExpectedEntryType => typeof(TEntry);

    public uint BucketId
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.BucketId;
            cacheBucketHeader.BucketId = mappedFileBucketInfo->BucketId;
            return cacheBucketHeader.BucketId;
        }
        set
        {
            if (value == cacheBucketHeader.BucketId || !Writable || !CanAccessHeaderFromFileView) return;
            mappedFileBucketInfo->BucketId = value;
            cacheBucketHeader.BucketId     = value;
        }
    }

    public uint ParentBucketId
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.ParentBucketId;
            cacheBucketHeader.ParentBucketId = mappedFileBucketInfo->ParentBucketId;
            return cacheBucketHeader.ParentBucketId;
        }
        set
        {
            if (value == cacheBucketHeader.ParentBucketId || !Writable || !CanAccessHeaderFromFileView) return;
            mappedFileBucketInfo->ParentBucketId = value;
            cacheBucketHeader.ParentBucketId     = value;
        }
    }

    public long ParentDeltaFileOffset
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.ParentDeltaFileOffset;
            cacheBucketHeader.ParentDeltaFileOffset = mappedFileBucketInfo->ParentDeltaFileOffset;
            return cacheBucketHeader.ParentDeltaFileOffset;
        }
        set
        {
            if (value == cacheBucketHeader.ParentDeltaFileOffset || !Writable || !CanAccessHeaderFromFileView) return;
            mappedFileBucketInfo->ParentDeltaFileOffset = value;
            cacheBucketHeader.ParentDeltaFileOffset     = value;
        }
    }

    public long FileCursorOffset { get; }

    public BucketFlags BucketFlags
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.BucketFlags;
            cacheBucketHeader.BucketFlags = mappedFileBucketInfo->BucketFlags;
            return cacheBucketHeader.BucketFlags;
        }
        set
        {
            if (value == cacheBucketHeader.BucketFlags || !Writable || !CanAccessHeaderFromFileView) return;
            if (BucketContainerIndexEntry != null) BucketContainerIndexEntry->BucketFlags = value;
            mappedFileBucketInfo->BucketFlags = value;
            cacheBucketHeader.BucketFlags     = value;
        }
    }

    public abstract TimeSeriesPeriod TimeSeriesPeriod { get; }

    public DateTime PeriodStartTime
    {
        get
        {
            if (!CanAccessHeaderFromFileView)
                return DateTime.FromBinary(cacheBucketHeader.PeriodStartTime * BucketHeader.LowestBucketGranularityTickDivisor);
            cacheBucketHeader.PeriodStartTime = mappedFileBucketInfo->PeriodStartTime;
            return DateTime.FromBinary(cacheBucketHeader.PeriodStartTime * BucketHeader.LowestBucketGranularityTickDivisor);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.PeriodStartTime || !Writable || !CanAccessHeaderFromFileView) return;
            mappedFileBucketInfo->PeriodStartTime = (uint)(value.Ticks / BucketHeader.LowestBucketGranularityTickDivisor);
            cacheBucketHeader.PeriodStartTime     = (uint)(value.Ticks / BucketHeader.LowestBucketGranularityTickDivisor);
        }
    }

    public long PreviousSiblingBucketDeltaFileOffset
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.PreviousSiblingDeltaFileOffset;
            cacheBucketHeader.PreviousSiblingDeltaFileOffset = mappedFileBucketInfo->PreviousSiblingDeltaFileOffset;
            return cacheBucketHeader.PreviousSiblingDeltaFileOffset;
        }

        set
        {
            if (value == cacheBucketHeader.PreviousSiblingDeltaFileOffset || !Writable || !CanAccessHeaderFromFileView) return;
            mappedFileBucketInfo->PreviousSiblingDeltaFileOffset = value;
            cacheBucketHeader.PreviousSiblingDeltaFileOffset     = value;
        }
    }

    public long NextSiblingBucketDeltaFileOffset
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.NextSiblingBucketDeltaFileOffset;
            cacheBucketHeader.NextSiblingBucketDeltaFileOffset = mappedFileBucketInfo->NextSiblingBucketDeltaFileOffset;
            return cacheBucketHeader.NextSiblingBucketDeltaFileOffset;
        }

        set
        {
            if (value == cacheBucketHeader.NextSiblingBucketDeltaFileOffset || !Writable || !CanAccessHeaderFromFileView) return;
            mappedFileBucketInfo->NextSiblingBucketDeltaFileOffset = value;
            cacheBucketHeader.NextSiblingBucketDeltaFileOffset     = value;
        }
    }

    public DateTime CreatedDateTime
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return DateTime.FromBinary(cacheBucketHeader.CreatedDateTime);
            cacheBucketHeader.CreatedDateTime = mappedFileBucketInfo->CreatedDateTime;
            return DateTime.FromBinary(cacheBucketHeader.CreatedDateTime);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.CreatedDateTime || !Writable || !CanAccessHeaderFromFileView) return;
            mappedFileBucketInfo->CreatedDateTime = value.Ticks;
            cacheBucketHeader.CreatedDateTime     = value.Ticks;
        }
    }

    public DateTime LastAmendedDateTime
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return DateTime.FromBinary(cacheBucketHeader.LastAmendedDateTime);
            cacheBucketHeader.LastAmendedDateTime = mappedFileBucketInfo->LastAmendedDateTime;
            return DateTime.FromBinary(cacheBucketHeader.LastAmendedDateTime);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.LastAmendedDateTime || !Writable || !CanAccessHeaderFromFileView) return;
            mappedFileBucketInfo->LastAmendedDateTime = value.Ticks;
            cacheBucketHeader.LastAmendedDateTime     = value.Ticks;
        }
    }

    public uint DataEntriesCount
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.DataEntriesCount;
            cacheBucketHeader.DataEntriesCount = mappedFileBucketInfo->DataEntriesCount;
            return cacheBucketHeader.DataEntriesCount;
        }

        set
        {
            if (value == cacheBucketHeader.DataEntriesCount || !Writable || !CanAccessHeaderFromFileView) return;
            if (BucketContainerIndexEntry != null)
            {
                var deltaEntries = value - mappedFileBucketInfo->DataEntriesCount;
                BucketContainerIndexEntry->NumEntries += deltaEntries;
                BucketContainer.DataEntriesCount      += deltaEntries;
            }

            mappedFileBucketInfo->DataEntriesCount = value;
            cacheBucketHeader.DataEntriesCount     = value;
        }
    }

    public virtual long BucketDataStartFileOffset => EndOfBucketHeaderSectionOffset;

    public ulong DataSizeBytes
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.DataSizeBytes;
            cacheBucketHeader.DataSizeBytes = mappedFileBucketInfo->DataSizeBytes;
            return cacheBucketHeader.DataSizeBytes;
        }

        set
        {
            if (value == cacheBucketHeader.DataSizeBytes || !Writable || !CanAccessHeaderFromFileView) return;
            if (BucketContainerIndexEntry != null)
            {
                var deltaBytes = value - mappedFileBucketInfo->DataSizeBytes;
                BucketContainerIndexEntry->DataSizeBytes += deltaBytes;
                BucketContainer.DataSizeBytes            += deltaBytes;
            }

            mappedFileBucketInfo->DataSizeBytes = value;
            cacheBucketHeader.DataSizeBytes     = value;
            var additionalWriteSize = Writable ? 512 : 0;
            BucketAppenderDataReaderFileView?.EnsureViewCoversFileCursorOffsetAndSize(BucketDataStartFileOffset, (long)value + additionalWriteSize);
        }
    }

    public uint NonDataSizeBytes
    {
        get => BucketHeaderSizeBytes;

        set
        {
            if (value > BucketHeaderSizeBytes || !Writable || !CanAccessHeaderFromFileView) return;
            if (BucketContainerIndexEntry == null) return;
            var additionalNonBucketDataSize = value - NonDataSizeBytes;
            BucketContainer.NonDataSizeBytes = additionalNonBucketDataSize;
        }
    }

    public bool IsOpen => BucketAppenderDataReaderFileView != null && BucketHeaderFileView != null;

    public bool IsOpenForAppend => BucketFlags.HasBucketCurrentAppendingFlag();

    public IBuffer? DataWriterAtAppendLocation
    {
        get
        {
            if (!IsOpenForAppend || BucketAppenderDataReaderFileView == null) return null;
            fileBuffer ??= new MemoryMappedFileBuffer(BucketAppenderDataReaderFileView, false);
            fileBuffer.SetFileWriterAt(BucketAppenderDataReaderFileView, BucketDataStartFileOffset + (long)DataSizeBytes);
            return fileBuffer;
        }
    }

    public IStorageTimeResolver<TEntry>? StorageTimeResolver { get; set; }

    public void RefreshViews(ShiftableMemoryMappedFileView? usingMappedFileView = null)
    {
        var isWriter = Writable;
        if (IsOpen) CloseBucketFileViews();
        OpenBucket(usingMappedFileView, isWriter);
    }

    public bool Intersects(DateTime? fromTime = null, DateTime? toTime = null) =>
        (PeriodStartTime < toTime || (toTime == null && fromTime != null))
     && (TimeSeriesPeriod.PeriodEnd(PeriodStartTime) > fromTime || (fromTime == null && toTime != null));

    public virtual uint CreateBucketId(uint previousHighestBucketId) => previousHighestBucketId + 1;

    public StorageAttemptResult CheckTimeSupported(DateTime storageDateTime)
    {
        if (!IsOpen) OpenBucket(asWritable: Writable);
        if (storageDateTime == default || storageDateTime == DateTimeConstants.UnixEpoch) return StorageAttemptResult.StorageTimeNotSupported;
        var bucketPeriod  = TimeSeriesPeriod;
        var bucketEndTime = bucketPeriod.PeriodEnd(PeriodStartTime);
        if (storageDateTime >= PeriodStartTime && storageDateTime < bucketEndTime)
            return BucketFlags.HasBucketCurrentAppendingFlag() ? StorageAttemptResult.PeriodRangeMatched : StorageAttemptResult.BucketClosedForAppend;

        if (storageDateTime >= bucketEndTime && storageDateTime < bucketPeriod.PeriodEnd(bucketEndTime))
            return StorageAttemptResult.NextBucketPeriod;
        if (storageDateTime >= bucketPeriod.PreviousPeriodStart(PeriodStartTime) && storageDateTime < PeriodStartTime)
            return StorageAttemptResult.BucketClosedForAppend;
        var filePeriod    = BucketContainer.ContainingSession.TimeSeriesPeriod;
        var fileStartTime = BucketContainer.ContainingSession.PeriodStartTime;
        if (storageDateTime >= fileStartTime && storageDateTime < filePeriod.PeriodEnd(fileStartTime)) return StorageAttemptResult.BucketSearchRange;
        return StorageAttemptResult.NextFilePeriod;
    }

    public virtual IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        BucketHeaderFileView             =   alternativeHeaderAndDataFileView ?? SelectBucketHeaderFileView();
        alternativeHeaderAndDataFileView ??= BucketContainer.ContainingSession.ActiveBucketDataFileView;
        alternativeHeaderAndDataFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, 0, asWritable);
        BucketAppenderDataReaderFileView = alternativeHeaderAndDataFileView;
        Writable                         = asWritable;
        BucketHeaderFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, 0, asWritable);
        RequiredHeaderViewFileCursorOffset = BucketHeaderFileView.LowerViewFileCursorOffset;
        RequiredHeaderViewLocation         = BucketHeaderFileView.StartAddress;
        EnsureHeaderViewReferencesCorrectlyMapped();
        if (BucketHeaderFileView.EnsureViewCoversFileCursorOffsetAndSize(FileCursorOffset, BucketHeaderSizeBytes))
            EnsureHeaderViewReferencesCorrectlyMapped();
        if (BucketHeaderFileView != BucketAppenderDataReaderFileView)
            BucketAppenderDataReaderFileView.EnsureViewCoversFileCursorOffsetAndSize(BucketDataStartFileOffset, (long)DataSizeBytes);
        if (BucketId != 0) BucketContainerIndexEntry = BucketContainer.BucketIndexes.GetBucketIndexInfo(BucketId);

        return this;
    }

    public virtual void InitializeNewBucket(DateTime containingTime)
    {
        var bucketStartTime = TimeSeriesPeriod.ContainingPeriodBoundaryStart(containingTime);
        PeriodStartTime = bucketStartTime;
        var bucketId                                                                                     = BucketContainer.CreateBucketId();
        if (BucketFactory.IsFileRootBucketType) BucketContainer.ContainingSession.Header.HighestBucketId = bucketId;

        BucketId            = bucketId;
        BucketFlags         = BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending;
        CreatedDateTime     = DateTime.UtcNow;
        LastAmendedDateTime = DateTime.UtcNow;
        DataEntriesCount    = 0;
        DataSizeBytes       = 0;
        var thisEndBucketTime                             = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
        var fileEndTime                                   = BucketContainer.TimeSeriesPeriod.PeriodEnd(BucketContainer.PeriodStartTime);
        if (fileEndTime == thisEndBucketTime) BucketFlags |= BucketFlags.IsLastPossibleSibling;
        ParentBucketId        = 0;
        ParentDeltaFileOffset = 0;
        BucketContainer.AddNewBucket(this);
        // during open
        BucketContainerIndexEntry        =  BucketContainer.BucketIndexes.GetBucketIndexInfo(BucketId);
        BucketContainer.NonDataSizeBytes += BucketHeaderSizeBytes;
    }

    public virtual void Dispose()
    {
        CloseBucketFileViews();
    }

    public virtual void CloseBucketFileViews()
    {
        if (!IsOpen) return;
        if (Writable) BucketHeaderFileView!.FlushPtrDataToDisk(mappedFileBucketInfo, sizeof(BucketHeader));
        BucketAppenderDataReaderFileView = null;
        BucketHeaderFileView             = null;
        mappedFileBucketInfo             = null;
        BucketContainerIndexEntry        = null;
        Writable                         = false;
    }

    public virtual long CalculateBucketEndFileOffset() => BucketDataStartFileOffset + (long)DataSizeBytes;

    public abstract IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext, long? fromFileCursorOffset = null);

    public virtual void VisitChildrenCacheAndClose()
    {
        CloseBucketFileViews();
    }

    public void SetEntrySerializer(IMessageSerializer useSerializer)
    {
        EntrySerializer = useSerializer;
    }

    public abstract StorageAttemptResult AppendEntry(TEntry entry);

    public bool BucketIntersects(PeriodRange? period = null) => period.IntersectsWith(TimeSeriesPeriod, PeriodStartTime);

    protected virtual void EnsureHeaderViewReferencesCorrectlyMapped()
    {
        if (BucketHeaderFileView == null) return;
        mappedFileBucketInfo = (BucketHeader*)BucketHeaderFileView!.FileCursorBufferPointer(FileCursorOffset, shouldGrow: Writable);
        cacheBucketHeader    = *mappedFileBucketInfo;
    }

    ~DataBucket()
    {
        Dispose();
    }

    protected virtual ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => ContainingFile.ActiveBucketHeaderFileView;
}
