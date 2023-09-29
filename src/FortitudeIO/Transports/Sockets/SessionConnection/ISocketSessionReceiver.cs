using System;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Sockets;

namespace FortitudeIO.Transports.Sockets.SessionConnection
{
    public interface ISocketSessionReceiver : ISessionReceiver, ISocketSession
    {
        ISocketSessionConnection Parent { get; set; }
        bool IsAcceptor { get; }
        event Action<ISocketSessionConnection> Accept;
        void OnAccept();

        bool ZeroBytesReadIsDisconnection { get; set; }
        IOSSocket AcceptClientSocketRequest();
    }
}