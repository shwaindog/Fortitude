#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using SocketsAPI = FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public sealed class PQUpdatePublisher : ConversationPublisher, IPQUpdateServer
{
    private static readonly PQServerSerializationRepository UpdateSerializationRepository = new(PQFeedType.Update);
    private static SocketsAPI.ISocketFactories? socketFactories;

    public PQUpdatePublisher(SocketsAPI.ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls)
        : base(socketSessionContext, initiateControls) =>
        socketSessionContext.SerdesFactory.StreamEncoderFactory = UpdateSerializationRepository;

    public static SocketsAPI.ISocketFactories SocketFactories
    {
        get => socketFactories ??= SocketsAPI.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public static PQUpdatePublisher BuildUdpMulticastPublisher(ISocketTopicConnectionConfig socketConnectionConfig)
    {
        var conversationType = ConversationType.Publisher;
        var conversationProtocol = SocketsAPI.SocketConversationProtocol.UdpPublisher;

        var socFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketsAPI.SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.TopicName, socketConnectionConfig, socFactories, serdesFactory);
        socketSessionContext.Name += "Publisher";

        var initiateControls = new InitiateControls(socketSessionContext);

        return new PQUpdatePublisher(socketSessionContext, initiateControls);
    }
}
