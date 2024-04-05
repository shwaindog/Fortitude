#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
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

    public OrxServerMessaging(ISocketSessionContext socketSessionContext, IAcceptorControls acceptorControls)
        : base(socketSessionContext, acceptorControls)
    {
        var orxSerdesRepoFactory = (IOrxSerdesRepositoryFactory)socketSessionContext.SerdesFactory;

        DeserializationRepository = orxSerdesRepoFactory.MessageDeserializationRepository;
        SerializationRepository = orxSerdesRepoFactory.MessageSerializationRepository;
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public IOrxDeserializationRepository DeserializationRepository { get; }

    public IMessageSerializationRepository SerializationRepository { get; }

    public void Send(ISession client, IVersionedMessage message)
    {
        throw new NotImplementedException();
    }

    public void Send(IConversation client, IVersionedMessage message)
    {
        var clientRequester = (IConversationRequester)client;
        clientRequester.StreamPublisher!.Send(message);
    }

    public void Send(ISocketSessionContext client, IVersionedMessage message)
    {
        client.SocketSender!.Send(message);
    }

    public static OrxServerMessaging BuildTcpResponder(INetworkTopicConnectionConfig networkConnectionConfig)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketConversationProtocol.TcpAcceptor;

        var socFactories = SocketFactories;

        var serdesFactory = new OrxSerdesRepositoryFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, socFactories, serdesFactory);
        socketSessionContext.Name += "Responder";

        var acceptorControls
            = (IAcceptorControls)socFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new OrxServerMessaging(socketSessionContext, acceptorControls);
    }
}
