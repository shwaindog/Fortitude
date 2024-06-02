// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public abstract unsafe class BucketBase<TEntry, TBucket> : IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(DataBucket<,>));

    protected readonly IMutableBucketContainer BucketContainer;
    protected readonly IBucketTrackingSession  ContainingFile;

    protected BucketIndexInfo*        BucketContainerIndexEntry;
    private   BucketFactory<TBucket>? bucketFactory;

    protected ShiftableMemoryMappedFileView? BucketHeaderFileView;
    private   BucketHeader                   cacheBucketHeader;
    protected IMessageSerializer?            EntrySerializer;
    protected BucketHeader*                  MappedFileBucketInfo;

    protected long  RequiredHeaderViewFileCursorOffset;
    protected byte* RequiredHeaderViewLocation;
    protected bool  Writable;

    protected BucketBase(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
    {
        BucketContainer                    = bucketContainer;
        ContainingFile                     = bucketContainer.ContainingSession;
        ContainingFile.CurrentlyOpenBucket = this;
        var headerRealignmentDelta = bucketFileCursorOffset % 8 > 0 ? 8 - bucketFileCursorOffset % 8 : 0;
        FileCursorOffset = bucketFileCursorOffset + headerRealignmentDelta;
        // Logger.Debug("{0} at FileCursorOffset {1}", GetType().Name, FileCursorOffset);
        // ReSharper disable VirtualMemberCallInConstructor
        OpenBucket(alternativeFileView, writable);
        // ReSharper restore VirtualMemberCallInConstructor
    }

    protected long EndOfBucketHeaderSectionOffset => FileCursorOffset + sizeof(BucketHeader);

    protected bool CanAccessHeaderFromFileView =>
        IsOpen && MappedFileBucketInfo != null
               && BucketHeaderFileView!.StartAddress == RequiredHeaderViewLocation
               && BucketHeaderFileView!.LowerViewFileCursorOffset == RequiredHeaderViewFileCursorOffset;

    public virtual Func<TEntry>? DefaultEntryFactory => null;

    public BucketFactory<TBucket> BucketFactory
    {
        get => bucketFactory ??= new BucketFactory<TBucket>();
        set => bucketFactory = value;
    }

    public virtual TBucket? CloseAndCreateNextBucket()
    {
        var nextStartPeriod  = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
        var nextPeriodResult = CheckTimeSupported(nextStartPeriod);
        if (nextPeriodResult == StorageAttemptResult.NextBucketPeriod)
        {
            BucketFlags = BucketFlags.BucketClosedGracefully | BucketFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            var writeDelimiterView = ContainingFile.ActiveBucketDataFileView;
            CloseBucketFileViews();
            var bucketEndCursor = BucketFactory.AppendCloseBucketDelimiter(writeDelimiterView, CalculateBucketEndFileOffset());
            var nextBucket      = BucketFactory.CreateNewBucket(BucketContainer, bucketEndCursor, nextStartPeriod, true);
            return nextBucket;
        }

        return null;
    }

    public uint BucketHeaderSizeBytes => (uint)(EndAllHeadersSectionFileOffset - FileCursorOffset);

    public Type ExpectedEntryType => typeof(TEntry);

    public uint BucketId
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.BucketId;
            cacheBucketHeader.BucketId = MappedFileBucketInfo->BucketId;
            return cacheBucketHeader.BucketId;
        }
        set
        {
            if (value == cacheBucketHeader.BucketId || !Writable || !CanAccessHeaderFromFileView) return;
            MappedFileBucketInfo->BucketId = value;
            cacheBucketHeader.BucketId     = value;
        }
    }

    public long FileCursorOffset { get; }

    public BucketFlags BucketFlags
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.BucketFlags;
            cacheBucketHeader.BucketFlags = MappedFileBucketInfo->BucketFlags;
            return cacheBucketHeader.BucketFlags;
        }
        set
        {
            if (value == cacheBucketHeader.BucketFlags || !Writable || !CanAccessHeaderFromFileView) return;
            if (BucketContainerIndexEntry != null) BucketContainerIndexEntry->BucketFlags = value;
            MappedFileBucketInfo->BucketFlags = value;
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
            cacheBucketHeader.PeriodStartTime = MappedFileBucketInfo->PeriodStartTime;
            return DateTime.FromBinary(cacheBucketHeader.PeriodStartTime * BucketHeader.LowestBucketGranularityTickDivisor);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.PeriodStartTime || !Writable || !CanAccessHeaderFromFileView) return;
            MappedFileBucketInfo->PeriodStartTime = (uint)(value.Ticks / BucketHeader.LowestBucketGranularityTickDivisor);
            cacheBucketHeader.PeriodStartTime     = (uint)(value.Ticks / BucketHeader.LowestBucketGranularityTickDivisor);
        }
    }

    public DateTime CreatedDateTime
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return DateTime.FromBinary(cacheBucketHeader.CreatedDateTime);
            cacheBucketHeader.CreatedDateTime = MappedFileBucketInfo->CreatedDateTime;
            return DateTime.FromBinary(cacheBucketHeader.CreatedDateTime);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.CreatedDateTime || !Writable || !CanAccessHeaderFromFileView) return;
            MappedFileBucketInfo->CreatedDateTime = value.Ticks;
            cacheBucketHeader.CreatedDateTime     = value.Ticks;
        }
    }

    public DateTime LastAmendedDateTime
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return DateTime.FromBinary(cacheBucketHeader.LastAmendedDateTime);
            cacheBucketHeader.LastAmendedDateTime = MappedFileBucketInfo->LastAmendedDateTime;
            return DateTime.FromBinary(cacheBucketHeader.LastAmendedDateTime);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.LastAmendedDateTime || !Writable || !CanAccessHeaderFromFileView) return;
            MappedFileBucketInfo->LastAmendedDateTime = value.Ticks;
            cacheBucketHeader.LastAmendedDateTime     = value.Ticks;
        }
    }

    public uint TotalDataEntriesCount
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.TotalDataEntriesCount;
            cacheBucketHeader.TotalDataEntriesCount = MappedFileBucketInfo->TotalDataEntriesCount;
            return cacheBucketHeader.TotalDataEntriesCount;
        }

        set
        {
            if (value == cacheBucketHeader.TotalDataEntriesCount || !Writable || !CanAccessHeaderFromFileView) return;
            if (BucketContainerIndexEntry != null)
            {
                var deltaEntries = value - MappedFileBucketInfo->TotalDataEntriesCount;
                BucketContainerIndexEntry->NumEntries += deltaEntries;
                BucketContainer.TotalDataEntriesCount += deltaEntries;
            }

            MappedFileBucketInfo->TotalDataEntriesCount = value;
            cacheBucketHeader.TotalDataEntriesCount     = value;
        }
    }

    public virtual long EndAllHeadersSectionFileOffset => EndOfBucketHeaderSectionOffset;

    public ulong TotalFileDataSizeBytes
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.TotalFileDataSizeBytes;
            cacheBucketHeader.TotalFileDataSizeBytes = MappedFileBucketInfo->TotalFileDataSizeBytes;
            return cacheBucketHeader.TotalFileDataSizeBytes;
        }

        set
        {
            if (!Writable || !CanAccessHeaderFromFileView) return;

            var deltaBytes = value - cacheBucketHeader.TotalFileDataSizeBytes;

            BucketContainer.TotalFileDataSizeBytes += deltaBytes;
            // Logger.Info("{0} {1:dd-HH} BucketContainer.TotalFileDataSizeBytes = {2:###,##0} deltaBytes = {3:#,##0}",
            //             GetType().Name, PeriodStartTime, BucketContainer.TotalFileDataSizeBytes, deltaBytes);
            MappedFileBucketInfo->TotalFileDataSizeBytes += deltaBytes;
            cacheBucketHeader.TotalFileDataSizeBytes     += deltaBytes;
        }
    }

    public uint TotalHeadersSizeBytes
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.TotalHeadersBytes;
            cacheBucketHeader.TotalHeadersBytes = MappedFileBucketInfo->TotalHeadersBytes;
            return cacheBucketHeader.TotalHeadersBytes;
        }

        set
        {
            if (!Writable || !CanAccessHeaderFromFileView) return;
            var deltaBytes = value - cacheBucketHeader.TotalHeadersBytes;
            BucketContainer.TotalHeadersSizeBytes        += deltaBytes;
            MappedFileBucketInfo->TotalFileDataSizeBytes += deltaBytes;
            cacheBucketHeader.TotalFileDataSizeBytes     += deltaBytes;
        }
    }

    public uint TotalFileIndexSizeBytes
    {
        get
        {
            if (!CanAccessHeaderFromFileView) return cacheBucketHeader.TotalFileIndexBytes;
            cacheBucketHeader.TotalFileIndexBytes = MappedFileBucketInfo->TotalFileIndexBytes;
            return cacheBucketHeader.TotalFileIndexBytes;
        }

        set
        {
            if (!Writable || !CanAccessHeaderFromFileView) return;
            var deltaBytes = value - cacheBucketHeader.TotalFileIndexBytes;

            BucketContainer.TotalFileIndexSizeBytes += deltaBytes;
            // Logger.Info("{0} BucketContainer.TotalFileIndexSizeBytes = {1:###,##0} deltaBytes = {2:#,##0}",
            //             GetType().Name, BucketContainer.TotalFileIndexSizeBytes, deltaBytes);

            MappedFileBucketInfo->TotalFileIndexBytes += deltaBytes;
            cacheBucketHeader.TotalFileIndexBytes     += deltaBytes;
        }
    }

    public bool IsOpen => BucketHeaderFileView != null;

    public bool IsOpenForAppend => BucketFlags.HasBucketCurrentAppendingFlag();


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
        // if (!IsOpen) OpenBucket(asWritable: Writable);
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
        BucketHeaderFileView = alternativeHeaderAndDataFileView ?? SelectBucketHeaderFileView();
        BucketHeaderFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, 0, asWritable);
        Writable = asWritable;
        EnsureHeaderViewReferencesCorrectlyMapped();
        if (BucketHeaderFileView.EnsureViewCoversFileCursorOffsetAndSize(FileCursorOffset, BucketHeaderSizeBytes, Writable))
            EnsureHeaderViewReferencesCorrectlyMapped();
        if (BucketId != 0) BucketContainerIndexEntry = BucketContainer.BucketIndexes.GetBucketIndexInfo(BucketId);

        return this;
    }

    public virtual void InitializeNewBucket(DateTime containingTime)
    {
        var bucketStartTime = TimeSeriesPeriod.ContainingPeriodBoundaryStart(containingTime);
        PeriodStartTime = bucketStartTime;
        var bucketId = BucketContainer.CreateBucketId();

        if (BucketFactory.IsFileRootBucketType) BucketContainer.ContainingSession.Header.HighestBucketId = bucketId;

        BucketId    =  bucketId;
        BucketFlags |= BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending;

        CreatedDateTime                              = DateTime.UtcNow;
        LastAmendedDateTime                          = DateTime.UtcNow;
        TotalDataEntriesCount                        = 0;
        MappedFileBucketInfo->TotalFileDataSizeBytes = 0;
        cacheBucketHeader.TotalFileDataSizeBytes     = 0;
        MappedFileBucketInfo->TotalFileIndexBytes    = 0;
        cacheBucketHeader.TotalFileIndexBytes        = 0;
        MappedFileBucketInfo->TotalFileDataSizeBytes = 0;
        cacheBucketHeader.TotalFileDataSizeBytes     = 0;

        var thisEndBucketTime = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
        var fileEndTime       = BucketContainer.TimeSeriesPeriod.PeriodEnd(BucketContainer.PeriodStartTime);

        if (fileEndTime == thisEndBucketTime) BucketFlags |= BucketFlags.IsLastPossibleSibling;
        BucketContainer.AddNewBucket(this);
        // during open
        BucketContainerIndexEntry               =  BucketContainer.BucketIndexes.GetBucketIndexInfo(BucketId);
        BucketContainer.TotalFileIndexSizeBytes += BucketHeaderSizeBytes;
    }

    public virtual void Dispose()
    {
        CloseBucketFileViews();
    }

    public virtual void CloseBucketFileViews()
    {
        if (!IsOpen) return;
        if (Writable) BucketHeaderFileView!.FlushPtrDataToDisk(MappedFileBucketInfo, (int)BucketHeaderSizeBytes);
        BucketHeaderFileView      = null;
        MappedFileBucketInfo      = null;
        BucketContainerIndexEntry = null;
        Writable                  = false;
    }

    public virtual long CalculateBucketEndFileOffset() => EndAllHeadersSectionFileOffset + (long)TotalFileDataSizeBytes;

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

    public abstract IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext);

    public void EnsureHeaderViewCoversAllHeaders()
    {
        if (BucketHeaderFileView!.EnsureViewCoversFileCursorOffsetAndSize(FileCursorOffset, BucketHeaderSizeBytes))
            EnsureHeaderViewReferencesCorrectlyMapped();
    }

    protected virtual void EnsureHeaderViewReferencesCorrectlyMapped()
    {
        if (BucketHeaderFileView == null) return;
        MappedFileBucketInfo               = (BucketHeader*)BucketHeaderFileView!.FileCursorBufferPointer(FileCursorOffset, shouldGrow: Writable);
        cacheBucketHeader                  = *MappedFileBucketInfo;
        RequiredHeaderViewFileCursorOffset = BucketHeaderFileView.LowerViewFileCursorOffset;
        RequiredHeaderViewLocation         = BucketHeaderFileView.StartAddress;
    }

    ~BucketBase()
    {
        Dispose();
    }

    protected virtual ShiftableMemoryMappedFileView SelectBucketHeaderFileView() =>
        BucketContainer.ContainerIndexAndHeaderFileView(BucketContainer.ContainerDepth + 1, BucketHeaderSizeBytes);
}
