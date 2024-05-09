#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public interface IOrxStreamDecoder : IMessageStreamDecoder
{
    new IConversationDeserializationRepository MessageDeserializationRepository { get; }
}

public sealed class OrxMessageStreamDecoder : IOrxStreamDecoder
{
    private uint messageId;

    private State state = State.Header;

    public OrxMessageStreamDecoder(IConversationDeserializationRepository deserializersRepo) => MessageDeserializationRepository = deserializersRepo;

    public uint ExpectedSize { get; private set; } = OrxMessageHeader.HeaderSize;

    public IConversationDeserializationRepository MessageDeserializationRepository { get; }

    IMessageDeserializationRepository IMessageStreamDecoder.MessageDeserializationRepository => MessageDeserializationRepository;


    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        var originalRead = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        object? lastDecodedObj = null;
        while (ExpectedSize <= socketBufferReadContext.EncodedBuffer.WriteCursor - socketBufferReadContext.EncodedBuffer!.ReadCursor)
            if (state == State.Header)
            {
                fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                {
                    var ptr = fptr + socketBufferReadContext.EncodedBuffer.BufferRelativeReadCursor;
                    var version = *ptr++;
                    var flags = *ptr++;
                    messageId = StreamByteOps.ToUInt(ref ptr);
                    ExpectedSize = StreamByteOps.ToUInt(ref ptr) - MessageHeader.SerializationSize;
                    socketBufferReadContext.MessageHeader = new MessageHeader(version, flags, messageId, ExpectedSize, socketBufferReadContext);
                    socketBufferReadContext.EncodedBuffer.ReadCursor += MessageHeader.SerializationSize;
                }

                state = State.Data;
            }
            else
            {
                var recycleable = lastDecodedObj as IRecyclableObject;
                recycleable?.DecrementRefCount();
                if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var messageDeserializer))
                {
                    lastDecodedObj = messageDeserializer!.Deserialize(socketBufferReadContext);
                    if (lastDecodedObj is ExpectSessionCloseMessage expectSessionCloseMessage)
                        socketBufferReadContext.SocketReceiver.ExpectSessionCloseMessage = expectSessionCloseMessage;
                }

                socketBufferReadContext.EncodedBuffer.ReadCursor += (int)ExpectedSize;
                state = State.Header;
                ExpectedSize = OrxMessageHeader.HeaderSize;
            }

        socketBufferReadContext.DispatchLatencyLogger?.Dedent();
        var amountRead = socketBufferReadContext.EncodedBuffer!.ReadCursor - originalRead;
        return (int)amountRead;
    }

    private enum State
    {
        Header
        , Data
    }
}
