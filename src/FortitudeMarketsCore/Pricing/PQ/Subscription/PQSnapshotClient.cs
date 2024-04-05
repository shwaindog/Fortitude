#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQSnapshotClient : IConversationRequester
{
    IPQClientQuoteDeserializerRepository DeserializerRepository { get; }

    void RequestSnapshots(IList<IUniqueSourceTickerIdentifier> sourceTickerIds);
}

public sealed class PQSnapshotClient : ConversationRequester, IPQSnapshotClient
{
    private static ISocketFactoryResolver? socketFactories;
    private readonly uint cxTimeoutMs;
    private readonly IIntraOSThreadSignal intraOSThreadSignal;
    private readonly IFLogger logger;
    private readonly IOSParallelController parallelController;

    private readonly IDictionary<uint, IUniqueSourceTickerIdentifier> requestsQueue =
        new Dictionary<uint, IUniqueSourceTickerIdentifier>();

    private DateTime lastSnapshotSent = DateTime.MinValue;
    private IPQClientMessageStreamDecoder? messageStreamDecoder;
    private ITimerCallbackSubscription? timerSubscription;

    public PQSnapshotClient(ISocketSessionContext socketSessionContext, IInitiateControls initiateControls)
        : base(socketSessionContext, initiateControls)
    {
        logger = FLoggerFactory.Instance.GetLogger(typeof(PQSnapshotClient));
        parallelController = socketSessionContext.SocketFactoryResolver.ParallelController!;
        intraOSThreadSignal = parallelController!.SingleOSThreadActivateSignal(false);
        cxTimeoutMs = socketSessionContext.NetworkTopicConnectionConfig.ConnectionTimeoutMs;
        socketSessionContext.SocketConnected += SocketConnection;
        socketSessionContext.SocketConnected += SendQueuedRequests;
        socketSessionContext.StateChanged += SocketSessionContextOnStateChanged;

        var serdesFactor = (IPQClientSerdesRepositoryFactory)socketSessionContext.SerdesFactory;
        DeserializerRepository = serdesFactor.MessageDeserializationRepository;
        socketSessionContext.SocketReceiverUpdated += () =>
        {
            if (socketSessionContext.SocketReceiver != null)
            {
                messageStreamDecoder = (IPQClientMessageStreamDecoder)socketSessionContext.SocketReceiver.Decoder!;
                messageStreamDecoder.ReceivedMessage += OnReceivedMessage;
                messageStreamDecoder.ReceivedData += OnReceivedMessage;
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

    public IPQClientQuoteDeserializerRepository DeserializerRepository { get; }

    public void RequestSnapshots(IList<IUniqueSourceTickerIdentifier> sourceTickerIds)
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

    public override void Connect()
    {
        base.Connect();
        EnableTimeout();
    }

    private void SocketSessionContextOnStateChanged(SocketSessionState socketState)
    {
        if (socketState == SocketSessionState.Disconnected) DisableTimeout();
    }

    public static PQSnapshotClient BuildTcpRequester(INetworkTopicConnectionConfig networkConnectionConfig
        , ISocketDispatcherResolver socketDispatcherResolver)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new PQClientClientSerdesRepositoryFactory(PQFeedType.Snapshot);

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, sockFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Requester";

        var initControls
            = (IInitiateControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQSnapshotClient(socketSessionContext, initControls);
    }

    private void SendQueuedRequests(ISocketConnection socketConnection)
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
        if (timedOut) ((IInitiateControls?)SocketSessionContext.StreamControls)?.Disconnect();
    }

    private void OnReceivedMessage()
    {
        DisableTimeout();
        EnableTimeout();
    }

    private void SocketConnection(ISocketConnection socketConnection)
    {
        DisableTimeout();
        EnableTimeout();
    }
}
