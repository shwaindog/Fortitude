#region

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
    void RemoveClient(IConversationRequester clientRequester);
    void Broadcast(IVersionedMessage message);
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

    public override bool Connect()
    {
        if (ConnectAttemptSucceeded()) return true;

        OnSessionFailure(
            $"'Could not connect to configured IP or port.  Is another process already connected?. " +
            $"Topic.Name: '{SocketSessionContext.NetworkTopicConnectionConfig.TopicName}'");
        return false;
    }

    public override void OnSessionFailure(string reason)
    {
        var connConfig = SocketSessionContext.NetworkTopicConnectionConfig;
        logger.Error("Failed to communicate TCP Acceptor socket for Topic.Name: {0}. " +
                     "Will not attempt reconnect.  Reason {1}", connConfig.TopicName, reason);
    }

    public override void Disconnect()
    {
        connSync.Acquire();
        try
        {
            if (SocketSessionContext.SocketSessionState != SocketSessionState.Connected ||
                !(SocketSessionContext.SocketReceiver?.IsAcceptor ?? false)) return;

            SocketSessionContext.OnDisconnecting();
            StopMessaging();
            logger.Info("Stopping publisher {0} @{1}", SocketSessionContext.Name,
                SocketSessionContext.SocketConnection!.ConnectedPort);
            clientsSync.Acquire();
            try
            {
                foreach (var client in clients.Values)
                    try
                    {
                        Unregister(client);
                        client.Stop();
                    }
                    catch
                    {
                        // do nothing
                    }

                clients.Clear();
            }
            finally
            {
                clientsSync.Release();
            }

            SocketSessionContext.SocketReceiver.Accept -= OnCxAccept;
            Unregister(SocketSessionContext);
            SocketSessionContext.SocketConnection.OSSocket.Close();

            logger.Info("Publisher {0} @{1} stopped", SocketSessionContext.Name,
                SocketSessionContext.SocketConnection.ConnectedPort);

            SocketSessionContext.OnDisconnected();
        }
        finally
        {
            connSync.Release();
        }
    }

    public void RemoveClient(IConversationRequester clientRequester)
    {
        if (SocketSessionContext.SocketSessionState != SocketSessionState.Disconnecting)
        {
            Unregister(clientRequester);
            try
            {
                if (clientRequester.Session is ISocketSessionContext socketSessionContext)
                {
                    if (socketSessionContext.SocketSessionState == SocketSessionState.Connected)
                        socketSessionContext.OnDisconnecting();

                    logger.Info(
                        "Client {0} (" + socketSessionContext.SocketConnection!.ConnectedPort +
                        ") disconnected from server {1} @{2}",
                        socketSessionContext.Id, socketSessionContext.Name
                        , socketSessionContext.SocketConnection.ConnectedPort);
                    socketSessionContext.SocketConnection.OSSocket.Close();
                    if (socketSessionContext.SocketSessionState != SocketSessionState.Disconnected)
                        socketSessionContext.OnDisconnected();
                }
            }
            catch
            {
                // do nothing
            }

            clientsSync.Acquire();
            try
            {
                clients.Remove(clientRequester.Id);
            }
            finally
            {
                clientsSync.Release();
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
                StartMessaging();
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
        try
        {
            if (SocketSessionContext.SocketSessionState != SocketSessionState.Disconnecting)
            {
                var client = RegisterSocketAsTcpRequesterConversation(SocketSessionContext.SocketReceiver!
                    .AcceptClientSocketRequest());
                var clientIpEndPoint = client.SocketConnection!.OSSocket.RemoteOrLocalIPEndPoint();
                logger.Info("Client {0} ({1}) connected to server {2} @{3}",
                    client.Id, clientIpEndPoint, SocketSessionContext.Name,
                    SocketSessionContext.SocketConnection!.ConnectedPort);
                var clientStreamInitiator = new TcpAcceptedClientControls(client);
                var clientRequester = new ConversationRequester(client, clientStreamInitiator);
                clientRequester.Disconnected += () => RemoveClient(clientRequester);
                clientsSync.Acquire();
                try
                {
                    clients.Add(client.Id, clientRequester);
                }
                finally
                {
                    clientsSync.Release();
                }

                NewClient?.Invoke(clientRequester);
                clientRequester.StartMessaging();
            }
            else
            {
                Disconnect();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error while connecting client from server {0} @{1}: {2}"
                , SocketSessionContext.Name,
                SocketSessionContext.SocketConnection!.ConnectedPort, ex);
        }
    }

    private void Unregister(ISocketSessionContext clientSocketSessionContext)
    {
        SocketSessionContext.SocketDispatcher.Listener?.UnregisterForListen(clientSocketSessionContext
            .SocketReceiver!);
    }

    private void Unregister(IConversationRequester clientRequester)
    {
        SocketSessionContext.SocketDispatcher.Listener?.UnregisterForListen(clientRequester
            .StreamListener!);
    }

    private ISocketSessionContext RegisterSocketAsTcpRequesterConversation(IOSSocket socket)
    {
        var clientNetworkTopicConnectionConfig = SocketSessionContext.NetworkTopicConnectionConfig.ToggleProtocolDirection();
        socket.SendBufferSize = clientNetworkTopicConnectionConfig.SendBufferSize;
        socket.ReceiveBufferSize = clientNetworkTopicConnectionConfig.ReceiveBufferSize;

        var socketSessionConnection = new SocketSessionContext(clientNetworkTopicConnectionConfig.TopicName + "AcceptedClient"
            , ConversationType.Requester,
            SocketConversationProtocol.TcpClient, clientNetworkTopicConnectionConfig,
            SocketSessionContext.SocketFactoryResolver, SocketSessionContext.SerdesFactory);
        var ipEndPoint = socket.RemoteOrLocalIPEndPoint()!;
        socketSessionConnection.OnConnected(new SocketConnection(
            SocketSessionContext.SocketConnection!.InstanceName,
            socketSessionConnection.ConversationType,
            socket, ipEndPoint.Address, (ushort)ipEndPoint.Port));
        return socketSessionConnection;
    }
}
