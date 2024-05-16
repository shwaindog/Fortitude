#region

using FortitudeIO.TimeSeries;
using FortitudeMarketsCore.Pricing.TimeSeries;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.TimeSeries;

[TestClass]
public class PeriodSummaryHelpersTests
{
    [TestMethod]
    public void UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly()
    {
        var t = new DateTime(2018, 2, 7, 22, 35, 10);
        var calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddSeconds(1));
        Assert.AreEqual(TimeSeriesPeriod.OneSecond, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(1));
        Assert.AreEqual(TimeSeriesPeriod.OneMinute, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(5));
        Assert.AreEqual(TimeSeriesPeriod.FiveMinutes, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(10));
        Assert.AreEqual(TimeSeriesPeriod.TenMinutes, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(15));
        Assert.AreEqual(TimeSeriesPeriod.FifteenMinutes, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMinutes(30));
        Assert.AreEqual(TimeSeriesPeriod.ThirtyMinutes, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddHours(1));
        Assert.AreEqual(TimeSeriesPeriod.OneHour, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddHours(4));
        Assert.AreEqual(TimeSeriesPeriod.FourHours, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddDays(1));
        Assert.AreEqual(TimeSeriesPeriod.OneDay, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddDays(7));
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddDays(28));
        Assert.AreEqual(TimeSeriesPeriod.OneMonth, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddDays(30));
        Assert.AreEqual(TimeSeriesPeriod.OneMonth, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddSeconds(2));
        Assert.AreEqual(TimeSeriesPeriod.ConsumerConflated, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new QuotePeriodSummary(TimeSeriesPeriod.None, t, t.AddMilliseconds(10));
        Assert.AreEqual(TimeSeriesPeriod.ConsumerConflated, calculatePeriodSummary.CalcTimeFrame());
    }
}
