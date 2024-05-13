#region

using System.Runtime.InteropServices;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BucketSummaryInfo
{
    public uint BucketId;
    public uint ParentBucketId;
    public long ParentFileOffset;
    public long FileCursorOffset;
    public long BucketPeriodStart;
    public BucketFlags BucketFlags;
    public TimeSeriesPeriod BucketPeriod;
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BucketInfo
{
    public BucketSummaryInfo BucketSummaryInfo;
    public long PreviousSiblingBucketFileOffset;
    public long NextSiblingBucketStartFileOffset;
    public long CreatedDateTime;
    public long LastAmendedDateTime;
    public uint EntryCount;
    public ushort MaxSubBucketCount;
    public long SubBucketSummaryListFileOffset;
    public long EntriesBufferFileOffset;
    public long NextEntryWriteFileOffsetLocation;
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SubBucketList
{
    public ushort PopulatedBucketCount;
    public BucketSummaryInfo FirstSubBucketSummaryInfo;
}

public interface IBucket
{
    uint BucketId { get; }
    BucketFlags BucketFlags { get; }
    uint ParentBucketId { get; }
    long ParentFileOffset { get; }
    long FileCursorOffset { get; }
    TimeSeriesPeriod BucketPeriod { get; }
    DateTime BucketPeriodStart { get; }
    bool IsOpen { get; }
    long PreviousSiblingBucketFileOffset { get; }
    long NextSiblingBucketStartFileOffset { get; }
    DateTime CreatedDateTime { get; }
    DateTime LastAmendedDateTime { get; }
    uint EntryCount { get; }
    ushort MaxSubBucketCount { get; }
    long SubBucketSummaryListFileOffset { get; }
    long EntriesBufferFileOffset { get; }
    IEnumerable<BucketSummaryInfo> SubBucketSummaryInfos { get; }
    IBucket MoveToBucket(long fileStartCursorOffset);
    void OpenBucket(ShiftableMemoryMappedFileView mappedFileView, bool asWritable = false);
}

public interface IBucket<TEntry> : IBucket
{
    new IBucket<TEntry> MoveToBucket(long fileStartCursorOffset);
    IEnumerable<TEntry> EntriesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null);

    IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromDateTime = null, DateTime? toDateTime = null)
        where TM : class, IVersionedMessage;

    int CopyTo(List<TEntry> destination, DateTime? fromDateTime = null, DateTime? toDateTime = null);
}

public interface IMutableBucket<TEntry> : IBucket<TEntry>
{
    new uint BucketId { get; set; }
    new BucketFlags BucketFlags { get; set; }
    new uint ParentBucketId { get; set; }
    new long ParentFileOffset { get; set; }
    new TimeSeriesPeriod BucketPeriod { get; set; }
    new DateTime BucketPeriodStart { get; set; }
    new long PreviousSiblingBucketFileOffset { get; set; }
    new long NextSiblingBucketStartFileOffset { get; set; }
    new DateTime CreatedDateTime { get; set; }
    new DateTime LastAmendedDateTime { get; set; }
    new uint EntryCount { get; set; }
    new ushort MaxSubBucketCount { get; set; }
    new long SubBucketSummaryListFileOffset { get; set; }
    new long EntriesBufferFileOffset { get; set; }
    long NextEntryWriteFileOffsetLocation { get; set; }
    IMutableBucket<TEntry> MoveToBucket(long fileStartCursorOffset, bool isWritable);

    bool AppendEntry(TEntry entry);

    IMutableBucket<TEntry> CloseAndCreateNextBucket();

    void SetEntrySerializer(IMessageSerializer useSerializer);
}

public abstract unsafe class SimpleBucket<TEntry> : IMutableBucket<TEntry>
{
    protected ShiftableMemoryMappedFileView? BucketMappedFileView;
    private BucketInfo cacheBucketInfo;
    protected List<BucketSummaryInfo>? CacheSubBucketSummaryInfos;
    protected IMessageSerializer EntrySerializer;
    private BucketInfo* mappedFileBucketInfo;
    private long requiredViewFileCursorOffset;
    private byte* requiredViewVirtualMemoryLocation;
    protected bool Writable;

    protected SimpleBucket(ShiftableMemoryMappedFileView bucketMappedFileView,
        long bucketFileCursorOffset, bool writable)
    {
        var packRealignment = bucketFileCursorOffset % 8;
        FileCursorOffset = bucketFileCursorOffset + bucketFileCursorOffset;
        // ReSharper disable once VirtualMemberCallInConstructor
        OpenBucket(bucketMappedFileView, writable);
    }

    protected virtual long EndOfInfoSectionOffset => FileCursorOffset + sizeof(BucketInfo);

    protected bool CanUseWritableBufferInfo =>
        IsOpen && mappedFileBucketInfo != null
               && BucketMappedFileView!.LowerHalfViewVirtualMemoryAddress == requiredViewVirtualMemoryLocation
               && BucketMappedFileView!.LowerViewFileCursorOffset == requiredViewFileCursorOffset;

    public uint BucketId
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.BucketSummaryInfo.BucketId;
            cacheBucketInfo.BucketSummaryInfo.BucketId = mappedFileBucketInfo->BucketSummaryInfo.BucketId;
            return cacheBucketInfo.BucketSummaryInfo.BucketId;
        }
        set
        {
            if (value == cacheBucketInfo.BucketSummaryInfo.BucketId || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketSummaryInfo.BucketId = value;
            cacheBucketInfo.BucketSummaryInfo.BucketId = value;
        }
    }

    public uint ParentBucketId
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.BucketSummaryInfo.ParentBucketId;
            cacheBucketInfo.BucketSummaryInfo.ParentBucketId = mappedFileBucketInfo->BucketSummaryInfo.ParentBucketId;
            return cacheBucketInfo.BucketSummaryInfo.ParentBucketId;
        }
        set
        {
            if (value == cacheBucketInfo.BucketSummaryInfo.ParentBucketId || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketSummaryInfo.ParentBucketId = value;
            cacheBucketInfo.BucketSummaryInfo.ParentBucketId = value;
        }
    }

    public long ParentFileOffset
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.BucketSummaryInfo.ParentFileOffset;
            cacheBucketInfo.BucketSummaryInfo.ParentFileOffset = mappedFileBucketInfo->BucketSummaryInfo.ParentFileOffset;
            return cacheBucketInfo.BucketSummaryInfo.ParentFileOffset;
        }
        set
        {
            if (value == cacheBucketInfo.BucketSummaryInfo.ParentFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketSummaryInfo.ParentFileOffset = value;
            cacheBucketInfo.BucketSummaryInfo.ParentFileOffset = value;
        }
    }

    public long FileCursorOffset { get; }

    public BucketFlags BucketFlags
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.BucketSummaryInfo.BucketFlags;
            cacheBucketInfo.BucketSummaryInfo.BucketFlags = mappedFileBucketInfo->BucketSummaryInfo.BucketFlags;
            return cacheBucketInfo.BucketSummaryInfo.BucketFlags;
        }
        set
        {
            if (value == cacheBucketInfo.BucketSummaryInfo.BucketFlags || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketSummaryInfo.BucketFlags = value;
            cacheBucketInfo.BucketSummaryInfo.BucketFlags = value;
        }
    }

    public TimeSeriesPeriod BucketPeriod
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.BucketSummaryInfo.BucketPeriod;
            cacheBucketInfo.BucketSummaryInfo.BucketPeriod = mappedFileBucketInfo->BucketSummaryInfo.BucketPeriod;
            return cacheBucketInfo.BucketSummaryInfo.BucketPeriod;
        }

        set
        {
            if (value == cacheBucketInfo.BucketSummaryInfo.BucketPeriod || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketSummaryInfo.BucketPeriod = value;
            cacheBucketInfo.BucketSummaryInfo.BucketPeriod = value;
        }
    }

    public DateTime BucketPeriodStart
    {
        get
        {
            if (!CanUseWritableBufferInfo) return DateTime.FromBinary(cacheBucketInfo.BucketSummaryInfo.BucketPeriodStart);
            cacheBucketInfo.BucketSummaryInfo.BucketPeriodStart = mappedFileBucketInfo->BucketSummaryInfo.BucketPeriodStart;
            return DateTime.FromBinary(cacheBucketInfo.BucketSummaryInfo.BucketPeriodStart);
        }

        set
        {
            if (value.Ticks == cacheBucketInfo.BucketSummaryInfo.BucketPeriodStart || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->BucketSummaryInfo.BucketPeriodStart = value.Ticks;
            cacheBucketInfo.BucketSummaryInfo.BucketPeriodStart = value.Ticks;
        }
    }

    public long PreviousSiblingBucketFileOffset
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.PreviousSiblingBucketFileOffset;
            cacheBucketInfo.PreviousSiblingBucketFileOffset = mappedFileBucketInfo->PreviousSiblingBucketFileOffset;
            return cacheBucketInfo.PreviousSiblingBucketFileOffset;
        }

        set
        {
            if (value == cacheBucketInfo.PreviousSiblingBucketFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->PreviousSiblingBucketFileOffset = value;
            cacheBucketInfo.PreviousSiblingBucketFileOffset = value;
        }
    }

    public long NextSiblingBucketStartFileOffset
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.NextSiblingBucketStartFileOffset;
            cacheBucketInfo.NextSiblingBucketStartFileOffset = mappedFileBucketInfo->NextSiblingBucketStartFileOffset;
            return cacheBucketInfo.NextSiblingBucketStartFileOffset;
        }

        set
        {
            if (value == cacheBucketInfo.NextSiblingBucketStartFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->NextSiblingBucketStartFileOffset = value;
            cacheBucketInfo.NextSiblingBucketStartFileOffset = value;
        }
    }

    public DateTime CreatedDateTime
    {
        get
        {
            if (!CanUseWritableBufferInfo) return DateTime.FromBinary(cacheBucketInfo.CreatedDateTime);
            cacheBucketInfo.CreatedDateTime = mappedFileBucketInfo->CreatedDateTime;
            return DateTime.FromBinary(cacheBucketInfo.CreatedDateTime);
        }

        set
        {
            if (value.Ticks == cacheBucketInfo.CreatedDateTime || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->CreatedDateTime = value.Ticks;
            cacheBucketInfo.CreatedDateTime = value.Ticks;
        }
    }

    public DateTime LastAmendedDateTime
    {
        get
        {
            if (!CanUseWritableBufferInfo) return DateTime.FromBinary(cacheBucketInfo.LastAmendedDateTime);
            cacheBucketInfo.LastAmendedDateTime = mappedFileBucketInfo->LastAmendedDateTime;
            return DateTime.FromBinary(cacheBucketInfo.LastAmendedDateTime);
        }

        set
        {
            if (value.Ticks == cacheBucketInfo.LastAmendedDateTime || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->LastAmendedDateTime = value.Ticks;
            cacheBucketInfo.LastAmendedDateTime = value.Ticks;
        }
    }

    public uint EntryCount
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.EntryCount;
            cacheBucketInfo.EntryCount = mappedFileBucketInfo->EntryCount;
            return cacheBucketInfo.EntryCount;
        }

        set
        {
            if (value == cacheBucketInfo.EntryCount || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->EntryCount = value;
            cacheBucketInfo.EntryCount = value;
        }
    }

    public ushort MaxSubBucketCount
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.MaxSubBucketCount;
            cacheBucketInfo.MaxSubBucketCount = mappedFileBucketInfo->MaxSubBucketCount;
            return cacheBucketInfo.MaxSubBucketCount;
        }

        set
        {
            if (value == cacheBucketInfo.MaxSubBucketCount || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->MaxSubBucketCount = value;
            cacheBucketInfo.MaxSubBucketCount = value;
        }
    }

    public long SubBucketSummaryListFileOffset
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.SubBucketSummaryListFileOffset;
            cacheBucketInfo.SubBucketSummaryListFileOffset = mappedFileBucketInfo->SubBucketSummaryListFileOffset;
            return cacheBucketInfo.SubBucketSummaryListFileOffset;
        }

        set
        {
            if (value == cacheBucketInfo.SubBucketSummaryListFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->SubBucketSummaryListFileOffset = value;
            cacheBucketInfo.SubBucketSummaryListFileOffset = value;
        }
    }

    public long EntriesBufferFileOffset
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.EntriesBufferFileOffset;
            cacheBucketInfo.EntriesBufferFileOffset = mappedFileBucketInfo->EntriesBufferFileOffset;
            return cacheBucketInfo.EntriesBufferFileOffset;
        }

        set
        {
            if (value == cacheBucketInfo.EntriesBufferFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->EntriesBufferFileOffset = value;
            cacheBucketInfo.EntriesBufferFileOffset = value;
        }
    }

    public long NextEntryWriteFileOffsetLocation
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheBucketInfo.NextEntryWriteFileOffsetLocation;
            cacheBucketInfo.NextEntryWriteFileOffsetLocation = mappedFileBucketInfo->NextEntryWriteFileOffsetLocation;
            return cacheBucketInfo.NextEntryWriteFileOffsetLocation;
        }

        set
        {
            if (value == cacheBucketInfo.NextEntryWriteFileOffsetLocation || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileBucketInfo->NextEntryWriteFileOffsetLocation = value;
            cacheBucketInfo.NextEntryWriteFileOffsetLocation = value;
        }
    }

    public bool IsOpen => BucketMappedFileView != null;

    public virtual void OpenBucket(ShiftableMemoryMappedFileView mappedFileView, bool asWritable = false)
    {
        mappedFileView.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset);
        BucketMappedFileView = mappedFileView;
        Writable = asWritable;
        requiredViewFileCursorOffset = BucketMappedFileView.LowerViewFileCursorOffset;
        requiredViewVirtualMemoryLocation = BucketMappedFileView.LowerHalfViewVirtualMemoryAddress;
        mappedFileBucketInfo = (BucketInfo*)BucketMappedFileView.FileCursorBufferPointer(FileCursorOffset);
        cacheBucketInfo = *mappedFileBucketInfo;
    }

    public IEnumerable<BucketSummaryInfo> SubBucketSummaryInfos
    {
        get
        {
            if (!CanUseWritableBufferInfo || SubBucketSummaryListFileOffset == 0)
                return CacheSubBucketSummaryInfos ?? Enumerable.Empty<BucketSummaryInfo>();
            var subBucketEntriesList = (SubBucketList*)(BucketMappedFileView!.LowerHalfViewVirtualMemoryAddress + SubBucketSummaryListFileOffset -
                                                        BucketMappedFileView.LowerViewFileCursorOffset);
            CacheSubBucketSummaryInfos ??= new List<BucketSummaryInfo>();
            CacheSubBucketSummaryInfos.Clear();
            for (var i = 0; i < subBucketEntriesList->PopulatedBucketCount; i++)
            {
                var subBucketSummary = &subBucketEntriesList->FirstSubBucketSummaryInfo + sizeof(BucketSummaryInfo) * i;
                var copyTo = *subBucketSummary;
                CacheSubBucketSummaryInfos.Add(copyTo);
            }

            return CacheSubBucketSummaryInfos;
        }
    }

    IBucket IBucket.MoveToBucket(long fileStartCursorOffset)
    {
        var nextBucket = MoveToBucket(fileStartCursorOffset, false);
        ClearBucketMappedFileView();
        return nextBucket;
    }

    IBucket<TEntry> IBucket<TEntry>.MoveToBucket(long fileStartCursorOffset)
    {
        var nextBucket = MoveToBucket(fileStartCursorOffset, false);
        ClearBucketMappedFileView();
        return nextBucket;
    }

    public abstract IMutableBucket<TEntry> MoveToBucket(long fileStartCursorOffset, bool isWritable);

    public abstract IEnumerable<TEntry> EntriesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null);

    public abstract IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromDateTime = null
        , DateTime? toDateTime = null) where TM : class, IVersionedMessage;

    public abstract int CopyTo(List<TEntry> destination, DateTime? fromDateTime = null, DateTime? toDateTime = null);

    public abstract bool AppendEntry(TEntry entry);

    public abstract IMutableBucket<TEntry> CloseAndCreateNextBucket();

    public void SetEntrySerializer(IMessageSerializer useSerializer)
    {
        EntrySerializer = useSerializer;
    }

    protected void ClearBucketMappedFileView()
    {
        BucketMappedFileView = null;
        mappedFileBucketInfo = null;
        Writable = false;
    }
}
