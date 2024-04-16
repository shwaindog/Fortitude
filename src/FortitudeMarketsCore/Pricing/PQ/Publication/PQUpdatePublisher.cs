#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQUpdateServer : IConversationPublisher { }

public sealed class PQUpdatePublisher : ConversationPublisher, IPQUpdateServer
{
    private static ISocketFactoryResolver? socketFactories;

    public PQUpdatePublisher(ISocketSessionContext socketSessionContext, IStreamControls streamControls) : base(socketSessionContext
        , streamControls) { }

    public static ISocketFactoryResolver SocketFactories
    {
        get =>
            socketFactories
                ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public static PQUpdatePublisher BuildUdpMulticastPublisher(INetworkTopicConnectionConfig networkConnectionConfig,
        ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Publisher;
        var conversationProtocol = SocketConversationProtocol.UdpPublisher;

        var socFactories = SocketFactories;

        var serdesFactory = new PQServerSerdesRepositoryFactory(PQMessageFlags.Update);

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, socFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Publisher";

        var streamControls = new InitiateControls(socketSessionContext);

        return new PQUpdatePublisher(socketSessionContext, streamControls);
    }
}
