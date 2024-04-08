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
        var readCursor = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        var originalRead = socketBufferReadContext.EncodedBuffer.ReadCursor;
        object? lastDecodedObj = null;
        while (ExpectedSize <= socketBufferReadContext.EncodedBuffer.WriteCursor - readCursor)
            if (state == State.Header)
            {
                fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                {
                    var ptr = fptr + readCursor;
                    socketBufferReadContext.MessageVersion = *ptr++;
                    messageId = StreamByteOps.ToUShort(ref ptr);
                    ExpectedSize = StreamByteOps.ToUShort(ref ptr);
                    socketBufferReadContext.MessageSize = ExpectedSize + OrxMessageHeader.HeaderSize;
                }

                state = State.Data;
            }
            else
            {
                var recycleable = lastDecodedObj as IRecyclableObject;
                recycleable?.DecrementRefCount();
                if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var u))
                {
                    socketBufferReadContext.EncodedBuffer.ReadCursor = readCursor;
                    lastDecodedObj = u!.Deserialize(socketBufferReadContext);
                }

                readCursor += ExpectedSize + OrxMessageHeader.HeaderSize;
                state = State.Header;
                ExpectedSize = OrxMessageHeader.HeaderSize;
            }

        socketBufferReadContext.DispatchLatencyLogger?.Dedent();
        var amountRead = readCursor - originalRead;
        socketBufferReadContext.EncodedBuffer.ReadCursor = readCursor;
        return amountRead;
    }

    private enum State
    {
        Header
        , Data
    }
}
