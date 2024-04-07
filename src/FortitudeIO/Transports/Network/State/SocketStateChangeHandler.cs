#region

using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeIO.Transports.Network.State;

public interface ISocketConnectivityChanged
{
    Action<SocketSessionState> GetOnConnectionChangedHandler();
}

public class SocketStateChangeHandler : ISocketConnectivityChanged
{
    private readonly ISocketReceiverFactory? socketReceiverFactory;
    private readonly ISocketSenderFactory? socketSenderFactory;
    private readonly ISocketSessionContext socketSessionContext;

    public SocketStateChangeHandler(ISocketSessionContext socketSessionContext)
    {
        this.socketSessionContext = socketSessionContext;
        socketSenderFactory = socketSessionContext.SocketFactoryResolver.SocketSenderFactory;
        socketReceiverFactory = socketSessionContext.SocketFactoryResolver.SocketReceiverFactory;
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

    private void Disconnected() { }

    private void Reconnected()
    {
        CreateConversationSenderAndReceivers();
    }

    private void CreateConversationSenderAndReceivers()
    {
        var socketCon = socketSessionContext.SocketConnection!;
        if (socketCon.IsConnected &&
            socketSenderFactory!.HasConversationPublisher(socketSessionContext.ConversationType))
            if (socketSessionContext.SocketSender == null)
            {
                var socketSender = socketSenderFactory.GetConversationPublisher(socketSessionContext);
                socketSessionContext.SocketSender = socketSender;
            }
            else
            {
                socketSessionContext.SocketSender.Socket = socketCon.OSSocket;
            }

        if (socketCon.IsConnected &&
            socketReceiverFactory!.HasConversationListener(socketSessionContext.ConversationType))
        {
            if (socketSessionContext.SocketReceiver == null)
            {
                var socketReceiver = socketReceiverFactory.GetConversationListener(socketSessionContext);
                socketReceiver.Decoder ??= socketSessionContext.SerdesFactory.MessageStreamDecoderFactory?.Supply();
                socketSessionContext.SocketReceiver = socketReceiver;
            }
            else
            {
                socketSessionContext.SocketDispatcher.Listener.UnregisterForListen(socketSessionContext.SocketReceiver);
                socketSessionContext.SocketReceiver.Socket = socketCon.OSSocket;
            }
        }
    }

    private void FailedToConnect()
    {
        Disconnected();
    }

    private void FirstConnect()
    {
        CreateConversationSenderAndReceivers();
    }
}
