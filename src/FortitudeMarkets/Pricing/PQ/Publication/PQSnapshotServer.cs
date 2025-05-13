#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
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
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication;

public interface IPQSnapshotServer : IConversationResponder
{
    event Action<IConversationRequester, PQSnapshotIdsRequest>? OnSnapshotRequest;
    event Action<PQSourceTickerInfoRequest, IConversationRequester>? ReceivedSourceTickerInfoRequest;
    void Send(IConversationRequester client, IVersionedMessage message);
}

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

    public event Action<PQSourceTickerInfoRequest, IConversationRequester>? ReceivedSourceTickerInfoRequest;

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

        var serdesFactory = new PQServerSerdesRepositoryFactory(PQMessageFlags.Snapshot);

        var socketSessionContext = new SocketSessionContext(networkConnectionConfig.TopicName + "Responder", conversationType, conversationProtocol,
            networkConnectionConfig, socFactories, serdesFactory
            , socketDispatcherResolver);


        var acceptorControls
            = (IAcceptorControls)socFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQSnapshotServer(socketSessionContext, acceptorControls);
    }

    private void HandleNewClient(IConversationRequester newClient)
    {
        var clientSocketReceiver = (ISocketReceiver)newClient.StreamListener!;
        var clientDecoder = (IPQServerMessageStreamDecoder)clientSocketReceiver.Decoder!;
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<PQSnapshotIdsRequest>()
            .AddDeserializedNotifier(
                new PassThroughDeserializedNotifier<PQSnapshotIdsRequest>($"{nameof(PQSnapshotServer)}.{nameof(OnSnapshotIdsRequest)}"
                    , OnSnapshotIdsRequest));
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<PQSourceTickerInfoRequest>()
            .AddDeserializedNotifier(
                new PassThroughDeserializedNotifier<PQSourceTickerInfoRequest>($"{nameof(PQSnapshotServer)}.{nameof(OnSourceTickerInfoRequest)}"
                    , OnSourceTickerInfoRequest));
        logger.Info($"New PQSnapshot Client Request {newClient}");
    }

    private void OnSnapshotIdsRequest(PQSnapshotIdsRequest snapshotIdsRequest, MessageHeader messageHeader, IConversation cx)
    {
        OnSnapshotRequest?.Invoke((cx as IConversationRequester)!, snapshotIdsRequest);
    }

    private void OnSourceTickerInfoRequest(PQSourceTickerInfoRequest sourceTickerInfoRequest, MessageHeader messageHeader, IConversation cx)
    {
        logger.Info("PQSnapshot Server received request for SourceTickerQuotInfo");
        ReceivedSourceTickerInfoRequest?.Invoke(sourceTickerInfoRequest, (cx as IConversationRequester)!);
    }
}
