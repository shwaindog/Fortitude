#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeTests.FortitudeIO.Transports.Network.Config;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQQuoteSerializerTests
{
    private const int BufferReadWriteOffset = 5;
    private readonly bool allowCatchup = true;
    private readonly uint retryWaitMs = 2000;
    private PQClientQuoteDeserializerRepository deserializerRepository = null!;

    private IReadOnlyList<IPQLevel0Quote> differingQuotes = null!;
    private PQLevel2Quote everyLayerL2Quote = null!;
    private ISourceTickerQuoteInfo everyLayerQuoteInfo = null!;
    private DateTime frozenDateTime;
    private PQLevel0Quote level0Quote = null!;
    private ISourceTickerQuoteInfo level0QuoteInfo = null!;
    private PQLevel1Quote level1Quote = null!;
    private ISourceTickerQuoteInfo level1QuoteInfo = null!;

    private Mock<ITimeContext> moqTimeContext = null!;
    private PQClientMessageStreamDecoder pqClientMessageStreamDecoder = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private ReadWriteBuffer readWriteBuffer = null!;
    private PQLevel3Quote simpleNoRecentlyTradedL3Quote = null!;
    private ISourceTickerQuoteInfo simpleNoRecentlyTradedQuoteInfo = null!;
    private PQQuoteSerializer snapshotQuoteSerializer = null!;
    private ISourceTickerQuoteInfo srcNmLstTrdQuoteInfo = null!;
    private PQLevel3Quote srcNmSmplRctlyTrdedL3Quote = null!;
    private PQLevel3Quote srcQtRefPdGvnVlmRcntlyTrdedL3Quote = null!;
    private ISourceTickerQuoteInfo srcQtRfPdGvnVlmQuoteInfo = null!;
    private byte[] testBuffer = null!;
    private ISourceTickerQuoteInfo trdrLyrTrdrPdGvnVlmDtlsQuoteInfo = null!;
    private PQLevel3Quote trdrPdGvnVlmRcntlyTrdedL3Quote = null!;
    private PQQuoteSerializer updateQuoteSerializer = null!;
    private PQLevel2Quote valueDateL2Quote = null!;
    private ISourceTickerQuoteInfo valueDateQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();
        updateQuoteSerializer = new PQQuoteSerializer(UpdateStyle.Updates);
        snapshotQuoteSerializer = new PQQuoteSerializer(UpdateStyle.FullSnapshot);

        level0QuoteInfo = new SourceTickerQuoteInfo(1, "TestSource1", 1, "TestTicker1", 20, 0.00001m, 30000m, 50000000m, 1000m, 1);
        level1QuoteInfo = new SourceTickerQuoteInfo(2, "TestSource2", 2, "TestTicker2", 20, 0.00001m, 30000m, 50000000m, 1000m, 1);
        valueDateQuoteInfo = new SourceTickerQuoteInfo(7, "TestSource", 7,
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.ValueDate
            , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        everyLayerQuoteInfo = new SourceTickerQuoteInfo(8, "TestSource", 8,
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume.AllFlags(), LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                          LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        simpleNoRecentlyTradedQuoteInfo = new SourceTickerQuoteInfo(3, "TestSource", 3, "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1);
        srcNmLstTrdQuoteInfo = new SourceTickerQuoteInfo(4,
            "TestSource", 4, "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceName, LastTradedFlags.LastTradedPrice |
                                                                          LastTradedFlags.LastTradedTime);
        srcQtRfPdGvnVlmQuoteInfo = new SourceTickerQuoteInfo(
            5, "TestSource", 5, "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceQuoteReference, LastTradedFlags.PaidOrGiven);
        trdrLyrTrdrPdGvnVlmDtlsQuoteInfo = new SourceTickerQuoteInfo(
            6, "TestSource", 6, "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize |
            LayerFlags.TraderCount, LastTradedFlags.TraderName);
        level0Quote = new PQLevel0Quote(level0QuoteInfo);
        level1Quote = new PQLevel1Quote(level1QuoteInfo);
        valueDateL2Quote = new PQLevel2Quote(valueDateQuoteInfo);
        everyLayerL2Quote = new PQLevel2Quote(everyLayerQuoteInfo);
        simpleNoRecentlyTradedL3Quote = new PQLevel3Quote(simpleNoRecentlyTradedQuoteInfo);
        srcNmSmplRctlyTrdedL3Quote = new PQLevel3Quote(srcNmLstTrdQuoteInfo);
        srcQtRefPdGvnVlmRcntlyTrdedL3Quote = new PQLevel3Quote(srcQtRfPdGvnVlmQuoteInfo);
        trdrPdGvnVlmRcntlyTrdedL3Quote = new PQLevel3Quote(trdrLyrTrdrPdGvnVlmDtlsQuoteInfo);

        quoteSequencedTestDataBuilder.InitializeQuote(level0Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(level1Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateL2Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerL2Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleNoRecentlyTradedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(srcNmSmplRctlyTrdedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(srcQtRefPdGvnVlmRcntlyTrdedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(trdrPdGvnVlmRcntlyTrdedL3Quote, 0);

        differingQuotes = new List<IPQLevel0Quote>
        {
            level0Quote, level1Quote, valueDateL2Quote, everyLayerL2Quote, simpleNoRecentlyTradedL3Quote
            , srcNmSmplRctlyTrdedL3Quote, srcQtRefPdGvnVlmRcntlyTrdedL3Quote, trdrPdGvnVlmRcntlyTrdedL3Quote
        };

        readWriteBuffer = new ReadWriteBuffer(new byte[9000]) { ReadCursor = BufferReadWriteOffset };

        moqTimeContext = new Mock<ITimeContext>();
        frozenDateTime = new DateTime(2018, 1, 15, 19, 51, 1);
        moqTimeContext.SetupGet(tc => tc.UtcNow).Returns(frozenDateTime);

        TimeContext.Provider = moqTimeContext.Object;

        var pricingServerConfig = new PricingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig
            , NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig, syncRetryIntervalMs: retryWaitMs, allowUpdatesCatchup: allowCatchup);

        deserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler());
        deserializerRepository.RegisterDeserializer(level0QuoteInfo.Id
            , new PQQuoteDeserializer<PQLevel0Quote>(new TickerPricingSubscriptionConfig(level0QuoteInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer(level1QuoteInfo.Id
            , new PQQuoteDeserializer<PQLevel1Quote>(new TickerPricingSubscriptionConfig(level1QuoteInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer(valueDateQuoteInfo.Id
            , new PQQuoteDeserializer<PQLevel2Quote>(new TickerPricingSubscriptionConfig(valueDateQuoteInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer(everyLayerQuoteInfo.Id
            , new PQQuoteDeserializer<PQLevel2Quote>(new TickerPricingSubscriptionConfig(everyLayerQuoteInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer(simpleNoRecentlyTradedQuoteInfo.Id
            , new PQQuoteDeserializer<PQLevel3Quote>(new TickerPricingSubscriptionConfig(simpleNoRecentlyTradedQuoteInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer(srcNmLstTrdQuoteInfo.Id
            , new PQQuoteDeserializer<PQLevel3Quote>(new TickerPricingSubscriptionConfig(srcNmLstTrdQuoteInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer(srcQtRfPdGvnVlmQuoteInfo.Id
            , new PQQuoteDeserializer<PQLevel3Quote>(new TickerPricingSubscriptionConfig(srcQtRfPdGvnVlmQuoteInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer(trdrLyrTrdrPdGvnVlmDtlsQuoteInfo.Id
            , new PQQuoteDeserializer<PQLevel3Quote>(new TickerPricingSubscriptionConfig(trdrLyrTrdrPdGvnVlmDtlsQuoteInfo, pricingServerConfig)));

        pqClientMessageStreamDecoder = new PQClientMessageStreamDecoder(deserializerRepository);

        testBuffer = new byte[400];
    }

    [TestCleanup]
    public void TearDown()
    {
        TimeContext.Provider = new HighPrecisionTimeContext();
    }

    [TestMethod]
    public void UpdateSerializerFullyUpdated_Serialize_WritesExpectedBytesToBuffer()
    {
        foreach (var pqQuote in differingQuotes)
        {
            var expectedFieldUpdates = pqQuote.GetDeltaUpdateFields(frozenDateTime, UpdateStyle.Updates)
                .ToList();
            var expectedStringUpdates = pqQuote.GetStringUpdates(frozenDateTime, UpdateStyle.Updates)
                .ToList();

            // so that adapterSentTime is changed.
            // ReSharper disable once UnusedVariable
            var ignored = pqQuote.GetDeltaUpdateFields(DateTimeConstants.UnixEpoch, UpdateStyle.Updates);

            var amtWritten = updateQuoteSerializer
                .Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, pqQuote);
            readWriteBuffer.WriteCursor = BufferReadWriteOffset + amtWritten;

            AssertExpectedBytesWriten(amtWritten, false, expectedFieldUpdates, expectedStringUpdates, pqQuote);
        }
    }

    [TestMethod]
    public void SnapshotSerializerNoUpdates_Serialize_WritesAllDataExpectedBytesToBuffer()
    {
        foreach (var pqQuote in differingQuotes)
        {
            pqQuote.HasUpdates = false;
            var expectedFieldUpdates = pqQuote.GetDeltaUpdateFields(frozenDateTime, UpdateStyle.FullSnapshot)
                .ToList();
            var expectedStringUpdates = pqQuote.GetStringUpdates(frozenDateTime, UpdateStyle.FullSnapshot)
                .ToList();

            // so that adapterSentTime is changed.
            // ReSharper disable once UnusedVariable
            var ignored = pqQuote.GetDeltaUpdateFields(DateTimeConstants.UnixEpoch, UpdateStyle.Updates);

            var amtWritten = snapshotQuoteSerializer
                .Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, pqQuote);
            readWriteBuffer.WriteCursor = BufferReadWriteOffset + amtWritten;

            AssertExpectedBytesWriten(amtWritten, true, expectedFieldUpdates, expectedStringUpdates, pqQuote);
        }
    }

    [TestMethod]
    public void UpdateSerializerSaveToBuffer_Deserializer_CreatesEqualObjects()
    {
        foreach (var pqQuote in differingQuotes)
        {
            readWriteBuffer = new ReadWriteBuffer(new byte[9000]) { ReadCursor = BufferReadWriteOffset };
            pqQuote.PQSequenceId = uint.MaxValue; // will roll to 0 on
            var amtWritten = updateQuoteSerializer
                .Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, pqQuote);
            readWriteBuffer.WriteCursor = BufferReadWriteOffset + amtWritten;

            var sockBuffContext = new SocketBufferReadContext
            {
                EncodedBuffer = readWriteBuffer
                , DispatchLatencyLogger = new PerfLogger("test", TimeSpan.FromSeconds(2), "")
                , DetectTimestamp = pqQuote.ClientReceivedTime
                , ReceivingTimestamp = pqQuote.SocketReceivingTime
                , DeserializerTimestamp = frozenDateTime
            };
            var bytesConsumed = pqClientMessageStreamDecoder.Process(sockBuffContext);

            Assert.AreEqual(amtWritten, bytesConsumed);

            var deserializedQuote = deserializerRepository.GetDeserializer(pqQuote.SourceTickerQuoteInfo!.Id);

            Assert.IsNotNull(deserializedQuote);
            IPQLevel0Quote? clientSideQuote = null;
            switch (deserializedQuote)
            {
                case IPQQuoteDeserializer<PQLevel0Quote> pq0BinaryDeserializer:
                    clientSideQuote = pq0BinaryDeserializer.PublishedQuote;
                    break;
                case IPQQuoteDeserializer<PQLevel1Quote> pq1BinaryDeserializer:
                    clientSideQuote = pq1BinaryDeserializer.PublishedQuote;
                    break;
                case IPQQuoteDeserializer<PQLevel2Quote> pq2BinaryDeserializer:
                    clientSideQuote = pq2BinaryDeserializer.PublishedQuote;
                    break;
                case IPQQuoteDeserializer<PQLevel3Quote> pq3BinaryDeserializer:
                    clientSideQuote = pq3BinaryDeserializer.PublishedQuote;
                    break;
                default:
                    Assert.Fail("Should not reach here");
                    break;
            }

            try
            {
                pqQuote.ProcessedTime = frozenDateTime; //set original to expected time
                pqQuote.DispatchedTime = frozenDateTime; //set original to expected time
                clientSideQuote.HasUpdates = false;
                clientSideQuote.LastPublicationTime = pqQuote.LastPublicationTime; //not sent via serialization
                clientSideQuote.DispatchedTime = pqQuote.DispatchedTime; //set original to expected time
                Assert.AreEqual(pqQuote, clientSideQuote);
            }
            catch (AssertFailedException)
            {
                Console.Out.WriteLine(pqQuote.DiffQuotes(clientSideQuote));
                throw;
            }
        }
    }

    private unsafe void AssertExpectedBytesWriten(int amtWritten, bool isSnapshot,
        List<PQFieldUpdate> expectedFieldUpdates, List<PQFieldStringUpdate> expectedStringUpdates,
        IPQLevel0Quote originalQuote)
    {
        fixed (byte* bufferPtr = readWriteBuffer.Buffer)
        {
            var startWritten = bufferPtr + BufferReadWriteOffset;
            var currPtr = bufferPtr + BufferReadWriteOffset;
            var protocolVersion = *currPtr++;
            Assert.AreEqual(1, protocolVersion);
            var messageFlags = *currPtr++;
            var extendedFields = (originalQuote.SourceTickerQuoteInfo!.LayerFlags & LayerFlags.ValueDate) > 0;

            Assert.AreEqual((byte)(PQMessageFlags.IsQuote |
                                   (isSnapshot ? PQMessageFlags.PublishAll : 0)), messageFlags);
            var sourceTickerId = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(originalQuote.SourceTickerQuoteInfo.Id, sourceTickerId);
            var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual((uint)amtWritten, messagesTotalSize);
            var sequenceNumber = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(level0Quote.PQSequenceId, sequenceNumber);
            foreach (var fieldUpdate in expectedFieldUpdates)
            {
                var flag = *currPtr++;
                Assert.AreEqual(fieldUpdate.Flag, flag);
                ushort fieldId;
                if ((flag & PQFieldFlags.IsExtendedFieldId) == 0)
                    fieldId = *currPtr++;
                else
                    fieldId = StreamByteOps.ToUShort(ref currPtr);
                Assert.AreEqual(fieldUpdate.Id, fieldId);

                var fieldValue = StreamByteOps.ToUInt(ref currPtr);
                Assert.AreEqual(fieldUpdate.Value, fieldValue);
            }

            foreach (var stringUpdate in expectedStringUpdates)
            {
                var flag = *currPtr++;
                Assert.AreEqual(stringUpdate.Field.Flag, flag);
                Assert.AreNotEqual(0, stringUpdate.Field.Flag);
                ushort fieldId;
                if ((stringUpdate.Field.Flag & PQFieldFlags.IsExtendedFieldId) == 0)
                    fieldId = *currPtr++;
                else
                    fieldId = StreamByteOps.ToUShort(ref currPtr);
                Assert.AreEqual(stringUpdate.Field.Id, fieldId);

                Assert.AreEqual(0u, stringUpdate.Field.Value);
                var fieldValue = StreamByteOps.ToUInt(ref currPtr);
                var bytesUsed = (uint)GetSerializedStringSize(stringUpdate.StringUpdate.Value);
                Assert.AreEqual(bytesUsed, fieldValue);

                var dictionaryId = StreamByteOps.ToInt(ref currPtr);
                Assert.AreEqual(stringUpdate.StringUpdate.DictionaryId, dictionaryId);

                var stringValue = StreamByteOps.ToString(ref currPtr, (int)fieldValue);
                Assert.AreEqual(stringUpdate.StringUpdate.Value, stringValue);

                var command = (flag & PQFieldFlags.IsUpdate) == PQFieldFlags.IsUpdate ? CrudCommand.Update
                    : (flag & PQFieldFlags.IsDelete) == PQFieldFlags.IsDelete ? CrudCommand.None
                    : CrudCommand.Insert;
                Assert.AreEqual(stringUpdate.StringUpdate.Command, command);
            }

            Assert.AreEqual(amtWritten, currPtr - startWritten);
        }
    }

    private unsafe int GetSerializedStringSize(string toSerialize)
    {
        fixed (byte* bufferPtr = testBuffer)
        {
            var currPtr = bufferPtr;
            return StreamByteOps.ToBytes(ref currPtr, toSerialize, testBuffer.Length);
        }
    }
}
