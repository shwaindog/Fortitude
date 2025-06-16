// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;

[TestClass]
public class PQCandleTests
{
    private PQCandle emptyCandle                = null!;
    private PQCandle fullyPopulatedCandle = null!;
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
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, MarketClassification.Unknown
                    , AUinMEL, AUinMEL, AUinMEL
                   , 20, 0.000001m, 0.00001m, 10_000_000m, 50_000_000_000m, 10_000_000m, 1
                   , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
                   , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                      LastTradedFlags.LastTradedTime));

        emptyCandle                = new PQCandle();
        fullyPopulatedCandle = new PQCandle();
        quoteSequencedTestDataBuilder.InitalizeCandle(fullyPopulatedCandle, 1);

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void EmptySummary_StartTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsStartTimeDateUpdated);
        Assert.IsFalse(emptyCandle.IsStartTimeSub2MinUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(DateTime.MinValue, emptyCandle.PeriodStartTime);
        Assert.AreEqual(0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                             pricePrecisionSettings).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyCandle.PeriodStartTime = expectedSetTime;
        Assert.IsTrue(emptyCandle.IsStartTimeDateUpdated);
        Assert.IsTrue(emptyCandle.IsStartTimeSub2MinUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyCandle.PeriodStartTime);
        var sourceAskUpdates = emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                                 pricePrecisionSettings).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var fifthByte           = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
        var expectedHour        = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartDateTime, hoursSinceUnixEpoch);
        var expectedSub2Min     = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartSub2MinTime, lowerFourBytes, fifthByte);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceAskUpdates[1]);

        emptyCandle.IsStartTimeDateUpdated = false;
        Assert.IsFalse(emptyCandle.IsStartTimeDateUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        sourceAskUpdates = emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                             pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceAskUpdates.Count);
        Assert.AreEqual(expectedSub2Min, sourceAskUpdates[0]);

        emptyCandle.IsStartTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyCandle.IsStartTimeSub2MinUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrNone());

        sourceAskUpdates = (from update in emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                                                                             pricePrecisionSettings)
            where update.PricingSubId is >= PQPricingSubFieldKeys.CandleStartDateTime and <= PQPricingSubFieldKeys.CandleStartSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceAskUpdates[1]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.PeriodStartTime);
        Assert.IsTrue(newEmpty.IsStartTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsStartTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsEndTimeDateUpdated);
        Assert.IsFalse(emptyCandle.IsEndTimeSub2MinUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(DateTime.MinValue, emptyCandle.PeriodEndTime);
        Assert.AreEqual(0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                             pricePrecisionSettings).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyCandle.PeriodEndTime = expectedSetTime;
        Assert.IsTrue(emptyCandle.IsEndTimeDateUpdated);
        Assert.IsTrue(emptyCandle.IsEndTimeSub2MinUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyCandle.PeriodEndTime);
        var sourceAskUpdates = emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                                 pricePrecisionSettings).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var fifthByte           = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
        var expectedHour        = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndDateTime, hoursSinceUnixEpoch);
        var expectedSub2Min     = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndSub2MinTime, lowerFourBytes, fifthByte);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceAskUpdates[1]);

        emptyCandle.IsEndTimeDateUpdated = false;
        Assert.IsFalse(emptyCandle.IsEndTimeDateUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        sourceAskUpdates = emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                             pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceAskUpdates.Count);
        Assert.AreEqual(expectedSub2Min, sourceAskUpdates[0]);

        emptyCandle.IsEndTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyCandle.IsEndTimeSub2MinUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrNone());

        sourceAskUpdates = (from update in emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                                                                             pricePrecisionSettings)
            where update.PricingSubId is >= PQPricingSubFieldKeys.CandleEndDateTime and <=
                PQPricingSubFieldKeys.CandleEndSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceAskUpdates[1]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.PeriodEndTime);
        Assert.IsTrue(newEmpty.IsEndTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsEndTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptySummary_StartBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsStartBidPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.StartBidPrice);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedStartBidPrice = 1.23456m;
        var scaleFactor           = pricePrecisionSettings.PriceScalingPrecision;
        emptyCandle.StartBidPrice = expectedStartBidPrice;
        Assert.IsTrue(emptyCandle.IsStartBidPriceUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedStartBidPrice, emptyCandle.StartBidPrice);
        var sourceUpdates = emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartPrice, expectedStartBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsStartBidPriceUpdated = false;
        Assert.IsFalse(emptyCandle.IsStartBidPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue
            (emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        sourceUpdates = (from update in emptyCandle.GetDeltaUpdateFields
                (testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update is {PricingSubId : PQPricingSubFieldKeys.CandleStartPrice, DepthId: PQDepthKey.None }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedStartBidPrice, newEmpty.StartBidPrice);
        Assert.IsTrue(newEmpty.IsStartBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_StartAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsStartAskPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.StartAskPrice);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedStartAskPrice = 1.23456m;
        var scaleFactor           = pricePrecisionSettings.PriceScalingPrecision;
        emptyCandle.StartAskPrice = expectedStartAskPrice;
        Assert.IsTrue(emptyCandle.IsStartAskPriceUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedStartAskPrice, emptyCandle.StartAskPrice);
        var sourceUpdates = emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate
            (PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleStartPrice, expectedStartAskPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsStartAskPriceUpdated = false;
        Assert.IsFalse(emptyCandle.IsStartAskPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue
            (emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        sourceUpdates = (from update in emptyCandle.GetDeltaUpdateFields
                (testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update is { PricingSubId: PQPricingSubFieldKeys.CandleStartPrice, DepthId: PQDepthKey.AskSide }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedStartAskPrice, newEmpty.StartAskPrice);
        Assert.IsTrue(newEmpty.IsStartAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_HighestBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsHighestBidPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.HighestBidPrice);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedHighestBidPrice = 1.23456m;
        var scaleFactor             = pricePrecisionSettings.PriceScalingPrecision;
        emptyCandle.HighestBidPrice = expectedHighestBidPrice;
        Assert.IsTrue(emptyCandle.IsHighestBidPriceUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedHighestBidPrice, emptyCandle.HighestBidPrice);
        var sourceUpdates = emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleHighestPrice, expectedHighestBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsHighestBidPriceUpdated = false;
        Assert.IsFalse(emptyCandle.IsHighestBidPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields
                          (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        sourceUpdates = (from update in emptyCandle.GetDeltaUpdateFields
                (testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update is {PricingSubId: PQPricingSubFieldKeys.CandleHighestPrice, DepthId: PQDepthKey.None}
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedHighestBidPrice, newEmpty.HighestBidPrice);
        Assert.IsTrue(newEmpty.IsHighestBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_HighestAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsHighestAskPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.HighestAskPrice);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedHighestAskPrice = 1.23456m;
        var scaleFactor             = pricePrecisionSettings.PriceScalingPrecision;
        emptyCandle.HighestAskPrice = expectedHighestAskPrice;
        Assert.IsTrue(emptyCandle.IsHighestAskPriceUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedHighestAskPrice, emptyCandle.HighestAskPrice);
        var sourceUpdates = emptyCandle.GetDeltaUpdateFields
            (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate
            (PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleHighestPrice, expectedHighestAskPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsHighestAskPriceUpdated = false;
        Assert.IsFalse(emptyCandle.IsHighestAskPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        sourceUpdates = (from update in emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot,
                                                                          pricePrecisionSettings)
            where update is { PricingSubId: PQPricingSubFieldKeys.CandleHighestPrice, DepthId: PQDepthKey.AskSide }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedHighestAskPrice, newEmpty.HighestAskPrice);
        Assert.IsTrue(newEmpty.IsHighestAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_LowestBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsLowestBidPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.LowestBidPrice);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedLowestBidPrice = 1.23456m;
        var scaleFactor            = pricePrecisionSettings.PriceScalingPrecision;
        emptyCandle.LowestBidPrice = expectedLowestBidPrice;
        Assert.IsTrue(emptyCandle.IsLowestBidPriceUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedLowestBidPrice, emptyCandle.LowestBidPrice);
        var sourceUpdates = emptyCandle.GetDeltaUpdateFields
            (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleLowestPrice, expectedLowestBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsLowestBidPriceUpdated = false;
        Assert.IsFalse(emptyCandle.IsLowestBidPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields
                          (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        sourceUpdates = (from update in emptyCandle.GetDeltaUpdateFields
                (testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update is {PricingSubId : PQPricingSubFieldKeys.CandleLowestPrice, DepthId: PQDepthKey.None }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLowestBidPrice, newEmpty.LowestBidPrice);
        Assert.IsTrue(newEmpty.IsLowestBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_LowestAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsLowestAskPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.LowestAskPrice);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedLowestAskPrice = 1.23456m;
        var scaleFactor            = pricePrecisionSettings.PriceScalingPrecision;
        emptyCandle.LowestAskPrice = expectedLowestAskPrice;
        Assert.IsTrue(emptyCandle.IsLowestAskPriceUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedLowestAskPrice, emptyCandle.LowestAskPrice);
        var sourceUpdates = emptyCandle.GetDeltaUpdateFields
            (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleLowestPrice, expectedLowestAskPrice,
                                                    scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsLowestAskPriceUpdated = false;
        Assert.IsFalse(emptyCandle.IsLowestAskPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields
                          (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        sourceUpdates = (from update in emptyCandle.GetDeltaUpdateFields
                (testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update is { PricingSubId: PQPricingSubFieldKeys.CandleLowestPrice, DepthId: PQDepthKey.AskSide }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLowestAskPrice, newEmpty.LowestAskPrice);
        Assert.IsTrue(newEmpty.IsLowestAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsEndBidPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.EndBidPrice);
        Assert.AreEqual(0, emptyCandle.GetDeltaUpdateFields
                            (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedEndBidPrice = 1.23456m;
        var scaleFactor         = pricePrecisionSettings.PriceScalingPrecision;
        emptyCandle.EndBidPrice = expectedEndBidPrice;
        Assert.IsTrue(emptyCandle.IsEndBidPriceUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedEndBidPrice, emptyCandle.EndBidPrice);
        var sourceUpdates = emptyCandle.GetDeltaUpdateFields
            (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndPrice, expectedEndBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsEndBidPriceUpdated = false;
        Assert.IsFalse(emptyCandle.IsEndBidPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields
                          (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        sourceUpdates = (from update in emptyCandle.GetDeltaUpdateFields
                (testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update is {PricingSubId: PQPricingSubFieldKeys.CandleEndPrice, DepthId: PQDepthKey.None}
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedEndBidPrice, newEmpty.EndBidPrice);
        Assert.IsTrue(newEmpty.IsEndBidPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsEndAskPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.EndAskPrice);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedEndAskPrice = 1.23456m;
        
        var scaleFactor         = pricePrecisionSettings.PriceScalingPrecision;
        emptyCandle.EndAskPrice = expectedEndAskPrice;
        Assert.IsTrue(emptyCandle.IsEndAskPriceUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedEndAskPrice, emptyCandle.EndAskPrice);
        var sourceUpdates =
            emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate =
            new PQFieldUpdate
                (PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleEndPrice, expectedEndAskPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsEndAskPriceUpdated = false;
        Assert.IsFalse(emptyCandle.IsEndAskPriceUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue
            (emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        var deltaUpdateFields = emptyCandle
            .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings).ToList();
        sourceUpdates = (from update in deltaUpdateFields
            where update is { PricingSubId: PQPricingSubFieldKeys.CandleEndPrice, DepthId: PQDepthKey.AskSide }
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedEndAskPrice, newEmpty.EndAskPrice);
        Assert.IsTrue(newEmpty.IsEndAskPriceUpdated);
    }

    [TestMethod]
    public void EmptySummary_TickCountChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsTickCountUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0m, emptyCandle.TickCount);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        var expectedTickCount = uint.MaxValue;
        emptyCandle.TickCount = expectedTickCount;
        Assert.IsTrue(emptyCandle.IsTickCountUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedTickCount, emptyCandle.TickCount);
        var sourceUpdates = emptyCandle.GetDeltaUpdateFields
            (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleTickCount, expectedTickCount);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyCandle.IsTickCountUpdated = false;
        Assert.IsFalse(emptyCandle.IsTickCountUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue(emptyCandle.GetDeltaUpdateFields
                          (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        sourceUpdates = (from update in emptyCandle.GetDeltaUpdateFields
                (testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update.PricingSubId == PQPricingSubFieldKeys.CandleTickCount
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedTickCount, newEmpty.TickCount);
        Assert.IsTrue(newEmpty.IsTickCountUpdated);
    }

    [TestMethod]
    public void EmptySummary_PeriodVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsPeriodVolumeUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0L, emptyCandle.PeriodVolume);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedPeriodVolume = 500_000_000_000;
        var scaleFactor          = pricePrecisionSettings.VolumeScalingPrecision;
        emptyCandle.PeriodVolume = expectedPeriodVolume;
        Assert.IsTrue(emptyCandle.IsPeriodVolumeUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedPeriodVolume, emptyCandle.PeriodVolume);
        var periodVolumeUpdates = emptyCandle.GetDeltaUpdateFields
            (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, periodVolumeUpdates.Count);
        var expectedFieldUpdate =
            new PQFieldUpdate
                (PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleVolume, expectedPeriodVolume, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, periodVolumeUpdates[0]);

        emptyCandle.IsPeriodVolumeUpdated = false;
        Assert.IsFalse(emptyCandle.IsPeriodVolumeUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue
            (emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        periodVolumeUpdates = (from update in emptyCandle
                .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update.PricingSubId == PQPricingSubFieldKeys.CandleVolume
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(1, periodVolumeUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, periodVolumeUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(periodVolumeUpdates[0]);
        Assert.AreEqual(expectedPeriodVolume, newEmpty.PeriodVolume);
        Assert.IsTrue(newEmpty.IsPeriodVolumeUpdated);
    }

    [TestMethod]
    public void EmptySummary_SummaryFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyCandle.IsCandleFlagsUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.AreEqual(0L, emptyCandle.PeriodVolume);
        Assert.AreEqual
            (0, emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).Count());

        var expectedFlagsVolume = CandleFlags.MissingTicksRange03;
        emptyCandle.CandleFlags = expectedFlagsVolume;
        Assert.IsTrue(emptyCandle.IsCandleFlagsUpdated);
        Assert.IsTrue(emptyCandle.HasUpdates);
        Assert.AreEqual(expectedFlagsVolume, emptyCandle.CandleFlags);
        var periodFlagUpdates = emptyCandle.GetDeltaUpdateFields
            (testDateTime, PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, periodFlagUpdates.Count);
        var expectedFieldUpdate =
            new PQFieldUpdate
                (PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleSummaryFlags, (uint)expectedFlagsVolume);
        Assert.AreEqual(expectedFieldUpdate, periodFlagUpdates[0]);

        emptyCandle.IsCandleFlagsUpdated = false;
        Assert.IsFalse(emptyCandle.IsCandleFlagsUpdated);
        Assert.IsFalse(emptyCandle.HasUpdates);
        Assert.IsTrue
            (emptyCandle.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pricePrecisionSettings).IsNullOrNone());

        periodFlagUpdates = (from update in emptyCandle
                .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, pricePrecisionSettings)
            where update.PricingSubId == PQPricingSubFieldKeys.CandleSummaryFlags
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(1, periodFlagUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, periodFlagUpdates[0]);

        var newEmpty = new PQCandle();
        newEmpty.UpdateField(periodFlagUpdates[0]);
        Assert.AreEqual(expectedFlagsVolume, newEmpty.CandleFlags);
        Assert.IsTrue(newEmpty.IsCandleFlagsUpdated);
    }

    [TestMethod]
    public void EmptySummary_DifferingCandlePeriod_IsSavedAndReturned()
    {
        Assert.AreEqual(TimeBoundaryPeriod.Tick, emptyCandle.TimeBoundaryPeriod);
        var wellKnownStartTime = new DateTime(2017, 11, 19, 19, 00, 00);
        emptyCandle.PeriodStartTime    = wellKnownStartTime;
        emptyCandle.TimeBoundaryPeriod = TimeBoundaryPeriod.Tick;
        Assert.AreEqual(TimeBoundaryPeriod.Tick, emptyCandle.TimeBoundaryPeriod);
        emptyCandle.TimeBoundaryPeriod = TimeBoundaryPeriod.OneDecade;
        Assert.AreEqual(TimeBoundaryPeriod.OneDecade, emptyCandle.TimeBoundaryPeriod);
    }

    [TestMethod]
    public void PopulatedCandleWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllTickInstantFields()
    {
        var pqFieldUpdates =
            fullyPopulatedCandle.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update, pricePrecisionSettings).ToList();
        AssertCandleContainsAllFields(pricePrecisionSettings, pqFieldUpdates, fullyPopulatedCandle);
    }

    [TestMethod]
    public void PopulatedCandleWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllTickInstantFields()
    {
        fullyPopulatedCandle.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedCandle.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Snapshot, pricePrecisionSettings).ToList();
        AssertCandleContainsAllFields(pricePrecisionSettings, pqFieldUpdates, fullyPopulatedCandle);
    }

    [TestMethod]
    public void PopulatedCandleWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedCandle.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedCandle.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedCandle_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        var pqFieldUpdates =
            fullyPopulatedCandle
                .GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59)
                   , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes, pricePrecisionSettings).ToList();
        var newEmpty = new PQCandle();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(fullyPopulatedCandle, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedCandle_CopyFromToEmptyCandle_CandlesEqualEachOther()
    {
        emptyCandle = new PQCandle();
        emptyCandle.CopyFrom(fullyPopulatedCandle);

        Assert.AreEqual(fullyPopulatedCandle, emptyCandle);
    }

    [TestMethod]
    public void FullyPopulatedCandle_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptyCandle                           = new PQCandle();
        fullyPopulatedCandle.HasUpdates = false;
        emptyCandle.CopyFrom(fullyPopulatedCandle);
        Assert.AreEqual(DateTime.MinValue, emptyCandle.PeriodStartTime);
        Assert.AreEqual(DateTime.MinValue, emptyCandle.PeriodEndTime);
        Assert.AreEqual(0m, emptyCandle.StartBidPrice);
        Assert.AreEqual(0m, emptyCandle.StartAskPrice);
        Assert.AreEqual(0m, emptyCandle.HighestBidPrice);
        Assert.AreEqual(0m, emptyCandle.HighestAskPrice);
        Assert.AreEqual(0m, emptyCandle.LowestBidPrice);
        Assert.AreEqual(0m, emptyCandle.LowestAskPrice);
        Assert.AreEqual(0m, emptyCandle.EndBidPrice);
        Assert.AreEqual(0m, emptyCandle.EndAskPrice);
        Assert.AreEqual(0u, emptyCandle.TickCount);
        Assert.AreEqual(0L, emptyCandle.PeriodVolume);
        Assert.IsFalse(emptyCandle.IsStartTimeDateUpdated);
        Assert.IsFalse(emptyCandle.IsStartTimeSub2MinUpdated);
        Assert.IsFalse(emptyCandle.IsStartBidPriceUpdated);
        Assert.IsFalse(emptyCandle.IsStartAskPriceUpdated);
        Assert.IsFalse(emptyCandle.IsHighestBidPriceUpdated);
        Assert.IsFalse(emptyCandle.IsHighestAskPriceUpdated);
        Assert.IsFalse(emptyCandle.IsLowestBidPriceUpdated);
        Assert.IsFalse(emptyCandle.IsLowestAskPriceUpdated);
        Assert.IsFalse(emptyCandle.IsEndBidPriceUpdated);
        Assert.IsFalse(emptyCandle.IsEndAskPriceUpdated);
        Assert.IsFalse(emptyCandle.IsTickCountUpdated);
        Assert.IsFalse(emptyCandle.IsPeriodVolumeUpdated);
        Assert.IsFalse(emptyCandle.IsCandleFlagsUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedCandle_CopyFromToEmptyCandle_CandlesEquivalentToEachOther()
    {
        var nonPQCandle = new Candle(fullyPopulatedCandle);
        emptyCandle.CopyFrom(nonPQCandle);
        Assert.IsTrue(fullyPopulatedCandle.AreEquivalent(emptyCandle));
    }

    [TestMethod]
    public void FullyInitializedQuote_Clone_CopiesQuoteExactly()
    {
        var clonedQuote = ((ICloneable<ICandle>)fullyPopulatedCandle).Clone();
        Assert.AreEqual(fullyPopulatedCandle, clonedQuote);
        var clonedQuote2 = ((ICloneable)fullyPopulatedCandle).Clone();
        Assert.AreEqual(fullyPopulatedCandle, clonedQuote2);
        var clonedQuote3 = ((IPQCandle)fullyPopulatedCandle).Clone();
        Assert.AreEqual(fullyPopulatedCandle, clonedQuote3);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQCandle)((ICloneable<ICandle>)fullyPopulatedCandle).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedCandle, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedCandle, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedCandle, fullyPopulatedCandle);
        Assert.AreEqual(fullyPopulatedCandle, ((ICloneable)fullyPopulatedCandle).Clone());
        Assert.AreEqual(fullyPopulatedCandle, ((ICloneable<ICandle>)fullyPopulatedCandle).Clone());
        Assert.AreEqual(fullyPopulatedCandle, ((IPQCandle)fullyPopulatedCandle).Clone());
    }

    [TestMethod]
    public void EmptyQuote_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = emptyCandle.GetHashCode();
        Assert.IsTrue(hashCode == 0);
        hashCode = fullyPopulatedCandle.GetHashCode();
        Assert.IsTrue(hashCode != 0);
        Assert.IsTrue(emptyCandle.GetHashCode() != hashCode);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
        (bool exactComparison, PQCandle original, PQCandle changingCandle)
    {
        Assert.IsTrue(original.AreEquivalent(changingCandle));
        Assert.IsTrue(changingCandle.AreEquivalent(original));

        Assert.IsFalse(changingCandle.AreEquivalent(null, exactComparison));
        Assert.AreEqual(!exactComparison,
                        changingCandle.AreEquivalent(new Candle(original), exactComparison));

        changingCandle.PeriodStartTime = new DateTime(2017, 11, 19, 21, 27, 32);
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.PeriodStartTime = original.PeriodStartTime;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.PeriodEndTime = new DateTime(2017, 11, 19, 21, 27, 32);
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.PeriodEndTime = original.PeriodEndTime;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.StartBidPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.StartBidPrice = original.StartBidPrice;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.StartAskPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.StartAskPrice = original.StartAskPrice;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.HighestBidPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.HighestBidPrice = original.HighestBidPrice;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.HighestAskPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.HighestAskPrice = original.HighestAskPrice;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.LowestBidPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.LowestBidPrice = original.LowestBidPrice;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.LowestAskPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.LowestAskPrice = original.LowestAskPrice;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.EndBidPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.EndBidPrice = original.EndBidPrice;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.EndAskPrice = 0.1234567m;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.EndAskPrice = original.EndAskPrice;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.TickCount = uint.MaxValue;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.TickCount = original.TickCount;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        changingCandle.PeriodVolume = long.MaxValue;
        Assert.IsFalse(original.AreEquivalent(changingCandle, exactComparison));
        changingCandle.PeriodVolume = original.PeriodVolume;
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));

        NonPublicInvocator.SetInstanceField(changingCandle, "updatedFlags", CandleUpdatedFlags.None);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingCandle, exactComparison));
        var originalUpdatedFlags =
            NonPublicInvocator.GetInstanceField<CandleUpdatedFlags>(original, "updatedFlags");
        NonPublicInvocator.SetInstanceField(changingCandle, "updatedFlags", originalUpdatedFlags);
        Assert.IsTrue(changingCandle.AreEquivalent(original, exactComparison));
    }

    public static void AssertCandleContainsAllFields
        (IPQPriceVolumePublicationPrecisionSettings precisionSettings,  IList<PQFieldUpdate> checkFieldUpdates, IPQCandle candle, PQFeedFields feedField = PQFeedFields.PriceCandleStick)
    {
        var priceScale  = precisionSettings.PriceScalingPrecision;
        var volumeScale = precisionSettings.VolumeScalingPrecision;
        Assert.AreEqual(new PQFieldUpdate
                            (feedField, PQPricingSubFieldKeys.CandleStartDateTime, candle.PeriodStartTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleStartDateTime),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        var fifthByte = candle.PeriodStartTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleStartSub2MinTime, lowerFourBytes, fifthByte),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleStartSub2MinTime),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate
                            (feedField, PQPricingSubFieldKeys.CandleEndDateTime, candle.PeriodEndTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleEndDateTime),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        fifthByte = candle.PeriodEndTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleEndSub2MinTime, lowerFourBytes, fifthByte),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleEndSub2MinTime),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleStartPrice, candle.StartBidPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleStartPrice, priceScale),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate (feedField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleStartPrice, candle.StartAskPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, feedField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleStartPrice, priceScale),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleHighestPrice, candle.HighestBidPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleHighestPrice, priceScale),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate
                            (feedField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleHighestPrice, candle.HighestAskPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, feedField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleHighestPrice, priceScale),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleLowestPrice, candle.LowestBidPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleLowestPrice, priceScale),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate
                            (feedField,  PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleLowestPrice,candle.LowestAskPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, feedField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleLowestPrice, priceScale),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleEndPrice, candle.EndBidPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleEndPrice, priceScale),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(feedField, PQDepthKey.AskSide,  PQPricingSubFieldKeys.CandleEndPrice, candle.EndAskPrice, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, feedField, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleEndPrice, priceScale),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleTickCount, candle.TickCount),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleTickCount),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleVolume, candle.PeriodVolume),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleVolume),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(feedField, PQPricingSubFieldKeys.CandleSummaryFlags, (uint)candle.CandleFlags),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, feedField, PQPricingSubFieldKeys.CandleSummaryFlags),
                        $"For Candle {candle} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }
}
