#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeIO.Transports.Network.State;

public interface ISocketConnectivityChanged
{
    CloseReason CloseReason { get; set; }
    string? ReasonText { get; set; }
    Action<SocketSessionState> GetOnConnectionChangedHandler();
}

public class SocketStateChangeHandler : ISocketConnectivityChanged
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SocketStateChangeHandler));
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

    public CloseReason CloseReason { get; set; }
    public string? ReasonText { get; set; }

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
                        NewToConnectedHandler();
                        break;
                    case SocketSessionState.Disconnecting:
                    case SocketSessionState.Disconnected:
                    case SocketSessionState.Reconnecting:
                        NewToFailedHandler();
                        break;
                }

                break;
            case SocketSessionState.Reconnecting:
                switch (newState)
                {
                    case SocketSessionState.Connected:
                        ReconnectingToConnectedHandler();
                        break;
                    case SocketSessionState.Disconnecting:
                    case SocketSessionState.Disconnected:
                    case SocketSessionState.Reconnecting:
                        ReconnectingToFailedHandler();
                        break;
                }

                break;
            case SocketSessionState.Connected:
                if (newState == SocketSessionState.Disconnecting) ConnectedToDisconnectingHandler();
                if (newState == SocketSessionState.Disconnected) ToDisconnectedHandler();
                break;
            case SocketSessionState.Disconnecting:
                if (newState == SocketSessionState.Disconnected) ToDisconnectedHandler();
                break;
        }
    }

    private void ConnectedToDisconnectingHandler()
    {
        Logger.Info("Starting graceful disconnect sequence for {0}", socketSessionContext.Name);
    }

    private void ReconnectingToFailedHandler()
    {
        ToDisconnectedHandler();
    }

    private void ToDisconnectedHandler()
    {
        var socketSender = socketSessionContext.SocketSender;
        var socketReceiver = socketSessionContext.SocketReceiver;
        var canSendExpectClose = CloseReason != CloseReason.RemoteDisconnecting && socketSender is { CanSend: true };

        if (socketReceiver == null && !canSendExpectClose)
        {
            socketSessionContext.SocketConnection?.OSSocket?.Close();
            Logger.Info("Socket.Close has been called on {0}", socketSessionContext.Name);
            socketSessionContext.SetDisconnected();
            return;
        }

        socketSender?.SetCloseReason(CloseReason, ReasonText);

        if (socketReceiver != null)
        {
            socketReceiver.AttemptCloseSocketOnListenerRemoval = true;
            socketSessionContext.SocketDispatcher.Listener?.UnregisterForListen(socketReceiver);
            socketReceiver.ExpectSessionCloseMessage = null;
            socketSessionContext.SocketReceiver = null;
            // will call socketSender in SendExpectSessionCloseMessageAndClose in socketReceiver.UnregisterHandler
        }
        else if (socketSender != null)
        {
            socketSender!.SendExpectSessionCloseMessageAndClose();
        }
    }

    private void ReconnectingToConnectedHandler()
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
                socketReceiver.Decoder ??= socketSessionContext.SerdesFactory.MessageStreamDecoderFactory(socketSessionContext.Name)
                    ?.Supply(socketSessionContext.Name);
                socketSessionContext.SocketReceiver = socketReceiver;
                socketReceiverFactory.RunRegisteredSocketReceiverConfiguration(socketReceiver);
            }
            else
            {
                socketSessionContext.SocketDispatcher.Listener?.UnregisterForListen(socketSessionContext.SocketReceiver);
                socketSessionContext.SocketReceiver.Socket = socketCon.OSSocket;
            }
        }
    }

    private void NewToFailedHandler()
    {
        ToDisconnectedHandler();
    }

    private void NewToConnectedHandler()
    {
        CreateConversationSenderAndReceivers();
    }
}
