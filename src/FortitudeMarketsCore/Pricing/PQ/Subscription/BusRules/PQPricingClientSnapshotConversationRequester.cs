#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Connectivity.Network;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
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

public sealed class PQPricingClientSnapshotConversationRequester : ConversationRequester, IPQSnapshotClientCommon
{
    private static ISocketFactoryResolver? socketFactories;
    private readonly IRule creatingRule;
    private readonly uint cxTimeoutMs;
    private readonly string feedName;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientSnapshotConversationRequester));
    private readonly IMessageBus messageBus;

    private readonly IRecycler recycler;
    private readonly string requestResponseHandlerRegistrationAddress;
    private readonly IMessageDeserializationRepository sharedDeserializationRepo;
    private readonly IActionTimer timer;
    private bool hasHadSuccessfullSourceTickerResponse;

    private IRule? receiverQueuePublishAmender;
    private ITimerUpdate? timeoutTimerUpdate;

    public PQPricingClientSnapshotConversationRequester(ISocketSessionContext socketSessionContext, IStreamControls streamControls, string feedName
        , IRule creatingRule
        , IMessageDeserializationRepository sharedDeserializationRepo)
        : base(socketSessionContext, streamControls)
    {
        this.feedName = feedName;
        this.creatingRule = creatingRule;
        this.sharedDeserializationRepo = sharedDeserializationRepo;
        recycler = creatingRule.Context.PooledRecycler;
        timer = creatingRule.Context.QueueTimer;
        messageBus = creatingRule.Context.MessageBus;
        cxTimeoutMs = socketSessionContext.NetworkTopicConnectionConfig.ConnectionTimeoutMs;
        requestResponseHandlerRegistrationAddress = feedName.FeedRequestResponseRegistrationAddress();
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

    public IIOInboundMessageQueue? IOInboundMessageQueue => SocketSessionContext.IOInboundMessageQueue(messageBus);

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
            reusableValueTaskSource.ResponseTimeoutAndRecycleTimer = creatingRule.Context.QueueTimer;
            registerRequest.RequestId = requestId;
            registerRequest.ResponseSource = reusableValueTaskSource;

            var registrationResponse = await messageBus.RequestAsync<RemoteRequestIdResponseRegistration, RemoteRegistrationResponse>(creatingRule
                , requestResponseHandlerRegistrationAddress, registerRequest
                , new DispatchOptions(RoutingFlags.TargetSpecific, MessageQueueType.IOInbound, receiverQueuePublishAmender));

            if (!registrationResponse.Response?.Succeeded ?? false)
            {
                logger.Info("Failed registration source ticker id response got {0}, for {1}", registrationResponse.Response, feedName);
                return null;
            }

            var responseValueTask = new ValueTask<PQSourceTickerInfoResponse>(reusableValueTaskSource, reusableValueTaskSource.Version);
            if (!hasHadSuccessfullSourceTickerResponse) logger.Info("Sending SourceTickerInfoRequest for source {0}", Name);
            Send(pqSourceTickerInfoRequest);
            pqSourceTickerInfoRequest.DecrementRefCount();
            var sourceTickerInfosResponse = await responseValueTask;
            if (!sourceTickerInfosResponse.SourceTickerQuoteInfos.Any())
                logger.Info("Request for source ticker info return no tickers for {0}. Got {1}", feedName, sourceTickerInfosResponse);
            else
                hasHadSuccessfullSourceTickerResponse = true;
            LastPublishedSourceTickerQuoteInfos = sourceTickerInfosResponse.SourceTickerQuoteInfos;
            return sourceTickerInfosResponse;
        }

        return null;
    }

    public override async ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        var started = await base.StartAsync(timeoutTimeSpan, alternativeExecutionContext);
        if (started) await CheckAndLaunchTopicRequestResponseRegistrationRuleStarted();
        return started;
    }

    public override async ValueTask<bool> StartAsync(int timeoutMs
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        var started = await base.StartAsync(timeoutMs, alternativeExecutionContext);
        if (started) await CheckAndLaunchTopicRequestResponseRegistrationRuleStarted();
        return started;
    }

    private async ValueTask CheckAndLaunchTopicRequestResponseRegistrationRuleStarted()
    {
        receiverQueuePublishAmender = messageBus.RulesMatching(r => r.FriendlyName == feedName.FeedRegisterRemoteResponseRuleName()).FirstOrDefault();
        if (receiverQueuePublishAmender == null || receiverQueuePublishAmender.LifeCycleState is RuleLifeCycle.NotStarted or RuleLifeCycle.Stopped)
        {
            var deployedSocketListenerQueue = IOInboundMessageQueue;
            receiverQueuePublishAmender
                = new PQPricingClientRequestResponseRegistrationRule(feedName, SocketSessionContext, sharedDeserializationRepo.Name);
            var dispatchResult = await messageBus.DeployRuleAsync(creatingRule, receiverQueuePublishAmender
                , new DeploymentOptions(RoutingFlags.TargetSpecific, MessageQueueType.IOInbound, 1, deployedSocketListenerQueue!.Name));

            logger.Info("Have deployed PQPricingClientRequestResponseRegistrationRule on {0} with dispatchResults {1}"
                , deployedSocketListenerQueue.Name, dispatchResult);
        }
    }

    public static PQPricingClientSnapshotConversationRequester BuildTcpRequester(IMarketConnectionConfig marketConnectionConfig
        , ISocketDispatcherResolver socketDispatcherResolver, string feedName, IRule creatingRule
        , IMessageDeserializationRepository sharedDeserializationRepository)
    {
        var networkConnectionConfig = marketConnectionConfig.PricingServerConfig!.SnapshotConnectionConfig;
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new PQClientClientSerdesRepositoryFactory();

        var socketSessionContext = new BusSocketSessionContext(networkConnectionConfig.TopicName + "Requester",
            conversationType, conversationProtocol, networkConnectionConfig, sockFactories, serdesFactory, socketDispatcherResolver);

        var streamControls = sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQPricingClientSnapshotConversationRequester(socketSessionContext, streamControls,
            feedName, creatingRule, sharedDeserializationRepository);
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
        logger.Warn("Closing PQSnapshotClient for {0} as no data timeout has been reach", feedName);
        SocketSessionContext.StreamControls?.Stop(CloseReason.Completed, "Timeout after a lack of activity.");
    }

    private void RestartTimeoutTimer()
    {
        DisableTimeout();
        EnableTimeout();
    }
}
