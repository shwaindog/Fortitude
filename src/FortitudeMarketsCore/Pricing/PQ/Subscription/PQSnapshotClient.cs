#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQSnapshotClient : IConversationRequester
{
    IPQClientQuoteDeserializerRepository DeserializerRepository { get; }

    IList<ISourceTickerQuoteInfo> LastPublishedSourceTickerQuoteInfos { get; }

    ISourceTickerQuoteInfoRepo SourceTickerQuoteInfoRepo { get; set; }

    bool HasSourceTickerInfo { get; }

    void RequestSourceTickerQuoteInfoList();

    void RequestSnapshots(IList<ISourceTickerQuoteInfo> sourceTickerIds);
}

public sealed class PQSnapshotClient : ConversationRequester, IPQSnapshotClient
{
    private static ISocketFactoryResolver? socketFactories;
    private readonly uint cxTimeoutMs;
    private readonly IIntraOSThreadSignal intraOSThreadSignal;
    private readonly IFLogger logger;
    private readonly IOSParallelController parallelController;

    private readonly IDictionary<uint, ISourceTickerQuoteInfo> requestsQueue =
        new Dictionary<uint, ISourceTickerQuoteInfo>();

    private DateTime lastSnapshotSent = DateTime.MinValue;
    private IPQClientMessageStreamDecoder? messageStreamDecoder;
    private PQSourceTickerInfoRequest? queuedSourceTickerInfoRequest;
    private ISourceTickerQuoteInfoRepo? sourceTickerQuoteInfoRepo;
    private ITimerCallbackSubscription? timerSubscription;

    public PQSnapshotClient(ISocketSessionContext socketSessionContext, IStreamControls streamControls)
        : base(socketSessionContext, streamControls)
    {
        logger = FLoggerFactory.Instance.GetLogger(typeof(PQSnapshotClient));
        parallelController = socketSessionContext.SocketFactoryResolver.ParallelController!;
        intraOSThreadSignal = parallelController!.SingleOSThreadActivateSignal(false);
        cxTimeoutMs = socketSessionContext.NetworkTopicConnectionConfig.ConnectionTimeoutMs;
        Connected += SocketConnection;
        Connected += SendQueuedRequests;
        Disconnected += SocketSessionContextOnStateChanged;

        var serdesFactor = (IPQClientSerdesRepositoryFactory)socketSessionContext.SerdesFactory;
        DeserializerRepository = serdesFactor.MessageDeserializationRepository;
        socketSessionContext.SocketReceiverUpdated += () =>
        {
            if (socketSessionContext.SocketReceiver != null)
            {
                messageStreamDecoder = (IPQClientMessageStreamDecoder)socketSessionContext.SocketReceiver.Decoder!;
                messageStreamDecoder.ReceivedMessage += OnReceivedMessage;
                messageStreamDecoder.ReceivedData += OnReceivedMessage;
                messageStreamDecoder.MessageDeserializationRepository.RegisterDeserializer<PQSourceTickerInfoResponse>()
                    .AddDeserializedNotifier(
                        new PassThroughDeserializedNotifier<PQSourceTickerInfoResponse>(
                            $"{nameof(PQSnapshotClient)}.{nameof(ReceivedSourceTickerInfoResponse)}"
                            , ReceivedSourceTickerInfoResponse));
            }
        };
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get =>
            socketFactories
                ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ISourceTickerQuoteInfoRepo SourceTickerQuoteInfoRepo
    {
        get => sourceTickerQuoteInfoRepo ??= new SourceTickerQuoteInfoRepo(Name);
        set => sourceTickerQuoteInfoRepo = value;
    }

    public IList<ISourceTickerQuoteInfo> LastPublishedSourceTickerQuoteInfos { get; private set; } = new List<ISourceTickerQuoteInfo>();

    public bool HasSourceTickerInfo => LastPublishedSourceTickerQuoteInfos.Any();

    public IPQClientQuoteDeserializerRepository DeserializerRepository { get; }

    public void RequestSnapshots(IList<ISourceTickerQuoteInfo> sourceTickerIds)
    {
        Connect();
        if (IsStarted)
        {
            logger.Info("Sending snapshot request for streams {0}",
                string.Join(",", sourceTickerIds.Select(sti => sti.Id)));
            var allStreams = sourceTickerIds.Select(x => x.Id).ToArray();
            Send(new PQSnapshotIdsRequest(allStreams));
            lastSnapshotSent = TimeContext.UtcNow;
        }
        else
        {
            var queuing = string.Empty;
            lock (requestsQueue)
            {
                foreach (var srcTkr in sourceTickerIds)
                    if (!requestsQueue.ContainsKey(srcTkr.Id))
                    {
                        requestsQueue[srcTkr.Id] = srcTkr;
                        queuing += queuing.Length > 0 ? "," + srcTkr.Id : srcTkr.Id.ToString();
                    }
            }

            if (!string.IsNullOrEmpty(queuing))
                logger.Info("Queuing snapshot request for ticker ids {0}", queuing);
            else
                logger.Info("Snapshot request already queued for ticker ids {0}, last snapshot sent at {1}",
                    string.Join(",", sourceTickerIds.Select(sti => sti.Id)), lastSnapshotSent.ToString("O"));
        }
    }

    public void RequestSourceTickerQuoteInfoList()
    {
        Connect();
        if (IsStarted)
        {
            logger.Info("Sending SourceTickerInfoRequest for source {0}", Name);
            Send(new PQSourceTickerInfoRequest());
        }
        else
        {
            logger.Info("Queuing SourceTickerInfoRequest for source {0}", Name);
            queuedSourceTickerInfoRequest = new PQSourceTickerInfoRequest();
        }
    }

    public override bool Connect()
    {
        var result = base.Connect();
        EnableTimeout();
        return result;
    }

    private void ReceivedSourceTickerInfoResponse(PQSourceTickerInfoResponse sourceTickerInfoResponse, MessageHeader messageHeader
        , IConversation conversation)
    {
        logger.Info("Received source ticker info response for streams to {0}", Name);
        SourceTickerQuoteInfoRepo.AppendReplace(sourceTickerInfoResponse.SourceTickerQuoteInfos);
        LastPublishedSourceTickerQuoteInfos = sourceTickerInfoResponse.SourceTickerQuoteInfos;
    }

    private void SocketSessionContextOnStateChanged()
    {
        DisableTimeout();
    }

    public static PQSnapshotClient BuildTcpRequester(INetworkTopicConnectionConfig networkConnectionConfig
        , ISocketDispatcherResolver socketDispatcherResolver)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new PQClientClientSerdesRepositoryFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, sockFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Requester";

        var streamControls = sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQSnapshotClient(socketSessionContext, streamControls);
    }

    private void SendQueuedRequests()
    {
        uint[] streamIDs;
        string streams;
        lock (requestsQueue)
        {
            streamIDs = requestsQueue.Keys.ToArray();
            streams = string.Join(",", streamIDs);
            requestsQueue.Clear();
        }

        if (streamIDs.Length > 0)
        {
            logger.Info("Sending queued snapshot requests for streams: {0}", streams);
            Send(new PQSnapshotIdsRequest(streamIDs));
            lastSnapshotSent = TimeContext.UtcNow;
        }

        if (queuedSourceTickerInfoRequest != null)
        {
            logger.Info("Sending queued SourceTickerInfoRequest for source {0}", Name);
            Send(new PQSourceTickerInfoRequest());
            queuedSourceTickerInfoRequest = null;
        }
    }

    private void EnableTimeout()
    {
        timerSubscription ??= parallelController.ScheduleWithEarlyTrigger(intraOSThreadSignal, TimeoutConnection!,
            cxTimeoutMs, false);
    }

    private void DisableTimeout()
    {
        if (timerSubscription == null) return;
        timerSubscription.Unregister(intraOSThreadSignal);
        timerSubscription = null;
    }

    private void TimeoutConnection(object state, bool timedOut)
    {
        if (timedOut) SocketSessionContext.StreamControls?.Disconnect();
    }

    private void OnReceivedMessage()
    {
        DisableTimeout();
        EnableTimeout();
    }

    private void SocketConnection()
    {
        DisableTimeout();
        EnableTimeout();
    }
}
