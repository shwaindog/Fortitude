#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.State;
using SocketsAPI = FortitudeIO.Transports.Network;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public sealed class OrxServerMessaging : ConversationResponder, IOrxMessageResponder
{
    private static ISocketFactoryResolver? socketFactories;

    private IOrxResponderStreamDecoder? messageStreamDecoder;

    public OrxServerMessaging(ISocketSessionContext socketSessionContext, IAcceptorControls acceptorControls)
        : base(socketSessionContext, acceptorControls)
    {
        var orxSerdesRepoFactory = (IOrxSerdesRepositoryFactory)socketSessionContext.SerdesFactory;

        SerializationRepository = orxSerdesRepoFactory.MessageSerializationRepository;

        socketSessionContext.SocketReceiverUpdated += () =>
        {
            messageStreamDecoder = (IOrxResponderStreamDecoder)socketSessionContext.SocketReceiver!.Decoder!;
        };
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public IConversationDeserializationRepository DeserializationRepository => messageStreamDecoder!.MessageDeserializationRepository;

    public IMessageSerializationRepository SerializationRepository { get; }

    public static OrxServerMessaging BuildTcpResponder(INetworkTopicConnectionConfig networkConnectionConfig)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketConversationProtocol.TcpAcceptor;

        var socFactories = SocketFactories;

        var serdesFactory = new OrxSerdesRepositoryFactory();

        var socketSessionContext = new SocketSessionContext(networkConnectionConfig.TopicName + "Responder", conversationType, conversationProtocol,
            networkConnectionConfig, socFactories, serdesFactory);

        var acceptorControls
            = (IAcceptorControls)socFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new OrxServerMessaging(socketSessionContext, acceptorControls);
    }
}
