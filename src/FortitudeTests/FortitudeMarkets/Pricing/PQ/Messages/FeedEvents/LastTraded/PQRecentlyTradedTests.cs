// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[TestClass]
public class PQRecentlyTradedTests
{
    private const uint    ExpectedTradeId     = 42;
    private const uint    ExpectedBatchId     = 24_942;
    private const uint    ExpectedOrderId     = 1_772_942;
    private const decimal ExpectedTradePrice  = 2.3456m;
    private const decimal ExpectedTradeVolume = 42_000_111m;

    private const LastTradedTypeFlags      ExpectedTradedTypeFlags     = LastTradedTypeFlags.HasPaidGivenDetails;
    private const LastTradedLifeCycleFlags ExpectedTradeLifeCycleFlags = LastTradedLifeCycleFlags.Confirmed;

    private static readonly DateTime ExpectedTradeTime           = new(2018, 03, 2, 14, 40, 30);
    private static readonly DateTime ExpectedFirstNotifiedTime   = new(2018, 03, 2, 14, 40, 31);
    private static readonly DateTime ExpectedAdapterReceivedTime = new(2018, 03, 2, 14, 40, 41);
    private static readonly DateTime ExpectedUpdateTime          = new(2018, 03, 2, 14, 40, 42);

    private const bool ExpectedWasGiven = true;
    private const bool ExpectedWasPaid  = true;

    private const int ExpectedTraderId       = 34_902;
    private const int ExpectedCounterPartyId = 2_198;

    private const string ExpectedTraderName       = "TraderName-Helen";
    private const string ExpectedCounterPartyName = "CounterPartyName-Valcopp";

    private const int MaxNumberOfEntries = QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades;

    private IList<PQRecentlyTraded>                 allFullyPopulatedRecentlyTraded = null!;
    private List<IReadOnlyList<IPQLastTrade>>       allPopulatedEntries             = null!;
    private IList<IPQLastPaidGivenTrade>            lastPaidGivenEntries            = null!;
    private IList<IPQLastExternalCounterPartyTrade> lastTraderPaidGivenEntries      = null!;

    private PQRecentlyTraded paidGivenVolumeRecentlyTradedFullyPopulatedLastTrades = null!;

    private IList<IPQLastTrade> simpleEntries = null!;

    private PQRecentlyTraded        simpleRecentlyTradedFullyPopulatedLastTrades      = null!;
    private PQNameIdLookupGenerator traderNameIdLookupGenerator                       = null!;
    private PQRecentlyTraded        fullSupportRecentlyTradedFullyPopulatedLastTrades = null!;
    // test being less than max.

    [TestInitialize]
    public void SetUp()
    {
        traderNameIdLookupGenerator = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);

        simpleEntries              = new List<IPQLastTrade>(MaxNumberOfEntries);
        lastPaidGivenEntries       = new List<IPQLastPaidGivenTrade>(MaxNumberOfEntries);
        lastTraderPaidGivenEntries = new List<IPQLastExternalCounterPartyTrade>(MaxNumberOfEntries);

        allPopulatedEntries = new List<IReadOnlyList<IPQLastTrade>>
        {
            (IReadOnlyList<IPQLastTrade>)simpleEntries, (IReadOnlyList<IPQLastTrade>)lastPaidGivenEntries
          , (IReadOnlyList<IPQLastTrade>)lastTraderPaidGivenEntries
        };

        for (var i = 0; i < MaxNumberOfEntries; i++)
        {
            simpleEntries.Add
                (new PQLastTrade
                    (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                   , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime));
            lastPaidGivenEntries.Add
                (new PQLastPaidGivenTrade
                    (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId, ExpectedWasPaid
                   , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
                   , ExpectedUpdateTime));
            lastTraderPaidGivenEntries.Add
                (new PQLastExternalCounterPartyTrade
                    (traderNameIdLookupGenerator, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
                   , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
                   , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
                   , ExpectedUpdateTime)
            {
                ExternalTraderName = "TestTraderName"
            });
        }

        simpleRecentlyTradedFullyPopulatedLastTrades          = new PQRecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, (IEnumerable<IPQLastTrade>)simpleEntries);
        paidGivenVolumeRecentlyTradedFullyPopulatedLastTrades = new PQRecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, (IEnumerable<IPQLastTrade>)lastPaidGivenEntries);
        fullSupportRecentlyTradedFullyPopulatedLastTrades     = new PQRecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, (IEnumerable<IPQLastTrade>)lastTraderPaidGivenEntries);

        allFullyPopulatedRecentlyTraded = new List<PQRecentlyTraded>
        {
            simpleRecentlyTradedFullyPopulatedLastTrades, paidGivenVolumeRecentlyTradedFullyPopulatedLastTrades
          , fullSupportRecentlyTradedFullyPopulatedLastTrades
        };
    }

    [TestMethod]
    public void NewRecentlyTraded_InitializedWithEntries_ContainsSameInstanceEntryAsInitialized()
    {
        for (var i = 0; i < allFullyPopulatedRecentlyTraded.Count; i++)
        {
            var populatedRecentlyTraded = allFullyPopulatedRecentlyTraded[i];
            var populatedEntries        = allPopulatedEntries[i];
            for (var j = 0; j < populatedEntries.Count; j++)
            {
                Assert.AreEqual(populatedEntries.Count, populatedRecentlyTraded.Count);
                Assert.AreNotSame(populatedEntries[j], populatedRecentlyTraded[j]);
            }
        }
    }

    [TestMethod]
    public void NewRecentlyTraded_InitializedFromRecentlyTraded_ClonesAllEntries()
    {
        for (var i = 0; i < allFullyPopulatedRecentlyTraded.Count; i++)
        {
            IRecentlyTraded populatedRecentlyTraded = allFullyPopulatedRecentlyTraded[i];
            var             clonedRecentlyTraded    = new PQRecentlyTraded(populatedRecentlyTraded);
            for (var j = 0; j < MaxNumberOfEntries; j++)
            {
                Assert.AreEqual(MaxNumberOfEntries, clonedRecentlyTraded.Count);
                Assert.AreNotSame(populatedRecentlyTraded[j], clonedRecentlyTraded[j]);
            }
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_AccessIndexerVariousInterfaces_GetsAndSetsLastTradeRemovesLastEntryIfNull()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
            for (var i = 0; i < MaxNumberOfEntries; i++)
            {
                var lastTrade       = ((IRecentlyTraded)populatedRecentlyTraded)[i];
                var clonedLastTrade = (IPQLastTrade)lastTrade!.Clone();
                populatedRecentlyTraded[i] = clonedLastTrade;
                Assert.AreNotSame(lastTrade, ((IMutableRecentlyTraded)populatedRecentlyTraded)[i]);
                Assert.AreSame(clonedLastTrade, populatedRecentlyTraded[i]);
                if (i == populatedRecentlyTraded.Count - 1)
                {
                    populatedRecentlyTraded[i] = populatedRecentlyTraded[i].ResetWithTracking();
                    Assert.AreEqual(MaxNumberOfEntries - 1, populatedRecentlyTraded.Count);
                }
            }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_Capacity_ShowMaxPossibleNumberOfEntriesNotNull()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            Assert.AreEqual(populatedRecentlyTraded.Count, populatedRecentlyTraded.Capacity);
            Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Capacity);
            populatedRecentlyTraded[MaxNumberOfEntries - 1] = populatedRecentlyTraded[MaxNumberOfEntries - 1].ResetWithTracking();
            Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Capacity);
            Assert.AreEqual(populatedRecentlyTraded.Count, populatedRecentlyTraded.Capacity - 1);
        }
    }


    [TestMethod]
    public void PopulatedRecentlyTraded_Count_UpdatesWhenPricesChanged()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            for (var i = MaxNumberOfEntries - 1; i >= 0; i--)
            {
                Assert.AreEqual(i, populatedRecentlyTraded.Count - 1);
                populatedRecentlyTraded[i]?.StateReset();
            }

            Assert.AreEqual(0, populatedRecentlyTraded.Count);
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTradedClearHasUpdates_HasUpdates_ChangeItemAtATimeReportsUpdates()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
            populatedRecentlyTraded.HasUpdates = false;
            Assert.IsFalse(populatedRecentlyTraded.HasUpdates);
            foreach (var lt in populatedRecentlyTraded)
            {
                lt.TradePrice = 3.456789m;
                Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
                Assert.IsTrue(lt.HasUpdates);
                lt.IsTradePriceUpdated = false;
                Assert.IsFalse(populatedRecentlyTraded.HasUpdates);
                Assert.IsFalse(lt.HasUpdates);
                lt.TradeTime = new DateTime(2018, 01, 04, 22, 04, 51);
                Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
                Assert.IsTrue(lt.HasUpdates);
                lt.IsTradeTimeDateUpdated = false;
                Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
                Assert.IsTrue(lt.HasUpdates);
                lt.IsTradeTimeSub2MinUpdated = false;
                Assert.IsFalse(populatedRecentlyTraded.HasUpdates);
                Assert.IsFalse(lt.HasUpdates);
                if (lt is IPQLastPaidGivenTrade lastPaidGivenTrade)
                {
                    Assert.IsFalse(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.TradeVolume = 42_121_333m;
                    Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
                    Assert.IsTrue(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.IsTradeVolumeUpdated = false;
                    Assert.IsFalse(populatedRecentlyTraded.HasUpdates);
                    Assert.IsFalse(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.WasGiven = !lastPaidGivenTrade.WasGiven;
                    Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
                    Assert.IsTrue(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.IsWasGivenUpdated = false;
                    Assert.IsFalse(populatedRecentlyTraded.HasUpdates);
                    Assert.IsFalse(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.WasPaid = !lastPaidGivenTrade.WasPaid;
                    Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
                    Assert.IsTrue(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.IsWasPaidUpdated = false;
                    Assert.IsFalse(populatedRecentlyTraded.HasUpdates);
                    Assert.IsFalse(lastPaidGivenTrade.HasUpdates);
                }

                if (lt is IPQLastExternalCounterPartyTrade traderPaidGivenTrader)
                {
                    traderPaidGivenTrader.ExternalTraderName = "TestChangedTraderName";
                    Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
                    Assert.IsTrue(traderPaidGivenTrader.HasUpdates);
                    traderPaidGivenTrader.IsExternalTraderNameUpdated = false;
                    traderPaidGivenTrader.NameIdLookup.HasUpdates     = false;
                    Assert.IsFalse(populatedRecentlyTraded.HasUpdates);
                    Assert.IsFalse(traderPaidGivenTrader.HasUpdates);
                }
            }
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_Reset_ResetsAllEntries()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Count);
            foreach (var pvl in populatedRecentlyTraded) Assert.IsFalse(pvl.IsEmpty);
            populatedRecentlyTraded.StateReset();
            Assert.AreEqual(0, populatedRecentlyTraded.Count);
            foreach (var pvl in populatedRecentlyTraded) Assert.IsTrue(pvl.IsEmpty);
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_Add_AppendsNewLastTradeToEndOfExisting()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Count);
            populatedRecentlyTraded.Add(populatedRecentlyTraded[0]!.Clone());
            Assert.AreEqual(MaxNumberOfEntries + 1, populatedRecentlyTraded.Count);
            populatedRecentlyTraded[MaxNumberOfEntries] = populatedRecentlyTraded[MaxNumberOfEntries].ResetWithTracking();
            Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Count);
            populatedRecentlyTraded.Add(populatedRecentlyTraded[0]!.Clone());
            Assert.AreEqual(MaxNumberOfEntries + 1, populatedRecentlyTraded.Count);
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTradedWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllRecentlyTradedFields()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var pqFieldUpdates =
                populatedRecentlyTraded.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Update).ToList();
            AssertContainsAllLevelRecentlyTradedFields(pqFieldUpdates, populatedRecentlyTraded);
        }
    }

    [TestMethod]
    public void NoUpdatesPopulatedRecentlyTraded_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllRecentlyTradedFields()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            populatedRecentlyTraded.HasUpdates = false;
            var pqFieldUpdates =
                populatedRecentlyTraded.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Snapshot).ToList();
            AssertContainsAllLevelRecentlyTradedFields(pqFieldUpdates, populatedRecentlyTraded);
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTradedWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            populatedRecentlyTraded.HasUpdates = false;
            var pqFieldUpdates =
                populatedRecentlyTraded.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            var pqStringUpdates =
                populatedRecentlyTraded.GetStringUpdates
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            Assert.AreEqual(0, pqStringUpdates.Count);
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_GetDeltaUpdatesUpdateUpdateFields_CopiesAllFieldsToNewRecentlyTraded()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var pqFieldUpdates =
                populatedRecentlyTraded.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var pqStringUpdates =
                populatedRecentlyTraded.GetStringUpdates
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var newEmpty = CreateNewEmpty(populatedRecentlyTraded);
            foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
            foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
            Assert.AreEqual(populatedRecentlyTraded, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_CopyFromToEmptyRecentlyTraded_RecentlyTradedEqualEachOther()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var newEmpty = CreateNewEmpty(populatedRecentlyTraded);
            newEmpty.CopyFrom(populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_CopyFromSubTypes_SubTypeSaysIsEquivalent()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        foreach (var subType in allFullyPopulatedRecentlyTraded.Where(ob => !ReferenceEquals(ob, populatedRecentlyTraded)))
        {
            if (!WholeyContainedBy(subType[0]!.GetType(), populatedRecentlyTraded[0]!.GetType())) continue;
            var newEmpty = new PQRecentlyTraded((IRecentlyTraded)populatedRecentlyTraded);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedRecentlyTraded, newEmpty);
            newEmpty.CopyFrom(subType);
            Assert.IsTrue(subType.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_CopyFromLessLastTrade_ReplicatesMissingValues()
    {
        var clonePopulated = simpleRecentlyTradedFullyPopulatedLastTrades.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        Assert.AreEqual(MaxNumberOfEntries - 3, clonePopulated.Count);
        var notEmpty = new PQRecentlyTraded((IRecentlyTraded)simpleRecentlyTradedFullyPopulatedLastTrades);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfEntries - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_CopyFromWithNull_ReplicatesGapAsEmpty()
    {
        var clonePopulated = simpleRecentlyTradedFullyPopulatedLastTrades.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);

        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();

        clonePopulated[5] = clonePopulated[5].ResetWithTracking();
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new PQRecentlyTraded((IRecentlyTraded)simpleRecentlyTradedFullyPopulatedLastTrades);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated, CopyMergeFlags.UpdateFlagsNone);
        Assert.AreEqual(new PQLastTrade(), notEmpty[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleRecentlyTradedFullyPopulatedLastTrades.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);

        clonePopulated[MaxNumberOfEntries - 1] = clonePopulated[MaxNumberOfEntries - 1].ResetWithTracking();
        clonePopulated[MaxNumberOfEntries - 2] = clonePopulated[MaxNumberOfEntries - 2].ResetWithTracking();

        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new PQRecentlyTraded((IRecentlyTraded)simpleRecentlyTradedFullyPopulatedLastTrades)
        {
            [5] = simpleRecentlyTradedFullyPopulatedLastTrades[5].Clone().ResetWithTracking()
        };
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var newEmpty = CreateNewEmpty(populatedRecentlyTraded);
            populatedRecentlyTraded.HasUpdates = false;
            newEmpty.CopyFrom(populatedRecentlyTraded);
            foreach (var pvl in newEmpty)
            {
                Assert.AreEqual(0m, pvl.TradePrice);
                Assert.AreEqual(DateTime.MinValue, pvl.TradeTime);
                Assert.IsFalse(pvl.IsTradePriceUpdated);
                Assert.IsFalse(pvl.IsTradeTimeDateUpdated);
                Assert.IsFalse(pvl.IsTradeTimeSub2MinUpdated);
                if (pvl is IPQLastPaidGivenTrade paidGivenTrade)
                {
                    Assert.IsFalse(paidGivenTrade.WasGiven);
                    Assert.IsFalse(paidGivenTrade.WasPaid);
                    Assert.AreEqual(0m, paidGivenTrade.TradeVolume);
                    Assert.IsFalse(paidGivenTrade.IsWasGivenUpdated);
                    Assert.IsFalse(paidGivenTrade.IsWasPaidUpdated);
                    Assert.IsFalse(paidGivenTrade.IsTradeVolumeUpdated);
                }

                if (pvl is IPQLastExternalCounterPartyTrade traderTrade)
                {
                    Assert.IsNull(traderTrade.ExternalTraderName);
                    Assert.IsFalse(traderTrade.IsExternalTraderNameUpdated);
                }
            }
        }
    }

    [TestMethod]
    public void NonPQRecentlyTraded_CopyFromToEmptyRecentlyTraded_RecentlyTradedAreEqual()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var nonPQRecentlyTraded = new RecentlyTraded(populatedRecentlyTraded);
            var newEmpty            = CreateNewEmpty(populatedRecentlyTraded);
            newEmpty.CopyFrom(nonPQRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, newEmpty);
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_EmptyCopyFromOtherRecentlyTradedType_UpgradesToEverythingRecentlyTradedItems()
    {
        foreach (var originalRecentlyTraded in allFullyPopulatedRecentlyTraded)
        foreach (var otherRecentlyTraded in allFullyPopulatedRecentlyTraded
                     .Where(ob => !ReferenceEquals(ob, originalRecentlyTraded)))
        {
            var emptyOriginalTypeRecentlyTraded = CreateNewEmpty(originalRecentlyTraded);
            AssertAllLastTradesAreOfTypeAndEquivalentTo(emptyOriginalTypeRecentlyTraded, originalRecentlyTraded,
                                                        originalRecentlyTraded[0]!.GetType(), false);
            emptyOriginalTypeRecentlyTraded.CopyFrom(otherRecentlyTraded);
            AssertAllLastTradesAreOfTypeAndEquivalentTo(emptyOriginalTypeRecentlyTraded, otherRecentlyTraded,
                                                        GetExpectedType(originalRecentlyTraded[0]!.GetType(),
                                                                        otherRecentlyTraded[0]!.GetType()));
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_CopyFromRecentlyTraded_UpgradesToEverythingRecentlyTradedItems()
    {
        foreach (var originalRecentlyTraded in allFullyPopulatedRecentlyTraded)
        foreach (var otherRecentlyTraded in allFullyPopulatedRecentlyTraded
                     .Where(ob => !ReferenceEquals(ob, originalRecentlyTraded)))
        {
            var clonedPopulatedRecentlyTraded = (PQRecentlyTraded)originalRecentlyTraded.Clone();
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedRecentlyTraded, originalRecentlyTraded,
                                                        originalRecentlyTraded[0]!.GetType(), false);
            clonedPopulatedRecentlyTraded.CopyFrom(otherRecentlyTraded);
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedRecentlyTraded, otherRecentlyTraded,
                                                        GetExpectedType(originalRecentlyTraded[0]!.GetType(),
                                                                        otherRecentlyTraded[0]!.GetType()));
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedRecentlyTraded, originalRecentlyTraded,
                                                        GetExpectedType(originalRecentlyTraded[0]!.GetType(),
                                                                        otherRecentlyTraded[0]!.GetType()));
        }
    }

    [TestMethod]
    public void FullyPopulateRecentlyTraded_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var clonedRecentlyTraded = ((ICloneable<IRecentlyTraded>)populatedRecentlyTraded).Clone();
            Assert.AreNotSame(clonedRecentlyTraded, populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, clonedRecentlyTraded);

            var cloned2 = (IPQRecentlyTraded)((ICloneable)populatedRecentlyTraded).Clone();
            Assert.AreNotSame(cloned2, populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, cloned2);
        }
    }

    [TestMethod]
    public void ClonedPopulatedRecentlyTraded_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var fullyPopulatedClone = (PQRecentlyTraded)((ICloneable)populatedRecentlyTraded).Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedRecentlyTraded,
                                                                fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedRecentlyTraded,
                                                                fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTradedSameObj_Equals_ReturnsTrue()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            Assert.AreEqual(populatedRecentlyTraded, populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, ((ICloneable)populatedRecentlyTraded).Clone());
            Assert.AreEqual(populatedRecentlyTraded, ((ICloneable<IRecentlyTraded>)populatedRecentlyTraded).Clone());
            Assert.AreEqual(populatedRecentlyTraded, ((IMutableRecentlyTraded)populatedRecentlyTraded).Clone());
            Assert.AreEqual(populatedRecentlyTraded, ((IPQRecentlyTraded)populatedRecentlyTraded).Clone());
        }
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_GetHashCode_ReturnNumberNoException()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var hashCode = populatedRecentlyTraded.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var q = populatedRecentlyTraded;

            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));

            Assert.IsTrue(toString.Contains(
                                            $"LastTrades: [{string.Join(", ", (IEnumerable<ILastTrade>)populatedRecentlyTraded)}]"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
        }
    }

    [TestMethod]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = fullSupportRecentlyTradedFullyPopulatedLastTrades;
        Assert.AreEqual(MaxNumberOfEntries, rt.Count);
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<IPQLastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<ILastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, rt.OfType<ILastTrade>().Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable<IPQLastTrade>)rt).Count());
        Assert.AreEqual(0, ((IEnumerable<ILastTrade>)rt).Count());
        Assert.AreEqual(0, rt.OfType<ILastTrade>().Count());
    }

    private Type GetExpectedType(Type originalType, Type copyType)
    {
        if (copyType == typeof(PQLastTrade)) return originalType;
        if ((originalType == typeof(PQLastPaidGivenTrade) || originalType == typeof(PQLastTrade)) && copyType == typeof(PQLastPaidGivenTrade)) return typeof(PQLastPaidGivenTrade);
        return typeof(PQLastExternalCounterPartyTrade);
    }

    private void AssertAllLastTradesAreOfTypeAndEquivalentTo
    (PQRecentlyTraded upgradedRecentlyTraded,
        PQRecentlyTraded equivalentTo, Type expectedType, bool compareForEquivalence = true,
        bool exactlyEquals = false)
    {

        for (var i = 0; i < upgradedRecentlyTraded.Capacity; i++)
        {
            var upgradedLastTrade = upgradedRecentlyTraded[i];
            var copyFromLastTrade = equivalentTo[i];

            Assert.IsInstanceOfType(upgradedLastTrade, expectedType);
            if (compareForEquivalence) Assert.IsTrue(copyFromLastTrade!.AreEquivalent(upgradedLastTrade, exactlyEquals));
        }
    }

    private bool WholeyContainedBy(Type copySourceType, Type copyDestinationType)
    {
        if (copySourceType == typeof(PQLastTrade)) return true;
        if (copySourceType == typeof(PQLastPaidGivenTrade))
            return copyDestinationType == typeof(PQLastTrade) ||
                   copyDestinationType == typeof(PQLastPaidGivenTrade);
        if (copySourceType == typeof(PQLastExternalCounterPartyTrade))
            return copyDestinationType == typeof(PQLastTrade) ||
                   copyDestinationType == typeof(PQLastPaidGivenTrade) ||
                   copyDestinationType == typeof(PQLastExternalCounterPartyTrade);
        return false;
    }

    private static PQRecentlyTraded CreateNewEmpty(PQRecentlyTraded populatedRecentlyTraded)
    {
        var cloneGensis = populatedRecentlyTraded[0]!.Clone();
        cloneGensis.StateReset();
        if (cloneGensis is IPQLastExternalCounterPartyTrade traderLastTrade)
            traderLastTrade.NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        var clonedEmptyEntries = new List<IPQLastTrade>(MaxNumberOfEntries);
        for (var i = 0; i < MaxNumberOfEntries; i++) clonedEmptyEntries.Add(cloneGensis.Clone());
        var newEmpty = new PQRecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, clonedEmptyEntries!);
        return newEmpty;
    }

    public static void AssertContainsAllLevelRecentlyTradedFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        IPQRecentlyTraded recentlyTraded
      , PQQuoteBooleanValues expectedQuoteBooleanFlags = PQQuoteBooleanValuesExtensions.LivePricingFieldsSetNoReplayOrSnapshots
      , PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        for (var i = 0; i < recentlyTraded.Count; i++)
        {
            var lastTrade = recentlyTraded[i]!;
            var depthId   = (PQDepthKey)i;


            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedAtPrice, lastTrade.TradePrice, priceScale)
                           ,
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId,
                                                                        PQTradingSubFieldKeys.LastTradedAtPrice, priceScale),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedTradeTimeDate,
                                              lastTrade.TradeTime.Get2MinIntervalsFromUnixEpoch()),
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId,
                                                                        PQTradingSubFieldKeys.LastTradedTradeTimeDate),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var extended = lastTrade.TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBase);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHourBase, extended)
                          , PQTickInstantTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime
                               , extended),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (lastTrade is IPQLastPaidGivenTrade pqPaidGivenTrade)
            {
                var lastTradedBoolFlags = pqPaidGivenTrade.WasGiven ? LastTradeBooleanFlags.WasGiven : LastTradeBooleanFlags.None;
                lastTradedBoolFlags |= pqPaidGivenTrade.WasPaid ? LastTradeBooleanFlags.WasPaid : LastTradeBooleanFlags.None;


                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)lastTradedBoolFlags)
                               ,
                                PQTickInstantTests.ExtractFieldUpdateWithId
                                    (checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedBooleanFlags),
                                $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }

            if (lastTrade is IPQLastExternalCounterPartyTrade pqTraderPaidGivenTrade)
            {
                var lastTradedTraderNameId = (uint)pqTraderPaidGivenTrade.ExternalTraderNameId;
                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedExternalTraderNameId, lastTradedTraderNameId)
                               ,
                                PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId,
                                                                            PQTradingSubFieldKeys.LastTradedExternalTraderNameId),
                                $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }
        }
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQRecentlyTraded original, PQRecentlyTraded changingRecentlyTraded, PQPublishableLevel3Quote? originalQuote = null,
        PQPublishableLevel3Quote? changingQuote = null)
    {
        if (original.GetType() == typeof(PQRecentlyTraded))
            Assert.AreEqual(!exactComparison,
                            changingRecentlyTraded.AreEquivalent(new RecentlyTraded(original), exactComparison));

        Assert.AreEqual(original.Count, changingRecentlyTraded.Count);

        for (var i = 0; i < original.Count; i++)
        {
            var originalEntry = original[i];
            var changingEntry = changingRecentlyTraded[i]!;
            PQLastTradeTests
                .AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalEntry as PQLastTrade,
                     (PQLastTrade)changingEntry, original,
                     changingRecentlyTraded, originalQuote, changingQuote);
            PQLastPaidGivenTradeTests
                .AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalEntry as PQLastPaidGivenTrade,
                     changingEntry as PQLastPaidGivenTrade, original,
                     changingRecentlyTraded, originalQuote, changingQuote);
            PQLastExternalCounterPartyTradeTests
                .AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalEntry as PQLastExternalCounterPartyTrade,
                     changingEntry as PQLastExternalCounterPartyTrade, original,
                     changingRecentlyTraded, originalQuote, changingQuote);
        }
    }
}
