#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using static FortitudeIO.TimeSeries.FileSystem.File.Reading.ResultFlags;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Reading;

public enum ReaderOptions
{
    None = 0
    , ReadFastAsPossible = 1
    , AtEntryStorageTime = 2
    , ConsumerControlled = 4
}

public enum EntryResultSourcing
{
    ReuseSingletonObject = 0
    , FromFactoryFuncUnlimited
    , FromFactoryFuncLimited
    , FromBlockingQueue
    , NewEachEntryUnlimited
    , NewEachEntryLimited
}

public interface IReaderContext<TEntry>
{
    IMessageDeserializer? BucketDeserializer { get; set; }
    IMessageSerializer? ResultWriter { get; set; }
    IMessageBufferContext? ResultBuffer { get; set; }
    IObserver<TEntry>? ResultObserver { get; set; }
    IStorageTimeResolver<TEntry>? StorageTimeResolver { get; set; }
    TEntry PopulateEntrySingleton { get; set; }
    bool ContinueSearching { get; }
    Func<TEntry>? SourceEntryFactory { get; set; }
    List<TEntry> ResultList { get; }
    Action<TEntry>? CallbackAction { get; set; }
    TEntry? FirstResult { get; set; }
    TEntry? LastResult { get; set; }
    TEntry GetNextEntryToPopulate { get; }
    IEnumerable<TEntry> ResultEnumerable { get; }
    IEnumerable<List<TEntry>> BatchedResultEnumerable { get; }
    IBlockingQueue<TEntry>? PublishEntriesQueue { get; set; }
    IBlockingQueue<TEntry>? SourceEntriesQueue { get; set; }
    PeriodRange? PeriodRange { get; set; }
    ReaderOptions ReaderOptions { get; set; }
    ResultFlags ResultPublishFlags { get; set; }
    EntryResultSourcing EntrySourcing { get; set; }
    int MaxResults { get; set; }
    int BatchLimit { get; set; }
    TimeSeriesPeriod SamplePeriod { get; set; }
    int MaxUnconsumedLimit { get; set; }
    int CountMatch { get; set; }
    int CountProcessed { get; set; }
    int CountBucketsVisited { get; set; }
    void FinishedConsumingEntry(TEntry entry);
    bool ProcessCandidateEntry(TEntry entry);

    void RunReader();
}

public class TimeSeriesFileReaderContext<TEntry> : IReaderContext<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    private readonly Func<TEntry> createNew;
    private readonly ITimeSeriesEntriesSession<TEntry> entriesFile;
    private Action<TEntry>? callbackAction;
    private DateTime lastSamplePeriodStart;
    private int maxUnconsumedLimit;

    private Semaphore? maxUnconsumedSemaphore;
    private TEntry? populateEntrySingleton;

    public TimeSeriesFileReaderContext(ITimeSeriesEntriesSession<TEntry> entriesFile, Func<TEntry>? createNew = null)
    {
        this.entriesFile = entriesFile;
        this.createNew = createNew ?? ReflectionHelper.DefaultCtorFunc<TEntry>();
    }

    public IMessageDeserializer? BucketDeserializer { get; set; }
    public IMessageSerializer? ResultWriter { get; set; }
    public IMessageBufferContext? ResultBuffer { get; set; }
    public IObserver<TEntry>? ResultObserver { get; set; }
    public Func<TEntry>? SourceEntryFactory { get; set; }
    public List<TEntry> ResultList { get; } = new();
    public TEntry? FirstResult { get; set; }
    public TEntry? LastResult { get; set; }

    public TEntry PopulateEntrySingleton
    {
        get => populateEntrySingleton ??= createNew();
        set => populateEntrySingleton = value;
    }

    public TimeSeriesPeriod SamplePeriod { get; set; }

    public TEntry GetNextEntryToPopulate
    {
        get
        {
            switch (EntrySourcing)
            {
                case EntryResultSourcing.ReuseSingletonObject: return PopulateEntrySingleton!;
                case EntryResultSourcing.FromFactoryFuncUnlimited: return SourceEntryFactory!();
                case EntryResultSourcing.FromFactoryFuncLimited:
                    maxUnconsumedSemaphore!.WaitOne();
                    return SourceEntryFactory!();
                case EntryResultSourcing.FromBlockingQueue: return SourceEntriesQueue!.Take();
                case EntryResultSourcing.NewEachEntryLimited:
                    maxUnconsumedSemaphore!.WaitOne();
                    return createNew();
                default: return createNew();
            }
        }
    }

    public void FinishedConsumingEntry(TEntry entry)
    {
        switch (EntrySourcing)
        {
            case EntryResultSourcing.FromFactoryFuncUnlimited:
                if (entry is IRecyclableObject recyclableEntry) recyclableEntry.DecrementRefCount();
                break;
            case EntryResultSourcing.NewEachEntryLimited:
            case EntryResultSourcing.FromFactoryFuncLimited:
                if (entry is IRecyclableObject recyclableObj) recyclableObj.DecrementRefCount();
                maxUnconsumedSemaphore!.Release();
                break;
            case EntryResultSourcing.FromBlockingQueue:
                SourceEntriesQueue!.Add(entry);
                break;
        }
    }

    public Action<TEntry>? CallbackAction
    {
        get => callbackAction;
        set
        {
            if (value != null) ResultPublishFlags |= RunCallback;
            else ResultPublishFlags = ResultPublishFlags.Unset(RunCallback);
            callbackAction = value;
        }
    }

    public void RunReader()
    {
        var processedCount = 0;
        foreach (var subscribePullResult in entriesFile.StartReaderContext(this)) processedCount++;
    }

    public int CountMatch { get; set; }
    public int CountProcessed { get; set; }
    public int CountBucketsVisited { get; set; }
    public bool ContinueSearching { get; private set; } = true;
    public IStorageTimeResolver<TEntry>? StorageTimeResolver { get; set; }

    public bool ProcessCandidateEntry(TEntry entry)
    {
        CountProcessed++;
        if (!ContinueSearching) return false;
        var rangeMatch = CheckPeriodRange(entry);
        if (!rangeMatch && CheckExceededPeriodRangeTime(entry))
        {
            ContinueSearching = false;
            return false;
        }

        if (!rangeMatch) return false;
        if (MaxResults > 0 && CountMatch >= MaxResults - 1)
        {
            ContinueSearching = false;
            return false;
        }

        CountMatch++;
        FirstResult ??= entry;
        LastResult = entry;

        var shouldIncludeThis = true;
        if (ResultPublishFlags.HasSampleResultsFlag())
        {
            var entryStorageTime = entry.StorageTime(StorageTimeResolver);
            var thisTimePeriodStart = SamplePeriod.ContainingPeriodBoundaryStart(entryStorageTime);
            shouldIncludeThis = SamplePeriod is TimeSeriesPeriod.None or TimeSeriesPeriod.ConsumerConflated
                                || thisTimePeriodStart != lastSamplePeriodStart;
            lastSamplePeriodStart = thisTimePeriodStart;
        }

        if (!ResultPublishFlags.HasCountOnlyFlag() && shouldIncludeThis)
            foreach (var publishType in Enum.GetValues(typeof(ResultFlags)).Cast<Enum>().Where(ResultPublishFlags.HasFlag))
                switch (publishType)
                {
                    case CopyToList:
                        ResultList.Add(entry);
                        break;
                    case PublishToBlockingQueue:
                        ResultList.Add(entry);
                        break;
                    case PublishOnObserver:
                        ResultObserver?.OnNext(entry);
                        break;
                    case RunCallback:
                        CallbackAction?.Invoke(entry);
                        break;
                    case WriteResultsToBuffer:
                        if (ResultBuffer != null) ResultWriter?.Serialize((IVersionedMessage)entry, ResultBuffer);
                        break;
                }

        if (ResultPublishFlags.HasNoneOf(CountOnly | AsManyAsPossible | CaptureLastResult)
            && ResultPublishFlags.HasCaptureFirstResultFlag())
            ContinueSearching = false;
        return shouldIncludeThis && ResultPublishFlags.HasAsEnumerableFlag();
    }

    public IEnumerable<TEntry> ResultEnumerable
    {
        get
        {
            FirstResult = default;
            LastResult = default;
            ResultPublishFlags |= AsEnumerable;
            ResultPublishFlags |= MaxResults > 0 ? LimitCount : None;
            CountMatch = 0;
            if (ReaderOptions is ReaderOptions.ConsumerControlled or ReaderOptions.AtEntryStorageTime)
            {
                foreach (var timeSeriesEntry in entriesFile.StartReaderContext(this)) yield return timeSeriesEntry;
            }
            else
            {
                ResultPublishFlags = ResultPublishFlags.Unset(CopyToList);
                ResultList.Clear();
                ResultList.AddRange(entriesFile.StartReaderContext(this));
                foreach (var timeSeriesEntry in ResultList) yield return timeSeriesEntry;
            }
        }
    }

    public IEnumerable<List<TEntry>> BatchedResultEnumerable
    {
        get
        {
            FirstResult = default;
            LastResult = default;
            ReaderOptions = ReaderOptions.ReadFastAsPossible;
            ResultPublishFlags = AsEnumerable;
            ResultPublishFlags = ResultPublishFlags.Unset(CopyToList);
            foreach (var timeSeriesEntry in entriesFile.StartReaderContext(this))
            {
                ResultList.Add(timeSeriesEntry);
                if (ResultList.Count >= BatchLimit)
                {
                    yield return ResultList;
                    ResultList.Clear();
                }
            }

            if (ResultList.Count > 0) yield return ResultList;
        }
    }

    public IBlockingQueue<TEntry>? PublishEntriesQueue { get; set; }
    public IBlockingQueue<TEntry>? SourceEntriesQueue { get; set; }
    public PeriodRange? PeriodRange { get; set; }
    public ReaderOptions ReaderOptions { get; set; }
    public ResultFlags ResultPublishFlags { get; set; }

    public EntryResultSourcing EntrySourcing { get; set; }

    public int MaxUnconsumedLimit
    {
        get => maxUnconsumedLimit;
        set
        {
            maxUnconsumedSemaphore = value == 0 ? null : new Semaphore(0, value);
            maxUnconsumedLimit = value;
        }
    }

    public int MaxResults { get; set; }
    public int BatchLimit { get; set; }

    protected bool CheckPeriodRange(TEntry candidateEntry)
    {
        if (PeriodRange == null) return true;
        var range = PeriodRange.Value;
        var entryStorageTime = candidateEntry.StorageTime(StorageTimeResolver);
        return (entryStorageTime < range.ToTime || (range.ToTime == null && range.FromTime != null))
               && (entryStorageTime > range.FromTime || (range.FromTime == null && range.ToTime != null));
    }

    protected bool CheckExceededPeriodRangeTime(TEntry candidateEntry)
    {
        if (PeriodRange == null) return false;
        var range = PeriodRange.Value;
        var entryStorageTime = candidateEntry.StorageTime(StorageTimeResolver);
        return entryStorageTime > range.ToTime && range.ToTime != null;
    }
}
