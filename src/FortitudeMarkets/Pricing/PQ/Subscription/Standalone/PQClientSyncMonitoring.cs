// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Reflection;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

public interface IPQClientSyncMonitoring
{
    void RegisterNewDeserializer(IPQQuoteDeserializer quoteDeserializer);
    void UnregisterSerializer(IPQQuoteDeserializer quoteDeserializer);
    void CheckStartMonitoring();
    void CheckStopMonitoring();
}

public class PQClientSyncMonitoring : IPQClientSyncMonitoring
{
    private const int TasksFrequencyMs = 1000;
    private const int MaxSnapshotBatch = 100;

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly Func<string, IMarketConnectionConfig?> getSourceServerConfig;

    private readonly IOSParallelController osParallelController;

    private readonly ISequencer pqSeq = new Sequencer();

    private readonly Action<INetworkTopicConnectionConfig, List<ISourceTickerInfo>> snapShotRequestAction;

    private readonly IIntraOSThreadSignal stopSignal;

    private readonly IDoublyLinkedList<IPQQuoteDeserializer> syncKo = new DoublyLinkedList<IPQQuoteDeserializer>();
    private readonly IDoublyLinkedList<IPQQuoteDeserializer> syncOk = new DoublyLinkedList<IPQQuoteDeserializer>();

    private volatile bool tasksActive;

    private IOSThread? tasksThread;

    public PQClientSyncMonitoring
    (Func<string, IMarketConnectionConfig?> getSourceServerConfig,
        Action<INetworkTopicConnectionConfig, List<ISourceTickerInfo>> snapShotRequestAction)
    {
        osParallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
        stopSignal           = osParallelController.SingleOSThreadActivateSignal(false);

        this.getSourceServerConfig = getSourceServerConfig;
        this.snapShotRequestAction = snapShotRequestAction;
    }

    public void RegisterNewDeserializer(IPQQuoteDeserializer quoteDeserializer)
    {
        quoteDeserializer.SyncOk         += OnInSync;
        quoteDeserializer.ReceivedUpdate += OnReceivedUpdate;
        quoteDeserializer.OutOfSync      += OnOutOfSync;

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

    public void UnregisterSerializer(IPQQuoteDeserializer quoteDeserializer)
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

    private void OnReceivedUpdate(IPQQuoteDeserializer quoteDeserializer)
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

    private void OnInSync(IPQQuoteDeserializer quoteDeserializer)
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

    private void OnOutOfSync(IPQQuoteDeserializer quoteDeserializer)
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
                IPQQuoteDeserializer? pu;
                if ((pu = syncOk.Head) == null || !pu.HasTimedOutAndNeedsSnapshot(TimeContext.UtcNow)) break;
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
        IPQQuoteDeserializer? firstToResync                  = null;
        var                   deserializersInNeedOfSnapshots = new Dictionary<string, List<ISourceTickerInfo>>();
        for (var count = 0; tasksActive && count < MaxSnapshotBatch; count++)
        {
            IPQQuoteDeserializer? pu;

            bool resync;
            var  seq = pqSeq.Claim();
            try
            {
                pqSeq.Serialize(seq);
                if ((pu = syncKo.Head) == null) break;
                if (firstToResync == null)
                    firstToResync = pu;
                else if (firstToResync == pu) break;
                resync = pu.CheckResync(TimeContext.UtcNow);
                syncKo.Remove(pu);
                syncKo.AddLast(pu);
            }
            finally
            {
                pqSeq.Release(seq);
            }

            if (!resync) continue;
            if (!deserializersInNeedOfSnapshots.TryGetValue(pu.Identifier.SourceName,
                                                            out var pqQuoteDeserializerList))
                deserializersInNeedOfSnapshots[pu.Identifier.SourceName] =
                    pqQuoteDeserializerList = new List<ISourceTickerInfo>();
            if (!pqQuoteDeserializerList.Contains(pu.Identifier)) pqQuoteDeserializerList.Add(pu.Identifier);
        }

        RequestSnapshotsForTickers(deserializersInNeedOfSnapshots);
    }

    private void RequestSnapshotsForTickers
    (
        Dictionary<string, List<ISourceTickerInfo>> deserializersInNeedOfSnapshots)
    {
        foreach (var kv in deserializersInNeedOfSnapshots)
        {
            var feedRef = getSourceServerConfig(kv.Key);
            if (feedRef != null) snapShotRequestAction(feedRef.PricingServerConfig!.SnapshotConnectionConfig!, kv.Value);
        }
    }
}
