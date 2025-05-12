// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class OrderBookLayerFactorySelectorTests
{
    private const decimal         ExpectedPrice                = 2.3456m;
    private const decimal         ExpectedVolume               = 420_100m;
    private const uint            ExpectedQuoteRef             = 41_111_2222u;
    private const uint            ExpectedOrdersCount          = 1;
    private const decimal         ExpectedInternalVolume       = 1_000_000;
    private const int             ExpectedOrderId              = 195_979;
    private const LayerOrderFlags ExpectedOrderFlags           = LayerOrderFlags.EstimatedFromOpenSnapshot | LayerOrderFlags.CalculatedAggregate;
    private const decimal         ExpectedOrderVolume          = ExpectedVolume * 2;
    private const decimal         ExpectedOrderRemainingVolume = ExpectedVolume;

    private static readonly DateTime ExpectedOrderCreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime ExpectedOrderUpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private readonly OrderBookLayerFactorySelector layerSelector            = new();
    private          string                        expectedCounterPartyName = null!;

    private string expectedSourceName = null!;
    private string expectedTraderName = null!;

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
    private PQSourceQuoteRefPriceVolumeLayer pqSourceQutoeRefPriceVolumeLayer = null!;

    private PQFullSupportPriceVolumeLayer pqSrcQtRefTrdrVlDtPvl       = null!;
    private PQValueDatePriceVolumeLayer                     pqValueDatePriceVolumeLayer = null!;

    private PriceVolumeLayer               priceVolumeLayer               = null!;
    private SourcePriceVolumeLayer         sourcePriceVolumeLayer         = null!;
    private SourceQuoteRefPriceVolumeLayer sourceQutoeRefPriceVolumeLayer = null!;
    private ISourceTickerInfo              sourceTickerInfo               = null!;

    private FullSupportPriceVolumeLayer srcQtRefTrdrVlDtPvl       = null!;
    private ValueDatePriceVolumeLayer                     valueDatePriceVolumeLayer = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdGenerator = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

        expectedTraderName       = "TraderName-Toly";
        expectedSourceName       = "SourceName-Latrobe";
        expectedCounterPartyName = "CounterParty-Chromo";

        expectedValueDate = new DateTime(2018, 01, 9, 22, 0, 0);

        priceVolumeLayer = new PriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        sourcePriceVolumeLayer = new SourcePriceVolumeLayer
            (ExpectedPrice, ExpectedVolume, expectedSourceName, true);
        sourceQutoeRefPriceVolumeLayer = new SourceQuoteRefPriceVolumeLayer
            (ExpectedPrice, ExpectedVolume, expectedSourceName, true, ExpectedQuoteRef);
        valueDatePriceVolumeLayer   = new ValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume, expectedValueDate);
        ordersCountPriceVolumeLayer = new OrdersCountPriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
        ordersAnonPriceVolumeLayer = new OrdersPriceVolumeLayer
            (LayerType.OrdersAnonymousPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume)
            {
                [0] = new AnonymousOrderLayerInfo
                    (ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime
                   , ExpectedOrderVolume, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume)
            };
        ordersCounterPartyPriceVolumeLayer = new OrdersPriceVolumeLayer
            (LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume)
            {
                [0] = new CounterPartyOrderLayerInfo
                    (ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderUpdatedTime
                   , ExpectedOrderRemainingVolume, expectedCounterPartyName, expectedTraderName)
            };
        srcQtRefTrdrVlDtPvl = new FullSupportPriceVolumeLayer
            (ExpectedPrice, ExpectedVolume, expectedValueDate, expectedSourceName, true, ExpectedQuoteRef, ExpectedOrdersCount
           , ExpectedInternalVolume)
            {
                [0] = new CounterPartyOrderLayerInfo
                    (ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime, ExpectedOrderVolume
                   , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, expectedCounterPartyName, expectedTraderName)
            };

        pqPriceVolumeLayer = new PQPriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        pqSourcePriceVolumeLayer = new PQSourcePriceVolumeLayer
            (nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume, expectedSourceName, true);
        pqSourceQutoeRefPriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer
            (nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume, expectedSourceName, true, ExpectedQuoteRef);
        pqValueDatePriceVolumeLayer = new PQValueDatePriceVolumeLayer
            (ExpectedPrice, ExpectedVolume, expectedValueDate);
        pqOrdersCountPriceVolumeLayer = new PQOrdersCountPriceVolumeLayer(ExpectedPrice, ExpectedVolume, ExpectedOrdersCount, ExpectedInternalVolume);
        pqAnonOrdersPriceVolumeLayer = new PQOrdersPriceVolumeLayer
            (nameIdGenerator.Clone(), LayerType.OrdersAnonymousPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount
           , ExpectedInternalVolume)
            {
                [0] = new PQAnonymousOrderLayerInfo
                    (ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime, ExpectedOrderVolume, ExpectedOrderUpdatedTime
                   , ExpectedOrderRemainingVolume)
            };
        pqCounterPartyOrdersPriceVolumeLayer = new PQOrdersPriceVolumeLayer
            (nameIdGenerator.Clone(), LayerType.OrdersFullPriceVolume, ExpectedPrice, ExpectedVolume, ExpectedOrdersCount
           , ExpectedInternalVolume)
            {
                [0] = new PQCounterPartyOrderLayerInfo
                    (nameIdGenerator.Clone(), ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime
                   , ExpectedOrderVolume, ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, expectedCounterPartyName
                   , expectedTraderName)
            };
        pqSrcQtRefTrdrVlDtPvl = new PQFullSupportPriceVolumeLayer
            (nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume, expectedValueDate, expectedSourceName, true, ExpectedQuoteRef
           , ExpectedOrdersCount, ExpectedInternalVolume)
            {
                [0] = new PQCounterPartyOrderLayerInfo
                    (nameIdGenerator.Clone(), ExpectedOrderId, ExpectedOrderFlags, ExpectedOrderCreatedTime, ExpectedOrderVolume
                   , ExpectedOrderUpdatedTime, ExpectedOrderRemainingVolume, expectedCounterPartyName, expectedTraderName)
            };

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void VariosLayerFlags_FindForLayerFlags_ReturnsPriceVolumeLayer()
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
    public void VariosLayerFlags_FindForLayerFlags_ReturnsSourcePriceVolumeLayer()
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
    public void VariosLayerFlags_FindForLayerFlags_ReturnsSourceQuoteRefPriceVolumeLayer()
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
    public void VariosLayerFlags_FindForLayerFlags_ReturnValueDatePriceVolumeLayer()
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
    public void VariosLayerFlags_FindForLayerFlags_ReturnTraderPriceVolumeLayer()
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
    public void VariosLayerFlags_FindForLayerFlags_ReturnsSrcQtRefTrdrVlDtPvl()
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
        Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.SourceQuoteRefPriceVolume, pqSourceQutoeRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pvl as SourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pqSrcQtRefPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqSrcQtRefPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
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
        Assert.AreEqual(ExpectedOrderFlags, pqAnonOrdersPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqAnonOrdersPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqAnonOrdersPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, pqAnonOrdersPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqAnonOrdersPvl[0]!.OrderRemainingVolume);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.OrdersFullPriceVolume, pqCounterPartyOrdersPriceVolumeLayer);
        var pqCpOrdersPvl = pvl as OrdersPriceVolumeLayer;
        Assert.IsNotNull(pqCpOrdersPvl);
        Assert.AreEqual(ExpectedPrice, pqCpOrdersPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqCpOrdersPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqCpOrdersPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqCpOrdersPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqCpOrdersPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, pqCpOrdersPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqCpOrdersPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqCpOrdersPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, pqCpOrdersPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqCpOrdersPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(expectedCounterPartyName, ((ICounterPartyOrderLayerInfo)pqCpOrdersPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(expectedTraderName, ((ICounterPartyOrderLayerInfo)pqCpOrdersPvl[0]!).ExternalTraderName);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.FullSupportPriceVolume, pqSrcQtRefTrdrVlDtPvl);
        var convertedPqSrcQtRefTrdrVlDtPvl = pvl as FullSupportPriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreEqual(ExpectedPrice, convertedPqSrcQtRefTrdrVlDtPvl.Price);
        Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl.Volume);
        Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
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
        Assert.AreEqual(expectedCounterPartyName, ((ICounterPartyOrderLayerInfo)convertedPqSrcQtRefTrdrVlDtPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(expectedTraderName, ((ICounterPartyOrderLayerInfo)convertedPqSrcQtRefTrdrVlDtPvl[0]!).ExternalTraderName);
    }

    [TestMethod]
    public void NonPQPriceVolumeLayerTypes_SelectPriceVolumeLayer_ReturnsUnchangeInstance()
    {
        var pvl = layerSelector.UpgradeExistingLayer(priceVolumeLayer, pqPriceVolumeLayer.LayerType);
        Assert.AreSame(priceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(sourcePriceVolumeLayer, sourcePriceVolumeLayer.LayerType);
        Assert.AreSame(sourcePriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(sourceQutoeRefPriceVolumeLayer, sourceQutoeRefPriceVolumeLayer.LayerType);
        Assert.AreSame(sourceQutoeRefPriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(valueDatePriceVolumeLayer, valueDatePriceVolumeLayer.LayerType);
        Assert.AreSame(valueDatePriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(ordersCountPriceVolumeLayer, ordersCountPriceVolumeLayer.LayerType);
        Assert.AreSame(ordersCountPriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(ordersAnonPriceVolumeLayer, ordersAnonPriceVolumeLayer.LayerType);
        Assert.AreSame(ordersAnonPriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(ordersCountPriceVolumeLayer, ordersCountPriceVolumeLayer.LayerType);
        Assert.AreSame(ordersCountPriceVolumeLayer, pvl);

        pvl = layerSelector.UpgradeExistingLayer(srcQtRefTrdrVlDtPvl, srcQtRefTrdrVlDtPvl.LayerType);
        Assert.AreSame(srcQtRefTrdrVlDtPvl, pvl);
    }

    [TestMethod]
    public void PriceVolumeLayerTypes_ConvertToExpectedImplementationCopyFrom_ReturnsCloneMatchingPriceVolumeLayerType()
    {
        var pvl = layerSelector.CreateExpectedImplementation(priceVolumeLayer.LayerType).CopyFrom(priceVolumeLayer);
        Assert.AreEqual(pvl.GetType(), typeof(PriceVolumeLayer));
        Assert.AreNotSame(sourceQutoeRefPriceVolumeLayer, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);

        pvl = layerSelector.CreateExpectedImplementation(sourcePriceVolumeLayer.LayerType).CopyFrom(sourcePriceVolumeLayer);
        var pqSrcPvl = pvl as SourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreNotSame(sourcePriceVolumeLayer, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pvl = layerSelector.CreateExpectedImplementation(sourceQutoeRefPriceVolumeLayer.LayerType).CopyFrom(sourceQutoeRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pvl as SourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreNotSame(sourceQutoeRefPriceVolumeLayer, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
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
        var pqTrdrPvl = pvl as OrdersPriceVolumeLayer;
        Assert.IsNotNull(pqTrdrPvl);
        Assert.AreNotSame(ordersAnonPriceVolumeLayer, pqTrdrPvl);
        Assert.AreEqual(ExpectedPrice, pqTrdrPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqTrdrPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, pqTrdrPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, pqTrdrPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, pqTrdrPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, pqTrdrPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, pqTrdrPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, pqTrdrPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, pqTrdrPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, pqTrdrPvl[0]!.OrderRemainingVolume);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.OrdersFullPriceVolume).CopyFrom(ordersCounterPartyPriceVolumeLayer);
        var countOrdersPvl = pvl as OrdersPriceVolumeLayer;
        Assert.IsNotNull(countOrdersPvl);
        Assert.AreEqual(ExpectedPrice, countOrdersPvl.Price);
        Assert.AreEqual(ExpectedVolume, countOrdersPvl.Volume);
        Assert.AreEqual(ExpectedOrdersCount, countOrdersPvl.OrdersCount);
        Assert.AreEqual(ExpectedInternalVolume, countOrdersPvl.InternalVolume);
        Assert.AreEqual(ExpectedOrderId, countOrdersPvl[0]!.OrderId);
        Assert.AreEqual(ExpectedOrderFlags, countOrdersPvl[0]!.OrderFlags);
        Assert.AreEqual(ExpectedOrderCreatedTime, countOrdersPvl[0]!.CreatedTime);
        Assert.AreEqual(ExpectedOrderUpdatedTime, countOrdersPvl[0]!.UpdatedTime);
        Assert.AreEqual(ExpectedOrderVolume, countOrdersPvl[0]!.OrderVolume);
        Assert.AreEqual(ExpectedOrderRemainingVolume, countOrdersPvl[0]!.OrderRemainingVolume);
        Assert.AreEqual(expectedCounterPartyName, ((ICounterPartyOrderLayerInfo)countOrdersPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(expectedTraderName, ((ICounterPartyOrderLayerInfo)countOrdersPvl[0]!).ExternalTraderName);

        pvl = layerSelector.CreateExpectedImplementation(srcQtRefTrdrVlDtPvl.LayerType).CopyFrom(srcQtRefTrdrVlDtPvl);
        var convertedPqSrcQtRefTrdrVlDtPvl = pvl as FullSupportPriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(srcQtRefTrdrVlDtPvl, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
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
        Assert.AreEqual(expectedCounterPartyName, ((ICounterPartyOrderLayerInfo)convertedPqSrcQtRefTrdrVlDtPvl[0]!).ExternalCounterPartyName);
        Assert.AreEqual(expectedTraderName, ((ICounterPartyOrderLayerInfo)convertedPqSrcQtRefTrdrVlDtPvl[0]!).ExternalTraderName);
    }
}
