#region

using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Construction;

public interface IStreamControlsFactory
{
    IStreamControls ResolveStreamControls(ISocketSessionContext socketSocketSessionContext);
}

public class StreamControlsFactory : IStreamControlsFactory
{
    public IStreamControls ResolveStreamControls(ISocketSessionContext socketSocketSessionContext)
    {
        var topicConnectionConfig = socketSocketSessionContext.NetworkTopicConnectionConfig;
        if (topicConnectionConfig.ConversationProtocol == SocketConversationProtocol.TcpAcceptor)
            return new TcpAcceptorControls(socketSocketSessionContext);
        return new InitiateControls(socketSocketSessionContext);
    }
}
