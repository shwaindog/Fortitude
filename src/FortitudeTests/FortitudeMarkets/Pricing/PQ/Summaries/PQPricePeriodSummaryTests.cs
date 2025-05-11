// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.PQ.Summaries;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Summaries;

[TestClass]
public class PQPricePeriodSummaryTests
{
    private PQPricePeriodSummary emptySummary                = null!;
    private PQPricePeriodSummary fullyPopulatedPeriodSummary = null!;
    private PQSourceTickerInfo   pricePrecisionSettings      = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        pricePrecisionSettings =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
                   , 20, 0.000001m, 0.00001m, 10_000_000m, 50_000_000_000m, 10_000_000m, 1
                   , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
                   , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                      LastTradedFlags.LastTradedTime));

        emptySummary                = new PQPricePeriodSummary();
        fullyPopulatedPeriodSummary = new PQPricePeriodSummary();
        quoteSequencedTestDataBuilder.InitalizePeriodSummary(fullyPopulatedPeriodSummary, 1);

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void EmptySummary_StartTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsStartTimeDateUpdated);
        Assert.IsFalse(emptySummary.IsStartTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(DateTime.MinValue, emptySummary.PeriodStartTime);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                             pricePrecisionSettings).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptySummary.PeriodStartTime = expectedSetTime;
        Assert.IsTrue(emptySummary.IsStartTimeDateUpdated);
        Assert.IsTrue(emptySummary.IsStartTimeSubHourUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptySummary.PeriodStartTime);
        var sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                                 pricePrecisionSettings).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var fifthByte           = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
        var expectedHour        = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartSub2MinTime, lowerFourBytes, fifthByte);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        emptySummary.IsStartTimeDateUpdated = false;
        Assert.IsFalse(emptySummary.IsStartTimeDateUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                             pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceAskUpdates.Count);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[0]);

        emptySummary.IsStartTimeSubHourUpdated = false;
        Assert.IsFalse(emptySummary.IsStartTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceAskUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot,
                                                                             pricePrecisionSettings)
            where update.PricingSubId is >= PQPricingSubFieldKeys.CandleStartDateTime and <= PQPricingSubFieldKeys.CandleStartSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.PeriodStartTime);
        Assert.IsTrue(newEmpty.IsStartTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsStartTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsEndTimeDateUpdated);
        Assert.IsFalse(emptySummary.IsEndTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(DateTime.MinValue, emptySummary.PeriodEndTime);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                             pricePrecisionSettings).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptySummary.PeriodEndTime = expectedSetTime;
        Assert.IsTrue(emptySummary.IsEndTimeDateUpdated);
        Assert.IsTrue(emptySummary.IsEndTimeSubHourUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptySummary.PeriodEndTime);
        var sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                                 pricePrecisionSettings).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var fifthByte           = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
        var expectedHour        = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndSub2MinTime, lowerFourBytes, fifthByte);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        emptySummary.IsEndTimeDateUpdated = false;
        Assert.IsFalse(emptySummary.IsEndTimeDateUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                             pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceAskUpdates.Count);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[0]);

        emptySummary.IsEndTimeSubHourUpdated = false;
        Assert.IsFalse(emptySummary.IsEndTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceAskUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot,
                                                                             pricePrecisionSettings)
            where update.PricingSubId is >= PQPricingSubFieldKeys.CandleEndDateTime and <=
                PQPricingSubFieldKeys.CandleEndSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.PeriodEndTime);
        Assert.IsTrue(newEmpty.IsEndTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsEndTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptySummary_StartBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsStartBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.StartBidPrice);
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedStartBidPrice = 1.23456m;
        var scaleFactor           = pricePrecisionSettings.PriceScalingPrecision;
        emptySummary.StartBidPrice = expectedStartBidPrice;
        Assert.IsTrue(emptySummary.IsStartBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedStartBidPrice, emptySummary.StartBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartPrice, expectedStartBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsStartBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsStartBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update is {PricingSubId : PQPricingSubFieldKeys.CandleStartPrice, DepthId: PQDepthKey.None }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedStartAskPrice = 1.23456m;
        var scaleFactor           = pricePrecisionSettings.PriceScalingPrecision;
        emptySummary.StartAskPrice = expectedStartAskPrice;
        Assert.IsTrue(emptySummary.IsStartAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedStartAskPrice, emptySummary.StartAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate
            (PQQuoteFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleStartPrice, expectedStartAskPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsStartAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsStartAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update is { PricingSubId: PQPricingSubFieldKeys.CandleStartPrice, DepthId: PQDepthKey.AskSide }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedHighestBidPrice = 1.23456m;
        var scaleFactor             = pricePrecisionSettings.PriceScalingPrecision;
        emptySummary.HighestBidPrice = expectedHighestBidPrice;
        Assert.IsTrue(emptySummary.IsHighestBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedHighestBidPrice, emptySummary.HighestBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleHighestPrice, expectedHighestBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsHighestBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsHighestBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update is {PricingSubId: PQPricingSubFieldKeys.CandleHighestPrice, DepthId: PQDepthKey.None}
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedHighestAskPrice = 1.23456m;
        var scaleFactor             = pricePrecisionSettings.PriceScalingPrecision;
        emptySummary.HighestAskPrice = expectedHighestAskPrice;
        Assert.IsTrue(emptySummary.IsHighestAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedHighestAskPrice, emptySummary.HighestAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate
            (PQQuoteFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleHighestPrice, expectedHighestAskPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsHighestAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsHighestAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot,
                                                                          pricePrecisionSettings)
            where update is { PricingSubId: PQPricingSubFieldKeys.CandleHighestPrice, DepthId: PQDepthKey.AskSide }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedLowestBidPrice = 1.23456m;
        var scaleFactor            = pricePrecisionSettings.PriceScalingPrecision;
        emptySummary.LowestBidPrice = expectedLowestBidPrice;
        Assert.IsTrue(emptySummary.IsLowestBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedLowestBidPrice, emptySummary.LowestBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleLowestPrice, expectedLowestBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsLowestBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsLowestBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update is {PricingSubId : PQPricingSubFieldKeys.CandleLowestPrice, DepthId: PQDepthKey.None }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedLowestAskPrice = 1.23456m;
        var scaleFactor            = pricePrecisionSettings.PriceScalingPrecision;
        emptySummary.LowestAskPrice = expectedLowestAskPrice;
        Assert.IsTrue(emptySummary.IsLowestAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedLowestAskPrice, emptySummary.LowestAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleLowestPrice, expectedLowestAskPrice,
                                                    scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsLowestAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsLowestAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update is { PricingSubId: PQPricingSubFieldKeys.CandleLowestPrice, DepthId: PQDepthKey.AskSide }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields
                            (testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedEndBidPrice = 1.23456m;
        var scaleFactor         = pricePrecisionSettings.PriceScalingPrecision;
        emptySummary.EndBidPrice = expectedEndBidPrice;
        Assert.IsTrue(emptySummary.IsEndBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedEndBidPrice, emptySummary.EndBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndPrice, expectedEndBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsEndBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsEndBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update is {PricingSubId: PQPricingSubFieldKeys.CandleEndPrice, DepthId: PQDepthKey.None}
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedEndBidPrice, newEmpty.EndBidPrice);
        Assert.IsTrue(newEmpty.IsEndBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsEndAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.EndAskPrice);
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedEndAskPrice = 1.23456m;
        
        var scaleFactor         = pricePrecisionSettings.PriceScalingPrecision;
        emptySummary.EndAskPrice = expectedEndAskPrice;
        Assert.IsTrue(emptySummary.IsEndAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedEndAskPrice, emptySummary.EndAskPrice);
        var sourceUpdates =
            emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate =
            new PQFieldUpdate
                (PQQuoteFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleEndPrice, expectedEndAskPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsEndAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsEndAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        var deltaUpdateFields = emptySummary
            .GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, pricePrecisionSettings).ToList();
        sourceUpdates = (from update in deltaUpdateFields
            where update is { PricingSubId: PQPricingSubFieldKeys.CandleEndPrice, DepthId: PQDepthKey.AskSide }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedEndAskPrice, newEmpty.EndAskPrice);
        Assert.IsTrue(newEmpty.IsEndAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_TickCountChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsTickCountUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0m, emptySummary.TickCount);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        var expectedTickCount = uint.MaxValue;
        emptySummary.TickCount = expectedTickCount;
        Assert.IsTrue(emptySummary.IsTickCountUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedTickCount, emptySummary.TickCount);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleTickCount, expectedTickCount);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsTickCountUpdated = false;
        Assert.IsFalse(emptySummary.IsTickCountUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.PricingSubId == PQPricingSubFieldKeys.CandleTickCount
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedTickCount, newEmpty.TickCount);
        Assert.IsTrue(newEmpty.IsTickCountUpdated);
    }

    [TestMethod]
    public void EmptySummary_PeriodVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsPeriodVolumeUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0L, emptySummary.PeriodVolume);
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedPeriodVolume = 500_000_000_000;
        var scaleFactor          = pricePrecisionSettings.VolumeScalingPrecision;
        emptySummary.PeriodVolume = expectedPeriodVolume;
        Assert.IsTrue(emptySummary.IsPeriodVolumeUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedPeriodVolume, emptySummary.PeriodVolume);
        var periodVolumeUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, periodVolumeUpdates.Count);
        var expectedFieldUpdate =
            new PQFieldUpdate
                (PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleVolume, expectedPeriodVolume, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, periodVolumeUpdates[0]);

        emptySummary.IsPeriodVolumeUpdated = false;
        Assert.IsFalse(emptySummary.IsPeriodVolumeUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        periodVolumeUpdates = (from update in emptySummary
                .GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.PricingSubId == PQPricingSubFieldKeys.CandleVolume
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(1, periodVolumeUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, periodVolumeUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(periodVolumeUpdates[0]);
        Assert.AreEqual(expectedPeriodVolume, newEmpty.PeriodVolume);
        Assert.IsTrue(newEmpty.IsPeriodVolumeUpdated);
    }

    [TestMethod]
    public void EmptySummary_SummaryFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsPricePeriodSummaryFlagsUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(0L, emptySummary.PeriodVolume);
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedFlagsVolume = PricePeriodSummaryFlags.MissingTicksRange03;
        emptySummary.PeriodSummaryFlags = expectedFlagsVolume;
        Assert.IsTrue(emptySummary.IsPricePeriodSummaryFlagsUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedFlagsVolume, emptySummary.PeriodSummaryFlags);
        var periodFlagUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, periodFlagUpdates.Count);
        var expectedFieldUpdate =
            new PQFieldUpdate
                (PQQuoteFields.PriceCandleStick, PQPricingSubFieldKeys.CandleSummaryFlags, (uint)expectedFlagsVolume);
        Assert.AreEqual(expectedFieldUpdate, periodFlagUpdates[0]);

        emptySummary.IsPricePeriodSummaryFlagsUpdated = false;
        Assert.IsFalse(emptySummary.IsPricePeriodSummaryFlagsUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        periodFlagUpdates = (from update in emptySummary
                .GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.PricingSubId == PQPricingSubFieldKeys.CandleSummaryFlags
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(1, periodFlagUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, periodFlagUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(periodFlagUpdates[0]);
        Assert.AreEqual(expectedFlagsVolume, newEmpty.PeriodSummaryFlags);
        Assert.IsTrue(newEmpty.IsPricePeriodSummaryFlagsUpdated);
    }

    [TestMethod]
    public void EmptySummary_DifferingSummaryPeriod_IsSavedAndReturned()
    {
        Assert.AreEqual(TimeBoundaryPeriod.Tick, emptySummary.TimeBoundaryPeriod);
        var wellKnownStartTime = new DateTime(2017, 11, 19, 19, 00, 00);
        emptySummary.PeriodStartTime    = wellKnownStartTime;
        emptySummary.TimeBoundaryPeriod = TimeBoundaryPeriod.Tick;
        Assert.AreEqual(TimeBoundaryPeriod.Tick, emptySummary.TimeBoundaryPeriod);
        emptySummary.TimeBoundaryPeriod = TimeBoundaryPeriod.OneDecade;
        Assert.AreEqual(TimeBoundaryPeriod.OneDecade, emptySummary.TimeBoundaryPeriod);
    }

    [TestMethod]
    public void PopulatedPeriodSummaryWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllTickInstantFields()
    {
        var pqFieldUpdates =
            fullyPopulatedPeriodSummary.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update, pricePrecisionSettings).ToList();
        AssertPeriodSummaryContainsAllFields(pricePrecisionSettings, pqFieldUpdates, fullyPopulatedPeriodSummary);
    }

    [TestMethod]
    public void PopulatedPeriodSummaryWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllTickInstantFields()
    {
        fullyPopulatedPeriodSummary.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedPeriodSummary.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Snapshot, pricePrecisionSettings).ToList();
        AssertPeriodSummaryContainsAllFields(pricePrecisionSettings, pqFieldUpdates, fullyPopulatedPeriodSummary);
    }

    [TestMethod]
    public void PopulatedPeriodSummaryWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedPeriodSummary.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedPeriodSummary.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPeriodSummary_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        var pqFieldUpdates =
            fullyPopulatedPeriodSummary
                .GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes, pricePrecisionSettings).ToList();
        var newEmpty = new PQPricePeriodSummary();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(fullyPopulatedPeriodSummary, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_QuotesEqualEachOther()
    {
        emptySummary = new PQPricePeriodSummary();
        emptySummary.CopyFrom(fullyPopulatedPeriodSummary);

        Assert.AreEqual(fullyPopulatedPeriodSummary, emptySummary);
    }

    [TestMethod]
    public void FullyPopulatedPeriodSummary_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptySummary                           = new PQPricePeriodSummary();
        fullyPopulatedPeriodSummary.HasUpdates = false;
        emptySummary.CopyFrom(fullyPopulatedPeriodSummary);
        Assert.AreEqual(DateTime.MinValue, emptySummary.PeriodStartTime);
        Assert.AreEqual(DateTime.MinValue, emptySummary.PeriodEndTime);
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
        Assert.IsFalse(emptySummary.IsPeriodVolumeUpdated);
        Assert.IsFalse(emptySummary.IsPricePeriodSummaryFlagsUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedPeriodSummary_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var nonPQPeriodSummary = new PricePeriodSummary(fullyPopulatedPeriodSummary);
        emptySummary.CopyFrom(nonPQPeriodSummary);
        Assert.IsTrue(fullyPopulatedPeriodSummary.AreEquivalent(emptySummary));
    }

    [TestMethod]
    public void FullyInitializedQuote_Clone_CopiesQuoteExactly()
    {
        var clonedQuote = ((ICloneable<IPricePeriodSummary>)fullyPopulatedPeriodSummary).Clone();
        Assert.AreEqual(fullyPopulatedPeriodSummary, clonedQuote);
        var clonedQuote2 = ((ICloneable)fullyPopulatedPeriodSummary).Clone();
        Assert.AreEqual(fullyPopulatedPeriodSummary, clonedQuote2);
        var clonedQuote3 = ((IPQPricePeriodSummary)fullyPopulatedPeriodSummary).Clone();
        Assert.AreEqual(fullyPopulatedPeriodSummary, clonedQuote3);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQPricePeriodSummary)((ICloneable<IPricePeriodSummary>)fullyPopulatedPeriodSummary).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedPeriodSummary, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedPeriodSummary, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedPeriodSummary, fullyPopulatedPeriodSummary);
        Assert.AreEqual(fullyPopulatedPeriodSummary, ((ICloneable)fullyPopulatedPeriodSummary).Clone());
        Assert.AreEqual(fullyPopulatedPeriodSummary, ((ICloneable<IPricePeriodSummary>)fullyPopulatedPeriodSummary).Clone());
        Assert.AreEqual(fullyPopulatedPeriodSummary, ((IPQPricePeriodSummary)fullyPopulatedPeriodSummary).Clone());
    }

    [TestMethod]
    public void EmptyQuote_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = emptySummary.GetHashCode();
        Assert.IsTrue(hashCode == 0);
        hashCode = fullyPopulatedPeriodSummary.GetHashCode();
        Assert.IsTrue(hashCode != 0);
        Assert.IsTrue(emptySummary.GetHashCode() != hashCode);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
        (bool exactComparison, PQPricePeriodSummary original, PQPricePeriodSummary changingPeriodSummary)
    {
        Assert.IsTrue(original.AreEquivalent(changingPeriodSummary));
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original));

        Assert.IsFalse(changingPeriodSummary.AreEquivalent(null, exactComparison));
        Assert.AreEqual(!exactComparison,
                        changingPeriodSummary.AreEquivalent(new PricePeriodSummary(original), exactComparison));

        changingPeriodSummary.PeriodStartTime = new DateTime(2017, 11, 19, 21, 27, 32);
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.PeriodStartTime = original.PeriodStartTime;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.PeriodEndTime = new DateTime(2017, 11, 19, 21, 27, 32);
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.PeriodEndTime = original.PeriodEndTime;
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

    public static void AssertPeriodSummaryContainsAllFields
        (IPQPriceVolumePublicationPrecisionSettings precisionSettings,  IList<PQFieldUpdate> checkFieldUpdates, IPQPricePeriodSummary periodSummary, PQQuoteFields quoteField = PQQuoteFields.PriceCandleStick)
    {
        var priceScale  = precisionSettings.PriceScalingPrecision;
        var volumeScale = precisionSettings.VolumeScalingPrecision;
        Assert.AreEqual(new PQFieldUpdate
                            (quoteField, PQPricingSubFieldKeys.CandleStartDateTime, periodSummary.PeriodStartTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleStartDateTime),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        var fifthByte = periodSummary.PeriodStartTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleStartSub2MinTime, lowerFourBytes, fifthByte),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleStartSub2MinTime),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate
                            (quoteField, PQPricingSubFieldKeys.CandleEndDateTime, periodSummary.PeriodEndTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleEndDateTime),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        fifthByte = periodSummary.PeriodEndTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleEndSub2MinTime, lowerFourBytes, fifthByte),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleEndSub2MinTime),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleStartPrice, periodSummary.StartBidPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleStartPrice, priceScale),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate (quoteField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleStartPrice, periodSummary.StartAskPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, quoteField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleStartPrice, priceScale),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleHighestPrice, periodSummary.HighestBidPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleHighestPrice, priceScale),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate
                            (quoteField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleHighestPrice, periodSummary.HighestAskPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, quoteField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleHighestPrice, priceScale),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleLowestPrice, periodSummary.LowestBidPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleLowestPrice, priceScale),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate
                            (quoteField,  PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleLowestPrice,periodSummary.LowestAskPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, quoteField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleLowestPrice, priceScale),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleEndPrice, periodSummary.EndBidPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleEndPrice, priceScale),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQDepthKey.AskSide,  PQPricingSubFieldKeys.CandleEndPrice, periodSummary.EndAskPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, quoteField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleEndPrice, priceScale),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleTickCount, periodSummary.TickCount),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleTickCount),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleVolume, periodSummary.PeriodVolume),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleVolume),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(quoteField, PQPricingSubFieldKeys.CandleSummaryFlags, (uint)periodSummary.PeriodSummaryFlags),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, quoteField, PQPricingSubFieldKeys.CandleSummaryFlags),
                        $"For PricePeriodSummary {periodSummary} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }
}
