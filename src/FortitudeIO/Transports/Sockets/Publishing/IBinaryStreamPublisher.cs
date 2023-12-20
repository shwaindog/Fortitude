#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public interface IBinaryStreamPublisher
{
    int SendBufferSize { get; }
    int RegisteredSerializersCount { get; }
    void RegisterSerializer<TM>(uint msgId) where TM : class, new();
    void UnregisterSerializer(uint msgId);
    void Enqueue(ISessionConnection cx, IVersionedMessage message);
}
