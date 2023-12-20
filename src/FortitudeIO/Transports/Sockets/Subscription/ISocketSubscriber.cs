#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Transports.Sockets.Subscription;

public interface ISocketSubscriber : ISocketLinkInitiator
{
    bool IsConnected { get; }
    void BlockUntilConnected();
    ISocketSessionConnection RegisterConnector(IOSSocket socket);

    void Connect();
    void Disconnect();

    void Send(IVersionedMessage message);
    event Action OnConnected;
    event Action OnDisconnecting;
    event Action OnDisconnected;
}
