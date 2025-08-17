#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
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
        using var fixBuffer = socketBufferReadContext.EncodedBuffer!;
        var originalRead = fixBuffer.ReadCursor;
        object? lastDecodedObj = null;

        while (ExpectedSize <= fixBuffer.WriteCursor - fixBuffer!.ReadCursor)
            if (state == State.Header)
            {
                var ptr = fixBuffer.ReadBuffer + fixBuffer.BufferRelativeReadCursor;
                var version = *ptr++;
                var flags = *ptr++;
                messageId = StreamByteOps.ToUInt(ref ptr);
                ExpectedSize = StreamByteOps.ToUInt(ref ptr) - MessageHeader.SerializationSize;
                socketBufferReadContext.MessageHeader = new MessageHeader(version, flags, messageId, ExpectedSize, socketBufferReadContext);
                fixBuffer.ReadCursor += MessageHeader.SerializationSize;

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

                fixBuffer.ReadCursor += (int)ExpectedSize;
                state = State.Header;
                ExpectedSize = OrxMessageHeader.HeaderSize;
            }

        socketBufferReadContext.DispatchLatencyLogger?.Dedent();
        var amountRead = fixBuffer!.ReadCursor - originalRead;
        return (int)amountRead;
    }

    private enum State
    {
        Header
        , Data
    }
}
