#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;
using FortitudeBusRules.Messages;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientBusSubscriptionsAmenderRule : TopicDeserializationRepositoryAmendingRule
{
    private const uint PQSourceTickerInfoResponseMessageId = (uint)PQMessageIds.SourceTickerInfoResponse;
    private readonly string feedName;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientBusSubscriptionsAmenderRule));
    private readonly IPricingServerConfig pricingServerConfig;
    private List<ISourceTickerQuoteInfo> feedSourceTickerQuoteInfos = new();

    private ISubscription? listenForFeedSourceTickerInfosSubscription;

    public PQPricingClientBusSubscriptionsAmenderRule(string feedName
        , ISocketSessionContext socketSessionContext
        , IPricingServerConfig pricingServerConfig
        , IConverterRepository? converterRepository = null, string? registrationRepoName = null)
        : base(feedName.FeedAmendTickerPublicationRuleName(), socketSessionContext, feedName.FeedAmendTickerPublicationAddress(),
            feedName.FeedRequestResponseRegistrationAddress(), converterRepository, registrationRepoName)
    {
        DefaultNotifyTypeFlags = DeserializeNotifyTypeFlags.JustMessage;
        this.feedName = feedName;
        this.pricingServerConfig = pricingServerConfig;
    }

    public override async ValueTask StartAsync()
    {
        logger.Info("PQClientSubscriptionsAmenderRule for feedName {0} deployed on {1} and awaiting SourceTickerInfos " +
                    "before enabling subscription.  Listening to {2}", feedName, Context.RegisteredOn.Name, RegisterRequestIdResponseListenAddress);
        await LauncherRequestIdResponseListener();
        await FeedSourceTickerInfoListener();
    }

    protected async ValueTask FeedSourceTickerInfoListener()
    {
        listenForFeedSourceTickerInfosSubscription = await Context.MessageBus.RegisterListenerAsync<FeedSourceTickerInfoUpdate>(this
            , feedName.FeedAvailableTickersUpdateAddress(), ReceivedFeedAvailableTickersUpdate);
    }

    private async ValueTask ReceivedFeedAvailableTickersUpdate(IBusMessage<FeedSourceTickerInfoUpdate> feedTickersMessage)
    {
        var feedSourceTickerInfosUpdate = feedTickersMessage.Payload.Body();
        var hadPreviousInfos = feedSourceTickerQuoteInfos.Any();
        if (ListenForPublishSubscriptions == null && feedSourceTickerInfosUpdate?.SourceTickerQuoteInfos.Any() == true)
        {
            feedSourceTickerQuoteInfos = feedSourceTickerInfosUpdate.SourceTickerQuoteInfos;
            if (!hadPreviousInfos && feedSourceTickerQuoteInfos.Any())
            {
                logger.Info("Received source tickers [\n {0}\n] will start listening to ticker subscriptions on {1}"
                    , string.Join("\n    ", feedSourceTickerQuoteInfos), RemotePublishRegistrationListenAddress);

                await LaunchTopicPublicationAmenderListener();
            }
        }
    }

    protected override async ValueTask LaunchTopicPublicationAmenderListener()
    {
        await base.LaunchTopicPublicationAmenderListener();

        var interceptorListenPath = feedName.FeedDefaultAllTickersPublishInterceptPattern();
        var addressPublicationAmenderInterceptor = new PQPricingClientTickerPublishAmenderInterceptor(feedName,
            new AddressMatcher(interceptorListenPath), (IIOInboundMessageQueue)Context.RegisteredOn);

        await Context.MessageBus.AddListenSubscribeInterceptor(this, addressPublicationAmenderInterceptor, MessageQueueType.AllNonIO);
    }

    protected override string ExtractSubscriptionPostfix(string fullMessageAddressDestination) =>
        fullMessageAddressDestination.ExtractTickerFromAmendPublicationAddress(feedName).Replace(feedName.FeedAmendTickerPublicationAddress(), "");

    protected override void RuleOverrideDeserializerResolverNoMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        if (CapturedAnyRootDeserializationRepository is not PQClientQuoteDeserializerRepository pqClientQuoteDeserializerRepository)
        {
            messageDeserializerResolveRun.FailureMessage
                = "Expected to have a root MessageDeserializationRepository that is of Type PQClientQuoteDeserializerRepository " +
                  "to be able look up ticker names to SourceTickerQuoteInfo and resolve a message deserializer.  Will not be able to register tickers requests.  " +
                  $"Will not be able to update ticker subscription requests for ticker {messageDeserializerResolveRun.SubscribePostFix}";
            logger.Warn(messageDeserializerResolveRun.FailureMessage);
            return;
        }

        if (messageDeserializerResolveRun.DeserializedType == typeof(PQSourceTickerInfoResponse))
        {
            messageDeserializerResolveRun.MessageId = PQSourceTickerInfoResponseMessageId;
            var pqSourceTickerInfoResponseDeserializer = pqClientQuoteDeserializerRepository
                .SourceNotifyingMessageDeserializerFromMessageId<PQSourceTickerInfoResponse>(PQSourceTickerInfoResponseMessageId);
            pqSourceTickerInfoResponseDeserializer!.RemoveOnZeroNotifiers = false;
            messageDeserializerResolveRun.MessageDeserializer = pqSourceTickerInfoResponseDeserializer;
            return;
        }

        var foundTicker = feedSourceTickerQuoteInfos.FirstOrDefault(stqi => messageDeserializerResolveRun.SubscribePostFix.Contains(stqi.Ticker));
        if (foundTicker == null)
        {
            messageDeserializerResolveRun.FailureMessage
                = $"Unable to source ticker name from subscription postfix address {messageDeserializerResolveRun.SubscribePostFix}";
            logger.Warn(messageDeserializerResolveRun.FailureMessage);
            return;
        }

        var tickerPricingSubscriptionConfig = new TickerPricingSubscriptionConfig(foundTicker, pricingServerConfig);

        var pqQuoteDeserializer = pqClientQuoteDeserializerRepository.CreateQuoteDeserializer(tickerPricingSubscriptionConfig
            , messageDeserializerResolveRun.DeserializedType ?? messageDeserializerResolveRun.PublishType);
        pqQuoteDeserializer!.RemoveOnZeroNotifiers = true;
        messageDeserializerResolveRun.MessageDeserializer = pqQuoteDeserializer;
        messageDeserializerResolveRun.MessageId = foundTicker.Id;
        if (messageDeserializerResolveRun.MessageDeserializer == null || messageDeserializerResolveRun.MessageId == null)
        {
            messageDeserializerResolveRun.FailureMessage
                = $"Unable to source ticker deserializer for {messageDeserializerResolveRun.SubscribePostFix}";
            logger.Warn(messageDeserializerResolveRun.FailureMessage);
        }
    }
}
