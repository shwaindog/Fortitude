﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using FortitudeMarketsCore.Pricing.Summaries;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Summaries;

[TestClass]
public class PQPricePeriodSummaryTests
{
    private PQPricePeriodSummary    emptySummary                = null!;
    private PQPricePeriodSummary    fullyPopulatedPeriodSummary = null!;
    private PQSourceTickerQuoteInfo pricePrecisionSettings      = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        pricePrecisionSettings =
            new PQSourceTickerQuoteInfo
                (new SourceTickerQuoteInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3, Unknown
                   , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
                   , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
                   , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime));

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
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptySummary.SummaryStartTime);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                             pricePrecisionSettings).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptySummary.SummaryStartTime = expectedSetTime;
        Assert.IsTrue(emptySummary.IsStartTimeDateUpdated);
        Assert.IsTrue(emptySummary.IsStartTimeSubHourUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptySummary.SummaryStartTime);
        var sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                                 pricePrecisionSettings).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var fifthByte           = expectedSetTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
        var expectedHour        = new PQFieldUpdate(PQFieldKeys.PeriodStartDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQFieldKeys.PeriodStartSubHourTime, lowerFourBytes, fifthByte);
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
            where update.Id >= PQFieldKeys.PeriodStartDateTime && update.Id <=
                PQFieldKeys.PeriodStartSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.SummaryStartTime);
        Assert.IsTrue(newEmpty.IsStartTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsStartTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptySummary_EndTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySummary.IsEndTimeDateUpdated);
        Assert.IsFalse(emptySummary.IsEndTimeSubHourUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptySummary.SummaryEndTime);
        Assert.AreEqual(0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                             pricePrecisionSettings).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptySummary.SummaryEndTime = expectedSetTime;
        Assert.IsTrue(emptySummary.IsEndTimeDateUpdated);
        Assert.IsTrue(emptySummary.IsEndTimeSubHourUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptySummary.SummaryEndTime);
        var sourceAskUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                                 pricePrecisionSettings).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var fifthByte           = expectedSetTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
        var expectedHour        = new PQFieldUpdate(PQFieldKeys.PeriodEndDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQFieldKeys.PeriodEndSubHourTime, lowerFourBytes, fifthByte);
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
            where update.Id >= PQFieldKeys.PeriodEndDateTime && update.Id <=
                PQFieldKeys.PeriodEndSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.SummaryEndTime);
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
        var scaleFactor           = PQScaling.FindScaleFactor(expectedStartBidPrice - 1);
        emptySummary.StartBidPrice = expectedStartBidPrice;
        Assert.IsTrue(emptySummary.IsStartBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedStartBidPrice, emptySummary.StartBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, expectedStartBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsStartBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsStartBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodStartPrice
               && update.Flag == scaleFactor
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
        var scaleFactor           = PQScaling.FindScaleFactor(expectedStartAskPrice - 1);
        emptySummary.StartAskPrice = expectedStartAskPrice;
        Assert.IsTrue(emptySummary.IsStartAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedStartAskPrice, emptySummary.StartAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate
            (PQFieldKeys.PeriodStartPrice, expectedStartAskPrice, (byte)(scaleFactor | PQFieldFlags.IsAskSideFlag));
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsStartAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsStartAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodStartPrice
               && update.Flag == (byte)(scaleFactor | PQFieldFlags.IsAskSideFlag)
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
        var scaleFactor             = PQScaling.FindScaleFactor(expectedHighestBidPrice - 1);
        emptySummary.HighestBidPrice = expectedHighestBidPrice;
        Assert.IsTrue(emptySummary.IsHighestBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedHighestBidPrice, emptySummary.HighestBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update,
                                                              pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, expectedHighestBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsHighestBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsHighestBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodHighestPrice
               && update.Flag == scaleFactor
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
        var scaleFactor             = PQScaling.FindScaleFactor(expectedHighestAskPrice - 1);
        emptySummary.HighestAskPrice = expectedHighestAskPrice;
        Assert.IsTrue(emptySummary.IsHighestAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedHighestAskPrice, emptySummary.HighestAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate
            (PQFieldKeys.PeriodHighestPrice, expectedHighestAskPrice, (byte)(scaleFactor | PQFieldFlags.IsAskSideFlag));
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsHighestAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsHighestAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot,
                                                                          pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodHighestPrice
               && update.Flag == (byte)(scaleFactor | PQFieldFlags.IsAskSideFlag)
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
        var scaleFactor            = PQScaling.FindScaleFactor(expectedLowestBidPrice - 1);
        emptySummary.LowestBidPrice = expectedLowestBidPrice;
        Assert.IsTrue(emptySummary.IsLowestBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedLowestBidPrice, emptySummary.LowestBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, expectedLowestBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsLowestBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsLowestBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodLowestPrice
               && update.Flag == scaleFactor
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
        var scaleFactor            = PQScaling.FindScaleFactor(expectedLowestAskPrice - 1);
        emptySummary.LowestAskPrice = expectedLowestAskPrice;
        Assert.IsTrue(emptySummary.IsLowestAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedLowestAskPrice, emptySummary.LowestAskPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, expectedLowestAskPrice,
                                                    (byte)(scaleFactor | PQFieldFlags.IsAskSideFlag));
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsLowestAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsLowestAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodLowestPrice
               && update.Flag == (byte)(scaleFactor | PQFieldFlags.IsAskSideFlag)
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
        var scaleFactor         = PQScaling.FindScaleFactor(expectedEndBidPrice - 1);
        emptySummary.EndBidPrice = expectedEndBidPrice;
        Assert.IsTrue(emptySummary.IsEndBidPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedEndBidPrice, emptySummary.EndBidPrice);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, expectedEndBidPrice, scaleFactor);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsEndBidPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsEndBidPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodEndPrice
               && update.Flag == scaleFactor
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        var expectedTickCount = uint.MaxValue;
        emptySummary.TickCount = expectedTickCount;
        Assert.IsTrue(emptySummary.IsTickCountUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedTickCount, emptySummary.TickCount);
        var sourceUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PeriodTickCount, expectedTickCount);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsTickCountUpdated = false;
        Assert.IsFalse(emptySummary.IsTickCountUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue(emptySummary.GetDeltaUpdateFields
                          (testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary.GetDeltaUpdateFields
                (testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodTickCount
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedEndAskPrice = 1.23456m;
        var scaleFactor         = PQScaling.FindScaleFactor(expectedEndAskPrice - 1);
        emptySummary.EndAskPrice = expectedEndAskPrice;
        Assert.IsTrue(emptySummary.IsEndAskPriceUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedEndAskPrice, emptySummary.EndAskPrice);
        var sourceUpdates =
            emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate =
            new PQFieldUpdate
                (PQFieldKeys.PeriodEndPrice, expectedEndAskPrice, (byte)(scaleFactor | PQFieldFlags.IsAskSideFlag));
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySummary.IsEndAskPriceUpdated = false;
        Assert.IsFalse(emptySummary.IsEndAskPriceUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        sourceUpdates = (from update in emptySummary
                .GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id == PQFieldKeys.PeriodEndPrice
               && update.Flag == (byte)(scaleFactor | PQFieldFlags.IsAskSideFlag)
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPricePeriodSummary();
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
        Assert.AreEqual
            (0, emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).Count());

        var expectedPeriodVolume = long.MaxValue;
        emptySummary.PeriodVolume = expectedPeriodVolume;
        Assert.IsTrue(emptySummary.IsPeriodVolumeLowerBytesUpdated);
        Assert.IsTrue(emptySummary.IsPeriodVolumeUpperBytesUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        Assert.AreEqual(expectedPeriodVolume, emptySummary.PeriodVolume);
        var periodVolumeUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(2, periodVolumeUpdates.Count);
        var lowerBytes         = (uint)emptySummary.PeriodVolume;
        var upperBytes         = (uint)(emptySummary.PeriodVolume >> 32);
        var expectedLowerBytes = new PQFieldUpdate(PQFieldKeys.PeriodVolumeLowerBytes, lowerBytes);
        var expectedUpperBytes = new PQFieldUpdate(PQFieldKeys.PeriodVolumeUpperBytes, upperBytes);
        Assert.AreEqual(expectedLowerBytes, periodVolumeUpdates[0]);
        Assert.AreEqual(expectedUpperBytes, periodVolumeUpdates[1]);

        emptySummary.IsPeriodVolumeLowerBytesUpdated = false;
        Assert.IsFalse(emptySummary.IsPeriodVolumeLowerBytesUpdated);
        Assert.IsTrue(emptySummary.HasUpdates);
        periodVolumeUpdates = emptySummary.GetDeltaUpdateFields
            (testDateTime, StorageFlags.Update, pricePrecisionSettings).ToList();
        Assert.AreEqual(1, periodVolumeUpdates.Count);
        Assert.AreEqual(expectedUpperBytes, periodVolumeUpdates[0]);

        emptySummary.IsPeriodVolumeUpperBytesUpdated = false;
        Assert.IsFalse(emptySummary.IsPeriodVolumeUpperBytesUpdated);
        Assert.IsFalse(emptySummary.HasUpdates);
        Assert.IsTrue
            (emptySummary.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pricePrecisionSettings).IsNullOrEmpty());

        periodVolumeUpdates = (from update in emptySummary
                .GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, pricePrecisionSettings)
            where update.Id >= PQFieldKeys.PeriodVolumeLowerBytes && update.Id <=
                PQFieldKeys.PeriodVolumeUpperBytes
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, periodVolumeUpdates.Count);
        Assert.AreEqual(expectedLowerBytes, periodVolumeUpdates[0]);
        Assert.AreEqual(expectedUpperBytes, periodVolumeUpdates[1]);

        var newEmpty = new PQPricePeriodSummary();
        newEmpty.UpdateField(periodVolumeUpdates[0]);
        newEmpty.UpdateField(periodVolumeUpdates[1]);
        Assert.AreEqual(expectedPeriodVolume, newEmpty.PeriodVolume);
        Assert.IsTrue(newEmpty.IsPeriodVolumeLowerBytesUpdated);
        Assert.IsTrue(newEmpty.IsPeriodVolumeUpperBytesUpdated);
    }

    [TestMethod]
    public void EmptySummary_DifferingSummaryPeriod_IsSavedAndReturned()
    {
        Assert.AreEqual(TimeSeriesPeriod.None, emptySummary.SummaryPeriod);
        var wellKnownStartTime = new DateTime(2017, 11, 19, 19, 00, 00);
        emptySummary.SummaryStartTime = wellKnownStartTime;
        Assert.AreEqual(TimeSeriesPeriod.None, emptySummary.SummaryPeriod);
        emptySummary.SummaryPeriod = TimeSeriesPeriod.Tick;
        Assert.AreEqual(TimeSeriesPeriod.Tick, emptySummary.SummaryPeriod);
        emptySummary.SummaryPeriod = TimeSeriesPeriod.OneDecade;
        Assert.AreEqual(TimeSeriesPeriod.OneDecade, emptySummary.SummaryPeriod);
    }

    [TestMethod]
    public void PopulatedPeriodSummaryWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel0Fields()
    {
        var pqFieldUpdates =
            fullyPopulatedPeriodSummary.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update, pricePrecisionSettings).ToList();
        AssertPeriodSummaryContainsAllFields(pricePrecisionSettings, pqFieldUpdates, fullyPopulatedPeriodSummary);
    }

    [TestMethod]
    public void PopulatedPeriodSummaryWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel0Fields()
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
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptySummary.SummaryStartTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptySummary.SummaryEndTime);
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
        Assert.IsTrue(hashCode != 0);
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

        changingPeriodSummary.SummaryStartTime = new DateTime(2017, 11, 19, 21, 27, 32);
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.SummaryStartTime = original.SummaryStartTime;
        Assert.IsTrue(changingPeriodSummary.AreEquivalent(original, exactComparison));

        changingPeriodSummary.SummaryEndTime = new DateTime(2017, 11, 19, 21, 27, 32);
        Assert.IsFalse(original.AreEquivalent(changingPeriodSummary, exactComparison));
        changingPeriodSummary.SummaryEndTime = original.SummaryEndTime;
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
        (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates, IPQPricePeriodSummary periodSummary)
    {
        var priceScale  = precisionSettings.PriceScalingPrecision;
        var volumeScale = precisionSettings.VolumeScalingPrecision;
        Assert.AreEqual(new PQFieldUpdate
                            (PQFieldKeys.PeriodStartDateTime, periodSummary.SummaryStartTime.GetHoursFromUnixEpoch()),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodStartDateTime));
        var fifthByte = periodSummary.SummaryStartTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodStartSubHourTime, lowerFourBytes, fifthByte),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodStartSubHourTime));
        Assert.AreEqual(new PQFieldUpdate
                            (PQFieldKeys.PeriodEndDateTime, periodSummary.SummaryEndTime.GetHoursFromUnixEpoch()),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodEndDateTime));
        fifthByte = periodSummary.SummaryEndTime.GetSubHourComponent().BreakLongToByteAndUint(out lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodEndSubHourTime, lowerFourBytes, fifthByte),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodEndSubHourTime));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, periodSummary.StartBidPrice, priceScale),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodStartPrice, priceScale));
        Assert.AreEqual(new PQFieldUpdate
                            (PQFieldKeys.PeriodStartPrice, PQScaling.Scale(periodSummary.StartAskPrice, priceScale)
                           , (byte)(PQFieldFlags.IsAskSideFlag | priceScale)),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFieldKeys.PeriodStartPrice, (byte)(PQFieldFlags.IsAskSideFlag | priceScale)));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, PQScaling.Scale(periodSummary.HighestBidPrice, priceScale), priceScale),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodHighestPrice, priceScale));
        Assert.AreEqual(new PQFieldUpdate
                            (PQFieldKeys.PeriodHighestPrice, PQScaling.Scale(periodSummary.HighestAskPrice, priceScale)
                           , (byte)(PQFieldFlags.IsAskSideFlag | priceScale)),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFieldKeys.PeriodHighestPrice, (byte)(PQFieldFlags.IsAskSideFlag | priceScale)));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, PQScaling.Scale(periodSummary.LowestBidPrice, priceScale), priceScale),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodLowestPrice, priceScale));
        Assert.AreEqual(new PQFieldUpdate
                            (PQFieldKeys.PeriodLowestPrice, PQScaling.Scale(periodSummary.LowestAskPrice, priceScale)
                           , (byte)(PQFieldFlags.IsAskSideFlag | priceScale)),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFieldKeys.PeriodLowestPrice, (byte)(PQFieldFlags.IsAskSideFlag | priceScale)));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, PQScaling.Scale(periodSummary.EndBidPrice, priceScale), priceScale),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PeriodEndPrice, priceScale));
        Assert.AreEqual(new PQFieldUpdate
                            (PQFieldKeys.PeriodEndPrice, PQScaling.Scale(periodSummary.EndAskPrice, priceScale)
                           , (byte)(PQFieldFlags.IsAskSideFlag | priceScale)),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFieldKeys.PeriodEndPrice, (byte)(PQFieldFlags.IsAskSideFlag | priceScale)));
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