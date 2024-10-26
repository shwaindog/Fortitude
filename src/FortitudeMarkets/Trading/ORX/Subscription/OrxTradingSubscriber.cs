#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Trading;

#endregion

namespace FortitudeMarkets.Trading.ORX.Subscription;

public sealed class OrxTradingClientMessaging : OrxClientMessaging
{
    private int nextSequence;

    public OrxTradingClientMessaging(ISocketSessionContext socketSessionContext, IStreamControls streamControls)
        : base(socketSessionContext, streamControls)
    {
        socketSessionContext.Disconnected += () => nextSequence = 0;
    }

    public static OrxTradingClientMessaging BuildTradingClient(INetworkTopicConnectionConfig networkConnectionConfig
        , ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new OrxSerdesRepositoryFactory();

        var socketSessionContext = new SocketSessionContext(networkConnectionConfig.TopicName + "Requester", conversationType, conversationProtocol,
            networkConnectionConfig, sockFactories, serdesFactory
            , socketDispatcherResolver);

        var streamControls = sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new OrxTradingClientMessaging(socketSessionContext, streamControls);
    }

    public override void Send(IVersionedMessage versionedMessage)
    {
        lock (SyncLock)
        {
            if (versionedMessage is ITradingMessage tradingMessage) tradingMessage.SequenceNumber = nextSequence++;
            base.Send(versionedMessage);
        }
    }
}
