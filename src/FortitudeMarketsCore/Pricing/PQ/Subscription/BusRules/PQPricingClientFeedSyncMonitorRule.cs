#region

using FortitudeBusRules.BusMessaging.Pipelines.Execution;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientFeedSyncMonitorRule : Rule
{
    private const int TasksFrequencyMs = 1000;
    private const int MaxSnapshotBatch = 100;
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientFeedSyncMonitorRule));

    private readonly string feedName;

    private readonly string feedRequestSnapshotsAddress;
    private readonly Dictionary<uint, DeserializerEventRegistrations> registeredDeserializers = new();
    private readonly IMessageDeserializationRepository sharedFeedDeserializationRepo;

    private readonly IDoublyLinkedList<IPQDeserializer> syncKo = new DoublyLinkedList<IPQDeserializer>();
    private readonly IDoublyLinkedList<IPQDeserializer> syncOk = new DoublyLinkedList<IPQDeserializer>();

    private DateTime lastReceivedTickTime = DateTimeConstants.UnixEpoch;

    public PQPricingClientFeedSyncMonitorRule(string feedName, IMessageDeserializationRepository sharedFeedDeserializationRepo)
        : base("PQClientFeedSyncMonitor" + feedName)
    {
        this.feedName = feedName;
        this.sharedFeedDeserializationRepo = sharedFeedDeserializationRepo;
        feedRequestSnapshotsAddress = feedName.FeedTickersSnapshotRequestAddress();
    }

    public override async ValueTask StartAsync()
    {
        await this.RegisterRequestListenerAsync<PricingFeedStatusRequest, PricingFeedStatusResponse?>(
            feedName.FeedTickerHealthRequestAddress(), ReceivedTickerHealthStatusRequest);
        sharedFeedDeserializationRepo.MessageDeserializerRegistered += NewDeserializerRegistered;
        sharedFeedDeserializationRepo.MessageDeserializerUnregistered += ExistingDeserializerUnregistered;
        Timer.RunEvery(TasksFrequencyMs, MonitorDeserializersForSnapshotResync);
    }

    private PricingFeedStatusResponse? ReceivedTickerHealthStatusRequest(
        IBusRespondingMessage<PricingFeedStatusRequest, PricingFeedStatusResponse?> busRequestMessage)
    {
        var pricingFeedStatusResponse = Context.PooledRecycler.Borrow<PricingFeedStatusResponse>();

        var pqMd = syncOk.Head;
        while (pqMd != null)
        {
            pricingFeedStatusResponse.HealthySubscribedTickers.Add(pqMd.Identifier);
            pqMd = pqMd.Next;
        }

        pqMd = syncKo.Head;
        while (pqMd != null)
        {
            pricingFeedStatusResponse.HealthySubscribedTickers.Add(pqMd.Identifier);
            pqMd = pqMd.Next;
        }

        pricingFeedStatusResponse.LastTickerRefreshTime = lastReceivedTickTime;

        return pricingFeedStatusResponse;
    }

    private void NewDeserializerRegistered(IMessageDeserializer addedMessageDeserializer)
    {
        if (addedMessageDeserializer is IPQDeserializer pqDeserializer)
        {
            var deserializerRegistration = new DeserializerEventRegistrations
            {
                SyncOk = SingleParamActionWrapper<IPQDeserializer>.WrapAndAttach(OnInSync)
                , ReceivedUpdate = SingleParamActionWrapper<IPQDeserializer>.WrapAndAttach(OnReceivedUpdate)
                , OutOfSync = SingleParamActionWrapper<IPQDeserializer>.WrapAndAttach(OnOutOfSync)
            };
            registeredDeserializers[pqDeserializer.Identifier.Id] = deserializerRegistration;

            pqDeserializer.SyncOk += deserializerRegistration.SyncOk;
            pqDeserializer.ReceivedUpdate += deserializerRegistration.ReceivedUpdate;
            pqDeserializer.OutOfSync += deserializerRegistration.OutOfSync;

            syncOk.AddFirst(pqDeserializer);
        }
    }

    private void ExistingDeserializerUnregistered(IMessageDeserializer removedMessageDeserializer)
    {
        if (removedMessageDeserializer is IPQDeserializer pqDeserializer)
        {
            if (registeredDeserializers.TryGetValue(pqDeserializer.Identifier.Id, out var deserializerRegistration))
            {
                pqDeserializer.SyncOk -= deserializerRegistration.SyncOk;
                pqDeserializer.ReceivedUpdate -= deserializerRegistration.ReceivedUpdate;
                pqDeserializer.OutOfSync -= deserializerRegistration.OutOfSync;
            }

            syncOk.Remove(pqDeserializer);
            syncKo.Remove(pqDeserializer);
        }
    }

    private void MonitorDeserializersForSnapshotResync()
    {
        try
        {
            if (syncOk.Head == null && syncKo.Head == null) return;

            MoveAllTimedOutTickersToKo();

            FindAndSnapshotKnockedOutTickersReadyForSnapshot();
        }
        catch (Exception ex)
        {
            Logger.Error("Unexpected error in task scheduler: {0}", ex);
        }
    }

    private void MoveAllTimedOutTickersToKo()
    {
        var pqMd = syncOk.Head;
        while (pqMd != null && pqMd.HasTimedOutAndNeedsSnapshot(TimeContext.UtcNow))
        {
            syncOk.Remove(pqMd);
            syncKo.AddFirst(pqMd);
            pqMd = pqMd.Next;
        }
    }

    private void FindAndSnapshotKnockedOutTickersReadyForSnapshot()
    {
        IPQDeserializer? firstToResync = null;

        var requestList = Context.PooledRecycler.Borrow<FeedSourceTickerInfoUpdate>();
        for (var count = 0; count < MaxSnapshotBatch; count++)
        {
            IPQDeserializer? pqMd;
            bool resync;
            if ((pqMd = syncKo.Head) == null)
                break;
            if (firstToResync == null)
                firstToResync = pqMd;
            else if (firstToResync == pqMd)
                break;
            resync = pqMd.CheckResync(TimeContext.UtcNow);
            syncKo.Remove(pqMd);
            syncKo.AddLast(pqMd);

            if (!resync) continue;
            if (!requestList.SourceTickerQuoteInfos.Contains(pqMd.Identifier))
                requestList.SourceTickerQuoteInfos.Add(pqMd.Identifier);
        }

        RequestSnapshotsForTickers(requestList);
    }

    private void RequestSnapshotsForTickers(FeedSourceTickerInfoUpdate deserializersInNeedOfSnapshots)
    {
        if (deserializersInNeedOfSnapshots.SourceTickerQuoteInfos.Any())
            this.Publish(feedRequestSnapshotsAddress, deserializersInNeedOfSnapshots, new DispatchOptions());
        else
            deserializersInNeedOfSnapshots.DecrementRefCount();
    }

    private void OnReceivedUpdate(IPQDeserializer quoteDeserializer)
    {
        lastReceivedTickTime = TimeContext.UtcNow;
        syncOk.Remove(quoteDeserializer);
        syncOk.AddLast(quoteDeserializer);
    }

    private void OnInSync(IPQDeserializer quoteDeserializer)
    {
        syncKo.Remove(quoteDeserializer);
        syncOk.AddLast(quoteDeserializer);
    }

    private void OnOutOfSync(IPQDeserializer quoteDeserializer)
    {
        syncOk.Remove(quoteDeserializer);
        syncKo.AddFirst(quoteDeserializer);
    }


    private class DeserializerEventRegistrations
    {
        public Action<IPQDeserializer> OutOfSync = null!;
        public Action<IPQDeserializer> ReceivedUpdate = null!;
        public Action<IPQDeserializer> SyncOk = null!;
    }
}
