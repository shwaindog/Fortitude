// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;

[TestClass]
public class PQLastPaidGivenTradeTests
{
    private PQLastPaidGivenTrade emptyLt     = null!;
    private PQLastPaidGivenTrade populatedLt = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyLt      = new PQLastPaidGivenTrade();
        testDateTime = new DateTime(2017, 12, 17, 16, 11, 52);
        populatedLt  = new PQLastPaidGivenTrade(4.2949_672m, testDateTime, 42_949_672.95m, true, true);
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt = new PQLastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true);
        Assert.AreEqual(20m, newLt.TradePrice);
        Assert.AreEqual(testDateTime, newLt.TradeTime);
        Assert.AreEqual(42_949_672.95m, newLt.TradeVolume);
        Assert.IsTrue(newLt.WasGiven);
        Assert.IsTrue(newLt.WasPaid);
        Assert.IsTrue(newLt.IsTradePriceUpdated);
        Assert.IsTrue(newLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(newLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(newLt.IsTradeVolumeUpdated);
        Assert.IsTrue(newLt.IsWasGivenUpdated);
        Assert.IsTrue(newLt.IsWasPaidUpdated);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, emptyLt.TradeTime);
        Assert.AreEqual(0m, emptyLt.TradeVolume);
        Assert.IsFalse(emptyLt.WasGiven);
        Assert.IsFalse(emptyLt.WasPaid);
        Assert.IsFalse(emptyLt.IsTradePriceUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(emptyLt.IsTradeVolumeUpdated);
        Assert.IsFalse(emptyLt.IsWasGivenUpdated);
        Assert.IsFalse(emptyLt.IsWasPaidUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new PQLastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true);
        var fromPQInstance = new PQLastPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);

        var nonPQLt           = new LastPaidGivenTrade(1.23456m, testDateTime, 42_949_672.95m, true, true);
        var fromNonPqInstance = new PQLastPaidGivenTrade(nonPQLt);
        Assert.AreEqual(1.23456m, fromNonPqInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromNonPqInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromNonPqInstance.TradeVolume);
        Assert.IsTrue(fromNonPqInstance.WasGiven);
        Assert.IsTrue(fromNonPqInstance.WasPaid);
        Assert.IsTrue(fromNonPqInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromNonPqInstance.IsWasPaidUpdated);

        var newEmptyLt = new PQLastPaidGivenTrade(emptyLt);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, newEmptyLt.TradeTime);
        Assert.AreEqual(0m, newEmptyLt.TradeVolume);
        Assert.IsFalse(newEmptyLt.WasGiven);
        Assert.IsFalse(newEmptyLt.WasPaid);
        Assert.IsFalse(newEmptyLt.IsTradePriceUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeVolumeUpdated);
        Assert.IsFalse(newEmptyLt.IsWasGivenUpdated);
        Assert.IsFalse(newEmptyLt.IsWasPaidUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedLt = new PQLastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true)
            { IsTradePriceUpdated = false };
        var fromPQInstance = new PQLastPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.IsFalse(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);

        newPopulatedLt = new PQLastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true)
            { IsTradeTimeDateUpdated = false };
        fromPQInstance = new PQLastPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);

        newPopulatedLt = new PQLastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true)
            { IsTradeTimeSub2MinUpdated = false };
        fromPQInstance = new PQLastPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);

        newPopulatedLt = new PQLastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true)
            { IsTradeVolumeUpdated = false };
        fromPQInstance = new PQLastPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);

        newPopulatedLt = new PQLastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true)
            { IsWasGivenUpdated = false };
        fromPQInstance = new PQLastPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);

        newPopulatedLt = new PQLastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true)
            { IsWasPaidUpdated = false };
        fromPQInstance = new PQLastPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsFalse(fromPQInstance.IsWasPaidUpdated);
    }

    [TestMethod]
    public void EmptyLt_TradeVolumeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyLt.IsTradeVolumeUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.AreEqual(0m, emptyLt.TradeVolume);
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const decimal expectedVolume = 42_949_672.95m;
        emptyLt.TradeVolume = expectedVolume;
        Assert.IsTrue(emptyLt.IsTradeVolumeUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(expectedVolume, emptyLt.TradeVolume);
        var sourceUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQSubFieldKeys.LastTradedOrderVolume, expectedVolume, (PQFieldFlags)6);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyLt.IsTradeVolumeUpdated = false;
        Assert.IsFalse(emptyLt.IsTradeVolumeUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        const decimal nextExpectedVolume = 0.11m;
        emptyLt.TradeVolume = nextExpectedVolume;
        Assert.IsTrue(emptyLt.IsTradeVolumeUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(nextExpectedVolume, emptyLt.TradeVolume);
        sourceUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQSubFieldKeys.LastTradedOrderVolume, nextExpectedVolume, (PQFieldFlags)6);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyLt.HasUpdates = false;
        sourceUpdates = (from update in emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.SubId == PQSubFieldKeys.LastTradedOrderVolume
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQLastPaidGivenTrade();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedVolume, newEmpty.TradeVolume);
        Assert.IsTrue(newEmpty.IsTradeVolumeUpdated);
    }

    [TestMethod]
    public void EmptyLt_WasGivenChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyLt.IsWasGivenUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsFalse(emptyLt.WasGiven);
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyLt.WasGiven = true;
        Assert.IsTrue(emptyLt.IsWasGivenUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.WasGiven);
        var sourceLayerUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasGiven);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        emptyLt.IsWasGivenUpdated = false;
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyLt.IsWasGivenUpdated = true;
        sourceLayerUpdates =
            (from update in emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.SubId == PQSubFieldKeys.LastTradedBooleanFlags
                select update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        var newEmpty = new PQLastPaidGivenTrade();
        newEmpty.UpdateField(sourceLayerUpdates[0]);
        Assert.IsTrue(newEmpty.WasGiven);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsWasGivenUpdated);
    }

    [TestMethod]
    public void EmptyLt_WasPaidChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyLt.IsWasPaidUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsFalse(emptyLt.WasPaid);
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyLt.WasPaid = true;
        Assert.IsTrue(emptyLt.IsWasPaidUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.WasPaid);
        var sourceLayerUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasPaid);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        emptyLt.IsWasPaidUpdated = false;
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyLt.IsWasPaidUpdated = true;
        sourceLayerUpdates =
            (from update in emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.SubId == PQSubFieldKeys.LastTradedBooleanFlags
                select update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        var newEmpty = new PQLastPaidGivenTrade();
        newEmpty.UpdateField(sourceLayerUpdates[0]);
        Assert.IsTrue(newEmpty.WasPaid);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsWasPaidUpdated);
    }

    [TestMethod]
    public void PopulatedLt_HasUpdates_ClearedAndSetAffectsAllTrackedFields()
    {
        Assert.IsTrue(populatedLt.HasUpdates);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
        populatedLt.HasUpdates = false;
        Assert.IsFalse(populatedLt.HasUpdates);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(populatedLt.IsTradeVolumeUpdated);
        Assert.IsFalse(populatedLt.IsWasGivenUpdated);
        Assert.IsFalse(populatedLt.IsWasPaidUpdated);
        populatedLt.HasUpdates = true;
        Assert.IsTrue(populatedLt.HasUpdates);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
    }

    [TestMethod]
    public void PopulatedLt_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.AreNotEqual(0m, populatedLt.TradePrice);
        Assert.AreNotEqual(DateTime.MinValue, populatedLt.TradeTime);
        Assert.AreNotEqual(0m, populatedLt.TradeVolume);
        Assert.IsTrue(populatedLt.WasGiven);
        Assert.IsTrue(populatedLt.WasPaid);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
        populatedLt.StateReset();
        Assert.IsTrue(populatedLt.IsEmpty);
        Assert.AreEqual(0m, populatedLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, populatedLt.TradeTime);
        Assert.AreEqual(0m, populatedLt.TradeVolume);
        Assert.IsFalse(populatedLt.WasGiven);
        Assert.IsFalse(populatedLt.WasPaid);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(populatedLt.IsTradeVolumeUpdated);
        Assert.IsFalse(populatedLt.IsWasGivenUpdated);
        Assert.IsFalse(populatedLt.IsWasPaidUpdated);
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
                                                              new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllLtFields(pqFieldUpdates, populatedLt);
    }

    [TestMethod]
    public void PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedLt.HasUpdates = false;
        var pqFieldUpdates = populatedLt.GetDeltaUpdateFields(
                                                              new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Snapshot).ToList();
        AssertContainsAllLtFields(pqFieldUpdates, populatedLt);
    }

    [TestMethod]
    public void PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedLt.HasUpdates = false;
        var pqFieldUpdates = populatedLt.GetDeltaUpdateFields(
                                                              new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedLt_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewLt_CopiesAllFieldsToNewLt()
    {
        var pqFieldUpdates = populatedLt.GetDeltaUpdateFields(
                                                              new DateTime(2017, 11, 04, 13, 33, 3)
                                                            , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQLastPaidGivenTrade();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedLt, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther()
    {
        var nonPQLt = new LastPaidGivenTrade(populatedLt);
        emptyLt.CopyFrom(nonPQLt);
        Assert.AreEqual(populatedLt, emptyLt);
    }

    [TestMethod]
    public void FullyPopulatedLt_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQLastPaidGivenTrade();
        populatedLt.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedLt);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.TradePrice);
        Assert.AreEqual(DateTime.MinValue, emptyPriceVolumeLayer.TradeTime);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.TradeVolume);
        Assert.IsFalse(emptyPriceVolumeLayer.WasGiven);
        Assert.IsFalse(emptyPriceVolumeLayer.WasPaid);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradePriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsWasGivenUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsWasPaidUpdated);
    }

    [TestMethod]
    public void FullyPopulatedLt_Clone_ClonedInstanceEqualsOriginal()
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
        clone = ((IPQLastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
    }

    [TestMethod]
    public void FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQLastPaidGivenTrade)((ICloneable)populatedLt).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedLt,
                                                            fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedLt,
                                                            fullyPopulatedClone);
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
    }

    public static void AssertContainsAllLtFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        IPQLastPaidGivenTrade lt)
    {
        PQLastTradeTests.AssertContainsAllLtFields(checkFieldUpdates, lt);

        var noVal = LastTradeBooleanFlags.None;

        var expectedFlag = lt.WasGiven ? LastTradeBooleanFlags.WasGiven : noVal;
        expectedFlag |= lt.WasPaid ? LastTradeBooleanFlags.WasPaid : noVal;

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQSubFieldKeys.LastTradedBooleanFlags,
                                          (uint)expectedFlag),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQQuoteFields.LastTradedTickTrades, PQSubFieldKeys.LastTradedBooleanFlags), $"For asklayer {lt.GetType().Name}");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQLastPaidGivenTrade? original, PQLastPaidGivenTrade? changingLastPaidGivenTrade,
        PQRecentlyTraded? originalRecentlyTraded = null, PQRecentlyTraded? changingRecentlyTraded = null,
        PQLevel3Quote? originalQuote = null, PQLevel3Quote? changingQuote = null)
    {
        if (original == null || changingLastPaidGivenTrade == null) return;

        PQLastTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison, original,
                                                                             changingLastPaidGivenTrade, originalRecentlyTraded
                                                                           , changingRecentlyTraded, originalQuote,
                                                                             changingQuote);

        if (original.GetType() == typeof(PQLastPaidGivenTrade))
            Assert.AreEqual(!exactComparison, changingLastPaidGivenTrade.AreEquivalent(
                                                                                       new LastPaidGivenTrade(original), exactComparison));

        changingLastPaidGivenTrade.TradeVolume = 1_234_567m;
        Assert.IsFalse(original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.TradeVolume = original.TradeVolume;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastPaidGivenTrade.WasGiven = !changingLastPaidGivenTrade.WasGiven;
        Assert.IsFalse(original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.WasGiven          = original.WasGiven;
        changingLastPaidGivenTrade.IsWasGivenUpdated = original.IsWasGivenUpdated;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastPaidGivenTrade.WasPaid = !changingLastPaidGivenTrade.WasPaid;
        Assert.IsFalse(original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.WasPaid          = original.WasPaid;
        changingLastPaidGivenTrade.IsWasPaidUpdated = original.IsWasPaidUpdated;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastPaidGivenTrade.IsTradeVolumeUpdated = !changingLastPaidGivenTrade.IsTradeVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.AreEqual(!exactComparison,
                            originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.IsTradeVolumeUpdated = original.IsTradeVolumeUpdated;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastPaidGivenTrade.IsWasGivenUpdated = !changingLastPaidGivenTrade.IsWasGivenUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.AreEqual(!exactComparison,
                            originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.IsWasGivenUpdated = original.IsWasGivenUpdated;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastPaidGivenTrade.IsWasPaidUpdated = !changingLastPaidGivenTrade.IsWasPaidUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.AreEqual(!exactComparison,
                            originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.IsWasPaidUpdated = original.IsWasPaidUpdated;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
