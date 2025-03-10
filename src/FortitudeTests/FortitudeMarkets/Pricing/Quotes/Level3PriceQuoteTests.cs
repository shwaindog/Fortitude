// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes.LastTraded;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes;

[TestClass]
public class Level3PriceQuoteTests
{
    private IList<Level3PriceQuote> allEmptyQuotes = null!;

    private IList<Level3PriceQuote> allFullyPopulatedQuotes = null!;

    private Level3PriceQuote noRecentlyTradedEmptyQuote          = null!;
    private Level3PriceQuote noRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo noRecentlyTradedSrcTkrInfo = null!;

    private Level3PriceQuote paidGivenVolumeRecentlyTradedEmptyQuote          = null!;
    private Level3PriceQuote paidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo             paidGivenVolumeRecentlyTradedSrcTkrInfo = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder           = null!;

    private Level3PriceQuote simpleRecentlyTradedEmptyQuote          = null!;
    private Level3PriceQuote simpleRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo simpleRecentlyTradedSrcTkrInfo = null!;

    private Level3PriceQuote traderPaidGivenVolumeRecentlyTradedEmptyQuote          = null!;
    private Level3PriceQuote traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo traderPaidGivenVolumeRecentlyTradedSrcTkrInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        noRecentlyTradedSrcTkrInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.ValueDate |
                         LayerFlags.TraderCount | LayerFlags.SourceQuoteReference
           , lastTradedFlags: LastTradedFlags.None);
        simpleRecentlyTradedSrcTkrInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MinValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.ValueDate |
                         LayerFlags.TraderCount | LayerFlags.SourceQuoteReference
           , lastTradedFlags: LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice);
        paidGivenVolumeRecentlyTradedSrcTkrInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.ValueDate |
                         LayerFlags.TraderCount | LayerFlags.SourceQuoteReference
           , lastTradedFlags: LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                              LastTradedFlags.LastTradedVolume);
        traderPaidGivenVolumeRecentlyTradedSrcTkrInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.ValueDate |
                         LayerFlags.TraderCount | LayerFlags.SourceQuoteReference
           , lastTradedFlags: LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice | LastTradedFlags.TraderName);
        noRecentlyTradedEmptyQuote          = new Level3PriceQuote(noRecentlyTradedSrcTkrInfo);
        noRecentlyTradedFullyPopulatedQuote = new Level3PriceQuote(noRecentlyTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(noRecentlyTradedFullyPopulatedQuote, 9);
        simpleRecentlyTradedEmptyQuote          = new Level3PriceQuote(simpleRecentlyTradedSrcTkrInfo);
        simpleRecentlyTradedFullyPopulatedQuote = new Level3PriceQuote(simpleRecentlyTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleRecentlyTradedFullyPopulatedQuote, 10);
        paidGivenVolumeRecentlyTradedEmptyQuote = new Level3PriceQuote(paidGivenVolumeRecentlyTradedSrcTkrInfo);
        paidGivenVolumeRecentlyTradedFullyPopulatedQuote =
            new Level3PriceQuote(paidGivenVolumeRecentlyTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(paidGivenVolumeRecentlyTradedFullyPopulatedQuote, 10);
        traderPaidGivenVolumeRecentlyTradedEmptyQuote =
            new Level3PriceQuote(traderPaidGivenVolumeRecentlyTradedSrcTkrInfo);
        traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote =
            new Level3PriceQuote(traderPaidGivenVolumeRecentlyTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder
            .InitializeQuote(traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote, 10);


        allFullyPopulatedQuotes = new List<Level3PriceQuote>
        {
            noRecentlyTradedFullyPopulatedQuote, simpleRecentlyTradedFullyPopulatedQuote
          , paidGivenVolumeRecentlyTradedFullyPopulatedQuote, traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote
        };
        allEmptyQuotes = new List<Level3PriceQuote>
        {
            noRecentlyTradedEmptyQuote, simpleRecentlyTradedEmptyQuote, paidGivenVolumeRecentlyTradedEmptyQuote
          , traderPaidGivenVolumeRecentlyTradedEmptyQuote
        };
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(noRecentlyTradedSrcTkrInfo, noRecentlyTradedEmptyQuote.SourceTickerInfo);
        Assert.AreSame(simpleRecentlyTradedSrcTkrInfo, simpleRecentlyTradedEmptyQuote.SourceTickerInfo);
        Assert.AreSame(paidGivenVolumeRecentlyTradedSrcTkrInfo,
                       paidGivenVolumeRecentlyTradedEmptyQuote.SourceTickerInfo);
        Assert.AreSame(traderPaidGivenVolumeRecentlyTradedSrcTkrInfo,
                       traderPaidGivenVolumeRecentlyTradedEmptyQuote.SourceTickerInfo);
        foreach (var emptyL3Quote in allEmptyQuotes)
        {
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL3Quote.SourceTime);
            Assert.AreEqual(false, emptyL3Quote.IsReplay);
            Assert.AreEqual(0m, emptyL3Quote.SingleTickValue);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL3Quote.ClientReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL3Quote.AdapterReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL3Quote.AdapterSentTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL3Quote.SourceBidTime);
            Assert.AreEqual(0m, emptyL3Quote.BidPriceTop);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL3Quote.SourceAskTime);
            Assert.AreEqual(0m, emptyL3Quote.AskPriceTop);
            Assert.AreEqual(false, emptyL3Quote.Executable);
            Assert.IsNull(emptyL3Quote.SummaryPeriod);
            Assert.AreEqual(new OrderBook(BookSide.BidBook, emptyL3Quote.SourceTickerInfo!), emptyL3Quote.BidBook);
            Assert.AreEqual(new OrderBook(BookSide.AskBook, emptyL3Quote.SourceTickerInfo!), emptyL3Quote.AskBook);
            Assert.IsFalse(emptyL3Quote.IsBidBookChanged);
            Assert.IsFalse(emptyL3Quote.IsAskBookChanged);
            Assert.IsTrue(emptyL3Quote.RecentlyTraded == null ||
                          emptyL3Quote.RecentlyTraded.Equals(new RecentlyTraded(emptyL3Quote.SourceTickerInfo!)));
            Assert.AreEqual(0u, emptyL3Quote.BatchId);
            Assert.AreEqual(0u, emptyL3Quote.SourceQuoteReference);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL3Quote.ValueDate);
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
            new OrderBook(BookSide.BidBook, simpleRecentlyTradedSrcTkrInfo)
            {
                [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var expectedAskBook =
            new OrderBook(BookSide.AskBook, simpleRecentlyTradedSrcTkrInfo)
            {
                [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };
        var expectedRecentlyTraded =
            new RecentlyTraded(simpleRecentlyTradedSrcTkrInfo)
            {
                [0] = new LastTrade(12345m, new DateTime(2018, 3, 3, 10, 53, 41))
            };
        var expectedBatchId        = 234567u;
        var expectedSourceQuoteRef = 678123u;
        var expectedValueDate      = new DateTime(2018, 3, 3, 10, 57, 23);

        var fromConstructor =
            new Level3PriceQuote
                (simpleRecentlyTradedSrcTkrInfo, expectedSourceTime, true, FeedSyncStatus.Good, expectedSingleValue, expectedClientReceivedTime
               , expectedAdapterReceiveTime, expectedAdapterSentTime, expectedSourceBidTime, true
               , expectedSourceAskTime, expectedSourceTime, expectedSourceTime.AddSeconds(2), true, true, expectedPeriodSummary, expectedBidBook
               , true, expectedAskBook, true, expectedRecentlyTraded, expectedBatchId
               , expectedSourceQuoteRef, expectedValueDate);

        Assert.AreSame(simpleRecentlyTradedSrcTkrInfo, fromConstructor.SourceTickerInfo);
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
        Assert.AreEqual(expectedRecentlyTraded, fromConstructor.RecentlyTraded);
        Assert.AreEqual(expectedBatchId, fromConstructor.BatchId);
        Assert.AreEqual(expectedSourceQuoteRef, fromConstructor.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, fromConstructor.ValueDate);
    }

    [TestMethod]
    public void NonRecentlyTraded_New_ConvertsToRecentlyTraded()
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
        var expectedBidBook =
            new OrderBook(BookSide.BidBook, simpleRecentlyTradedSrcTkrInfo)
            {
                [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var expectedAskBook =
            new OrderBook(BookSide.AskBook, simpleRecentlyTradedSrcTkrInfo)
            {
                [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };
        var convertedRecentlyTraded
            = new PQRecentlyTraded(new PQSourceTickerInfo(simpleRecentlyTradedSrcTkrInfo))
            {
                [0] = new PQLastTrade(12345m, new DateTime(2018, 3, 3, 10, 53, 41))
            };
        var expectedRecentlyTraded = new RecentlyTraded(convertedRecentlyTraded);
        var expectedBatchId        = 234567u;
        var expectedSourceQuoteRef = 678123u;
        var expectedValueDate      = new DateTime(2018, 3, 3, 10, 57, 23);

        var fromConstructor =
            new Level3PriceQuote
                (simpleRecentlyTradedSrcTkrInfo, expectedSourceTime, true, FeedSyncStatus.Good, expectedSingleValue, expectedClientReceivedTime
               , expectedAdapterReceiveTime, expectedAdapterSentTime, expectedSourceBidTime, true
               , expectedSourceAskTime, expectedSourceTime, expectedSourceTime.AddSeconds(2), true, true
               , expectedPeriodSummary, expectedBidBook, true, expectedAskBook, true, convertedRecentlyTraded
               , expectedBatchId, expectedSourceQuoteRef, expectedValueDate);

        Assert.IsInstanceOfType(fromConstructor.RecentlyTraded, typeof(RecentlyTraded));
        Assert.AreEqual(expectedRecentlyTraded, fromConstructor.RecentlyTraded);
    }

    [TestMethod]
    public void NoRecentlyTradedLevel3Quote_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        Assert.IsNull(noRecentlyTradedEmptyQuote.RecentlyTraded);
        Assert.IsNull(noRecentlyTradedFullyPopulatedQuote.RecentlyTraded);
    }

    [TestMethod]
    public void SimpleLevel3Quote_New_BuildsOnlySimpleLastTradeEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(LastTrade), simpleRecentlyTradedEmptyQuote, simpleRecentlyTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void PaidGivenVolumeLevel3Quote_New_BuildsOnlyPaidGivenTradeEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(LastPaidGivenTrade), paidGivenVolumeRecentlyTradedEmptyQuote, paidGivenVolumeRecentlyTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void TraderPaidGivenVolumeLevel3Quote_New_BuildsLastTraderPaidGivenEntries()
    {
        AssertLastTradeTypeIsExpected(typeof(LastTraderPaidGivenTrade), traderPaidGivenVolumeRecentlyTradedEmptyQuote
                                    , traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValues()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var copyQuote = new Level3PriceQuote(populatedQuote);
            Assert.AreEqual(populatedQuote, copyQuote);
        }
    }

    [TestMethod]
    public void NonRecentlyTradedPopulatedQuote_New_CopiesValuesConvertsRecentlyTraded()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var originalRecentlyTraded = populatedQuote.RecentlyTraded;
            if (originalRecentlyTraded != null)
            {
                var pqRecentlyTraded = new PQRecentlyTraded(originalRecentlyTraded);
                populatedQuote.RecentlyTraded = pqRecentlyTraded;

                var copyQuote = new Level3PriceQuote(populatedQuote);
                Assert.AreNotEqual(populatedQuote, copyQuote);
                Assert.IsTrue(populatedQuote.AreEquivalent(copyQuote));
                Assert.IsTrue(copyQuote.AreEquivalent(populatedQuote));

                populatedQuote.RecentlyTraded = originalRecentlyTraded;
            }
            else
            {
                var copyQuote = new Level3PriceQuote(populatedQuote);
                Assert.IsNull(copyQuote.RecentlyTraded);
            }
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
        var expectedBatchId            = 23456u;
        var expectedSourceQuoteRef     = 56789u;
        var expectedValueDate          = new DateTime(2018, 03, 03, 17, 33, 6);

        foreach (var emptyQuote in allEmptyQuotes)
        {
            var expectedBidOrderBook = emptyQuote.BidBook.Clone();
            expectedBidOrderBook[0]!.Price = expectedBidPriceTop;
            var expectedAskOrderBook = emptyQuote.AskBook.Clone();
            expectedAskOrderBook[0]!.Price = expectedAskPriceTop;
            var expectedRecentlyTraded                                                = emptyQuote.RecentlyTraded;
            if (expectedRecentlyTraded != null) expectedRecentlyTraded[0]!.TradePrice = 12345m;

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
            emptyQuote.RecentlyTraded       = expectedRecentlyTraded;
            emptyQuote.BatchId              = expectedBatchId;
            emptyQuote.SourceQuoteReference = expectedSourceQuoteRef;
            emptyQuote.ValueDate            = expectedValueDate;

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
            Assert.AreEqual(expectedRecentlyTraded, emptyQuote.RecentlyTraded);
            Assert.AreEqual(expectedBatchId, emptyQuote.BatchId);
            Assert.AreEqual(expectedSourceQuoteRef, emptyQuote.SourceQuoteReference);
            Assert.AreEqual(expectedValueDate, emptyQuote.ValueDate);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyQuote = new Level3PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            emptyQuote.CopyFrom(fullyPopulatedQuote);

            Assert.AreEqual(fullyPopulatedQuote, emptyQuote);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyLowerLevelQuote = new Level2PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
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
            var pqLevel3Quote = new PQLevel3Quote(fullyPopulatedQuote);
            var newEmpty      = new Level3PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            newEmpty.CopyFrom(pqLevel3Quote);
            Assert.IsTrue(newEmpty.AreEquivalent(pqLevel3Quote));
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
            clone = (IMutableTickInstant)((ICloneable<ILevel3Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableTickInstant)((ILevel3Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutableLevel3Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
        }
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
            AssertAreEquivalentMeetsExpectedExactComparisonType
                (false, populatedQuote, (IMutableLevel3Quote)populatedQuote.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes) Assert.AreNotEqual(0, populatedQuote.GetHashCode());
    }


    [TestMethod]
    public void NoRecentlyTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = noRecentlyTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SimpleRecentlyTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = simpleRecentlyTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void PaidGivenRecentlyTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = paidGivenVolumeRecentlyTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void TraderPaidGivenRecentlyFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote;
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
            Assert.IsTrue(toString.Contains($"{nameof(q.RecentlyTraded)}: {q.RecentlyTraded}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.BatchId)}: {q.BatchId}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SourceQuoteReference)}: {q.SourceQuoteReference}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.ValueDate)}: {q.ValueDate:u}"));
        }
    }

    public static Level3PriceQuote GenerateL3QuoteWithTraderLayerAndLastTrade(ISourceTickerInfo sourceTickerInfo, int i = 0)
    {
        var priceDiff = i * 0.00015m;
        var volDiff   = i * 5_000m;
        var sourceBidBook =
            GenerateBook
                (BookSide.BidBook, 20, 1.1123m + priceDiff, -0.0001m, 100000m + volDiff
               , 10000m, (price, volume) => new TraderPriceVolumeLayer(price, volume));
        var sourceAskBook =
            GenerateBook
                (BookSide.AskBook, 20, 1.1125m, 0.0001m, 100000m, 10000m
               , (price, volume) => new TraderPriceVolumeLayer(price, volume));


        var volStart = i * 1_000m;
        UpdateTraderQuoteBook(sourceBidBook, 20, 1, 10000 + volStart, 1000 + volDiff);
        UpdateTraderQuoteBook(sourceAskBook, 20, 1, 20000 + volStart, 500 + volDiff);
        var     toggleBool   = false;
        decimal growVolume   = 10000;
        var     traderNumber = 1;

        var recentlyTraded =
            GenerateRecentlyTraded
                (10, 1.1124m, 0.00005m + priceDiff
               , new DateTime(2015, 10, 18, 11, 33, 48) + TimeSpan.FromSeconds(i)
               , new TimeSpan(20 + 1 * TimeSpan.TicksPerMillisecond)
               , (price, time) =>
                     new LastTraderPaidGivenTrade
                         (price, time, growVolume += growVolume, toggleBool = !toggleBool,
                          toggleBool = !toggleBool, "TraderName" + ++traderNumber));

        // setup source quote
        return new Level3PriceQuote
            (sourceTickerInfo, new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123 + i)
           , false, FeedSyncStatus.Good, 1.234538m + priceDiff
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(234 + 1)
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(345 + 1)
           , DateTime.Parse("2015-08-06 22:07:23.123")
           , new DateTime(2015, 08, 06, 22, 07, 22).AddMilliseconds(i)
           , true, new DateTime(2015, 08, 06, 22, 07, 22).AddMilliseconds(i)
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123 + i)
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(2_123 + i)
           , false, true, new PricePeriodSummary(), sourceBidBook, true, sourceAskBook
           , true, recentlyTraded, 1008 + (uint)i, 43749887 + (uint)i
           , new DateTime(2017, 12, 29, 21, 0, 0).AddMilliseconds(i));
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


    private static RecentlyTraded GenerateRecentlyTraded<T>
    (int numberOfRecentlyTraded, decimal startingPrice,
        decimal deltaPrice,
        DateTime startingTime, TimeSpan deltaTime, Func<decimal, DateTime, T> generateLastTraded) where T : IMutableLastTrade
    {
        var currentPrice    = startingPrice;
        var currentDateTime = startingTime;
        var lastTrades      = new List<IMutableLastTrade>(numberOfRecentlyTraded);

        for (var i = 0; i < numberOfRecentlyTraded; i++)
        {
            var lastTrade = generateLastTraded(currentPrice, currentDateTime);
            lastTrades.Add(lastTrade);
            currentPrice    += deltaPrice;
            currentDateTime += deltaTime;
        }

        return new RecentlyTraded(lastTrades);
    }

    private static void UpdateTraderQuoteBook
    (IOrderBook toUpdate, int numberOfLayers,
        int numberOfTradersPerLayer, decimal startingVolume, decimal deltaVolumePerLayer)
    {
        var currentVolume = startingVolume;
        for (var i = 0; i < numberOfLayers; i++)
        {
            var traderLayer = (IMutableTraderPriceVolumeLayer)toUpdate[i]!;
            for (var j = 0; j < numberOfTradersPerLayer; j++)
            {
                string? traderName                                                = null;
                if (startingVolume != 0m && deltaVolumePerLayer != 0m) traderName = $"TraderLayer{i}_{j}";

                if (traderLayer.Count <= j)
                {
                    traderLayer.Add(traderName!, currentVolume + j * deltaVolumePerLayer);
                }
                else
                {
                    var traderLayerInfo = traderLayer[j]!;
                    traderLayerInfo.TraderName   = traderName;
                    traderLayerInfo.TraderVolume = currentVolume + j * deltaVolumePerLayer;
                }
            }

            currentVolume += deltaVolumePerLayer;
        }
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableLevel3Quote commonCompareQuote, IMutableLevel3Quote changingQuote)
    {
        Level1PriceQuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote, changingQuote);

        RecentlyTradedTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote.RecentlyTraded!, changingQuote.RecentlyTraded!
           , commonCompareQuote, changingQuote);
    }

    private void AssertLastTradeTypeIsExpected(Type expectedType, params Level3PriceQuote[] quotesToCheck)
    {
        foreach (var level3Quote in quotesToCheck)
            for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
                Assert.AreEqual(expectedType, level3Quote.RecentlyTraded![i]!.GetType());
    }
}
