// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[TestClass]
public class PQRecentlyTradedTests
{
    private const uint ExpectedTradeId = 42;
    private const uint ExpectedBatchId = 24_942;
    private const uint ExpectedOrderId = 1_772_942;

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
    private IList<IPQLastExternalCounterPartyTrade> lastExternalCounterPartyEntries = null!;

    private PQRecentlyTraded paidGivenVolumeRecentlyTradedFullyPopulatedLastTrades = null!;

    private PQNameIdLookupGenerator traderNameIdLookupGenerator = null!;

    private IList<IPQLastTrade> simpleEntries = null!;

    private PQRecentlyTraded simpleRecentlyTradedFullyPopulatedLastTrades      = null!;
    private PQRecentlyTraded fullSupportRecentlyTradedFullyPopulated = null!;
    // test being less than max.
    private const ListTransmissionFlags AllLimitedAllPublishing
        = ListTransmissionFlags.LimitByPeriodTime | ListTransmissionFlags.LimitByTradeCount |
          ListTransmissionFlags.PublishesOnDeltaUpdates | ListTransmissionFlags.PublishOnCompleteOrSnapshot;

    private readonly PQSourceTickerInfo forGetDeltaUpdates = PQSourceTickerInfoTests.FullSupportL3TraderNamePaidOrGivenSti;

    [TestInitialize]
    public void SetUp()
    {
        traderNameIdLookupGenerator = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);

        simpleEntries                   = new List<IPQLastTrade>(MaxNumberOfEntries);
        lastPaidGivenEntries            = new List<IPQLastPaidGivenTrade>(MaxNumberOfEntries);
        lastExternalCounterPartyEntries = new List<IPQLastExternalCounterPartyTrade>(MaxNumberOfEntries);

        allPopulatedEntries =
        [
            (IReadOnlyList<IPQLastTrade>)simpleEntries, (IReadOnlyList<IPQLastTrade>)lastPaidGivenEntries
          , (IReadOnlyList<IPQLastTrade>)lastExternalCounterPartyEntries
        ];

        for (var i = 0u; i < MaxNumberOfEntries; i++)
        {
            simpleEntries.Add
                (new PQLastTrade
                    (ExpectedTradeId + i, ExpectedBatchId + i, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                   , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime));
            lastPaidGivenEntries.Add
                (new PQLastPaidGivenTrade
                    (ExpectedTradeId + i, ExpectedBatchId + i, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId
                   , ExpectedWasPaid
                   , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
                   , ExpectedUpdateTime));
            lastExternalCounterPartyEntries.Add
                (new PQLastExternalCounterPartyTrade
                    (traderNameIdLookupGenerator, ExpectedTradeId + i, ExpectedBatchId + i, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
                   , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
                   , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
                   , ExpectedUpdateTime)
                    {
                        ExternalTraderName = "TestTraderName"
                    });
        }
        // ReSharper disable RedundantCast
        simpleRecentlyTradedFullyPopulatedLastTrades = new PQRecentlyTraded(AllLimitedAllPublishing, (IEnumerable<IPQLastTrade>)simpleEntries);
        paidGivenVolumeRecentlyTradedFullyPopulatedLastTrades
            = new PQRecentlyTraded(AllLimitedAllPublishing, (IEnumerable<IPQLastTrade>)lastPaidGivenEntries);
        fullSupportRecentlyTradedFullyPopulated
            = new PQRecentlyTraded(AllLimitedAllPublishing, (IEnumerable<IPQLastTrade>)lastExternalCounterPartyEntries);
        // ReSharper restore RedundantCast
        allFullyPopulatedRecentlyTraded = new List<PQRecentlyTraded>
        {
            simpleRecentlyTradedFullyPopulatedLastTrades, paidGivenVolumeRecentlyTradedFullyPopulatedLastTrades
          , fullSupportRecentlyTradedFullyPopulated
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
                var clonedLastTrade = (IPQLastTrade)lastTrade.Clone();
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
                populatedRecentlyTraded[i].StateReset();
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
            populatedRecentlyTraded.Add(populatedRecentlyTraded[0].Clone());
            Assert.AreEqual(MaxNumberOfEntries + 1, populatedRecentlyTraded.Count);
            populatedRecentlyTraded[MaxNumberOfEntries] = populatedRecentlyTraded[MaxNumberOfEntries].ResetWithTracking();
            Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Count);
            populatedRecentlyTraded.Add(populatedRecentlyTraded[0].Clone());
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
                    (new DateTime(2017, 11, 04, 12, 33, 1), PQMessageFlags.Update).ToList();
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
                    (new DateTime(2017, 11, 04, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
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
                    (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
            var pqStringUpdates =
                populatedRecentlyTraded.GetStringUpdates
                    (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
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
                   , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
            var pqStringUpdates =
                populatedRecentlyTraded.GetStringUpdates
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
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
            if (!WhollyContainedBy(subType[0].GetType(), populatedRecentlyTraded[0].GetType())) continue;
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
                                                        originalRecentlyTraded[0].GetType(), false);
            emptyOriginalTypeRecentlyTraded.CopyFrom(otherRecentlyTraded);
            AssertAllLastTradesAreOfTypeAndEquivalentTo(emptyOriginalTypeRecentlyTraded, otherRecentlyTraded,
                                                        GetExpectedType(originalRecentlyTraded[0].GetType(),
                                                                        otherRecentlyTraded[0].GetType()));
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_CopyFromRecentlyTraded_UpgradesToEverythingRecentlyTradedItems()
    {
        foreach (var originalRecentlyTraded in allFullyPopulatedRecentlyTraded)
        foreach (var otherRecentlyTraded in allFullyPopulatedRecentlyTraded
                     .Where(ob => !ReferenceEquals(ob, originalRecentlyTraded)))
        {
            var clonedPopulatedRecentlyTraded = originalRecentlyTraded.Clone();
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedRecentlyTraded, originalRecentlyTraded,
                                                        originalRecentlyTraded[0].GetType(), false);
            clonedPopulatedRecentlyTraded.CopyFrom(otherRecentlyTraded);
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedRecentlyTraded, otherRecentlyTraded,
                                                        GetExpectedType(originalRecentlyTraded[0].GetType(),
                                                                        otherRecentlyTraded[0].GetType()));
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedRecentlyTraded, originalRecentlyTraded,
                                                        GetExpectedType(originalRecentlyTraded[0].GetType(),
                                                                        otherRecentlyTraded[0].GetType()));
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
            var rt = populatedRecentlyTraded;

            var toString = rt.ToString();

            Assert.IsTrue(toString.Contains(rt.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(rt.MaxAllowedSize)}: {rt.MaxAllowedSize:N0}"));
            Assert.IsTrue(toString.Contains($"{nameof(rt.Count)}: {rt.Count}"));
            Assert.IsTrue(toString.Contains($"{nameof(rt.LastTrades)}: [\n{rt.EachLastTradeByIndexOnNewLines()}]"));
        }
    }

    [TestMethod]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = fullSupportRecentlyTradedFullyPopulated;
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

    [TestMethod]
    public void PopulatedRecentlyTraded_SmallerToLargerCalculateShifts_ShiftRightCommandsExpected()
    {
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);


        int[]        expectedIndices = [0, 2, 5, 9];
        ILastTrade[] instances       = new ILastTrade[10];


        int oldIndex    = 0;                   // original    0,1,2,3,4,5,6,7,8,9,10,11
        int actualIndex = 0;                   // deleted     1,3,4,6,7,8,10,11 
        var count       = toShift.Count;       // leaving     0,2,5,9
        for (var i = 0; oldIndex < count; i++) // shifts at   (2,3),(1,2)(0,1)
        {
            Console.Out.WriteLine($"Leaving index {oldIndex} with TradeId {toShift[actualIndex].TradeId}");
            instances[oldIndex] = toShift[actualIndex];
            oldIndex++;
            actualIndex++;
            for (var j = i + 1; j < 2 + 2 * i && oldIndex < count; j++)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with TradeId {toShift[actualIndex].TradeId}");
                toShift.RemoveAt(actualIndex);
                oldIndex++;
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportRecentlyTradedFullyPopulated.Clone();
        toShift.CalculateShift(ExpectedTradeTime, shiftedNext);

        Assert.AreEqual(3, toShift.ShiftCommands.Count);
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < toShift.ShiftCommands.Count; i++)
            {
                var shift = toShift.ShiftCommands[i];
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(2, shift.PinnedFromIndex);
                        Assert.AreEqual(3, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(1, shift.PinnedFromIndex);
                        Assert.AreEqual(2, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(0, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in toShift.ShiftCommands)
        {
            toShift.ApplyListShiftCommand(shiftElementShift);
        }
        foreach (var expectedIndex in expectedIndices)
        {
            Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated[expectedIndex], toShift[expectedIndex]);
            Assert.AreSame(instances[expectedIndex], toShift[expectedIndex]);
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_SmallerToLargerCalculateShiftsWithElementMovedToStart_ShiftRightCommandsExpected()
    {
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        int[]        expectedIndices = [0, 2, 4, 6, 7, 8, 10];
        ILastTrade[] instances       = new ILastTrade[11];


        int oldIndex    = 0;                   // original    0,1,2,3,4,5,6,7,8,9,10,11        
        int actualIndex = 0;                   // deleted     1,3,5,9,11         
        var count       = toShift.Count;       // leaving     7,0,2,4,6,8,10        
        for (var i = 0; oldIndex < count; i++) // shifts at   (6,1),(5,1),(4,1),(3,1)(2,1)(1,1)(0,1)
        {
            if (i % 2 == 1 && i != 7)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with TradeId {toShift[actualIndex].TradeId}");
                toShift.RemoveAt(actualIndex);
                oldIndex++;
            }
            else if (i == 7)
            {
                Console.Out.WriteLine($"Moving index {oldIndex} with TradeId {toShift[actualIndex].TradeId} to Start");
                instances[oldIndex] = toShift[actualIndex];
                toShift.MoveToStart(actualIndex);
                oldIndex++;
                actualIndex++;
            }
            else
            {
                Console.Out.WriteLine($"Leaving index {oldIndex} with TradeId {toShift[actualIndex].TradeId}");
                instances[oldIndex] = toShift[actualIndex];
                oldIndex++;
                actualIndex++;
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportRecentlyTradedFullyPopulated.Clone();
        toShift.CalculateShift(ExpectedTradeTime, shiftedNext);
        
        Console.Out.WriteLine($"{toShift.ShiftCommands.JoinShiftCommandsOnNewLine()}");
        Assert.AreEqual(6, toShift.ShiftCommands.Count);
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < toShift.ShiftCommands.Count; i++)
            {
                var shift = toShift.ShiftCommands[i];
                Console.Out.WriteLine($"shift: {shift}");
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(5, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(4, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(3, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 3:
                        Assert.AreEqual(2, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 4:
                        Assert.AreEqual(1, shift.PinnedFromIndex);
                        Assert.AreEqual(0, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.MoveSingleElement | ListShiftCommandType.InsertElementsRange, shift.ShiftCommandType);
                        break;
                    case 5:
                        Assert.AreEqual(1, shift.PinnedFromIndex);
                        Assert.AreEqual(7, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.MoveSingleElement | ListShiftCommandType.InsertElementsRange, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in toShift.ShiftCommands)
        {
            toShift.ApplyListShiftCommand(shiftElementShift);
        }
        for (int i = 0; i < expectedIndices.Length; i++)
        {
            Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated[expectedIndices[i]], toShift[expectedIndices[i]]);
            Assert.AreSame(instances[expectedIndices[i]], toShift[expectedIndices[i]]);
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_LargerToSmallerCalculateShiftsWithNewEntryInMiddle_CalculateShiftLeftCommandsReturnsExpected()
    {
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var count       = toShift.Count;    // original    0,1,2,3,4,5,6,7,8,9,10,11
        int oldIndex    = count - 1;        // deleted     0,1,3,4,5,7,8,10                         
        int actualIndex = oldIndex;         // leaving     2,6,{new entry}, 9,11                     
        for (var i = 0; oldIndex >= 0; i++) // shifts at   (-1,-2),(0,-3), {new entry} ,(2,-1),(3,-1)
        {
            Console.Out.WriteLine($"Leaving index {oldIndex} with TradeId {toShift[actualIndex].TradeId}");
            oldIndex--;
            actualIndex--;
            for (var j = i + 1; j < 2 + 2 * i && oldIndex >= 0; j++)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with TradeId {toShift[actualIndex].TradeId}");
                toShift.RemoveAt(actualIndex--);
                oldIndex--;
            }
        }

        var newLastTrade = new PQLastExternalCounterPartyTrade
            (traderNameIdLookupGenerator, ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

        toShift.InsertAt(2, newLastTrade);

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = fullSupportRecentlyTradedFullyPopulated.Clone();

        int[] expectedIndices = [0, 1, 3, 4];

        ILastTrade[] instances = new ILastTrade[5];

        instances[expectedIndices[0]] = shiftedNext[2];
        instances[expectedIndices[1]] = shiftedNext[6];
        instances[expectedIndices[2]] = shiftedNext[9];
        instances[expectedIndices[3]] = shiftedNext[11];
        shiftedNext.CalculateShift(ExpectedTradeTime, toShift);

        Console.Out.WriteLine($"{shiftedNext.ShiftCommands.JoinShiftCommandsOnNewLine()}");

        AssertExpectedShiftCommands();
        Assert.AreEqual(4, shiftedNext.ShiftCommands.Count);

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < shiftedNext.ShiftCommands.Count; i++)
            {
                var shift = shiftedNext.ShiftCommands[i];
                Console.Out.WriteLine($"shift: {shift}");
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(-1, shift.PinnedFromIndex);
                        Assert.AreEqual(-2, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(0, shift.PinnedFromIndex);
                        Assert.AreEqual(-3, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(2, shift.PinnedFromIndex);
                        Assert.AreEqual(-1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 3:
                        Assert.AreEqual(3, shift.PinnedFromIndex);
                        Assert.AreEqual(-1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in shiftedNext.ShiftCommands)
        {
            shiftedNext.ApplyListShiftCommand(shiftElementShift);
        }

        foreach (var expectedIndex in expectedIndices)
        {
            Assert.AreEqual(toShift[expectedIndex], shiftedNext[expectedIndex]);
            Assert.AreSame(instances[expectedIndex], shiftedNext[expectedIndex]);
        }
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ClearAfterMidElement_ListIsReducedByHalf()
    {
        var halfListSize = MaxNumberOfEntries / 2;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ClearRemainingElementsFromIndex = halfListSize;

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }
        for (int i = halfListSize; i < fullSupportRecentlyTradedFullyPopulated.Count; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(halfListSize, toShift.Count);

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(halfListSize, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_InsertNewElementAtStart_RemainingElementsShiftRightByOne()
    {
        var newLastTrade = new PQLastExternalCounterPartyTrade
            (traderNameIdLookupGenerator, ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.InsertAtStart(newLastTrade);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i + 1;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(0, toShift.IndexOf(newLastTrade));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries + 1, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxedSizeRecentlyTraded_DeleteMiddleElement_RemainingElementsShiftLeftByOne()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;

        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];

        toShift.DeleteAt(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i + 1;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated.Count, toShift.Count + 1);


        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.Delete(middleElement);
        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i + 1;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated.Count, toShift.Count + 1);


        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_InsertNewElementAtStart_RemainingElementsShiftRightExceptLastIsRemoved()
    {
        var newLastTrade = new PQLastExternalCounterPartyTrade
            (traderNameIdLookupGenerator, ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        toShift.InsertAtStart(newLastTrade);

        for (int i = 1; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i - 1;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(0, toShift.IndexOf(newLastTrade));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_InsertNewElementAtEnd_NewElementAppearsAtTheEnd()
    {
        var newLastTrade = new PQLastExternalCounterPartyTrade
            (traderNameIdLookupGenerator, ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.AppendAtEnd(newLastTrade);

        for (int i = 0; i < toShift.Count - 1; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries + 1, toShift.Count);
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(newLastTrade));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries + 1, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries + 1, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachRecentlyTraded_AttemptInsertNewElementAtEnd_ReturnsFalseAndNoElementIsAdded()
    {
        var newLastTrade = new PQLastExternalCounterPartyTrade
            (traderNameIdLookupGenerator, ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var result = toShift.AppendAtEnd(newLastTrade);
        Assert.IsFalse(result);
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated[^1], toShift[^1]);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_MoveMiddleToStart_FormerMiddleElementIsAtStart()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];

        toShift.MoveToStart(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i == midIndex ? 0 : i < midIndex ? i + 1 : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(0, toShift.IndexOf(middleElement));

        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveToStart(middleElement);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i == midIndex ? 0 : i < midIndex ? i + 1 : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(0, toShift.IndexOf(middleElement));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleToStart_FormerMiddleElementIsAtStart()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];

        toShift.MoveToStart(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i == midIndex ? 0 : i < midIndex ? i + 1 : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(0, toShift.IndexOf(middleElement));

        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveToStart(middleElement);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i == midIndex ? 0 : i < midIndex ? i + 1 : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(0, toShift.IndexOf(middleElement));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_MoveMiddleToEnd_FormerMiddleElementIsAtTheEnd()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];

        toShift.MoveToEnd(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i == toShift.Count - 1 ? midIndex : i + 1;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(middleElement));

        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveToEnd(middleElement);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i == toShift.Count - 1 ? midIndex : i + 1;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(middleElement));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleToEnd_FormerMiddleElementIsAtTheEnd()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];

        toShift.MoveToEnd(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i == toShift.Count - 1 ? midIndex : i + 1;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(middleElement));

        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveToEnd(middleElement);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i == toShift.Count - 1 ? midIndex : i + 1;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(middleElement));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_MoveMiddleRightByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];
        var shiftAmount   = 2;

        toShift.MoveSingleElementBy(midIndex, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i < midIndex + shiftAmount ? i + 1 : i == midIndex + shiftAmount ? midIndex : i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveSingleElementBy(middleElement, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i < midIndex + shiftAmount ? i + 1 : i == midIndex + shiftAmount ? midIndex : i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleRightByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];
        var shiftAmount   = 2;

        toShift.MoveSingleElementBy(midIndex, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i < midIndex + shiftAmount ? i + 1 : i == midIndex + shiftAmount ? midIndex : i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveSingleElementBy(middleElement, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i < midIndex + shiftAmount ? i + 1 : i == midIndex + shiftAmount ? midIndex : i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_MoveMiddleLeftByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];
        var shiftAmount   = -2;

        toShift.MoveSingleElementBy(midIndex, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i < midIndex + shiftAmount ? i : i < midIndex ? i + 1 : i == midIndex ? midIndex + shiftAmount : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveSingleElementBy(middleElement, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i < midIndex + shiftAmount ? i : i < midIndex ? i + 1 : i == midIndex ? midIndex + shiftAmount : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleLeftByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        var middleElement = toShift[midIndex];
        var shiftAmount   = -2;

        toShift.MoveSingleElementBy(midIndex, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i < midIndex + shiftAmount ? i : i < midIndex ? i + 1 : i == midIndex ? midIndex + shiftAmount : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveSingleElementBy(middleElement, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i < midIndex + shiftAmount ? i : i < midIndex ? i + 1 : i == midIndex ? midIndex + shiftAmount : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], fullSupportRecentlyTradedFullyPopulated[prevIndex]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftLeftFromMiddleByOne_DeletesEntryFirstEntryCreatesEmptyOneBelowPinIndex()
    {
        var pinAt = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(-1, pinAt);

        for (int i = 0; i < pinAt - 1; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i + 1]);
        }
        Assert.IsTrue(toShift[pinAt - 1].IsEmpty);
        for (int i = pinAt; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftLeftFromMiddleByOne_DeletesEntryFirstEntryCreatesEmptyOneBelowPinIndex()
    {
        var pinAt = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(-1, pinAt);

        for (int i = 0; i < pinAt - 1; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i + 1]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.IsTrue(toShift[pinAt - 1].IsEmpty);
        for (int i = pinAt; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftRightFromMiddleByOne_CreatesEmptyOneAbovePinIndexAndExtendsList()
    {
        var pinAt = MaxNumberOfEntries / 2 - 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(1, pinAt);

        for (int i = pinAt + 2; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i - 1]);
        }
        Assert.IsTrue(toShift[pinAt + 1].IsEmpty);
        for (int i = 0; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftRightFromMiddleByOne_CreatesEmptyOneAbovePinIndexAndDeletesLastEntry()
    {
        var pinAt = MaxNumberOfEntries / 2 - 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(1, pinAt);

        for (int i = pinAt + 2; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i - 1]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.IsTrue(toShift[pinAt + 1].IsEmpty);
        for (int i = 0; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftLeftTowardMiddleByOne_DeletesPreMiddleEntryCreatesEmptyAtEnd()
    {
        var pinAt = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsUntil(-1, pinAt);

        for (int i = pinAt + 1; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i + 1]);
        }
        Assert.AreEqual(MaxNumberOfEntries - 1, toShift.Count);
        Assert.IsTrue(toShift[fullSupportRecentlyTradedFullyPopulated.Count - 1].IsEmpty);
        for (int i = 0; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries - 1, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries - 1, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedCacheMaxAllowedSizeReachedRecentlyTraded_ShiftLeftTowardMiddleByOne_DeletesPreMiddleEntryCreatesEmptyAtEnd()
    {
        var pinAt = MaxNumberOfEntries / 2 + 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsUntil(-1, pinAt);

        for (int i = pinAt + 1; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i + 1]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count + 1);
        Assert.IsTrue(toShift[fullSupportRecentlyTradedFullyPopulated.Count - 1].IsEmpty);
        for (int i = 0; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count + 1);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count + 1);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftRightTowardMiddleByOne_CreatesEmptyAtStartDeletesPreMiddleEntry()
    {
        var pinAt = MaxNumberOfEntries / 2 - 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsUntil(1, pinAt);

        for (int i = 1; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i - 1]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.IsTrue(toShift[0].IsEmpty);
        for (int i = pinAt; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftRightTowardMiddleByOne_CreatesEmptyAtStartDeletesPreMiddleEntry()
    {
        var pinAt = MaxNumberOfEntries / 2 - 1;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsUntil(1, pinAt);

        for (int i = 1; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i - 1]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.IsTrue(toShift[0].IsEmpty);
        for (int i = pinAt; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i]);
        }

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = MaxNumberOfEntries / 2;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(-halfListSize, short.MaxValue);

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i + halfListSize]);
        }
        Assert.AreEqual(halfListSize, toShift.Count);

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(halfListSize, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = MaxNumberOfEntries / 2;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(-halfListSize, short.MaxValue);

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i + halfListSize]);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count + halfListSize);
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated.Count, toShift.Count + halfListSize);

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(halfListSize, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftRightFromStart_CreatesEmptyAtStartAndExtendsListByHalf()
    {
        var halfListSize = MaxNumberOfEntries / 2;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates = false;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(halfListSize, short.MinValue);

        for (int i = halfListSize; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i - halfListSize]);
        }
        Assert.AreEqual(MaxNumberOfEntries + halfListSize, toShift.Count);
        for (int i = 0; i < halfListSize; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated.Count, toShift.Count - halfListSize);

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries + halfListSize, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries + halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftRightFromStartByHalfList_CreatesEmptyItemsAtStartAndTruncatesLastHalfOfOrders()
    {
        var halfListSize = MaxNumberOfEntries / 2;
        fullSupportRecentlyTradedFullyPopulated.HasUpdates     = false;
        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(halfListSize, short.MinValue);

        for (int i = halfListSize; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i - halfListSize]);
        }
        for (int i = 0; i < halfListSize; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(MaxNumberOfEntries, toShift.Count);
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated.Count, toShift.Count);

        var shiftViaDeltaUpdates = fullSupportRecentlyTradedFullyPopulated.Clone();
        foreach (var deltaUpdateField in toShift.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update, forGetDeltaUpdates))
        {
            shiftViaDeltaUpdates.UpdateField(deltaUpdateField);
        }
        Assert.AreEqual(MaxNumberOfEntries, shiftViaDeltaUpdates.Count);
        Assert.AreEqual(toShift, shiftViaDeltaUpdates);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    private Type GetExpectedType(Type originalType, Type copyType)
    {
        if (copyType == typeof(PQLastTrade)) return originalType;
        if ((originalType == typeof(PQLastPaidGivenTrade) || originalType == typeof(PQLastTrade)) && copyType == typeof(PQLastPaidGivenTrade))
            return typeof(PQLastPaidGivenTrade);
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
            if (compareForEquivalence) Assert.IsTrue(copyFromLastTrade.AreEquivalent(upgradedLastTrade, exactlyEquals));
        }
    }

    private bool WhollyContainedBy(Type copySourceType, Type copyDestinationType)
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
        var cloneGenesis = populatedRecentlyTraded[0].Clone();
        cloneGenesis.StateReset();
        if (cloneGenesis is IPQLastExternalCounterPartyTrade traderLastTrade)
            traderLastTrade.NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        var clonedEmptyEntries = new List<IPQLastTrade>(MaxNumberOfEntries);
        for (var i = 0; i < MaxNumberOfEntries; i++) clonedEmptyEntries.Add(cloneGenesis.Clone());
        var newEmpty = new PQRecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, clonedEmptyEntries);
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
            var lastTrade = recentlyTraded[i];
            var depthId   = (PQDepthKey)i;


            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedAtPrice, lastTrade.TradePrice, priceScale)
                           ,
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId,
                                                                        PQTradingSubFieldKeys.LastTradedAtPrice, priceScale),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedTradeTimeDate
                                             ,
                                              lastTrade.TradeTime.Get2MinIntervalsFromUnixEpoch()),
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId,
                                                                        PQTradingSubFieldKeys.LastTradedTradeTimeDate),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var extended = lastTrade.TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBase);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHourBase, extended)
                          , PQTickInstantTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId
                               , PQTradingSubFieldKeys.LastTradedTradeSub2MinTime
                               , extended),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (lastTrade is IPQLastPaidGivenTrade pqPaidGivenTrade)
            {
                var lastTradedBoolFlags = pqPaidGivenTrade.WasGiven ? LastTradeBooleanFlags.WasGiven : LastTradeBooleanFlags.None;
                lastTradedBoolFlags |= pqPaidGivenTrade.WasPaid ? LastTradeBooleanFlags.WasPaid : LastTradeBooleanFlags.None;


                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)lastTradedBoolFlags)
                               ,
                                PQTickInstantTests.ExtractFieldUpdateWithId
                                    (checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId
                                   , PQTradingSubFieldKeys.LastTradedBooleanFlags),
                                $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }

            if (lastTrade is IPQLastExternalCounterPartyTrade pqTraderPaidGivenTrade)
            {
                var lastTradedTraderNameId = (uint)pqTraderPaidGivenTrade.ExternalTraderNameId;
                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, depthId, PQTradingSubFieldKeys.LastTradedExternalTraderNameId, lastTradedTraderNameId)
                               ,
                                PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedAllRecentlyLimitedHistory
                                                                          , depthId,
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
            var changingEntry = changingRecentlyTraded[i];
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
