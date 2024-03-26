#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Controls;

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
    private readonly ISocketSessionContext acceptorSocketSessionContext;
    private readonly ISyncLock clientsSync = new YieldLockLight();
    private readonly ISyncLock connSync = new YieldLockLight();
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TcpAcceptorControls));
    private readonly ISocketFactory socketFactory;

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private Dictionary<int, IConversationRequester> clients = new();

    public TcpAcceptorControls(ISocketSessionContext acceptorSocketSessionContext) : base(acceptorSocketSessionContext)
    {
        this.acceptorSocketSessionContext = acceptorSocketSessionContext;
        socketFactory = acceptorSocketSessionContext.SocketFactories.SocketFactory!;
    }

    public event Action<IConversationRequester>? NewClient;

    public event Action<IConversationRequester>? ClientRemoved;

    public IReadOnlyDictionary<int, IConversationRequester> Clients
    {
        get { return clients.ToDictionary(kvp => kvp.Key, kvp => kvp.Value); }
    }

    public override void Connect()
    {
        connSync.Acquire();
        try
        {
            if (acceptorSocketSessionContext.SocketSessionState == SocketSessionState.Connected ||
                acceptorSocketSessionContext.SocketSessionState == SocketSessionState.Connecting) return;
            acceptorSocketSessionContext.OnSocketStateChanged(SocketSessionState.Connecting);
            var connConfig = acceptorSocketSessionContext.SocketTopicConnectionConfig;
            foreach (var socketConConfig in connConfig)
                try
                {
                    logger.Info("Starting publisher {0} @{1}",
                        acceptorSocketSessionContext.Name, socketConConfig.Port);

                    var listeningSocket = socketFactory.Create(acceptorSocketSessionContext.SocketTopicConnectionConfig
                        , socketConConfig);

                    var localEndPointIp = listeningSocket.RemoteOrLocalIPEndPoint()!;
                    acceptorSocketSessionContext.OnConnected(new SocketConnection(connConfig.TopicName
                        , acceptorSocketSessionContext.ConversationType,
                        listeningSocket, localEndPointIp.Address, socketConConfig.Port));
                    acceptorSocketSessionContext.SocketReceiver!.ZeroBytesReadIsDisconnection = false;

                    logger.Info("Publisher {0} {1}@{2} started",
                        acceptorSocketSessionContext.Name, localEndPointIp.Address, socketConConfig.Port);
                    break;
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to open socket for {}. Got {1}", connConfig, ex);
                }

            if (acceptorSocketSessionContext.SocketConnection?.IsConnected ?? false)
            {
                acceptorSocketSessionContext.SocketReceiver!.Accept += OnCxAccept;
                StartMessaging();
            }
        }
        finally
        {
            connSync.Release();
        }
    }

    public override void Disconnect()
    {
        connSync.Acquire();
        try
        {
            if (acceptorSocketSessionContext.SocketSessionState != SocketSessionState.Connected ||
                !(acceptorSocketSessionContext.SocketReceiver?.IsAcceptor ?? false)) return;

            acceptorSocketSessionContext.OnDisconnecting();
            StopMessaging();
            logger.Info("Stopping publisher {0} @{1}", acceptorSocketSessionContext.Name,
                acceptorSocketSessionContext.SocketConnection!.ConnectedPort);
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

            acceptorSocketSessionContext.SocketReceiver.Accept -= OnCxAccept;
            Unregister(acceptorSocketSessionContext);
            acceptorSocketSessionContext.SocketConnection.OSSocket.Close();

            logger.Info("Publisher {0} @{0} stopped", acceptorSocketSessionContext.Name,
                acceptorSocketSessionContext.SocketConnection.ConnectedPort);

            acceptorSocketSessionContext.OnDisconnected();
        }
        finally
        {
            connSync.Release();
        }
    }

    public void RemoveClient(IConversationRequester clientRequester)
    {
        if (acceptorSocketSessionContext.SocketSessionState != SocketSessionState.Disconnecting)
        {
            Unregister(clientRequester);
            try
            {
                if (clientRequester.Session is ISocketSessionContext socketSessionContext)
                {
                    logger.Info(
                        "Client {0} (" + socketSessionContext.SocketConnection!.ConnectedPort +
                        ") disconnected from server {1} @{2}",
                        socketSessionContext.Id, socketSessionContext.Name
                        , socketSessionContext.SocketConnection.ConnectedPort);
                    socketSessionContext.SocketConnection.OSSocket.Close();
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

    private void OnCxAccept()
    {
        try
        {
            if (acceptorSocketSessionContext.SocketSessionState != SocketSessionState.Disconnecting)
            {
                var client = RegisterSocketAsTcpRequesterConversation(acceptorSocketSessionContext.SocketReceiver!
                    .AcceptClientSocketRequest());
                var clientIpEndPoint = client.SocketConnection!.OSSocket.RemoteOrLocalIPEndPoint();
                logger.Info("Client {0} ({1}) connected to server {2} @{3}",
                    client.Id, clientIpEndPoint, acceptorSocketSessionContext.Name,
                    acceptorSocketSessionContext.SocketConnection!.ConnectedPort);
                var clientStreamInitiator = new InitiateControls(client);
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
            }
            else
            {
                Disconnect();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error while connecting client from server {0} @{1}: {2}"
                , acceptorSocketSessionContext.Name,
                acceptorSocketSessionContext.SocketConnection!.ConnectedPort, ex);
        }
    }

    private void Unregister(ISocketSessionContext clientSocketSessionContext)
    {
        acceptorSocketSessionContext.SocketDispatcher.Listener.UnregisterForListen(clientSocketSessionContext
            .SocketReceiver!);
    }

    private void Unregister(IConversationRequester clientRequester)
    {
        acceptorSocketSessionContext.SocketDispatcher.Listener.UnregisterForListen(clientRequester
            .StreamListener!);
    }

    private ISocketSessionContext RegisterSocketAsTcpRequesterConversation(IOSSocket socket)
    {
        socket.SendBufferSize = acceptorSocketSessionContext.SocketTopicConnectionConfig.SendBufferSize;
        socket.ReceiveBufferSize = acceptorSocketSessionContext.SocketTopicConnectionConfig.ReceiveBufferSize;

        var socketSessionConnection = new SocketSessionContext(ConversationType.Requester,
            SocketConversationProtocol.TcpClient, acceptorSocketSessionContext.Name,
            acceptorSocketSessionContext.SocketTopicConnectionConfig,
            acceptorSocketSessionContext.SocketFactories, acceptorSocketSessionContext.SerdesFactory);
        var ipEndPoint = socket.RemoteOrLocalIPEndPoint()!;
        socketSessionConnection.SocketDispatcher = acceptorSocketSessionContext.SocketDispatcher;
        socketSessionConnection.OnConnected(new SocketConnection(
            acceptorSocketSessionContext.SocketConnection!.InstanceName,
            socketSessionConnection.ConversationType,
            socket, ipEndPoint.Address, (ushort)ipEndPoint.Port));
        return socketSessionConnection;
    }
}
