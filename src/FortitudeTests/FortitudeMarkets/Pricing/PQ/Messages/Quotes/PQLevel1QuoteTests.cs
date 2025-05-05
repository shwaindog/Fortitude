// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.PQ.Summaries;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeMarkets.Pricing.TimeSeries;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Summaries;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;
using static FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates.PQBooleanValues;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQLevel1QuoteTests
{
    private ISourceTickerInfo blankSourceTickerInfo = null!;

    private PQLevel1Quote emptyQuote                  = null!;
    private PQLevel1Quote fullyPopulatedPqLevel1Quote = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private PQSourceTickerInfo            sourceTickerInfo              = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        sourceTickerInfo =
            new PQSourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.0000001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        blankSourceTickerInfo       = new SourceTickerInfo(0, "", 0, "", Level1Quote, Unknown);
        fullyPopulatedPqLevel1Quote = new PQLevel1Quote(new PQSourceTickerInfo(sourceTickerInfo));
        emptyQuote                  = new PQLevel1Quote(new PQSourceTickerInfo(sourceTickerInfo)) { HasUpdates = false };
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
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceAskTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceAskTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceAskTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceAskTime);
        var sourceAskUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
            where update.Id is >= PQQuoteFields.SourceAskDateTime and <= PQQuoteFields.SourceAskSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var extended            = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourComponent);
        var expectedHour        = new PQFieldUpdate(PQQuoteFields.SourceAskDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQQuoteFields.SourceAskSub2MinTime, subHourComponent, extended);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceAskUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
            where update.Id is >= PQQuoteFields.SourceAskDateTime and <= PQQuoteFields.SourceAskSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        emptyQuote.IsSourceAskTimeDateUpdated    = false;
        emptyQuote.IsSourceAskTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceAskTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());


        var newEmpty = new PQLevel1Quote(sourceTickerInfo);
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
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceBidTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceBidTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceBidTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceBidTime);
        var sourceBidUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
            where update.Id is >= PQQuoteFields.SourceBidDateTime and <= PQQuoteFields.SourceBidSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceBidUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var extended            = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourComponent);
        var expectedHour        = new PQFieldUpdate(PQQuoteFields.SourceBidDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQQuoteFields.SourceBidSub2MinTime, subHourComponent, extended);
        Assert.AreEqual(expectedHour, sourceBidUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceBidUpdates[1]);

        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceBidUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
            where update.Id is >= PQQuoteFields.SourceBidDateTime and <= PQQuoteFields.SourceBidSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceBidUpdates.Count);
        Assert.AreEqual(expectedHour, sourceBidUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceBidUpdates[1]);

        emptyQuote.IsSourceBidTimeDateUpdated    = false;
        emptyQuote.IsSourceBidTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceBidTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var newEmpty = new PQLevel1Quote(sourceTickerInfo);
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
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.AdapterSentTime = expectedSetTime;
        emptyQuote.IsReplay        = true;
        Assert.IsTrue(emptyQuote.IsAdapterSentTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsAdapterSentTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.AdapterSentTime);
        var adapterSentUpdates = emptyQuote.GetDeltaUpdateFields(expectedSetTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, adapterSentUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var subHourComponent    = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerBytes);
        var expectedHour        = new PQFieldUpdate(PQQuoteFields.AdapterSentDateTime, hoursSinceUnixEpoch);
        var expectedSubHour = new PQFieldUpdate
            (PQQuoteFields.AdapterSentSub2MinTime, lowerBytes, subHourComponent);
        Assert.AreEqual(expectedHour, adapterSentUpdates[1]);
        Assert.AreEqual(expectedSubHour, adapterSentUpdates[2]);

        emptyQuote.IsAdapterSentTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        adapterSentUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(2, adapterSentUpdates.Count);
        Assert.AreEqual(expectedSubHour, adapterSentUpdates[1]);

        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeSub2MinUpdated);
        emptyQuote.IsReplay        = false;
        emptyQuote.IsReplayUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        adapterSentUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id is >= PQQuoteFields.AdapterSentDateTime and <= PQQuoteFields.AdapterSentSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, adapterSentUpdates.Count);
        hoursSinceUnixEpoch = testDateTime.Get2MinIntervalsFromUnixEpoch();
        expectedHour        = new PQFieldUpdate(PQQuoteFields.AdapterSentDateTime, hoursSinceUnixEpoch);
        subHourComponent    = testDateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out lowerBytes);
        expectedSubHour     = new PQFieldUpdate(PQQuoteFields.AdapterSentSub2MinTime, lowerBytes, subHourComponent);
        Assert.AreEqual(expectedHour, adapterSentUpdates[0]);
        Assert.AreEqual(expectedSubHour, adapterSentUpdates[1]);

        var newEmpty = new PQLevel1Quote(sourceTickerInfo);
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
        emptyQuote.AdapterReceivedTime = DateTime.Now;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.AdapterReceivedTime = DateTime.MinValue;
        emptyQuote.HasUpdates          = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.AdapterReceivedTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsAdapterReceivedTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsAdapterReceivedTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.AdapterReceivedTime);
        var adapterReceivedUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
            where update.Id is >= PQQuoteFields.AdapterReceivedDateTime and <= PQQuoteFields.AdapterReceivedSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, adapterReceivedUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var extended            = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourComponent);
        var expectedHour        = new PQFieldUpdate(PQQuoteFields.AdapterReceivedDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQQuoteFields.AdapterReceivedSub2MinTime, subHourComponent, extended);
        Assert.AreEqual(expectedHour, adapterReceivedUpdates[0]);
        Assert.AreEqual(expectedSubHour, adapterReceivedUpdates[1]);

        Assert.IsTrue(emptyQuote.HasUpdates);
        adapterReceivedUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
            where update.Id is >= PQQuoteFields.AdapterReceivedDateTime and <= PQQuoteFields.AdapterReceivedSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, adapterReceivedUpdates.Count);
        Assert.AreEqual(expectedHour, adapterReceivedUpdates[0]);
        Assert.AreEqual(expectedSubHour, adapterReceivedUpdates[1]);

        emptyQuote.IsAdapterReceivedTimeDateUpdated    = false;
        emptyQuote.IsAdapterReceivedTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.AdapterSentTime                 = testDateTime;
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var newEmpty = new PQLevel1Quote(sourceTickerInfo);
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
        emptyQuote.BidPriceTop = 0;
        emptyQuote.IsBidPriceTopChanged = false;
        emptyQuote.HasUpdates  = false;
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedBidPriceTop = 1.2345678m;
        var priceScale          = sourceTickerInfo.PriceScalingPrecision;
        emptyQuote.BidPriceTop = expectedBidPriceTop;
        Assert.IsTrue(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedBidPriceTop, emptyQuote.BidPriceTop);
        var sourceUpdatesWithUpdated = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(4, sourceUpdatesWithUpdated.Count);
        var expectedTopOfBookUpdatedFieldUpdate =
            new PQFieldUpdate
                (PQQuoteFields.QuoteBooleanFlags
               , (uint)(IsBidPriceTopChangedUpdatedFlag | IsBidPriceTopChangedSetFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedTopOfBookUpdatedFieldUpdate, sourceUpdatesWithUpdated[0]);
        var expectedTopOfBookPriceFieldUpdate = new PQFieldUpdate(PQQuoteFields.Price, expectedBidPriceTop, priceScale);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithUpdated[3]);

        emptyQuote.IsBidPriceTopUpdated = false;
        Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsBidPriceTopChanged            = false;
        emptyQuote.IsBidPriceTopChangedUpdated     = false;
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var sourceUpdatesWithoutUpdated = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.Price
               && update.Flag == priceScale
            select update).ToList();
        Assert.AreEqual(1, sourceUpdatesWithoutUpdated.Count);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithoutUpdated[0]);

        var newEmpty = new PQLevel1Quote(sourceTickerInfo);
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
        emptyQuote.AskPriceTop = 1m;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.AskPriceTop          = 0;
        emptyQuote.IsAskPriceTopChanged = false;
        emptyQuote.HasUpdates           = false;
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        var deltaUpdateFields = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update);
        Assert.AreEqual(2, deltaUpdateFields.Count());

        var expectedAskPriceTop = 1.2345678m;
        var priceScale          = sourceTickerInfo.PriceScalingPrecision;
        emptyQuote.AskPriceTop = expectedAskPriceTop;
        Assert.IsTrue(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedAskPriceTop, emptyQuote.AskPriceTop);
        var sourceUpdatesWithUpdated = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(4, sourceUpdatesWithUpdated.Count);
        var expectedTopOfBookUpdatedFieldUpdate =
            new PQFieldUpdate
                (PQQuoteFields.QuoteBooleanFlags, (uint)(IsAskPriceTopChangedUpdatedFlag | IsAskPriceTopChangedSetFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedTopOfBookUpdatedFieldUpdate, sourceUpdatesWithUpdated[0]);
        var expectedTopOfBookPriceFieldUpdate =
            new PQFieldUpdate
                (PQQuoteFields.Price, PQDepthKey.AskSide, expectedAskPriceTop, priceScale);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithUpdated[3]);

        emptyQuote.IsAskPriceTopUpdated = false;
        Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);

        emptyQuote.IsAskPriceTopChanged            = false;
        emptyQuote.IsAskPriceTopChangedUpdated     = false;
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var allUpdates =
            (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
                select update).ToList();
        var sourceUpdatesWithoutUpdated = (from update in allUpdates
            where update is { Id: PQQuoteFields.Price, DepthId: PQDepthKey.AskSide }
               && update.Flag == (priceScale | PQFieldFlags.IncludesDepth)
            select update).ToList();
        Assert.AreEqual(1, sourceUpdatesWithoutUpdated.Count, $"Where all updates is \n[{string.Join(",\n", allUpdates)}]");
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithoutUpdated[0]);

        var newEmpty = new PQLevel1Quote(sourceTickerInfo);
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
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const bool expectedExecutable = false;
        emptyQuote.Executable = expectedExecutable;
        Assert.IsTrue(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedExecutable, emptyQuote.Executable);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate
            (PQQuoteFields.QuoteBooleanFlags, (uint)IsExecutableUpdatedFlag);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsExecutableUpdated = false;
        Assert.IsFalse(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyQuote.Executable = true;
        Assert.IsTrue(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(true, emptyQuote.Executable);
        sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate
            (PQQuoteFields.QuoteBooleanFlags, (uint)(IsExecutableUpdatedFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
        expectedFieldUpdate = new PQFieldUpdate
            (PQQuoteFields.QuoteBooleanFlags, (uint)(IsExecutableUpdatedFlag | IsExecutableSetFlag | PQBooleanValuesExtensions.AllUpdated));

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.QuoteBooleanFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQLevel1Quote(sourceTickerInfo)
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
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        emptyQuote.SourceTime = DateTime.Now;
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.UpdateComplete();
        emptyQuote.SourceTime = DateTime.MinValue;
        emptyQuote.HasUpdates = false;
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyQuote.IsReplay       = true;
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

        emptyQuote.ResetFields();

        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(false, emptyQuote.IsReplay);
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
            (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllLevel1Fields
            ((PQSourceTickerInfo)fullyPopulatedPqLevel1Quote.SourceTickerInfo!, pqFieldUpdates, fullyPopulatedPqLevel1Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel1Fields()
    {
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields
            (new DateTime(2017, 11, 04, 15, 33, 5), StorageFlags.Snapshot).ToList();
        AssertContainsAllLevel1Fields
            ((PQSourceTickerInfo)fullyPopulatedPqLevel1Quote.SourceTickerInfo!, pqFieldUpdates, fullyPopulatedPqLevel1Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        fullyPopulatedPqLevel1Quote.IsReplay   = true;
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields
            (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields
            (new DateTime(2017, 11, 04, 13, 33, 3), StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQLevel1Quote(sourceTickerInfo)
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
        emptyQuote = new PQLevel1Quote(blankSourceTickerInfo);
        emptyQuote.CopyFrom(fullyPopulatedPqLevel1Quote, CopyMergeFlags.FullReplace);

        Assert.AreEqual(fullyPopulatedPqLevel1Quote, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptyQuote                             = new PQLevel1Quote(blankSourceTickerInfo);
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        emptyQuote.CopyFrom(fullyPopulatedPqLevel1Quote);
        Assert.AreEqual(fullyPopulatedPqLevel1Quote.PQSequenceId, emptyQuote.PQSequenceId);
        Assert.AreEqual(default, emptyQuote.SourceTime);
        Assert.IsFalse
            (fullyPopulatedPqLevel1Quote.SourceTickerInfo!.AreEquivalent(emptyQuote.SourceTickerInfo));
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
        Assert.AreEqual(default, emptyQuote.SourceBidTime);
        Assert.AreEqual(default, emptyQuote.SourceAskTime);
        Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(default, emptyQuote.AdapterSentTime);
        Assert.AreEqual(default, emptyQuote.ClientReceivedTime);
        Assert.AreEqual(default, emptyQuote.DispatchedTime);
        Assert.AreEqual(default, emptyQuote.ProcessedTime);
        Assert.AreEqual(default, emptyQuote.SocketReceivingTime);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.IsTrue(emptyQuote.Executable);
        Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
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
        var nonPQLevel1Quote = new Level1PriceQuote(fullyPopulatedPqLevel1Quote);
        emptyQuote.CopyFrom(nonPQLevel1Quote);
        Assert.IsTrue(fullyPopulatedPqLevel1Quote.AreEquivalent(emptyQuote));
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<ITickInstant>)fullyPopulatedPqLevel1Quote).Clone();
        Assert.AreNotSame(clonedQuote, fullyPopulatedPqLevel1Quote);
        if (!clonedQuote.Equals(fullyPopulatedPqLevel1Quote))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                + clonedQuote.DiffQuotes(fullyPopulatedPqLevel1Quote) + "'");
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, clonedQuote);

        var cloned2 = (PQLevel1Quote)((ICloneable)fullyPopulatedPqLevel1Quote).Clone();
        Assert.AreNotSame(cloned2, fullyPopulatedPqLevel1Quote);
        if (!cloned2.Equals(fullyPopulatedPqLevel1Quote))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                + cloned2.DiffQuotes(fullyPopulatedPqLevel1Quote) + "'");
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, cloned2);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQLevel1Quote)((ICloneable<ITickInstant>)fullyPopulatedPqLevel1Quote).Clone();
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
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, ((ICloneable<ITickInstant>)fullyPopulatedPqLevel1Quote).Clone());
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, ((ICloneable<ILevel1Quote>)fullyPopulatedPqLevel1Quote).Clone());
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
        (bool exactComparison, PQLevel1Quote original, PQLevel1Quote changingLevel1Quote)
    {
        PQTickInstantTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingLevel1Quote);

        PQPricePeriodSummaryTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, (PQPricePeriodSummary)original.SummaryPeriod!, (PQPricePeriodSummary)changingLevel1Quote.SummaryPeriod!);

        if (original.GetType() == typeof(PQLevel1Quote))
            Assert.AreEqual
                (!exactComparison, changingLevel1Quote.AreEquivalent(new Level1PriceQuote(original), exactComparison));

        changingLevel1Quote.SourceBidTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.SourceBidTime = original.SourceBidTime;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.SourceAskTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.IsFalse(changingLevel1Quote.AreEquivalent(original, exactComparison));
        changingLevel1Quote.SourceAskTime = original.SourceAskTime;
        Assert.IsTrue(original.AreEquivalent(changingLevel1Quote, exactComparison));

        changingLevel1Quote.AdapterReceivedTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.AdapterReceivedTime = original.AdapterReceivedTime;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.AdapterSentTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.IsFalse(changingLevel1Quote.AreEquivalent(original, exactComparison));
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
    (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates, PQLevel1Quote l1Q
      , PQBooleanValues expectedBooleanFlags = PQBooleanValuesExtensions.AllFields)
    {
        var priceScale = precisionSettings.PriceScalingPrecision;
        PQPricePeriodSummaryTests.AssertPeriodSummaryContainsAllFields(precisionSettings, checkFieldUpdates, l1Q.SummaryPeriod!);

        PQTickInstantTests.AssertContainsAllTickInstantFields(precisionSettings, checkFieldUpdates, l1Q, expectedBooleanFlags);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SourceBidDateTime, l1Q.SourceBidTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SourceBidDateTime));
        var flag = l1Q.SourceBidTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SourceBidSub2MinTime, value, flag),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SourceBidSub2MinTime));
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SourceAskDateTime, l1Q.SourceAskTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SourceAskDateTime));
        flag = l1Q.SourceAskTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
        Assert.AreEqual
            (new PQFieldUpdate(PQQuoteFields.SourceAskSub2MinTime, value, flag)
           , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SourceAskSub2MinTime));
        Assert.AreEqual
            (new PQFieldUpdate(PQQuoteFields.AdapterReceivedDateTime, l1Q.AdapterReceivedTime.Get2MinIntervalsFromUnixEpoch()),
             PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.AdapterReceivedDateTime));
        flag = l1Q.AdapterReceivedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.AdapterReceivedSub2MinTime, value, flag),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.AdapterReceivedSub2MinTime));
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.AdapterSentDateTime, l1Q.AdapterSentTime.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.AdapterSentDateTime));
        flag = l1Q.AdapterSentTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.AdapterSentSub2MinTime, value, flag),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.AdapterSentSub2MinTime));
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.Price, PQScaling.Scale(l1Q.BidPriceTop, priceScale), priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.Price, priceScale));
        Assert.AreEqual
            (new PQFieldUpdate(PQQuoteFields.Price, PQDepthKey.AskSide, PQScaling.Scale(l1Q.AskPriceTop, priceScale), priceScale)
           , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.Price, PQDepthKey.AskSide, priceScale));
    }

    internal class DummyLevel1Quote : PQTickInstantTests.DummyPQTickInstant, IPQLevel1Quote
    {
        public          bool              IsBidPriceTopChangedUpdated { get; set; }
        public          bool              IsAskPriceTopChangedUpdated { get; set; }
        public override TickerDetailLevel TickerDetailLevel           => Level1Quote;

        IMutablePricePeriodSummary? IMutableLevel1Quote.SummaryPeriod
        {
            get => SummaryPeriod;
            set => SummaryPeriod = value as IPQPricePeriodSummary;
        }

        public DateTime AdapterReceivedTime
        {
            get => DateTime.Now;
            set { }
        }

        IPricePeriodSummary? ILevel1Quote.SummaryPeriod => SummaryPeriod;
        public IPQPricePeriodSummary?     SummaryPeriod { get; set; }

        public BidAskPair BidAskTop => new(BidPriceTop, AskPriceTop);

        public DateTime AdapterSentTime { get; set; }
        public DateTime SourceBidTime   { get; set; }
        public decimal  BidPriceTop     { get; set; }
        public DateTime SourceAskTime   { get; set; }
        public decimal  AskPriceTop     { get; set; }
        public bool     Executable      { get; set; }

        public bool IsAskPriceTopChanged       { get; set; }
        public bool IsBidPriceTopChanged       { get; set; }
        public bool IsSourceAskTimeDateUpdated { get; set; }

        public bool IsSourceAskTimeSub2MinUpdated { get; set; }
        public bool IsSourceBidTimeDateUpdated    { get; set; }
        public bool IsSourceBidTimeSub2MinUpdated { get; set; }
        public bool IsAdapterSentTimeDateUpdated  { get; set; }

        public bool IsAdapterSentTimeSub2MinUpdated     { get; set; }
        public bool IsAdapterReceivedTimeDateUpdated    { get; set; }
        public bool IsAdapterReceivedTimeSub2MinUpdated { get; set; }

        public bool IsBidPriceTopUpdated { get; set; }
        public bool IsAskPriceTopUpdated { get; set; }
        public bool IsExecutableUpdated  { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo   { get; set; }

        public bool IsValidFromTimeDateUpdated    { get; set; }
        public bool IsValidFromTimeSub2MinUpdated { get; set; }
        public bool IsValidToTimeDateUpdated      { get; set; }
        public bool IsValidToTimeSub2MinUpdated   { get; set; }

        IMutableLevel1Quote IMutableLevel1Quote.Clone() => (IMutableLevel1Quote)Clone();
        IPQLevel1Quote IPQLevel1Quote.          Clone() => this;
        ILevel1Quote ILevel1Quote.              Clone() => this;
        ILevel1Quote ICloneable<ILevel1Quote>.  Clone() => this;

        ILevel1Quote? IDoublyLinkedListNode<ILevel1Quote>.    Previous { get; set; }
        ILevel1Quote? IDoublyLinkedListNode<ILevel1Quote>.    Next     { get; set; }
        IPQLevel1Quote? IDoublyLinkedListNode<IPQLevel1Quote>.Previous { get; set; }
        IPQLevel1Quote? IDoublyLinkedListNode<IPQLevel1Quote>.Next     { get; set; }

        public decimal BidPrice => BidPriceTop;
        public decimal AskPrice => AskPriceTop;

        IReusableObject<IBidAskInstant> ITransferState<IReusableObject<IBidAskInstant>>.CopyFrom
            (IReusableObject<IBidAskInstant> source, CopyMergeFlags copyMergeFlags) =>
            this;

        public override IPQLevel1Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => this;

        IBidAskInstant ICloneable<IBidAskInstant>.Clone() => (ILevel1Quote)Clone();

        IBidAskInstant ITransferState<IBidAskInstant>.CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags) => this;

        public bool AreEquivalent(IBidAskInstant? other, bool exactTypes = false) => throw new NotImplementedException();

        IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Previous { get; set; }
        IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Next     { get; set; }

        public DateTime AtTime => SourceTime;

        ILevel1Quote? ILevel1Quote.    Next     { get; set; }
        ILevel1Quote? ILevel1Quote.    Previous { get; set; }
        IPQLevel1Quote? IPQLevel1Quote.Next     { get; set; }
        IPQLevel1Quote? IPQLevel1Quote.Previous { get; set; }

        public DateTime StorageTime(IStorageTimeResolver<ILevel1Quote>? resolver = null)
        {
            resolver ??= QuoteStorageTimeResolver.Instance;
            return resolver.ResolveStorageTime(this);
        }
    }
}
