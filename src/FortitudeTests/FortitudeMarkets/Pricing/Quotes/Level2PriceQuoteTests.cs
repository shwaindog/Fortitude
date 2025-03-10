// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes;

[TestClass]
public class Level2PriceQuoteTests
{
    private IList<Level2PriceQuote> allEmptyQuotes          = null!;
    private IList<Level2PriceQuote> allFullyPopulatedQuotes = null!;

    private Level2PriceQuote everyLayerEmptyLevel2Quote          = null!;
    private Level2PriceQuote everyLayerFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo everyLayerSourceTickerInfo = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private Level2PriceQuote simpleEmptyLevel2Quote          = null!;
    private Level2PriceQuote simpleFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo simpleSourceTickerInfo = null!;

    private Level2PriceQuote sourceNameEmptyLevel2Quote          = null!;
    private Level2PriceQuote sourceNameFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo sourceNameSourceTickerInfo = null!;

    private Level2PriceQuote sourceQuoteRefEmptyLevel2Quote          = null!;
    private Level2PriceQuote sourceQuoteRefFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo sourceRefSourceTickerInfo = null!;

    private DateTime         testDateTime;
    private Level2PriceQuote traderDetailsEmptyLevel2Quote          = null!;
    private Level2PriceQuote traderDetailsFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo traderDetailsSourceTickerInfo = null!;

    private Level2PriceQuote valueDateEmptyLevel2Quote          = null!;
    private Level2PriceQuote valueDateFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo valueDateSourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        simpleSourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        sourceNameSourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceName
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        sourceRefSourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceQuoteReference
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        traderDetailsSourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        valueDateSourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.ValueDate
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        everyLayerSourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume.AllFlags()
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        simpleEmptyLevel2Quote          = new Level2PriceQuote(simpleSourceTickerInfo);
        simpleFullyPopulatedLevel2Quote = new Level2PriceQuote(simpleSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleFullyPopulatedLevel2Quote, 1);
        sourceNameEmptyLevel2Quote          = new Level2PriceQuote(sourceNameSourceTickerInfo);
        sourceNameFullyPopulatedLevel2Quote = new Level2PriceQuote(sourceNameSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceNameFullyPopulatedLevel2Quote, 2);
        sourceQuoteRefEmptyLevel2Quote          = new Level2PriceQuote(sourceRefSourceTickerInfo);
        sourceQuoteRefFullyPopulatedLevel2Quote = new Level2PriceQuote(sourceRefSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceQuoteRefFullyPopulatedLevel2Quote, 3);
        traderDetailsEmptyLevel2Quote          = new Level2PriceQuote(traderDetailsSourceTickerInfo);
        traderDetailsFullyPopulatedLevel2Quote = new Level2PriceQuote(traderDetailsSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(traderDetailsFullyPopulatedLevel2Quote, 4);
        valueDateEmptyLevel2Quote          = new Level2PriceQuote(valueDateSourceTickerInfo);
        valueDateFullyPopulatedLevel2Quote = new Level2PriceQuote(valueDateSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateFullyPopulatedLevel2Quote, 5);
        everyLayerEmptyLevel2Quote          = new Level2PriceQuote(everyLayerSourceTickerInfo);
        everyLayerFullyPopulatedLevel2Quote = new Level2PriceQuote(everyLayerSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerFullyPopulatedLevel2Quote, 5);

        allFullyPopulatedQuotes = new List<Level2PriceQuote>
        {
            simpleFullyPopulatedLevel2Quote, sourceNameFullyPopulatedLevel2Quote
          , sourceQuoteRefFullyPopulatedLevel2Quote, traderDetailsFullyPopulatedLevel2Quote
          , valueDateFullyPopulatedLevel2Quote, everyLayerFullyPopulatedLevel2Quote
        };
        allEmptyQuotes = new List<Level2PriceQuote>
        {
            simpleEmptyLevel2Quote, sourceNameEmptyLevel2Quote, sourceQuoteRefEmptyLevel2Quote
          , traderDetailsEmptyLevel2Quote, valueDateEmptyLevel2Quote, everyLayerEmptyLevel2Quote
        };

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(simpleSourceTickerInfo, simpleEmptyLevel2Quote.SourceTickerInfo);
        Assert.AreSame(sourceNameSourceTickerInfo, sourceNameEmptyLevel2Quote.SourceTickerInfo);
        Assert.AreSame(sourceRefSourceTickerInfo, sourceQuoteRefEmptyLevel2Quote.SourceTickerInfo);
        Assert.AreSame(valueDateSourceTickerInfo, valueDateEmptyLevel2Quote.SourceTickerInfo);
        Assert.AreSame(traderDetailsSourceTickerInfo, traderDetailsEmptyLevel2Quote.SourceTickerInfo);
        Assert.AreSame(everyLayerSourceTickerInfo, everyLayerEmptyLevel2Quote.SourceTickerInfo);
        foreach (var emptyL2Quote in allEmptyQuotes)
        {
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.SourceTime);
            Assert.AreEqual(false, emptyL2Quote.IsReplay);
            Assert.AreEqual(0m, emptyL2Quote.SingleTickValue);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.ClientReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.AdapterReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.AdapterSentTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.SourceBidTime);
            Assert.AreEqual(0m, emptyL2Quote.BidPriceTop);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.SourceAskTime);
            Assert.AreEqual(0m, emptyL2Quote.AskPriceTop);
            Assert.AreEqual(false, emptyL2Quote.Executable);
            Assert.IsNull(emptyL2Quote.SummaryPeriod);
            Assert.AreEqual(new OrderBook(BookSide.BidBook, emptyL2Quote.SourceTickerInfo!), emptyL2Quote.BidBook);
            Assert.AreEqual(new OrderBook(BookSide.AskBook, emptyL2Quote.SourceTickerInfo!), emptyL2Quote.AskBook);
            Assert.IsFalse(emptyL2Quote.IsBidBookChanged);
            Assert.IsFalse(emptyL2Quote.IsAskBookChanged);
        }
    }

    [TestMethod]
    public void IntializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSingleValue        = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime    = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime      = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop        = 2.34567m;
        var expectedSourceAskTime      = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop        = 3.45678m;
        var expectedPeriodSummary      = new PricePeriodSummary();
        var expectedBidBook =
            new OrderBook
                (BookSide.BidBook, simpleSourceTickerInfo)
                {
                    [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
                };
        var expectedAskBook =
            new OrderBook(BookSide.AskBook, simpleSourceTickerInfo)
            {
                [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };

        var fromConstructor =
            new Level2PriceQuote
                (simpleSourceTickerInfo, expectedSourceTime, true, FeedSyncStatus.Good, expectedSingleValue, expectedClientReceivedTime
               , expectedAdapterReceiveTime, expectedAdapterSentTime, expectedSourceBidTime, true
               , expectedSourceAskTime, expectedSourceTime, expectedSourceTime.AddSeconds(2), true, true
               , expectedPeriodSummary, expectedBidBook, true, expectedAskBook, true);

        Assert.AreSame(simpleSourceTickerInfo, fromConstructor.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(true, fromConstructor.IsReplay);
        Assert.AreEqual(expectedSingleValue, fromConstructor.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, fromConstructor.ClientReceivedTime);
        Assert.AreEqual(expectedAdapterReceiveTime, fromConstructor.AdapterReceivedTime);
        Assert.AreEqual(expectedAdapterSentTime, fromConstructor.AdapterSentTime);
        Assert.AreEqual(expectedSourceBidTime, fromConstructor.SourceBidTime);
        Assert.AreEqual(expectedBidPriceTop, fromConstructor.BidPriceTop);
        Assert.AreEqual(true, fromConstructor.IsBidPriceTopUpdated);
        Assert.AreEqual(expectedSourceAskTime, fromConstructor.SourceAskTime);
        Assert.AreEqual(expectedAskPriceTop, fromConstructor.AskPriceTop);
        Assert.AreEqual(true, fromConstructor.IsAskPriceTopUpdated);
        Assert.AreEqual(true, fromConstructor.Executable);
        Assert.AreEqual(expectedPeriodSummary, fromConstructor.SummaryPeriod);
        Assert.AreEqual(expectedBidBook, fromConstructor.BidBook);
        Assert.AreEqual(expectedAskBook, fromConstructor.AskBook);
        Assert.IsTrue(fromConstructor.IsBidBookChanged);
        Assert.IsTrue(fromConstructor.IsAskBookChanged);
    }

    [TestMethod]
    public void NonOrderBooks_New_ConvertsToOrderBook()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSingleValue        = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime    = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime      = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop        = 2.34567m;
        var expectedSourceAskTime      = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop        = 3.45678m;
        var expectedPeriodSummary      = new PricePeriodSummary();
        var convertedBidBook =
            new PQOrderBook(BookSide.BidBook, new PQSourceTickerInfo(simpleSourceTickerInfo))
            {
                [0] = new PQPriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var convertedAskBook =
            new PQOrderBook(BookSide.AskBook, new PQSourceTickerInfo(simpleSourceTickerInfo))
            {
                [0] = new PQPriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };
        var expectedBidBook = new OrderBook(convertedBidBook);
        var expectedAskBook = new OrderBook(convertedAskBook);

        var fromConstructor =
            new Level2PriceQuote
                (simpleSourceTickerInfo, expectedSourceTime, true, FeedSyncStatus.Good, expectedSingleValue, expectedClientReceivedTime
               , expectedAdapterReceiveTime, expectedAdapterSentTime, expectedSourceBidTime, true
               , expectedSourceAskTime, expectedSourceTime, expectedSourceTime.AddSeconds(2), true
               , true, expectedPeriodSummary, convertedBidBook, true, convertedAskBook, true);

        Assert.AreEqual(expectedBidBook, fromConstructor.BidBook);
        Assert.AreEqual(expectedAskBook, fromConstructor.AskBook);
    }

    [TestMethod]
    public void SimpleLevel2Quote_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PriceVolumeLayer), simpleEmptyLevel2Quote, simpleFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void SourceNameLevel2Quote_New_BuildsSourcePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(SourcePriceVolumeLayer), sourceNameEmptyLevel2Quote, sourceNameFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void SourceQuoteRefLevel2Quote_New_BuildsSourceQuoteRefPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(SourceQuoteRefPriceVolumeLayer), sourceQuoteRefEmptyLevel2Quote, sourceQuoteRefFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void TraderLevel2Quote_New_BuildsTraderPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(TraderPriceVolumeLayer), traderDetailsEmptyLevel2Quote, traderDetailsFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void ValueDateLevel2Quote_New_BuildsValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(ValueDatePriceVolumeLayer), valueDateEmptyLevel2Quote, valueDateFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void EveryLayerLevel2Quote_New_BuildsSourceQuoteRefTraderValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer), everyLayerEmptyLevel2Quote, everyLayerFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValues()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var copyQuote = new Level2PriceQuote(populatedQuote);
            Assert.AreEqual(populatedQuote, copyQuote);
        }
    }

    [TestMethod]
    public void NonOrderBookPopulatedQuote_New_CopiesValuesConvertsOrderBook()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var originalBidBook = populatedQuote.BidBook;
            var originalAskBook = populatedQuote.AskBook;

            var pqBidBook = new PQOrderBook(originalBidBook);
            var pqAskBook = new PQOrderBook(originalAskBook);

            populatedQuote.BidBook = pqBidBook;
            populatedQuote.AskBook = pqAskBook;

            var copyQuote = new Level2PriceQuote(populatedQuote);
            Assert.AreNotEqual(populatedQuote, copyQuote);
            Assert.IsTrue(populatedQuote.AreEquivalent(copyQuote));
            Assert.IsTrue(copyQuote.AreEquivalent(populatedQuote));

            populatedQuote.BidBook = originalBidBook;
            populatedQuote.AskBook = originalAskBook;
        }
    }

    [TestMethod]
    public void PopulatedQuote_Mutate_UpdatesFields()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSingleValue        = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime    = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime      = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop        = 2.34567m;
        var expectedSourceAskTime      = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop        = 3.45678m;
        var expectedPeriodSummary      = new PricePeriodSummary();

        foreach (var emptyQuote in allEmptyQuotes)
        {
            var expectedBidOrderBook = emptyQuote.BidBook.Clone();
            expectedBidOrderBook[0]!.Price = expectedBidPriceTop;
            var expectedAskOrderBook = emptyQuote.AskBook.Clone();
            expectedAskOrderBook[0]!.Price = expectedAskPriceTop;

            emptyQuote.SourceTime           = expectedSourceTime;
            emptyQuote.IsReplay             = true;
            emptyQuote.SingleTickValue      = expectedSingleValue;
            emptyQuote.ClientReceivedTime   = expectedClientReceivedTime;
            emptyQuote.AdapterReceivedTime  = expectedAdapterReceiveTime;
            emptyQuote.AdapterSentTime      = expectedAdapterSentTime;
            emptyQuote.SourceBidTime        = expectedSourceBidTime;
            emptyQuote.BidPriceTop          = expectedBidPriceTop;
            emptyQuote.IsBidPriceTopUpdated = true;
            emptyQuote.SourceAskTime        = expectedSourceAskTime;
            emptyQuote.AskPriceTop          = expectedAskPriceTop;
            emptyQuote.IsAskPriceTopUpdated = true;
            emptyQuote.Executable           = true;
            emptyQuote.SummaryPeriod        = expectedPeriodSummary;
            emptyQuote.BidBook              = expectedBidOrderBook;
            emptyQuote.IsBidBookChanged     = true;
            emptyQuote.AskBook              = expectedAskOrderBook;
            emptyQuote.IsAskBookChanged     = true;

            Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
            Assert.AreEqual(true, emptyQuote.IsReplay);
            Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
            Assert.AreEqual(expectedClientReceivedTime, emptyQuote.ClientReceivedTime);
            Assert.AreEqual(expectedAdapterReceiveTime, emptyQuote.AdapterReceivedTime);
            Assert.AreEqual(expectedAdapterSentTime, emptyQuote.AdapterSentTime);
            Assert.AreEqual(expectedSourceBidTime, emptyQuote.SourceBidTime);
            Assert.AreEqual(expectedBidPriceTop, emptyQuote.BidPriceTop);
            Assert.AreEqual(true, emptyQuote.IsBidPriceTopUpdated);
            Assert.AreEqual(expectedSourceAskTime, emptyQuote.SourceAskTime);
            Assert.AreEqual(expectedAskPriceTop, emptyQuote.AskPriceTop);
            Assert.AreEqual(true, emptyQuote.IsAskPriceTopUpdated);
            Assert.AreEqual(true, emptyQuote.Executable);
            Assert.AreEqual(expectedPeriodSummary, emptyQuote.SummaryPeriod);
            Assert.AreSame(expectedBidOrderBook, emptyQuote.BidBook);
            Assert.AreEqual(true, emptyQuote.IsBidBookChanged);
            Assert.AreSame(expectedAskOrderBook, emptyQuote.AskBook);
            Assert.AreEqual(true, emptyQuote.IsAskBookChanged);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyQuote = new Level2PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            emptyQuote.CopyFrom(fullyPopulatedQuote);

            Assert.AreEqual(fullyPopulatedQuote, emptyQuote);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyLowerLevelQuote = new Level1PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            emptyLowerLevelQuote.CopyFrom(fullyPopulatedQuote);

            Assert.AreNotEqual(fullyPopulatedQuote, emptyLowerLevelQuote);
            Assert.IsTrue(emptyLowerLevelQuote.AreEquivalent(fullyPopulatedQuote));
        }
    }

    [TestMethod]
    public void PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var pqLevel2Quote = new PQLevel2Quote(fullyPopulatedQuote);
            var newEmpty      = new Level2PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            newEmpty.CopyFrom(pqLevel2Quote);
            Assert.IsTrue(newEmpty.AreEquivalent(pqLevel2Quote));
        }
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            IMutableTickInstant clone = populatedQuote.Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableTickInstant)((ICloneable<ITickInstant>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableTickInstant)((ITickInstant)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutableTickInstant)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableTickInstant)((ICloneable<ILevel1Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableTickInstant)((ILevel1Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutableLevel1Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableTickInstant)((ICloneable<ILevel2Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableTickInstant)((ILevel2Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutableLevel2Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
        }
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedQuote,
                                                                (IMutableLevel2Quote)populatedQuote.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes) Assert.AreNotEqual(0, populatedQuote.GetHashCode());
    }

    [TestMethod]
    public void SimpleFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = simpleFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SourceNameFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = sourceNameFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SourceQuoteRefFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = sourceQuoteRefFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void TraderDetailsFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = traderDetailsFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void ValueDateFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = valueDateFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SourceQuoteRefTraderDetailsValueDateFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = everyLayerFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var q        = populatedQuote;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));

            Assert.IsTrue(toString.Contains($"{nameof(q.SourceTickerInfo)}: {q.SourceTickerInfo}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsReplay)}: {q.IsReplay}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SingleTickValue)}: {q.SingleTickValue:N5}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.ClientReceivedTime)}: {q.ClientReceivedTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AdapterReceivedTime)}: {q.AdapterReceivedTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AdapterSentTime)}: {q.AdapterSentTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SourceBidTime)}: {q.SourceBidTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.BidPriceTop)}: {q.BidPriceTop:N5}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsBidPriceTopUpdated)}: {q.IsBidPriceTopUpdated}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SourceAskTime)}: {q.SourceAskTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AskPriceTop)}: {q.AskPriceTop:N5}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsAskPriceTopUpdated)}: {q.IsAskPriceTopUpdated}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Executable)}: {q.Executable}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SummaryPeriod)}: {q.SummaryPeriod}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.BidBook)}: {q.BidBook}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsBidBookChanged)}: {q.IsBidBookChanged}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AskBook)}: {q.AskBook}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsAskBookChanged)}: {q.IsAskBookChanged}"));
        }
    }

    public static Level2PriceQuote GenerateL2QuoteWithSourceNameLayer(ISourceTickerInfo sourceTickerInfo, int i = 0)
    {
        var sourceBidBook = GenerateBook(BookSide.BidBook, 20, 1.1123m, -0.0001m, 100000m, 10000m,
                                         (price, volume) => new SourcePriceVolumeLayer(price, volume, "SourceName" + i++, true));
        var sourceAskBook = GenerateBook(BookSide.AskBook, 20, 1.1125m, 0.0001m, 100000m, 10000m,
                                         (price, volume) => new SourcePriceVolumeLayer(price, volume, "SourceName" + i++, true));

        UpdateSourceQuoteBook(sourceBidBook, 20, 20, 1);
        UpdateSourceQuoteBook(sourceAskBook, 20, 20, 1);

        // setup source quote
        return new Level2PriceQuote
            (sourceTickerInfo, new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123)
           , false, FeedSyncStatus.Good, 1.234538m
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(234)
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(345)
           , DateTime.Parse("2015-08-06 22:07:23.123")
           , new DateTime(2015, 08, 06, 22, 07, 22)
           , true, new DateTime(2015, 08, 06, 22, 07, 22)
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123)
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(2_123), false
           , true, new PricePeriodSummary(), sourceBidBook, true, sourceAskBook, true);
    }

    private static OrderBook GenerateBook<T>
    (BookSide bookSide, int numberOfLayers, decimal startingPrice, decimal deltaPricePerLayer,
        decimal startingVolume, decimal deltaVolumePerLayer, Func<decimal, decimal, T> genNewLayerObj)
        where T : IPriceVolumeLayer
    {
        var generatedLayers = new List<T>();
        var currentPrice    = startingPrice;
        var currentVolume   = startingVolume;
        for (var i = 0; i < numberOfLayers; i++)
        {
            generatedLayers.Add(genNewLayerObj(currentPrice, currentVolume));
            currentPrice  += deltaPricePerLayer;
            currentVolume += deltaVolumePerLayer;
        }

        return new OrderBook(bookSide, generatedLayers.Cast<IPriceVolumeLayer>().ToList());
    }


    private static void UpdateSourceQuoteBook(IOrderBook toUpdate, int numberOfLayers, decimal startingVolume, decimal deltaVolumePerLayer)
    {
        var currentVolume = startingVolume;
        for (var i = 0; i < numberOfLayers; i++)
        {
            var sourceLayer = (IMutableSourcePriceVolumeLayer)toUpdate[i]!;

            string? traderName                                                = null;
            if (startingVolume != 0m && deltaVolumePerLayer != 0m) traderName = $"SourceNameUpdate{i}";

            sourceLayer.SourceName = traderName;
            sourceLayer.Volume     = currentVolume + i * deltaVolumePerLayer;
        }
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
        (bool exactComparison, IMutableLevel2Quote commonCompareQuote, IMutableLevel2Quote changingQuote)
    {
        Level1PriceQuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote, changingQuote);

        OrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, (OrderBook)commonCompareQuote.BidBook
           , (OrderBook)changingQuote.BidBook, commonCompareQuote, changingQuote);

        OrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, (OrderBook)commonCompareQuote.AskBook
           , (OrderBook)changingQuote.AskBook, commonCompareQuote, changingQuote);
    }

    private void AssertLayerTypeIsExpected(Type expectedType, params Level2PriceQuote[] quotesToCheck)
    {
        foreach (var level2Quote in quotesToCheck)
            for (var i = 0; i < level2Quote.SourceTickerInfo!.MaximumPublishedLayers; i++)
            {
                Assert.AreEqual(expectedType, level2Quote.BidBook[i]?.GetType(),
                                $"BidBook[{i}] expectedType: {expectedType.Name} " +
                                $"actualType: {level2Quote.BidBook[i]?.GetType()?.Name ?? "null"}");
                Assert.AreEqual(expectedType, level2Quote.AskBook[i]?.GetType(),
                                $"BidBook[{i}] expectedType: {expectedType.Name} " +
                                $"actualType: {level2Quote.BidBook[i]?.GetType()?.Name ?? "null"}");
            }
    }
}
