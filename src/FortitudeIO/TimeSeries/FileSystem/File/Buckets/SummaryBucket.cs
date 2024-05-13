#region

using System.Runtime.InteropServices;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SummaryBucketSummaryInfo
{
    public uint BucketId;
    public uint ParentBucketId;
    public long ParentFileOffset;
    public long FileCursorOffset;
    public long BucketPeriodStart;
    public BucketFlags BucketFlags;
    public TimeSeriesPeriod BucketPeriod;
    public TimeSeriesPeriod SummaryEntries;
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SummaryBucketList
{
    public ushort PopulatedSummaryBucketCount;
    public SummaryBucketSummaryInfo FirstSubBucketSummaryInfo;
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SummaryBucketExtensionInfo
{
    public TimeSeriesPeriod SummaryEntriesPeriod;
    public ushort MaxSummaryBucketCount;
    public long SummaryBucketInfoListFileOffset;
    public long NextEntryWriteFileOffsetLocation;
}

public interface ISummaryBucket : IBucket
{
    TimeSeriesPeriod EntriesAvailableSummaryPeriods { get; }
}

public interface ISummaryBucket<TEntry, TSummary> : IBucket<TEntry>, ISummaryBucket
{
    new ISummaryBucket<TEntry, TSummary> MoveToBucket(long fileStartCursorOffset);
    uint SummaryEntriesCount(TimeSeriesPeriod summaryPeriod);
    IEnumerable<TSummary> SummariesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null);
    int CopyTo(List<TSummary> destination, TimeSeriesPeriod summaryPeriod, DateTime? fromDateTime = null, DateTime? toDateTime = null);
}

public interface IMutableSummaryBucket<TEntry, TSummary> : ISummaryBucket<TEntry, TSummary>
{
    public long SummaryBucketInfoListFileOffset { get; set; }

    TimeSeriesPeriod PrepareSummarySubBuckets();
    new IMutableSummaryBucket<TEntry, TSummary> MoveToBucket(long fileStartCursorOffset, bool isWritable);
    bool PopulateSummaryPeriod(TimeSeriesPeriod summaryPeriod);
    void SetSummaryPeriodSerializer(IMessageSerializer useSerializer);
}

public abstract unsafe class SummaryBucket<TEntry, TSummary> : SimpleBucket<TEntry>, IMutableSummaryBucket<TEntry, TSummary>
{
    private SummaryBucketExtensionInfo cacheSummaryBucketExtensionInfo;
    protected List<SummaryBucketSummaryInfo>? CacheSummaryBucketInfos;
    protected List<ISummaryBucket>? CacheSummaryBuckets;
    private SummaryBucketExtensionInfo* mappedFileSummaryBucketExtensionInfo;
    protected IMessageSerializer SummaryMessageSerializer;

    protected SummaryBucket(ShiftableMemoryMappedFileView bucketMappedFileView, long bucketFileCursorOffset, bool writable)
        : base(bucketMappedFileView, bucketFileCursorOffset, writable)
    {
        if (mappedFileSummaryBucketExtensionInfo->SummaryBucketInfoListFileOffset == 0) PrepareSummarySubBuckets();
    }

    private bool CanUseWritableSummaryBufferExtensionInfo =>
        IsOpen && mappedFileSummaryBucketExtensionInfo != null
               && CanUseWritableBufferInfo;

    protected override long EndOfInfoSectionOffset => base.EndOfInfoSectionOffset + sizeof(SummaryBucketExtensionInfo);

    public long SummaryBucketInfoListFileOffset
    {
        get
        {
            if (!CanUseWritableSummaryBufferExtensionInfo) return cacheSummaryBucketExtensionInfo.SummaryBucketInfoListFileOffset;
            cacheSummaryBucketExtensionInfo.SummaryBucketInfoListFileOffset = mappedFileSummaryBucketExtensionInfo->SummaryBucketInfoListFileOffset;
            return cacheSummaryBucketExtensionInfo.SummaryBucketInfoListFileOffset;
        }

        set
        {
            if (value == cacheSummaryBucketExtensionInfo.SummaryBucketInfoListFileOffset || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileSummaryBucketExtensionInfo->SummaryBucketInfoListFileOffset = value;
            cacheSummaryBucketExtensionInfo.SummaryBucketInfoListFileOffset = value;
        }
    }

    public TimeSeriesPeriod EntriesAvailableSummaryPeriods
    {
        get
        {
            if (!CanUseWritableSummaryBufferExtensionInfo || SummaryBucketInfoListFileOffset == 0)
                return cacheSummaryBucketExtensionInfo.SummaryEntriesPeriod;
            var subBucketEntriesList = (SummaryBucketList*)(BucketMappedFileView!.LowerHalfViewVirtualMemoryAddress +
                                                            SummaryBucketInfoListFileOffset -
                                                            BucketMappedFileView.LowerViewFileCursorOffset);
            CacheSummaryBucketInfos ??= new List<SummaryBucketSummaryInfo>();
            CacheSummaryBucketInfos.Clear();
            for (var i = 0; i < subBucketEntriesList->PopulatedSummaryBucketCount; i++)
            {
                var summaryBucketSummaryInfo = &subBucketEntriesList->FirstSubBucketSummaryInfo + sizeof(SummaryBucketSummaryInfo) * i;
                var copyTo = *summaryBucketSummaryInfo;
                CacheSummaryBucketInfos.Add(copyTo);
            }

            mappedFileSummaryBucketExtensionInfo->SummaryEntriesPeriod
                = CacheSummaryBucketInfos.Aggregate(TimeSeriesPeriod.None, (prev, sbsi) => sbsi.SummaryEntries | prev);
            cacheSummaryBucketExtensionInfo.SummaryEntriesPeriod = mappedFileSummaryBucketExtensionInfo->SummaryEntriesPeriod;
            return cacheSummaryBucketExtensionInfo.SummaryEntriesPeriod;
        }
    }

    public uint SummaryEntriesCount(TimeSeriesPeriod summaryPeriod)
    {
        if ((summaryPeriod & EntriesAvailableSummaryPeriods) == 0) return 0;
        var selectedSummaryBucket = CacheSummaryBuckets!.FirstOrDefault(sb => sb.EntriesAvailableSummaryPeriods == summaryPeriod);
        if (selectedSummaryBucket == null)
        {
            if (!CanUseWritableSummaryBufferExtensionInfo) return 0;
            var selectedSummaryBucketInfo = CacheSummaryBucketInfos!.First(sbsi => sbsi.SummaryEntries == summaryPeriod);
            selectedSummaryBucket = OpenSummaryBucket(BucketMappedFileView!, selectedSummaryBucketInfo.FileCursorOffset, false);
            BucketMappedFileView!.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset);
        }

        return selectedSummaryBucket.EntryCount;
    }

    public abstract IEnumerable<TSummary> SummariesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null);

    public abstract int CopyTo(List<TSummary> destination, TimeSeriesPeriod summaryPeriod, DateTime? fromDateTime = null
        , DateTime? toDateTime = null);

    public abstract TimeSeriesPeriod PrepareSummarySubBuckets();

    IMutableSummaryBucket<TEntry, TSummary> IMutableSummaryBucket<TEntry, TSummary>.MoveToBucket(
        long fileStartCursorOffset, bool isWritable) =>
        (IMutableSummaryBucket<TEntry, TSummary>)MoveToBucket(fileStartCursorOffset, false);

    public abstract bool PopulateSummaryPeriod(TimeSeriesPeriod summaryPeriod);

    ISummaryBucket<TEntry, TSummary> ISummaryBucket<TEntry, TSummary>.MoveToBucket(long fileStartCursorOffset) =>
        (ISummaryBucket<TEntry, TSummary>)MoveToBucket(fileStartCursorOffset, false);

    public void SetSummaryPeriodSerializer(IMessageSerializer useSerializer)
    {
        SummaryMessageSerializer = useSerializer;
    }

    public override void OpenBucket(ShiftableMemoryMappedFileView mappedFileView, bool asWritable = false)
    {
        base.OpenBucket(mappedFileView, asWritable);
        mappedFileSummaryBucketExtensionInfo = (SummaryBucketExtensionInfo*)base.EndOfInfoSectionOffset;
        cacheSummaryBucketExtensionInfo = *mappedFileSummaryBucketExtensionInfo;
    }

    protected abstract ISummaryBucket OpenSummaryBucket(ShiftableMemoryMappedFileView mappedFileView, long fileCursorOffset, bool asWritable = false);
}
