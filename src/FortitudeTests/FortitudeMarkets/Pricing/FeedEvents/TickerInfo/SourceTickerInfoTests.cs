using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

[TestClass]
public class SourceTickerInfoTests
{
    public static readonly SourceTickerInfo BaseL2PriceVolumeSti = 
        new(ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
       , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
       , layerFlags: LayerFlagsExtensions.PriceVolumeLayerFlags
          , lastTradedFlags: LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo BaseL3PriceVolumeSti = 
        new(ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
       , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
       , layerFlags: LayerFlagsExtensions.PriceVolumeLayerFlags
          , lastTradedFlags: LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo SimpleL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags);

    public static readonly SourceTickerInfo SourceNameL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags);

    public static readonly SourceTickerInfo OrdersCountL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags);

    public static readonly SourceTickerInfo OrdersAnonL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags);

    public static readonly SourceTickerInfo OrdersCounterPartyL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags);

    public static readonly SourceTickerInfo ValueDateL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags);

    public static readonly SourceTickerInfo FullSupportL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags);

    public static readonly SourceTickerInfo SimpleL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags);

    public static readonly SourceTickerInfo SimpleL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo SimpleL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo SimpleL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo SimpleL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo SourceNameL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags);

    public static readonly SourceTickerInfo SourceNameL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo SourceNameL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo SourceNameL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo SourceNameL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo SourceQuoteRefL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo OrdersCountL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags);

    public static readonly SourceTickerInfo  OrdersCountL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo  OrdersCountL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  OrdersCountL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  OrdersCountL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo OrdersAnonL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags);

    public static readonly SourceTickerInfo  OrdersAnonL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo OrdersAnonL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  OrdersAnonL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  OrdersAnonL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo OrdersCounterPartyL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags);

    public static readonly SourceTickerInfo OrdersCounterPartyL3NoOnTickLastTradedSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo OrdersCounterPartyL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  OrdersCounterPartyL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  OrdersCounterPartyL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo ValueDateL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags);

    public static readonly SourceTickerInfo ValueDateL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo ValueDateL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  ValueDateL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  ValueDateL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo FullSupportL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags);

    public static readonly SourceTickerInfo FullSupportL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo FullSupportL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  FullSupportL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  FullSupportL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    private uint expectedId;

    private SourceTickerInfo sourceTickerInfo = null!;

    public static SourceTickerInfo DummySourceTickerInfo =>
        new(1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown, 10
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
            (srcId, "TestSource", tkrId, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);

        expectedId = ((uint)srcId << 16) + tkrId;

        Assert.AreEqual(expectedId, firstUniSrcTkrId.SourceInstrumentId);
        Assert.AreEqual("TestTicker", firstUniSrcTkrId.InstrumentName);
        Assert.AreEqual("TestSource", firstUniSrcTkrId.SourceName);

        var secondUniSrcTkrId = new SourceTickerInfo
            (srcId, "TestSource", tkrId, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);
        Assert.AreEqual(firstUniSrcTkrId, secondUniSrcTkrId);
    }

    [TestMethod]
    public void DummySourceTickerInfo_New_PropertiesAreAsExpected()
    {
        expectedId = ((uint)sourceTickerInfo.SourceId << 16) + sourceTickerInfo.InstrumentId;
        Assert.AreEqual(expectedId, sourceTickerInfo.SourceInstrumentId);
        Assert.AreEqual("TestSource", sourceTickerInfo.SourceName);
        Assert.AreEqual("TestTicker", sourceTickerInfo.InstrumentName);
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
            (1, "MinimalSource", 1, "MinalTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);

        expectedId = ((uint)minimalSrcTkrInfo.SourceId << 16) + minimalSrcTkrInfo.InstrumentId;
        Assert.AreEqual(expectedId, minimalSrcTkrInfo.SourceInstrumentId);
        Assert.AreEqual("MinimalSource", minimalSrcTkrInfo.SourceName);
        Assert.AreEqual("MinalTicker", minimalSrcTkrInfo.InstrumentName);
        Assert.AreEqual(SourceTickerInfo.DefaultMaximumPublishedLayers, minimalSrcTkrInfo.MaximumPublishedLayers);
        Assert.AreEqual(SourceTickerInfo.DefaultRoundingPrecision, minimalSrcTkrInfo.RoundingPrecision);
        Assert.AreEqual(SourceTickerInfo.DefaultPip, minimalSrcTkrInfo.Pip);
        Assert.AreEqual(SourceTickerInfo.DefaultMinSubmitSize, minimalSrcTkrInfo.MinSubmitSize);
        Assert.AreEqual(SourceTickerInfo.DefaultMaxSubmitSize, minimalSrcTkrInfo.MaxSubmitSize);
        Assert.AreEqual(SourceTickerInfo.DefaultIncrementSize, minimalSrcTkrInfo.IncrementSize);
        Assert.AreEqual(SourceTickerInfo.DefaultMinimumQuoteLife, minimalSrcTkrInfo.MinimumQuoteLife);
        Assert.AreEqual(SourceTickerInfo.DefaultDefaultMaxValidMs, sourceTickerInfo.DefaultMaxValidMs);
        Assert.AreEqual(SourceTickerInfo.PriceVolumeFlags, minimalSrcTkrInfo.LayerFlags);
        Assert.AreEqual(SourceTickerInfo.DefaultLastTradedFlags, minimalSrcTkrInfo.LastTradedFlags);
    }

    [TestMethod]
    public void DummySourceTickerInfo_FormatPrice_ReturnsStringFormatterToPrecision()
    {
        var formatPriceString = sourceTickerInfo.FormatPrice;
        Assert.AreEqual("0.00000", formatPriceString);

        var twoDecimalPrecision = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown,
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
            (12345, "TestSource", 12345, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);

        var nonStqi = new SourceTickerInfo
            (12345, "TestSource", 12345, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);

        Assert.IsTrue(commonStqi.AreEquivalent(nonStqi));
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent()
    {
        var commonStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);

        var sourceIdDiffStqi = new SourceTickerInfo
            (2, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);
        Assert.AreNotEqual(commonStqi, sourceIdDiffStqi);

        var tickerIdDiffStqi = new SourceTickerInfo
            (1, "TestSource", 2, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);
        Assert.AreNotEqual(commonStqi, tickerIdDiffStqi);

        var srcDiffStqi = new SourceTickerInfo
            (1, "DiffSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);
        Assert.AreNotEqual(commonStqi, srcDiffStqi);

        var tkrDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "DiffTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);
        Assert.AreNotEqual(commonStqi, tkrDiffStqi);

        var numLayersDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , 4);
        Assert.AreNotEqual(commonStqi, numLayersDiffStqi);

        var roundingPrecisionDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , 20, 0.01m);
        Assert.AreNotEqual(commonStqi, roundingPrecisionDiffStqi);

        var minSubSizeDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 1m);
        Assert.AreNotEqual(commonStqi, minSubSizeDiffStqi);

        var maxSubSizeDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000_000);
        Assert.AreNotEqual(commonStqi, maxSubSizeDiffStqi);

        var incrementSizeDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000, 1m);
        Assert.AreNotEqual(commonStqi, incrementSizeDiffStqi);

        var minQuoteLifeDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000, 0.01m, 250);
        Assert.AreNotEqual(commonStqi, minQuoteLifeDiffStqi);

        var layerFlagsDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100
           , layerFlags: LayerFlags.SourceQuoteReference | LayerFlags.ValueDate);
        Assert.AreNotEqual(commonStqi, layerFlagsDiffStqi);

        var lastTradeFlagsDiffStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , 20, 0.0001m, 0.01m, 1_000_000, 0.01m, 100
           , layerFlags: LayerFlags.Price | LayerFlags.Volume
           , lastTradedFlags: LastTradedFlags.TraderName);
        Assert.AreNotEqual(commonStqi, lastTradeFlagsDiffStqi);

        // ReSharper disable RedundantArgumentDefaultValue
        var matchingStqi = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown
           , SourceTickerInfo.DefaultMaximumPublishedLayers, SourceTickerInfo.DefaultRoundingPrecision, SourceTickerInfo.DefaultPip,
             SourceTickerInfo.DefaultMinSubmitSize, SourceTickerInfo.DefaultMaxSubmitSize, SourceTickerInfo.DefaultIncrementSize,
             SourceTickerInfo.DefaultMinimumQuoteLife, SourceTickerInfo.DefaultDefaultMaxValidMs, SourceTickerInfo.DefaultSubscribeToPrices,
             SourceTickerInfo.DefaultTradingEnabled, LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None);
        Assert.AreEqual(commonStqi, matchingStqi);
        // ReSharper restore RedundantArgumentDefaultValue
    }

    [TestMethod]
    public void PopulatedSti_GetHashCode_NotEqualTo0()
    {
        var firstSrcTkrQuoteInfo = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);

        Assert.AreNotEqual(0, firstSrcTkrQuoteInfo.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedSti_ToString_ReturnsNameAndValues()
    {
        var srcTkrInfo = new SourceTickerInfo
            (1, "TestSource", 1, "TestTicker", TickerQuoteDetailLevel.Level2Quote, Unknown);
        var toString = srcTkrInfo.ToString();

        Assert.IsTrue(toString.Contains(srcTkrInfo.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.SourceId)}: {srcTkrInfo.SourceId}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.SourceName)}: {srcTkrInfo.SourceName}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.InstrumentId)}: {srcTkrInfo.InstrumentId}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.InstrumentName)}: {srcTkrInfo.InstrumentName}"));
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
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.SubscribeToPrices)}: " +
                                        $"{srcTkrInfo.SubscribeToPrices}"));
        Assert.IsTrue(toString.Contains($"{nameof(srcTkrInfo.TradingEnabled)}: " +
                                        $"{srcTkrInfo.TradingEnabled}"));
    }

}