// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public class PQQuoteDeserializationSequencedTestDataBuilder
{
    private const int MessageHeaderByteSize     = PQQuoteMessageHeader.HeaderSize;
    private const int BufferReadWriteOffset     = 20;
    private const int UDP_DATAGRAM_PAYLOAD_SIZE = 65_507;

    private readonly int BUFFER_SIZE = (int)(2 * Math.Pow(2, 20));

    private readonly IList<IPQPublishableTickInstant>         expectedQuotes;
    private readonly IPerfLogger                   perfLogger;
    private readonly QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = new();

    public PQQuoteDeserializationSequencedTestDataBuilder
    (IList<IPQPublishableTickInstant> expectedQuotes,
        IPerfLogger perfLogger)
    {
        this.expectedQuotes = expectedQuotes;
        this.perfLogger     = perfLogger;
    }

    internal IList<IList<SocketBufferReadContext>> BuildQuotesStartingAt
        (uint sequenceId, int numberBatches, IList<uint> snapshotSequenceIds)
    {
        IList<IList<SocketBufferReadContext>> sequenceIdBatches = new List<IList<SocketBufferReadContext>>();

        for (var i = sequenceId; i < sequenceId + numberBatches; i++)
        {
            var isSnapshot = snapshotSequenceIds?.Contains(i) ?? false;
            quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, i);
            var currentBatch =
                BuildSerializeContextForQuotes(expectedQuotes, isSnapshot ? PQMessageFlags.Snapshot : PQMessageFlags.Update, i);
            sequenceIdBatches.Add(currentBatch);
        }

        return sequenceIdBatches;
    }

    internal IList<SocketBufferReadContext> BuildSerializeContextForQuotes
    (
        IList<IPQPublishableTickInstant> serializeQuotes, PQMessageFlags feedType, uint sequenceId)
    {
        var deserializeContexts = new List<SocketBufferReadContext>(serializeQuotes.Count);
        var quoteSerializer = new PQMessageSerializer(feedType);
        foreach (var quote in serializeQuotes)
        {
            quote.PQSequenceId = sequenceId;
            quote.HasUpdates   = true;
            var sequenceIdTimeSpan = TimeOffsetForSequenceId(sequenceId);
            var sockBuffContext = new SocketBufferReadContext
            {
                DetectTimestamp       = ClientReceivedTimestamp(sequenceIdTimeSpan)
              , ReceivingTimestamp    = RecevingTimestampBaseTime(sequenceIdTimeSpan)
              , EncodedBuffer         = new CircularReadWriteBuffer(new byte[UDP_DATAGRAM_PAYLOAD_SIZE * 4])
              , DispatchLatencyLogger = perfLogger
              , MessageHeader         = new MessageHeader(1, 0, 0, 1)
            };

            sockBuffContext.EncodedBuffer.WriteCursor = BufferReadWriteOffset;
            var amountWritten = quoteSerializer.Serialize(sockBuffContext.EncodedBuffer, quote);
            if (amountWritten < 0) throw new Exception("Serializer wrote less than expected to buffer.");
            sockBuffContext.EncodedBuffer.ReadCursor  =  BufferReadWriteOffset + PQQuoteMessageHeader.HeaderSize;
            sockBuffContext.EncodedBuffer.WriteCursor += amountWritten;

            sockBuffContext.MessageHeader   = new MessageHeader(1, (byte)feedType, quote.MessageId, (uint)amountWritten, sockBuffContext);
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
