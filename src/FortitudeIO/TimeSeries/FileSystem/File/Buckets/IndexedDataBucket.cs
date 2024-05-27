// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct IndexedContainerHeaderExtension
{
    public ushort IndexCount;
}

public interface IIndexedDataBucket : IBucket
{
    ushort                         IndexCount            { get; }
    long                           BucketIndexFileOffset { get; }
    IReadonlyBucketIndexDictionary BucketIndexes         { get; }
}

public interface IMutableIndexedDataBucket : IIndexedDataBucket, IMutableBucket
{
    new ushort                 IndexCount    { get; set; }
    new IBucketIndexDictionary BucketIndexes { get; }
}

public abstract unsafe class IndexedDataBucket<TEntry, TBucket> : DataBucket<TEntry, TBucket>, IMutableIndexedDataBucket
    where TEntry : ITimeSeriesEntry<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    protected IBucketIndexDictionary?          BucketIndexDictionary;
    private   long                             bucketIndexRealignmentDelta;
    private   IndexedContainerHeaderExtension  cacheIndexedContainerHeaderExtension;
    private   long                             headerExtensionRealignmentDelta;
    private   IndexedContainerHeaderExtension* mappedIndexedContainerHeader;

    protected IndexedDataBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable
      , ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        BucketFlags |= BucketFlags.HasBucketIndex;

    protected override long EndOfBucketHeaderSectionOffset =>
        base.EndOfBucketHeaderSectionOffset + sizeof(IndexedContainerHeaderExtension) + headerExtensionRealignmentDelta;


    public override long BucketDataStartFileOffset => BucketIndexFileOffset + CalculateBucketIndexByteSize(IndexCount);

    public override uint BucketHeaderSizeBytes => (uint)(HeaderRealignmentDelta + (BucketDataStartFileOffset - FileCursorOffset));

    public virtual long BucketIndexFileOffset
    {
        get
        {
            bucketIndexRealignmentDelta = EndOfBucketHeaderSectionOffset % 8 > 0 ? 8 - EndOfBucketHeaderSectionOffset % 8 : 0;
            return EndOfBucketHeaderSectionOffset + bucketIndexRealignmentDelta;
        }
    }

    public IBucketIndexDictionary BucketIndexes
    {
        get
        {
            if (!CanUseWritableBufferInfo && BucketIndexDictionary != null) return BucketIndexDictionary!;
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

    public ushort IndexCount
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheIndexedContainerHeaderExtension.IndexCount;
            cacheIndexedContainerHeaderExtension.IndexCount = mappedIndexedContainerHeader->IndexCount;
            return cacheIndexedContainerHeaderExtension.IndexCount;
        }

        set
        {
            if (value == cacheIndexedContainerHeaderExtension.IndexCount || !Writable || !CanUseWritableBufferInfo) return;
            mappedIndexedContainerHeader->IndexCount        = value;
            cacheIndexedContainerHeaderExtension.IndexCount = value;
        }
    }

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        base.OpenBucket(alternativeHeaderAndDataFileView, asWritable);
        headerExtensionRealignmentDelta = (FileCursorOffset + sizeof(BucketHeader)) % 8 > 0 ? 8 - (FileCursorOffset + sizeof(BucketHeader)) % 8 : 0;
        var headerExtensionRealignmentDeltaFileOffset = FileCursorOffset + sizeof(BucketHeader) + headerExtensionRealignmentDelta;
        mappedIndexedContainerHeader
            = (IndexedContainerHeaderExtension*)BucketHeaderFileView!.FileCursorBufferPointer(headerExtensionRealignmentDeltaFileOffset
                                                                                            , shouldGrow: asWritable);

        cacheIndexedContainerHeaderExtension = *mappedIndexedContainerHeader;
        if (IndexCount > 0)
        {
            if (BucketIndexDictionary != null)
                BucketIndexDictionary.OpenWithFileView(BucketHeaderFileView!, !Writable);
            else
                BucketIndexDictionary
                    ??= new BucketIndexDictionary(BucketHeaderFileView!, BucketIndexFileOffset, IndexCount, !Writable);
        }

        return this;
    }

    public override void Dispose()
    {
        if (Writable) BucketHeaderFileView!.FlushPtrDataToDisk(mappedIndexedContainerHeader, sizeof(IndexedContainerHeaderExtension));
        BucketIndexDictionary?.CacheAndCloseFileView();
        base.Dispose();
    }

    public override void CloseFileView()
    {
        if (Writable) BucketHeaderFileView!.FlushPtrDataToDisk(mappedIndexedContainerHeader, sizeof(IndexedContainerHeaderExtension));
        BucketIndexDictionary?.CacheAndCloseFileView();
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

    public override IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext, long? fromFileCursorOffset = null)
    {
        var firstMatchingIndexEntry = BucketIndexes.Values.FirstOrDefault(bii => bii.Intersects(readerContext.PeriodRange));
        if (!Equals(firstMatchingIndexEntry, default(BucketIndexInfo)))
            return ReadEntries(readerContext, FileCursorOffset + firstMatchingIndexEntry.ParentOrFileOffset);
        return Enumerable.Empty<TEntry>();
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
