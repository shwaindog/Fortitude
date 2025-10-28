#region

using System.Reflection;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

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

    private uint messageId;
    private MessageSection messageSection;

    private uint messageSize;

    public PQClientMessageStreamDecoder(IConversationDeserializationRepository messageDeserializationRepository)
    {
        MessageDeserializationRepository = messageDeserializationRepository;
        messageSection = MessageSection.TransmissionHeader;
        ExpectedSize = PQQuoteMessageHeader.HeaderSize;
    }

    public int ExpectedSize { get; private set; }

    public IConversationDeserializationRepository MessageDeserializationRepository { get; }

    IMessageDeserializationRepository IMessageStreamDecoder.MessageDeserializationRepository => MessageDeserializationRepository;


    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        using var fixedBuffer = socketBufferReadContext.EncodedBuffer!;
        var originalRead = fixedBuffer.ReadCursor;
        while (ExpectedSize <= fixedBuffer.WriteCursor - fixedBuffer.ReadCursor)
            switch (messageSection)
            {
                case MessageSection.TransmissionHeader:
                    var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
                    var version = *ptr++;
                    var messageFlags = *ptr++;
                    messageId = StreamByteOps.ToUInt(ref ptr);
                    messageSize = StreamByteOps.ToUInt(ref ptr);
                    messageSection = MessageSection.MessageData;
                    socketBufferReadContext.MessageHeader
                        = new MessageHeader(version, messageFlags, messageId, messageSize, socketBufferReadContext);
                    if (messageSize > 0) ExpectedSize = (int)messageSize - PQQuoteMessageHeader.HeaderSize;
                    fixedBuffer.ReadCursor += PQQuoteMessageHeader.HeaderSize;

                    break;
                case MessageSection.MessageData:
                    if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var bu))
                    {
                        var message = bu!.Deserialize(socketBufferReadContext);
                        if (message is ExpectSessionCloseMessage expectSessionCloseMessage)
                            socketBufferReadContext.SocketReceiver.ExpectSessionCloseMessage = expectSessionCloseMessage;

                        ReceivedMessage?.Invoke();
                    }
                    else
                    {
                        ReceivedData?.Invoke();
                    }

                    fixedBuffer.ReadCursor += ExpectedSize;
                    ExpectedSize = PQQuoteMessageHeader.HeaderSize;
                    messageSection = MessageSection.TransmissionHeader;
                    break;
            }

        if (ExpectedSize > 1048576 / 2)
        {
            Logger.Error($"The value to read from the socket {ExpectedSize:N0}B is larger than any PQ message is " +
                         "expected to be.  Resetting socket read location as it is probably corrupt.");
            fixedBuffer.ReadCursor = fixedBuffer.WriteCursor;
            messageSection = MessageSection.TransmissionHeader;
            ExpectedSize = PQQuoteMessageHeader.HeaderSize;
        }

        var amountRead = fixedBuffer.ReadCursor - originalRead;
        return (int)amountRead;
    }

    public event Action? ReceivedMessage;
    public event Action? ReceivedData;

    private enum MessageSection
    {
        TransmissionHeader
        , MessageData
    }
}
