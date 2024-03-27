#region

using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.State;
using SocketsAPI = FortitudeIO.Transports.NewSocketAPI;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public sealed class OrxServerMessaging : ConversationResponder, IOrxMessageResponder
{
    private static ISocketFactoryResolver? socketFactories;

    public OrxServerMessaging(ISocketSessionContext socketSessionContext, IAcceptorControls acceptorControls)
        : base(socketSessionContext, acceptorControls)
    {
        RecyclingFactory = new Recycler();
        OrxSerdesFactory = new OrxSerdesFactory(RecyclingFactory);
        DeserializationRepository
            = new OrxStreamDecoderFactory((deserializers) =>
                new OrxMessageStreamDecoder(deserializers), OrxSerdesFactory, RecyclingFactory);
        socketSessionContext.SerdesFactory.StreamDecoderFactory
            = DeserializationRepository;
        SerializationRepository
            = new OrxSerializationRepository(new ConcurrentDictionary<uint, IMessageSerializer>(), OrxSerdesFactory
                , RecyclingFactory);
        socketSessionContext.SerdesFactory.StreamEncoderFactory = SerializationRepository;
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    internal OrxSerdesFactory OrxSerdesFactory { get; }

    public IOrxDeserializationRepository DeserializationRepository { get; }

    public IOrxSerializationRepository SerializationRepository { get; }

    public IRecycler RecyclingFactory { get; }

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

    public static OrxServerMessaging BuildTcpResponder(ISocketTopicConnectionConfig socketConnectionConfig)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketConversationProtocol.TcpAcceptor;

        var socFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.TopicName, socketConnectionConfig, socFactories, serdesFactory);
        socketSessionContext.Name += "Responder";

        var acceptorControls
            = (IAcceptorControls)socFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new OrxServerMessaging(socketSessionContext, acceptorControls);
    }
}
