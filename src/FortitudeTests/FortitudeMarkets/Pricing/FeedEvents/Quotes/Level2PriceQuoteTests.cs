// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

[TestClass]
public class Level2PriceQuoteTests
{
    private IList<PublishableLevel2PriceQuote> allEmptyQuotes          = null!;
    private IList<PublishableLevel2PriceQuote> allFullyPopulatedQuotes = null!;

    private PublishableLevel2PriceQuote everyLayerEmptyLevel2Quote          = null!;
    private PublishableLevel2PriceQuote everyLayerFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo fullSupportSourceTickerInfo = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private PublishableLevel2PriceQuote simpleEmptyLevel2Quote          = null!;
    private PublishableLevel2PriceQuote simpleFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo simpleSourceTickerInfo = null!;

    private PublishableLevel2PriceQuote sourceNameEmptyLevel2Quote          = null!;
    private PublishableLevel2PriceQuote sourceNameFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo sourceNameSourceTickerInfo = null!;

    private PublishableLevel2PriceQuote sourceQuoteRefEmptyLevel2Quote          = null!;
    private PublishableLevel2PriceQuote sourceQuoteRefFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo sourceRefSourceTickerInfo = null!;

    private DateTime                    testDateTime;
    private PublishableLevel2PriceQuote traderDetailsEmptyLevel2Quote          = null!;
    private PublishableLevel2PriceQuote traderDetailsFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo ordersCounterPartySourceTickerInfo = null!;

    private PublishableLevel2PriceQuote valueDateEmptyLevel2Quote          = null!;
    private PublishableLevel2PriceQuote valueDateFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo valueDateSourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        simpleSourceTickerInfo             = SourceTickerInfoTests.SimpleL2PriceVolumeSti;
        sourceNameSourceTickerInfo         = SourceTickerInfoTests.SourceNameL2PriceVolumeSti;
        sourceRefSourceTickerInfo          = SourceTickerInfoTests.SourceQuoteRefL2PriceVolumeSti;
        ordersCounterPartySourceTickerInfo = SourceTickerInfoTests.OrdersCounterPartyL2PriceVolumeSti;
        valueDateSourceTickerInfo          = SourceTickerInfoTests.ValueDateL2PriceVolumeSti;
        fullSupportSourceTickerInfo        = SourceTickerInfoTests.FullSupportL2PriceVolumeSti;

        simpleEmptyLevel2Quote          = new PublishableLevel2PriceQuote(simpleSourceTickerInfo);
        simpleFullyPopulatedLevel2Quote = new PublishableLevel2PriceQuote(simpleSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleFullyPopulatedLevel2Quote, 1);
        sourceNameEmptyLevel2Quote          = new PublishableLevel2PriceQuote(sourceNameSourceTickerInfo);
        sourceNameFullyPopulatedLevel2Quote = new PublishableLevel2PriceQuote(sourceNameSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceNameFullyPopulatedLevel2Quote, 2);
        sourceQuoteRefEmptyLevel2Quote          = new PublishableLevel2PriceQuote(sourceRefSourceTickerInfo);
        sourceQuoteRefFullyPopulatedLevel2Quote = new PublishableLevel2PriceQuote(sourceRefSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceQuoteRefFullyPopulatedLevel2Quote, 3);
        traderDetailsEmptyLevel2Quote          = new PublishableLevel2PriceQuote(ordersCounterPartySourceTickerInfo);
        traderDetailsFullyPopulatedLevel2Quote = new PublishableLevel2PriceQuote(ordersCounterPartySourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(traderDetailsFullyPopulatedLevel2Quote, 4);
        valueDateEmptyLevel2Quote          = new PublishableLevel2PriceQuote(valueDateSourceTickerInfo);
        valueDateFullyPopulatedLevel2Quote = new PublishableLevel2PriceQuote(valueDateSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateFullyPopulatedLevel2Quote, 5);
        everyLayerEmptyLevel2Quote          = new PublishableLevel2PriceQuote(fullSupportSourceTickerInfo);
        everyLayerFullyPopulatedLevel2Quote = new PublishableLevel2PriceQuote(fullSupportSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerFullyPopulatedLevel2Quote, 5);

        allFullyPopulatedQuotes = new List<PublishableLevel2PriceQuote>
        {
            simpleFullyPopulatedLevel2Quote, sourceNameFullyPopulatedLevel2Quote
          , sourceQuoteRefFullyPopulatedLevel2Quote, traderDetailsFullyPopulatedLevel2Quote
          , valueDateFullyPopulatedLevel2Quote, everyLayerFullyPopulatedLevel2Quote
        };
        allEmptyQuotes = new List<PublishableLevel2PriceQuote>
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
        Assert.AreSame(ordersCounterPartySourceTickerInfo, traderDetailsEmptyLevel2Quote.SourceTickerInfo);
        Assert.AreSame(fullSupportSourceTickerInfo, everyLayerEmptyLevel2Quote.SourceTickerInfo);
        foreach (var emptyL2Quote in allEmptyQuotes)
        {
            Assert.AreEqual(DateTime.MinValue, emptyL2Quote.SourceTime);
            Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyL2Quote.FeedMarketConnectivityStatus);
            Assert.AreEqual(0m, emptyL2Quote.SingleTickValue);
            Assert.AreEqual(DateTime.MinValue, emptyL2Quote.ClientReceivedTime);
            Assert.AreEqual(DateTime.MinValue, emptyL2Quote.AdapterReceivedTime);
            Assert.AreEqual(DateTime.MinValue, emptyL2Quote.AdapterSentTime);
            Assert.AreEqual(DateTime.MinValue, emptyL2Quote.SourceBidTime);
            Assert.AreEqual(0m, emptyL2Quote.BidPriceTop);
            Assert.AreEqual(DateTime.MinValue, emptyL2Quote.SourceAskTime);
            Assert.AreEqual(0m, emptyL2Quote.AskPriceTop);
            Assert.AreEqual(false, emptyL2Quote.Executable);
            Assert.IsNull(emptyL2Quote.ConflatedTicksCandle);
            Assert.AreEqual(new OrderBookSide(BookSide.BidBook, emptyL2Quote.SourceTickerInfo!), emptyL2Quote.BidBook);
            Assert.AreEqual(new OrderBookSide(BookSide.AskBook, emptyL2Quote.SourceTickerInfo!), emptyL2Quote.AskBook);
            Assert.IsFalse(emptyL2Quote.OrderBook.IsBidBookChanged);
            Assert.IsFalse(emptyL2Quote.OrderBook.IsAskBookChanged);
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
        var expectedCandle      = new Candle();
        var expectedDailyTickCount     = 10u;
        var expectedBidBook =
            new OrderBookSide(BookSide.BidBook, simpleSourceTickerInfo)
            {
                [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var expectedAskBook =
            new OrderBookSide(BookSide.AskBook, simpleSourceTickerInfo)
            {
                [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };

        var fromConstructor =
            new PublishableLevel2PriceQuote
                (simpleSourceTickerInfo, expectedSourceTime, new OrderBook(expectedBidBook, expectedAskBook, expectedDailyTickCount, true)
               , true, true, expectedSourceBidTime, expectedSourceAskTime, expectedSourceTime, expectedSourceTime.AddSeconds(2)
               , true,  FeedSyncStatus.Good, FeedConnectivityStatusFlags.IsAdapterReplay, expectedSingleValue, expectedCandle)
                {
                    ClientReceivedTime = expectedClientReceivedTime,
                    AdapterReceivedTime = expectedAdapterReceiveTime,
                    AdapterSentTime = expectedAdapterSentTime
                };

        Assert.AreSame(simpleSourceTickerInfo, fromConstructor.SourceTickerInfo);
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
        Assert.AreEqual(expectedDailyTickCount, fromConstructor.OrderBook.DailyTickUpdateCount);
        Assert.IsFalse(fromConstructor.OrderBook.IsBidBookChanged);
        Assert.IsFalse(fromConstructor.OrderBook.IsAskBookChanged);
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
        var expectedDailyTickCount     = 10u;
        var expectedCandle      = new Candle();
        var convertedBidBook =
            new PQOrderBookSide(BookSide.BidBook, new PQSourceTickerInfo(simpleSourceTickerInfo))
            {
                IsLadder = true, [0] = new PQPriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var convertedAskBook =
            new PQOrderBookSide(BookSide.AskBook, new PQSourceTickerInfo(simpleSourceTickerInfo))
            {
                IsLadder = true, [0] = new PQPriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };
        var expectedBidBook = new OrderBookSide(convertedBidBook);
        var expectedAskBook = new OrderBookSide(convertedAskBook);

        var fromConstructor =
            new PublishableLevel2PriceQuote
                (simpleSourceTickerInfo, expectedSourceTime, new OrderBook(convertedBidBook, convertedAskBook, expectedDailyTickCount, true)
               , true, true, expectedSourceBidTime, expectedSourceAskTime, expectedSourceTime, expectedSourceTime.AddSeconds(2)
               , true, FeedSyncStatus.Good, FeedConnectivityStatusFlags.IsAdapterReplay, expectedSingleValue, expectedCandle)
                {
                    ClientReceivedTime = expectedClientReceivedTime,
                    AdapterReceivedTime = expectedAdapterReceiveTime,
                    AdapterSentTime = expectedAdapterSentTime
                };

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
            (typeof(OrdersPriceVolumeLayer), traderDetailsEmptyLevel2Quote, traderDetailsFullyPopulatedLevel2Quote);
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
            (typeof(FullSupportPriceVolumeLayer), everyLayerEmptyLevel2Quote, everyLayerFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValues()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var copyQuote = new PublishableLevel2PriceQuote(populatedQuote);
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

            var pqBidBook = new PQOrderBookSide(originalBidBook);
            var pqAskBook = new PQOrderBookSide(originalAskBook);

            populatedQuote.OrderBook.BidSide = pqBidBook;
            populatedQuote.OrderBook.AskSide = pqAskBook;

            var copyQuote = new PublishableLevel2PriceQuote(populatedQuote);
            Assert.AreNotEqual(populatedQuote, copyQuote);
            Assert.IsTrue(populatedQuote.AreEquivalent(copyQuote));
            Assert.IsTrue(copyQuote.AreEquivalent(populatedQuote));

            populatedQuote.OrderBook.BidSide = originalBidBook;
            populatedQuote.OrderBook.AskSide = originalAskBook;
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
        var expectedCandle      = new Candle();

        foreach (var emptyQuote in allEmptyQuotes)
        {
            var expectedBidOrderBook = emptyQuote.BidBook.Clone();
            expectedBidOrderBook[0]!.Price = expectedBidPriceTop;
            var expectedAskOrderBook = emptyQuote.AskBook.Clone();
            expectedAskOrderBook[0]!.Price = expectedAskPriceTop;

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
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyQuote = new PublishableLevel2PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            emptyQuote.CopyFrom(fullyPopulatedQuote);

            Assert.AreEqual(fullyPopulatedQuote, emptyQuote);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyLowerLevelQuote = new PublishableLevel1PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
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
            var pqLevel2Quote = new PQPublishableLevel2Quote(fullyPopulatedQuote);
            var newEmpty      = new PublishableLevel2PriceQuote(fullyPopulatedQuote.SourceTickerInfo!);
            newEmpty.CopyFrom(pqLevel2Quote);
            Assert.IsTrue(newEmpty.AreEquivalent(pqLevel2Quote));
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
        }
    }

    public static PublishableLevel2PriceQuote GenerateL2QuoteWithSourceNameLayer(ISourceTickerInfo sourceTickerInfo, int i = 0)
    {
        var sourceBidBook = GenerateBook(BookSide.BidBook, 20, 1.1123m, -0.0001m, 100000m, 10000m,
                                         (price, volume) => new SourcePriceVolumeLayer(price, volume, "SourceName" + i++, true));
        var sourceAskBook = GenerateBook(BookSide.AskBook, 20, 1.1125m, 0.0001m, 100000m, 10000m,
                                         (price, volume) => new SourcePriceVolumeLayer(price, volume, "SourceName" + i++, true));

        UpdateSourceQuoteBook(sourceBidBook, 20, 20, 1);
        UpdateSourceQuoteBook(sourceAskBook, 20, 20, 1);

        // setup source quote
        return new PublishableLevel2PriceQuote
            (sourceTickerInfo, new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123)
           , new OrderBook(sourceBidBook, sourceAskBook, 15, true)
           , false, true
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(234)
           , new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(345)
           , DateTime.Parse("2015-08-06 22:07:23.123")
           , new DateTime(2015, 08, 06, 22, 07, 22)
           , true, FeedSyncStatus.Good, FeedConnectivityStatusFlags.None,  1.234538m
           , new Candle());
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


    private static void UpdateSourceQuoteBook(IOrderBookSide toUpdate, int numberOfLayers, decimal startingVolume, decimal deltaVolumePerLayer)
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
    (bool exactComparison,
        IMutablePublishableLevel2Quote commonCompareQuote,
        IMutablePublishableLevel2Quote changingQuote)
    {
        Level1PriceQuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote, changingQuote);

        OrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote.OrderBook
           , changingQuote.OrderBook, commonCompareQuote, changingQuote);
    }

    private void AssertLayerTypeIsExpected(Type expectedType, params PublishableLevel2PriceQuote[] quotesToCheck)
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
