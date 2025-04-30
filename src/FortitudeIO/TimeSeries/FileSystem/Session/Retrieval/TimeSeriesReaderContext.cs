// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using static FortitudeIO.TimeSeries.FileSystem.Session.Retrieval.ResultFlags;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;

[Flags]
public enum ReaderOptions
{
    None                      = 0
  , ReadFastAsPossible        = 1
  , AtEntryStorageTime        = 2
  , ConsumerControlled        = 4
  , ReverseChronologicalOrder = 8
}

public enum EntryResultSourcing
{
    FromRecycler = 0
  , ReuseSingletonObject
  , FromFactoryFuncUnlimited
  , FromFactoryFuncLimited
  , FromBlockingQueue
  , NewEachEntryUnlimited
  , NewEachEntryLimited
}

public interface IReaderContext<TEntry> : IDisposable
{
    IStorageTimeResolver<TEntry>?     StorageTimeResolver     { get; set; }
    IEnumerable<ReusableList<TEntry>> BatchedResultEnumerable { get; }

    IMessageDeserializer?   BucketDeserializer  { get; set; }
    IMessageSerializer?     ResultWriter        { get; set; }
    IMessageBufferContext?  ResultBuffer        { get; set; }
    IBlockingQueue<TEntry>? PublishEntriesQueue { get; set; }
    IBlockingQueue<TEntry>? SourceEntriesQueue  { get; set; }
    IEnumerable<TEntry>     ResultEnumerable    { get; }

    UnboundedTimeRange? PeriodRange        { get; set; }
    EntryResultSourcing EntrySourcing      { get; set; }
    IObserver<TEntry>?  ResultObserver     { get; set; }
    ReaderOptions       ReaderOptions      { get; set; }
    ResultFlags         ResultPublishFlags { get; set; }
    TimeBoundaryPeriod  SamplePeriod       { get; set; }
    Func<TEntry>?       SourceEntryFactory { get; set; }
    List<TEntry>        ResultList         { get; }
    Action<TEntry>?     CallbackAction     { get; set; }

    TEntry  PopulateEntrySingleton { get; set; }
    TEntry? FirstResult            { get; set; }
    TEntry? LastResult             { get; set; }
    TEntry  GetNextEntryToPopulate { get; }

    bool ContinueSearching   { get; }
    int  MaxResults          { get; set; }
    int  BatchLimit          { get; set; }
    int  MaxUnconsumedLimit  { get; set; }
    int  CountMatch          { get; set; }
    int  CountProcessed      { get; set; }
    int  CountBucketsVisited { get; set; }

    bool IsReverseChronologicalOrder { get; }

    bool CheckExceededPeriodRangeTime(TEntry candidateEntry);

    void ClearReadReverse();
    void ReadReverseAddToStart(TEntry entry);
    void FinishedConsumingEntry(TEntry entry);
    bool ProcessCandidateEntry(TEntry entry);
    void CloseReaderSession();
    void RunReader();

    IEnumerable<TEntry> ReadReverse();

    IFlowRateEnumerable<TEntry>               FlowRateEnumerable(FlowRate flowRate);
    IFlowRateEnumerable<ReusableList<TEntry>> BatchedFlowRateEnumerable(BatchRate batchRate);
}

public class TimeSeriesReaderContext<TEntry> : RecyclableObject, IReaderContext<TEntry> where TEntry : ITimeSeriesEntry
{
    private Action<TEntry>? callbackAction;

    private Type?        concreteType;
    private Func<TEntry> createNew = null!;

    private DateTime   lastSamplePeriodStart;
    private int        maxUnconsumedLimit;
    private Semaphore? maxUnconsumedSemaphore;
    private TEntry?    populateEntrySingleton;

    private IReaderSession<TEntry> readerSession = null!;

    private IDoublyLinkedList<DoublyLinkedListWrapperNode<TEntry>>? readReverseBucket;

    private IRecycler resultsRecycler = null!;

    public TimeSeriesReaderContext() => ReaderOptions = ReaderOptions.None;

    public TimeSeriesReaderContext
    (IReaderSession<TEntry> readerSession, IRecycler resultsRecycler,
        EntryResultSourcing defaultEntryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        ReaderOptions readerOptions = ReaderOptions.ConsumerControlled,
        Func<TEntry>? createNew = null)
    {
        Configure(readerSession, resultsRecycler, defaultEntryResultSourcing, readerOptions, createNew);
    }

    public void Dispose()
    {
        CloseReaderSession();
    }

    public void CloseReaderSession()
    {
        if (readerSession.IsOpen) readerSession.Dispose();
    }

    public IMessageDeserializer?  BucketDeserializer { get; set; }
    public IMessageSerializer?    ResultWriter       { get; set; }
    public IMessageBufferContext? ResultBuffer       { get; set; }
    public IObserver<TEntry>?     ResultObserver     { get; set; }

    public Func<TEntry>? SourceEntryFactory { get; set; }
    public List<TEntry>  ResultList         { get; } = new();

    public TEntry? FirstResult { get; set; }
    public TEntry? LastResult  { get; set; }

    public TEntry PopulateEntrySingleton
    {
        get => populateEntrySingleton ??= createNew();
        set => populateEntrySingleton = value;
    }

    public TimeBoundaryPeriod SamplePeriod { get; set; }

    public TEntry GetNextEntryToPopulate
    {
        get
        {
            switch (EntrySourcing)
            {
                case EntryResultSourcing.FromRecycler:             return (TEntry)resultsRecycler.Borrow(concreteType ?? typeof(TEntry));
                case EntryResultSourcing.ReuseSingletonObject:     return PopulateEntrySingleton!;
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
            case EntryResultSourcing.FromBlockingQueue: SourceEntriesQueue!.Add(entry); break;
        }
    }

    public Action<TEntry>? CallbackAction
    {
        get => callbackAction;
        set
        {
            if (value != null)
                ResultPublishFlags |= RunCallback;
            else
                ResultPublishFlags = ResultPublishFlags.Unset(RunCallback);
            callbackAction = value;
        }
    }

    public void RunReader()
    {
        var processedCount = 0;
        foreach (var subscribePullResult in readerSession.StartReaderContext(this)) processedCount++;
    }

    public int CountMatch          { get; set; }
    public int CountProcessed      { get; set; }
    public int CountBucketsVisited { get; set; }

    public bool ContinueSearching { get; private set; } = true;

    public bool IsReverseChronologicalOrder => (ReaderOptions & ReaderOptions.ReverseChronologicalOrder) > 0;

    public void ClearReadReverse()
    {
        var currentWrapped = readReverseBucket?.Head;
        while (currentWrapped != null)
        {
            var next = currentWrapped.Next;
            readReverseBucket!.Remove(currentWrapped);
            currentWrapped.DecrementRefCount(); // stateReset will check if payload is recyclable and decrement
            currentWrapped = next;
        }
        readReverseBucket?.DecrementRefCount();
    }

    public void ReadReverseAddToStart(TEntry entry)
    {
        readReverseBucket ??= resultsRecycler.Borrow<DoublyLinkedList<DoublyLinkedListWrapperNode<TEntry>>>();
        var wrapperNode = resultsRecycler.Borrow<DoublyLinkedListWrapperNode<TEntry>>();
        wrapperNode.Configure(entry);
        readReverseBucket.AddFirst(wrapperNode);
    }

    public IEnumerable<TEntry> ReadReverse()
    {
        var currentWrapped = readReverseBucket?.Head;
        while (currentWrapped != null)
        {
            yield return currentWrapped.Payload;

            currentWrapped = currentWrapped.Next;
        }
    }

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
        LastResult  =   entry;

        var shouldIncludeThis = true;
        if (ResultPublishFlags.HasSampleResultsFlag())
        {
            var entryStorageTime    = entry.StorageTime(StorageTimeResolver);
            var thisTimePeriodStart = SamplePeriod.ContainingPeriodBoundaryStart(entryStorageTime);
            shouldIncludeThis = SamplePeriod is TimeBoundaryPeriod.Tick
                             || thisTimePeriodStart != lastSamplePeriodStart;
            lastSamplePeriodStart = thisTimePeriodStart;
        }

        if (!ResultPublishFlags.HasCountOnlyFlag() && shouldIncludeThis)
            foreach (var publishType in Enum.GetValues(typeof(ResultFlags)).Cast<Enum>().Where(ResultPublishFlags.HasFlag))
                switch (publishType)
                {
                    case CopyToList:             ResultList.Add(entry); break;
                    case PublishToBlockingQueue: ResultList.Add(entry); break;
                    case PublishOnObserver:      ResultObserver?.OnNext(entry); break;
                    case RunCallback:            CallbackAction?.Invoke(entry); break;
                    case WriteResultsToBuffer:
                        if (ResultBuffer != null) ResultWriter?.Serialize((IVersionedMessage)entry, ResultBuffer);
                        break;
                }

        if (ResultPublishFlags.HasNoneOf(CountOnly | AsManyAsPossible | CaptureLastResult)
         && ResultPublishFlags.HasCaptureFirstResultFlag())
            ContinueSearching = false;
        return shouldIncludeThis && ResultPublishFlags.HasAsEnumerableFlag();
    }

    public IFlowRateEnumerable<TEntry> FlowRateEnumerable(FlowRate flowRate) => new FlowRateEnumerableWrapper<TEntry>(flowRate, ResultEnumerable);

    public IEnumerable<TEntry> ResultEnumerable
    {
        get
        {
            FirstResult = default;
            LastResult  = default;

            ResultPublishFlags |= AsEnumerable;
            ResultPublishFlags |= MaxResults > 0 ? LimitCount : None;

            CountMatch = 0;

            if (ReaderOptions is ReaderOptions.ConsumerControlled or ReaderOptions.AtEntryStorageTime)
            {
                foreach (var timeSeriesEntry in readerSession.StartReaderContext(this)) yield return timeSeriesEntry;
            }
            else
            {
                ResultPublishFlags = ResultPublishFlags.Unset(CopyToList);
                ResultList.Clear();
                ResultList.AddRange(readerSession.StartReaderContext(this));
                foreach (var timeSeriesEntry in ResultList) yield return timeSeriesEntry;
            }
        }
    }

    public IFlowRateEnumerable<ReusableList<TEntry>> BatchedFlowRateEnumerable(BatchRate batchRate)
    {
        BatchLimit = batchRate.NumberInBatch;
        return new BatchFlowRateEnumerableWrapper<ReusableList<TEntry>, TEntry>(new FlowRate(batchRate), BatchedResultEnumerable);
    }

    public IEnumerable<ReusableList<TEntry>> BatchedResultEnumerable
    {
        get
        {
            FirstResult = default;
            LastResult  = default;

            ReaderOptions      = ReaderOptions.ReadFastAsPossible;
            ResultPublishFlags = AsEnumerable;
            ResultPublishFlags = ResultPublishFlags.Unset(CopyToList);

            var reusableList = resultsRecycler.Borrow<ReusableList<TEntry>>();
            foreach (var timeSeriesEntry in readerSession.StartReaderContext(this))
            {
                reusableList.Add(timeSeriesEntry);
                if (reusableList.Count >= BatchLimit)
                {
                    yield return reusableList;
                    reusableList = resultsRecycler.Borrow<ReusableList<TEntry>>();
                }
            }

            if (ResultList.Count > 0) yield return reusableList;
        }
    }

    public IBlockingQueue<TEntry>? PublishEntriesQueue { get; set; }
    public IBlockingQueue<TEntry>? SourceEntriesQueue  { get; set; }

    public UnboundedTimeRange? PeriodRange        { get; set; }
    public ReaderOptions       ReaderOptions      { get; set; }
    public ResultFlags         ResultPublishFlags { get; set; }

    public EntryResultSourcing EntrySourcing { get; set; }

    public int MaxUnconsumedLimit
    {
        get => maxUnconsumedLimit;
        set
        {
            maxUnconsumedSemaphore = value == 0 ? null : new Semaphore(0, value);
            maxUnconsumedLimit     = value;
        }
    }

    public int MaxResults { get; set; }
    public int BatchLimit { get; set; }

    public bool CheckExceededPeriodRangeTime(TEntry candidateEntry)
    {
        if (PeriodRange == null) return false;
        var range            = PeriodRange.Value;
        var entryStorageTime = candidateEntry.StorageTime(StorageTimeResolver);
        return entryStorageTime > range.ToTime && range.ToTime != null;
    }

    public void Configure
    (IReaderSession<TEntry> readSession, IRecycler resultRecycler,
        EntryResultSourcing defaultEntryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        ReaderOptions readerOptions = ReaderOptions.ConsumerControlled,
        Func<TEntry>? createNewFunc = null)
    {
        readerSession   = readSession;
        resultsRecycler = resultRecycler;
        ReaderOptions   = readerOptions;
        if (!IsReverseChronologicalOrder)
            EntrySourcing = defaultEntryResultSourcing;
        else if (defaultEntryResultSourcing == EntryResultSourcing.ReuseSingletonObject)
            EntrySourcing = createNewFunc == null ? EntryResultSourcing.FromRecycler : EntryResultSourcing.FromFactoryFuncUnlimited;
        createNew = SourceEntryFactory = createNewFunc ?? ReflectionHelper.DefaultCtorFunc<TEntry>();
    }

    public void Configure<TConcreteEntry>
    (IReaderSession<TEntry> readSession, IRecycler resultRecycler,
        EntryResultSourcing defaultEntryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        ReaderOptions readerOptions = ReaderOptions.ConsumerControlled) where TConcreteEntry : class, TEntry, ITimeSeriesEntry, new()
    {
        concreteType    = typeof(TConcreteEntry);
        readerSession   = readSession;
        resultsRecycler = resultRecycler;
        ReaderOptions   = readerOptions;
        if (!IsReverseChronologicalOrder)
            EntrySourcing                                                                              = defaultEntryResultSourcing;
        else if (defaultEntryResultSourcing == EntryResultSourcing.ReuseSingletonObject) EntrySourcing = EntryResultSourcing.FromRecycler;
        createNew = SourceEntryFactory = resultRecycler.Borrow<TConcreteEntry>;
    }

    public override void StateReset()
    {
        ClearReadReverse();
        maxUnconsumedSemaphore = null!;

        concreteType    = null!;
        createNew       = null!;
        readerSession   = null!;
        resultsRecycler = null!;
        callbackAction  = null;
        ReaderOptions   = ReaderOptions.None;
    }

    protected bool CheckPeriodRange(TEntry candidateEntry)
    {
        if (PeriodRange == null) return true;
        var range            = PeriodRange.Value;
        var entryStorageTime = candidateEntry.StorageTime(StorageTimeResolver);
        return (entryStorageTime < range.ToTime || (range.ToTime == null && range.FromTime != null))
            && (entryStorageTime > range.FromTime || (range.FromTime == null && range.ToTime != null));
    }
}
