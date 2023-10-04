#region

using FortitudeIO.Protocols;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public interface ISocketPublisher : ISocketConnector, IBinaryStreamPublisher
{
    void Send(ISession client, IVersionedMessage message);
    void Send(ISocketSessionConnection client, IVersionedMessage message);
}
