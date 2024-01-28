#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public sealed class OrxMessageStreamDecoder : IMessageStreamDecoder
{
    private readonly IMap<uint, IMessageDeserializer> deserializers;

    private ushort messageId;

    private State state = State.Header;

    public OrxMessageStreamDecoder(IMap<uint, IMessageDeserializer> deserializers) =>
        this.deserializers = deserializers;

    public int ExpectedSize { get; private set; } = OrxMessageHeader.HeaderSize;

    public int NumberOfReceivesPerPoll => 1;

    public bool ZeroByteReadIsDisconnection => true;

    public bool AddMessageDecoder(uint msgId, IMessageDeserializer deserializer)
    {
        deserializers.Add(msgId, deserializer);
        return deserializers[msgId] == deserializer;
    }

    public unsafe int Process(ReadSocketBufferContext readSocketBufferContext)
    {
        var read = readSocketBufferContext.EncodedBuffer!.ReadCursor;
        var originalRead = readSocketBufferContext.EncodedBuffer.ReadCursor;
        while (ExpectedSize <= readSocketBufferContext.EncodedBuffer.WrittenCursor - read)
            if (state == State.Header)
            {
                fixed (byte* fptr = readSocketBufferContext.EncodedBuffer.Buffer)
                {
                    var ptr = fptr + read;
                    readSocketBufferContext.MessageVersion = *ptr++;
                    messageId = StreamByteOps.ToUShort(ref ptr);
                    ExpectedSize = StreamByteOps.ToUShort(ref ptr);
                    readSocketBufferContext.MessageSize = ExpectedSize + OrxMessageHeader.HeaderSize;
                }

                state = State.Data;
            }
            else
            {
                if (deserializers.TryGetValue(messageId, out var u))
                {
                    readSocketBufferContext.EncodedBuffer.ReadCursor = read;
                    u!.Deserialize(readSocketBufferContext);
                }

                read += ExpectedSize + OrxMessageHeader.HeaderSize;
                state = State.Header;
                ExpectedSize = OrxMessageHeader.HeaderSize;
            }

        readSocketBufferContext.DispatchLatencyLogger?.Dedent();
        var amountRead = read - originalRead;
        readSocketBufferContext.EncodedBuffer.ReadCursor = read;
        return amountRead;
    }

    private enum State
    {
        Header
        , Data
    }
}
