// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Configuration;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
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

    private const int     ExpectedOrderId              = 203_123;
    private const decimal ExpectedOrderVolume          = 1_345_123;
    private const decimal ExpectedOrderRemainingVolume = 1_100_050;

    private const int ExpectedTraderId       = 2;
    private const int ExpectedCounterPartyId = 1;

    
    private const QuoteInstantBehaviorFlags      QuoteBehavior = QuoteInstantBehaviorFlags.None;
    private const QuoteLayerInstantBehaviorFlags LayerBehavior = QuoteLayerInstantBehaviorFlags.None;

    private const OrderGenesisFlags ExpectedGenesisFlags = OrderGenesisFlags.FromAdapter | OrderGenesisFlags.IsInternal;
    private const OrderType         ExpectedOrderType    = OrderType.PassiveLimit;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.ConfirmedActiveOnMarket;

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

    private PQFullSupportPriceVolumeLayer pqFullSupportPvl = null!;

    private readonly LayerType pqFullSupportPvlType = new PQFullSupportPriceVolumeLayer(nameIdGenerator).LayerType;

    private readonly LayerFlags pqFullSupportPvlTypeSupportedFlags = new PQFullSupportPriceVolumeLayer(nameIdGenerator).SupportsLayerFlags;
    private PQValueDatePriceVolumeLayer pqValueDatePriceVolumeLayer = null!;


    private PriceVolumeLayer               priceVolumeLayer               = null!;
    private SourcePriceVolumeLayer         sourcePriceVolumeLayer         = null!;
    private SourceQuoteRefPriceVolumeLayer sourceQuoteRefPriceVolumeLayer = null!;

    private FullSupportPriceVolumeLayer fullSupportPvl = null!;

    private readonly LayerType fullSupportPvlType = new FullSupportPriceVolumeLayer().LayerType;

    private readonly LayerFlags fullSupportPvlTypeSupportedFlags = new FullSupportPriceVolumeLayer().SupportsLayerFlags;
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
        ordersAnonPriceVolumeLayer = new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume)
        {
            [0] = new AnonymousOrder
                (ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags
               , ExpectedLifecycleState
               , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
        };
        ordersCounterPartyPriceVolumeLayer = new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume, internalVolume: ExpectedInternalVolume)
        {
            [0] = new ExternalCounterPartyOrder
                (new AnonymousOrder
                    (ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags
                   , ExpectedLifecycleState
                   , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
                    {
                        ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo(ExpectedCounterPartyId, ExpectedOrderCounterPartyName, ExpectedTraderId, ExpectedOrderTraderName)
                    } )
        };
        fullSupportPvl = new FullSupportPriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                              expectedValueDate, ExpectedSourceName, true, ExpectedQuoteRef, internalVolume: ExpectedInternalVolume)
        {
            [0] = new ExternalCounterPartyOrder
                (new AnonymousOrder
                    (ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags
                   , ExpectedLifecycleState
                   , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
                    {
                        ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo(ExpectedCounterPartyId, ExpectedOrderCounterPartyName, ExpectedTraderId, ExpectedOrderTraderName)
                    })
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
                [0] = new PQAnonymousOrder
                    (nameIdGenerator, ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState
                   , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
            };
        pqOrdersCounterPartyPriceVolumeLayer
            = new PQOrdersPriceVolumeLayer(nameIdGenerator.Clone(), LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume)
            {
                [0] = new PQExternalCounterPartyOrder
                    (new PQAnonymousOrder
                        (nameIdGenerator, ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags
                       , ExpectedLifecycleState
                       , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
                        {
                            ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(nameIdGenerator,  ExpectedCounterPartyId, ExpectedOrderCounterPartyName, ExpectedTraderId, ExpectedOrderTraderName)
                        }        
                    )
            };
        pqFullSupportPvl = new PQFullSupportPriceVolumeLayer(nameIdGenerator, ExpectedPrice, ExpectedVolume,
                                                                  expectedValueDate, ExpectedSourceName, true, ExpectedQuoteRef)
        {
            [0] = new PQExternalCounterPartyOrder
                (new PQAnonymousOrder
                    (nameIdGenerator, ExpectedOrderId, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderType, ExpectedGenesisFlags
                   , ExpectedLifecycleState
                   , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
                    {
                        ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(nameIdGenerator,  ExpectedCounterPartyId, ExpectedOrderCounterPartyName, ExpectedTraderId, ExpectedOrderTraderName)
                    }        
                )
        };


        ipqSourceTickerInfo =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, MarketClassification.Unknown
                  ,  AUinMEL, AUinMEL, AUinMEL
                   , 20, 0.00001m, 30000m, 50000000m, 1000m
                   , layerFlags: LayerFlags.Volume | LayerFlags.Price
                   , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                      LastTradedFlags.LastTradedTime));
        ipqSourceTickerInfo.NameIdLookup = nameIdGenerator;
    }

    [TestMethod]
    public void VariousLayerFlags_Select_ReturnsPriceVolumeLayerFactory()
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
    public void VariousLayerFlags_Select_ReturnsSourcePriceVolumeLayerFactory()
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
    public void VariousLayerFlags_Select_ReturnsSourceQuoteRefPriceVolumeLayerFactory()
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
    public void VariousLayerFlags_Select_ReturnValueDatePriceVolumeLayerFactory()
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
    public void VariousLayerFlags_Select_ReturnTraderPriceVolumeLayerFactory()
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
    public void VariousLayerFlags_Select_ReturnsFullSupportPvlFactory()
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
        var pqPvl = layerSelector.UpgradeExistingLayer(priceVolumeLayer, priceVolumeLayerType, LayerBehavior, priceVolumeLayer);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

        pqPvl = layerSelector.UpgradeExistingLayer(sourcePriceVolumeLayer, sourcePriceVolumeLayerType, LayerBehavior, sourcePriceVolumeLayer);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pqPvl = layerSelector.UpgradeExistingLayer(sourcePriceVolumeLayer, sourceQuoteRefPriceVolumeLayerType, LayerBehavior, sourcePriceVolumeLayer)
                             .CopyFrom(sourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcQtRefPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcQtRefPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pqPvl = layerSelector.UpgradeExistingLayer(valueDatePriceVolumeLayer, valueDatePriceVolumeLayerType, LayerBehavior, valueDatePriceVolumeLayer);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqVlDtPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqVlDtPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pqPvl = layerSelector.UpgradeExistingLayer(ordersCountPriceVolumeLayer, ordersCountPriceVolumeLayerType, LayerBehavior, ordersCountPriceVolumeLayer);
        var pqOrdCntPvl = pqPvl as PQOrdersCountPriceVolumeLayer;
        Assert.IsNotNull(pqOrdCntPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdCntPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdCntPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdCntPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdCntPvl.InternalVolume);

        pqPvl = layerSelector.UpgradeExistingLayer(ordersAnonPriceVolumeLayer, ordersAnonPriceVolumeLayerType, LayerBehavior, ordersAnonPriceVolumeLayer);
        var pqOrdersAnonPvl = pqPvl as PQOrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersAnonPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersAnonPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersAnonPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersAnonPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersAnonPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersAnonPvl[0].OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, pqOrdersAnonPvl[0].GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersAnonPvl[0].CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersAnonPvl[0].UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, pqOrdersAnonPvl[0].OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersAnonPvl[0].OrderRemainingVolume);

        pqPvl = layerSelector.UpgradeExistingLayer(ordersCounterPartyPriceVolumeLayer, ordersCounterPartyPriceVolumeLayerType, LayerBehavior
                                                 , ordersCounterPartyPriceVolumeLayer);
        var pqOrdersCpPvl = pqPvl as PQOrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersCpPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersCpPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersCpPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersCpPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersCpPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersCpPvl[0].OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, pqOrdersCpPvl[0].GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersCpPvl[0].CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersCpPvl[0].UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, pqOrdersCpPvl[0].OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersCpPvl[0].OrderRemainingVolume);
        Assert.AreEqual(ExpectedOrderCounterPartyName, ((IExternalCounterPartyOrder)pqOrdersCpPvl[0]).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedOrderTraderName, ((IExternalCounterPartyOrder)pqOrdersCpPvl[0]).ExternalTraderName);

        pqPvl = layerSelector.UpgradeExistingLayer(fullSupportPvl, fullSupportPvlType, LayerBehavior, fullSupportPvl);
        var convertedPqFullSupportPvl = pqPvl as PQFullSupportPriceVolumeLayer;
        Assert.IsNotNull(convertedPqFullSupportPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, convertedPqFullSupportPvl.SourceName);
        Assert.AreEqual(true, convertedPqFullSupportPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqFullSupportPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqFullSupportPvl.ValueDate);
        Assert.AreEqual(ExpectedOrdersCount, convertedPqFullSupportPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, convertedPqFullSupportPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, convertedPqFullSupportPvl[0].OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, convertedPqFullSupportPvl[0].GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, convertedPqFullSupportPvl[0].CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, convertedPqFullSupportPvl[0].UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, convertedPqFullSupportPvl[0].OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, convertedPqFullSupportPvl[0].OrderRemainingVolume);
        Assert.AreEqual(ExpectedOrderCounterPartyName, ((IExternalCounterPartyOrder)convertedPqFullSupportPvl[0]).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedOrderTraderName, ((IExternalCounterPartyOrder)convertedPqFullSupportPvl[0]).ExternalTraderName);
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_CreateExpectedImplementation_ClonesPQPriceVolumeLayerType()
    {
        var pqPvl = layerSelector.CreateExpectedImplementation(priceVolumeLayerType, nameIdGenerator, LayerBehavior).CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreNotSame(pqPriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

        pqPvl = layerSelector.CreateExpectedImplementation(sourcePriceVolumeLayerType, nameIdGenerator, LayerBehavior).CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreNotSame(pqSourcePriceVolumeLayer, pqSrcPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pqPvl = layerSelector.CreateExpectedImplementation(sourceQuoteRefPriceVolumeLayerType, nameIdGenerator, LayerBehavior)
                             .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreNotSame(pqSourceQuoteRefPriceVolumeLayer, pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcQtRefPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcQtRefPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pqPvl = layerSelector.CreateExpectedImplementation(valueDatePriceVolumeLayerType, nameIdGenerator, LayerBehavior).CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreNotSame(pqValueDatePriceVolumeLayer, pqVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqVlDtPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqVlDtPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pqPvl = layerSelector.CreateExpectedImplementation(ordersCountPriceVolumeLayerType, nameIdGenerator, LayerBehavior).CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        var pqOrdCntPvl = pqPvl as PQOrdersCountPriceVolumeLayer;
        Assert.IsNotNull(pqOrdCntPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdCntPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdCntPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdCntPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdCntPvl.InternalVolume);

        pqPvl = layerSelector.CreateExpectedImplementation(ordersAnonPriceVolumeLayerType, nameIdGenerator, LayerBehavior).CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        var pqOrdersAnonPvl = pqPvl as PQOrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersAnonPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersAnonPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersAnonPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersAnonPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersAnonPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersAnonPvl[0].OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, pqOrdersAnonPvl[0].GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersAnonPvl[0].CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersAnonPvl[0].UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, pqOrdersAnonPvl[0].OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersAnonPvl[0].OrderRemainingVolume);

        pqPvl = layerSelector.CreateExpectedImplementation(ordersCounterPartyPriceVolumeLayerType, nameIdGenerator, LayerBehavior)
                             .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        var pqOrdersCpPvl = pqPvl as PQOrdersPriceVolumeLayer;
        Assert.IsNotNull(pqOrdersCpPvl);
        Assert.AreEqual(ExpectedPrice, pqOrdersCpPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqOrdersCpPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqOrdersCpPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqOrdersCpPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqOrdersCpPvl[0].OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, pqOrdersCpPvl[0].GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqOrdersCpPvl[0].CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqOrdersCpPvl[0].UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, pqOrdersCpPvl[0].OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqOrdersCpPvl[0].OrderRemainingVolume);
        Assert.AreEqual(ExpectedOrderCounterPartyName, ((IExternalCounterPartyOrder)pqOrdersCpPvl[0]).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedOrderTraderName, ((IExternalCounterPartyOrder)pqOrdersCpPvl[0]).ExternalTraderName);

        pqPvl = layerSelector.CreateExpectedImplementation(fullSupportPvlType, nameIdGenerator, LayerBehavior).CopyFrom(pqFullSupportPvl, QuoteBehavior);
        var convertedPqFullSupportPvl = pqPvl as PQFullSupportPriceVolumeLayer;
        Assert.IsNotNull(convertedPqFullSupportPvl);
        Assert.AreNotSame(pqFullSupportPvl, convertedPqFullSupportPvl);
        Assert.AreNotSame(fullSupportPvl, convertedPqFullSupportPvl);
        Assert.AreEqual(ExpectedPrice, convertedPqFullSupportPvl.Price);
        Assert.AreEqual(ExpectedVolume, convertedPqFullSupportPvl.Volume);
        Assert.AreEqual(ExpectedSourceName, convertedPqFullSupportPvl.SourceName);
        Assert.AreEqual(true, convertedPqFullSupportPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqFullSupportPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqFullSupportPvl.ValueDate);
        Assert.AreEqual(ExpectedOrdersCount, convertedPqFullSupportPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, convertedPqFullSupportPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, convertedPqFullSupportPvl[0].OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, convertedPqFullSupportPvl[0].GenesisFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, convertedPqFullSupportPvl[0].CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, convertedPqFullSupportPvl[0].UpdateTime);
        Assert.AreEqual(ExpectedOrderVolume, convertedPqFullSupportPvl[0].OrderDisplayVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, convertedPqFullSupportPvl[0].OrderRemainingVolume);
        Assert.AreEqual(ExpectedOrderCounterPartyName, ((IExternalCounterPartyOrder)convertedPqFullSupportPvl[0]).ExternalCounterPartyName);
        Assert.AreEqual(ExpectedOrderTraderName, ((IExternalCounterPartyOrder)convertedPqFullSupportPvl[0]).ExternalTraderName);
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
        var ltFullSupportPvlFlags               = LayerType.FullSupportPriceVolume.SupportedLayerFlags();


        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, priceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, sourcePriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, sourceQuoteRefPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, valueDatePriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, ordersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, ordersCountPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, ordersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, ordersAnonPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, ordersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, ordersCounterPartyPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, fullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, fullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, fullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, fullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, fullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, fullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, fullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, fullSupportPvlTypeSupportedFlags));
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_TypeCanWhollyContain_ReturnsAsExpected()
    {
        var ltPriceVolumeLayerFlags                  = LayerType.PriceVolume.SupportedLayerFlags();
        var ltSourcePriceVolumeLayerFlags            = LayerType.SourcePriceVolume.SupportedLayerFlags();
        var ltSourceQuoteRefPriceVolumeLayerFlags    = LayerType.SourceQuoteRefPriceVolume.SupportedLayerFlags();
        var ltValueDatePriceVolumeLayerFlags         = LayerType.ValueDatePriceVolume.SupportedLayerFlags();
        var ltOrderCountPriceVolumeLayerFlags        = LayerType.OrdersCountPriceVolume.SupportedLayerFlags();
        var ltOrderAnonPriceVolumeLayerFlags         = LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags();
        var ltOrderCounterPartyPriceVolumeLayerFlags = LayerType.OrdersFullPriceVolume.SupportedLayerFlags();
        var ltFullSupportPvlFlags               = LayerType.FullSupportPriceVolume.SupportedLayerFlags();

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, pqPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, pqSourcePriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, pqValueDatePriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqOrdersCountPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, pqOrdersCountPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqOrdersAnonPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, pqOrdersAnonPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags
                                                           , pqOrdersCounterPartyPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, pqOrdersCounterPartyPriceVolumeSupportedFlags));

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqFullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqFullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqFullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltValueDatePriceVolumeLayerFlags, pqFullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCountPriceVolumeLayerFlags, pqFullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderAnonPriceVolumeLayerFlags, pqFullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltOrderCounterPartyPriceVolumeLayerFlags, pqFullSupportPvlTypeSupportedFlags));
        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltFullSupportPvlFlags, pqFullSupportPvlTypeSupportedFlags));
    }


    [TestMethod]
    public void NonPQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToMostCompactSupportedType()
    {
        IPriceVolumeLayer result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreSame(result, pqPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCountPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourcePriceVolumeLayerType).CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourcePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, sourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourcePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, sourcePriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior,  sourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, sourcePriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCountPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersCountPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCountPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCountPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCountPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCountPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, ordersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersAnonPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersAnonPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersAnonPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersAnonPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior,  ordersAnonPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, ordersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, fullSupportPvlType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, fullSupportPvlType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, fullSupportPvlType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, fullSupportPvlType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, fullSupportPvlType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, fullSupportPvlType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, fullSupportPvlType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, fullSupportPvlType);
        Assert.AreSame(result, pqFullSupportPvl);
    }

    [TestMethod]
    public void PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToFullSupportPVLIfCantContain()
    {
        IPriceVolumeLayer result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCountPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourcePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqFullSupportPvl, QuoteBehavior);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, pqSourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqValueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, pqValueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCountPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersCountPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCountPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCountPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCountPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCountPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, pqOrdersCountPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersAnonPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersAnonPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersAnonPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersAnonPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersAnonPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersAnonPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, pqOrdersAnonPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCounterPartyPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCounterPartyPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCounterPartyPriceVolumeLayerType);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQOrdersPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqOrdersCounterPartyPriceVolumeLayerType);
        Assert.AreSame(result, pqOrdersCounterPartyPriceVolumeLayer);

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, pqOrdersCounterPartyPriceVolumeLayerType);
        Assert.AreSame(result, pqFullSupportPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqFullSupportPvlType)
                              .CopyFrom(pqPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqFullSupportPvlType)
                              .CopyFrom(pqSourcePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqFullSupportPvlType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, LayerBehavior, pqFullSupportPvlType)
                              .CopyFrom(pqValueDatePriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCountPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqFullSupportPvlType)
                              .CopyFrom(pqOrdersCountPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCountPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCountPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersAnonPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqFullSupportPvlType)
                              .CopyFrom(pqOrdersAnonPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersAnonPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersAnonPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqOrdersCounterPartyPriceVolumeLayer, nameIdGenerator, LayerBehavior, pqFullSupportPvlType)
                              .CopyFrom(pqOrdersCounterPartyPriceVolumeLayer, QuoteBehavior);
        Assert.AreNotSame(result, pqOrdersCounterPartyPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQFullSupportPriceVolumeLayer));
        Assert.IsTrue(pqOrdersCounterPartyPriceVolumeLayer.AreEquivalent(result));

        result = layerSelector.UpgradeExistingLayer(pqFullSupportPvl, nameIdGenerator, LayerBehavior, pqFullSupportPvlType);
        Assert.AreSame(result, pqFullSupportPvl);
    }

    [TestMethod]
    public void NullPriceVolumeLayerEntries_SelectPriceDesiredLayerType_HandlesEmptyValues()
    {
        var result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, LayerBehavior, priceVolumeLayerType);
        Assert.AreEqual(typeof(PQPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, LayerBehavior, sourcePriceVolumeLayerType);
        Assert.AreEqual(typeof(PQSourcePriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, LayerBehavior, sourceQuoteRefPriceVolumeLayerType);
        Assert.AreEqual(typeof(PQSourceQuoteRefPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, LayerBehavior, valueDatePriceVolumeLayerType);
        Assert.AreEqual(typeof(PQValueDatePriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, LayerBehavior, ordersCounterPartyPriceVolumeLayerType);
        Assert.AreEqual(typeof(PQOrdersPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, LayerBehavior, fullSupportPvlType);
        Assert.AreEqual(typeof(PQFullSupportPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
    }
}
