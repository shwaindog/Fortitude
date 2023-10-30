using FortitudeIO.Transports.Sockets.Subscription;

namespace FortitudeIO.Transports.Sockets.Publishing
{
    public interface IPQPublisher : ISocketPublisher
    {
       ISocketSubscriber SocketStreamFromSubscriber { get; }
    }
}