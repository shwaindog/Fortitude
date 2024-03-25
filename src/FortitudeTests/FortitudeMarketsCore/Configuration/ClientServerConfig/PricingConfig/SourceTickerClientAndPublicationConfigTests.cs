#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeTests.FortitudeIO.Transports.NewSocketAPI.ConnectionConfig;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;

[TestClass]
public class SourceTickerClientAndPublicationConfigTests
{
    private SourceTickerClientAndPublicationConfig sourceTickerClientAndPublicationConfig = null!;

    public static SourceTickerClientAndPublicationConfig DummySourceTickerClientAndPublicationConfig =>
        new(1U, "TestSource", "TestTicker",
            10, 0.00001m, 30000m, 10000000m, 10000m, 250, LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName,
            LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedVolume,
            SnapshotUpdatePricingServerConfigTests.DummySnapshotUpdatePricingServerConfig, 5000, false);

    public static void UpdateSourceTickerPublicationConfig(ISourceTickerClientAndPublicationConfig updateConfig,
        string sourceName, string tickerName, decimal roundingPrecision,
        decimal minSubmitSize, decimal maxSubmitSize, decimal incrementSize, ushort minimumQuoteLife,
        byte maximumPublishedLayers, LayerFlags layerFlags, LastTradedFlags lastTradedFlags,
        ISnapshotUpdatePricingServerConfig marketPriceQuoteMarketsServer, INameIdLookup? convertLayerIdLookup = null,
        uint resyncIntervalMs = 4000, bool allowUpdatesCatchup = true)
    {
        SourceTickerPublicationConfigTests.UpdateSourceTickerPublicationConfig(updateConfig, sourceName, tickerName,
            roundingPrecision, minSubmitSize, maxSubmitSize, incrementSize, minimumQuoteLife, maximumPublishedLayers,
            layerFlags, lastTradedFlags, marketPriceQuoteMarketsServer);
        NonPublicInvocator.SetAutoPropertyInstanceField(updateConfig,
            (SourceTickerClientAndPublicationConfig x) => x.SyncRetryIntervalMs, resyncIntervalMs);
        NonPublicInvocator.SetAutoPropertyInstanceField(updateConfig,
            (SourceTickerClientAndPublicationConfig x) => x.AllowUpdatesCatchup, allowUpdatesCatchup);
    }

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerClientAndPublicationConfig = DummySourceTickerClientAndPublicationConfig;
    }

    [TestMethod]
    public void DummySourceTickerQuoteInfo_New_PropertiesAreAsExpected()
    {
        Assert.AreEqual(1U, sourceTickerClientAndPublicationConfig.Id);
        Assert.AreEqual("TestSource", sourceTickerClientAndPublicationConfig.Source);
        Assert.AreEqual("TestTicker", sourceTickerClientAndPublicationConfig.Ticker);
        Assert.AreEqual(10, sourceTickerClientAndPublicationConfig.MaximumPublishedLayers);
        Assert.AreEqual(0.00001m, sourceTickerClientAndPublicationConfig.RoundingPrecision);
        Assert.AreEqual(30_000m, sourceTickerClientAndPublicationConfig.MinSubmitSize);
        Assert.AreEqual(10_000_000m, sourceTickerClientAndPublicationConfig.MaxSubmitSize);
        Assert.AreEqual(10_000m, sourceTickerClientAndPublicationConfig.IncrementSize);
        Assert.AreEqual(250, sourceTickerClientAndPublicationConfig.MinimumQuoteLife);
        Assert.AreEqual(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName,
            sourceTickerClientAndPublicationConfig.LayerFlags);
        Assert.AreEqual(LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime |
                        LastTradedFlags.LastTradedVolume, sourceTickerClientAndPublicationConfig.LastTradedFlags);
        Assert.AreEqual(SnapshotUpdatePricingServerConfigTests.DummySnapshotUpdatePricingServerConfig,
            sourceTickerClientAndPublicationConfig.MarketPriceQuoteServer);
        Assert.AreEqual(5000u, sourceTickerClientAndPublicationConfig.SyncRetryIntervalMs);
        Assert.IsFalse(sourceTickerClientAndPublicationConfig.AllowUpdatesCatchup);
    }

    [TestMethod]
    public void EmptySourceTickerQuoteInfo_New_DefaultAreAsExpected()
    {
        var minimalSrcTkrClntAndPubCfg =
            new SourceTickerClientAndPublicationConfig(1, "MinimalSource", "MinalTicker");

        Assert.AreEqual(1U, minimalSrcTkrClntAndPubCfg.Id);
        Assert.AreEqual("MinimalSource", minimalSrcTkrClntAndPubCfg.Source);
        Assert.AreEqual("MinalTicker", minimalSrcTkrClntAndPubCfg.Ticker);
        Assert.AreEqual(20, minimalSrcTkrClntAndPubCfg.MaximumPublishedLayers);
        Assert.AreEqual(0.0001m, minimalSrcTkrClntAndPubCfg.RoundingPrecision);
        Assert.AreEqual(0.01m, minimalSrcTkrClntAndPubCfg.MinSubmitSize);
        Assert.AreEqual(1_000_000m, minimalSrcTkrClntAndPubCfg.MaxSubmitSize);
        Assert.AreEqual(0.01m, minimalSrcTkrClntAndPubCfg.IncrementSize);
        Assert.AreEqual(100, minimalSrcTkrClntAndPubCfg.MinimumQuoteLife);
        Assert.AreEqual(LayerFlags.Price | LayerFlags.Volume, minimalSrcTkrClntAndPubCfg.LayerFlags);
        Assert.AreEqual(LastTradedFlags.None, minimalSrcTkrClntAndPubCfg.LastTradedFlags);
        Assert.IsNull(minimalSrcTkrClntAndPubCfg.MarketPriceQuoteServer);
        Assert.AreEqual(4000u, minimalSrcTkrClntAndPubCfg.SyncRetryIntervalMs);
        Assert.IsTrue(minimalSrcTkrClntAndPubCfg.AllowUpdatesCatchup);
    }

    [TestMethod]
    public void When_Cloned_NewButEqualConfigCreated()
    {
        var cloneIdLookup = sourceTickerClientAndPublicationConfig.Clone();
        Assert.AreNotSame(cloneIdLookup, sourceTickerClientAndPublicationConfig);
        Assert.AreEqual(sourceTickerClientAndPublicationConfig, cloneIdLookup);

        var clone2 = ((ICloneable<IUniqueSourceTickerIdentifier>)sourceTickerClientAndPublicationConfig).Clone();
        Assert.AreNotSame(sourceTickerClientAndPublicationConfig, clone2);
        Assert.AreEqual(sourceTickerClientAndPublicationConfig, clone2);

        var clone4 = ((ISourceTickerQuoteInfo)sourceTickerClientAndPublicationConfig).Clone();
        Assert.AreNotSame(sourceTickerClientAndPublicationConfig, clone4);
        Assert.AreEqual(sourceTickerClientAndPublicationConfig, clone4);

        var clone5 = ((ISourceTickerPublicationConfig)sourceTickerClientAndPublicationConfig).Clone();
        Assert.AreNotSame(sourceTickerClientAndPublicationConfig, clone5);
        Assert.AreEqual(sourceTickerClientAndPublicationConfig, clone5);
    }


    [TestMethod]
    public void NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame()
    {
        var commonStcpc = new SourceTickerClientAndPublicationConfig(12345, "TestSource", "TestTicker");

        var nonStqi = new DerivedSrcTkrClientAndPubConfig(12345, "TestSource", "TestTicker");

        Assert.IsTrue(commonStcpc.AreEquivalent(nonStqi));
        Assert.IsFalse(commonStcpc.AreEquivalent(nonStqi, true));
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent()
    {
        var commonStpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker");

        var idDiffStcpc = new SourceTickerClientAndPublicationConfig(2, "TestSource", "TestTicker");
        Assert.AreNotEqual(commonStpc, idDiffStcpc);

        var srcDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "DiffSource", "TestTicker");
        Assert.AreNotEqual(commonStpc, srcDiffStcpc);

        var tkrDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "DiffTicker");
        Assert.AreNotEqual(commonStpc, tkrDiffStcpc);

        var numLayersDiffStpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker", 4);
        Assert.AreNotEqual(commonStpc, numLayersDiffStpc);

        var roundingPrecisionDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker",
            20, 0.01m);
        Assert.AreNotEqual(commonStpc, roundingPrecisionDiffStcpc);

        var minSubSizeDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker", 20,
            0.0001m, 1m);
        Assert.AreNotEqual(commonStpc, minSubSizeDiffStcpc);

        var maxSubSizeDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker", 20,
            0.0001m, 0.01m, 1_000_000_000);
        Assert.AreNotEqual(commonStpc, maxSubSizeDiffStcpc);

        var incrementSizeDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker", 20,
            0.0001m, 0.01m, 1_000_000, 1m);
        Assert.AreNotEqual(commonStpc, incrementSizeDiffStcpc);

        var minQuoteLifeDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker", 20,
            0.0001m, 0.01m, 1_000_000, 0.01m, 250);
        Assert.AreNotEqual(commonStpc, minQuoteLifeDiffStcpc);

        var layerFlagsDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker", 20,
            0.0001m, 0.01m, 1_000_000, 0.01m, 100, LayerFlags.SourceQuoteReference | LayerFlags.ValueDate);
        Assert.AreNotEqual(commonStpc, layerFlagsDiffStcpc);

        var lastTradeFlagsDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker", 20,
            0.0001m, 0.01m, 1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.TraderName);
        Assert.AreNotEqual(commonStpc, lastTradeFlagsDiffStcpc);

        var marketPriceQuoteSrvrConfigDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource",
            "TestTicker", 20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100,
            LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None,
            new SnapshotUpdatePricingServerConfig("DiffServer", MarketServerType.MarketData,
                new[] { ConnectionConfigTests.DummyTopicConnectionConfig }, null, ushort.MaxValue
                , Enumerable.Empty<ISourceTickerPublicationConfig>(),
                true, true));
        Assert.AreNotEqual(commonStpc, marketPriceQuoteSrvrConfigDiffStcpc);

        var syncRetryDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker",
            20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None,
            null, 100);
        Assert.AreNotEqual(commonStpc, syncRetryDiffStcpc);

        var allowUpdateCatchupsDiffStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker",
            20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None,
            null, 4000, false);
        Assert.AreNotEqual(commonStpc, allowUpdateCatchupsDiffStcpc);

        // ReSharper disable RedundantArgumentDefaultValue
        var matchingStcpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker",
            20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None,
            null, 4000, true);
        Assert.AreEqual(commonStpc, matchingStcpc);
        // ReSharper restore RedundantArgumentDefaultValue
    }

    [TestMethod]
    public void PopulatedStcpc_GetHashCode_NotEqualTo0()
    {
        var stpc = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker");

        Assert.AreNotEqual(0, stpc.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedSti_ToString_ReturnsNameAndValues()
    {
        var srcTkrPubCfg = new SourceTickerClientAndPublicationConfig(1, "TestSource", "TestTicker");
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
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.SyncRetryIntervalMs)}: " +
                                        $"{srcTkrPubCfg.SyncRetryIntervalMs}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrPubCfg.AllowUpdatesCatchup)}: " +
                                        $"{srcTkrPubCfg.AllowUpdatesCatchup}"));
    }

    private class DerivedSrcTkrClientAndPubConfig : SourceTickerClientAndPublicationConfig
    {
        public DerivedSrcTkrClientAndPubConfig(uint uniqueId, string source, string ticker,
            byte maximumPublishedLayers = 20, decimal roundingPrecision = 0.0001m,
            decimal minSubmitSize = 0.01m, decimal maxSubmitSize = 1000000, decimal incrementSize = 0.01m,
            ushort minimumQuoteLife = 100,
            LayerFlags layerFlags = LayerFlags.None | LayerFlags.Price | LayerFlags.Volume,
            LastTradedFlags lastTradedFlags = LastTradedFlags.None,
            ISnapshotUpdatePricingServerConfig? marketPriceQuoteMarketsServer = null, uint resyncIntervalMs = 4000,
            bool allowUpdateCatchups = true) : base(uniqueId, source, ticker, maximumPublishedLayers,
            roundingPrecision, minSubmitSize, maxSubmitSize, incrementSize, minimumQuoteLife, layerFlags,
            lastTradedFlags, marketPriceQuoteMarketsServer, resyncIntervalMs, allowUpdateCatchups) { }
    }
}
