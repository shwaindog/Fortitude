#region

using System.Runtime.InteropServices;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SubBucketContainerHeaderExtension
{
    public ushort MaxSubBucketCount;
}

public interface ISubBucketContainerBucket<TEntry, TBucket, TSubBucket> : IBucket
    where TEntry : ITimeSeriesEntry<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>
{
    ushort MaxSubBucketCount { get; }
    long SubBucketIndexDictionaryFileOffset { get; }
    IReadonlyBucketIndexDictionary SubBucketIndexDictionary { get; }
    IEnumerable<TSubBucket> SubBuckets { get; }
}

public interface IMutableSubBucketContainerBucket : IMutableBucket
{
    uint? LastAddedChildBucketId { get; }
    void AddNewChildBucket(IMutableBucket newChild);
}

public abstract unsafe class SubBucketOnlyBucket<TEntry, TBucket, TSubBucket> : DataBucket<TEntry, TBucket>,
    ISubBucketContainerBucket<TEntry, TBucket, TSubBucket>, IMutableSubBucketContainerBucket
    where TEntry : ITimeSeriesEntry<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>
{
    private readonly List<TSubBucket> cacheSubBuckets = new();
    private readonly BucketFactory<TSubBucket> subBucketFactory = new();
    private SubBucketContainerHeaderExtension cacheSubBucketContainerHeaderExtension;
    private TSubBucket? currentlyOpenSubBucket;
    private long headerExtensionRealignmentDelta;
    private uint lastAddedIndexKey = 0;
    private SubBucketContainerHeaderExtension* mappedFileSubBucketContainerHeaderExtension;
    protected IBucketIndexDictionary? SubBucketIndex;
    private long subBucketIndexRealignmentDelta;

    protected SubBucketOnlyBucket(IBucketTrackingTimeSeriesFile containingTimeSeriesFile, long bucketFileCursorOffset, bool writable)
        : base(containingTimeSeriesFile, bucketFileCursorOffset, writable) =>
        BucketFlags |= BucketFlags.HasOnlySubBuckets;

    protected override long EndOfBucketHeaderSectionOffset =>
        base.EndOfBucketHeaderSectionOffset + sizeof(SubBucketContainerHeaderExtension) + headerExtensionRealignmentDelta;

    protected virtual long SubBucketIndexFileOffset
    {
        get
        {
            subBucketIndexRealignmentDelta = EndOfBucketHeaderSectionOffset % 8;
            return EndOfBucketHeaderSectionOffset + subBucketIndexRealignmentDelta;
        }
    }

    protected virtual long StartSubBucketFileOffset =>
        SubBucketIndexFileOffset + CalculateSubBucketIndexByteSize(MaxSubBucketCount) + subBucketIndexRealignmentDelta;

    public TSubBucket? LastAddedSubBucket
    {
        get
        {
            SubBucketIndex
                ??= new BucketIndexDictionary(BucketAppenderFileView!, SubBucketIndexDictionaryFileOffset, MaxSubBucketCount, Writable);
            var previousLastCreatedBucketNullable = SubBucketIndex.LastAddedBucketIndexInfo;
            if (previousLastCreatedBucketNullable != null)
            {
                var previousLastCreated = previousLastCreatedBucketNullable.Value;
                currentlyOpenSubBucket
                    = subBucketFactory.OpenExistingBucket(ContainingTimeSeriesFile, previousLastCreated.ParentOrFileOffset, Writable);
            }

            return currentlyOpenSubBucket;
        }
    }

    public override IEnumerable<TEntry> AllEntries =>
        SubBuckets.SelectMany(subBucket =>
        {
            if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
            var result = subBucket.AllEntries;
            subBucket.CloseFileView();
            return result;
        });

    public uint? LastAddedChildBucketId => LastAddedSubBucket?.BucketId;

    public void AddNewChildBucket(IMutableBucket newChild)
    {
        SubBucketIndex
            ??= new BucketIndexDictionary(BucketAppenderFileView!, SubBucketIndexDictionaryFileOffset, MaxSubBucketCount, Writable);
        var previousLastCreatedSubBucketNullable = SubBucketIndex.LastAddedBucketIndexInfo;
        if (previousLastCreatedSubBucketNullable != null)
        {
            var previousLastCreated = previousLastCreatedSubBucketNullable.Value;
            var previousFileFlags = previousLastCreated.BucketFlags;
            var previousLastBucket = cacheSubBuckets.FirstOrDefault(b => b.BucketId == previousLastCreated.BucketId);
            if (previousLastBucket != null)
            {
                previousLastBucket.OpenBucket(ContainingTimeSeriesFile.AmendRelatedFileView, true);
                previousLastBucket.NextSiblingBucketDeltaFileOffset = newChild.FileCursorOffset - previousLastBucket.FileCursorOffset;
                newChild.PreviousSiblingBucketDeltaFileOffset = previousLastBucket.FileCursorOffset - newChild.FileCursorOffset;
                previousFileFlags = previousLastBucket.BucketFlags;
            }

            previousLastCreated.BucketFlags = previousFileFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            SubBucketIndex.Add(lastAddedIndexKey, previousLastCreated);
        }

        var bucketIndexOffset
            = new BucketIndexInfo(newChild.BucketId, newChild.BucketPeriodStart, newChild.BucketFlags, newChild.BucketPeriod
                , newChild.FileCursorOffset - FileCursorOffset);
        lastAddedIndexKey = SubBucketIndex.NextEmptyIndexKey;
        SubBucketIndex.Add(lastAddedIndexKey, bucketIndexOffset);
        cacheSubBuckets.Add((TSubBucket)newChild);
    }

    public override void InitializeNewBucket(DateTime containingTime, IMutableSubBucketContainerBucket? parentBucket = null)
    {
        if (MaxSubBucketCount <= 0) throw new Exception("SubBucketOnlyBucket must have at least one sub-bucket configured");
        base.InitializeNewBucket(containingTime, parentBucket);
        var ptr = BucketAppenderFileView!.FileCursorBufferPointer(EndOfBucketHeaderSectionOffset, true);
        for (var i = 0; i < CalculateSubBucketIndexByteSize(MaxSubBucketCount); i++)
            *ptr++ = 0; // maybe overwritting existing data so should zero out existing data
    }

    public override long CalculateBucketEndFileOffset() => SubBuckets.Max(sb => sb.CalculateBucketEndFileOffset());

    public long SubBucketIndexDictionaryFileOffset => base.EndOfBucketHeaderSectionOffset + EndOfBucketHeaderSectionOffset % 8;

    public IReadonlyBucketIndexDictionary SubBucketIndexDictionary
    {
        get
        {
            if (!CanUseWritableBufferInfo) return SubBucketIndex!;
            SubBucketIndex
                ??= new BucketIndexDictionary(BucketAppenderFileView!, SubBucketIndexDictionaryFileOffset, MaxSubBucketCount, Writable);
            return SubBucketIndex;
        }
    }

    public IEnumerable<TSubBucket> SubBuckets
    {
        get
        {
            if (IsOpen) return cacheSubBuckets;
            cacheSubBuckets.Clear();

            foreach (var subBucketIndexOffset in SubBucketIndexDictionary.Values)
            {
                if (currentlyOpenSubBucket is { IsOpen: true }) currentlyOpenSubBucket.CloseFileView();
                if (subBucketIndexOffset is { BucketFlags: BucketFlags.None })
                {
                    currentlyOpenSubBucket = subBucketFactory.OpenExistingBucket(ContainingTimeSeriesFile
                        , FileCursorOffset + subBucketIndexOffset.ParentOrFileOffset, false);
                    cacheSubBuckets.Add(currentlyOpenSubBucket);
                }
            }

            return cacheSubBuckets;
        }
    }

    public ushort MaxSubBucketCount
    {
        get
        {
            if (!CanUseWritableBufferInfo) return cacheSubBucketContainerHeaderExtension.MaxSubBucketCount;
            cacheSubBucketContainerHeaderExtension.MaxSubBucketCount = mappedFileSubBucketContainerHeaderExtension->MaxSubBucketCount;
            return cacheSubBucketContainerHeaderExtension.MaxSubBucketCount;
        }

        set
        {
            if (value == cacheSubBucketContainerHeaderExtension.MaxSubBucketCount || !Writable || !CanUseWritableBufferInfo) return;
            mappedFileSubBucketContainerHeaderExtension->MaxSubBucketCount = value;
            cacheSubBucketContainerHeaderExtension.MaxSubBucketCount = value;
        }
    }

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? mappedFileView = null, bool asWritable = false)
    {
        base.OpenBucket(mappedFileView, asWritable);
        headerExtensionRealignmentDelta = (FileCursorOffset + sizeof(BucketHeader)) % 8;
        var headerExtensionRealignmentDeltaFileOffset = FileCursorOffset + sizeof(BucketHeader) + headerExtensionRealignmentDelta;
        mappedFileSubBucketContainerHeaderExtension
            = (SubBucketContainerHeaderExtension*)BucketAppenderFileView!.FileCursorBufferPointer(headerExtensionRealignmentDeltaFileOffset
                , asWritable);
        cacheSubBucketContainerHeaderExtension = *mappedFileSubBucketContainerHeaderExtension;
        return this;
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

    public override IEnumerable<TEntry> EntriesBetween(DateTime? fromDateTime = null, DateTime? toDateTime = null)
    {
        return SubBuckets.Where(sb => sb.Intersects(fromDateTime, toDateTime))
            .SelectMany(subBucket =>
            {
                if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
                var result = subBucket.EntriesBetween(fromDateTime, toDateTime);
                subBucket.CloseFileView();
                return result;
            });
    }

    public override IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromDateTime = null
        , DateTime? toDateTime = null)
    {
        return SubBuckets.Where(sb => sb.Intersects(fromDateTime, toDateTime))
            .SelectMany(subBucket =>
            {
                if (!subBucket.IsOpen) subBucket.OpenBucket(asWritable: Writable);
                var result = subBucket.EntriesBetween(usingMessageDeserializer, fromDateTime, toDateTime);
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
                           LastAddedSubBucket?.CheckTimeSupported(entryStorageTime) ?? StorageAttemptResult.TypeNotCompatible;
        if (searchResult == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenSubBucket!.AppendEntry(entry);
        var parentCheckResult = CheckTimeSupported(entryStorageTime);
        if (parentCheckResult != StorageAttemptResult.PeriodRangeMatched) return parentCheckResult;
        foreach (var existingBucket in SubBuckets)
            if (existingBucket.CheckTimeSupported(entryStorageTime) == StorageAttemptResult.PeriodRangeMatched)
            {
                currentlyOpenSubBucket = (TSubBucket)existingBucket.OpenBucket(ContainingTimeSeriesFile.ActiveBucketAppenderFileView, true);
                return existingBucket.AppendEntry(entry);
            }

        if (searchResult == StorageAttemptResult.ForNextTimePeriod &&
            (currentlyOpenSubBucket?.BucketFlags.HasBucketCurrentAppendingFlag() ?? false))
        {
            currentlyOpenSubBucket = currentlyOpenSubBucket.CloseAndCreateNextBucket(this)!;
            return currentlyOpenSubBucket.AppendEntry(entry);
        }

        currentlyOpenSubBucket = subBucketFactory.CreateNewBucket(ContainingTimeSeriesFile, StartSubBucketFileOffset,
            entryStorageTime, true, this);
        return currentlyOpenSubBucket.AppendEntry(entry);
    }

    protected override ShiftableMemoryMappedFileView SelectAppenderFileView() => ContainingTimeSeriesFile.ParentBucketFileView;

    protected long CalculateSubBucketIndexByteSize(ushort subBucketCount)
    {
        var sizeOfDictionary = Buckets.SubBucketIndexDictionary.CalculateDictionarySizeInBytes(subBucketCount, SubBucketIndexFileOffset);
        return sizeOfDictionary;
    }
}
