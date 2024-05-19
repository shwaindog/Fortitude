#region

using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.Protocols.Serdes.Binary;

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
    private readonly List<TSubBucket> cacheSubBuckets = new();
    private readonly BucketFactory<TSubBucket> subBucketFactory = new();
    private TSubBucket? currentlyOpenSubBucket;
    private uint lastAddedIndexKey;

    protected SubBucketOnlyBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable)
        : base(bucketContainer, bucketFileCursorOffset, writable) =>
        BucketFlags |= BucketFlags.HasOnlySubBuckets;

    public TSubBucket? LastAddedSubBucket
    {
        get
        {
            BucketIndexDictionary
                ??= new BucketIndexDictionary(SelectBucketIndexFileView(), BucketIndexFileOffset, IndexCount, !Writable);
            var previousLastCreatedBucketNullable = BucketIndexDictionary.LastAddedBucketIndexInfo;
            if (previousLastCreatedBucketNullable != null)
            {
                var previousLastCreated = previousLastCreatedBucketNullable.Value;
                currentlyOpenSubBucket
                    = subBucketFactory.OpenExistingBucket(BucketContainer, previousLastCreated.ParentOrFileOffset, Writable);
            }

            return currentlyOpenSubBucket;
        }
    }

    public uint LastAddedBucketId { get; private set; }

    public int ContainerDepth => BucketContainer.ContainerDepth + 1;

    public ShiftableMemoryMappedFileView ContainerHeaderFileView(int depth) => BucketContainer.ContainerHeaderFileView(depth);

    public ShiftableMemoryMappedFileView ContainerIndexFileView(int depth) => BucketContainer.ContainerIndexFileView(depth);

    public IBucketTrackingTimeSeriesFile ContainingTimeSeriesFile => ContainingFile;
    public uint CreateBucketId() => LastAddedBucketId <= 0 ? BucketId * 1000 + 1 : LastAddedBucketId + 1;

    public void AddNewBucket(IMutableBucket newChild)
    {
        BucketIndexDictionary
            ??= new BucketIndexDictionary(SelectBucketIndexFileView(), BucketIndexFileOffset, IndexCount, !Writable);
        var previousLastCreatedSubBucketNullable = BucketIndexDictionary.LastAddedBucketIndexInfo;
        if (previousLastCreatedSubBucketNullable != null)
        {
            var previousLastCreated = previousLastCreatedSubBucketNullable.Value;
            var previousFileFlags = previousLastCreated.BucketFlags;
            var previousLastBucket = cacheSubBuckets.FirstOrDefault(b => b.BucketId == previousLastCreated.BucketId);
            if (previousLastBucket != null)
            {
                previousLastBucket.OpenBucket(ContainingFile.AmendRelatedFileView, true);
                previousLastBucket.NextSiblingBucketDeltaFileOffset = newChild.FileCursorOffset - previousLastBucket.FileCursorOffset;
                newChild.PreviousSiblingBucketDeltaFileOffset = previousLastBucket.FileCursorOffset - newChild.FileCursorOffset;
                previousFileFlags = previousLastBucket.BucketFlags;
            }

            previousLastCreated.BucketFlags = previousFileFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            BucketIndexDictionary.Add(lastAddedIndexKey, previousLastCreated);
        }

        var bucketIndexOffset
            = new BucketIndexInfo(newChild.BucketId, newChild.PeriodStartTime, newChild.BucketFlags, newChild.TimeSeriesPeriod
                , newChild.FileCursorOffset - FileCursorOffset);
        lastAddedIndexKey = BucketIndexDictionary.NextEmptyIndexKey;
        LastAddedBucketId = newChild.BucketId;
        BucketIndexDictionary.Add(lastAddedIndexKey, bucketIndexOffset);
        cacheSubBuckets.Add((TSubBucket)newChild);
    }


    public IEnumerable<TSubBucket> SubBuckets
    {
        get
        {
            if (IsOpen) return cacheSubBuckets;
            cacheSubBuckets.Clear();

            foreach (var subBucketIndexOffset in BucketIndexDictionary!.Values)
            {
                if (currentlyOpenSubBucket is { IsOpen: true }) currentlyOpenSubBucket.CloseFileView();
                if (subBucketIndexOffset is { BucketFlags: BucketFlags.None })
                {
                    currentlyOpenSubBucket = subBucketFactory.OpenExistingBucket(BucketContainer
                        , FileCursorOffset + subBucketIndexOffset.ParentOrFileOffset, false);
                    cacheSubBuckets.Add(currentlyOpenSubBucket);
                }
            }

            return cacheSubBuckets;
        }
    }

    public override void Dispose()
    {
        foreach (var childBuckets in cacheSubBuckets) childBuckets.Dispose();
        base.Dispose();
    }

    public override void CloseFileView()
    {
        foreach (var childBuckets in cacheSubBuckets) childBuckets.CloseFileView();
        base.CloseFileView();
    }

    public override uint CreateBucketId(uint previousHighestBucketId) => BucketId * 1000 + LastAddedBucketId + 1;

    public override long CalculateBucketEndFileOffset() => SubBuckets.Max(sb => sb.CalculateBucketEndFileOffset());

    public override IEnumerable<TEntry> AllBucketEntriesFrom(long? fileCursorOffset = null) =>
        SubBuckets.SelectMany(subBucket =>
        {
            if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
            var result = subBucket.AllBucketEntriesFrom();
            subBucket.CloseFileView();
            return result;
        });

    public override IEnumerable<TEntry> EntriesBetween(DateTime? fromTime = null, DateTime? toTime = null)
    {
        return SubBuckets.Where(sb => sb.Intersects(fromTime, toTime))
            .SelectMany(subBucket =>
            {
                if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
                var result = subBucket.EntriesBetween(fromTime, toTime);
                subBucket.CloseFileView();
                return result;
            });
    }

    public override IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromTime = null
        , DateTime? toTime = null)
    {
        return SubBuckets.Where(sb => sb.Intersects(fromTime, toTime))
            .SelectMany(subBucket =>
            {
                if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
                var result = subBucket.EntriesBetween(usingMessageDeserializer, fromTime, toTime);
                subBucket.CloseFileView();
                return result;
            });
    }

    public override IEnumerable<TEntry> EntriesBetween(long fileCursorOffset, DateTime? fromTime = null, DateTime? toTime = null)
    {
        return SubBuckets.Where(sb => sb.Intersects(fromTime, toTime))
            .SelectMany(subBucket =>
            {
                if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
                var result = subBucket.EntriesBetween(fromTime, toTime);
                subBucket.CloseFileView();
                return result;
            });
    }

    public override IEnumerable<TM> EntriesBetween<TM>(long fileCursorOffset, IMessageDeserializer<TM> usingMessageDeserializer
        , DateTime? fromTime = null
        , DateTime? toTime = null)
    {
        return SubBuckets.Where(sb => sb.Intersects(fromTime, toTime))
            .SelectMany(subBucket =>
            {
                if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
                var result = subBucket.EntriesBetween(usingMessageDeserializer, fromTime, toTime);
                subBucket.CloseFileView();
                return result;
            });
    }

    public override int CopyTo(List<TEntry> destination, DateTime? fromDateTime = null, DateTime? toDateTime = null)
    {
        var preAppendSize = destination.Count;
        foreach (var subBucket in SubBuckets.Where(sb => sb.Intersects(fromDateTime, toDateTime)))
        {
            if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
            subBucket.CopyTo(destination, fromDateTime, toDateTime);
            subBucket.CloseFileView();
        }

        return destination.Count - preAppendSize;
    }

    public override StorageAttemptResult AppendEntry(TEntry entry)
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
            currentlyOpenSubBucket = currentlyOpenSubBucket.CloseAndCreateNextBucket()!;
            return currentlyOpenSubBucket.AppendEntry(entry);
        }

        currentlyOpenSubBucket = subBucketFactory.CreateNewBucket(this, BucketDataStartFileOffset,
            entryStorageTime, true);
        return currentlyOpenSubBucket.AppendEntry(entry);
    }

    protected override ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => BucketContainer.ContainerHeaderFileView(ContainerDepth);
    protected virtual ShiftableMemoryMappedFileView SelectBucketIndexFileView() => BucketContainer.ContainerIndexFileView(ContainerDepth);
}
