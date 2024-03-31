#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Trading;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Subscription;

public sealed class OrxTradingClientMessaging : OrxClientMessaging
{
    private int nextSequence;

    public OrxTradingClientMessaging(ISocketSessionContext socketSessionContext, IInitiateControls initiateControls)
        : base(socketSessionContext, initiateControls)
    {
        socketSessionContext.Disconnected += () => nextSequence = 0;
    }

    public static OrxTradingClientMessaging BuildTradingClient(INetworkTopicConnectionConfig networkConnectionConfig
        , ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, sockFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Requester";

        var initControls
            = (IInitiateControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new OrxTradingClientMessaging(socketSessionContext, initControls);
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
