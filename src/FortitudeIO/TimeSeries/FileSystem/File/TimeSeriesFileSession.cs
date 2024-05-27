// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

public interface ITimeSeriesFileSession : IDisposable
{
    ITimeSeriesFile TimeSeriesFile { get; }
    bool            IsOpen         { get; }
    bool            IsWritable     { get; }
    void            Close();
    bool            ReopenSession(FileFlags fileFlags = FileFlags.None);
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
    StorageAttemptResult AppendEntry(TEntry entry);
}

public interface IMutableBucketContainer
{
    int                           ContainerDepth    { get; }
    DateTime                      PeriodStartTime   { get; }
    TimeSeriesPeriod              TimeSeriesPeriod  { get; }
    IBucketIndexDictionary        BucketIndexes     { get; }
    uint                          DataEntriesCount  { get; set; }
    ulong                         DataSizeBytes     { get; set; }
    uint                          NonDataSizeBytes  { get; set; }
    IBucketTrackingSession        ContainingSession { get; }
    ShiftableMemoryMappedFileView ContainerIndexAndHeaderFileView(int depth, uint requiredViewSize);
    uint                          CreateBucketId();
    void                          AddNewBucket(IMutableBucket newChild);
}

public interface IBucketTrackingSession : IMutableBucketContainer
{
    IMutableTimeSeriesFileHeader  Header                     { get; }
    IBucket?                      CurrentlyOpenBucket        { get; set; }
    ShiftableMemoryMappedFileView ActiveBucketDataFileView   { get; }
    ShiftableMemoryMappedFileView ActiveBucketHeaderFileView { get; }
    ShiftableMemoryMappedFileView AmendRelatedFileView       { get; }
    ShiftableMemoryMappedFileView ReadChildrenFileView       { get; }
}

public interface IReaderFileSession<TBucket, TEntry> : ITimeSeriesEntriesSession<TEntry>, ITimeSeriesBucketSession<TBucket>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    void                   SetFileDefaultEntryFactory(Func<TEntry> fileDefaultEntryFactory);
    void                   VisitChildrenCacheAndClose();
    IReaderContext<TEntry> GetAllEntriesReader(Func<TEntry>? createNew = null);
    IReaderContext<TEntry> GetEntriesBetweenReader(PeriodRange? periodRange, Func<TEntry>? createNew = null);
}

public interface IWriterFileSession<TBucket, TEntry> : IMutableTimeSeriesEntriesSession<TEntry>, IMutableTimeSeriesBucketSession<TBucket>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry> { }

public class TimeSeriesFileSession<TBucket, TEntry> : IReaderFileSession<TBucket, TEntry>, IWriterFileSession<TBucket, TEntry>
  , IBucketTrackingSession
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    private readonly int                                 activeBucketDataViewSizePages;
    private readonly BucketFactory<TBucket>              bucketFactory                    = new(true);
    private readonly List<TBucket>                       cacheBuckets                     = new();
    private readonly List<ShiftableMemoryMappedFileView> parentBucketsHeaderAndIndexViews = new();
    private readonly TimeSeriesFile<TBucket, TEntry>     timeSeriesFile;
    private          ShiftableMemoryMappedFileView?      activeBucketHeaderView;
    private          ShiftableMemoryMappedFileView?      amendOffsetView;
    private          ShiftableMemoryMappedFileView?      bucketAppenderView;
    private          TBucket?                            currentlyOpenBucket;
    private          bool                                isWritable;
    private          uint                                lastAddedIndexKey;
    private          ShiftableMemoryMappedFileView?      readChildBucketsView;

    public TimeSeriesFileSession(TimeSeriesFile<TBucket, TEntry> file, bool isWritable, int activeBucketDataViewSizePages = 1)
    {
        timeSeriesFile                     = file;
        this.activeBucketDataViewSizePages = Math.Max(1, activeBucketDataViewSizePages);
        IsWritable                         = isWritable;
        IsOpen                             = true;
    }

    public TBucket? LastAddedBucket
    {
        get
        {
            var previousLastCreatedBucketNullable = BucketIndexes.LastAddedBucketIndexInfo;
            if (previousLastCreatedBucketNullable != null)
            {
                var previousLastCreated = previousLastCreatedBucketNullable.Value;
                CurrentlyOpenBucket = bucketFactory.OpenExistingBucket(this, previousLastCreated.ParentOrFileOffset, false);
            }

            return currentlyOpenBucket;
        }
    }

    public IBucket? CurrentlyOpenBucket
    {
        get => currentlyOpenBucket;
        set
        {
            if (value is not TBucket tBucketValue) return;
            if (currentlyOpenBucket == null) SetFileDefaultEntryFactory(tBucketValue.DefaultEntryFactory);
            currentlyOpenBucket = tBucketValue;
        }
    }

    public ShiftableMemoryMappedFileView ActiveBucketDataFileView =>
        bucketAppenderView ??= timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
         "activeBucketDataAppender", activeBucketDataViewSizePages, closePagedMemoryMappedFileOnDispose: false);

    public ShiftableMemoryMappedFileView AmendRelatedFileView =>
        amendOffsetView ??= timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
         "amendRelatedBucketOffsets", closePagedMemoryMappedFileOnDispose: false);

    public ShiftableMemoryMappedFileView ActiveBucketHeaderFileView =>
        activeBucketHeaderView ??= timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
         "activeBucketHeader", closePagedMemoryMappedFileOnDispose: false);


    public ShiftableMemoryMappedFileView ReadChildrenFileView =>
        readChildBucketsView ??= timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView(
         "readChildBuckets", closePagedMemoryMappedFileOnDispose: false);

    public IMutableTimeSeriesFileHeader Header => timeSeriesFile.Header;

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

    public DateTime         PeriodStartTime  => timeSeriesFile.PeriodStartTime;
    public TimeSeriesPeriod TimeSeriesPeriod => timeSeriesFile.TimeSeriesPeriod;

    public IBucketTrackingSession ContainingSession => this;

    public uint CreateBucketId()
    {
        return Header.HighestBucketId += 1;
    }

    public IBucketIndexDictionary BucketIndexes => timeSeriesFile.Header.BucketIndexes;

    public int ContainerDepth => 0;

    public uint NonDataSizeBytes
    {
        get => Header.TotalNonDataSizeBytes;
        set => Header.TotalNonDataSizeBytes = value;
    }

    public ShiftableMemoryMappedFileView ContainerIndexAndHeaderFileView(int depth, uint requiredViewSize)
    {
        var halfViewSizeInPages = Math.Max(1, (int)(requiredViewSize / PagedMemoryMappedFile.PageSize) + 1);
        for (var i = parentBucketsHeaderAndIndexViews.Count; i < depth; i++)
        {
            var headerAndIndex
                = timeSeriesFile.TimeSeriesMemoryMappedFile!.CreateShiftableMemoryMappedFileView("bucketContainerHeaderView_" + i
                                                                                               , halfViewSizeInPages);
            headerAndIndex.UpperViewTriggerChunkShiftTolerance = requiredViewSize;
            parentBucketsHeaderAndIndexViews.Add(headerAndIndex);
        }

        return parentBucketsHeaderAndIndexViews[depth - 1];
    }

    public void AddNewBucket(IMutableBucket bucket)
    {
        var previousLastCreatedBucketNullable = BucketIndexes.LastAddedBucketIndexInfo;
        if (previousLastCreatedBucketNullable != null)
        {
            var previousLastCreated = previousLastCreatedBucketNullable.Value;
            var previousFileFlags   = previousLastCreated.BucketFlags;
            var previousLastBucket  = cacheBuckets.FirstOrDefault(b => b.BucketId == previousLastCreated.BucketId);
            if (previousLastBucket != null)
            {
                previousLastBucket.OpenBucket(AmendRelatedFileView, true);
                previousLastBucket.NextSiblingBucketDeltaFileOffset = bucket.FileCursorOffset - previousLastBucket.FileCursorOffset;
                bucket.PreviousSiblingBucketDeltaFileOffset         = previousLastBucket.FileCursorOffset - bucket.FileCursorOffset;
                previousFileFlags                                   = previousLastBucket.BucketFlags;
            }

            previousLastCreated.BucketFlags = previousFileFlags.Unset(BucketFlags.IsHighestSibling | BucketFlags.BucketCurrentAppending);
            BucketIndexes.Add(lastAddedIndexKey, previousLastCreated);
        }

        var bucketIndexOffset
            = new BucketIndexInfo(bucket.BucketId, bucket.PeriodStartTime, bucket.BucketFlags, bucket.TimeSeriesPeriod, bucket.FileCursorOffset);
        lastAddedIndexKey = BucketIndexes.NextEmptyIndexKey;
        BucketIndexes.Add(lastAddedIndexKey, bucketIndexOffset);
        cacheBuckets.Add((TBucket)bucket);
    }

    public void SetFileDefaultEntryFactory(Func<TEntry>? fileDefaultEntryFactory)
    {
        timeSeriesFile.DefaultEntryFactory ??= fileDefaultEntryFactory;
    }

    public IReaderContext<TEntry> GetAllEntriesReader(Func<TEntry>? createNew = null) => new TimeSeriesFileReaderContext<TEntry>(this, createNew);

    public IReaderContext<TEntry> GetEntriesBetweenReader(PeriodRange? periodRange, Func<TEntry>? createNew = null) =>
        new TimeSeriesFileReaderContext<TEntry>(this, createNew)
        {
            PeriodRange = periodRange
        };

    public ITimeSeriesFile TimeSeriesFile => timeSeriesFile;

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

    public bool IsWritable
    {
        get => isWritable && IsOpen;
        private set => isWritable = value;
    }

    public bool IsOpen { get; private set; }

    public void Close()
    {
        if (!IsOpen) return;
        if (IsWritable) BucketIndexes.FlushIndexToDisk();
        IsOpen     = false;
        IsWritable = false;
        foreach (var bucket in cacheBuckets) bucket.CloseFileView();
        foreach (var parentHeaderIndexView in parentBucketsHeaderAndIndexViews) parentHeaderIndexView.Dispose();
        parentBucketsHeaderAndIndexViews.Clear();
        activeBucketHeaderView?.Dispose();
        activeBucketHeaderView = null;
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
        CurrentlyOpenBucket = bucketFactory.OpenExistingBucket(this, bucketIndexInfo.ParentOrFileOffset, IsWritable);
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
            if (CurrentlyOpenBucket is { IsOpen: true }) CurrentlyOpenBucket.CloseFileView();
            if (bucketIndexOffset is { NumEntries: > 0 })
            {
                CurrentlyOpenBucket = bucketFactory.OpenExistingBucket(this, bucketIndexOffset.ParentOrFileOffset, false);
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

    public StorageAttemptResult AppendEntry(TEntry entry)
    {
        if (!IsWritable) return StorageAttemptResult.BucketClosedForAppend;
        var entryTime = entry.StorageTime();
        var bucketMatch = CurrentlyOpenBucket?.CheckTimeSupported(entryTime) ?? LastAddedBucket?.CheckTimeSupported(entryTime)
         ?? StorageAttemptResult.NoBucketChecked;
        if (bucketMatch == StorageAttemptResult.BucketClosedForAppend) return bucketMatch;
        if (bucketMatch == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenBucket!.AppendEntry(entry);
        if (bucketMatch == StorageAttemptResult.NextBucketPeriod)
        {
            CurrentlyOpenBucket = currentlyOpenBucket!.CloseAndCreateNextBucket();
            return currentlyOpenBucket?.AppendEntry(entry) ?? StorageAttemptResult.NextFilePeriod;
        }

        var searchedBucket = GetOrCreateBucketFor(entryTime);
        if (searchedBucket == null) return StorageAttemptResult.NextFilePeriod;
        return searchedBucket.AppendEntry(entry);
    }

    public TBucket? GetOrCreateBucketFor(DateTime storageDateTime)
    {
        var searchResult = CurrentlyOpenBucket?.CheckTimeSupported(storageDateTime)
                        ?? LastAddedBucket?.CheckTimeSupported(storageDateTime) ?? StorageAttemptResult.NoBucketChecked;

        if (searchResult == StorageAttemptResult.PeriodRangeMatched) return currentlyOpenBucket;
        if (searchResult == StorageAttemptResult.NextFilePeriod)
            throw new InvalidOperationException("Entry can not be contained within this file");
        if (searchResult == StorageAttemptResult.NoBucketChecked)
        {
            var fileEndTime = TimeSeriesPeriod.PeriodEnd(PeriodStartTime);
            if (storageDateTime < PeriodStartTime || storageDateTime > fileEndTime) return null;
        }

        if (searchResult == StorageAttemptResult.BucketSearchRange)
            foreach (var existingBucket in ChronologicallyOrderedBuckets())
                if (existingBucket.CheckTimeSupported(storageDateTime) == StorageAttemptResult.PeriodRangeMatched)
                    return existingBucket;
        if (!cacheBuckets.Any()) CurrentlyOpenBucket = bucketFactory.CreateNewBucket(this, Header.EndOfHeaderFileOffset, storageDateTime, true);

        if (IsWritable && searchResult == StorageAttemptResult.NextBucketPeriod &&
            (currentlyOpenBucket?.BucketFlags.HasBucketCurrentAppendingFlag() ?? false))
            currentlyOpenBucket = currentlyOpenBucket.CloseAndCreateNextBucket()!;

        return currentlyOpenBucket;
    }

    ~TimeSeriesFileSession()
    {
        Dispose();
    }
}
