﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeIO.Transports.Network.Config;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using Moq;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQQuoteSerializerTests
{
    private const int BufferReadWriteOffset = 5;

    private readonly bool allowCatchup = true;
    private readonly uint retryWaitMs  = 2000;

    private PQClientQuoteDeserializerRepository deserializerRepository = null!;
    private IReadOnlyList<IPQTickInstant>       differingQuotes        = null!;

    private ISourceTickerInfo  everyLayerInfo    = null!;
    private PQLevel2Quote      everyLayerL2Quote = null!;
    private DateTime           frozenDateTime;
    private ISourceTickerInfo  level1Info     = null!;
    private PQLevel1Quote      level1Quote    = null!;
    private Mock<ITimeContext> moqTimeContext = null!;

    private PQClientMessageStreamDecoder  pqClientMessageStreamDecoder  = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private CircularReadWriteBuffer readWriteBuffer = null!;

    private ISourceTickerInfo simpleNoRecentlyTradedInfo         = null!;
    private PQLevel3Quote     simpleNoRecentlyTradedL3Quote      = null!;
    private PQQuoteSerializer snapshotQuoteSerializer            = null!;
    private ISourceTickerInfo srcNmLstTrdInfo                    = null!;
    private PQLevel3Quote     srcNmSmplRctlyTrdedL3Quote         = null!;
    private PQLevel3Quote     srcQtRefPdGvnVlmRcntlyTrdedL3Quote = null!;
    private ISourceTickerInfo srcQtRfPdGvnVlmInfo                = null!;
    private byte[]            testBuffer                         = null!;
    private PQTickInstant     tickInstant                        = null!;
    private ISourceTickerInfo tickInstantInfo                    = null!;
    private ISourceTickerInfo trdrLyrTrdrPdGvnVlmDtlsInfo        = null!;
    private PQLevel3Quote     trdrPdGvnVlmRcntlyTrdedL3Quote     = null!;
    private PQQuoteSerializer updateQuoteSerializer              = null!;
    private ISourceTickerInfo valueDateInfo                      = null!;
    private PQLevel2Quote     valueDateL2Quote                   = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();
        updateQuoteSerializer         = new PQQuoteSerializer(PQMessageFlags.Update);
        snapshotQuoteSerializer       = new PQQuoteSerializer(PQMessageFlags.Snapshot);

        tickInstantInfo =
            new SourceTickerInfo
                (1, "TestSource1", 1, "TestTicker1", Level3Quote, Unknown
               , 20, 0.000001m, 0.00001m, 30000m, 50000000m, 1000m, 1);
        level1Info =
            new SourceTickerInfo
                (2, "TestSource2", 2, "TestTicker2", Level3Quote, Unknown
               , 20, 0.000001m, 0.00001m, 30000m, 50000000m, 1000m, 1);
        valueDateInfo =
            new SourceTickerInfo
                (7, "TestSource", 7, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.00001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.ValueDate
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        everyLayerInfo =
            new SourceTickerInfo
                (8, "TestSource", 8, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.00001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume.AllFlags()
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        simpleNoRecentlyTradedInfo
            = new SourceTickerInfo
                (3, "TestSource", 3, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.00001m, 30000m, 50000000m, 1000m, 1);
        srcNmLstTrdInfo =
            new SourceTickerInfo
                (4, "TestSource", 4, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.00001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceName
               , lastTradedFlags: LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime);
        srcQtRfPdGvnVlmInfo =
            new SourceTickerInfo
                (5, "TestSource", 5, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.00001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceQuoteReference, lastTradedFlags: LastTradedFlags.PaidOrGiven);
        trdrLyrTrdrPdGvnVlmDtlsInfo =
            new SourceTickerInfo
                (6, "TestSource", 6, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.00001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
               , lastTradedFlags: LastTradedFlags.TraderName);
        tickInstant       = new PQTickInstant(tickInstantInfo);
        level1Quote       = new PQLevel1Quote(level1Info);
        valueDateL2Quote  = new PQLevel2Quote(valueDateInfo);
        everyLayerL2Quote = new PQLevel2Quote(everyLayerInfo);

        simpleNoRecentlyTradedL3Quote = new PQLevel3Quote(simpleNoRecentlyTradedInfo);
        srcNmSmplRctlyTrdedL3Quote    = new PQLevel3Quote(srcNmLstTrdInfo);

        srcQtRefPdGvnVlmRcntlyTrdedL3Quote = new PQLevel3Quote(srcQtRfPdGvnVlmInfo);
        trdrPdGvnVlmRcntlyTrdedL3Quote     = new PQLevel3Quote(trdrLyrTrdrPdGvnVlmDtlsInfo);

        quoteSequencedTestDataBuilder.InitializeQuote(tickInstant, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(level1Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateL2Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerL2Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleNoRecentlyTradedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(srcNmSmplRctlyTrdedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(srcQtRefPdGvnVlmRcntlyTrdedL3Quote, 0);
        quoteSequencedTestDataBuilder.InitializeQuote(trdrPdGvnVlmRcntlyTrdedL3Quote, 0);

        differingQuotes = new List<IPQTickInstant>
        {
            tickInstant, level1Quote, valueDateL2Quote, everyLayerL2Quote, simpleNoRecentlyTradedL3Quote
          , srcNmSmplRctlyTrdedL3Quote, srcQtRefPdGvnVlmRcntlyTrdedL3Quote, trdrPdGvnVlmRcntlyTrdedL3Quote
        };

        readWriteBuffer = new CircularReadWriteBuffer(new byte[9000]) { ReadCursor = BufferReadWriteOffset };

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
            (tickInstantInfo.SourceTickerId,
             new PQQuoteDeserializer<PQTickInstant>
                 (new TickerPricingSubscriptionConfig(tickInstantInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (level1Info.SourceTickerId
           , new PQQuoteDeserializer<PQLevel1Quote>(new TickerPricingSubscriptionConfig(level1Info, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (valueDateInfo.SourceTickerId,
             new PQQuoteDeserializer<PQLevel2Quote>
                 (new TickerPricingSubscriptionConfig(valueDateInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (everyLayerInfo.SourceTickerId,
             new PQQuoteDeserializer<PQLevel2Quote>
                 (new TickerPricingSubscriptionConfig(everyLayerInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (simpleNoRecentlyTradedInfo.SourceTickerId,
             new PQQuoteDeserializer<PQLevel3Quote>
                 (new TickerPricingSubscriptionConfig(simpleNoRecentlyTradedInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (srcNmLstTrdInfo.SourceTickerId,
             new PQQuoteDeserializer<PQLevel3Quote>
                 (new TickerPricingSubscriptionConfig(srcNmLstTrdInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (srcQtRfPdGvnVlmInfo.SourceTickerId,
             new PQQuoteDeserializer<PQLevel3Quote>
                 (new TickerPricingSubscriptionConfig(srcQtRfPdGvnVlmInfo, pricingServerConfig)));
        deserializerRepository.RegisterDeserializer
            (trdrLyrTrdrPdGvnVlmDtlsInfo.SourceTickerId,
             new PQQuoteDeserializer<PQLevel3Quote>
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
                pqQuote.GetDeltaUpdateFields(frozenDateTime, StorageFlags.Update).ToList();
            var expectedStringUpdates =
                pqQuote.GetStringUpdates(frozenDateTime, StorageFlags.Update).ToList();

            // so that adapterSentTime is changed.
            // ReSharper disable once UnusedVariable
            var ignored = pqQuote.GetDeltaUpdateFields(DateTimeConstants.UnixEpoch, StorageFlags.Update);

            readWriteBuffer.WriteCursor = BufferReadWriteOffset;
            var amtWritten = updateQuoteSerializer
                .Serialize(readWriteBuffer, pqQuote);
            readWriteBuffer.WriteCursor += amtWritten;

            AssertExpectedBytesWriten(amtWritten, false, expectedFieldUpdates, expectedStringUpdates, pqQuote);
        }
    }

    [TestMethod]
    public void SnapshotSerializerNoUpdates_Serialize_WritesAllDataExpectedBytesToBuffer()
    {
        foreach (var pqQuote in differingQuotes)
        {
            pqQuote.HasUpdates = false;
            var expectedFieldUpdates =
                pqQuote.GetDeltaUpdateFields(frozenDateTime, StorageFlags.Snapshot).ToList();
            var expectedStringUpdates =
                pqQuote.GetStringUpdates(frozenDateTime, StorageFlags.Snapshot).ToList();

            // so that adapterSentTime is changed.
            // ReSharper disable once UnusedVariable
            var ignored = pqQuote.GetDeltaUpdateFields(DateTimeConstants.UnixEpoch, StorageFlags.Update);

            readWriteBuffer.WriteCursor = BufferReadWriteOffset;
            var amtWritten = snapshotQuoteSerializer
                .Serialize(readWriteBuffer, pqQuote);
            readWriteBuffer.WriteCursor += amtWritten;

            AssertExpectedBytesWriten(amtWritten, true, expectedFieldUpdates, expectedStringUpdates, pqQuote);
        }
    }

    [TestMethod]
    public void UpdateSerializerSaveToBuffer_Deserializer_CreatesEqualObjects()
    {
        foreach (var pqQuote in differingQuotes)
        {
            readWriteBuffer      = new CircularReadWriteBuffer(new byte[9000]) { ReadCursor = BufferReadWriteOffset };
            pqQuote.PQSequenceId = uint.MaxValue; // will roll to 0 on

            readWriteBuffer.WriteCursor = BufferReadWriteOffset;
            var amtWritten = updateQuoteSerializer
                .Serialize(readWriteBuffer, pqQuote);
            readWriteBuffer.WriteCursor += amtWritten;

            var sockBuffContext = new SocketBufferReadContext
            {
                EncodedBuffer         = readWriteBuffer
              , DispatchLatencyLogger = new PerfLogger("test", TimeSpan.FromSeconds(2), "")
              , DetectTimestamp       = pqQuote.ClientReceivedTime
              , ReceivingTimestamp    = pqQuote.SocketReceivingTime
              , DeserializerTime      = frozenDateTime
            };
            var bytesConsumed = pqClientMessageStreamDecoder.Process(sockBuffContext);

            Assert.AreEqual(amtWritten, bytesConsumed);

            var deserializedQuote = deserializerRepository.GetDeserializer(pqQuote.SourceTickerInfo!.SourceTickerId);

            Assert.IsNotNull(deserializedQuote);
            IPQTickInstant? clientSideQuote = null;
            switch (deserializedQuote)
            {
                case IPQQuoteDeserializer<PQTickInstant> pq0BinaryDeserializer:
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
                pqQuote.ProcessedTime  = frozenDateTime; //set original to expected time
                pqQuote.DispatchedTime = frozenDateTime; //set original to expected time

                clientSideQuote.HasUpdates          = false;
                clientSideQuote.LastPublicationTime = pqQuote.LastPublicationTime; //not sent via serialization
                clientSideQuote.DispatchedTime      = pqQuote.DispatchedTime;      //set original to expected time

                clientSideQuote.IsDispatchedTimeDateUpdated    = pqQuote.IsDispatchedTimeDateUpdated;
                clientSideQuote.IsDispatchedTimeSubHourUpdated = pqQuote.IsDispatchedTimeSubHourUpdated;
                clientSideQuote.IsProcessedTimeDateUpdated     = pqQuote.IsProcessedTimeDateUpdated;
                clientSideQuote.IsProcessedTimeSubHourUpdated  = pqQuote.IsProcessedTimeSubHourUpdated;

                Assert.AreEqual(pqQuote, clientSideQuote);
            }
            catch (AssertFailedException)
            {
                Console.Out.WriteLine(pqQuote.DiffQuotes(clientSideQuote));
                throw;
            }
        }
    }

    private unsafe void AssertExpectedBytesWriten
    (int amtWritten, bool isSnapshot, List<PQFieldUpdate> expectedFieldUpdates, List<PQFieldStringUpdate> expectedStringUpdates
      , IPQTickInstant originalQuote)
    {
        using var fixedBuffer = readWriteBuffer;

        var startWritten    = fixedBuffer.ReadBuffer + BufferReadWriteOffset;
        var currPtr         = startWritten;
        var protocolVersion = *currPtr++;

        Assert.AreEqual(1, protocolVersion);

        var messageFlags   = *currPtr++;
        var extendedFields = (originalQuote.SourceTickerInfo!.LayerFlags & LayerFlags.ValueDate) > 0;

        Assert.AreEqual((byte)(isSnapshot ? PQMessageFlags.Snapshot : PQMessageFlags.Update), messageFlags);
        var sourceTickerId = StreamByteOps.ToUInt(ref currPtr);
        Assert.AreEqual(originalQuote.SourceTickerInfo.SourceTickerId, sourceTickerId);
        var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
        Assert.AreEqual((uint)amtWritten, messagesTotalSize);
        var sequenceNumber = StreamByteOps.ToUInt(ref currPtr);
        Assert.AreEqual(tickInstant.PQSequenceId, sequenceNumber);
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
            var bytesUsed  = (uint)GetSerializedStringSize(stringUpdate.StringUpdate.Value);
            Assert.AreEqual(bytesUsed, fieldValue);

            var dictionaryId = StreamByteOps.ToInt(ref currPtr);
            Assert.AreEqual(stringUpdate.StringUpdate.DictionaryId, dictionaryId);

            var stringValue = StreamByteOps.ToString(ref currPtr, (int)fieldValue);
            Assert.AreEqual(stringUpdate.StringUpdate.Value, stringValue);

            var command = (flag & PQFieldFlags.IsUpsert) == PQFieldFlags.IsUpsert ? CrudCommand.Upsert : CrudCommand.Delete;
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
