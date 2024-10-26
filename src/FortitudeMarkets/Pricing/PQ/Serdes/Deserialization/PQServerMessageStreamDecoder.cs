#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public interface IPQServerMessageStreamDecoder : IMessageStreamDecoder
{
    new IConversationDeserializationRepository MessageDeserializationRepository { get; }
}

public sealed class PQServerMessageStreamDecoder : IPQServerMessageStreamDecoder
{
    private const int HeaderSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(uint);
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQServerMessageStreamDecoder));
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
        using var fixedBuffer = socketBufferReadContext.EncodedBuffer!;
        var originalRead = fixedBuffer.ReadCursor;
        byte messageFlags = 0;
        uint messageId = 0;

        while (ExpectedSize <= fixedBuffer.WriteCursor - fixedBuffer.ReadCursor)
            switch (messageSection)
            {
                case MessageSection.Header:
                    var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
                    var version = *ptr++;
                    messageFlags = *ptr++;
                    messageId = StreamByteOps.ToUInt(ref ptr);
                    var messageSize = StreamByteOps.ToUInt(ref ptr);
                    socketBufferReadContext.MessageHeader
                        = new MessageHeader(version, messageFlags, messageId, messageSize, socketBufferReadContext);

                    fixedBuffer.ReadCursor += HeaderSize;
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

                    if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var messageDeserializer))
                    {
                        var message = messageDeserializer!.Deserialize(socketBufferReadContext);
                        if (message is ExpectSessionCloseMessage expectSessionCloseMessage)
                            socketBufferReadContext.SocketReceiver.ExpectSessionCloseMessage = expectSessionCloseMessage;
                    }
                    else
                    {
                        Logger.Warn("Received a message with an unregistered serializer so was ignored {0}", socketBufferReadContext.MessageHeader);
                    }

                    fixedBuffer.ReadCursor += socketBufferReadContext.LastReadLength - HeaderSize;
                    messageSection = MessageSection.Header;
                    ExpectedSize = HeaderSize;
                    break;
            }

        return (int)(fixedBuffer.ReadCursor - originalRead);
    }

    private enum MessageSection
    {
        Header
        , Data
    }
}
