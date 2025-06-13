// Licensed under the MIT license.
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
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Config.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

[TestClass]
public class PQTickInstantTests
{
    private ISourceTickerInfo blankSourceTickerInfo = null!;

    private PQPublishableTickInstant emptyQuote                  = null!;
    private PQPublishableTickInstant fullyPopulatedPQTickInstant = null!;
    private PQPublishableTickInstant newlyPopulatedPQTickInstant = null!;

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
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.0000001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        blankSourceTickerInfo       = new SourceTickerInfo(0, "", 0, "", Level1Quote, MarketClassification.Unknown);
        fullyPopulatedPQTickInstant = new PQPublishableTickInstant(new PQSourceTickerInfo(sourceTickerInfo));
        emptyQuote = new PQPublishableTickInstant(new PQSourceTickerInfo(sourceTickerInfo))
        {
            FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay, HasUpdates = false
        };
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedPQTickInstant, 1);
        newlyPopulatedPQTickInstant = new PQPublishableTickInstant(sourceTickerInfo);
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
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceTime);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.Get2MinIntervalsFromUnixEpoch();
        var extended            = expectedSetTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourComponent);
        var expectedHour        = new PQFieldUpdate(PQFeedFields.SourceQuoteSentDateTime, hoursSinceUnixEpoch);
        var expectedSub2Min     = new PQFieldUpdate(PQFeedFields.SourceQuoteSentSub2MinTime, subHourComponent, extended);
        Assert.AreEqual(expectedHour, sourceUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceUpdates[1]);

        emptyQuote.IsSourceTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedSub2Min, sourceUpdates[0]);

        emptyQuote.IsSourceTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id is >= PQFeedFields.SourceQuoteSentDateTime and <= PQFeedFields.SourceQuoteSentSub2MinTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        Assert.AreEqual(expectedHour, sourceUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceUpdates[1]);

        var newEmpty = new PQPublishableTickInstant(sourceTickerInfo);
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
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedSyncStatus = FeedSyncStatus.FeedDown;
        emptyQuote.FeedSyncStatus = expectedSyncStatus;
        Assert.IsTrue(emptyQuote.IsFeedSyncStatusUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSyncStatus, emptyQuote.FeedSyncStatus);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.IncludeReceiverTimes).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.PQSyncStatus, (byte)expectedSyncStatus);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsFeedSyncStatusUpdated = false;
        Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot | PQMessageFlags.IncludeReceiverTimes)
            where update.Id == PQFeedFields.PQSyncStatus
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPublishableTickInstant(sourceTickerInfo);
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
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedSingleValue = 1.2345678m;
        var priceScale          = sourceTickerInfo.PriceScalingPrecision;
        emptyQuote.SingleTickValue = expectedSingleValue;
        Assert.IsTrue(emptyQuote.IsSingleValueUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.SingleTickValue, PQScaling.Scale(expectedSingleValue, priceScale), priceScale);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsSingleValueUpdated = false;
        Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.SingleTickValue
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPublishableTickInstant(sourceTickerInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedSingleValue, newEmpty.SingleTickValue);
        Assert.IsTrue(newEmpty.IsSingleValueUpdated);
    }

    [TestMethod]
    public void EmptyQuote_FieldsSetThenResetFields_SameEmptyQuoteEquivalent()
    {
        Assert.IsFalse(emptyQuote.IsFeedConnectivityStatusUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(FeedConnectivityStatusFlags.IsAdapterReplay, emptyQuote.FeedMarketConnectivityStatus);
        var deltaUpdateFields = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.IsTrue(deltaUpdateFields.IsNullOrEmpty());

        emptyQuote.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.AboutToStop;
        emptyQuote.FeedSyncStatus               = FeedSyncStatus.Good;
        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceTime = expectedSetTime;
        var expectedSingleValue = 1.2345678m;
        emptyQuote.SingleTickValue = expectedSingleValue;
        Assert.IsTrue(emptyQuote.HasUpdates);

        emptyQuote.ResetWithTracking();

        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyQuote.FeedMarketConnectivityStatus);
        Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
        Assert.AreEqual(default, emptyQuote.SourceTime);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllTickInstantFields()
    {
        var pqFieldUpdates =
            fullyPopulatedPQTickInstant.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        AssertContainsAllTickInstantFields(sourceTickerInfo, pqFieldUpdates, fullyPopulatedPQTickInstant);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllTickInstantFields()
    {
        fullyPopulatedPQTickInstant.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedPQTickInstant.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllTickInstantFields(sourceTickerInfo, pqFieldUpdates, fullyPopulatedPQTickInstant);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedPQTickInstant.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedPQTickInstant.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
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
        fullyPopulatedPQTickInstant.IsFeedConnectivityStatusUpdated    = true;
        fullyPopulatedPQTickInstant.IsSingleValueUpdated               = true;
        fullyPopulatedPQTickInstant.IsFeedSyncStatusUpdated            = true;
        var pqFieldUpdates = fullyPopulatedPQTickInstant.GetDeltaUpdateFields(
                                                                              new DateTime(2017, 11, 04, 16, 33, 59)
                                                                            , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes)
                                                        .ToList();
        var newEmpty = new PQPublishableTickInstant(sourceTickerInfo)
        {
            PQSequenceId = fullyPopulatedPQTickInstant.PQSequenceId
        };
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        // not copied from field updates as is used in by server to track publication times.
        newEmpty.LastPublicationTime = fullyPopulatedPQTickInstant.LastPublicationTime;
        Assert.AreEqual(fullyPopulatedPQTickInstant, newEmpty);
    }

    [TestMethod]
    public void PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerInfo()
    {
        var pqFieldUpdates = fullyPopulatedPQTickInstant.GetStringUpdates
            (new DateTime(2017, 11, 04, 16, 33, 59)
           , PQMessageFlags.Update).ToList();
        Assert.AreEqual(PQSourceTickerInfoTests.ExpectedSourceStringUpdate
                            (fullyPopulatedPQTickInstant.SourceTickerInfo!.SourceName),
                        ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFeedFields.SourceTickerDefinitionStringUpdates, 1));
        Assert.AreEqual(PQSourceTickerInfoTests.ExpectedTickerStringUpdate
                            (fullyPopulatedPQTickInstant.SourceTickerInfo.InstrumentName),
                        ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFeedFields.SourceTickerDefinitionStringUpdates, 2));
    }


    [TestMethod]
    public void EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues()
    {
        var expectedNewTicker = "NewTestTickerName";
        var expectedNewSource = "NewTestSourceName";

        var tickerStringUpdate = PQSourceTickerInfoTests.ExpectedTickerStringUpdate(expectedNewTicker);
        var sourceStringUpdate = PQSourceTickerInfoTests.ExpectedSourceStringUpdate(expectedNewSource);

        emptyQuote.UpdateField(new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.InstrumentNameId, 3));
        emptyQuote.UpdateFieldString(tickerStringUpdate.WithDictionaryId(3));
        Assert.AreEqual(expectedNewTicker, emptyQuote.SourceTickerInfo!.InstrumentName);
        emptyQuote.UpdateField(new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.SourceNameId, 4));
        emptyQuote.UpdateFieldString(sourceStringUpdate.WithDictionaryId(4));
        Assert.AreEqual(expectedNewSource, emptyQuote.SourceTickerInfo.SourceName);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new PQPublishableTickInstant(blankSourceTickerInfo);
        emptyQuote.CopyFrom(fullyPopulatedPQTickInstant);

        Assert.AreEqual(fullyPopulatedPQTickInstant, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptyQuote = new PQPublishableTickInstant(blankSourceTickerInfo);

        fullyPopulatedPQTickInstant.HasUpdates = false;
        emptyQuote.CopyFrom(fullyPopulatedPQTickInstant);
        Assert.AreEqual(fullyPopulatedPQTickInstant.PQSequenceId, emptyQuote.PQSequenceId);
        Assert.AreEqual(default, emptyQuote.SourceTime);
        Assert.AreEqual(default, emptyQuote.ClientReceivedTime);
        Assert.IsFalse(fullyPopulatedPQTickInstant.SourceTickerInfo!.AreEquivalent(emptyQuote.SourceTickerInfo));
        Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyQuote.FeedMarketConnectivityStatus);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
        Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
        Assert.IsFalse(emptyQuote.IsFeedConnectivityStatusUpdated);
        Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
        Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var nonPQTickInstant = new PublishableTickInstant(fullyPopulatedPQTickInstant);
        emptyQuote.CopyFrom(nonPQTickInstant, CopyMergeFlags.Default);
        Assert.IsTrue(fullyPopulatedPQTickInstant.AreEquivalent(emptyQuote));
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<IPublishableTickInstant>)fullyPopulatedPQTickInstant).Clone();
        Assert.AreEqual(fullyPopulatedPQTickInstant, clonedQuote);

        clonedQuote = ((IMutablePublishableTickInstant)fullyPopulatedPQTickInstant).Clone();
        Assert.AreNotSame(clonedQuote, fullyPopulatedPQTickInstant);
        if (!clonedQuote.Equals(fullyPopulatedPQTickInstant))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                + clonedQuote.DiffQuotes(fullyPopulatedPQTickInstant) + "'");

        var cloned2 = (PQPublishableTickInstant)((ICloneable)fullyPopulatedPQTickInstant).Clone();
        Assert.AreNotSame(cloned2, fullyPopulatedPQTickInstant);
        if (!cloned2.Equals(fullyPopulatedPQTickInstant))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                + cloned2.DiffQuotes(fullyPopulatedPQTickInstant) + "'");

        var cloned3Quote = ((IPQPublishableTickInstant)fullyPopulatedPQTickInstant).Clone();
        Assert.AreEqual(fullyPopulatedPQTickInstant, cloned3Quote);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQPublishableTickInstant)((ICloneable<IPublishableTickInstant>)fullyPopulatedPQTickInstant).Clone();
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
        Assert.AreEqual(fullyPopulatedPQTickInstant, ((ICloneable<IPublishableTickInstant>)fullyPopulatedPQTickInstant).Clone());
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
        (bool exactComparison, PQPublishableTickInstant original, PQPublishableTickInstant changingTickInstant)
    {
        Assert.IsTrue(original.AreEquivalent(changingTickInstant));
        Assert.IsTrue(changingTickInstant.AreEquivalent(original));

        PQSourceTickerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, (PQSourceTickerInfo)original.SourceTickerInfo!,
             (PQSourceTickerInfo)changingTickInstant.SourceTickerInfo!);

        Assert.IsFalse(changingTickInstant.AreEquivalent(null, exactComparison));
        if (original.GetType() == typeof(PQPublishableTickInstant))
            Assert.AreEqual(!exactComparison,
                            changingTickInstant.AreEquivalent(new PublishableTickInstant(original), exactComparison));

        changingTickInstant.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.AboutToRestart | FeedConnectivityStatusFlags.ClosedOutOfHours;
        Assert.IsFalse(original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.FeedMarketConnectivityStatus = original.FeedMarketConnectivityStatus;
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

        changingTickInstant.FeedSyncStatus = FeedSyncStatus.FeedDown;
        Assert.IsFalse(original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.FeedSyncStatus          = original.FeedSyncStatus;
        changingTickInstant.IsFeedSyncStatusUpdated = original.IsFeedSyncStatusUpdated; // not enabled unless updated from default
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));

        changingTickInstant.InboundSocketReceivingTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, changingTickInstant.AreEquivalent(original, exactComparison));
        changingTickInstant.InboundSocketReceivingTime = original.InboundSocketReceivingTime;
        Assert.IsTrue(original.AreEquivalent(changingTickInstant, exactComparison));

        changingTickInstant.LastPublicationTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.LastPublicationTime = original.LastPublicationTime;
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));

        changingTickInstant.InboundProcessedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, changingTickInstant.AreEquivalent(original, exactComparison));
        changingTickInstant.InboundProcessedTime = original.InboundProcessedTime;
        Assert.IsTrue(original.AreEquivalent(changingTickInstant, exactComparison));

        changingTickInstant.SubscriberDispatchedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.SubscriberDispatchedTime = original.SubscriberDispatchedTime;
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));

        changingTickInstant.ClientReceivedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTickInstant, exactComparison));
        changingTickInstant.ClientReceivedTime = original.ClientReceivedTime;
        Assert.IsTrue(changingTickInstant.AreEquivalent(original, exactComparison));
    }

    public static void AssertContainsAllTickInstantFields
    (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates, PQPublishableTickInstant originalQuote
      , PQQuoteBooleanValues expectedQuoteBooleanFlags = PQQuoteBooleanValues.None)
    {
        var priceScale = precisionSettings.PriceScalingPrecision;
        PQSourceTickerInfoTests.AssertSourceTickerInfoContainsAllFields
            (checkFieldUpdates, (PQSourceTickerInfo)originalQuote.SourceTickerInfo!);
        if (originalQuote.IsFeedSyncStatusUpdated)
        {
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.PQSyncStatus, (uint)originalQuote.FeedSyncStatus),
                            ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.PQSyncStatus),
                            $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        }
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.SingleTickValue, PQScaling.Scale(originalQuote.SingleTickValue, priceScale), priceScale),
                        ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.SingleTickValue),
                        $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        var quoteContainer = originalQuote.AsNonPublishable;
        var sourceTime     = NonPublicInvocator.GetInstanceField<DateTime>(quoteContainer, "sourceTime");
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.SourceQuoteSentDateTime, sourceTime.Get2MinIntervalsFromUnixEpoch()),
                        ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.SourceQuoteSentDateTime),
                        $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        var flag = sourceTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.SourceQuoteSentSub2MinTime, value, flag),
                        ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.SourceQuoteSentSub2MinTime),
                        $"For {originalQuote.GetType().Name} and {originalQuote.SourceTickerInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }

    public static PQFieldStringUpdate ExtractFieldStringUpdateWithId(IList<PQFieldStringUpdate> allUpdates, PQFeedFields id, int dictionaryId)
    {
        return allUpdates.FirstOrDefault(fu => fu.Field.Id == id && fu.StringUpdate.DictionaryId == dictionaryId);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQFieldFlags flagValue = PQFieldFlags.None)
    {
        return allUpdates.FirstOrDefault(fu => fu.Id == id);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQPricingSubFieldKeys subId, PQFieldFlags flagValue = PQFieldFlags.None)
    {
        return ExtractFieldUpdateWithId(allUpdates, id, (byte)subId, flagValue);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQTickerDefSubFieldKeys subId, PQFieldFlags flagValue = PQFieldFlags.None)
    {
        return ExtractFieldUpdateWithId(allUpdates, id, (byte)subId, flagValue);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQTradingSubFieldKeys subId, PQFieldFlags flagValue = PQFieldFlags.None)
    {
        return ExtractFieldUpdateWithId(allUpdates, id, (byte)subId, flagValue);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, byte subId, PQFieldFlags flagValue = PQFieldFlags.None)
    {
        return allUpdates.FirstOrDefault(fu => fu.Id == id && fu.SubIdByte == subId);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQDepthKey depthId, PQFieldFlags flag = PQFieldFlags.None)
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
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQDepthKey depthId, PQPricingSubFieldKeys subId, PQFieldFlags flag = PQFieldFlags.None)
    {
        return ExtractFieldUpdateWithId(allUpdates, id, depthId, (byte)subId, flag);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQDepthKey depthId, PQTradingSubFieldKeys subId, PQFieldFlags flag = PQFieldFlags.None)
    {
        return ExtractFieldUpdateWithId(allUpdates, id, depthId, (byte)subId, flag);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId
        (IList<PQFieldUpdate> allUpdates, PQFeedFields id, PQDepthKey depthId, byte subId, PQFieldFlags flag = PQFieldFlags.None)
    {
        var useSubId     = subId > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None;
        var useDepthFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        var tryFlags     = flag | useDepthFlag | useSubId;
        var tryGetValue = allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.SubIdByte == subId &&
                                                          fu.Flag == tryFlags);
        var tryAgainValue = !Equals(tryGetValue, default(PQFieldUpdate))
            ? tryGetValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.SubIdByte == subId &&
                                              fu.Flag == (flag | PQFieldFlags.IncludesDepth));
        var tryTryAgainValue = !Equals(tryAgainValue, default(PQFieldUpdate))
            ? tryGetValue
            : allUpdates.FirstOrDefault(fu => fu.Id == id && fu.DepthId == depthId && fu.SubIdByte == subId && fu.Flag == flag);
        return tryTryAgainValue;
    }
}
