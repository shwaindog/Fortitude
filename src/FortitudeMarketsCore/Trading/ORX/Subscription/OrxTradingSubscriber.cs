#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
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

    public static OrxTradingClientMessaging BuildTradingClient(ISocketConnectionConfig socketConnectionConfig
        , ISocketDispatcherResolver socketDispatcherResolver)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, sockFactories, serdesFactory
            , socketDispatcherResolver.Resolve(socketConnectionConfig));
        socketSessionContext.Name += "Requester";

        var clientInitiateControls = new InitiateControls(socketSessionContext);

        return new OrxTradingClientMessaging(socketSessionContext, clientInitiateControls);
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
