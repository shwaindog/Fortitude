using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo
{
    [TestClass]
    public class SourceTickerQuoteInfoTests
    {
        private SourceTickerQuoteInfo sourceTickerQuoteInfo;

        public static SourceTickerQuoteInfo DummySourceTickerQuoteInfo =>
            new SourceTickerQuoteInfo(1U, "TestSource", "TestTicker", 10,
                0.00001m, 30_000m, 10_000_000m, 10_000m, 250, LayerFlags.Price | LayerFlags.Volume | 
                LayerFlags.SourceName, LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime | 
                LastTradedFlags.LastTradedVolume);


        public static void UpdateSourceTickerQuoteInfo(ISourceTickerQuoteInfo updateInfo,
            string sourceName, string tickerName, decimal roundingPrecision,
            decimal minSubmitSize, decimal maxSubmitSize, decimal incrementSize, ushort minimumQuoteLife,
            byte maximumPublishedLayers, LayerFlags layerFlags, LastTradedFlags lastTradedFlags)
        {
            UniqueSourceTickerIdentifierTests.UpdateUniqueSourceTickerIdentifier(updateInfo, sourceName, tickerName);

            NonPublicInvocator.SetAutoPropertyInstanceField(updateInfo,
                (SourceTickerQuoteInfo x) => x.RoundingPrecision, roundingPrecision);
            NonPublicInvocator.SetAutoPropertyInstanceField(updateInfo,
                (SourceTickerQuoteInfo x) => x.MinSubmitSize, minSubmitSize);
            NonPublicInvocator.SetAutoPropertyInstanceField(updateInfo,
                (SourceTickerQuoteInfo x) => x.MaxSubmitSize, maxSubmitSize);
            NonPublicInvocator.SetAutoPropertyInstanceField(updateInfo,
                (SourceTickerQuoteInfo x) => x.IncrementSize, incrementSize);
            NonPublicInvocator.SetAutoPropertyInstanceField(updateInfo,
                (SourceTickerQuoteInfo x) => x.MinimumQuoteLife, minimumQuoteLife);
            NonPublicInvocator.SetAutoPropertyInstanceField(updateInfo,
                (SourceTickerQuoteInfo x) => x.MaximumPublishedLayers, maximumPublishedLayers);
            NonPublicInvocator.SetAutoPropertyInstanceField(updateInfo,
                (SourceTickerQuoteInfo x) => x.LayerFlags, layerFlags);
            NonPublicInvocator.SetAutoPropertyInstanceField(updateInfo,
                (SourceTickerQuoteInfo x) => x.LastTradedFlags, lastTradedFlags);
        }

        [TestInitialize]
        public void SetUp()
        {
            sourceTickerQuoteInfo = DummySourceTickerQuoteInfo;
        }

        [TestMethod]
        public void DummySourceTickerQuoteInfo_New_PropertiesAreAsExpected()
        {
            Assert.AreEqual(1U, sourceTickerQuoteInfo.Id);
            Assert.AreEqual("TestSource", sourceTickerQuoteInfo.Source);
            Assert.AreEqual("TestTicker", sourceTickerQuoteInfo.Ticker);
            Assert.AreEqual(10, sourceTickerQuoteInfo.MaximumPublishedLayers);
            Assert.AreEqual(0.00001m, sourceTickerQuoteInfo.RoundingPrecision);
            Assert.AreEqual(30_000m, sourceTickerQuoteInfo.MinSubmitSize);
            Assert.AreEqual(10_000_000m, sourceTickerQuoteInfo.MaxSubmitSize);
            Assert.AreEqual(10_000m, sourceTickerQuoteInfo.IncrementSize);
            Assert.AreEqual(250, sourceTickerQuoteInfo.MinimumQuoteLife);
            Assert.AreEqual(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName, 
                sourceTickerQuoteInfo.LayerFlags);
            Assert.AreEqual(LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime | 
                LastTradedFlags.LastTradedVolume, sourceTickerQuoteInfo.LastTradedFlags);
        }

        [TestMethod]
        public void EmptySourceTickerQuoteInfo_New_DefaultAreAsExpected()
        {
            var minimalSrcTkrQtInfo = new SourceTickerQuoteInfo(1, "MinimalSource", "MinalTicker");

            Assert.AreEqual(1U, minimalSrcTkrQtInfo.Id);
            Assert.AreEqual("MinimalSource", minimalSrcTkrQtInfo.Source);
            Assert.AreEqual("MinalTicker", minimalSrcTkrQtInfo.Ticker);
            Assert.AreEqual(20, minimalSrcTkrQtInfo.MaximumPublishedLayers);
            Assert.AreEqual(0.0001m, minimalSrcTkrQtInfo.RoundingPrecision);
            Assert.AreEqual(0.01m, minimalSrcTkrQtInfo.MinSubmitSize);
            Assert.AreEqual(1_000_000m, minimalSrcTkrQtInfo.MaxSubmitSize);
            Assert.AreEqual(0.01m, minimalSrcTkrQtInfo.IncrementSize);
            Assert.AreEqual(100, minimalSrcTkrQtInfo.MinimumQuoteLife);
            Assert.AreEqual(LayerFlags.Price | LayerFlags.Volume, minimalSrcTkrQtInfo.LayerFlags);
            Assert.AreEqual(LastTradedFlags.None, minimalSrcTkrQtInfo.LastTradedFlags);
        }

        [TestMethod]
        public void DummySourceTickerQuoteInfo_FormatPrice_ReturnsStringFormatterToPrecision()
        {
            var formatPriceString = sourceTickerQuoteInfo.FormatPrice;
            Assert.AreEqual("0.00000", formatPriceString);

            var twoDecimalPrecision = new SourceTickerQuoteInfo(1U, "TestSource", "TestTicker", 10, 0.01m );
            formatPriceString = twoDecimalPrecision.FormatPrice;
            Assert.AreEqual("0.00", formatPriceString);
        }

        [TestMethod]
        public void When_Cloned_NewButEqualConfigCreated()
        {
            var cloneIdLookup = sourceTickerQuoteInfo.Clone();
            Assert.AreNotSame(cloneIdLookup, sourceTickerQuoteInfo);
            Assert.AreEqual(sourceTickerQuoteInfo, cloneIdLookup);

            var clone2 = ((ICloneable<IUniqueSourceTickerIdentifier>)sourceTickerQuoteInfo).Clone();
            Assert.AreNotSame(sourceTickerQuoteInfo, clone2);
            Assert.AreEqual(sourceTickerQuoteInfo, clone2);

            var clone4 = ((ISourceTickerQuoteInfo)sourceTickerQuoteInfo).Clone();
            Assert.AreNotSame(sourceTickerQuoteInfo, clone4);
            Assert.AreEqual(sourceTickerQuoteInfo, clone4);
        }

        [TestMethod]
        public void NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame()
        {
            var commonStqi = new SourceTickerQuoteInfo(12345, "TestSource", "TestTicker");

            var nonStqi = new SourceTickerPublicationConfig(12345, "TestSource", "TestTicker");

            Assert.IsTrue(commonStqi.AreEquivalent(nonStqi));
            Assert.IsFalse(commonStqi.AreEquivalent(nonStqi, true));
        }

        [TestMethod]
        public void OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent()
        {
            var commonStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker");

            var idDiffStqi = new SourceTickerQuoteInfo(2, "TestSource", "TestTicker");
            Assert.AreNotEqual(commonStqi, idDiffStqi);

            var srcDiffStqi = new SourceTickerQuoteInfo(1, "DiffSource", "TestTicker");
            Assert.AreNotEqual(commonStqi, srcDiffStqi);

            var tkrDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "DiffTicker");
            Assert.AreNotEqual(commonStqi, tkrDiffStqi);

            var numLayersDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 4);
            Assert.AreNotEqual(commonStqi, numLayersDiffStqi);

            var roundingPrecisionDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 20, 0.01m);
            Assert.AreNotEqual(commonStqi, roundingPrecisionDiffStqi);

            var minSubSizeDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 20, 0.0001m, 1m);
            Assert.AreNotEqual(commonStqi, minSubSizeDiffStqi);

            var maxSubSizeDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 20, 0.0001m, 0.01m, 
                1_000_000_000);
            Assert.AreNotEqual(commonStqi, maxSubSizeDiffStqi);

            var incrementSizeDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 20, 0.0001m, 0.01m, 
                1_000_000, 1m);
            Assert.AreNotEqual(commonStqi, incrementSizeDiffStqi);

            var minQuoteLifeDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 20, 0.0001m, 0.01m, 
                1_000_000, 0.01m, 250);
            Assert.AreNotEqual(commonStqi, minQuoteLifeDiffStqi);

            var layerFlagsDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 20, 0.0001m, 0.01m, 
                1_000_000, 0.01m, 100, LayerFlags.SourceQuoteReference | LayerFlags.ValueDate);
            Assert.AreNotEqual(commonStqi, layerFlagsDiffStqi);

            var lastTradeFlagsDiffStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 20, 0.0001m, 0.01m, 
                1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.TraderName);
            Assert.AreNotEqual(commonStqi, lastTradeFlagsDiffStqi);

            // ReSharper disable RedundantArgumentDefaultValue
            var matchingStqi = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker", 20, 0.0001m, 0.01m,
                1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None);
            Assert.AreEqual(commonStqi, matchingStqi);
            // ReSharper restore RedundantArgumentDefaultValue
        }

        [TestMethod]
        public void PopulatedSti_GetHashCode_NotEqualTo0()
        {
            var firstSrcTkrQuoteInfo = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker");

            Assert.AreNotEqual(0, firstSrcTkrQuoteInfo.GetHashCode());
        }

        [TestMethod]
        public void FullyPopulatedSti_ToString_ReturnsNameAndValues()
        {
            var srcTkrQtInfo = new SourceTickerQuoteInfo(1, "TestSource", "TestTicker");
            var toString = srcTkrQtInfo.ToString();

            Assert.IsTrue(toString.Contains(srcTkrQtInfo.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.Id)}: {srcTkrQtInfo.Id}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.Ticker)}: {srcTkrQtInfo.Ticker}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.Source)}: {srcTkrQtInfo.Source}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.MaximumPublishedLayers)}: " +
                                            $"{srcTkrQtInfo.MaximumPublishedLayers}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.RoundingPrecision)}: " +
                                            $"{srcTkrQtInfo.RoundingPrecision}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.MinSubmitSize)}: {srcTkrQtInfo.MinSubmitSize}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.MaxSubmitSize)}: {srcTkrQtInfo.MaxSubmitSize}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.IncrementSize)}: {srcTkrQtInfo.IncrementSize}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.MinimumQuoteLife)}: " +
                                            $"{srcTkrQtInfo.MinimumQuoteLife}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.LayerFlags)}: {srcTkrQtInfo.LayerFlags}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.LastTradedFlags)}: " +
                                            $"{srcTkrQtInfo.LastTradedFlags}"));
        }
    }
}