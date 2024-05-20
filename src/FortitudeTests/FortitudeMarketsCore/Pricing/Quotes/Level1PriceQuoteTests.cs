#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.TimeSeries;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

[TestClass]
public class Level1PriceQuoteTests
{
    private Level1PriceQuote emptyQuote = null!;
    private Level1PriceQuote fullyPopulatedLevel1Quote = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private ISourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        sourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
            | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                                                  | LastTradedFlags.LastTradedVolume |
                                                                  LastTradedFlags.LastTradedTime);
        emptyQuote = new Level1PriceQuote(sourceTickerQuoteInfo);
        fullyPopulatedLevel1Quote = new Level1PriceQuote(sourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedLevel1Quote, 1);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(sourceTickerQuoteInfo, emptyQuote.SourceTickerQuoteInfo);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SinglePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ClientReceivedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterSentTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceBidTime);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceAskTime);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.AreEqual(false, emptyQuote.Executable);
        Assert.IsNull(emptyQuote.SummaryPeriod);
    }

    [TestMethod]
    public void IntializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedSourceTime = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSinglePrice = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop = 2.34567m;
        var expectedSourceAskTime = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop = 3.45678m;
        var expectedPeriodSummary = new PricePeriodSummary();

        var fromConstructor = new Level1PriceQuote(sourceTickerQuoteInfo, expectedSourceTime, true,
            expectedSinglePrice, expectedClientReceivedTime, expectedAdapterReceiveTime, expectedAdapterSentTime,
            expectedSourceBidTime, expectedBidPriceTop, true, expectedSourceAskTime, expectedAskPriceTop,
            true, true, expectedPeriodSummary);

        Assert.AreSame(sourceTickerQuoteInfo, fromConstructor.SourceTickerQuoteInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(true, fromConstructor.IsReplay);
        Assert.AreEqual(expectedSinglePrice, fromConstructor.SinglePrice);
        Assert.AreEqual(expectedClientReceivedTime, fromConstructor.ClientReceivedTime);
        Assert.AreEqual(expectedAdapterReceiveTime, fromConstructor.AdapterReceivedTime);
        Assert.AreEqual(expectedAdapterSentTime, fromConstructor.AdapterSentTime);
        Assert.AreEqual(expectedSourceBidTime, fromConstructor.SourceBidTime);
        Assert.AreEqual(expectedBidPriceTop, fromConstructor.BidPriceTop);
        Assert.AreEqual(true, fromConstructor.IsBidPriceTopUpdated);
        Assert.AreEqual(expectedSourceAskTime, fromConstructor.SourceAskTime);
        Assert.AreEqual(expectedAskPriceTop, fromConstructor.AskPriceTop);
        Assert.AreEqual(true, fromConstructor.IsAskPriceTopUpdated);
        Assert.AreEqual(true, fromConstructor.Executable);
        Assert.AreEqual(expectedPeriodSummary, fromConstructor.SummaryPeriod);
    }

    [TestMethod]
    public void NonPeriodSummary_New_ConvertsToPeriodSummary()
    {
        var pqPeriodSummary = new PQPricePeriodSummary();

        var nonSourceTickerQuoteInfoQuote = new Level1PriceQuote(sourceTickerQuoteInfo, DateTime.Now, true,
            1m, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, 1m, true, DateTime.Now, 1m,
            true, true, pqPeriodSummary);

        Assert.IsInstanceOfType(nonSourceTickerQuoteInfoQuote.SummaryPeriod, typeof(PricePeriodSummary));
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValues()
    {
        var copyQuote = new Level1PriceQuote(fullyPopulatedLevel1Quote);

        Assert.AreEqual(fullyPopulatedLevel1Quote, copyQuote);
    }

    [TestMethod]
    public void NonSourceTickerQuoteInfo_New_CopiesExceptPeriodSummaryIsConverted()
    {
        var originalPeriodSummary = fullyPopulatedLevel1Quote.SummaryPeriod!;
        var pqPeriodSummary = new PQPricePeriodSummary(originalPeriodSummary);
        fullyPopulatedLevel1Quote.SummaryPeriod = pqPeriodSummary;
        var copyQuote = new Level1PriceQuote(fullyPopulatedLevel1Quote);
        Assert.AreNotEqual(fullyPopulatedLevel1Quote, copyQuote);
        fullyPopulatedLevel1Quote.SummaryPeriod = originalPeriodSummary;
        Assert.AreEqual(fullyPopulatedLevel1Quote, copyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_SourceTimeIsGreaterOfBidAskOrOriginalSourceTime()
    {
        var originalSourceTime = new DateTime(2017, 11, 08, 22, 30, 51);
        emptyQuote.SourceTime = originalSourceTime;
        Assert.AreEqual(originalSourceTime, emptyQuote.SourceTime);

        var higherAskTime = originalSourceTime.AddMilliseconds(123);
        emptyQuote.SourceAskTime = higherAskTime;
        Assert.AreEqual(higherAskTime, emptyQuote.SourceTime);

        var higherBidTime = higherAskTime.AddMilliseconds(123);
        emptyQuote.SourceBidTime = higherBidTime;
        Assert.AreEqual(higherBidTime, emptyQuote.SourceTime);

        var highestSourceTime = higherBidTime.AddMilliseconds(123);
        emptyQuote.SourceTime = highestSourceTime;
        Assert.AreEqual(highestSourceTime, emptyQuote.SourceTime);
    }

    [TestMethod]
    public void PopulatedQuote_Mutate_UpdatesFields()
    {
        var expectedSourceTime = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSinglePrice = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop = 2.34567m;
        var expectedSourceAskTime = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop = 3.45678m;
        var expectedPeriodSummary = new PricePeriodSummary();

        emptyQuote.SourceTime = expectedSourceTime;
        emptyQuote.IsReplay = true;
        emptyQuote.SinglePrice = expectedSinglePrice;
        emptyQuote.ClientReceivedTime = expectedClientReceivedTime;
        emptyQuote.AdapterReceivedTime = expectedAdapterReceiveTime;
        emptyQuote.AdapterSentTime = expectedAdapterSentTime;
        emptyQuote.SourceBidTime = expectedSourceBidTime;
        emptyQuote.BidPriceTop = expectedBidPriceTop;
        emptyQuote.IsBidPriceTopUpdated = true;
        emptyQuote.SourceAskTime = expectedSourceAskTime;
        emptyQuote.AskPriceTop = expectedAskPriceTop;
        emptyQuote.IsAskPriceTopUpdated = true;
        emptyQuote.Executable = true;
        emptyQuote.SummaryPeriod = expectedPeriodSummary;

        Assert.AreSame(sourceTickerQuoteInfo, emptyQuote.SourceTickerQuoteInfo);
        Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
        Assert.AreEqual(true, emptyQuote.IsReplay);
        Assert.AreEqual(expectedSinglePrice, emptyQuote.SinglePrice);
        Assert.AreEqual(expectedClientReceivedTime, emptyQuote.ClientReceivedTime);
        Assert.AreEqual(expectedAdapterReceiveTime, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(expectedAdapterSentTime, emptyQuote.AdapterSentTime);
        Assert.AreEqual(expectedSourceBidTime, emptyQuote.SourceBidTime);
        Assert.AreEqual(expectedBidPriceTop, emptyQuote.BidPriceTop);
        Assert.AreEqual(true, emptyQuote.IsBidPriceTopUpdated);
        Assert.AreEqual(expectedSourceAskTime, emptyQuote.SourceAskTime);
        Assert.AreEqual(expectedAskPriceTop, emptyQuote.AskPriceTop);
        Assert.AreEqual(true, emptyQuote.IsAskPriceTopUpdated);
        Assert.AreEqual(true, emptyQuote.Executable);
        Assert.AreEqual(expectedPeriodSummary, emptyQuote.SummaryPeriod);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new Level1PriceQuote(sourceTickerQuoteInfo);
        emptyQuote.CopyFrom(fullyPopulatedLevel1Quote);

        Assert.AreEqual(fullyPopulatedLevel1Quote, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent()
    {
        var emptyLowerLevelQuote = new Level0PriceQuote(sourceTickerQuoteInfo);
        emptyLowerLevelQuote.CopyFrom(fullyPopulatedLevel1Quote);

        Assert.AreNotEqual(fullyPopulatedLevel1Quote, emptyLowerLevelQuote);
        Assert.IsTrue(emptyLowerLevelQuote.AreEquivalent(fullyPopulatedLevel1Quote));
    }

    [TestMethod]
    public void PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var pqLevel1Quote = new PQLevel1Quote(fullyPopulatedLevel1Quote);
        emptyQuote.CopyFrom(fullyPopulatedLevel1Quote);
        Assert.IsTrue(emptyQuote.AreEquivalent(pqLevel1Quote));
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = fullyPopulatedLevel1Quote.Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = (IMutableLevel0Quote)((ICloneable<ILevel0Quote>)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = (IMutableLevel0Quote)((ILevel0Quote)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = ((IMutableLevel0Quote)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = (IMutableLevel0Quote)((ICloneable<ILevel1Quote>)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = (IMutableLevel0Quote)((ILevel1Quote)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = ((IMutableLevel1Quote)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedLevel1Quote,
            (IMutableLevel1Quote)fullyPopulatedLevel1Quote.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedLevel1Quote.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q = fullyPopulatedLevel1Quote;
        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTickerQuoteInfo)}: {q.SourceTickerQuoteInfo}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsReplay)}: {q.IsReplay}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SinglePrice)}: {q.SinglePrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.ClientReceivedTime)}: {q.ClientReceivedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.AdapterReceivedTime)}: {q.AdapterReceivedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.AdapterSentTime)}: {q.AdapterSentTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceBidTime)}: {q.SourceBidTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.BidPriceTop)}: {q.BidPriceTop:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsBidPriceTopUpdated)}: {q.IsBidPriceTopUpdated}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceAskTime)}: {q.SourceAskTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.AskPriceTop)}: {q.AskPriceTop:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsAskPriceTopUpdated)}: {q.IsAskPriceTopUpdated}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.Executable)}: {q.Executable}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SummaryPeriod)}: {q.SummaryPeriod}"));
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IMutableLevel1Quote commonCompareQuote, IMutableLevel1Quote changingQuote)
    {
        Level0PriceQuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            commonCompareQuote, changingQuote);

        var diffPeriodSummary = commonCompareQuote.SummaryPeriod!.Clone();
        diffPeriodSummary.HighestAskPrice = 3.45678m;
        changingQuote.SummaryPeriod = diffPeriodSummary;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SummaryPeriod = commonCompareQuote.SummaryPeriod.Clone();
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.AdapterReceivedTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.AdapterReceivedTime = commonCompareQuote.AdapterReceivedTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.AdapterSentTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.AdapterSentTime = commonCompareQuote.AdapterSentTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SourceBidTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceBidTime = commonCompareQuote.SourceBidTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.BidPriceTop = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.BidPriceTop = commonCompareQuote.BidPriceTop;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsBidPriceTopUpdated = !commonCompareQuote.IsBidPriceTopUpdated;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsBidPriceTopUpdated = commonCompareQuote.IsBidPriceTopUpdated;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SourceAskTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceAskTime = commonCompareQuote.SourceAskTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.AskPriceTop = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.AskPriceTop = commonCompareQuote.AskPriceTop;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsAskPriceTopUpdated = !commonCompareQuote.IsAskPriceTopUpdated;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsAskPriceTopUpdated = commonCompareQuote.IsAskPriceTopUpdated;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.Executable = !commonCompareQuote.Executable;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.Executable = commonCompareQuote.Executable;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));
    }
}
