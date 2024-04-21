#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;
using FortitudeBusRules.Messages;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQClientSubscriptionsAmenderRule : TopicDeserializationRepositoryAmendingRule
{
    private readonly string feedName;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQClientSubscriptionsAmenderRule));
    private readonly IPricingServerConfig pricingServerConfig;
    private IList<ISourceTickerQuoteInfo> feedSourceTickerQuoteInfos = new List<ISourceTickerQuoteInfo>();

    private ISubscription? listenForFeedSourceTickerInfosSubscription;
    private uint pqSourceTickerInfoResponseMessageId = (uint)PQMessageIds.SourceTickerInfoResponse;

    public PQClientSubscriptionsAmenderRule(ISocketSessionContext socketSessionContext, string feedName, IPricingServerConfig pricingServerConfig
        , IConverterRepository? converterRepository = null, string? registrationRepoName = null)
        : base(socketSessionContext, feedName.AmendTickerSubscribeAndPublishAddress(), feedName.RegisterRequestIdResponseSourceAddress()
            , converterRepository
            , registrationRepoName)
    {
        this.feedName = feedName;
        this.pricingServerConfig = pricingServerConfig;
    }

    public override async ValueTask StartAsync()
    {
        logger.Info(
            "PQClientSubscriptionsAmenderRule for feedName {0} deployed on {1} and awaiting SourceTickerInfos before enabling subscription.  Listening to {2}"
            , feedName, Context.RegisteredOn.Name, RegisterRequestIdResponseListenAddress);
        await LauncherRequestIdResponseListener();
        await FeedSourceTickerInfoListener();
    }

    protected async ValueTask FeedSourceTickerInfoListener()
    {
        listenForFeedSourceTickerInfosSubscription = await Context.MessageBus.RegisterListenerAsync<FeedSourceTickerInfoUpdate>(this
            , feedName.FeedAvailableTickersUpdateAddress(), ReceivedSourceTickerInfos);
    }

    private async ValueTask ReceivedSourceTickerInfos(IBusMessage<FeedSourceTickerInfoUpdate> feedTickersMessage)
    {
        var feedSourceTickerInfosUpdate = feedTickersMessage.PayLoad.Body!;
        if (ListenForPublishSubscriptions == null && feedSourceTickerInfosUpdate.FeedSourceTickerQuoteInfos.Any())
        {
            feedSourceTickerQuoteInfos = feedSourceTickerInfosUpdate.FeedSourceTickerQuoteInfos;
            logger.Info("Received source tickers [{0}] will start listening to ticker subscriptions on {1}"
                , string.Join(", ", feedSourceTickerQuoteInfos), RemotePublishRegistrationListenAddress);

            await LaunchTopicPublicationAmenderListener();
        }
    }

    protected override string ExtractSubscriptionPostfix(string fullMessageAddressDestination)
    {
        fullMessageAddressDestination.ExtractTickerFromDefaultPublishAddress(feedName);
        return base.ExtractSubscriptionPostfix(fullMessageAddressDestination);
    }

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
            messageDeserializerResolveRun.MessageId = pqSourceTickerInfoResponseMessageId;
            messageDeserializerResolveRun.MessageDeserializer
                = pqClientQuoteDeserializerRepository.SourceNotifyingMessageDeserializerFromMessageId<PQSourceTickerInfoResponse>(
                    pqSourceTickerInfoResponseMessageId);
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

        messageDeserializerResolveRun.MessageDeserializer = pqClientQuoteDeserializerRepository.CreateQuoteDeserializer(
            tickerPricingSubscriptionConfig
            , messageDeserializerResolveRun.DeserializedType ?? messageDeserializerResolveRun.PublishType);
    }
}
