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

    TimeSeriesPeriod FileTimePeriod { get; }

    DateTime FileStartPeriod { get; }

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

public interface IBucketTrackingTimeSeriesFile : ITimeSeriesFile
{
    new IMutableTimeSeriesFileHeader Header { get; set; }
    IBucket? CurrentlyOpenBucket { get; set; }
    ShiftableMemoryMappedFileView ActiveBucketAppenderFileView { get; }
    ShiftableMemoryMappedFileView ActiveBucketHeaderFileView { get; }
    ShiftableMemoryMappedFileView AmendRelatedFileView { get; }
    ShiftableMemoryMappedFileView ParentBucketFileView { get; }

    void AddBucket(IMutableBucket bucket);
}

public unsafe class TimeSeriesFile<TBucket, TEntry> : IMutableTimeSeriesFile<TBucket, TEntry>, IBucketTrackingTimeSeriesFile
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    private ShiftableMemoryMappedFileView? activeBucketHeaderView;
    private ShiftableMemoryMappedFileView? amendOffsetView;
    private ShiftableMemoryMappedFileView? bucketAppenderView;
    private BucketFactory<TBucket> bucketFactory = new(true);
    private List<TBucket> cacheBuckets = new();
    private TBucket? currentlyOpenBucket;
    private IBucketIndexDictionary? fileBucketIndexOffsets;
    private IMutableTimeSeriesFileHeader fileHeader;
    private bool isWritable;
    private uint lastAddedIndexKey;
    private ShiftableMemoryMappedFileView? parentBucketReaderView;
    private PagedMemoryMappedFile? timeSeriesMemoryMappedFile;

    public TimeSeriesFile(string filePath, bool isWritable)
    {
        FileName = filePath;
        IsWritable = isWritable;
        timeSeriesMemoryMappedFile = new PagedMemoryMappedFile(filePath);
        var headerFileView = timeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        var ptr = headerFileView.LowerHalfViewVirtualMemoryAddress;
        FileVersion = StreamByteOps.ToUShort(ref ptr);
        if (FileVersion is 0 or 1) // select appropriate header file based on file version
            fileHeader = new TimeSeriesFileHeaderFromV1(headerFileView, isWritable);
        else
            throw new Exception("Unsupported file version being loaded");

        if (fileHeader.BucketType != typeof(TBucket))
            throw new Exception("Attempting to open a file saved with a different bucket type");
        FileTimePeriod = fileHeader.FilePeriod;
        FileStartPeriod = fileHeader.FileStartPeriod;
    }

    public TimeSeriesFile(string filePath, TimeSeriesPeriod filePeriod, DateTime fileStartPeriod,
        FileFlags fileFlags = FileFlags.None, uint internalIndexSize = 0, ushort maxStringSizeBytes = 512)
    {
        FileName = filePath;
        IsWritable = fileFlags.HasWriterOpenedFlag();
        timeSeriesMemoryMappedFile = new PagedMemoryMappedFile(filePath);
        var headerFileView = timeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        var ptr = headerFileView.LowerHalfViewVirtualMemoryAddress;
        FileVersion = StreamByteOps.ToUShort(ref ptr);
        if (FileVersion is 0 or 1) // select appropriate header file based on file version
            fileHeader = TimeSeriesFileHeaderFromV1.NewFileCreateHeader(headerFileView, fileFlags, internalIndexSize, maxStringSizeBytes);
        else
            throw new Exception("Unsupported file version being loaded");

        FileTimePeriod = filePeriod;
        FileStartPeriod = FileTimePeriod.ContainingPeriodBoundaryStart(fileStartPeriod);
        fileHeader.FilePeriod = filePeriod;
        fileHeader.FileStartPeriod = fileStartPeriod;
    }

    public TBucket? LastAddedBucket
    {
        get
        {
            fileBucketIndexOffsets ??= fileHeader.BucketIndexes;
            var previousLastCreatedBucketNullable = fileBucketIndexOffsets.LastAddedBucketIndexInfo;
            if (previousLastCreatedBucketNullable != null)
            {
                var previousLastCreated = previousLastCreatedBucketNullable.Value;
                currentlyOpenBucket = bucketFactory.OpenExistingBucket(this, previousLastCreated.ParentOrFileOffset, false);
            }

            return currentlyOpenBucket;
        }
    }

    IMutableTimeSeriesFileHeader IBucketTrackingTimeSeriesFile.Header
    {
        get => fileHeader;
        set => fileHeader = value;
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

    public ShiftableMemoryMappedFileView ActiveBucketAppenderFileView =>
        bucketAppenderView ??= timeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
            "activeBucketDataAppender", closePagedMemoryMappedFileOnDispose: false);

    public ShiftableMemoryMappedFileView AmendRelatedFileView =>
        amendOffsetView ??= timeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
            "amendRelatedBucketOffsets", closePagedMemoryMappedFileOnDispose: false);

    public ShiftableMemoryMappedFileView ParentBucketFileView =>
        amendOffsetView ??= timeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
            "parentBucketHeader", closePagedMemoryMappedFileOnDispose: false);

    public ShiftableMemoryMappedFileView ActiveBucketHeaderFileView =>
        activeBucketHeaderView ??= timeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
            "activeBucketHeader", closePagedMemoryMappedFileOnDispose: false);

    public void AddBucket(IMutableBucket bucket)
    {
        fileBucketIndexOffsets ??= fileHeader.BucketIndexes;
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
            = new BucketIndexInfo(bucket.BucketId, bucket.BucketPeriodStart, bucket.BucketFlags, bucket.BucketPeriod, bucket.FileCursorOffset);
        lastAddedIndexKey = fileBucketIndexOffsets.NextEmptyIndexKey;
        fileBucketIndexOffsets.Add(lastAddedIndexKey, bucketIndexOffset);
        cacheBuckets.Add((TBucket)bucket);
    }

    public ushort FileVersion { get; }

    IMutableTimeSeriesFileHeader IMutableTimeSeriesFile.Header
    {
        get => fileHeader;
        set => fileHeader = value;
    }

    public ITimeSeriesFileHeader Header => fileHeader;
    public TimeSeriesPeriod FileTimePeriod { get; }

    public DateTime FileStartPeriod { get; }

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
            ?? StorageAttemptResult.TypeNotCompatible;
        if (bucketMatch == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenBucket!.AppendEntry(entry);
        if (bucketMatch == StorageAttemptResult.ForNextTimePeriod)
        {
            currentlyOpenBucket = currentlyOpenBucket!.CloseAndCreateNextBucket();
            return currentlyOpenBucket?.AppendEntry(entry) ?? StorageAttemptResult.FileRangeNotSupported;
        }

        var searchedBucket = GetOrCreateBucketFor(entryTime);
        if (searchedBucket == null) return StorageAttemptResult.FileRangeNotSupported;
        return searchedBucket.AppendEntry(entry);
    }

    public IEnumerable<TEntry> AllEntries =>
        ChronologicallyOrderedBuckets().SelectMany(bucket =>
        {
            if (!bucket.IsOpen) bucket.OpenBucket(asWritable: IsWritable);
            return bucket.AllEntries;
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
        activeBucketHeaderView?.Dispose();
        activeBucketHeaderView = null;
        bucketAppenderView?.Dispose();
        bucketAppenderView = null;
        amendOffsetView?.Dispose();
        amendOffsetView = null;
        parentBucketReaderView?.Dispose();
        parentBucketReaderView = null;
        fileBucketIndexOffsets?.CacheAndCloseFileView();
        fileBucketIndexOffsets = null;
        timeSeriesMemoryMappedFile?.Dispose();
        timeSeriesMemoryMappedFile = null;
    }

    public bool ReopenFile(FileFlags fileFlags = FileFlags.None)
    {
        if (IsOpen) return true;
        timeSeriesMemoryMappedFile = new PagedMemoryMappedFile(FileName);
        var headerFileView = timeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        fileHeader.ReopenFileView(headerFileView, fileFlags);
        return true;
    }

    public TBucket? BucketAt(uint index)
    {
        if (IsOpen) return null;
        fileBucketIndexOffsets ??= fileHeader.BucketIndexes;
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
        fileBucketIndexOffsets ??= fileHeader.BucketIndexes;
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
                           ?? LastAddedBucket?.CheckTimeSupported(storageDateTime) ?? StorageAttemptResult.TypeNotCompatible;

        if (searchResult == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenBucket;
        if (searchResult == StorageAttemptResult.FileRangeNotSupported)
            throw new InvalidOperationException("Entry can not be contained within this file");
        if (searchResult == StorageAttemptResult.TypeNotCompatible)
        {
            var fileEndTime = FileTimePeriod.PeriodEnd(FileStartPeriod);
            if (storageDateTime < FileStartPeriod || storageDateTime > fileEndTime) return null;
        }

        foreach (var existingBucket in ChronologicallyOrderedBuckets())
            if (existingBucket.CheckTimeSupported(storageDateTime) == StorageAttemptResult.PeriodRangeMatched)
                return existingBucket;
        if (!cacheBuckets.Any()) currentlyOpenBucket = bucketFactory.CreateNewBucket(this, Header.EndOfHeaderFileOffset, storageDateTime, true);

        if (IsWritable && searchResult == StorageAttemptResult.ForNextTimePeriod &&
            (currentlyOpenBucket?.BucketFlags.HasBucketCurrentAppendingFlag() ?? false))
            currentlyOpenBucket = (TBucket)currentlyOpenBucket.CloseAndCreateNextBucket()!;
        return currentlyOpenBucket;
    }

    ~TimeSeriesFile()
    {
        Dispose();
    }
}
