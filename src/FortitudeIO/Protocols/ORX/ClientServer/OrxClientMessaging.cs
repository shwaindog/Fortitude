#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public class OrxClientMessaging : ConversationRequester, IOrxClientRequester
{
    private static ISocketFactoryResolver? socketFactories;

    protected readonly object SyncLock = new();

    private IOrxResponderStreamDecoder? messageStreamDecoder;

    protected OrxClientMessaging(ISocketSessionContext socketSessionContext, IInitiateControls initiateControls)
        : base(socketSessionContext, initiateControls)
    {
        var orxSerdesRepoFactory = (IOrxSerdesRepositoryFactory)socketSessionContext.SerdesFactory;

        SerializationRepository = orxSerdesRepoFactory.MessageSerializationRepository;

        socketSessionContext.SocketReceiverUpdated += () =>
        {
            messageStreamDecoder = (IOrxResponderStreamDecoder)socketSessionContext.SocketReceiver?.Decoder!;
        };
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }


    public IOrxDeserializationRepository DeserializationRepository => messageStreamDecoder?.MessageDeserializationRepository!;

    public IMessageSerializationRepository SerializationRepository { get; }


    public static OrxClientMessaging BuildTcpRequester(INetworkTopicConnectionConfig networkTopicConnectionConfig
        , ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new OrxSerdesRepositoryFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkTopicConnectionConfig.TopicName, networkTopicConnectionConfig, sockFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Requester";

        var initControls
            = (IInitiateControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new OrxClientMessaging(socketSessionContext, initControls);
    }
}
