// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[TestClass]
public class PQLastTradeTests
{
    private const uint    ExpectedTradeId    = 42;
    private const uint    ExpectedBatchId    = 24_942;
    private const decimal ExpectedTradePrice = 2.3456m;

    private const LastTradedTypeFlags      ExpectedTradedTypeFlags     = LastTradedTypeFlags.HasPaidGivenDetails;
    private const LastTradedLifeCycleFlags ExpectedTradeLifeCycleFlags = LastTradedLifeCycleFlags.Confirmed;

    private static readonly DateTime ExpectedTradeTime           = new(2018, 03, 2, 14, 40, 30);
    private static readonly DateTime ExpectedFirstNotifiedTime   = new(2018, 03, 2, 14, 40, 31);
    private static readonly DateTime ExpectedAdapterReceivedTime = new(2018, 03, 2, 14, 40, 41);
    private static readonly DateTime ExpectedUpdateTime          = new(2018, 03, 2, 14, 40, 42);


    private PQLastTrade emptyLt     = null!;
    private PQLastTrade populatedLt = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyLt      = new PQLastTrade();
        populatedLt = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                    , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                  , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        Assert.AreEqual(ExpectedTradePrice, newLt.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, newLt.TradeTime);
        Assert.IsTrue(newLt.IsTradePriceUpdated);
        Assert.IsTrue(newLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(newLt.IsTradeTimeSub2MinUpdated);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, emptyLt.TradeTime);
        Assert.IsFalse(emptyLt.IsTradePriceUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                           , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        var fromPQInstance = new PQLastTrade(newPopulatedLt);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);

        var nonPQLt = new LastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                  , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        var fromNonPqInstance = new PQLastTrade(nonPQLt);
        Assert.AreEqual(ExpectedTradePrice, fromNonPqInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromNonPqInstance.TradeTime);
        Assert.IsTrue(fromNonPqInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeSub2MinUpdated);

        var newEmptyLt = new PQLastTrade(emptyLt);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, newEmptyLt.TradeTime);
        Assert.IsFalse(newEmptyLt.IsTradePriceUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedLt = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                           , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime)
            { IsTradePriceUpdated = false };
        var fromPQInstance = new PQLastTrade(newPopulatedLt);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.IsFalse(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);

        newPopulatedLt = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                       , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime)
            { IsTradeTimeDateUpdated = false };
        fromPQInstance = new PQLastTrade(newPopulatedLt);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);

        newPopulatedLt = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                       , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime)
            { IsTradeTimeSub2MinUpdated = false };
        fromPQInstance = new PQLastTrade(newPopulatedLt);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
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
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).Count());

        const decimal expectedPrice = 2.345678m;
        emptyLt.TradePrice = expectedPrice;
        Assert.IsTrue(emptyLt.IsTradePriceUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(expectedPrice, emptyLt.TradePrice);
        var sourceUpdates = emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate
            = new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAtPrice, expectedPrice, (PQFieldFlags)1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyLt.IsTradePriceUpdated = false;
        Assert.IsFalse(emptyLt.IsTradePriceUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).IsNullOrNone());

        const decimal nextExpectedPrice = 2.345677m;
        emptyLt.TradePrice = nextExpectedPrice;
        Assert.IsTrue(emptyLt.IsTradePriceUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(nextExpectedPrice, emptyLt.TradePrice);
        sourceUpdates = emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate
            = new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAtPrice, nextExpectedPrice, (PQFieldFlags)1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyLt.HasUpdates = false;
        sourceUpdates = (from update in emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Snapshot)
            where update.TradingSubId == PQTradingSubFieldKeys.LastTradedAtPrice
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
        Assert.AreEqual(DateTime.MinValue, emptyLt.TradeTime);
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).Count());

        var expectedDateTime = new DateTime(2018, 1, 6, 16, 34, 35);
        emptyLt.TradeTime = expectedDateTime;
        Assert.IsTrue(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(expectedDateTime, emptyLt.TradeTime);
        var sourceUpdates = emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);

        var expectedHoursFieldUpdate = new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeTimeDate,
                                                         expectedDateTime.Get2MinIntervalsFromUnixEpoch());
        var extended = expectedDateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHoursBase);
        var expectedSub2Min
            = new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHoursBase, extended);
        Assert.AreEqual(expectedHoursFieldUpdate, sourceUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceUpdates[1]);

        emptyLt.IsTradeTimeSub2MinUpdated = false;
        Assert.IsFalse(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(1, emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).Count());

        emptyLt.IsTradeTimeDateUpdated = false;
        Assert.IsFalse(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).IsNullOrNone());

        var nextExpectedPrice = new DateTime(2018, 1, 6, 15, 35, 35);
        emptyLt.TradeTime = nextExpectedPrice;
        Assert.IsTrue(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(nextExpectedPrice, emptyLt.TradeTime);
        sourceUpdates = emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        expectedHoursFieldUpdate =
            new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeTimeDate,
                              nextExpectedPrice.Get2MinIntervalsFromUnixEpoch());
        extended = nextExpectedPrice.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out subHoursBase);
        expectedSub2Min =
            new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHoursBase, extended);
        Assert.AreEqual(expectedHoursFieldUpdate, sourceUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceUpdates[1]);

        emptyLt.HasUpdates = false;
        sourceUpdates = (from update in emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Snapshot)
            where update.TradingSubId == PQTradingSubFieldKeys.LastTradedTradeTimeDate ||
                  update.TradingSubId == PQTradingSubFieldKeys.LastTradedTradeSub2MinTime
            select update).ToList();
        Assert.AreEqual(2, sourceUpdates.Count);
        Assert.AreEqual(expectedHoursFieldUpdate, sourceUpdates[0]);
        Assert.AreEqual(expectedSub2Min, sourceUpdates[1]);

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
        Assert.AreNotEqual(DateTime.MinValue, populatedLt.TradeTime);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        populatedLt.StateReset();
        Assert.IsTrue(populatedLt.IsEmpty);
        Assert.AreEqual(0m, populatedLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, populatedLt.TradeTime);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSub2MinUpdated);
    }

    [TestMethod]
    public void PopulatedLtWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates =
            populatedLt.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllLtFields(pqFieldUpdates, populatedLt);
    }

    [TestMethod]
    public void PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedLt.HasUpdates = false;
        var pqFieldUpdates =
            populatedLt.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllLtFields(pqFieldUpdates, populatedLt);
    }

    [TestMethod]
    public void PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedLt.HasUpdates = false;
        var pqFieldUpdates =
            populatedLt.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedLt_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewLt_CopiesAllFieldsToNewLt()
    {
        var pqFieldUpdates =
            populatedLt.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
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
        Assert.AreEqual(DateTime.MinValue, emptyPriceVolumeLayer.TradeTime);
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeId)}: {populatedLt.TradeId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.BatchId)}: {populatedLt.BatchId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradePrice)}: {populatedLt.TradePrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeTime)}: {populatedLt.TradeTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeTypeFlags)}: {populatedLt.TradeTypeFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeLifeCycleStatus)}: {populatedLt.TradeLifeCycleStatus}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.FirstNotifiedTime)}: {populatedLt.FirstNotifiedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.AdapterReceivedTime)}: {populatedLt.AdapterReceivedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.UpdateTime)}: {populatedLt.UpdateTime:O}"));
    }

    public static void AssertContainsAllLtFields(IList<PQFieldUpdate> checkFieldUpdates, IPQLastTrade lt)
    {
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeId, lt.TradeId),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeId)
                      , $"For lastTrade {lt.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedBatchId, lt.BatchId),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedBatchId)
                      , $"For lastTrade {lt.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAtPrice, lt.TradePrice, (PQFieldFlags)1),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAtPrice, (PQFieldFlags)1)
                      , $"For lastTrade {lt.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeTimeDate,
                                          lt.TradeTime.Get2MinIntervalsFromUnixEpoch())
                      , PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeTimeDate)
                      , $"For lastTrade {lt.GetType().Name} ");

        var extended = lt.TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, lowerFourBytes, extended)
                       , PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime)
                      , $"For lastTrade {lt.GetType().Name} ");
        

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTypeFlags, (uint)lt.TradeTypeFlags),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTypeFlags)
                      , $"For lastTrade {lt.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedLifeCycleStatus, (uint)lt.TradeLifeCycleStatus),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedLifeCycleStatus)
                      , $"For lastTrade {lt.GetType().Name} ");
        
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedFirstNotifiedDate,
                                          lt.FirstNotifiedTime.Get2MinIntervalsFromUnixEpoch())
                      , PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedFirstNotifiedDate)
                      , $"For lastTrade {lt.GetType().Name} ");

        extended = lt.FirstNotifiedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedFirstNotifiedSub2MinTime, lowerFourBytes, extended)
                       , PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedFirstNotifiedSub2MinTime)
                      , $"For lastTrade {lt.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAdapterReceivedDate,
                                          lt.AdapterReceivedTime.Get2MinIntervalsFromUnixEpoch())
                      , PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAdapterReceivedDate)
                      , $"For lastTrade {lt.GetType().Name} ");

        extended = lt.AdapterReceivedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAdapterReceivedSub2MinTime, lowerFourBytes, extended)
                       , PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAdapterReceivedSub2MinTime)
                      , $"For lastTrade {lt.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedUpdateDate,
                                          lt.UpdateTime.Get2MinIntervalsFromUnixEpoch())
                      , PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedUpdateDate)
                      , $"For lastTrade {lt.GetType().Name} ");

        extended = lt.UpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out lowerFourBytes);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedUpdateSub2MinTime, lowerFourBytes, extended)
                       , PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedUpdateSub2MinTime)
                      , $"For lastTrade {lt.GetType().Name} ");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison, PQLastTrade? original,
        PQLastTrade changingLastTrade, PQLastTradedList? originalLastTradedList = null
      , PQLastTradedList? changingLastTradedList = null,
        PQPublishableLevel3Quote? originalQuote = null, PQPublishableLevel3Quote? changingQuote = null)
    {
        if (original == null) return;

        if (original.GetType() == typeof(PQLastTrade))
            Assert.AreEqual(!exactComparison
                          , changingLastTrade.AreEquivalent(new LastTrade(original), exactComparison));

        changingLastTrade.TradeId = 77_889;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeId = original.TradeId;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.BatchId = 277_889;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.BatchId = original.BatchId;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradePrice = 12.34567m;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradePrice = original.TradePrice;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradeTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeTime = original.TradeTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradeTypeFlags = LastTradedTypeFlags.HasPaidGivenDetails | LastTradedTypeFlags.IsInternalOrderTradeUpdate;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeTypeFlags = original.TradeTypeFlags;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradeLifeCycleStatus = LastTradedLifeCycleFlags.Confirmed | LastTradedLifeCycleFlags.Rejected;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeLifeCycleStatus = original.TradeLifeCycleStatus;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.FirstNotifiedTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.FirstNotifiedTime = original.FirstNotifiedTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.AdapterReceivedTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.AdapterReceivedTime = original.AdapterReceivedTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.UpdateTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.UpdateTime = original.UpdateTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsTradeIdUpdated = !changingLastTrade.IsTradeIdUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradeIdUpdated = original.IsTradeIdUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsBatchIdUpdated = !changingLastTrade.IsBatchIdUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsBatchIdUpdated = original.IsBatchIdUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsTradePriceUpdated = !changingLastTrade.IsTradePriceUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradePriceUpdated = original.IsTradePriceUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsTradeTimeDateUpdated = !changingLastTrade.IsTradeTimeDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradeTimeDateUpdated = original.IsTradeTimeDateUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsTradeTimeSub2MinUpdated = !changingLastTrade.IsTradeTimeSub2MinUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradeTimeSub2MinUpdated = original.IsTradeTimeSub2MinUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
        
        changingLastTrade.IsTradeTypeFlagsUpdated = !changingLastTrade.IsTradeTypeFlagsUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradeTypeFlagsUpdated = original.IsTradeTypeFlagsUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
        
        changingLastTrade.IsTradeLifeCycleStatusUpdated = !changingLastTrade.IsTradeLifeCycleStatusUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsTradeLifeCycleStatusUpdated = original.IsTradeLifeCycleStatusUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsFirstNotifiedDateUpdated = !changingLastTrade.IsFirstNotifiedDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsFirstNotifiedDateUpdated = original.IsFirstNotifiedDateUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsFirstNotifiedSub2MinTimeUpdated = !changingLastTrade.IsFirstNotifiedSub2MinTimeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsFirstNotifiedSub2MinTimeUpdated = original.IsFirstNotifiedSub2MinTimeUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsAdapterReceivedDateUpdated = !changingLastTrade.IsAdapterReceivedDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsAdapterReceivedDateUpdated = original.IsAdapterReceivedDateUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsAdapterReceivedSub2MinTimeUpdated = !changingLastTrade.IsAdapterReceivedSub2MinTimeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsAdapterReceivedSub2MinTimeUpdated = original.IsAdapterReceivedSub2MinTimeUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsUpdatedDateUpdated = !changingLastTrade.IsUpdatedDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsUpdatedDateUpdated = original.IsUpdatedDateUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.IsUpdateSub2MinTimeUpdated = !changingLastTrade.IsUpdateSub2MinTimeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.AreEqual(!exactComparison,
                            originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.IsUpdateSub2MinTimeUpdated = original.IsUpdateSub2MinTimeUpdated;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
