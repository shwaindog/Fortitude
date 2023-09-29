using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders
{
    public class TCPRequestResponseRequesterBuilder
    {
        private ISocketFactories socketFactories;

        public RequestResponseRequester Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
        {
            ConversationType conversationType = ConversationType.RequestResponseRequester;
            SocketConversationProtocol conversationProtocol = SocketConversationProtocol.TCPClient;

            var sockFactories = SocketFactories;

            SocketSessionContext socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol, 
                socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, sockFactories, serdesFactory);
            socketSessionContext.ConversationDescription += "Requester";

            InitiateControls initiateControls = new InitiateControls(socketSessionContext);

            return new RequestResponseRequester(socketSessionContext, initiateControls);
        }

        public ISocketFactories SocketFactories
        {
            get => socketFactories ?? (socketFactories = Sockets.SocketFactories.GetRealSocketFactories());
            set => socketFactories = value;
        }
    }
}
