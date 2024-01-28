#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Transports.Sockets.Subscription;

public interface IBinaryStreamSubscriber
{
    int RecvBufferSize { get; }
    int WholeMessagesPerReceive { get; }
    int RegisteredDeserializersCount { get; }

    void RegisterDeserializer<Tm>(uint msgId, Action<Tm, object?, ISession?> msgHandler)
        where Tm : class, IVersionedMessage, new();

    void UnregisterDeserializer<Tm>(uint msgId, Action<Tm, object?, ISession?> msgHandler)
        where Tm : class, IVersionedMessage, new();

    IMessageStreamDecoder? GetDecoder(IMap<uint, IMessageDeserializer> deserializers);
}
