#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Construction;

public interface ISocketReceiverFactory
{
    bool HasConversationListener(ConversationType conversationType);
    ISocketReceiver GetConversationListener(ISocketSessionContext transportConversation);

    event Action<ISocketReceiver> ConfigureNewSocketReceiver;
    void RunRegisteredSocketReceiverConfiguration(ISocketReceiver newReceiverSession);
}

public class SocketReceiverFactory : ISocketReceiverFactory
{
    public bool HasConversationListener(ConversationType conversationType) =>
        conversationType == ConversationType.Subscriber
        || conversationType == ConversationType.Requester
        || conversationType == ConversationType.Responder;

    public ISocketReceiver GetConversationListener(ISocketSessionContext transportConversation)
    {
        var newReceiverSession = new SocketReceiver(transportConversation);
        return newReceiverSession;
    }

    public event Action<ISocketReceiver>? ConfigureNewSocketReceiver;

    public void RunRegisteredSocketReceiverConfiguration(ISocketReceiver newReceiverSession)
    {
        ConfigureNewSocketReceiver?.Invoke(newReceiverSession);
    }
}
