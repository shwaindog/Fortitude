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

    protected override long EndOfBucketHeaderSectionOffset => IndexHeaderSectionOffset + sizeof(IndexedContainerHeaderExtension);

    private long IndexHeaderSectionOffset => FileCursorOffset + sizeof(BucketHeader) + headerExtensionRealignmentDelta;

    protected virtual bool CanAccessIndexHeaderFromFileView =>
        base.CanAccessHeaderFromFileView
     && BucketHeaderFileView!.HighestFileCursor > EndOfBucketHeaderSectionOffset
     && mappedIndexedContainerHeader != null;

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
            if (!CanAccessIndexHeaderFromFileView && BucketIndexDictionary != null) return BucketIndexDictionary!;
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
            if (!CanAccessIndexHeaderFromFileView) return BucketIndexDictionary!;
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
            if (!CanAccessIndexHeaderFromFileView) return cacheIndexedContainerHeaderExtension.IndexCount;
            cacheIndexedContainerHeaderExtension.IndexCount = mappedIndexedContainerHeader->IndexCount;
            return cacheIndexedContainerHeaderExtension.IndexCount;
        }

        set
        {
            if (value == cacheIndexedContainerHeaderExtension.IndexCount || !Writable || !CanAccessIndexHeaderFromFileView) return;
            mappedIndexedContainerHeader->IndexCount        = value;
            cacheIndexedContainerHeaderExtension.IndexCount = value;
            BucketHeaderFileView!.EnsureViewCoversFileCursorOffsetAndSize(FileCursorOffset, BucketHeaderSizeBytes);
        }
    }

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        base.OpenBucket(alternativeHeaderAndDataFileView, asWritable);
        headerExtensionRealignmentDelta = (FileCursorOffset + sizeof(BucketHeader)) % 8 > 0 ? 8 - (FileCursorOffset + sizeof(BucketHeader)) % 8 : 0;
        mappedIndexedContainerHeader = (IndexedContainerHeaderExtension*)BucketHeaderFileView!
            .FileCursorBufferPointer(IndexHeaderSectionOffset, shouldGrow: Writable);
        cacheIndexedContainerHeaderExtension = *mappedIndexedContainerHeader;
        if (IndexCount > 0)
        {
            if (BucketHeaderFileView!.EnsureViewCoversFileCursorOffsetAndSize(FileCursorOffset, BucketHeaderSizeBytes))
                EnsureHeaderViewReferencesCorrectlyMapped();
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

    public override void CloseBucketFileViews()
    {
        if (Writable) BucketHeaderFileView!.FlushPtrDataToDisk(mappedIndexedContainerHeader, sizeof(IndexedContainerHeaderExtension));
        BucketIndexDictionary?.CacheAndCloseFileView();
        base.CloseBucketFileViews();
    }

    public override void InitializeNewBucket(DateTime containingTime)
    {
        if (IndexCount <= 0) throw new Exception("IndexedDataBucket must have at least one sub-bucket configured");
        base.InitializeNewBucket(containingTime);
        var ptr = BucketAppenderDataReaderFileView!.FileCursorBufferPointer(BucketIndexFileOffset, shouldGrow: true);
        for (var i = 0; i < CalculateBucketIndexByteSize(IndexCount); i++)
            *ptr++ = 0; // maybe overwritting existing data so should zero out existing data
    }

    protected override void EnsureHeaderViewReferencesCorrectlyMapped()
    {
        if (BucketHeaderFileView == null) return;
        base.EnsureHeaderViewReferencesCorrectlyMapped();
        mappedIndexedContainerHeader
            = (IndexedContainerHeaderExtension*)BucketHeaderFileView!
                .FileCursorBufferPointer(IndexHeaderSectionOffset, shouldGrow: Writable);
        cacheIndexedContainerHeaderExtension = *mappedIndexedContainerHeader;
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
