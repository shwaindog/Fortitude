// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

[TestClass]
public class TickInstantTests
{
    private TickInstant emptyQuote                = null!;
    private TickInstant fullyPopulatedTickInstant = null!;
    private TickInstant newlyPopulatedTickInstant = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private ISourceTickerInfo sourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        emptyQuote                = new TickInstant(sourceTickerInfo);
        fullyPopulatedTickInstant = new TickInstant(sourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedTickInstant, 1);
        newlyPopulatedTickInstant = new TickInstant(sourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(newlyPopulatedTickInstant, 2);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(sourceTickerInfo, emptyQuote.SourceTickerInfo);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ClientReceivedTime);
    }

    [TestMethod]
    public void IntializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);

        var expectedSinglePrice = 1.23456m;

        var fromConstructor = new TickInstant
            (sourceTickerInfo, expectedSourceTime, true, FeedSyncStatus.Good, expectedSinglePrice, expectedClientReceivedTime);

        Assert.AreSame(sourceTickerInfo, fromConstructor.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(true, fromConstructor.IsReplay);
        Assert.AreEqual(expectedSinglePrice, fromConstructor.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, fromConstructor.ClientReceivedTime);
    }

    [TestMethod]
    public void NonSourceTickerInfo_New_ConvertsToSourceTickerInfo()
    {
        var pqSourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);

        var nonSourceTickerInfoTickInstant = new TickInstant(pqSourceTickerInfo);

        Assert.IsInstanceOfType(nonSourceTickerInfoTickInstant.SourceTickerInfo, typeof(SourceTickerInfo));
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValuesExceptQuoteInfo()
    {
        var copyQuote = new TickInstant(fullyPopulatedTickInstant);

        Assert.AreEqual(fullyPopulatedTickInstant, copyQuote);
    }

    [TestMethod]
    public void NonSourceTickerInfo_New_CopiesExceptSourceTickerInfoIsConverted()
    {
        var pqSourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);
        fullyPopulatedTickInstant.SourceTickerInfo = pqSourceTickerInfo;
        var copyQuote = new TickInstant(fullyPopulatedTickInstant);
        Assert.AreNotEqual(fullyPopulatedTickInstant, copyQuote);
        fullyPopulatedTickInstant.SourceTickerInfo = sourceTickerInfo;
        Assert.AreEqual(fullyPopulatedTickInstant, copyQuote);
    }

    [TestMethod]
    public void EmptyQuote_Mutate_UpdatesFields()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);

        var expectedSingleValue = 1.23456m;

        emptyQuote.IsReplay           = true;
        emptyQuote.SourceTime         = expectedSourceTime;
        emptyQuote.ClientReceivedTime = expectedClientReceivedTime;
        emptyQuote.SingleTickValue    = expectedSingleValue;

        Assert.AreSame(sourceTickerInfo, emptyQuote.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
        Assert.AreEqual(true, emptyQuote.IsReplay);
        Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, emptyQuote.ClientReceivedTime);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new TickInstant(sourceTickerInfo);
        emptyQuote.CopyFrom(fullyPopulatedTickInstant);

        Assert.AreEqual(fullyPopulatedTickInstant, emptyQuote);
    }

    [TestMethod]
    public void PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var pqTickInstant = new PQTickInstant(fullyPopulatedTickInstant);
        emptyQuote.CopyFrom(fullyPopulatedTickInstant);
        Assert.IsTrue(emptyQuote.AreEquivalent(pqTickInstant));
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        IMutableTickInstant clone = fullyPopulatedTickInstant.Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = (IMutableTickInstant)((ICloneable<ITickInstant>)fullyPopulatedTickInstant).Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = (IMutableTickInstant)((ITickInstant)fullyPopulatedTickInstant).Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = ((IMutableTickInstant)fullyPopulatedTickInstant).Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, fullyPopulatedTickInstant, fullyPopulatedTickInstant.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedTickInstant.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q        = fullyPopulatedTickInstant;
        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTickerInfo)}: {q.SourceTickerInfo}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsReplay)}: {q.IsReplay}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SingleTickValue)}: {q.SingleTickValue}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.ClientReceivedTime)}: {q.ClientReceivedTime:O}"));
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableTickInstant commonCompareQuote, IMutableTickInstant changingQuote)
    {
        var diffSrcTkrInfo = commonCompareQuote.SourceTickerInfo!.Clone();
        diffSrcTkrInfo.Source          = "DifferSourceName";
        changingQuote.SourceTickerInfo = diffSrcTkrInfo;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceTickerInfo.Source = commonCompareQuote.SourceTickerInfo.Source;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SourceTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceTime = commonCompareQuote.SourceTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsReplay = !commonCompareQuote.IsReplay;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsReplay = commonCompareQuote.IsReplay;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SingleTickValue = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SingleTickValue = commonCompareQuote.SingleTickValue;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.ClientReceivedTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.ClientReceivedTime = commonCompareQuote.ClientReceivedTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));
    }
}
