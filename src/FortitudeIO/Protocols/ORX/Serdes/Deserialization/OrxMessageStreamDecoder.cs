#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public interface IOrxResponderStreamDecoder : IMessageStreamDecoder
{
    new IOrxDeserializationRepository MessageDeserializationRepository { get; }
}

public sealed class OrxMessageStreamDecoder : IOrxResponderStreamDecoder
{
    private IOrxDeserializationRepository messageDeserializationRepository;
    private ushort messageId;

    private State state = State.Header;

    public OrxMessageStreamDecoder(IOrxDeserializationRepository deserializersRepo) => MessageDeserializationRepository = deserializersRepo;

    public int ExpectedSize { get; private set; } = OrxMessageHeader.HeaderSize;

    public IOrxDeserializationRepository MessageDeserializationRepository { get; }

    IMessageDeserializationRepository IMessageStreamDecoder.MessageDeserializationRepository => MessageDeserializationRepository;


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
                    var version = *ptr++;
                    messageId = StreamByteOps.ToUShort(ref ptr);
                    ExpectedSize = StreamByteOps.ToUShort(ref ptr);
                    socketBufferReadContext.MessageHeader = new MessageHeader(version, 0, messageId, (uint)ExpectedSize, socketBufferReadContext);
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
