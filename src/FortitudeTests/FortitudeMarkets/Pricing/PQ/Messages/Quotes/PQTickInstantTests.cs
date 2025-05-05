// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.TimeSeries;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQTickInstantTests
{
    private ISourceTickerInfo blankSourceTickerInfo = null!;

    private PQTickInstant emptyQuote                  = null!;
    private PQTickInstant fullyPopulatedPQTickInstant = null!;
    private PQTickInstant newlyPopulatedPQTickInstant = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private PQSourceTickerInfo sourceTickerInfo = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        sourceTickerInfo =
            new PQSourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, FxMajor
               , 20, 0.0000001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        blankSourceTickerInfo       = new SourceTickerInfo(0, "", 0, "", Level1Quote, Unknown);
        fullyPopulatedPQTickInstant = new PQTickInstant(new PQSourceTickerInfo(sourceTickerInfo));
        emptyQuote                  = new PQTickInstant(new PQSourceTickerInfo(sourceTickerInfo)) { HasUpdates = false };
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedPQTickInstant, 1);
        newlyPopulatedPQTickInstant = new PQTickInstant(sourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(newlyPopulatedPQTickInstant, 2);

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void EmptyQuote_SourceTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(default, emptyQuote.SourceTime);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceTime);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var extended            = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourComponent);
        var expectedHour        = new PQFieldUpdate(PQQuoteFields.SourceSentDateTime, hoursSinceUnixEpoch);
        var expectedSubHour     = new PQFieldUpdate(PQQuoteFields.SourceSentSub2MinTime, subHourComponent, extended);
        Assert.AreEqual(expectedHour, sourceUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceUpdates[1]);

        emptyQuote.IsSourceTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedSubHour, sourceUpdates[0]);

        emptyQuote.IsSourceTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id >= PQQuoteFields.SourceSentDateTime && update.Id <= PQQuoteFields.SourceSentSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        Assert.AreEqual(expectedHour, sourceUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceUpdates[1]);

        var newEmpty = new PQTickInstant(sourceTickerInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        newEmpty.UpdateField(sourceUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.SourceTime);
        Assert.IsTrue(newEmpty.IsSourceTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsSourceTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptyQuote_SyncStatusChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedSyncStatus = FeedSyncStatus.FeedDown;
        emptyQuote.FeedSyncStatus = expectedSyncStatus;
        Assert.IsTrue(emptyQuote.IsFeedSyncStatusUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSyncStatus, emptyQuote.FeedSyncStatus);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.PQSyncStatus, (byte)expectedSyncStatus);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsFeedSyncStatusUpdated = false;
        Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.PQSyncStatus
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQTickInstant(sourceTickerInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedSyncStatus, newEmpty.FeedSyncStatus);
        Assert.IsTrue(newEmpty.IsFeedSyncStatusUpdated);
    }

    [TestMethod]
    public void EmptyQuote_SingPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedSingleValue = 1.2345678m;
        var priceScale          = sourceTickerInfo.PriceScalingPrecision;
        emptyQuote.SingleTickValue = expectedSingleValue;
        Assert.IsTrue(emptyQuote.IsSingleValueUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.SingleTickValue, PQScaling.Scale(expectedSingleValue, priceScale), priceScale);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsSingleValueUpdated = false;
        Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.SingleTickValue
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQTickInstant(sourceTickerInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedSingleValue, newEmpty.SingleTickValue);
        Assert.IsTrue(newEmpty.IsSingleValueUpdated);
    }

    [TestMethod]
    public void EmptyQuote_ReplayChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        const bool expectedReplay = true;
        emptyQuote.IsReplay = expectedReplay;
        Assert.IsTrue(emptyQuote.IsReplayUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedReplay, emptyQuote.IsReplay);
        var sourceUpdatesWithUpdated = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdatesWithUpdated.Count);
        var expectedFieldUpdateWithUpdated
            = new PQFieldUpdate(PQQuoteFields.QuoteBooleanFlags, (uint)(PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsReplaySetFlag));
        Assert.AreEqual(expectedFieldUpdateWithUpdated, sourceUpdatesWithUpdated[0]);

        emptyQuote.IsReplayUpdated = false;
        Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var sourceUpdatesNotUpdated = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.QuoteBooleanFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdatesNotUpdated.Count);
        var expectedFieldUpdateWithoutUpdated
            = new PQFieldUpdate(PQQuoteFields.QuoteBooleanFlags, (uint)(PQBooleanValues.IsReplaySetFlag | PQBooleanValues.IsReplayUpdatedFlag));
        Assert.AreEqual(expectedFieldUpdateWithoutUpdated, sourceUpdatesNotUpdated[0]);

        var newEmpty = new PQTickInstant(sourceTickerInfo);
        newEmpty.UpdateField(sourceUpdatesNotUpdated[0]);
        Assert.AreEqual(true, newEmpty.IsReplay);
        Assert.IsTrue(newEmpty.IsReplayUpdated);
    }

    [TestMethod]
    public void EmptyQuote_FieldsSetThenResetFields_SameEmptyQuoteEquivalent()
    {
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyQuote.IsReplay       = true;
        emptyQuote.FeedSyncStatus = FeedSyncStatus.Good;
        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceTime = expectedSetTime;
        var expectedSingleValue = 1.2345678m;
        emptyQuote.SingleTickValue = expectedSingleValue;
        Assert.IsTrue(emptyQuote.HasUpdates);

        emptyQuote.ResetFields();

        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
        Assert.AreEqual(default, emptyQuote.SourceTime);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllTickInstantFields()
    {
        var pqFieldUpdates =
            fullyPopulatedPQTickInstant.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        AssertContainsAllTickInstantFields(sourceTickerInfo, pqFieldUpdates, fullyPopulatedPQTickInstant);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllTickInstantFields()
    {
        fullyPopulatedPQTickInstant.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedPQTickInstant.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Snapshot).ToList();
        AssertContainsAllTickInstantFields(sourceTickerInfo, pqFieldUpdates, fullyPopulatedPQTickInstant);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedPQTickInstant.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedPQTickInstant.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateIncludeReceiverTimesThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        ((PQSourceTickerInfo)fullyPopulatedPQTickInstant.SourceTickerInfo!).HasUpdates = true;

        fullyPopulatedPQTickInstant.IsSourceTimeDateUpdated            = true;
        fullyPopulatedPQTickInstant.IsSourceTimeSub2MinUpdated         = true;
        fullyPopulatedPQTickInstant.IsSocketReceivedTimeDateUpdated    = true;
        fullyPopulatedPQTickInstant.IsSocketReceivedTimeSub2MinUpdated = true;
        fullyPopulatedPQTickInstant.IsProcessedTimeDateUpdated         = true;
        fullyPopulatedPQTickInstant.IsProcessedTimeSub2MinUpdated      = true;
        fullyPopulatedPQTickInstant.IsDispatchedTimeDateUpdated        = true;
        fullyPopulatedPQTickInstant.IsDispatchedTimeSub2MinUpdated     = true;
        fullyPopulatedPQTickInstant.IsClientReceivedTimeDateUpdated    = true;
        fullyPopulatedPQTickInstant.IsClientReceivedTimeSub2MinUpdated = true;
        fullyPopulatedPQTickInstant.IsReplayUpdated                    = true;
        fullyPopulatedPQTickInstant.IsSingleValueUpdated               = true;
        fullyPopulatedPQTickInstant.IsFeedSyncStatusUpdated            = true;
        var pqFieldUpdates = fullyPopulatedPQTickInstant.GetDeltaUpdateFields(
                                                                              new DateTime(2017, 11, 04, 16, 33, 59)
                                                                            , StorageFlags.Update | StorageFlags.IncludeReceiverTimes)
                                                        .ToList();
        var newEmpty = new PQTickInstant(sourceTickerInfo);
        newEmpty.PQSequenceId = fullyPopulatedPQTickInstant.PQSequenceId;
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        // not copied from field updates as is used in by server to track publication times.
        newEmpty.LastPublicationTime = fullyPopulatedPQTickInstant.LastPublicationTime;
        Assert.AreEqual(fullyPopulatedPQTickInstant, newEmpty);
    }

    [TestMethod]
    public void PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerInfo()
    {
        var pqFieldUpdates = fullyPopulatedPQTickInstant.GetStringUpdates(
                                                                          new DateTime(2017, 11, 04, 16, 33, 59)
                                                                        , StorageFlags.Update).ToList();
        Assert.AreEqual(PQSourceTickerInfoTests.ExpectedSourceStringUpdate
                            (fullyPopulatedPQTickInstant.SourceTickerInfo!.SourceName),
                        ExtractFieldStringUpdateWithId(pqFieldUpdates, PQQuoteFields.SourceTickerNames, 0));
        Assert.AreEqual(PQSourceTickerInfoTests.ExpectedTickerStringUpdate
                            (fullyPopulatedPQTickInstant.SourceTickerInfo.InstrumentName),
                        ExtractFieldStringUpdateWithId(pqFieldUpdates, PQQuoteFields.SourceTickerNames, 1));
    }

    [TestMethod]
    public void EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateField_ReturnsSizeFoundInField()
    {
        var expectedSize         = 37;
        var pqStringFieldSize    = new PQFieldUpdate(PQQuoteFields.SourceTickerNames, expectedSize);
        var sizeToReadFromBuffer = emptyQuote.UpdateField(pqStringFieldSize);
        Assert.AreEqual(expectedSize, sizeToReadFromBuffer);
    }

    [TestMethod]
    public void EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues()
    {
        var expectedNewTicker = "NewTestTickerName";
        var expectedNewSource = "NewTestSourceName";

        var tickerStringUpdate = PQSourceTickerInfoTests.ExpectedTickerStringUpdate(expectedNewTicker);
        var sourceStringUpdate = PQSourceTickerInfoTests.ExpectedSourceStringUpdate(expectedNewSource);

        emptyQuote.UpdateFieldString(tickerStringUpdate);
        Assert.AreEqual(expectedNewTicker, emptyQuote.SourceTickerInfo!.InstrumentName);
        emptyQuote.UpdateFieldString(sourceStringUpdate);
        Assert.AreEqual(expectedNewSource, emptyQuote.SourceTickerInfo.SourceName);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new PQTickInstant(blankSourceTickerInfo);
        emptyQuote.CopyFrom(fullyPopulatedPQTickInstant);

        Assert.AreEqual(fullyPopulatedPQTickInstant, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptyQuote = new PQTickInstant(blankSourceTickerInfo);

        fullyPopulatedPQTickInstant.HasUpdates = false;
        emptyQuote.CopyFrom(fullyPopulatedPQTickInstant);
        Assert.AreEqual(fullyPopulatedPQTickInstant.PQSequenceId, emptyQuote.PQSequenceId);
        Assert.AreEqual(default, emptyQuote.SourceTime);
        Assert.AreEqual(default, emptyQuote.ClientReceivedTime);
        Assert.IsTrue(
                      fullyPopulatedPQTickInstant.SourceTickerInfo!.AreEquivalent(emptyQuote.SourceTickerInfo));
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
        Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
        Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
        Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var nonPQTickInstant = new TickInstant(fullyPopulatedPQTickInstant);
        emptyQuote.CopyFrom(nonPQTickInstant);
        Assert.IsTrue(fullyPopulatedPQTickInstant.AreEquivalent(emptyQuote));
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<ITickInstant>)fullyPopulatedPQTickInstant).Clone();
        Assert.AreEqual(fullyPopulatedPQTickInstant, clonedQuote);

        clonedQuote = ((IMutableTickInstant)fullyPopulatedPQTickInstant).Clone();
        Assert.AreNotSame(clonedQuote, fullyPopulatedPQTickInstant);
        if (!clonedQuote.Equals(fullyPopulatedPQTickInstant))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                + clonedQuote.DiffQuotes(fullyPopulatedPQTickInstant) + "'");

        var cloned2 = (PQTickInstant)((ICloneable)fullyPopulatedPQTickInstant).Clone();
        Assert.AreNotSame(cloned2, fullyPopulatedPQTickInstant);
        if (!cloned2.Equals(fullyPopulatedPQTickInstant))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                + cloned2.DiffQuotes(fullyPopulatedPQTickInstant) + "'");

        var cloned3Quote = ((IPQTickInstant)fullyPopulatedPQTickInstant).Clone();
        Assert.AreEqual(fullyPopulatedPQTickInstant, cloned3Quote);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQTickInstant)((ICloneable<ITickInstant>)fullyPopulatedPQTickInstant).Clone();
        // by default SourceTickerInfo is shared.
        fullyPopulatedClone.SourceTickerInfo =
            new PQSourceTickerInfo(fullyPopulatedPQTickInstant.SourceTickerInfo!);
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedPQTickInstant, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedPQTickInstant, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedPQTickInstant, fullyPopulatedPQTickInstant);
        Assert.AreEqual(fullyPopulatedPQTickInstant, ((ICloneable)fullyPopulatedPQTickInstant).Clone());
        Assert.AreEqual(fullyPopulatedPQTickInstant, ((ICloneable<ITickInstant>)fullyPopulatedPQTickInstant).Clone());
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
        var q      = fullyPopulatedPQTickInstant;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
        (bool exactComparison, PQTickInstant original, PQTickInstant changingTickInstant)
    {
        Assert.IsTrue(original.AreEquivalent(changingTickInstant));
        Assert.IsTrue(changingTickInstant.AreEquivalent(original));

        PQSourceTickerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, (PQSourceTickerInfo)original.SourceTickerInfo!,
             (PQSourceTickerInfo)changingTickInstant.SourceTickerInfo!);

        Assert.IsFalse(changingTickInstant.AreEquivalent(null, exactComparison));
        if (original.GetType() == typeof(PQTickInstant))
            Assert.AreEqual(!exactComparison,
                            changingTickInstant.AreEquivalent(new TickInstant(original), exactComparison));

        changingTickInstant.IsReplay = !changingTickInstant.IsReplay;
        Assert.IsFalse(original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.IsReplay = !changingTickInstant.IsReplay;
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));

        changingTickInstant.SingleTickValue = 9.8765432m;
        Assert.IsFalse(changingTickInstant.AreEquivalent(original, exactComparison));
        changingTickInstant.SingleTickValue = original.SingleTickValue;
        Assert.IsTrue(original.AreEquivalent(changingTickInstant, exactComparison));

        changingTickInstant.SourceTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.IsFalse(original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.SourceTime = original.SourceTime;
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));

        changingTickInstant.PQSequenceId = 9999;
        Assert.AreEqual(!exactComparison, changingTickInstant.AreEquivalent(original, exactComparison));
        changingTickInstant.PQSequenceId = original.PQSequenceId;
        Assert.IsTrue(original.AreEquivalent(changingTickInstant, exactComparison));

        changingTickInstant.FeedSyncStatus          = FeedSyncStatus.FeedDown;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.FeedSyncStatus          = original.FeedSyncStatus;
        changingTickInstant.IsFeedSyncStatusUpdated = original.IsFeedSyncStatusUpdated; // not enabled unless updated from default
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));

        changingTickInstant.SocketReceivingTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, changingTickInstant.AreEquivalent(original, exactComparison));
        changingTickInstant.SocketReceivingTime = original.SocketReceivingTime;
        Assert.IsTrue(original.AreEquivalent(changingTickInstant, exactComparison));

        changingTickInstant.LastPublicationTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.LastPublicationTime = original.LastPublicationTime;
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));

        changingTickInstant.ProcessedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, changingTickInstant.AreEquivalent(original, exactComparison));
        changingTickInstant.ProcessedTime = original.ProcessedTime;
        Assert.IsTrue(original.AreEquivalent(changingTickInstant, exactComparison));

        changingTickInstant.DispatchedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.DispatchedTime = original.DispatchedTime;
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));

        changingTickInstant.ClientReceivedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.ClientReceivedTime = original.ClientReceivedTime;
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));
    }

    public static void AssertContainsAllTickInstantFields
    (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates, PQTickInstant originalQuote
      , PQBooleanValues expectedBooleanFlags = PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsReplaySetFlag)
    {
        var priceScale = precisionSettings.PriceScalingPrecision;
        PQSourceTickerInfoTests.AssertSourceTickerInfoContainsAllFields
            (checkFieldUpdates, (PQSourceTickerInfo)originalQuote.SourceTickerInfo!);
        if (originalQuote.IsFeedSyncStatusUpdated)
        {
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.PQSyncStatus, (uint)originalQuote.FeedSyncStatus),
                            ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.PQSyncStatus),
                            $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        }
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SingleTickValue, PQScaling.Scale(originalQuote.SingleTickValue, priceScale), priceScale),
                        ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SingleTickValue),
                        $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        var sourceTime = NonPublicInvocator.GetInstanceField<DateTime>(originalQuote, "sourceTime");
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SourceSentDateTime, sourceTime.Get2MinIntervalsFromUnixEpoch()),
                        ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SourceSentDateTime),
                        $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        var flag = sourceTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SourceSentSub2MinTime, value, flag),
                        ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SourceSentSub2MinTime),
                        $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.QuoteBooleanFlags, (uint)expectedBooleanFlags),
                        ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.QuoteBooleanFlags),
                        $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }

    public static PQFieldStringUpdate ExtractFieldStringUpdateWithId(IList<PQFieldStringUpdate> allUpdates, PQQuoteFields id, int dictionaryId)
    {
        return allUpdates.FirstOrDefault(fu => fu.Field.Id == id && fu.StringUpdate.DictionaryId == dictionaryId);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQQuoteFields id, PQFieldFlags flagValue = PQFieldFlags.None)
    {
        return allUpdates.FirstOrDefault(fu => fu.Id == id);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQQuoteFields id, PQDepthKey depthId, PQFieldFlags flag = PQFieldFlags.None)
    {
        var useDepthFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        var tryFlags     = flag | useDepthFlag;
        var tryGetValue
            = allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.Flag == tryFlags);
        var tryAgainValue = !Equals(tryGetValue, default(PQFieldUpdate))
            ? tryGetValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.Flag == flag);
        tryAgainValue = !Equals(tryGetValue, default(PQFieldUpdate))
            ? tryGetValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId);
        if (!Equals(tryGetValue, tryAgainValue)) Console.Out.WriteLine("");
        return tryAgainValue;
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQQuoteFields id, PQDepthKey depthId, PQSubFieldKey subId, PQFieldFlags flag = PQFieldFlags.None)
    {
        var useExtendedFlag = subId > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None;
        var useDepthFlag    = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        var tryFlags        = flag | useDepthFlag | useExtendedFlag;
        var tryGetValue = allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.SubId == subId &&
                                                          fu.Flag == tryFlags);
        var tryAgainValue = !Equals(tryGetValue, default(PQFieldUpdate))
            ? tryGetValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.SubId == subId &&
                                              fu.Flag == (flag | PQFieldFlags.IncludesDepth));
        var tryTryAgainValue = !Equals(tryAgainValue, default(PQFieldUpdate))
            ? tryGetValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.SubId == subId && fu.Flag == flag);
        return tryTryAgainValue;
    }

    /// Created because when built Moq couldn't handle a property redefinition in interfaces and sets up only
    /// the most base form of the property leaving the redefined property unsetup.
    internal class DummyPQTickInstant : ReusableObject<ITickInstant>, IPQTickInstant, ITransferState<DummyPQTickInstant>
    {
        public uint MessageId    => (uint)PQMessageIds.Quote;
        public byte Version      => 1;
        public uint PQSequenceId { get; set; }

        public virtual TickerDetailLevel TickerDetailLevel => SingleValue;

        public ISyncLock Lock { get; } = new SpinLockLight();

        public IPQTickInstant? Previous { get; set; }
        public IPQTickInstant? Next     { get; set; }

        public PQMessageFlags? OverrideSerializationFlags { get; set; }

        bool ITickInstant.IsReplay => false;

        public DateTime SourceTime => DateTime.Now;

        public DateTime ClientReceivedTime => DateTime.Now;

        public ISourceTickerInfo? SourceTickerInfo { get; set; }

        public DateTime SocketReceivingTime { get; set; }

        public DateTime ProcessedTime  { get; set; }
        public DateTime DispatchedTime { get; set; }

        public FeedSyncStatus FeedSyncStatus { get; set; }

        public decimal SingleTickValue { get; set; } = 0m;

        bool IMutableTickInstant.    IsReplay   { get; set; }
        DateTime IMutableTickInstant.SourceTime { get; set; }

        DateTime IMutableTickInstant.ClientReceivedTime { get; set; }

        public bool HasUpdates { get; set; }

        public bool IsSourceTimeDateUpdated            { get; set; }
        public bool IsSourceTimeSub2MinUpdated         { get; set; }
        public bool IsSocketReceivedTimeDateUpdated    { get; set; }
        public bool IsSocketReceivedTimeSub2MinUpdated { get; set; }
        public bool IsProcessedTimeDateUpdated         { get; set; }
        public bool IsProcessedTimeSub2MinUpdated      { get; set; }
        public bool IsDispatchedTimeDateUpdated        { get; set; }
        public bool IsDispatchedTimeSub2MinUpdated     { get; set; }
        public bool IsClientReceivedTimeDateUpdated    { get; set; }
        public bool IsClientReceivedTimeSub2MinUpdated { get; set; }
        public bool IsReplayUpdated                    { get; set; }
        public bool IsSingleValueUpdated               { get; set; }
        public bool IsFeedSyncStatusUpdated            { get; set; }

        public DateTime LastPublicationTime { get; set; }

        public uint UpdateCount => 0;

        public void UpdateComplete()
        {
            HasUpdates = false;
        }

        IVersionedMessage ICloneable<IVersionedMessage>.Clone() => (IVersionedMessage)Clone();

        ITickInstant ICloneable<ITickInstant>.  Clone() => Clone();
        IMutableTickInstant IMutableTickInstant.Clone() => (IMutableTickInstant)Clone();

        IPQTickInstant IPQTickInstant.Clone() => (IPQTickInstant)Clone();

        ITickInstant? IDoublyLinkedListNode<ITickInstant>.Previous { get; set; }
        ITickInstant? IDoublyLinkedListNode<ITickInstant>.Next     { get; set; }

        public void IncrementTimeBy(TimeSpan toChangeBy) { }

        public IVersionedMessage CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            throw new NotImplementedException();

        public int UpdateField(PQFieldUpdate updates) => -1;

        public bool UpdateFieldString(PQFieldStringUpdate stringUpdate) => false;

        public override ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => this;

        public IReusableObject<IVersionedMessage> CopyFrom
            (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            this;

        public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags,
            IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
        {
            yield break;
        }

        public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
        {
            yield break;
        }

        public void ResetFields() { }

        public DateTime StorageTime(IStorageTimeResolver? resolver)
        {
            if (resolver is IStorageTimeResolver<ITickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
            return QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);
        }

        public void EnsureRelatedItemsAreConfigured(ITickInstant? referenceInstance) { }

        public bool AreEquivalent(ITickInstant? other, bool exactTypes = false) => false;

        IPQTickInstant IPQTickInstant.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => this;

        public virtual DummyPQTickInstant CopyFrom(DummyPQTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => this;

        public IPQTickInstant CopyFrom(IPQTickInstant source, CopyMergeFlags copyMergeFlags) => this;

        public override ITickInstant Clone() => new PQLevel1QuoteTests.DummyLevel1Quote();

        public void SetPricePrecision(decimal precision)  { }
        public void SetVolumePrecision(decimal precision) { }
    }
}
