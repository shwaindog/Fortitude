#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public sealed class OrxMessageStreamDecoder : IMessageStreamDecoder
{
    public const int HeaderSize = 2 * OrxConstants.UInt16Sz + OrxConstants.UInt8Sz;

    private readonly IMap<uint, IMessageDeserializer> deserializers;

    private ushort messageId;

    private State state = State.Header;

    public OrxMessageStreamDecoder(IMap<uint, IMessageDeserializer> deserializers) =>
        this.deserializers = deserializers;

    public int ExpectedSize { get; private set; } = HeaderSize;

    public int NumberOfReceivesPerPoll => 1;

    public bool ZeroByteReadIsDisconnection => true;

    public bool AddMessageDecoder(uint msgId, IMessageDeserializer deserializer)
    {
        deserializers.Add(msgId, deserializer);
        return deserializers[msgId] == deserializer;
    }

    public unsafe int Process(DispatchContext dispatchContext)
    {
        var read = dispatchContext.EncodedBuffer!.ReadCursor;
        var originalRead = dispatchContext.EncodedBuffer.ReadCursor;
        while (ExpectedSize <= dispatchContext.EncodedBuffer.WrittenCursor - read)
            if (state == State.Header)
            {
                fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                {
                    var ptr = fptr + read;
                    dispatchContext.MessageVersion = *ptr++;
                    messageId = StreamByteOps.ToUShort(ref ptr);
                    ExpectedSize = StreamByteOps.ToUShort(ref ptr);
                    dispatchContext.MessageSize = ExpectedSize;
                }

                state = State.Data;
                read += HeaderSize;
            }
            else
            {
                if (deserializers.TryGetValue(messageId, out var u))
                {
                    dispatchContext.EncodedBuffer.ReadCursor = read;
                    u!.Deserialize(dispatchContext);
                }

                read += ExpectedSize;
                state = State.Header;
                ExpectedSize = HeaderSize;
            }

        dispatchContext.DispatchLatencyLogger?.Dedent();
        var amountRead = read - originalRead;
        dispatchContext.EncodedBuffer.ReadCursor = read;
        return amountRead;
    }

    private enum State
    {
        Header
        , Data
    }
}
