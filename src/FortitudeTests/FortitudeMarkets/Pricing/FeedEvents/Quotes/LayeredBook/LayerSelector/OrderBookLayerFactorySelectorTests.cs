// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class OrderBookLayerFactorySelectorTests
{
    private const decimal ExpectedPrice                = 2.3456m;
    private const decimal ExpectedVolume               = 420_100m;
    private const uint    ExpectedQuoteRef             = 41_111_2222u;
    private const uint    ExpectedOrdersCount          = 1;
    private const decimal ExpectedInternalVolume       = 1_000_000;
    private const int     ExpectedOrderId              = 195_979;
    private const decimal ExpectedOrderVolume          = ExpectedVolume * 2;
    private const decimal ExpectedOrderRemainingVolume = ExpectedVolume;

    private const int ExpectedTraderId       = 2;
    private const int ExpectedCounterPartyId       = 1;
    private const string ExpectedTraderName       = "TraderName-Toly";
    private const string ExpectedCounterPartyName = "CounterParty-Chromo";
    private const string ExpectedSourceName = "SourceName-Latrobe";

    private const OrderGenesisFlags           ExpectedGenesisFlags      = OrderGenesisFlags.FromAdapter;
    private const OrderType           ExpectedOrderType      = OrderType.PassiveLimit;
    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.ConfirmedActiveOnMarket;

    private static readonly DateTime ExpectedOrderCreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime ExpectedOrderUpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private readonly OrderBookLayerFactorySelector layerSelector            = new();

    private DateTime expectedValueDate;

    private IPQNameIdLookupGenerator nameIdGenerator                    = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
    private OrdersPriceVolumeLayer   ordersAnonPriceVolumeLayer         = null!;
    private OrdersPriceVolumeLayer   ordersCounterPartyPriceVolumeLayer = null!;

    private OrdersCountPriceVolumeLayer ordersCountPriceVolumeLayer          = null!;
    private PQOrdersPriceVolumeLayer    pqAnonOrdersPriceVolumeLayer         = null!;
    private PQOrdersPriceVolumeLayer    pqCounterPartyOrdersPriceVolumeLayer = null!;

    private PQOrdersCountPriceVolumeLayer pqOrdersCountPriceVolumeLayer = null!;

    private PQPriceVolumeLayer               pqPriceVolumeLayer               = null!;
    private PQSourcePriceVolumeLayer         pqSourcePriceVolumeLayer         = null!;
    private PQSourceQuoteRefPriceVolumeLayer pqSourceQuoteRefPriceVolumeLayer = null!;

    private PQFullSupportPriceVolumeLayer pqFullSupportPvl       = null!;
    private PQValueDatePriceVolumeLayer   pqValueDatePriceVolumeLayer = null!;

    private PriceVolumeLayer               priceVolumeLayer               = null!;
    private SourcePriceVolumeLayer         sourcePriceVolumeLayer         = null!;
    private SourceQuoteRefPriceVolumeLayer sourceQuoteRefPriceVolumeLayer = null!;
    private ISourceTickerInfo              sourceTickerInfo               = null!;

    private FullSupportPriceVolumeLayer fullSupportPvl       = null!;
    private ValueDatePriceVolumeLayer   valueDatePriceVolumeLayer = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdGenerator = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

        expectedValueDate = new DateTime(2018, 01, 9, 22, 0, 0);

        priceVolumeLayer = new PriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        sourcePriceVolumeLayer = new SourcePriceVolumeLayer
            (ExpectedPrice, ExpectedVolume, ExpectedSourceName, true);
        sourceQuoteRefPriceVolumeLayer = new SourceQuoteRefPriceVolumeLayer
            (ExpectedPrice, ExpectedVolume, ExpectedSourceName, true, ExpectedQuoteRef);
        valueDatePriceVolumeLayer   = new ValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume, expectedValueDate);
        ordersCountPriceVolumeLayer = new OrdersCountPriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
        ordersAnonPriceVolumeLayer = new OrdersPriceVolumeLayer
            (LayerType.OrdersAnonymousPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume)
            {
                [0] = new AnonymousOrder
                    (ExpectedOrderId, ExpectedOrderCreatedTime
                   , ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState
                   , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
            };
        ordersCounterPartyPriceVolumeLayer = new OrdersPriceVolumeLayer
            (LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume)
            {
                [0] = new ExternalCounterPartyOrder
                    (new AnonymousOrder(ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags,
                     ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
                     {
                         ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo( ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName)
                     })
            };
        fullSupportPvl = new FullSupportPriceVolumeLayer
            (ExpectedPrice, ExpectedVolume, expectedValueDate, ExpectedSourceName, true, ExpectedQuoteRef, ExpectedOrdersCount
           , ExpectedInternalVolume)
            {
                [0] = new ExternalCounterPartyOrder
                    (new AnonymousOrder(ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags,
                     ExpectedLifecycleState, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
                     {
                         ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo(ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName)
                     })
            };

        pqPriceVolumeLayer = new PQPriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        pqSourcePriceVolumeLayer = new PQSourcePriceVolumeLayer
            (nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume, ExpectedSourceName, true);
        pqSourceQuoteRefPriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer
            (nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume, ExpectedSourceName, true, ExpectedQuoteRef);
        pqValueDatePriceVolumeLayer = new PQValueDatePriceVolumeLayer
            (ExpectedPrice, ExpectedVolume, expectedValueDate);
        pqOrdersCountPriceVolumeLayer = new PQOrdersCountPriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
        pqAnonOrdersPriceVolumeLayer = new PQOrdersPriceVolumeLayer
            (nameIdGenerator.Clone(), LayerType.OrdersAnonymousPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount
           , ExpectedInternalVolume)
            {
                [0] = new PQAnonymousOrder
                    (nameIdGenerator,  ExpectedOrderId, ExpectedOrderCreatedTime
                   , ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState
                   , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
            };
        pqCounterPartyOrdersPriceVolumeLayer = new PQOrdersPriceVolumeLayer
            (nameIdGenerator.Clone(), LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount
           , ExpectedInternalVolume)
            {
                [0] = new PQExternalCounterPartyOrder
                    (new PQAnonymousOrder
                        (nameIdGenerator,  ExpectedOrderId, ExpectedOrderCreatedTime
                       , ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState
                       , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
                        {
                            ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(nameIdGenerator, ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName)
                        })
            };
        pqFullSupportPvl = new PQFullSupportPriceVolumeLayer
            (nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume, expectedValueDate, ExpectedSourceName, true, ExpectedQuoteRef
           , ExpectedOrdersCount, ExpectedInternalVolume)
            {
                [0] = new PQExternalCounterPartyOrder
                    (new PQAnonymousOrder
                        (nameIdGenerator,  ExpectedOrderId, ExpectedOrderCreatedTime
                       , ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState
                       , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
                        {
                            ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(nameIdGenerator, ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName)
                        })
            };

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m
           , layerFlags: LayerFlags.Volume | LayerFlags.Price
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void VariousLayerFlags_FindForLayerFlags_ReturnsPriceVolumeLayer()
    {
        sourceTickerInfo.LayerFlags = LayerFlags.None;
        var pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(PriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(PriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(PriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(PriceVolumeLayer));
    }

    [TestMethod]
    public void VariousLayerFlags_FindForLayerFlags_ReturnsSourcePriceVolumeLayer()
    {
        sourceTickerInfo.LayerFlags = LayerFlags.SourceName;
        var pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));

        sourceTickerInfo.LayerFlags = LayerFlags.Executable;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.Executable | LayerFlags.SourceName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourcePriceVolumeLayer));
    }

    [TestMethod]
    public void VariousLayerFlags_FindForLayerFlags_ReturnsSourceQuoteRefPriceVolumeLayer()
    {
        sourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference;
        var pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceQuoteReference;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(SourceQuoteRefPriceVolumeLayer));
    }

    [TestMethod]
    public void VariousLayerFlags_FindForLayerFlags_ReturnValueDatePriceVolumeLayer()
    {
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate;
        var pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(ValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.ValueDate;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(ValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.ValueDate;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(ValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.ValueDate;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(ValueDatePriceVolumeLayer));
    }

    [TestMethod]
    public void VariousLayerFlags_FindForLayerFlags_ReturnTraderPriceVolumeLayer()
    {
        sourceTickerInfo.LayerFlags = LayerFlags.OrderTraderName;
        var pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(OrdersPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.OrderTraderName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(OrdersPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.OrderTraderName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(OrdersPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.OrderTraderName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(OrdersPriceVolumeLayer));
    }

    [TestMethod]
    public void VariousLayerFlags_FindForLayerFlags_ReturnsFullSupportPvl()
    {
        sourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.OrderTraderName;
        var pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.OrderTraderName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.ValueDate | LayerFlags.SourceName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.OrderTraderName | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.OrderTraderName | LayerFlags.Executable | LayerFlags.ValueDate;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.OrderTraderName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference |
                                      LayerFlags.ValueDate | LayerFlags.OrderTraderName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(FullSupportPriceVolumeLayer));
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ConvertsToNonPQPriceVolumeLayerType()
    {
        var pvl = layerSelector.CreateExpectedImplementation(LayerType.PriceVolume, pqPriceVolumeLayer);
        Assert.AreEqual(pvl.GetType(), typeof(PriceVolumeLayer));
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.SourcePriceVolume, pqSourcePriceVolumeLayer);
        var pqSrcPvl = pvl as SourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.SourceQuoteRefPriceVolume, pqSourceQuoteRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pvl as SourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcQtRefPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcQtRefPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.ValueDatePriceVolume, pqValueDatePriceVolumeLayer);
        var pqVlDtPvl = pvl as ValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqVlDtPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqVlDtPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.OrdersCountPriceVolume, pqOrdersCountPriceVolumeLayer);
        var pqOrdersCountPvl = pvl as OrdersCountPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersCountPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersCountPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersCountPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersCountPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersCountPvl.InternalVolume);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.OrdersAnonymousPriceVolume, pqAnonOrdersPriceVolumeLayer);
        var pqAnonOrdersPvl = pvl as OrdersPriceVolumeLayer;
        Assert.IsNotNull(pqAnonOrdersPvl);
        Assert.AreEqual(ExpectedPrice, pqAnonOrdersPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqAnonOrdersPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqAnonOrdersPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqAnonOrdersPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqAnonOrdersPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, pqAnonOrdersPvl[0]!.GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqAnonOrdersPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqAnonOrdersPvl[0]!.UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, ((IAnonymousOrder)pqAnonOrdersPvl[0]!).OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqAnonOrdersPvl[0]!.OrderRemainingVolume);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.OrdersFullPriceVolume, pqCounterPartyOrdersPriceVolumeLayer);
        var pqCpOrdersPvl = pvl as OrdersPriceVolumeLayer;
        Assert.IsNotNull(pqCpOrdersPvl);
        Assert.AreEqual(ExpectedPrice, pqCpOrdersPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqCpOrdersPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqCpOrdersPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqCpOrdersPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqCpOrdersPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, pqCpOrdersPvl[0]!.GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqCpOrdersPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqCpOrdersPvl[0]!.UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, ((IAnonymousOrder)pqCpOrdersPvl[0]!).OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqCpOrdersPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(ExpectedCounterPartyName, ((IExternalCounterPartyOrder)pqCpOrdersPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedTraderName, ((IExternalCounterPartyOrder)pqCpOrdersPvl[0]!).ExternalTraderName);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.FullSupportPriceVolume, pqFullSupportPvl);
        var convertedPqFullSupportPvl = pvl as FullSupportPriceVolumeLayer;
        Assert.IsNotNull(convertedPqFullSupportPvl);
        Assert.AreEqual(ExpectedPrice, convertedPqFullSupportPvl.Price);
        Assert.AreEqual(ExpectedVolume, convertedPqFullSupportPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, convertedPqFullSupportPvl.SourceName);
        Assert.AreEqual(true, convertedPqFullSupportPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqFullSupportPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqFullSupportPvl.ValueDate);
        Assert.AreEqual(ExpectedOrdersCount, convertedPqFullSupportPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, convertedPqFullSupportPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, convertedPqFullSupportPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, convertedPqFullSupportPvl[0]!.GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, convertedPqFullSupportPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, convertedPqFullSupportPvl[0]!.UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, ((IAnonymousOrder)convertedPqFullSupportPvl[0]!).OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, convertedPqFullSupportPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(ExpectedCounterPartyName, ((IExternalCounterPartyOrder)convertedPqFullSupportPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedTraderName, ((IExternalCounterPartyOrder)convertedPqFullSupportPvl[0]!).ExternalTraderName);
    }

    [TestMethod]
    public void NonPQPriceVolumeLayerTypes_SelectPriceVolumeLayer_ReturnsUnchangedInstance()
    {
        var pvl = layerSelector.UpgradeExistingLayer(priceVolumeLayer, pqPriceVolumeLayer.LayerType);
        Assert.AreSame(priceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(sourcePriceVolumeLayer, sourcePriceVolumeLayer.LayerType);
        Assert.AreSame(sourcePriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(sourceQuoteRefPriceVolumeLayer, sourceQuoteRefPriceVolumeLayer.LayerType);
        Assert.AreSame(sourceQuoteRefPriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(valueDatePriceVolumeLayer, valueDatePriceVolumeLayer.LayerType);
        Assert.AreSame(valueDatePriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(ordersCountPriceVolumeLayer, ordersCountPriceVolumeLayer.LayerType);
        Assert.AreSame(ordersCountPriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(ordersAnonPriceVolumeLayer, ordersAnonPriceVolumeLayer.LayerType);
        Assert.AreSame(ordersAnonPriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(ordersCountPriceVolumeLayer, ordersCountPriceVolumeLayer.LayerType);
        Assert.AreSame(ordersCountPriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(fullSupportPvl, fullSupportPvl.LayerType);
        Assert.AreSame(fullSupportPvl, pvl);
    }

    [TestMethod]
    public void PriceVolumeLayerTypes_ConvertToExpectedImplementationCopyFrom_ReturnsCloneMatchingPriceVolumeLayerType()
    {
        var pvl = layerSelector.CreateExpectedImplementation(priceVolumeLayer.LayerType).CopyFrom(priceVolumeLayer);
        Assert.AreEqual(pvl.GetType(), typeof(PriceVolumeLayer));
        Assert.AreNotSame(sourceQuoteRefPriceVolumeLayer, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);

        pvl = layerSelector.CreateExpectedImplementation(sourcePriceVolumeLayer.LayerType).CopyFrom(sourcePriceVolumeLayer);
        var pqSrcPvl = pvl as SourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreNotSame(sourcePriceVolumeLayer, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pvl = layerSelector.CreateExpectedImplementation(sourceQuoteRefPriceVolumeLayer.LayerType).CopyFrom(sourceQuoteRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pvl as SourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreNotSame(sourceQuoteRefPriceVolumeLayer, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pvl = layerSelector.CreateExpectedImplementation(valueDatePriceVolumeLayer.LayerType).CopyFrom(valueDatePriceVolumeLayer);
        var pqVlDtPvl = pvl as ValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreNotSame(valueDatePriceVolumeLayer, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.OrdersCountPriceVolume).CopyFrom(ordersCountPriceVolumeLayer);
        var pqOrdersCountPvl = pvl as OrdersCountPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersCountPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersCountPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersCountPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersCountPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersCountPvl.InternalVolume);

        pvl = layerSelector.CreateExpectedImplementation(ordersAnonPriceVolumeLayer.LayerType).CopyFrom(ordersAnonPriceVolumeLayer);
        var pqOrdersPvl = pvl as OrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersPvl);
        Assert.AreNotSame(ordersAnonPriceVolumeLayer, pqOrdersPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, pqOrdersPvl[0]!.GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersPvl[0]!.UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, ((IAnonymousOrder)pqOrdersPvl[0]!).OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersPvl[0]!.OrderRemainingVolume);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.OrdersFullPriceVolume).CopyFrom(ordersCounterPartyPriceVolumeLayer);
        var countOrdersPvl = pvl as OrdersPriceVolumeLayer;
        Assert.IsNotNull(countOrdersPvl);
        Assert.AreEqual(ExpectedPrice, countOrdersPvl.Price);
        Assert.AreEqual(ExpectedVolume, countOrdersPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, countOrdersPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, countOrdersPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, countOrdersPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, countOrdersPvl[0]!.GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, countOrdersPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, countOrdersPvl[0]!.UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, ((IAnonymousOrder)countOrdersPvl[0]!).OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, countOrdersPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(ExpectedCounterPartyName, ((IExternalCounterPartyOrder)countOrdersPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedTraderName, ((IExternalCounterPartyOrder)countOrdersPvl[0]!).ExternalTraderName);

        pvl = layerSelector.CreateExpectedImplementation(fullSupportPvl.LayerType).CopyFrom(fullSupportPvl);
        var convertedPqFullSupportPvl = pvl as FullSupportPriceVolumeLayer;
        Assert.IsNotNull(convertedPqFullSupportPvl);
        Assert.AreNotSame(fullSupportPvl, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(ExpectedSourceName, convertedPqFullSupportPvl.SourceName);
        Assert.AreEqual(true, convertedPqFullSupportPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqFullSupportPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqFullSupportPvl.ValueDate);
        Assert.AreEqual(ExpectedOrdersCount, convertedPqFullSupportPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, convertedPqFullSupportPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, convertedPqFullSupportPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, convertedPqFullSupportPvl[0]!.GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, convertedPqFullSupportPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, convertedPqFullSupportPvl[0]!.UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, ((IAnonymousOrder)convertedPqFullSupportPvl[0]!).OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, convertedPqFullSupportPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(ExpectedCounterPartyName, ((IExternalCounterPartyOrder)convertedPqFullSupportPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedTraderName, ((IExternalCounterPartyOrder)convertedPqFullSupportPvl[0]!).ExternalTraderName);
    }
}
