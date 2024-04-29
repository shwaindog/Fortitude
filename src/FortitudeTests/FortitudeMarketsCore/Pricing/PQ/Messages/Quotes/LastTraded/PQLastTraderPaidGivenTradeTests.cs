#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;

[TestClass]
public class PQLastTraderPaidGivenTradeTests
{
    private const string WellKnownTraderName = "TestTraderName";
    private PQLastTraderPaidGivenTrade emptyLt = null!;
    private IPQNameIdLookupGenerator emptyNameIdLookup = null!;
    private IPQNameIdLookupGenerator nameIdLookup = null!;
    private PQLastTraderPaidGivenTrade populatedLt = null!;
    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
        nameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
        emptyLt = new PQLastTraderPaidGivenTrade(emptyNameIdLookup.Clone());
        testDateTime = new DateTime(2017, 12, 17, 16, 11, 52);
        populatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup.Clone(), 4.2949_672m, testDateTime, 42_949_672.95m, true, true)
        {
            TraderName = WellKnownTraderName
        };
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt = new PQLastTraderPaidGivenTrade(nameIdLookup, 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName
        };
        Assert.AreEqual(20m, newLt.TradePrice);
        Assert.AreEqual(testDateTime, newLt.TradeTime);
        Assert.AreEqual(42_949_672.95m, newLt.TradeVolume);
        Assert.IsTrue(newLt.WasGiven);
        Assert.IsTrue(newLt.WasPaid);
        Assert.AreEqual(WellKnownTraderName, newLt.TraderName);
        Assert.IsTrue(newLt.IsTradePriceUpdated);
        Assert.IsTrue(newLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(newLt.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(newLt.IsTradeVolumeUpdated);
        Assert.IsTrue(newLt.IsWasGivenUpdated);
        Assert.IsTrue(newLt.IsWasPaidUpdated);
        Assert.IsTrue(newLt.IsTraderNameUpdated);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyLt.TradeTime);
        Assert.AreEqual(0m, emptyLt.TradeVolume);
        Assert.IsFalse(emptyLt.WasGiven);
        Assert.IsFalse(emptyLt.WasPaid);
        Assert.IsNull(emptyLt.TraderName);
        Assert.IsFalse(emptyLt.IsTradePriceUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeSubHourUpdated);
        Assert.IsFalse(emptyLt.IsTradeVolumeUpdated);
        Assert.IsFalse(emptyLt.IsWasGivenUpdated);
        Assert.IsFalse(emptyLt.IsWasPaidUpdated);
        Assert.IsFalse(emptyLt.IsTraderNameUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup.Clone(), 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName
        };
        var fromPQInstance = new PQLastTraderPaidGivenTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);

        var nonPQLt = new LastTraderPaidGivenTrade(1.23456m, testDateTime, 42_949_672.95m, true, true,
            WellKnownTraderName);
        var fromNonPqInstance = new PQLastTraderPaidGivenTrade(nonPQLt, emptyNameIdLookup.Clone());
        Assert.AreEqual(1.23456m, fromNonPqInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromNonPqInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromNonPqInstance.TradeVolume);
        Assert.IsTrue(fromNonPqInstance.WasGiven);
        Assert.IsTrue(fromNonPqInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromNonPqInstance.TraderName);
        Assert.IsTrue(fromNonPqInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromNonPqInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTraderNameUpdated);

        var newEmptyLt = new PQLastTraderPaidGivenTrade(emptyLt, emptyNameIdLookup);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, newEmptyLt.TradeTime);
        Assert.AreEqual(0m, newEmptyLt.TradeVolume);
        Assert.IsFalse(newEmptyLt.WasGiven);
        Assert.IsFalse(newEmptyLt.WasPaid);
        Assert.IsNull(newEmptyLt.TraderName);
        Assert.IsFalse(newEmptyLt.IsTradePriceUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeSubHourUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeVolumeUpdated);
        Assert.IsFalse(newEmptyLt.IsWasGivenUpdated);
        Assert.IsFalse(newEmptyLt.IsWasPaidUpdated);
        Assert.IsFalse(newEmptyLt.IsTraderNameUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup.Clone(), 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName, IsTradePriceUpdated = false
        };
        var fromPQInstance = new PQLastTraderPaidGivenTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);
        Assert.IsFalse(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);

        newPopulatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup.Clone(), 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName, IsTradeTimeDateUpdated = false
        };
        fromPQInstance = new PQLastTraderPaidGivenTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);

        newPopulatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup, 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName, IsTradeTimeSubHourUpdated = false
        };
        fromPQInstance = new PQLastTraderPaidGivenTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);

        newPopulatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup, 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName, IsTradeVolumeUpdated = false
        };
        fromPQInstance = new PQLastTraderPaidGivenTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSubHourUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);

        newPopulatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup, 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName, IsWasGivenUpdated = false
        };
        fromPQInstance = new PQLastTraderPaidGivenTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);

        newPopulatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup, 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName, IsWasPaidUpdated = false
        };
        fromPQInstance = new PQLastTraderPaidGivenTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsFalse(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);

        newPopulatedLt = new PQLastTraderPaidGivenTrade(nameIdLookup, 20, testDateTime, 42_949_672.95m,
            true, true)
        {
            TraderName = WellKnownTraderName, IsTraderNameUpdated = false
        };
        fromPQInstance = new PQLastTraderPaidGivenTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsFalse(fromPQInstance.IsTraderNameUpdated);
    }

    [TestMethod]
    public void EmptyLt_TraderNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyLt.IsTraderNameUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.AreEqual(null, emptyLt.TraderName);
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        emptyLt.TraderName = WellKnownTraderName;
        Assert.IsTrue(emptyLt.IsTraderNameUpdated);
        Assert.AreEqual(emptyNameIdLookup[WellKnownTraderName], emptyLt.TraderId);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(WellKnownTraderName, emptyLt.TraderName);
        var sourceUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LastTraderIdOffset,
            emptyLt.TraderId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyLt.IsTraderNameUpdated = false;
        Assert.IsFalse(emptyLt.IsTraderNameUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        emptyLt.NameIdLookup.HasUpdates = false;
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var nextExpectedTraderName = "AnotherTraderName";
        emptyLt.TraderName = nextExpectedTraderName;
        Assert.IsTrue(emptyLt.IsTraderNameUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(nextExpectedTraderName, emptyLt.TraderName);
        sourceUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var stringUpdates = emptyLt.GetStringUpdates(testDateTime, PQMessageFlags.Update)
            .ToList();
        Assert.AreEqual(1, stringUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LastTraderIdOffset,
            emptyLt.TraderId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
        var expectedStringUpdates = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(
                PQFieldKeys.LastTraderDictionaryUpsertCommand, 0u, PQFieldFlags.IsUpsert)
            , StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.Upsert, DictionaryId = emptyLt.NameIdLookup[emptyLt.TraderName]
                , Value = emptyLt.TraderName
            }
        };
        Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

        emptyLt.HasUpdates = false;
        sourceUpdates = (from update in emptyLt.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFieldKeys.LastTraderIdOffset
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmptyNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
        var newEmpty = new PQLastTraderPaidGivenTrade(newEmptyNameIdLookup);
        newEmpty.UpdateField(sourceUpdates[0]);
        newEmpty.UpdateFieldString(stringUpdates[0]);
        Assert.AreEqual(nextExpectedTraderName, newEmpty.TraderName);
        Assert.IsTrue(newEmpty.IsTraderNameUpdated);
    }

    [TestMethod]
    public void PopulatedLt_HasUpdates_ClearedAndSetAffectsAllTrackedFields()
    {
        Assert.IsTrue(populatedLt.HasUpdates);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
        Assert.IsTrue(populatedLt.IsTraderNameUpdated);
        populatedLt.HasUpdates = false;
        Assert.IsFalse(populatedLt.HasUpdates);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSubHourUpdated);
        Assert.IsFalse(populatedLt.IsTradeVolumeUpdated);
        Assert.IsFalse(populatedLt.IsWasGivenUpdated);
        Assert.IsFalse(populatedLt.IsWasPaidUpdated);
        Assert.IsFalse(populatedLt.IsTraderNameUpdated);
        populatedLt.HasUpdates = true;
        Assert.IsTrue(populatedLt.HasUpdates);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
        Assert.IsTrue(populatedLt.IsTraderNameUpdated);
    }

    [TestMethod]
    public void PopulatedLt_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.AreNotEqual(0m, populatedLt.TradePrice);
        Assert.AreNotEqual(DateTimeConstants.UnixEpoch, populatedLt.TradeTime);
        Assert.AreNotEqual(0m, populatedLt.TradeVolume);
        Assert.IsTrue(populatedLt.WasGiven);
        Assert.IsTrue(populatedLt.WasPaid);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSubHourUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
        Assert.IsTrue(populatedLt.IsTraderNameUpdated);
        populatedLt.StateReset();
        Assert.IsTrue(populatedLt.IsEmpty);
        Assert.AreEqual(0m, populatedLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, populatedLt.TradeTime);
        Assert.AreEqual(0m, populatedLt.TradeVolume);
        Assert.IsFalse(populatedLt.WasGiven);
        Assert.IsFalse(populatedLt.WasPaid);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSubHourUpdated);
        Assert.IsFalse(populatedLt.IsTradeVolumeUpdated);
        Assert.IsFalse(populatedLt.IsWasGivenUpdated);
        Assert.IsFalse(populatedLt.IsWasPaidUpdated);
        Assert.IsFalse(populatedLt.IsTraderNameUpdated);
    }

    [TestMethod]
    public void EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.IsTrue(emptyLt.IsEmpty);
    }

    [TestMethod]
    public void PopulatedLtWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates = populatedLt.GetDeltaUpdateFields(
            new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllLtFields(pqFieldUpdates, populatedLt);
    }

    [TestMethod]
    public void PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedLt.HasUpdates = false;
        var pqFieldUpdates = populatedLt.GetDeltaUpdateFields(
            new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllLtFields(pqFieldUpdates, populatedLt);
    }

    [TestMethod]
    public void PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedLt.HasUpdates = false;
        var pqFieldUpdates = populatedLt.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedLt_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewLt_CopiesAllFieldsToNewLt()
    {
        var pqFieldUpdates = populatedLt.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 13, 33, 3), PQMessageFlags.Update | PQMessageFlags.Replay).ToList();
        var pqStringUpdates = populatedLt.GetStringUpdates(
            new DateTime(2017, 11, 04, 13, 33, 3), PQMessageFlags.Update | PQMessageFlags.Replay).ToList();
        var newEmpty = new PQLastTraderPaidGivenTrade(nameIdLookup);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
        Assert.AreEqual(populatedLt, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther()
    {
        var nonPQLt = new LastTraderPaidGivenTrade(populatedLt);
        emptyLt.CopyFrom(nonPQLt);
        Assert.AreEqual(populatedLt, emptyLt);
    }

    [TestMethod]
    public void FullyPopulatedLt_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQLastTraderPaidGivenTrade(nameIdLookup);
        populatedLt.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedLt);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPriceVolumeLayer.TradeTime);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.TradeVolume);
        Assert.IsFalse(emptyPriceVolumeLayer.WasGiven);
        Assert.IsFalse(emptyPriceVolumeLayer.WasPaid);
        Assert.IsNull(emptyPriceVolumeLayer.TraderName);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradePriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeTimeSubHourUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsWasGivenUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsWasPaidUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTraderNameUpdated);
    }

    [TestMethod]
    public void FromInterfacePopulatedLastTrade_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = populatedLt.Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((ICloneable<ILastTrade>)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((ILastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IMutableLastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((ILastTraderPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IMutableLastTraderPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IPQLastTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IPQLastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IPQLastTraderPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
    }

    [TestMethod]
    public void FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQLastTraderPaidGivenTrade)((ICloneable)populatedLt).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedLt,
            fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedLt,
            fullyPopulatedClone);
    }

    public static void AssertContainsAllLtFields(IList<PQFieldUpdate> checkFieldUpdates,
        IPQLastTraderPaidGivenTrade lt)
    {
        PQLastPaidGivenTradeTests.AssertContainsAllLtFields(checkFieldUpdates, lt);

        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LastTraderIdOffset, lt.TraderId),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                PQFieldKeys.LastTraderIdOffset), $"For asklayer {lt.GetType().Name}");
    }

    [TestMethod]
    public void FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedLt, populatedLt);
        Assert.AreEqual(populatedLt, ((ICloneable)populatedLt).Clone());
        Assert.AreEqual(populatedLt, ((ICloneable<ILastTrade>)populatedLt).Clone());
    }

    [TestMethod]
    public void FullyPopulatedPvl_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedLt.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedLt.ToString();

        Assert.IsTrue(toString.Contains(populatedLt.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradePrice)}: {populatedLt.TradePrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeTime)}: {populatedLt.TradeTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeVolume)}: {populatedLt.TradeVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasGiven)}: {populatedLt.WasGiven}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasPaid)}: {populatedLt.WasPaid}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TraderName)}: {populatedLt.TraderName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        PQLastTraderPaidGivenTrade? original, PQLastTraderPaidGivenTrade? changingLastTraderPaidGivenTrade = null,
        PQRecentlyTraded? originalRecentlyTraded = null, PQRecentlyTraded? changingRecentlyTraded = null,
        PQLevel3Quote? originalQuote = null, PQLevel3Quote? changingQuote = null)
    {
        if (original == null) return;


        PQLastPaidGivenTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison, original,
            changingLastTraderPaidGivenTrade, originalRecentlyTraded, changingRecentlyTraded, originalQuote
            , changingQuote);

        if (original.GetType() == typeof(PQLastTraderPaidGivenTrade))
            Assert.AreEqual(!exactComparison, changingLastTraderPaidGivenTrade!.AreEquivalent(
                new LastTraderPaidGivenTrade(original), exactComparison));

        changingLastTraderPaidGivenTrade!.TraderName = "Changed Trader Name";
        Assert.IsFalse(original.AreEquivalent(changingLastTraderPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTraderPaidGivenTrade.TraderName = original.TraderName;
        Assert.IsTrue(changingLastTraderPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTraderPaidGivenTrade.IsTraderNameUpdated = !changingLastTraderPaidGivenTrade.IsTraderNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTraderPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.AreEqual(!exactComparison,
                originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTraderPaidGivenTrade.IsTraderNameUpdated = original.IsTraderNameUpdated;
        Assert.IsTrue(changingLastTraderPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
