#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using SocketsAPI = FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public sealed class PQSnapshotServer : ConversationResponder, IPQSnapshotServer
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQSnapshotServer));
    private static readonly PQServerSerializationRepository SnapshotSerializationRepository = new(PQFeedType.Snapshot);
    private static SocketsAPI.ISocketFactories? socketFactories;

    public PQSnapshotServer(SocketsAPI.ISocketSessionContext socketSessionContext, IAcceptorControls acceptorControls)
        : base(socketSessionContext, acceptorControls)
    {
        socketSessionContext.SerdesFactory.StreamDecoderFactory
            = new SocketsAPI.SocketStreamDecoderFactory(deserializer => new PQServerMessageStreamDecoder(OnRequest));
        socketSessionContext.SerdesFactory.StreamEncoderFactory = SnapshotSerializationRepository;
        NewClient += HandleNewClient;
    }

    public static SocketsAPI.ISocketFactories SocketFactories
    {
        get => socketFactories ??= SocketsAPI.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public int RegisteredSerializersCount =>
        SocketSessionContext.SerdesFactory.StreamEncoderFactory!.RegisteredSerializerCount;

    public event Action<IConversationRequester, uint[]>? OnSnapshotRequest;

    public void Send(IConversationRequester client, IVersionedMessage message)
    {
        client.StreamPublisher!.Send(message);
    }

    public static PQSnapshotServer BuildTcpResponder(ISocketTopicConnectionConfig socketConnectionConfig,
        ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketsAPI.SocketConversationProtocol.TcpAcceptor;

        var socFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketsAPI.SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.TopicName, socketConnectionConfig, socFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Responder";


        var acceptorControls
            = (IAcceptorControls)socFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQSnapshotServer(socketSessionContext, acceptorControls);
    }

    private void HandleNewClient(IConversationRequester newClient)
    {
        logger.Info($"New PQSnapshot Client Request {newClient}");
    }

    private void OnRequest(IConversationRequester cx, uint[] streamIDs)
    {
        OnSnapshotRequest?.Invoke(cx, streamIDs);
    }

    public void RegisterSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new()
    {
        SocketSessionContext.SerdesFactory.StreamEncoderFactory!.RegisterMessageSerializer(msgId
            , SnapshotSerializationRepository.GetSerializer<TM>(msgId));
    }

    public void UnregisterSerializer(uint msgId)
    {
        SocketSessionContext.SerdesFactory.StreamEncoderFactory!.UnregisterMessageSerializer(msgId);
    }

    public void Enqueue(ISessionConnection cx, IVersionedMessage message)
    {
        cx.SessionSender!.Enqueue(message, SnapshotSerializationRepository.GetSerializer(message.MessageId));
    }
}
