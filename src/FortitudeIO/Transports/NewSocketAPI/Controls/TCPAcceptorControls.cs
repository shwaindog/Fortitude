using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Sockets
{
    public interface IAcceptorControls : IStreamControls
    {
        event Action<ISocketConversation> OnNewClient;
        event Action<ISocketConversation> OnClientRemoved;
        IReadOnlyDictionary<int, ISocketConversation> Clients {get;}
        void RemoveClient(ISocketConversation clientSocketSessionContext);
        void Broadcast(IVersionedMessage message);
    }

    public class TCPAcceptorControls : IAcceptorControls
    {
        private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TCPAcceptorControls));
        private readonly ISyncLock clientsSync = new YieldLockLight();
        private readonly ISyncLock connSync = new YieldLockLight();
        private readonly ISocketFactory socketFactory;
        private IDirectOSNetworkingApi directOSNetworkingApi;
        private readonly ISocketSessionContext acceptorSocketSessionContext;
        private readonly Dictionary<int, ISocketSessionContext> clients = new Dictionary<int, ISocketSessionContext>();

        public event Action<ISocketConversation> OnNewClient;

        public event Action<ISocketConversation> OnClientRemoved;

        public TCPAcceptorControls(ISocketSessionContext acceptorSocketSessionContext)
        {
            this.acceptorSocketSessionContext = acceptorSocketSessionContext;
            socketFactory = acceptorSocketSessionContext.SocketFactories.SocketFactory;
        }

        public IReadOnlyDictionary<int, ISocketConversation> Clients
        {
            get
            {
                return clients.ToDictionary(kvp => kvp.Key, kvp => (ISocketConversation) kvp.Value);
            }
        }

        public virtual void StartMessaging()
        {
            acceptorSocketSessionContext.SocketDispatcher.Start();
        }

        public virtual void StopMessaging()
        {
            acceptorSocketSessionContext.SocketDispatcher.Stop();
        }

        public void Connect()
        {
            connSync.Acquire();
            try
            {
                if (acceptorSocketSessionContext.SocketReceiver != null) return;
                var connConfig = acceptorSocketSessionContext.SocketConnectionConfig;
                var maxAttempts = Math.Max(connConfig.PortEndRange - connConfig.PortStartRange, 1);
                for (ushort attemptNum = 0; attemptNum < maxAttempts; attemptNum++)
                {
                    try
                    {
                        var port = socketFactory.GetPort(connConfig, attemptNum);
                        logger.Info("Starting publisher {0} @{1}",
                            acceptorSocketSessionContext.ConversationDescription, port);

                        IOSSocket listeningSocket = socketFactory.Create(acceptorSocketSessionContext.SocketConversationProtocol,
                            acceptorSocketSessionContext.SocketConnectionConfig, attemptNum);

                        logger.Info("Publisher {0} @{1} started",
                            acceptorSocketSessionContext.ConversationDescription, port);

                        var localEndPointIp = listeningSocket.RemoteOrLocalIPEndPoint();
                        acceptorSocketSessionContext.OnConnected(new SocketConnection(connConfig.InstanceName, acceptorSocketSessionContext.ConversationType, 
                            listeningSocket, localEndPointIp.Address, port));
                        acceptorSocketSessionContext.SocketReceiver.ZeroBytesReadIsDisconnection = false;
                        break;
                    }
                    catch (Exception e)
                    {
                        if (attemptNum == maxAttempts - 1)
                        {
                            logger.Error($"Failed to open socket for {connConfig}");
                        }
                    }
                }
                if (acceptorSocketSessionContext.SocketConnection?.IsConnected ?? false)
                {
                    acceptorSocketSessionContext.SocketReceiver.Accept += OnCxAccept;
                    StartMessaging();
                }
            }
            finally
            {
                connSync.Release();
            }
        }

        private void OnCxAccept()
        {
            try
            {
                if (acceptorSocketSessionContext.SocketSessionState != SocketSessionState.Disconnecting)
                {
                    var client = RegisterSocketAsTcpRequestResponseRequester(acceptorSocketSessionContext.SocketReceiver.AcceptClientSocketRequest());
                    var clientIpEndPoint = client.SocketConnection.OSSocket.RemoteOrLocalIPEndPoint();
                    logger.Info("Client {0} (" + clientIpEndPoint + ") connected to server {1} @{2}",
                        client.Id, acceptorSocketSessionContext.ConversationDescription, 
                        acceptorSocketSessionContext.SocketConnection.ConnectedPort);
                    clientsSync.Acquire();
                    try
                    {
                        clients.Add(client.Id, client);
                    }
                    finally
                    {
                        clientsSync.Release();
                    }
                    OnNewClient?.Invoke(client);
                }
                else
                {
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error while connecting client from server {0} @{1}: {2}", acceptorSocketSessionContext.ConversationDescription, 
                    acceptorSocketSessionContext.SocketConnection.ConnectedPort, ex);
            }
        }

        public virtual void Disconnect()
        {
            connSync.Acquire();
            try
            {
                if (!acceptorSocketSessionContext.SocketReceiver.IsAcceptor) return;

                acceptorSocketSessionContext.OnDisconnecting();
                StopMessaging();
                logger.Info("Stopping publisher {0} @{1}", acceptorSocketSessionContext.ConversationDescription, 
                    acceptorSocketSessionContext.SocketConnection.ConnectedPort);
                clientsSync.Acquire();
                try
                {
                    foreach (var client in clients.Values)
                        try
                        {
                            Unregister(client);
                            client.SocketConnection.OSSocket.Close();
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

                logger.Info("Publisher {0} @{0} stopped", acceptorSocketSessionContext.ConversationDescription, 
                    acceptorSocketSessionContext.SocketConnection.ConnectedPort);
                
                acceptorSocketSessionContext.OnDisconnected();
            }
            finally
            {
                connSync.Release();
            }
        }
        
        public void RemoveClient(ISocketConversation socketConversation)
        {
            var clientSocketSessionContext = socketConversation as ISocketSessionContext;
            if (acceptorSocketSessionContext.SocketSessionState != SocketSessionState.Disconnecting)
            {
                Unregister(clientSocketSessionContext);
                try
                {
                    logger.Info("Client {0} (" + clientSocketSessionContext.SocketConnection.ConnectedPort + ") disconnected from server {1} @{2}",
                        clientSocketSessionContext.Id, clientSocketSessionContext.ConversationDescription, clientSocketSessionContext.SocketConnection.ConnectedPort);
                    clientSocketSessionContext.SocketConnection.OSSocket.Close();
                }
                catch
                {
                    // do nothing
                }
                clientsSync.Acquire();
                try
                {
                    clients.Remove(clientSocketSessionContext.Id);
                }
                finally
                {
                    clientsSync.Release();
                }
                OnClientRemoved?.Invoke(clientSocketSessionContext);
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
                    {
                        clientSessionOperator.ConversationPublisher.Send(message);
                    }
                }
                finally
                {
                    clientsSync.Release();
                }
            }
        }

        private void Unregister(ISocketSessionContext clientSocketSessionContext)
        {
            acceptorSocketSessionContext.SocketDispatcher.Listener.UnregisterForListen(clientSocketSessionContext.SocketReceiver);
        }

        private ISocketSessionContext RegisterSocketAsTcpRequestResponseRequester(IOSSocket socket)
        {
            socket.SendBufferSize = acceptorSocketSessionContext.SocketConnectionConfig.SendBufferSize;
            socket.ReceiveBufferSize = acceptorSocketSessionContext.SocketConnectionConfig.ReceiveBufferSize;

            var socketSessionConnection = new SocketSessionContext(ConversationType.RequestResponseRequester,
                SocketConversationProtocol.TCPClient, acceptorSocketSessionContext.ConversationDescription, 
                acceptorSocketSessionContext.SocketConnectionConfig,
                acceptorSocketSessionContext.SocketFactories, acceptorSocketSessionContext.SerdesFactory);
            var ipEndPoint = socket.RemoteOrLocalIPEndPoint();
            socketSessionConnection.SocketDispatcher = acceptorSocketSessionContext.SocketDispatcher;
            socketSessionConnection.OnConnected(new SocketConnection(acceptorSocketSessionContext.SocketConnection.InstanceName,
                socketSessionConnection.ConversationType,
                socket, ipEndPoint.Address, (ushort)ipEndPoint.Port));
            return socketSessionConnection;
        }
    }
}