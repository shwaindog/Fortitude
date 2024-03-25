#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Transports.Sockets.Subscription;

public abstract class InitiateControls : SocketStreamSubscriber, ISocketSubscriber
{
    private readonly object connSync = new();
    private readonly IConnectionConfig? initialConnectionConfig;
    protected readonly IOSNetworkingController NetworkingController;
    protected readonly IOSParallelController ParallelController;
    private IConnectionConfig? connectedConnectionConfig;

    private volatile bool connecting;

    private IIntraOSThreadSignal? triggerConnectNowSignal;

    protected InitiateControls(IFLogger logger, ISocketDispatcher dispatcher,
        IOSNetworkingController networkingController, IConnectionConfig? connectionConfig,
        string sessionDescription, int wholeMessagesPerReceive,
        IMap<uint, IMessageDeserializer>? serializerCache = null)
        : base(logger, dispatcher, sessionDescription, wholeMessagesPerReceive, serializerCache)
    {
        ParallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
        NetworkingController = networkingController;
        initialConnectionConfig = connectionConfig;
        connectionConfig?.Updates?.Subscribe(sc =>
        {
            if (sc.EventType == EventType.Updated ||
                sc.EventType == EventType.Deleted)
                Disconnect(false);
            if (sc.EventType == EventType.Updated)
                Connect();
        });
    }

    protected virtual ISocketSessionConnection? Connector { get; set; }

    public string Host => initialConnectionConfig!.Hostname;
    public int Port => initialConnectionConfig!.Port;

    public ISocketSessionConnection RegisterConnector(IOSSocket socket)
    {
        socket.ReceiveBufferSize = RecvBufferSize;
        var socketSessionConnection = new SocketSessionConnection(
            new SocketSessionReceiver(socket, NetworkingController.DirectOSNetworkingApi, GetDecoder(deserializers),
                SessionDescription, WholeMessagesPerReceive, ZeroBytesReadIsDisconnection),
            new SocketSessionSender(socket, NetworkingController.DirectOSNetworkingApi, SessionDescription),
            OnCxError);
        Dispatcher.Listener.RegisterForListen(socketSessionConnection);
        return socketSessionConnection;
    }

    public bool IsConnected =>
        Connector != null &&
        ((Connector?.Socket?.Connected ?? false) || (Connector?.Socket?.IsBound ?? false));

    public void BlockUntilConnected()
    {
        connecting = true;
        Connect(new object());
    }

    public void Connect()
    {
        ScheduleConnect(0);
    }

    public event Action? OnConnected;
    public event Action? OnDisconnecting;
    public event Action? OnDisconnected;

    public void Disconnect()
    {
        Disconnect(false);
    }

    public virtual void Send(IVersionedMessage message)
    {
        StreamToPublisher?.Enqueue(Connector!, message);
    }

    public override void OnCxError(ISocketSessionConnection cx, string errorMsg, int proposedReconnectMs)
    {
        Logger.Info("Closing connection {0}-{1} to {2}:{3} [{4}]", SessionDescription, cx.Id, Host, Port, errorMsg);
        Disconnect(true);

        ScheduleConnect(proposedReconnectMs < 0 ?
            initialConnectionConfig!.ReconnectIntervalMs :
            (uint)proposedReconnectMs);
    }

    protected abstract IOSSocket? CreateAndConnect(string host, int port);

    private void ScheduleConnect(uint spanMs)
    {
        lock (connSync)
        {
            if (IsConnected || connecting || triggerConnectNowSignal != null) return;
            connecting = true;
            if (spanMs > 0)
                triggerConnectNowSignal = ParallelController.ScheduleWithEarlyTrigger(
                    (state, timeout) => Connect(state!), spanMs);
            else ParallelController.CallFromThreadPool(Connect!);
        }
    }

    private void Connect(object state)
    {
        lock (connSync)
        {
            triggerConnectNowSignal = null;
            connectedConnectionConfig = null;
            if (IsConnected || !connecting) return;
            var currentConnectionConfig = initialConnectionConfig;
            while (currentConnectionConfig != null)
            {
                try
                {
                    Connector = RegisterConnector(CreateAndConnect(currentConnectionConfig.Hostname,
                        currentConnectionConfig.Port)!);
                }
                catch (Exception ex)
                {
                    Logger.Info("Connection to {0} {1}:{2} rejected: {3}", SessionDescription,
                        currentConnectionConfig.Hostname, currentConnectionConfig.Port, ex);
                }

                if (IsConnected)
                {
                    connecting = false;
                    Logger.Info("Connection to id:{0} {1}:{2} accepted", SessionDescription, Connector?.Id,
                        currentConnectionConfig.Hostname, currentConnectionConfig.Port);
                    StartMessaging();
                    connectedConnectionConfig = currentConnectionConfig;
                    OnConnected?.Invoke();
                    break;
                }

                currentConnectionConfig = currentConnectionConfig.FallBackConnectionConfig;
            }

            if (IsConnected) return;
            Disconnect(true);
            ScheduleConnect(initialConnectionConfig!.ReconnectIntervalMs);
        }
    }

    protected void Disconnect(bool inError)
    {
        lock (connSync)
        {
            connecting = false;
            if (Connector != null)
            {
                triggerConnectNowSignal?.Set();
                if (Connector == null || connectedConnectionConfig == null) return;
                if (!inError) OnDisconnecting?.Invoke();
                StopMessaging();
                Unregister(Connector);
                Logger.Info("Connection to {0} {1} id {2}:{3} closed", SessionDescription, Connector.Id,
                    connectedConnectionConfig.Hostname, connectedConnectionConfig.Port);
                Connector.Socket?.Close();
                OnDisconnected?.Invoke();
            }
        }
    }
}
