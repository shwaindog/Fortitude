#region

using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Construction;

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
