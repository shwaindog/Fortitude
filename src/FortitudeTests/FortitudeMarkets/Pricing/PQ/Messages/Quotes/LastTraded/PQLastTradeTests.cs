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
public class PQLastTradeTests
{
    private PQLastTrade emptyLt     = null!;
    private PQLastTrade populatedLt = null!;
    private DateTime    testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyLt      = new PQLastTrade();
        testDateTime = new DateTime(2017, 12, 17, 16, 11, 52);
        populatedLt  = new PQLastTrade(4.2949_672m, testDateTime);
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt = new PQLastTrade(20, testDateTime);
        Assert.AreEqual(20m, newLt.TradePrice);
        Assert.AreEqual(testDateTime, newLt.TradeTime);
        Assert.IsTrue(newLt.IsTradePriceUpdated);
        Assert.IsTrue(newLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(newLt.IsTradeTimeSub2MinUpdated);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyLt.TradeTime);
        Assert.IsFalse(emptyLt.IsTradePriceUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new PQLastTrade(20, testDateTime);
        var fromPQInstance = new PQLastTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);

        var nonPQLt           = new LastTrade(1.23456m, testDateTime);
        var fromNonPqInstance = new PQLastTrade(nonPQLt);
        Assert.AreEqual(1.23456m, fromNonPqInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromNonPqInstance.TradeTime);
        Assert.IsTrue(fromNonPqInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeSub2MinUpdated);

        var newEmptyLt = new PQLastTrade(emptyLt);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, newEmptyLt.TradeTime);
        Assert.IsFalse(newEmptyLt.IsTradePriceUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedLt = new PQLastTrade(20, testDateTime) { IsTradePriceUpdated = false };
        var fromPQInstance = new PQLastTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.IsFalse(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);

        newPopulatedLt = new PQLastTrade(20, testDateTime) { IsTradeTimeDateUpdated = false };
        fromPQInstance = new PQLastTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);

        newPopulatedLt = new PQLastTrade(20, testDateTime) { IsTradeTimeSub2MinUpdated = false };
        fromPQInstance = new PQLastTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptyLt_TradePriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyLt.IsTradePriceUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.AreEqual(0m, emptyLt.TradePrice);
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const decimal expectedPrice = 2.345678m;
        emptyLt.TradePrice = expectedPrice;
        Assert.IsTrue(emptyLt.IsTradePriceUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(expectedPrice, emptyLt.TradePrice);
        var sourceUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.LastTradedAtPrice, expectedPrice, (PQFieldFlags)1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyLt.IsTradePriceUpdated = false;
        Assert.IsFalse(emptyLt.IsTradePriceUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        const decimal nextExpectedPrice = 2.345677m;
        emptyLt.TradePrice = nextExpectedPrice;
        Assert.IsTrue(emptyLt.IsTradePriceUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(nextExpectedPrice, emptyLt.TradePrice);
        sourceUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.LastTradedAtPrice, nextExpectedPrice, (PQFieldFlags)1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyLt.HasUpdates = false;
        sourceUpdates = (from update in emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.LastTradedAtPrice
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQLastTrade();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedPrice, newEmpty.TradePrice);
        Assert.IsTrue(newEmpty.IsTradePriceUpdated);
    }

    [TestMethod]
    public void EmptyLt_TradeTimeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyLt.TradeTime);
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedDateTime = new DateTime(2018, 1, 6, 16, 34, 35);
        emptyLt.TradeTime = expectedDateTime;
        Assert.IsTrue(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(expectedDateTime, emptyLt.TradeTime);
        var sourceUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);

        var expectedHoursFieldUpdate = new PQFieldUpdate(PQQuoteFields.LastTradedTradeTimeDate,
                                                         expectedDateTime.Get2MinIntervalsFromUnixEpoch());
        var extended                    = expectedDateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHoursBase);
        var expectedSubHoursFieldUpdate = new PQFieldUpdate(PQQuoteFields.LastTradedTradeSub2MinTime, subHoursBase, extended);
        Assert.AreEqual(expectedHoursFieldUpdate, sourceUpdates[0]);
        Assert.AreEqual(expectedSubHoursFieldUpdate, sourceUpdates[1]);

        emptyLt.IsTradeTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(1, emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyLt.IsTradeTimeDateUpdated = false;
        Assert.IsFalse(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var nextExpectedPrice = new DateTime(2018, 1, 6, 15, 35, 35);
        emptyLt.TradeTime = nextExpectedPrice;
        Assert.IsTrue(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(nextExpectedPrice, emptyLt.TradeTime);
        sourceUpdates = emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        expectedHoursFieldUpdate    = new PQFieldUpdate(PQQuoteFields.LastTradedTradeTimeDate, nextExpectedPrice.Get2MinIntervalsFromUnixEpoch());
        extended                    = nextExpectedPrice.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out subHoursBase);
        expectedSubHoursFieldUpdate = new PQFieldUpdate(PQQuoteFields.LastTradedTradeSub2MinTime, subHoursBase, extended);
        Assert.AreEqual(expectedHoursFieldUpdate, sourceUpdates[0]);
        Assert.AreEqual(expectedSubHoursFieldUpdate, sourceUpdates[1]);

        emptyLt.HasUpdates = false;
        sourceUpdates = (from update in emptyLt.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.LastTradedTradeTimeDate ||
                  update.Id == PQQuoteFields.LastTradedTradeSub2MinTime
            select update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        Assert.AreEqual(expectedHoursFieldUpdate, sourceUpdates[0]);
        Assert.AreEqual(expectedSubHoursFieldUpdate, sourceUpdates[1]);

        var newEmpty = new PQLastTrade();
        newEmpty.UpdateField(sourceUpdates[0]);
        newEmpty.UpdateField(sourceUpdates[1]);
        Assert.AreEqual(nextExpectedPrice, newEmpty.TradeTime);
        Assert.IsTrue(newEmpty.IsTradeTimeDateUpdated);
        Assert.IsTrue(newEmpty.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void PopulatedLt_HasUpdates_ClearedAndSetAffectsAllTrackedFields()
    {
        Assert.IsTrue(populatedLt.HasUpdates);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        populatedLt.HasUpdates = false;
        Assert.IsFalse(populatedLt.HasUpdates);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSub2MinUpdated);
        populatedLt.HasUpdates = true;
        Assert.IsTrue(populatedLt.HasUpdates);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.IsTrue(emptyLt.IsEmpty);
    }

    [TestMethod]
    public void PopulatedLt_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.AreNotEqual(0m, populatedLt.TradePrice);
        Assert.AreNotEqual(DateTimeConstants.UnixEpoch, populatedLt.TradeTime);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        populatedLt.StateReset();
        Assert.IsTrue(populatedLt.IsEmpty);
        Assert.AreEqual(0m, populatedLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, populatedLt.TradeTime);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSub2MinUpdated);
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
        var newEmpty = new PQLastTrade();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedLt, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther()
    {
        var nonPQLt = new LastTrade(populatedLt);
        emptyLt.CopyFrom(nonPQLt);
        Assert.AreEqual(populatedLt, emptyLt);
    }

    [TestMethod]
    public void FullyPopulatedLt_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQLastTrade();
        populatedLt.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedLt);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPriceVolumeLayer.TradeTime);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradePriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void FullyPopulatedLt_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedLt = ((ICloneable<ILastTrade>)populatedLt).Clone();
        Assert.AreNotSame(clonedLt, populatedLt);
        Assert.AreEqual(populatedLt, clonedLt);

        var cloned2 = (PQLastTrade)((ICloneable)populatedLt).Clone();
        Assert.AreNotSame(cloned2, populatedLt);
        Assert.AreEqual(populatedLt, cloned2);
    }

    [TestMethod]
    public void FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQLastTrade)((ICloneable)populatedLt).Clone();
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
    }

    public static void AssertContainsAllLtFields(IList<PQFieldUpdate> checkFieldUpdates, IPQLastTrade lt)
    {
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedAtPrice, lt.TradePrice, (PQFieldFlags)1),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQQuoteFields.LastTradedAtPrice, (PQFieldFlags)1), $"For {lt.GetType().Name} ");
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedTradeTimeDate,
                                          lt.TradeTime.Get2MinIntervalsFromUnixEpoch()), PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                         PQQuoteFields.LastTradedTradeTimeDate), $"For {lt.GetType().Name} ");
        var extended = lt.TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedTradeSub2MinTime, lowerFourBytes, extended),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQQuoteFields.LastTradedTradeSub2MinTime), $"For {lt.GetType().Name} ");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison, PQLastTrade? original,
        PQLastTrade changingLastTrade, PQRecentlyTraded? originalRecentlyTraded = null
      , PQRecentlyTraded? changingRecentlyTraded = null,
        PQLevel3Quote? originalQuote = null, PQLevel3Quote? changingQuote = null)
    {
        if (original == null) return;

        if (original.GetType() == typeof(PQLastTrade))
            Assert.AreEqual(!exactComparison
                          , changingLastTrade.AreEquivalent(new LastTrade(original), exactComparison));

        changingLastTrade.TradePrice = 12.34567m;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradePrice = original.TradePrice;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradeTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeTime = original.TradeTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsTradePriceUpdated = !changingLastTrade.IsTradePriceUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.AreEqual(!exactComparison,
                            originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradePriceUpdated = original.IsTradePriceUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsTradeTimeDateUpdated = !changingLastTrade.IsTradeTimeDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.AreEqual(!exactComparison,
                            originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradeTimeDateUpdated = original.IsTradeTimeDateUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsTradeTimeSub2MinUpdated = !changingLastTrade.IsTradeTimeSub2MinUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.AreEqual(!exactComparison,
                            originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradeTimeSub2MinUpdated = original.IsTradeTimeSub2MinUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
