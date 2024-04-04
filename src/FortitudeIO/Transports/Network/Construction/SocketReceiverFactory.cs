#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Construction;

public interface ISocketReceiverFactory
{
    bool HasConversationListener(ConversationType conversationType);
    SocketReceiver GetConversationListener(ISocketSessionContext transportConversation);
}

public class SocketReceiverFactory : ISocketReceiverFactory
{
    public bool HasConversationListener(ConversationType conversationType) =>
        conversationType == ConversationType.Subscriber
        || conversationType == ConversationType.Requester
        || conversationType == ConversationType.Responder;

    public SocketReceiver GetConversationListener(ISocketSessionContext transportConversation) => new(transportConversation);
}
