#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public struct SourceTickerSubscriptionRequest
{
    public string Ticker { get; set; }
}

public struct SourceTickerSubscriptionResponse
{
    public bool Succeeded { get; }

    public string Message { get; }
}

public class PQClientSourceFeedRule : Rule
{
    private readonly string defaultAllTickerSubAddress;

    private readonly string feedAddress;
    private readonly string feedName;
    private IMarketConnectionConfig marketConnectionConfig;

    private PQClientSnapshotRequesterRule? pqClientSnapshotRequesterRule;
    private PQClientUpdateSubscriberRule? pqClientUpdateSubscriberRule;
    private MessageDeserializationRepository sharedSnapshotAndUpdateDeserializationRepository;
    private SourceFeedStatus snapshotFeedStatus;
    private ISocketDispatcherResolver socketDispatcherResolver;

    private SourceFeedStatus updateFeedStatus;

    public PQClientSourceFeedRule(IMarketConnectionConfig marketConnectionConfig) : base("PQClientSourceFeedRule" + marketConnectionConfig.Name)
    {
        this.marketConnectionConfig = marketConnectionConfig;
        feedName = marketConnectionConfig.Name;
        feedAddress = feedName.FeedAddress();
        defaultAllTickerSubAddress = feedName.DefaultAllTickersPublishAddress();
    }

    public override async ValueTask StartAsync()
    {
        await Context.MessageBus.RegisterRequestListenerAsync<SourceTickerSubscriptionRequest, SourceTickerSubscriptionResponse>(this
            , defaultAllTickerSubAddress
            , ReceivedTickerSubscriptionRequest);
        Context.MessageBus.Publish(this, feedAddress, new SourceFeedUpdate(SourceFeedStatus.Starting, feedName, feedAddress)
            , new DispatchOptions(RoutingFlags.DefaultPublish));
        socketDispatcherResolver = Context.MessageBus.BusIOResolver.GetDispatcherResolver(QueueSelectionStrategy.FirstInSet);
        sharedSnapshotAndUpdateDeserializationRepository
            = new MessageDeserializationRepository($"{feedName}SharedSnapshotAndUpdateRepo", Context.PooledRecycler);
        pqClientSnapshotRequesterRule = new PQClientSnapshotRequesterRule(feedName, marketConnectionConfig
            , sharedSnapshotAndUpdateDeserializationRepository, socketDispatcherResolver);
        pqClientUpdateSubscriberRule = new PQClientUpdateSubscriberRule(feedName, marketConnectionConfig
            , sharedSnapshotAndUpdateDeserializationRepository, socketDispatcherResolver);
        var launchSnapshotClient = Context.MessageBus.DeployRuleAsync(this, pqClientSnapshotRequesterRule, new DeploymentOptions());
        var launchUpdateClient = Context.MessageBus.DeployRuleAsync(this, pqClientUpdateSubscriberRule, new DeploymentOptions());
        await launchSnapshotClient;
        await launchUpdateClient;
    }

    public SourceTickerSubscriptionResponse ReceivedTickerSubscriptionRequest(
        IBusRespondingMessage<SourceTickerSubscriptionRequest, SourceTickerSubscriptionResponse> busRequestMessage) =>
        new();
}
