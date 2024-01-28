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

    private readonly IMap<uint, IMessageDeserializer> deserializers;

    private readonly PQQuoteTransmissionHeader msgHeader;
    private PQBinaryMessageFlags messageFlags;
    private MessageSection messageSection;

    private uint messagesTotalSize;

    public PQClientMessageStreamDecoder(IMap<uint, IMessageDeserializer> deserializers, PQFeedType feed)
    {
        this.deserializers = deserializers;
        messageSection = MessageSection.TransmissionHeader;
        ExpectedSize = PQQuoteMessageHeader.HeaderSize;

        msgHeader = new PQQuoteTransmissionHeader(feed);
    }

    public int ExpectedSize { get; private set; }

    public bool ZeroByteReadIsDisconnection => true;

    public int NumberOfReceivesPerPoll => 50;

    public bool AddMessageDecoder(uint msgId, IMessageDeserializer deserializer) =>
        throw new NotImplementedException("No deserializers required for this stream");

    public unsafe int Process(ReadSocketBufferContext readSocketBufferContext)
    {
        var read = readSocketBufferContext.EncodedBuffer!.ReadCursor;
        var originalRead = readSocketBufferContext.EncodedBuffer.ReadCursor;
        readSocketBufferContext.MessageHeader = msgHeader;
        while (ExpectedSize <= readSocketBufferContext.EncodedBuffer.WrittenCursor - read)
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
                        if (deserializers.TryGetValue(sourceTickerId, out var bu))
                        {
                            readSocketBufferContext.EncodedBuffer.ReadCursor = read;
                            bu!.Deserialize(readSocketBufferContext);
                            OnResponse?.Invoke();
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
                    if (deserializers.TryGetValue(streamId, out var u))
                    {
                        u!.Deserialize(readSocketBufferContext);
                        OnResponse?.Invoke();
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
            read = readSocketBufferContext.EncodedBuffer.WrittenCursor;
            messageSection = MessageSection.TransmissionHeader;
            ExpectedSize = PQQuoteMessageHeader.HeaderSize;
        }

        var amountRead = read - originalRead;

        readSocketBufferContext.EncodedBuffer.ReadCursor = read;
        return amountRead;
    }

    public event Action? OnResponse;

    private enum MessageSection
    {
        TransmissionHeader
        , HeartBeats
        , MessageData
    }
}
