using System.Collections.Generic;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization
{
    [TestClass]
    public class PQHeartbeatSerializerTests
    {
        private ReadWriteBuffer readWriteBuffer;
        private PQHeartbeatSerializer pqHeartBeatSerializer;
        private const int BufferReadWriteOffset = 5;

        private SourceTickerQuoteInfo firstQuoteInfo;
        private SourceTickerQuoteInfo secondQuoteInfo;
        private SourceTickerQuoteInfo thirdQuoteInfo;
        private SourceTickerQuoteInfo fourthQuoteInfo;

        private PQLevel0Quote firstQuote;
        private PQLevel1Quote secondQuote;
        private PQLevel2Quote thirdQuote;
        private PQLevel3Quote fourthQuote;

        private PQHeartBeatQuotesMessage firstBatchOfQuotes;
        private PQHeartBeatQuotesMessage secondBatchOfQuotes;

        private uint retryWaitMs = 2000;
        private bool allowCatchup = true;

        [TestInitialize]
        public void SetUp()
        {
            firstQuoteInfo = new SourceTickerClientAndPublicationConfig(1, "TestSource",
                "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
                | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                          | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, null, 
                retryWaitMs, allowCatchup);
            secondQuoteInfo = new SourceTickerClientAndPublicationConfig(2, "TestSource",
                "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
                | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                          | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, null, 
                retryWaitMs, allowCatchup);
            thirdQuoteInfo = new SourceTickerClientAndPublicationConfig(3, "TestSource",
                "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
                | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                          | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, null, 
                retryWaitMs, allowCatchup);
            fourthQuoteInfo = new SourceTickerClientAndPublicationConfig(4, "TestSource",
                "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
                | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                          | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, null, 
                retryWaitMs, allowCatchup);

            firstQuote = new PQLevel0Quote(firstQuoteInfo);
            secondQuote = new PQLevel1Quote(secondQuoteInfo);
            thirdQuote = new PQLevel2Quote(thirdQuoteInfo);
            fourthQuote = new PQLevel3Quote(fourthQuoteInfo);

            firstBatchOfQuotes = new PQHeartBeatQuotesMessage(new List<IPQLevel0Quote>{ firstQuote, secondQuote, thirdQuote});
            secondBatchOfQuotes = new PQHeartBeatQuotesMessage(new List<IPQLevel0Quote>{ thirdQuote, fourthQuote});

            readWriteBuffer = new ReadWriteBuffer(new byte[9000]) {ReadCursor = BufferReadWriteOffset};

            pqHeartBeatSerializer = new PQHeartbeatSerializer();
        }

        [TestMethod]
        public unsafe void TwoQuotesBatchToHeartBeat_Serialize_SetTheExpectedBytesToBuffer()
        {
            var amtWritten = pqHeartBeatSerializer
                .Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, firstBatchOfQuotes);
            readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

            fixed (byte* bufferPtr = readWriteBuffer.Buffer)
            {
                byte* startWritten = bufferPtr + BufferReadWriteOffset;
                byte* currPtr = bufferPtr + BufferReadWriteOffset;
                byte protocolVersion = *currPtr++;
                Assert.AreEqual(1, protocolVersion);
                byte messageFlags = *currPtr++;
                Assert.AreEqual((byte)PQBinaryMessageFlags.IsHeartBeat, messageFlags);
                var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
                Assert.AreEqual((uint)amtWritten, messagesTotalSize);
                foreach (var firstBatchOfQuote in firstBatchOfQuotes)
                {
                    var sourceTickerId = StreamByteOps.ToUInt(ref currPtr);
                    Assert.AreEqual(sourceTickerId, firstBatchOfQuote.SourceTickerQuoteInfo.Id);
                    var sequenceNumber = StreamByteOps.ToUInt(ref currPtr);
                    Assert.AreEqual(sequenceNumber, firstBatchOfQuote.PQSequenceId);
                }
                Assert.AreEqual(amtWritten, currPtr - startWritten);
            }

            amtWritten = pqHeartBeatSerializer
                .Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, secondBatchOfQuotes);
            readWriteBuffer.WrittenCursor += amtWritten;

            fixed (byte* bufferPtr = readWriteBuffer.Buffer)
            {
                byte* startWritten = bufferPtr + readWriteBuffer.ReadCursor;
                byte* currPtr = bufferPtr + readWriteBuffer.ReadCursor;
                byte protocolVersion = *currPtr++;
                Assert.AreEqual(1, protocolVersion);
                byte messageFlags = *currPtr++;
                Assert.AreEqual((byte)PQBinaryMessageFlags.IsHeartBeat, messageFlags);
                var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
                Assert.AreEqual((uint)amtWritten, messagesTotalSize);
                foreach (var firstBatchOfQuote in secondBatchOfQuotes)
                {
                    var sourceTickerId = StreamByteOps.ToUInt(ref currPtr);
                    Assert.AreEqual(sourceTickerId, firstBatchOfQuote.SourceTickerQuoteInfo.Id);
                    var sequenceNumber = StreamByteOps.ToUInt(ref currPtr);
                    Assert.AreEqual(sequenceNumber, firstBatchOfQuote.PQSequenceId);
                }
                Assert.AreEqual(amtWritten, currPtr - startWritten);
            }
        }

        [TestMethod]
        public void FullBuffer_Serialize_WritesNothingReturnsNegativeWrittenBytes()
        {
            var amtWritten = pqHeartBeatSerializer
                .Serialize(readWriteBuffer.Buffer, readWriteBuffer.Buffer.Length - 1, firstBatchOfQuotes);

            Assert.AreEqual(-1, amtWritten);
        }

        [TestMethod]
        public unsafe void AlmostFullBuffer_Serialize_WritesHeaderReturnsNegativeWrittenAmount()
        {
            var amtWritten = pqHeartBeatSerializer
                .Serialize(readWriteBuffer.Buffer, readWriteBuffer.Buffer.Length - 8, firstBatchOfQuotes);

            Assert.AreEqual(-1, amtWritten);
            
            fixed (byte* bufferPtr = readWriteBuffer.Buffer)
            {
                byte* currPtr = bufferPtr + readWriteBuffer.Buffer.Length - 8;
                byte protocolVersion = *currPtr++;
                Assert.AreEqual(1, protocolVersion);
                byte messageFlags = *currPtr++;
                Assert.AreEqual((byte) PQBinaryMessageFlags.IsHeartBeat, messageFlags);
            }
        }
    }
}