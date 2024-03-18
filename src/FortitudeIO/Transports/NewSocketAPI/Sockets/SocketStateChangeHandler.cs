#region

using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Publishing;
using FortitudeIO.Transports.NewSocketAPI.Receiving;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public interface ISocketConnectivityChanged
{
    Action<SocketSessionState> GetOnConnectionChangedHandler();
}

public class SocketStateChangeHandler : ISocketConnectivityChanged
{
    private static readonly IDictionary<uint, IMessageSerializer> emptyStreamMap
        = new Dictionary<uint, IMessageSerializer>();

    private readonly ISerdesFactory serdesFactory;

    private readonly ISocketReceiverFactory? sockeReceiverFactory;
    private readonly ISocketSenderFactory? socketSenderFactory;
    private readonly ISocketSessionContext socketSessionContext;

    public SocketStateChangeHandler(ISocketSessionContext socketSessionContext)
    {
        this.socketSessionContext = socketSessionContext;
        socketSenderFactory = socketSessionContext.SocketFactories.SocketSenderFactory;
        sockeReceiverFactory = socketSessionContext.SocketFactories.SocketReceiverFactory;
        serdesFactory = socketSessionContext.SerdesFactory;
    }

    public Action<SocketSessionState> GetOnConnectionChangedHandler() => OnConnectionChanged;

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
                        FirstConnect();
                        break;
                    case SocketSessionState.Disconnecting:
                    case SocketSessionState.Disconnected:
                    case SocketSessionState.Reconnecting:
                        FailedToConnect();
                        break;
                }

                break;
            case SocketSessionState.Reconnecting:
                switch (newState)
                {
                    case SocketSessionState.Connected:
                        Reconnected();
                        break;
                    case SocketSessionState.Disconnecting:
                    case SocketSessionState.Disconnected:
                    case SocketSessionState.Reconnecting:
                        FailedToReconnect();
                        break;
                }

                break;
            case SocketSessionState.Connected:
            case SocketSessionState.Disconnecting:
                if (newState == SocketSessionState.Disconnected) Disconnected();
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
            socketSessionContext.SocketDispatcher?.Listener.UnregisterForListen(socketSessionContext.SocketReceiver);
    }

    private void Reconnected() { }

    private void FailedToConnect()
    {
        Disconnected();
    }

    private void FirstConnect()
    {
        var socketCon = socketSessionContext.SocketConnection!;
        if (socketCon.IsConnected &&
            socketSenderFactory!.HasConversationPublisher(socketSessionContext.ConversationType))
            if (socketSessionContext.SocketSender == null)
            {
                var socketSender = socketSenderFactory.GetConversationPublisher(socketSessionContext);
                socketSessionContext.SocketSender = socketSender;
            }

        if (socketCon.IsConnected &&
            sockeReceiverFactory!.HasConversationListener(socketSessionContext.ConversationType))
        {
            if (socketSessionContext.SocketReceiver != null)
                socketSessionContext.SocketDispatcher.Listener.UnregisterForListen(socketSessionContext.SocketReceiver);

            var socketReceiver = sockeReceiverFactory.GetConversationListener(socketSessionContext);
            socketSessionContext.SocketReceiver = socketReceiver;
            socketReceiver.Decoder ??= socketSessionContext.SerdesFactory.StreamDecoderFactory?.Supply();
            socketSessionContext.SocketDispatcher.Listener.RegisterForListen(socketSessionContext.SocketReceiver);
        }
    }
}
