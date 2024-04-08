#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public sealed class PQSnapshotServer : ConversationResponder, IPQSnapshotServer
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQSnapshotServer));
    private static ISocketFactoryResolver? socketFactories;

    public PQSnapshotServer(ISocketSessionContext socketSessionContext, IAcceptorControls acceptorControls)
        : base(socketSessionContext, acceptorControls)
    {
        MessageSerializationRepository = socketSessionContext.SerdesFactory.MessageSerializationRepository;
        NewClient += HandleNewClient;
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public IMessageSerializationRepository MessageSerializationRepository { get; }

    public event Action<IConversationRequester, PQSnapshotIdsRequest>? OnSnapshotRequest;

    public void Send(IConversationRequester client, IVersionedMessage message)
    {
        client.StreamPublisher!.Send(message);
    }

    public static PQSnapshotServer BuildTcpResponder(INetworkTopicConnectionConfig networkConnectionConfig,
        ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketConversationProtocol.TcpAcceptor;

        var socFactories = SocketFactories;

        var serdesFactory = new PQServerSerdesRepositoryFactory(PQFeedType.Snapshot);

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, socFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Responder";


        var acceptorControls
            = (IAcceptorControls)socFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQSnapshotServer(socketSessionContext, acceptorControls);
    }

    private void HandleNewClient(IConversationRequester newClient)
    {
        var clientSocketReceiver = (ISocketReceiver)newClient.StreamListener!;
        var clientDecoder = (IPQServerMessageStreamDecoder)clientSocketReceiver.Decoder!;
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<PQSnapshotIdsRequest>(OnRequest);
        logger.Info($"New PQSnapshot Client Request {newClient}");
    }

    private void OnRequest(PQSnapshotIdsRequest snapshotIdsRequest, object? header, IConversation? cx)
    {
        OnSnapshotRequest?.Invoke((IConversationRequester)cx, snapshotIdsRequest);
    }
}
