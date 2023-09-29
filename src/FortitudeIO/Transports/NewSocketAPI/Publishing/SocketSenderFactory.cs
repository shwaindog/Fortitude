using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Publishing
{
    public interface ISocketSenderFactory
    {
        bool hasConversationPublisher(ConversationType conversationType);
        SocketSender getConversationPublisher(ISocketSessionContext socketSocketSessionContext);
    }

    public class SocketSenderFactory : ISocketSenderFactory
    {
        private readonly IDirectOSNetworkingApi directOSNetworkingApi;
        private ConversationType conversationType;

        public SocketSenderFactory(IDirectOSNetworkingApi directOSNetworkingApi)
        {
            this.directOSNetworkingApi = directOSNetworkingApi;
        }

        public bool hasConversationPublisher(ConversationType conversationType)
        {
            return conversationType == ConversationType.Publisher
                   || conversationType == ConversationType.RequestResponseRequester
                   || conversationType == ConversationType.RequestResponseResponder;
        }

        public SocketSender getConversationPublisher(ISocketSessionContext socketSocketSessionContext)
        {
            return new SocketSender(socketSocketSessionContext);
        }
    }
}
