#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Transports.Sockets.Subscription;

public interface IBinaryStreamSubscriber
{
    int RecvBufferSize { get; }
    int WholeMessagesPerReceive { get; }
    int RegisteredDeserializersCount { get; }
    void RegisterDeserializer<Tm>(uint msgId, Action<Tm, object?, ISession?> msgHandler) where Tm : class, new();
    void UnregisterDeserializer<Tm>(uint msgId, Action<Tm, object?, ISession?> msgHandler) where Tm : class, new();
    IStreamDecoder? GetDecoder(IMap<uint, IBinaryDeserializer> deserializers);
}
