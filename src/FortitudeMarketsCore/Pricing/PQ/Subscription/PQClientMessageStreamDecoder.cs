#region

using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

internal sealed class PQClientMessageStreamDecoder : IMessageStreamDecoder
{
    private static readonly IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly PQQuoteTransmissionHeader msgHeader;
    private PQBinaryMessageFlags messageFlags;
    private MessageSection messageSection;

    private uint messagesTotalSize;

    public PQClientMessageStreamDecoder(IMap<uint, IMessageDeserializer> deserializers, PQFeedType feed)
    {
        Deserializers = deserializers;
        messageSection = MessageSection.TransmissionHeader;
        ExpectedSize = PQQuoteMessageHeader.HeaderSize;

        msgHeader = new PQQuoteTransmissionHeader(feed);
    }

    public IMap<uint, IMessageDeserializer> Deserializers { get; set; }

    public int ExpectedSize { get; private set; }

    public bool ZeroByteReadIsDisconnection => true;

    public int NumberOfReceivesPerPoll => 50;

    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializers => Deserializers;

    public bool AddMessageDeserializer(uint msgId, IMessageDeserializer deserializer)
    {
        try
        {
            Deserializers[msgId] = deserializer;
        }
        catch (Exception ex)
        {
            Logger.Warn($"Caught exception attempting to add msgId: {msgId}.  Got {ex}");
            return false;
        }

        return true;
    }

    public unsafe int Process(ReadSocketBufferContext readSocketBufferContext)
    {
        var read = readSocketBufferContext.EncodedBuffer!.ReadCursor;
        var originalRead = readSocketBufferContext.EncodedBuffer.ReadCursor;
        readSocketBufferContext.MessageHeader = msgHeader;
        while (ExpectedSize <= readSocketBufferContext.EncodedBuffer.WriteCursor - read)
            switch (messageSection)
            {
                case MessageSection.TransmissionHeader:
                    fixed (byte* fptr = readSocketBufferContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        readSocketBufferContext.MessageVersion = *ptr++;
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
                    fixed (byte* fptr = readSocketBufferContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read + PQQuoteMessageHeader.SourceTickerIdOffset;
                        var sourceTickerId = StreamByteOps.ToUInt(ref ptr);
                        if (Deserializers.TryGetValue(sourceTickerId, out var bu))
                        {
                            readSocketBufferContext.EncodedBuffer.ReadCursor = read;
                            bu!.Deserialize(readSocketBufferContext);
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
                    readSocketBufferContext.EncodedBuffer.ReadCursor = read;
                    var streamId = readSocketBufferContext.ReadCurrentMessageSourceTickerId();
                    readSocketBufferContext.MessageSize = ExpectedSize;
                    if (Deserializers.TryGetValue(streamId, out var u))
                    {
                        u!.Deserialize(readSocketBufferContext);
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
            read = readSocketBufferContext.EncodedBuffer.WriteCursor;
            messageSection = MessageSection.TransmissionHeader;
            ExpectedSize = PQQuoteMessageHeader.HeaderSize;
        }

        var amountRead = read - originalRead;

        readSocketBufferContext.EncodedBuffer.ReadCursor = read;
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
