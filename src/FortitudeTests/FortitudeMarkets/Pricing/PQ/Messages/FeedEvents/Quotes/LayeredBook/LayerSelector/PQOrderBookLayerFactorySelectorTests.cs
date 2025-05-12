// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQOrderBookLayerFactorySelectorTests
{
    private const decimal ExpectedPrice                 = 2.3456m;
    private const decimal ExpectedVolume                = 42_000_100m;
    private const string  ExpectedSourceName            = "SourceName-Wattle";
    private const uint    ExpectedQuoteRef              = 41_111_2222u;
    private const uint    ExpectedOrdersCount           = 1u;
    private const decimal ExpectedInternalVolume        = 1_100_050;
    private const string  ExpectedOrderCounterPartyName = "ChromoCon";
    private const string  ExpectedOrderTraderName       = "IronMan";

    private const int             ExpectedOrderId              = 203_123;
    private const LayerOrderFlags ExpectedOrderFlags           = LayerOrderFlags.ExplicitlyDefinedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;
    private const decimal         ExpectedOrderVolume          = 1_345_123;
    private const decimal         ExpectedOrderRemainingVolume = 1_100_050;

    private static readonly DateTime ExpectedOrderCreatedTime = new(2025, 4, 24, 7, 7, 23);
    private static readonly DateTime ExpectedOrderUpdatedTime = new(2025, 4, 24, 9, 12, 18);

    private static   IPQNameIdLookupGenerator nameIdGenerator = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
    private readonly LayerType ordersAnonPriceVolumeLayerType = new OrdersPriceVolumeLayer().LayerType;
    private readonly LayerFlags ordersAnonPriceVolumeSupportedFlags = new OrdersPriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType ordersCounterPartyPriceVolumeLayerType = new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume).LayerType;
    private readonly LayerFlags ordersCounterPartyPriceVolumeSupportedFlags
        = new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume).SupportsLayerFlags;
    private readonly LayerType  ordersCountPriceVolumeLayerType      = new OrdersCountPriceVolumeLayer().LayerType;
    private readonly LayerFlags ordersCountPriceVolumeSupportedFlags = new OrdersCountPriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType pqOrdersAnonPriceVolumeLayerType
        = new PQOrdersPriceVolumeLayer(nameIdGenerator, LayerType.OrdersAnonymousPriceVolume).LayerType;
    private readonly LayerFlags pqOrdersAnonPriceVolumeSupportedFlags
        = new PQOrdersPriceVolumeLayer(nameIdGenerator, LayerType.OrdersAnonymousPriceVolume).SupportsLayerFlags;
    private readonly LayerType pqOrdersCounterPartyPriceVolumeLayerType
        = new PQOrdersPriceVolumeLayer(nameIdGenerator, LayerType.OrdersFullPriceVolume).LayerType;
    private readonly LayerFlags pqOrdersCounterPartyPriceVolumeSupportedFlags
        = new PQOrdersPriceVolumeLayer(nameIdGenerator, LayerType.OrdersFullPriceVolume).SupportsLayerFlags;

    private readonly LayerType  pqOrdersCountPriceVolumeLayerType      = new PQOrdersCountPriceVolumeLayer().LayerType;
    private readonly LayerFlags pqOrdersCountPriceVolumeSupportedFlags = new PQOrdersCountPriceVolumeLayer().SupportsLayerFlags;

    private readonly LayerType  pqPriceVolumeLayerType      = new PQPriceVolumeLayer().LayerType;
    private readonly LayerFlags pqPriceVolumeSupportedFlags = new PQPriceVolumeLayer().SupportsLayerFlags;

    private readonly LayerType  pqSourcePriceVolumeLayerType              = new PQSourcePriceVolumeLayer(nameIdGenerator).LayerType;
    private readonly LayerFlags pqSourcePriceVolumeSupportedFlags         = new PQSourcePriceVolumeLayer(nameIdGenerator).SupportsLayerFlags;
    private readonly LayerType  pqSourceQuoteRefPriceVolumeLayerType      = new PQSourceQuoteRefPriceVolumeLayer(nameIdGenerator).LayerType;
    private readonly LayerFlags pqSourceQuoteRefPriceVolumeSupportedFlags = new PQSourceQuoteRefPriceVolumeLayer(nameIdGenerator).SupportsLayerFlags;
    private readonly LayerType  pqValueDatePriceVolumeLayerType           = new PQValueDatePriceVolumeLayer().LayerType;
    private readonly LayerFlags pqValueDatePriceVolumeSupportedFlags      = new PQValueDatePriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  priceVolumeLayerType                      = new PriceVolumeLayer().LayerType;
    private readonly LayerFlags priceVolumeSupportedFlags                 = new PriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  sourcePriceVolumeLayerType                = new SourcePriceVolumeLayer().LayerType;
    private readonly LayerFlags sourcePriceVolumeSupportedFlags           = new SourcePriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  sourceQuoteRefPriceVolumeLayerType        = new SourceQuoteRefPriceVolumeLayer().LayerType;
    private readonly LayerFlags sourceQuoteRefPriceVolumeSupportedFlags   = new SourceQuoteRefPriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  valueDatePriceVolumeLayerType             = new ValueDatePriceVolumeLayer().LayerType;
    private readonly LayerFlags valueDatePriceVolumeSupportedFlags        = new ValueDatePriceVolumeLayer().SupportsLayerFlags;

    private DateTime expectedValueDate;

    private IPQSourceTickerInfo ipqSourceTickerInfo = new PQSourceTickerInfo();

    private PQOrderBookLayerFactorySelector layerSelector                        = null!;
    private OrdersPriceVolumeLayer          ordersAnonPriceVolumeLayer           = null!;
    private OrdersPriceVolumeLayer          ordersCounterPartyPriceVolumeLayer   = null!;
    private OrdersCountPriceVolumeLayer     ordersCountPriceVolumeLayer          = null!;
    private PQOrdersPriceVolumeLayer        pqOrdersAnonPriceVolumeLayer         = null!;
    private PQOrdersPriceVolumeLayer        pqOrdersCounterPartyPriceVolumeLayer = null!;
    private PQOrdersCountPriceVolumeLayer   pqOrdersCountPriceVolumeLayer        = null!;

    private PQPriceVolumeLayer               pqPriceVolumeLayer               = null!;
    private PQSourcePriceVolumeLayer         pqSourcePriceVolumeLayer         = null!;
    private PQSourceQuoteRefPriceVolumeLayer pqSourceQuoteRefPriceVolumeLayer = null!;

    private PQFullSupportPriceVolumeLayer pqSrcQtRefTrdrVlDtPvl = null!;

    private LayerType pqSrcQtRefTrdrVlDtPvlType =
        new PQFullSupportPriceVolumeLayer(nameIdGenerator).LayerType;

    private LayerFlags pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags =
        new PQFullSupportPriceVolumeLayer(nameIdGenerator).SupportsLayerFlags;
    private PQValueDatePriceVolumeLayer pqValueDatePriceVolumeLayer = null!;


    private PriceVolumeLayer               priceVolumeLayer               = null!;
    private SourcePriceVolumeLayer         sourcePriceVolumeLayer         = null!;
    private SourceQuoteRefPriceVolumeLayer sourceQuoteRefPriceVolumeLayer = null!;

    private FullSupportPriceVolumeLayer srcQtRefTrdrVlDtPvl = null!;

    private LayerType srcQtRefTrdrVlDtPvlType =
        new FullSupportPriceVolumeLayer().LayerType;

    private LayerFlags srcQtRefTrdrVlDtPvlTypeSupportedFlags =
        new FullSupportPriceVolumeLayer().SupportsLayerFlags;
    private ValueDatePriceVolumeLayer valueDatePriceVolumeLayer = null!;


    [TestInitialize]
    public void SetUp()
    {
        nameIdGenerator   = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        layerSelector     = new PQOrderBookLayerFactorySelector(nameIdGenerator);
        expectedValueDate = new DateTime(2018, 01, 9, 22, 0, 0);

        nameIdGenerator.GetOrAddId(ExpectedSourceName);
        nameIdGenerator.GetOrAddId(ExpectedOrderCounterPartyName);
        nameIdGenerator.GetOrAddId(ExpectedOrderTraderName);

        priceVolumeLayer = new PriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        sourcePriceVolumeLayer =
            new SourcePriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedSourceName, true);
        sourceQuoteRefPriceVolumeLayer =
            new SourceQuoteRefPriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedSourceName, true, ExpectedQuoteRef);
        valueDatePriceVolumeLayer   = new ValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume, expectedValueDate);
        ordersCountPriceVolumeLayer = new OrdersCountPriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
        ordersAnonPriceVolumeLayer = new OrdersPriceVolumeLayer(price: ExpectedPrice, volume: ExpectedVolume)
        {
            [0] = new AnonymousOrderLayerInfo(ExpectedOrderId,
                                              ExpectedOrderFlags, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderUpdatedTime
                                            , ExpectedOrderRemainingVolume)
        };
        ordersCounterPartyPriceVolumeLayer = new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume)
        {
            [0] = new CounterPartyOrderLayerInfo(ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime,
                                                 ExpectedOrderVolume, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume,
                                                 ExpectedOrderCounterPartyName, ExpectedOrderTraderName)
        };
        srcQtRefTrdrVlDtPvl = new FullSupportPriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                                expectedValueDate, ExpectedSourceName, true, ExpectedQuoteRef)
        {
            [0] = new CounterPartyOrderLayerInfo(ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime,
                                                 ExpectedOrderVolume, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume,
                                                 ExpectedOrderCounterPartyName, ExpectedOrderTraderName)
        };

        pqPriceVolumeLayer = new PQPriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        pqSourcePriceVolumeLayer = new PQSourcePriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume,
                                                                ExpectedSourceName, true);
        pqSourceQuoteRefPriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume,
                                                                                ExpectedSourceName, true, ExpectedQuoteRef);
        pqValueDatePriceVolumeLayer   = new PQValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume, expectedValueDate);
        pqOrdersCountPriceVolumeLayer = new PQOrdersCountPriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
        pqOrdersAnonPriceVolumeLayer
            = new PQOrdersPriceVolumeLayer(nameIdGenerator.Clone(), LayerType.OrdersAnonymousPriceVolume, ExpectedPrice, ExpectedVolume)
            {
                [0] = new PQAnonymousOrderLayerInfo(ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime,
                                                    ExpectedOrderVolume, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
            };
        pqOrdersCounterPartyPriceVolumeLayer
            = new PQOrdersPriceVolumeLayer(nameIdGenerator.Clone(), LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume)
            {
                [0] = new PQCounterPartyOrderLayerInfo(nameIdGenerator.Clone(), ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime,
                                                       ExpectedOrderVolume, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume,
                                                       ExpectedOrderCounterPartyName, ExpectedOrderTraderName)
            };
        pqSrcQtRefTrdrVlDtPvl = new PQFullSupportPriceVolumeLayer(nameIdGenerator, ExpectedPrice, ExpectedVolume,
                                                                                    expectedValueDate, ExpectedSourceName, true, ExpectedQuoteRef)
        {
            [0] = new PQCounterPartyOrderLayerInfo(nameIdGenerator.Clone(), ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime,
                                                   ExpectedOrderVolume, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume,
                                                   ExpectedOrderCounterPartyName, ExpectedOrderTraderName)
        };


        ipqSourceTickerInfo =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
                   , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
                   , layerFlags: LayerFlags.Volume | LayerFlags.Price
                   , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                      LastTradedFlags.LastTradedTime));
        ipqSourceTickerInfo.NameIdLookup = nameIdGenerator;
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsPriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.None;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSourcePriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.SourceName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                         LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSourceQuoteRefPriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.IsInstanceOfType(pqRecentlyTradedFactory, typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                         LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                         LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                         LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                         LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                         LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                         LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                         LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnValueDatePriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.ValueDate;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.ValueDate;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.ValueDate;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                         LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnTraderPriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.OrderTraderName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQOrdersPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.OrderTraderName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQOrdersPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.OrderTraderName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQOrdersPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                         LayerFlags.OrderTraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQOrdersPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSrcQtRefTrdrVlDtPvlFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.OrderTraderName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.OrderTraderName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.OrderTraderName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.OrderTraderName | LayerFlags.Executable | LayerFlags.ValueDate;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Executable | LayerFlags.SourceName | LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName | LayerFlags.SourceQuoteReference | LayerFlags.OrderTraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName | LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName | LayerFlags.SourceQuoteReference
          | LayerFlags.ValueDate | LayerFlags.OrderTraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQFullSupportPriceVolumeLayerFactory));
    }


    [TestMethod]
    public void NonPQPriceVolumeLayerTypes_UpgradeExistingLayer_ConvertsToPQPriceVolumeLayerType()
    {
        var pqPvl = layerSelector.UpgradeExistingLayer(priceVolumeLayer, priceVolumeLayerType, priceVolumeLayer);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

        pqPvl = layerSelector.UpgradeExistingLayer(sourcePriceVolumeLayer, sourcePriceVolumeLayerType, sourcePriceVolumeLayer);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pqPvl = layerSelector.UpgradeExistingLayer(sourcePriceVolumeLayer, sourceQuoteRefPriceVolumeLayerType, sourcePriceVolumeLayer)
                             .CopyFrom(sourceQuoteRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcQtRefPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcQtRefPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pqPvl = layerSelector.UpgradeExistingLayer(valueDatePriceVolumeLayer, valueDatePriceVolumeLayerType, valueDatePriceVolumeLayer);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqVlDtPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqVlDtPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pqPvl = layerSelector.UpgradeExistingLayer(ordersCountPriceVolumeLayer, ordersCountPriceVolumeLayerType, ordersCountPriceVolumeLayer);
        var pqOrdCntPvl = pqPvl as PQOrdersCountPriceVolumeLayer;
        Assert.IsNotNull(pqOrdCntPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdCntPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdCntPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdCntPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdCntPvl.InternalVolume);

        pqPvl = layerSelector.UpgradeExistingLayer(ordersAnonPriceVolumeLayer, ordersAnonPriceVolumeLayerType, ordersAnonPriceVolumeLayer);
        var pqOrdersAnonPvl = pqPvl as PQOrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersAnonPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersAnonPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersAnonPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersAnonPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersAnonPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersAnonPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, pqOrdersAnonPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersAnonPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersAnonPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, pqOrdersAnonPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersAnonPvl[0]!.OrderRemainingVolume);

        pqPvl = layerSelector.UpgradeExistingLayer(ordersCounterPartyPriceVolumeLayer, ordersCounterPartyPriceVolumeLayerType
                                                 , ordersCounterPartyPriceVolumeLayer);
        var pqOrdersCntrPtyPvl = pqPvl as PQOrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersCntrPtyPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersCntrPtyPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersCntrPtyPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersCntrPtyPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersCntrPtyPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersCntrPtyPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, pqOrdersCntrPtyPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersCntrPtyPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersCntrPtyPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, pqOrdersCntrPtyPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersCntrPtyPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(ExpectedOrderCounterPartyName, ((ICounterPartyOrderLayerInfo)pqOrdersCntrPtyPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedOrderTraderName, ((ICounterPartyOrderLayerInfo)pqOrdersCntrPtyPvl[0]!).ExternalTraderName);

        pqPvl = layerSelector.UpgradeExistingLayer(srcQtRefTrdrVlDtPvl, srcQtRefTrdrVlDtPvlType, srcQtRefTrdrVlDtPvl);
        var convertedPqSrcQtRefTrdrVlDtPvl = pqPvl as PQFullSupportPriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
        Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
        Assert.AreEqual(ExpectedOrdersCount, convertedPqSrcQtRefTrdrVlDtPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, convertedPqSrcQtRefTrdrVlDtPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, convertedPqSrcQtRefTrdrVlDtPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, convertedPqSrcQtRefTrdrVlDtPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, convertedPqSrcQtRefTrdrVlDtPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, convertedPqSrcQtRefTrdrVlDtPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(ExpectedOrderCounterPartyName, ((ICounterPartyOrderLayerInfo)convertedPqSrcQtRefTrdrVlDtPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedOrderTraderName, ((ICounterPartyOrderLayerInfo)convertedPqSrcQtRefTrdrVlDtPvl[0]!).ExternalTraderName);
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_CreateExpectedImplementation_ClonesPQPriceVolumeLayerType()
    {
        var pqPvl = layerSelector.CreateExpectedImplementation(priceVolumeLayerType, nameIdGenerator).CopyFrom(pqPriceVolumeLayer);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreNotSame(pqPriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

        pqPvl = layerSelector.CreateExpectedImplementation(sourcePriceVolumeLayerType, nameIdGenerator).CopyFrom(pqSourcePriceVolumeLayer);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreNotSame(pqSourcePriceVolumeLayer, pqSrcPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pqPvl = layerSelector.CreateExpectedImplementation(sourceQuoteRefPriceVolumeLayerType, nameIdGenerator)
                             .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreNotSame(pqSourceQuoteRefPriceVolumeLayer, pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcQtRefPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcQtRefPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pqPvl = layerSelector.CreateExpectedImplementation(valueDatePriceVolumeLayerType, nameIdGenerator).CopyFrom(pqValueDatePriceVolumeLayer);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreNotSame(pqValueDatePriceVolumeLayer, pqVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqVlDtPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqVlDtPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pqPvl = layerSelector.CreateExpectedImplementation(ordersCountPriceVolumeLayerType, nameIdGenerator).CopyFrom(pqOrdersCountPriceVolumeLayer);
        var pqOrdCntPvl = pqPvl as PQOrdersCountPriceVolumeLayer;
        Assert.IsNotNull(pqOrdCntPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdCntPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdCntPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdCntPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdCntPvl.InternalVolume);

        pqPvl = layerSelector.CreateExpectedImplementation(ordersAnonPriceVolumeLayerType, nameIdGenerator).CopyFrom(pqOrdersAnonPriceVolumeLayer);
        var pqOrdersAnonPvl = pqPvl as PQOrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersAnonPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersAnonPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersAnonPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersAnonPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersAnonPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersAnonPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, pqOrdersAnonPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersAnonPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersAnonPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, pqOrdersAnonPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersAnonPvl[0]!.OrderRemainingVolume);

        pqPvl = layerSelector.CreateExpectedImplementation(ordersCounterPartyPriceVolumeLayerType, nameIdGenerator)
                             .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        var pqOrdersCntrPtyPvl = pqPvl as PQOrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersCntrPtyPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersCntrPtyPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersCntrPtyPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersCntrPtyPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersCntrPtyPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersCntrPtyPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, pqOrdersCntrPtyPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersCntrPtyPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersCntrPtyPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, pqOrdersCntrPtyPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersCntrPtyPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(ExpectedOrderCounterPartyName, ((ICounterPartyOrderLayerInfo)pqOrdersCntrPtyPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedOrderTraderName, ((ICounterPartyOrderLayerInfo)pqOrdersCntrPtyPvl[0]!).ExternalTraderName);

        pqPvl = layerSelector.CreateExpectedImplementation(srcQtRefTrdrVlDtPvlType, nameIdGenerator).CopyFrom(pqSrcQtRefTrdrVlDtPvl);
        var convertedPqSrcQtRefTrdrVlDtPvl = pqPvl as PQFullSupportPriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(pqSrcQtRefTrdrVlDtPvl, convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(srcQtRefTrdrVlDtPvl, convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreEqual(ExpectedPrice, convertedPqSrcQtRefTrdrVlDtPvl.Price);
        Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
        Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
        Assert.AreEqual(ExpectedOrdersCount, convertedPqSrcQtRefTrdrVlDtPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, convertedPqSrcQtRefTrdrVlDtPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, convertedPqSrcQtRefTrdrVlDtPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, convertedPqSrcQtRefTrdrVlDtPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, convertedPqSrcQtRefTrdrVlDtPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, convertedPqSrcQtRefTrdrVlDtPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(ExpectedOrderCounterPartyName, ((ICounterPartyOrderLayerInfo)convertedPqSrcQtRefTrdrVlDtPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedOrderTraderName, ((ICounterPartyOrderLayerInfo)convertedPqSrcQtRefTrdrVlDtPvl[0]!).ExternalTraderName);
    }

    [TestMethod]
    public void NonPQPriceVolumeLayerTypes_TypeCanWhollyContain_ReturnsAsExpected()
    {
        var ltPriceVolumeLayerFlags                  = LayerType.PriceVolume.SupportedLayerFlags();
        var ltSourcePriceVolumeLayerFlags            = LayerType.SourcePriceVolume.SupportedLayerFlags();
        var ltSourceQuoteRefPriceVolumeLayerFlags    = LayerType.SourceQuoteRefPriceVolume.SupportedLayerFlags();
        var ltValueDatePriceVolumeLayerFlags         = LayerType.ValueDatePriceVolume.SupportedLayerFlags();
        var ltOrderCountPriceVolumeLayerFlags        = LayerType.OrdersCountPriceVolume.SupportedLayerFlags();
        var ltOrderAnonPriceVolumeLayerFlags         = LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags();
        var ltOrderCounterPartyPriceVolumeLayerFlags = LayerType.OrdersFullPriceVolume.SupportedLayerFlags();
        var ltSrcQtRefTrdrVlDtPvlFlags               = LayerType.FullSupportPriceVolume.SupportedLayerFlags();


        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, priceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, sourcePriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, sourceQuoteRefPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, valueDatePriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, ordersCountPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, ordersAnonPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, ordersCounterPartyPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_TypeCanWholeyContain_ReturnsAsExpected()
    {
        var ltPriceVolumeLayerFlags                  = LayerType.PriceVolume.SupportedLayerFlags();
        var ltSourcePriceVolumeLayerFlags            = LayerType.SourcePriceVolume.SupportedLayerFlags();
        var ltSourceQuoteRefPriceVolumeLayerFlags    = LayerType.SourceQuoteRefPriceVolume.SupportedLayerFlags();
        var ltValueDatePriceVolumeLayerFlags         = LayerType.ValueDatePriceVolume.SupportedLayerFlags();
        var ltOrderCountPriceVolumeLayerFlags        = LayerType.OrdersCountPriceVolume.SupportedLayerFlags();
        var ltOrderAnonPriceVolumeLayerFlags         = LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags();
        var ltOrderCounterPartyPriceVolumeLayerFlags = LayerType.OrdersFullPriceVolume.SupportedLayerFlags();
        var ltSrcQtRefTrdrVlDtPvlFlags               = LayerType.FullSupportPriceVolume.SupportedLayerFlags();

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, pqPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, pqSourcePriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, pqValueDatePriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, pqOrdersCountPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, pqOrdersAnonPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags
                                                           , pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSrcQtRefTrdrVlDtPvlFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
    }


    [TestMethod]
    public void NonPQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToMostCompactSupportedType()
    {
        IPriceVolumeLayer result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCountPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourcePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, sourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, valueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, ordersCountPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersCountPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, ordersCountPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, ordersCountPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, ordersCountPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, ordersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCountPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, ordersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, ordersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, ordersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, ordersAnonPriceVolumeLayerType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, ordersAnonPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, ordersAnonPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, ordersAnonPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, ordersAnonPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, ordersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, ordersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, ordersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, srcQtRefTrdrVlDtPvlType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);
    }

    [TestMethod]
    public void PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToSrcQtRefTrdrVlDtPVLIfCantContain()
    {
        IPriceVolumeLayer result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCountPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourcePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqSrcQtRefTrdrVlDtPvl);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqValueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqOrdersCountPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersCountPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqOrdersCountPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqOrdersCountPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqOrdersCountPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, pqOrdersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCountPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, pqOrdersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, pqOrdersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqOrdersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqOrdersAnonPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqOrdersAnonPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqOrdersAnonPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqOrdersAnonPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, pqOrdersAnonPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, pqOrdersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, pqOrdersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqOrdersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqOrdersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqOrdersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqOrdersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqOrdersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, pqOrdersCounterPartyPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, pqOrdersCounterPartyPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, pqOrdersCounterPartyPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqOrdersCounterPartyPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);
    }

    [TestMethod]
    public void NullPriceVolumeLayerEntries_SelectPriceDesiredLayerType_HandlesEmptyValues()
    {
        var result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, priceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, sourcePriceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQSourcePriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQSourceQuoteRefPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, valueDatePriceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQValueDatePriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, ordersCounterPartyPriceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQOrdersPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, srcQtRefTrdrVlDtPvlType)!;
        Assert.AreEqual(typeof(PQFullSupportPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
    }
}
