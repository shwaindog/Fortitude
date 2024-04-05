#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public interface IOrxResponderStreamDecoder : IMessageStreamDecoder { }

public sealed class OrxMessageStreamDecoder : IOrxResponderStreamDecoder
{
    private ushort messageId;

    private State state = State.Header;

    public OrxMessageStreamDecoder(IMessageDeserializationRepository deserializersRepo) => MessageDeserializationRepository = deserializersRepo;

    public int ExpectedSize { get; private set; } = OrxMessageHeader.HeaderSize;

    public IMessageDeserializationRepository MessageDeserializationRepository { get; }


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
                if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var u))
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
