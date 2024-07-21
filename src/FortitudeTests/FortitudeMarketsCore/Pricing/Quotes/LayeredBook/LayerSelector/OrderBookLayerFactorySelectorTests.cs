// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class OrderBookLayerFactorySelectorTests
{
    private const decimal ExpectedPrice    = 2.3456m;
    private const decimal ExpectedVolume   = 42_000_100m;
    private const uint    ExpectedQuoteRef = 41_111_2222u;

    private readonly OrderBookLayerFactorySelector layerSelector = new();

    private string   expectedSourceName = null!;
    private string   expectedTraderName = null!;
    private DateTime expectedValueDate;

    private IPQNameIdLookupGenerator nameIdGenerator = new PQNameIdLookupGenerator(
                                                                                   PQFieldKeys.LayerNameDictionaryUpsertCommand);

    private PQPriceVolumeLayer               pqPriceVolumeLayer               = null!;
    private PQSourcePriceVolumeLayer         pqSourcePriceVolumeLayer         = null!;
    private PQSourceQuoteRefPriceVolumeLayer pqSourceQutoeRefPriceVolumeLayer = null!;

    private PQSourceQuoteRefTraderValueDatePriceVolumeLayer pqSrcQtRefTrdrVlDtPvl = null!;

    private PQTraderPriceVolumeLayer    pqTraderPriceVolumeLayer    = null!;
    private PQValueDatePriceVolumeLayer pqValueDatePriceVolumeLayer = null!;

    private PriceVolumeLayer               priceVolumeLayer               = null!;
    private SourcePriceVolumeLayer         sourcePriceVolumeLayer         = null!;
    private SourceQuoteRefPriceVolumeLayer sourceQutoeRefPriceVolumeLayer = null!;
    private ISourceTickerInfo              sourceTickerInfo               = null!;

    private SourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvl = null!;

    private TraderPriceVolumeLayer    traderPriceVolumeLayer    = null!;
    private ValueDatePriceVolumeLayer valueDatePriceVolumeLayer = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdGenerator = new PQNameIdLookupGenerator(
                                                      PQFieldKeys.LayerNameDictionaryUpsertCommand);
        expectedTraderName = "TraderName-Toly";
        expectedSourceName = "SourceName-Latrobe";
        expectedValueDate  = new DateTime(2018, 01, 9, 22, 0, 0);

        priceVolumeLayer = new PriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        sourcePriceVolumeLayer = new SourcePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                            expectedSourceName, true);
        sourceQutoeRefPriceVolumeLayer = new SourceQuoteRefPriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                            expectedSourceName, true, ExpectedQuoteRef);
        valueDatePriceVolumeLayer = new ValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                  expectedValueDate);
        traderPriceVolumeLayer = new TraderPriceVolumeLayer(ExpectedPrice, ExpectedVolume)
        {
            [0] = new TraderLayerInfo(expectedTraderName, ExpectedVolume)
        };
        srcQtRefTrdrVlDtPvl = new SourceQuoteRefTraderValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                                expectedValueDate, expectedSourceName, true, ExpectedQuoteRef)
        {
            [0] = new TraderLayerInfo(expectedTraderName, ExpectedVolume)
        };

        pqPriceVolumeLayer = new PQPriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        pqSourcePriceVolumeLayer = new PQSourcePriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume,
                                                                expectedSourceName, true);
        pqSourceQutoeRefPriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume,
                                                                                expectedSourceName, true, ExpectedQuoteRef);
        pqValueDatePriceVolumeLayer = new PQValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                      expectedValueDate);
        pqTraderPriceVolumeLayer = new PQTraderPriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume)
        {
            [0] = new PQTraderLayerInfo(nameIdGenerator.Clone(), expectedTraderName, ExpectedVolume)
        };
        pqSrcQtRefTrdrVlDtPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume
                                                                                  , expectedValueDate, expectedSourceName, true, ExpectedQuoteRef)
        {
            [0] = new PQTraderLayerInfo(nameIdGenerator.Clone(), expectedTraderName, ExpectedVolume)
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
        sourceTickerInfo.LayerFlags = LayerFlags.TraderName;
        var pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(TraderPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.TraderName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(TraderPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.TraderName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(TraderPriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.TraderName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(), typeof(TraderPriceVolumeLayer));
    }

    [TestMethod]
    public void VariosLayerFlags_FindForLayerFlags_ReturnsSrcQtRefTrdrVlDtPvl()
    {
        sourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        var pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.TraderName;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.ValueDate | LayerFlags.SourceName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable | LayerFlags.ValueDate;
        pvl                         = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference |
                                      LayerFlags.ValueDate | LayerFlags.TraderName;
        pvl = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(pvl.GetType(),
                        typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
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
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.SourceQuoteRefPriceVolume, pqSourceQutoeRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pvl as SourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.ValueDatePriceVolume, pqValueDatePriceVolumeLayer);
        var pqVlDtPvl = pvl as ValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.TraderPriceVolume, pqTraderPriceVolumeLayer);
        var pqTrdrPvl = pvl as TraderPriceVolumeLayer;
        Assert.IsNotNull(pqTrdrPvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedTraderName, pqTrdrPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, pqTrdrPvl[0]!.TraderVolume);

        pvl = layerSelector.CreateExpectedImplementation(LayerType.SourceQuoteRefTraderValueDatePriceVolume, pqSrcQtRefTrdrVlDtPvl);
        var convertedPqSrcQtRefTrdrVlDtPvl = pvl as SourceQuoteRefTraderValueDatePriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
        Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
        Assert.AreEqual(expectedTraderName, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderVolume);
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

        pvl = layerSelector.UpgradeExistingLayer(traderPriceVolumeLayer, traderPriceVolumeLayer.LayerType);
        Assert.AreSame(traderPriceVolumeLayer, pvl);

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

        pvl = layerSelector.CreateExpectedImplementation(traderPriceVolumeLayer.LayerType).CopyFrom(traderPriceVolumeLayer);
        var pqTrdrPvl = pvl as TraderPriceVolumeLayer;
        Assert.IsNotNull(pqTrdrPvl);
        Assert.AreNotSame(traderPriceVolumeLayer, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedTraderName, pqTrdrPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, pqTrdrPvl[0]!.TraderVolume);

        pvl = layerSelector.CreateExpectedImplementation(srcQtRefTrdrVlDtPvl.LayerType).CopyFrom(srcQtRefTrdrVlDtPvl);
        var convertedPqSrcQtRefTrdrVlDtPvl = pvl as SourceQuoteRefTraderValueDatePriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(srcQtRefTrdrVlDtPvl, pvl);
        Assert.AreEqual(ExpectedPrice, pvl.Price);
        Assert.AreEqual(ExpectedVolume, pvl.Volume);
        Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
        Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
        Assert.AreEqual(expectedTraderName, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderVolume);
    }
}
