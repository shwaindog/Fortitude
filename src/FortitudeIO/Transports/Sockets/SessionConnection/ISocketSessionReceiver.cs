#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;

#endregion

namespace FortitudeIO.Transports.Sockets.SessionConnection;

public interface ISocketSessionReceiver : ISessionReceiver, ISocketSession
{
    ISocketSessionConnection? Parent { get; set; }
    bool IsAcceptor { get; }

    bool ZeroBytesReadIsDisconnection { get; set; }
    event Action<ISocketSessionConnection> Accept;
    void OnAccept();
    IOSSocket AcceptClientSocketRequest();
}
