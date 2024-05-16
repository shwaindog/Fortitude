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

public interface ISummaryBucket<TEntry, TSummary> : IBucket<TEntry>, ISummaryBucket where TEntry : ITimeSeriesEntry<TEntry>
{
    uint SummaryEntriesCount(TimeSeriesPeriod summaryPeriod);
    IEnumerable<TSummary> SummariesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null);
    int CopyTo(List<TSummary> destination, TimeSeriesPeriod summaryPeriod, DateTime? fromDateTime = null, DateTime? toDateTime = null);
}

public interface IMutableSummaryBucket<TEntry, TSummary> : ISummaryBucket<TEntry, TSummary> where TEntry : ITimeSeriesEntry<TEntry>
{
    public long SummaryBucketInfoListFileOffset { get; set; }

    TimeSeriesPeriod PrepareSummarySubBuckets();
    bool PopulateSummaryPeriod(TimeSeriesPeriod summaryPeriod);
    void SetSummaryPeriodSerializer(IMessageSerializer useSerializer);
}

public abstract unsafe class SummaryBucket<TEntry, TBucket, TSummary> : DataBucket<TEntry, TBucket>, IMutableSummaryBucket<TEntry, TSummary>
    where TEntry : ITimeSeriesEntry<TEntry> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private SummaryBucketExtensionInfo cacheSummaryBucketExtensionInfo;
    protected List<SummaryBucketSummaryInfo>? CacheSummaryBucketInfos;
    protected List<ISummaryBucket>? CacheSummaryBuckets;
    private SummaryBucketExtensionInfo* mappedFileSummaryBucketExtensionInfo;
    protected IMessageSerializer? SummaryMessageSerializer;

    protected SummaryBucket(IBucketTrackingTimeSeriesFile containingTimeSeriesFile, long bucketFileCursorOffset, bool writable)
        : base(containingTimeSeriesFile, bucketFileCursorOffset, writable)
    {
        if (mappedFileSummaryBucketExtensionInfo->SummaryBucketInfoListFileOffset == 0) PrepareSummarySubBuckets();
    }

    private bool CanUseWritableSummaryBufferExtensionInfo =>
        IsOpen && mappedFileSummaryBucketExtensionInfo != null
               && CanUseWritableBufferInfo;

    protected override long EndOfBucketHeaderSectionOffset => base.EndOfBucketHeaderSectionOffset + sizeof(SummaryBucketExtensionInfo);

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
            var subBucketEntriesList = (SummaryBucketList*)(BucketAppenderFileView!.LowerHalfViewVirtualMemoryAddress +
                                                            SummaryBucketInfoListFileOffset -
                                                            BucketAppenderFileView.LowerViewFileCursorOffset);
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
            selectedSummaryBucket = OpenSummaryBucket(BucketAppenderFileView!, selectedSummaryBucketInfo.FileCursorOffset, false);
            BucketAppenderFileView!.EnsureLowerViewContainsFileCursorOffset(FileCursorOffset);
        }

        return selectedSummaryBucket.EntryCount;
    }

    public abstract IEnumerable<TSummary> SummariesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null);

    public abstract int CopyTo(List<TSummary> destination, TimeSeriesPeriod summaryPeriod, DateTime? fromDateTime = null
        , DateTime? toDateTime = null);

    public abstract TimeSeriesPeriod PrepareSummarySubBuckets();


    public abstract bool PopulateSummaryPeriod(TimeSeriesPeriod summaryPeriod);


    public void SetSummaryPeriodSerializer(IMessageSerializer useSerializer)
    {
        SummaryMessageSerializer = useSerializer;
    }

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? mappedFileView = null, bool asWritable = false)
    {
        base.OpenBucket(mappedFileView, asWritable);
        mappedFileSummaryBucketExtensionInfo = (SummaryBucketExtensionInfo*)base.EndOfBucketHeaderSectionOffset;
        cacheSummaryBucketExtensionInfo = *mappedFileSummaryBucketExtensionInfo;
        return this;
    }

    protected abstract ISummaryBucket OpenSummaryBucket(ShiftableMemoryMappedFileView mappedFileView, long fileCursorOffset, bool asWritable = false);
}
