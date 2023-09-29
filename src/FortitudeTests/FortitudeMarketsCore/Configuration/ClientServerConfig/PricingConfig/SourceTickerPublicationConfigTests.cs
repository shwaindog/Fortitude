using System;
using System.Collections.Generic;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeTests.FortitudeIO.Transports.Sockets;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig
{
    [TestClass]
    public class SourceTickerPublicationConfigTests
    {
        private SourceTickerPublicationConfig sourceTickerPublicationConfig;

        public static SourceTickerPublicationConfig DummySourceTickerPublicationConfig =>
            new SourceTickerPublicationConfig(1U, "TestSource", "TestTicker", 10,
                0.00001m, 30000m, 10000000m, 10000m, 250, LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName,
                LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedVolume,
                SnapshotUpdatePricingServerConfigTests.DummySnapshotUpdatePricingServerConfig);

        public static void UpdateSourceTickerPublicationConfig(ISourceTickerPublicationConfig updateConfig,
            string sourceName, string tickerName, decimal roundingPrecision,
            decimal minSubmitSize, decimal maxSubmitSize, decimal incrementSize, ushort minimumQuoteLife,
            byte maximumPublishedLayers, LayerFlags layerFlags, LastTradedFlags lastTradedFlags,
            ISnapshotUpdatePricingServerConfig marketPriceQuoteMarketsServer, INameIdLookup convertLayerIdLookup = null)
        {
            SourceTickerQuoteInfoTests.UpdateSourceTickerQuoteInfo(updateConfig, sourceName, tickerName, roundingPrecision, 
                minSubmitSize, maxSubmitSize, incrementSize, minimumQuoteLife, maximumPublishedLayers, layerFlags, 
                lastTradedFlags);
            NonPublicInvocator.SetAutoPropertyInstanceField(updateConfig,
                (SourceTickerPublicationConfig x) => x.MarketPriceQuoteServer, marketPriceQuoteMarketsServer);
        }

        public static IEnumerable<SourceTickerPublicationConfig> SampleSourceTickerPublicationConfigs => new[]
        {
            new SourceTickerPublicationConfig(1U, "TestSource", "TestTicker1", 10,
                0.00001m, 30000m, 10000000m, 10000m, 250, LayerFlags.Price | LayerFlags.Volume,
                LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime |
                LastTradedFlags.LastTradedVolume,
                SnapshotUpdatePricingServerConfigTests.DummySnapshotUpdatePricingServerConfig),
            new SourceTickerPublicationConfig(2U, "TestSource", "TestTicker2", 10,
                0.00001m, 30000m, 10000000m, 10000m, 250, LayerFlags.Price | LayerFlags.Volume,
                LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime |
                LastTradedFlags.LastTradedVolume,
                SnapshotUpdatePricingServerConfigTests.DummySnapshotUpdatePricingServerConfig),
            new SourceTickerPublicationConfig(3U, "TestSource", "TestTicker2", 10,
                0.00001m, 30000m, 10000000m, 10000m, 250, LayerFlags.Price | LayerFlags.Volume,
                LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime |
                LastTradedFlags.LastTradedVolume,
                SnapshotUpdatePricingServerConfigTests.DummySnapshotUpdatePricingServerConfig)
        };

        [TestInitialize]
        public void SetUp()
        {
            sourceTickerPublicationConfig = DummySourceTickerPublicationConfig;
        }

        [TestMethod]
        public void DummySourceTickerQuoteInfo_New_PropertiesAreAsExpected()
        {
            Assert.AreEqual(1U, sourceTickerPublicationConfig.Id);
            Assert.AreEqual("TestSource", sourceTickerPublicationConfig.Source);
            Assert.AreEqual("TestTicker", sourceTickerPublicationConfig.Ticker);
            Assert.AreEqual(10, sourceTickerPublicationConfig.MaximumPublishedLayers);
            Assert.AreEqual(0.00001m, sourceTickerPublicationConfig.RoundingPrecision);
            Assert.AreEqual(30_000m, sourceTickerPublicationConfig.MinSubmitSize);
            Assert.AreEqual(10_000_000m, sourceTickerPublicationConfig.MaxSubmitSize);
            Assert.AreEqual(10_000m, sourceTickerPublicationConfig.IncrementSize);
            Assert.AreEqual(250, sourceTickerPublicationConfig.MinimumQuoteLife);
            Assert.AreEqual(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName,
                sourceTickerPublicationConfig.LayerFlags);
            Assert.AreEqual(LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime |
                LastTradedFlags.LastTradedVolume, sourceTickerPublicationConfig.LastTradedFlags);
            Assert.AreEqual(SnapshotUpdatePricingServerConfigTests.DummySnapshotUpdatePricingServerConfig,
                sourceTickerPublicationConfig.MarketPriceQuoteServer);
        }

        [TestMethod]
        public void EmptySourceTickerQuoteInfo_New_DefaultAreAsExpected()
        {
            var minimalSrcTkrPubCfg = new SourceTickerPublicationConfig(1, "MinimalSource", "MinalTicker");

            Assert.AreEqual(1U, minimalSrcTkrPubCfg.Id);
            Assert.AreEqual("MinimalSource", minimalSrcTkrPubCfg.Source);
            Assert.AreEqual("MinalTicker", minimalSrcTkrPubCfg.Ticker);
            Assert.AreEqual(20, minimalSrcTkrPubCfg.MaximumPublishedLayers);
            Assert.AreEqual(0.0001m, minimalSrcTkrPubCfg.RoundingPrecision);
            Assert.AreEqual(0.01m, minimalSrcTkrPubCfg.MinSubmitSize);
            Assert.AreEqual(1_000_000m, minimalSrcTkrPubCfg.MaxSubmitSize);
            Assert.AreEqual(0.01m, minimalSrcTkrPubCfg.IncrementSize);
            Assert.AreEqual(100, minimalSrcTkrPubCfg.MinimumQuoteLife);
            Assert.AreEqual(LayerFlags.Price | LayerFlags.Volume, minimalSrcTkrPubCfg.LayerFlags);
            Assert.AreEqual(LastTradedFlags.None, minimalSrcTkrPubCfg.LastTradedFlags);
            Assert.IsNull(minimalSrcTkrPubCfg.MarketPriceQuoteServer);
        }

        [TestMethod]
        public void When_Cloned_NewButEqualConfigCreated()
        {
            var cloneIdLookup = sourceTickerPublicationConfig.Clone();
            Assert.AreNotSame(cloneIdLookup, sourceTickerPublicationConfig);
            Assert.AreEqual(sourceTickerPublicationConfig, cloneIdLookup);

            var clone2 = ((ICloneable<IUniqueSourceTickerIdentifier>)sourceTickerPublicationConfig).Clone();
            Assert.AreNotSame(sourceTickerPublicationConfig, clone2);
            Assert.AreEqual(sourceTickerPublicationConfig, clone2);

            var clone4 = ((ISourceTickerQuoteInfo)sourceTickerPublicationConfig).Clone();
            Assert.AreNotSame(sourceTickerPublicationConfig, clone4);
            Assert.AreEqual(sourceTickerPublicationConfig, clone4);

            var clone5 = ((ISourceTickerPublicationConfig)sourceTickerPublicationConfig).Clone();
            Assert.AreNotSame(sourceTickerPublicationConfig, clone5);
            Assert.AreEqual(sourceTickerPublicationConfig, clone5);
        }

        [TestMethod]
        public void NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame()
        {
            var commonStqi = new SourceTickerPublicationConfig(12345, "TestSource", "TestTicker");

            var nonStqi = new SourceTickerClientAndPublicationConfig(12345, "TestSource", "TestTicker");

            Assert.IsTrue(commonStqi.AreEquivalent(nonStqi));
            Assert.IsFalse(commonStqi.AreEquivalent(nonStqi, true));
        }

        [TestMethod]
        public void OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent()
        {
            var commonStpc = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker");

            var idDiffStqi = new SourceTickerPublicationConfig(2, "TestSource", "TestTicker");
            Assert.AreNotEqual(commonStpc, idDiffStqi);

            var srcDiffStqi = new SourceTickerPublicationConfig(1, "DiffSource", "TestTicker");
            Assert.AreNotEqual(commonStpc, srcDiffStqi);

            var tkrDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "DiffTicker");
            Assert.AreNotEqual(commonStpc, tkrDiffStqi);

            var numLayersDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 4);
            Assert.AreNotEqual(commonStpc, numLayersDiffStqi);

            var roundingPrecisionDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 20, 0.01m);
            Assert.AreNotEqual(commonStpc, roundingPrecisionDiffStqi);

            var minSubSizeDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 20, 0.0001m, 1m);
            Assert.AreNotEqual(commonStpc, minSubSizeDiffStqi);

            var maxSubSizeDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 20, 0.0001m,
                0.01m, 1_000_000_000);
            Assert.AreNotEqual(commonStpc, maxSubSizeDiffStqi);

            var incrementSizeDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 20, 0.0001m, 
                0.01m, 1_000_000, 1m);
            Assert.AreNotEqual(commonStpc, incrementSizeDiffStqi);

            var minQuoteLifeDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 20, 0.0001m, 
                0.01m, 1_000_000, 0.01m, 250);
            Assert.AreNotEqual(commonStpc, minQuoteLifeDiffStqi);

            var layerFlagsDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 20, 0.0001m, 
                0.01m, 1_000_000, 0.01m, 100, LayerFlags.SourceQuoteReference | LayerFlags.ValueDate);
            Assert.AreNotEqual(commonStpc, layerFlagsDiffStqi);

            var lastTradeFlagsDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 20, 0.0001m, 
                0.01m, 1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.TraderName);
            Assert.AreNotEqual(commonStpc, lastTradeFlagsDiffStqi);

            var marketPriceQuoteSrvrConfigDiffStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker", 
                20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None,
                new SnapshotUpdatePricingServerConfig("DiffServer", MarketServerType.MarketData, 
                new[] { ConnectionConfigTests.DummyConnectionConfig }, null, UInt16.MaxValue, null,
                true, true ));
            Assert.AreNotEqual(commonStpc, marketPriceQuoteSrvrConfigDiffStqi);

            // ReSharper disable RedundantArgumentDefaultValue
            var matchingStqi = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker",
                20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None,
                null);
            Assert.AreEqual(commonStpc, matchingStqi);
            // ReSharper restore RedundantArgumentDefaultValue
        }

        [TestMethod]
        public void PopulatedStpc_GetHashCode_NotEqualTo0()
        {
            var stpc = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker");

            Assert.AreNotEqual(0, stpc.GetHashCode());
        }

        [TestMethod]
        public void FullyPopulatedStpc_ToString_ReturnsNameAndValues()
        {
            var srcTkrPubCfg = new SourceTickerPublicationConfig(1, "TestSource", "TestTicker");
            var toString = srcTkrPubCfg.ToString();

            Assert.IsTrue(toString.Contains(srcTkrPubCfg.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.Id)}: {srcTkrPubCfg.Id}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.Ticker)}: {srcTkrPubCfg.Ticker}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.Source)}: {srcTkrPubCfg.Source}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.MaximumPublishedLayers)}: " +
                                            $"{srcTkrPubCfg.MaximumPublishedLayers}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.RoundingPrecision)}: " +
                                            $"{srcTkrPubCfg.RoundingPrecision}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.MinSubmitSize)}: {srcTkrPubCfg.MinSubmitSize}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.MaxSubmitSize)}: {srcTkrPubCfg.MaxSubmitSize}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.IncrementSize)}: {srcTkrPubCfg.IncrementSize}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.MinimumQuoteLife)}: " +
                                            $"{srcTkrPubCfg.MinimumQuoteLife}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.LayerFlags)}: {srcTkrPubCfg.LayerFlags}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.LastTradedFlags)}: " +
                                            $"{srcTkrPubCfg.LastTradedFlags}"));
            Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.MarketPriceQuoteServer)}: " +
                                            $"{srcTkrPubCfg.MarketPriceQuoteServer}"));
        }
    }
}