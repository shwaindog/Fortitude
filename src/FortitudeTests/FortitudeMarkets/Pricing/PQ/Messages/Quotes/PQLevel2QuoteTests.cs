// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQLevel2QuoteTests
{
    private IList<PQLevel2Quote> allEmptyQuotes                      = null!;
    private IList<PQLevel2Quote> allFullyPopulatedQuotes             = null!;
    private PQLevel2Quote        everyLayerEmptyLevel2Quote          = null!;
    private PQLevel2Quote        everyLayerFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo everyLayerSourceTickerInfo                  = null!;
    private PQLevel2Quote     ordersAnonEmptyLevel2Quote                  = null!;
    private PQLevel2Quote     ordersAnonFullyPopulatedLevel2Quote         = null!;
    private ISourceTickerInfo ordersAnonSourceTickerInfo                  = null!;
    private PQLevel2Quote     ordersCountEmptyLevel2Quote                 = null!;
    private PQLevel2Quote     ordersCounterPartyEmptyLevel2Quote          = null!;
    private PQLevel2Quote     ordersCounterPartyFullyPopulatedLevel2Quote = null!;
    private ISourceTickerInfo ordersCounterPartySourceTickerInfo          = null!;
    private PQLevel2Quote     ordersCountFullyPopulatedLevel2Quote        = null!;
    private ISourceTickerInfo ordersCountSourceTickerInfo                 = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private PQLevel2Quote simpleEmptyLevel2Quote          = null!;
    private PQLevel2Quote simpleFullyPopulatedLevel2Quote = null!;

    private ISourceTickerInfo simpleSourceTickerInfo                  = null!;
    private PQLevel2Quote     sourceNameEmptyLevel2Quote              = null!;
    private PQLevel2Quote     sourceNameFullyPopulatedLevel2Quote     = null!;
    private ISourceTickerInfo sourceNameSourceTickerInfo              = null!;
    private PQLevel2Quote     sourceQuoteRefEmptyLevel2Quote          = null!;
    private PQLevel2Quote     sourceQuoteRefFullyPopulatedLevel2Quote = null!;
    private ISourceTickerInfo sourceRefSourceTickerInfo               = null!;

    private DateTime          testDateTime;
    private PQLevel2Quote     valueDateEmptyLevel2Quote          = null!;
    private PQLevel2Quote     valueDateFullyPopulatedLevel2Quote = null!;
    private ISourceTickerInfo valueDateSourceTickerInfo          = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        simpleSourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 30_000m, 50000000m, 30_000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        sourceNameSourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 30_000m, 50000000m, 30_000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceName
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        sourceRefSourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 30_000m, 50000000m, 30_000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceQuoteReference
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        ordersCountSourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 1, 50000000m, 30_000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrdersCount | LayerFlags.InternalVolume
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        ordersAnonSourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 1m, 50000000m, 30_000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrdersCount | LayerFlags.InternalVolume | LayerFlags.OrderId |
                             LayerFlags.OrderCreated |
                             LayerFlags.OrderUpdated | LayerFlags.OrderSize | LayerFlags.OrderRemainingSize
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        ordersCounterPartySourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 1m, 50000000m, 30_000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrdersCount | LayerFlags.InternalVolume | LayerFlags.OrderId |
                             LayerFlags.OrderCreated | LayerFlags.OrderUpdated | LayerFlags.OrderSize | LayerFlags.OrderRemainingSize |
                             LayerFlags.OrderCounterPartyName | LayerFlags.OrderTraderName
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        valueDateSourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 30_000m, 50000000m, 30_000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.ValueDate
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        everyLayerSourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 1m, 50000000m, 30_000m, 1
               , layerFlags: LayerFlags.Volume.AllFlags()
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        simpleEmptyLevel2Quote          = new PQLevel2Quote(new PQSourceTickerInfo(simpleSourceTickerInfo)) { HasUpdates = false };
        simpleFullyPopulatedLevel2Quote = new PQLevel2Quote(simpleSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleFullyPopulatedLevel2Quote, 1);
        sourceNameEmptyLevel2Quote          = new PQLevel2Quote(new PQSourceTickerInfo(sourceNameSourceTickerInfo)) { HasUpdates = false };
        sourceNameFullyPopulatedLevel2Quote = new PQLevel2Quote(sourceNameSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceNameFullyPopulatedLevel2Quote, 2);
        sourceQuoteRefEmptyLevel2Quote          = new PQLevel2Quote(new PQSourceTickerInfo(sourceRefSourceTickerInfo)) { HasUpdates = false };
        sourceQuoteRefFullyPopulatedLevel2Quote = new PQLevel2Quote(sourceRefSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceQuoteRefFullyPopulatedLevel2Quote, 3);
        ordersCountEmptyLevel2Quote          = new PQLevel2Quote(new PQSourceTickerInfo(ordersCountSourceTickerInfo)) { HasUpdates = false };
        ordersCountFullyPopulatedLevel2Quote = new PQLevel2Quote(ordersCountSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(ordersCountFullyPopulatedLevel2Quote, 4);
        ordersAnonEmptyLevel2Quote          = new PQLevel2Quote(new PQSourceTickerInfo(ordersAnonSourceTickerInfo)) { HasUpdates = false };
        ordersAnonFullyPopulatedLevel2Quote = new PQLevel2Quote(ordersAnonSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(ordersAnonFullyPopulatedLevel2Quote, 4);
        ordersCounterPartyEmptyLevel2Quote = new PQLevel2Quote(new PQSourceTickerInfo(ordersCounterPartySourceTickerInfo)) { HasUpdates = false };
        ordersCounterPartyFullyPopulatedLevel2Quote = new PQLevel2Quote(ordersCounterPartySourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(ordersCounterPartyFullyPopulatedLevel2Quote, 4);
        valueDateEmptyLevel2Quote          = new PQLevel2Quote(new PQSourceTickerInfo(valueDateSourceTickerInfo)) { HasUpdates = false };
        valueDateFullyPopulatedLevel2Quote = new PQLevel2Quote(valueDateSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateFullyPopulatedLevel2Quote, 5);
        everyLayerEmptyLevel2Quote          = new PQLevel2Quote(new PQSourceTickerInfo(everyLayerSourceTickerInfo)) { HasUpdates = false };
        everyLayerFullyPopulatedLevel2Quote = new PQLevel2Quote(everyLayerSourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerFullyPopulatedLevel2Quote, 5);

        allFullyPopulatedQuotes = new List<PQLevel2Quote>
        {
            simpleFullyPopulatedLevel2Quote, sourceNameFullyPopulatedLevel2Quote
          , sourceQuoteRefFullyPopulatedLevel2Quote, ordersCountFullyPopulatedLevel2Quote
          , ordersAnonFullyPopulatedLevel2Quote, ordersCounterPartyFullyPopulatedLevel2Quote
          , valueDateFullyPopulatedLevel2Quote, everyLayerFullyPopulatedLevel2Quote
        };
        // {
        //     ordersCountDetailsFullyPopulatedLevel2Quote
        // };
        allEmptyQuotes = new List<PQLevel2Quote>
        {
            simpleEmptyLevel2Quote, sourceNameEmptyLevel2Quote, sourceQuoteRefEmptyLevel2Quote
          , ordersCountEmptyLevel2Quote, ordersAnonEmptyLevel2Quote, ordersCounterPartyEmptyLevel2Quote, valueDateEmptyLevel2Quote
          , everyLayerEmptyLevel2Quote
        };

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
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
        var cappedLevel2Quote = new PQLevel2Quote(tooLargeMaxDepth);
        Assert.AreEqual(PQFieldKeys.TwoByteFieldIdMaxBookDepth, cappedLevel2Quote.BidBook.Capacity);
        Assert.AreEqual(PQFieldKeys.TwoByteFieldIdMaxBookDepth, cappedLevel2Quote.AskBook.Capacity);
    }

    [TestMethod]
    public void TooSmalMaxBookDepth_New_IncreaseBookDepthAtLeast1Level()
    {
        var tooLargeMaxDepth =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 0, 0.00001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        var cappedLevel2Quote = new PQLevel2Quote(tooLargeMaxDepth);
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
            (typeof(PQSourceQuoteRefOrdersValueDatePriceVolumeLayer), everyLayerEmptyLevel2Quote, everyLayerFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_IsBookChanged_SetsAndReadsBookChangedStatus()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            Assert.IsTrue(populatedL2Quote.IsBidBookChanged);
            Assert.IsTrue(populatedL2Quote.IsAskBookChanged);
            Assert.IsTrue(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsTrue(populatedL2Quote.AskBook.HasUpdates);

            populatedL2Quote.IsBidBookChanged = false;
            populatedL2Quote.IsAskBookChanged = false;

            Assert.IsFalse(populatedL2Quote.IsBidBookChanged);
            Assert.IsFalse(populatedL2Quote.IsAskBookChanged);
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

        Assert.AreSame(orderBidBookViaClass, ((ILevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, ((IPQLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);

        Assert.AreSame(orderAskBookViaClass, ((ILevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, ((IPQLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
    }

    [TestMethod]
    public void SimpleLevel2Quote_MutableSetOrderBook_OnlyAllowsPQOrderBookToBeSet()
    {
        var orderBidBookViaClass = simpleFullyPopulatedLevel2Quote.BidBook;
        var orderAskBookViaClass = simpleFullyPopulatedLevel2Quote.AskBook;

        var caughtException = false;
        var nonPQOrderBook  = new OrderBook(BookSide.AskBook, new List<IPriceVolumeLayer>());

        try
        {
            ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook = nonPQOrderBook;
        }
        catch (InvalidCastException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
        Assert.AreSame(orderBidBookViaClass, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, simpleFullyPopulatedLevel2Quote.BidBook);

        caughtException = false;

        try
        {
            ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook = nonPQOrderBook;
        }
        catch (InvalidCastException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
        Assert.AreSame(orderAskBookViaClass, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, simpleFullyPopulatedLevel2Quote.AskBook);

        var newPqOrderBook = new PQOrderBook(BookSide.BidBook, new List<IPriceVolumeLayer>());
        ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook = newPqOrderBook;
        ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook = newPqOrderBook;
        Assert.AreSame(newPqOrderBook, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(newPqOrderBook, simpleFullyPopulatedLevel2Quote.BidBook);
        Assert.AreSame(newPqOrderBook, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
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
    public void AllLevel2QuoteTypes_LayerPriceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var priceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook))
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, priceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, priceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i)
                                         .FirstOrDefault();

            Assert.IsFalse(priceVolumeLayer!.IsPriceUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.AreEqual(0m, priceVolumeLayer.Price);
            Assert.AreEqual(0, priceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedPrice = 50.1221m;
            var pqSrcTkrInfo  = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
            var priceScale    = pqSrcTkrInfo.PriceScalingPrecision;
            priceVolumeLayer.Price = expectedPrice;
            Assert.IsTrue(priceVolumeLayer.IsPriceUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.AreEqual(expectedPrice, priceVolumeLayer.Price);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = priceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pqSrcTkrInfo).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate
                (PQQuoteFields.Price, PQScaling.Scale(expectedPrice, priceScale), priceScale);
            var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
            var expectedSideAdjustedLayerField =
                new PQFieldUpdate
                    (PQQuoteFields.Price, depthId, expectedLayerField.Payload,
                     expectedLayerField.Flag);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            priceVolumeLayer.IsPriceUpdated = false;
            Assert.IsFalse(priceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated    = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            priceVolumeLayer.IsPriceUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.Id == PQQuoteFields.Price && update.DepthId == depthId
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);

            priceVolumeLayer.Price = 0m;

            priceVolumeLayer.IsPriceUpdated = false;

            var newEmpty = new PQLevel2Quote(simpleSourceTickerInfo);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop];
            Assert.AreEqual(expectedPrice, foundLayer!.Price);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsPriceUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var priceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook))
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, priceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, priceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(priceVolumeLayer!.IsVolumeUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.AreEqual(0m, priceVolumeLayer.Volume);
            Assert.AreEqual(0, priceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedVolume = 40_000_000m;
            priceVolumeLayer.Volume = expectedVolume;
            var pqSrcTkrInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
            var volumeScale  = pqSrcTkrInfo.VolumeScalingPrecision;
            Assert.IsTrue(priceVolumeLayer.IsVolumeUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.AreEqual(expectedVolume, priceVolumeLayer.Volume);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = priceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pqSrcTkrInfo).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate
                (PQQuoteFields.Volume, PQScaling.Scale(expectedVolume, volumeScale), volumeScale);
            var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
            var expectedSideAdjustedLayerField =
                new PQFieldUpdate
                    (PQQuoteFields.Volume, depthId, expectedLayerField.Payload, expectedLayerField.Flag);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            priceVolumeLayer.IsVolumeUpdated = false;
            Assert.IsFalse(priceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated    = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            priceVolumeLayer.IsVolumeUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.Id == PQQuoteFields.Volume && update.DepthId == depthId
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            priceVolumeLayer.Volume          = 0m;
            priceVolumeLayer.IsVolumeUpdated = false;

            var newEmpty = new PQLevel2Quote(simpleSourceTickerInfo);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop];
            Assert.AreEqual(expectedVolume, foundLayer!.Volume);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsVolumeUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerSourceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var nameCounter = 0;
            foreach (var sourcePriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                             .OfType<IPQSourcePriceVolumeLayer>())
            {
                nameCounter++;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, sourcePriceVolumeLayer));
                var indexFromTop =
                    (isBid
                        ? emptyQuote.BidBook
                        : emptyQuote.AskBook).AllLayers
                                             .Select((pvl, i) => new { i, pvl })
                                             .Where(indexPvl => ReferenceEquals(indexPvl.pvl, sourcePriceVolumeLayer))
                                             .Select(indexPvl => indexPvl.i).FirstOrDefault();

                Assert.IsFalse(sourcePriceVolumeLayer.IsSourceNameUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(null, sourcePriceVolumeLayer.SourceName);
                Assert.AreEqual(0, sourcePriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());


                var expectedSourceName = "NewChangedSourceName" + nameCounter;
                sourcePriceVolumeLayer.SourceName = expectedSourceName;
                Assert.IsTrue(sourcePriceVolumeLayer.IsSourceNameUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedSourceName, sourcePriceVolumeLayer.SourceName);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates =
                    sourcePriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var expectedLayerField = new PQFieldUpdate(PQQuoteFields.SourceId,
                                                           (uint)sourcePriceVolumeLayer.NameIdLookup[sourcePriceVolumeLayer.SourceName]);
                var depthId                        = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField = new PQFieldUpdate(PQQuoteFields.SourceId, depthId, expectedLayerField.Payload);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);
                var stringUpdates          = sourcePriceVolumeLayer.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
                var stringUpdatelayerFlags = (ushort)CrudCommand.Upsert;
                var selectedStringUpdate =
                    stringUpdates.FirstOrDefault
                        (su => su.Field.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand
                            && su.Field.ExtendedPayload == stringUpdatelayerFlags && su.StringUpdate.DictionaryId == layerUpdates[0].Payload);
                Assert.IsFalse(Equals(selectedStringUpdate, new PQFieldStringUpdate()));
                var expectedStringUpdates = new PQFieldStringUpdate
                {
                    Field = new PQFieldUpdate(PQQuoteFields.LayerNameDictionaryUpsertCommand, 0u, stringUpdatelayerFlags)
                  , StringUpdate = new PQStringUpdate
                    {
                        Command      = CrudCommand.Upsert
                      , DictionaryId = sourcePriceVolumeLayer.NameIdLookup[sourcePriceVolumeLayer.SourceName]
                      , Value        = expectedSourceName
                    }
                };
                Assert.AreEqual(expectedStringUpdates, selectedStringUpdate);

                sourcePriceVolumeLayer.IsSourceNameUpdated     = false;
                sourcePriceVolumeLayer.NameIdLookup.HasUpdates = false;
                Assert.IsFalse(sourcePriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                sourcePriceVolumeLayer.IsSourceNameUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.SourceId && update.DepthId == depthId
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                sourcePriceVolumeLayer.SourceName          = null;
                sourcePriceVolumeLayer.IsSourceNameUpdated = false;

                var diffNameIdLookupSrcTkrInfo =
                    new PQSourceTickerInfo(emptyQuote.SourceTickerInfo!)
                    {
                        NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand)
                    };
                var newEmpty = new PQLevel2Quote(diffNameIdLookupSrcTkrInfo);
                newEmpty.UpdateField(quoteUpdates[0]);
                var applySided = expectedStringUpdates.WithDepth(depthId);
                newEmpty.UpdateFieldString(applySided);
                var foundLayer
                    = (IPQSourcePriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                Assert.AreEqual(expectedSourceName, foundLayer!.SourceName);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsSourceNameUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var srcPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                      .OfType<IPQSourcePriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, srcPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, srcPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(srcPriceVolumeLayer.IsExecutableUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsFalse(srcPriceVolumeLayer.Executable);
            Assert.AreEqual(0, srcPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            srcPriceVolumeLayer.Executable = true;
            Assert.IsTrue(srcPriceVolumeLayer.IsExecutableUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.IsTrue(srcPriceVolumeLayer.Executable);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = srcPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerBooleanFlags, (uint)LayerBooleanFlags.IsExecutableFlag);
            var depthId            = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
            var expectedSideAdjustedLayerField =
                new PQFieldUpdate(PQQuoteFields.LayerBooleanFlags, depthId, (uint)LayerBooleanFlags.IsExecutableFlag);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            srcPriceVolumeLayer.IsExecutableUpdated = false;
            Assert.IsFalse(srcPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated    = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            srcPriceVolumeLayer.IsExecutableUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.Id == PQQuoteFields.LayerBooleanFlags && update.DepthId == depthId
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            srcPriceVolumeLayer.Executable          = false;
            srcPriceVolumeLayer.IsExecutableUpdated = false;

            var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (IPQSourcePriceVolumeLayer)
                (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.IsTrue(foundLayer.Executable);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsExecutableUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerSourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var srcQtRefPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                           .OfType<IPQSourceQuoteRefPriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, srcQtRefPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, srcQtRefPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.AreEqual(0u, srcQtRefPriceVolumeLayer.SourceQuoteReference);
            Assert.AreEqual(0, srcQtRefPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedSourceQuoteRef = 246813u;
            srcQtRefPriceVolumeLayer.SourceQuoteReference = expectedSourceQuoteRef;
            Assert.IsTrue(srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.AreEqual(expectedSourceQuoteRef, srcQtRefPriceVolumeLayer.SourceQuoteReference);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = srcQtRefPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerSourceQuoteRef, expectedSourceQuoteRef);
            var depthId            = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
            var expectedSideAdjustedLayerField =
                new PQFieldUpdate(PQQuoteFields.LayerSourceQuoteRef, depthId, expectedLayerField.Payload);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated = false;
            Assert.IsFalse(srcQtRefPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated    = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.Id == PQQuoteFields.LayerSourceQuoteRef && update.DepthId == depthId
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            srcQtRefPriceVolumeLayer.SourceQuoteReference          = 0u;
            srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated = false;

            var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (IPQSourceQuoteRefPriceVolumeLayer)
                (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.AreEqual(expectedSourceQuoteRef, foundLayer.SourceQuoteReference);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsSourceQuoteReferenceUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerValueDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var srcQtRefPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                           .OfType<IPQValueDatePriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers
                                  .Any(pvl => ReferenceEquals(pvl, srcQtRefPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, srcQtRefPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(srcQtRefPriceVolumeLayer.IsValueDateUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, srcQtRefPriceVolumeLayer.ValueDate);
            Assert.AreEqual(0, srcQtRefPriceVolumeLayer
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedValueDate = new DateTime(2017, 12, 03, 19, 0, 0); //only to the nearest hour
            srcQtRefPriceVolumeLayer.ValueDate = expectedValueDate;
            Assert.IsTrue(srcQtRefPriceVolumeLayer.IsValueDateUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.AreEqual(expectedValueDate, srcQtRefPriceVolumeLayer.ValueDate);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = srcQtRefPriceVolumeLayer
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var hoursSinceUnixEpoch = expectedValueDate.GetHoursFromUnixEpoch();
            var expectedLayerField  = new PQFieldUpdate(PQQuoteFields.LayerValueDate, hoursSinceUnixEpoch);
            var depthId             = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
            var expectedSideAdjustedLayerField =
                new PQFieldUpdate(PQQuoteFields.LayerValueDate, depthId, expectedLayerField.Payload);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            srcQtRefPriceVolumeLayer.IsValueDateUpdated = false;
            Assert.IsFalse(srcQtRefPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated    = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            srcQtRefPriceVolumeLayer.IsValueDateUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.Id == PQQuoteFields.LayerValueDate && update.DepthId == depthId
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            srcQtRefPriceVolumeLayer.ValueDate          = DateTimeConstants.UnixEpoch;
            srcQtRefPriceVolumeLayer.IsValueDateUpdated = false;

            var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (IPQValueDatePriceVolumeLayer)
                (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.AreEqual(expectedValueDate, foundLayer.ValueDate);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsValueDateUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersCountPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                              .OfType<IPQOrdersCountPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, ordersCountPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersCountPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            testDateTime = testDateTime.AddHours(1).AddMinutes(1);

            Assert.IsFalse(ordersCountPriceVolumeLayer.IsOrdersCountUpdated);
            Assert.IsFalse(ordersCountPriceVolumeLayer.HasUpdates);
            Assert.AreEqual(0, ordersCountPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            ordersCountPriceVolumeLayer.OrdersCount = byte.MaxValue;
            Assert.IsTrue(ordersCountPriceVolumeLayer.HasUpdates);
            Assert.AreEqual(byte.MaxValue, ordersCountPriceVolumeLayer.OrdersCount);
            Assert.IsTrue(emptyQuote.HasUpdates);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = ordersCountPriceVolumeLayer
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.OrdersCount, byte.MaxValue);
            var depthId            = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
            var expectedSideAdjustedLayerField =
                new PQFieldUpdate(PQQuoteFields.OrdersCount, depthId, byte.MaxValue);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            ordersCountPriceVolumeLayer.HasUpdates = false;
            Assert.IsFalse(ordersCountPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated    = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            ordersCountPriceVolumeLayer.HasUpdates = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.Id == PQQuoteFields.OrdersCount && update.DepthId == depthId
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            ordersCountPriceVolumeLayer.OrdersCount = 0;
            ordersCountPriceVolumeLayer.HasUpdates  = false;

            var diffNameIdLookupSrcTkrInfo =
                new PQSourceTickerInfo(emptyQuote.SourceTickerInfo!)
                {
                    NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand)
                };
            var newEmpty = new PQLevel2Quote(diffNameIdLookupSrcTkrInfo);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer =
                (IPQOrdersCountPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.AreEqual(byte.MaxValue, foundLayer.OrdersCount);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                         .OfType<IPQOrdersPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, ordersPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = ordersPriceVolumeLayer[i]!;

                Assert.IsFalse(anonOrderLayer.IsOrderIdUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0, anonOrderLayer.OrderId);
                Assert.AreEqual(0,
                                ordersPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedOrderId = 254682;
                anonOrderLayer.OrderId = expectedOrderId;
                Assert.IsTrue(anonOrderLayer.IsOrderIdUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedOrderId, anonOrderLayer.OrderId);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = ordersPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var orderIndex = (ushort)i;
                var expectedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderId, (uint)expectedOrderId, 0, orderIndex);
                var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderId, depthId,
                                      expectedLayerField.Payload, 0, orderIndex, expectedLayerField.Flag);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                anonOrderLayer.IsOrderIdUpdated = false;
                Assert.IsFalse(ordersPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsOrderIdUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.OrderId && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                anonOrderLayer.OrderId          = 0;
                anonOrderLayer.IsOrderIdUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundAnonOrderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedOrderId, foundAnonOrderInfo.OrderId);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.IsOrderIdUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                         .OfType<IPQOrdersPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, ordersPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = ordersPriceVolumeLayer[i]!;

                Assert.IsFalse(anonOrderLayer.IsOrderFlagsUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(LayerOrderFlags.None, anonOrderLayer.OrderFlags);
                Assert.AreEqual(0,
                                ordersPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedOrderFlags = LayerOrderFlags.IsInternallyCreatedOrder | LayerOrderFlags.IsSyntheticTrackingOrder;
                anonOrderLayer.OrderFlags = expectedOrderFlags;
                Assert.IsTrue(anonOrderLayer.IsOrderFlagsUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedOrderFlags, anonOrderLayer.OrderFlags);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = ordersPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var orderIndex = (ushort)i;
                var expectedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderFlags, (uint)expectedOrderFlags, 0, orderIndex);
                var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderFlags, depthId,
                                      expectedLayerField.Payload, 0, orderIndex, expectedLayerField.Flag);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                anonOrderLayer.IsOrderFlagsUpdated = false;
                Assert.IsFalse(ordersPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsOrderFlagsUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.OrderFlags && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                anonOrderLayer.OrderFlags          = LayerOrderFlags.None;
                anonOrderLayer.IsOrderFlagsUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundAnonOrderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderFlags);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.IsOrderFlagsUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderCreatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                         .OfType<IPQOrdersPriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers
                                  .Any(pvl => ReferenceEquals(pvl, ordersPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook)
                .AllLayers
                .Select((pvl, i) => new { i, pvl })
                .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersPriceVolumeLayer))
                .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = ordersPriceVolumeLayer[i]!;

                Assert.IsFalse(anonOrderLayer.IsCreatedTimeDateUpdated);
                Assert.IsFalse(anonOrderLayer.IsCreatedTimeSubHourUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(DateTime.MinValue, anonOrderLayer.CreatedTime);
                Assert.AreEqual(0, anonOrderLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedCreatedTime = new DateTime(2017, 12, 03, 19, 27, 53);
                anonOrderLayer.CreatedTime = expectedCreatedTime;
                Assert.IsTrue(anonOrderLayer.IsCreatedTimeDateUpdated);
                Assert.IsTrue(anonOrderLayer.IsCreatedTimeSubHourUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedCreatedTime, anonOrderLayer.CreatedTime);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(4, quoteUpdates.Count);
                var layerUpdates = ordersPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(2, layerUpdates.Count);
                var orderIndex                = (ushort)i;
                var hoursSinceUnixEpoch       = expectedCreatedTime.GetHoursFromUnixEpoch();
                var expectedDateLayerField    = new PQFieldUpdate(PQQuoteFields.OrderCreatedDate, hoursSinceUnixEpoch, 0, orderIndex);
                var extended                  = expectedCreatedTime.GetSubHourComponent().BreakLongToUShortAndUint(out var subHourBottom);
                var expectedSubHourLayerField = new PQFieldUpdate(PQQuoteFields.OrderCreatedTimeSubHour, subHourBottom, extended, orderIndex);
                var depthId                   = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideDateLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderCreatedDate, depthId, expectedDateLayerField.Payload, 0, orderIndex);
                var expectedSideSubHourLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderCreatedTimeSubHour, depthId, expectedSubHourLayerField.Payload,
                                      expectedSubHourLayerField.ExtendedPayload, orderIndex);
                Assert.AreEqual(expectedDateLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSubHourLayerField, layerUpdates[1]);
                Assert.AreEqual(expectedSideDateLayerField, quoteUpdates[2]);
                Assert.AreEqual(expectedSideSubHourLayerField, quoteUpdates[3]);

                anonOrderLayer.IsCreatedTimeDateUpdated    = false;
                anonOrderLayer.IsCreatedTimeSubHourUpdated = false;
                Assert.IsFalse(ordersPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsCreatedTimeDateUpdated    = true;
                anonOrderLayer.IsCreatedTimeSubHourUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where (update.Id == PQQuoteFields.OrderCreatedDate && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex)
                           || (update.Id == PQQuoteFields.OrderCreatedTimeSubHour && update.DepthId == depthId &&
                               update.AuxiliaryPayload == orderIndex)
                        select update).ToList();
                Assert.AreEqual(2, quoteUpdates.Count);
                Assert.AreEqual(expectedSideDateLayerField, quoteUpdates[0]);
                Assert.AreEqual(expectedSideSubHourLayerField, quoteUpdates[1]);
                anonOrderLayer.CreatedTime                 = DateTimeConstants.UnixEpoch;
                anonOrderLayer.IsCreatedTimeDateUpdated    = false;
                anonOrderLayer.IsCreatedTimeSubHourUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateField(quoteUpdates[1]);
                var foundLayer = (IPQOrdersPriceVolumeLayer)
                    (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundAnonOrderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedCreatedTime, foundAnonOrderInfo.CreatedTime);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeDateUpdated);
                Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeSubHourUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderUpdatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                         .OfType<IPQOrdersPriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers
                                  .Any(pvl => ReferenceEquals(pvl, ordersPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = ordersPriceVolumeLayer[i]!;

                Assert.IsFalse(anonOrderLayer.IsUpdatedTimeDateUpdated);
                Assert.IsFalse(anonOrderLayer.IsUpdatedTimeSubHourUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(DateTime.MinValue, anonOrderLayer.UpdatedTime);
                Assert.AreEqual(0, anonOrderLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedUpdatedTime = new DateTime(2017, 12, 03, 19, 41, 22); //only to the nearest hour
                anonOrderLayer.UpdatedTime = expectedUpdatedTime;
                Assert.IsTrue(anonOrderLayer.IsUpdatedTimeDateUpdated);
                Assert.IsTrue(anonOrderLayer.IsUpdatedTimeSubHourUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedUpdatedTime, anonOrderLayer.UpdatedTime);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(4, quoteUpdates.Count);
                var layerUpdates = ordersPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(2, layerUpdates.Count);
                var orderIndex                = (ushort)i;
                var hoursSinceUnixEpoch       = expectedUpdatedTime.GetHoursFromUnixEpoch();
                var expectedDateLayerField    = new PQFieldUpdate(PQQuoteFields.OrderUpdatedDate, hoursSinceUnixEpoch, 0, orderIndex);
                var extended                  = expectedUpdatedTime.GetSubHourComponent().BreakLongToUShortAndUint(out var subHourBottom);
                var expectedSubHourLayerField = new PQFieldUpdate(PQQuoteFields.OrderUpdatedTimeSubHour, subHourBottom, extended, orderIndex);
                var depthId                   = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideDateLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderUpdatedDate, depthId, expectedDateLayerField.Payload, 0, orderIndex);
                var expectedSideSubHourLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderUpdatedTimeSubHour, depthId, expectedSubHourLayerField.Payload,
                                      expectedSubHourLayerField.ExtendedPayload, orderIndex);
                Assert.AreEqual(expectedDateLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSubHourLayerField, layerUpdates[1]);
                Assert.AreEqual(expectedSideDateLayerField, quoteUpdates[2]);
                Assert.AreEqual(expectedSideSubHourLayerField, quoteUpdates[3]);

                anonOrderLayer.IsUpdatedTimeDateUpdated    = false;
                anonOrderLayer.IsUpdatedTimeSubHourUpdated = false;
                Assert.IsFalse(ordersPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsUpdatedTimeDateUpdated    = true;
                anonOrderLayer.IsUpdatedTimeSubHourUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where (update.Id == PQQuoteFields.OrderUpdatedDate && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex)
                           || (update.Id == PQQuoteFields.OrderUpdatedTimeSubHour && update.DepthId == depthId &&
                               update.AuxiliaryPayload == orderIndex)
                        select update).ToList();
                Assert.AreEqual(2, quoteUpdates.Count);
                Assert.AreEqual(expectedSideDateLayerField, quoteUpdates[0]);
                Assert.AreEqual(expectedSideSubHourLayerField, quoteUpdates[1]);
                anonOrderLayer.UpdatedTime                 = DateTimeConstants.UnixEpoch;
                anonOrderLayer.IsUpdatedTimeDateUpdated    = false;
                anonOrderLayer.IsUpdatedTimeSubHourUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateField(quoteUpdates[1]);
                var foundLayer = (IPQOrdersPriceVolumeLayer)
                    (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundAnonOrderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdatedTime);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeDateUpdated);
                Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeSubHourUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var orderPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                        .OfType<IPQOrdersPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, orderPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, orderPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayerInfo = orderPriceVolumeLayer[i]!;

                Assert.IsFalse(anonOrderLayerInfo.IsOrderVolumeUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0m, anonOrderLayerInfo.OrderVolume);
                Assert.AreEqual(0,
                                orderPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedOrderVolume = 254682m;
                anonOrderLayerInfo.OrderVolume = expectedOrderVolume;
                Assert.IsTrue(anonOrderLayerInfo.IsOrderVolumeUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedOrderVolume, anonOrderLayerInfo.OrderVolume);
                var precisionSettings = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var quoteUpdates      = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = orderPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var orderIndex             = (ushort)i;
                var volumeScalingPrecision = precisionSettings.VolumeScalingPrecision;
                var expectedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderVolume, expectedOrderVolume, orderIndex, volumeScalingPrecision);
                var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderVolume, depthId,
                                      expectedLayerField.Payload, expectedLayerField.ExtendedPayload, expectedLayerField.AuxiliaryPayload
                                    , expectedLayerField.Flag);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                anonOrderLayerInfo.IsOrderVolumeUpdated = false;
                Assert.IsFalse(orderPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayerInfo.IsOrderVolumeUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.OrderVolume && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                anonOrderLayerInfo.OrderVolume          = 0m;
                anonOrderLayerInfo.IsOrderVolumeUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundTraderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedOrderVolume, foundTraderInfo.OrderVolume);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundTraderInfo.HasUpdates);
                Assert.IsTrue(foundTraderInfo.IsOrderVolumeUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderRemainingVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var orderPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                        .OfType<IPQOrdersPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, orderPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                         .Select((pvl, i) => new { i, pvl })
                                         .Where(indexPvl => ReferenceEquals(indexPvl.pvl, orderPriceVolumeLayer))
                                         .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayerInfo = orderPriceVolumeLayer[i]!;

                Assert.IsFalse(anonOrderLayerInfo.IsOrderRemainingVolumeUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0m, anonOrderLayerInfo.OrderRemainingVolume);
                Assert.AreEqual(0,
                                orderPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedOrderRemainingVolume = 254682m;
                anonOrderLayerInfo.OrderRemainingVolume = expectedOrderRemainingVolume;
                Assert.IsTrue(anonOrderLayerInfo.IsOrderRemainingVolumeUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedOrderRemainingVolume, anonOrderLayerInfo.OrderRemainingVolume);
                var precisionSettings = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var quoteUpdates      = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = orderPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var orderIndex             = (ushort)i;
                var volumeScalingPrecision = precisionSettings.VolumeScalingPrecision;
                var expectedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderRemainingVolume, expectedOrderRemainingVolume, orderIndex, volumeScalingPrecision);
                var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderRemainingVolume, depthId,
                                      expectedLayerField.Payload, expectedLayerField.ExtendedPayload, expectedLayerField.AuxiliaryPayload
                                    , expectedLayerField.Flag);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                anonOrderLayerInfo.IsOrderRemainingVolumeUpdated = false;
                Assert.IsFalse(orderPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayerInfo.IsOrderRemainingVolumeUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.OrderRemainingVolume && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                anonOrderLayerInfo.OrderRemainingVolume          = 0m;
                anonOrderLayerInfo.IsOrderRemainingVolumeUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundTraderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedOrderRemainingVolume, foundTraderInfo.OrderRemainingVolume);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundTraderInfo.HasUpdates);
                Assert.IsTrue(foundTraderInfo.IsOrderRemainingVolumeUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerOrderTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var nameCounter = 0;
            foreach (var cpOrdersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                               .OfType<IPQOrdersPriceVolumeLayer>()
                                                               .Where(opvl => opvl.LayerType.SupportsOrdersFullPriceVolume()))
            {
                var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, cpOrdersPriceVolumeLayer));
                var indexFromTop =
                    (isBid
                        ? emptyQuote.BidBook
                        : emptyQuote.AskBook).AllLayers
                                             .Select((pvl, i) => new { i, pvl })
                                             .Where(indexPvl => ReferenceEquals(indexPvl.pvl, cpOrdersPriceVolumeLayer))
                                             .Select(indexPvl => indexPvl.i).FirstOrDefault();

                for (var i = 0; i < 256; i++)
                {
                    nameCounter++;
                    if (i == 5) i = 254;
                    testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                    var cpOrderLayerInfo = (IPQCounterPartyOrderLayerInfo)cpOrdersPriceVolumeLayer[i]!;

                    Assert.IsFalse(cpOrderLayerInfo.IsTraderNameUpdated);
                    Assert.IsFalse(emptyQuote.HasUpdates);
                    Assert.AreEqual(null, cpOrderLayerInfo.TraderName);
                    Assert.AreEqual(0, cpOrdersPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                    Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                    var expectedTraderName = "NewChangedTraderName" + nameCounter;
                    cpOrderLayerInfo.TraderName = expectedTraderName;
                    Assert.IsTrue(cpOrderLayerInfo.IsTraderNameUpdated);
                    Assert.IsTrue(emptyQuote.HasUpdates);
                    Assert.AreEqual(expectedTraderName, cpOrderLayerInfo.TraderName);
                    var allDeltaUpdateFields = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                    var quoteUpdates =
                        allDeltaUpdateFields
                            .Where(fu => fu.Id is PQQuoteFields.OrderTraderNameId).ToList();
                    Assert.AreEqual(1, quoteUpdates.Count);
                    var layerUpdates = cpOrdersPriceVolumeLayer
                                       .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                    Assert.AreEqual(1, layerUpdates.Count);
                    var orderIndex         = (ushort)i;
                    var dictId             = cpOrdersPriceVolumeLayer.NameIdLookup[cpOrderLayerInfo.TraderName];
                    var expectedLayerField = new PQFieldUpdate(PQQuoteFields.OrderTraderNameId, (uint)dictId, 0, orderIndex);
                    var depthId            = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                    var expectedSideAdjustedLayerField =
                        new PQFieldUpdate
                            (PQQuoteFields.OrderTraderNameId, depthId, expectedLayerField.Payload, 0, orderIndex);
                    Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                    Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                    var stringUpdates =
                        cpOrdersPriceVolumeLayer.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
                    var stringUpdateCommand = CrudCommand.Upsert.ToUShort();
                    var selectedStringUpdate =
                        stringUpdates.FirstOrDefault
                            (su => su.Field.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand
                                && su.Field.ExtendedPayload == stringUpdateCommand && su.StringUpdate.DictionaryId == dictId);
                    Assert.IsFalse(Equals(selectedStringUpdate, new PQFieldStringUpdate()));
                    var expectedStringUpdates = new PQFieldStringUpdate
                    {
                        Field = new PQFieldUpdate(PQQuoteFields.LayerNameDictionaryUpsertCommand, 0u, stringUpdateCommand, orderIndex)
                      , StringUpdate = new PQStringUpdate
                        {
                            Command      = CrudCommand.Upsert
                          , DictionaryId = dictId
                          , Value        = expectedTraderName
                        }
                    };
                    Assert.AreEqual(expectedStringUpdates, selectedStringUpdate);

                    cpOrderLayerInfo.IsTraderNameUpdated      = false;
                    cpOrderLayerInfo.NameIdLookup!.HasUpdates = false;
                    Assert.IsFalse(cpOrdersPriceVolumeLayer.HasUpdates);
                    Assert.IsTrue(emptyQuote.HasUpdates);
                    emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                    emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                    Assert.IsFalse(emptyQuote.HasUpdates);
                    Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                    cpOrderLayerInfo.IsTraderNameUpdated = true;
                    quoteUpdates =
                        (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                            where update.Id == PQQuoteFields.OrderTraderNameId && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                            select update).ToList();
                    Assert.AreEqual(1, quoteUpdates.Count);
                    Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                    cpOrderLayerInfo.NameIdLookup.Clear();
                    cpOrderLayerInfo.TraderName          = null;
                    cpOrderLayerInfo.IsTraderNameUpdated = false;

                    var diffNameIdLookupSrcTkrInfo =
                        new PQSourceTickerInfo(emptyQuote.SourceTickerInfo!)
                        {
                            NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand)
                        };
                    var newEmpty = new PQLevel2Quote(diffNameIdLookupSrcTkrInfo);
                    newEmpty.UpdateField(quoteUpdates[0]);
                    var applySided = expectedStringUpdates.WithDepth(depthId);
                    newEmpty.UpdateFieldString(applySided);
                    var foundLayer =
                        (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                    var foundTraderInfo = (IPQCounterPartyOrderLayerInfo)foundLayer[i]!;
                    Assert.AreEqual(expectedTraderName, foundTraderInfo.TraderName);
                    Assert.IsTrue(newEmpty.HasUpdates);
                    Assert.IsTrue(foundTraderInfo.HasUpdates);
                    Assert.IsTrue(foundTraderInfo.IsTraderNameUpdated);
                }
            }
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
            AssertContainsAllLevel2Fields(precisionSettings, pqFieldUpdates, populatedL2Quote, PQBooleanValuesExtensions.AllFields);
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
           , ordersCountFullyPopulatedLevel2Quote, PQBooleanValuesExtensions.AllFields);
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
                ((PQSourceTickerInfo)populatedL2Quote.SourceTickerInfo!, pqFieldUpdates, populatedL2Quote, PQBooleanValuesExtensions.AllFields);
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
            ((PQSourceTickerInfo)ordersCountFullyPopulatedLevel2Quote.SourceTickerInfo!, pqFieldUpdates, ordersCountFullyPopulatedLevel2Quote);
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
            ((PQSourceTickerInfo)ordersAnonFullyPopulatedLevel2Quote.SourceTickerInfo!, pqFieldUpdates, ordersAnonFullyPopulatedLevel2Quote);
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
            ((PQSourceTickerInfo)ordersCountFullyPopulatedLevel2Quote.SourceTickerInfo!, pqFieldUpdates, ordersCountFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            populatedL2Quote.IsReplay   = true;
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
                    NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand)
                };
            var newEmpty = new PQLevel2Quote(emptyQuoteSourceTickerInfo);
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
            var newEmpty = new PQLevel2Quote(emptyQuoteSourceTickerInfo);
            newEmpty.CopyFrom(populatedL2Quote);
            Assert.AreEqual(populatedL2Quote, newEmpty, populatedL2Quote.DiffQuotes(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL2Quote.SourceTickerInfo!);
            var emptyQuote = new PQLevel2Quote(emptyQuoteSourceTickerInfo);
            populatedL2Quote.HasUpdates = false;
            emptyQuote.CopyFrom(populatedL2Quote);
            Assert.AreEqual(populatedL2Quote.PQSequenceId, emptyQuote.PQSequenceId);
            Assert.AreEqual(default, emptyQuote.SourceTime);
            Assert.IsTrue(populatedL2Quote.SourceTickerInfo!.AreEquivalent(emptyQuote.SourceTickerInfo));
            Assert.AreEqual(false, emptyQuote.IsReplay);
            Assert.AreEqual(0m, emptyQuote.SingleTickValue);
            Assert.AreEqual(FeedSyncStatus.OutOfSync, emptyQuote.FeedSyncStatus);
            Assert.AreEqual(default, emptyQuote.SourceBidTime);
            Assert.AreEqual(default, emptyQuote.SourceAskTime);
            Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
            Assert.AreEqual(default, emptyQuote.AdapterSentTime);
            Assert.AreEqual(default, emptyQuote.ClientReceivedTime);
            Assert.AreEqual(default, emptyQuote.ProcessedTime);
            Assert.AreEqual(default, emptyQuote.DispatchedTime);
            Assert.AreEqual(default, emptyQuote.SocketReceivingTime);
            Assert.AreEqual(0m, emptyQuote.BidPriceTop);
            Assert.AreEqual(0m, emptyQuote.AskPriceTop);
            Assert.IsTrue(emptyQuote.Executable);
            Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsReplayUpdated);
            Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
            Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
            Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
            Assert.IsTrue(emptyQuote.IsExecutableUpdated);
            foreach (var pvl in emptyQuote.BidBook) AssertAreDefaultValues(pvl);
            foreach (var pvl in emptyQuote.AskBook) AssertAreDefaultValues(pvl);
        }
    }

    [TestMethod]
    public void NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var nonPQLevel2Quote = new Level2PriceQuote(populatedL2Quote);
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL2Quote.SourceTickerInfo!);
            var newEmpty = new PQLevel2Quote(emptyQuoteSourceTickerInfo);
            newEmpty.CopyFrom(nonPQLevel2Quote);
            Assert.IsTrue(populatedL2Quote.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var clonedQuote = ((ICloneable<ITickInstant>)populatedL2Quote).Clone();
            Assert.AreNotSame(clonedQuote, populatedL2Quote);
            if (!clonedQuote.Equals(populatedL2Quote))
                Console.Out.WriteLine("clonedQuote differences are \n" + clonedQuote.DiffQuotes(populatedL2Quote) + "'");
            Assert.AreEqual(populatedL2Quote, clonedQuote);

            var cloned2 = (PQLevel2Quote)((ICloneable)populatedL2Quote).Clone();
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
            var fullyPopulatedClone = (PQLevel2Quote)((ICloneable)populatedL2Quote).Clone();
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
            Assert.AreEqual(populatedL2Quote, ((ICloneable<ITickInstant>)populatedL2Quote).Clone());
            Assert.AreEqual(populatedL2Quote, ((ICloneable<ILevel1Quote>)populatedL2Quote).Clone());
            Assert.AreEqual(populatedL2Quote, ((ICloneable<ILevel2Quote>)populatedL2Quote).Clone());
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
        var q      = everyLayerFullyPopulatedLevel2Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQLevel2Quote original, PQLevel2Quote changingLevel2Quote)
    {
        PQLevel1QuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingLevel2Quote);

        if (original.GetType() == typeof(PQLevel2Quote))
            Assert.AreEqual(!exactComparison,
                            changingLevel2Quote.AreEquivalent(new Level2PriceQuote(original), exactComparison));

        PQOrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, (PQOrderBook)original.BidBook, (PQOrderBook)changingLevel2Quote.BidBook,
             original, changingLevel2Quote);

        PQOrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, (PQOrderBook)original.AskBook, (PQOrderBook)changingLevel2Quote.AskBook,
             original, changingLevel2Quote);
    }

    public static void AssertContainsAllLevel2Fields
    (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates,
        PQLevel2Quote l2Q, PQBooleanValues expectedBooleanFlags = PQBooleanValuesExtensions.AllFields)
    {
        PQLevel1QuoteTests.AssertContainsAllLevel1Fields(precisionSettings, checkFieldUpdates, l2Q, expectedBooleanFlags);

        var priceScale  = precisionSettings.PriceScalingPrecision;
        var volumeScale = precisionSettings.VolumeScalingPrecision;

        var maxLayers = Math.Min(l2Q.BidBook.Count, l2Q.AskBook.Count);

        for (var i = 0; i < maxLayers; i++)
        {
            var bidL2Pvl = l2Q.BidBook[i]!;
            var askL2Pvl = l2Q.AskBook[i]!;

            var bidDepthId = (PQDepthKey)i;
            var askDepthId = (PQDepthKey)i | PQDepthKey.AskSide;

            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.Price, bidDepthId, PQScaling.Scale(bidL2Pvl.Price, priceScale), priceScale)
                          , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.Price, bidDepthId, priceScale)
                          , $"For bidlayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual
                (new PQFieldUpdate(PQQuoteFields.Price, askDepthId, PQScaling.Scale(askL2Pvl.Price, priceScale), priceScale)
               , PQTickInstantTests.ExtractFieldUpdateWithId
                     (checkFieldUpdates, PQQuoteFields.Price, askDepthId, priceScale)
               , $"For asklayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual
                (new PQFieldUpdate
                     (PQQuoteFields.Volume, bidDepthId, PQScaling.Scale(bidL2Pvl.Volume, volumeScale), volumeScale)
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.Volume, bidDepthId, volumeScale)
               , $"For bidlayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual
                (new PQFieldUpdate
                     (PQQuoteFields.Volume, askDepthId, PQScaling.Scale(askL2Pvl.Volume, volumeScale), volumeScale)
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.Volume, askDepthId, volumeScale)
               , $"For asklayer {bidL2Pvl.GetType().Name} level {i}");

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
    (IList<PQFieldUpdate> allUpdates, PQQuoteFields id, PQDepthKey depthId,
        uint value, ushort extended, ushort auxiliaryPayload, PQFieldFlags flag = 0)
    {
        var useExtendedFlag  = extended > 0 ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None;
        var useAuxiliaryFlag = auxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None;
        var useDepthFlag     = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        var tryFlags         = flag | useDepthFlag | useExtendedFlag | useAuxiliaryFlag;
        var tryGetValue = allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.ExtendedPayload == extended
                                                       && fu.AuxiliaryPayload == auxiliaryPayload && fu.Payload == value && fu.Flag == tryFlags);
        var tryAgainValue = !Equals(tryGetValue, default(PQFieldUpdate))
            ? tryGetValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.ExtendedPayload == extended &&
                                              fu.Flag == (flag | useDepthFlag | useExtendedFlag));
        var tryTryAgainValue = !Equals(tryAgainValue, default(PQFieldUpdate))
            ? tryAgainValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.Flag == flag);
        return tryTryAgainValue;
    }

    private void AssertLayerTypeIsExpected(Type expectedType, params PQLevel2Quote[] quotesToCheck)
    {
        foreach (var level2Quote in quotesToCheck)
            for (var i = 0; i < level2Quote.SourceTickerInfo!.MaximumPublishedLayers; i++)
            {
                Assert.AreEqual(expectedType, level2Quote.BidBook[i]!.GetType());
                Assert.AreEqual(expectedType, level2Quote.AskBook[i]!.GetType());
                switch (level2Quote.BidBook[i])
                {
                    case PQSourceQuoteRefOrdersValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvl:
                        Assert.IsTrue
                            (ReferenceEquals
                                (((IPQSourcePriceVolumeLayer)level2Quote.BidBook[0]!).NameIdLookup, srcQtRefTrdrVlDtPvl.NameIdLookup));
                        Assert.IsTrue
                            (ReferenceEquals
                                (((IPQOrdersPriceVolumeLayer)level2Quote.BidBook[0]!).NameIdLookup, srcQtRefTrdrVlDtPvl.NameIdLookup));
                        break;
                    case PQSourcePriceVolumeLayer sourcePriceVolumeLayer:
                        Assert.IsTrue
                            (ReferenceEquals
                                (((IPQSourcePriceVolumeLayer)level2Quote.BidBook[0]!).NameIdLookup, sourcePriceVolumeLayer.NameIdLookup));
                        break;
                    case PQOrdersPriceVolumeLayer traderPriceVolumeLayer:
                        Assert.IsTrue
                            (ReferenceEquals
                                (((IPQOrdersPriceVolumeLayer)level2Quote.BidBook[0]!).NameIdLookup, traderPriceVolumeLayer.NameIdLookup));
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
            Assert.AreEqual(DateTimeConstants.UnixEpoch, valueDatePvl.ValueDate);
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
            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SourceId, bidDepthId);
        var askSrcId =
            PQTickInstantTests.ExtractFieldUpdateWithId
                (checkFieldUpdates, PQQuoteFields.SourceId, askDepthId);

        var bidNameIdLookup = pqBidL2Pvl.NameIdLookup;
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SourceId, bidDepthId, (uint)bidNameIdLookup[pqBidL2Pvl.SourceName!]), bidSrcId);
        var askNameIdLookup = pqAskL2Pvl.NameIdLookup;
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SourceId, askDepthId, (uint)askNameIdLookup[pqAskL2Pvl.SourceName!]), askSrcId);

        var bidSrcExecutable = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerBooleanFlags, bidDepthId);
        var askSrcExecutable = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerBooleanFlags, askDepthId);

        var bidExecutable = (uint)(pqBidL2Pvl.Executable ? LayerBooleanFlags.IsExecutableFlag : LayerBooleanFlags.None);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerBooleanFlags, bidDepthId, bidExecutable), bidSrcExecutable);
        var askExecutable = (uint)(pqAskL2Pvl.Executable ? LayerBooleanFlags.IsExecutableFlag : LayerBooleanFlags.None);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerBooleanFlags, askDepthId, askExecutable), askSrcExecutable);
    }

    private static void AssertSourceQuoteRefContainsAllFields
    (IList<PQFieldUpdate> checkFieldUpdates, int i,
        IPQSourceQuoteRefPriceVolumeLayer bidSrcQtRefPvl, IPQSourceQuoteRefPriceVolumeLayer askSrcQtRefPvl)
    {
        var bidDepthId = (PQDepthKey)i;
        var askDepthId = (PQDepthKey)i | PQDepthKey.AskSide;

        var bidSrcQtRef = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerSourceQuoteRef, bidDepthId);
        var askSrcQtRef = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerSourceQuoteRef, askDepthId);

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerSourceQuoteRef, bidDepthId, bidSrcQtRefPvl.SourceQuoteReference), bidSrcQtRef);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerSourceQuoteRef, askDepthId, askSrcQtRefPvl.SourceQuoteReference), askSrcQtRef);
    }

    private static void AssertValueDateLayerContainsAllFields
    (IList<PQFieldUpdate> checkFieldUpdates, int i,
        IPQValueDatePriceVolumeLayer bidValueDatePvl, IPQValueDatePriceVolumeLayer askValueDatePvl)
    {
        var bidDepthId = (PQDepthKey)i;
        var askDepthId = (PQDepthKey)i | PQDepthKey.AskSide;

        var bidValueDate = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerValueDate, bidDepthId);
        var askValueDate = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerValueDate, askDepthId);

        var dateAsHoursFromEpoch = bidValueDatePvl.ValueDate.GetHoursFromUnixEpoch();
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerValueDate, bidDepthId, dateAsHoursFromEpoch), bidValueDate);
        dateAsHoursFromEpoch = askValueDatePvl.ValueDate.GetHoursFromUnixEpoch();
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerValueDate, askDepthId, dateAsHoursFromEpoch), askValueDate);
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

            var orderId   = (uint)anonOrderLayerInfo.OrderId;
            var orderIdFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderId, depthId, orderId, 0, orderIndex);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderId, depthId, orderId, 0, orderIndex), orderIdFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderFlags   = (uint)anonOrderLayerInfo.OrderFlags;
            var orderFlagsFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderFlags, depthId, orderFlags, 0, orderIndex);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderFlags, depthId, orderFlags, 0, orderIndex), orderFlagsFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderCreatedDate = anonOrderLayerInfo.CreatedTime.GetHoursFromUnixEpoch();
            var orderCreatedDateFu
                = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderCreatedDate, depthId, orderCreatedDate, 0, orderIndex);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderCreatedDate, depthId, orderCreatedDate, 0, orderIndex), orderCreatedDateFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderCreatedSubHourExtended
                = anonOrderLayerInfo.CreatedTime.GetSubHourComponent().BreakLongToUShortAndUint(out var orderCreatedSubHour);
            var orderCreatedSubHourFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderCreatedTimeSubHour, depthId
                                                               , orderCreatedSubHour, orderCreatedSubHourExtended, orderIndex);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderCreatedTimeSubHour, depthId, orderCreatedSubHour, orderCreatedSubHourExtended, orderIndex)
                          , orderCreatedSubHourFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderUpdatedDate = anonOrderLayerInfo.UpdatedTime.GetHoursFromUnixEpoch();
            var orderUpdatedDateFu
                = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderUpdatedDate, depthId, orderUpdatedDate, 0, orderIndex);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderUpdatedDate, depthId, orderUpdatedDate, 0, orderIndex), orderUpdatedDateFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderUpdatedSubHourExtended
                = anonOrderLayerInfo.UpdatedTime.GetSubHourComponent().BreakLongToUShortAndUint(out var orderUpdatedSubHour);
            var orderUpdatedSubHourFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderUpdatedTimeSubHour, depthId
                                                               , orderUpdatedSubHour, orderUpdatedSubHourExtended, orderIndex);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderUpdatedTimeSubHour, depthId, orderUpdatedSubHour, orderUpdatedSubHourExtended, orderIndex)
                          , orderUpdatedSubHourFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderVolume = PQScaling.Scale(anonOrderLayerInfo.OrderVolume, volumeScale);
            var orderVolumeFu
                = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderVolume, depthId, orderVolume, 0, orderIndex, volumeScale);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderVolume, depthId, orderVolume, 0, orderIndex, volumeScale), orderVolumeFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var orderRemainingVolume = PQScaling.Scale(anonOrderLayerInfo.OrderRemainingVolume, volumeScale);
            var orderRemainingVolumeFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderRemainingVolume, depthId, orderRemainingVolume
                                                                , 0, orderIndex, volumeScale);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderRemainingVolume, depthId, orderRemainingVolume, 0, orderIndex, volumeScale)
                          , orderRemainingVolumeFu,
                            $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (anonOrderLayerInfo is IPQCounterPartyOrderLayerInfo cpOrderLayerInfo)
            {
                var orderCpNameId = (uint)nameIdLookup[cpOrderLayerInfo.CounterPartyName];
                var orderCpIdFu = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderCounterPartyNameId, depthId, orderCpNameId, 0
                                                         , orderIndex);
                Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderCounterPartyNameId, depthId, orderCpNameId, 0, orderIndex), orderCpIdFu,
                                $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");

                var orderTraderId = (uint)nameIdLookup[cpOrderLayerInfo.TraderName];
                var orderTraderIdFu
                    = ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrderTraderNameId, depthId, orderTraderId, 0, orderIndex);
                Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrderTraderNameId, depthId, orderTraderId, 0, orderIndex), orderTraderIdFu,
                                $"For {ordersPvl.GetType().Name} at [{depthIndex}][{j}] with these fields\n{string.Join(",\n", checkFieldUpdates)}");
                return;
            }
        }
    }
}
