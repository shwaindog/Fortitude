// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;

#endregion

namespace FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

[TestClass]
public class SourceTickerQuoteInfoTests
{
    private uint                  expectedId;
    private SourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    public static SourceTickerQuoteInfo DummySourceTickerQuoteInfo =>
        new(1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown, 10,
            0.00001m, 30_000m, 10_000_000m, 10_000m, 250,
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName,
            LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedVolume);

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerQuoteInfo = DummySourceTickerQuoteInfo;
    }

    [TestMethod]
    public void NewUniqueSourceTickerIdentifer_New_IdGeneratedIsExpected()
    {
        ushort srcId = 123;
        ushort tkrId = 234;

        var firstUniSrcTkrId = new SourceTickerQuoteInfo
            (srcId, "TestSource", tkrId, "TestTicker", QuoteLevel.Level2, Unknown);

        expectedId = ((uint)srcId << 16) + tkrId;

        Assert.AreEqual(expectedId, firstUniSrcTkrId.Id);
        Assert.AreEqual("TestTicker", firstUniSrcTkrId.Ticker);
        Assert.AreEqual("TestSource", firstUniSrcTkrId.Source);

        var secondUniSrcTkrId = new SourceTickerQuoteInfo
            (srcId, "TestSource", tkrId, "TestTicker", QuoteLevel.Level2, Unknown);
        Assert.AreEqual(firstUniSrcTkrId, secondUniSrcTkrId);
    }

    [TestMethod]
    public void DummySourceTickerQuoteInfo_New_PropertiesAreAsExpected()
    {
        expectedId = ((uint)sourceTickerQuoteInfo.SourceId << 16) + sourceTickerQuoteInfo.TickerId;
        Assert.AreEqual(expectedId, sourceTickerQuoteInfo.Id);
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
        var minimalSrcTkrQtInfo = new SourceTickerQuoteInfo
            (1, "MinimalSource", 1, "MinalTicker", QuoteLevel.Level2, Unknown);

        expectedId = ((uint)minimalSrcTkrQtInfo.SourceId << 16) + minimalSrcTkrQtInfo.TickerId;
        Assert.AreEqual(expectedId, minimalSrcTkrQtInfo.Id);
        Assert.AreEqual("MinimalSource", minimalSrcTkrQtInfo.Source);
        Assert.AreEqual("MinalTicker", minimalSrcTkrQtInfo.Ticker);
        Assert.AreEqual(20, minimalSrcTkrQtInfo.MaximumPublishedLayers);
        Assert.AreEqual(0.00001m, minimalSrcTkrQtInfo.RoundingPrecision);
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

        var twoDecimalPrecision = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             10, 0.01m);
        formatPriceString = twoDecimalPrecision.FormatPrice;
        Assert.AreEqual("0.00", formatPriceString);
    }

    [TestMethod]
    public void When_Cloned_NewButEqualConfigCreated()
    {
        var cloneIdLookup = sourceTickerQuoteInfo.Clone();
        Assert.AreNotSame(cloneIdLookup, sourceTickerQuoteInfo);
        Assert.AreEqual(sourceTickerQuoteInfo, cloneIdLookup);

        var clone2 = sourceTickerQuoteInfo.Clone();
        Assert.AreNotSame(sourceTickerQuoteInfo, clone2);
        Assert.AreEqual(sourceTickerQuoteInfo, clone2);

        var clone4 = ((ISourceTickerQuoteInfo)sourceTickerQuoteInfo).Clone();
        Assert.AreNotSame(sourceTickerQuoteInfo, clone4);
        Assert.AreEqual(sourceTickerQuoteInfo, clone4);
    }

    [TestMethod]
    public void NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame()
    {
        var commonStqi = new SourceTickerQuoteInfo
            (12345, "TestSource", 12345, "TestTicker", QuoteLevel.Level2, Unknown);

        var nonStqi = new SourceTickerQuoteInfo
            (12345, "TestSource", 12345, "TestTicker", QuoteLevel.Level2, Unknown);

        Assert.IsTrue(commonStqi.AreEquivalent(nonStqi));
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent()
    {
        var commonStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown);

        var sourceIdDiffStqi = new SourceTickerQuoteInfo
            (2, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown);
        Assert.AreNotEqual(commonStqi, sourceIdDiffStqi);

        var tickerIdDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 2, "TestTicker", QuoteLevel.Level2, Unknown);
        Assert.AreNotEqual(commonStqi, tickerIdDiffStqi);

        var srcDiffStqi = new SourceTickerQuoteInfo
            (1, "DiffSource", 1, "TestTicker", QuoteLevel.Level2, Unknown);
        Assert.AreNotEqual(commonStqi, srcDiffStqi);

        var tkrDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "DiffTicker", QuoteLevel.Level2, Unknown);
        Assert.AreNotEqual(commonStqi, tkrDiffStqi);

        var numLayersDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             4);
        Assert.AreNotEqual(commonStqi, numLayersDiffStqi);

        var roundingPrecisionDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             20, 0.01m);
        Assert.AreNotEqual(commonStqi, roundingPrecisionDiffStqi);

        var minSubSizeDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             20, 0.0001m, 1m);
        Assert.AreNotEqual(commonStqi, minSubSizeDiffStqi);

        var maxSubSizeDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             20, 0.0001m, 0.01m,
             1_000_000_000);
        Assert.AreNotEqual(commonStqi, maxSubSizeDiffStqi);

        var incrementSizeDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             20, 0.0001m, 0.01m,
             1_000_000, 1m);
        Assert.AreNotEqual(commonStqi, incrementSizeDiffStqi);

        var minQuoteLifeDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             20, 0.0001m, 0.01m,
             1_000_000, 0.01m, 250);
        Assert.AreNotEqual(commonStqi, minQuoteLifeDiffStqi);

        var layerFlagsDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             20, 0.0001m, 0.01m,
             1_000_000, 0.01m, 100, LayerFlags.SourceQuoteReference | LayerFlags.ValueDate);
        Assert.AreNotEqual(commonStqi, layerFlagsDiffStqi);

        var lastTradeFlagsDiffStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             20, 0.0001m, 0.01m,
             1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.TraderName);
        Assert.AreNotEqual(commonStqi, lastTradeFlagsDiffStqi);

        // ReSharper disable RedundantArgumentDefaultValue
        var matchingStqi = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown,
             20, 0.00001m, 0.01m,
             1_000_000, 0.01m, 100, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None);
        Assert.AreEqual(commonStqi, matchingStqi);
        // ReSharper restore RedundantArgumentDefaultValue
    }

    [TestMethod]
    public void PopulatedSti_GetHashCode_NotEqualTo0()
    {
        var firstSrcTkrQuoteInfo = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown);

        Assert.AreNotEqual(0, firstSrcTkrQuoteInfo.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedSti_ToString_ReturnsNameAndValues()
    {
        var srcTkrQtInfo = new SourceTickerQuoteInfo
            (1, "TestSource", 1, "TestTicker", QuoteLevel.Level2, Unknown);
        var toString = srcTkrQtInfo.ToString();

        Assert.IsTrue(toString.Contains(srcTkrQtInfo.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.SourceId)}: {srcTkrQtInfo.SourceId}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrQtInfo.TickerId)}: {srcTkrQtInfo.TickerId}"));
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
