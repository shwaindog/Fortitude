#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQServerMessageStreamDecoder : IMessageStreamDecoder
{
    new IConversationDeserializationRepository MessageDeserializationRepository { get; }
}

public sealed class PQServerMessageStreamDecoder : IPQServerMessageStreamDecoder
{
    private const int HeaderSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(ushort);
    private MessageSection messageSection;

    public PQServerMessageStreamDecoder(IConversationDeserializationRepository messageDeserializationRepository)
    {
        MessageDeserializationRepository = messageDeserializationRepository;
        messageSection = MessageSection.Header;
        ExpectedSize = HeaderSize;
    }

    public int ExpectedSize { get; private set; }

    public IConversationDeserializationRepository MessageDeserializationRepository { get; }
    IMessageDeserializationRepository IMessageStreamDecoder.MessageDeserializationRepository => MessageDeserializationRepository;

    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        var readCursor = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        var originalRead = socketBufferReadContext.EncodedBuffer.ReadCursor;
        byte flags = 0;
        uint messageId = 0;
        while (ExpectedSize <= socketBufferReadContext.EncodedBuffer.WriteCursor - readCursor)
            switch (messageSection)
            {
                case MessageSection.Header:
                    fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + readCursor;
                        socketBufferReadContext.MessageVersion = *ptr++;
                        flags = *ptr++;
                        messageId = StreamByteOps.ToUInt(ref ptr);
                        socketBufferReadContext.MessageSize = StreamByteOps.ToUShort(ref ptr);
                    }

                    readCursor += HeaderSize;
                    if (socketBufferReadContext.MessageSize - HeaderSize > 0)
                    {
                        messageSection = MessageSection.Data;
                        ExpectedSize = socketBufferReadContext.MessageSize - HeaderSize;
                    }
                    else
                    {
                        messageSection = MessageSection.Header;
                        ExpectedSize = HeaderSize;
                    }

                    break;
                case MessageSection.Data:

                    if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var u))
                    {
                        socketBufferReadContext.EncodedBuffer.ReadCursor = readCursor;
                        u!.Deserialize(socketBufferReadContext);
                    }

                    readCursor += socketBufferReadContext.LastReadLength - HeaderSize;
                    messageSection = MessageSection.Header;
                    ExpectedSize = HeaderSize;
                    break;
            }

        socketBufferReadContext.EncodedBuffer.ReadCursor = readCursor;
        return readCursor - originalRead;
    }

    private enum MessageSection
    {
        Header
        , Data
    }
}
