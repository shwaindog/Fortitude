using System.Net.Sockets;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders
{
    public class UDPSubscriberBuilder
    {
        private ISocketFactories socketFactories;

        public SubscriberConversation Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
        {
            ConversationType conversationType = ConversationType.Subscriber;
            SocketConversationProtocol conversationProtocol = SocketConversationProtocol.UDPSubscriber;

            var socketFactories = SocketFactories;

            SocketSessionContext socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol, 
                socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, socketFactories, serdesFactory);
            socketSessionContext.ConversationDescription += "Subscriber";

            InitiateControls initiateControls = new InitiateControls(socketSessionContext);

            return new SubscriberConversation(socketSessionContext, initiateControls);
        }

        public ISocketFactories SocketFactories
        {
            get => socketFactories ?? (socketFactories = Sockets.SocketFactories.GetRealSocketFactories());
            set => socketFactories = value;
        }
    }
}
