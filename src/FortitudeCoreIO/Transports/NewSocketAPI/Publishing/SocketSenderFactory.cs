#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Publishing;

public interface ISocketSenderFactory
{
    bool HasConversationPublisher(ConversationType conversationType);
    SocketSender GetConversationPublisher(ISocketSessionContext socketSocketSessionContext);
}

public class SocketSenderFactory : ISocketSenderFactory
{
    private readonly IDirectOSNetworkingApi directOSNetworkingApi;
    private ConversationType conversationType = ConversationType.Publisher;

    public SocketSenderFactory(IDirectOSNetworkingApi directOSNetworkingApi) =>
        this.directOSNetworkingApi = directOSNetworkingApi;

    public bool HasConversationPublisher(ConversationType conversationType) =>
        conversationType is ConversationType.Publisher or ConversationType.RequestResponseRequester
            or ConversationType.RequestResponseResponder;

    public SocketSender GetConversationPublisher(ISocketSessionContext socketSocketSessionContext) =>
        new(socketSocketSessionContext);
}
