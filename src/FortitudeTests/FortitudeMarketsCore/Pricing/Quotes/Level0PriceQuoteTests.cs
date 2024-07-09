// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

[TestClass]
public class Level0PriceQuoteTests
{
    private Level0PriceQuote emptyQuote                = null!;
    private Level0PriceQuote fullyPopulatedLevel0Quote = null!;
    private Level0PriceQuote newlyPopulatedLevel0Quote = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private ISourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        sourceTickerQuoteInfo = new SourceTickerQuoteInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        emptyQuote                = new Level0PriceQuote(sourceTickerQuoteInfo);
        fullyPopulatedLevel0Quote = new Level0PriceQuote(sourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedLevel0Quote, 1);
        newlyPopulatedLevel0Quote = new Level0PriceQuote(sourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(newlyPopulatedLevel0Quote, 2);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(sourceTickerQuoteInfo, emptyQuote.SourceTickerQuoteInfo);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SinglePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ClientReceivedTime);
    }

    [TestMethod]
    public void IntializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);

        var expectedSinglePrice = 1.23456m;

        var fromConstructor = new Level0PriceQuote
            (sourceTickerQuoteInfo, expectedSourceTime, true, expectedSinglePrice, expectedClientReceivedTime);

        Assert.AreSame(sourceTickerQuoteInfo, fromConstructor.SourceTickerQuoteInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(true, fromConstructor.IsReplay);
        Assert.AreEqual(expectedSinglePrice, fromConstructor.SinglePrice);
        Assert.AreEqual(expectedClientReceivedTime, fromConstructor.ClientReceivedTime);
    }

    [TestMethod]
    public void NonSourceTickerQuoteInfo_New_ConvertsToSourceTickerQuoteInfo()
    {
        var pqSourceTickerQuoteInfo = new PQSourceTickerQuoteInfo(sourceTickerQuoteInfo);

        var nonSourceTickerQuoteInfoQuote = new Level0PriceQuote(pqSourceTickerQuoteInfo);

        Assert.IsInstanceOfType(nonSourceTickerQuoteInfoQuote.SourceTickerQuoteInfo, typeof(SourceTickerQuoteInfo));
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValuesExceptQuoteInfo()
    {
        var copyQuote = new Level0PriceQuote(fullyPopulatedLevel0Quote);

        Assert.AreEqual(fullyPopulatedLevel0Quote, copyQuote);
    }

    [TestMethod]
    public void NonSourceTickerQuoteInfo_New_CopiesExceptSourceTickerQuoteInfoIsConverted()
    {
        var pqSourceTickerQuoteInfo = new PQSourceTickerQuoteInfo(sourceTickerQuoteInfo);
        fullyPopulatedLevel0Quote.SourceTickerQuoteInfo = pqSourceTickerQuoteInfo;
        var copyQuote = new Level0PriceQuote(fullyPopulatedLevel0Quote);
        Assert.AreNotEqual(fullyPopulatedLevel0Quote, copyQuote);
        fullyPopulatedLevel0Quote.SourceTickerQuoteInfo = sourceTickerQuoteInfo;
        Assert.AreEqual(fullyPopulatedLevel0Quote, copyQuote);
    }

    [TestMethod]
    public void EmptyQuote_Mutate_UpdatesFields()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);

        var expectedSinglePrice = 1.23456m;

        emptyQuote.IsReplay           = true;
        emptyQuote.SourceTime         = expectedSourceTime;
        emptyQuote.ClientReceivedTime = expectedClientReceivedTime;
        emptyQuote.SinglePrice        = expectedSinglePrice;

        Assert.AreSame(sourceTickerQuoteInfo, emptyQuote.SourceTickerQuoteInfo);
        Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
        Assert.AreEqual(true, emptyQuote.IsReplay);
        Assert.AreEqual(expectedSinglePrice, emptyQuote.SinglePrice);
        Assert.AreEqual(expectedClientReceivedTime, emptyQuote.ClientReceivedTime);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new Level0PriceQuote(sourceTickerQuoteInfo);
        emptyQuote.CopyFrom(fullyPopulatedLevel0Quote);

        Assert.AreEqual(fullyPopulatedLevel0Quote, emptyQuote);
    }

    [TestMethod]
    public void PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var pqLevel0Quote = new PQLevel0Quote(fullyPopulatedLevel0Quote);
        emptyQuote.CopyFrom(fullyPopulatedLevel0Quote);
        Assert.IsTrue(emptyQuote.AreEquivalent(pqLevel0Quote));
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = fullyPopulatedLevel0Quote.Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel0Quote);
        Assert.AreEqual(fullyPopulatedLevel0Quote, clone);
        clone = (IMutableLevel0Quote)((ICloneable<ILevel0Quote>)fullyPopulatedLevel0Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel0Quote);
        Assert.AreEqual(fullyPopulatedLevel0Quote, clone);
        clone = (IMutableLevel0Quote)((ILevel0Quote)fullyPopulatedLevel0Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel0Quote);
        Assert.AreEqual(fullyPopulatedLevel0Quote, clone);
        clone = ((IMutableLevel0Quote)fullyPopulatedLevel0Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel0Quote);
        Assert.AreEqual(fullyPopulatedLevel0Quote, clone);
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, fullyPopulatedLevel0Quote, fullyPopulatedLevel0Quote.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedLevel0Quote.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q        = fullyPopulatedLevel0Quote;
        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTickerQuoteInfo)}: {q.SourceTickerQuoteInfo}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsReplay)}: {q.IsReplay}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SinglePrice)}: {q.SinglePrice}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.ClientReceivedTime)}: {q.ClientReceivedTime:O}"));
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableLevel0Quote commonCompareQuote, IMutableLevel0Quote changingQuote)
    {
        var diffSrcTkrQtInfo = commonCompareQuote.SourceTickerQuoteInfo!.Clone();
        diffSrcTkrQtInfo.Source             = "DifferSourceName";
        changingQuote.SourceTickerQuoteInfo = diffSrcTkrQtInfo;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceTickerQuoteInfo.Source = commonCompareQuote.SourceTickerQuoteInfo.Source;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SourceTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceTime = commonCompareQuote.SourceTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsReplay = !commonCompareQuote.IsReplay;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsReplay = commonCompareQuote.IsReplay;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SinglePrice = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SinglePrice = commonCompareQuote.SinglePrice;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.ClientReceivedTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.ClientReceivedTime = commonCompareQuote.ClientReceivedTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));
    }
}
