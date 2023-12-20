#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
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
#pragma warning disable CS0414 // Field is assigned but its value is never used
    private ConversationType conversationType = ConversationType.Publisher;
#pragma warning restore CS0414 // Field is assigned but its value is never used

    public SocketSenderFactory(IDirectOSNetworkingApi directOSNetworkingApi) =>
        this.directOSNetworkingApi = directOSNetworkingApi;

    public bool HasConversationPublisher(ConversationType conversationType) =>
        conversationType is ConversationType.Publisher or ConversationType.RequestResponseRequester
            or ConversationType.RequestResponseResponder;

    public SocketSender GetConversationPublisher(ISocketSessionContext socketSocketSessionContext) =>
        new(socketSocketSessionContext);
}
