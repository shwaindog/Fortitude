#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Conflation;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.PQ.Conflation;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation;

[TestClass]
public class PQPeriodSummaryTests
{
    private PQPeriodSummary emptySummary = null!;
    private PQPeriodSummary fullyPopulatedPeriodSummary = null!;
    private PQSourceTickerQuoteInfo pricePrecisionSettings = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        pricePrecisionSettings = new PQSourceTickerQuoteInfo(new SourceTickerQuoteInfo(
            ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", QuoteLevel.Level3, 20,
            0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount,
            LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime)
        );

        emptySummary = new PQPeriodSummary();
        fullyPopulatedPeriodSummary = new PQPeriodSummary();
        quoteSequencedTestDataBuilder.InitalizePeriodSummary(fullyPopulatedPeriodSummary, 1);

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void EmptySummary_StartTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsStartTimeDateUpdated);
        Assert.IsFalse(emptySummary.IsStartTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptySummary.StartTime);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptySummary.StartTime = expectedSetTime;
        Assert.IsTrue(emptySummary.IsStartTimeDateUpdated);
        Assert.IsTrue(emptySummary.IsStartTimeSubHourUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptySummary.StartTime);
        var sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var fifthByte = expectedSetTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
        var expectedHour = new PQFieldUpdate(PQFieldKeys.PeriodStartDateTime, hoursSinceUnixEpoch);
        var expectedSubHour = new PQFieldUpdate(PQFieldKeys.PeriodStartSubHourTime, lowerFourBytes, fifthByte);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        emptySummary.IsStartTimeDateUpdated = false;
        Assert.IsFalse(emptySummary.IsStartTimeDateUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceAskUpdates.Count);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[0]);

        emptySummary.IsStartTimeSubHourUpdated = false;
        Assert.IsFalse(emptySummary.IsStartTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceAskUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id >= PQFieldKeys.PeriodStartDateTime && update.Id <=
                PQFieldKeys.PeriodStartSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.StartTime);
        Assert.IsTrue(newEmpty.IsStartTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsStartTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsEndTimeDateUpdated);
        Assert.IsFalse(emptySummary.IsEndTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptySummary.EndTime);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptySummary.EndTime = expectedSetTime;
        Assert.IsTrue(emptySummary.IsEndTimeDateUpdated);
        Assert.IsTrue(emptySummary.IsEndTimeSubHourUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptySummary.EndTime);
        var sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var fifthByte = expectedSetTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
        var expectedHour = new PQFieldUpdate(PQFieldKeys.PeriodEndDateTime, hoursSinceUnixEpoch);
        var expectedSubHour = new PQFieldUpdate(PQFieldKeys.PeriodEndSubHourTime, lowerFourBytes, fifthByte);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        emptySummary.IsEndTimeDateUpdated = false;
        Assert.IsFalse(emptySummary.IsEndTimeDateUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceAskUpdates.Count);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[0]);

        emptySummary.IsEndTimeSubHourUpdated = false;
        Assert.IsFalse(emptySummary.IsEndTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceAskUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id >= PQFieldKeys.PeriodEndDateTime && update.Id <=
                PQFieldKeys.PeriodEndSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.EndTime);
        Assert.IsTrue(newEmpty.IsEndTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsEndTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptySummary_StartBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsStartBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.StartBidPrice);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedStartBidPrice = 1.2345678m;
        emptySummary.StartBidPrice = expectedStartBidPrice;
        Assert.IsTrue(emptySummary.IsStartBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedStartBidPrice, emptySummary.StartBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, expectedStartBidPrice, 1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsStartBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsStartBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodStartPrice
                  && update.Flag == 1
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedStartBidPrice, newEmpty.StartBidPrice);
        Assert.IsTrue(newEmpty.IsStartBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_StartAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsStartAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.StartAskPrice);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedStartAskPrice = 1.2345678m;
        emptySummary.StartAskPrice = expectedStartAskPrice;
        Assert.IsTrue(emptySummary.IsStartAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedStartAskPrice, emptySummary.StartAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, expectedStartAskPrice,
            1 | PQFieldFlags.IsAskSideFlag);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsStartAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsStartAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodStartPrice
                  && update.Flag == (1 | PQFieldFlags.IsAskSideFlag)
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedStartAskPrice, newEmpty.StartAskPrice);
        Assert.IsTrue(newEmpty.IsStartAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_HighestBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsHighestBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.HighestBidPrice);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedHighestBidPrice = 1.2345678m;
        emptySummary.HighestBidPrice = expectedHighestBidPrice;
        Assert.IsTrue(emptySummary.IsHighestBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedHighestBidPrice, emptySummary.HighestBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, expectedHighestBidPrice, 1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsHighestBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsHighestBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodHighestPrice
                  && update.Flag == 1
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedHighestBidPrice, newEmpty.HighestBidPrice);
        Assert.IsTrue(newEmpty.IsHighestBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_HighestAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsHighestAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.HighestAskPrice);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedHighestAskPrice = 1.2345678m;
        emptySummary.HighestAskPrice = expectedHighestAskPrice;
        Assert.IsTrue(emptySummary.IsHighestAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedHighestAskPrice, emptySummary.HighestAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, expectedHighestAskPrice,
            1 | PQFieldFlags.IsAskSideFlag);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsHighestAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsHighestAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodHighestPrice
                  && update.Flag == (1 | PQFieldFlags.IsAskSideFlag)
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedHighestAskPrice, newEmpty.HighestAskPrice);
        Assert.IsTrue(newEmpty.IsHighestAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_LowestBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsLowestBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.LowestBidPrice);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedLowestBidPrice = 1.2345678m;
        emptySummary.LowestBidPrice = expectedLowestBidPrice;
        Assert.IsTrue(emptySummary.IsLowestBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedLowestBidPrice, emptySummary.LowestBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, expectedLowestBidPrice, 1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsLowestBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsLowestBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodLowestPrice
                  && update.Flag == 1
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLowestBidPrice, newEmpty.LowestBidPrice);
        Assert.IsTrue(newEmpty.IsLowestBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_LowestAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsLowestAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.LowestAskPrice);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedLowestAskPrice = 1.2345678m;
        emptySummary.LowestAskPrice = expectedLowestAskPrice;
        Assert.IsTrue(emptySummary.IsLowestAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedLowestAskPrice, emptySummary.LowestAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, expectedLowestAskPrice,
            1 | PQFieldFlags.IsAskSideFlag);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsLowestAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsLowestAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodLowestPrice
                  && update.Flag == (1 | PQFieldFlags.IsAskSideFlag)
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLowestAskPrice, newEmpty.LowestAskPrice);
        Assert.IsTrue(newEmpty.IsLowestAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsEndBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.EndBidPrice);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedEndBidPrice = 1.2345678m;
        emptySummary.EndBidPrice = expectedEndBidPrice;
        Assert.IsTrue(emptySummary.IsEndBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedEndBidPrice, emptySummary.EndBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, expectedEndBidPrice, 1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsEndBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsEndBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodEndPrice
                  && update.Flag == 1
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedEndBidPrice, newEmpty.EndBidPrice);
        Assert.IsTrue(newEmpty.IsEndBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_TickCountChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsTickCountUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.TickCount);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        var expectedTickCount = uint.MaxValue;
        emptySummary.TickCount = expectedTickCount;
        Assert.IsTrue(emptySummary.IsTickCountUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedTickCount, emptySummary.TickCount);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodTickCount, expectedTickCount);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsTickCountUpdated = false;
        Assert.IsFalse(emptySummary.IsTickCountUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodTickCount
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedTickCount, newEmpty.TickCount);
        Assert.IsTrue(newEmpty.IsTickCountUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsEndAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.EndAskPrice);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedEndAskPrice = 1.2345678m;
        emptySummary.EndAskPrice = expectedEndAskPrice;
        Assert.IsTrue(emptySummary.IsEndAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedEndAskPrice, emptySummary.EndAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, expectedEndAskPrice,
            1 | PQFieldFlags.IsAskSideFlag);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsEndAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsEndAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodEndPrice
                  && update.Flag == (1 | PQFieldFlags.IsAskSideFlag)
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedEndAskPrice, newEmpty.EndAskPrice);
        Assert.IsTrue(newEmpty.IsEndAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_PeriodVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsPeriodVolumeLowerBytesUpdated);
        Assert.IsFalse(emptySummary.IsPeriodVolumeUpperBytesUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0L, emptySummary.PeriodVolume);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).Count());

        var expectedPeriodVolume = long.MaxValue;
        emptySummary.PeriodVolume = expectedPeriodVolume;
        Assert.IsTrue(emptySummary.IsPeriodVolumeLowerBytesUpdated);
        Assert.IsTrue(emptySummary.IsPeriodVolumeUpperBytesUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedPeriodVolume, emptySummary.PeriodVolume);
        var periodVolumeUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(2, periodVolumeUpdates.Count);
        var lowerBytes = (uint)emptySummary.PeriodVolume;
        var upperBytes = (uint)(emptySummary.PeriodVolume >> 32);
        var expectedLowerBytes = new PQFieldUpdate(PQFieldKeys.PeriodVolumeLowerBytes, lowerBytes);
        var expectedUpperBytes = new PQFieldUpdate(PQFieldKeys.PeriodVolumeUpperBytes, upperBytes);
        Assert.AreEqual(expectedLowerBytes, periodVolumeUpdates[0]);
        Assert.AreEqual(expectedUpperBytes, periodVolumeUpdates[1]);

        emptySummary.IsPeriodVolumeLowerBytesUpdated = false;
        Assert.IsFalse(emptySummary.IsPeriodVolumeLowerBytesUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        periodVolumeUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).ToList();
        Assert.AreEqual(1, periodVolumeUpdates.Count);
        Assert.AreEqual(expectedUpperBytes, periodVolumeUpdates[0]);

        emptySummary.IsPeriodVolumeUpperBytesUpdated = false;
        Assert.IsFalse(emptySummary.IsPeriodVolumeUpperBytesUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
            pricePrecisionSettings).IsNullOrEmpty());

        periodVolumeUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                pricePrecisionSettings)
            where update.Id >= PQFieldKeys.PeriodVolumeLowerBytes && update.Id <=
                PQFieldKeys.PeriodVolumeUpperBytes
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, periodVolumeUpdates.Count);
        Assert.AreEqual(expectedLowerBytes, periodVolumeUpdates[0]);
        Assert.AreEqual(expectedUpperBytes, periodVolumeUpdates[1]);

        var newEmpty = new PQPeriodSummary();
        newEmpty.UpdateField(periodVolumeUpdates[0]);
        newEmpty.UpdateField(periodVolumeUpdates[1]);
        Assert.AreEqual(expectedPeriodVolume, newEmpty.PeriodVolume);
        Assert.IsTrue(newEmpty.IsPeriodVolumeLowerBytesUpdated);
        Assert.IsTrue(newEmpty.IsPeriodVolumeUpperBytesUpdated);
    }

    [TestMethod]
    public void EmptySummary_DifferingStartEndTimesCalcTimeFrame_ReturnsExpectedTimeFrame()
    {
        Assert.AreEqual(TimeFrame.Unknown, emptySummary.TimeFrame);
        var wellKnownStartTime = new DateTime(2017, 11, 19, 19, 00, 00);
        emptySummary.StartTime = wellKnownStartTime;
        Assert.AreEqual(TimeFrame.Unknown, emptySummary.TimeFrame);
        emptySummary.EndTime = wellKnownStartTime;
        Assert.AreEqual(TimeFrame.Tick, emptySummary.TimeFrame);
        var endTime = wellKnownStartTime.AddSeconds(1);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.OneSecond, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddMinutes(1);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.OneMinute, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddMinutes(5);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.FiveMinutes, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddMinutes(10);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.TenMinutes, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddMinutes(15);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.FifteenMinutes, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddMinutes(30);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.ThirtyMinutes, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddHours(1);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.OneHour, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddHours(4);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.FourHours, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddDays(1);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.OneDay, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddDays(5.00001);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.OneWeek, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddDays(6.999999);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.OneWeek, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddDays(28);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.OneMonth, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddDays(31);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.OneMonth, emptySummary.TimeFrame);
        endTime = wellKnownStartTime.AddMilliseconds(512);
        emptySummary.TimeFrame = TimeFrame.Unknown;
        emptySummary.EndTime = endTime;
        Assert.AreEqual(TimeFrame.Conflation, emptySummary.TimeFrame);
    }

    [TestMethod]
    public void PopulatedPeriodSummaryWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel0Fields()
    {
        var pqFieldUpdates = fullyPopulatedPeriodSummary.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update, pricePrecisionSettings).ToList();
        AssertPeriodSummaryContainsAllFields(pqFieldUpdates, fullyPopulatedPeriodSummary);
    }

    [TestMethod]
    public void PopulatedPeriodSummaryWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel0Fields()
    {
        fullyPopulatedPeriodSummary.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPeriodSummary.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Snapshot, pricePrecisionSettings).ToList();
        AssertPeriodSummaryContainsAllFields(pqFieldUpdates, fullyPopulatedPeriodSummary);
    }

    [TestMethod]
    public void PopulatedPeriodSummaryWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedPeriodSummary.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPeriodSummary.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPeriodSummary_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        var pqFieldUpdates = fullyPopulatedPeriodSummary.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update | PQMessageFlags.Replay
                , pricePrecisionSettings)
            .ToList();
        var newEmpty = new PQPeriodSummary();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(fullyPopulatedPeriodSummary, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_QuotesEqualEachOther()
    {
        emptySummary = new PQPeriodSummary();
        emptySummary.CopyFrom(fullyPopulatedPeriodSummary);

        Assert.AreEqual(fullyPopulatedPeriodSummary, emptySummary);
    }

    [TestMethod]
    public void FullyPopulatedPeriodSummary_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptySummary = new PQPeriodSummary();
        fullyPopulatedPeriodSummary.HasUpdates = false;
        emptySummary.CopyFrom(fullyPopulatedPeriodSummary);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptySummary.StartTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptySummary.EndTime);
        Assert.AreEqual(0m, emptySummary.StartBidPrice);
        Assert.AreEqual(0m, emptySummary.StartAskPrice);
        Assert.AreEqual(0m, emptySummary.HighestBidPrice);
        Assert.AreEqual(0m, emptySummary.HighestAskPrice);
        Assert.AreEqual(0m, emptySummary.LowestBidPrice);
        Assert.AreEqual(0m, emptySummary.LowestAskPrice);
        Assert.AreEqual(0m, emptySummary.EndBidPrice);
        Assert.AreEqual(0m, emptySummary.EndAskPrice);
        Assert.AreEqual(0u, emptySummary.TickCount);
        Assert.AreEqual(0L, emptySummary.PeriodVolume);
        Assert.IsFalse(emptySummary.IsStartTimeDateUpdated);
        Assert.IsFalse(emptySummary.IsStartTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.IsStartBidPriceUpdated);
        Assert.IsFalse(emptySummary.IsStartAskPriceUpdated);
        Assert.IsFalse(emptySummary.IsHighestBidPriceUpdated);
        Assert.IsFalse(emptySummary.IsHighestAskPriceUpdated);
        Assert.IsFalse(emptySummary.IsLowestBidPriceUpdated);
        Assert.IsFalse(emptySummary.IsLowestAskPriceUpdated);
        Assert.IsFalse(emptySummary.IsEndBidPriceUpdated);
        Assert.IsFalse(emptySummary.IsEndAskPriceUpdated);
        Assert.IsFalse(emptySummary.IsTickCountUpdated);
        Assert.IsFalse(emptySummary.IsPeriodVolumeLowerBytesUpdated);
        Assert.IsFalse(emptySummary.IsPeriodVolumeUpperBytesUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedPeriodSummary_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var nonPQPeriodSummary = new PeriodSummary(fullyPopulatedPeriodSummary);
        emptySummary.CopyFrom(nonPQPeriodSummary);
        Assert.IsTrue(fullyPopulatedPeriodSummary.AreEquivalent(emptySummary));
    }

    [TestMethod]
    public void FullyInitializedQuote_Clone_CopiesQuoteExactly()
    {
        var clonedQuote = ((ICloneable<IPeriodSummary>)fullyPopulatedPeriodSummary).Clone();
        Assert.AreEqual(fullyPopulatedPeriodSummary, clonedQuote);
        var clonedQuote2 = ((ICloneable)fullyPopulatedPeriodSummary).Clone();
        Assert.AreEqual(fullyPopulatedPeriodSummary, clonedQuote2);
        var clonedQuote3 = ((IPQPeriodSummary)fullyPopulatedPeriodSummary).Clone();
        Assert.AreEqual(fullyPopulatedPeriodSummary, clonedQuote3);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQPeriodSummary)((ICloneable<IPeriodSummary>)fullyPopulatedPeriodSummary).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedPeriodSummary, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedPeriodSummary, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedPeriodSummary, fullyPopulatedPeriodSummary);
        Assert.AreEqual(fullyPopulatedPeriodSummary, ((ICloneable)fullyPopulatedPeriodSummary).Clone());
        Assert.AreEqual(fullyPopulatedPeriodSummary, ((ICloneable<IPeriodSummary>)fullyPopulatedPeriodSummary).Clone());
        Assert.AreEqual(fullyPopulatedPeriodSummary, ((IPQPeriodSummary)fullyPopulatedPeriodSummary).Clone());
    }

    [TestMethod]
    public void EmptyQuote_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = emptySummary.GetHashCode();
        Assert.IsTrue(hashCode != 0);
        hashCode = fullyPopulatedPeriodSummary.GetHashCode();
        Assert.IsTrue(hashCode != 0);
        Assert.IsTrue(emptySummary.GetHashCode() != hashCode);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison
        , PQPeriodSummary original,
        PQPeriodSummary changingPeriodSummary)
    {
        Assert.IsTrue(original.AreEquivalent(changingPeriodSummary));
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original));

        Assert.IsFalse(changingPeriodSummary.AreEquivalent(null, exactComparison));
        Assert.AreEqual(!exactComparison,
            changingPeriodSummary.AreEquivalent(new PeriodSummary(original), exactComparison));

        changingPeriodSummary.StartTime = new DateTime(2017, 11, 19, 21, 27, 32);
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.StartTime = original.StartTime;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.EndTime = new DateTime(2017, 11, 19, 21, 27, 32);
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.EndTime = original.EndTime;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.StartBidPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.StartBidPrice = original.StartBidPrice;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.StartAskPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.StartAskPrice = original.StartAskPrice;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.HighestBidPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.HighestBidPrice = original.HighestBidPrice;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.HighestAskPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.HighestAskPrice = original.HighestAskPrice;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.LowestBidPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.LowestBidPrice = original.LowestBidPrice;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.LowestAskPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.LowestAskPrice = original.LowestAskPrice;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.EndBidPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.EndBidPrice = original.EndBidPrice;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.EndAskPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.EndAskPrice = original.EndAskPrice;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.TickCount = uint.MaxValue;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.TickCount = original.TickCount;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.PeriodVolume = long.MaxValue;
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.PeriodVolume = original.PeriodVolume;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        NonPublicInvocator.SetInstanceField(changingPeriodSummary, "updatedFlags", PeriodSummaryUpdatedFlags.None);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPeriodSummary, exactComparison));
        var originalUpdatedFlags =
            NonPublicInvocator.GetInstanceField<PeriodSummaryUpdatedFlags>(original, "updatedFlags");
        NonPublicInvocator.SetInstanceField(changingPeriodSummary, "updatedFlags", originalUpdatedFlags);
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));
    }

    public static void AssertPeriodSummaryContainsAllFields(IList<PQFieldUpdate> checkFieldUpdates,
        IPQPeriodSummary periodSummary)
    {
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodStartDateTime, periodSummary.StartTime
                .GetHoursFromUnixEpoch()),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodStartDateTime));
        var fifthByte = periodSummary.StartTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodStartSubHourTime, lowerFourBytes, fifthByte),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodStartSubHourTime));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodEndDateTime, periodSummary.EndTime
                .GetHoursFromUnixEpoch()),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodEndDateTime));
        fifthByte = periodSummary.EndTime.GetSubHourComponent().BreakLongToByteAndUint(out lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodEndSubHourTime, lowerFourBytes, fifthByte),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodEndSubHourTime));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, periodSummary.StartBidPrice, 1),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodStartPrice, 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, periodSummary.StartAskPrice,
                PQFieldFlags.IsAskSideFlag | 1),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodStartPrice,
                PQFieldFlags.IsAskSideFlag | 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, periodSummary.HighestBidPrice, 1),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodHighestPrice, 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, periodSummary.HighestAskPrice,
                PQFieldFlags.IsAskSideFlag | 1),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodHighestPrice,
                PQFieldFlags.IsAskSideFlag | 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, periodSummary.LowestBidPrice, 1),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodLowestPrice, 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, periodSummary.LowestAskPrice,
                PQFieldFlags.IsAskSideFlag | 1),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodLowestPrice,
                PQFieldFlags.IsAskSideFlag | 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, periodSummary.EndBidPrice, 1),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodEndPrice, 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, periodSummary.EndAskPrice,
                PQFieldFlags.IsAskSideFlag | 1),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodEndPrice,
                PQFieldFlags.IsAskSideFlag | 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodTickCount, periodSummary.TickCount),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodTickCount));
        var lowerBytesPeriodVolume = (uint)periodSummary.PeriodVolume;
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodVolumeLowerBytes, lowerBytesPeriodVolume),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodVolumeLowerBytes));
        var upperBytesPeriodVolume = (uint)(periodSummary.PeriodVolume >> 32);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodVolumeUpperBytes, upperBytesPeriodVolume),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodVolumeUpperBytes));
    }
}
