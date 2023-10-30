using System;
using System.Collections.Generic;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization
{
    public class PQQuoteDeserializationSequencedTestDataBuilder
    {
        private readonly IList<IPQLevel0Quote> expectedQuotes;
        private readonly QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();
        private readonly IPerfLogger perfLogger;
        private const int MessageHeaderByteSize = 14;
        private const int BufferReadWriteOffset = 20;
        private const int UDP_DATAGRAM_PAYLOAD_SIZE = 65_507;
        private readonly int BUFFER_SIZE = (int)( 2 * Math.Pow(2, 20));


        public PQQuoteDeserializationSequencedTestDataBuilder(IList<IPQLevel0Quote> expectedQuotes, 
            IPerfLogger perfLogger)
        {
            this.expectedQuotes = expectedQuotes;
            this.perfLogger = perfLogger;
        }

        internal IList<IList<DispatchContext>> BuildQuotesStartingAt(int sequenceId, int numberBatches,
            IList<int> snapshotSequenceIds)
        {
            IList<IList<DispatchContext>> sequenceIdBatches = new List<IList<DispatchContext>>();

            for (var i = sequenceId; i < sequenceId + numberBatches; i++)
            {
                var isSnapshot = snapshotSequenceIds?.Contains(i) ?? false;
                quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, i);
                var currentBatch = BuildSerializeContextForQuotes(expectedQuotes,
                    isSnapshot ? PQFeedType.Snapshot : PQFeedType.Update, (uint)i);
                sequenceIdBatches.Add(currentBatch);
            }
            return sequenceIdBatches;
        }
        

        internal IList<DispatchContext> BuildSerializeContextForQuotes(
            IList<IPQLevel0Quote> serializeQuotes, PQFeedType feedType, uint sequenceId)
        {
            var deserializeContexts = new List<DispatchContext>(
                serializeQuotes.Count);
            var quoteSerializer = new PQQuoteSerializer(UpdateStyle.FullSnapshot);
            foreach (var quote in serializeQuotes)
            {
                var sequenceIdTimeSpan = TimeOffsetForSequenceId(sequenceId);
                var dispatchContext = new DispatchContext
                {
                    DetectTimestamp = ClientReceivedTimestamp(sequenceIdTimeSpan),
                    ReceivingTimestamp = RecevingTimestampBaseTime(sequenceIdTimeSpan),
                    EncodedBuffer = new ReadWriteBuffer(new byte[UDP_DATAGRAM_PAYLOAD_SIZE*4]),
                    DispatchLatencyLogger = perfLogger
                };

                var amountWritten = quoteSerializer.Serialize(dispatchContext.EncodedBuffer.Buffer,
                    BufferReadWriteOffset - MessageHeaderByteSize, quote);
                if (amountWritten < 0)
                {
                    throw new Exception("Serializer wrote less than expected to buffer.");
                }
                dispatchContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset;
                dispatchContext.EncodedBuffer.WrittenCursor = BufferReadWriteOffset + amountWritten -
                                                              MessageHeaderByteSize;

                dispatchContext.MessageHeader = new PQQuoteTransmissionHeader(feedType) { SequenceId = sequenceId };
                dispatchContext.MessageSize = amountWritten - MessageHeaderByteSize;
                deserializeContexts.Add(dispatchContext);
            }
            return deserializeContexts;
        }

        internal static DateTime RecevingTimestampBaseTime(TimeSpan sequenceIdTimeSpan)
        {
            return new DateTime(2017, 07, 16, 16, 31, 49).Add(sequenceIdTimeSpan);
        }

        internal static TimeSpan TimeOffsetForSequenceId(uint sequenceId)
        {
            return TimeSpan.FromMilliseconds(sequenceId * 100);
        }

        internal static DateTime ClientReceivedTimestamp(TimeSpan sequenceIdTimeSpan)
        {
            return new DateTime(2017, 07, 16, 16, 31, 48).Add(sequenceIdTimeSpan);
        }

        internal class DeserializeContext
        {
            public DeserializeContext(DispatchContext dispatchContext, int length)
            {
                DispatchContext = dispatchContext;
                Length = length;
            }

            public DispatchContext DispatchContext { get; private set; }
            public int Length { get; private set; }
        }
    }
}
