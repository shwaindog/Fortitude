#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication.BusRules;

public class PQPricingServerQuotePublisherRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingServerQuotePublisherRule));
    private readonly string feedName;
    private readonly int heartBeatPublishIntervalMs;
    private readonly int heartBeatPublishToleranceRangeMs;
    private readonly IDoublyLinkedList<IPQLevel0Quote> heartbeatQuotes = new DoublyLinkedList<IPQLevel0Quote>();
    private readonly IPricingServerConfig pricingServerConfig;
    private readonly IMap<uint, PQLevel0Quote> publishedQuotesMap = new ConcurrentMap<uint, PQLevel0Quote>();
    private readonly INetworkTopicConnectionConfig updatesConnectionConfig;
    private ITimerUpdate? heartBestRunner;
    private PQPricingServerQuoteUpdatePublisher updatePublisher = null!;

    public PQPricingServerQuotePublisherRule(string feedName, IPricingServerConfig pricingServerConfig)
    {
        this.feedName = feedName;
        this.pricingServerConfig = pricingServerConfig;
        updatesConnectionConfig = pricingServerConfig!.UpdateConnectionConfig;
        heartBeatPublishToleranceRangeMs = pricingServerConfig.HeartBeatServerToleranceRangeMs;
        heartBeatPublishIntervalMs = pricingServerConfig.HeartBeatPublishIntervalMs;
    }

    public override async ValueTask StartAsync()
    {
        await AttemptStartUpdatePublisher();
        await this.RegisterListenerAsync<PublishQuoteEvent>(feedName.FeedTickerPublishAddress(), PublishReceivedTickerHandler);
        await this.RegisterRequestListenerAsync<IList<uint>, IList<ILevel0Quote>>(feedName.FeedTickerLastPublishedRequestAddress()
            , ReceivedQuoteLastPublishedRequest);
    }

    private IList<ILevel0Quote> ReceivedQuoteLastPublishedRequest(IBusRespondingMessage<IList<uint>, IList<ILevel0Quote>> arg)
    {
        var quotesIdsToReturns = arg.Payload.Body();
        var response = Context.PooledRecycler.Borrow<ReusableList<ILevel0Quote>>();
        foreach (var quotesIdsToReturn in quotesIdsToReturns)
        {
            if (!publishedQuotesMap.TryGetValue(quotesIdsToReturn, out var toCopy)) continue;
            var cloned = toCopy!.Clone();
            cloned.AutoRecycleAtRefCountZero = true;
            response.Add(cloned);
        }

        return response;
    }

    private void PublishReceivedTickerHandler(IBusMessage<PublishQuoteEvent> pricePublishRequestMessage)
    {
        var quotePublishEvent = pricePublishRequestMessage.Payload.Body();
        var tickerQuoteInfo = quotePublishEvent.PublishQuote.SourceTickerQuoteInfo;
        var overrideSequenceNumber = quotePublishEvent.OverrideSequenceNumber;
        var overrideMessageFlags = quotePublishEvent.MessageFlags;
        if (!publishedQuotesMap.TryGetValue(tickerQuoteInfo!.Id, out var sendToSerializer))
        {
            sendToSerializer = tickerQuoteInfo.PublishedTypePQInstance();
            sendToSerializer.PQSequenceId = uint.MaxValue;
            sendToSerializer.OverrideSerializationFlags = quotePublishEvent.MessageFlags;
            sendToSerializer.AutoRecycleAtRefCountZero = false;
            sendToSerializer.Recycler = Context.PooledRecycler;
            publishedQuotesMap.Add(tickerQuoteInfo.Id, sendToSerializer);
            if (quotePublishEvent.PublishQuote.QuoteLevel.LessThan(sendToSerializer!.QuoteLevel))
            {
                Logger.Warn("Received a quote lower than the published level.  This would result in unset fields and so wil NOT be published");
                return;
            }

            if (overrideSequenceNumber != null)
            {
                sendToSerializer.PQSequenceId = overrideSequenceNumber.Value;
                sendToSerializer.OverrideSerializationFlags = overrideMessageFlags;
            }

            Publish(sendToSerializer);
            heartBestRunner ??= Context.QueueTimer.RunEvery(heartBeatPublishIntervalMs, HeartBeatIntervalCheck);
        }
        else
        {
            if (quotePublishEvent.PublishQuote.QuoteLevel.LessThan(sendToSerializer!.QuoteLevel))
            {
                Logger.Warn("Received a quote lower than the published level.  This would result in unset fields and so wil NOT be published");
                return;
            }

            sendToSerializer!.CopyFrom(quotePublishEvent.PublishQuote);
            if (overrideSequenceNumber != null)
            {
                sendToSerializer.PQSequenceId = overrideSequenceNumber.Value;
                sendToSerializer.OverrideSerializationFlags = overrideMessageFlags;
            }

            sendToSerializer.OverrideSerializationFlags = quotePublishEvent.MessageFlags;
            Publish(sendToSerializer);
        }
    }

    public void Publish(PQLevel0Quote toSerialize)
    {
        if (!toSerialize.HasUpdates) return;
        if (updatePublisher.IsStarted) updatePublisher.Send(toSerialize);
        heartbeatQuotes.Remove(toSerialize);
        heartbeatQuotes.AddLast(toSerialize);
        toSerialize.LastPublicationTime = TimeContext.UtcNow;
    }

    public void HeartBeatIntervalCheck()
    {
        var heartBeatQuotesMessage = Context.PooledRecycler.Borrow<PQHeartBeatQuotesMessage>();
        var heartBeatsToSend = heartBeatQuotesMessage.QuotesToSendHeartBeats;

        while (!heartbeatQuotes.IsEmpty)
        {
            IPQLevel0Quote? level0Quote;
            if ((level0Quote = heartbeatQuotes.Head) == null
                || (TimeContext.UtcNow - level0Quote.LastPublicationTime).TotalMilliseconds <
                heartBeatPublishIntervalMs - heartBeatPublishToleranceRangeMs)
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
