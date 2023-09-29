using System;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    [TestClass]
    public class PQOrderBookLayerFactorySelectorTests
    {
        private IPQSourceTickerQuoteInfo pqSourceTickerQuoteInfo;
        readonly IPQNameIdLookupGenerator sourceNameIdGenerator = new PQNameIdLookupGenerator(
            PQFieldKeys.LayerNameDictionaryUpsertCommand, PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        readonly IPQNameIdLookupGenerator traderNameIdGenerator = new PQNameIdLookupGenerator(
            PQFieldKeys.LayerNameDictionaryUpsertCommand, PQFieldFlags.TraderNameIdLookupSubDictionaryKey);

        readonly PQOrderBookLayerFactorySelector layerSelector = new PQOrderBookLayerFactorySelector();

        private const decimal ExpectedPrice = 2.3456m;
        private const decimal ExpectedVolume = 42_000_111m;
        private const uint ExpectedQuoteRef = 41_111_2222u;
        private DateTime expectedValueDate;
        private string expectedTraderName;
        private string expectedSourceName;

        private PriceVolumeLayer priceVolumeLayer;
        private SourcePriceVolumeLayer sourcePriceVolumeLayer;
        private SourceQuoteRefPriceVolumeLayer sourceQutoeRefPriceVolumeLayer;
        private ValueDatePriceVolumeLayer valueDatePriceVolumeLayer;
        private TraderPriceVolumeLayer traderPriceVolumeLayer;
        private SourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvl;

        private PQPriceVolumeLayer pqPriceVolumeLayer;
        private PQSourcePriceVolumeLayer pqSourcePriceVolumeLayer;
        private PQSourceQuoteRefPriceVolumeLayer pqSourceQutoeRefPriceVolumeLayer;
        private PQValueDatePriceVolumeLayer pqValueDatePriceVolumeLayer;
        private PQTraderPriceVolumeLayer pqTraderPriceVolumeLayer;
        private PQSourceQuoteRefTraderValueDatePriceVolumeLayer pqSrcQtRefTrdrVlDtPvl;

        [TestInitialize]
        public void SetUp()
        {
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
            pqSourcePriceVolumeLayer = new PQSourcePriceVolumeLayer(ExpectedPrice, ExpectedVolume, null,
                                                                expectedSourceName, true);
            pqSourceQutoeRefPriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                null, expectedSourceName, true, ExpectedQuoteRef);
            pqValueDatePriceVolumeLayer = new PQValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume, 
                                                                        expectedValueDate);
            pqTraderPriceVolumeLayer = new PQTraderPriceVolumeLayer(ExpectedPrice, ExpectedVolume)
            {
                [0] = new PQTraderLayerInfo(null, expectedTraderName, ExpectedVolume)
            };
            pqSrcQtRefTrdrVlDtPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                null, null, expectedValueDate, expectedSourceName, true, ExpectedQuoteRef)
            {
                [0] = new PQTraderLayerInfo(null, expectedTraderName, ExpectedVolume)
            };


            pqSourceTickerQuoteInfo = new PQSourceTickerQuoteInfo(new SourceTickerQuoteInfo(uint.MaxValue, "TestSource",
                "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime));
            pqSourceTickerQuoteInfo.SourceNameIdLookup = sourceNameIdGenerator;
            pqSourceTickerQuoteInfo.TraderNameIdLookup = traderNameIdGenerator;
        }

        [TestMethod]
        public void VariosLayerFlags_Select_ReturnsPriceVolumeLayerFactory()
        {
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.None;
            var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PriceVolumeLayerFactory));
        }

        [TestMethod]
        public void VariosLayerFlags_Select_ReturnsSourcePriceVolumeLayerFactory()
        {
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceName;
            var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));

            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));

            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                 LayerFlags.Executable | LayerFlags.SourceName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourcePriceVolumeLayerFactory));
        }

        [TestMethod]
        public void VariosLayerFlags_Select_ReturnsSourceQuoteRefPriceVolumeLayerFactory()
        {
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceQuoteReference;
            var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));

            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                                 LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                                 LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                 LayerFlags.Executable | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));

            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                                 LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                                 LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                                 LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable | 
                                                 LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(SourceQuoteRefPriceVolumeLayerFactory));
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
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(TraderPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.TraderName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(TraderPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.TraderName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(TraderPriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                 LayerFlags.TraderName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(TraderPriceVolumeLayerFactory));
        }

        [TestMethod]
        public void VariosLayerFlags_Select_ReturnsSrcQtRefTrdrVlDtPvlFactory()
        {
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
            var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.TraderName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                 LayerFlags.ValueDate | LayerFlags.SourceName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));

            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable |
                                                 LayerFlags.SourceQuoteReference;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable | LayerFlags.ValueDate;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable | 
                                                 LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));

            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                                 LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName | 
                                                 LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName | 
                                                 LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
            pqSourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable | 
                LayerFlags.SourceName | LayerFlags.SourceQuoteReference | LayerFlags.ValueDate | LayerFlags.TraderName;
            pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(pqSourceTickerQuoteInfo);
            Assert.AreEqual(pqRecentlyTradedFactory.GetType(), 
                typeof(SourceQuoteRefTraderValueDatePriceVolumeLayerFactory));
        }
        

        [TestMethod]
        public void NonPQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ConvertsToPQPriceVolumeLayerType()
        {
            var pqPvl = layerSelector.ConvertToExpectedImplementation(priceVolumeLayer);
            Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

            pqPvl = layerSelector.ConvertToExpectedImplementation(sourcePriceVolumeLayer);
            var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
            Assert.IsNotNull(pqSrcPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
            Assert.AreEqual(true, pqSrcPvl.Executable);

            pqPvl = layerSelector.ConvertToExpectedImplementation(sourceQutoeRefPriceVolumeLayer);
            var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
            Assert.IsNotNull(pqSrcQtRefPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
            Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
            Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

            pqPvl = layerSelector.ConvertToExpectedImplementation(valueDatePriceVolumeLayer);
            var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
            Assert.IsNotNull(pqVlDtPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

            pqPvl = layerSelector.ConvertToExpectedImplementation(traderPriceVolumeLayer);
            var pqTrdrPvl = pqPvl as PQTraderPriceVolumeLayer;
            Assert.IsNotNull(pqTrdrPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedTraderName, pqTrdrPvl[0].TraderName);
            Assert.AreEqual(ExpectedVolume, pqTrdrPvl[0].TraderVolume);

            pqPvl = layerSelector.ConvertToExpectedImplementation(srcQtRefTrdrVlDtPvl);
            var convertedPqSrcQtRefTrdrVlDtPvl = pqPvl as PQSourceQuoteRefTraderValueDatePriceVolumeLayer;
            Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
            Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
            Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
            Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
            Assert.AreEqual(expectedTraderName, convertedPqSrcQtRefTrdrVlDtPvl[0].TraderName);
            Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl[0].TraderVolume);
        }

        [TestMethod]
        public void PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ClonesPQPriceVolumeLayerType()
        {
            var pqPvl = layerSelector.ConvertToExpectedImplementation(pqPriceVolumeLayer, true);
            Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
            Assert.AreNotSame(pqPriceVolumeLayer, pqPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqSourcePriceVolumeLayer, true);
            var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
            Assert.IsNotNull(pqSrcPvl);
            Assert.AreNotSame(pqSourcePriceVolumeLayer, pqPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
            Assert.AreEqual(true, pqSrcPvl.Executable);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqSourceQutoeRefPriceVolumeLayer, true);
            var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
            Assert.IsNotNull(pqSrcQtRefPvl);
            Assert.AreNotSame(pqSourceQutoeRefPriceVolumeLayer, pqPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
            Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
            Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqValueDatePriceVolumeLayer, true);
            var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
            Assert.IsNotNull(pqVlDtPvl);
            Assert.AreNotSame(pqValueDatePriceVolumeLayer, pqPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqTraderPriceVolumeLayer, true);
            var pqTrdrPvl = pqPvl as PQTraderPriceVolumeLayer;
            Assert.IsNotNull(pqTrdrPvl);
            Assert.AreNotSame(pqTraderPriceVolumeLayer, pqPvl);
            Assert.AreEqual(ExpectedPrice, pqPvl.Price);
            Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
            Assert.AreEqual(expectedTraderName, pqTrdrPvl[0].TraderName);
            Assert.AreEqual(ExpectedVolume, pqTrdrPvl[0].TraderVolume);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqSrcQtRefTrdrVlDtPvl, true);
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
            Assert.AreEqual(expectedTraderName, convertedPqSrcQtRefTrdrVlDtPvl[0].TraderName);
            Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl[0].TraderVolume);
        }

        [TestMethod]
        public void PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ReturnsSamePQPriceVolumeLayerType()
        {
            var pqPvl = layerSelector.ConvertToExpectedImplementation(pqPriceVolumeLayer);
            Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
            Assert.AreSame(pqPriceVolumeLayer, pqPvl);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqSourcePriceVolumeLayer);
            var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
            Assert.IsNotNull(pqSrcPvl);
            Assert.AreSame(pqSourcePriceVolumeLayer, pqPvl);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqSourceQutoeRefPriceVolumeLayer);
            var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
            Assert.IsNotNull(pqSrcQtRefPvl);
            Assert.AreSame(pqSourceQutoeRefPriceVolumeLayer, pqPvl);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqValueDatePriceVolumeLayer);
            var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
            Assert.IsNotNull(pqVlDtPvl);
            Assert.AreSame(pqValueDatePriceVolumeLayer, pqPvl);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqTraderPriceVolumeLayer);
            var pqTrdrPvl = pqPvl as PQTraderPriceVolumeLayer;
            Assert.IsNotNull(pqTrdrPvl);
            Assert.AreSame(pqTraderPriceVolumeLayer, pqPvl);

            pqPvl = layerSelector.ConvertToExpectedImplementation(pqSrcQtRefTrdrVlDtPvl);
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
            var result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, priceVolumeLayer);
            Assert.AreSame(result, pqPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, priceVolumeLayer);
            Assert.AreSame(result, pqSourcePriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, priceVolumeLayer);
            Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, priceVolumeLayer);
            Assert.AreSame(result, pqValueDatePriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, priceVolumeLayer);
            Assert.AreSame(result, pqTraderPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, priceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, sourcePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, sourcePriceVolumeLayer);
            Assert.AreSame(result, pqSourcePriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, sourcePriceVolumeLayer);
            Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, sourcePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, sourcePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, sourcePriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, sourceQutoeRefPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, sourceQutoeRefPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, 
                sourceQutoeRefPriceVolumeLayer);
            Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, sourceQutoeRefPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, sourceQutoeRefPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, sourceQutoeRefPriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, valueDatePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, valueDatePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, valueDatePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, valueDatePriceVolumeLayer);
            Assert.AreSame(result, pqValueDatePriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, valueDatePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, valueDatePriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, traderPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, traderPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, traderPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, traderPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, traderPriceVolumeLayer);
            Assert.AreSame(result, pqTraderPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, traderPriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, srcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, srcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, srcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, srcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, srcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, srcQtRefTrdrVlDtPvl);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);
        }
        
        [TestMethod]
        public void PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToSrcQtRefTrdrVlDtPVLIfCantContain()
        {
            var result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, pqPriceVolumeLayer);
            Assert.AreSame(result, pqPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, pqPriceVolumeLayer);
            Assert.AreSame(result, pqSourcePriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, pqPriceVolumeLayer);
            Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, pqPriceVolumeLayer);
            Assert.AreSame(result, pqValueDatePriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, pqPriceVolumeLayer);
            Assert.AreSame(result, pqTraderPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, pqPriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, pqSourcePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, pqSourcePriceVolumeLayer);
            Assert.AreSame(result, pqSourcePriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, pqSourcePriceVolumeLayer);
            Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, pqSourcePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, pqSourcePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, pqSourcePriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, pqSourceQutoeRefPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, pqSourceQutoeRefPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer,
                pqSourceQutoeRefPriceVolumeLayer);
            Assert.AreSame(result, pqSourceQutoeRefPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, pqSourceQutoeRefPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, pqSourceQutoeRefPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, pqSourceQutoeRefPriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, pqValueDatePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, pqValueDatePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, pqValueDatePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, pqValueDatePriceVolumeLayer);
            Assert.AreSame(result, pqValueDatePriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, pqValueDatePriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, pqValueDatePriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, pqTraderPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, pqTraderPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, pqTraderPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, pqTraderPriceVolumeLayer);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, pqTraderPriceVolumeLayer);
            Assert.AreSame(result, pqTraderPriceVolumeLayer);
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, pqTraderPriceVolumeLayer);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, pqSrcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourcePriceVolumeLayer, pqSrcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSourceQutoeRefPriceVolumeLayer, pqSrcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqSourceQutoeRefPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqValueDatePriceVolumeLayer, pqSrcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqTraderPriceVolumeLayer, pqSrcQtRefTrdrVlDtPvl);
            Assert.AreNotSame(result, pqPriceVolumeLayer);
            Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
            Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
            result = layerSelector.SelectPriceVolumeLayer(pqSrcQtRefTrdrVlDtPvl, pqSrcQtRefTrdrVlDtPvl);
            Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);
        }

        [TestMethod]
        public void NullPriceVolumeLayerEntries_SelectPriceVolumeLayer_HandlesEmptyValues()
        {
            var result = layerSelector.SelectPriceVolumeLayer(null, priceVolumeLayer);
            Assert.AreEqual(typeof(PQPriceVolumeLayer), result.GetType());
            Assert.IsTrue(result.IsEmpty);
            result = layerSelector.SelectPriceVolumeLayer(pqPriceVolumeLayer, null);
            Assert.IsNull(result);
        }
    }
}