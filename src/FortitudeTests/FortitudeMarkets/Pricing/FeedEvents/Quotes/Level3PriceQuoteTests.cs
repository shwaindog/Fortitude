// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

[TestClass]
public class Level3PriceQuoteTests
{
    private IList<PublishableLevel3PriceQuote> allEmptyQuotes = null!;

    private IList<PublishableLevel3PriceQuote> allFullyPopulatedQuotes = null!;

    private PublishableLevel3PriceQuote noOnTickLastTradedEmptyQuote          = null!;
    private PublishableLevel3PriceQuote noOnTickLastTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo noOnTickLastTradedSrcTkrInfo = null!;

    private PublishableLevel3PriceQuote paidGivenVolumeOnTickLastTradedEmptyQuote          = null!;
    private PublishableLevel3PriceQuote paidGivenVolumeOnTickLastTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo             paidGivenVolumeOnTickLastTradedSrcTkrInfo = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder             = null!;

    private PublishableLevel3PriceQuote simpleOnTickLastTradedEmptyQuote          = null!;
    private PublishableLevel3PriceQuote simpleOnTickLastTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo simpleOnTickLastTradedSrcTkrInfo = null!;

    private PublishableLevel3PriceQuote fullSupportOnTickLastTradedEmptyQuote          = null!;
    private PublishableLevel3PriceQuote fullSupportOnTickLastTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo fullSupportOnTickLastTradedSrcTkrInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        noOnTickLastTradedSrcTkrInfo              = SourceTickerInfoTests.OrdersCounterPartyL3NoOnTickLastTradedSti;
        simpleOnTickLastTradedSrcTkrInfo          = SourceTickerInfoTests.OrdersCounterPartyL3JustTradeTradeSti;
        paidGivenVolumeOnTickLastTradedSrcTkrInfo = SourceTickerInfoTests.FullSupportL3PaidOrGivenTradeSti;
        fullSupportOnTickLastTradedSrcTkrInfo     = SourceTickerInfoTests.FullSupportL3TraderNamePaidOrGivenSti;
        noOnTickLastTradedEmptyQuote              = new PublishableLevel3PriceQuote(noOnTickLastTradedSrcTkrInfo);
        noOnTickLastTradedFullyPopulatedQuote     = new PublishableLevel3PriceQuote(noOnTickLastTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(noOnTickLastTradedFullyPopulatedQuote, 9);
        simpleOnTickLastTradedEmptyQuote          = new PublishableLevel3PriceQuote(simpleOnTickLastTradedSrcTkrInfo);
        simpleOnTickLastTradedFullyPopulatedQuote = new PublishableLevel3PriceQuote(simpleOnTickLastTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleOnTickLastTradedFullyPopulatedQuote, 10);
        paidGivenVolumeOnTickLastTradedEmptyQuote = new PublishableLevel3PriceQuote(paidGivenVolumeOnTickLastTradedSrcTkrInfo);
        paidGivenVolumeOnTickLastTradedFullyPopulatedQuote =
            new PublishableLevel3PriceQuote(paidGivenVolumeOnTickLastTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(paidGivenVolumeOnTickLastTradedFullyPopulatedQuote, 10);
        fullSupportOnTickLastTradedEmptyQuote =
            new PublishableLevel3PriceQuote(fullSupportOnTickLastTradedSrcTkrInfo);
        fullSupportOnTickLastTradedFullyPopulatedQuote =
            new PublishableLevel3PriceQuote(fullSupportOnTickLastTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder
            .InitializeQuote(fullSupportOnTickLastTradedFullyPopulatedQuote, 10);


        allFullyPopulatedQuotes = new List<PublishableLevel3PriceQuote>
        {
            noOnTickLastTradedFullyPopulatedQuote, simpleOnTickLastTradedFullyPopulatedQuote
          , paidGivenVolumeOnTickLastTradedFullyPopulatedQuote, fullSupportOnTickLastTradedFullyPopulatedQuote
        };
        allEmptyQuotes = new List<PublishableLevel3PriceQuote>
        {
            noOnTickLastTradedEmptyQuote, simpleOnTickLastTradedEmptyQuote, paidGivenVolumeOnTickLastTradedEmptyQuote
          , fullSupportOnTickLastTradedEmptyQuote
        };
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(noOnTickLastTradedSrcTkrInfo, noOnTickLastTradedEmptyQuote.SourceTickerInfo);
        Assert.AreSame(simpleOnTickLastTradedSrcTkrInfo, simpleOnTickLastTradedEmptyQuote.SourceTickerInfo);
        Assert.AreSame(paidGivenVolumeOnTickLastTradedSrcTkrInfo,
                       paidGivenVolumeOnTickLastTradedEmptyQuote.SourceTickerInfo);
        Assert.AreSame(fullSupportOnTickLastTradedSrcTkrInfo,
                       fullSupportOnTickLastTradedEmptyQuote.SourceTickerInfo);
        foreach (var emptyL3Quote in allEmptyQuotes)
        {
            Assert.AreEqual(DateTime.MinValue, emptyL3Quote.SourceTime);
            Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyL3Quote.FeedMarketConnectivityStatus);
            Assert.AreEqual(0m, emptyL3Quote.SingleTickValue);
            Assert.AreEqual(DateTime.MinValue, emptyL3Quote.ClientReceivedTime);
            Assert.AreEqual(DateTime.MinValue, emptyL3Quote.AdapterReceivedTime);
            Assert.AreEqual(DateTime.MinValue, emptyL3Quote.AdapterSentTime);
            Assert.AreEqual(DateTime.MinValue, emptyL3Quote.SourceBidTime);
            Assert.AreEqual(0m, emptyL3Quote.BidPriceTop);
            Assert.AreEqual(DateTime.MinValue, emptyL3Quote.SourceAskTime);
            Assert.AreEqual(0m, emptyL3Quote.AskPriceTop);
            Assert.AreEqual(false, emptyL3Quote.Executable);
            Assert.IsNull(emptyL3Quote.ConflatedTicksCandle);
            Assert.AreEqual(new OrderBookSide(BookSide.BidBook, emptyL3Quote.SourceTickerInfo!), emptyL3Quote.BidBook);
            Assert.AreEqual(new OrderBookSide(BookSide.AskBook, emptyL3Quote.SourceTickerInfo!), emptyL3Quote.AskBook);
            Assert.IsFalse(emptyL3Quote.OrderBook.IsBidBookChanged);
            Assert.IsFalse(emptyL3Quote.OrderBook.IsAskBookChanged);
            Assert.IsTrue(emptyL3Quote.OnTickLastTraded == null ||
                          emptyL3Quote.OnTickLastTraded.Equals(new OnTickLastTraded(emptyL3Quote.SourceTickerInfo!)));
            Assert.AreEqual(0u, emptyL3Quote.BatchId);
            Assert.AreEqual(0u, emptyL3Quote.SourceQuoteReference);
            Assert.AreEqual(DateTime.MinValue, emptyL3Quote.ValueDate);
        }
    }

    [TestMethod]
    public void InitializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedTradeId            = 10u;
        var expectedBatchId            = 234567u;
        var expectedTradePrice         = 1.23156m;
        var expectedTradeFlags         = LastTradedTypeFlags.HasPaidGivenDetails;
        var expectedTradeLifeCycle     = LastTradedLifeCycleFlags.DropCopyReceived;
        var expectedSourceTime         = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSingleValue        = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime    = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime      = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop        = 2.34567m;
        var expectedSourceAskTime      = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop        = 3.45678m;
        var quoteBehavior              = PublishableQuoteInstantBehaviorFlags.NoPublishableQuoteUpdates;
        var expectedCandle             = new Candle();
        var expectedBidBook =
            new OrderBookSide(BookSide.BidBook, simpleOnTickLastTradedSrcTkrInfo)
            {
                [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var expectedAskBook =
            new OrderBookSide(BookSide.AskBook, simpleOnTickLastTradedSrcTkrInfo)
            {
                [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };
        var expectedOnTickLastTraded =
            new OnTickLastTraded(simpleOnTickLastTradedSrcTkrInfo)
            {
                [0] = new LastTrade(expectedTradeId, expectedBatchId, expectedTradePrice, new DateTime(2018, 3, 3, 10, 53, 41)
                                  , expectedTradeFlags, expectedTradeLifeCycle)
            };
        var expectedSourceQuoteRef = 678123u;
        var expectedValueDate      = new DateTime(2018, 3, 3, 10, 57, 23);

        var fromConstructor =
            new PublishableLevel3PriceQuote
                (simpleOnTickLastTradedSrcTkrInfo, expectedSourceTime, new OrderBook(expectedBidBook, expectedAskBook)
               , expectedOnTickLastTraded, quoteBehavior, expectedBatchId, expectedSourceQuoteRef, expectedValueDate, true, true
               , expectedSourceBidTime, expectedSourceAskTime, expectedSourceTime, expectedSourceTime.AddSeconds(2), true
               , FeedSyncStatus.Good, FeedConnectivityStatusFlags.IsAdapterReplay, expectedSingleValue, expectedCandle)
                {
                    ClientReceivedTime = expectedClientReceivedTime, AdapterReceivedTime = expectedAdapterReceiveTime
                  , AdapterSentTime    = expectedAdapterSentTime
                };

        Assert.AreSame(simpleOnTickLastTradedSrcTkrInfo, fromConstructor.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(FeedConnectivityStatusFlags.IsAdapterReplay, fromConstructor.FeedMarketConnectivityStatus);
        Assert.AreEqual(expectedSingleValue, fromConstructor.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, fromConstructor.ClientReceivedTime);
        Assert.AreEqual(expectedAdapterReceiveTime, fromConstructor.AdapterReceivedTime);
        Assert.AreEqual(expectedAdapterSentTime, fromConstructor.AdapterSentTime);
        Assert.AreEqual(expectedSourceBidTime, fromConstructor.SourceBidTime);
        Assert.AreEqual(expectedBidPriceTop, fromConstructor.BidPriceTop);
        Assert.AreEqual(true, fromConstructor.IsBidPriceTopChanged);
        Assert.AreEqual(expectedSourceAskTime, fromConstructor.SourceAskTime);
        Assert.AreEqual(expectedAskPriceTop, fromConstructor.AskPriceTop);
        Assert.AreEqual(true, fromConstructor.IsAskPriceTopChanged);
        Assert.AreEqual(true, fromConstructor.Executable);
        Assert.AreEqual(expectedCandle, fromConstructor.ConflatedTicksCandle);
        Assert.AreEqual(expectedBidBook, fromConstructor.BidBook);
        Assert.AreEqual(expectedAskBook, fromConstructor.AskBook);
        Assert.IsFalse(fromConstructor.OrderBook.IsBidBookChanged);
        Assert.IsFalse(fromConstructor.OrderBook.IsAskBookChanged);
        Assert.AreEqual(expectedOnTickLastTraded, fromConstructor.OnTickLastTraded);
        Assert.AreEqual(expectedBatchId, fromConstructor.BatchId);
        Assert.AreEqual(expectedSourceQuoteRef, fromConstructor.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, fromConstructor.ValueDate);
    }

    [TestMethod]
    public void NonOnTickLastTraded_New_ConvertsToOnTickLastTraded()
    {
        var expectedTradeId            = 10u;
        var expectedBatchId            = 234567u;
        var expectedTradePrice         = 1.23156m;
        var expectedTradeFlags         = LastTradedTypeFlags.HasPaidGivenDetails;
        var expectedTradeLifeCycle     = LastTradedLifeCycleFlags.DropCopyReceived;
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSingleValue        = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime    = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime      = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop        = 2.34567m;
        var expectedSourceAskTime      = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop        = 3.45678m;
        var quoteBehavior              = PublishableQuoteInstantBehaviorFlags.NoPublishableQuoteUpdates;
        var expectedCandle             = new Candle();
        var expectedBidBook =
            new OrderBookSide(BookSide.BidBook, simpleOnTickLastTradedSrcTkrInfo)
            {
                [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var expectedAskBook =
            new OrderBookSide(BookSide.AskBook, simpleOnTickLastTradedSrcTkrInfo)
            {
                [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };
        var convertedOnTickLastTraded
            = new PQOnTickLastTraded(new PQSourceTickerInfo(simpleOnTickLastTradedSrcTkrInfo))
            {
                [0] = new PQLastTrade(expectedTradeId, expectedBatchId, expectedTradePrice, new DateTime(2018, 3, 3, 10, 53, 41)
                                  , expectedTradeFlags, expectedTradeLifeCycle)
            };
        var expectedOnTickLastTraded = new OnTickLastTraded(convertedOnTickLastTraded);
        var expectedSourceQuoteRef   = 678123u;
        var expectedValueDate        = new DateTime(2018, 3, 3, 10, 57, 23);

        var fromConstructor =
            new PublishableLevel3PriceQuote
                (simpleOnTickLastTradedSrcTkrInfo, expectedSourceTime, new OrderBook(expectedBidBook, expectedAskBook), convertedOnTickLastTraded
               , quoteBehavior,  expectedBatchId, expectedSourceQuoteRef, expectedValueDate, true, true, expectedSourceBidTime
               , expectedSourceAskTime, expectedSourceTime, expectedSourceTime.AddSeconds(2), true
               , FeedSyncStatus.Good, FeedConnectivityStatusFlags.IsAdapterReplay, expectedSingleValue, expectedCandle)
                {
                    ClientReceivedTime = expectedClientReceivedTime, AdapterReceivedTime = expectedAdapterReceiveTime
                  , AdapterSentTime    = expectedAdapterSentTime,
                };

        Assert.IsInstanceOfType(fromConstructor.OnTickLastTraded, typeof(OnTickLastTraded));
        Assert.AreEqual(expectedOnTickLastTraded, fromConstructor.OnTickLastTraded);
    }

    [TestMethod]
    public void NoOnTickLastTradedLevel3Quote_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        Assert.IsNull(noOnTickLastTradedEmptyQuote.OnTickLastTraded);
        Assert.IsNull(noOnTickLastTradedFullyPopulatedQuote.OnTickLastTraded);
    }

    [TestMethod]
    public void SimpleLevel3Quote_New_BuildsOnlySimpleLastTradeEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(LastTrade), simpleOnTickLastTradedEmptyQuote, simpleOnTickLastTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void PaidGivenVolumeLevel3Quote_New_BuildsOnlyPaidGivenTradeEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(LastPaidGivenTrade), paidGivenVolumeOnTickLastTradedEmptyQuote, paidGivenVolumeOnTickLastTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void TraderPaidGivenVolumeLevel3Quote_New_BuildsLastTraderPaidGivenEntries()
    {
        AssertLastTradeTypeIsExpected(typeof(LastExternalCounterPartyTrade), fullSupportOnTickLastTradedEmptyQuote
                                    , fullSupportOnTickLastTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValues()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var copyQuote = new PublishableLevel3PriceQuote(populatedQuote);
            Assert.AreEqual(populatedQuote, copyQuote);
        }
    }

    [TestMethod]
    public void NonOnTickLastTradedPopulatedQuote_New_CopiesValuesConvertsOnTickLastTraded()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var originalOnTickLastTraded = populatedQuote.OnTickLastTraded;
            if (originalOnTickLastTraded != null)
            {
                var pqOnTickLastTraded = new PQOnTickLastTraded(originalOnTickLastTraded);
                populatedQuote.OnTickLastTraded = pqOnTickLastTraded;

                var copyQuote = new PublishableLevel3PriceQuote(populatedQuote);
                Assert.AreNotEqual(populatedQuote, copyQuote);
                Assert.IsTrue(populatedQuote.AreEquivalent(copyQuote));
                Assert.IsTrue(copyQuote.AreEquivalent(populatedQuote));

                populatedQuote.OnTickLastTraded = originalOnTickLastTraded;
            }
            else
            {
                var copyQuote = new PublishableLevel3PriceQuote(populatedQuote);
                Assert.IsNull(copyQuote.OnTickLastTraded);
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
        var expectedCandle             = new Candle();
        var expectedBatchId            = 23456u;
        var expectedSourceQuoteRef     = 56789u;
        var expectedValueDate          = new DateTime(2018, 03, 03, 17, 33, 6);

        foreach (var emptyQuote in allEmptyQuotes)
        {
            var expectedBidOrderBook = emptyQuote.BidBook.Clone();
            expectedBidOrderBook[0].Price = expectedBidPriceTop;
            var expectedAskOrderBook = emptyQuote.AskBook.Clone();
            expectedAskOrderBook[0].Price = expectedAskPriceTop;
            var expectedOnTickLastTraded                                                  = emptyQuote.OnTickLastTraded;
            if (expectedOnTickLastTraded != null) expectedOnTickLastTraded[0].TradePrice = 12345m;

            emptyQuote.SourceTime                   = expectedSourceTime;
            emptyQuote.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay;
            emptyQuote.SingleTickValue              = expectedSingleValue;
            emptyQuote.ClientReceivedTime           = expectedClientReceivedTime;
            emptyQuote.AdapterReceivedTime          = expectedAdapterReceiveTime;
            emptyQuote.AdapterSentTime              = expectedAdapterSentTime;
            emptyQuote.SourceBidTime                = expectedSourceBidTime;
            emptyQuote.BidPriceTop                  = expectedBidPriceTop;
            emptyQuote.IsBidPriceTopChanged         = true;
            emptyQuote.SourceAskTime                = expectedSourceAskTime;
            emptyQuote.AskPriceTop                  = expectedAskPriceTop;
            emptyQuote.IsAskPriceTopChanged         = true;
            emptyQuote.Executable                   = true;
            emptyQuote.ConflatedTicksCandle         = expectedCandle;
            emptyQuote.OrderBook.BidSide            = expectedBidOrderBook;
            emptyQuote.OrderBook.IsBidBookChanged   = true;
            emptyQuote.OrderBook.AskSide            = expectedAskOrderBook;
            emptyQuote.OrderBook.IsAskBookChanged   = true;
            emptyQuote.OnTickLastTraded             = expectedOnTickLastTraded;
            emptyQuote.BatchId                      = expectedBatchId;
            emptyQuote.SourceQuoteReference         = expectedSourceQuoteRef;
            emptyQuote.ValueDate                    = expectedValueDate;

            Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
            Assert.AreEqual(FeedConnectivityStatusFlags.IsAdapterReplay, emptyQuote.FeedMarketConnectivityStatus);
            Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
            Assert.AreEqual(expectedClientReceivedTime, emptyQuote.ClientReceivedTime);
            Assert.AreEqual(expectedAdapterReceiveTime, emptyQuote.AdapterReceivedTime);
            Assert.AreEqual(expectedAdapterSentTime, emptyQuote.AdapterSentTime);
            Assert.AreEqual(expectedSourceBidTime, emptyQuote.SourceBidTime);
            Assert.AreEqual(expectedBidPriceTop, emptyQuote.BidPriceTop);
            Assert.AreEqual(true, emptyQuote.IsBidPriceTopChanged);
            Assert.AreEqual(expectedSourceAskTime, emptyQuote.SourceAskTime);
            Assert.AreEqual(expectedAskPriceTop, emptyQuote.AskPriceTop);
            Assert.AreEqual(true, emptyQuote.IsAskPriceTopChanged);
            Assert.AreEqual(true, emptyQuote.Executable);
            Assert.AreEqual(expectedCandle, emptyQuote.ConflatedTicksCandle);
            Assert.AreSame(expectedBidOrderBook, emptyQuote.BidBook);
            Assert.AreEqual(true, emptyQuote.OrderBook.IsBidBookChanged);
            Assert.AreSame(expectedAskOrderBook, emptyQuote.AskBook);
            Assert.AreEqual(true, emptyQuote.OrderBook.IsAskBookChanged);
            Assert.AreEqual(expectedOnTickLastTraded, emptyQuote.OnTickLastTraded);
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
            var emptyQuote = new PublishableLevel3PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            emptyQuote.CopyFrom(fullyPopulatedQuote);

            Assert.AreEqual(fullyPopulatedQuote, emptyQuote);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyLowerLevelQuote = new PublishableLevel2PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
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
            var pqLevel3Quote = new PQPublishableLevel3Quote(fullyPopulatedQuote);
            var newEmpty      = new PublishableLevel3PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            newEmpty.CopyFrom(pqLevel3Quote);
            Assert.IsTrue(newEmpty.AreEquivalent(pqLevel3Quote));
        }
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            IMutablePublishableTickInstant clone = populatedQuote.Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutablePublishableTickInstant)((ICloneable<IPublishableTickInstant>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutablePublishableTickInstant)((IPublishableTickInstant)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutablePublishableTickInstant)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutablePublishableTickInstant)((ICloneable<IPublishableLevel1Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutablePublishableTickInstant)((IPublishableLevel1Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutablePublishableLevel1Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutablePublishableTickInstant)((ICloneable<IPublishableLevel2Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutablePublishableTickInstant)((IPublishableLevel2Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutablePublishableLevel2Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutablePublishableTickInstant)((ICloneable<IPublishableLevel3Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutablePublishableTickInstant)((IPublishableLevel3Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutablePublishableLevel3Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
        }
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedQuote, populatedQuote.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes) Assert.AreNotEqual(0, populatedQuote.GetHashCode());
    }


    [TestMethod]
    public void NoOnTickLastTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = noOnTickLastTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SimpleOnTickLastTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = simpleOnTickLastTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void PaidGivenOnTickLastTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = paidGivenVolumeOnTickLastTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void FullSupportOnTickLastTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = fullSupportOnTickLastTradedFullyPopulatedQuote;
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
            Assert.IsTrue(toString.Contains($"{nameof(q.FeedMarketConnectivityStatus)}: {q.FeedMarketConnectivityStatus}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SingleTickValue)}: {q.SingleTickValue:N5}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.ClientReceivedTime)}: {q.ClientReceivedTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AdapterReceivedTime)}: {q.AdapterReceivedTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AdapterSentTime)}: {q.AdapterSentTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SourceBidTime)}: {q.SourceBidTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.BidPriceTop)}: {q.BidPriceTop:N5}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsBidPriceTopChanged)}: {q.IsBidPriceTopChanged}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SourceAskTime)}: {q.SourceAskTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AskPriceTop)}: {q.AskPriceTop:N5}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsAskPriceTopChanged)}: {q.IsAskPriceTopChanged}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Executable)}: {q.Executable}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.ConflatedTicksCandle)}: {q.ConflatedTicksCandle}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.OrderBook)}: {q.OrderBook}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.OnTickLastTraded)}: {q.OnTickLastTraded}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.BatchId)}: {q.BatchId}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SourceQuoteReference)}: {q.SourceQuoteReference}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.ValueDate)}: {q.ValueDate:u}"));
        }
    }

    public static PublishableLevel3PriceQuote GenerateL3QuoteWithTraderLayerAndLastTrade(ISourceTickerInfo sourceTickerInfo, int i = 0)
    {
        var priceDiff = i * 0.00015m;
        var volDiff   = i * 5_000m;
        var sourceBidBook =
            GenerateBook
                (BookSide.BidBook, 20, 1.1123m + priceDiff, -0.0001m, 100000m + volDiff
               , 10000m, (price, volume) => new OrdersPriceVolumeLayer(price: price, volume: volume));
        var sourceAskBook =
            GenerateBook
                (BookSide.AskBook, 20, 1.1125m, 0.0001m, 100000m, 10000m
               , (price, volume) => new OrdersPriceVolumeLayer(price: price, volume: volume));


        var volStart = i * 1_000m;
        UpdateOrdersQuoteBook(sourceBidBook, 20, 1, 10000 + volStart, 1000 + volDiff);
        UpdateOrdersQuoteBook(sourceAskBook, 20, 1, 20000 + volStart, 500 + volDiff);
        var     toggleBool      = false;
        decimal growVolume      = 10000;
        var     traderNumber    = 1;
        var     expectedTradeId = 10u;
        var     expectedBatchId = 234567u;
        var     quoteBehavior   = PublishableQuoteInstantBehaviorFlags.NoPublishableQuoteUpdates;

        var onTickLastTraded =
            GenerateOnTickLastTraded
                (10, 1.1124m, 0.00005m + priceDiff
               , new DateTime(2015, 10, 18, 11, 33, 48) + TimeSpan.FromSeconds(i)
               , new TimeSpan(20 + 1 * TimeSpan.TicksPerMillisecond)
               , (price, time) =>
                     new LastExternalCounterPartyTrade
                         (expectedTradeId, expectedBatchId, price, time, growVolume += growVolume,i +1, "CounterPartyName_" + traderNumber,
                          i+ 10, "TraderName" + ++traderNumber, (uint)(i + 100), toggleBool = !toggleBool,
                          toggleBool = !toggleBool));

        // setup source quote
        return new PublishableLevel3PriceQuote
            (sourceTickerInfo, new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123 + i)
           , new OrderBook(sourceBidBook, sourceAskBook)
           , onTickLastTraded, quoteBehavior, 1008 + (uint)i, 43749887 + (uint)i
           , new DateTime(2017, 12, 29, 21, 0, 0).AddMilliseconds(i), false, true
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(234 + 1)
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(345 + 1)
           , DateTime.Parse("2015-08-06 22:07:23.123")
           , new DateTime(2015, 08, 06, 22, 07, 22).AddMilliseconds(i)
           , true, FeedSyncStatus.Good, FeedConnectivityStatusFlags.None, 1.234538m + priceDiff, new Candle());
    }

    private static OrderBookSide GenerateBook<T>
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

        return new OrderBookSide(bookSide, generatedLayers.Cast<IPriceVolumeLayer>().ToList());
    }


    private static OnTickLastTraded GenerateOnTickLastTraded<T>
    (int numberOfOnTickLastTradedTrades, decimal startingPrice,
        decimal deltaPrice,
        DateTime startingTime, TimeSpan deltaTime, Func<decimal, DateTime, T> generateLastTraded) where T : IMutableLastTrade
    {
        var currentPrice    = startingPrice;
        var currentDateTime = startingTime;
        var lastTrades      = new List<IMutableLastTrade>(numberOfOnTickLastTradedTrades);

        for (var i = 0; i < numberOfOnTickLastTradedTrades; i++)
        {
            var lastTrade = generateLastTraded(currentPrice, currentDateTime);
            lastTrades.Add(lastTrade);
            currentPrice    += deltaPrice;
            currentDateTime += deltaTime;
        }

        return new OnTickLastTraded(lastTrades);
    }

    private static void UpdateOrdersQuoteBook
    (IOrderBookSide toUpdate, int numberOfLayers,
        int numberOfOrdersPerLayer, decimal startingVolume, decimal deltaVolumePerLayer)
    {
        var currentVolume = startingVolume;
        for (var i = 0; i < numberOfLayers; i++)
        {
            var traderLayer = (IMutableOrdersPriceVolumeLayer)toUpdate[i];
            for (var j = 0; j < numberOfOrdersPerLayer; j++)
            {
                string? traderName                                                = null;
                if (startingVolume != 0m && deltaVolumePerLayer != 0m) traderName = $"TraderLayer{i}_{j}";

                if (traderLayer.OrdersCount <= j)
                {
                    traderLayer.Add
                        (new ExternalCounterPartyOrder
                            (new AnonymousOrder(j + 1, DateTime.Now
                           , currentVolume + j * deltaVolumePerLayer)
                            {
                                ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo(externalTraderName: traderName!)
                            }));
                }
                else
                {
                    var traderLayerInfo = (IMutableExternalCounterPartyOrder)traderLayer[j];
                    traderLayerInfo.ExternalTraderName = traderName;
                    traderLayerInfo.OrderDisplayVolume = currentVolume + j * deltaVolumePerLayer;
                }
            }

            currentVolume += deltaVolumePerLayer;
        }
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutablePublishableLevel3Quote commonCompareQuote, IMutablePublishableLevel3Quote changingQuote)
    {
        Level1PriceQuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote, changingQuote);

        OnTickLastTradedTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote.OnTickLastTraded!, changingQuote.OnTickLastTraded!
           , commonCompareQuote, changingQuote);
    }

    private void AssertLastTradeTypeIsExpected(Type expectedType, params PublishableLevel3PriceQuote[] quotesToCheck)
    {
        foreach (var level3Quote in quotesToCheck)
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
                Assert.AreEqual(expectedType, level3Quote.OnTickLastTraded![i].GetType());
    }
}
