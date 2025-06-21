// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Session;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct IndexedContainerHeaderExtension
{
    public uint   IndexFileSize;
    public ushort IndexCount;
}

public interface IIndexedDataBucket : IBucket
{
    bool   IndexCompressed       { get; }
    ushort IndexCount            { get; }
    long   BucketIndexFileOffset { get; }

    IReadonlyBucketIndexDictionary BucketIndexes { get; }
}

public interface IMutableIndexedDataBucket : IIndexedDataBucket, IMutableBucket
{
    new ushort                 IndexCount    { get; set; }
    new IBucketIndexDictionary BucketIndexes { get; }
}

public abstract unsafe class IndexedBucket<TEntry, TBucket> : BucketBase<TEntry, TBucket>, IMutableIndexedDataBucket
    where TEntry : ITimeSeriesEntry
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    protected IBucketIndexDictionary?          BucketIndexDictionary;
    private   long                             bucketIndexRealignmentDelta;
    private   IndexedContainerHeaderExtension  cacheIndexedContainerHeaderExtension;
    private   long                             indexBucketHeaderExtensionRealignmentDelta;
    private   IndexedContainerHeaderExtension* mappedIndexedContainerHeader;

    protected IndexedBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable
      , ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        BucketFlags |= BucketFlags.HasBucketIndex;

    protected long EndOfIndexBucketHeaderSectionOffset => StartIndexHeaderSectionOffset + sizeof(IndexedContainerHeaderExtension);

    protected long StartIndexHeaderSectionOffset => EndOfBucketHeaderSectionOffset + indexBucketHeaderExtensionRealignmentDelta;

    protected virtual bool CanAccessIndexHeaderFromFileView =>
        CanAccessHeaderFromFileView
     && BucketHeaderFileView!.HighestFileCursor > EndAllHeadersSectionFileOffset
     && mappedIndexedContainerHeader != null;

    public override long EndAllHeadersSectionFileOffset =>
        BucketIndexFileOffset + CalculateBucketIndexByteSize(cacheIndexedContainerHeaderExtension.IndexCount);

    public virtual long BucketIndexFileOffset => EndOfIndexBucketHeaderSectionOffset + bucketIndexRealignmentDelta;

    public IBucketIndexDictionary BucketIndexes
    {
        get
        {
            if (!CanAccessIndexHeaderFromFileView && BucketIndexDictionary != null) return BucketIndexDictionary!;
            BucketIndexDictionary
                ??= new BucketIndexDictionary(SelectBucketHeaderFileView(), BucketIndexFileOffset
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
                ??= new BucketIndexDictionary(SelectBucketHeaderFileView(), BucketIndexFileOffset
                                            , IndexCount, !Writable);
            return BucketIndexDictionary;
        }
    }

    public bool IndexCompressed => BucketFlags.HasCompressedBucketIndexFlag();

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
            EnsureHeaderViewCoversAllHeaders();
        }
    }

    public override void Dispose()
    {
        if (Writable) BucketHeaderFileView!.FlushPtrDataToDisk(mappedIndexedContainerHeader, sizeof(IndexedContainerHeaderExtension));
        BucketIndexDictionary?.CacheAndCloseFileView();
        base.Dispose();
    }

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        base.OpenBucket(alternativeHeaderAndDataFileView, asWritable);
        indexBucketHeaderExtensionRealignmentDelta = EndOfBucketHeaderSectionOffset % 8 > 0 ? 8 - EndOfBucketHeaderSectionOffset % 8 : 0;
        bucketIndexRealignmentDelta                = EndOfIndexBucketHeaderSectionOffset % 8 > 0 ? 8 - EndOfIndexBucketHeaderSectionOffset % 8 : 0;
        mappedIndexedContainerHeader = (IndexedContainerHeaderExtension*)BucketHeaderFileView!
            .FileCursorBufferPointer(StartIndexHeaderSectionOffset, shouldGrow: Writable);
        cacheIndexedContainerHeaderExtension = *mappedIndexedContainerHeader;
        if (IndexCount > 0)
        {
            EnsureHeaderViewCoversAllHeaders();
            if (BucketIndexDictionary != null)
                BucketIndexDictionary.OpenWithFileView(BucketHeaderFileView!, !Writable);
            else
                BucketIndexDictionary
                    ??= new BucketIndexDictionary(BucketHeaderFileView!, BucketIndexFileOffset, IndexCount, !Writable);
        }

        return this;
    }

    public override void CloseBucketFileViews()
    {
        BucketIndexDictionary?.CacheAndCloseFileView();
        base.CloseBucketFileViews();
    }

    public override void InitializeNewBucket(DateTime containingTime)
    {
        if (IndexCount <= 0) throw new Exception("IndexedDataBucket must have at least one sub-bucket configured");
        base.InitializeNewBucket(containingTime);
        var ptr = BucketHeaderFileView!.FileCursorBufferPointer(BucketIndexFileOffset, shouldGrow: true);
        for (var i = 0; i < CalculateBucketIndexByteSize(IndexCount); i++)
            *ptr++ = 0; // maybe overwritting existing data so should zero out existing data
    }

    protected override void EnsureHeaderViewReferencesCorrectlyMapped()
    {
        if (BucketHeaderFileView == null) return;
        base.EnsureHeaderViewReferencesCorrectlyMapped();
        mappedIndexedContainerHeader
            = (IndexedContainerHeaderExtension*)BucketHeaderFileView!
                .FileCursorBufferPointer(StartIndexHeaderSectionOffset, shouldGrow: Writable);
        cacheIndexedContainerHeaderExtension = *mappedIndexedContainerHeader;
        BucketIndexDictionary?.CacheAndCloseFileView();
        BucketIndexDictionary?.OpenWithFileView(BucketHeaderFileView!, !Writable);
    }

    protected long CalculateBucketIndexByteSize(ushort subBucketCount)
    {
        var sizeOfDictionary = Buckets.BucketIndexDictionary.CalculateDictionarySizeInBytes(subBucketCount, BucketIndexFileOffset);
        return sizeOfDictionary;
    }
}
