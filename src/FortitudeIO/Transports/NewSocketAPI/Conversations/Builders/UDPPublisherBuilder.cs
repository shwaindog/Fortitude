using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders
{
    public class UDPPublisherBuilder
    {
        private ISocketFactories socketFactories;

        public PublisherConversation Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
        {
            ConversationType conversationType = ConversationType.Publisher;
            SocketConversationProtocol conversationProtocol = SocketConversationProtocol.UDPPublisher;

            var socketFactories = SocketFactories;

            SocketSessionContext socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol, 
                socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, socketFactories, serdesFactory);
            socketSessionContext.ConversationDescription += "Publisher";

            InitiateControls initiateControls = new InitiateControls(socketSessionContext);

            return new PublisherConversation(socketSessionContext, initiateControls);
        }

        public ISocketFactories SocketFactories
        {
            get => socketFactories ?? (socketFactories = Sockets.SocketFactories.GetRealSocketFactories());
            set => socketFactories = value;
        }
    }
}
