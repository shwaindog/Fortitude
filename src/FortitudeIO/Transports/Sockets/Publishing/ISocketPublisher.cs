#region

using FortitudeIO.Protocols;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public interface ISocketPublisher : ISocketConnector
{
    void Send(ISession client, IVersionedMessage message);
    void Send(ISocketSessionContext client, IVersionedMessage message);
}
