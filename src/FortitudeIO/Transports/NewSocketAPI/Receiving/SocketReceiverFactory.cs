#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Receiving;

public interface ISocketReceiverFactory
{
    bool hasConversationListener(ConversationType conversationType);
    SocketReceiver getConversationListener(ISocketSessionContext transportConversation);
}

public class SocketReceiverFactory : ISocketReceiverFactory
{
    private readonly int wholeMessagesPerReceive;
    private readonly bool zeroBytesReadIsDisconnection;

    public SocketReceiverFactory(int wholeMessagesPerReceive = 1, bool zeroBytesReadIsDisconnection = true)
    {
        this.wholeMessagesPerReceive = wholeMessagesPerReceive;
        this.zeroBytesReadIsDisconnection = zeroBytesReadIsDisconnection;
    }

    public bool hasConversationListener(ConversationType conversationType) =>
        conversationType == ConversationType.Subscriber
        || conversationType == ConversationType.RequestResponseRequester
        || conversationType == ConversationType.RequestResponseResponder;

    public SocketReceiver getConversationListener(ISocketSessionContext transportConversation) =>
        new(transportConversation, wholeMessagesPerReceive, zeroBytesReadIsDisconnection);
}
