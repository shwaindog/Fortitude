#region

using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

internal sealed class PQClientMessageStreamDecoder : IMessageStreamDecoder
{
    private const int TransmissionHeaderSize = sizeof(byte) * 2 + sizeof(uint);
    private const int TickerSourceIdSequenceNumberBodySize = 2 * sizeof(uint);

    private static readonly IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly IMap<uint, IMessageDeserializer> deserializers;

    private readonly PQQuoteTransmissionHeader msgHeader;
    private PQBinaryMessageFlags messageFlags;
    private MessageSection messageSection;

    private uint messagesTotalSize;
    private uint streamId;

    public PQClientMessageStreamDecoder(IMap<uint, IMessageDeserializer> deserializers, PQFeedType feed)
    {
        this.deserializers = deserializers;
        messageSection = MessageSection.TransmissionHeader;
        ExpectedSize = TransmissionHeaderSize;

        msgHeader = new PQQuoteTransmissionHeader(feed);
    }

    public int ExpectedSize { get; private set; }

    public bool ZeroByteReadIsDisconnection => true;

    public int NumberOfReceivesPerPoll => 50;

    public bool AddMessageDecoder(uint msgId, IMessageDeserializer deserializer) =>
        throw new NotImplementedException("No deserializers required for this stream");

    public unsafe int Process(DispatchContext dispatchContext)
    {
        var read = dispatchContext.EncodedBuffer!.ReadCursor;
        var originalRead = dispatchContext.EncodedBuffer.ReadCursor;
        dispatchContext.MessageHeader = msgHeader;
        while (ExpectedSize <= dispatchContext.EncodedBuffer.WrittenCursor - read)
            switch (messageSection)
            {
                case MessageSection.TransmissionHeader:
                    fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        dispatchContext.MessageVersion = *ptr++;
                        messageFlags = (PQBinaryMessageFlags)(*ptr++);
                        messagesTotalSize = StreamByteOps.ToUInt(ref ptr);
                        read += ExpectedSize;
                        messageSection = (messageFlags & PQBinaryMessageFlags.IsHeartBeat)
                                         == PQBinaryMessageFlags.IsHeartBeat ?
                            MessageSection.HeartBeats :
                            MessageSection.MessageData;
                        if (messagesTotalSize > 0)
                            ExpectedSize = (int)messagesTotalSize - TransmissionHeaderSize;
                    }

                    break;
                case MessageSection.HeartBeats:
                    var noBeats = ExpectedSize / TickerSourceIdSequenceNumberBodySize;
                    fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        for (var i = 0; i < noBeats; i++)
                        {
                            streamId = StreamByteOps.ToUInt(ref ptr);
                            msgHeader.SequenceId = StreamByteOps.ToUInt(ref ptr);
                            read += TickerSourceIdSequenceNumberBodySize;
                            dispatchContext.EncodedBuffer.ReadCursor = read;
                            dispatchContext.MessageSize = 0;
                            if (deserializers.TryGetValue(streamId, out var bu))
                            {
                                dispatchContext.EncodedBuffer.ReadCursor = read;
                                bu!.Deserialize(dispatchContext);
                                OnResponse?.Invoke();
                            }
                        }
                    }

                    ExpectedSize = TransmissionHeaderSize;
                    messageSection = MessageSection.TransmissionHeader;
                    break;
                case MessageSection.MessageData:
                    fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        streamId = StreamByteOps.ToUInt(ref ptr);
                        msgHeader.SequenceId = StreamByteOps.ToUInt(ref ptr);
                    }

                    read += TickerSourceIdSequenceNumberBodySize;
                    ExpectedSize -= TickerSourceIdSequenceNumberBodySize;
                    dispatchContext.MessageSize = ExpectedSize;
                    if (deserializers.TryGetValue(streamId, out var u))
                    {
                        dispatchContext.EncodedBuffer.ReadCursor = read;
                        u!.Deserialize(dispatchContext);
                        OnResponse?.Invoke();
                    }

                    read += ExpectedSize;
                    ExpectedSize = TransmissionHeaderSize;
                    messageSection = MessageSection.TransmissionHeader;
                    break;
            }

        if (ExpectedSize > 1048576 / 2)
        {
            Logger.Error($"The value to read from the socket {ExpectedSize:N0}B is larger than any PQ message is " +
                         "expected to be.  Resetting socket read location as it is probably corrupt.");
            read = dispatchContext.EncodedBuffer.WrittenCursor;
            messageSection = MessageSection.TransmissionHeader;
            ExpectedSize = TransmissionHeaderSize;
        }

        var amountRead = read - originalRead;
        dispatchContext.EncodedBuffer.ReadCursor = read;
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
