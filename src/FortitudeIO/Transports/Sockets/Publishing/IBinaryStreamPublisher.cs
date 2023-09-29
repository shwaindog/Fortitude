using FortitudeIO.Protocols;
using FortitudeIO.Sockets;

namespace FortitudeIO.Transports.Sockets.Publishing
{
    public interface IBinaryStreamPublisher
    {
        int SendBufferSize { get; }
        void RegisterSerializer<TM>(uint msgId) where TM : class;
        void UnregisterSerializer(uint msgId);
        int RegisteredSerializersCount { get; }
        void Enqueue(ISessionConnection cx, IVersionedMessage message);
    }
}