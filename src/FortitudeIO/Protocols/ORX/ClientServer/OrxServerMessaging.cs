#region

using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using SocketsAPI = FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public sealed class OrxServerMessaging : ConversationResponder, IOrxPublisher
{
    private static SocketsAPI.ISocketFactories? socketFactories;
    private readonly SocketsAPI.SocketStreamMessageEncoderFactory socketEncoderFactory;

    public OrxServerMessaging(SocketsAPI.ISocketSessionContext socketSessionContext, IAcceptorControls acceptorControls)
        : base(socketSessionContext, acceptorControls)
    {
        RecyclingFactory = new Recycler();
        OrxSerializationRepository = new OrxSerializationRepository(RecyclingFactory);
        DeserializationRepository
            = new OrxStreamDecoderFactory((deserializers) =>
                new OrxMessageStreamDecoder(deserializers), OrxSerializationRepository, RecyclingFactory);
        socketSessionContext.SerdesFactory.StreamDecoderFactory
            = DeserializationRepository;
        socketEncoderFactory
            = new SocketsAPI.SocketStreamMessageEncoderFactory(new ConcurrentDictionary<uint, IMessageSerializer>());
        socketSessionContext.SerdesFactory.StreamEncoderFactory = socketEncoderFactory;
    }

    public static SocketsAPI.ISocketFactories SocketFactories
    {
        get => socketFactories ??= SocketsAPI.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    internal OrxSerializationRepository OrxSerializationRepository { get; }

    public IOrxDeserializationRepository DeserializationRepository { get; }

    public IRecycler RecyclingFactory { get; }

    public void Send(ISession client, IVersionedMessage message)
    {
        throw new NotImplementedException();
    }

    public void Send(SocketsAPI.ISocketSessionContext client, IVersionedMessage message)
    {
        client.SocketSender!.Send(message);
    }

    public void Send(IConversation client, IVersionedMessage message)
    {
        var clientRequester = (IConversationRequester)client;
        clientRequester.ConversationPublisher!.Send(message);
    }

    public void RegisterSerializer<T>() where T : class, IVersionedMessage, new()
    {
        var instanceOfTypeToSerialize = RecyclingFactory.Borrow<T>();
        var serializer = OrxSerializationRepository.GetSerializer<T>(instanceOfTypeToSerialize.MessageId);
        socketEncoderFactory.RegisterMessageSerializer(instanceOfTypeToSerialize.MessageId, serializer);
        instanceOfTypeToSerialize.DecrementRefCount();
    }

    public static OrxServerMessaging BuildTcpResponder(ISocketConnectionConfig socketConnectionConfig)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketsAPI.SocketConversationProtocol.TcpAcceptor;

        var socFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketsAPI.SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, socFactories, serdesFactory);
        socketSessionContext.Name += "Responder";

        var tcpAcceptorControls = new TcpAcceptorControls(socketSessionContext);

        return new OrxServerMessaging(socketSessionContext, tcpAcceptorControls);
    }
}
