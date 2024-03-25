#region

using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Controls;

public interface IStreamControlsFactory
{
    IStreamControls ResolveStreamControls(ISocketSessionContext socketSocketSessionContext);
}

public class StreamControlsFactory : IStreamControlsFactory
{
    public IStreamControls ResolveStreamControls(ISocketSessionContext socketSocketSessionContext)
    {
        var topicConnectionConfig = socketSocketSessionContext.SocketTopicConnectionConfig;
        if (topicConnectionConfig.ConversationProtocol == SocketConversationProtocol.TcpAcceptor)
            return new TcpAcceptorControls(socketSocketSessionContext);
        return new InitiateControls(socketSocketSessionContext);
    }
}
