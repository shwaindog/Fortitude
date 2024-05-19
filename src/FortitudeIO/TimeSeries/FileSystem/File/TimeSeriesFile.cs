#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

public enum TimeSeriesFileStatus
{
    Unknown
    , New
    , CompleteHealthy
    , IncompleteAppending
    , WriterInterrupted
    , Corrupt
}

public interface ITimeSeriesFile : IDisposable
{
    ushort FileVersion { get; }
    ITimeSeriesFileHeader Header { get; }

    TimeSeriesPeriod TimeSeriesPeriod { get; }

    DateTime PeriodStartTime { get; }

    string FileName { get; }
    bool IsWritable { get; }
    bool IsOpen { get; }
    void Close();
    bool ReopenFile(FileFlags fileFlags = FileFlags.None);
}

public interface ITimeSeriesFile<TBucket> : ITimeSeriesFile where TBucket : class, IBucket
{
    TBucket? BucketAt(uint index);
    IEnumerable<TBucket> BucketsContaining(DateTime? fromDateTime = null, DateTime? toDateTime = null);
    IEnumerable<TBucket> ChronologicallyOrderedBuckets();
}

public interface ITimeSeriesFile<TBucket, TEntry> : ITimeSeriesFile<TBucket>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket where TEntry : ITimeSeriesEntry<TEntry>
{
    IEnumerable<TEntry> AllEntries { get; }
    IEnumerable<TEntry> Entries(DateTime? fromTime = null, DateTime? toTime = null);
}

public interface IMutableTimeSeriesFile : ITimeSeriesFile
{
    new IMutableTimeSeriesFileHeader Header { get; set; }
}

public interface IMutableTimeSeriesFile<TBucket> : ITimeSeriesFile<TBucket>, IMutableTimeSeriesFile
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket
{
    TBucket? GetOrCreateBucketFor(DateTime storageDateTime);
}

public interface IMutableTimeSeriesFile<TBucket, TEntry> : IMutableTimeSeriesFile<TBucket>, ITimeSeriesFile<TBucket, TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    StorageAttemptResult AppendEntry(TEntry entry);
}

public interface IMutableBucketContainer
{
    int ContainerDepth { get; }
    DateTime PeriodStartTime { get; }
    TimeSeriesPeriod TimeSeriesPeriod { get; }
    IBucketIndexDictionary BucketIndexes { get; }
    uint DataEntriesCount { get; set; }
    ulong DataSizeBytes { get; set; }
    uint NonDataSizeBytes { get; set; }
    IBucketTrackingTimeSeriesFile ContainingTimeSeriesFile { get; }
    ShiftableMemoryMappedFileView ContainerHeaderFileView(int depth);
    uint CreateBucketId();
    void AddNewBucket(IMutableBucket newChild);
}

public interface IBucketTrackingTimeSeriesFile : ITimeSeriesFile, IMutableBucketContainer
{
    new bool IsOpen { get; }
    new DateTime PeriodStartTime { get; }
    new TimeSeriesPeriod TimeSeriesPeriod { get; }
    new IMutableTimeSeriesFileHeader Header { get; set; }
    IBucket? CurrentlyOpenBucket { get; set; }
    ShiftableMemoryMappedFileView ActiveBucketDataFileView { get; }
    ShiftableMemoryMappedFileView ActiveBucketHeaderFileView { get; }
    ShiftableMemoryMappedFileView AmendRelatedFileView { get; }
}

public unsafe class TimeSeriesFile<TBucket, TEntry> : IMutableTimeSeriesFile<TBucket, TEntry>, IBucketTrackingTimeSeriesFile
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    private readonly int activeBucketDataViewSizePages;
    private readonly List<ShiftableMemoryMappedFileView> parentBucketsHeaderViews = new();
    private ShiftableMemoryMappedFileView? activeBucketHeaderView;
    private ShiftableMemoryMappedFileView? amendOffsetView;
    private ShiftableMemoryMappedFileView? bucketAppenderView;
    private BucketFactory<TBucket> bucketFactory = new(true);
    private List<TBucket> cacheBuckets = new();
    private TBucket? currentlyOpenBucket;
    private IBucketIndexDictionary? fileBucketIndexOffsets;
    private bool isWritable;
    private uint lastAddedIndexKey;
    private PagedMemoryMappedFile? timeSeriesMemoryMappedFile;

    public TimeSeriesFile(string filePath, bool isWritable, int activeBucketDataViewSizePages = 1)
    {
        this.activeBucketDataViewSizePages = Math.Max(1, activeBucketDataViewSizePages);
        FileName = filePath;
        IsWritable = isWritable;
        timeSeriesMemoryMappedFile = new PagedMemoryMappedFile(filePath);
        var headerFileView = timeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        var ptr = headerFileView.LowerHalfViewVirtualMemoryAddress;
        FileVersion = StreamByteOps.ToUShort(ref ptr);
        if (FileVersion is 0 or 1) // select appropriate header file based on file version
            Header = new TimeSeriesFileHeaderFromV1(headerFileView, isWritable);
        else
            throw new Exception("Unsupported file version being loaded");

        if (Header.BucketType != typeof(TBucket))
            throw new Exception("Attempting to open a file saved with a different bucket type");
        TimeSeriesPeriod = Header.FilePeriod;
        PeriodStartTime = Header.FileStartPeriod;
    }

    public TimeSeriesFile(string filePath, TimeSeriesPeriod filePeriod, DateTime fileStartPeriod, int initialFileSizePages,
        int activeBucketDataViewSizePages = 1, FileFlags fileFlags = FileFlags.None, uint internalIndexSize = 0, ushort maxStringSizeBytes = 512)
    {
        FileName = filePath;
        this.activeBucketDataViewSizePages = Math.Max(1, activeBucketDataViewSizePages);
        IsWritable = fileFlags.HasWriterOpenedFlag();
        timeSeriesMemoryMappedFile = new PagedMemoryMappedFile(filePath, initialFileSizePages);
        var headerFileView = timeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        var ptr = headerFileView.LowerHalfViewVirtualMemoryAddress;
        FileVersion = StreamByteOps.ToUShort(ref ptr);
        if (FileVersion is 0 or 1) // select appropriate header file based on file version
            Header = TimeSeriesFileHeaderFromV1.NewFileCreateHeader(headerFileView, fileFlags, internalIndexSize, maxStringSizeBytes);
        else
            throw new Exception("Unsupported file version being loaded");

        TimeSeriesPeriod = filePeriod;
        PeriodStartTime = TimeSeriesPeriod.ContainingPeriodBoundaryStart(fileStartPeriod);
        Header.FilePeriod = filePeriod;
        Header.FileStartPeriod = fileStartPeriod;
    }

    public TBucket? LastAddedBucket
    {
        get
        {
            fileBucketIndexOffsets ??= Header.BucketIndexes;
            var previousLastCreatedBucketNullable = fileBucketIndexOffsets.LastAddedBucketIndexInfo;
            if (previousLastCreatedBucketNullable != null)
            {
                var previousLastCreated = previousLastCreatedBucketNullable.Value;
                currentlyOpenBucket = bucketFactory.OpenExistingBucket(this, previousLastCreated.ParentOrFileOffset, false);
            }

            return currentlyOpenBucket;
        }
    }

    public uint DataEntriesCount
    {
        get => Header.TotalEntries;
        set => Header.TotalEntries = value;
    }

    public ulong DataSizeBytes
    {
        get => Header.TotalDataSizeBytes;
        set => Header.TotalDataSizeBytes = value;
    }

    public IBucketTrackingTimeSeriesFile ContainingTimeSeriesFile => this;

    public uint CreateBucketId()
    {
        return Header.HighestBucketId += 1;
    }

    public IBucketIndexDictionary BucketIndexes => fileBucketIndexOffsets ??= Header.BucketIndexes;

    public int ContainerDepth => 0;

    public uint NonDataSizeBytes
    {
        get => Header.TotalNonDataSizeBytes;
        set => Header.TotalNonDataSizeBytes = value;
    }

    public IBucket? CurrentlyOpenBucket
    {
        get => currentlyOpenBucket;
        set
        {
            if (value is not TBucket tBucketValue) return;
            currentlyOpenBucket = tBucketValue;
        }
    }

    public ShiftableMemoryMappedFileView ActiveBucketDataFileView =>
        bucketAppenderView ??= timeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
            "activeBucketDataAppender", activeBucketDataViewSizePages, closePagedMemoryMappedFileOnDispose: false);

    public ShiftableMemoryMappedFileView AmendRelatedFileView =>
        amendOffsetView ??= timeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
            "amendRelatedBucketOffsets", closePagedMemoryMappedFileOnDispose: false);

    public ShiftableMemoryMappedFileView ContainerHeaderFileView(int depth)
    {
        for (var i = parentBucketsHeaderViews.Count; i < depth; i++)
            parentBucketsHeaderViews.Add(timeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView("bucketContainerHeaderView_" + i));
        return parentBucketsHeaderViews[depth - 1];
    }

    public ShiftableMemoryMappedFileView ActiveBucketHeaderFileView =>
        activeBucketHeaderView ??= timeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
            "activeBucketHeader", closePagedMemoryMappedFileOnDispose: false);

    public void AddNewBucket(IMutableBucket bucket)
    {
        fileBucketIndexOffsets ??= Header.BucketIndexes;
        var previousLastCreatedBucketNullable = fileBucketIndexOffsets.LastAddedBucketIndexInfo;
        if (previousLastCreatedBucketNullable != null)
        {
            var previousLastCreated = previousLastCreatedBucketNullable.Value;
            var previousFileFlags = previousLastCreated.BucketFlags;
            var previousLastBucket = cacheBuckets.FirstOrDefault(b => b.BucketId == previousLastCreated.BucketId);
            if (previousLastBucket != null)
            {
                previousLastBucket.OpenBucket(AmendRelatedFileView, true);
                previousLastBucket.NextSiblingBucketDeltaFileOffset = bucket.FileCursorOffset - previousLastBucket.FileCursorOffset;
                bucket.PreviousSiblingBucketDeltaFileOffset = previousLastBucket.FileCursorOffset - bucket.FileCursorOffset;
                previousFileFlags = previousLastBucket.BucketFlags;
            }

            previousLastCreated.BucketFlags = previousFileFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            fileBucketIndexOffsets.Add(lastAddedIndexKey, previousLastCreated);
        }

        var bucketIndexOffset
            = new BucketIndexInfo(bucket.BucketId, bucket.PeriodStartTime, bucket.BucketFlags, bucket.TimeSeriesPeriod, bucket.FileCursorOffset);
        lastAddedIndexKey = fileBucketIndexOffsets.NextEmptyIndexKey;
        fileBucketIndexOffsets.Add(lastAddedIndexKey, bucketIndexOffset);
        cacheBuckets.Add((TBucket)bucket);
    }

    public ushort FileVersion { get; }

    public IMutableTimeSeriesFileHeader Header { get; set; }

    ITimeSeriesFileHeader ITimeSeriesFile.Header => Header;
    public TimeSeriesPeriod TimeSeriesPeriod { get; }

    public DateTime PeriodStartTime { get; }

    public string FileName { get; }

    public bool IsWritable
    {
        get => isWritable && IsOpen;
        private set => isWritable = value;
    }

    public bool IsOpen => timeSeriesMemoryMappedFile != null;

    public StorageAttemptResult AppendEntry(TEntry entry)
    {
        if (!IsWritable) return StorageAttemptResult.BucketClosedForAppend;
        var entryTime = entry.StorageTime();
        var bucketMatch = currentlyOpenBucket?.CheckTimeSupported(entryTime) ?? LastAddedBucket?.CheckTimeSupported(entryTime)
            ?? StorageAttemptResult.NoBucketChecked;
        if (bucketMatch == StorageAttemptResult.BucketClosedForAppend) return bucketMatch;
        if (bucketMatch == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenBucket!.AppendEntry(entry);
        if (bucketMatch == StorageAttemptResult.NextBucketPeriod)
        {
            currentlyOpenBucket = currentlyOpenBucket!.CloseAndCreateNextBucket();
            return currentlyOpenBucket?.AppendEntry(entry) ?? StorageAttemptResult.NextFilePeriod;
        }

        var searchedBucket = GetOrCreateBucketFor(entryTime);
        if (searchedBucket == null) return StorageAttemptResult.NextFilePeriod;
        return searchedBucket.AppendEntry(entry);
    }

    public IEnumerable<TEntry> AllEntries =>
        ChronologicallyOrderedBuckets().SelectMany(bucket =>
        {
            if (!bucket.IsOpen) bucket.OpenBucket(asWritable: IsWritable);
            return bucket.AllBucketEntriesFrom();
        });

    public IEnumerable<TEntry> Entries(DateTime? fromTime = null, DateTime? toTime = null) => throw new NotImplementedException();

    public void Dispose()
    {
        Close();
    }

    public void Close()
    {
        IsWritable = false;
        foreach (var bucket in cacheBuckets) bucket.CloseFileView();
        foreach (var depthParentBucketHeaderViews in parentBucketsHeaderViews) depthParentBucketHeaderViews.Dispose();
        parentBucketsHeaderViews.Clear();
        activeBucketHeaderView?.Dispose();
        activeBucketHeaderView = null;
        bucketAppenderView?.Dispose();
        bucketAppenderView = null;
        amendOffsetView?.Dispose();
        amendOffsetView = null;
        fileBucketIndexOffsets?.CacheAndCloseFileView();
        fileBucketIndexOffsets = null;
        Header.CloseFileView();
        timeSeriesMemoryMappedFile?.Dispose();
        timeSeriesMemoryMappedFile = null;
    }

    public bool ReopenFile(FileFlags fileFlags = FileFlags.None)
    {
        if (IsOpen) return true;
        timeSeriesMemoryMappedFile = new PagedMemoryMappedFile(FileName);
        var headerFileView = timeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        Header.ReopenFileView(headerFileView, fileFlags);
        return true;
    }

    public TBucket? BucketAt(uint index)
    {
        if (IsOpen) return null;
        fileBucketIndexOffsets ??= Header.BucketIndexes;
        if (fileBucketIndexOffsets.ContainsKey(index)) return null;
        var bucketIndexInfo = fileBucketIndexOffsets[index];
        currentlyOpenBucket = bucketFactory.OpenExistingBucket(this, bucketIndexInfo.ParentOrFileOffset, IsWritable);
        return currentlyOpenBucket;
    }

    public IEnumerable<TBucket> BucketsContaining(DateTime? fromDateTime = null, DateTime? toDateTime = null)
    {
        if (IsOpen) return Enumerable.Empty<TBucket>();
        ChronologicallyOrderedBuckets();
        return cacheBuckets.Where(b => b.Intersects(fromDateTime, toDateTime));
    }

    public IEnumerable<TBucket> ChronologicallyOrderedBuckets()
    {
        if (IsOpen) return cacheBuckets;
        fileBucketIndexOffsets ??= Header.BucketIndexes;
        cacheBuckets.Clear();

        foreach (var bucketIndexOffset in fileBucketIndexOffsets.Values)
        {
            if (currentlyOpenBucket is { IsOpen: true }) currentlyOpenBucket.CloseFileView();
            if (bucketIndexOffset is { BucketFlags: BucketFlags.None })
            {
                currentlyOpenBucket = bucketFactory.OpenExistingBucket(this, bucketIndexOffset.ParentOrFileOffset, false);
                cacheBuckets.Add(currentlyOpenBucket);
            }
        }

        return cacheBuckets;
    }

    public TBucket? GetOrCreateBucketFor(DateTime storageDateTime)
    {
        var searchResult = currentlyOpenBucket?.CheckTimeSupported(storageDateTime)
                           ?? LastAddedBucket?.CheckTimeSupported(storageDateTime) ?? StorageAttemptResult.NoBucketChecked;

        if (searchResult == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenBucket;
        if (searchResult == StorageAttemptResult.NextFilePeriod)
            throw new InvalidOperationException("Entry can not be contained within this file");
        if (searchResult == StorageAttemptResult.NoBucketChecked)
        {
            var fileEndTime = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
            if (storageDateTime < PeriodStartTime || storageDateTime > fileEndTime) return null;
        }

        foreach (var existingBucket in ChronologicallyOrderedBuckets())
            if (existingBucket.CheckTimeSupported(storageDateTime) == StorageAttemptResult.PeriodRangeMatched)
                return existingBucket;
        if (!cacheBuckets.Any()) currentlyOpenBucket = bucketFactory.CreateNewBucket(this, Header.EndOfHeaderFileOffset, storageDateTime, true);

        if (IsWritable && searchResult == StorageAttemptResult.NextBucketPeriod &&
            (currentlyOpenBucket?.BucketFlags.HasBucketCurrentAppendingFlag() ?? false))
            currentlyOpenBucket = currentlyOpenBucket.CloseAndCreateNextBucket()!;
        return currentlyOpenBucket;
    }

    ~TimeSeriesFile()
    {
        Dispose();
    }
}
