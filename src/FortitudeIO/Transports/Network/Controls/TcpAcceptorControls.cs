#region

using System.Net;
using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Controls;

public interface IAcceptorControls : IStreamControls
{
    IReadOnlyDictionary<int, IConversationRequester> Clients { get; }
    event Action<IConversationRequester> NewClient;
    event Action<IConversationRequester> ClientRemoved;
    void RemoveClient(IConversationRequester clientRequester, CloseReason closeReason, string? reason);
    void Broadcast(IVersionedMessage message);
    void StopImmediate(CloseReason closeReason, string? reason);
}

public class TcpAcceptorControls : SocketStreamControls, IAcceptorControls
{
    private readonly ISyncLock clientsSync = new YieldLockLight();
    private readonly ISyncLock connSync = new YieldLockLight();
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TcpAcceptorControls));
    private readonly ISocketFactory socketFactory;

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private Dictionary<int, IConversationRequester> clients = new();

    public TcpAcceptorControls(ISocketSessionContext acceptorSocketSessionContext) : base(acceptorSocketSessionContext) =>
        socketFactory = acceptorSocketSessionContext.SocketFactoryResolver.SocketFactory!;

    public event Action<IConversationRequester>? NewClient;

    public event Action<IConversationRequester>? ClientRemoved;

    public IReadOnlyDictionary<int, IConversationRequester> Clients
    {
        get { return clients.ToDictionary(kvp => kvp.Key, kvp => kvp.Value); }
    }

    public override void OnSessionFailure(string reason)
    {
        var connConfig = SocketSessionContext.NetworkTopicConnectionConfig;
        logger.Error("Failed to communicate TCP Acceptor socket for Topic.Name: {0}. " +
                     "Will not attempt reconnect.  Reason {1}", connConfig.TopicName, reason);
    }

    public void RemoveClient(IConversationRequester clientRequester, CloseReason closeReason, string? reason = null)
    {
        if (clients.ContainsKey(clientRequester.Id))
        {
            clientsSync.Acquire();
            try
            {
                clients.Remove(clientRequester.Id);
            }
            finally
            {
                clientsSync.Release();
            }

            try
            {
                if (clientRequester.Session is ISocketSessionContext socketSessionContext)
                    if (clientRequester.ConversationState == ConversationState.Started)
                    {
                        logger.Info("Client.Id: {0}, Name {1} will be disconnected from server {2}",
                            socketSessionContext.Id, socketSessionContext.Name, SocketSessionContext.Name);
                        clientRequester.Stop(CloseReason.RemoteRequestedClose, reason);
                    }
            }
            catch
            {
                // do nothing
            }


            ClientRemoved?.Invoke(clientRequester);
        }
    }

    public void Broadcast(IVersionedMessage message)
    {
        if (Clients.Any())
        {
            clientsSync.Acquire();
            try
            {
                foreach (var clientSessionOperator in Clients.Values)
                    clientSessionOperator.StreamPublisher!.Send(message);
            }
            finally
            {
                clientsSync.Release();
            }
        }
    }

    public void StopImmediate(CloseReason closeReason, string? reason)
    {
        if (SocketSessionContext.SocketConnection is { IsConnected: true }) Disconnect(closeReason, reason);
        if (!HaveStartedMessaging) return;
        HaveStartedMessaging = false;
        SocketSessionContext.SocketDispatcher.StopImmediate();
        SocketSessionContext.OnStopped();
    }

    public override bool Connect()
    {
        if (ConnectAttemptSucceeded()) return true;

        OnSessionFailure(
            $"'Could not connect to configured IP or port.  Is another process already connected?. " +
            $"Topic.Name: '{SocketSessionContext.NetworkTopicConnectionConfig.TopicName}'");
        return false;
    }

    public override void Disconnect(CloseReason closeReason, string? reason = null)
    {
        connSync.Acquire();
        try
        {
            if (SocketSessionContext.SocketSessionState != SocketSessionState.Connected ||
                !(SocketSessionContext.SocketReceiver?.IsAcceptor ?? false)) return;
            SocketSessionContext.OnDisconnecting();
            Unregister(SocketSessionContext);

            foreach (var client in clients.Values)
                try
                {
                    if (client.ConversationState == ConversationState.Started) client.Stop(CloseReason.RemoteDisconnecting, reason);
                }
                catch
                {
                    // do nothing
                }

            clientsSync.Acquire();
            try
            {
                clients.Clear();
            }
            finally
            {
                clientsSync.Release();
            }

            SocketSessionContext.SocketReceiver.Accept -= OnCxAccept;

            SocketSessionContext.OnDisconnected(closeReason, reason);

            logger.Info("Publisher {0} on  {1}:{2} stopped. {3}", SocketSessionContext.Name,
                SocketSessionContext.SocketConnection?.InstanceName, SocketSessionContext.SocketConnection?.ConnectedPort, reason);
        }
        finally
        {
            connSync.Release();
        }
    }

    protected override bool ConnectAttemptSucceeded()
    {
        connSync.Acquire();
        try
        {
            if (SocketSessionContext.SocketSessionState == SocketSessionState.Connected ||
                SocketSessionContext.SocketSessionState == SocketSessionState.Connecting) return true;
            SocketSessionContext.OnSocketStateChanged(SocketSessionState.Connecting);
            var connConfig = SocketSessionContext.NetworkTopicConnectionConfig;
            foreach (var socketConConfig in connConfig)
                try
                {
                    logger.Info("Starting publisher {0} @{1}",
                        SocketSessionContext.Name, socketConConfig.Port);

                    var listeningSocket = socketFactory.Create(SocketSessionContext.NetworkTopicConnectionConfig
                        , socketConConfig);

                    var localEndPointIp = listeningSocket.RemoteOrLocalIPEndPoint()!;
                    SocketSessionContext.OnConnected(new SocketConnection(connConfig.TopicName
                        , SocketSessionContext.ConversationType,
                        listeningSocket, localEndPointIp.Address, socketConConfig.Port));
                    SocketSessionContext.SocketReceiver!.ZeroBytesReadIsDisconnection = false;

                    logger.Info("Publisher {0} {1}@{2} started",
                        SocketSessionContext.Name, localEndPointIp.Address, socketConConfig.Port);
                    break;
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to open socket for {0}. Got {1}", connConfig, ex);
                }

            if (SocketSessionContext.SocketConnection?.IsConnected ?? false)
            {
                SocketSessionContext.SocketReceiver!.Accept += OnCxAccept;
                return true;
            }
        }
        finally
        {
            connSync.Release();
        }

        return false;
    }

    private void OnCxAccept()
    {
        IPEndPoint? clientIpEndPoint = null;
        try
        {
            if (SocketSessionContext.SocketSessionState != SocketSessionState.Disconnecting)
            {
                var acceptedSocket = SocketSessionContext.SocketReceiver!.AcceptClientSocketRequest();
                acceptedSocket.LingerState = new LingerOption(true, 2);
                clientIpEndPoint = acceptedSocket.RemoteOrLocalIPEndPoint()!;
                var clientConversation = RegisterSocketAsTcpRequesterConversation(acceptedSocket, clientIpEndPoint);
                logger.Info("Client {0} connected to server {1}", SocketSessionContext.Name, clientIpEndPoint);
                clientConversation.Disconnected += () => RemoveClient(clientConversation, CloseReason.RemoteDisconnecting);
                clientsSync.Acquire();
                try
                {
                    clients.Add(clientConversation.Id, clientConversation);
                }
                finally
                {
                    clientsSync.Release();
                }

                NewClient?.Invoke(clientConversation);
                clientConversation.Start();
            }
            else
            {
                logger.Warn("Not accepting connection as Server is shutting down!");
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error while connecting client from server {0} remote {1}.  Got {2}"
                , SocketSessionContext.Name, clientIpEndPoint, ex);
        }
    }

    private void Unregister(ISocketSessionContext clientSocketSessionContext)
    {
        SocketSessionContext.SocketDispatcher.Listener?.UnregisterForListen(clientSocketSessionContext
            .SocketReceiver!);
    }

    private ConversationRequester RegisterSocketAsTcpRequesterConversation(IOSSocket socket, IPEndPoint receiverEndpoint)
    {
        var clientNetworkTopicConnectionConfig = SocketSessionContext.NetworkTopicConnectionConfig.ToggleProtocolDirection();
        socket.SendBufferSize = clientNetworkTopicConnectionConfig.SendBufferSize;
        socket.ReceiveBufferSize = clientNetworkTopicConnectionConfig.ReceiveBufferSize;

        var socketSessionConnection = new SocketSessionContext(clientNetworkTopicConnectionConfig.TopicName + "AcceptedClient"
            , ConversationType.Requester,
            SocketConversationProtocol.TcpClient, clientNetworkTopicConnectionConfig,
            SocketSessionContext.SocketFactoryResolver, SocketSessionContext.SerdesFactory);

        var clientStreamInitiator = new TcpAcceptedClientControls(socketSessionConnection);
        var clientRequester = new ConversationRequester(socketSessionConnection, clientStreamInitiator, true);
        socketSessionConnection.OnConnected(new SocketConnection(
            SocketSessionContext.SocketConnection!.InstanceName,
            socketSessionConnection.ConversationType,
            socket, receiverEndpoint.Address, (ushort)receiverEndpoint.Port));
        return clientRequester;
    }
}
