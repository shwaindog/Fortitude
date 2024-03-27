#region

using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public class OrxClientMessaging : ConversationRequester, IOrxMessageRequester
{
    private static ISocketFactoryResolver? socketFactories;

    protected readonly object SyncLock = new();

    protected OrxClientMessaging(ISocketSessionContext socketSessionContext, IInitiateControls initiateControls)
        : base(socketSessionContext, initiateControls)
    {
        RecyclingFactory = new Recycler();
        OrxSerdesFactory = new OrxSerdesFactory(RecyclingFactory);
        DeserializationRepository
            = new OrxStreamDecoderFactory((deserializers) =>
                new OrxMessageStreamDecoder(deserializers), OrxSerdesFactory, RecyclingFactory);
        socketSessionContext.SerdesFactory.StreamDecoderFactory = DeserializationRepository;
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


    public static OrxClientMessaging BuildTcpRequester(ISocketTopicConnectionConfig socketTopicConnectionConfig
        , ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketTopicConnectionConfig.TopicName, socketTopicConnectionConfig, sockFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Requester";

        var initControls
            = (IInitiateControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new OrxClientMessaging(socketSessionContext, initControls);
    }
}
