// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries.FileSystem.File.Appending;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public abstract class SubBucketOnlyBucket<TEntry, TBucket, TSubBucket> : IndexedBucket<TEntry, TBucket>, IMutableBucketContainer
    where TEntry : ITimeSeriesEntry<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SubBucketOnlyBucket<,,>));

    private readonly List<TSubBucket> cacheSubBuckets = new();

    private TSubBucket? currentlyOpenSubBucket;
    private uint        lastAddedIndexKey;
    private DateTime    nextFileIndexReadTime = DateTime.MinValue;

    protected IBucketFactory<TSubBucket>? SubBucketFac;

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
                    = SubBucketFactory.OpenExistingBucket(BucketContainer,
                                                          FileCursorOffset + previousLastCreated.ParentOrFileOffset, Writable
                                                        , OwningSession.ReadChildrenFileView);
                currentlyOpenSubBucket.CloseBucketFileViews();
            }

            return currentlyOpenSubBucket;
        }
    }

    protected virtual IBucketFactory<TSubBucket> SubBucketFactory
    {
        get { return SubBucketFac ??= new BucketFactory<TSubBucket>(); }
    }

    public uint LastAddedBucketId { get; private set; }


    public IEnumerable<TSubBucket> SubBuckets
    {
        get
        {
            if (!IsOpen || (cacheSubBuckets.Any() && nextFileIndexReadTime < DateTime.Now)) return cacheSubBuckets;
            if (!CanAccessHeaderFromFileView) RefreshViews();

            nextFileIndexReadTime = DateTime.Now.AddMinutes(1);
            cacheSubBuckets.Clear();

            foreach (var subBucketIndexOffset in BucketIndexes.Values)
                if (subBucketIndexOffset is not { BucketFlags: BucketFlags.None })
                {
                    // Logger.Info("BucketId {0} at {1} Attempting to open child BucketId {2} having offset {3} with at {4}",
                    //             BucketId, FileCursorOffset, subBucketIndexOffset.BucketId,
                    //             subBucketIndexOffset.ParentOrFileOffset,
                    //             FileCursorOffset + subBucketIndexOffset.ParentOrFileOffset);
                    currentlyOpenSubBucket = SubBucketFactory.OpenExistingBucket(this
                                                                               , FileCursorOffset + subBucketIndexOffset.ParentOrFileOffset, false
                                                                               , OwningSession.ReadChildrenFileView);
                    cacheSubBuckets.Add(currentlyOpenSubBucket);
                    currentlyOpenSubBucket.CloseBucketFileViews();
                }
            return cacheSubBuckets;
        }
    }

    public int ContainerDepth => BucketContainer.ContainerDepth + 1;

    public ShiftableMemoryMappedFileView ContainerIndexAndHeaderFileView(int depth, uint requiredViewSize) =>
        OwningSession.ContainerIndexAndHeaderFileView(depth, requiredViewSize);

    public IBucketTrackingSession ContainingSession => OwningSession;
    public uint                   CreateBucketId()  => LastAddedBucketId <= 0 ? BucketId * 1000 + 1 : LastAddedBucketId + 1;

    public void AddNewBucket(IMutableBucket newChild)
    {
        var bucketIndexOffset
            = new BucketIndexInfo(newChild.BucketId, newChild.PeriodStartTime, newChild.BucketFlags, newChild.TimeSeriesPeriod
                                , newChild.FileCursorOffset - FileCursorOffset);
        lastAddedIndexKey = BucketIndexes.NextEmptyIndexKey;
        LastAddedBucketId = newChild.BucketId;
        // Logger.Info("BucketId {0} at {1} added child BucketId {2} at {3} with key {4} and delta {5}", BucketId, FileCursorOffset, newChild.BucketId
        //           , newChild.FileCursorOffset, lastAddedIndexKey, newChild.FileCursorOffset - FileCursorOffset);
        BucketIndexes.Add(lastAddedIndexKey, bucketIndexOffset);
        cacheSubBuckets.Add((TSubBucket)newChild);
        IndexCount            = (ushort)cacheSubBuckets.Count;
        nextFileIndexReadTime = DateTime.Now.AddMinutes(1);
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

    public override IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext) =>
        SubBuckets.Where(sb => sb.BucketIntersects(readerContext.PeriodRange))
                  .SelectMany(subBucket =>
                  {
                      subBucket.RefreshViews();
                      return subBucket.ReadEntries(readerContext);
                  });

    public override AppendResult AppendEntry(IAppendContext<TEntry> entryContext)
    {
        if (!Writable) return new AppendResult(StorageAttemptResult.BucketClosedForAppend);
        var entryStorageTime = entryContext.StorageTime;
        var searchResult = currentlyOpenSubBucket?.CheckTimeSupported(entryStorageTime) ??
                           LastAddedSubBucket?.CheckTimeSupported(entryStorageTime) ?? StorageAttemptResult.NoBucketChecked;
        if (searchResult == StorageAttemptResult.BucketClosedForAppend) return new AppendResult(searchResult);
        if (searchResult == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenSubBucket!.AppendEntry(entryContext);
        var parentCheckResult = CheckTimeSupported(entryStorageTime);
        if (parentCheckResult != StorageAttemptResult.PeriodRangeMatched) return new AppendResult(parentCheckResult);

        if (searchResult == StorageAttemptResult.NextBucketPeriod &&
            (currentlyOpenSubBucket?.BucketFlags.HasBucketCurrentAppendingFlag() ?? false))
        {
            currentlyOpenSubBucket = currentlyOpenSubBucket.CloseAndCreateNextBucket()!;
            return currentlyOpenSubBucket.AppendEntry(entryContext);
        }
        foreach (var existingBucket in SubBuckets)
            if (existingBucket.CheckTimeSupported(entryStorageTime) == StorageAttemptResult.PeriodRangeMatched)
            {
                currentlyOpenSubBucket = (TSubBucket)existingBucket.OpenBucket(OwningSession.ActiveBucketDataFileView, true);
                return existingBucket.AppendEntry(entryContext);
            }

        currentlyOpenSubBucket = SubBucketFactory.CreateNewBucket(this, EndAllHeadersSectionFileOffset,
                                                                  entryStorageTime, true);
        return currentlyOpenSubBucket.AppendEntry(entryContext);
    }
}
