#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public class PQQuoteDeserializationSequencedTestDataBuilder
{
    private const int MessageHeaderByteSize = PQQuoteMessageHeader.HeaderSize;
    private const int BufferReadWriteOffset = 20;
    private const int UDP_DATAGRAM_PAYLOAD_SIZE = 65_507;
    private readonly int BUFFER_SIZE = (int)(2 * Math.Pow(2, 20));
    private readonly IList<IPQLevel0Quote> expectedQuotes;
    private readonly IPerfLogger perfLogger;
    private readonly QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = new();

    public PQQuoteDeserializationSequencedTestDataBuilder(IList<IPQLevel0Quote> expectedQuotes,
        IPerfLogger perfLogger)
    {
        this.expectedQuotes = expectedQuotes;
        this.perfLogger = perfLogger;
    }

    internal IList<IList<SocketBufferReadContext>> BuildQuotesStartingAt(uint sequenceId, int numberBatches,
        IList<uint> snapshotSequenceIds)
    {
        IList<IList<SocketBufferReadContext>> sequenceIdBatches = new List<IList<SocketBufferReadContext>>();

        for (var i = sequenceId; i < sequenceId + numberBatches; i++)
        {
            var isSnapshot = snapshotSequenceIds?.Contains(i) ?? false;
            quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, i);
            var currentBatch = BuildSerializeContextForQuotes(expectedQuotes,
                isSnapshot ? PQMessageFlags.Snapshot : PQMessageFlags.Update, i);
            sequenceIdBatches.Add(currentBatch);
        }

        return sequenceIdBatches;
    }

    internal IList<SocketBufferReadContext> BuildSerializeContextForQuotes(
        IList<IPQLevel0Quote> serializeQuotes, PQMessageFlags feedType, uint sequenceId)
    {
        var deserializeContexts = new List<SocketBufferReadContext>(
            serializeQuotes.Count);
        var quoteSerializer
            = new PQQuoteSerializer(feedType);
        foreach (var quote in serializeQuotes)
        {
            quote.PQSequenceId = sequenceId;
            var sequenceIdTimeSpan = TimeOffsetForSequenceId(sequenceId);
            var sockBuffContext = new SocketBufferReadContext
            {
                DetectTimestamp = ClientReceivedTimestamp(sequenceIdTimeSpan)
                , ReceivingTimestamp = RecevingTimestampBaseTime(sequenceIdTimeSpan)
                , EncodedBuffer = new CircularReadWriteBuffer(new byte[UDP_DATAGRAM_PAYLOAD_SIZE * 4])
                , DispatchLatencyLogger = perfLogger
                , MessageHeader = new MessageHeader(1, 0, 0, 1)
            };

            var amountWritten = quoteSerializer.Serialize(sockBuffContext.EncodedBuffer.Buffer,
                BufferReadWriteOffset, quote);
            if (amountWritten < 0) throw new Exception("Serializer wrote less than expected to buffer.");
            sockBuffContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + PQQuoteMessageHeader.HeaderSize;
            sockBuffContext.EncodedBuffer.WriteCursor = BufferReadWriteOffset + amountWritten;
            sockBuffContext.MessageHeader = new MessageHeader(1, (byte)feedType, quote.MessageId, (uint)amountWritten, sockBuffContext);
            sockBuffContext.LastWriteLength = amountWritten;
            deserializeContexts.Add(sockBuffContext);
        }

        return deserializeContexts;
    }

    internal static DateTime RecevingTimestampBaseTime(TimeSpan sequenceIdTimeSpan) => new DateTime(2017, 07, 16, 16, 31, 49).Add(sequenceIdTimeSpan);

    internal static TimeSpan TimeOffsetForSequenceId(uint sequenceId) => TimeSpan.FromMilliseconds(sequenceId * 100);

    internal static DateTime ClientReceivedTimestamp(TimeSpan sequenceIdTimeSpan) => new DateTime(2017, 07, 16, 16, 31, 48).Add(sequenceIdTimeSpan);

    internal class DeserializeContext
    {
        public DeserializeContext(SocketBufferReadContext socketBufferReadContext, int length)
        {
            SocketBufferReadContext = socketBufferReadContext;
            Length = length;
        }

        public SocketBufferReadContext SocketBufferReadContext { get; private set; }
        public int Length { get; private set; }
    }
}
