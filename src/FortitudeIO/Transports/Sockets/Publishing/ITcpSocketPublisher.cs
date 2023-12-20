#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public interface ITcpSocketPublisher : ISocketPublisher
{
    ISocketSessionConnection RegisterAcceptor(IOSSocket socket, Action<ISocketSessionConnection> acceptor);
    void RemoveClient(ISocketSessionConnection client);
    void Broadcast(IVersionedMessage message);
}
