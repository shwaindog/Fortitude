using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Receiving
{
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

        public bool hasConversationListener(ConversationType conversationType)
        {
            return conversationType == ConversationType.Subscriber
                    || conversationType == ConversationType.RequestResponseRequester
                    || conversationType == ConversationType.RequestResponseResponder;
        }

        public SocketReceiver getConversationListener(ISocketSessionContext transportConversation)
        {
            return new SocketReceiver(transportConversation, wholeMessagesPerReceive, zeroBytesReadIsDisconnection);
        }
    }
}
