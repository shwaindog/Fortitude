// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientBusTopicPublicationAmenderRule : RemoteMessageBusTopicPublicationAmendingRule
{
    private readonly string feedName;
    private readonly string feedTickersSnapshotRequestAddress;

    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientBusTopicPublicationAmenderRule));

    private readonly IPricingServerConfig pricingServerConfig;

    private List<ISourceTickerInfo> feedSourceTickerInfos;

    public PQPricingClientBusTopicPublicationAmenderRule
    (string feedName, List<ISourceTickerInfo> feedSourceTickerInfos
      , ISocketSessionContext socketSessionContext
      , IPricingServerConfig pricingServerConfig
      , IConverterRepository? converterRepository = null, string? registrationRepoName = null)
        : base(feedName.FeedAmendTickerPublicationRuleName(), socketSessionContext, feedName.FeedAmendTickerPublicationAddress(),
               converterRepository, registrationRepoName)
    {
        DefaultNotifyTypeFlags            = DeserializeNotifyTypeFlags.JustMessage;
        this.feedName                     = feedName;
        this.feedSourceTickerInfos        = feedSourceTickerInfos;
        this.pricingServerConfig          = pricingServerConfig;
        feedTickersSnapshotRequestAddress = feedName.FeedTickersSnapshotRequestAddress();
    }

    public override async ValueTask StartAsync()
    {
        logger.Info("PQPricingClientBusTopicPublicationAmenderRule for feedName {0} deployed on {1} and listening to {2}",
                    feedName, Context.RegisteredOn.Name, ListeningOnAddress);
        await FeedSourceTickerInfoListener();
        await LaunchTopicPublicationAmenderListener();
    }

    public override async ValueTask StopAsync()
    {
        await base.StopAsync();
    }

    protected override void MessageDeserializerRegistered(IMessageDeserializer newlyRegisteredMessageDeserializer)
    {
        logger.Info("New Deserializer registered {0}", newlyRegisteredMessageDeserializer.InstanceNumber);
        if (newlyRegisteredMessageDeserializer is IPQQuoteDeserializer)
        {
            var foundTicker
                = feedSourceTickerInfos.FirstOrDefault(stqi => stqi.SourceTickerId == newlyRegisteredMessageDeserializer.RegisteredForMessageId);
            if (foundTicker == null)
            {
                logger.Warn("Could not resolve the feed source ticker requiring a snapshot for message id {0} with [\n{1}\n]",
                            newlyRegisteredMessageDeserializer.RegisteredForMessageId, string.Join("\n    ", feedSourceTickerInfos));
                return;
            }

            var requestSnapshotOnTicker = Context.PooledRecycler.Borrow<FeedSourceTickerInfoUpdate>();
            requestSnapshotOnTicker.FeedName = feedName;
            requestSnapshotOnTicker.SourceTickerInfos.Add(foundTicker);
            Context.MessageBus.Publish(this, feedTickersSnapshotRequestAddress, requestSnapshotOnTicker, new DispatchOptions());
        }
    }

    protected async ValueTask FeedSourceTickerInfoListener()
    {
        await this.RegisterListenerAsync<FeedSourceTickerInfoUpdate>
            (feedName.FeedAvailableTickersUpdateAddress(), ReceivedFeedAvailableTickersUpdate);
    }

    private void ReceivedFeedAvailableTickersUpdate(IBusMessage<FeedSourceTickerInfoUpdate> feedTickersMessage)
    {
        var feedSourceTickerInfosUpdate = feedTickersMessage.Payload.Body();
        if (feedSourceTickerInfosUpdate?.SourceTickerInfos.Any() == true)
            if (!feedSourceTickerInfos.Any() && !feedSourceTickerInfosUpdate.SourceTickerInfos.SequenceEqual(feedSourceTickerInfos))
            {
                feedSourceTickerInfos = feedSourceTickerInfosUpdate.SourceTickerInfos;
                logger.Info("Received updated source tickers [\n {0}\n]", string.Join("\n    ", feedSourceTickerInfos));
            }
    }

    protected override async ValueTask LaunchTopicPublicationAmenderListener()
    {
        await base.LaunchTopicPublicationAmenderListener();
        var interceptorListenPath = feedName.FeedDefaultAllTickersPublishInterceptPattern();
        var addressPublicationAmenderInterceptor = new PQPricingClientTickerPublishAmenderInterceptor(feedName,
         new AddressMatcher(interceptorListenPath), (INetworkInboundMessageQueue)Context.RegisteredOn);

        await this.AddListenSubscribeInterceptor(addressPublicationAmenderInterceptor, MessageQueueType.AllNonNetwork);
    }

    protected override string ExtractSubscriptionPostfix(string fullMessageAddressDestination) =>
        fullMessageAddressDestination.ExtractTickerFromAmendPublicationAddress(feedName).Replace(feedName.FeedAmendTickerPublicationAddress(), "");

    protected override void RuleOverrideDeserializerResolverNoMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        if (CapturedAnyRootDeserializationRepository is not PQClientQuoteDeserializerRepository pqClientQuoteDeserializerRepository)
        {
            messageDeserializerResolveRun.FailureMessage
                = "Expected to have a root MessageDeserializationRepository that is of Type PQClientQuoteDeserializerRepository " +
                  "to be able look up ticker names to SourceTickerInfo and resolve a message deserializer.  Will not be able to register tickers requests.  " +
                  $"Will not be able to update ticker subscription requests for ticker {messageDeserializerResolveRun.SubscribePostFix}";
            logger.Warn(messageDeserializerResolveRun.FailureMessage);
            return;
        }

        var foundTicker = feedSourceTickerInfos.FirstOrDefault(stqi => messageDeserializerResolveRun.SubscribePostFix.Contains(stqi.Ticker));
        if (foundTicker == null)
        {
            messageDeserializerResolveRun.FailureMessage
                = $"Unable to source ticker name from subscription postfix address {messageDeserializerResolveRun.SubscribePostFix}";
            logger.Warn(messageDeserializerResolveRun.FailureMessage);
            return;
        }

        var tickerPricingSubscriptionConfig = new TickerPricingSubscriptionConfig(foundTicker, pricingServerConfig);
        logger.Info("Will create new Deserializer for {0}", foundTicker);

        var pqQuoteDeserializer = pqClientQuoteDeserializerRepository.CreateQuoteDeserializer
            (tickerPricingSubscriptionConfig, messageDeserializerResolveRun.DeserializedType ?? messageDeserializerResolveRun.PublishType);
        pqQuoteDeserializer!.RemoveOnZeroNotifiers        = true;
        messageDeserializerResolveRun.MessageDeserializer = pqQuoteDeserializer;
        messageDeserializerResolveRun.MessageId           = foundTicker.SourceTickerId;
        if (messageDeserializerResolveRun.MessageDeserializer == null || messageDeserializerResolveRun.MessageId == null)
        {
            messageDeserializerResolveRun.FailureMessage
                = $"Unable to source ticker deserializer for {messageDeserializerResolveRun.SubscribePostFix}";
            logger.Warn(messageDeserializerResolveRun.FailureMessage);
        }
    }
}
