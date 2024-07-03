// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Summaries;

[TestClass]
public class PricePeriodSummaryTests
{
    private PricePeriodSummary emptyPricePeriodSummary = null!;

    private decimal  expectedEndAskPrice;
    private decimal  expectedEndBidPrice;
    private DateTime expectedEndTime;
    private decimal  expectedHighestAskPrice;
    private decimal  expectedHighestBidPrice;
    private decimal  expectedLowestAskPrice;
    private decimal  expectedLowestBidPrice;
    private decimal  expectedStartAskPrice;
    private decimal  expectedStartBidPrice;
    private DateTime expectedStartTime;
    private uint     expectedTickCount;

    private TimeSeriesPeriod expectedTimeSeriesPeriod;
    private long             expectedVolume;

    private PricePeriodSummary fullyPopulatedPricePeriodSummary = null!;

    [TestInitialize]
    public void SetUp()
    {
        expectedTimeSeriesPeriod = TimeSeriesPeriod.OneMinute;
        expectedStartTime        = new DateTime(2018, 2, 6, 20, 51, 0);
        expectedEndTime          = new DateTime(2018, 2, 6, 20, 52, 0);
        expectedStartBidPrice    = 1.234567m;
        expectedStartAskPrice    = 1.234577m;
        expectedHighestBidPrice  = 1.235567m;
        expectedHighestAskPrice  = 1.235577m;
        expectedLowestBidPrice   = 1.233567m;
        expectedLowestAskPrice   = 1.233577m;
        expectedEndBidPrice      = 1.234467m;
        expectedEndAskPrice      = 1.234477m;
        expectedTickCount        = 532;
        expectedVolume           = 428700;

        emptyPricePeriodSummary = new PricePeriodSummary();
        fullyPopulatedPricePeriodSummary =
            new PricePeriodSummary
                (expectedTimeSeriesPeriod, expectedStartTime, expectedEndTime, expectedStartBidPrice, expectedStartAskPrice
               , expectedHighestBidPrice, expectedHighestAskPrice, expectedLowestBidPrice, expectedLowestAskPrice
               , expectedEndBidPrice, expectedEndAskPrice, expectedTickCount, expectedVolume);
    }

    [TestMethod]
    public void EmptyPeriodSummary_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(TimeSeriesPeriod.None, emptyPricePeriodSummary.TimeSeriesPeriod);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPricePeriodSummary.PeriodStartTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPricePeriodSummary.PeriodEndTime);

        Assert.AreEqual(0m, emptyPricePeriodSummary.StartBidPrice);
        Assert.AreEqual(0m, emptyPricePeriodSummary.StartAskPrice);
        Assert.AreEqual(0m, emptyPricePeriodSummary.HighestBidPrice);
        Assert.AreEqual(0m, emptyPricePeriodSummary.HighestAskPrice);
        Assert.AreEqual(0m, emptyPricePeriodSummary.LowestBidPrice);
        Assert.AreEqual(0m, emptyPricePeriodSummary.LowestAskPrice);
        Assert.AreEqual(0m, emptyPricePeriodSummary.EndBidPrice);
        Assert.AreEqual(0m, emptyPricePeriodSummary.EndAskPrice);
        Assert.AreEqual(0u, emptyPricePeriodSummary.TickCount);
        Assert.AreEqual(0L, emptyPricePeriodSummary.PeriodVolume);
    }

    [TestMethod]
    public void PopulatedPeriodSummary_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(expectedTimeSeriesPeriod, fullyPopulatedPricePeriodSummary.TimeSeriesPeriod);
        Assert.AreEqual(expectedStartTime, fullyPopulatedPricePeriodSummary.PeriodStartTime);
        Assert.AreEqual(expectedEndTime, fullyPopulatedPricePeriodSummary.PeriodEndTime);
        Assert.AreEqual(expectedStartBidPrice, fullyPopulatedPricePeriodSummary.StartBidPrice);
        Assert.AreEqual(expectedStartAskPrice, fullyPopulatedPricePeriodSummary.StartAskPrice);
        Assert.AreEqual(expectedHighestBidPrice, fullyPopulatedPricePeriodSummary.HighestBidPrice);
        Assert.AreEqual(expectedHighestAskPrice, fullyPopulatedPricePeriodSummary.HighestAskPrice);
        Assert.AreEqual(expectedLowestBidPrice, fullyPopulatedPricePeriodSummary.LowestBidPrice);
        Assert.AreEqual(expectedLowestAskPrice, fullyPopulatedPricePeriodSummary.LowestAskPrice);
        Assert.AreEqual(expectedEndBidPrice, fullyPopulatedPricePeriodSummary.EndBidPrice);
        Assert.AreEqual(expectedEndAskPrice, fullyPopulatedPricePeriodSummary.EndAskPrice);
        Assert.AreEqual(expectedTickCount, fullyPopulatedPricePeriodSummary.TickCount);
        Assert.AreEqual(expectedVolume, fullyPopulatedPricePeriodSummary.PeriodVolume);
    }

    [TestMethod]
    public void UnknownTimeFrame_New_CalculatesCorrectTimeFrameForPeriodSummary()
    {
        fullyPopulatedPricePeriodSummary =
            new PricePeriodSummary
                (expectedTimeSeriesPeriod, expectedStartTime, expectedEndTime, expectedStartBidPrice, expectedStartAskPrice
               , expectedHighestBidPrice, expectedHighestAskPrice, expectedLowestBidPrice, expectedLowestAskPrice
               , expectedEndBidPrice, expectedEndAskPrice, expectedTickCount, expectedVolume);

        Assert.AreEqual(expectedTimeSeriesPeriod, fullyPopulatedPricePeriodSummary.TimeSeriesPeriod);
    }

    [TestMethod]
    public void PopulatedPeriodSummary_New_CopiesValues()
    {
        var copyPeriodSummary = new PricePeriodSummary(fullyPopulatedPricePeriodSummary);

        Assert.AreEqual(fullyPopulatedPricePeriodSummary, copyPeriodSummary);
    }

    [TestMethod]
    public void UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly()
    {
        var t = expectedStartTime;

        var calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.OneSecond, t, t.AddSeconds(1));
        Assert.AreEqual(TimeSeriesPeriod.OneSecond, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.OneMinute, t, t.AddMinutes(1));
        Assert.AreEqual(TimeSeriesPeriod.OneMinute, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.FiveMinutes, t, t.AddMinutes(5));
        Assert.AreEqual(TimeSeriesPeriod.FiveMinutes, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.TenMinutes, t, t.AddMinutes(10));
        Assert.AreEqual(TimeSeriesPeriod.TenMinutes, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.FifteenMinutes, t, t.AddMinutes(15));
        Assert.AreEqual(TimeSeriesPeriod.FifteenMinutes, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.ThirtyMinutes, t, t.AddMinutes(30));
        Assert.AreEqual(TimeSeriesPeriod.ThirtyMinutes, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.OneHour, t, t.AddHours(1));
        Assert.AreEqual(TimeSeriesPeriod.OneHour, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.FourHours, t, t.AddHours(4));
        Assert.AreEqual(TimeSeriesPeriod.FourHours, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.OneDay, t, t.AddDays(1));
        Assert.AreEqual(TimeSeriesPeriod.OneDay, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.OneWeek, t, t.AddDays(7));
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.OneMonth, t, t.AddDays(28));
        Assert.AreEqual(TimeSeriesPeriod.OneMonth, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.OneMonth, t, t.AddDays(30));
        Assert.AreEqual(TimeSeriesPeriod.OneMonth, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.None, t, t.AddSeconds(2));
        Assert.AreEqual(TimeSeriesPeriod.None, calculatePeriodSummary.TimeSeriesPeriod);
        calculatePeriodSummary = new PricePeriodSummary(TimeSeriesPeriod.None, t, t.AddMilliseconds(10));
        Assert.AreEqual(TimeSeriesPeriod.None, calculatePeriodSummary.TimeSeriesPeriod);
    }

    [TestMethod]
    public void PopulatedQuote_Mutate_UpdatesFields()
    {
        emptyPricePeriodSummary.TimeSeriesPeriod = expectedTimeSeriesPeriod;
        emptyPricePeriodSummary.PeriodStartTime  = expectedStartTime;
        emptyPricePeriodSummary.PeriodEndTime    = expectedEndTime;
        emptyPricePeriodSummary.StartBidPrice    = expectedStartBidPrice;
        emptyPricePeriodSummary.StartAskPrice    = expectedStartAskPrice;
        emptyPricePeriodSummary.HighestBidPrice  = expectedHighestBidPrice;
        emptyPricePeriodSummary.HighestAskPrice  = expectedHighestAskPrice;
        emptyPricePeriodSummary.LowestBidPrice   = expectedLowestBidPrice;
        emptyPricePeriodSummary.LowestAskPrice   = expectedLowestAskPrice;
        emptyPricePeriodSummary.EndBidPrice      = expectedEndBidPrice;
        emptyPricePeriodSummary.EndAskPrice      = expectedEndAskPrice;
        emptyPricePeriodSummary.TickCount        = expectedTickCount;
        emptyPricePeriodSummary.PeriodVolume     = expectedVolume;

        Assert.AreEqual(expectedTimeSeriesPeriod, emptyPricePeriodSummary.TimeSeriesPeriod);
        Assert.AreEqual(expectedStartTime, emptyPricePeriodSummary.PeriodStartTime);
        Assert.AreEqual(expectedEndTime, emptyPricePeriodSummary.PeriodEndTime);
        Assert.AreEqual(expectedStartBidPrice, emptyPricePeriodSummary.StartBidPrice);
        Assert.AreEqual(expectedStartAskPrice, emptyPricePeriodSummary.StartAskPrice);
        Assert.AreEqual(expectedHighestBidPrice, emptyPricePeriodSummary.HighestBidPrice);
        Assert.AreEqual(expectedHighestAskPrice, emptyPricePeriodSummary.HighestAskPrice);
        Assert.AreEqual(expectedLowestBidPrice, emptyPricePeriodSummary.LowestBidPrice);
        Assert.AreEqual(expectedLowestAskPrice, emptyPricePeriodSummary.LowestAskPrice);
        Assert.AreEqual(expectedEndBidPrice, emptyPricePeriodSummary.EndBidPrice);
        Assert.AreEqual(expectedEndAskPrice, emptyPricePeriodSummary.EndAskPrice);
        Assert.AreEqual(expectedTickCount, emptyPricePeriodSummary.TickCount);
        Assert.AreEqual(expectedVolume, emptyPricePeriodSummary.PeriodVolume);
    }

    [TestMethod]
    public void FullyPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_PeriodSummariesEqualEachOther()
    {
        emptyPricePeriodSummary.CopyFrom(fullyPopulatedPricePeriodSummary);

        Assert.AreEqual(fullyPopulatedPricePeriodSummary, emptyPricePeriodSummary);
    }

    [TestMethod]
    public void PQPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_PeriodSummariesEquivalentToEachOther()
    {
        var pqPeriodSummary = new PQPricePeriodSummary(fullyPopulatedPricePeriodSummary);
        emptyPricePeriodSummary.CopyFrom(fullyPopulatedPricePeriodSummary);
        Assert.IsTrue(emptyPricePeriodSummary.AreEquivalent(pqPeriodSummary));
    }

    [TestMethod]
    public void PopulatedPeriodSummary_Clone_CreatesCopyOfEverythingExceptSrcTkrQtInfo()
    {
        var clone = fullyPopulatedPricePeriodSummary.Clone();

        Assert.AreEqual(fullyPopulatedPricePeriodSummary, clone);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)fullyPopulatedPricePeriodSummary).Clone();

        Assert.AreNotSame(clone, fullyPopulatedPricePeriodSummary);
        Assert.AreEqual(fullyPopulatedPricePeriodSummary, clone);
        clone = fullyPopulatedPricePeriodSummary.Clone();
        Assert.AreNotSame(clone, fullyPopulatedPricePeriodSummary);
        Assert.AreEqual(fullyPopulatedPricePeriodSummary, clone);
        clone = ((ICloneable<IPricePeriodSummary>)fullyPopulatedPricePeriodSummary).Clone();
        Assert.AreNotSame(clone, fullyPopulatedPricePeriodSummary);
        Assert.AreEqual(fullyPopulatedPricePeriodSummary, clone);
        clone = ((IMutablePricePeriodSummary)fullyPopulatedPricePeriodSummary).Clone();
        Assert.AreNotSame(clone, fullyPopulatedPricePeriodSummary);
        Assert.AreEqual(fullyPopulatedPricePeriodSummary, clone);
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        AssertOneDifferenceAtATime
            (false, fullyPopulatedPricePeriodSummary, fullyPopulatedPricePeriodSummary.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedPricePeriodSummary.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q = fullyPopulatedPricePeriodSummary;

        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.TimeSeriesPeriod)}: {q.TimeSeriesPeriod}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.PeriodStartTime)}: {q.PeriodStartTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.PeriodEndTime)}: {q.PeriodEndTime:O}"));
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

    internal static void AssertOneDifferenceAtATime
    (bool exactComparison, IMutablePricePeriodSummary commonPricePeriodSummary,
        IMutablePricePeriodSummary changingPricePeriodSummary)
    {
        changingPricePeriodSummary.TimeSeriesPeriod = TimeSeriesPeriod.FourHours;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.TimeSeriesPeriod = commonPricePeriodSummary.TimeSeriesPeriod;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.PeriodStartTime = DateTime.Now;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.PeriodStartTime = commonPricePeriodSummary.PeriodStartTime;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.PeriodEndTime = DateTime.Now;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.PeriodEndTime = commonPricePeriodSummary.PeriodEndTime;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.StartBidPrice = 6.78901m;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.StartBidPrice = commonPricePeriodSummary.StartBidPrice;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.StartAskPrice = 6.78901m;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.StartAskPrice = commonPricePeriodSummary.StartAskPrice;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.HighestBidPrice = 6.78901m;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.HighestBidPrice = commonPricePeriodSummary.HighestBidPrice;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.HighestAskPrice = 6.78901m;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.HighestAskPrice = commonPricePeriodSummary.HighestAskPrice;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.LowestBidPrice = 6.78901m;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.LowestBidPrice = commonPricePeriodSummary.LowestBidPrice;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.LowestAskPrice = 6.78901m;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.LowestAskPrice = commonPricePeriodSummary.LowestAskPrice;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.EndBidPrice = 6.78901m;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.EndBidPrice = commonPricePeriodSummary.EndBidPrice;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.EndAskPrice = 6.78901m;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.EndAskPrice = commonPricePeriodSummary.EndAskPrice;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.TickCount = 23456u;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.TickCount = commonPricePeriodSummary.TickCount;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));

        changingPricePeriodSummary.PeriodVolume = 98765L;
        Assert.IsFalse(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
        changingPricePeriodSummary.PeriodVolume = commonPricePeriodSummary.PeriodVolume;
        Assert.IsTrue(commonPricePeriodSummary.AreEquivalent(changingPricePeriodSummary));
    }


    public static PricePeriodSummary CreatePricePeriodSummary
        (TimeSeriesPeriod period, DateTime startTime, decimal mid, decimal openCloseSpread = 0.02m, decimal highLowSpread = 0.4m)
    {
        var halfSpread        = openCloseSpread / 2;
        var halfHighLowSpread = highLowSpread / 2;

        var bid1 = mid - halfSpread;
        var ask1 = mid + halfSpread;
        var bid2 = mid + halfHighLowSpread - halfSpread;
        var ask2 = mid + halfHighLowSpread + halfSpread;
        var bid3 = mid - halfHighLowSpread - halfSpread;
        var ask3 = mid - halfHighLowSpread + halfSpread;

        return new PricePeriodSummary
            (period, startTime, period.PeriodEnd(startTime), bid1, ask1, bid2, ask2, bid3
           , ask3, bid1, ask1, 10, 0, bid1, ask1);
    }
}
