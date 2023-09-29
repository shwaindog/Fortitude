using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders
{
    public class TCPRequestResponseResponderBuilder
    {
        private ISocketFactories socketFactories;

        public RequestResponseResponder Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
        {
            ConversationType conversationType = ConversationType.RequestResponseResponder;
            SocketConversationProtocol conversationProtocol = SocketConversationProtocol.TCPAcceptor;

            var socketFactories = SocketFactories;

            SocketSessionContext socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol, 
                socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, socketFactories, serdesFactory);
            socketSessionContext.ConversationDescription += "Responder";

            TCPAcceptorControls tcpAcceptorControls = new TCPAcceptorControls(socketSessionContext);

            return new RequestResponseResponder(socketSessionContext, tcpAcceptorControls);
        }

        public ISocketFactories SocketFactories
        {
            get => socketFactories ?? (socketFactories = Sockets.SocketFactories.GetRealSocketFactories());
            set => socketFactories = value;
        }
    }
}
