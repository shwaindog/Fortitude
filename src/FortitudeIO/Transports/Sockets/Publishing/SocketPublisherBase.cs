﻿#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public abstract class SocketPublisherBase : SocketStreamPublisher
{
    protected readonly ISyncLock clientsSync = new YieldLockLight();

    protected readonly ISyncLock connSync = new YieldLockLight();
    protected readonly IOSNetworkingController NetworkingController;
    public readonly int Port;

    protected SocketPublisherBase(IFLogger logger, ISocketDispatcher dispatcher, string sessionDescription,
        IOSNetworkingController networkingController, int port) : base(logger, dispatcher, sessionDescription)
    {
        NetworkingController = networkingController;
        Port = port;
    }

    public event Action? Connected;
    public event Action? Disconnected;


    protected void OnConnected()
    {
        Connected?.Invoke();
    }

    protected void OnDisconnected()
    {
        Disconnected?.Invoke();
    }

    public void Send(ISession client, IVersionedMessage message)
    {
        Enqueue((ISocketSessionConnection)client, message);
    }

    public void Send(ISocketSessionConnection client, IVersionedMessage message)
    {
        Enqueue(client, message);
    }

    public virtual void Disconnect()
    {
        connSync.Acquire();
        try
        {
            StopMessaging();

            Logger.Info("Server {0} @{0} stopped", SessionDescription, Port);

            OnDisconnected();
        }
        finally
        {
            connSync.Release();
        }
    }
}
