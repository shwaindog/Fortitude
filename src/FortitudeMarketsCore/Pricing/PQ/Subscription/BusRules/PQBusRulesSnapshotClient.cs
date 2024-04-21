#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Connectivity.Network;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public sealed class PQBusRulesSnapshotClient : ConversationRequester, IPQSnapshotClientCommon
{
    private static ISocketFactoryResolver? socketFactories;
    private readonly string busPublicationSubscriptionAmenderAddress;
    private readonly IRule creatingRule;
    private readonly uint cxTimeoutMs;
    private readonly string feedName;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQBusRulesSnapshotClient));
    private readonly IMessageBus messageBus;
    private readonly IPricingServerConfig pricingServerConfig;

    private readonly IRecycler recycler;
    private readonly string requestResponseHandlerRegistrationAddress;
    private readonly IMessageDeserializationRepository sharedDeserializationRepo;
    private readonly IActionTimer timer;

    private TopicDeserializationRepositoryAmendingRule? receiverQueuePublishAmender;
    private ITimerUpdate? timeoutTimerUpdate;

    public PQBusRulesSnapshotClient(ISocketSessionContext socketSessionContext, IStreamControls streamControls, string feedName, IRule creatingRule
        , IMessageDeserializationRepository sharedDeserializationRepo, IPricingServerConfig pricingServerConfig)
        : base(socketSessionContext, streamControls)
    {
        this.feedName = feedName;
        this.pricingServerConfig = pricingServerConfig;
        this.creatingRule = creatingRule;
        this.sharedDeserializationRepo = sharedDeserializationRepo;
        recycler = creatingRule.Context.PooledRecycler;
        timer = creatingRule.Context.Timer;
        messageBus = creatingRule.Context.MessageBus;
        cxTimeoutMs = socketSessionContext.NetworkTopicConnectionConfig.ConnectionTimeoutMs;
        requestResponseHandlerRegistrationAddress = feedName.RegisterRequestIdResponseSourceAddress();
        busPublicationSubscriptionAmenderAddress = feedName.AmendTickerSubscribeAndPublishAddress();
        Connected += RestartTimeoutTimer;
        Disconnected += DisableTimeout;
        socketSessionContext.SocketFactoryResolver.SocketReceiverFactory.ConfigureNewSocketReceiver += socketReceiver =>
        {
            var currentDeserializerRepo = socketReceiver!.Decoder!.MessageDeserializationRepository;
            currentDeserializerRepo.AttachToEndOfConnectedFallbackRepos(sharedDeserializationRepo);
        };

        socketSessionContext.SocketReceiverUpdated += () =>
        {
            if (socketSessionContext.SocketReceiver != null)
            {
                var messageStreamDecoder = (IPQClientMessageStreamDecoder)socketSessionContext.SocketReceiver.Decoder!;
                messageStreamDecoder.ReceivedMessage += RestartTimeoutTimer;
                messageStreamDecoder.ReceivedData += RestartTimeoutTimer;
            }
        };
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public IList<ISourceTickerQuoteInfo> LastPublishedSourceTickerQuoteInfos { get; private set; } = new List<ISourceTickerQuoteInfo>();

    public async ValueTask<bool> RequestSnapshots(IList<ISourceTickerQuoteInfo> sourceTickerIds, int timeout = 10_000
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        RestartTimeoutTimer();
        var started = await StartAsync(timeout, alternativeExecutionContext);
        if (started)
        {
            logger.Info("Sending snapshot request for streams {0}",
                string.Join(",", sourceTickerIds.Select(sti => sti.Id)));
            var allStreams = sourceTickerIds.Select(x => x.Id).ToArray();
            Send(new PQSnapshotIdsRequest(allStreams));
            return true;
        }

        return false;
    }

    public async ValueTask<PQSourceTickerInfoResponse?> RequestSourceTickerQuoteInfoListAsync(int timeout = 10_000
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        RestartTimeoutTimer();
        var started = await StartAsync(timeout, alternativeExecutionContext);
        if (started)
        {
            var pqSourceTickerInfoRequest = new PQSourceTickerInfoRequest();
            var requestId = pqSourceTickerInfoRequest.NewRequestId();
            var reusableValueTaskSource = recycler.Borrow<ReusableValueTaskSource<PQSourceTickerInfoResponse>>();
            var registerRequest = recycler.Borrow<RemoteRequestIdResponseRegistration>();
            reusableValueTaskSource.ResponseTimeoutAndRecycleTimer = creatingRule.Context.Timer;
            registerRequest.RequestId = requestId;
            registerRequest.ResponseSource = reusableValueTaskSource;

            var registrationResponse = await messageBus.RequestAsync<RemoteRequestIdResponseRegistration, RemoteRegistrationResponse>(creatingRule
                , requestResponseHandlerRegistrationAddress, registerRequest
                , new DispatchOptions(RoutingFlags.TargetSpecific, MessageQueueType.IOInbound, receiverQueuePublishAmender));

            logger.Info("Response to registration is {0}", registrationResponse.Response);

            var responseValueTask = new ValueTask<PQSourceTickerInfoResponse>(reusableValueTaskSource, reusableValueTaskSource.Version);
            logger.Info("Sending SourceTickerInfoRequest for source {0}", Name);
            Send(pqSourceTickerInfoRequest);
            pqSourceTickerInfoRequest.DecrementRefCount();
            var sourceTickerInfosResponse = await responseValueTask;
            LastPublishedSourceTickerQuoteInfos = sourceTickerInfosResponse.SourceTickerQuoteInfos;
            return sourceTickerInfosResponse;
        }

        return null;
    }

    public override async ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        var started = await base.StartAsync(timeoutTimeSpan, alternativeExecutionContext);
        if (started) await CheckAndLaunchTopicDeserializationRepositoryAmendingRuleStarted();
        return started;
    }

    public override async ValueTask<bool> StartAsync(int timeoutMs
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        var started = await base.StartAsync(timeoutMs, alternativeExecutionContext);
        if (started) await CheckAndLaunchTopicDeserializationRepositoryAmendingRuleStarted();
        return started;
    }

    private async ValueTask CheckAndLaunchTopicDeserializationRepositoryAmendingRuleStarted()
    {
        if (receiverQueuePublishAmender == null || receiverQueuePublishAmender.LifeCycleState is RuleLifeCycle.NotStarted or RuleLifeCycle.Stopped)
        {
            var deployedSocketListenerQueue
                = messageBus.BusIOResolver.GetInboundQueueOnSocketListener(SocketSessionContext.SocketDispatcher.Listener!);
            receiverQueuePublishAmender = new PQClientSubscriptionsAmenderRule(SocketSessionContext
                , feedName, pricingServerConfig, null, sharedDeserializationRepo.Name);
            var dispatchResult = await messageBus.DeployRuleAsync(creatingRule, receiverQueuePublishAmender
                , new DeploymentOptions(RoutingFlags.TargetSpecific, MessageQueueType.IOInbound, 1, deployedSocketListenerQueue!.Name));

            logger.Info("Have deployed topicDeserializationRepositoryAmendingRule on {0} with dispatchResults {1}"
                , deployedSocketListenerQueue.Name, dispatchResult);
        }
    }

    public static PQBusRulesSnapshotClient BuildTcpRequester(IMarketConnectionConfig marketConnectionConfig
        , ISocketDispatcherResolver socketDispatcherResolver, string feedName, IRule creatingRule
        , IMessageDeserializationRepository sharedDeserializationRepository)
    {
        var networkConnectionConfig = marketConnectionConfig.PricingServerConfig!.SnapshotConnectionConfig;
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new PQClientClientSerdesRepositoryFactory();

        var socketSessionContext = new BusSocketSessionContext(networkConnectionConfig.TopicName + "Requester", conversationType, conversationProtocol
            ,
            networkConnectionConfig, sockFactories, serdesFactory, socketDispatcherResolver);

        var streamControls = sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQBusRulesSnapshotClient(socketSessionContext, streamControls, feedName, creatingRule, sharedDeserializationRepository
            , marketConnectionConfig.PricingServerConfig);
    }

    private void EnableTimeout()
    {
        timeoutTimerUpdate = timer.RunIn((int)cxTimeoutMs, TimeoutConnection);
    }

    private void DisableTimeout()
    {
        if (timeoutTimerUpdate == null) return;
        timeoutTimerUpdate.Cancel();
        timeoutTimerUpdate = null;
    }

    private void TimeoutConnection()
    {
        SocketSessionContext.StreamControls?.Disconnect();
    }

    private void RestartTimeoutTimer()
    {
        DisableTimeout();
        EnableTimeout();
    }
}
