#region

using System.Reflection;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQClientMessageStreamDecoder : IMessageStreamDecoder
{
    event Action? ReceivedMessage;
    event Action? ReceivedData;
}

internal sealed class PQClientMessageStreamDecoder : IPQClientMessageStreamDecoder
{
    private static readonly IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly PQQuoteTransmissionHeader msgHeader;
    private PQBinaryMessageFlags messageFlags;
    private MessageSection messageSection;

    private uint messagesTotalSize;

    public PQClientMessageStreamDecoder(IMessageDeserializationRepository messageDeserializationRepository, PQFeedType feed)
    {
        MessageDeserializationRepository = messageDeserializationRepository;
        messageSection = MessageSection.TransmissionHeader;
        ExpectedSize = PQQuoteMessageHeader.HeaderSize;

        msgHeader = new PQQuoteTransmissionHeader(feed);
    }

    public int ExpectedSize { get; private set; }

    public IMessageDeserializationRepository MessageDeserializationRepository { get; }

    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        var read = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        var originalRead = socketBufferReadContext.EncodedBuffer.ReadCursor;
        socketBufferReadContext.MessageHeader = msgHeader;
        while (ExpectedSize <= socketBufferReadContext.EncodedBuffer.WriteCursor - read)
            switch (messageSection)
            {
                case MessageSection.TransmissionHeader:
                    fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        socketBufferReadContext.MessageVersion = *ptr++;
                        messageFlags = (PQBinaryMessageFlags)(*ptr++);
                        messagesTotalSize = StreamByteOps.ToUInt(ref ptr);
                        messageSection = (messageFlags & PQBinaryMessageFlags.IsHeartBeat)
                                         == PQBinaryMessageFlags.IsHeartBeat ?
                            MessageSection.HeartBeats :
                            MessageSection.MessageData;
                        if (messagesTotalSize > 0)
                            ExpectedSize = (int)messagesTotalSize;
                    }

                    break;
                case MessageSection.HeartBeats:
                    fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read + PQQuoteMessageHeader.SourceTickerIdOffset;
                        var sourceTickerId = StreamByteOps.ToUInt(ref ptr);
                        if (MessageDeserializationRepository.TryGetDeserializer(sourceTickerId, out var bu))
                        {
                            socketBufferReadContext.EncodedBuffer.ReadCursor = read;
                            bu!.Deserialize(socketBufferReadContext);
                            ReceivedMessage?.Invoke();
                        }
                        else
                        {
                            ReceivedData?.Invoke();
                        }
                    }

                    read += ExpectedSize;
                    ExpectedSize = PQQuoteMessageHeader.HeaderSize;
                    messageSection = MessageSection.TransmissionHeader;
                    break;
                case MessageSection.MessageData:
                    socketBufferReadContext.EncodedBuffer.ReadCursor = read;
                    var streamId = socketBufferReadContext.ReadCurrentMessageSourceTickerId();
                    socketBufferReadContext.MessageSize = ExpectedSize;
                    if (MessageDeserializationRepository.TryGetDeserializer(streamId, out var u))
                    {
                        u!.Deserialize(socketBufferReadContext);
                        ReceivedMessage?.Invoke();
                    }
                    else
                    {
                        ReceivedData?.Invoke();
                    }

                    read += ExpectedSize;
                    ExpectedSize = PQQuoteMessageHeader.HeaderSize;
                    messageSection = MessageSection.TransmissionHeader;
                    break;
            }

        if (ExpectedSize > 1048576 / 2)
        {
            Logger.Error($"The value to read from the socket {ExpectedSize:N0}B is larger than any PQ message is " +
                         "expected to be.  Resetting socket read location as it is probably corrupt.");
            read = socketBufferReadContext.EncodedBuffer.WriteCursor;
            messageSection = MessageSection.TransmissionHeader;
            ExpectedSize = PQQuoteMessageHeader.HeaderSize;
        }

        var amountRead = read - originalRead;

        socketBufferReadContext.EncodedBuffer.ReadCursor = read;
        return amountRead;
    }

    public event Action? ReceivedMessage;
    public event Action? ReceivedData;

    private enum MessageSection
    {
        TransmissionHeader
        , HeartBeats
        , MessageData
    }
}
