#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public interface IOrxResponderStreamDecoder : IMessageStreamDecoder { }

public sealed class OrxMessageStreamDecoder : IOrxResponderStreamDecoder
{
    private readonly IMap<uint, IMessageDeserializer> deserializers;

    private ushort messageId;

    private State state = State.Header;

    public OrxMessageStreamDecoder(IMap<uint, IMessageDeserializer> registeredDeserializers) => deserializers = registeredDeserializers;

    public int ExpectedSize { get; private set; } = OrxMessageHeader.HeaderSize;

    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializers => deserializers;

    public bool AddMessageDeserializer(uint msgId, IMessageDeserializer deserializer)
    {
        deserializers.Add(msgId, deserializer);
        return deserializers[msgId] == deserializer;
    }

    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        var read = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        var originalRead = socketBufferReadContext.EncodedBuffer.ReadCursor;
        while (ExpectedSize <= socketBufferReadContext.EncodedBuffer.WriteCursor - read)
            if (state == State.Header)
            {
                fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                {
                    var ptr = fptr + read;
                    socketBufferReadContext.MessageVersion = *ptr++;
                    messageId = StreamByteOps.ToUShort(ref ptr);
                    ExpectedSize = StreamByteOps.ToUShort(ref ptr);
                    socketBufferReadContext.MessageSize = ExpectedSize + OrxMessageHeader.HeaderSize;
                }

                state = State.Data;
            }
            else
            {
                if (deserializers.TryGetValue(messageId, out var u))
                {
                    socketBufferReadContext.EncodedBuffer.ReadCursor = read;
                    u!.Deserialize(socketBufferReadContext);
                }

                read += ExpectedSize + OrxMessageHeader.HeaderSize;
                state = State.Header;
                ExpectedSize = OrxMessageHeader.HeaderSize;
            }

        socketBufferReadContext.DispatchLatencyLogger?.Dedent();
        var amountRead = read - originalRead;
        socketBufferReadContext.EncodedBuffer.ReadCursor = read;
        return amountRead;
    }

    private enum State
    {
        Header
        , Data
    }
}
