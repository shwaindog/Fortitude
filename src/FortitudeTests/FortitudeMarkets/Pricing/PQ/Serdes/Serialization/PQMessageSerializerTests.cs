// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Config;
using FortitudeMarkets.Config.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeIO.Transports.Network.Config;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using Moq;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQMessageSerializerTests
{
    private const int BufferReadWriteOffset = 5;

    private readonly bool allowCatchup = true;
    private readonly uint retryWaitMs  = 2000;

    private PQClientQuoteDeserializerRepository deserializerRepository = null!;
    private IReadOnlyList<IPQPublishableTickInstant>       differingQuotes        = null!;

    private ISourceTickerInfo  everyLayerInfo    = null!;
    private PQPublishableLevel2Quote      everyLayerL2Quote = null!;
    private DateTime           frozenDateTime;
    private ISourceTickerInfo  level1Info     = null!;
    private PQPublishableLevel1Quote      level1Quote    = null!;
    private Mock<ITimeContext> moqTimeContext = null!;

    private PQClientMessageStreamDecoder  pqClientMessageStreamDecoder  = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private CircularReadWriteBuffer readWriteBuffer = null!;

    private ISourceTickerInfo simpleNoRecentlyTradedInfo         = null!;
    private PQPublishableLevel3Quote     simpleNoRecentlyTradedL3Quote      = null!;
    private PQMessageSerializer snapshotMessageSerializer            = null!;
    private ISourceTickerInfo srcNmLstTrdInfo                    = null!;
    private PQPublishableLevel3Quote     srcNmSmplRecentlyTradedL3Quote         = null!;
    private PQPublishableLevel3Quote     srcQtRefPdGvnVlmRecentlyTradedL3Quote = null!;
    private ISourceTickerInfo srcQtRfPdGvnVlmInfo                = null!;
    private byte[]            testBuffer                         = null!;
    private PQPublishableTickInstant     tickInstant                        = null!;
    private ISourceTickerInfo tickInstantInfo                    = null!;
    private ISourceTickerInfo trdrLyrTrdrPdGvnVlmDtlsInfo        = null!;
    private PQPublishableLevel3Quote     trdrPdGvnVlmRcntlyTrdedL3Quote     = null!;
    private PQMessageSerializer updateMessageSerializer              = null!;
    private ISourceTickerInfo valueDateInfo                      = null!;
    private PQPublishableLevel2Quote     valueDateL2Quote                   = null!;

    private const int SingleQuoteBufferSize = 17000;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();
        updateMessageSerializer       = new PQMessageSerializer(global::FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.PQMessageFlags.Update);
        snapshotMessageSerializer     = new PQMessageSerializer(global::FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.PQMessageFlags.Snapshot);

        tickInstantInfo =
            new SourceTickerInfo
                (1, "TestSource1", 1, "TestTicker1", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.000001m, 0.00001m, 1m, 50000000m, 1000m, 1);
        level1Info =
            new SourceTickerInfo
                (2, "TestSource2", 2, "TestTicker2", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.000001m, 0.00001m, 1m, 50000000m, 1000m, 1);
        valueDateInfo =
            new SourceTickerInfo
                (7, "TestSource", 7, "TestTicker", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.000001m, 0.00001m, 1m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.ValueDate
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        everyLayerInfo =
            new SourceTickerInfo
                (8, "TestSource", 8, "TestTicker", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.000001m, 0.00001m, 1m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume.AllFlags()
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        simpleNoRecentlyTradedInfo
            = new SourceTickerInfo
                (3, "TestSource", 3, "TestTicker", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.000001m, 0.00001m, 1m, 50000000m, 1000m, 1);
        srcNmLstTrdInfo =
            new SourceTickerInfo
                (4, "TestSource", 4, "TestTicker", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.000001m, 0.00001m, 1m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceName
               , lastTradedFlags: LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime);
        srcQtRfPdGvnVlmInfo =
            new SourceTickerInfo
                (5, "TestSource", 5, "TestTicker", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.000001m, 0.00001m, 1m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceQuoteReference, lastTradedFlags: LastTradedFlags.PaidOrGiven);
        trdrLyrTrdrPdGvnVlmDtlsInfo =
            new SourceTickerInfo
                (6, "TestSource", 6, "TestTicker", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.000001m, 0.00001m, 1m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
               , lastTradedFlags: LastTradedFlags.TraderName);
        tickInstant       = new PQPublishableTickInstant(tickInstantInfo);
        level1Quote       = new PQPublishableLevel1Quote(level1Info);
        valueDateL2Quote  = new PQPublishableLevel2Quote(valueDateInfo);
        everyLayerL2Quote = new PQPublishableLevel2Quote(everyLayerInfo);

        simpleNoRecentlyTradedL3Quote = new PQPublishableLevel3Quote(simpleNoRecentlyTradedInfo);
        srcNmSmplRecentlyTradedL3Quote    = new PQPublishableLevel3Quote(srcNmLstTrdInfo);

        srcQtRefPdGvnVlmRecentlyTradedL3Quote = new PQPublishableLevel3Quote(srcQtRfPdGvnVlmInfo);
        trdrPdGvnVlmRcntlyTrdedL3Quote     = new PQPublishableLevel3Quote(trdrLyrTrdrPdGvnVlmDtlsInfo);

        quoteSequencedTestDataBuilder.InitializeQuote(tickInstant, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(level1Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateL2Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerL2Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleNoRecentlyTradedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(srcNmSmplRecentlyTradedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(srcQtRefPdGvnVlmRecentlyTradedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(trdrPdGvnVlmRcntlyTrdedL3Quote, 0);

        differingQuotes = new List<IPQPublishableTickInstant>
            {
                tickInstant, level1Quote, valueDateL2Quote, everyLayerL2Quote, simpleNoRecentlyTradedL3Quote
              , srcNmSmplRecentlyTradedL3Quote, srcQtRefPdGvnVlmRecentlyTradedL3Quote, trdrPdGvnVlmRcntlyTrdedL3Quote
            };
            // {
            //     srcNmSmplRctlyTrdedL3Quote
            // };

        readWriteBuffer = new CircularReadWriteBuffer(new byte[SingleQuoteBufferSize]) { ReadCursor = BufferReadWriteOffset };

        moqTimeContext = new Mock<ITimeContext>();
        frozenDateTime = new DateTime(2018, 1, 15, 19, 51, 1);
        moqTimeContext.SetupGet(tc => tc.UtcNow).Returns(frozenDateTime);

        TimeContext.Provider = moqTimeContext.Object;

        var pricingServerConfig =
            new PricingServerConfig
                (NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig
               , NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig
               , syncRetryIntervalMs: retryWaitMs, allowUpdatesCatchup: allowCatchup);

        deserializerRepository = new PQClientQuoteDeserializerRepository("PQClientTest1", new Recycler());
        deserializerRepository.RegisterDeserializer
            (tickInstantInfo.SourceInstrumentId,
             new PQMessageDeserializer<PQPublishableTickInstant>
                 (new TickerPricingSubscriptionConfig(tickInstantInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (level1Info.SourceInstrumentId
           , new PQMessageDeserializer<PQPublishableLevel1Quote>(new TickerPricingSubscriptionConfig(level1Info, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (valueDateInfo.SourceInstrumentId,
             new PQMessageDeserializer<PQPublishableLevel2Quote>
                 (new TickerPricingSubscriptionConfig(valueDateInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (everyLayerInfo.SourceInstrumentId,
             new PQMessageDeserializer<PQPublishableLevel2Quote>
                 (new TickerPricingSubscriptionConfig(everyLayerInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (simpleNoRecentlyTradedInfo.SourceInstrumentId,
             new PQMessageDeserializer<PQPublishableLevel3Quote>
                 (new TickerPricingSubscriptionConfig(simpleNoRecentlyTradedInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (srcNmLstTrdInfo.SourceInstrumentId,
             new PQMessageDeserializer<PQPublishableLevel3Quote>
                 (new TickerPricingSubscriptionConfig(srcNmLstTrdInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (srcQtRfPdGvnVlmInfo.SourceInstrumentId,
             new PQMessageDeserializer<PQPublishableLevel3Quote>
                 (new TickerPricingSubscriptionConfig(srcQtRfPdGvnVlmInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (trdrLyrTrdrPdGvnVlmDtlsInfo.SourceInstrumentId,
             new PQMessageDeserializer<PQPublishableLevel3Quote>
                 (new TickerPricingSubscriptionConfig(trdrLyrTrdrPdGvnVlmDtlsInfo, pricingServerConfig)));

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
            var expectedFieldUpdates =
                pqQuote.GetDeltaUpdateFields(frozenDateTime, PQMessageFlags.Update).ToList();
            var expectedStringUpdates =
                pqQuote.GetStringUpdates(frozenDateTime, PQMessageFlags.Update).ToList();

            // so that adapterSentTime is changed.
            // ReSharper disable once UnusedVariable
            var ignored = pqQuote.GetDeltaUpdateFields(DateTimeConstants.UnixEpoch, PQMessageFlags.Update);

            readWriteBuffer.WriteCursor = BufferReadWriteOffset;
            var amtWritten = updateMessageSerializer
                .Serialize(readWriteBuffer, pqQuote);
            readWriteBuffer.WriteCursor += amtWritten;

            AssertExpectedBytesWritten(amtWritten, false, expectedFieldUpdates, expectedStringUpdates, pqQuote);
        }
    }

    [TestMethod]
    public void SnapshotSerializerNoUpdates_Serialize_WritesAllDataExpectedBytesToBuffer()
    {
        foreach (var pqQuote in differingQuotes)
        {
            pqQuote.HasUpdates = false;
            var expectedFieldUpdates =
                pqQuote.GetDeltaUpdateFields(frozenDateTime, PQMessageFlags.Snapshot).ToList();
            var expectedStringUpdates =
                pqQuote.GetStringUpdates(frozenDateTime, PQMessageFlags.Snapshot).ToList();

            // so that adapterSentTime is changed.
            // ReSharper disable once UnusedVariable
            var ignored = pqQuote.GetDeltaUpdateFields(DateTimeConstants.UnixEpoch, PQMessageFlags.Update);

            readWriteBuffer.WriteCursor = BufferReadWriteOffset;
            var amtWritten = snapshotMessageSerializer
                .Serialize(readWriteBuffer, pqQuote);
            readWriteBuffer.WriteCursor += amtWritten;

            AssertExpectedBytesWritten(amtWritten, true, expectedFieldUpdates, expectedStringUpdates, pqQuote);
        }
    }

    [TestMethod]
    public void UpdateSerializerSaveToBuffer_Deserializer_CreatesEqualObjects()
    {
        foreach (var pqQuote in differingQuotes)
        {
            readWriteBuffer      = new CircularReadWriteBuffer(new byte[SingleQuoteBufferSize]) { ReadCursor = BufferReadWriteOffset };
            pqQuote.PQSequenceId = 0;

            readWriteBuffer.WriteCursor = BufferReadWriteOffset;
            var amtWritten = updateMessageSerializer
                .Serialize(readWriteBuffer, pqQuote);
            readWriteBuffer.WriteCursor += amtWritten;

            var sockBuffContext = new SocketBufferReadContext
            {
                EncodedBuffer         = readWriteBuffer
              , DispatchLatencyLogger = new PerfLogger("test", TimeSpan.FromSeconds(2), "")
              , DetectTimestamp       = pqQuote.ClientReceivedTime
              , ReceivingTimestamp    = pqQuote.InboundSocketReceivingTime
              , DeserializerTime      = frozenDateTime
            };
            var bytesConsumed = pqClientMessageStreamDecoder.Process(sockBuffContext);

            Assert.AreEqual(amtWritten, bytesConsumed);

            var deserializedQuote = deserializerRepository.GetDeserializer(pqQuote.SourceTickerInfo!.SourceInstrumentId);

            Assert.IsNotNull(deserializedQuote);
            IPQPublishableTickInstant? clientSideQuote = null;
            switch (deserializedQuote)
            {
                case IPQMessageDeserializer<PQPublishableTickInstant> pq0BinaryDeserializer: clientSideQuote = pq0BinaryDeserializer.PublishedQuote; break;
                case IPQMessageDeserializer<PQPublishableLevel1Quote> pq1BinaryDeserializer: clientSideQuote = pq1BinaryDeserializer.PublishedQuote; break;
                case IPQMessageDeserializer<PQPublishableLevel2Quote> pq2BinaryDeserializer: clientSideQuote = pq2BinaryDeserializer.PublishedQuote; break;
                case IPQMessageDeserializer<PQPublishableLevel3Quote> pq3BinaryDeserializer: clientSideQuote = pq3BinaryDeserializer.PublishedQuote; break;

                default: Assert.Fail("Should not reach here"); break;
            }

            try
            {
                pqQuote.PQSequenceId   -= 1;
                pqQuote.InboundProcessedTime  =  frozenDateTime; //set original to expected time
                pqQuote.SubscriberDispatchedTime =  frozenDateTime; //set original to expected time

                clientSideQuote.HasUpdates          = false;
                clientSideQuote.LastPublicationTime = pqQuote.LastPublicationTime; //not sent via serialization
                clientSideQuote.SubscriberDispatchedTime      = pqQuote.SubscriberDispatchedTime;      //set original to expected time

                clientSideQuote.IsDispatchedTimeDateUpdated    = pqQuote.IsDispatchedTimeDateUpdated;
                clientSideQuote.IsDispatchedTimeSub2MinUpdated = pqQuote.IsDispatchedTimeSub2MinUpdated;
                clientSideQuote.IsProcessedTimeDateUpdated     = pqQuote.IsProcessedTimeDateUpdated;
                clientSideQuote.IsProcessedTimeSub2MinUpdated  = pqQuote.IsProcessedTimeSub2MinUpdated;

                Assert.AreEqual(pqQuote, clientSideQuote);
            }
            catch (AssertFailedException)
            {
                Console.Out.WriteLine(pqQuote.DiffQuotes(clientSideQuote));
                Thread.Sleep(100);
                throw;
            }
        }
    }

    private unsafe void AssertExpectedBytesWritten
    (int amtWritten, bool isSnapshot, List<PQFieldUpdate> expectedFieldUpdates, List<PQFieldStringUpdate> expectedStringUpdates
      , IPQPublishableTickInstant originalQuote)
    {
        using var fixedBuffer = readWriteBuffer;

        var startWritten    = fixedBuffer.ReadBuffer + BufferReadWriteOffset;
        var currPtr         = startWritten;
        var protocolVersion = *currPtr++;

        Assert.AreEqual(1, protocolVersion);

        var messageFlags   = *currPtr++;
        var extendedFields = (originalQuote.SourceTickerInfo!.LayerFlags & LayerFlags.ValueDate) > 0;

        Assert.AreEqual((byte)(isSnapshot ? global::FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.PQMessageFlags.Snapshot : global::FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.PQMessageFlags.Update), messageFlags);
        var sourceTickerId = StreamByteOps.ToUInt(ref currPtr);
        Assert.AreEqual(originalQuote.SourceTickerInfo.SourceInstrumentId, sourceTickerId);
        var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
        Assert.AreEqual((uint)amtWritten, messagesTotalSize);
        var sequenceNumber = StreamByteOps.ToUInt(ref currPtr);
        Assert.AreEqual((uint)Math.Max(0, (int)tickInstant.PQSequenceId - 1), sequenceNumber);
        foreach (var fieldUpdate in expectedFieldUpdates)
        {
            var flag = (PQFieldFlags)(*currPtr++);
            Assert.AreEqual(fieldUpdate.Flag, flag);
            var fieldId = (PQFeedFields)(*currPtr++);
            Assert.AreEqual(fieldUpdate.Id, fieldId);

            if (flag.HasDepthKeyFlag())
            {
                var depthByte = *currPtr++;
                var depthKey  = depthByte.IsTwoByteDepth() ? depthByte.ToDepthKey(*currPtr++) : depthByte.ToDepthKey();
                Assert.AreEqual(fieldUpdate.DepthId, depthKey);
            }
            if (flag.HasSubIdFlag())
            {
                var subId = (PQPricingSubFieldKeys)(*currPtr++);
                Assert.AreEqual(fieldUpdate.PricingSubId, subId);
            }
            if (flag.HasAuxiliaryPayloadFlag())
            {
                var auxPayload = StreamByteOps.ToUShort(ref currPtr);
                Assert.AreEqual(fieldUpdate.AuxiliaryPayload, auxPayload);
            }

            var fieldValue = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(fieldUpdate.Payload, fieldValue);
        }

        foreach (var stringUpdate in expectedStringUpdates)
        {
            var flag = (PQFieldFlags)(*currPtr++);
            Assert.AreEqual(stringUpdate.Field.Flag, flag);
            Assert.AreNotEqual(0, stringUpdate.Field.Flag);
            var fieldId = (PQFeedFields)(*currPtr++);
            Assert.AreEqual(stringUpdate.Field.Id, fieldId);
            if (flag.HasDepthKeyFlag())
            {
                var depthByte = *currPtr++;
                var depthKey  = depthByte.IsTwoByteDepth() ? depthByte.ToDepthKey(*currPtr++) : depthByte.ToDepthKey();
                Assert.AreEqual(stringUpdate.Field.DepthId, depthKey);
            }
            PQPricingSubFieldKeys subId = PQPricingSubFieldKeys.None;
            if (flag.HasSubIdFlag())
            {
                subId = (PQPricingSubFieldKeys)(*currPtr++);
                Assert.AreEqual(stringUpdate.Field.PricingSubId, subId);
            }
            if (flag.HasAuxiliaryPayloadFlag())
            {
                var auxPayload = StreamByteOps.ToUShort(ref currPtr);
                Assert.AreEqual(stringUpdate.Field.AuxiliaryPayload, auxPayload);
            }

            Assert.AreEqual(0u, stringUpdate.Field.Payload);
            var fieldValue = StreamByteOps.ToUInt(ref currPtr);
            var bytesUsed  = (uint)GetSerializedStringSize(stringUpdate.StringUpdate.Value);
            Assert.AreEqual(bytesUsed, fieldValue);

            var dictionaryId = StreamByteOps.ToInt(ref currPtr);
            Assert.AreEqual(stringUpdate.StringUpdate.DictionaryId, dictionaryId);

            var stringValue = StreamByteOps.ToString(ref currPtr, (int)fieldValue);
            Assert.AreEqual(stringUpdate.StringUpdate.Value, stringValue);

            var command = subId == PQPricingSubFieldKeys.CommandUpsert ? CrudCommand.Upsert : CrudCommand.Delete;
            Assert.AreEqual(stringUpdate.StringUpdate.Command, command
                          , $"For stringUpdate {stringUpdate} got {command} when expected {stringUpdate.StringUpdate.Command}");
        }

        Assert.AreEqual(amtWritten, currPtr - startWritten);
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
