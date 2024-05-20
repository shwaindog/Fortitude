#region

using System.Runtime.InteropServices;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct IndexedContainerHeaderExtension
{
    public ushort IndexCount;
}

public interface IIndexedDataBucket : IBucket
{
    ushort IndexCount { get; }
    long BucketIndexFileOffset { get; }
    IReadonlyBucketIndexDictionary BucketIndexes { get; }
}

public interface IMutableIndexedDataBucket : IIndexedDataBucket, IMutableBucket
{
    new ushort IndexCount { get; set; }
    new IBucketIndexDictionary BucketIndexes { get; }
}

public abstract unsafe class IndexedDataBucket<TEntry, TBucket> : DataBucket<TEntry, TBucket>, IMutableIndexedDataBucket
    where TEntry : ITimeSeriesEntry<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    protected IBucketIndexDictionary? BucketIndexDictionary;
    private IndexedContainerHeaderExtension cacheIndexedContainerHeaderExtension;
    private long headerExtensionRealignmentDelta;
    private IndexedContainerHeaderExtension* mappedFileSubBucketContainerHeaderExtension;
    private long subBucketIndexRealignmentDelta;

    protected IndexedDataBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable)
        : base(bucketContainer, bucketFileCursorOffset, writable) =>
        BucketFlags |= BucketFlags.HasBucketIndex;

    protected override long EndOfBucketHeaderSectionOffset =>
        base.EndOfBucketHeaderSectionOffset + sizeof(IndexedContainerHeaderExtension) + headerExtensionRealignmentDelta;


    public override long BucketDataStartFileOffset => BucketIndexFileOffset + CalculateBucketIndexByteSize(IndexCount);

    public virtual long BucketIndexFileOffset
    {
        get
        {
            subBucketIndexRealignmentDelta = EndOfBucketHeaderSectionOffset % 8;
            return EndOfBucketHeaderSectionOffset + subBucketIndexRealignmentDelta;
        }
    }

    public IBucketIndexDictionary BucketIndexes
    {
        get
        {
            if (!CanUseWritableBufferInfo) return BucketIndexDictionary!;
            BucketIndexDictionary
                ??= new BucketIndexDictionary(SelectBucketHeaderAndIndexFileView(), BucketIndexFileOffset
                    , IndexCount, !Writable);
            return BucketIndexDictionary;
        }
    }

    IReadonlyBucketIndexDictionary IIndexedDataBucket.BucketIndexes
    {
        get
        {
            if (!CanUseWritableBufferInfo) return BucketIndexDictionary!;
            BucketIndexDictionary
                ??= new BucketIndexDictionary(SelectBucketHeaderAndIndexFileView(), BucketIndexFileOffset
                    , IndexCount, !Writable);
            return BucketIndexDictionary;
        }
    }

    public override uint BucketHeaderSizeBytes => (uint)(HeaderRealignmentDelta + (BucketDataStartFileOffset - FileCursorOffset));

    public ushort IndexCount
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheIndexedContainerHeaderExtension.IndexCount;
            cacheIndexedContainerHeaderExtension.IndexCount = mappedFileSubBucketContainerHeaderExtension->IndexCount;
            return cacheIndexedContainerHeaderExtension.IndexCount;
        }

        set
        {
            if (value == cacheIndexedContainerHeaderExtension.IndexCount || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileSubBucketContainerHeaderExtension->IndexCount = value;
            cacheIndexedContainerHeaderExtension.IndexCount = value;
        }
    }

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        base.OpenBucket(alternativeHeaderAndDataFileView, asWritable);
        headerExtensionRealignmentDelta = (FileCursorOffset + sizeof(BucketHeader)) % 8;
        var headerExtensionRealignmentDeltaFileOffset = FileCursorOffset + sizeof(BucketHeader) + headerExtensionRealignmentDelta;
        mappedFileSubBucketContainerHeaderExtension
            = (IndexedContainerHeaderExtension*)BucketHeaderFileView!.FileCursorBufferPointer(headerExtensionRealignmentDeltaFileOffset
                , shouldGrow: asWritable);
        cacheIndexedContainerHeaderExtension = *mappedFileSubBucketContainerHeaderExtension;
        return this;
    }

    public override void Dispose()
    {
        BucketIndexes.CacheAndCloseFileView();
        base.Dispose();
    }

    public override void CloseFileView()
    {
        BucketIndexes.CacheAndCloseFileView();
        base.CloseFileView();
    }

    public override void InitializeNewBucket(DateTime containingTime)
    {
        if (IndexCount <= 0) throw new Exception("IndexedDataBucket must have at least one sub-bucket configured");
        base.InitializeNewBucket(containingTime);
        var ptr = BucketAppenderFileView!.FileCursorBufferPointer(BucketIndexFileOffset, shouldGrow: true);
        for (var i = 0; i < CalculateBucketIndexByteSize(IndexCount); i++)
            *ptr++ = 0; // maybe overwritting existing data so should zero out existing data
    }

    public override IEnumerable<TEntry> EntriesBetween(DateTime? fromTime = null, DateTime? toTime = null)
    {
        var firstMatchingIndexEntry = BucketIndexes.Values.FirstOrDefault(bii => bii.Intersects(fromTime, toTime));
        if (!Equals(firstMatchingIndexEntry, default(BucketIndexInfo)))
            return EntriesBetween(FileCursorOffset + firstMatchingIndexEntry.ParentOrFileOffset, fromTime, toTime);
        return Enumerable.Empty<TEntry>();
    }

    public override IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromTime = null
        , DateTime? toTime = null)
    {
        var firstMatchingIndexEntry = BucketIndexes.Values.FirstOrDefault(bii => bii.Intersects(fromTime, toTime));
        if (!Equals(firstMatchingIndexEntry, default(BucketIndexInfo)))
            return EntriesBetween(FileCursorOffset + firstMatchingIndexEntry.ParentOrFileOffset, usingMessageDeserializer, fromTime, toTime);
        return Enumerable.Empty<TM>();
    }

    protected override ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => ContainingFile.ActiveBucketHeaderFileView;

    protected virtual ShiftableMemoryMappedFileView SelectBucketHeaderAndIndexFileView() =>
        BucketContainer.ContainerIndexAndHeaderFileView(BucketContainer.ContainerDepth + 1, BucketHeaderSizeBytes);

    protected long CalculateBucketIndexByteSize(ushort subBucketCount)
    {
        var sizeOfDictionary = Buckets.BucketIndexDictionary.CalculateDictionarySizeInBytes(subBucketCount, BucketIndexFileOffset);
        return sizeOfDictionary;
    }
}
