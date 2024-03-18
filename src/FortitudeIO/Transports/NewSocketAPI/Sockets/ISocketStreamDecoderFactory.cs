#region

using System.Collections;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public class SocketStreamDecoderFactory : IStreamDecoderFactory
{
    private readonly IMessageStreamDecoder messageStreamDecoder;

    public SocketStreamDecoderFactory(IMessageStreamDecoder messageStreamDecoder) =>
        this.messageStreamDecoder = messageStreamDecoder;


    public IMessageStreamDecoder Supply() => messageStreamDecoder;
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
