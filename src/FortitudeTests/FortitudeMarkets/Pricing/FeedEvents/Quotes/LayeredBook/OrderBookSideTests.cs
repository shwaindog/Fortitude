// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

[TestClass]
public class OrderBookSideTests
{
    private const int MaxNumberOfLayers  = 12; // test being less than max.
    private const int MaxNumberOfTraders = 3;  // not too many traders.

    private const decimal ExpectedPrice          = 1.234567m;
    private const decimal ExpectedVolume         = 40_000_000m;
    private const string  ExpectedSourceName     = "TestSourceName";
    private const bool    ExpectedExecutable     = true;
    private const uint    ExpectedSourceQuoteRef = 12345678u;
    private const int     ExpectedOrdersCount    = 3; // not too many traders.
    private const decimal ExpectedInternalVolume = 20_000_000m;

    private const OrderGenesisFlags ExpectedGenesisFlags
        = OrderGenesisFlags.FromAdapter | OrderGenesisFlags.IsExternalOrder | OrderGenesisFlags.HasExternalCounterPartyInfo;
    private const OrderGenesisFlags ExpectedAnonymousGenesisFlags
        = OrderGenesisFlags.FromAdapter;
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

    private static readonly MarketAggregate ExpectedSidedOpenInterest =
        new(ExpectedDataSource, ExpectedOpenInterestVolume, ExpectedOpenInterestVwap, new(2025, 5, 8, 12, 8, 59));

    private static readonly DateTime ExpectedValueDate        = new(2017, 12, 09, 14, 0, 0);
    private static readonly DateTime ExpectedOrderCreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime ExpectedOrderUpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
    
    private const QuoteInstantBehaviorFlags QuoteBehavior = QuoteInstantBehaviorFlags.None;

    private IList<IFullSupportPriceVolumeLayer> fullSupportLayers = null!;

    private List<OrderBookSide> allPopulatedOrderBooks = null!;

    private IPQNameIdLookupGenerator emptyNameIdLookupGenerator   = null!;
    private ISourceTickerInfo        publicationPrecisionSettings = null!;

    private IList<IPriceVolumeLayer>               simpleLayers             = null!;
    private IList<ISourcePriceVolumeLayer>         sourceLayers             = null!;
    private IList<ISourceQuoteRefPriceVolumeLayer> sourceQtRefLayers        = null!;
    private IList<IOrdersCountPriceVolumeLayer>    ordersCountLayers        = null!;
    private IList<IOrdersPriceVolumeLayer>         ordersAnonLayers         = null!;
    private IList<IOrdersPriceVolumeLayer>         ordersCounterPartyLayers = null!;
    private IList<IValueDatePriceVolumeLayer>      valueDateLayers          = null!;
    private List<IReadOnlyList<IPriceVolumeLayer>> allPopulatedLayers       = null!;

    private OrderBookSide simpleFullyPopulatedOrderBookSide             = null!;
    private OrderBookSide sourceFullyPopulatedOrderBookSide             = null!;
    private OrderBookSide sourceQtRefFullyPopulatedOrderBookSide        = null!;
    private OrderBookSide valueDateFullyPopulatedOrderBookSide          = null!;
    private OrderBookSide ordersCountFullyPopulatedOrderBookSide        = null!;
    private OrderBookSide ordersAnonFullyPopulatedOrderBookSide         = null!;
    private OrderBookSide ordersCounterPartyFullyPopulatedOrderBookSide = null!;
    private OrderBookSide fullSupportFullyPopulatedOrderBookSide        = null!;

    private static readonly DateTime TestDateTime = new(2025, 5, 31, 10, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

        ordersCounterPartyLayers = new List<IOrdersPriceVolumeLayer>(MaxNumberOfLayers);

        simpleLayers      = new List<IPriceVolumeLayer>(MaxNumberOfLayers);
        sourceLayers      = new List<ISourcePriceVolumeLayer>(MaxNumberOfLayers);
        sourceQtRefLayers = new List<ISourceQuoteRefPriceVolumeLayer>(MaxNumberOfLayers);
        valueDateLayers   = new List<IValueDatePriceVolumeLayer>(MaxNumberOfLayers);
        ordersCountLayers = new List<IOrdersCountPriceVolumeLayer>(MaxNumberOfLayers);
        ordersAnonLayers  = new List<IOrdersPriceVolumeLayer>(MaxNumberOfLayers);
        fullSupportLayers = new List<IFullSupportPriceVolumeLayer>(MaxNumberOfLayers);

        allPopulatedLayers =
        [
            (IReadOnlyList<IPriceVolumeLayer>)simpleLayers, (IReadOnlyList<IPriceVolumeLayer>)sourceLayers
          , (IReadOnlyList<IPriceVolumeLayer>)sourceQtRefLayers, (IReadOnlyList<IPriceVolumeLayer>)valueDateLayers
          , (IReadOnlyList<IPriceVolumeLayer>)ordersCountLayers, (IReadOnlyList<IPriceVolumeLayer>)ordersAnonLayers
          , (IReadOnlyList<IPriceVolumeLayer>)ordersCounterPartyLayers, (IReadOnlyList<IPriceVolumeLayer>)fullSupportLayers
        ];

        for (var i = 0; i < MaxNumberOfLayers; i++)
        {
            simpleLayers.Add(new PriceVolumeLayer(ExpectedPrice + (0.0001m * i), ExpectedVolume));
            var sourcePvl =
                new SourcePriceVolumeLayer(ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedSourceName, ExpectedExecutable);
            sourceLayers.Add(sourcePvl);
            var srcQtRefPvl = new SourceQuoteRefPriceVolumeLayer
                (ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedSourceName, ExpectedExecutable
               , ExpectedSourceQuoteRef);
            sourceQtRefLayers.Add(srcQtRefPvl);
            valueDateLayers.Add(new ValueDatePriceVolumeLayer(ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedValueDate));
            var ordersCountPvl
                = new OrdersCountPriceVolumeLayer(ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
            ordersCountLayers.Add(ordersCountPvl);
            var anonOrdersPvL = new OrdersPriceVolumeLayer
                (LayerType.OrdersAnonymousPriceVolume, ExpectedPrice + (0.0001m * i)
               , ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
            ordersAnonLayers.Add(anonOrdersPvL);
            var counterPartyOrdersPvL = new OrdersPriceVolumeLayer
                (LayerType.OrdersFullPriceVolume, ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
            ordersCounterPartyLayers.Add(counterPartyOrdersPvL);
            var fullSupportPvL = new FullSupportPriceVolumeLayer
                (ExpectedPrice + (0.0001m * i), ExpectedVolume, ExpectedValueDate,
                 ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef, ExpectedOrdersCount, ExpectedInternalVolume);
            fullSupportLayers.Add(fullSupportPvL);
            for (var j = 0; j < MaxNumberOfTraders; j++)
            {
                fullSupportPvL.Add
                    (new ExternalCounterPartyOrder
                        (new AnonymousOrder
                            (ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags
                           , ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, ExpectedTrackingId)
                            {
                                ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo
                                    (i + 1, ExpectedCounterPartyBase + i
                                   , i + 1, ExpectedTraderNameBase + i)
                            })
                    );
                anonOrdersPvL.Add
                    (new AnonymousOrder
                        (ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedAnonymousGenesisFlags
                       , ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, ExpectedTrackingId));
                counterPartyOrdersPvL.Add
                    (new ExternalCounterPartyOrder
                        (new AnonymousOrder
                            (ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags
                           , ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, ExpectedTrackingId)
                            {
                                ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo
                                    (i + 1, ExpectedCounterPartyBase + i
                                   , i + 1, ExpectedTraderNameBase + i)
                            })
                    );
            }
        }

        simpleFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.AskBook, simpleLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        sourceFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.BidBook, sourceLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        sourceQtRefFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.BidBook, sourceQtRefLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        valueDateFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.AskBook, valueDateLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        ordersCountFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.BidBook, ordersCountLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        ordersAnonFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.BidBook, ordersAnonLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };
        ordersCounterPartyFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.AskBook, ordersCounterPartyLayers)
            {
                DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
            };
        fullSupportFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.BidBook, fullSupportLayers)
        {
            DailyTickUpdateCount = ExpectedDailyTickCount, OpenInterestSide = ExpectedSidedOpenInterest
        };

        allPopulatedOrderBooks =
        [
            simpleFullyPopulatedOrderBookSide, sourceFullyPopulatedOrderBookSide, sourceQtRefFullyPopulatedOrderBookSide
          , valueDateFullyPopulatedOrderBookSide, ordersCountFullyPopulatedOrderBookSide, ordersAnonFullyPopulatedOrderBookSide
          , ordersCounterPartyFullyPopulatedOrderBookSide, fullSupportFullyPopulatedOrderBookSide
        ];
        publicationPrecisionSettings = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, MarketClassification.Unknown
          ,  AUinMEL, AUinMEL, AUinMEL
           , 20, 0.00001m, 30000m, 50000000m, 1000m
           , layerFlags: LayerFlags.Volume | LayerFlags.Price
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void FromSourceTickerInfo_New_InitializesOrderBookWithExpectedLayerTypes()
    {
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var orderBook = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable;
        orderBook = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(SourcePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        orderBook = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate;
        orderBook = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(ValueDatePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrdersCount | LayerFlags.OrderSize;
        orderBook = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(OrdersPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price.AllFlags();

        orderBook = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(FullSupportPriceVolumeLayer));
    }

    [TestMethod]
    public void PQLayers_New_ConvertsToExpectedEquivalent()
    {
        IList<IPriceVolumeLayer> pqList = new List<IPriceVolumeLayer>
        {
            new PQPriceVolumeLayer()
        };
        var orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PriceVolumeLayer));

        pqList.Clear();
        pqList.Add(new PQSourcePriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(SourcePriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQSourceQuoteRefPriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(SourceQuoteRefPriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQValueDatePriceVolumeLayer());
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(ValueDatePriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQOrdersPriceVolumeLayer(emptyNameIdLookupGenerator.Clone(), LayerType.OrdersFullPriceVolume));
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(OrdersPriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQFullSupportPriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(FullSupportPriceVolumeLayer));

        orderBook = new OrderBookSide(BookSide.BidBook, []);
        Assert.AreEqual(0, orderBook.Count);
    }

    [TestMethod]
    public void PQOrderBook_InitializedFromOrderBook_ConvertsLayers()
    {
        var pqSrcTkrQuoteInfo =
            new PQSourceTickerInfo(publicationPrecisionSettings)
            {
                LayerFlags = LayerFlags.Price | LayerFlags.Volume
            };
        var pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        var orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(PriceVolumeLayer));

        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable;
        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(SourcePriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefPriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate;
        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(ValueDatePriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrdersCount | LayerFlags.OrderSize;
        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(OrdersPriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags = LayerFlags.Price.AllFlags();

        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(FullSupportPriceVolumeLayer));
    }

    [TestMethod]
    public void NewOrderBook_InitializedWithLayers_ContainsNewInstanceLayersWithSameValues()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var populatedLayers    = allPopulatedLayers[i];
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Count);
            for (var j = 0; j < MaxNumberOfTraders; j++)
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
            var clonedOrderBook    = new OrderBookSide(populatedOrderBook);
            Assert.AreEqual(MaxNumberOfLayers, clonedOrderBook.Count);
            for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreNotSame(populatedOrderBook[j], clonedOrderBook[j]);
        }

        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var emptyOrderBook = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        Assert.AreEqual(0, emptyOrderBook.Count);
        var clonedEmptyOrderBook = new OrderBookSide(emptyOrderBook);
        Assert.AreEqual(0, clonedEmptyOrderBook.Count);
        for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreNotSame(emptyOrderBook[j], clonedEmptyOrderBook[j]);
    }

    [TestMethod]
    public void PopulatedOrderBook_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
            for (var i = 0; i < MaxNumberOfLayers; i++)
            {
                var layer       = ((IOrderBookSide)populatedOrderBook)[i];
                var clonedLayer = (IMutablePriceVolumeLayer)layer.Clone();
                populatedOrderBook[i] = clonedLayer;
                Assert.AreNotSame(layer, ((IMutableOrderBookSide)populatedOrderBook)[i]);
                Assert.AreSame(clonedLayer, populatedOrderBook[i]);
                if (i == populatedOrderBook.Count - 1)
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
            Assert.AreEqual(populatedOrderBook.Count, populatedOrderBook.Capacity);
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Capacity);
            populatedOrderBook[MaxNumberOfLayers - 1] = populatedOrderBook[MaxNumberOfLayers - 1].ResetWithTracking();
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Capacity);
            Assert.AreEqual(populatedOrderBook.Count + 1, populatedOrderBook.Capacity);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PopulatedOrderBook_CapacityLargerThanMaxBookDepth_ThrowsException()
    {
        simpleFullyPopulatedOrderBookSide.Capacity = PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth + 1;
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
    public void StaticDefault_LayerConverter_IsOrderBookLayerFactorySelector()
    {
        Assert.AreSame(typeof(OrderBookLayerFactorySelector), OrderBookSide.LayerSelector.GetType());
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
    public void FullyPopulatedOrderBook_CopyFromToEmptyQuote_OrderBooksEqualEachOther()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var newEmpty = new OrderBookSide(populatedOrderBook);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            newEmpty.CopyFrom(populatedOrderBook, QuoteBehavior);
            Assert.AreEqual(populatedOrderBook, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromSubTypes_SubTypeSaysIsEquivalent()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        foreach (var subType in allPopulatedOrderBooks.Where(ob => !ReferenceEquals(ob, populatedOrderBook)))
        {
            if (!WhollyContainedBy(subType[0].GetType(), populatedOrderBook[0].GetType())) continue;
            var newEmpty = new OrderBookSide(populatedOrderBook);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            newEmpty.CopyFrom(subType, QuoteBehavior);
            Assert.IsTrue(subType.AreEquivalent(newEmpty));
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
        var notEmpty = new OrderBookSide(simpleFullyPopulatedOrderBookSide);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated, QuoteBehavior);
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
        var notEmpty = new OrderBookSide(simpleFullyPopulatedOrderBookSide);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated, QuoteBehavior);
        Assert.AreEqual(notEmpty[5], new PriceVolumeLayer()); // null copies to empty
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
        var notEmpty = new OrderBookSide(simpleFullyPopulatedOrderBookSide)
            { [5] = simpleFullyPopulatedOrderBookSide[5].Clone().ResetWithTracking() };
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated, QuoteBehavior);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfLayers - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var clonedOrderBook = populatedOrderBook.Clone();
            Assert.AreNotSame(clonedOrderBook, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clonedOrderBook);
            var cloned2 = ((ICloneable<IMutableOrderBookSide>)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned2, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned2);
            var cloned3 = ((IMutableOrderBookSide)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned3, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned3);
            var cloned4 = ((ICloneable<IOrderBookSide>)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned4, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned4);
            var cloned5 = ((ICloneable)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned5, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned5);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var fullyPopulatedClone = populatedOrderBook.Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedOrderBook,
                                                                fullyPopulatedClone);
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
            var q        = populatedQuote;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(q.Capacity)}: {q.Capacity}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.MaxAllowedSize)}: {q.MaxAllowedSize}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.LayerSupportedFlags)}: {q.LayerSupportedFlags}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.OpenInterestSide)}: {q.OpenInterestSide}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AllLayers)}: [\n{q.EachLayerByIndexOnNewLines()}]"));
        }
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = fullSupportFullyPopulatedOrderBookSide;
        Assert.AreEqual(MaxNumberOfLayers, rt.Count);
        Assert.AreEqual(MaxNumberOfLayers, ((IEnumerable)rt).Cast<IPriceVolumeLayer>().Count());
        Assert.AreEqual(MaxNumberOfLayers, ((IEnumerable<IPriceVolumeLayer>)rt).Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable)rt).Cast<IPriceVolumeLayer>().Count());
        Assert.AreEqual(0, ((IEnumerable<IPriceVolumeLayer>)rt).Count());
    }

    [TestMethod]
    public void PopulatedOrderBookSide_SmallerToLargerCalculateShifts_ShiftRightCommandsExpected()
    {
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
        toShift.CalculateShift(TestDateTime, shiftedNext);

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
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        int[]               expectedIndices = [0, 2, 4, 5, 6, 8, 10];
        IPriceVolumeLayer[] instances       = new IPriceVolumeLayer[11];

        var counterPartyOrdersPvL = new FullSupportPriceVolumeLayer
            (ExpectedPrice + (0.0001m * 13), ExpectedVolume, ExpectedValueDate,
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
        toShift.CalculateShift(TestDateTime, shiftedNext);

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
                var counterPartyOrdersPvL = new FullSupportPriceVolumeLayer
                    ( ExpectedPrice + (0.0001m * (20 + oldIndex)), ExpectedVolume, ExpectedValueDate,
                     ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef, ExpectedOrdersCount, ExpectedInternalVolume);
                Console.Out.WriteLine($"Inserting at index {oldIndex} at new Index {actualIndex} with Price {counterPartyOrdersPvL.Price}");
                toShift.InsertAt(actualIndex, counterPartyOrdersPvL);
                actualIndex++;
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftedNext.CalculateShift(TestDateTime, toShift);

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
    public void PopulatedOrderBookSide_DeleteAtStartNewAtEnd_OneShiftLeftCommandExpected()
    {
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
                    var counterPartyOrdersPvL = new FullSupportPriceVolumeLayer
                        ( ExpectedPrice + (0.0001m * (20 + oldIndex)), ExpectedVolume, ExpectedValueDate,
                         ExpectedSourceName, ExpectedExecutable, ExpectedSourceQuoteRef, ExpectedOrdersCount, ExpectedInternalVolume);
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
        shiftedNext.CalculateShift(TestDateTime, toShift);

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

        var shiftCopyFrom = ordersAnonFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_InsertNewElementAtStart_RemainingElementsShiftRightByOne()
    {
        var newPvl = new PriceVolumeLayer(ExpectedPrice + (0.0001m * 13), ExpectedVolume);

        simpleFullyPopulatedOrderBookSide.MaxAllowedSize = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;
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

        var shiftCopyFrom = simpleFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_DeleteMiddleElement_RemainingElementsShiftLeftByOne()
    {
        var midIndex = MaxNumberOfLayers / 2 + 1;

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

        var shiftCopyFrom = sourceFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrderBookSide_InsertNewElementAtStart_RemainingElementsShiftRightExceptLastIsRemoved()
    {
        var newLastTrade = new ValueDatePriceVolumeLayer
            (ExpectedPrice + (0.0001m * 13), ExpectedVolume, ExpectedValueDate);

        valueDateFullyPopulatedOrderBookSide.MaxAllowedSize = MaxNumberOfLayers;
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

        var shiftCopyFrom = valueDateFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(MaxNumberOfLayers, toShift.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_InsertNewElementAtEnd_NewElementAppearsAtTheEnd()
    {
        var newLastTrade = new OrdersPriceVolumeLayer
            (LayerType.OrdersAnonymousPriceVolume, ExpectedPrice + (0.0001m * 13)
           , ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);

        ordersAnonFullyPopulatedOrderBookSide.MaxAllowedSize = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;
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

        var shiftCopyFrom = ordersAnonFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrderBookSide_AttemptInsertNewElementAtEnd_ReturnsFalseAndNoElementIsAdded()
    {
        var newPvl = new PriceVolumeLayer(ExpectedPrice + (0.0001m * 13), ExpectedVolume);

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
        var toShift = fullSupportFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide, toShift);

        toShift.ShiftElements(-halfListSize);

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportFullyPopulatedOrderBookSide[i + halfListSize]);
        }
        Assert.AreEqual(fullSupportFullyPopulatedOrderBookSide.Count, toShift.Count + halfListSize);

        var shiftCopyFrom = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrderBookSide_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = MaxNumberOfLayers / 2;
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

        var shiftCopyFrom = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrderBookSide_ShiftRightFromStart_CreatesEmptyAtStartAndExtendsListByHalf()
    {
        var halfListSize = MaxNumberOfLayers / 2;
        fullSupportFullyPopulatedOrderBookSide.MaxAllowedSize = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;
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

        var shiftCopyFrom = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrderBookSide_ShiftRightFromStart_CreatesEmptyAtStartAndExtendsListByHalf()
    {
        var halfListSize = MaxNumberOfLayers / 2;
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

        var shiftCopyFrom = fullSupportFullyPopulatedOrderBookSide.Clone();
        shiftCopyFrom.CopyFrom(toShift, QuoteBehavior);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    private bool WhollyContainedBy(Type copySourceType, Type copyDestinationType)
    {
        if (copySourceType == typeof(PriceVolumeLayer)) return true;
        if (copySourceType == typeof(SourcePriceVolumeLayer))
            return copyDestinationType == typeof(SourcePriceVolumeLayer) ||
                   copyDestinationType == typeof(SourceQuoteRefPriceVolumeLayer) ||
                   copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        if (copySourceType == typeof(SourceQuoteRefPriceVolumeLayer))
            return copyDestinationType == typeof(SourceQuoteRefPriceVolumeLayer) ||
                   copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        if (copySourceType == typeof(ValueDatePriceVolumeLayer))
            return copyDestinationType == typeof(ValueDatePriceVolumeLayer) ||
                   copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        if (copySourceType == typeof(OrdersPriceVolumeLayer))
            return copyDestinationType == typeof(OrdersPriceVolumeLayer) ||
                   copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        if (copySourceType == typeof(FullSupportPriceVolumeLayer)) return copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        return false;
    }

    private void AssertBookHasLayersOfType(OrderBookSide orderBookSide, Type expectedType)
    {
        for (var i = 0; i < MaxNumberOfLayers; i++) Assert.IsInstanceOfType(orderBookSide[i], expectedType);
    }

    internal static OrderBookSide GenerateBookSide<T>
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
            if (bookSide == BookSide.AskBook)
            {
                currentPrice += deltaPricePerLayer;
            }
            else
            {
                currentPrice -= deltaPricePerLayer;
            }
            currentVolume += deltaVolumePerLayer;
        }

        return new OrderBookSide(bookSide, generatedLayers.Cast<IPriceVolumeLayer>().ToList());
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableOrderBookSide commonOrderBookSide,
        IMutableOrderBookSide changingOrderBookSide,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        IMutablePublishableLevel2Quote? originalQuote = null,
        IMutablePublishableLevel2Quote? changingQuote = null)
    {
        if (changingOrderBook == null && changingOrderBook == null) return;
        Assert.AreEqual(commonOrderBookSide.Count, changingOrderBookSide.Count);

        for (var i = 0; i < commonOrderBookSide.Count; i++)
        {
            PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison, commonOrderBookSide[i],
                 changingOrderBookSide[i], commonOrderBookSide,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            SourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as IMutableSourcePriceVolumeLayer,
                 changingOrderBookSide[i] as IMutableSourcePriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            SourceQuoteRefPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as
                     IMutableSourceQuoteRefPriceVolumeLayer,
                 changingOrderBookSide[i] as
                     IMutableSourceQuoteRefPriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            ValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as IMutableValueDatePriceVolumeLayer,
                 changingOrderBookSide[i] as IMutableValueDatePriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            OrdersCountPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as IMutableOrdersCountPriceVolumeLayer,
                 changingOrderBookSide[i] as IMutableOrdersCountPriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            OrdersPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as IMutableOrdersPriceVolumeLayer,
                 changingOrderBookSide[i] as IMutableOrdersPriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            FullSupportPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (exactComparison, commonOrderBookSide[i] as IMutableFullSupportPriceVolumeLayer
               , changingOrderBookSide[i] as IMutableFullSupportPriceVolumeLayer, commonOrderBookSide
               , changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
        }
    }
}
