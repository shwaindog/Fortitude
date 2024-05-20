#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQLevel0QuoteTests
{
    private ISourceTickerQuoteInfo blankSourceTickerQuoteInfo = null!;
    private PQLevel0Quote emptyQuote = null!;
    private PQLevel0Quote fullyPopulatedPqLevel0Quote = null!;
    private PQLevel0Quote newlyPopulatedPqLevel0Quote = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private ISourceTickerQuoteInfo sourceTickerQuoteInfo = null!;
    private DateTime testDateTime;

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
        blankSourceTickerQuoteInfo = new SourceTickerQuoteInfo(0, "", 0, "", QuoteLevel.Level1);
        emptyQuote = new PQLevel0Quote(sourceTickerQuoteInfo) { HasUpdates = false };
        fullyPopulatedPqLevel0Quote = new PQLevel0Quote(sourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedPqLevel0Quote, 1);
        newlyPopulatedPqLevel0Quote = new PQLevel0Quote(sourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(newlyPopulatedPqLevel0Quote, 2);

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void EmptyQuote_SourceTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceTime = expectedSetTime;
        Assert.IsTrue(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsTrue(emptyQuote.IsSourceTimeSubHourUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSetTime, emptyQuote.SourceTime);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        var hoursSinceUnixEpoch = expectedSetTime.GetHoursFromUnixEpoch();
        var subHourComponent = expectedSetTime.GetSubHourComponent();
        var expectedHour = new PQFieldUpdate(PQFieldKeys.SourceSentDateTime, hoursSinceUnixEpoch);
        var expectedSubHour = new PQFieldUpdate(PQFieldKeys.SourceSentSubHourTime, subHourComponent, 15);
        Assert.AreEqual(expectedHour, sourceUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceUpdates[1]);

        emptyQuote.IsSourceTimeDateUpdated = false;
        Assert.IsTrue(emptyQuote.HasUpdates);
        sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedSubHour, sourceUpdates[0]);

        emptyQuote.IsSourceTimeSubHourUpdated = false;
        Assert.IsFalse(emptyQuote.IsSourceTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id >= PQFieldKeys.SourceSentDateTime && update.Id <= PQFieldKeys.SourceSentSubHourTime
            orderby update.Id
            select update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        Assert.AreEqual(expectedHour, sourceUpdates[0]);
        Assert.AreEqual(expectedSubHour, sourceUpdates[1]);

        var newEmpty = new PQLevel0Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        newEmpty.UpdateField(sourceUpdates[1]);
        Assert.AreEqual(expectedSetTime, newEmpty.SourceTime);
        Assert.IsTrue(newEmpty.IsSourceTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsSourceTimeSubHourUpdated);
    }

    [TestMethod]
    public void EmptyQuote_SyncStatusChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsSyncStatusUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(PQSyncStatus.OutOfSync, emptyQuote.PQSyncStatus);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedSyncStatus = PQSyncStatus.Good;
        emptyQuote.PQSyncStatus = expectedSyncStatus;
        Assert.IsTrue(emptyQuote.IsSyncStatusUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSyncStatus, emptyQuote.PQSyncStatus);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PQSyncStatus, (byte)expectedSyncStatus);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsSyncStatusUpdated = false;
        Assert.IsFalse(emptyQuote.IsSyncStatusUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFieldKeys.PQSyncStatus
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQLevel0Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedSyncStatus, newEmpty.PQSyncStatus);
        Assert.IsTrue(newEmpty.IsSyncStatusUpdated);
    }

    [TestMethod]
    public void EmptyQuote_SingPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsSinglePriceUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(0m, emptyQuote.SinglePrice);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedSinglePrice = 1.2345678m;
        emptyQuote.SinglePrice = expectedSinglePrice;
        Assert.IsTrue(emptyQuote.IsSinglePriceUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedSinglePrice, emptyQuote.SinglePrice);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.SinglePrice, expectedSinglePrice, 1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsSinglePriceUpdated = false;
        Assert.IsFalse(emptyQuote.IsSinglePriceUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFieldKeys.SinglePrice
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQLevel0Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedSinglePrice, newEmpty.SinglePrice);
        Assert.IsTrue(newEmpty.IsSinglePriceUpdated);
    }

    [TestMethod]
    public void EmptyQuote_ReplayChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        const bool expectedReplay = true;
        emptyQuote.IsReplay = expectedReplay;
        Assert.IsTrue(emptyQuote.IsReplayUpdated);
        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(expectedReplay, emptyQuote.IsReplay);
        var sourceUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.QuoteBooleanFlags, 1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyQuote.IsReplayUpdated = false;
        Assert.IsFalse(emptyQuote.IsSinglePriceUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFieldKeys.QuoteBooleanFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQLevel0Quote(sourceTickerQuoteInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedReplay, newEmpty.IsReplay);
        Assert.IsTrue(newEmpty.IsReplayUpdated);
    }

    [TestMethod]
    public void EmptyQuote_FieldsSetThenResetFields_SameEmptyQuoteEquivalent()
    {
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
        Assert.IsFalse(emptyQuote.HasUpdates);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        emptyQuote.IsReplay = true;
        emptyQuote.PQSyncStatus = PQSyncStatus.Good;
        var expectedSetTime = new DateTime(2017, 10, 14, 15, 10, 59).AddTicks(9879879);
        emptyQuote.SourceTime = expectedSetTime;
        var expectedSinglePrice = 1.2345678m;
        emptyQuote.SinglePrice = expectedSinglePrice;
        Assert.IsTrue(emptyQuote.HasUpdates);

        emptyQuote.ResetFields();

        Assert.IsTrue(emptyQuote.HasUpdates);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(PQSyncStatus.OutOfSync, emptyQuote.PQSyncStatus);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.AreEqual(0m, emptyQuote.SinglePrice);
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel0Fields()
    {
        var pqFieldUpdates = fullyPopulatedPqLevel0Quote.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        AssertContainsAllLevel0Fields(pqFieldUpdates, fullyPopulatedPqLevel0Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel0Fields()
    {
        fullyPopulatedPqLevel0Quote.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPqLevel0Quote.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllLevel0Fields(pqFieldUpdates, fullyPopulatedPqLevel0Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedPqLevel0Quote.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedPqLevel0Quote.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateIncludeReceiverTimesThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        ((PQSourceTickerQuoteInfo)fullyPopulatedPqLevel0Quote.SourceTickerQuoteInfo!).HasUpdates = true;
        fullyPopulatedPqLevel0Quote.IsSourceTimeDateUpdated = true;
        fullyPopulatedPqLevel0Quote.IsSourceTimeSubHourUpdated = true;
        fullyPopulatedPqLevel0Quote.IsReplayUpdated = true;
        fullyPopulatedPqLevel0Quote.IsSinglePriceUpdated = true;
        fullyPopulatedPqLevel0Quote.IsSyncStatusUpdated = true;
        var pqFieldUpdates = fullyPopulatedPqLevel0Quote.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQLevel0Quote(sourceTickerQuoteInfo);
        newEmpty.PQSequenceId = fullyPopulatedPqLevel0Quote.PQSequenceId;
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        // not copied from field updates as is used in by server to track publication times.
        newEmpty.LastPublicationTime = fullyPopulatedPqLevel0Quote.LastPublicationTime;
        Assert.AreEqual(fullyPopulatedPqLevel0Quote, newEmpty);
    }

    [TestMethod]
    public void PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerQuoteInfo()
    {
        var pqFieldUpdates = fullyPopulatedPqLevel0Quote.GetStringUpdates(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(PQSourceTickerQuoteInfoTests.ExpectedSourceStringUpdate(
                fullyPopulatedPqLevel0Quote.SourceTickerQuoteInfo!.Source),
            ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFieldKeys.SourceTickerNames, 0));
        Assert.AreEqual(PQSourceTickerQuoteInfoTests.ExpectedTickerStringUpdate(
                fullyPopulatedPqLevel0Quote.SourceTickerQuoteInfo.Ticker),
            ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFieldKeys.SourceTickerNames, 1));
    }

    [TestMethod]
    public void EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateField_ReturnsSizeFoundInField()
    {
        var expectedSize = 37;
        var pqStringFieldSize = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, expectedSize);
        var sizeToReadFromBuffer = emptyQuote.UpdateField(pqStringFieldSize);
        Assert.AreEqual(expectedSize, sizeToReadFromBuffer);
    }

    [TestMethod]
    public void EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues()
    {
        var expectedNewTicker = "NewTestTickerName";
        var expectedNewSource = "NewTestSourceName";

        var tickerStringUpdate = PQSourceTickerQuoteInfoTests.ExpectedTickerStringUpdate(expectedNewTicker);
        var sourceStringUpdate = PQSourceTickerQuoteInfoTests.ExpectedSourceStringUpdate(expectedNewSource);


        emptyQuote.UpdateFieldString(tickerStringUpdate);
        Assert.AreEqual(expectedNewTicker, emptyQuote.SourceTickerQuoteInfo!.Ticker);
        emptyQuote.UpdateFieldString(sourceStringUpdate);
        Assert.AreEqual(expectedNewSource, emptyQuote.SourceTickerQuoteInfo.Source);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new PQLevel0Quote(blankSourceTickerQuoteInfo);
        emptyQuote.CopyFrom(fullyPopulatedPqLevel0Quote);

        Assert.AreEqual(fullyPopulatedPqLevel0Quote, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        emptyQuote = new PQLevel0Quote(blankSourceTickerQuoteInfo);
        fullyPopulatedPqLevel0Quote.HasUpdates = false;
        emptyQuote.CopyFrom(fullyPopulatedPqLevel0Quote);
        Assert.AreEqual(fullyPopulatedPqLevel0Quote.ClientReceivedTime, emptyQuote.ClientReceivedTime);
        Assert.AreEqual(fullyPopulatedPqLevel0Quote.PQSequenceId, emptyQuote.PQSequenceId);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.IsTrue(
            fullyPopulatedPqLevel0Quote.SourceTickerQuoteInfo!.AreEquivalent(emptyQuote.SourceTickerQuoteInfo));
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SinglePrice);
        Assert.AreEqual(PQSyncStatus.OutOfSync, emptyQuote.PQSyncStatus);
        Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
        Assert.IsFalse(emptyQuote.IsSourceTimeSubHourUpdated);
        Assert.IsFalse(emptyQuote.IsReplayUpdated);
        Assert.IsFalse(emptyQuote.IsSinglePriceUpdated);
        Assert.IsFalse(emptyQuote.IsSyncStatusUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var nonPQLevel0Quote = new Level0PriceQuote(fullyPopulatedPqLevel0Quote);
        emptyQuote.CopyFrom(nonPQLevel0Quote);
        Assert.IsTrue(fullyPopulatedPqLevel0Quote.AreEquivalent(emptyQuote));
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<ILevel0Quote>)fullyPopulatedPqLevel0Quote).Clone();
        Assert.AreEqual(fullyPopulatedPqLevel0Quote, clonedQuote);

        clonedQuote = ((IMutableLevel0Quote)fullyPopulatedPqLevel0Quote).Clone();
        Assert.AreNotSame(clonedQuote, fullyPopulatedPqLevel0Quote);
        if (!clonedQuote.Equals(fullyPopulatedPqLevel0Quote))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                  + clonedQuote.DiffQuotes(fullyPopulatedPqLevel0Quote) + "'");

        var cloned2 = (PQLevel0Quote)((ICloneable)fullyPopulatedPqLevel0Quote).Clone();
        Assert.AreNotSame(cloned2, fullyPopulatedPqLevel0Quote);
        if (!cloned2.Equals(fullyPopulatedPqLevel0Quote))
            Console.Out.WriteLine("clonedQuote differences are \n '"
                                  + cloned2.DiffQuotes(fullyPopulatedPqLevel0Quote) + "'");

        var cloned3Quote = ((IPQLevel0Quote)fullyPopulatedPqLevel0Quote).Clone();
        Assert.AreEqual(fullyPopulatedPqLevel0Quote, cloned3Quote);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQLevel0Quote)((ICloneable<ILevel0Quote>)fullyPopulatedPqLevel0Quote).Clone();
        // by default SourceTickerQuoteInfo is shared.
        fullyPopulatedClone.SourceTickerQuoteInfo =
            new PQSourceTickerQuoteInfo(fullyPopulatedPqLevel0Quote.SourceTickerQuoteInfo!);
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedPqLevel0Quote, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedPqLevel0Quote, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedPqLevel0Quote, fullyPopulatedPqLevel0Quote);
        Assert.AreEqual(fullyPopulatedPqLevel0Quote, ((ICloneable)fullyPopulatedPqLevel0Quote).Clone());
        Assert.AreEqual(fullyPopulatedPqLevel0Quote, ((ICloneable<ILevel0Quote>)fullyPopulatedPqLevel0Quote).Clone());
    }

    [TestMethod]
    public void EmptyQuote_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = emptyQuote.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison, PQLevel0Quote original,
        PQLevel0Quote changingLevel0Quote)
    {
        Assert.IsTrue(original.AreEquivalent(changingLevel0Quote));
        Assert.IsTrue(changingLevel0Quote.AreEquivalent(original));

        PQSourceTickerQuoteInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            (PQSourceTickerQuoteInfo)original.SourceTickerQuoteInfo!,
            (PQSourceTickerQuoteInfo)changingLevel0Quote.SourceTickerQuoteInfo!);

        Assert.IsFalse(changingLevel0Quote.AreEquivalent(null, exactComparison));
        if (original.GetType() == typeof(PQLevel0Quote))
            Assert.AreEqual(!exactComparison,
                changingLevel0Quote.AreEquivalent(new Level0PriceQuote(original), exactComparison));

        changingLevel0Quote.IsReplay = !changingLevel0Quote.IsReplay;
        Assert.IsFalse(original.AreEquivalent(changingLevel0Quote, exactComparison));
        changingLevel0Quote.IsReplay = !changingLevel0Quote.IsReplay;
        Assert.IsTrue(changingLevel0Quote.AreEquivalent(original, exactComparison));

        changingLevel0Quote.SinglePrice = 9.8765432m;
        Assert.IsFalse(changingLevel0Quote.AreEquivalent(original, exactComparison));
        changingLevel0Quote.SinglePrice = original.SinglePrice;
        Assert.IsTrue(original.AreEquivalent(changingLevel0Quote, exactComparison));

        changingLevel0Quote.SourceTime = new DateTime(2017, 11, 06, 11, 51, 07);
        Assert.IsFalse(original.AreEquivalent(changingLevel0Quote, exactComparison));
        changingLevel0Quote.SourceTime = original.SourceTime;
        Assert.IsTrue(changingLevel0Quote.AreEquivalent(original, exactComparison));

        changingLevel0Quote.PQSequenceId = 9999;
        Assert.AreEqual(!exactComparison, changingLevel0Quote.AreEquivalent(original, exactComparison));
        changingLevel0Quote.PQSequenceId = original.PQSequenceId;
        Assert.IsTrue(original.AreEquivalent(changingLevel0Quote, exactComparison));

        changingLevel0Quote.PQSyncStatus = PQSyncStatus.FeedDown;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLevel0Quote, exactComparison));
        changingLevel0Quote.PQSyncStatus = original.PQSyncStatus;
        Assert.IsTrue(changingLevel0Quote.AreEquivalent(original, exactComparison));

        changingLevel0Quote.SocketReceivingTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, changingLevel0Quote.AreEquivalent(original, exactComparison));
        changingLevel0Quote.SocketReceivingTime = original.SocketReceivingTime;
        Assert.IsTrue(original.AreEquivalent(changingLevel0Quote, exactComparison));

        changingLevel0Quote.LastPublicationTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLevel0Quote, exactComparison));
        changingLevel0Quote.LastPublicationTime = original.LastPublicationTime;
        Assert.IsTrue(changingLevel0Quote.AreEquivalent(original, exactComparison));

        changingLevel0Quote.ProcessedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, changingLevel0Quote.AreEquivalent(original, exactComparison));
        changingLevel0Quote.ProcessedTime = original.ProcessedTime;
        Assert.IsTrue(original.AreEquivalent(changingLevel0Quote, exactComparison));

        changingLevel0Quote.DispatchedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLevel0Quote, exactComparison));
        changingLevel0Quote.DispatchedTime = original.DispatchedTime;
        Assert.IsTrue(changingLevel0Quote.AreEquivalent(original, exactComparison));

        changingLevel0Quote.ClientReceivedTime = new DateTime(2017, 11, 06, 21, 24, 41);
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLevel0Quote, exactComparison));
        changingLevel0Quote.ClientReceivedTime = original.ClientReceivedTime;
        Assert.IsTrue(changingLevel0Quote.AreEquivalent(original, exactComparison));
    }

    public static void AssertContainsAllLevel0Fields(IList<PQFieldUpdate> checkFieldUpdates,
        PQLevel0Quote originalQuote, uint expectedBooleanFlags = 1)
    {
        PQSourceTickerQuoteInfoTests.AssertSourceTickerInfoContainsAllFields(checkFieldUpdates,
            originalQuote.SourceTickerQuoteInfo!);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PQSyncStatus, (uint)originalQuote.PQSyncStatus),
            ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PQSyncStatus));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SinglePrice, originalQuote.SinglePrice, 1),
            ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SinglePrice));
        var sourceTime = NonPublicInvocator.GetInstanceField<DateTime>(originalQuote, "sourceTime");
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceSentDateTime, sourceTime.GetHoursFromUnixEpoch()),
            ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceSentDateTime));
        var flag = sourceTime.GetSubHourComponent().BreakLongToByteAndUint(out var value);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceSentSubHourTime, value, flag),
            ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceSentSubHourTime));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.QuoteBooleanFlags, expectedBooleanFlags),
            ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.QuoteBooleanFlags));
    }

    public static PQFieldStringUpdate ExtractFieldStringUpdateWithId(IList<PQFieldStringUpdate> allUpdates,
        byte id, int dictionaryId)
    {
        return allUpdates.FirstOrDefault(fu => fu.Field.Id == id && fu.StringUpdate.DictionaryId == dictionaryId);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId(IList<PQFieldUpdate> allUpdates, ushort id)
    {
        return allUpdates.FirstOrDefault(fu => fu.Id == id);
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId(IList<PQFieldUpdate> allUpdates, ushort id, byte flagValue)
    {
        return allUpdates.FirstOrDefault(fu => fu.Id == id && fu.Flag == flagValue);
    }

    /// Created because when built Moq couldn't handle a property redefinition in interfaces and sets up only
    /// the most base form of the property leaving the redefined property unsetup.
    internal class DummyPQLevel0Quote : ReusableObject<ILevel0Quote>, IPQLevel0Quote, IStoreState<DummyPQLevel0Quote>
    {
        public uint MessageId => (uint)PQMessageIds.Quote;
        public byte Version => 1;
        public uint PQSequenceId { get; set; }
        public virtual QuoteLevel QuoteLevel => QuoteLevel.Level0;
        public ISyncLock Lock { get; } = new SpinLockLight();
        public IPQLevel0Quote? Previous { get; set; }
        public IPQLevel0Quote? Next { get; set; }

        public PQMessageFlags? OverrideSerializationFlags { get; set; }

        IVersionedMessage ICloneable<IVersionedMessage>.Clone() => (IVersionedMessage)Clone();

        ILevel0Quote ICloneable<ILevel0Quote>.Clone() => Clone();
        IMutableLevel0Quote IMutableLevel0Quote.Clone() => (IMutableLevel0Quote)Clone();
        IPQLevel0Quote IPQLevel0Quote.Clone() => (IPQLevel0Quote)Clone();

        public IVersionedMessage CopyFrom(IVersionedMessage source
            , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            throw new NotImplementedException();

        bool ILevel0Quote.IsReplay => false;
        DateTime ILevel0Quote.SourceTime => DateTime.Now;
        DateTime ILevel0Quote.ClientReceivedTime => DateTime.Now;
        public ISourceTickerQuoteInfo? SourceTickerQuoteInfo { get; set; }
        public DateTime SocketReceivingTime { get; set; }
        public DateTime ProcessedTime { get; set; }
        public DateTime DispatchedTime { get; set; }
        public PQSyncStatus PQSyncStatus { get; set; }
        public decimal SinglePrice { get; set; } = 0m;
        bool IMutableLevel0Quote.IsReplay { get; set; }
        DateTime IMutableLevel0Quote.SourceTime { get; set; }
        DateTime IMutableLevel0Quote.ClientReceivedTime { get; set; }
        public int UpdateField(PQFieldUpdate updates) => -1;
        public bool UpdateFieldString(PQFieldStringUpdate stringUpdate) => false;

        public override ILevel0Quote CopyFrom(ILevel0Quote source
            , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            this;

        public IReusableObject<IVersionedMessage> CopyFrom(IReusableObject<IVersionedMessage> source
            , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            this;

        public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, PQMessageFlags messageFlags,
            IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
        {
            yield break;
        }

        public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags)
        {
            yield break;
        }

        public bool HasUpdates { get; set; }
        public void ResetFields() { }
        public bool IsSourceTimeDateUpdated { get; set; }
        public bool IsSourceTimeSubHourUpdated { get; set; }
        public bool IsReplayUpdated { get; set; }
        public bool IsSinglePriceUpdated { get; set; }
        public bool IsSyncStatusUpdated { get; set; }
        public DateTime LastPublicationTime { get; set; }

        public DateTime StorageTime(IStorageTimeResolver<ILevel0Quote>? resolver = null) =>
            QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);

        public void EnsureRelatedItemsAreConfigured(ILevel0Quote? referenceInstance) { }

        public bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false) => false;

        public DummyPQLevel0Quote CopyFrom(DummyPQLevel0Quote source
            , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            this;

        public override ILevel0Quote Clone() => new PQLevel1QuoteTests.DummyLevel1Quote();

        public void SetPricePrecision(decimal precision) { }
        public void SetVolumePrecision(decimal precision) { }
    }
}
