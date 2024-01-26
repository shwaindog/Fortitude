#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization;

[TestClass]
public class PQQuoteSerializerTests
{
    private const int BufferReadWriteOffset = 5;
    private readonly bool allowCatchup = true;
    private readonly uint retryWaitMs = 2000;
    private IMap<uint, IMessageDeserializer> binaryDeserializers = null!;

    private IReadOnlyList<IPQLevel0Quote> differingQuotes = null!;
    private PQLevel2Quote everyLayerL2Quote = null!;
    private ISourceTickerClientAndPublicationConfig everyLayerQuoteInfo = null!;
    private DateTime frozenDateTime;
    private PQLevel0Quote level0Quote = null!;
    private ISourceTickerClientAndPublicationConfig level0QuoteInfo = null!;
    private PQLevel1Quote level1Quote = null!;
    private ISourceTickerClientAndPublicationConfig level1QuoteInfo = null!;

    private Mock<ITimeContext> moqTimeContext = null!;
    private PQClientMessageStreamDecoder pqClientMessageStreamDecoder = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private ReadWriteBuffer readWriteBuffer = null!;
    private PQLevel3Quote simpleNoRecentlyTradedL3Quote = null!;
    private ISourceTickerClientAndPublicationConfig simpleNoRecentlyTradedQuoteInfo = null!;
    private PQQuoteSerializer snapshotQuoteSerializer = null!;
    private ISourceTickerClientAndPublicationConfig srcNmLstTrdQuoteInfo = null!;
    private PQLevel3Quote srcNmSmplRctlyTrdedL3Quote = null!;
    private PQLevel3Quote srcQtRefPdGvnVlmRcntlyTrdedL3Quote = null!;
    private ISourceTickerClientAndPublicationConfig srcQtRfPdGvnVlmQuoteInfo = null!;
    private byte[] testBuffer = null!;
    private ISourceTickerClientAndPublicationConfig trdrLyrTrdrPdGvnVlmDtlsQuoteInfo = null!;
    private PQLevel3Quote trdrPdGvnVlmRcntlyTrdedL3Quote = null!;
    private PQQuoteSerializer updateQuoteSerializer = null!;
    private PQLevel2Quote valueDateL2Quote = null!;
    private ISourceTickerClientAndPublicationConfig valueDateQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();
        updateQuoteSerializer = new PQQuoteSerializer(UpdateStyle.Updates);
        snapshotQuoteSerializer = new PQQuoteSerializer(UpdateStyle.FullSnapshot);

        level0QuoteInfo = new SourceTickerClientAndPublicationConfig(1,
            "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.None, null, retryWaitMs, allowCatchup);
        level1QuoteInfo = new SourceTickerClientAndPublicationConfig(2,
            "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.None, null, retryWaitMs, allowCatchup);
        valueDateQuoteInfo = new SourceTickerClientAndPublicationConfig(7, "TestSource",
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.ValueDate, LastTradedFlags.PaidOrGiven |
                                                                         LastTradedFlags.TraderName |
                                                                         LastTradedFlags.LastTradedVolume |
                                                                         LastTradedFlags.LastTradedTime, null,
            retryWaitMs, allowCatchup);
        everyLayerQuoteInfo = new SourceTickerClientAndPublicationConfig(8, "TestSource",
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume.AllFlags(), LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                          LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, null
            , retryWaitMs, allowCatchup);
        simpleNoRecentlyTradedQuoteInfo = new SourceTickerClientAndPublicationConfig(3,
            "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.None, null, retryWaitMs, allowCatchup);
        srcNmLstTrdQuoteInfo = new SourceTickerClientAndPublicationConfig(4,
            "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceName, LastTradedFlags.LastTradedPrice |
                                                                          LastTradedFlags.LastTradedTime, null
            , retryWaitMs, allowCatchup);
        srcQtRfPdGvnVlmQuoteInfo = new SourceTickerClientAndPublicationConfig(
            5, "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceQuoteReference, LastTradedFlags.PaidOrGiven,
            null, retryWaitMs, allowCatchup);
        trdrLyrTrdrPdGvnVlmDtlsQuoteInfo = new SourceTickerClientAndPublicationConfig(
            6, "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize |
            LayerFlags.TraderCount, LastTradedFlags.TraderName, null, retryWaitMs, allowCatchup);
        level0Quote = new PQLevel0Quote(level0QuoteInfo);
        level1Quote = new PQLevel1Quote(level1QuoteInfo);
        valueDateL2Quote = new PQLevel2Quote(valueDateQuoteInfo);
        everyLayerL2Quote = new PQLevel2Quote(everyLayerQuoteInfo);
        simpleNoRecentlyTradedL3Quote = new PQLevel3Quote(simpleNoRecentlyTradedQuoteInfo);
        srcNmSmplRctlyTrdedL3Quote = new PQLevel3Quote(srcNmLstTrdQuoteInfo);
        srcQtRefPdGvnVlmRcntlyTrdedL3Quote = new PQLevel3Quote(srcQtRfPdGvnVlmQuoteInfo);
        trdrPdGvnVlmRcntlyTrdedL3Quote = new PQLevel3Quote(trdrLyrTrdrPdGvnVlmDtlsQuoteInfo);

        quoteSequencedTestDataBuilder.InitializeQuote(level0Quote, 10);
        quoteSequencedTestDataBuilder.InitializeQuote(level1Quote, 10);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateL2Quote, 10);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerL2Quote, 10);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleNoRecentlyTradedL3Quote, 10);
        quoteSequencedTestDataBuilder.InitializeQuote(srcNmSmplRctlyTrdedL3Quote, 10);
        quoteSequencedTestDataBuilder.InitializeQuote(srcQtRefPdGvnVlmRcntlyTrdedL3Quote, 10);
        quoteSequencedTestDataBuilder.InitializeQuote(trdrPdGvnVlmRcntlyTrdedL3Quote, 10);

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

        binaryDeserializers = new LinkedListCache<uint, IMessageDeserializer>
        {
            { level0QuoteInfo.Id, new PQQuoteDeserializer<PQLevel0Quote>(level0QuoteInfo) }
            , { level1QuoteInfo.Id, new PQQuoteDeserializer<PQLevel1Quote>(level1QuoteInfo) }
            , { valueDateQuoteInfo.Id, new PQQuoteDeserializer<PQLevel2Quote>(valueDateQuoteInfo) }
            , { everyLayerQuoteInfo.Id, new PQQuoteDeserializer<PQLevel2Quote>(everyLayerQuoteInfo) },
            {
                simpleNoRecentlyTradedQuoteInfo.Id
                , new PQQuoteDeserializer<PQLevel3Quote>(simpleNoRecentlyTradedQuoteInfo)
            }
            , { srcNmLstTrdQuoteInfo.Id, new PQQuoteDeserializer<PQLevel3Quote>(srcNmLstTrdQuoteInfo) }
            , { srcQtRfPdGvnVlmQuoteInfo.Id, new PQQuoteDeserializer<PQLevel3Quote>(srcQtRfPdGvnVlmQuoteInfo) },
            {
                trdrLyrTrdrPdGvnVlmDtlsQuoteInfo.Id
                , new PQQuoteDeserializer<PQLevel3Quote>(trdrLyrTrdrPdGvnVlmDtlsQuoteInfo)
            }
        };
        pqClientMessageStreamDecoder = new PQClientMessageStreamDecoder(binaryDeserializers, PQFeedType.Snapshot);


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
            readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

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
            readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

            AssertExpectedBytesWriten(amtWritten, true, expectedFieldUpdates, expectedStringUpdates, pqQuote);
        }
    }

    [TestMethod]
    public void UpdateSerializerSaveToBuffer_Deserializer_CreatesEqualObjects()
    {
        foreach (var pqQuote in differingQuotes)
        {
            readWriteBuffer = new ReadWriteBuffer(new byte[9000]) { ReadCursor = BufferReadWriteOffset };
            var amtWritten = updateQuoteSerializer
                .Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, pqQuote);
            readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

            var dispatchContext = new DispatchContext
            {
                EncodedBuffer = readWriteBuffer
                , DispatchLatencyLogger = new PerfLogger("test", TimeSpan.FromSeconds(2), "")
                , DetectTimestamp = pqQuote.ClientReceivedTime, ReceivingTimestamp = pqQuote.SocketReceivingTime
                , DeserializerTimestamp = frozenDateTime
            };

            var bytesConsumed = pqClientMessageStreamDecoder.Process(dispatchContext);

            Assert.AreEqual(amtWritten, bytesConsumed);

            var deserializedQuote = binaryDeserializers[pqQuote.SourceTickerQuoteInfo!.Id];

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

            Assert.AreEqual((byte)(PQBinaryMessageFlags.ContainsStringUpdate |
                                   (isSnapshot ? PQBinaryMessageFlags.PublishAll : 0) |
                                   (extendedFields ? PQBinaryMessageFlags.ExtendedFieldId : 0)), messageFlags);
            var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual((uint)amtWritten, messagesTotalSize);
            var sourceTickerId = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(originalQuote.SourceTickerQuoteInfo.Id, sourceTickerId);
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
