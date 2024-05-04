#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeBusRules.Connectivity.Network;

public class BusTcpAcceptorControls : TcpAcceptorControls
{
    public BusTcpAcceptorControls(ISocketSessionContext acceptorSocketSessionContext) : base(acceptorSocketSessionContext) { }

    protected override ISocketSessionContext CreateClientSocketSessionContext(INetworkTopicConnectionConfig clientNetworkTopicConnectionConfig) =>
        new BusSocketSessionContext(clientNetworkTopicConnectionConfig.TopicName + "AcceptedClient"
            , ConversationType.Requester,
            SocketConversationProtocol.TcpClient, clientNetworkTopicConnectionConfig,
            SocketSessionContext.SocketFactoryResolver, SocketSessionContext.SerdesFactory);

    protected override void StartClientAcceptedSession(ConversationRequester clientConversation)
    {
        // do nothing all bus rules clients will start the session when the rule has launched and configured it self.
    }
}
