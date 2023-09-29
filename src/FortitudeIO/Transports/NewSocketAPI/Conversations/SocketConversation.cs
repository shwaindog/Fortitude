using System;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations
{
    public class SocketConversation : ISocketConversation
    {
        protected readonly ISocketSessionContext SocketSessionContext;
        private readonly IStreamControls initiateControls;

        public SocketConversation(ISocketSessionContext socketSessionContext, IStreamControls initiateControls)
        {
            this.SocketSessionContext = socketSessionContext;
            this.initiateControls = initiateControls;
        }

        public event Action Started
        {
            add => SocketSessionContext.Connected += value;
            remove => SocketSessionContext.Connected -= value;
        }

        public event Action Stopped
        {
            add => SocketSessionContext.Disconnected += value;
            remove => SocketSessionContext.Disconnected -= value;
        }

        public void Start()
        {
            initiateControls.Connect();
        }

        public void Stop()
        {
            initiateControls.Disconnect();
        }

        public event Action<string, int> Error
        {
            add => SocketSessionContext.Error += value;
            remove => SocketSessionContext.Error -= value;
        }

        public event Action Connected
        {
            add => SocketSessionContext.Connected += value;
            remove => SocketSessionContext.Connected -= value;
        }

        public event Action Disconnected
        {
            add => SocketSessionContext.Disconnected += value;
            remove => SocketSessionContext.Disconnected -= value;
        }

        public ConversationType ConversationType { get; set; }

        public string ConversationDescription
        {
            get => SocketSessionContext.ConversationDescription;
            set => SocketSessionContext.ConversationDescription = value;
        }

        public ConversationState ConversationState => SocketSessionContext.ConversationState;

        public IConversationListener ConversationListener => SocketSessionContext.SocketReceiver;

        public IConversationPublisher ConversationPublisher => SocketSessionContext.SocketSender;

        public void Connect() => initiateControls.Connect();

        public void Disconnect() => initiateControls.Disconnect();

        public void StartMessaging() => initiateControls.StartMessaging();

        public void StopMessaging() => initiateControls.StopMessaging();
    }
}