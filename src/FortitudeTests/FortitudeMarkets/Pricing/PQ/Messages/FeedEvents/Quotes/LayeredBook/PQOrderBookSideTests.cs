// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
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
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;

[TestClass]
public class PQOrderBookSideTests
{
    private const int MaxNumberOfLayers = 12; // test being less than max.

    private const decimal ExpectedPrice          = 1.234567m;
    private const decimal ExpectedVolume         = 40_000_000m;
    private const string  ExpectedSourceName     = "TestSourceName";
    private const bool    ExpectedExecutable     = true;
    private const uint    ExpectedSourceQuoteRef = 12345678u;
    private const int     ExpectedOrdersCount    = 3; // not too many traders.
    private const decimal ExpectedInternalVolume = 20_000_000m;

    private const OrderGenesisFlags ExpectedGenesisFlags
        = OrderGenesisFlags.FromAdapter | OrderGenesisFlags.IsExternalOrder | OrderGenesisFlags.HasExternalCounterPartyInfo;
    private const OrderType ExpectedOrderType = OrderType.PassiveLimit;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.ConfirmedActiveOnMarket;

    private const uint ExpectedTrackingId = 12467u;

    private const int     ExpectedOrderId              = 250;
    private const decimal ExpectedOrderVolume          = 50.50m;
    private const decimal ExpectedOrderRemainingVolume = 10.25m;
    private const string  ExpectedCounterPartyBase     = "TestCounterPartyName_";
    private const string  ExpectedTraderNameBase       = "TestTraderName_";

    private const MarketDataSource ExpectedDataSource = MarketDataSource.Venue;

    private const decimal ExpectedOpenInterestVolume = ExpectedOrderVolume * 100;
    private const decimal ExpectedOpenInterestVwap   = ExpectedPrice * 2m;

    private const uint ExpectedDailyTickCount = 2_582;

    private static readonly PQMarketAggregate ExpectedSidedOpenInterest =
        new(ExpectedDataSource, ExpectedOpenInterestVolume, ExpectedOpenInterestVwap, new(2025, 5, 8, 12, 8, 59));

    private static readonly DateTime ExpectedValueDate        = new(2017, 12, 09, 14, 0, 0);
    private static readonly DateTime ExpectedOrderCreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime ExpectedOrderUpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private List<IReadOnlyList<IPQPriceVolumeLayer?>> allPopulatedLayers     = null!;
    private List<IPQOrderBookSide>                    allPopulatedOrderBooks = null!;

    private IList<IPQPriceVolumeLayer>               simpleLayers             = null!;
    private IList<IPQSourcePriceVolumeLayer>         sourceLayers             = null!;
    private IList<IPQSourceQuoteRefPriceVolumeLayer> sourceQtRefLayers        = null!;
    private IList<IPQValueDatePriceVolumeLayer>      valueDateLayers          = null!;
    private IList<IPQOrdersCountPriceVolumeLayer>    ordersCountLayers        = null!;
    private IList<IPQOrdersPriceVolumeLayer>         ordersAnonLayers         = null!;
    private IList<IPQOrdersPriceVolumeLayer>         ordersCounterPartyLayers = null!;
    private IList<IPQFullSupportPriceVolumeLayer>    fullSupportLayers        = null!;

    private IPQOrderBookSide simpleFullyPopulatedOrderBookSide             = null!;
    private IPQOrderBookSide sourceFullyPopulatedOrderBookSide             = null!;
    private IPQOrderBookSide sourceQtRefFullyPopulatedOrderBookSide        = null!;
    private IPQOrderBookSide valueDateFullyPopulatedOrderBookSide          = null!;
    private IPQOrderBookSide ordersCountFullyPopulatedOrderBookSide        = null!;
    private IPQOrderBookSide ordersAnonFullyPopulatedOrderBookSide         = null!;
    private IPQOrderBookSide ordersCounterPartyFullyPopulatedOrderBookSide = null!;
    private IPQOrderBookSide fullSupportFullyPopulatedOrderBookSide        = null!;

    private PQSourceTickerInfo      publicationPrecisionSettings = null!;
    private PQNameIdLookupGenerator nameIdLookupGenerator        = null!;

    private static DateTime testDateTime = new(2025, 5, 7, 18, 33, 24);

    private readonly PQSourceTickerInfo forGetDeltaUpdates = PQSourceTickerInfoTests.FullSupportL2PriceVolumeSti;

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
        fullSupportLayers = new List<IPQFullSupportPriceVolumeLayer>(MaxNumberOfLayers);

        // placed in the same order as the orderBooks at the end of Setup
        allPopulatedLayers =
        [
            (IReadOnlyList<IPQPriceVolumeLayer>)simpleLayers, (IReadOnlyList<IPQPriceVolumeLayer>)sourceLayers
          , (IReadOnlyList<IPQPriceVolumeLayer>)sourceQtRefLayers, (IReadOnlyList<IPQPriceVolumeLayer>)valueDateLayers
          , (IReadOnlyList<IPQPriceVolumeLayer>)ordersCountLayers, (IReadOnlyList<IPQPriceVolumeLayer>)ordersAnonLayers
          , (IReadOnlyList<IPQPriceVolumeLayer>)ordersCounterPartyLayers, (IReadOnlyList<IPQPriceVolumeLayer>)fullSupportLayers
        ];

        for (var i = 0; i < MaxNumberOfLayers; i++)
        {
            simpleLayers.Add(new PQPriceVolumeLayer(ExpectedPrice + (0.0001m * i), ExpectedVolume));

            var sourcePvl = new PQSourcePriceVolumeLayer
                (nameIdLookupGenerator, ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedSourceName, ExpectedExecutable);
            sourceLayers.Add(sourcePvl);

            var srcQtRefPvl = new PQSourceQuoteRefPriceVolumeLayer
                (nameIdLookupGenerator, ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedSourceName, ExpectedExecutable
               , ExpectedSourceQuoteRef);
            sourceQtRefLayers.Add(srcQtRefPvl);

            valueDateLayers.Add(new PQValueDatePriceVolumeLayer(ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedValueDate));
            var ordersCountPvl
                = new PQOrdersCountPriceVolumeLayer(ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
            ordersCountLayers.Add(ordersCountPvl);
            var anonOrdersPvL = new PQOrdersPriceVolumeLayer
                (nameIdLookupGenerator, LayerType.OrdersAnonymousPriceVolume, ExpectedPrice + (0.0001m * i)
               , ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
            ordersAnonLayers.Add(anonOrdersPvL);
            var counterPartyOrdersPvL = new PQOrdersPriceVolumeLayer
                (nameIdLookupGenerator, LayerType.OrdersFullPriceVolume, ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedOrdersCount
               , ExpectedInternalVolume);
            ordersCounterPartyLayers.Add(counterPartyOrdersPvL);
            var fullSupportPvL = new PQFullSupportPriceVolumeLayer
                (nameIdLookupGenerator, ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedValueDate,
                 ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef, ExpectedOrdersCount, ExpectedInternalVolume);
            fullSupportLayers.Add(fullSupportPvL);
            for (var j = 0; j < ExpectedOrdersCount; j++)
            {
                fullSupportPvL.Add
                    (new PQExternalCounterPartyOrder
                        (new PQAnonymousOrder
                            (nameIdLookupGenerator, ExpectedOrderId + j, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType
                           , ExpectedGenesisFlags, ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, ExpectedTrackingId)
                            {
                                ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo
                                        (nameIdLookupGenerator, i + 1, ExpectedCounterPartyBase + i
                                       , i + 1, ExpectedTraderNameBase + i)
                            }));
                anonOrdersPvL.Add
                    (new PQAnonymousOrder
                        (nameIdLookupGenerator, ExpectedOrderId + j, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType
                       , ExpectedGenesisFlags, ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, ExpectedTrackingId));
                counterPartyOrdersPvL.Add
                    (new PQExternalCounterPartyOrder
                        (new PQAnonymousOrder
                            (nameIdLookupGenerator, ExpectedOrderId + j, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType
                           , ExpectedGenesisFlags , ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume
                           , ExpectedTrackingId)
                            {
                                ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo
                                        (nameIdLookupGenerator, i + 1, ExpectedCounterPartyBase + i
                                       , i + 1 , ExpectedTraderNameBase + i)
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
        valueDateFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, valueDateLayers)
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
        ordersCounterPartyFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, ordersCounterPartyLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        fullSupportFullyPopulatedOrderBookSide = new PQOrderBookSide(BookSide.BidBook, fullSupportLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };

        allPopulatedOrderBooks =
        [
            simpleFullyPopulatedOrderBookSide, sourceFullyPopulatedOrderBookSide, sourceQtRefFullyPopulatedOrderBookSide
          , valueDateFullyPopulatedOrderBookSide, ordersCountFullyPopulatedOrderBookSide, ordersAnonFullyPopulatedOrderBookSide
          , ordersCounterPartyFullyPopulatedOrderBookSide, fullSupportFullyPopulatedOrderBookSide
        ];
        // [
        //     ordersAnonFullyPopulatedOrderBookSide, ordersCounterPartyFullyPopulatedOrderBookSide
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
            for (var j = 0; j < MaxNumberOfLayers; j++)
            {
                Assert.AreSame(populatedLayers[j], populatedOrderBook[j]);
            }
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
                var clonedLayer = (IPQPriceVolumeLayer)layer.Clone();

                populatedOrderBook[i] = clonedLayer;
                Assert.AreNotSame(layer, ((IMutableOrderBookSide)populatedOrderBook)[i]);
                Assert.AreSame(clonedLayer, populatedOrderBook[i]);
                if (i == populatedOrderBook.AllLayers.Count - 1)
                {
                    populatedOrderBook[i] = populatedOrderBook[i].ResetWithTracking();
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
            populatedOrderBook[MaxNumberOfLayers - 1] = populatedOrderBook[MaxNumberOfLayers - 1].ResetWithTracking();
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Capacity);
            Assert.AreEqual(populatedOrderBook.AllLayers.Count + 1, populatedOrderBook.Capacity);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PopulatedOrderBook_CapacityLargerThanMaxBookDepth_ThrowsException()
    {
        simpleFullyPopulatedOrderBookSide.Capacity = PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth + 1;
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

        testDateTime = testDateTime.AddHours(2).AddMinutes(2);

        Assert.IsFalse(orderBookSide.IsDailyTickUpdateCountUpdated);
        Assert.IsFalse(orderBookSide.HasUpdates);
        orderBookSide.DailyTickUpdateCount = 12;
        Assert.IsTrue(orderBookSide.HasUpdates);
        orderBookSide.UpdateComplete();
        orderBookSide.DailyTickUpdateCount          = 0;
        orderBookSide.IsDailyTickUpdateCountUpdated = false;
        orderBookSide.HasUpdates                    = false;

        Assert.AreEqual(0, orderBookSide.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull)
        {
            var deltaUpdateFields = l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
            Assert.AreEqual(2, deltaUpdateFields.Count);
        }

        var expectedDailyTickCount = 128u;
        orderBookSide.DailyTickUpdateCount = expectedDailyTickCount;
        Assert.IsTrue(orderBookSide.HasUpdates);
        Assert.AreEqual(expectedDailyTickCount, orderBookSide.DailyTickUpdateCount);
        Assert.IsTrue(orderBookSide.IsDailyTickUpdateCountUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        var layerUpdates = orderBookSide
                           .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
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
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(orderBookSide.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
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
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
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
            (from update in orderBookSide.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
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
                populatedOrderBook[i].StateReset();
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
                        var orderLayerInfo = ordersPvl[i];

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
                    (new DateTime(2017, 11, 04, 12, 33, 1), PQMessageFlags.Update, publicationPrecisionSettings).ToList();
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
                    (new DateTime(2017, 11, 04, 12, 33, 1), PQMessageFlags.Snapshot, publicationPrecisionSettings).ToList();
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
                    (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
            var pqStringUpdates =
                populatedOrderBook.GetStringUpdates
                    (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
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
                   , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
            var pqStringUpdates =
                populatedOrderBook.GetStringUpdates
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
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
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
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
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[5]  = clonePopulated[5].ResetWithTracking();
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
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
        var notEmpty = new PQOrderBookSide(simpleFullyPopulatedOrderBookSide)
            { [5] = simpleFullyPopulatedOrderBookSide[5].Clone().ResetWithTracking() };
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
               , originalTypeOrderBook[0].GetType(), false);
            emptyOriginalTypeOrderBook.CopyFrom(otherOrderBook);
            AssertAllLayersAreOfTypeAndEquivalentTo
                (emptyOriginalTypeOrderBook, otherOrderBook
               , GetExpectedType(originalTypeOrderBook[0].LayerType, otherOrderBook[0].LayerType));
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
                                                    originalTypeOrderBook[0].GetType(), false);
            clonedPopulatedOrderBook.CopyFrom(otherOrderBook);
            AssertAllLayersAreOfTypeAndEquivalentTo
                (clonedPopulatedOrderBook, otherOrderBook
               , GetExpectedType(originalTypeOrderBook[0].LayerType, otherOrderBook[0].LayerType));
            AssertAllLayersAreOfTypeAndEquivalentTo
                (clonedPopulatedOrderBook, originalTypeOrderBook
               , GetExpectedType(originalTypeOrderBook[0].LayerType, otherOrderBook[0].LayerType));
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
            Assert.IsTrue(toString.Contains($"{nameof(q.AllLayers)}: [\n{q.EachLayerByIndexOnNewLines()}]"));
        }
    }

    [TestMethod]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = fullSupportFullyPopulatedOrderBookSide;
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

    [TestMethod]
    public void PopulatedOrderBookSide_SmallerToLargerCalculateShifts_ShiftRightCommandsExpected()
    {
        fullSupportFullyPopulatedOrderBookSide.HasUpdates = false;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        int[]               expectedIndices = [0, 2, 5, 9];
        IPriceVolumeLayer[] instances       = new IPriceVolumeLayer[10];

        int oldIndex    = 0;                   // original    0,1,2,3,4,5,6,7,8,9,10,11
        int actualIndex = 0;                   // deleted     1,3,4,6,7,8,10,11 
        var count       = toShift.Count;       // leaving     0,2,5,9
        for (var i = 0; oldIndex < count; i++) // shifts at   (2,3),(1,2)(0,1)
        {
            Console.Out.WriteLine($"Leaving index {oldIndex} with Price {toShift[actualIndex].Price}");
            instances[oldIndex] = toShift[actualIndex];
            oldIndex++;
            actualIndex++;
            for (var j = i + 1; j < 2 + 2 * i && oldIndex < count; j++)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with Price {toShift[actualIndex].Price}");
                toShift.RemoveAt(actualIndex);
                oldIndex++;
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportFullyPopulatedOrderBookSide.Clone();
        toShift.CalculateShift(testDateTime, shiftedNext);

        Assert.AreEqual(3, toShift.ShiftCommands.Count);
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < toShift.ShiftCommands.Count; i++)
            {
                var shift = toShift.ShiftCommands[i];
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(2, shift.PinnedFromIndex);
                        Assert.AreEqual(3, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(1, shift.PinnedFromIndex);
                        Assert.AreEqual(2, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(0, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in toShift.ShiftCommands)
        {
            toShift.ApplyListShiftCommand(shiftElementShift);
        }
        foreach (var expectedIndex in expectedIndices)
        {
            Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide[expectedIndex], toShift[expectedIndex]);
            Assert.AreSame(instances[expectedIndex], toShift[expectedIndex]);
        }
    }

    [TestMethod]
    public void PopulatedOrderBookSide_SmallerToLargerCalculateShiftsNewInsertInMiddle_ShiftRightCommandsExpected()
    {
        fullSupportFullyPopulatedOrderBookSide.HasUpdates = false;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        int[]               expectedIndices = [0, 2, 4, 5, 6, 8, 10];
        IPriceVolumeLayer[] instances       = new IPriceVolumeLayer[11];

        var counterPartyOrdersPvL = new PQFullSupportPriceVolumeLayer
            (nameIdLookupGenerator, ExpectedPrice + (0.0001m * 13), ExpectedVolume, ExpectedValueDate,
             ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef, ExpectedOrdersCount, ExpectedInternalVolume);


        int oldIndex    = 0;                   // original    0,1,2,3,4,5,6,7,8,9,10,11        
        int actualIndex = 0;                   // deleted     1,3,5,9,11         
        var count       = toShift.Count;       // leaving     0,2,4,{new},6,8,10        
        for (var i = 0; oldIndex < count; i++) // shifts at   (5,1),(4,1),(1,1),(0,1)
        {
            if (i % 2 == 1)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with Price {toShift[actualIndex].Price}");
                toShift.RemoveAt(actualIndex);
                oldIndex++;
            }
            else
            {
                Console.Out.WriteLine($"Leaving index {oldIndex} with Price {toShift[actualIndex].Price}");
                instances[oldIndex] = toShift[actualIndex];
                oldIndex++;
                actualIndex++;
                if (actualIndex == 3)
                {
                    Console.Out.WriteLine($"Inserting at index {oldIndex} with Price {counterPartyOrdersPvL.Price}");
                    toShift.InsertAt(actualIndex, counterPartyOrdersPvL);
                    instances[oldIndex] = counterPartyOrdersPvL;
                    actualIndex++;
                }
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportFullyPopulatedOrderBookSide.Clone();
        toShift.CalculateShift(testDateTime, shiftedNext);

        Console.Out.WriteLine($"{toShift.ShiftCommands.JoinShiftCommandsOnNewLine()}");

        Assert.AreEqual(4, toShift.ShiftCommands.Count);
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < toShift.ShiftCommands.Count; i++)
            {
                var shift = toShift.ShiftCommands[i];
                Console.Out.WriteLine($"shift: {shift}");
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(5, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(4, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(1, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 3:
                        Assert.AreEqual(0, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in toShift.ShiftCommands)
        {
            toShift.ApplyListShiftCommand(shiftElementShift);
        }
        for (int i = 0; i < expectedIndices.Length; i++)
        {
            if (i != 3) Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide[expectedIndices[i]], toShift[expectedIndices[i]]);
            Assert.AreSame(instances[expectedIndices[i]], toShift[expectedIndices[i]]);
        }
    }

    [TestMethod]
    public void PopulatedOrderBookSide_ClearEdgesSomeGapsRemainingWithNewEntries_ShiftLeftAndClearCommandsExpected()
    {
        fullSupportFullyPopulatedOrderBookSide.HasUpdates = false;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        IPriceVolumeLayer[] instances = new IPriceVolumeLayer[10];


        int[] leaveExisting             = [3, 6, 9];
        int[] existingNewLocation       = [1, 3, 4];
        int[] insertNewIndexesLocations = [0, 4, 10, 11, 12];    // original    0,1,2,3,4,5,6,7,8,9,10,11        
        int   actualIndex               = 0;                     // deleted     0,1,2,4,5,7,8,10,11         
        var   count                     = toShift.Count;         // leaving     {new},3,{?},6,9,{new},{new},{new}        
        for (var oldIndex = 0; oldIndex < count + 1; oldIndex++) // shifts at   {clear(0,3)},(0,-2),(2,-1),(3,-2),{clear(4,2)}
        {
            if (leaveExisting.Contains(oldIndex))
            {
                Console.Out.WriteLine($"Leaving index {oldIndex} at new Index {actualIndex} with Price {toShift[actualIndex].Price}");
                instances[oldIndex] = toShift[actualIndex];
                actualIndex++;
            }
            else if (oldIndex < count)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with Price {toShift[actualIndex].Price}");
                toShift.RemoveAt(actualIndex);
            }
            if (insertNewIndexesLocations.Contains(oldIndex))
            {
                var counterPartyOrdersPvL = new PQFullSupportPriceVolumeLayer
                    (nameIdLookupGenerator, ExpectedPrice + (0.0001m * (20 + oldIndex)), ExpectedVolume, ExpectedValueDate,
                     ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef, ExpectedOrdersCount, ExpectedInternalVolume);
                Console.Out.WriteLine($"Inserting at index {oldIndex} at new Index {actualIndex} with Price {counterPartyOrdersPvL.Price}");
                toShift.InsertAt(actualIndex, counterPartyOrdersPvL);
                actualIndex++;
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftedNext.CalculateShift(testDateTime, toShift);

        Console.Out.WriteLine($"{shiftedNext.ShiftCommands.JoinShiftCommandsOnNewLine()}");

        Assert.AreEqual(5, shiftedNext.ShiftCommands.Count);
        instances[leaveExisting[0]] = shiftedNext[leaveExisting[0]];
        instances[leaveExisting[1]] = shiftedNext[leaveExisting[1]];
        instances[leaveExisting[2]] = shiftedNext[leaveExisting[2]];
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < shiftedNext.ShiftCommands.Count; i++)
            {
                var shift = shiftedNext.ShiftCommands[i];
                Console.Out.WriteLine($"shift: {shift}");
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(0, shift.PinnedFromIndex);
                        Assert.AreEqual(3, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.RemoveElementsRange | ListShiftCommandType.InsertElementsRange, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(0, shift.PinnedFromIndex);
                        Assert.AreEqual(-2, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(2, shift.PinnedFromIndex);
                        Assert.AreEqual(-1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 3:
                        Assert.AreEqual(3, shift.PinnedFromIndex);
                        Assert.AreEqual(-2, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 4:
                        Assert.AreEqual(5, shift.PinnedFromIndex);
                        Assert.AreEqual(2, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.RemoveElementsRange | ListShiftCommandType.InsertElementsRange, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in shiftedNext.ShiftCommands)
        {
            shiftedNext.ApplyListShiftCommand(shiftElementShift);
        }
        for (int i = 0; i < existingNewLocation.Length; i++)
        {
            Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide[leaveExisting[i]], shiftedNext[existingNewLocation[i]]);
            Assert.AreSame(instances[leaveExisting[i]], shiftedNext[existingNewLocation[i]]);
        }
    }

    [TestMethod]
    public void PopulatedOrderBookSide_InsertNewAtStart_OneShiftRightCommandExpected()
    {
        fullSupportFullyPopulatedOrderBookSide.HasUpdates = false;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        int[] expectedExistingIndices = [4, 5, 6, 7];
        int[] expectedNewIndices      = [0, 1, 2, 3];

        IPriceVolumeLayer[] instances = new IPriceVolumeLayer[12];

        int actualIndex = 0;                                 // original    0,1,2,3,4,5,6,7,8,9,10,11
        var count       = toShift.Count;                     // inserting   {new},{new},{new},{new} 
        for (var oldIndex = 0; oldIndex < count; oldIndex++) // deleted     8,9,10,11
        {                                                    // leaving     {new},{new},{new},{new},4,5,6,7
            if (oldIndex < 8)                                // shifts at   {clear(4,8)}(-1,4) 
            {
                if (oldIndex < 4)
                {
                    var counterPartyOrdersPvL = new PQFullSupportPriceVolumeLayer
                        (nameIdLookupGenerator, ExpectedPrice + (0.0001m * (20 + oldIndex)), ExpectedVolume, ExpectedValueDate,
                         ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef, ExpectedOrdersCount, ExpectedInternalVolume);
                    Console.Out.WriteLine($"Inserting at index {oldIndex} with Price {counterPartyOrdersPvL.Price}");
                    toShift.InsertAt(actualIndex, counterPartyOrdersPvL);
                }
                else
                {
                    Console.Out.WriteLine($"Leaving original index {oldIndex - 4} at new index {oldIndex} with Price {toShift[actualIndex].Price}");
                }
                actualIndex++;
            }
            else
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with Price {toShift[actualIndex].Price}");
                toShift.RemoveAt(actualIndex);
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportFullyPopulatedOrderBookSide.Clone();
        instances[expectedNewIndices[0]] = shiftedNext[expectedNewIndices[0]];
        instances[expectedNewIndices[1]] = shiftedNext[expectedNewIndices[1]];
        instances[expectedNewIndices[2]] = shiftedNext[expectedNewIndices[2]];
        instances[expectedNewIndices[3]] = shiftedNext[expectedNewIndices[3]];
        shiftedNext.CalculateShift(testDateTime, toShift);

        Console.Out.WriteLine($"{shiftedNext.ShiftCommands.JoinShiftCommandsOnNewLine()}");

        Assert.AreEqual(2, shiftedNext.ShiftCommands.Count);
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < shiftedNext.ShiftCommands.Count; i++)
            {
                var shift = shiftedNext.ShiftCommands[i];
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(4, shift.PinnedFromIndex);
                        Assert.AreEqual(8, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.RemoveElementsRange | ListShiftCommandType.InsertElementsRange, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(-1, shift.PinnedFromIndex);
                        Assert.AreEqual(4, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in shiftedNext.ShiftCommands)
        {
            shiftedNext.ApplyListShiftCommand(shiftElementShift);
        }
        for (var i = 0; i < expectedExistingIndices.Length; i++)
        {
            var newIndex   = expectedExistingIndices[i];
            var oldIndexes = expectedNewIndices[i];
            Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide[oldIndexes], shiftedNext[newIndex]);
            Assert.AreSame(instances[oldIndexes], shiftedNext[newIndex]);
        }
    }

    [TestMethod]
    public void PopulatedOrderBookSide_DeleteAtStartNewAtEnd_OneShiftLeftCommandExpected()
    {
        fullSupportFullyPopulatedOrderBookSide.HasUpdates = false;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        int[] expectedExistingIndices = [4, 5, 6, 7, 8, 9, 10, 11];
        int[] expectedNewIndices      = [0, 1, 2, 3, 4, 5, 6, 7];

        IPriceVolumeLayer[] instances = new IPriceVolumeLayer[12];

        int actualIndex = 0;                                     // original    0,1,2,3,4,5,6,7,8,9,10,11 
        var count       = toShift.Count;                         // inserting                     {new},{new},{new},{new}
        for (var oldIndex = 0; oldIndex < count + 4; oldIndex++) // deleted     0,1,2,3,
        {                                                        // leaving     4,5,6,7,8,9,10,11,{new},{new},{new},{new}
            if (oldIndex > 3)                                    // shifts at   (-1,-4)
            {
                if (oldIndex > 11)
                {
                    var counterPartyOrdersPvL = new PQOrdersPriceVolumeLayer
                        (nameIdLookupGenerator, LayerType.OrdersFullPriceVolume, ExpectedPrice + (0.0001m * (20 + oldIndex))
                       , ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
                    Console.Out.WriteLine($"Inserting new entry index {actualIndex} with Price {counterPartyOrdersPvL.Price}");
                    toShift.InsertAt(actualIndex, counterPartyOrdersPvL);
                }
                else
                {
                    Console.Out.WriteLine($"Leaving original index {oldIndex} at new index {actualIndex} with Price {toShift[actualIndex].Price}");
                    instances[oldIndex] = toShift[actualIndex];
                }
                actualIndex++;
            }
            else
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with Price {toShift[actualIndex].Price}");
                toShift.RemoveAt(actualIndex);
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportFullyPopulatedOrderBookSide.Clone();
        instances[expectedExistingIndices[0]] = shiftedNext[expectedExistingIndices[0]];
        instances[expectedExistingIndices[1]] = shiftedNext[expectedExistingIndices[1]];
        instances[expectedExistingIndices[2]] = shiftedNext[expectedExistingIndices[2]];
        instances[expectedExistingIndices[3]] = shiftedNext[expectedExistingIndices[3]];
        instances[expectedExistingIndices[4]] = shiftedNext[expectedExistingIndices[4]];
        instances[expectedExistingIndices[5]] = shiftedNext[expectedExistingIndices[5]];
        instances[expectedExistingIndices[6]] = shiftedNext[expectedExistingIndices[6]];
        instances[expectedExistingIndices[7]] = shiftedNext[expectedExistingIndices[7]];
        shiftedNext.CalculateShift(testDateTime, toShift);

        Console.Out.WriteLine($"{shiftedNext.ShiftCommands.JoinShiftCommandsOnNewLine()}");

        Assert.AreEqual(1, shiftedNext.ShiftCommands.Count);
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < shiftedNext.ShiftCommands.Count; i++)
            {
                var shift = shiftedNext.ShiftCommands[i];
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(-1, shift.PinnedFromIndex);
                        Assert.AreEqual(-4, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in shiftedNext.ShiftCommands)
        {
            shiftedNext.ApplyListShiftCommand(shiftElementShift);
        }
        for (var i = 0; i < expectedExistingIndices.Length; i++)
        {
            var updatedIndex    = expectedNewIndices[i];
            var previousIndexes = expectedExistingIndices[i];
            Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide[previousIndexes], shiftedNext[updatedIndex]);
            Assert.AreSame(instances[previousIndexes], shiftedNext[updatedIndex]);
        }
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_ClearAfterMidElement_ListIsReducedByHalf()
    {
        var halfListSize = MaxNumberOfLayers / 2;
        ordersAnonFullyPopulatedOrderBookSide.HasUpdates = false;
        var toShift = ordersAnonFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(ordersAnonFullyPopulatedOrderBookSide, toShift);

        toShift.ClearRemainingElementsFromIndex = halfListSize;

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], ordersAnonFullyPopulatedOrderBookSide[i]);
        }
        for (int i = halfListSize; i < ordersAnonFullyPopulatedOrderBookSide.Count; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(ordersAnonFullyPopulatedOrderBookSide.Count, toShift.Count + halfListSize);

        var shiftViaDeltaUpdates = ordersAnonFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = ordersAnonFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_InsertNewElementAtStart_RemainingElementsShiftRightByOne()
    {
        var newPvl = new PQPriceVolumeLayer(ExpectedPrice + (0.0001m * 13), ExpectedVolume);

        simpleFullyPopulatedOrderBookSide.MaxAllowedSize = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;
        simpleFullyPopulatedOrderBookSide.HasUpdates     = false;
        var toShift = simpleFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(simpleFullyPopulatedOrderBookSide, toShift);

        toShift.InsertAtStart(newPvl);

        for (int i = 1; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i - 1;
            Assert.AreEqual(toShift[shiftIndex], simpleFullyPopulatedOrderBookSide[prevIndex]);
        }
        Assert.AreEqual(0, toShift.IndexOf(newPvl));

        var shiftViaDeltaUpdates = simpleFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = simpleFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_DeleteMiddleElement_RemainingElementsShiftLeftByOne()
    {
        var midIndex = MaxNumberOfLayers / 2 + 1;

        sourceFullyPopulatedOrderBookSide.HasUpdates = false;
        var toShift = sourceFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(sourceFullyPopulatedOrderBookSide, toShift);

        var middleElement = toShift[midIndex];

        toShift.DeleteAt(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i + 1;
            Assert.AreEqual(toShift[shiftIndex], sourceFullyPopulatedOrderBookSide[prevIndex]);
        }
        Assert.AreEqual(sourceFullyPopulatedOrderBookSide.Count, toShift.Count + 1);

        toShift = sourceFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(sourceFullyPopulatedOrderBookSide, toShift);

        toShift.Delete(middleElement);
        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i + 1;
            Assert.AreEqual(toShift[shiftIndex], sourceFullyPopulatedOrderBookSide[prevIndex]);
        }
        Assert.AreEqual(sourceFullyPopulatedOrderBookSide.Count, toShift.Count + 1);


        var shiftViaDeltaUpdates = sourceFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = sourceFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrderBookSide_InsertNewElementAtStart_RemainingElementsShiftRightExceptLastIsRemoved()
    {
        var newLastTrade = new PQValueDatePriceVolumeLayer
            (ExpectedPrice + (0.0001m * 13), ExpectedVolume, ExpectedValueDate);

        valueDateFullyPopulatedOrderBookSide.MaxAllowedSize = MaxNumberOfLayers;
        valueDateFullyPopulatedOrderBookSide.HasUpdates     = false;
        var toShift = valueDateFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(valueDateFullyPopulatedOrderBookSide, toShift);

        Assert.AreEqual(MaxNumberOfLayers, toShift.Count);
        toShift.InsertAtStart(newLastTrade);

        for (int i = 1; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i - 1;
            Assert.AreEqual(toShift[shiftIndex], valueDateFullyPopulatedOrderBookSide[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfLayers, toShift.Count);
        Assert.AreEqual(0, toShift.IndexOf(newLastTrade));

        var shiftViaDeltaUpdates = valueDateFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfLayers, toShift.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = valueDateFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfLayers, toShift.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_InsertNewElementAtEnd_NewElementAppearsAtTheEnd()
    {
        var newLastTrade = new PQOrdersPriceVolumeLayer
            (nameIdLookupGenerator, LayerType.OrdersAnonymousPriceVolume, ExpectedPrice + (0.0001m * 13)
           , ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);

        ordersAnonFullyPopulatedOrderBookSide.MaxAllowedSize = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;
        ordersAnonFullyPopulatedOrderBookSide.HasUpdates     = false;
        var toShift = ordersAnonFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(ordersAnonFullyPopulatedOrderBookSide, toShift);

        toShift.AppendAtEnd(newLastTrade);

        for (int i = 0; i < toShift.Count - 1; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], ordersAnonFullyPopulatedOrderBookSide[prevIndex]);
        }
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(newLastTrade));

        var shiftViaDeltaUpdates = ordersAnonFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = ordersAnonFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrderBookSide_AttemptInsertNewElementAtEnd_ReturnsFalseAndNoElementIsAdded()
    {
        var newPvl = new PQPriceVolumeLayer(ExpectedPrice + (0.0001m * 13), ExpectedVolume);

        simpleFullyPopulatedOrderBookSide.HasUpdates     = false;
        simpleFullyPopulatedOrderBookSide.MaxAllowedSize = MaxNumberOfLayers;
        var toShift = simpleFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(simpleFullyPopulatedOrderBookSide, toShift);

        var result = toShift.AppendAtEnd(newPvl);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = MaxNumberOfLayers / 2;
        fullSupportFullyPopulatedOrderBookSide.MaxAllowedSize = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;
        fullSupportFullyPopulatedOrderBookSide.HasUpdates     = false;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        toShift.ShiftElements(-halfListSize);

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportFullyPopulatedOrderBookSide[i + halfListSize]);
        }
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide.Count, toShift.Count + halfListSize);

        var shiftViaDeltaUpdates = fullSupportFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrderBookSide_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = MaxNumberOfLayers / 2;
        fullSupportFullyPopulatedOrderBookSide.HasUpdates     = false;
        fullSupportFullyPopulatedOrderBookSide.MaxAllowedSize = MaxNumberOfLayers;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        toShift.ShiftElements(-halfListSize);

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportFullyPopulatedOrderBookSide[i + halfListSize]);
        }
        Assert.AreEqual(MaxNumberOfLayers, toShift.Count + halfListSize);
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide.Count, toShift.Count + halfListSize);

        var shiftViaDeltaUpdates = fullSupportFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_ShiftRightFromStart_CreatesEmptyAtStartAndExtendsListByHalf()
    {
        var halfListSize = MaxNumberOfLayers / 2;
        fullSupportFullyPopulatedOrderBookSide.MaxAllowedSize = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;
        fullSupportFullyPopulatedOrderBookSide.HasUpdates     = false;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        toShift.ShiftElements(halfListSize);

        for (int i = halfListSize; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportFullyPopulatedOrderBookSide[i - halfListSize]);
        }
        for (int i = 0; i < halfListSize; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide.Count, toShift.Count - halfListSize);

        var shiftViaDeltaUpdates = fullSupportFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrderBookSide_ShiftRightFromStart_CreatesEmptyAtStartAndExtendsListByHalf()
    {
        var halfListSize = MaxNumberOfLayers / 2;
        fullSupportFullyPopulatedOrderBookSide.HasUpdates     = false;
        fullSupportFullyPopulatedOrderBookSide.MaxAllowedSize = MaxNumberOfLayers;
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        toShift.ShiftElements(halfListSize);

        for (int i = halfListSize; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportFullyPopulatedOrderBookSide[i - halfListSize]);
        }
        for (int i = 0; i < halfListSize; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(MaxNumberOfLayers, toShift.Count);
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide.Count, toShift.Count);

        var shiftViaDeltaUpdates = fullSupportFullyPopulatedOrderBookSide.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
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
                (new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, depthId, pvl.Price, priceScale),
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
                Assert.IsTrue(copyFromLayer.AreEquivalent(upgradedLayer, exactlyEquals),
                              $"Expected {copyFromLayer} to be equivalent to {upgradedLayer} when exactlyEquals {exactlyEquals}");
        }
    }

    private static IPQOrderBookSide CreateNewEmpty(IPQOrderBookSide populatedOrderBookSide)
    {
        var cloneGenesis = populatedOrderBookSide[0].Clone();
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
