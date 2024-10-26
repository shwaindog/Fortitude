#region

using FortitudeBusRules.Connectivity.Network;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication.BusRules;

public class PQPricingServerQuoteUpdatePublisher : ConversationPublisher
{
    private static ISocketFactoryResolver? socketFactories;
    private readonly string feedName;

    public PQPricingServerQuoteUpdatePublisher(string feedName, ISocketSessionContext socketSessionContext, IStreamControls streamControls) : base(
        socketSessionContext
        , streamControls) =>
        this.feedName = feedName;

    public static ISocketFactoryResolver SocketFactories
    {
        get =>
            socketFactories
                ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public static PQPricingServerQuoteUpdatePublisher BuildUdpMulticastPublisher(string feedName
        , INetworkTopicConnectionConfig networkConnectionConfig,
        ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Publisher;
        var conversationProtocol = SocketConversationProtocol.UdpPublisher;

        var socFactories = SocketFactories;

        var serdesFactory = new PQServerSerdesRepositoryFactory(PQMessageFlags.Update);

        var socketSessionContext = new BusSocketSessionContext(networkConnectionConfig.TopicName + "Publisher", conversationType, conversationProtocol
            ,
            networkConnectionConfig, socFactories, serdesFactory
            , socketDispatcherResolver);

        var streamControls = new InitiateControls(socketSessionContext);

        return new PQPricingServerQuoteUpdatePublisher(feedName, socketSessionContext, streamControls);
    }
}
