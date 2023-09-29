using System;
using System.Collections.Generic;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Publishing;
using FortitudeIO.Transports.NewSocketAPI.Receiving;

namespace FortitudeIO.Transports.NewSocketAPI.Sockets
{
    public interface ISocketConnectivityChanged
    {
        Action<SocketSessionState> GetOnConnectionChangedHandler();
    }

    public class SocketStateChangeHandler : ISocketConnectivityChanged
    {
        private readonly ISocketSessionContext socketSessionContext;
        private ISocketSenderFactory socketSenderFactory;
        private ISocketReceiverFactory sockeReceiverFactory;
        private ISerdesFactory serdesFactory;
        private static IDictionary<uint, IBinarySerializer> emptyStreamMap = new Dictionary<uint, IBinarySerializer>();

        public SocketStateChangeHandler(ISocketSessionContext socketSessionContext)
        {
            this.socketSessionContext = socketSessionContext;
            socketSenderFactory = socketSessionContext.SocketFactories.SocketSenderFactory;
            sockeReceiverFactory = socketSessionContext.SocketFactories.SocketReceiverFactory;
            serdesFactory = socketSessionContext.SerdesFactory;
        }

        public Action<SocketSessionState> GetOnConnectionChangedHandler()
        {
            return OnConnectionChanged;
        }
        
        protected virtual void OnConnectionChanged(SocketSessionState oldSessionState)
        {
            var newState = socketSessionContext.SocketSessionState;
            switch (oldSessionState)
            {
                case SocketSessionState.Connecting:
                case SocketSessionState.New:
                    switch (newState)
                    {
                        case SocketSessionState.Connected:
                            FirstConnect(); break;
                        case SocketSessionState.Disconnecting:
                        case SocketSessionState.Disconnected:
                        case SocketSessionState.Reconnecting:
                            FailedToConnect(); break;
                    } break;
                case SocketSessionState.Reconnecting:
                    switch (newState)
                    {
                        case SocketSessionState.Connected:
                            Reconnected(); break;
                        case SocketSessionState.Disconnecting:
                        case SocketSessionState.Disconnected:
                        case SocketSessionState.Reconnecting:
                            FailedToReconnect(); break;
                    }
                    break;
                case SocketSessionState.Connected:
                case SocketSessionState.Disconnecting:
                    if (newState == SocketSessionState.Disconnected)
                    {
                        Disconnected();
                    }
                    break;
            }
        }

        private void FailedToReconnect()
        {
            Disconnected();
        }

        private void Disconnected()
        {
            if (socketSessionContext.SocketReceiver != null)
            {
                socketSessionContext.SocketDispatcher?.Listener.UnregisterForListen(socketSessionContext.SocketReceiver);
            }
        }

        private void Reconnected()
        {
            
        }

        void FailedToConnect()
        {
            Disconnected();
        }

        private void FirstConnect()
        {
            var socketCon = socketSessionContext.SocketConnection;
            if (socketCon.IsConnected && socketSenderFactory.hasConversationPublisher(socketSessionContext.ConversationType))
            {
                if (socketSessionContext.SocketSender == null)
                {
                    var socketSender = socketSenderFactory.getConversationPublisher(socketSessionContext);
                    var streamSerializers = serdesFactory?.StreamSerializers ?? emptyStreamMap;
                    foreach (var streamSerializerEntry in streamSerializers)
                    {
                        socketSender.RegisterSerializer(streamSerializerEntry.Key, streamSerializerEntry.Value);
                    }
                    socketSessionContext.SocketSender = socketSender;
                }
            }

            if (socketCon.IsConnected && sockeReceiverFactory.hasConversationListener(socketSessionContext.ConversationType))
            {
                if (socketSessionContext.SocketReceiver != null)
                {
                    socketSessionContext.SocketDispatcher.Listener.UnregisterForListen(socketSessionContext.SocketReceiver);
                }

                var socketReceiver = sockeReceiverFactory.getConversationListener(socketSessionContext);
                socketSessionContext.SocketReceiver = socketReceiver;
                socketReceiver.Decoder = socketReceiver.DecoderFactory.Supply();
                socketSessionContext.SocketDispatcher.Listener.RegisterForListen(socketSessionContext.SocketReceiver);
            }
        }
    }
}
