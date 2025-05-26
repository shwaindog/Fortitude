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
public class PQOnTickLastTradedTests
{
    private const int MaxNumberOfEntries = QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades;

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


    private IList<PQOnTickLastTraded>               allFullyPopulatedOnTickLastTraded = null!;
    private List<IReadOnlyList<IPQLastTrade>>       allPopulatedEntries               = null!;
    private IList<IPQLastPaidGivenTrade>            lastPaidGivenEntries              = null!;
    private IList<IPQLastExternalCounterPartyTrade> lastTraderPaidGivenEntries        = null!;

    private PQOnTickLastTraded paidGivenVolumeOnTickTradedFullyPopulatedQuote = null!;

    private IList<IPQLastTrade> simpleEntries = null!;

    private PQOnTickLastTraded      simpleOnTickLastTradedFullyPopulatedQuote               = null!;
    private PQNameIdLookupGenerator traderNameIdLookupGenerator                             = null!;
    private PQOnTickLastTraded      fullSupportLastTradeOnTickLastTradedFullyPopulatedQuote = null!;
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
            simpleEntries.Add(new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                            , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
                                            , ExpectedUpdateTime));
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

        simpleOnTickLastTradedFullyPopulatedQuote               = new PQOnTickLastTraded((IEnumerable<IPQLastTrade>)simpleEntries);
        paidGivenVolumeOnTickTradedFullyPopulatedQuote          = new PQOnTickLastTraded((IEnumerable<IPQLastTrade>)lastPaidGivenEntries);
        fullSupportLastTradeOnTickLastTradedFullyPopulatedQuote = new PQOnTickLastTraded((IEnumerable<IPQLastTrade>)lastTraderPaidGivenEntries);

        allFullyPopulatedOnTickLastTraded = new List<PQOnTickLastTraded>
        {
            simpleOnTickLastTradedFullyPopulatedQuote, paidGivenVolumeOnTickTradedFullyPopulatedQuote
          , fullSupportLastTradeOnTickLastTradedFullyPopulatedQuote
        };
    }

    [TestMethod]
    public void NewOnTickLastTraded_InitializedWithEntries_ContainsSameInstanceEntryAsInitialized()
    {
        for (var i = 0; i < allFullyPopulatedOnTickLastTraded.Count; i++)
        {
            var populatedOnTickLastTraded = allFullyPopulatedOnTickLastTraded[i];
            var populatedEntries          = allPopulatedEntries[i];
            for (var j = 0; j < populatedEntries.Count; j++)
            {
                Assert.AreEqual(populatedEntries.Count, populatedOnTickLastTraded.Count);
                Assert.AreNotSame(populatedEntries[j], populatedOnTickLastTraded[j]);
            }
        }
    }

    [TestMethod]
    public void NewOnTickLastTraded_InitializedFromOnTickLastTraded_ClonesAllEntries()
    {
        for (var i = 0; i < allFullyPopulatedOnTickLastTraded.Count; i++)
        {
            IOnTickLastTraded populatedOnTickLastTraded = allFullyPopulatedOnTickLastTraded[i];
            var               clonedOrderBook           = new PQOnTickLastTraded(populatedOnTickLastTraded);
            for (var j = 0; j < MaxNumberOfEntries; j++)
            {
                Assert.AreEqual(MaxNumberOfEntries, clonedOrderBook.Count);
                Assert.AreNotSame(populatedOnTickLastTraded[j], clonedOrderBook[j]);
            }
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_AccessIndexerVariousInterfaces_GetsAndSetsLastTradesRemovesLastEntryIfNull()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
            for (var i = 0; i < MaxNumberOfEntries; i++)
            {
                var lastTrade       = ((IOnTickLastTraded)populatedOnTickLastTraded)[i];
                var clonedLastTrade = (IPQLastTrade)lastTrade!.Clone();
                populatedOnTickLastTraded[i] = clonedLastTrade;
                Assert.AreNotSame(lastTrade, ((IMutableOnTickLastTraded)populatedOnTickLastTraded)[i]);
                Assert.AreSame(clonedLastTrade, populatedOnTickLastTraded[i]);
                if (i == populatedOnTickLastTraded.Count - 1)
                {
                    ((IMutableOnTickLastTraded)populatedOnTickLastTraded)[i] = populatedOnTickLastTraded[i].ResetWithTracking();
                    Assert.AreEqual(MaxNumberOfEntries - 1, populatedOnTickLastTraded.Count);
                }
            }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_Capacity_ShowMaxPossibleNumberOfEntriesNotNull()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            Assert.AreEqual(populatedOnTickLastTraded.Count, populatedOnTickLastTraded.Capacity);
            Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Capacity);
            populatedOnTickLastTraded[MaxNumberOfEntries - 1] = populatedOnTickLastTraded[MaxNumberOfEntries - 1].ResetWithTracking();
            Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Capacity);
            Assert.AreEqual(populatedOnTickLastTraded.Count, populatedOnTickLastTraded.Capacity - 1);
        }
    }


    [TestMethod]
    public void PopulatedOnTickLastTraded_Count_UpdatesWhenPricesChanged()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            for (var i = MaxNumberOfEntries - 1; i >= 0; i--)
            {
                Assert.AreEqual(i, populatedOnTickLastTraded.Count - 1);
                populatedOnTickLastTraded[i]?.StateReset();
            }

            Assert.AreEqual(0, populatedOnTickLastTraded.Count);
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTradedClearHasUpdates_HasUpdates_ChangeItemAtATimeReportsUpdates()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            Assert.IsTrue(populatedOnTickLastTraded.HasUpdates);
            populatedOnTickLastTraded.HasUpdates = false;
            Assert.IsFalse(populatedOnTickLastTraded.HasUpdates);
            foreach (var lt in populatedOnTickLastTraded)
            {
                lt.TradePrice = 3.456789m;
                Assert.IsTrue(populatedOnTickLastTraded.HasUpdates);
                Assert.IsTrue(lt.HasUpdates);
                lt.IsTradePriceUpdated = false;
                Assert.IsFalse(populatedOnTickLastTraded.HasUpdates);
                Assert.IsFalse(lt.HasUpdates);
                lt.TradeTime = new DateTime(2018, 01, 04, 22, 04, 51);
                Assert.IsTrue(populatedOnTickLastTraded.HasUpdates);
                Assert.IsTrue(lt.HasUpdates);
                lt.IsTradeTimeDateUpdated = false;
                Assert.IsTrue(populatedOnTickLastTraded.HasUpdates);
                Assert.IsTrue(lt.HasUpdates);
                lt.IsTradeTimeSub2MinUpdated = false;
                Assert.IsFalse(populatedOnTickLastTraded.HasUpdates);
                Assert.IsFalse(lt.HasUpdates);
                if (lt is IPQLastPaidGivenTrade lastPaidGivenTrade)
                {
                    Assert.IsFalse(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.TradeVolume = 42_121_333m;
                    Assert.IsTrue(populatedOnTickLastTraded.HasUpdates);
                    Assert.IsTrue(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.IsTradeVolumeUpdated = false;
                    Assert.IsFalse(populatedOnTickLastTraded.HasUpdates);
                    Assert.IsFalse(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.WasGiven = !lastPaidGivenTrade.WasGiven;
                    Assert.IsTrue(populatedOnTickLastTraded.HasUpdates);
                    Assert.IsTrue(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.IsWasGivenUpdated = false;
                    Assert.IsFalse(populatedOnTickLastTraded.HasUpdates);
                    Assert.IsFalse(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.WasPaid = !lastPaidGivenTrade.WasPaid;
                    Assert.IsTrue(populatedOnTickLastTraded.HasUpdates);
                    Assert.IsTrue(lastPaidGivenTrade.HasUpdates);
                    lastPaidGivenTrade.IsWasPaidUpdated = false;
                    Assert.IsFalse(populatedOnTickLastTraded.HasUpdates);
                    Assert.IsFalse(lastPaidGivenTrade.HasUpdates);
                }

                if (lt is IPQLastExternalCounterPartyTrade traderPaidGivenTrader)
                {
                    traderPaidGivenTrader.ExternalTraderName = "TestChangedTraderName";
                    Assert.IsTrue(populatedOnTickLastTraded.HasUpdates);
                    Assert.IsTrue(traderPaidGivenTrader.HasUpdates);
                    traderPaidGivenTrader.IsExternalTraderNameUpdated = false;
                    traderPaidGivenTrader.NameIdLookup.HasUpdates     = false;
                    Assert.IsFalse(populatedOnTickLastTraded.HasUpdates);
                    Assert.IsFalse(traderPaidGivenTrader.HasUpdates);
                }
            }
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_Reset_ResetsAllEntries()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Count);
            foreach (var pvl in populatedOnTickLastTraded) Assert.IsFalse(pvl.IsEmpty);
            populatedOnTickLastTraded.StateReset();
            Assert.AreEqual(0, populatedOnTickLastTraded.Count);
            foreach (var pvl in populatedOnTickLastTraded) Assert.IsTrue(pvl.IsEmpty);
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_Add_AppendsNewLastTradeToEndOfExisting()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Count);
            populatedOnTickLastTraded.Add(populatedOnTickLastTraded[0]!.Clone());
            Assert.AreEqual(MaxNumberOfEntries + 1, populatedOnTickLastTraded.Count);
            populatedOnTickLastTraded[MaxNumberOfEntries] = populatedOnTickLastTraded[MaxNumberOfEntries].ResetWithTracking();
            Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Count);
            populatedOnTickLastTraded.Add(populatedOnTickLastTraded[0]!.Clone());
            Assert.AreEqual(MaxNumberOfEntries + 1, populatedOnTickLastTraded.Count);
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTradedWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllOnTickLastTradedFields()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            var pqFieldUpdates =
                populatedOnTickLastTraded.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Update).ToList();
            AssertContainsAllLevelOnTickLastTradedFields(pqFieldUpdates, populatedOnTickLastTraded);
        }
    }

    [TestMethod]
    public void NoUpdatesPopulatedOnTickLastTraded_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllOnTickLastTradedFields()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            populatedOnTickLastTraded.HasUpdates = false;
            var pqFieldUpdates =
                populatedOnTickLastTraded.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Snapshot).ToList();
            AssertContainsAllLevelOnTickLastTradedFields(pqFieldUpdates, populatedOnTickLastTraded);
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTradedWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            populatedOnTickLastTraded.HasUpdates = false;
            var pqFieldUpdates =
                populatedOnTickLastTraded.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            var pqStringUpdates =
                populatedOnTickLastTraded.GetStringUpdates
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            Assert.AreEqual(0, pqStringUpdates.Count);
        }
    }


    [TestMethod]
    public void PopulatedOnTickLastTraded_GetDeltaUpdatesUpdateUpdateFields_CopiesAllFieldsToNewOnTickLastTraded()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            var pqFieldUpdates =
                populatedOnTickLastTraded.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var pqStringUpdates =
                populatedOnTickLastTraded.GetStringUpdates
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var newEmpty = CreateNewEmpty(populatedOnTickLastTraded);
            foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
            foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
            Assert.AreEqual(populatedOnTickLastTraded, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromToEmptyOnTickLastTraded_OnTickLastTradedEqualEachOther()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            var newEmpty = CreateNewEmpty(populatedOnTickLastTraded);
            newEmpty.CopyFrom(populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromSubTypes_SubTypeSaysIsEquivalent()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        foreach (var subType in allFullyPopulatedOnTickLastTraded.Where(ob => !ReferenceEquals(ob, populatedOnTickLastTraded)))
        {
            if (!WhollyContainedBy(subType[0]!.GetType(), populatedOnTickLastTraded[0]!.GetType())) continue;
            var newEmpty = new PQOnTickLastTraded((IOnTickLastTraded)populatedOnTickLastTraded);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOnTickLastTraded, newEmpty);
            newEmpty.CopyFrom(subType);
            Assert.IsTrue(subType.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromLessLayers_ReplicatesMissingValues()
    {
        var clonePopulated = simpleOnTickLastTradedFullyPopulatedQuote.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        Assert.AreEqual(MaxNumberOfEntries - 3, clonePopulated.Count);
        var notEmpty = new PQOnTickLastTraded((IOnTickLastTraded)simpleOnTickLastTradedFullyPopulatedQuote);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfEntries - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulateOnTickLastTraded_CopyFromWithNull_ReplicatesGapAsEmpty()
    {
        var clonePopulated = simpleOnTickLastTradedFullyPopulatedQuote.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);

        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();

        clonePopulated[5] = clonePopulated[5].ResetWithTracking();
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new PQOnTickLastTraded((IOnTickLastTraded)simpleOnTickLastTradedFullyPopulatedQuote);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated, CopyMergeFlags.UpdateFlagsNone);
        Assert.AreEqual(new PQLastTrade(), notEmpty[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleOnTickLastTradedFullyPopulatedQuote.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);

        clonePopulated[MaxNumberOfEntries - 1] = clonePopulated[MaxNumberOfEntries - 1].ResetWithTracking();
        clonePopulated[MaxNumberOfEntries - 2] = clonePopulated[MaxNumberOfEntries - 2].ResetWithTracking();

        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new PQOnTickLastTraded((IOnTickLastTraded)simpleOnTickLastTradedFullyPopulatedQuote)
        {
            [5] = simpleOnTickLastTradedFullyPopulatedQuote[5].Clone().ResetWithTracking()
        };
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOnTickTraded_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            var newEmpty = CreateNewEmpty(populatedOnTickLastTraded);
            populatedOnTickLastTraded.HasUpdates = false;
            newEmpty.CopyFrom(populatedOnTickLastTraded);
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
    public void NonPQOnTickLastTraded_CopyFromToEmptyOnTickLastTraded_OnTickLastTradedAreEqual()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            var nonPQOnTickLastTraded = new OnTickLastTraded(populatedOnTickLastTraded);
            var newEmpty              = CreateNewEmpty(populatedOnTickLastTraded);
            newEmpty.CopyFrom(nonPQOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, newEmpty);
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_EmptyCopyFromOtherOnTickLastTradedType_UpgradesToEverythingOnTickLastTradedItems()
    {
        foreach (var originalOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        foreach (var otherOnTickLastTraded in allFullyPopulatedOnTickLastTraded
                     .Where(ob => !ReferenceEquals(ob, originalOnTickLastTraded)))
        {
            var emptyOriginalTypeOrderBook = CreateNewEmpty(originalOnTickLastTraded);
            AssertAllLastTradesAreOfTypeAndEquivalentTo(emptyOriginalTypeOrderBook, originalOnTickLastTraded,
                                                        originalOnTickLastTraded[0]!.GetType(), false);
            emptyOriginalTypeOrderBook.CopyFrom(otherOnTickLastTraded);
            AssertAllLastTradesAreOfTypeAndEquivalentTo(emptyOriginalTypeOrderBook, otherOnTickLastTraded,
                                                        GetExpectedType(originalOnTickLastTraded[0]!.GetType(),
                                                                        otherOnTickLastTraded[0]!.GetType()));
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_CopyFromOnTickLastTraded_UpgradesToEverythingOnTickLastTradedItems()
    {
        foreach (var originalOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        foreach (var otherOnTickLastTraded in allFullyPopulatedOnTickLastTraded
                     .Where(ob => !ReferenceEquals(ob, originalOnTickLastTraded)))
        {
            var clonedPopulatedOrderBook = originalOnTickLastTraded.Clone();
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, originalOnTickLastTraded,
                                                        originalOnTickLastTraded[0]!.GetType(), false);
            clonedPopulatedOrderBook.CopyFrom(otherOnTickLastTraded);
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, otherOnTickLastTraded,
                                                        GetExpectedType(originalOnTickLastTraded[0]!.GetType(),
                                                                        otherOnTickLastTraded[0]!.GetType()));
            AssertAllLastTradesAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, originalOnTickLastTraded,
                                                        GetExpectedType(originalOnTickLastTraded[0]!.GetType(),
                                                                        otherOnTickLastTraded[0]!.GetType()));
        }
    }

    [TestMethod]
    public void FullyPopulateOnTickLastTraded_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            var clonedOnTickLastTraded = ((ICloneable<IOnTickLastTraded>)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clonedOnTickLastTraded, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clonedOnTickLastTraded);

            clonedOnTickLastTraded = ((IOnTickLastTraded)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clonedOnTickLastTraded, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clonedOnTickLastTraded);

            var clonedMutableOnTickLastTraded = ((IMutableOnTickLastTraded)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clonedMutableOnTickLastTraded, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clonedMutableOnTickLastTraded);

            clonedMutableOnTickLastTraded = ((ICloneable<IMutableOnTickLastTraded>)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clonedMutableOnTickLastTraded, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clonedMutableOnTickLastTraded);

            var clonedPQOnLastTraded = ((IPQOnTickLastTraded)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clonedPQOnLastTraded, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clonedPQOnLastTraded);

            clonedPQOnLastTraded = (IPQOnTickLastTraded)((ICloneable)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clonedPQOnLastTraded, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clonedPQOnLastTraded);
        }
    }

    [TestMethod]
    public void ClonedPopulatedOnTickLastTraded_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            var fullyPopulatedClone = (PQOnTickLastTraded)((ICloneable)populatedOnTickLastTraded).Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedOnTickLastTraded,
                                                                fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedOnTickLastTraded,
                                                                fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedOnLastTradedSameObj_Equals_ReturnsTrue()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            Assert.AreEqual(populatedOnTickLastTraded, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, ((ICloneable)populatedOnTickLastTraded).Clone());
            Assert.AreEqual(populatedOnTickLastTraded, ((ICloneable<IOnTickLastTraded>)populatedOnTickLastTraded).Clone());
            Assert.AreEqual(populatedOnTickLastTraded, ((ICloneable<IMutableOnTickLastTraded>)populatedOnTickLastTraded).Clone());
            Assert.AreEqual(populatedOnTickLastTraded, ((IMutableOnTickLastTraded)populatedOnTickLastTraded).Clone());
            Assert.AreEqual(populatedOnTickLastTraded, ((IPQOnTickLastTraded)populatedOnTickLastTraded).Clone());
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_GetHashCode_ReturnNumberNoException()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedOnTickLastTraded)
        {
            var hashCode = populatedOnTickLastTraded.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedQuote in allFullyPopulatedOnTickLastTraded)
        {
            var q = populatedQuote;

            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));

            Assert.IsTrue(toString.Contains(
                                            $"LastTrades: [{string.Join(", ", (IEnumerable<ILastTrade>)populatedQuote)}]"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
        }
    }

    [TestMethod]
    public void FullyPopulatedLastTradeVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = fullSupportLastTradeOnTickLastTradedFullyPopulatedQuote;
        Assert.AreEqual(MaxNumberOfEntries, rt.Count);
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<IPQLastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<IMutableLastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<ILastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, rt.OfType<ILastTrade>().Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable<IPQLastTrade>)rt).Count());
        Assert.AreEqual(0, ((IEnumerable<IMutableLastTrade>)rt).Count());
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
    (PQOnTickLastTraded upgradedOnTickLastTraded,
        PQOnTickLastTraded equivalentTo, Type expectedType, bool compareForEquivalence = true,
        bool exactlyEquals = false)
    {
        for (var i = 0; i < upgradedOnTickLastTraded.Capacity; i++)
        {
            var upgradedLastTrade = upgradedOnTickLastTraded[i];
            var copyFromLastTrade = equivalentTo[i];

            Assert.IsInstanceOfType(upgradedLastTrade, expectedType);
            if (compareForEquivalence) Assert.IsTrue(copyFromLastTrade!.AreEquivalent(upgradedLastTrade, exactlyEquals));
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

    private static PQOnTickLastTraded CreateNewEmpty(PQOnTickLastTraded populatedOnTickLastTraded)
    {
        var cloneGenesis = populatedOnTickLastTraded[0]!.Clone();
        cloneGenesis.StateReset();
        if (cloneGenesis is IPQLastExternalCounterPartyTrade traderLastTrade)
            traderLastTrade.NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        var clonedEmptyEntries = new List<IPQLastTrade>(MaxNumberOfEntries);
        for (var i = 0; i < MaxNumberOfEntries; i++) clonedEmptyEntries.Add(cloneGenesis.Clone());
        var newEmpty = new PQOnTickLastTraded(clonedEmptyEntries!);
        return newEmpty;
    }

    public static void AssertContainsAllLevelOnTickLastTradedFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        IPQOnTickLastTraded onTickLastTraded, PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        for (var i = 0; i < onTickLastTraded.Count; i++)
        {
            var lastTrade = onTickLastTraded[i]!;
            var depthId   = (PQDepthKey)i;


            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, depthId, PQTradingSubFieldKeys.LastTradedAtPrice, lastTrade.TradePrice, priceScale)
                           ,
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedTickTrades, depthId,
                                                                        PQTradingSubFieldKeys.LastTradedAtPrice, priceScale),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, depthId, PQTradingSubFieldKeys.LastTradedTradeTimeDate,
                                              lastTrade.TradeTime.Get2MinIntervalsFromUnixEpoch()),
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedTickTrades, depthId,
                                                                        PQTradingSubFieldKeys.LastTradedTradeTimeDate),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var extended = lastTrade.TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBase);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, depthId, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHourBase, extended)
                          , PQTickInstantTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, depthId, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime
                               , extended),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (lastTrade is IPQLastPaidGivenTrade pqPaidGivenTrade)
            {
                var lastTradedBoolFlags = pqPaidGivenTrade.WasGiven ? LastTradeBooleanFlags.WasGiven : LastTradeBooleanFlags.None;
                lastTradedBoolFlags |= pqPaidGivenTrade.WasPaid ? LastTradeBooleanFlags.WasPaid : LastTradeBooleanFlags.None;


                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, depthId, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)lastTradedBoolFlags)
                               ,
                                PQTickInstantTests.ExtractFieldUpdateWithId
                                    (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, depthId, PQTradingSubFieldKeys.LastTradedBooleanFlags),
                                $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }

            if (lastTrade is IPQLastExternalCounterPartyTrade pqTraderPaidGivenTrade)
            {
                var lastTradedTraderNameId = (uint)pqTraderPaidGivenTrade.ExternalTraderNameId;
                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, depthId, PQTradingSubFieldKeys.LastTradedExternalTraderNameId, lastTradedTraderNameId)
                               ,
                                PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedTickTrades, depthId,
                                                                            PQTradingSubFieldKeys.LastTradedExternalTraderNameId),
                                $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }
        }
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQOnTickLastTraded original, PQOnTickLastTraded changingOnTickLastTraded, PQPublishableLevel3Quote? originalQuote = null,
        PQPublishableLevel3Quote? changingQuote = null)
    {
        if (original.GetType() == typeof(PQOnTickLastTraded))
            Assert.AreEqual(!exactComparison,
                            changingOnTickLastTraded.AreEquivalent(new OnTickLastTraded(original), exactComparison));

        Assert.AreEqual(original.Count, changingOnTickLastTraded.Count);

        for (var i = 0; i < original.Count; i++)
        {
            var originalEntry = original[i];
            var changingEntry = changingOnTickLastTraded[i]!;
            PQLastTradeTests
                .AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalEntry as PQLastTrade,
                     (PQLastTrade)changingEntry, original,
                     changingOnTickLastTraded, originalQuote, changingQuote);
            PQLastPaidGivenTradeTests
                .AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalEntry as PQLastPaidGivenTrade,
                     changingEntry as PQLastPaidGivenTrade, original,
                     changingOnTickLastTraded, originalQuote, changingQuote);
            PQLastExternalCounterPartyTradeTests
                .AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalEntry as PQLastExternalCounterPartyTrade,
                     changingEntry as PQLastExternalCounterPartyTrade, original,
                     changingOnTickLastTraded, originalQuote, changingQuote);
        }
    }
}
