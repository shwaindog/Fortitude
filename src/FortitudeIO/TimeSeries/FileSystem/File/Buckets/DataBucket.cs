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
    protected readonly IBucketTrackingTimeSeriesFile ContainingTimeSeriesFile;
    protected ShiftableMemoryMappedFileView? BucketAppenderFileView;
    protected ShiftableMemoryMappedFileView? BucketHeaderFileView;
    private BucketHeader cacheBucketHeader;
    protected IMessageSerializer? EntrySerializer;
    private MemoryMappedFileBuffer? fileBuffer;
    private long headerRealignmentDelta;
    private BucketHeader* mappedFileBucketInfo;
    private long requiredViewFileCursorOffset;
    private byte* requiredViewVirtualMemoryLocation;
    protected bool Writable;

    protected DataBucket(IBucketTrackingTimeSeriesFile containingTimeSeriesFile,
        long bucketFileCursorOffset, bool writable)
    {
        ContainingTimeSeriesFile = containingTimeSeriesFile;
        containingTimeSeriesFile.CurrentlyOpenBucket = this;
        headerRealignmentDelta = bucketFileCursorOffset % 8;
        FileCursorOffset = bucketFileCursorOffset + headerRealignmentDelta;
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
            BucketFactory.OpenExistingBucket(ContainingTimeSeriesFile, FileCursorOffset + NextSiblingBucketDeltaFileOffset, Writable) :
            default;

    public virtual TBucket? CloseAndCreateNextBucket(IMutableSubBucketContainerBucket? parentBucket = null)
    {
        if (!Writable) return null;
        var nextStartPeriod = BucketPeriod.PeriodEnd(BucketPeriodStart);
        var nextPeriodResult = CheckTimeSupported(nextStartPeriod);
        if (nextPeriodResult == StorageAttemptResult.ForNextTimePeriod)
        {
            BucketFlags = BucketFlags.BucketClosedGracefully | BucketFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            // TODO SubBucketOnlyBucket needs to add up all sizes of containing buckets when closing.
            var bucketEndCursor = BucketFactory.AppendCloseBucketDelimiter(BucketAppenderFileView!, CalculateBucketEndFileOffset());
            return BucketFactory.CreateNewBucket(ContainingTimeSeriesFile, bucketEndCursor, nextStartPeriod, Writable, parentBucket);
        }

        return null;
    }

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

    public abstract TimeSeriesPeriod BucketPeriod { get; }

    public bool Intersects(DateTime? fromTime = null, DateTime? toTime = null) =>
        (BucketPeriodStart < toTime || (toTime == null && fromTime != null))
        && (BucketPeriod.PeriodEnd(BucketPeriodStart) > fromTime || (fromTime == null && toTime != null));

    public DateTime BucketPeriodStart
    {
        get
        {
            if (!CanUseWritableBufferInfo) return DateTime.FromBinary(cacheBucketHeader.BucketPeriodStart);
            cacheBucketHeader.BucketPeriodStart = mappedFileBucketInfo->BucketPeriodStart;
            return DateTime.FromBinary(cacheBucketHeader.BucketPeriodStart);
        }

        set
        {
            if (value.Ticks == cacheBucketHeader.BucketPeriodStart || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketPeriodStart = value.Ticks;
            cacheBucketHeader.BucketPeriodStart = value.Ticks;
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

    public uint EntryCount
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.EntryCount;
            cacheBucketHeader.EntryCount = mappedFileBucketInfo->EntryCount;
            return cacheBucketHeader.EntryCount;
        }

        set
        {
            if (value == cacheBucketHeader.EntryCount || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->EntryCount = value;
            cacheBucketHeader.EntryCount = value;
        }
    }

    public long EntriesBufferFileOffset => EndOfBucketHeaderSectionOffset;

    public long SerializedEntriesBytes
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketHeader.SerializedEntriesBytes;
            cacheBucketHeader.SerializedEntriesBytes = mappedFileBucketInfo->SerializedEntriesBytes;
            return cacheBucketHeader.SerializedEntriesBytes;
        }

        set
        {
            if (value == cacheBucketHeader.SerializedEntriesBytes || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->SerializedEntriesBytes = value;
            cacheBucketHeader.SerializedEntriesBytes = value;
        }
    }

    public IStorageTimeResolver<TEntry>? StorageTimeResolver { get; set; }

    public virtual uint CreateBucketId(uint previouslyHighestBucketId) => previouslyHighestBucketId + 1;

    public StorageAttemptResult CheckTimeSupported(DateTime storageDateTime)
    {
        if (!IsOpen) OpenBucket(asWritable: Writable);
        if (storageDateTime == default || storageDateTime == DateTimeConstants.UnixEpoch) return StorageAttemptResult.StorageTimeNotSupported;
        var bucketPeriod = BucketPeriod;
        var bucketEndTime = bucketPeriod.PeriodEnd(BucketPeriodStart);
        if (storageDateTime >= BucketPeriodStart && storageDateTime < bucketEndTime)
            return StorageAttemptResult.PeriodRangeMatched;
        if (storageDateTime >= bucketEndTime && storageDateTime < bucketPeriod.PeriodEnd(bucketEndTime))
            return StorageAttemptResult.ForNextTimePeriod;
        if (storageDateTime >= bucketPeriod.PreviousPeriodStart(BucketPeriodStart) && storageDateTime < BucketPeriodStart)
            return StorageAttemptResult.ForPreviousTimePeriod;
        var filePeriod = ContainingTimeSeriesFile.FileTimePeriod;
        var fileStartTime = ContainingTimeSeriesFile.FileStartPeriod;
        if (storageDateTime >= fileStartTime && storageDateTime < filePeriod.PeriodEnd(fileStartTime)) return StorageAttemptResult.BucketSearchRange;
        return StorageAttemptResult.FileRangeNotSupported;
    }

    public bool IsOpen => BucketAppenderFileView != null && BucketHeaderFileView != null;

    public bool IsOpenForAppend => BucketFlags.HasBucketCurrentAppendingFlag();

    public IBuffer? DataWriterAtAppendLocation
    {
        get
        {
            if (!IsOpenForAppend || BucketAppenderFileView == null) return null;
            fileBuffer ??= new MemoryMappedFileBuffer(BucketAppenderFileView, false);
            fileBuffer.SetFileWriterAt(BucketAppenderFileView, EntriesBufferFileOffset + SerializedEntriesBytes);
            return fileBuffer;
        }
    }

    public virtual IBucket OpenBucket(ShiftableMemoryMappedFileView? mappedFileView = null, bool asWritable = false)
    {
        BucketHeaderFileView = ContainingTimeSeriesFile.ActiveBucketHeaderFileView;
        BucketHeaderFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, asWritable);
        mappedFileView ??= SelectAppenderFileView();
        mappedFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset, asWritable);
        BucketAppenderFileView = mappedFileView;
        Writable = asWritable;
        requiredViewFileCursorOffset = BucketHeaderFileView.LowerViewFileCursorOffset;
        requiredViewVirtualMemoryLocation = BucketHeaderFileView.LowerHalfViewVirtualMemoryAddress;
        mappedFileBucketInfo = (BucketHeader*)BucketHeaderFileView.FileCursorBufferPointer(FileCursorOffset, asWritable);
        cacheBucketHeader = *mappedFileBucketInfo;
        return this;
    }

    public virtual void InitializeNewBucket(DateTime containingTime, IMutableSubBucketContainerBucket? parentBucket = null)
    {
        var bucketStartTime = BucketPeriod.ContainingPeriodBoundaryStart(containingTime);
        BucketPeriodStart = bucketStartTime;
        var bucketId = parentBucket != null ?
            CreateBucketId(parentBucket.BucketId, parentBucket.LastAddedChildBucketId) :
            CreateBucketId(ContainingTimeSeriesFile.Header.HighestBucketId);
        if (BucketFactory.IsFileRootBucketType) ContainingTimeSeriesFile.Header.HighestBucketId = bucketId;

        BucketId = bucketId;
        BucketFlags = BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending;
        CreatedDateTime = DateTime.UtcNow;
        LastAmendedDateTime = DateTime.UtcNow;
        EntryCount = 0;
        SerializedEntriesBytes = 0;
        var thisEndBucketTime = BucketPeriod.PeriodEnd(BucketPeriodStart);
        if (parentBucket != null)
        {
            var parentBucketEndTime = parentBucket.BucketPeriod.PeriodEnd(parentBucket.BucketPeriodStart);
            if (parentBucketEndTime == thisEndBucketTime) BucketFlags |= BucketFlags.IsLastPossibleSibling;
            ParentBucketId = parentBucket.BucketId;
            ParentDeltaFileOffset = parentBucket.FileCursorOffset - FileCursorOffset;
            parentBucket.AddNewChildBucket(this);
        }
        else
        {
            var fileEndTime = ContainingTimeSeriesFile.FileTimePeriod.PeriodEnd(ContainingTimeSeriesFile.FileStartPeriod);
            if (fileEndTime == thisEndBucketTime) BucketFlags |= BucketFlags.IsLastPossibleSibling;
            ParentBucketId = 0;
            ParentDeltaFileOffset = 0;
            ContainingTimeSeriesFile.AddBucket(this);
        }
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
        Writable = false;
    }

    public virtual long CalculateBucketEndFileOffset() => EntriesBufferFileOffset + SerializedEntriesBytes;

    public abstract IEnumerable<TEntry> AllEntries { get; }

    public virtual IEnumerable<TEntry> EntriesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null)
    {
        foreach (var timeSeriesEntry in AllEntries)
            if (EntryIntersects(timeSeriesEntry, fromDateTime, toDateTime))
                yield return timeSeriesEntry;
    }

    public abstract IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromDateTime = null
        , DateTime? toDateTime = null) where TM : class, IVersionedMessage;

    public abstract int CopyTo(List<TEntry> destination, DateTime? fromDateTime = null, DateTime? toDateTime = null);

    public void SetEntrySerializer(IMessageSerializer useSerializer)
    {
        EntrySerializer = useSerializer;
    }

    public abstract StorageAttemptResult AppendEntry(TEntry entry);

    public virtual uint CreateBucketId(uint parentBucketId, uint? previousSiblingBucketId) =>
        previousSiblingBucketId != null ? previousSiblingBucketId.Value + 1 : parentBucketId * 1000;

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

    protected virtual ShiftableMemoryMappedFileView SelectAppenderFileView() => ContainingTimeSeriesFile.ActiveBucketAppenderFileView;
}
