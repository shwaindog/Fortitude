// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQHeartbeatSerializerTests
{
    private const int BufferReadWriteOffset = 5;

    private PQHeartBeatQuotesMessage firstBatchOfQuotes = null!;

    private PQLevel0Quote firstQuote = null!;

    private SourceTickerQuoteInfo    firstQuoteInfo        = null!;
    private PQLevel3Quote            fourthQuote           = null!;
    private SourceTickerQuoteInfo    fourthQuoteInfo       = null!;
    private PQHeartbeatSerializer    pqHeartBeatSerializer = null!;
    private CircularReadWriteBuffer  readWriteBuffer       = null!;
    private PQHeartBeatQuotesMessage secondBatchOfQuotes   = null!;
    private PQLevel1Quote            secondQuote           = null!;
    private SourceTickerQuoteInfo    secondQuoteInfo       = null!;
    private PQLevel2Quote            thirdQuote            = null!;
    private SourceTickerQuoteInfo    thirdQuoteInfo        = null!;

    [TestInitialize]
    public void SetUp()
    {
        firstQuoteInfo = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", Level3, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        secondQuoteInfo = new SourceTickerQuoteInfo
            (2, "TestSource", 2, "TestTicker", Level3, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        thirdQuoteInfo = new SourceTickerQuoteInfo
            (3, "TestSource", 3, "TestTicker", Level3, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        fourthQuoteInfo = new SourceTickerQuoteInfo
            (4, "TestSource", 4, "TestTicker", Level3, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);

        firstQuote  = new PQLevel0Quote(firstQuoteInfo);
        secondQuote = new PQLevel1Quote(secondQuoteInfo);
        thirdQuote  = new PQLevel2Quote(thirdQuoteInfo);
        fourthQuote = new PQLevel3Quote(fourthQuoteInfo);

        firstBatchOfQuotes
            = new PQHeartBeatQuotesMessage(new List<IPQLevel0Quote> { firstQuote, secondQuote, thirdQuote });
        secondBatchOfQuotes = new PQHeartBeatQuotesMessage(new List<IPQLevel0Quote> { thirdQuote, fourthQuote });

        readWriteBuffer = new CircularReadWriteBuffer(new byte[9000]) { ReadCursor = BufferReadWriteOffset };

        pqHeartBeatSerializer = new PQHeartbeatSerializer();
    }

    [TestMethod]
    public unsafe void TwoQuotesBatchToHeartBeat_Serialize_SetTheExpectedBytesToBuffer()
    {
        readWriteBuffer.WriteCursor = BufferReadWriteOffset;
        var amtWritten = pqHeartBeatSerializer.Serialize(readWriteBuffer, firstBatchOfQuotes);
        readWriteBuffer.WriteCursor += amtWritten;
        readWriteBuffer.ReadCursor  =  BufferReadWriteOffset;
        using var fixedBuffer = readWriteBuffer;

        var startWritten = fixedBuffer.ReadBuffer + readWriteBuffer.BufferRelativeReadCursor;
        var currPtr      = startWritten;
        Assert.AreEqual(amtWritten
                      , (PQQuoteMessageHeader.HeaderSize + 4) * firstBatchOfQuotes.QuotesToSendHeartBeats.Count);
        foreach (var firstBatchOfQuote in firstBatchOfQuotes)
        {
            var protocolVersion = *currPtr++;
            Assert.AreEqual(1, protocolVersion);
            var messageFlags = *currPtr++;
            Assert.AreEqual((byte)PQMessageFlags.None, messageFlags);
            var sourceTickerId = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(sourceTickerId, firstBatchOfQuote.SourceTickerQuoteInfo!.Id);
            var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(messagesTotalSize, (uint)PQQuoteMessageHeader.HeaderSize + sizeof(uint));
            var sequenceNumber = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(sequenceNumber, firstBatchOfQuote.PQSequenceId);
        }

        Assert.AreEqual(amtWritten, currPtr - startWritten);

        readWriteBuffer.ReadCursor  += amtWritten;
        amtWritten                  =  pqHeartBeatSerializer.Serialize(readWriteBuffer, secondBatchOfQuotes);
        readWriteBuffer.WriteCursor += amtWritten;

        startWritten = fixedBuffer.ReadBuffer + readWriteBuffer.BufferRelativeReadCursor;
        currPtr      = startWritten;
        Assert.AreEqual(amtWritten
                      , (PQQuoteMessageHeader.HeaderSize + sizeof(uint)) * secondBatchOfQuotes.QuotesToSendHeartBeats.Count);
        foreach (var firstBatchOfQuote in secondBatchOfQuotes)
        {
            var protocolVersion = *currPtr++;
            Assert.AreEqual(1, protocolVersion);
            var messageFlags = *currPtr++;
            Assert.AreEqual((byte)PQMessageFlags.None, messageFlags);
            var sourceTickerId = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(sourceTickerId, firstBatchOfQuote.SourceTickerQuoteInfo!.Id);
            var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(messagesTotalSize, (uint)PQQuoteMessageHeader.HeaderSize + sizeof(uint));
            var sequenceNumber = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(sequenceNumber, firstBatchOfQuote.PQSequenceId);
        }

        Assert.AreEqual(amtWritten, currPtr - startWritten);
    }

    [TestMethod]
    public void FullBuffer_Serialize_WritesNothingReturnsNegativeWrittenBytes()
    {
        readWriteBuffer.WriteCursor = (nint)readWriteBuffer.Length - 1;
        var amtWritten = pqHeartBeatSerializer
            .Serialize(readWriteBuffer, firstBatchOfQuotes);

        Assert.AreEqual(-1, amtWritten);
    }

    [TestMethod]
    public unsafe void AlmostFullBuffer_Serialize_WritesHeaderReturnsNegativeWrittenAmount()
    {
        readWriteBuffer.WriteCursor = (nint)readWriteBuffer.Length - 8;
        var amtWritten = pqHeartBeatSerializer.Serialize(readWriteBuffer, firstBatchOfQuotes);

        Assert.AreEqual(-1, amtWritten);
        using var fixedBuffer     = readWriteBuffer;
        var       currPtr         = fixedBuffer.ReadBuffer + readWriteBuffer.Length - 8;
        var       protocolVersion = *currPtr++;
        Assert.AreEqual(1, protocolVersion);
        var messageFlags = *currPtr++;
        Assert.AreEqual((byte)PQMessageFlags.None, messageFlags);
    }
}
