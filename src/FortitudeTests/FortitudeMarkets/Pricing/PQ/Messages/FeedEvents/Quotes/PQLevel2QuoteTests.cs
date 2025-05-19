// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

[TestClass]
public class PQLevel2QuoteTests
{
    private IList<PQPublishableLevel2Quote> allEmptyQuotes                       = null!;
    private IList<PQPublishableLevel2Quote> allFullyPopulatedQuotes              = null!;
    private PQPublishableLevel2Quote        fullSupportEmptyLevel2Quote          = null!;
    private PQPublishableLevel2Quote        fullSupportFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo fullSupportSourceTickerInfo                 = null!;
    private PQPublishableLevel2Quote     ordersAnonEmptyLevel2Quote                  = null!;
    private PQPublishableLevel2Quote     ordersAnonFullyPopulatedLevel2Quote         = null!;
    private ISourceTickerInfo ordersAnonSourceTickerInfo                  = null!;
    private PQPublishableLevel2Quote     ordersCountEmptyLevel2Quote                 = null!;
    private PQPublishableLevel2Quote     ordersCounterPartyEmptyLevel2Quote          = null!;
    private PQPublishableLevel2Quote     ordersCounterPartyFullyPopulatedLevel2Quote = null!;
    private ISourceTickerInfo ordersCounterPartySourceTickerInfo          = null!;
    private PQPublishableLevel2Quote     ordersCountFullyPopulatedLevel2Quote        = null!;
    private ISourceTickerInfo ordersCountSourceTickerInfo                 = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private PQPublishableLevel2Quote simpleEmptyLevel2Quote          = null!;
    private PQPublishableLevel2Quote simpleFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo simpleSourceTickerInfo                  = null!;
    private PQPublishableLevel2Quote     sourceNameEmptyLevel2Quote              = null!;
    private PQPublishableLevel2Quote     sourceNameFullyPopulatedLevel2Quote     = null!;
    private ISourceTickerInfo sourceNameSourceTickerInfo              = null!;
    private PQPublishableLevel2Quote     sourceQuoteRefEmptyLevel2Quote          = null!;
    private PQPublishableLevel2Quote     sourceQuoteRefFullyPopulatedLevel2Quote = null!;
    private ISourceTickerInfo sourceRefSourceTickerInfo               = null!;

    private PQPublishableLevel2Quote     valueDateEmptyLevel2Quote          = null!;
    private PQPublishableLevel2Quote     valueDateFullyPopulatedLevel2Quote = null!;
    private ISourceTickerInfo valueDateSourceTickerInfo          = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        simpleSourceTickerInfo             = PQSourceTickerInfoTests.SimpleL3TraderNamePaidOrGivenSti;
        sourceNameSourceTickerInfo         = PQSourceTickerInfoTests.SourceNameL3TraderNamePaidOrGivenSti;
        sourceRefSourceTickerInfo          = PQSourceTickerInfoTests.SourceQuoteRefL3TraderNamePaidOrGivenSti;
        ordersCountSourceTickerInfo        = PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        ordersAnonSourceTickerInfo         = PQSourceTickerInfoTests.OrdersAnonL3JustTradeTradeSti;
        ordersCounterPartySourceTickerInfo = PQSourceTickerInfoTests.OrdersCounterPartyL3TraderNamePaidOrGivenSti;
        valueDateSourceTickerInfo          = PQSourceTickerInfoTests.ValueDateL3TraderNamePaidOrGivenSti;
        fullSupportSourceTickerInfo        = PQSourceTickerInfoTests.FullSupportL3TraderNamePaidOrGivenSti;
        simpleEmptyLevel2Quote             = new PQPublishableLevel2Quote(simpleSourceTickerInfo.Clone()) { HasUpdates = false };
        simpleFullyPopulatedLevel2Quote    = new PQPublishableLevel2Quote(simpleSourceTickerInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(simpleFullyPopulatedLevel2Quote, 1);
        sourceNameEmptyLevel2Quote          = new PQPublishableLevel2Quote(sourceNameSourceTickerInfo.Clone()) { HasUpdates = false };
        sourceNameFullyPopulatedLevel2Quote = new PQPublishableLevel2Quote(sourceNameSourceTickerInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(sourceNameFullyPopulatedLevel2Quote, 2);
        sourceQuoteRefEmptyLevel2Quote          = new PQPublishableLevel2Quote(sourceRefSourceTickerInfo.Clone()) { HasUpdates = false };
        sourceQuoteRefFullyPopulatedLevel2Quote = new PQPublishableLevel2Quote(sourceRefSourceTickerInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(sourceQuoteRefFullyPopulatedLevel2Quote, 3);
        ordersCountEmptyLevel2Quote          = new PQPublishableLevel2Quote(ordersCountSourceTickerInfo.Clone()) { HasUpdates = false };
        ordersCountFullyPopulatedLevel2Quote = new PQPublishableLevel2Quote(ordersCountSourceTickerInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(ordersCountFullyPopulatedLevel2Quote, 4);
        ordersAnonEmptyLevel2Quote          = new PQPublishableLevel2Quote(ordersAnonSourceTickerInfo.Clone()) { HasUpdates = false };
        ordersAnonFullyPopulatedLevel2Quote = new PQPublishableLevel2Quote(ordersAnonSourceTickerInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(ordersAnonFullyPopulatedLevel2Quote, 4);
        ordersCounterPartyEmptyLevel2Quote          = new PQPublishableLevel2Quote(ordersCounterPartySourceTickerInfo.Clone()) { HasUpdates = false };
        ordersCounterPartyFullyPopulatedLevel2Quote = new PQPublishableLevel2Quote(ordersCounterPartySourceTickerInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(ordersCounterPartyFullyPopulatedLevel2Quote, 4);
        valueDateEmptyLevel2Quote          = new PQPublishableLevel2Quote(valueDateSourceTickerInfo.Clone()) { HasUpdates = false };
        valueDateFullyPopulatedLevel2Quote = new PQPublishableLevel2Quote(valueDateSourceTickerInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateFullyPopulatedLevel2Quote, 5);
        fullSupportEmptyLevel2Quote          = new PQPublishableLevel2Quote(fullSupportSourceTickerInfo.Clone()) { HasUpdates = false };
        fullSupportFullyPopulatedLevel2Quote = new PQPublishableLevel2Quote(fullSupportSourceTickerInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(fullSupportFullyPopulatedLevel2Quote, 5);

        allFullyPopulatedQuotes = new List<PQPublishableLevel2Quote>
        {
            simpleFullyPopulatedLevel2Quote, sourceNameFullyPopulatedLevel2Quote
          , sourceQuoteRefFullyPopulatedLevel2Quote, ordersCountFullyPopulatedLevel2Quote
          , ordersAnonFullyPopulatedLevel2Quote, ordersCounterPartyFullyPopulatedLevel2Quote
          , valueDateFullyPopulatedLevel2Quote, fullSupportFullyPopulatedLevel2Quote
        };
        // {
        //     everyLayerFullyPopulatedLevel2Quote
        // };
        allEmptyQuotes = new List<PQPublishableLevel2Quote>
        {
            simpleEmptyLevel2Quote, sourceNameEmptyLevel2Quote, sourceQuoteRefEmptyLevel2Quote
          , ordersCountEmptyLevel2Quote, ordersAnonEmptyLevel2Quote, ordersCounterPartyEmptyLevel2Quote, valueDateEmptyLevel2Quote
          , fullSupportEmptyLevel2Quote
        };
    }

    [TestMethod]
    public void TooLargeMaxBookDepth_New_CapsBookDepthTo()
    {
        var tooLargeMaxDepth =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , ushort.MaxValue, 0.00001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        var cappedLevel2Quote = new PQPublishableLevel2Quote(tooLargeMaxDepth);
        Assert.AreEqual(PQQuoteFieldsExtensions.TwoByteFieldIdMaxBookDepth, cappedLevel2Quote.BidBook.Capacity);
        Assert.AreEqual(PQQuoteFieldsExtensions.TwoByteFieldIdMaxBookDepth, cappedLevel2Quote.AskBook.Capacity);
    }

    [TestMethod]
    public void TooSmallMaxBookDepth_New_IncreaseBookDepthAtLeast1Level()
    {
        var tooLargeMaxDepth =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 0, 0.00001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        var cappedLevel2Quote = new PQPublishableLevel2Quote(tooLargeMaxDepth);
        Assert.AreEqual(1, cappedLevel2Quote.BidBook.Capacity);
        Assert.AreEqual(1, cappedLevel2Quote.AskBook.Capacity);
    }

    [TestMethod]
    public void SimpleLevel2Quote_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQPriceVolumeLayer), simpleEmptyLevel2Quote, simpleFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void SourceNameLevel2Quote_New_BuildsSourcePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQSourcePriceVolumeLayer), sourceNameEmptyLevel2Quote, sourceNameFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void SourceQuoteRefLevel2Quote_New_BuildsSourceQuoteRefPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQSourceQuoteRefPriceVolumeLayer), sourceQuoteRefEmptyLevel2Quote, sourceQuoteRefFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void OrdersCountLevel2Quote_New_BuildsTraderPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQOrdersCountPriceVolumeLayer), ordersCountEmptyLevel2Quote, ordersCountFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void OrdersAnonLevel2Quote_New_BuildsTraderPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQOrdersPriceVolumeLayer), ordersAnonEmptyLevel2Quote, ordersAnonFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void OrdersCounterPartyLevel2Quote_New_BuildsTraderPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQOrdersPriceVolumeLayer), ordersCounterPartyEmptyLevel2Quote, ordersCounterPartyFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void ValueDateLevel2Quote_New_BuildsValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQValueDatePriceVolumeLayer), valueDateEmptyLevel2Quote, valueDateFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void EveryLayerLevel2Quote_New_BuildsSourceQuoteRefTraderValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQFullSupportPriceVolumeLayer), fullSupportEmptyLevel2Quote, fullSupportFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_IsBookChanged_SetsAndReadsBookChangedStatus()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            Assert.IsTrue(populatedL2Quote.OrderBook.IsBidBookChanged);
            Assert.IsTrue(populatedL2Quote.OrderBook.IsAskBookChanged);
            Assert.IsTrue(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsTrue(populatedL2Quote.AskBook.HasUpdates);

            populatedL2Quote.OrderBook.IsBidBookChanged = false;
            populatedL2Quote.OrderBook.IsAskBookChanged = false;

            Assert.IsFalse(populatedL2Quote.OrderBook.IsBidBookChanged);
            Assert.IsFalse(populatedL2Quote.OrderBook.IsAskBookChanged);
            Assert.IsFalse(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsFalse(populatedL2Quote.AskBook.HasUpdates);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_IsHasUpdates_SetsAndReadsBookChangedStatus()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            Assert.IsTrue(populatedL2Quote.HasUpdates);
            Assert.IsTrue(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsTrue(populatedL2Quote.AskBook.HasUpdates);

            populatedL2Quote.HasUpdates = false;

            Assert.IsFalse(populatedL2Quote.HasUpdates);
            Assert.IsFalse(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsFalse(populatedL2Quote.AskBook.HasUpdates);
        }
    }

    [TestMethod]
    public void SimpleLevel2Quote_OrderBookViaInterfaces_RetrievesSameInstance()
    {
        var orderBidBookViaClass = simpleFullyPopulatedLevel2Quote.BidBook;
        var orderAskBookViaClass = simpleFullyPopulatedLevel2Quote.AskBook;

        Assert.AreSame(orderBidBookViaClass, ((IPublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, ((IPQPublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);

        Assert.AreSame(orderAskBookViaClass, ((IPublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, ((IPQPublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
    }

    [TestMethod]
    public void SimpleLevel2Quote_MutableSetOrderBook_OnlyAllowsPQOrderBookToBeSet()
    {
        var orderBidBookViaClass = simpleFullyPopulatedLevel2Quote.BidBook;
        var orderAskBookViaClass = simpleFullyPopulatedLevel2Quote.AskBook;

        var caughtException = false;
        var nonPQOrderBook  = new OrderBookSide(BookSide.AskBook, new List<IPriceVolumeLayer>());

        try
        {
            ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).OrderBook.BidSide = nonPQOrderBook;
        }
        catch (InvalidCastException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
        Assert.AreSame(orderBidBookViaClass, ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, simpleFullyPopulatedLevel2Quote.BidBook);

        caughtException = false;

        try
        {
            ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).OrderBook.AskSide = nonPQOrderBook;
        }
        catch (InvalidCastException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
        Assert.AreSame(orderAskBookViaClass, ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, simpleFullyPopulatedLevel2Quote.AskBook);

        var newPqOrderBook = new PQOrderBookSide(BookSide.BidBook, new List<IPriceVolumeLayer>());
        ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).OrderBook.BidSide = newPqOrderBook;
        ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).OrderBook.AskSide = newPqOrderBook;
        Assert.AreSame(newPqOrderBook, ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(newPqOrderBook, simpleFullyPopulatedLevel2Quote.BidBook);
        Assert.AreSame(newPqOrderBook, ((IMutablePublishableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(newPqOrderBook, simpleFullyPopulatedLevel2Quote.AskBook);
    }

    [TestMethod]
    public void SimpleLevel2Quote_AskPriceTop_SameAsBookTickInstant()
    {
        var orderAskBookViaClass = simpleFullyPopulatedLevel2Quote.AskBook;

        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.AskPriceTop, orderAskBookViaClass[0]!.Price);

        orderAskBookViaClass[0]!.Price = 456321m;
        Assert.AreEqual(456321m, simpleFullyPopulatedLevel2Quote.AskPriceTop);
        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.AskPriceTop, orderAskBookViaClass[0]!.Price);

        simpleFullyPopulatedLevel2Quote.AskPriceTop = 12398745m;
        Assert.AreEqual(12398745m, orderAskBookViaClass[0]!.Price);
        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.AskPriceTop, orderAskBookViaClass[0]!.Price);
    }

    [TestMethod]
    public void SimpleLevel2Quote_BidPriceTop_SameAsBookTickInstant()
    {
        var orderBidBookViaClass = simpleFullyPopulatedLevel2Quote.BidBook;

        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.BidPriceTop, orderBidBookViaClass[0]!.Price);

        orderBidBookViaClass[0]!.Price = 456321m;
        Assert.AreEqual(456321m, simpleFullyPopulatedLevel2Quote.BidPriceTop);
        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.BidPriceTop, orderBidBookViaClass[0]!.Price);

        simpleFullyPopulatedLevel2Quote.BidPriceTop = 12398745m;
        Assert.AreEqual(12398745m, orderBidBookViaClass[0]!.Price);
        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.BidPriceTop, orderBidBookViaClass[0]!.Price);
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_OrderBookTotalDailyTickUpdateCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;

            PQOrderBookTests.AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected(orderBook, emptyQuote);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_OrderBookTotalOpenInterestFieldsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook         = emptyQuote.OrderBook;
            var totalOpenInterest = orderBook.OpenInterest!;
            totalOpenInterest.HasUpdates = false;

            PQMarketAggregateTests.AssertAllMarketAggregateFieldsUpdatesReturnAsExpected
                (totalOpenInterest, PQFeedFields.QuoteOpenInterestTotal, null, orderBook, emptyQuote);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_AllOrderBookBidSideFieldsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook         = emptyQuote.OrderBook;
            var bidBook           = orderBook.BidSide;
            var sideOpenInterest = bidBook.OpenInterestSide!;
            sideOpenInterest.HasUpdates = false;

            PQMarketAggregateTests.AssertAllMarketAggregateFieldsUpdatesReturnAsExpected
                (sideOpenInterest, PQFeedFields.QuoteOpenInterestSided, bidBook, orderBook, emptyQuote);

            PQOrderBookSideTests.AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected(bidBook, orderBook, emptyQuote);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_AllOrderBookAskSideFieldsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook         = emptyQuote.OrderBook;
            var askBook           = orderBook.AskSide;
            var sideOpenInterest = askBook.OpenInterestSide!;
            sideOpenInterest.HasUpdates = false;

            PQMarketAggregateTests.AssertAllMarketAggregateFieldsUpdatesReturnAsExpected
                (sideOpenInterest, PQFeedFields.QuoteOpenInterestSided, askBook, orderBook, emptyQuote);

            PQOrderBookSideTests.AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected(askBook, orderBook, emptyQuote);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerPriceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            var bidBook   = orderBook.BidSide;

            for (int i = 0; i < bidBook.Capacity; i++)
            {
                var priceVolumeLayer = bidBook[i]!;
                PQPriceVolumeLayerTests.AssertPriceFieldUpdatesReturnAsExpected(priceVolumeLayer, i, bidBook, orderBook, emptyQuote);
            }
            var askBook = orderBook.AskSide;

            for (int i = 0; i < askBook.Capacity; i++)
            {
                var priceVolumeLayer = askBook[i]!;
                PQPriceVolumeLayerTests.AssertPriceFieldUpdatesReturnAsExpected(priceVolumeLayer, i, askBook, orderBook, emptyQuote);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            var bidBook   = orderBook.BidSide;

            for (int i = 0; i < bidBook.Capacity; i++)
            {
                var priceVolumeLayer = bidBook[i]!;
                PQPriceVolumeLayerTests.AssertVolumeFieldUpdatesReturnAsExpected(priceVolumeLayer, i, bidBook, orderBook, emptyQuote);
            }
            var askBook = orderBook.AskSide;

            for (int i = 0; i < askBook.Capacity; i++)
            {
                var priceVolumeLayer = askBook[i]!;
                PQPriceVolumeLayerTests.AssertVolumeFieldUpdatesReturnAsExpected(priceVolumeLayer, i, askBook, orderBook, emptyQuote);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerSourceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.SourcePriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQSourcePriceVolumeLayer)bidBook[i]!;
                    PQSourcePriceVolumeLayerTests.AssertSourceNameFieldUpdatesReturnAsExpected
                        (ordersCountLayer, i, bidBook, orderBook, emptyQuote);
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQSourcePriceVolumeLayer)askBook[i]!;
                    PQSourcePriceVolumeLayerTests.AssertSourceNameFieldUpdatesReturnAsExpected
                        (ordersCountLayer, i, askBook, orderBook, emptyQuote);
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.SourcePriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQSourcePriceVolumeLayer)bidBook[i]!;
                    PQSourcePriceVolumeLayerTests.AssertExecutableFieldUpdatesReturnAsExpected
                        (ordersCountLayer, i, bidBook, orderBook, emptyQuote);
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQSourcePriceVolumeLayer)askBook[i]!;
                    PQSourcePriceVolumeLayerTests.AssertExecutableFieldUpdatesReturnAsExpected
                        (ordersCountLayer, i, askBook, orderBook, emptyQuote);
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerSourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.SourceQuoteRefPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQSourceQuoteRefPriceVolumeLayer)bidBook[i]!;
                    PQSourceQuoteRefPriceVolumeLayerTests.AssertSourceQuoteRefFieldUpdatesReturnAsExpected
                        (ordersCountLayer, i, bidBook, orderBook, emptyQuote);
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQSourceQuoteRefPriceVolumeLayer)askBook[i]!;
                    PQSourceQuoteRefPriceVolumeLayerTests.AssertSourceQuoteRefFieldUpdatesReturnAsExpected
                        (ordersCountLayer, i, askBook, orderBook, emptyQuote);
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerValueDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.ValueDatePriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQValueDatePriceVolumeLayer)bidBook[i]!;
                    PQValueDatePriceVolumeLayerTests.AssertValueDateFieldUpdatesReturnAsExpected
                        (ordersCountLayer, i, bidBook, orderBook, emptyQuote);
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQValueDatePriceVolumeLayer)askBook[i]!;
                    PQValueDatePriceVolumeLayerTests.AssertValueDateFieldUpdatesReturnAsExpected
                        (ordersCountLayer, i, askBook, orderBook, emptyQuote);
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersCountPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQOrdersCountPriceVolumeLayer)bidBook[i]!;
                    PQOrdersCountPriceVolumeLayerTests.AssertOrdersCountFieldUpdatesReturnAsExpected(ordersCountLayer, i, bidBook, orderBook
                   , emptyQuote);
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQOrdersCountPriceVolumeLayer)askBook[i]!;
                    PQOrdersCountPriceVolumeLayerTests.AssertOrdersCountFieldUpdatesReturnAsExpected(ordersCountLayer, i, askBook, orderBook
                   , emptyQuote);
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerInternalVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersCountPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQOrdersCountPriceVolumeLayer)bidBook[i]!;
                    PQOrdersCountPriceVolumeLayerTests.AssertInternalVolumeFieldUpdatesReturnAsExpected(ordersCountLayer, i, bidBook, orderBook
                   , emptyQuote);
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQOrdersCountPriceVolumeLayer)askBook[i]!;
                    PQOrdersCountPriceVolumeLayerTests.AssertInternalVolumeFieldUpdatesReturnAsExpected(ordersCountLayer, i, askBook, orderBook
                   , emptyQuote);
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderIdFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderIdFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
                       , orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderFlagsFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderFlagsFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
                       , orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderCreatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderCreatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderCreatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , askBook, orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderUpdatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderUpdatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderUpdatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , askBook, orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
                       , orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderRemainingVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderRemainingVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderRemainingVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , askBook, orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderCounterPartyNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersFullPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQCounterPartyOrderLayerInfo)ordersLayer[j]!;
                        PQCounterPartyOrderLayerInfoTests.AssertOrdersCounterPartyNameFieldUpdatesReturnAsExpected
                            (cpOrderInfo, j, ordersLayer, i, bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQCounterPartyOrderLayerInfo)ordersLayer[j]!;
                        PQCounterPartyOrderLayerInfoTests.AssertOrdersCounterPartyNameFieldUpdatesReturnAsExpected
                            (cpOrderInfo, j, ordersLayer, i, askBook, orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersFullPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQCounterPartyOrderLayerInfo)ordersLayer[j]!;
                        PQCounterPartyOrderLayerInfoTests.AssertOrdersTraderNameFieldUpdatesReturnAsExpected(cpOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQCounterPartyOrderLayerInfo)ordersLayer[j]!;
                        PQCounterPartyOrderLayerInfoTests.AssertOrdersTraderNameFieldUpdatesReturnAsExpected(cpOrderInfo, j, ordersLayer, i, askBook
                       , orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllFullyPopulatedQuotes_Reset_SameAsEmptyQuotes()
    {
        Assert.AreEqual(allFullyPopulatedQuotes.Count, allEmptyQuotes.Count);
        for (var i = 0; i < allFullyPopulatedQuotes.Count; i++)
        {
            var popQuote   = allFullyPopulatedQuotes[i];
            var emptyQuote = allEmptyQuotes[i];

            Assert.IsFalse(popQuote.AreEquivalent(emptyQuote));

            popQuote.ResetWithTracking();

            Assert.IsTrue(popQuote.AreEquivalent(emptyQuote));
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel2Fields()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var precisionSettings = (PQSourceTickerInfo)populatedL2Quote.SourceTickerInfo!;
            var pqFieldUpdates =
                populatedL2Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Update, precisionSettings).ToList();
            AssertContainsAllLevel2Fields(precisionSettings, pqFieldUpdates, populatedL2Quote);
        }
    }

    [TestMethod]
    public void TraderPopulatedWithUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel2Fields()
    {
        var pqFieldUpdates =
            ordersCountFullyPopulatedLevel2Quote
                .GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllLevel2Fields
            ((PQSourceTickerInfo)ordersCountFullyPopulatedLevel2Quote.SourceTickerInfo!, pqFieldUpdates
           , ordersCountFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel2Fields()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            populatedL2Quote.HasUpdates = false;
            var pqFieldUpdates =
                populatedL2Quote
                    .GetDeltaUpdateFields
                        (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Snapshot).ToList();
            AssertContainsAllLevel2Fields
                ((PQSourceTickerInfo)populatedL2Quote.SourceTickerInfo!, pqFieldUpdates, populatedL2Quote
               , PQQuoteBooleanValuesExtensions.ExpectedL1QuoteSnapshotBooleanValues);
        }
    }

    [TestMethod]
    public void OrdersCountPopulatedWithUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel2Fields()
    {
        ordersCountFullyPopulatedLevel2Quote.HasUpdates = false;
        var pqFieldUpdates =
            ordersCountFullyPopulatedLevel2Quote
                .GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Snapshot).ToList();
        AssertContainsAllLevel2Fields
            ((PQSourceTickerInfo)ordersCountFullyPopulatedLevel2Quote.SourceTickerInfo!, pqFieldUpdates, ordersCountFullyPopulatedLevel2Quote
            , PQQuoteBooleanValuesExtensions.ExpectedL1QuoteSnapshotBooleanValues);
    }

    [TestMethod]
    public void OrdersAnonPopulatedWithUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel2Fields()
    {
        ordersAnonFullyPopulatedLevel2Quote.HasUpdates = false;
        var pqFieldUpdates =
            ordersAnonFullyPopulatedLevel2Quote
                .GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Snapshot).ToList();
        AssertContainsAllLevel2Fields
            ((PQSourceTickerInfo)ordersAnonFullyPopulatedLevel2Quote.SourceTickerInfo!, pqFieldUpdates, ordersAnonFullyPopulatedLevel2Quote
           , PQQuoteBooleanValuesExtensions.ExpectedL1QuoteSnapshotBooleanValues);
    }

    [TestMethod]
    public void OrdersCounterPartyPopulatedWithUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel2Fields()
    {
        ordersCountFullyPopulatedLevel2Quote.HasUpdates = false;
        var pqFieldUpdates =
            ordersCountFullyPopulatedLevel2Quote
                .GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Snapshot).ToList();
        AssertContainsAllLevel2Fields
            ((PQSourceTickerInfo)ordersCountFullyPopulatedLevel2Quote.SourceTickerInfo!, pqFieldUpdates, ordersCountFullyPopulatedLevel2Quote
            , PQQuoteBooleanValuesExtensions.ExpectedL1QuoteSnapshotBooleanValues);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            populatedL2Quote.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay;
            populatedL2Quote.HasUpdates = false;
            var pqFieldUpdates =
                populatedL2Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            var pqStringUpdates =
                populatedL2Quote.GetStringUpdates
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            Assert.AreEqual(0, pqStringUpdates.Count);
        }
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var pqFieldUpdates =
                populatedL2Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var pqStringUpdates =
                populatedL2Quote.GetStringUpdates
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL2Quote.SourceTickerInfo!)
                {
                    NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates)
                };
            var newEmpty = new PQPublishableLevel2Quote(emptyQuoteSourceTickerInfo);
            foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
            foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
            // not copied from field updates as is used in by server to track publication times.
            newEmpty.PQSequenceId        = populatedL2Quote.PQSequenceId;
            newEmpty.LastPublicationTime = populatedL2Quote.LastPublicationTime;

            Assert.AreEqual(populatedL2Quote, newEmpty
                          , "Expected quotes to be the same but were \n " + populatedL2Quote.DiffQuotes(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL2Quote.SourceTickerInfo!);
            var newEmpty = new PQPublishableLevel2Quote(emptyQuoteSourceTickerInfo);
            newEmpty.CopyFrom(populatedL2Quote);
            Assert.AreEqual(populatedL2Quote, newEmpty, $"\n Differences are {populatedL2Quote.DiffQuotes(newEmpty)}");
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL2Quote.SourceTickerInfo!);
            var emptyQuote = new PQPublishableLevel2Quote(emptyQuoteSourceTickerInfo);
            populatedL2Quote.HasUpdates = false;
            emptyQuote.CopyFrom(populatedL2Quote);
            Assert.AreEqual(populatedL2Quote.PQSequenceId, emptyQuote.PQSequenceId);
            Assert.AreEqual(default, emptyQuote.SourceTime);
            Assert.IsTrue(populatedL2Quote.SourceTickerInfo!.AreEquivalent(emptyQuote.SourceTickerInfo));
            Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyQuote.FeedMarketConnectivityStatus);
            Assert.AreEqual(0m, emptyQuote.SingleTickValue);
            Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
            Assert.AreEqual(default, emptyQuote.SourceBidTime);
            Assert.AreEqual(default, emptyQuote.SourceAskTime);
            Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
            Assert.AreEqual(default, emptyQuote.AdapterSentTime);
            Assert.AreEqual(default, emptyQuote.ClientReceivedTime);
            Assert.AreEqual(default, emptyQuote.InboundProcessedTime);
            Assert.AreEqual(default, emptyQuote.SubscriberDispatchedTime);
            Assert.AreEqual(default, emptyQuote.InboundSocketReceivingTime);
            Assert.AreEqual(0m, emptyQuote.BidPriceTop);
            Assert.AreEqual(0m, emptyQuote.AskPriceTop);
            Assert.IsTrue(emptyQuote.Executable);
            Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsFeedConnectivityStatusUpdated);
            Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
            Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
            Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
            Assert.IsFalse(emptyQuote.IsExecutableUpdated);
            foreach (var pvl in emptyQuote.BidBook) AssertAreDefaultValues(pvl);
            foreach (var pvl in emptyQuote.AskBook) AssertAreDefaultValues(pvl);
        }
    }

    [TestMethod]
    public void NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var nonPQLevel2Quote = new PublishableLevel2PriceQuote(populatedL2Quote);
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL2Quote.SourceTickerInfo!);
            var newEmpty = new PQPublishableLevel2Quote(emptyQuoteSourceTickerInfo);
            newEmpty.CopyFrom(nonPQLevel2Quote, CopyMergeFlags.Default);
            Assert.IsTrue(populatedL2Quote.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var clonedQuote = ((ICloneable<IPublishableTickInstant>)populatedL2Quote).Clone();
            Assert.AreNotSame(clonedQuote, populatedL2Quote);
            if (!clonedQuote.Equals(populatedL2Quote))
                Console.Out.WriteLine("clonedQuote differences are \n" + clonedQuote.DiffQuotes(populatedL2Quote) + "'");
            Assert.AreEqual(populatedL2Quote, clonedQuote);

            var cloned2 = (PQPublishableLevel2Quote)((ICloneable)populatedL2Quote).Clone();
            Assert.AreNotSame(cloned2, populatedL2Quote);
            if (!cloned2.Equals(populatedL2Quote))
                Console.Out.WriteLine("clonedQuote differences are \n '" + cloned2.DiffQuotes(populatedL2Quote) + "'");
            Assert.AreEqual(populatedL2Quote, cloned2);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            populatedL2Quote.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay;
            var fullyPopulatedClone = (PQPublishableLevel2Quote)((ICloneable)populatedL2Quote).Clone();
            // by default SourceTickerInfo is shared
            fullyPopulatedClone.SourceTickerInfo
                = new PQSourceTickerInfo(populatedL2Quote.SourceTickerInfo!);

            AssertAreEquivalentMeetsExpectedExactComparisonType
                (true, populatedL2Quote, fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType
                (false, populatedL2Quote, fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            Assert.AreEqual(populatedL2Quote, populatedL2Quote);
            Assert.AreEqual(populatedL2Quote, ((ICloneable)populatedL2Quote).Clone());
            Assert.AreEqual(populatedL2Quote, ((ICloneable<IPublishableTickInstant>)populatedL2Quote).Clone());
            Assert.AreEqual(populatedL2Quote, ((ICloneable<IPublishableLevel1Quote>)populatedL2Quote).Clone());
            Assert.AreEqual(populatedL2Quote, ((ICloneable<IPublishableLevel2Quote>)populatedL2Quote).Clone());
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_GetHashCode_ReturnNumberNoException()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var hashCode = populatedL2Quote.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }
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
    public void OrdersCountFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = ordersCountFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void OrdersAnonFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = ordersAnonFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void OrdersCounterPartyFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = ordersCounterPartyFullyPopulatedLevel2Quote;
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
        var q      = fullSupportFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQPublishableLevel2Quote original, PQPublishableLevel2Quote changingLevel2Quote)
    {
        PQLevel1QuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingLevel2Quote);

        if (original.GetType() == typeof(PQPublishableLevel2Quote))
            Assert.AreEqual(!exactComparison,
                            changingLevel2Quote.AreEquivalent(new PublishableLevel2PriceQuote(original), exactComparison));

        PQOrderBookSideTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original.BidBook, changingLevel2Quote.BidBook, original.OrderBook, changingLevel2Quote.OrderBook,
             original, changingLevel2Quote);

        PQOrderBookSideTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original.AskBook, changingLevel2Quote.AskBook, original.OrderBook, changingLevel2Quote.OrderBook,
             original, changingLevel2Quote);
    }

    public static void AssertContainsAllLevel2Fields
    (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates,
        PQPublishableLevel2Quote l2Q, PQQuoteBooleanValues expectedQuoteBooleanFlags = PQQuoteBooleanValuesExtensions.LivePricingFieldsSetNoReplayOrSnapshots)
    {
        PQLevel1QuoteTests.AssertContainsAllLevel1Fields(precisionSettings, checkFieldUpdates, l2Q, expectedQuoteBooleanFlags);

        var priceScale  = precisionSettings.PriceScalingPrecision;
        var volumeScale = precisionSettings.VolumeScalingPrecision;

        var maxLayers = Math.Min(l2Q.BidBook.Count, l2Q.AskBook.Count);

        for (var i = 0; i < maxLayers; i++)
        {
            var bidL2Pvl = l2Q.BidBook[i]!;
            var askL2Pvl = l2Q.AskBook[i]!;

            var bidDepthId = (PQDepthKey)i;
            var askDepthId = (PQDepthKey)i | PQDepthKey.AskSide;

            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, bidDepthId, PQScaling.Scale(bidL2Pvl.Price, priceScale), priceScale)
                          , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerPrice, bidDepthId, priceScale)
                          , $"For BidLayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual
                (new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, askDepthId, PQScaling.Scale(askL2Pvl.Price, priceScale), priceScale)
               , PQTickInstantTests.ExtractFieldUpdateWithId
                     (checkFieldUpdates, PQFeedFields.QuoteLayerPrice, askDepthId, priceScale)
               , $"For AskLayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual
                (new PQFieldUpdate
                     (PQFeedFields.QuoteLayerVolume, bidDepthId, PQScaling.Scale(bidL2Pvl.Volume, volumeScale), volumeScale)
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerVolume, bidDepthId, volumeScale)
               , $"For BidLayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual
                (new PQFieldUpdate
                     (PQFeedFields.QuoteLayerVolume, askDepthId, PQScaling.Scale(askL2Pvl.Volume, volumeScale), volumeScale)
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerVolume, askDepthId, volumeScale)
               , $"For AskLayer {bidL2Pvl.GetType().Name} level {i}");

            if (bidL2Pvl is IPQSourcePriceVolumeLayer pqBidL2Pvl &&
                askL2Pvl is IPQSourcePriceVolumeLayer pqAskL2Pvl)
                AssertSourceContainsAllFields(checkFieldUpdates, i, pqBidL2Pvl, pqAskL2Pvl);

            if (bidL2Pvl is IPQSourceQuoteRefPriceVolumeLayer bidSrcQtRefPvl &&
                askL2Pvl is IPQSourceQuoteRefPriceVolumeLayer askSrcQtRefPvl)
                AssertSourceQuoteRefContainsAllFields(checkFieldUpdates, i, bidSrcQtRefPvl, askSrcQtRefPvl);

            if (bidL2Pvl is IPQValueDatePriceVolumeLayer bidValueDatePvl &&
                askL2Pvl is IPQValueDatePriceVolumeLayer askValueDatePvl)
                AssertValueDateLayerContainsAllFields(checkFieldUpdates, i, bidValueDatePvl, askValueDatePvl);

            if (bidL2Pvl is IPQOrdersPriceVolumeLayer bidTrdPvl &&
                askL2Pvl is IPQOrdersPriceVolumeLayer askTrdPvl)
            {
                var bidNameIdLookup = bidTrdPvl.NameIdLookup;
                AssertOrdersLayerInfoIsExpected(checkFieldUpdates, bidTrdPvl, false, i, bidNameIdLookup, volumeScale);
                var askNameIdLookup = askTrdPvl.NameIdLookup;
                AssertOrdersLayerInfoIsExpected(checkFieldUpdates, askTrdPvl, true, i, askNameIdLookup, volumeScale);
            }
        }
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
    (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQDepthKey depthId,
        PQOrdersSubFieldKeys subId, ushort auxiliaryPayload,
        uint value, PQFieldFlags flag = 0)
    {
        return ExtractFieldUpdateWithId(allUpdates, id, depthId, (byte)subId, auxiliaryPayload, value, flag);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
    (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQDepthKey depthId,
        PQPricingSubFieldKeys subId, ushort auxiliaryPayload,
        uint value, PQFieldFlags flag = 0)
    {
        return ExtractFieldUpdateWithId(allUpdates, id, depthId, (byte)subId, auxiliaryPayload, value, flag);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
    (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQDepthKey depthId,
        PQTradingSubFieldKeys subId, ushort auxiliaryPayload,
        uint value, PQFieldFlags flag = 0)
    {
        return ExtractFieldUpdateWithId(allUpdates, id, depthId, (byte)subId, auxiliaryPayload, value, flag);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
    (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQDepthKey depthId,
        byte subId, ushort auxiliaryPayload,
        uint value, PQFieldFlags flag = 0)
    {
        var useExtendedFlag  = subId > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None;
        var useAuxiliaryFlag = auxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None;
        var useDepthFlag     = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        var tryFlags         = flag | useDepthFlag | useExtendedFlag | useAuxiliaryFlag;
        var tryGetValue = allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.SubIdByte == subId
                                                       && fu.AuxiliaryPayload == auxiliaryPayload && fu.Payload == value && fu.Flag == tryFlags);
        var tryAgainValue = !Equals(tryGetValue, default(PQFieldUpdate))
            ? tryGetValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.SubIdByte == subId &&
                                              fu.Flag == (flag | useDepthFlag | useExtendedFlag));
        var tryTryAgainValue = !Equals(tryAgainValue, default(PQFieldUpdate))
            ? tryAgainValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.Flag == flag);
        return tryTryAgainValue;
    }

    private void AssertLayerTypeIsExpected(Type expectedType, params PQPublishableLevel2Quote[] quotesToCheck)
    {
        foreach (var level2Quote in quotesToCheck)
            for (var i = 0; i < level2Quote.SourceTickerInfo!.MaximumPublishedLayers; i++)
            {
                Assert.AreEqual(expectedType, level2Quote.BidBook[i]!.GetType());
                Assert.AreEqual(expectedType, level2Quote.AskBook[i]!.GetType());
                switch (level2Quote.BidBook[i])
                {
                    case PQFullSupportPriceVolumeLayer fullSupportPvl:
                        Assert.IsTrue
                            (ReferenceEquals
                                (((IPQSourcePriceVolumeLayer)level2Quote.BidBook[0]!).NameIdLookup, fullSupportPvl.NameIdLookup));
                        Assert.IsTrue
                            (ReferenceEquals
                                (((IPQOrdersPriceVolumeLayer)level2Quote.BidBook[0]!).NameIdLookup, fullSupportPvl.NameIdLookup));
                        break;
                    case PQSourcePriceVolumeLayer sourcePriceVolumeLayer:
                        Assert.IsTrue
                            (ReferenceEquals
                                (((IPQSourcePriceVolumeLayer)level2Quote.BidBook[0]!).NameIdLookup, sourcePriceVolumeLayer.NameIdLookup));
                        break;
                    case PQOrdersPriceVolumeLayer ordersPriceVolumeLayer:
                        Assert.IsTrue
                            (ReferenceEquals
                                (((IPQOrdersPriceVolumeLayer)level2Quote.BidBook[0]!).NameIdLookup, ordersPriceVolumeLayer.NameIdLookup));
                        break;
                }
            }
    }

    public static void AssertAreDefaultValues(IPQPriceVolumeLayer pvl)
    {
        Assert.AreEqual(0m, pvl.Price);
        Assert.AreEqual(0m, pvl.Volume);
        Assert.IsFalse(pvl.IsPriceUpdated);
        Assert.IsFalse(pvl.IsVolumeUpdated);
        if (pvl is IPQSourcePriceVolumeLayer sourcePvl)
        {
            Assert.AreEqual(null, sourcePvl.SourceName);
            Assert.IsFalse(sourcePvl.IsSourceNameUpdated);
        }

        if (pvl is IPQSourceQuoteRefPriceVolumeLayer sourceQtRefPvl)
        {
            Assert.AreEqual(0m, sourceQtRefPvl.SourceQuoteReference);
            Assert.IsFalse(sourceQtRefPvl.IsSourceQuoteReferenceUpdated);
        }

        if (pvl is IPQValueDatePriceVolumeLayer valueDatePvl)
        {
            Assert.AreEqual(DateTime.MinValue, valueDatePvl.ValueDate);
            Assert.IsFalse(valueDatePvl.IsValueDateUpdated);
        }

        if (pvl is IPQOrdersPriceVolumeLayer traderPvl) Assert.AreEqual(0, traderPvl.OrdersCount);
    }

    private static void AssertSourceContainsAllFields
    (IList<PQFieldUpdate> checkFieldUpdates, int i,
        IPQSourcePriceVolumeLayer pqBidL2Pvl,
        IPQSourcePriceVolumeLayer pqAskL2Pvl)
    {
        var bidDepthId = (PQDepthKey)i;
        var askDepthId = (PQDepthKey)i | PQDepthKey.AskSide;

        var bidSrcId =
            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerSourceId, bidDepthId);
        var askSrcId =
            PQTickInstantTests.ExtractFieldUpdateWithId
                (checkFieldUpdates, PQFeedFields.QuoteLayerSourceId, askDepthId);

        var bidNameIdLookup = pqBidL2Pvl.NameIdLookup;
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerSourceId, bidDepthId, (uint)bidNameIdLookup[pqBidL2Pvl.SourceName!]), bidSrcId);
        var askNameIdLookup = pqAskL2Pvl.NameIdLookup;
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerSourceId, askDepthId, (uint)askNameIdLookup[pqAskL2Pvl.SourceName!]), askSrcId);

        var bidSrcExecutable = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerBooleanFlags, bidDepthId);
        var askSrcExecutable = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerBooleanFlags, askDepthId);

        var bidExecutable = (uint)(pqBidL2Pvl.Executable ? LayerBooleanFlags.IsExecutableFlag : LayerBooleanFlags.None);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerBooleanFlags, bidDepthId, bidExecutable), bidSrcExecutable);
        var askExecutable = (uint)(pqAskL2Pvl.Executable ? LayerBooleanFlags.IsExecutableFlag : LayerBooleanFlags.None);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerBooleanFlags, askDepthId, askExecutable), askSrcExecutable);
    }

    private static void AssertSourceQuoteRefContainsAllFields
    (IList<PQFieldUpdate> checkFieldUpdates, int i,
        IPQSourceQuoteRefPriceVolumeLayer bidSrcQtRefPvl, IPQSourceQuoteRefPriceVolumeLayer askSrcQtRefPvl)
    {
        var bidDepthId = (PQDepthKey)i;
        var askDepthId = (PQDepthKey)i | PQDepthKey.AskSide;

        var bidSrcQtRef = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerSourceQuoteRef, bidDepthId);
        var askSrcQtRef = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerSourceQuoteRef, askDepthId);

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerSourceQuoteRef, bidDepthId, bidSrcQtRefPvl.SourceQuoteReference), bidSrcQtRef);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerSourceQuoteRef, askDepthId, askSrcQtRefPvl.SourceQuoteReference), askSrcQtRef);
    }

    private static void AssertValueDateLayerContainsAllFields
    (IList<PQFieldUpdate> checkFieldUpdates, int i,
        IPQValueDatePriceVolumeLayer bidValueDatePvl, IPQValueDatePriceVolumeLayer askValueDatePvl)
    {
        var bidDepthId = (PQDepthKey)i;
        var askDepthId = (PQDepthKey)i | PQDepthKey.AskSide;

        var bidValueDate = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerValueDate, bidDepthId);
        var askValueDate = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerValueDate, askDepthId);

        var dateAsHoursFromEpoch = bidValueDatePvl.ValueDate.Get2MinIntervalsFromUnixEpoch();
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, bidDepthId, dateAsHoursFromEpoch), bidValueDate);
        dateAsHoursFromEpoch = askValueDatePvl.ValueDate.Get2MinIntervalsFromUnixEpoch();
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, askDepthId, dateAsHoursFromEpoch), askValueDate);
    }

    private static void AssertOrdersLayerInfoIsExpected
    (IList<PQFieldUpdate> checkFieldUpdates,
        IPQOrdersPriceVolumeLayer ordersPvl, bool isAskSide, int depthIndex, IPQNameIdLookupGenerator nameIdLookup, PQFieldFlags volumeScale)
    {
        var depthId = (PQDepthKey)depthIndex | (isAskSide ? PQDepthKey.AskSide : PQDepthKey.None);
        for (var j = 0; j < ordersPvl.OrdersCount; j++)
        {
            var anonOrderLayerInfo = ordersPvl[j]!;
            var orderIndex         = (ushort)j;

            var orderId = (uint)anonOrderLayerInfo.OrderId;
            var orderIdFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderId, orderIndex
                                                   , orderId);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderId, orderIndex, orderId), orderIdFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderFlags = (uint)anonOrderLayerInfo.OrderLayerFlags;
            var orderFlagsFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderLayerFlags,
                                                        orderIndex, orderFlags);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderLayerFlags, orderIndex, orderFlags), orderFlagsFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderCreatedDate = anonOrderLayerInfo.CreatedTime.Get2MinIntervalsFromUnixEpoch();
            var orderCreatedDateFu
                = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderCreatedDate, orderIndex
                                         , orderCreatedDate);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderCreatedDate, orderIndex, orderCreatedDate)
                          , orderCreatedDateFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderCreatedSub2MinExtended
                = anonOrderLayerInfo.CreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var orderCreatedSub2Min);
            var orderCreatedSub2MinFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                               , PQOrdersSubFieldKeys.OrderCreatedSub2MinTime, orderIndex, orderCreatedSub2Min
                                                               , orderCreatedSub2MinExtended);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderCreatedSub2MinTime, orderIndex, orderCreatedSub2Min, orderCreatedSub2MinExtended)
                          , orderCreatedSub2MinFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderUpdatedDate = anonOrderLayerInfo.UpdateTime.Get2MinIntervalsFromUnixEpoch();
            var orderUpdatedDateFu
                = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderUpdatedDate, orderIndex
                                         , orderUpdatedDate);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderUpdatedDate, orderIndex, orderUpdatedDate)
                          , orderUpdatedDateFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderUpdatedSub2MinExtended
                = anonOrderLayerInfo.UpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var orderUpdatedSub2Min);
            var orderUpdatedSub2MinFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                               , PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime, orderIndex, orderUpdatedSub2Min
                                                               , orderUpdatedSub2MinExtended);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime, orderIndex, orderUpdatedSub2Min, orderUpdatedSub2MinExtended)
                          , orderUpdatedSub2MinFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderVolume = PQScaling.Scale(((IPublishedOrder)anonOrderLayerInfo).OrderDisplayVolume, volumeScale);
            var orderVolumeFu
                = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderDisplayVolume, orderIndex, orderVolume
                                         , volumeScale);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderDisplayVolume, orderIndex, orderVolume, volumeScale)
                          , orderVolumeFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderRemainingVolume = PQScaling.Scale(anonOrderLayerInfo.OrderRemainingVolume, volumeScale);
            var orderRemainingVolumeFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                                , PQOrdersSubFieldKeys.OrderRemainingVolume, orderIndex, orderRemainingVolume, volumeScale);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderRemainingVolume, orderIndex, orderRemainingVolume, volumeScale)
                          , orderRemainingVolumeFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (anonOrderLayerInfo is IPQCounterPartyOrderLayerInfo cpOrderLayerInfo)
            {
                var orderCpNameId = (uint)nameIdLookup[cpOrderLayerInfo.ExternalCounterPartyName];
                var orderCpIdFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                         , PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId
                                                         , orderIndex, orderCpNameId);
                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId, orderIndex, orderCpNameId)
                              , orderCpIdFu,
                                $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

                var orderTraderId = (uint)nameIdLookup[((IExternalCounterPartyInfoOrder)cpOrderLayerInfo).ExternalTraderName];
                var orderTraderIdFu
                    = ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderExternalTraderNameId, orderIndex
                                             , orderTraderId);
                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderExternalTraderNameId, orderIndex, orderTraderId)
                              , orderTraderIdFu,
                                $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");
                return;
            }
        }
    }
}
