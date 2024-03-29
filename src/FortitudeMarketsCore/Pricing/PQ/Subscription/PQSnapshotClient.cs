﻿#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.NewSocketAPI.State;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public sealed class PQSnapshotClient : ConversationRequester, IPQSnapshotClient
{
    private static ISocketFactoryResolver? socketFactories;
    private readonly uint cxTimeoutMs;
    private readonly IIntraOSThreadSignal intraOSThreadSignal;
    private readonly IFLogger logger;
    private readonly PQClientMessageStreamDecoder messageStreamDecoder;
    private readonly IOSParallelController parallelController;

    private readonly IDictionary<uint, IUniqueSourceTickerIdentifier> requestsQueue =
        new Dictionary<uint, IUniqueSourceTickerIdentifier>();

    private readonly IPQQuoteSerializerRepository snapshotSerializationRepository = new PQQuoteSerializerRepository();

    private DateTime lastSnapshotSent = DateTime.MinValue;
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
        messageStreamDecoder
            = new PQClientMessageStreamDecoder(new ConcurrentMap<uint, IMessageDeserializer>(), PQFeedType.Snapshot);
        messageStreamDecoder.ReceivedMessage += OnReceivedMessage;
        messageStreamDecoder.ReceivedData += OnReceivedMessage;
        socketSessionContext.SerdesFactory.StreamDecoderFactory
            = new SocketStreamDecoderFactory(deserializer => messageStreamDecoder);
        socketSessionContext.SerdesFactory.StreamEncoderFactory = snapshotSerializationRepository;
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get =>
            socketFactories
                ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public IMessageStreamDecoder MessageStreamDecoder => messageStreamDecoder;

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

        var serdesFactory = new SerdesFactory();

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

    public class SnapShotStreamPublisher : VersionedMessage
    {
        public SnapShotStreamPublisher() { }
        public SnapShotStreamPublisher(IVersionedMessage toClone) : base(toClone) { }
        public SnapShotStreamPublisher(byte version) : base(version) { }

        public SnapShotStreamPublisher(SnapShotStreamPublisher toClone)
        {
            Version = toClone.Version;
            Ids = toClone.Ids.ToArray();
        }

        public override uint MessageId => 2121502;
        private uint[] Ids { get; } = Array.Empty<uint>();

        public override IVersionedMessage Clone() => new SnapShotStreamPublisher(this);
    }
}
