#region

using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public interface IPQPublisher : ISocketPublisher
{
    ISocketSubscriber SocketStreamFromSubscriber { get; }
}
