#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
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
    private const int HeaderSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(uint);
    private MessageSection messageSection;

    public PQServerMessageStreamDecoder(IConversationDeserializationRepository messageDeserializationRepository)
    {
        MessageDeserializationRepository = messageDeserializationRepository;
        messageSection = MessageSection.Header;
        ExpectedSize = HeaderSize;
    }

    public uint ExpectedSize { get; private set; }

    public IConversationDeserializationRepository MessageDeserializationRepository { get; }
    IMessageDeserializationRepository IMessageStreamDecoder.MessageDeserializationRepository => MessageDeserializationRepository;

    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        var readCursor = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        var originalRead = socketBufferReadContext.EncodedBuffer.ReadCursor;
        byte messageFlags = 0;
        uint messageId = 0;
        while (ExpectedSize <= socketBufferReadContext.EncodedBuffer.WriteCursor - readCursor)
            switch (messageSection)
            {
                case MessageSection.Header:
                    fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + readCursor;
                        var version = *ptr++;
                        messageFlags = *ptr++;
                        messageId = StreamByteOps.ToUInt(ref ptr);
                        var messageSize = StreamByteOps.ToUInt(ref ptr);
                        socketBufferReadContext.MessageHeader
                            = new MessageHeader(version, messageFlags, messageId, messageSize, socketBufferReadContext);
                    }

                    readCursor += HeaderSize;
                    if ((int)socketBufferReadContext.MessageHeader.MessageSize - HeaderSize >= 0)
                    {
                        messageSection = MessageSection.Data;
                        ExpectedSize = socketBufferReadContext.MessageHeader.MessageSize - HeaderSize;
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
