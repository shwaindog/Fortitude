#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Controls;

public interface IInitiateControls : IStreamControls
{
    void StartAsync();
}

public class InitiateControls : SocketStreamControls, IInitiateControls
{
    private readonly object connSync = new();
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(InitiateControls));
    private readonly IOSParallelController parallelController;
    private readonly ISocketReconnectConfig reconnectConfig;
    private readonly ISocketSessionContext socketSessionContext;
    private IIntraOSThreadSignal? triggerConnectNowSignal;

    public InitiateControls(ISocketSessionContext socketSessionContext) : base(socketSessionContext)
    {
        parallelController = socketSessionContext.SocketFactoryResolver.ParallelController!;
        reconnectConfig = socketSessionContext.NetworkTopicConnectionConfig.ReconnectConfig;
        this.socketSessionContext = socketSessionContext;
    }

    public virtual void StartAsync()
    {
        ScheduleConnect(0);
    }

    public override void Connect()
    {
        lock (connSync)
        {
            triggerConnectNowSignal = null;
            if (socketSessionContext.SocketConnection is { IsConnected: true } ||
                socketSessionContext.SocketSessionState == SocketSessionState.Connecting ||
                socketSessionContext.SocketSessionState == SocketSessionState.Disconnecting) return;
            socketSessionContext.OnSocketStateChanged(SocketSessionState.Connecting);
            var connConfig = socketSessionContext.NetworkTopicConnectionConfig;
            var socketFactory = socketSessionContext.SocketFactoryResolver.SocketFactory!;
            foreach (var socketConConfig in socketSessionContext.NetworkTopicConnectionConfig)
            {
                try
                {
                    var subscriberSocket = socketFactory.Create(connConfig, socketConConfig);

                    var ipEndPoint = subscriberSocket.RemoteOrLocalIPEndPoint()!;

                    // will create socketSessionContext.SocketReceive and register listener with dispatcher
                    socketSessionContext.OnConnected(new SocketConnection(connConfig.TopicName
                        , socketSessionContext.ConversationType,
                        subscriberSocket, ipEndPoint.Address, (ushort)ipEndPoint.Port));
                }
                catch (Exception ex)
                {
                    logger.Info("Connection to {0} {1}:{2} rejected: {3}",
                        socketSessionContext.Name, socketConConfig.Hostname, socketConConfig.Port, ex);
                }

                if (socketSessionContext.SocketConnection?.IsConnected ?? false)
                {
                    reconnectConfig.NextReconnectIntervalMs = reconnectConfig.StartReconnectIntervalMs;
                    logger.Info("Connection to id:{0} {1}:{2} accepted",
                        socketSessionContext.Name, socketSessionContext.Id,
                        socketConConfig.Hostname, socketConConfig.Port);
                    StartMessaging();
                    break;
                }
            }

            if (socketSessionContext.SocketConnection?.IsConnected ?? false) return;
            OnSessionFailure($"'Connect' failed to successfully open connection to Topic.Name: '{connConfig.TopicName}'");
        }
    }

    public override void OnSessionFailure(string reason)
    {
        Disconnect(true);
        var scheduleConnectWaitMs = reconnectConfig.NextReconnectIntervalMs;
        logger.Info("Will attempt reconnecting to {0} {1} id {2} reason {3} in {4}ms",
            SocketSocketSessionContext.Name, SocketSocketSessionContext.Id,
            SocketSocketSessionContext.NetworkTopicConnectionConfig, reason, scheduleConnectWaitMs);
        socketSessionContext.OnReconnecting();
        ScheduleConnect(scheduleConnectWaitMs);
    }

    public override void Disconnect()
    {
        Disconnect(false);
    }

    private void ScheduleConnect(uint spanMs)
    {
        lock (connSync)
        {
            if (socketSessionContext.SocketConnection is { IsConnected: true } ||
                socketSessionContext.SocketSessionState == SocketSessionState.Connecting ||
                triggerConnectNowSignal != null) return;
            if (spanMs > 0)
                triggerConnectNowSignal = parallelController.ScheduleWithEarlyTrigger(
                    (state, timeout) => Connect(), spanMs);
            else parallelController.CallFromThreadPool(state => Connect());
        }
    }

    protected void Disconnect(bool inError)
    {
        lock (connSync)
        {
            if (!inError || socketSessionContext.SocketSessionState == SocketSessionState.Connected ||
                socketSessionContext.SocketSessionState == SocketSessionState.Connecting)
                socketSessionContext.OnDisconnecting();
            if (socketSessionContext.SocketConnection?.IsConnected ?? false)
            {
                triggerConnectNowSignal?.Set();
                StopMessaging();
                logger.Info("Connection to {0} {1} id {2}:{3} closed",
                    socketSessionContext.Name, socketSessionContext.Id,
                    socketSessionContext.NetworkTopicConnectionConfig.Current.Hostname,
                    socketSessionContext.SocketConnection.ConnectedPort);
                socketSessionContext.SocketConnection.OSSocket?.Close();
            }

            socketSessionContext.OnDisconnected();
        }
    }
}
