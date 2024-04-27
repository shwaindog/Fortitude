#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
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
    private readonly IIOInboundMessageQueue subscribeQueue;

    public PQPricingClientTickerPublishAmenderInterceptor(string feedName, IAddressMatcher addressMatcher,
        IIOInboundMessageQueue subscribeQueue) : base(feedName.FeedAddress(), addressMatcher)
    {
        if (!addressMatcher.AddressMatchPattern.Contains(feedName.FeedDefaultAllTickersPublishBaseAddress()))
            throw new ArgumentException(
                $"Expected this rule to be listening to subscriptions from {feedName.FeedDefaultAllTickersPublishBaseAddress()}"
                , feedName.FeedDefaultAllTickersPublishBaseAddress());
        this.feedName = feedName;
        this.subscribeQueue = subscribeQueue;
    }

    public override async ValueTask RunInterceptorAction(IMessageListenerSubscription messageListenerSubscription)
    {
        var tickerSubscribeAddress = messageListenerSubscription.PublishAddress;
        var subscribeTicker = tickerSubscribeAddress.ExtractTickerFromFeedDefaultTickerPublishAddress(feedName);
        var amendTickerPublicationAddress = feedName.FeedAmendTickerPublicationAddress(subscribeTicker);

        var subscriberRule = messageListenerSubscription.SubscriberRule;
        var busPublicationRegistration
            = subscriberRule.Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
        busPublicationRegistration.DeserializedType = messageListenerSubscription.PayloadType;
        busPublicationRegistration.PublishType = messageListenerSubscription.PayloadType;
        busPublicationRegistration.AddRemoveRegistration = AddRemoveCommand.Add;
        busPublicationRegistration.PublishAddress = tickerSubscribeAddress;
        busPublicationRegistration.QueueContext = subscriberRule.Context;

        var amendResult = await subscribeQueue
            .RequestFromPayloadAsync<RemoteMessageBusPublishRegistration, RemoteMessageBusPublishRegistrationResponse>(
                busPublicationRegistration, subscriberRule, amendTickerPublicationAddress);

        Logger.Info("When subscribing to {0}, got {1} for {2}", tickerSubscribeAddress, amendResult, messageListenerSubscription);

        messageListenerSubscription.Unsubscribed += UnsubscribeRequest;
    }

    public async ValueTask UnsubscribeRequest(IRule rule, IMessageListenerSubscription messageListenerSubscription, string listenAddress)
    {
        var tickerSubscribeAddress = messageListenerSubscription.PublishAddress;
        var subscribeTicker = tickerSubscribeAddress.ExtractTickerFromFeedDefaultTickerPublishAddress(feedName);
        var amendTickerPublicationAddress = feedName.FeedAmendTickerPublicationAddress(subscribeTicker);

        var subscriberRule = messageListenerSubscription.SubscriberRule;
        var busPublicationRegistration
            = subscriberRule.Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
        busPublicationRegistration.DeserializedType = messageListenerSubscription.PayloadType;
        busPublicationRegistration.PublishType = messageListenerSubscription.PayloadType;
        busPublicationRegistration.AddRemoveRegistration = AddRemoveCommand.Remove;
        busPublicationRegistration.PublishAddress = tickerSubscribeAddress;
        busPublicationRegistration.QueueContext = subscriberRule.Context;

        var amendResult = await subscribeQueue
            .RequestFromPayloadAsync<RemoteMessageBusPublishRegistration, RemoteMessageBusPublishRegistrationResponse>(
                busPublicationRegistration, subscriberRule, amendTickerPublicationAddress);

        Logger.Info("When unsubscribing to {0}, got {1} for {2}", tickerSubscribeAddress, amendResult, messageListenerSubscription);
    }
}
