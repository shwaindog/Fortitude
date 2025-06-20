﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;
using static FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.PQQuoteBooleanValues;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

[TestClass]
public class PQLevel1QuoteTests
{
    private ISourceTickerInfo blankSourceTickerInfo = null!;

    private PQPublishableLevel1Quote emptyQuote                  = null!;
    private PQPublishableLevel1Quote fullyPopulatedPqLevel1Quote = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private PQSourceTickerInfo            sourceTickerInfo              = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        sourceTickerInfo =
            new PQSourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.0000001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime)
                {
                    HasUpdates = true
                };
        blankSourceTickerInfo       = new SourceTickerInfo(0, "", 0, "", Level1Quote, MarketClassification.Unknown);
        fullyPopulatedPqLevel1Quote = new PQPublishableLevel1Quote(new PQSourceTickerInfo(sourceTickerInfo));
        emptyQuote                  = new PQPublishableLevel1Quote(new PQSourceTickerInfo(sourceTickerInfo))
        {
            PQSequenceId = 1,
            HasUpdates = false
        };
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedPqLevel1Quote, 1);

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
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
    public void EmptyQuote_SourceAskTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsSourceAskTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceAskTimeSub2MinUpdated);
        emptyQuote.SourceAskTime = DateTime.Now;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.SourceAskTime = DateTime.MinValue;
        emptyQuote.HasUpdates    = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(default, emptyQuote.SourceAskTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceAskTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceAskTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceAskTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceAskTime);
        var sourceAskUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
            where update.Id is >= PQFeedFields.SourceQuoteAskDateTime and <= PQFeedFields.SourceQuoteAskSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var extended            = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourComponent);
        var expectedHour        = new PQFieldUpdate(PQFeedFields.SourceQuoteAskDateTime, hoursSinceUnixEpoch);
        var expectedSub2Min     = new PQFieldUpdate(PQFeedFields.SourceQuoteAskSub2MinTime, subHourComponent, extended);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceAskUpdates[1]);

        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceAskUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
            where update.Id is >= PQFeedFields.SourceQuoteAskDateTime and <= PQFeedFields.SourceQuoteAskSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceAskUpdates[1]);

        emptyQuote.IsSourceAskTimeDateUpdated    = false;
        emptyQuote.IsSourceAskTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceAskTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrNone());


        var newEmpty = new PQPublishableLevel1Quote(sourceTickerInfo);
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.SourceAskTime);
        Assert.IsTrue(newEmpty.IsSourceAskTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsSourceAskTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptyQuote_SourceBidTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceBidTimeSub2MinUpdated);
        emptyQuote.SourceBidTime = DateTime.Now;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.SourceBidTime = DateTime.MinValue;
        emptyQuote.HasUpdates    = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(default, emptyQuote.SourceBidTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceBidTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceBidTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceBidTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceBidTime);
        var sourceBidUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
            where update.Id is >= PQFeedFields.SourceQuoteBidDateTime and <= PQFeedFields.SourceQuoteBidSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceBidUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var extended            = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourComponent);
        var expectedHour        = new PQFieldUpdate(PQFeedFields.SourceQuoteBidDateTime, hoursSinceUnixEpoch);
        var expectedSub2Min     = new PQFieldUpdate(PQFeedFields.SourceQuoteBidSub2MinTime, subHourComponent, extended);
        Assert.AreEqual(expectedHour, sourceBidUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceBidUpdates[1]);

        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceBidUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
            where update.Id is >= PQFeedFields.SourceQuoteBidDateTime and <= PQFeedFields.SourceQuoteBidSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceBidUpdates.Count);
        Assert.AreEqual(expectedHour, sourceBidUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceBidUpdates[1]);

        emptyQuote.IsSourceBidTimeDateUpdated    = false;
        emptyQuote.IsSourceBidTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceBidTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrNone());

        var newEmpty = new PQPublishableLevel1Quote(sourceTickerInfo);
        newEmpty.UpdateField(sourceBidUpdates[0]);
        newEmpty.UpdateField(sourceBidUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.SourceBidTime);
        Assert.IsTrue(newEmpty.IsSourceBidTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsSourceBidTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptyQuote_AdapterSentTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(default, emptyQuote.AdapterSentTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.AdapterSentTime = expectedSetTime;
        emptyQuote.WithIsAdapterReplay(true);
        Assert.IsTrue(emptyQuote.IsAdapterSentTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsAdapterSentTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.AdapterSentTime);
        var adapterSentUpdates = emptyQuote.GetDeltaUpdateFields(expectedSetTime, PQMessageFlags.Update, emptyQuote.SourceTickerInfo).ToList();
        Assert.AreEqual(3, adapterSentUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var subHourComponent    = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerBytes);
        var expectedHour        = new PQFieldUpdate(PQFeedFields.AdapterSentDateTime, hoursSinceUnixEpoch);
        var expectedSub2Min = new PQFieldUpdate
            (PQFeedFields.AdapterSentSub2MinTime, lowerBytes, subHourComponent);
        Assert.AreEqual(expectedHour, adapterSentUpdates[1]);
        Assert.AreEqual(expectedSub2Min, adapterSentUpdates[2]);

        emptyQuote.IsAdapterSentTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        adapterSentUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(2, adapterSentUpdates.Count);
        Assert.AreEqual(expectedSub2Min, adapterSentUpdates[1]);

        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeSub2MinUpdated);
        emptyQuote.IsFeedConnectivityStatusUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrNone());
        emptyQuote.WithIsAdapterReplay(false);
        emptyQuote.IsFeedConnectivityStatusUpdated = false;

        adapterSentUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id is >= PQFeedFields.AdapterSentDateTime and <= PQFeedFields.AdapterSentSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, adapterSentUpdates.Count);
        hoursSinceUnixEpoch = testDateTime.Get2MinIntervalsFromUnixEpoch();
        expectedHour        = new PQFieldUpdate(PQFeedFields.AdapterSentDateTime, hoursSinceUnixEpoch);
        subHourComponent    = testDateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out lowerBytes);
        expectedSub2Min     = new PQFieldUpdate(PQFeedFields.AdapterSentSub2MinTime, lowerBytes, subHourComponent);
        Assert.AreEqual(expectedHour, adapterSentUpdates[0]);
        Assert.AreEqual(expectedSub2Min, adapterSentUpdates[1]);

        var newEmpty = new PQPublishableLevel1Quote(sourceTickerInfo);
        newEmpty.UpdateField(adapterSentUpdates[0]);
        newEmpty.UpdateField(adapterSentUpdates[1]);
        Assert.AreEqual(testDateTime, newEmpty.AdapterSentTime);
        Assert.IsTrue(newEmpty.IsAdapterSentTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsAdapterSentTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptyQuote_AdapterReceivedTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.AdapterReceivedTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsAdapterReceivedTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsAdapterReceivedTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.AdapterReceivedTime);
        var adapterReceivedUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
            where update.Id is >= PQFeedFields.AdapterReceivedDateTime and <= PQFeedFields.AdapterReceivedSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, adapterReceivedUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var extended            = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourComponent);
        var expectedHour        = new PQFieldUpdate(PQFeedFields.AdapterReceivedDateTime, hoursSinceUnixEpoch);
        var expectedSub2Min     = new PQFieldUpdate(PQFeedFields.AdapterReceivedSub2MinTime, subHourComponent, extended);
        Assert.AreEqual(expectedHour, adapterReceivedUpdates[0]);
        Assert.AreEqual(expectedSub2Min, adapterReceivedUpdates[1]);

        Assert.IsTrue(emptyQuote.HasUpdates);
        adapterReceivedUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
            where update.Id is >= PQFeedFields.AdapterReceivedDateTime and <= PQFeedFields.AdapterReceivedSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, adapterReceivedUpdates.Count);
        Assert.AreEqual(expectedHour, adapterReceivedUpdates[0]);
        Assert.AreEqual(expectedSub2Min, adapterReceivedUpdates[1]);

        emptyQuote.IsAdapterReceivedTimeDateUpdated    = false;
        emptyQuote.IsAdapterReceivedTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.AdapterSentTime                 = testDateTime;
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        var deltaUpdateFields = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.IsTrue(deltaUpdateFields.IsNullOrNone());

        var newEmpty = new PQPublishableLevel1Quote(sourceTickerInfo);
        newEmpty.UpdateField(adapterReceivedUpdates[0]);
        newEmpty.UpdateField(adapterReceivedUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.AdapterReceivedTime);
        Assert.IsTrue(newEmpty.IsAdapterReceivedTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsAdapterReceivedTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptyQuote_BidPriceTopChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        emptyQuote.BidPriceTop = 1m;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.BidPriceTop          = 0;
        emptyQuote.IsBidPriceTopChanged = false;
        emptyQuote.HasUpdates           = false;
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        var checkDeltaUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(2, checkDeltaUpdates.Count);

        var expectedBidPriceTop = 1.2345678m;
        var priceScale          = sourceTickerInfo.PriceScalingPrecision;
        emptyQuote.BidPriceTop = expectedBidPriceTop;
        Assert.IsTrue(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedBidPriceTop, emptyQuote.BidPriceTop);
        var sourceUpdatesWithUpdated = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(4, sourceUpdatesWithUpdated.Count);
        var expectedTopOfBookUpdatedFieldUpdate =
            new PQFieldUpdate
                (PQFeedFields.QuoteBooleanFlags
               , (uint)(IsBidPriceTopChangedUpdatedFlag | IsBidPriceTopChangedSetFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedTopOfBookUpdatedFieldUpdate, sourceUpdatesWithUpdated[2]);
        var expectedTopOfBookPriceFieldUpdate = new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, expectedBidPriceTop, priceScale);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithUpdated[3]);

        emptyQuote.IsBidPriceTopUpdated = false;
        Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsBidPriceTopChanged            = false;
        emptyQuote.IsBidPriceTopChangedUpdated     = false;
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrNone());

        var sourceUpdatesWithoutUpdated = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.QuoteLayerPrice
               && update.Flag == priceScale
            select update).ToList();
        Assert.AreEqual(1, sourceUpdatesWithoutUpdated.Count);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithoutUpdated[0]);

        var newEmpty = new PQPublishableLevel1Quote(sourceTickerInfo);
        Assert.IsFalse(newEmpty.IsBidPriceTopChanged);
        newEmpty.UpdateField(sourceUpdatesWithoutUpdated[0]);
        Assert.AreEqual(expectedBidPriceTop, newEmpty.BidPriceTop);
        Assert.IsFalse(newEmpty.IsBidPriceTopChanged);
        Assert.IsTrue(newEmpty.IsBidPriceTopUpdated);
    }

    [TestMethod]
    public void EmptyQuote_AskPriceTopChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        emptyQuote.AskPriceTop = 24m;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.AskPriceTop          = 0;
        emptyQuote.IsAskPriceTopChanged = false;
        emptyQuote.HasUpdates           = false;
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        var deltaUpdateFields = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update);
        Assert.AreEqual(2, deltaUpdateFields.Count());

        var expectedAskPriceTop = 1.2345678m;
        var priceScale          = sourceTickerInfo.PriceScalingPrecision;
        emptyQuote.AskPriceTop = expectedAskPriceTop;
        Assert.IsTrue(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedAskPriceTop, emptyQuote.AskPriceTop);
        var sourceUpdatesWithUpdated = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(4, sourceUpdatesWithUpdated.Count);
        var expectedTopOfBookUpdatedFieldUpdate =
            new PQFieldUpdate
                (PQFeedFields.QuoteBooleanFlags, (uint)(IsAskPriceTopChangedUpdatedFlag | IsAskPriceTopChangedSetFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedTopOfBookUpdatedFieldUpdate, sourceUpdatesWithUpdated[2]);
        var expectedTopOfBookPriceFieldUpdate =
            new PQFieldUpdate
                (PQFeedFields.QuoteLayerPrice, PQDepthKey.AskSide, expectedAskPriceTop, priceScale);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithUpdated[3]);

        emptyQuote.IsAskPriceTopUpdated = false;
        Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);

        emptyQuote.IsAskPriceTopChanged            = false;
        emptyQuote.IsAskPriceTopChangedUpdated     = false;
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        var checkNoUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.IsTrue(checkNoUpdates.IsNullOrNone());

        var allUpdates =
            (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
                select update).ToList();
        var sourceUpdatesWithoutUpdated = (from update in allUpdates
            where update is { Id: PQFeedFields.QuoteLayerPrice, DepthId: PQDepthKey.AskSide }
               && update.Flag == (priceScale | PQFieldFlags.IncludesDepth)
            select update).ToList();
        Assert.AreEqual(1, sourceUpdatesWithoutUpdated.Count, $"Where all updates is \n[{string.Join(",\n", allUpdates)}]");
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithoutUpdated[0]);

        var newEmpty = new PQPublishableLevel1Quote(sourceTickerInfo);
        Assert.IsFalse(newEmpty.IsAskPriceTopChanged);
        newEmpty.UpdateField(sourceUpdatesWithoutUpdated[0]);
        Assert.AreEqual(expectedAskPriceTop, newEmpty.AskPriceTop);
        Assert.IsTrue(newEmpty.IsAskPriceTopUpdated);
        Assert.IsFalse(newEmpty.IsAskPriceTopChanged);
    }

    [TestMethod]
    public void EmptyQuote_ExecutableChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsExecutableUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        emptyQuote.Executable = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.Executable = true;
        emptyQuote.HasUpdates = false;
        Assert.AreEqual(true, emptyQuote.Executable);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        const bool expectedExecutable = false;
        emptyQuote.Executable = expectedExecutable;
        Assert.IsTrue(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedExecutable, emptyQuote.Executable);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(3, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate
            (PQFeedFields.QuoteBooleanFlags, (uint)IsExecutableUpdatedFlag);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[2]);

        emptyQuote.IsExecutableUpdated = false;
        Assert.IsFalse(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrNone());

        emptyQuote.Executable = true;
        Assert.IsTrue(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(true, emptyQuote.Executable);
        sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate
            (PQFeedFields.QuoteBooleanFlags, (uint)(IsExecutableUpdatedFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
        expectedFieldUpdate = new PQFieldUpdate
            (PQFeedFields.QuoteBooleanFlags, (uint)(IsExecutableUpdatedFlag | IsExecutableSetFlag | PQQuoteBooleanValuesExtensions.AllUpdated));

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.QuoteBooleanFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPublishableLevel1Quote(sourceTickerInfo)
        {
            Executable = false, IsExecutableUpdated = false
        };
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(true, newEmpty.Executable);
        Assert.IsTrue(newEmpty.IsExecutableUpdated);
    }

    [TestMethod]
    public void EmptyQuote_FieldsSetThenResetFields_SameEmptyQuoteEquivalent()
    {
        Assert.IsFalse(emptyQuote.IsFeedConnectivityStatusUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        emptyQuote.SourceTime = DateTime.Now;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.SourceTime = DateTime.MinValue;
        emptyQuote.HasUpdates = false;
        Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyQuote.FeedMarketConnectivityStatus);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        emptyQuote.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.ClosingSoon;
        emptyQuote.FeedSyncStatus = FeedSyncStatus.Good;
        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceTime = expectedSetTime;
        var expectedSingleValue = 1.2345678m;
        emptyQuote.SingleTickValue     = expectedSingleValue;
        emptyQuote.SourceAskTime       = new DateTime(2017, 11, 09, 22, 04, 15);
        emptyQuote.SourceBidTime       = new DateTime(2017, 11, 09, 22, 04, 15).AddMilliseconds(123);
        emptyQuote.AdapterReceivedTime = new DateTime(2017, 11, 09, 22, 04, 15).AddMilliseconds(234);
        emptyQuote.AdapterSentTime     = new DateTime(2017, 11, 09, 22, 04, 15).AddMilliseconds(456);
        var expectBidPriceTop = 1.2344567m;
        emptyQuote.BidPriceTop = expectBidPriceTop;
        var expectedAskPriceTop = 1.2346789m;
        emptyQuote.AskPriceTop = expectedAskPriceTop;
        emptyQuote.Executable  = false;
        Assert.IsTrue(emptyQuote.HasUpdates);

        emptyQuote.ResetWithTracking();

        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyQuote.FeedMarketConnectivityStatus);
        Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
        Assert.AreEqual(default, emptyQuote.SourceTime);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(default, emptyQuote.SourceAskTime);
        Assert.AreEqual(default, emptyQuote.SourceBidTime);
        Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(default, emptyQuote.AdapterSentTime);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.IsTrue(emptyQuote.Executable);
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel1Fields()
    {
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields
            (new DateTime(2017, 11, 04, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllLevel1Fields
            ((PQSourceTickerInfo)fullyPopulatedPqLevel1Quote.SourceTickerInfo!, pqFieldUpdates, fullyPopulatedPqLevel1Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel1Fields()
    {
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields
            (new DateTime(2017, 11, 04, 15, 33, 5), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllLevel1Fields
            ((PQSourceTickerInfo)fullyPopulatedPqLevel1Quote.SourceTickerInfo!, pqFieldUpdates, fullyPopulatedPqLevel1Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        fullyPopulatedPqLevel1Quote.FeedMarketConnectivityStatus   = FeedConnectivityStatusFlags.IsAdapterReplay;
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields
            (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields
            (new DateTime(2017, 11, 04, 13, 33, 3), PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQPublishableLevel1Quote(sourceTickerInfo)
        {
            PQSequenceId = fullyPopulatedPqLevel1Quote.PQSequenceId
        };
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        // not copied from field updates as is used in by server to track publication times.
        newEmpty.LastPublicationTime = fullyPopulatedPqLevel1Quote.LastPublicationTime;
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new PQPublishableLevel1Quote(blankSourceTickerInfo);
        emptyQuote.CopyFrom(fullyPopulatedPqLevel1Quote, CopyMergeFlags.FullReplace);

        Assert.AreEqual(fullyPopulatedPqLevel1Quote, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptyQuote                             = new PQPublishableLevel1Quote(blankSourceTickerInfo);
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        emptyQuote.CopyFrom(fullyPopulatedPqLevel1Quote);
        Assert.AreEqual(fullyPopulatedPqLevel1Quote.PQSequenceId, emptyQuote.PQSequenceId);
        Assert.AreEqual(default, emptyQuote.SourceTime);
        Assert.IsFalse
            (fullyPopulatedPqLevel1Quote.SourceTickerInfo!.AreEquivalent(emptyQuote.SourceTickerInfo));
        Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyQuote.FeedMarketConnectivityStatus);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
        Assert.AreEqual(default, emptyQuote.SourceBidTime);
        Assert.AreEqual(default, emptyQuote.SourceAskTime);
        Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(default, emptyQuote.AdapterSentTime);
        Assert.AreEqual(default, emptyQuote.ClientReceivedTime);
        Assert.AreEqual(default, emptyQuote.SubscriberDispatchedTime);
        Assert.AreEqual(default, emptyQuote.InboundProcessedTime);
        Assert.AreEqual(default, emptyQuote.InboundSocketReceivingTime);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.IsTrue(emptyQuote.Executable);
        Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.IsFeedConnectivityStatusUpdated);
        Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
        Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
        Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceBidTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.IsSourceAskTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceAskTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsFalse(emptyQuote.IsExecutableUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var nonPQLevel1Quote = new PublishableLevel1PriceQuote(fullyPopulatedPqLevel1Quote);
        emptyQuote.CopyFrom(nonPQLevel1Quote, CopyMergeFlags.Default);
        Assert.IsTrue(fullyPopulatedPqLevel1Quote.AreEquivalent(emptyQuote));
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<IPublishableTickInstant>)fullyPopulatedPqLevel1Quote).Clone();
        Assert.AreNotSame(clonedQuote, fullyPopulatedPqLevel1Quote);
        if (!clonedQuote.Equals(fullyPopulatedPqLevel1Quote))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                + clonedQuote.DiffQuotes(fullyPopulatedPqLevel1Quote) + "'");
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, clonedQuote);

        var cloned2 = (PQPublishableLevel1Quote)((ICloneable)fullyPopulatedPqLevel1Quote).Clone();
        Assert.AreNotSame(cloned2, fullyPopulatedPqLevel1Quote);
        if (!cloned2.Equals(fullyPopulatedPqLevel1Quote))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                + cloned2.DiffQuotes(fullyPopulatedPqLevel1Quote) + "'");
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, cloned2);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQPublishableLevel1Quote)((ICloneable<IPublishableTickInstant>)fullyPopulatedPqLevel1Quote).Clone();
        // by default SourceTickerInfo is shared
        fullyPopulatedClone.SourceTickerInfo
            = new PQSourceTickerInfo(fullyPopulatedPqLevel1Quote.SourceTickerInfo!);
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedPqLevel1Quote, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedPqLevel1Quote, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, fullyPopulatedPqLevel1Quote);
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, ((ICloneable)fullyPopulatedPqLevel1Quote).Clone());
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, ((ICloneable<IPublishableTickInstant>)fullyPopulatedPqLevel1Quote).Clone());
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, ((ICloneable<IPublishableLevel1Quote>)fullyPopulatedPqLevel1Quote).Clone());
    }

    [TestMethod]
    public void EmptyQuote_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = emptyQuote.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = fullyPopulatedPqLevel1Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
        (bool exactComparison, PQPublishableLevel1Quote original, PQPublishableLevel1Quote changingLevel1Quote)
    {
        PQTickInstantTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingLevel1Quote);

        PQCandleTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, (PQCandle)original.ConflatedTicksCandle!, (PQCandle)changingLevel1Quote.ConflatedTicksCandle!);

        if (original.GetType() == typeof(PQPublishableLevel1Quote))
            Assert.AreEqual
                (!exactComparison, changingLevel1Quote.AreEquivalent(new PublishableLevel1PriceQuote(original), exactComparison));

        changingLevel1Quote.SourceBidTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.SourceBidTime = original.SourceBidTime;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.SourceAskTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.IsFalse(changingLevel1Quote.AreEquivalent(original, exactComparison));
        changingLevel1Quote.SourceAskTime = original.SourceAskTime;
        Assert.IsTrue(original.AreEquivalent(changingLevel1Quote, exactComparison));

        changingLevel1Quote.AdapterReceivedTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.AdapterReceivedTime = original.AdapterReceivedTime;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.AdapterSentTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.AreEqual(!exactComparison, changingLevel1Quote.AreEquivalent(original, exactComparison));
        changingLevel1Quote.AdapterSentTime = original.AdapterSentTime;
        Assert.IsTrue(original.AreEquivalent(changingLevel1Quote, exactComparison));

        changingLevel1Quote.BidPriceTop = 9.8765432m;
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.BidPriceTop          = original.BidPriceTop;
        changingLevel1Quote.IsBidPriceTopUpdated = original.IsBidPriceTopUpdated;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.AskPriceTop = 9.8765432m;
        Assert.IsFalse(changingLevel1Quote.AreEquivalent(original, exactComparison));
        changingLevel1Quote.AskPriceTop          = original.AskPriceTop;
        changingLevel1Quote.IsAskPriceTopUpdated = original.IsAskPriceTopUpdated;
        Assert.IsTrue(original.AreEquivalent(changingLevel1Quote, exactComparison));

        changingLevel1Quote.Executable = !changingLevel1Quote.Executable;
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.Executable          = original.Executable;
        changingLevel1Quote.IsExecutableUpdated = original.IsExecutableUpdated;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.IsAskPriceTopChanged = !changingLevel1Quote.IsAskPriceTopChanged;
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.IsAskPriceTopChanged = original.IsAskPriceTopChanged;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.IsBidPriceTopChanged = !changingLevel1Quote.IsBidPriceTopChanged;
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.IsBidPriceTopChanged = original.IsBidPriceTopChanged;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.IsExecutableUpdated = !changingLevel1Quote.IsExecutableUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.IsExecutableUpdated = original.IsExecutableUpdated;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));
    }

    public static void AssertContainsAllLevel1Fields
    (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates, PQPublishableLevel1Quote l1Q
      , PQQuoteBooleanValues expectedQuoteBooleanFlags = PQQuoteBooleanValuesExtensions.LivePricingFieldsSetNoReplayOrSnapshots)
    {
        var priceScale = precisionSettings.PriceScalingPrecision;
        PQCandleTests.AssertCandleContainsAllFields(precisionSettings, checkFieldUpdates, l1Q.ConflatedTicksCandle!
                                                                     , PQFeedFields.CandleConflationSummary);
        
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteBooleanFlags, (uint)expectedQuoteBooleanFlags),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteBooleanFlags),
                        $"For {l1Q.GetType().Name} and {l1Q.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        PQTickInstantTests.AssertContainsAllTickInstantFields(precisionSettings, checkFieldUpdates, l1Q, expectedQuoteBooleanFlags);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.SourceQuoteBidDateTime, l1Q.SourceBidTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.SourceQuoteBidDateTime));
        var flag = l1Q.SourceBidTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.SourceQuoteBidSub2MinTime, value, flag),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.SourceQuoteBidSub2MinTime));
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.SourceQuoteAskDateTime, l1Q.SourceAskTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.SourceQuoteAskDateTime));
        flag = l1Q.SourceAskTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
        Assert.AreEqual
            (new PQFieldUpdate(PQFeedFields.SourceQuoteAskSub2MinTime, value, flag)
           , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.SourceQuoteAskSub2MinTime));
        Assert.AreEqual
            (new PQFieldUpdate(PQFeedFields.AdapterReceivedDateTime, l1Q.AdapterReceivedTime.Get2MinIntervalsFromUnixEpoch()),
             PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.AdapterReceivedDateTime));
        flag = l1Q.AdapterReceivedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.AdapterReceivedSub2MinTime, value, flag),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.AdapterReceivedSub2MinTime));
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.AdapterSentDateTime, l1Q.AdapterSentTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.AdapterSentDateTime));
        flag = l1Q.AdapterSentTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.AdapterSentSub2MinTime, value, flag),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.AdapterSentSub2MinTime));
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, PQScaling.Scale(l1Q.BidPriceTop, priceScale), priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerPrice, priceScale));
        Assert.AreEqual
            (new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, PQDepthKey.AskSide, PQScaling.Scale(l1Q.AskPriceTop, priceScale), priceScale)
           , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerPrice, PQDepthKey.AskSide, priceScale));
    }
}
