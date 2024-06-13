// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Session;

public interface IMutableBucketContainer
{
    int   ContainerDepth          { get; }
    uint  TotalDataEntriesCount   { get; set; }
    uint  TotalHeadersSizeBytes   { get; set; }
    ulong TotalFileDataSizeBytes  { get; set; }
    uint  TotalFileIndexSizeBytes { get; set; }

    TimeSeriesPeriodRange  TimeSeriesPeriodRange { get; }
    IBucketIndexDictionary BucketIndexes         { get; }
    IBucketTrackingSession ContainingSession     { get; }

    uint CreateBucketId();
    void AddNewBucket(IMutableBucket newChild);

    ShiftableMemoryMappedFileView ContainerIndexAndHeaderFileView(int depth, uint requiredViewSize);
}

public interface IBucketTrackingSession : IMutableBucketContainer
{
    IMutableTimeSeriesFileHeader FileHeader          { get; }
    IBucket?                     CurrentlyOpenBucket { get; set; }

    ShiftableMemoryMappedFileView ActiveBucketDataFileView   { get; }
    ShiftableMemoryMappedFileView ActiveBucketHeaderFileView { get; }
    ShiftableMemoryMappedFileView ReadChildrenFileView       { get; }
    FixedByteArrayBuffer          UncompressedBuffer         { get; }
}

public interface IFileReaderSession<TEntry> : IReaderSession<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>
{
    bool ReopenSession(FileFlags fileFlags = FileFlags.None);
}

public interface IFileWriterSession<TEntry> : IWriterSession<TEntry>, IFileReaderSession<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry> { }

public class TimeSeriesFileSession<TFile, TBucket, TEntry> : IFileWriterSession<TEntry>
  , IBucketTrackingSession
    where TFile : TimeSeriesFile<TFile, TBucket, TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>
{
    private readonly int                                    activeViewAdditionalMultiple;
    private readonly List<TBucket>                          cacheBuckets = new();
    private readonly int                                    defaultViewSizeBytes;
    private readonly List<ShiftableMemoryMappedFileView>    parentBucketsHeaderAndIndexViews = new();
    private readonly int                                    reserveMultiple;
    private readonly TimeSeriesFile<TFile, TBucket, TEntry> timeSeriesFile;

    private ShiftableMemoryMappedFileView?          activeBucketHeaderView;
    private ShiftableMemoryMappedFileView?          amendOffsetView;
    private ISessionAppendContext<TEntry, TBucket>? appendContext;
    private ShiftableMemoryMappedFileView?          bucketAppenderView;

    private TBucket? currentlyOpenBucket;
    private bool     isWritable;
    private uint     lastAddedIndexKey;

    private ShiftableMemoryMappedFileView? readChildBucketsView;
    private FixedByteArrayBuffer?          uncompressedBuffer;

    public TimeSeriesFileSession(TimeSeriesFile<TFile, TBucket, TEntry> file, bool isWritable,
        int defaultViewSizeBytes = ushort.MaxValue * 2, int reserveMultiple = 2, int activeViewAdditionalMultiple = 2)
    {
        timeSeriesFile                    = file;
        this.reserveMultiple              = reserveMultiple;
        this.activeViewAdditionalMultiple = activeViewAdditionalMultiple;
        this.defaultViewSizeBytes         = Math.Max(ushort.MaxValue * 2, defaultViewSizeBytes);
        IsWritable                        = isWritable;
        IsOpen                            = true;
    }

    protected virtual IBucketFactory<TBucket> FileBucketFactory => timeSeriesFile.RootBucketFactory;

    public bool IsWritable
    {
        get => isWritable && IsOpen;
        private set => isWritable = value;
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
        bucketAppenderView ??= timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
         "activeBucketDataAppender", 0, defaultViewSizeBytes * activeViewAdditionalMultiple, reserveMultiple * activeViewAdditionalMultiple
       , false);

    public ShiftableMemoryMappedFileView ActiveBucketHeaderFileView =>
        activeBucketHeaderView ??= timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
         "activeBucketHeader", 0, defaultViewSizeBytes, reserveMultiple, false);

    public ShiftableMemoryMappedFileView ReadChildrenFileView =>
        readChildBucketsView ??= timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
         "readChildBuckets", 0, defaultViewSizeBytes, reserveMultiple, false);


    public FixedByteArrayBuffer UncompressedBuffer
    {
        get { return uncompressedBuffer ??= new FixedByteArrayBuffer(MemoryUtils.CreateUnmanagedByteArray((long)ushort.MaxValue * 400)); }
    }
    public IMutableTimeSeriesFileHeader FileHeader => timeSeriesFile.Header;

    public uint TotalDataEntriesCount
    {
        get => FileHeader.TotalEntries;
        set => FileHeader.TotalEntries = value;
    }

    public ulong TotalFileDataSizeBytes
    {
        get => FileHeader.TotalFileDataSizeBytes;
        set => FileHeader.TotalFileDataSizeBytes = value;
    }

    public TimeSeriesPeriodRange TimeSeriesPeriodRange => timeSeriesFile.TimeSeriesPeriodRange;

    public IBucketTrackingSession ContainingSession => this;

    public uint CreateBucketId()
    {
        return FileHeader.HighestBucketId += 1;
    }

    public IBucketIndexDictionary BucketIndexes => timeSeriesFile.Header.BucketIndexes;

    public int ContainerDepth => 0;

    public uint TotalHeadersSizeBytes
    {
        get => FileHeader.TotalHeaderSizeBytes;
        set => FileHeader.TotalHeaderSizeBytes = value;
    }

    public uint TotalFileIndexSizeBytes
    {
        get => FileHeader.TotalFileIndexSizeBytes;
        set => FileHeader.TotalFileIndexSizeBytes = value;
    }

    public ShiftableMemoryMappedFileView ContainerIndexAndHeaderFileView(int depth, uint requiredViewSize)
    {
        for (var i = parentBucketsHeaderAndIndexViews.Count; i < depth; i++)
        {
            var headerAndIndex
                = timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView("bucketContainerHeaderView_" + i);
            headerAndIndex.UpperViewTriggerChunkShiftTolerance = requiredViewSize;
            parentBucketsHeaderAndIndexViews.Add(headerAndIndex);
        }

        return parentBucketsHeaderAndIndexViews[depth - 1];
    }

    public void AddNewBucket(IMutableBucket bucket)
    {
        var bucketIndexOffset
            = new BucketIndexInfo(bucket.BucketId, bucket.PeriodStartTime, bucket.BucketFlags, bucket.TimeSeriesPeriod, bucket.FileCursorOffset);
        lastAddedIndexKey = BucketIndexes.NextEmptyIndexKey;
        BucketIndexes.Add(lastAddedIndexKey, bucketIndexOffset);
        cacheBuckets.Add((TBucket)bucket);
    }

    public IReaderContext<TEntry> GetAllEntriesReader(EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null) =>
        new TimeSeriesReaderContext<TEntry>(this, entryResultSourcing, createNew);

    public IReaderContext<TEntry> GetEntriesBetweenReader(TimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null) =>
        new TimeSeriesReaderContext<TEntry>(this, entryResultSourcing, createNew)
        {
            PeriodRange = periodRange
        };

    public IEnumerable<TEntry> StartReaderContext(IReaderContext<TEntry> readerContext) =>
        ChronologicallyOrderedBuckets().SelectMany(bucket =>
        {
            bucket.RefreshViews();
            return bucket.ReadEntries(readerContext);
        });

    public void VisitChildrenCacheAndClose()
    {
        foreach (var bucket in ChronologicallyOrderedBuckets())
        {
            bucket.RefreshViews();
            bucket.VisitChildrenCacheAndClose();
        }

        Close();
    }

    public bool IsOpen { get; private set; }

    public void Close()
    {
        if (!IsOpen) return;
        if (IsWritable) BucketIndexes.FlushIndexToDisk();
        IsOpen     = false;
        IsWritable = false;
        currentlyOpenBucket?.CloseBucketFileViews();
        currentlyOpenBucket = null;
        foreach (var bucket in cacheBuckets) bucket.CloseBucketFileViews();
        foreach (var parentHeaderIndexView in parentBucketsHeaderAndIndexViews) parentHeaderIndexView.Dispose();
        parentBucketsHeaderAndIndexViews.Clear();
        activeBucketHeaderView?.Dispose();
        activeBucketHeaderView = null;
        uncompressedBuffer?.BackingMemoryAddressRange.Dispose();
        uncompressedBuffer = null;
        bucketAppenderView?.Dispose();
        bucketAppenderView = null;
        amendOffsetView?.Dispose();
        amendOffsetView = null;
        readChildBucketsView?.Dispose();
        readChildBucketsView = null;
        timeSeriesFile.DecrementSessionCount();
    }

    public bool ReopenSession(FileFlags fileFlags = FileFlags.None)
    {
        if (IsOpen) return true;
        isWritable = fileFlags.HasWriterOpenedFlag();
        return true;
    }

    public void Dispose()
    {
        Close();
    }

    public AppendResult AppendEntry(TEntry entry)
    {
        if (!IsWritable) return new AppendResult(StorageAttemptResult.BucketClosedForAppend);

        appendContext ??= timeSeriesFile.CreateAppendContext();

        appendContext.PreviousEntry = appendContext.CurrentEntry;
        appendContext.CurrentEntry  = entry;
        appendContext.StorageTime   = entry.StorageTime(timeSeriesFile.StorageTimeResolver);

        var bucketMatch = CurrentlyOpenBucket?.CheckTimeSupported(appendContext.StorageTime)
                       ?? appendContext?.LastAddedRootBucket?.CheckTimeSupported(appendContext.StorageTime)
                       ?? StorageAttemptResult.NoBucketChecked;
        if (bucketMatch == StorageAttemptResult.BucketClosedForAppend) return new AppendResult(bucketMatch);
        if (bucketMatch == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenBucket!.AppendEntry(appendContext!);
        if (bucketMatch == StorageAttemptResult.NextBucketPeriod)
        {
            CurrentlyOpenBucket                = currentlyOpenBucket!.CloseAndCreateNextBucket();
            appendContext!.LastAddedRootBucket = currentlyOpenBucket;
            return currentlyOpenBucket?.AppendEntry(appendContext) ?? new AppendResult(StorageAttemptResult.NextFilePeriod);
        }

        var searchedBucket = GetOrCreateBucketFor(appendContext!.StorageTime);
        if (searchedBucket == null) return new AppendResult(StorageAttemptResult.NextFilePeriod);
        appendContext.LastAddedRootBucket = searchedBucket;
        return searchedBucket.AppendEntry(appendContext);
    }

    public IEnumerable<TBucket> ChronologicallyOrderedBuckets()
    {
        if (!IsOpen) return cacheBuckets;
        cacheBuckets.Clear();

        foreach (var bucketIndexOffset in BucketIndexes.Values)
        {
            if (CurrentlyOpenBucket is { IsOpen: true }) CurrentlyOpenBucket.CloseBucketFileViews();
            if (bucketIndexOffset is { NumEntries: > 0 })
            {
                CurrentlyOpenBucket = FileBucketFactory.OpenExistingBucket(this, bucketIndexOffset.ParentOrFileOffset, false);
                cacheBuckets.Add(currentlyOpenBucket!);
            }
        }

        return cacheBuckets;
    }

    public TBucket? GetOrCreateBucketFor(DateTime storageDateTime)
    {
        var searchResult = CurrentlyOpenBucket?.CheckTimeSupported(storageDateTime)
                        ?? appendContext?.LastAddedRootBucket?.CheckTimeSupported(storageDateTime) ?? StorageAttemptResult.NoBucketChecked;

        if (searchResult == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenBucket;
        if (searchResult == StorageAttemptResult.NextFilePeriod)
            throw new InvalidOperationException("Entry can not be contained within this file");
        if (IsWritable && searchResult == StorageAttemptResult.NextBucketPeriod &&
            (currentlyOpenBucket?.BucketFlags.HasBucketCurrentAppendingFlag() ?? false))
        {
            CurrentlyOpenBucket = currentlyOpenBucket.CloseAndCreateNextBucket()!;
            return currentlyOpenBucket;
        }
        if (searchResult == StorageAttemptResult.NoBucketChecked)
        {
            var fileEndTime = TimeSeriesPeriodRange.PeriodEnd();
            if (storageDateTime < TimeSeriesPeriodRange.PeriodStartTime || storageDateTime > fileEndTime) return null;
        }

        if (searchResult == StorageAttemptResult.BucketSearchRange)
            foreach (var existingBucket in ChronologicallyOrderedBuckets())
                if (existingBucket.CheckTimeSupported(storageDateTime) == StorageAttemptResult.PeriodRangeMatched)
                    return existingBucket;
        if (!cacheBuckets.Any())
            CurrentlyOpenBucket = FileBucketFactory.CreateNewBucket(this, FileHeader.EndOfHeaderFileOffset, storageDateTime, true);


        return currentlyOpenBucket;
    }
}
