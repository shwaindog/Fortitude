#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQUpdateClient : IConversationSubscriber
{
    IPQClientQuoteDeserializerRepository DeserializerRepository { get; }
}

public sealed class PQUpdateClient : ConversationSubscriber, IPQUpdateClient
{
    private static ISocketFactoryResolver? socketFactories;
    private IPQClientQuoteDeserializerRepository? deserializerRepository;

    public PQUpdateClient(ISocketSessionContext socketSessionContext, IStreamControls streamControls)
        : base(socketSessionContext, streamControls)
    {
        socketSessionContext.SocketReceiverUpdated += () =>
        {
            DeserializerRepository
                = (IPQClientQuoteDeserializerRepository)socketSessionContext.SerdesFactory
                    .MessageDeserializationRepository(socketSessionContext.Name);
        };
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public IPQClientQuoteDeserializerRepository DeserializerRepository
    {
        get => deserializerRepository ?? throw new Exception("Can not access DeserializerRepository until session has connected");
        private set => deserializerRepository = value;
    }

    public static PQUpdateClient BuildUdpSubscriber(INetworkTopicConnectionConfig networkConnectionConfig
        , ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Subscriber;
        var conversationProtocol = SocketConversationProtocol.UdpSubscriber;

        var sockFactories = SocketFactories;

        var serdesFactory = new PQClientClientSerdesRepositoryFactory();

        var socketSessionContext = new SocketSessionContext(networkConnectionConfig.TopicName + "Subscriber", conversationType, conversationProtocol,
            networkConnectionConfig, sockFactories
            , serdesFactory, socketDispatcherResolver);

        var streamControls = sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQUpdateClient(socketSessionContext, streamControls);
    }
}
