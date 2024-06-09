// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File.Appending;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

public interface ITimeSeriesFileSession : IDisposable
{
    bool IsOpen { get; }
    void Close();
    bool ReopenSession(FileFlags fileFlags = FileFlags.None);
}

public interface ITimeSeriesBucketSession<TBucket> : ITimeSeriesFileSession where TBucket : class, IBucket
{
    TBucket?             BucketAt(uint index);
    IEnumerable<TBucket> BucketsContaining(DateTime? fromDateTime = null, DateTime? toDateTime = null);
    IEnumerable<TBucket> ChronologicallyOrderedBuckets();
}

public interface IMutableTimeSeriesBucketSession<TBucket> : ITimeSeriesBucketSession<TBucket>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket
{
    TBucket? GetOrCreateBucketFor(DateTime storageDateTime);
}

public interface ITimeSeriesEntriesSession<TEntry> : ITimeSeriesFileSession
    where TEntry : ITimeSeriesEntry<TEntry>
{
    IEnumerable<TEntry> StartReaderContext(IReaderContext<TEntry> readerContext);
}

public interface IMutableTimeSeriesEntriesSession<TEntry> : ITimeSeriesEntriesSession<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    AppendResult AppendEntry(TEntry entry);
}

public interface IMutableBucketContainer
{
    int                           ContainerDepth          { get; }
    DateTime                      PeriodStartTime         { get; }
    TimeSeriesPeriod              TimeSeriesPeriod        { get; }
    IBucketIndexDictionary        BucketIndexes           { get; }
    uint                          TotalDataEntriesCount   { get; set; }
    uint                          TotalHeadersSizeBytes   { get; set; }
    ulong                         TotalFileDataSizeBytes  { get; set; }
    uint                          TotalFileIndexSizeBytes { get; set; }
    IBucketTrackingSession        ContainingSession       { get; }
    ShiftableMemoryMappedFileView ContainerIndexAndHeaderFileView(int depth, uint requiredViewSize);
    uint                          CreateBucketId();
    void                          AddNewBucket(IMutableBucket newChild);
}

public interface IBucketTrackingSession : IMutableBucketContainer
{
    IMutableTimeSeriesFileHeader  FileHeader                 { get; }
    IBucket?                      CurrentlyOpenBucket        { get; set; }
    ShiftableMemoryMappedFileView ActiveBucketDataFileView   { get; }
    ShiftableMemoryMappedFileView ActiveBucketHeaderFileView { get; }
    ShiftableMemoryMappedFileView ReadChildrenFileView       { get; }

    FixedByteArrayBuffer UncompressedBuffer { get; }
}

public interface IReaderFileSession<TBucket, TEntry> : ITimeSeriesEntriesSession<TEntry>, ITimeSeriesBucketSession<TBucket>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    void VisitChildrenCacheAndClose();

    IReaderContext<TEntry> GetAllEntriesReader(EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject
      , Func<TEntry>? createNew = null);

    IReaderContext<TEntry> GetEntriesBetweenReader(PeriodRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null);
}

public interface IWriterFileSession<TBucket, TEntry> : IMutableTimeSeriesEntriesSession<TEntry>, IMutableTimeSeriesBucketSession<TBucket>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry> { }

public class TimeSeriesFileSession<TFile, TBucket, TEntry> : IReaderFileSession<TBucket, TEntry>, IWriterFileSession<TBucket, TEntry>
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

    private ShiftableMemoryMappedFileView? activeBucketHeaderView;
    private ShiftableMemoryMappedFileView? amendOffsetView;

    private ISessionAppendContext<TEntry, TBucket>? appendContext;
    private ShiftableMemoryMappedFileView?          bucketAppenderView;
    private TBucket?                                currentlyOpenBucket;
    private bool                                    isWritable;
    private uint                                    lastAddedIndexKey;
    private ShiftableMemoryMappedFileView?          readChildBucketsView;
    private FixedByteArrayBuffer?                   uncompressedBuffer;

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

    public ITimeSeriesFile TimeSeriesFile => timeSeriesFile;

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

    public DateTime         PeriodStartTime  => timeSeriesFile.PeriodStartTime;
    public TimeSeriesPeriod TimeSeriesPeriod => timeSeriesFile.TimeSeriesPeriod;

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
        new TimeSeriesFileReaderContext<TEntry>(this, entryResultSourcing, createNew);

    public IReaderContext<TEntry> GetEntriesBetweenReader(PeriodRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null) =>
        new TimeSeriesFileReaderContext<TEntry>(this, entryResultSourcing, createNew)
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

    public TBucket? BucketAt(uint index)
    {
        if (IsOpen) return null;
        if (BucketIndexes.ContainsKey(index)) return null;
        var bucketIndexInfo = BucketIndexes[index];
        CurrentlyOpenBucket = FileBucketFactory.OpenExistingBucket(this, bucketIndexInfo.ParentOrFileOffset, IsWritable);
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
            var fileEndTime = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
            if (storageDateTime < PeriodStartTime || storageDateTime > fileEndTime) return null;
        }

        if (searchResult == StorageAttemptResult.BucketSearchRange)
            foreach (var existingBucket in ChronologicallyOrderedBuckets())
                if (existingBucket.CheckTimeSupported(storageDateTime) == StorageAttemptResult.PeriodRangeMatched)
                    return existingBucket;
        if (!cacheBuckets.Any())
            CurrentlyOpenBucket = FileBucketFactory.CreateNewBucket(this, FileHeader.EndOfHeaderFileOffset, storageDateTime, true);


        return currentlyOpenBucket;
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
}
