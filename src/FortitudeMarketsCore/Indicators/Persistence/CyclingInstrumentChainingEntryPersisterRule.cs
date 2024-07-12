// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Persistence;

public struct CyclingInstrumentChainingEntryPersisterParams
{
    public CyclingInstrumentChainingEntryPersisterParams
        (TimeSeriesRepositoryParams repositoryParams, string appendListenAddress)
    {
        RepositoryParams    = repositoryParams;
        AppendListenAddress = appendListenAddress;
    }

    public TimeSeriesRepositoryParams RepositoryParams { get; }

    public string AppendListenAddress   { get; set; }
    public bool   RunInstrumentsPersist { get; set; } = true;

    public int  RunMaxInstrumentPersist { get; set; } = 1_000;
    public int  RunMaxTotalPersist      { get; set; } = 10_000;
    public bool AutoAdjustRatioDown     { get; set; } = true;
    public bool AutoAdjustRatioUp       { get; set; } = false;
    public int  AutoAdjustTargetRunMs   { get; set; } = 2_000;
}

public readonly struct ChainableInstrumentPayload<TEntry>(PricingInstrumentId instrumentId, TEntry entry)
    where TEntry : class, ITimeSeriesEntry<TEntry>, IDoublyLinkedListNode<TEntry>, new()
{
    public PricingInstrumentId PricingInstrumentId { get; } = instrumentId;

    public TEntry Entry { get; } = entry;
}

public class InstrumentPersistenceState<TEntry>(InstrumentFileInfo instrumentFileInfo, PricingInstrumentId pricingInstrumentId)
    where TEntry : class, ITimeSeriesEntry<TEntry>, IDoublyLinkedListNode<TEntry>, new()
{
    public double   AverageBatchSize;
    public DateTime BackOffNextAttempt  = DateTime.MinValue;
    public TimeSpan BackOffNextTimeSpan = TimeSpan.FromSeconds(2);
    public int      ConsecutiveFailuresCount;

    public InstrumentFileInfo InstrumentFileInfo = instrumentFileInfo;

    public List<string>?       LastFailures;
    public DateTime            LastPersistTime     = DateTime.MinValue;
    public DateTime            LastQueueTime       = DateTime.MinValue;
    public PricingInstrumentId PricingInstrumentId = pricingInstrumentId;

    public IDoublyLinkedList<TEntry> Queue = new DoublyLinkedList<TEntry>();
    public int                       SessionPersistCounter;
    public IWriterSession<TEntry>?   WriterSession;

    public int Add(TEntry toAdd)
    {
        Queue.AddLast(toAdd);
        LastQueueTime = DateTime.UtcNow;
        return Queue.Count;
    }
}

public class CyclingInstrumentChainingEntryPersisterRule<TEntry> : TimeSeriesRepositoryAccessRule
    where TEntry : class, ITimeSeriesEntry<TEntry>, IDoublyLinkedListNode<TEntry>, new()
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(CyclingInstrumentChainingEntryPersisterRule<>));

    private readonly List<PricingInstrumentId> entriesToPersist  = new();
    private readonly List<PricingInstrumentId> forNextRunPersist = new();

    private readonly Dictionary<PricingInstrumentId, InstrumentPersistenceState<TEntry>> persistBacklog = new();

    private readonly CyclingInstrumentChainingEntryPersisterParams persisterParams;

    private ISubscription? appendEntrySubscription;

    private ISubscription? fullDrainRequestSubscription;

    private DateTime? lastAppendTime;
    private long      lastTotalPersistedCount;

    private ITimerUpdate? monitorTimerUpdate;

    private int    noPersistCount;
    private double persistRatio = 1.0;

    private ITimerUpdate? persistTimerUpdate;

    private DateTime? previousLastAppendTime;

    private bool shouldRunPersist;
    private long totalPersistedCount;

    public CyclingInstrumentChainingEntryPersisterRule(CyclingInstrumentChainingEntryPersisterParams persisterParams)
        : base(persisterParams.RepositoryParams
             , $"{nameof(CyclingInstrumentChainingEntryPersisterRule<TEntry>)}_{typeof(TEntry).Name}")
    {
        this.persisterParams = persisterParams;
        shouldRunPersist     = persisterParams.RunInstrumentsPersist;
    }

    public static string FullDrainRequestAddress => IndicatorPersisterConstants.FullDrainRequest<TEntry>();

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();

        fullDrainRequestSubscription = await this.RegisterRequestListenerAsync<string, int>(FullDrainRequestAddress, FullDrainRequestHandler);

        monitorTimerUpdate = Timer.RunEvery(20_000, MonitorPersist);
        persistTimerUpdate = Timer.RunEvery(persisterParams.AutoAdjustTargetRunMs * 2, RunPersist);

        appendEntrySubscription
            = await this.RegisterListenerAsync<ChainableInstrumentPayload<TEntry>>(persisterParams.AppendListenAddress, ReceiveEntryToPersist);
    }

    private int FullDrainRequestHandler(IBusRespondingMessage<string, int> fullDrainRequestMsg)
    {
        var requester = fullDrainRequestMsg.Payload.Body();
        Logger.Info(" {0} requested full drain of CyclingInstrumentChainingEntryPersisterRule<{1}>", requester, typeof(TEntry).Name);
        return Persist(true);
    }

    public override async ValueTask StopAsync()
    {
        await appendEntrySubscription.NullSafeUnsubscribe();
        await fullDrainRequestSubscription.NullSafeUnsubscribe();
        monitorTimerUpdate?.Cancel();
        persistTimerUpdate?.Cancel();
        Persist(true);
        await base.StopAsync();
    }

    private void MonitorPersist()
    {
        if (lastAppendTime == previousLastAppendTime || totalPersistedCount == lastTotalPersistedCount)
            noPersistCount++;
        else
            noPersistCount = 0;
        if (noPersistCount % 15 == 0) Logger.Warn("No entries have been persisted since '{0}'", lastAppendTime?.ToString() ?? "start");
        previousLastAppendTime  = lastAppendTime;
        lastTotalPersistedCount = totalPersistedCount;
    }

    private void ReceiveEntryToPersist(IBusMessage<ChainableInstrumentPayload<TEntry>> appendEntryMsg)
    {
        var instrumentPayload = appendEntryMsg.Payload.Body();
        var pricingId         = instrumentPayload.PricingInstrumentId;
        if (!persistBacklog.TryGetValue(pricingId, out var state))
        {
            var existingInstruments =
                InstrumentFileInfos(pricingId.Ticker, pricingId.Source, pricingId.InstrumentType, pricingId.EntryPeriod)
                    .ToList();
            InstrumentFileInfo instrumentFileInfo = default;
            if (existingInstruments.Any())
            {
                if (existingInstruments.Count == 1)
                    instrumentFileInfo = existingInstruments[0];
                else
                    throw new Exception
                        ($"More than one instrument exists for {pricingId.Ticker}, {pricingId.Source}, {pricingId.InstrumentType}, {pricingId.EntryPeriod}");
            }
            else
            {
                var instrument = new PricingInstrument(pricingId);
                var fileInfo   = TimeSeriesRepository.GetInstrumentFileInfo(instrument);

                if (fileInfo.FilePeriod > TimeSeriesPeriod.None) instrumentFileInfo = new InstrumentFileInfo(instrument, fileInfo.FilePeriod);
            }
            if (Equals(instrumentFileInfo, default(InstrumentFileInfo)))
                throw new
                    Exception($"Could not locate a repository structure for {pricingId.Ticker}, {pricingId.Source}, {pricingId.InstrumentType}, {pricingId.EntryPeriod}");
            state = new InstrumentPersistenceState<TEntry>(instrumentFileInfo, pricingId);
            persistBacklog.Add(pricingId, state);
        }
        var instrumentQueueLength = state.Add(instrumentPayload.Entry);
        if (!entriesToPersist.Contains(pricingId)) entriesToPersist.Add(pricingId);
        if (instrumentQueueLength > 0 && instrumentQueueLength % 1000 == 0)
            Logger.Warn("Persister queue length for {0} for entry type {1} is {2}", pricingId.GetReferenceShortName(), typeof(TEntry).Name
                      , instrumentQueueLength);
    }

    private void RunPersist()
    {
        Persist(false);
    }

    private int Persist(bool fullDrain)
    {
        if (!shouldRunPersist) return 0;
        var startTime = DateTime.Now;
        forNextRunPersist.Clear();
        var runMax                 = (int)(persisterParams.RunMaxTotalPersist * persistRatio);
        var perInstrumentToPersist = (int)(persisterParams.RunMaxInstrumentPersist * persistRatio);
        var runCurrent             = 0;
        for (; (fullDrain || runCurrent < runMax) && entriesToPersist.Count > 0; runCurrent++)
        {
            var pricingId    = entriesToPersist[0];
            var persistState = persistBacklog[pricingId];
            if (persistState.BackOffNextAttempt > DateTime.UtcNow)
            {
                forNextRunPersist.Add(pricingId);
                entriesToPersist.RemoveAt(0);
                continue;
            }
            var queue   = persistState.Queue;
            var entries = queue.Count;

            persistState.AverageBatchSize = (persistState.AverageBatchSize * 19 + entries) / 20;

            if (entries > 0)
            {
                var writerSession = persistState.WriterSession ?? (persistState.WriterSession
                    = TimeSeriesRepository.GetWriterSession<TEntry>(persistState.InstrumentFileInfo.Instrument));
                var j = 0;
                try
                {
                    if (!writerSession!.IsOpen) writerSession.Reopen();
                    var currentEntry = queue.Head;
                    for (; currentEntry != null && j < entries && j < perInstrumentToPersist; j++)
                    {
                        writerSession.AppendEntry(currentEntry);

                        persistState.SessionPersistCounter++;
                        totalPersistedCount++;
                        var removed = queue.Remove(currentEntry);
                        if (removed is IRecyclableObject recyclable) recyclable.DecrementRefCount();
                        currentEntry = queue.Head;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn("When trying to write to repository persister {0} for entry type {1} got {2}"
                              , pricingId.GetReferenceShortName(), typeof(TEntry).Name, ex);
                    persistState.ConsecutiveFailuresCount++;
                    persistState.LastFailures ??= new List<string>();
                    persistState.LastFailures.Add(ex.Message);
                    persistState.BackOffNextTimeSpan += persistState.BackOffNextTimeSpan;
                    persistState.BackOffNextAttempt  =  DateTime.UtcNow + persistState.BackOffNextTimeSpan;
                }
                finally
                {
                    writerSession?.Close();
                }
                lastAppendTime = persistState.LastPersistTime = DateTime.UtcNow;
                if (j < entries) forNextRunPersist.Add(pricingId);
                entriesToPersist.RemoveAt(0);
            }
        }
        entriesToPersist.AddRange(forNextRunPersist);
        var endTime = DateTime.Now;
        var totalMs = Math.Max(1, (int)(endTime - startTime).TotalMilliseconds);
        if (persisterParams.AutoAdjustRatioDown && totalMs > persisterParams.AutoAdjustTargetRunMs)
            persistRatio = (persistRatio * 19 + Math.Max(0.1, persisterParams.AutoAdjustTargetRunMs / totalMs)) / 20;
        if (persisterParams.AutoAdjustRatioUp && totalMs < persisterParams.AutoAdjustTargetRunMs)
            persistRatio = (persistRatio * 19 + Math.Min(100, persisterParams.AutoAdjustTargetRunMs / totalMs)) / 20;
        return runCurrent;
    }
}
