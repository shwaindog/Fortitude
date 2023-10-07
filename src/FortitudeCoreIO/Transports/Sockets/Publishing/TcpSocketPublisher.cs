#region

using System.Net;
using System.Net.Sockets;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public abstract class TcpSocketPublisher : SocketPublisherBase, ITcpSocketPublisher
{
    private readonly IDoublyLinkedList<ISocketSessionConnection> clients =
        new DoublyLinkedList<ISocketSessionConnection>();

    private volatile bool disconnecting;

    protected TcpSocketPublisher(IFLogger logger, ISocketDispatcher dispatcher,
        IOSNetworkingController networkingController, int port, string sessionDescription)
        : base(logger, dispatcher, sessionDescription, networkingController, port) { }

    protected virtual ISocketSessionConnection? Acceptor { get; set; }

    public ISocketSessionConnection RegisterAcceptor(IOSSocket socket, Action<ISocketSessionConnection> acceptor)
    {
        var socketSessionConnection = new SocketSessionConnection(new SocketSessionReceiver(socket,
                NetworkingController.DirectOSNetworkingApi, acceptor, SessionDescription),
            new SocketSessionSender(socket, NetworkingController.DirectOSNetworkingApi, SessionDescription),
            OnCxError);
        socket.SendBufferSize = SendBufferSize;
        Dispatcher.Listener.RegisterForListen(socketSessionConnection);
        return socketSessionConnection;
    }

    public virtual void Connect()
    {
        connSync.Acquire();
        try
        {
            if (Acceptor != null) return;
            Logger.Info("Starting publisher {0} @{1}", SessionDescription, Port);

            var listeningSocket = NetworkingController.CreateOSSocket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.NoDelay = true;
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
            listeningSocket.Listen(10);
            Acceptor = RegisterAcceptor(listeningSocket, OnCxAccept);

            StartMessaging();

            Logger.Info("Publisher {0} @{1} started", SessionDescription, Port);
            OnConnected?.Invoke();
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
            if (Acceptor == null) return;
            disconnecting = true;

            OnDisconnecting?.Invoke();
            StopMessaging();
            Logger.Info("Stopping publisher {0} @{1}", SessionDescription, Port);
            clientsSync.Acquire();
            try
            {
                foreach (var client in clients)
                    try
                    {
                        Unregister(client);
                        client.Socket.Close();
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

            Unregister(Acceptor);
            Acceptor.Socket.Close();
            Acceptor = null;

            Logger.Info("Publisher {0} @{0} stopped", SessionDescription, Port);

            disconnecting = false;

            OnDisconnected?.Invoke();
        }
        finally
        {
            connSync.Release();
        }
    }

    public bool IsConnected => Acceptor != null && (Acceptor.Socket.Connected || Acceptor.Socket.IsBound);

    public void RemoveClient(ISocketSessionConnection client)
    {
        if (!disconnecting)
        {
            Unregister(client);
            try
            {
                Logger.Info("Client {0} (" + client.Socket.RemoteEndPoint + ") disconnected from server {1} @{2}",
                    client.Id, SessionDescription, Port);
                client.Socket.Close();
            }
            catch
            {
                // do nothing
            }

            clientsSync.Acquire();
            try
            {
                clients.Remove(client);
            }
            finally
            {
                clientsSync.Release();
            }

            OnClientRemoved?.Invoke(client);
        }
    }

    public void Broadcast(IVersionedMessage message)
    {
        if (!clients.IsEmpty)
        {
            clientsSync.Acquire();
            try
            {
                Enqueue(clients, message);
            }
            finally
            {
                clientsSync.Release();
            }
        }
    }

    protected virtual void OnCxError(ISocketSessionConnection client, string errorMsg, int proposedReconnect)
    {
        if (disconnecting) return;
        try
        {
            Logger.Info(
                "Disconnecting client {0} (" + client.Socket.RemoteEndPoint + ") [{1}] from server {2} @{3}",
                client.Id, errorMsg, SessionDescription, Port);
            RemoveClient(client);
        }
        catch
        {
            // ignored
        }
    }

    private void OnCxAccept(ISocketSessionConnection cx)
    {
        try
        {
            if (!disconnecting && StreamFromSubscriber is ISocketSubscriber clientStream)
            {
                var client = clientStream.RegisterConnector(cx.SessionReceiver!.AcceptClientSocketRequest());
                client.Socket.SendBufferSize = SendBufferSize;
                Logger.Info("Client {0} (" + client.Socket.RemoteEndPoint + ") connected to server {1} @{2}",
                    client.Id, SessionDescription + client.Socket.RemoteEndPoint, Port);
                clientsSync.Acquire();
                try
                {
                    clients.AddLast(client);
                }
                finally
                {
                    clientsSync.Release();
                }

                OnNewClient?.Invoke(client);
            }
            else
            {
                cx.Socket.Close();
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Error while connecting client from server {0} @{1}: {2}", SessionDescription, Port, ex);
        }
    }

    public event Action<ISocketSessionConnection>? OnNewClient;

    public event Action<ISocketSessionConnection>? OnClientRemoved;

    public override event Action? OnConnected;
    public event Action? OnDisconnecting;
    public override event Action? OnDisconnected;
}
