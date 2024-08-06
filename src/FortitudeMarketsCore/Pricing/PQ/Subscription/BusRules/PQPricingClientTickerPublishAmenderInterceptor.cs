// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientTickerPublishAmenderInterceptor : AddressListenSubscribeInterceptor
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientTickerPublishAmenderInterceptor));

    private readonly string feedName;

    private readonly INetworkInboundMessageQueue subscribeQueue;

    public PQPricingClientTickerPublishAmenderInterceptor
        (string feedName, IAddressMatcher addressMatcher, INetworkInboundMessageQueue subscribeQueue)
        : base(feedName.FeedAddress(), addressMatcher)
    {
        if (!addressMatcher.AddressMatchPattern.Contains(feedName.FeedDefaultAllTickersPublishBaseAddress()))
            throw new ArgumentException
                ($"Expected this rule to be listening to subscriptions from {feedName.FeedDefaultAllTickersPublishBaseAddress()}"
               , feedName.FeedDefaultAllTickersPublishBaseAddress());
        this.feedName       = feedName;
        this.subscribeQueue = subscribeQueue;
    }

    public override async ValueTask RunInterceptorAction(IMessageListenerRegistration messageListenerRegistration)
    {
        var tickerSubscribeAddress        = messageListenerRegistration.PublishAddress;
        var subscribeTicker               = tickerSubscribeAddress.ExtractTickerFromFeedDefaultTickerPublishAddress(feedName);
        var amendTickerPublicationAddress = feedName.FeedAmendTickerPublicationAddress(subscribeTicker);

        var subscriberRule = messageListenerRegistration.SubscriberRule;
        var busPublicationRegistration
            = subscriberRule.Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
        busPublicationRegistration.DeserializedType      = messageListenerRegistration.PayloadType;
        busPublicationRegistration.PublishType           = messageListenerRegistration.PayloadType;
        busPublicationRegistration.AddRemoveRegistration = AddRemoveCommand.Add;
        busPublicationRegistration.PublishAddress        = tickerSubscribeAddress;
        busPublicationRegistration.QueueContext          = subscriberRule.Context;

        var amendResult = await subscribeQueue
            .RequestWithPayloadAsync<RemoteMessageBusPublishRegistration, RemoteMessageBusPublishRegistrationResponse>(
             busPublicationRegistration, subscriberRule, amendTickerPublicationAddress);
        if (!amendResult?.Succeeded ?? false)
            Logger.Warn("Failed when subscribing to {0}, got {1} for {2}", tickerSubscribeAddress, amendResult, messageListenerRegistration);

        messageListenerRegistration.Unsubscribed += UnsubscribeRequest;
    }

    public async ValueTask UnsubscribeRequest(IRule rule, IMessageListenerRegistration messageListenerRegistration, string listenAddress)
    {
        var tickerSubscribeAddress        = messageListenerRegistration.PublishAddress;
        var subscribeTicker               = tickerSubscribeAddress.ExtractTickerFromFeedDefaultTickerPublishAddress(feedName);
        var amendTickerPublicationAddress = feedName.FeedAmendTickerPublicationAddress(subscribeTicker);

        var subscriberRule = messageListenerRegistration.SubscriberRule;
        var busPublicationRegistration
            = subscriberRule.Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
        busPublicationRegistration.DeserializedType      = messageListenerRegistration.PayloadType;
        busPublicationRegistration.PublishType           = messageListenerRegistration.PayloadType;
        busPublicationRegistration.AddRemoveRegistration = AddRemoveCommand.Remove;
        busPublicationRegistration.PublishAddress        = tickerSubscribeAddress;
        busPublicationRegistration.QueueContext          = subscriberRule.Context;

        var amendResult = await subscribeQueue
            .RequestWithPayloadAsync<RemoteMessageBusPublishRegistration, RemoteMessageBusPublishRegistrationResponse>(
             busPublicationRegistration, subscriberRule, amendTickerPublicationAddress);

        if (!amendResult?.Succeeded ?? false)
            Logger.Warn("Failed unsubscribing to {0}, got {1} for {2}", tickerSubscribeAddress, amendResult, messageListenerRegistration);
    }
}
