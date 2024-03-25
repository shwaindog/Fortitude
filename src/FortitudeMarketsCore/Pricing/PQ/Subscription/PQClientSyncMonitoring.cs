#region

using System.Reflection;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public class PQClientSyncMonitoring : IPQClientSyncMonitoring
{
    private const int TasksFrequencyMs = 1000;
    private const int MaxSnapshotBatch = 100;

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly Func<string, ISnapshotUpdatePricingServerConfig?> getSourceServerConfig;
    private readonly IOSParallelController osParallelController;

    private readonly ISequencer pqSeq = new Sequencer();
    private readonly Action<ISocketTopicConnectionConfig, List<IUniqueSourceTickerIdentifier>> snapShotRequestAction;
    private readonly IIntraOSThreadSignal stopSignal;
    private readonly IDoublyLinkedList<IPQDeserializer> syncKo = new DoublyLinkedList<IPQDeserializer>();

    private readonly IDoublyLinkedList<IPQDeserializer> syncOk = new DoublyLinkedList<IPQDeserializer>();
    private volatile bool tasksActive;
    private IOSThread? tasksThread;

    public PQClientSyncMonitoring(Func<string, ISnapshotUpdatePricingServerConfig?> getSourceServerConfig,
        Action<ISocketTopicConnectionConfig, List<IUniqueSourceTickerIdentifier>> snapShotRequestAction)
    {
        osParallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
        stopSignal = osParallelController.SingleOSThreadActivateSignal(false);
        this.getSourceServerConfig = getSourceServerConfig;
        this.snapShotRequestAction = snapShotRequestAction;
    }

    public void RegisterNewDeserializer(IPQDeserializer quoteDeserializer)
    {
        quoteDeserializer.SyncOk += OnInSync;
        quoteDeserializer.ReceivedUpdate += OnReceivedUpdate;
        quoteDeserializer.OutOfSync += OnOutOfSync;

        var seq = pqSeq.Claim();
        try
        {
            pqSeq.Serialize(seq);
            syncKo.AddFirst(quoteDeserializer);
        }
        finally
        {
            pqSeq.Release(seq);
        }
    }

    public void UnregisterSerializer(IPQDeserializer quoteDeserializer)
    {
        if (syncOk.SafeContains(quoteDeserializer))
        {
            var seq = pqSeq.Claim();
            try
            {
                pqSeq.Serialize(seq);
                syncOk.Remove(quoteDeserializer);
            }
            finally
            {
                pqSeq.Release(seq);
            }
        }

        if (syncKo.SafeContains(quoteDeserializer))
        {
            var seq = pqSeq.Claim();
            try
            {
                pqSeq.Serialize(seq);
                syncKo.Remove(quoteDeserializer);
            }
            finally
            {
                pqSeq.Release(seq);
            }
        }
    }

    public void CheckStartMonitoring()
    {
        if (!tasksActive)
        {
            tasksActive = true;
            tasksThread = osParallelController.CreateNewOSThread(MonitorDeserializersForSnapshotResync);
            tasksThread.IsBackground = true;
            tasksThread.Start();
        }
    }

    public void CheckStopMonitoring()
    {
        if (tasksActive)
        {
            tasksActive = false;
            stopSignal.Set();
            tasksThread?.Join();
        }
    }

    private void OnReceivedUpdate(IPQDeserializer quoteDeserializer)
    {
        var seq = pqSeq.Claim();
        try
        {
            pqSeq.Serialize(seq);
            syncOk.Remove(quoteDeserializer);
            syncOk.AddLast(quoteDeserializer);
        }
        finally
        {
            pqSeq.Release(seq);
        }
    }

    private void OnInSync(IPQDeserializer quoteDeserializer)
    {
        var seq = pqSeq.Claim();
        try
        {
            pqSeq.Serialize(seq);
            syncKo.Remove(quoteDeserializer);
            syncOk.AddLast(quoteDeserializer);
        }
        finally
        {
            pqSeq.Release(seq);
        }
    }

    private void OnOutOfSync(IPQDeserializer quoteDeserializer)
    {
        var seq = pqSeq.Claim();
        try
        {
            pqSeq.Serialize(seq);
            syncOk.Remove(quoteDeserializer);
            syncKo.AddFirst(quoteDeserializer);
        }
        finally
        {
            pqSeq.Release(seq);
        }
    }

    private void MonitorDeserializersForSnapshotResync()
    {
        var lastRun = TimeContext.UtcNow;
        while (tasksActive)
            try
            {
                var elapsedMs = (int)(TimeContext.UtcNow - lastRun).TotalMilliseconds;
                if (elapsedMs < TasksFrequencyMs)
                    stopSignal.WaitOne(TasksFrequencyMs - elapsedMs);
                else
                    Logger.Warn("Tasks scheduler slow time in Ms:{0}", elapsedMs);
                lastRun = TimeContext.UtcNow;

                MoveAllTimedoutTickersToKo();

                FindAndSnapshotKnockedOutTickersReadyForSnapshot();
            }
            catch (Exception ex)
            {
                Logger.Error("Unexpected error in task scheduler: {0}", ex);
            }
    }

    private void MoveAllTimedoutTickersToKo()
    {
        while (tasksActive)
        {
            var seq = pqSeq.Claim();
            try
            {
                pqSeq.Serialize(seq);
                IPQDeserializer? pu;
                if ((pu = syncOk.Head) == null || !pu.HasTimedOutAndNeedsSnapshot(TimeContext.UtcNow))
                    break;
                syncOk.Remove(pu);
                syncKo.AddFirst(pu);
            }
            finally
            {
                pqSeq.Release(seq);
            }
        }
    }

    private void FindAndSnapshotKnockedOutTickersReadyForSnapshot()
    {
        IPQDeserializer? firstToResync = null;
        var deserializersInNeedOfSnapshots = new Dictionary<string, List<IUniqueSourceTickerIdentifier>>();
        for (var count = 0; tasksActive && count < MaxSnapshotBatch; count++)
        {
            IPQDeserializer? pu;
            bool resync;
            var seq = pqSeq.Claim();
            try
            {
                pqSeq.Serialize(seq);
                if ((pu = syncKo.Head) == null)
                    break;
                if (firstToResync == null)
                    firstToResync = pu;
                else if (firstToResync == pu)
                    break;
                resync = pu.CheckResync(TimeContext.UtcNow);
                syncKo.Remove(pu);
                syncKo.AddLast(pu);
            }
            finally
            {
                pqSeq.Release(seq);
            }

            if (!resync) continue;
            if (!deserializersInNeedOfSnapshots.TryGetValue(pu.Identifier.Source,
                    out var pqQuoteDeserializerList))
                deserializersInNeedOfSnapshots[pu.Identifier.Source] =
                    pqQuoteDeserializerList = new List<IUniqueSourceTickerIdentifier>();
            if (!pqQuoteDeserializerList.Contains(pu.Identifier))
                pqQuoteDeserializerList.Add(pu.Identifier);
        }

        RequestSnapshotsForTickers(deserializersInNeedOfSnapshots);
    }

    private void RequestSnapshotsForTickers(
        Dictionary<string, List<IUniqueSourceTickerIdentifier>> deserializersInNeedOfSnapshots)
    {
        foreach (var kv in deserializersInNeedOfSnapshots)
        {
            var feedRef = getSourceServerConfig(kv.Key);
            if (feedRef != null)
                snapShotRequestAction(feedRef.SnapshotConnectionConfig!, kv.Value);
        }
    }
}
