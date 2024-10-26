// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;

#endregion

namespace FortitudeTests.FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;

[TestClass]
public class SourceTickerInfoTests
{
    private uint expectedId;

    private SourceTickerInfo sourceTickerInfo = null!;

    public static SourceTickerInfo DummySourceTickerInfo =>
        new(1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown, 10
          , 0.00001m, 0.0001m, 30_000m, 10_000_000m, 10_000m, 250, 10_000
          , layerFlags: LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName
          , lastTradedFlags: LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedVolume);

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerInfo = DummySourceTickerInfo;
    }

    [TestMethod]
    public void NewUniqueSourceTickerIdentifer_New_IdGeneratedIsExpected()
    {
        ushort srcId = 123;
        ushort tkrId = 234;

        var firstUniSrcTkrId = new SourceTickerInfo
            (srcId, "TestSource", tkrId, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);

        expectedId = ((uint)srcId << 16) + tkrId;

        Assert.AreEqual(expectedId, firstUniSrcTkrId.SourceTickerId);
        Assert.AreEqual("TestTicker", firstUniSrcTkrId.Ticker);
        Assert.AreEqual("TestSource", firstUniSrcTkrId.Source);

        var secondUniSrcTkrId = new SourceTickerInfo
            (srcId, "TestSource", tkrId, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);
        Assert.AreEqual(firstUniSrcTkrId, secondUniSrcTkrId);
    }

    [TestMethod]
    public void DummySourceTickerInfo_New_PropertiesAreAsExpected()
    {
        expectedId = ((uint)sourceTickerInfo.SourceId << 16) + sourceTickerInfo.TickerId;
        Assert.AreEqual(expectedId, sourceTickerInfo.SourceTickerId);
        Assert.AreEqual("TestSource", sourceTickerInfo.Source);
        Assert.AreEqual("TestTicker", sourceTickerInfo.Ticker);
        Assert.AreEqual(10, sourceTickerInfo.MaximumPublishedLayers);
        Assert.AreEqual(0.00001m, sourceTickerInfo.RoundingPrecision);
        Assert.AreEqual(0.0001m, sourceTickerInfo.Pip);
        Assert.AreEqual(30_000m, sourceTickerInfo.MinSubmitSize);
        Assert.AreEqual(10_000_000m, sourceTickerInfo.MaxSubmitSize);
        Assert.AreEqual(10_000m, sourceTickerInfo.IncrementSize);
        Assert.AreEqual(250, sourceTickerInfo.MinimumQuoteLife);
        Assert.AreEqual(10_000u, sourceTickerInfo.DefaultMaxValidMs);
        Assert.AreEqual(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName,
                        sourceTickerInfo.LayerFlags);
        Assert.AreEqual(LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime |
                        LastTradedFlags.LastTradedVolume, sourceTickerInfo.LastTradedFlags);
    }

    [TestMethod]
    public void EmptySourceTickerInfo_New_DefaultAreAsExpected()
    {
        var minimalSrcTkrInfo = new SourceTickerInfo
            (1, "MinimalSource", 1, "MinalTicker", TickerDetailLevel.Level2Quote, Unknown);

        expectedId = ((uint)minimalSrcTkrInfo.SourceId << 16) + minimalSrcTkrInfo.TickerId;
        Assert.AreEqual(expectedId, minimalSrcTkrInfo.SourceTickerId);
        Assert.AreEqual("MinimalSource", minimalSrcTkrInfo.Source);
        Assert.AreEqual("MinalTicker", minimalSrcTkrInfo.Ticker);
        Assert.AreEqual(20, minimalSrcTkrInfo.MaximumPublishedLayers);
        Assert.AreEqual(0.00001m, minimalSrcTkrInfo.RoundingPrecision);
        Assert.AreEqual(0.0001m, minimalSrcTkrInfo.Pip);
        Assert.AreEqual(0.01m, minimalSrcTkrInfo.MinSubmitSize);
        Assert.AreEqual(1_000_000m, minimalSrcTkrInfo.MaxSubmitSize);
        Assert.AreEqual(0.01m, minimalSrcTkrInfo.IncrementSize);
        Assert.AreEqual(100, minimalSrcTkrInfo.MinimumQuoteLife);
        Assert.AreEqual(10_000u, sourceTickerInfo.DefaultMaxValidMs);
        Assert.AreEqual(LayerFlags.Price | LayerFlags.Volume, minimalSrcTkrInfo.LayerFlags);
        Assert.AreEqual(LastTradedFlags.None, minimalSrcTkrInfo.LastTradedFlags);
    }

    [TestMethod]
    public void DummySourceTickerInfo_FormatPrice_ReturnsStringFormatterToPrecision()
    {
        var formatPriceString = sourceTickerInfo.FormatPrice;
        Assert.AreEqual("0.00000", formatPriceString);

        var twoDecimalPrecision = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown,
             10, 0.01m);
        formatPriceString = twoDecimalPrecision.FormatPrice;
        Assert.AreEqual("0.00", formatPriceString);
    }

    [TestMethod]
    public void When_Cloned_NewButEqualConfigCreated()
    {
        var cloneIdLookup = sourceTickerInfo.Clone();
        Assert.AreNotSame(cloneIdLookup, sourceTickerInfo);
        Assert.AreEqual(sourceTickerInfo, cloneIdLookup);

        var clone2 = sourceTickerInfo.Clone();
        Assert.AreNotSame(sourceTickerInfo, clone2);
        Assert.AreEqual(sourceTickerInfo, clone2);

        var clone4 = ((ISourceTickerInfo)sourceTickerInfo).Clone();
        Assert.AreNotSame(sourceTickerInfo, clone4);
        Assert.AreEqual(sourceTickerInfo, clone4);
    }

    [TestMethod]
    public void NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame()
    {
        var commonStqi = new SourceTickerInfo
            (12345, "TestSource", 12345, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);

        var nonStqi = new SourceTickerInfo
            (12345, "TestSource", 12345, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);

        Assert.IsTrue(commonStqi.AreEquivalent(nonStqi));
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent()
    {
        var commonStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);

        var sourceIdDiffStqi = new SourceTickerInfo
            (2, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);
        Assert.AreNotEqual(commonStqi, sourceIdDiffStqi);

        var tickerIdDiffStqi = new SourceTickerInfo
            (1, "TestSource", 2, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);
        Assert.AreNotEqual(commonStqi, tickerIdDiffStqi);

        var srcDiffStqi = new SourceTickerInfo
            (1, "DiffSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);
        Assert.AreNotEqual(commonStqi, srcDiffStqi);

        var tkrDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "DiffTicker", TickerDetailLevel.Level2Quote, Unknown);
        Assert.AreNotEqual(commonStqi, tkrDiffStqi);

        var numLayersDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 4);
        Assert.AreNotEqual(commonStqi, numLayersDiffStqi);

        var roundingPrecisionDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 20, 0.01m);
        Assert.AreNotEqual(commonStqi, roundingPrecisionDiffStqi);

        var minSubSizeDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 1m);
        Assert.AreNotEqual(commonStqi, minSubSizeDiffStqi);

        var maxSubSizeDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000_000);
        Assert.AreNotEqual(commonStqi, maxSubSizeDiffStqi);

        var incrementSizeDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000, 1m);
        Assert.AreNotEqual(commonStqi, incrementSizeDiffStqi);

        var minQuoteLifeDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000, 0.01m, 250);
        Assert.AreNotEqual(commonStqi, minQuoteLifeDiffStqi);

        var layerFlagsDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100
           , layerFlags: LayerFlags.SourceQuoteReference | LayerFlags.ValueDate);
        Assert.AreNotEqual(commonStqi, layerFlagsDiffStqi);

        var lastTradeFlagsDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100
           , layerFlags: LayerFlags.Price | LayerFlags.Volume
           , lastTradedFlags: LastTradedFlags.TraderName);
        Assert.AreNotEqual(commonStqi, lastTradeFlagsDiffStqi);

        // ReSharper disable RedundantArgumentDefaultValue
        var matchingStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown
           , 20, 0.00001m, 0.0001m, 0.01m, 1_000_000, 0.01m, 100
           , 10_000, layerFlags: LayerFlags.Price | LayerFlags.Volume
           , lastTradedFlags: LastTradedFlags.None);
        Assert.AreEqual(commonStqi, matchingStqi);
        // ReSharper restore RedundantArgumentDefaultValue
    }

    [TestMethod]
    public void PopulatedSti_GetHashCode_NotEqualTo0()
    {
        var firstSrcTkrQuoteInfo = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);

        Assert.AreNotEqual(0, firstSrcTkrQuoteInfo.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedSti_ToString_ReturnsNameAndValues()
    {
        var srcTkrInfo = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerDetailLevel.Level2Quote, Unknown);
        var toString = srcTkrInfo.ToString();

        Assert.IsTrue(toString.Contains(srcTkrInfo.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.SourceId)}: {srcTkrInfo.SourceId}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.TickerId)}: {srcTkrInfo.TickerId}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.Ticker)}: {srcTkrInfo.Ticker}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.Source)}: {srcTkrInfo.Source}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.MaximumPublishedLayers)}: " +
                                        $"{srcTkrInfo.MaximumPublishedLayers}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.RoundingPrecision)}: " +
                                        $"{srcTkrInfo.RoundingPrecision}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.MinSubmitSize)}: {srcTkrInfo.MinSubmitSize}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.MaxSubmitSize)}: {srcTkrInfo.MaxSubmitSize}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.IncrementSize)}: {srcTkrInfo.IncrementSize}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.MinimumQuoteLife)}: " +
                                        $"{srcTkrInfo.MinimumQuoteLife}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.LayerFlags)}: {srcTkrInfo.LayerFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.LastTradedFlags)}: " +
                                        $"{srcTkrInfo.LastTradedFlags}"));
    }
}
