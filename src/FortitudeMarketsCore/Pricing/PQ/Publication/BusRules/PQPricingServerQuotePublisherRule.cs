#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication.BusRules;

public class PQPricingServerQuotePublisherRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingServerQuotePublisherRule));
    private readonly string feedName;
    private readonly int HeartBeatPublishIntervalMs;
    private readonly int HeartBeatPublishToleranceRangeMs;
    private readonly IDoublyLinkedList<IPQLevel0Quote> heartbeatQuotes = new DoublyLinkedList<IPQLevel0Quote>();
    private readonly IMap<uint, PQLevel0Quote> lastPublished = new ConcurrentMap<uint, PQLevel0Quote>();
    private readonly IPricingServerConfig pricingServerConfig;
    private readonly INetworkTopicConnectionConfig updatesConnectionConfig;
    private ITimerUpdate? heartBestRunner;
    private PQPricingServerQuoteUpdatePublisher updatePublisher = null!;

    public PQPricingServerQuotePublisherRule(string feedName, IPricingServerConfig pricingServerConfig)
    {
        this.feedName = feedName;
        this.pricingServerConfig = pricingServerConfig;
        updatesConnectionConfig = pricingServerConfig!.UpdateConnectionConfig;
        HeartBeatPublishToleranceRangeMs = pricingServerConfig.HeartBeatServerToleranceRangeMs;
        HeartBeatPublishIntervalMs = pricingServerConfig.HeartBeatPublishIntervalMs;
    }

    public override async ValueTask StartAsync()
    {
        await AttemptStartUpdatePublisher();
        await Context.MessageBus.RegisterListenerAsync<PublishQuoteEvent>(this, feedName.FeedTickerPublishAddress(), PublishReceivedTickerHandler);
        heartBestRunner = Context.QueueTimer.RunEvery(HeartBeatPublishIntervalMs, HeartBeatIntervalCheck);

        IncrementLifeTimeCount();
    }

    private void PublishReceivedTickerHandler(IBusMessage<PublishQuoteEvent> pricePublishRequestMessage)
    {
        var quotePublishEvent = pricePublishRequestMessage.Payload.Body()!;
        var tickerQuoteInfo = quotePublishEvent.PublishQuote.SourceTickerQuoteInfo;
        if (!lastPublished.TryGetValue(tickerQuoteInfo!.Id, out var pqPicture))
        {
            pqPicture = tickerQuoteInfo.PublishedTypePQInstance();
            pqPicture.PQSequenceId = uint.MaxValue;
            pqPicture.OverrideSerializationFlags = quotePublishEvent.MessageFlags;
            lastPublished.Add(tickerQuoteInfo.Id, pqPicture);
            Publish(pqPicture);
        }
        else
        {
            pqPicture!.CopyFrom(quotePublishEvent.PublishQuote);
            pqPicture.OverrideSerializationFlags = quotePublishEvent.MessageFlags;
            Publish(pqPicture);
        }
    }

    public void Publish(PQLevel0Quote quoteAtAnyLevel)
    {
        if (!quoteAtAnyLevel.HasUpdates) return;
        if (updatePublisher.IsStarted) updatePublisher.Send(quoteAtAnyLevel);
        heartbeatQuotes.Remove(quoteAtAnyLevel);
        heartbeatQuotes.AddLast(quoteAtAnyLevel);
        quoteAtAnyLevel.LastPublicationTime = TimeContext.UtcNow;
    }

    public void HeartBeatIntervalCheck()
    {
        var heartBeatQuotesMessage = Context.PooledRecycler.Borrow<PQHeartBeatQuotesMessage>();
        var heartBeatsToSend = heartBeatQuotesMessage.QuotesToSendHeartBeats;

        while (!heartbeatQuotes.IsEmpty)
        {
            IPQLevel0Quote? level0Quote;
            if ((level0Quote = heartbeatQuotes.Head) == null
                || (TimeContext.UtcNow - level0Quote.LastPublicationTime).TotalMilliseconds < HeartBeatPublishToleranceRangeMs)
                break;
            heartbeatQuotes.Remove(level0Quote);
            heartbeatQuotes.AddLast(level0Quote);
            level0Quote.LastPublicationTime = TimeContext.UtcNow;

            heartBeatsToSend.Add(level0Quote);
        }

        if (heartBeatsToSend.Count > 0 && updatePublisher.IsStarted) updatePublisher.Send(heartBeatQuotesMessage);
    }

    public async ValueTask AttemptStartUpdatePublisher()
    {
        var thisQueue = Context.RegisteredOn;
        if (thisQueue is not IIOOutboundMessageQueue)
            Logger.Warn("Expected this rule to be deployed on an IOOutboundQueue so that it can reduce Queue hops.  Was deployed on {0}"
                , thisQueue.Name);
        var updateServerDispatcher = Context.MessageBus.BusIOResolver.GetOutboundDispatcherResolver(thisQueue as IIOOutboundMessageQueue);
        updatePublisher = PQPricingServerQuoteUpdatePublisher.BuildUdpMulticastPublisher(feedName, updatesConnectionConfig, updateServerDispatcher);
        var workerQueueConnect = Context.GetEventQueues(MessageQueueType.Worker)
            .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted).GetExecutionContextResult<bool, TimeSpan>(this);
        var connected = await updatePublisher.StartAsync(10_000, workerQueueConnect);
        if (connected)
        {
            Logger.Info("Successfully connected to multicast pricing publisher adapter and port for {0}", feedName);
            return;
        }

        Logger.Error(
            "Warning did not connect to the configured adapter and port typically this means there is a misconfiguration between your configuration " +
            "and environment and will not auto resolve.");
    }
}
