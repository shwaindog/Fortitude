// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates.PQBooleanValues;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQLevel1QuoteTests
{
    private ISourceTickerQuoteInfo        blankSourceTickerQuoteInfo    = null!;
    private PQLevel1Quote                 emptyQuote                    = null!;
    private PQLevel1Quote                 fullyPopulatedPqLevel1Quote   = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private ISourceTickerQuoteInfo        sourceTickerQuoteInfo         = null!;
    private DateTime                      testDateTime;

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
        blankSourceTickerQuoteInfo  = new SourceTickerQuoteInfo(0, "", 0, "", QuoteLevel.Level1);
        emptyQuote                  = new PQLevel1Quote(sourceTickerQuoteInfo) { HasUpdates = false };
        fullyPopulatedPqLevel1Quote = new PQLevel1Quote(sourceTickerQuoteInfo);
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
        Assert.IsFalse(emptyQuote.IsSourceAskTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceAskTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceAskTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceAskTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceAskTimeSubHourUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceAskTime);
        var sourceAskUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(4, sourceAskUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var subHourComponent    = expectedSetTime.GetSubHourComponent();
        var expectedHour        = new PQFieldUpdate(PQFieldKeys.SourceAskDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQFieldKeys.SourceAskSubHourTime, subHourComponent, 15);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        emptyQuote.IsSourceAskTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceAskUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, sourceAskUpdates.Count);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[0]);

        emptyQuote.IsSourceAskTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceAskTimeSubHourUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceAskUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id >= PQFieldKeys.SourceAskDateTime && update.Id <=
                PQFieldKeys.SourceAskSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceAskUpdates.Count);
        Assert.AreEqual(expectedHour, sourceAskUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceAskUpdates[1]);

        var newEmpty = new PQLevel1Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(sourceAskUpdates[0]);
        newEmpty.UpdateField(sourceAskUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.SourceAskTime);
        Assert.IsTrue(newEmpty.IsSourceAskTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsSourceAskTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptyQuote_SourceBidTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceBidTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceBidTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceBidTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceBidTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceBidTimeSubHourUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceBidTime);
        var sourceBidUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(4, sourceBidUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var subHourComponent    = expectedSetTime.GetSubHourComponent();
        var expectedHour        = new PQFieldUpdate(PQFieldKeys.SourceBidDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQFieldKeys.SourceBidSubHourTime, subHourComponent, 15);
        Assert.AreEqual(expectedHour, sourceBidUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceBidUpdates[1]);

        emptyQuote.IsSourceBidTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceBidUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, sourceBidUpdates.Count);
        Assert.AreEqual(expectedSubHour, sourceBidUpdates[0]);

        emptyQuote.IsSourceBidTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceBidTimeSubHourUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceBidUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id >= PQFieldKeys.SourceBidDateTime && update.Id <=
                PQFieldKeys.SourceBidSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceBidUpdates.Count);
        Assert.AreEqual(expectedHour, sourceBidUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceBidUpdates[1]);

        var newEmpty = new PQLevel1Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(sourceBidUpdates[0]);
        newEmpty.UpdateField(sourceBidUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.SourceBidTime);
        Assert.IsTrue(newEmpty.IsSourceBidTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsSourceBidTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptyQuote_AdapterSentTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterSentTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.AdapterSentTime = expectedSetTime;
        emptyQuote.IsReplay        = true;
        Assert.IsTrue(emptyQuote.IsAdapterSentTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsAdapterSentTimeSubHourUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.AdapterSentTime);
        var adapterSentUpdates = emptyQuote.GetDeltaUpdateFields(expectedSetTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, adapterSentUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var subHourComponent    = expectedSetTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerBytes);
        var expectedHour        = new PQFieldUpdate(PQFieldKeys.AdapterSentDateTime, hoursSinceUnixEpoch);
        var expectedSubHour = new PQFieldUpdate(PQFieldKeys.AdapterSentSubHourTime, lowerBytes,
                                                subHourComponent);
        Assert.AreEqual(expectedHour, adapterSentUpdates[1]);
        Assert.AreEqual(expectedSubHour, adapterSentUpdates[2]);

        emptyQuote.IsAdapterSentTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        adapterSentUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(2, adapterSentUpdates.Count);
        Assert.AreEqual(expectedSubHour, adapterSentUpdates[1]);

        emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeSubHourUpdated);
        emptyQuote.IsReplay        = false;
        emptyQuote.IsReplayUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        adapterSentUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id >= PQFieldKeys.AdapterSentDateTime && update.Id <=
                PQFieldKeys.AdapterSentSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, adapterSentUpdates.Count);
        hoursSinceUnixEpoch = testDateTime.GetHoursFromUnixEpoch();
        expectedHour        = new PQFieldUpdate(PQFieldKeys.AdapterSentDateTime, hoursSinceUnixEpoch);
        subHourComponent    = testDateTime.GetSubHourComponent().BreakLongToByteAndUint(out lowerBytes);
        expectedSubHour     = new PQFieldUpdate(PQFieldKeys.AdapterSentSubHourTime, lowerBytes, subHourComponent);
        Assert.AreEqual(expectedHour, adapterSentUpdates[0]);
        Assert.AreEqual(expectedSubHour, adapterSentUpdates[1]);

        var newEmpty = new PQLevel1Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(adapterSentUpdates[0]);
        newEmpty.UpdateField(adapterSentUpdates[1]);
        Assert.AreEqual(testDateTime, newEmpty.AdapterSentTime);
        Assert.IsTrue(newEmpty.IsAdapterSentTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsAdapterSentTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptyQuote_AdapterReceivedTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.AdapterReceivedTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsAdapterReceivedTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsAdapterReceivedTimeSubHourUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.AdapterReceivedTime);
        var adapterReceivedUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(4, adapterReceivedUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var subHourComponent    = expectedSetTime.GetSubHourComponent();
        var expectedHour        = new PQFieldUpdate(PQFieldKeys.AdapterReceivedDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQFieldKeys.AdapterReceivedSubHourTime, subHourComponent, 15);
        Assert.AreEqual(expectedHour, adapterReceivedUpdates[0]);
        Assert.AreEqual(expectedSubHour, adapterReceivedUpdates[1]);

        emptyQuote.IsAdapterReceivedTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        adapterReceivedUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, adapterReceivedUpdates.Count);
        Assert.AreEqual(expectedSubHour, adapterReceivedUpdates[0]);

        emptyQuote.IsAdapterReceivedTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSubHourUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        adapterReceivedUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id >= PQFieldKeys.AdapterReceivedDateTime && update.Id <=
                PQFieldKeys.AdapterReceivedSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, adapterReceivedUpdates.Count);
        Assert.AreEqual(expectedHour, adapterReceivedUpdates[0]);
        Assert.AreEqual(expectedSubHour, adapterReceivedUpdates[1]);

        var newEmpty = new PQLevel1Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(adapterReceivedUpdates[0]);
        newEmpty.UpdateField(adapterReceivedUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.AdapterReceivedTime);
        Assert.IsTrue(newEmpty.IsAdapterReceivedTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsAdapterReceivedTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptyQuote_BidPriceTopChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedBidPriceTop = 1.2345678m;
        emptyQuote.BidPriceTop = expectedBidPriceTop;
        Assert.IsTrue(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedBidPriceTop, emptyQuote.BidPriceTop);
        var sourceUpdatesWithUpdated = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(4, sourceUpdatesWithUpdated.Count);
        var expectedTopOfBookUpdatedFieldUpdate =
            new PQFieldUpdate
                (PQFieldKeys.QuoteBooleanFlags
               , (uint)(IsBidPriceTopUpdatedChangedFlag | IsBidPriceTopUpdatedSetFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedTopOfBookUpdatedFieldUpdate, sourceUpdatesWithUpdated[0]);
        var expectedTopOfBookPriceFieldUpdate = new PQFieldUpdate(PQFieldKeys.BidAskTopOfBookPrice, expectedBidPriceTop, 1);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithUpdated[3]);

        emptyQuote.IsBidPriceTopUpdated = false;
        Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsBidPriceTopUpdatedChanged     = false;
        emptyQuote.IsBidPriceTopChanged            = false;
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var sourceUpdatesWithoutUpdated = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.BidAskTopOfBookPrice
               && update.Flag == 1
            select update).ToList();
        Assert.AreEqual(1, sourceUpdatesWithoutUpdated.Count);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithoutUpdated[0]);

        var newEmpty = new PQLevel1Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(sourceUpdatesWithoutUpdated[0]);
        Assert.AreEqual(expectedBidPriceTop, newEmpty.BidPriceTop);
        Assert.IsFalse(newEmpty.IsBidPriceTopUpdated);
        newEmpty.UpdateField(expectedTopOfBookUpdatedFieldUpdate);
        Assert.IsTrue(newEmpty.IsBidPriceTopUpdated);
    }

    [TestMethod]
    public void EmptyQuote_AskPriceTopChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedAskPriceTop = 1.2345678m;
        emptyQuote.AskPriceTop = expectedAskPriceTop;
        Assert.IsTrue(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedAskPriceTop, emptyQuote.AskPriceTop);
        var sourceUpdatesWithUpdated = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(4, sourceUpdatesWithUpdated.Count);
        var expectedTopOfBookUpdatedFieldUpdate =
            new PQFieldUpdate
                (PQFieldKeys.QuoteBooleanFlags
               , (uint)(IsAskPriceTopUpdatedChangedFlag | IsAskPriceTopUpdatedSetFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedTopOfBookUpdatedFieldUpdate, sourceUpdatesWithUpdated[0]);
        var expectedTopOfBookPriceFieldUpdate = new PQFieldUpdate(PQFieldKeys.BidAskTopOfBookPrice, expectedAskPriceTop,
                                                                  1 | PQFieldFlags.IsAskSideFlag);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithUpdated[3]);

        emptyQuote.IsAskPriceTopUpdated = false;
        Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);

        emptyQuote.IsAskPriceTopUpdatedChanged     = false;
        emptyQuote.IsAskPriceTopChanged            = false;
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var sourceUpdatesWithoutUpdated = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.BidAskTopOfBookPrice
               && update.Flag == (1 | PQFieldFlags.IsAskSideFlag)
            select update).ToList();
        Assert.AreEqual(1, sourceUpdatesWithoutUpdated.Count);
        Assert.AreEqual(expectedTopOfBookPriceFieldUpdate, sourceUpdatesWithoutUpdated[0]);

        var newEmpty = new PQLevel1Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(sourceUpdatesWithoutUpdated[0]);
        Assert.AreEqual(expectedAskPriceTop, newEmpty.AskPriceTop);
        Assert.IsFalse(newEmpty.IsAskPriceTopUpdated);
        newEmpty.UpdateField(expectedTopOfBookUpdatedFieldUpdate);
        Assert.IsTrue(newEmpty.IsAskPriceTopUpdated);
    }

    [TestMethod]
    public void EmptyQuote_ExecutableChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsExecutableUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(true, emptyQuote.Executable);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const bool expectedExecutable = false;
        emptyQuote.Executable = expectedExecutable;
        Assert.IsTrue(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedExecutable, emptyQuote.Executable);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.QuoteBooleanFlags,
                                                    (uint)IsExecutableUpdatedFlag);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsExecutableUpdated = false;
        Assert.IsFalse(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        emptyQuote.IsAdapterSentTimeDateUpdated    = false;
        emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyQuote.Executable = true;
        Assert.IsTrue(emptyQuote.IsExecutableUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(true, emptyQuote.Executable);
        sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.QuoteBooleanFlags
                                              , (uint)(IsExecutableUpdatedFlag | IsExecutableSetFlag));
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
        expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.QuoteBooleanFlags
                                              , (uint)(IsExecutableUpdatedFlag | IsExecutableSetFlag | PQBooleanValuesExtensions.AllUpdated));

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.QuoteBooleanFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQLevel1Quote(sourceTickerQuoteInfo)
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
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyQuote.IsReplay     = true;
        emptyQuote.PQSyncStatus = PQSyncStatus.Good;
        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceTime = expectedSetTime;
        var expectedSinglePrice = 1.2345678m;
        emptyQuote.SinglePrice         = expectedSinglePrice;
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
        Assert.AreEqual(PQSyncStatus.OutOfSync, emptyQuote.PQSyncStatus);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.AreEqual(0m, emptyQuote.SinglePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceAskTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceBidTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterSentTime);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.IsTrue(emptyQuote.Executable);
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel1Fields()
    {
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields(
                                                                              new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllLevel1Fields(pqFieldUpdates, fullyPopulatedPqLevel1Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel1Fields()
    {
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields(
                                                                              new DateTime(2017, 11, 04, 15, 33, 5)
                                                                            , StorageFlags.Snapshot).ToList();
        AssertContainsAllLevel1Fields
            (pqFieldUpdates, fullyPopulatedPqLevel1Quote, PQBooleanValuesExtensions.AllFields);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        fullyPopulatedPqLevel1Quote.IsReplay   = true;
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields(
                                                                              new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        var pqFieldUpdates = fullyPopulatedPqLevel1Quote.GetDeltaUpdateFields(
                                                                              new DateTime(2017, 11, 04, 13, 33, 3)
                                                                            , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQLevel1Quote(sourceTickerQuoteInfo);
        newEmpty.PQSequenceId = fullyPopulatedPqLevel1Quote.PQSequenceId;
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        // not copied from field updates as is used in by server to track publication times.
        newEmpty.LastPublicationTime = fullyPopulatedPqLevel1Quote.LastPublicationTime;
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new PQLevel1Quote(blankSourceTickerQuoteInfo);
        emptyQuote.CopyFrom(fullyPopulatedPqLevel1Quote, CopyMergeFlags.FullReplace);

        Assert.AreEqual(fullyPopulatedPqLevel1Quote, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptyQuote                             = new PQLevel1Quote(blankSourceTickerQuoteInfo);
        fullyPopulatedPqLevel1Quote.HasUpdates = false;
        emptyQuote.CopyFrom(fullyPopulatedPqLevel1Quote);
        Assert.AreEqual(fullyPopulatedPqLevel1Quote.PQSequenceId, emptyQuote.PQSequenceId);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.IsTrue(
                      fullyPopulatedPqLevel1Quote.SourceTickerQuoteInfo!.AreEquivalent(emptyQuote.SourceTickerQuoteInfo));
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SinglePrice);
        Assert.AreEqual(PQSyncStatus.OutOfSync, emptyQuote.PQSyncStatus);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceBidTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceAskTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterSentTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ClientReceivedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.DispatchedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ProcessedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SocketReceivingTime);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.IsTrue(emptyQuote.Executable);
        Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
        Assert.IsFalse(emptyQuote.IsSinglePriceUpdated);
        Assert.IsFalse(emptyQuote.IsSyncStatusUpdated);
        Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceBidTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.IsSourceAskTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceAskTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsAdapterSentTimeSubHourUpdated);
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
        var clonedQuote = ((ICloneable<ILevel0Quote>)fullyPopulatedPqLevel1Quote).Clone();
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
        var fullyPopulatedClone = (PQLevel1Quote)((ICloneable<ILevel0Quote>)fullyPopulatedPqLevel1Quote).Clone();
        // by default SourceTickerQuoteInfo is shared
        fullyPopulatedClone.SourceTickerQuoteInfo
            = new PQSourceTickerQuoteInfo(fullyPopulatedPqLevel1Quote.SourceTickerQuoteInfo!);
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedPqLevel1Quote, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedPqLevel1Quote, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, fullyPopulatedPqLevel1Quote);
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, ((ICloneable)fullyPopulatedPqLevel1Quote).Clone());
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, ((ICloneable<ILevel0Quote>)fullyPopulatedPqLevel1Quote).Clone());
        Assert.AreEqual(fullyPopulatedPqLevel1Quote, ((ICloneable<ILevel1Quote>)fullyPopulatedPqLevel1Quote).Clone());
    }

    [TestMethod]
    public void EmptyQuote_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = emptyQuote.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison, PQLevel1Quote original,
        PQLevel1Quote changingLevel1Quote)
    {
        PQLevel0QuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison, original,
                                                                               changingLevel1Quote);

        PQPricePeriodSummaryTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
                                                                                      (PQPricePeriodSummary)original.SummaryPeriod!,
                                                                                      (PQPricePeriodSummary)changingLevel1Quote.SummaryPeriod!);

        if (original.GetType() == typeof(PQLevel1Quote))
            Assert.AreEqual(!exactComparison,
                            changingLevel1Quote.AreEquivalent(new Level1PriceQuote(original), exactComparison));

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
        changingLevel1Quote.BidPriceTop = original.BidPriceTop;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.AskPriceTop = 9.8765432m;
        Assert.IsFalse(changingLevel1Quote.AreEquivalent(original, exactComparison));
        changingLevel1Quote.AskPriceTop = original.AskPriceTop;
        Assert.IsTrue(original.AreEquivalent(changingLevel1Quote, exactComparison));

        changingLevel1Quote.Executable = !changingLevel1Quote.Executable;
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.Executable          = original.Executable;
        changingLevel1Quote.IsExecutableUpdated = original.IsExecutableUpdated;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.IsAskPriceTopUpdated = !changingLevel1Quote.IsAskPriceTopUpdated;
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.IsAskPriceTopUpdated = original.IsAskPriceTopUpdated;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.IsBidPriceTopUpdated = !changingLevel1Quote.IsBidPriceTopUpdated;
        Assert.IsFalse(original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.IsBidPriceTopUpdated = original.IsBidPriceTopUpdated;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));

        changingLevel1Quote.IsExecutableUpdated = !changingLevel1Quote.IsExecutableUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLevel1Quote, exactComparison));
        changingLevel1Quote.IsExecutableUpdated = original.IsExecutableUpdated;
        Assert.IsTrue(changingLevel1Quote.AreEquivalent(original, exactComparison));
    }

    public static void AssertContainsAllLevel1Fields(IList<PQFieldUpdate> checkFieldUpdates,
        PQLevel1Quote l1Q, PQBooleanValues expectedBooleanFlags = PQBooleanValuesExtensions.AllExceptExecutableUpdated)
    {
        PQPricePeriodSummaryTests.AssertPeriodSummaryContainsAllFields(checkFieldUpdates, l1Q.SummaryPeriod!);

        PQLevel0QuoteTests.AssertContainsAllLevel0Fields(checkFieldUpdates, l1Q, expectedBooleanFlags);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceBidDateTime, l1Q.SourceBidTime.GetHoursFromUnixEpoch()),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceBidDateTime));
        var flag = l1Q.SourceBidTime.GetSubHourComponent().BreakLongToByteAndUint(out var value);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceBidSubHourTime, value, flag),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceBidSubHourTime));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceAskDateTime, l1Q.SourceAskTime.GetHoursFromUnixEpoch()),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceAskDateTime));
        flag = l1Q.SourceAskTime.GetSubHourComponent().BreakLongToByteAndUint(out value);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceAskSubHourTime, value, flag),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceAskSubHourTime));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.AdapterReceivedDateTime, l1Q.AdapterReceivedTime
                                                                                  .GetHoursFromUnixEpoch()),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.AdapterReceivedDateTime));
        flag = l1Q.AdapterReceivedTime.GetSubHourComponent().BreakLongToByteAndUint(out value);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.AdapterReceivedSubHourTime, value, flag),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.AdapterReceivedSubHourTime));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.AdapterSentDateTime, l1Q.AdapterSentTime.GetHoursFromUnixEpoch()),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.AdapterSentDateTime));
        flag = l1Q.AdapterSentTime.GetSubHourComponent().BreakLongToByteAndUint(out value);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.AdapterSentSubHourTime, value, flag),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.AdapterSentSubHourTime));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, l1Q.BidPriceTop, 1),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.LayerPriceOffset, 1));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, l1Q.AskPriceTop, 1 |
                                                                                         PQFieldFlags.IsAskSideFlag),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.LayerPriceOffset, 1 |
                                                                    PQFieldFlags.IsAskSideFlag));
    }

    internal class DummyLevel1Quote : PQLevel0QuoteTests.DummyPQLevel0Quote, IPQLevel1Quote
    {
        public override QuoteLevel QuoteLevel => QuoteLevel.Level1;

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

        IPricePeriodSummary? ILevel1Quote.      SummaryPeriod => SummaryPeriod;
        public IPQPricePeriodSummary?           SummaryPeriod { get; set; }
        IMutableLevel1Quote IMutableLevel1Quote.Clone()       => (IMutableLevel1Quote)Clone();
        IPQLevel1Quote IPQLevel1Quote.          Clone()       => this;
        ILevel1Quote ILevel1Quote.              Clone()       => this;
        ILevel1Quote ICloneable<ILevel1Quote>.  Clone()       => this;

        public DateTime AdapterSentTime { get; set; }
        public DateTime SourceBidTime   { get; set; }
        public decimal  BidPriceTop     { get; set; }
        public DateTime SourceAskTime   { get; set; }
        public decimal  AskPriceTop     { get; set; }

        public bool Executable           { get; set; }
        public bool IsAskPriceTopUpdated { get; set; }
        public bool IsBidPriceTopUpdated { get; set; }

        public bool IsBidPriceTopUpdatedChanged         { get; set; }
        public bool IsAskPriceTopUpdatedChanged         { get; set; }
        public bool IsSourceAskTimeDateUpdated          { get; set; }
        public bool IsSourceAskTimeSubHourUpdated       { get; set; }
        public bool IsSourceBidTimeDateUpdated          { get; set; }
        public bool IsSourceBidTimeSubHourUpdated       { get; set; }
        public bool IsAdapterSentTimeDateUpdated        { get; set; }
        public bool IsAdapterSentTimeSubHourUpdated     { get; set; }
        public bool IsAdapterReceivedTimeDateUpdated    { get; set; }
        public bool IsAdapterReceivedTimeSubHourUpdated { get; set; }
        public bool IsBidPriceTopChanged                { get; set; }
        public bool IsAskPriceTopChanged                { get; set; }
        public bool IsExecutableUpdated                 { get; set; }

        public DateTime StorageTime(IStorageTimeResolver<ILevel1Quote>? resolver = null)
        {
            resolver ??= QuoteStorageTimeResolver.Instance;
            return resolver.ResolveStorageTime(this);
        }
    }
}
