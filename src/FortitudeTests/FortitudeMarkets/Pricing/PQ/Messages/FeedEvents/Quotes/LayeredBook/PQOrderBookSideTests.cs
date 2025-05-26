// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;

[TestClass]
public class PQOrderBookSideTests
{
    private const int MaxNumberOfLayers = 19; // test being less than max.

    private const decimal ExpectedPrice          = 1.234567m;
    private const decimal ExpectedVolume         = 40_000_000m;
    private const string  ExpectedSourceName     = "TestSourceName";
    private const bool    ExpectedExecutable     = true;
    private const uint    ExpectedSourceQuoteRef = 12345678u;
    private const int     ExpectedOrdersCount    = 3; // not too many traders.
    private const decimal ExpectedInternalVolume = 20_000_000m;

    private const OrderGenesisFlags ExpectedGenesisFlags = OrderGenesisFlags.FromAdapter | OrderGenesisFlags.IsExternalOrder | OrderGenesisFlags.HasExternalCounterPartyInfo;
    private const OrderType         ExpectedOrderType    = OrderType.PassiveLimit;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.ConfirmedActiveOnMarket;

    private const uint    ExpectedTrackingId = 12467u;
    private const uint    ExpectedCounterPartyId = 1u;
    private const uint    ExpectedTraderId = 2u;

    private const int     ExpectedOrderId              = 250;
    private const decimal ExpectedOrderVolume          = 50.50m;
    private const decimal ExpectedOrderRemainingVolume = 10.25m;
    private const string  ExpectedCounterPartyBase     = "TestCounterPartyName_";
    private const string  ExpectedTraderNameBase       = "TestTraderName_";

    private const MarketDataSource ExpectedDataSource         = MarketDataSource.Venue;

    private const decimal          ExpectedOpenInterestVolume = ExpectedOrderVolume * 100;
    private const decimal          ExpectedOpenInterestVwap   = ExpectedPrice * 2m;

    private const uint ExpectedDailyTickCount = 2_582;

    private static readonly PQMarketAggregate ExpectedSidedOpenInterest =
        new(ExpectedDataSource, ExpectedOpenInterestVolume, ExpectedOpenInterestVwap, new(2025, 5, 8, 12, 8, 59));

    private static readonly DateTime ExpectedValueDate        = new(2017, 12, 09, 14, 0, 0);
    private static readonly DateTime ExpectedOrderCreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime ExpectedOrderUpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
    
    private IPQOrderBookSide allFieldsFullyPopulatedOrderBookSide = null!;

    private IList<IPQFullSupportPriceVolumeLayer> allFieldsLayers = null!;

    private List<IReadOnlyList<IPQPriceVolumeLayer?>> allPopulatedLayers = null!;

    private List<IPQOrderBookSide> allPopulatedOrderBooks = null!;

    private PQNameIdLookupGenerator          nameIdLookupGenerator                         = null!;
    private IPQOrderBookSide                 ordersAnonFullyPopulatedOrderBookSide         = null!;
    private IList<IPQOrdersPriceVolumeLayer> ordersAnonLayers                              = null!;
    private IPQOrderBookSide                 ordersCounterPartyFullyPopulatedOrderBookSide = null!;
    private IList<IPQOrdersPriceVolumeLayer> ordersCounterPartyLayers                      = null!;
    private IPQOrderBookSide                 ordersCountFullyPopulatedOrderBookSide        = null!;

    private IList<IPQOrdersCountPriceVolumeLayer> ordersCountLayers = null!;

    private PQSourceTickerInfo publicationPrecisionSettings = null!;

    private IPQOrderBookSide simpleFullyPopulatedOrderBookSide = null!;

    private IList<IPQPriceVolumeLayer>       simpleLayers                           = null!;
    private IPQOrderBookSide                 sourceFullyPopulatedOrderBookSide      = null!;
    private IList<IPQSourcePriceVolumeLayer> sourceLayers                           = null!;
    private IPQOrderBookSide                 sourceQtRefFullyPopulatedOrderBookSide = null!;

    private IList<IPQSourceQuoteRefPriceVolumeLayer> sourceQtRefLayers                    = null!;
    private IPQOrderBookSide                         valueDateFullyPopulatedOrderBookSide = null!;
    private IList<IPQValueDatePriceVolumeLayer>      valueDateLayers                      = null!;

    private static DateTime testDateTime = new(2025, 5, 7, 18, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator    = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        ordersCounterPartyLayers = new List<IPQOrdersPriceVolumeLayer>(MaxNumberOfLayers);

        simpleLayers      = new List<IPQPriceVolumeLayer>(MaxNumberOfLayers);
        sourceLayers      = new List<IPQSourcePriceVolumeLayer>(MaxNumberOfLayers);
        sourceQtRefLayers = new List<IPQSourceQuoteRefPriceVolumeLayer>(MaxNumberOfLayers);
        valueDateLayers   = new List<IPQValueDatePriceVolumeLayer>(MaxNumberOfLayers);
        ordersCountLayers = new List<IPQOrdersCountPriceVolumeLayer>(MaxNumberOfLayers);
        ordersAnonLayers  = new List<IPQOrdersPriceVolumeLayer>(MaxNumberOfLayers);
        allFieldsLayers   = new List<IPQFullSupportPriceVolumeLayer>(MaxNumberOfLayers);

        // placed in the same order as the orderBooks at the end of Setup
        allPopulatedLayers =
        [
            (IReadOnlyList<IPQPriceVolumeLayer>)simpleLayers, (IReadOnlyList<IPQPriceVolumeLayer>)sourceLayers
          , (IReadOnlyList<IPQPriceVolumeLayer>)sourceQtRefLayers, (IReadOnlyList<IPQPriceVolumeLayer>)valueDateLayers
          , (IReadOnlyList<IPQPriceVolumeLayer>)ordersCountLayers, (IReadOnlyList<IPQPriceVolumeLayer>)ordersAnonLayers
          , (IReadOnlyList<IPQPriceVolumeLayer>)ordersCounterPartyLayers, (IReadOnlyList<IPQPriceVolumeLayer>)allFieldsLayers
        ];

        for (var i = 0; i < MaxNumberOfLayers; i++)
        {
            simpleLayers.Add(new PQPriceVolumeLayer(ExpectedPrice, ExpectedVolume));

            var sourcePvl =
                new PQSourcePriceVolumeLayer
                    (nameIdLookupGenerator, ExpectedPrice, ExpectedVolume, ExpectedSourceName, ExpectedExecutable);
            sourceLayers.Add(sourcePvl);

            var srcQtRefPvl = new PQSourceQuoteRefPriceVolumeLayer
                (nameIdLookupGenerator, ExpectedPrice, ExpectedVolume, ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef);
            sourceQtRefLayers.Add(srcQtRefPvl);

            valueDateLayers.Add
                (new PQValueDatePriceVolumeLayer
                    (ExpectedPrice, ExpectedVolume, ExpectedValueDate));

            var allFieldsPvL = new PQFullSupportPriceVolumeLayer
                (nameIdLookupGenerator, ExpectedPrice, ExpectedVolume, ExpectedValueDate,
                 ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef, ExpectedOrdersCount, ExpectedInternalVolume);
            allFieldsLayers.Add(allFieldsPvL);
            var ordersCountPvl = new PQOrdersCountPriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
            ordersCountLayers.Add(ordersCountPvl);
            var anonOrdersPvL = new PQOrdersPriceVolumeLayer(nameIdLookupGenerator, LayerType.OrdersAnonymousPriceVolume, ExpectedPrice
                                                           , ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
            ordersAnonLayers.Add(anonOrdersPvL);
            var counterPartyOrdersPvL = new PQOrdersPriceVolumeLayer
                (nameIdLookupGenerator, LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
            ordersCounterPartyLayers.Add(counterPartyOrdersPvL);
            for (var j = 0; j < ExpectedOrdersCount; j++)
            {
                allFieldsPvL.Add
                    (new PQExternalCounterPartyOrder
                        (new PQAnonymousOrder(nameIdLookupGenerator, ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType
                       , ExpectedGenesisFlags, ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, ExpectedTrackingId)
                         {
                             ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(nameIdLookupGenerator, i + 1, ExpectedCounterPartyBase + i, i+ 1, ExpectedTraderNameBase + i)
                         }));
                anonOrdersPvL.Add
                    (new PQAnonymousOrder
                        (nameIdLookupGenerator, ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags
                       , ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, ExpectedTrackingId));
                counterPartyOrdersPvL.Add
                    (new PQExternalCounterPartyOrder
                        (new PQAnonymousOrder(nameIdLookupGenerator, ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType
                                            , ExpectedGenesisFlags, ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, ExpectedTrackingId)
                        {
                            ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(nameIdLookupGenerator, i + 1, ExpectedCounterPartyBase + i, i+ 1, ExpectedTraderNameBase + i)
                        }));
            }
        }

        simpleFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, simpleLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        sourceFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, sourceLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        sourceQtRefFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, sourceQtRefLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        ordersCountFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, ordersCountLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        ordersAnonFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, ordersAnonLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        valueDateFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, valueDateLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };

        ordersCounterPartyFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, ordersCounterPartyLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };

        allFieldsFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, allFieldsLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };

        allPopulatedOrderBooks =
        [
            simpleFullyPopulatedOrderBookSide, sourceFullyPopulatedOrderBookSide, sourceQtRefFullyPopulatedOrderBookSide
          , valueDateFullyPopulatedOrderBookSide, ordersCountFullyPopulatedOrderBookSide, ordersAnonFullyPopulatedOrderBookSide
          , ordersCounterPartyFullyPopulatedOrderBookSide, allFieldsFullyPopulatedOrderBookSide
        ];
        // [
        //     ordersAnonFullyPopulatedOrderBookSide, allFieldsFullyPopulatedOrderBookSide
        // ];
        publicationPrecisionSettings =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
                   , MaxNumberOfLayers, 0.000001m, 30000m, 50000000m, 1000m
                   , layerFlags: LayerFlags.Volume | LayerFlags.Price
                   , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                      LastTradedFlags.LastTradedTime));
    }

    [TestMethod]
    public void FromSourceTickerInfo_New_InitializesOrderBookWithExpectedLayerTypes()
    {
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var orderBook = new PQOrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable;
        orderBook = new PQOrderBookSide(BookSide.AskBook, publicationPrecisionSettings);

        AssertBookHasLayersOfType(orderBook, typeof(PQSourcePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        orderBook = new PQOrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQSourceQuoteRefPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate;
        orderBook = new PQOrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQValueDatePriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrdersCount;
        orderBook = new PQOrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQOrdersCountPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrdersCount | LayerFlags.OrderId | LayerFlags.OrderCreated
          | LayerFlags.OrderUpdated | LayerFlags.OrderSize | LayerFlags.OrderRemainingSize;
        orderBook = new PQOrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQOrdersCountPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrdersCount | LayerFlags.OrderId | LayerFlags.OrderCreated
          | LayerFlags.OrderUpdated | LayerFlags.OrderSize | LayerFlags.OrderRemainingSize | LayerFlags.OrderCounterPartyName |
            LayerFlags.OrderTraderName;
        orderBook = new PQOrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQOrdersPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags = LayerFlags.Price.AllFlags();
        orderBook                               = new PQOrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQFullSupportPriceVolumeLayer));
    }

    [TestMethod]
    public void NonPQLayers_New_ConvertsToPQEquivalent()
    {
        IList<IPriceVolumeLayer> nonPQList = new List<IPriceVolumeLayer>
        {
            new PriceVolumeLayer()
        };
        var orderBook = new PQOrderBookSide(BookSide.BidBook, nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQPriceVolumeLayer));

        nonPQList.Clear();
        nonPQList.Add(new SourcePriceVolumeLayer());
        orderBook = new PQOrderBookSide(BookSide.AskBook, nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQSourcePriceVolumeLayer));

        nonPQList.Clear();
        nonPQList.Add(new SourceQuoteRefPriceVolumeLayer());
        orderBook = new PQOrderBookSide(BookSide.BidBook, nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQSourceQuoteRefPriceVolumeLayer));

        nonPQList.Clear();
        nonPQList.Add(new ValueDatePriceVolumeLayer());
        orderBook = new PQOrderBookSide(BookSide.AskBook, nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQValueDatePriceVolumeLayer));

        nonPQList.Clear();
        nonPQList.Add(new OrdersCountPriceVolumeLayer());
        orderBook = new PQOrderBookSide(BookSide.BidBook, nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQOrdersCountPriceVolumeLayer));

        nonPQList.Clear();
        nonPQList.Add(new OrdersPriceVolumeLayer());
        orderBook = new PQOrderBookSide(BookSide.BidBook, nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQOrdersPriceVolumeLayer));

        nonPQList.Clear();
        nonPQList.Add(new FullSupportPriceVolumeLayer());
        orderBook = new PQOrderBookSide(BookSide.AskBook, nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQFullSupportPriceVolumeLayer));

        orderBook = new PQOrderBookSide();
        Assert.AreEqual(0, orderBook.Count);
    }

    [TestMethod]
    public void PQOrderBook_InitializedFromOrderBook_ConvertsLayers()
    {
        var nonPqOrderBookSide = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        var pqOrderBookSide    = new PQOrderBookSide(nonPqOrderBookSide);
        AssertBookHasLayersOfType(pqOrderBookSide, typeof(PQPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable;
        nonPqOrderBookSide = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        pqOrderBookSide    = new PQOrderBookSide(nonPqOrderBookSide);
        AssertBookHasLayersOfType(pqOrderBookSide, typeof(PQSourcePriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        nonPqOrderBookSide = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        pqOrderBookSide    = new PQOrderBookSide(nonPqOrderBookSide);
        AssertBookHasLayersOfType(pqOrderBookSide, typeof(PQSourceQuoteRefPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate;
        nonPqOrderBookSide = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        pqOrderBookSide    = new PQOrderBookSide(nonPqOrderBookSide);
        AssertBookHasLayersOfType(pqOrderBookSide, typeof(PQValueDatePriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrdersCount;
        nonPqOrderBookSide = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        pqOrderBookSide    = new PQOrderBookSide(nonPqOrderBookSide);
        AssertBookHasLayersOfType(pqOrderBookSide, typeof(PQOrdersCountPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrdersCount | LayerFlags.OrderId | LayerFlags.OrderCreated
          | LayerFlags.OrderUpdated | LayerFlags.OrderSize | LayerFlags.OrderRemainingSize;
        nonPqOrderBookSide = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        pqOrderBookSide    = new PQOrderBookSide(nonPqOrderBookSide);
        AssertBookHasLayersOfType(pqOrderBookSide, typeof(PQOrdersPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrdersCount | LayerFlags.OrderSize;
        nonPqOrderBookSide = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        pqOrderBookSide    = new PQOrderBookSide(nonPqOrderBookSide);
        AssertBookHasLayersOfType(pqOrderBookSide, typeof(PQOrdersPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags = LayerFlags.Price.AllFlags();

        nonPqOrderBookSide = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        pqOrderBookSide    = new PQOrderBookSide(nonPqOrderBookSide);

        AssertBookHasLayersOfType(pqOrderBookSide, typeof(PQFullSupportPriceVolumeLayer));
    }

    [TestMethod]
    public void NewOrderBook_InitializedWithLayers_ContainsSameInstanceLayersAsInitialized()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var populatedLayers    = allPopulatedLayers[i];
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.AllLayers.Count);
            for (var j = 0; j < MaxNumberOfLayers; j++) Assert.AreSame(populatedLayers[j], populatedOrderBook[j]);
        }
    }

    [TestMethod]
    public void NewOrderBook_InitializedFromOrderBook_ClonesAllLayers()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var clonedOrderBook    = new PQOrderBookSide(populatedOrderBook);
            Assert.AreEqual(MaxNumberOfLayers, clonedOrderBook.AllLayers.Count);
            for (var j = 0; j < ExpectedOrdersCount; j++) Assert.AreNotSame(populatedOrderBook[j], clonedOrderBook[j]);
        }

        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var emptyOrderBook = new PQOrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        Assert.AreEqual(0, emptyOrderBook.Count);
        var clonedEmptyOrderBook = new PQOrderBookSide(emptyOrderBook);
        Assert.AreEqual(0, clonedEmptyOrderBook.Count);
        for (var j = 0; j < ExpectedOrdersCount; j++) Assert.AreNotSame(emptyOrderBook[j], clonedEmptyOrderBook[j]);
    }

    [TestMethod]
    public void PopulatedOrderBook_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
            for (var i = 0; i < MaxNumberOfLayers; i++)
            {
                var layer       = ((IOrderBookSide)populatedOrderBook)[i];
                var clonedLayer = (IPQPriceVolumeLayer)layer!.Clone();

                populatedOrderBook[i] = clonedLayer;
                Assert.AreNotSame(layer, ((IMutableOrderBookSide)populatedOrderBook)[i]);
                Assert.AreSame(clonedLayer, populatedOrderBook[i]);
                if (i == populatedOrderBook.AllLayers.Count - 1)
                {
                    ((IMutableOrderBookSide)populatedOrderBook)[i] = null;
                    Assert.AreEqual(MaxNumberOfLayers - 1, populatedOrderBook.Count);
                }
            }
    }


    [TestMethod]
    public void PopulatedOrderBook_Capacity_ShowMaxPossibleNumberOfLayersNotNull()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.AreEqual(populatedOrderBook.AllLayers.Count, populatedOrderBook.Capacity);
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Capacity);
            populatedOrderBook[MaxNumberOfLayers - 1] = null;
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Capacity);
            Assert.AreEqual(populatedOrderBook.AllLayers.Count, populatedOrderBook.Capacity);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PopulatedOrderBook_CapacityLargerThanMaxBookDepth_ThrowsException()
    {
        simpleFullyPopulatedOrderBookSide.Capacity = PQQuoteFieldsExtensions.SingleByteFieldIdMaxBookDepth + 1;
    }

    [TestMethod]
    public void PopulatedOrderBook_DailyTickUpdateCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        simpleFullyPopulatedOrderBookSide.HasUpdates = false;

        AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected(simpleFullyPopulatedOrderBookSide);
    }

    public static void AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected
    (
        IPQOrderBookSide? orderBookSide,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (orderBookSide == null) return;
        var l2QNotNull = l2Quote != null;
        var bkNotNull  = orderBook != null;
        var isBid      = orderBookSide.BookSide == BookSide.BidBook;
        var sidedDepth = isBid ? PQDepthKey.None : PQDepthKey.AskSide;

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(orderBookSide.IsDailyTickUpdateCountUpdated);
        Assert.IsFalse(orderBookSide.HasUpdates);
        orderBookSide.DailyTickUpdateCount = 12;
        Assert.IsTrue(orderBookSide.HasUpdates);
        orderBookSide.UpdateComplete();
        orderBookSide.DailyTickUpdateCount          = 0;
        orderBookSide.IsDailyTickUpdateCountUpdated = false;
        orderBookSide.HasUpdates                    = false;

        Assert.AreEqual(0, orderBookSide.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedDailyTickCount = 128u;
        orderBookSide.DailyTickUpdateCount = expectedDailyTickCount;
        Assert.IsTrue(orderBookSide.HasUpdates);
        Assert.AreEqual(expectedDailyTickCount, orderBookSide.DailyTickUpdateCount);
        Assert.IsTrue(orderBookSide.IsDailyTickUpdateCountUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        var layerUpdates = orderBookSide
                           .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var expectedUpdate
            = new PQFieldUpdate(PQFeedFields.QuoteDailySidedTickCount, expectedDailyTickCount);
        var expectedOrderBook = expectedUpdate.WithDepth(sidedDepth);
        Assert.AreEqual(expectedUpdate, layerUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        orderBookSide.IsDailyTickUpdateCountUpdated = false;
        Assert.IsFalse(orderBookSide.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(orderBookSide.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteDailySidedTickCount && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundBkSide = isBid ? newEmpty.OrderBook.BidSide : newEmpty.OrderBook.AskSide;
            Assert.AreEqual(expectedDailyTickCount, foundBkSide.DailyTickUpdateCount);
            Assert.IsTrue(foundBkSide.IsDailyTickUpdateCountUpdated);
            Assert.IsTrue(foundBkSide.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteDailySidedTickCount && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundBkSide = isBid ? newEmpty.BidSide : newEmpty.AskSide;
            Assert.AreEqual(expectedDailyTickCount, foundBkSide.DailyTickUpdateCount);
            Assert.IsTrue(foundBkSide.IsDailyTickUpdateCountUpdated);
            Assert.IsTrue(foundBkSide.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        layerUpdates =
            (from update in orderBookSide.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update.Id == PQFeedFields.QuoteDailySidedTickCount
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedUpdate, layerUpdates[0]);

        var newPQOrderBook = new PQOrderBookSide();
        newPQOrderBook.UpdateField(layerUpdates[0]);
        Assert.AreEqual(expectedDailyTickCount, newPQOrderBook.DailyTickUpdateCount);
        Assert.IsTrue(newPQOrderBook.IsDailyTickUpdateCountUpdated);
        Assert.IsTrue(newPQOrderBook.HasUpdates);

        orderBookSide.DailyTickUpdateCount = 0u;
        orderBookSide.HasUpdates           = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedOrderBook_Count_UpdatesWhenPricesChanged()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            for (var i = MaxNumberOfLayers - 1; i >= 0; i--)
            {
                Assert.AreEqual(i, populatedOrderBook.Count - 1);
                populatedOrderBook[i]?.StateReset();
            }

            Assert.AreEqual(0, populatedOrderBook.Count);
        }
    }

    [TestMethod]
    public void PopulatedOrderBookClearHasUpdates_HasUpdates_ChangeItemAtATimeReportsUpdates()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.IsTrue(populatedOrderBook.HasUpdates);
            populatedOrderBook.HasUpdates = false;
            Assert.IsFalse(populatedOrderBook.HasUpdates);
            foreach (var pvl in populatedOrderBook)
            {
                pvl.Price = 3.456789m;
                Assert.IsTrue(populatedOrderBook.HasUpdates);
                pvl.IsPriceUpdated = false;
                Assert.IsFalse(populatedOrderBook.HasUpdates);
                pvl.Volume = 2_345_678m;
                Assert.IsTrue(populatedOrderBook.HasUpdates);
                pvl.IsVolumeUpdated = false;
                Assert.IsFalse(populatedOrderBook.HasUpdates);
                if (pvl is IPQSourcePriceVolumeLayer srcPvl)
                {
                    srcPvl.SourceName = "newSourceName";
                    Assert.IsTrue(populatedOrderBook.HasUpdates);
                    srcPvl.IsSourceNameUpdated     = false;
                    srcPvl.NameIdLookup.HasUpdates = false;
                    Assert.IsFalse(populatedOrderBook.HasUpdates);
                }

                if (pvl is IPQSourceQuoteRefPriceVolumeLayer srcQtRefPvl)
                {
                    srcQtRefPvl.SourceQuoteReference = 98_765_421u;
                    Assert.IsTrue(populatedOrderBook.HasUpdates);
                    srcQtRefPvl.IsSourceQuoteReferenceUpdated = false;
                    Assert.IsFalse(populatedOrderBook.HasUpdates);
                }

                if (pvl is IPQValueDatePriceVolumeLayer valueDatePvl)
                {
                    valueDatePvl.ValueDate = new DateTime(2017, 12, 10, 0, 0, 0);
                    Assert.IsTrue(populatedOrderBook.HasUpdates);
                    valueDatePvl.IsValueDateUpdated = false;
                    Assert.IsFalse(populatedOrderBook.HasUpdates);
                }

                if (pvl is IPQOrdersCountPriceVolumeLayer ordersCountPvl and not IPQOrdersPriceVolumeLayer)
                {
                    ordersCountPvl.OrdersCount = 5;
                    Assert.IsTrue(populatedOrderBook.HasUpdates);
                    ordersCountPvl.IsOrdersCountUpdated = false;
                    Assert.IsFalse(populatedOrderBook.HasUpdates);
                    ordersCountPvl.InternalVolume = 2_000;
                    Assert.IsTrue(populatedOrderBook.HasUpdates);
                    ordersCountPvl.IsInternalVolumeUpdated = false;
                    Assert.IsFalse(populatedOrderBook.HasUpdates);
                }

                if (pvl is IPQOrdersPriceVolumeLayer ordersPvl)
                    for (var i = 0; i < ExpectedOrdersCount; i++)
                    {
                        var orderLayerInfo = ordersPvl[i]!;

                        orderLayerInfo.OrderId = 321;
                        Assert.IsTrue(populatedOrderBook.HasUpdates);
                        orderLayerInfo.IsOrderIdUpdated = false;
                        Assert.IsFalse(populatedOrderBook.HasUpdates);

                        orderLayerInfo.GenesisFlags = OrderGenesisFlags.IsInternal | OrderGenesisFlags.IsSynthetic;
                        Assert.IsTrue(populatedOrderBook.HasUpdates);
                        orderLayerInfo.IsGenesisFlagsUpdated = false;
                        Assert.IsFalse(populatedOrderBook.HasUpdates);

                        orderLayerInfo.CreatedTime = new DateTime(2025, 4, 25, 19, 18, 23);
                        Assert.IsTrue(populatedOrderBook.HasUpdates);
                        orderLayerInfo.IsCreatedTimeDateUpdated    = false;
                        orderLayerInfo.IsCreatedTimeSub2MinUpdated = false;
                        Assert.IsFalse(populatedOrderBook.HasUpdates);

                        orderLayerInfo.UpdateTime = new DateTime(2025, 4, 25, 19, 18, 23);
                        Assert.IsTrue(populatedOrderBook.HasUpdates);
                        orderLayerInfo.IsUpdateTimeDateUpdated    = false;
                        orderLayerInfo.IsUpdateTimeSub2MinUpdated = false;
                        Assert.IsFalse(populatedOrderBook.HasUpdates);

                        orderLayerInfo.OrderDisplayVolume = 3_000m;
                        Assert.IsTrue(populatedOrderBook.HasUpdates);
                        orderLayerInfo.IsOrderVolumeUpdated = false;
                        Assert.IsFalse(populatedOrderBook.HasUpdates);

                        orderLayerInfo.OrderRemainingVolume = 7_777m;
                        Assert.IsTrue(populatedOrderBook.HasUpdates);
                        orderLayerInfo.IsOrderRemainingVolumeUpdated = false;
                        Assert.IsFalse(populatedOrderBook.HasUpdates);

                        if (orderLayerInfo is IPQExternalCounterPartyOrder pqCounterPartyOrder)
                        {
                            pqCounterPartyOrder.ExternalCounterPartyName = "NewCounterPartyName_" + i;
                            Assert.IsTrue(pqCounterPartyOrder.HasUpdates);
                            pqCounterPartyOrder.IsExternalCounterPartyNameUpdated = false;

                            pqCounterPartyOrder.ExternalTraderName = "NewTraderName_" + i;
                            Assert.IsTrue(pqCounterPartyOrder.HasUpdates);
                            pqCounterPartyOrder.IsExternalTraderNameUpdated = false;

                            pqCounterPartyOrder.NameIdLookup.HasUpdates = false;
                            Assert.IsFalse(pqCounterPartyOrder.HasUpdates);
                        }
                    }
            }
        }
    }

    [TestMethod]
    public void PopulatedOrderBook_Reset_ResetsAllLayers()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Count);
            foreach (var pvl in populatedOrderBook) Assert.IsFalse(pvl.IsEmpty);
            populatedOrderBook.StateReset();
            Assert.AreEqual(0, populatedOrderBook.Count);
            foreach (var pvl in populatedOrderBook) Assert.IsTrue(pvl.IsEmpty);
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllOrderBookFields()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var pqFieldUpdates =
                populatedOrderBook.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Update, publicationPrecisionSettings).ToList();
            AssertContainsAllOrderBookFields(publicationPrecisionSettings, pqFieldUpdates, populatedOrderBook);
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllOrderBookFields()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            populatedOrderBook.HasUpdates = false;
            var pqFieldUpdates =
                populatedOrderBook.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Snapshot, publicationPrecisionSettings).ToList();
            AssertContainsAllOrderBookFields(publicationPrecisionSettings, pqFieldUpdates, populatedOrderBook);
        }
    }

    [TestMethod]
    public void PopulatedOrderBookWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            populatedOrderBook.HasUpdates = false;
            var pqFieldUpdates =
                populatedOrderBook.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            var pqStringUpdates =
                populatedOrderBook.GetStringUpdates
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            Assert.AreEqual(0, pqStringUpdates.Count);
        }
    }

    [TestMethod]
    public void PopulatedOrderBook_GetDeltaUpdatesUpdateFieldNewOrderBook_CopiesAllFieldsToNewOrderBook()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var pqFieldUpdates =
                populatedOrderBook.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var pqStringUpdates =
                populatedOrderBook.GetStringUpdates
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var newEmpty = CreateNewEmpty(populatedOrderBook);
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
            foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
            Assert.AreEqual(populatedOrderBook, newEmpty);
        }
    }

    [TestMethod]
    public void FullyOrderBook_CopyFromToEmptyQuote_OrderBooksEqualEachOther()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var newEmpty = CreateNewEmpty(populatedOrderBook);
            newEmpty.CopyFrom(populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromLessLayers_ReplicatesMissingValues()
    {
        var clonePopulated = simpleFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[^1] = null;
        clonePopulated[^1] = null;
        clonePopulated[^1] = null;
        Assert.AreEqual(MaxNumberOfLayers - 3, clonePopulated.Count);
        var notEmpty = new PQOrderBookSide(simpleFullyPopulatedOrderBookSide);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfLayers - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGap()
    {
        var clonePopulated = simpleFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[^1] = null;
        clonePopulated[^1] = null;
        clonePopulated[5]  = null;
        Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
        var notEmpty = new PQOrderBookSide(simpleFullyPopulatedOrderBookSide);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], new PQPriceVolumeLayer() { IsPriceUpdated = ExpectedExecutable, IsVolumeUpdated = ExpectedExecutable });
        Assert.AreEqual(MaxNumberOfLayers - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[^1] = null;
        clonePopulated[^1] = null;
        Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
        var notEmpty = new PQOrderBookSide(simpleFullyPopulatedOrderBookSide) { [5] = null };
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfLayers - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyOrderBook_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var newEmpty = CreateNewEmpty(populatedOrderBook);
            populatedOrderBook.HasUpdates = false;
            newEmpty.CopyFrom(populatedOrderBook);
            foreach (var pvl in newEmpty)
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
        }
    }

    [TestMethod]
    public void NonPQOrderBook_CopyFromToEmptyOrderBook_OrderBooksEquivalentToEachOther()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var nonPQOrderBook = new OrderBookSide(populatedOrderBook);
            var newEmpty       = CreateNewEmpty(populatedOrderBook);
            newEmpty.CopyFrom(nonPQOrderBook);
            Assert.AreEqual(populatedOrderBook, newEmpty);
        }
    }

    [TestMethod]
    public void ForEachOrderBookType_EmptyOrderBookCopyFromAnotherOrderBookType_UpgradesToEverythingOrderBookItems()
    {
        foreach (var originalTypeOrderBook in allPopulatedOrderBooks)
        foreach (var otherOrderBook in allPopulatedOrderBooks
                     .Where(ob => !ReferenceEquals(ob, originalTypeOrderBook)))
        {
            var emptyOriginalTypeOrderBook = CreateNewEmpty(originalTypeOrderBook);
            AssertAllLayersAreOfTypeAndEquivalentTo
                (emptyOriginalTypeOrderBook, originalTypeOrderBook
               , originalTypeOrderBook[0]!.GetType(), false);
            emptyOriginalTypeOrderBook.CopyFrom(otherOrderBook);
            AssertAllLayersAreOfTypeAndEquivalentTo
                (emptyOriginalTypeOrderBook, otherOrderBook
               , GetExpectedType(originalTypeOrderBook[0]!.LayerType, otherOrderBook[0]!.LayerType));
        }
    }

    [TestMethod]
    public void ForEachOrderBookType_PopulatedCopyFromAnotherOrderBookType_UpgradesToEverythingOrderBookItems()
    {
        foreach (var originalTypeOrderBook in allPopulatedOrderBooks)
        foreach (var otherOrderBook in allPopulatedOrderBooks
                     .Where(ob => !ReferenceEquals(ob, originalTypeOrderBook)))
        {
            var clonedPopulatedOrderBook = originalTypeOrderBook.Clone();
            AssertAllLayersAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, originalTypeOrderBook,
                                                    originalTypeOrderBook[0]!.GetType(), false);
            clonedPopulatedOrderBook.CopyFrom(otherOrderBook);
            AssertAllLayersAreOfTypeAndEquivalentTo
                (clonedPopulatedOrderBook, otherOrderBook
               , GetExpectedType(originalTypeOrderBook[0]!.LayerType, otherOrderBook[0]!.LayerType));
            AssertAllLayersAreOfTypeAndEquivalentTo
                (clonedPopulatedOrderBook, originalTypeOrderBook
               , GetExpectedType(originalTypeOrderBook[0]!.LayerType, otherOrderBook[0]!.LayerType));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var clonedOrderBook = ((ICloneable<IOrderBookSide>)populatedOrderBook).Clone();
            Assert.AreNotSame(clonedOrderBook, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clonedOrderBook);

            var cloned2 = (IPQOrderBookSide)((ICloneable)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned2, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned2);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var fullyPopulatedClone = (IPQOrderBookSide)((ICloneable)populatedOrderBook).Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType(ExpectedExecutable, populatedOrderBook,
                                                                fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedOrderBook,
                                                                fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBookSameObj_Equals_ReturnsTrue()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.AreEqual(populatedOrderBook, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, ((ICloneable)populatedOrderBook).Clone());
            Assert.AreEqual(populatedOrderBook, ((ICloneable<IOrderBookSide>)populatedOrderBook).Clone());
            Assert.AreEqual(populatedOrderBook, ((ICloneable<IMutableOrderBookSide>)populatedOrderBook).Clone());
            Assert.AreEqual(populatedOrderBook, ((ICloneable<IPQOrderBookSide>)populatedOrderBook).Clone());
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_GetHashCode_ReturnNumberNoException()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var hashCode = populatedOrderBook.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedQuote in allPopulatedOrderBooks)
        {
            var q = populatedQuote;

            var toString = q.ToString()!;

            Assert.IsTrue(toString.Contains(q.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(q.Capacity)}: {q.Capacity}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
            Assert.IsTrue(toString.Contains($"AllLayers:[" +
                                            $"{string.Join(", ", (IEnumerable<IPQPriceVolumeLayer>)q)}]"));
        }
    }

    [TestMethod]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = allFieldsFullyPopulatedOrderBookSide;
        Assert.AreEqual(MaxNumberOfLayers, rt.Count);
        Assert.AreEqual(MaxNumberOfLayers, ((IEnumerable<IPQPriceVolumeLayer>)rt).Count());
        Assert.AreEqual(MaxNumberOfLayers, ((IEnumerable<IPriceVolumeLayer>)rt).Count());
        Assert.AreEqual(MaxNumberOfLayers, rt.OfType<IPriceVolumeLayer>().Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable<IPQPriceVolumeLayer>)rt).Count());
        Assert.AreEqual(0, ((IEnumerable<IPriceVolumeLayer>)rt).Count());
        Assert.AreEqual(0, rt.OfType<IPriceVolumeLayer>().Count());
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQOrderBookSide original,
        IPQOrderBookSide changingOrderBookSide,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        IPQPublishableLevel2Quote? originalQuote = null
      , IPQPublishableLevel2Quote? changingQuote = null)
    {
        if (original.GetType() == typeof(PQOrderBookSide))
            Assert.AreEqual
                (!exactComparison, changingOrderBookSide.AreEquivalent(new OrderBookSide(original), exactComparison));

        Assert.AreEqual(original.Count, changingOrderBookSide.Count);

        var originalLayers = original.AllLayers;
        var changingLayers = changingOrderBookSide.AllLayers;

        for (var i = 0; i < original.Count; i++)
        {
            PQPriceVolumeLayerTests
                .AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, (PQPriceVolumeLayer)originalLayers[i],
                     (PQPriceVolumeLayer)changingLayers[i], original,
                     changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote!, changingQuote!);
            if (originalLayers[i] is PQSourcePriceVolumeLayer)
                PQSourcePriceVolumeLayerTests
                    .AssertAreEquivalentMeetsExpectedExactComparisonType
                        (exactComparison, (PQSourcePriceVolumeLayer)originalLayers[i],
                         (PQSourcePriceVolumeLayer)changingLayers[i], original,
                         changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote!, changingQuote!);
            if (originalLayers[i] is PQSourceQuoteRefPriceVolumeLayer)
                PQSourceQuoteRefPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                 exactComparison, (PQSourceQuoteRefPriceVolumeLayer)originalLayers[i],
                 (PQSourceQuoteRefPriceVolumeLayer)changingLayers[i], original,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote!, changingQuote!);
            if (originalLayers[i] is PQValueDatePriceVolumeLayer)
                PQValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                 exactComparison, (PQValueDatePriceVolumeLayer)originalLayers[i],
                 (PQValueDatePriceVolumeLayer)changingLayers[i], original,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote!, changingQuote);
            if (originalLayers[i] is PQOrdersPriceVolumeLayer)
                PQOrdersPriceVolumeLayerTests
                    .AssertAreEquivalentMeetsExpectedExactComparisonType
                        (exactComparison, (PQOrdersPriceVolumeLayer)originalLayers[i],
                         (PQOrdersPriceVolumeLayer)changingLayers[i], original,
                         changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);
            if (originalLayers[i] is PQFullSupportPriceVolumeLayer)
                PQFullSupportPriceVolumeLayerTests
                    .AssertAreEquivalentMeetsExpectedExactComparisonType
                        (exactComparison, (PQFullSupportPriceVolumeLayer)originalLayers[i],
                         (PQFullSupportPriceVolumeLayer)changingLayers[i], original
                       , changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);
        }
    }

    public static void AssertContainsAllOrderBookFields
        (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates, IPQOrderBookSide orderBookSide)
    {
        var priceScale  = precisionSettings.PriceScalingPrecision;
        var volumeScale = precisionSettings.VolumeScalingPrecision;
        for (var i = 0; i < MaxNumberOfLayers; i++)
        {
            var pvl = orderBookSide[i];

            var depthId = (PQDepthKey)i | (orderBookSide.BookSide == BookSide.AskBook ? PQDepthKey.AskSide : PQDepthKey.None);
            Assert.AreEqual
                (new PQFieldUpdate
                     (PQFeedFields.QuoteLayerPrice, depthId, pvl!.Price, priceScale),
                 PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerPrice, depthId, priceScale),
                 $"For {orderBookSide.BookSide}  {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            Assert.AreEqual
                (new PQFieldUpdate
                     (PQFeedFields.QuoteLayerVolume, depthId, PQScaling.Scale(pvl.Volume, volumeScale), volumeScale),
                 PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerVolume, depthId, volumeScale),
                 $"For {orderBookSide.BookSide}  {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (pvl is IPQSourcePriceVolumeLayer srcPvl)
            {
                var srcId = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerSourceId, depthId);

                var nameIdLookup = srcPvl.NameIdLookup;

                Assert.AreEqual
                    (new PQFieldUpdate
                         (PQFeedFields.QuoteLayerSourceId, depthId, (uint)nameIdLookup[srcPvl.SourceName!]), srcId,
                     $"For {orderBookSide.BookSide}  {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }

            if (pvl is IPQSourceQuoteRefPriceVolumeLayer srcQtRefPvl)
            {
                var srcQtRef = PQTickInstantTests.ExtractFieldUpdateWithId
                    (checkFieldUpdates, PQFeedFields.QuoteLayerSourceQuoteRef, depthId);

                Assert.AreEqual(new PQFieldUpdate
                                    (PQFeedFields.QuoteLayerSourceQuoteRef, depthId, srcQtRefPvl.SourceQuoteReference), srcQtRef);
            }

            if (pvl is IPQValueDatePriceVolumeLayer valueDatePvl)
            {
                var valueDate = PQTickInstantTests.ExtractFieldUpdateWithId
                    (checkFieldUpdates, PQFeedFields.QuoteLayerValueDate, depthId);

                var dateAsHoursFromEpoch = valueDatePvl.ValueDate.Get2MinIntervalsFromUnixEpoch();

                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, depthId, dateAsHoursFromEpoch), valueDate);
            }

            if (pvl is IPQOrdersPriceVolumeLayer bidTrdPvl)
            {
                AssertOrderLayerInfoIsExpected(checkFieldUpdates, bidTrdPvl, i, priceScale, volumeScale);
            }
        }
    }

    private Type GetExpectedType(LayerType originalType, LayerType copyType)
    {
        return originalType switch
               {
                   LayerType.PriceVolume =>
                       copyType switch
                       {
                           LayerType.PriceVolume                => typeof(PQPriceVolumeLayer)
                         , LayerType.SourcePriceVolume          => typeof(PQSourcePriceVolumeLayer)
                         , LayerType.SourceQuoteRefPriceVolume  => typeof(PQSourceQuoteRefPriceVolumeLayer)
                         , LayerType.OrdersCountPriceVolume     => typeof(PQOrdersCountPriceVolumeLayer)
                         , LayerType.OrdersAnonymousPriceVolume => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.OrdersFullPriceVolume      => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.ValueDatePriceVolume       => typeof(PQValueDatePriceVolumeLayer)
                         , LayerType.FullSupportPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , _ => throw new ArgumentException
                               ($"Expected originalType to be on of " +
                                $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                                $" but was '{originalType}'")
                       }
                 , LayerType.SourcePriceVolume =>
                       copyType switch
                       {
                           LayerType.PriceVolume                => typeof(PQSourcePriceVolumeLayer)
                         , LayerType.SourcePriceVolume          => typeof(PQSourcePriceVolumeLayer)
                         , LayerType.SourceQuoteRefPriceVolume  => typeof(PQSourceQuoteRefPriceVolumeLayer)
                         , LayerType.OrdersCountPriceVolume     => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersAnonymousPriceVolume => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersFullPriceVolume      => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.ValueDatePriceVolume       => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.FullSupportPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , _ => throw new ArgumentException
                               ($"Expected originalType to be on of " +
                                $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                                $" but was '{originalType}'")
                       }
                 , LayerType.SourceQuoteRefPriceVolume =>
                       copyType switch
                       {
                           LayerType.PriceVolume                => typeof(PQSourceQuoteRefPriceVolumeLayer)
                         , LayerType.SourcePriceVolume          => typeof(PQSourceQuoteRefPriceVolumeLayer)
                         , LayerType.SourceQuoteRefPriceVolume  => typeof(PQSourceQuoteRefPriceVolumeLayer)
                         , LayerType.OrdersCountPriceVolume     => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersAnonymousPriceVolume => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersFullPriceVolume      => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.ValueDatePriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.FullSupportPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , _ => throw new ArgumentException
                               ($"Expected originalType to be on of " +
                                $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                                $" but was '{originalType}'")
                       }
                 , LayerType.ValueDatePriceVolume =>
                       copyType switch
                       {
                           LayerType.PriceVolume       => typeof(PQValueDatePriceVolumeLayer)
                         , LayerType.SourcePriceVolume => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.SourceQuoteRefPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersCountPriceVolume     => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersAnonymousPriceVolume => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersFullPriceVolume      => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.ValueDatePriceVolume       => typeof(PQValueDatePriceVolumeLayer)
                         , LayerType.FullSupportPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , _ => throw new ArgumentException
                               ($"Expected originalType to be on of " +
                                $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                                $" but was '{originalType}'")
                       }
                 , LayerType.OrdersCountPriceVolume =>
                       copyType switch
                       {
                           LayerType.PriceVolume                => typeof(PQOrdersCountPriceVolumeLayer)
                         , LayerType.SourcePriceVolume          => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.SourceQuoteRefPriceVolume  => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersCountPriceVolume     => typeof(PQOrdersCountPriceVolumeLayer)
                         , LayerType.OrdersAnonymousPriceVolume => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.OrdersFullPriceVolume      => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.ValueDatePriceVolume       => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.FullSupportPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , _ => throw new ArgumentException
                               ($"Expected originalType to be on of " +
                                $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                                $" but was '{originalType}'")
                       }
                 , LayerType.OrdersAnonymousPriceVolume =>
                       copyType switch
                       {
                           LayerType.PriceVolume                => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.SourcePriceVolume          => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.SourceQuoteRefPriceVolume  => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersCountPriceVolume     => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.OrdersAnonymousPriceVolume => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.OrdersFullPriceVolume      => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.ValueDatePriceVolume       => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.FullSupportPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , _ => throw new ArgumentException
                               ($"Expected originalType to be on of " +
                                $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                                $" but was '{originalType}'")
                       }
                 , LayerType.OrdersFullPriceVolume =>
                       copyType switch
                       {
                           LayerType.PriceVolume                => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.SourcePriceVolume          => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.SourceQuoteRefPriceVolume  => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersCountPriceVolume     => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.OrdersAnonymousPriceVolume => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.OrdersFullPriceVolume      => typeof(PQOrdersPriceVolumeLayer)
                         , LayerType.ValueDatePriceVolume       => typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.FullSupportPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , _ => throw new ArgumentException
                               ($"Expected originalType to be on of " +
                                $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                                $" but was '{originalType}'")
                       }
                 , LayerType.FullSupportPriceVolume =>
                       copyType switch
                       {
                           LayerType.PriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.SourcePriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.SourceQuoteRefPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersCountPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersAnonymousPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.OrdersFullPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.ValueDatePriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , LayerType.FullSupportPriceVolume =>
                               typeof(PQFullSupportPriceVolumeLayer)
                         , _ => throw new ArgumentException
                               ($"Expected originalType to be on of " +
                                $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                                $" but was '{originalType}'")
                       }
                 , _ => throw new ArgumentException
                       ($"Expected originalType to be on of " +
                        $"[{Enum.GetValues<LayerType>().Where(lt => lt != LayerType.None).JoinToString()}]" +
                        $" but was '{originalType}'")
               };
    }

    private void AssertAllLayersAreOfTypeAndEquivalentTo
    (IPQOrderBookSide upgradedOrderBookSide, IPQOrderBookSide equivalentTo, Type expectedType
      , bool compareForEquivalence = ExpectedExecutable, bool exactlyEquals = false)
    {
        for (var i = 0; i < upgradedOrderBookSide.Capacity; i++)
        {
            var upgradedLayer = upgradedOrderBookSide[i];

            var copyFromLayer = equivalentTo[i];

            Assert.IsInstanceOfType(upgradedLayer, expectedType);
            if (compareForEquivalence)
                Assert.IsTrue(copyFromLayer!.AreEquivalent(upgradedLayer, exactlyEquals),
                              $"Expected {copyFromLayer} to be equivalent to {upgradedLayer} when exactlyEquals {exactlyEquals}");
        }
    }

    private static IPQOrderBookSide CreateNewEmpty(IPQOrderBookSide populatedOrderBookSide)
    {
        var cloneGenesis = populatedOrderBookSide[0]!.Clone();
        cloneGenesis.StateReset();
        var clonedEmptyLayers = new List<IPQPriceVolumeLayer>(MaxNumberOfLayers);
        for (var i = 0; i < MaxNumberOfLayers; i++) clonedEmptyLayers.Add(cloneGenesis.Clone());
        var newEmpty = new PQOrderBookSide(populatedOrderBookSide.BookSide, clonedEmptyLayers,
                                           populatedOrderBookSide.IsLadder, populatedOrderBookSide.NameIdLookup.Clone());
        return newEmpty;
    }

    private void AssertBookHasLayersOfType(IPQOrderBookSide orderBookSide, Type expectedType)
    {
        for (var i = 0; i < MaxNumberOfLayers; i++) Assert.IsInstanceOfType(orderBookSide[i], expectedType);
    }

    private static void AssertOrderLayerInfoIsExpected
    (IList<PQFieldUpdate> checkFieldUpdates,
        IPQOrdersPriceVolumeLayer ordersPvl, int bookIndex = 0,
        PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        PQOrdersPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, ordersPvl, bookIndex, priceScale, volumeScale);
    }
}
