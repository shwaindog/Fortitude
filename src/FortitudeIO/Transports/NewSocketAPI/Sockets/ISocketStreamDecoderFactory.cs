#region

using System.Collections;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public class SocketStreamDecoderFactory : IStreamDecoderFactory
{
    protected readonly IMap<uint, IMessageDeserializer> DeserializersMap
        = new ConcurrentMap<uint, IMessageDeserializer>();

    private readonly Func<IMap<uint, IMessageDeserializer>, IMessageStreamDecoder> messageStreamDecoderFactory;

    public SocketStreamDecoderFactory(
        Func<IMap<uint, IMessageDeserializer>, IMessageStreamDecoder> messageStreamDecoderFactory) =>
        this.messageStreamDecoderFactory = messageStreamDecoderFactory;

    public int RegisteredDeserializerCount => DeserializersMap.Count;
    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializers => DeserializersMap;

    public void RegisterMessageDeserializer(uint id, IMessageDeserializer messageSerializer)
    {
        DeserializersMap[id] = messageSerializer;
    }

    public void UnregisterMessageDeserializer(uint id)
    {
        DeserializersMap.Remove(id);
    }

    public IMessageStreamDecoder Supply() => messageStreamDecoderFactory(DeserializersMap);
}

public class SocketStreamMessageEncoderFactory : IStreamEncoderFactory
{
    private readonly IDictionary<uint, IMessageSerializer> messageSerializerLookup;

    public SocketStreamMessageEncoderFactory(IDictionary<uint, IMessageSerializer> messageSerializerLookup) =>
        this.messageSerializerLookup = messageSerializerLookup;

    public int RegisteredSerializerCount => messageSerializerLookup.Count;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<uint, IMessageSerializer>> GetEnumerator() =>
        messageSerializerLookup.GetEnumerator();

    public IMessageSerializer<T> MessageEncoder<T>(IBufferContext bufferContext, T message)
        where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)messageSerializerLookup[message.MessageId];

    public IMessageSerializer<T> MessageEncoder<T>(T message) where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)messageSerializerLookup[message.MessageId];

    public IMessageSerializer<T> MessageEncoder<T>(uint id) where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)messageSerializerLookup[id];

    public IMessageSerializer MessageEncoder(uint id) => messageSerializerLookup[id];

    public IMessageSerializer MessageEncoder(IBufferContext bufferContext, uint id) => messageSerializerLookup[id];

    public void RegisterMessageSerializer(uint id, IMessageSerializer messageSerializer)
    {
        messageSerializerLookup[id] = messageSerializer;
    }

    public void UnregisterMessageSerializer(uint id)
    {
        messageSerializerLookup.Remove(id);
    }
}
