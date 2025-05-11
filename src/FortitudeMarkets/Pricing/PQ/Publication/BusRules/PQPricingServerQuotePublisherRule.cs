// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication.BusRules.BusMessages;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication.BusRules;

public class PQPricingServerQuotePublisherRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingServerQuotePublisherRule));

    private readonly string feedName;

    private readonly int heartBeatPublishIntervalMs;
    private readonly int heartBeatPublishToleranceRangeMs;

    private readonly IDoublyLinkedList<IPQPublishableTickInstant> heartbeatQuotes = new DoublyLinkedList<IPQPublishableTickInstant>();

    private readonly IPricingServerConfig          pricingServerConfig;
    private readonly IMap<uint, PQPublishableTickInstant>     publishedQuotesMap = new ConcurrentMap<uint, PQPublishableTickInstant>();
    private readonly INetworkTopicConnectionConfig updatesConnectionConfig;

    private ITimerUpdate? heartBestRunner;

    private PQPricingServerQuoteUpdatePublisher updatePublisher = null!;

    public PQPricingServerQuotePublisherRule(string feedName, IPricingServerConfig pricingServerConfig)
        : base(feedName + "_" + nameof(PQPricingServerQuotePublisherRule))
    {
        this.feedName = feedName;

        this.pricingServerConfig = pricingServerConfig;
        updatesConnectionConfig  = pricingServerConfig!.UpdateConnectionConfig;

        heartBeatPublishToleranceRangeMs = pricingServerConfig.HeartBeatServerToleranceRangeMs;
        heartBeatPublishIntervalMs       = pricingServerConfig.HeartBeatPublishIntervalMs;
    }

    public override async ValueTask StartAsync()
    {
        await AttemptStartUpdatePublisher();
        await this.RegisterListenerAsync<PublishQuoteEvent>(feedName.FeedTickerPublishAddress(), PublishReceivedTickerHandler);
        await this.RegisterRequestListenerAsync<IList<uint>, IList<IPQPublishableTickInstant>>
            (feedName.FeedTickerLastPublishedQuotesRequestAddress(), ReceivedQuoteLastPublishedRequest);
    }

    private IList<IPQPublishableTickInstant> ReceivedQuoteLastPublishedRequest(IBusRespondingMessage<IList<uint>, IList<IPQPublishableTickInstant>> arg)
    {
        var response           = Context.PooledRecycler.Borrow<ReusableList<IPQPublishableTickInstant>>();
        var quotesIdsToReturns = arg.Payload.Body();
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
        var quoteToPublish    = quotePublishEvent.PublishQuote;
        // if (quoteToPublish.SingleValue > 0) Logger.Info("PublishReceivedTickerHandler received {0}", quoteToPublish);
        var tickerInfo             = quoteToPublish.SourceTickerInfo;
        var overrideSequenceNumber = quotePublishEvent.OverrideSequenceNumber;
        var overrideMessageFlags   = quotePublishEvent.MessageFlags;
        if (!publishedQuotesMap.TryGetValue(tickerInfo!.SourceTickerId, out var sendToSerializer))
        {
            sendToSerializer                            = tickerInfo.PublishedTypePQInstance();
            sendToSerializer.PQSequenceId               = uint.MaxValue;
            sendToSerializer.OverrideSerializationFlags = quotePublishEvent.MessageFlags;
            sendToSerializer.AutoRecycleAtRefCountZero  = false;
            sendToSerializer.Recycler                   = Context.PooledRecycler;
            sendToSerializer.CopyFrom(quoteToPublish);
            publishedQuotesMap.Add(tickerInfo.SourceTickerId, sendToSerializer);
            if (quoteToPublish.TickerQuoteDetailLevel.LessThan(sendToSerializer!.TickerQuoteDetailLevel))
            {
                Logger.Warn("Received a quote lower than the published level.  This would result in unset fields and so wil NOT be published");
                return;
            }

            if (overrideSequenceNumber != null) sendToSerializer.PQSequenceId = overrideSequenceNumber.Value;

            Publish(sendToSerializer);
            heartBestRunner ??= Context.QueueTimer.RunEvery(heartBeatPublishIntervalMs, HeartBeatIntervalCheck);
        }
        else
        {
            if (quoteToPublish.TickerQuoteDetailLevel.LessThan(sendToSerializer!.TickerQuoteDetailLevel))
            {
                Logger.Warn("Received a quote lower than the published level.  This would result in unset fields and so wil NOT be published");
                return;
            }

            sendToSerializer!.CopyFrom(quoteToPublish);
            if (overrideSequenceNumber != null) sendToSerializer.PQSequenceId = overrideSequenceNumber.Value;
            sendToSerializer.OverrideSerializationFlags = overrideMessageFlags;

            sendToSerializer.OverrideSerializationFlags = quotePublishEvent.MessageFlags;
            Publish(sendToSerializer);
        }
    }

    public void Publish(PQPublishableTickInstant toSerialize)
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
        var heartBeatsToSend       = heartBeatQuotesMessage.QuotesToSendHeartBeats;

        while (!heartbeatQuotes.IsEmpty)
        {
            IPQPublishableTickInstant? pqTickInstant;
            if ((pqTickInstant = heartbeatQuotes.Head) == null
             || (TimeContext.UtcNow - pqTickInstant.LastPublicationTime).TotalMilliseconds <
                heartBeatPublishIntervalMs - heartBeatPublishToleranceRangeMs)
                break;
            heartbeatQuotes.Remove(pqTickInstant);
            heartbeatQuotes.AddLast(pqTickInstant);
            pqTickInstant.LastPublicationTime = TimeContext.UtcNow;

            heartBeatsToSend.Add(pqTickInstant);
        }

        if (heartBeatsToSend.Count > 0 && updatePublisher.IsStarted)
        {
            Logger.Info("Publishing heartbeats for [{0}]"
                      , heartBeatQuotesMessage.QuotesToSendHeartBeats
                                              .Select(q => $"MessageId: {q.SourceTickerInfo!.SourceTickerId}, PQSequenceId {q.PQSequenceId}")
                                              .JoinToString());
            updatePublisher.Send(heartBeatQuotesMessage);
        }
    }

    public async ValueTask AttemptStartUpdatePublisher()
    {
        var thisQueue = Context.RegisteredOn;
        if (thisQueue is not INetworkOutboundMessageQueue)
            Logger.Warn("Expected this rule to be deployed on an NetworkOutboundQueue so that it can reduce Queue hops.  Was deployed on {0}"
                      , thisQueue.Name);
        var updateServerDispatcher
            = Context.MessageBus.BusNetworkResolver.GetNetworkOutboundDispatcherResolver(thisQueue as INetworkOutboundMessageQueue);
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
