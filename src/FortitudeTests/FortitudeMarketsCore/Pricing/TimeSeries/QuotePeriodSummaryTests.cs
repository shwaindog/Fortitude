#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries;
using FortitudeMarketsCore.Pricing.TimeSeries;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.TimeSeries;

[TestClass]
public class QuotePeriodSummaryTests
{
    private QuotePeriodSummary emptyQuotePeriodSummary = null!;
    private decimal expectedEndAskPrice;
    private decimal expectedEndBidPrice;
    private DateTime expectedEndTime;
    private decimal expectedHighestAskPrice;
    private decimal expectedHighestBidPrice;
    private decimal expectedLowestAskPrice;
    private decimal expectedLowestBidPrice;
    private decimal expectedStartAskPrice;
    private decimal expectedStartBidPrice;
    private DateTime expectedStartTime;
    private uint expectedTickCount;

    private TimeSeriesPeriod expectedTimeSeriesPeriod;
    private long expectedVolume;
    private QuotePeriodSummary fullyPopulatedQuotePeriodSummary = null!;

    [TestInitialize]
    public void SetUp()
    {
        expectedTimeSeriesPeriod = TimeSeriesPeriod.OneMinute;
        expectedStartTime = new DateTime(2018, 2, 6, 20, 51, 0);
        expectedEndTime = new DateTime(2018, 2, 6, 20, 52, 0);
        expectedStartBidPrice = 1.234567m;
        expectedStartAskPrice = 1.234577m;
        expectedHighestBidPrice = 1.235567m;
        expectedHighestAskPrice = 1.235577m;
        expectedLowestBidPrice = 1.233567m;
        expectedLowestAskPrice = 1.233577m;
        expectedEndBidPrice = 1.234467m;
        expectedEndAskPrice = 1.234477m;
        expectedTickCount = 532;
        expectedVolume = 428700;

        emptyQuotePeriodSummary = new QuotePeriodSummary();
        fullyPopulatedQuotePeriodSummary = new QuotePeriodSummary(expectedTimeSeriesPeriod, expectedStartTime,
            expectedEndTime, expectedStartBidPrice, expectedStartAskPrice, expectedHighestBidPrice,
            expectedHighestAskPrice, expectedLowestBidPrice, expectedLowestAskPrice, expectedEndBidPrice,
            expectedEndAskPrice, expectedTickCount, expectedVolume);
    }

    [TestMethod]
    public void EmptyPeriodSummary_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(TimeSeriesPeriod.None, emptyQuotePeriodSummary.SummaryPeriod);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuotePeriodSummary.SummaryStartTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuotePeriodSummary.SummaryEndTime);
        Assert.AreEqual(0m, emptyQuotePeriodSummary.StartBidPrice);
        Assert.AreEqual(0m, emptyQuotePeriodSummary.StartAskPrice);
        Assert.AreEqual(0m, emptyQuotePeriodSummary.HighestBidPrice);
        Assert.AreEqual(0m, emptyQuotePeriodSummary.HighestAskPrice);
        Assert.AreEqual(0m, emptyQuotePeriodSummary.LowestBidPrice);
        Assert.AreEqual(0m, emptyQuotePeriodSummary.LowestAskPrice);
        Assert.AreEqual(0m, emptyQuotePeriodSummary.EndBidPrice);
        Assert.AreEqual(0m, emptyQuotePeriodSummary.EndAskPrice);
        Assert.AreEqual(0u, emptyQuotePeriodSummary.TickCount);
        Assert.AreEqual(0L, emptyQuotePeriodSummary.PeriodVolume);
    }

    [TestMethod]
    public void PopulatedPeriodSummary_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(expectedTimeSeriesPeriod, fullyPopulatedQuotePeriodSummary.SummaryPeriod);
        Assert.AreEqual(expectedStartTime, fullyPopulatedQuotePeriodSummary.SummaryStartTime);
        Assert.AreEqual(expectedEndTime, fullyPopulatedQuotePeriodSummary.SummaryEndTime);
        Assert.AreEqual(expectedStartBidPrice, fullyPopulatedQuotePeriodSummary.StartBidPrice);
        Assert.AreEqual(expectedStartAskPrice, fullyPopulatedQuotePeriodSummary.StartAskPrice);
        Assert.AreEqual(expectedHighestBidPrice, fullyPopulatedQuotePeriodSummary.HighestBidPrice);
        Assert.AreEqual(expectedHighestAskPrice, fullyPopulatedQuotePeriodSummary.HighestAskPrice);
        Assert.AreEqual(expectedLowestBidPrice, fullyPopulatedQuotePeriodSummary.LowestBidPrice);
        Assert.AreEqual(expectedLowestAskPrice, fullyPopulatedQuotePeriodSummary.LowestAskPrice);
        Assert.AreEqual(expectedEndBidPrice, fullyPopulatedQuotePeriodSummary.EndBidPrice);
        Assert.AreEqual(expectedEndAskPrice, fullyPopulatedQuotePeriodSummary.EndAskPrice);
        Assert.AreEqual(expectedTickCount, fullyPopulatedQuotePeriodSummary.TickCount);
        Assert.AreEqual(expectedVolume, fullyPopulatedQuotePeriodSummary.PeriodVolume);
    }

    [TestMethod]
    public void UnknownTimeFrame_New_CalculatesCorrectTimeFrameForPeriodSummary()
    {
        fullyPopulatedQuotePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, expectedStartTime,
            expectedEndTime, expectedStartBidPrice, expectedStartAskPrice, expectedHighestBidPrice,
            expectedHighestAskPrice, expectedLowestBidPrice, expectedLowestAskPrice, expectedEndBidPrice,
            expectedEndAskPrice, expectedTickCount, expectedVolume);

        Assert.AreEqual(expectedTimeSeriesPeriod, fullyPopulatedQuotePeriodSummary.SummaryPeriod);
    }

    [TestMethod]
    public void PopulatedPeriodSummary_New_CopiesValues()
    {
        var copyPeriodSummary = new QuotePeriodSummary(fullyPopulatedQuotePeriodSummary);

        Assert.AreEqual(fullyPopulatedQuotePeriodSummary, copyPeriodSummary);
    }

    [TestMethod]
    public void UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly()
    {
        var t = expectedStartTime;
        var calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddSeconds(1));
        Assert.AreEqual(TimeSeriesPeriod.OneSecond, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(1));
        Assert.AreEqual(TimeSeriesPeriod.OneMinute, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(5));
        Assert.AreEqual(TimeSeriesPeriod.FiveMinutes, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(10));
        Assert.AreEqual(TimeSeriesPeriod.TenMinutes, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(15));
        Assert.AreEqual(TimeSeriesPeriod.FifteenMinutes, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(30));
        Assert.AreEqual(TimeSeriesPeriod.ThirtyMinutes, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddHours(1));
        Assert.AreEqual(TimeSeriesPeriod.OneHour, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddHours(4));
        Assert.AreEqual(TimeSeriesPeriod.FourHours, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddDays(1));
        Assert.AreEqual(TimeSeriesPeriod.OneDay, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddDays(7));
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddDays(28));
        Assert.AreEqual(TimeSeriesPeriod.OneMonth, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddDays(30));
        Assert.AreEqual(TimeSeriesPeriod.OneMonth, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddSeconds(2));
        Assert.AreEqual(TimeSeriesPeriod.ConsumerConflated, calculatePeriodSummary.SummaryPeriod);
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMilliseconds(10));
        Assert.AreEqual(TimeSeriesPeriod.ConsumerConflated, calculatePeriodSummary.SummaryPeriod);
    }

    [TestMethod]
    public void PopulatedQuote_Mutate_UpdatesFields()
    {
        emptyQuotePeriodSummary.SummaryPeriod = expectedTimeSeriesPeriod;
        emptyQuotePeriodSummary.SummaryStartTime = expectedStartTime;
        emptyQuotePeriodSummary.SummaryEndTime = expectedEndTime;
        emptyQuotePeriodSummary.StartBidPrice = expectedStartBidPrice;
        emptyQuotePeriodSummary.StartAskPrice = expectedStartAskPrice;
        emptyQuotePeriodSummary.HighestBidPrice = expectedHighestBidPrice;
        emptyQuotePeriodSummary.HighestAskPrice = expectedHighestAskPrice;
        emptyQuotePeriodSummary.LowestBidPrice = expectedLowestBidPrice;
        emptyQuotePeriodSummary.LowestAskPrice = expectedLowestAskPrice;
        emptyQuotePeriodSummary.EndBidPrice = expectedEndBidPrice;
        emptyQuotePeriodSummary.EndAskPrice = expectedEndAskPrice;
        emptyQuotePeriodSummary.TickCount = expectedTickCount;
        emptyQuotePeriodSummary.PeriodVolume = expectedVolume;

        Assert.AreEqual(expectedTimeSeriesPeriod, emptyQuotePeriodSummary.SummaryPeriod);
        Assert.AreEqual(expectedStartTime, emptyQuotePeriodSummary.SummaryStartTime);
        Assert.AreEqual(expectedEndTime, emptyQuotePeriodSummary.SummaryEndTime);
        Assert.AreEqual(expectedStartBidPrice, emptyQuotePeriodSummary.StartBidPrice);
        Assert.AreEqual(expectedStartAskPrice, emptyQuotePeriodSummary.StartAskPrice);
        Assert.AreEqual(expectedHighestBidPrice, emptyQuotePeriodSummary.HighestBidPrice);
        Assert.AreEqual(expectedHighestAskPrice, emptyQuotePeriodSummary.HighestAskPrice);
        Assert.AreEqual(expectedLowestBidPrice, emptyQuotePeriodSummary.LowestBidPrice);
        Assert.AreEqual(expectedLowestAskPrice, emptyQuotePeriodSummary.LowestAskPrice);
        Assert.AreEqual(expectedEndBidPrice, emptyQuotePeriodSummary.EndBidPrice);
        Assert.AreEqual(expectedEndAskPrice, emptyQuotePeriodSummary.EndAskPrice);
        Assert.AreEqual(expectedTickCount, emptyQuotePeriodSummary.TickCount);
        Assert.AreEqual(expectedVolume, emptyQuotePeriodSummary.PeriodVolume);
    }

    [TestMethod]
    public void FullyPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_PeriodSummariesEqualEachOther()
    {
        emptyQuotePeriodSummary.CopyFrom(fullyPopulatedQuotePeriodSummary);

        Assert.AreEqual(fullyPopulatedQuotePeriodSummary, emptyQuotePeriodSummary);
    }

    [TestMethod]
    public void PQPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_PeriodSummariesEquivalentToEachOther()
    {
        var pqPeriodSummary = new PQQuotePeriodSummary(fullyPopulatedQuotePeriodSummary);
        emptyQuotePeriodSummary.CopyFrom(fullyPopulatedQuotePeriodSummary);
        Assert.IsTrue(emptyQuotePeriodSummary.AreEquivalent(pqPeriodSummary));
    }

    [TestMethod]
    public void PopulatedPeriodSummary_Clone_CreatesCopyOfEverythingExceptSrcTkrQtInfo()
    {
        var clone = fullyPopulatedQuotePeriodSummary.Clone();
        Assert.AreEqual(fullyPopulatedQuotePeriodSummary, clone);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)fullyPopulatedQuotePeriodSummary).Clone();
        Assert.AreNotSame(clone, fullyPopulatedQuotePeriodSummary);
        Assert.AreEqual(fullyPopulatedQuotePeriodSummary, clone);
        clone = fullyPopulatedQuotePeriodSummary.Clone();
        Assert.AreNotSame(clone, fullyPopulatedQuotePeriodSummary);
        Assert.AreEqual(fullyPopulatedQuotePeriodSummary, clone);
        clone = ((ICloneable<IQuotePeriodSummary>)fullyPopulatedQuotePeriodSummary).Clone();
        Assert.AreNotSame(clone, fullyPopulatedQuotePeriodSummary);
        Assert.AreEqual(fullyPopulatedQuotePeriodSummary, clone);
        clone = ((IMutableQuotePeriodSummary)fullyPopulatedQuotePeriodSummary).Clone();
        Assert.AreNotSame(clone, fullyPopulatedQuotePeriodSummary);
        Assert.AreEqual(fullyPopulatedQuotePeriodSummary, clone);
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        AssertOneDifferenceAtATime(false, fullyPopulatedQuotePeriodSummary,
            fullyPopulatedQuotePeriodSummary.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedQuotePeriodSummary.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q = fullyPopulatedQuotePeriodSummary;
        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.SummaryPeriod)}: {q.SummaryPeriod}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SummaryStartTime)}: {q.SummaryStartTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SummaryEndTime)}: {q.SummaryEndTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.StartBidPrice)}: {q.StartBidPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.StartAskPrice)}: {q.StartAskPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.HighestBidPrice)}: {q.HighestBidPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.HighestAskPrice)}: {q.HighestAskPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.LowestBidPrice)}: {q.LowestBidPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.LowestAskPrice)}: {q.LowestAskPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.EndBidPrice)}: {q.EndBidPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.EndAskPrice)}: {q.EndAskPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.TickCount)}: {q.TickCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.PeriodVolume)}: {q.PeriodVolume:N2}"));
    }

    internal static void AssertOneDifferenceAtATime(bool exactComparison, IMutableQuotePeriodSummary commonQuotePeriodSummary,
        IMutableQuotePeriodSummary changingQuotePeriodSummary)
    {
        changingQuotePeriodSummary.SummaryPeriod = TimeSeriesPeriod.FourHours;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.SummaryPeriod = commonQuotePeriodSummary.SummaryPeriod;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.SummaryStartTime = DateTime.Now;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.SummaryStartTime = commonQuotePeriodSummary.SummaryStartTime;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.SummaryEndTime = DateTime.Now;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.SummaryEndTime = commonQuotePeriodSummary.SummaryEndTime;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.StartBidPrice = 6.78901m;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.StartBidPrice = commonQuotePeriodSummary.StartBidPrice;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.StartAskPrice = 6.78901m;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.StartAskPrice = commonQuotePeriodSummary.StartAskPrice;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.HighestBidPrice = 6.78901m;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.HighestBidPrice = commonQuotePeriodSummary.HighestBidPrice;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.HighestAskPrice = 6.78901m;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.HighestAskPrice = commonQuotePeriodSummary.HighestAskPrice;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.LowestBidPrice = 6.78901m;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.LowestBidPrice = commonQuotePeriodSummary.LowestBidPrice;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.LowestAskPrice = 6.78901m;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.LowestAskPrice = commonQuotePeriodSummary.LowestAskPrice;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.EndBidPrice = 6.78901m;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.EndBidPrice = commonQuotePeriodSummary.EndBidPrice;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.EndAskPrice = 6.78901m;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.EndAskPrice = commonQuotePeriodSummary.EndAskPrice;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.TickCount = 23456u;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.TickCount = commonQuotePeriodSummary.TickCount;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));

        changingQuotePeriodSummary.PeriodVolume = 98765L;
        Assert.IsFalse(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
        changingQuotePeriodSummary.PeriodVolume = commonQuotePeriodSummary.PeriodVolume;
        Assert.IsTrue(commonQuotePeriodSummary.AreEquivalent(changingQuotePeriodSummary));
    }
}
