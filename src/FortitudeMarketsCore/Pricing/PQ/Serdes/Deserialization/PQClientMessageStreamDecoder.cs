#region

using System.Reflection;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQClientMessageStreamDecoder : IMessageStreamDecoder
{
    new IConversationDeserializationRepository MessageDeserializationRepository { get; }

    event Action? ReceivedMessage;
    event Action? ReceivedData;
}

internal sealed class PQClientMessageStreamDecoder : IPQClientMessageStreamDecoder
{
    private static readonly IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly PQQuoteTransmissionHeader msgHeader;
    private uint messageId;
    private MessageSection messageSection;

    private uint messagesTotalSize;

    public PQClientMessageStreamDecoder(IConversationDeserializationRepository messageDeserializationRepository, PQFeedType feed)
    {
        MessageDeserializationRepository = messageDeserializationRepository;
        messageSection = MessageSection.TransmissionHeader;
        ExpectedSize = PQQuoteMessageHeader.HeaderSize;

        msgHeader = new PQQuoteTransmissionHeader(feed);
    }

    public int ExpectedSize { get; private set; }

    public IConversationDeserializationRepository MessageDeserializationRepository { get; }

    IMessageDeserializationRepository IMessageStreamDecoder.MessageDeserializationRepository => MessageDeserializationRepository;


    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        var readCursor = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        var originalRead = socketBufferReadContext.EncodedBuffer.ReadCursor;
        socketBufferReadContext.MessageHeader = msgHeader;
        while (ExpectedSize <= socketBufferReadContext.EncodedBuffer.WriteCursor - readCursor)
            switch (messageSection)
            {
                case MessageSection.TransmissionHeader:
                    fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + readCursor;
                        socketBufferReadContext.MessageVersion = *ptr++;
                        msgHeader.MessageFlags = (PQMessageFlags)(*ptr++);
                        messageId = StreamByteOps.ToUInt(ref ptr);
                        messagesTotalSize = StreamByteOps.ToUInt(ref ptr);
                        messageSection = MessageSection.MessageData;
                        if (messagesTotalSize > 0) ExpectedSize = (int)messagesTotalSize;
                    }

                    break;
                case MessageSection.MessageData:
                    if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var bu))
                    {
                        socketBufferReadContext.MessageSize = (int)messagesTotalSize;
                        socketBufferReadContext.EncodedBuffer.ReadCursor = readCursor + PQQuoteMessageHeader.HeaderSize;
                        bu!.Deserialize(socketBufferReadContext);
                        ReceivedMessage?.Invoke();
                    }
                    else
                    {
                        ReceivedData?.Invoke();
                    }

                    readCursor += ExpectedSize;
                    ExpectedSize = PQQuoteMessageHeader.HeaderSize;
                    messageSection = MessageSection.TransmissionHeader;
                    break;
            }

        if (ExpectedSize > 1048576 / 2)
        {
            Logger.Error($"The value to read from the socket {ExpectedSize:N0}B is larger than any PQ message is " +
                         "expected to be.  Resetting socket read location as it is probably corrupt.");
            readCursor = socketBufferReadContext.EncodedBuffer.WriteCursor;
            messageSection = MessageSection.TransmissionHeader;
            ExpectedSize = PQQuoteMessageHeader.HeaderSize;
        }

        var amountRead = readCursor - originalRead;

        socketBufferReadContext.EncodedBuffer.ReadCursor = readCursor;
        return amountRead;
    }

    public event Action? ReceivedMessage;
    public event Action? ReceivedData;

    private enum MessageSection
    {
        TransmissionHeader
        , MessageData
    }
}
