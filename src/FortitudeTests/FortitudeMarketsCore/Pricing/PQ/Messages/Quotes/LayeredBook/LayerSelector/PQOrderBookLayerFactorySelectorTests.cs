#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQOrderBookLayerFactorySelectorTests
{
    private const decimal ExpectedPrice = 2.3456m;
    private const decimal ExpectedVolume = 42_000_111m;
    private const uint ExpectedQuoteRef = 41_111_2222u;

    private string expectedSourceName = null!;
    private string expectedTraderName = null!;
    private DateTime expectedValueDate;

    private PQOrderBookLayerFactorySelector layerSelector = null!;

    private IPQNameIdLookupGenerator nameIdGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);

    private PQPriceVolumeLayer pqPriceVolumeLayer = null!;
    private PQSourcePriceVolumeLayer pqSourcePriceVolumeLayer = null!;
    private PQSourceQuoteRefPriceVolumeLayer pqSourceQutoeRefPriceVolumeLayer = null!;
    private IPQSourceTickerQuoteInfo pqSourceTickerQuoteInfo = new PQSourceTickerQuoteInfo();
    private PQSourceQuoteRefTraderValueDatePriceVolumeLayer pqSrcQtRefTrdrVlDtPvl = null!;
    private PQTraderPriceVolumeLayer pqTraderPriceVolumeLayer = null!;
    private PQValueDatePriceVolumeLayer pqValueDatePriceVolumeLayer = null!;

    private PriceVolumeLayer priceVolumeLayer = null!;
    private SourcePriceVolumeLayer sourcePriceVolumeLayer = null!;
    private SourceQuoteRefPriceVolumeLayer sourceQutoeRefPriceVolumeLayer = null!;
    private SourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvl = null!;
    private TraderPriceVolumeLayer traderPriceVolumeLayer = null!;
    private ValueDatePriceVolumeLayer valueDatePriceVolumeLayer = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        layerSelector = new PQOrderBookLayerFactorySelector(nameIdGenerator);
        expectedTraderName = "TraderName-Leila";
        expectedSourceName = "SourceName-Wattle";
        expectedValueDate = new DateTime(2018, 01, 9, 22, 0, 0);
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
        pqSrcQtRefTrdrVlDtPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nameIdGenerator, ExpectedPrice, ExpectedVolume,
            expectedValueDate, expectedSourceName, true, ExpectedQuoteRef)
        {
            [0] = new PQTraderLayerInfo(nameIdGenerator.Clone(), expectedTraderName, ExpectedVolume)
        };


        pqSourceTickerQuoteInfo = new PQSourceTickerQuoteInfo(new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                                  LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime));
        pqSourceTickerQuoteInfo.NameIdLookup = nameIdGenerator;
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsPriceVolumeLayerFactory()
    {
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.None;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSourcePriceVolumeLayerFactory()
    {
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));

        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));

        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                             LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSourceQuoteRefPriceVolumeLayerFactory()
    {
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceQuoteReference;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));

        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                             LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                             LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                             LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));

        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                             LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                             LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                             LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                             LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnValueDatePriceVolumeLayerFactory()
    {
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.ValueDate;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                             LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnTraderPriceVolumeLayerFactory()
    {
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.TraderName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQTraderPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQTraderPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQTraderPriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                             LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQTraderPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSrcQtRefTrdrVlDtPvlFactory()
    {
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                             LayerFlags.ValueDate | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));

        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable |
                                             LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                             LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));

        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                             LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName |
                                             LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName |
                                             LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                             LayerFlags.SourceName | LayerFlags.SourceQuoteReference |
                                             LayerFlags.ValueDate | LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
            typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
    }


    [TestMethod]
    public void NonPQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ConvertsToPQPriceVolumeLayerType()
    {
        var pqPvl = layerSelector.ConvertToExpectedImplementation(priceVolumeLayer, nameIdGenerator);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

        pqPvl = layerSelector.ConvertToExpectedImplementation(sourcePriceVolumeLayer, nameIdGenerator);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pqPvl = layerSelector.ConvertToExpectedImplementation(sourceQutoeRefPriceVolumeLayer, nameIdGenerator);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pqPvl = layerSelector.ConvertToExpectedImplementation(valueDatePriceVolumeLayer, nameIdGenerator);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pqPvl = layerSelector.ConvertToExpectedImplementation(traderPriceVolumeLayer, nameIdGenerator);
        var pqTrdrPvl = pqPvl as PQTraderPriceVolumeLayer;
        Assert.IsNotNull(pqTrdrPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedTraderName, pqTrdrPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, pqTrdrPvl[0]!.TraderVolume);

        pqPvl = layerSelector.ConvertToExpectedImplementation(srcQtRefTrdrVlDtPvl, nameIdGenerator);
        var convertedPqSrcQtRefTrdrVlDtPvl = pqPvl as PQSourceQuoteRefTraderValueDatePriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
        Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
        Assert.AreEqual(expectedTraderName, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderVolume);
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ClonesPQPriceVolumeLayerType()
    {
        var pqPvl = layerSelector.ConvertToExpectedImplementation(pqPriceVolumeLayer, nameIdGenerator, true);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreNotSame(pqPriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqSourcePriceVolumeLayer, nameIdGenerator, true);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreNotSame(pqSourcePriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, true);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreNotSame(pqSourceQutoeRefPriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqValueDatePriceVolumeLayer, nameIdGenerator, true);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreNotSame(pqValueDatePriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqTraderPriceVolumeLayer, nameIdGenerator, true);
        var pqTrdrPvl = pqPvl as PQTraderPriceVolumeLayer;
        Assert.IsNotNull(pqTrdrPvl);
        Assert.AreNotSame(pqTraderPriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedTraderName, pqTrdrPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, pqTrdrPvl[0]!.TraderVolume);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, true);
        var convertedPqSrcQtRefTrdrVlDtPvl = pqPvl as PQSourceQuoteRefTraderValueDatePriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(pqSrcQtRefTrdrVlDtPvl, pqPvl);
        Assert.AreNotSame(srcQtRefTrdrVlDtPvl, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
        Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
        Assert.AreEqual(expectedTraderName, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderVolume);
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ReturnsSamePQPriceVolumeLayerType()
    {
        var pqPvl = layerSelector.ConvertToExpectedImplementation(pqPriceVolumeLayer, nameIdGenerator);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreSame(pqPriceVolumeLayer, pqPvl);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqSourcePriceVolumeLayer, nameIdGenerator);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreSame(pqSourcePriceVolumeLayer, pqPvl);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreSame(pqSourceQutoeRefPriceVolumeLayer, pqPvl);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqValueDatePriceVolumeLayer, nameIdGenerator);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreSame(pqValueDatePriceVolumeLayer, pqPvl);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqTraderPriceVolumeLayer, nameIdGenerator);
        var pqTrdrPvl = pqPvl as PQTraderPriceVolumeLayer;
        Assert.IsNotNull(pqTrdrPvl);
        Assert.AreSame(pqTraderPriceVolumeLayer, pqPvl);

        pqPvl = layerSelector.ConvertToExpectedImplementation(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator);
        var convertedPqSrcQtRefTrdrVlDtPvl = pqPvl as PQSourceQuoteRefTraderValueDatePriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreSame(pqSrcQtRefTrdrVlDtPvl, pqPvl);
    }

    [TestMethod]
    public void NonPQPriceVolumeLayerTypes_TypeCanWholeyContain_ReturnsAsExpected()
    {
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer), typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourcePriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefPriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(ValueDatePriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(TraderPriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(SourcePriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefPriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(ValueDatePriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(TraderPriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(SourcePriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefPriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(ValueDatePriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(TraderPriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourcePriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefPriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(ValueDatePriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(TraderPriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourcePriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefPriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(ValueDatePriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(TraderPriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(SourcePriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefPriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(ValueDatePriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(TraderPriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_TypeCanWholeyContain_ReturnsAsExpected()
    {
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQPriceVolumeLayer), typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourcePriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefPriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQValueDatePriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQTraderPriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQPriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQSourcePriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefPriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQValueDatePriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQTraderPriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQSourcePriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQSourcePriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefPriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQValueDatePriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQTraderPriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQSourceQuoteRefPriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourcePriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefPriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQValueDatePriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQTraderPriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQValueDatePriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourcePriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefPriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQValueDatePriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQTraderPriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));
        Assert.IsFalse(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQTraderPriceVolumeLayer)));

        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQSourcePriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefPriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQValueDatePriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQTraderPriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
        Assert.IsTrue(layerSelector.TypeCanWholeyContain(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer),
            typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer)));
    }

    [TestMethod]
    public void NonPQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToSrcQtRefTrdrVlDtPVLIfCantContain()
    {
        var result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, priceVolumeLayer);
        Assert.AreSame(result, pqPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, priceVolumeLayer);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, priceVolumeLayer);
        Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, priceVolumeLayer);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, priceVolumeLayer);
        Assert.AreSame(result, pqTraderPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, priceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayer);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayer);
        Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, sourcePriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, sourceQutoeRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, sourceQutoeRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator,
            sourceQutoeRefPriceVolumeLayer);
        Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, sourceQutoeRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, sourceQutoeRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, sourceQutoeRefPriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayer);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, valueDatePriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayer);
        Assert.AreSame(result, pqTraderPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, traderPriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, srcQtRefTrdrVlDtPvl);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);
    }

    [TestMethod]
    public void PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToSrcQtRefTrdrVlDtPVLIfCantContain()
    {
        var result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayer);
        Assert.AreSame(result, pqPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayer);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayer);
        Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayer);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayer);
        Assert.AreSame(result, pqTraderPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqPriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayer);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayer);
        Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSourcePriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, pqSourceQutoeRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSourceQutoeRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, pqSourceQutoeRefPriceVolumeLayer);
        Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSourceQutoeRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqSourceQutoeRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSourceQutoeRefPriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayer);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqValueDatePriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayer);
        Assert.AreSame(result, pqTraderPriceVolumeLayer);
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqTraderPriceVolumeLayer);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSrcQtRefTrdrVlDtPvl);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);
    }

    [TestMethod]
    public void NullPriceVolumeLayerEntries_SelectPriceVolumeLayer_HandlesEmptyValues()
    {
        var result = layerSelector.SelectPriceVolumeLayer(null, nameIdGenerator, priceVolumeLayer)!;
        Assert.AreEqual(typeof(PQPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, nameIdGenerator, null);
        Assert.IsNull(result);
    }
}
