// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public interface ISubBucketContainerBucket<TEntry, TBucket, out TSubBucket> : IIndexedDataBucket
    where TEntry : ITimeSeriesEntry<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>
{
    IEnumerable<TSubBucket> SubBuckets { get; }
}

public abstract class SubBucketOnlyBucket<TEntry, TBucket, TSubBucket> : IndexedDataBucket<TEntry, TBucket>,
    ISubBucketContainerBucket<TEntry, TBucket, TSubBucket>, IMutableBucketContainer
    where TEntry : ITimeSeriesEntry<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>
{
    private static readonly IFLogger                  Logger           = FLoggerFactory.Instance.GetLogger(typeof(SubBucketOnlyBucket<,,>));
    private readonly        List<TSubBucket>          cacheSubBuckets  = new();
    private readonly        BucketFactory<TSubBucket> subBucketFactory = new();
    private                 TSubBucket?               currentlyOpenSubBucket;
    private                 uint                      lastAddedIndexKey;

    protected SubBucketOnlyBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable
      , ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        BucketFlags |= BucketFlags.HasOnlySubBuckets;

    public TSubBucket? LastAddedSubBucket
    {
        get
        {
            var previousLastCreatedBucketNullable = BucketIndexes.LastAddedBucketIndexInfo;
            if (previousLastCreatedBucketNullable != null)
            {
                var previousLastCreated = previousLastCreatedBucketNullable.Value;
                currentlyOpenSubBucket
                    = subBucketFactory.OpenExistingBucket(BucketContainer,
                                                          FileCursorOffset + previousLastCreated.ParentOrFileOffset, Writable
                                                        , ContainingFile.ReadChildrenFileView);
                currentlyOpenSubBucket.CloseBucketFileViews();
            }

            return currentlyOpenSubBucket;
        }
    }

    public uint LastAddedBucketId { get; private set; }

    public int ContainerDepth => BucketContainer.ContainerDepth + 1;

    public ShiftableMemoryMappedFileView ContainerIndexAndHeaderFileView(int depth, uint requiredViewSize) =>
        ContainingFile.ContainerIndexAndHeaderFileView(depth, requiredViewSize);

    public IBucketTrackingSession ContainingSession => ContainingFile;
    public uint                   CreateBucketId()  => LastAddedBucketId <= 0 ? BucketId * 1000 + 1 : LastAddedBucketId + 1;

    public void AddNewBucket(IMutableBucket newChild)
    {
        var previousLastCreatedSubBucketNullable = BucketIndexes.LastAddedBucketIndexInfo;
        if (previousLastCreatedSubBucketNullable != null)
        {
            var previousLastCreated = previousLastCreatedSubBucketNullable.Value;
            var previousFileFlags   = previousLastCreated.BucketFlags;
            var previousLastBucket  = cacheSubBuckets.FirstOrDefault(b => b.BucketId == previousLastCreated.BucketId);
            if (previousLastBucket != null)
            {
                previousLastBucket.OpenBucket(ContainingFile.AmendRelatedFileView, true);
                previousLastBucket.NextSiblingBucketDeltaFileOffset = newChild.FileCursorOffset - previousLastBucket.FileCursorOffset;
                newChild.PreviousSiblingBucketDeltaFileOffset       = previousLastBucket.FileCursorOffset - newChild.FileCursorOffset;
                previousFileFlags                                   = previousLastBucket.BucketFlags;
            }

            previousLastCreated.BucketFlags = previousFileFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            BucketIndexes.Add(lastAddedIndexKey, previousLastCreated);
        }

        var bucketIndexOffset
            = new BucketIndexInfo(newChild.BucketId, newChild.PeriodStartTime, newChild.BucketFlags, newChild.TimeSeriesPeriod
                                , newChild.FileCursorOffset - FileCursorOffset);
        // Logger.Info("BucketId {0} at {1} added child BucketId {2} at {3} with delta {4}", BucketId, FileCursorOffset, newChild.BucketId
        //     , newChild.FileCursorOffset, newChild.FileCursorOffset - FileCursorOffset);
        lastAddedIndexKey = BucketIndexes.NextEmptyIndexKey;
        LastAddedBucketId = newChild.BucketId;
        BucketIndexes.Add(lastAddedIndexKey, bucketIndexOffset);
        cacheSubBuckets.Add((TSubBucket)newChild);
    }


    public IEnumerable<TSubBucket> SubBuckets
    {
        get
        {
            if (!IsOpen || (Writable && cacheSubBuckets.Any())) return cacheSubBuckets;
            if (!CanAccessHeaderFromFileView) RefreshViews();

            cacheSubBuckets.Clear();

            foreach (var subBucketIndexOffset in BucketIndexes.Values)
                if (subBucketIndexOffset is not { BucketFlags: BucketFlags.None })
                {
                    // Logger.Info("BucketId {0} at {1} Attempting to open child BucketId {2} having offset {3} with at {4}", BucketId, FileCursorOffset
                    //     , subBucketIndexOffset.BucketId, subBucketIndexOffset.ParentOrFileOffset
                    //     , FileCursorOffset + subBucketIndexOffset.ParentOrFileOffset);
                    currentlyOpenSubBucket = subBucketFactory.OpenExistingBucket(this
                                                                               , FileCursorOffset + subBucketIndexOffset.ParentOrFileOffset, false
                                                                               , ContainingFile.ReadChildrenFileView);
                    cacheSubBuckets.Add(currentlyOpenSubBucket);
                    currentlyOpenSubBucket.CloseBucketFileViews();
                }

            return cacheSubBuckets;
        }
    }

    public override void Dispose()
    {
        foreach (var childBuckets in cacheSubBuckets) childBuckets.Dispose();
        base.Dispose();
    }

    public override void CloseBucketFileViews()
    {
        foreach (var childBuckets in cacheSubBuckets) childBuckets.CloseBucketFileViews();
        base.CloseBucketFileViews();
    }

    public override void VisitChildrenCacheAndClose()
    {
        foreach (var subBucket in SubBuckets)
        {
            subBucket.RefreshViews();
            subBucket.VisitChildrenCacheAndClose();
        }

        CloseBucketFileViews();
    }

    public override uint CreateBucketId(uint previousHighestBucketId) => BucketId * 1000 + LastAddedBucketId + 1;

    public override long CalculateBucketEndFileOffset() => SubBuckets.Max(sb => sb.CalculateBucketEndFileOffset());

    public override IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext, long? fromFileCursorOffset = null) =>
        SubBuckets.Where(sb => sb.BucketIntersects(readerContext.PeriodRange))
                  .SelectMany(subBucket =>
                  {
                      subBucket.RefreshViews();
                      return subBucket.ReadEntries(readerContext, fromFileCursorOffset);
                  });

    public override unsafe StorageAttemptResult AppendEntry(TEntry entry)
    {
        if (!Writable) return StorageAttemptResult.BucketClosedForAppend;
        var entryStorageTime = entry.StorageTime(StorageTimeResolver);
        var searchResult = currentlyOpenSubBucket?.CheckTimeSupported(entryStorageTime) ??
                           LastAddedSubBucket?.CheckTimeSupported(entryStorageTime) ?? StorageAttemptResult.NoBucketChecked;
        if (searchResult == StorageAttemptResult.BucketClosedForAppend) return searchResult;
        if (searchResult == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenSubBucket!.AppendEntry(entry);
        var parentCheckResult = CheckTimeSupported(entryStorageTime);
        if (parentCheckResult != StorageAttemptResult.PeriodRangeMatched) return parentCheckResult;
        foreach (var existingBucket in SubBuckets)
            if (existingBucket.CheckTimeSupported(entryStorageTime) == StorageAttemptResult.PeriodRangeMatched)
            {
                currentlyOpenSubBucket = (TSubBucket)existingBucket.OpenBucket(ContainingFile.ActiveBucketDataFileView, true);
                return existingBucket.AppendEntry(entry);
            }

        if (searchResult == StorageAttemptResult.NextBucketPeriod &&
            (currentlyOpenSubBucket?.BucketFlags.HasBucketCurrentAppendingFlag() ?? false))
        {
            currentlyOpenSubBucket                     =  currentlyOpenSubBucket.CloseAndCreateNextBucket()!;
            BucketContainerIndexEntry->NumIndexEntries += 1;
            return currentlyOpenSubBucket.AppendEntry(entry);
        }

        currentlyOpenSubBucket = subBucketFactory.CreateNewBucket(this, BucketDataStartFileOffset,
                                                                  entryStorageTime, true);
        BucketContainerIndexEntry->NumIndexEntries = 1;
        return currentlyOpenSubBucket.AppendEntry(entry);
    }

    protected override ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => SelectBucketHeaderAndIndexFileView();
}
