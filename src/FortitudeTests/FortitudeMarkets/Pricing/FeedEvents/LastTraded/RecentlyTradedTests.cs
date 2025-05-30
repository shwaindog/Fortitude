﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.LastTraded;

[TestClass]
public class RecentlyTradedTests
{
    private const int MaxNumberOfEntries = QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades - 1;
    
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


    private IList<RecentlyTraded> allFullyPopulatedRecentlyTraded = null!;

    private List<IReadOnlyList<ILastTrade>>   allPopulatedEntries  = null!;
    private IList<IMutableLastPaidGivenTrade> lastPaidGivenEntries = null!;

    private IList<IMutableLastExternalCounterPartyTrade> lastTraderPaidGivenEntries = null!;

    private RecentlyTraded paidGivenVolumeFullyPopulatedRecentlyTraded = null!;

    private IList<IMutableLastTrade> simpleEntries = null!;

    private RecentlyTraded simpleFullyPopulatedRecentlyTraded = null!;

    private RecentlyTraded fullSupportLastTradesFullyPopulatedRecentlyTraded = null!;
    // test being less than max.

    [TestInitialize]
    public void SetUp()
    {
        simpleEntries        = new List<IMutableLastTrade>(MaxNumberOfEntries);
        lastPaidGivenEntries = new List<IMutableLastPaidGivenTrade>(MaxNumberOfEntries);

        lastTraderPaidGivenEntries = new List<IMutableLastExternalCounterPartyTrade>(MaxNumberOfEntries);

        allPopulatedEntries =
        [
            (IReadOnlyList<ILastTrade>)simpleEntries, (IReadOnlyList<ILastTrade>)lastPaidGivenEntries
          , (IReadOnlyList<ILastTrade>)lastTraderPaidGivenEntries
        ];

        for (var i = 0; i < MaxNumberOfEntries; i++)
        {
            simpleEntries.Add(new LastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                          , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime));
            lastPaidGivenEntries.Add
                (new LastPaidGivenTrade
                    (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId, ExpectedWasPaid
                   , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
                   , ExpectedUpdateTime));
            lastTraderPaidGivenEntries.Add
                (new LastExternalCounterPartyTrade
                    (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedCounterPartyId
                   , ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid, ExpectedWasGiven
                   , ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime));
        }

        simpleFullyPopulatedRecentlyTraded          = new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, simpleEntries);
        paidGivenVolumeFullyPopulatedRecentlyTraded = new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, lastPaidGivenEntries);

        fullSupportLastTradesFullyPopulatedRecentlyTraded = new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, lastTraderPaidGivenEntries);

        allFullyPopulatedRecentlyTraded = new List<RecentlyTraded>
        {
            simpleFullyPopulatedRecentlyTraded, paidGivenVolumeFullyPopulatedRecentlyTraded
          , fullSupportLastTradesFullyPopulatedRecentlyTraded
        };
    }


    [TestMethod]
    public void NewRecentlyTraded_InitializedWithEntries_ContainsSameInstanceEntryAsInitialized()
    {
        for (var i = 0; i < allFullyPopulatedRecentlyTraded.Count; i++)
        {
            var populatedRecentlyTraded = allFullyPopulatedRecentlyTraded[i];
            var populatedEntries        = allPopulatedEntries[i];
            for (var j = 0; j < MaxNumberOfEntries; j++)
            {
                Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Count);
                Assert.AreSame(populatedEntries[j], populatedRecentlyTraded[j]);
            }
        }
    }

    [TestMethod]
    public void NewRecentlyTraded_InitializedFromRecentlyTraded_ClonesAllEntries()
    {
        for (var i = 0; i < allFullyPopulatedRecentlyTraded.Count; i++)
        {
            IRecentlyTraded populatedOrderBook = allFullyPopulatedRecentlyTraded[i];

            var clonedOrderBook = new PQRecentlyTraded(populatedOrderBook);
            for (var j = 0; j < MaxNumberOfEntries; j++)
            {
                Assert.AreEqual(MaxNumberOfEntries, clonedOrderBook.Count);
                Assert.AreNotSame(populatedOrderBook[j], clonedOrderBook[j]);
            }
        }
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_AccessIndexerVariousInterfaces_GetsAndSetsLastTradeRemovesLastEntryIfNull()
    {
        foreach (var populatedOrderBook in allFullyPopulatedRecentlyTraded)
            for (var i = 0; i < MaxNumberOfEntries; i++)
            {
                var lastTrade       = ((IRecentlyTraded)populatedOrderBook)[i];
                var clonedLastTrade = (IMutableLastTrade)lastTrade.Clone();
                populatedOrderBook[i] = clonedLastTrade;
                Assert.AreNotSame(lastTrade, ((IMutableRecentlyTraded)populatedOrderBook)[i]);
                Assert.AreSame(clonedLastTrade, populatedOrderBook[i]);
                if (i == populatedOrderBook.Count - 1)
                {
                    ((IMutableRecentlyTraded)populatedOrderBook)[i] = populatedOrderBook[i].ResetWithTracking();
                    Assert.AreEqual(MaxNumberOfEntries - 1, populatedOrderBook.Count);
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
            Assert.AreEqual(populatedRecentlyTraded.Count + 1, populatedRecentlyTraded.Capacity);
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
    public void StaticDefault_EntryConverter_IsPQLastTradeEntrySelector()
    {
        Assert.IsInstanceOfType
            (LastTradedList.LastTradeEntrySelector, typeof(LastTradedLastTradeEntrySelector));
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
        foreach (var populatedOrderBook in allFullyPopulatedRecentlyTraded)
        foreach (var subType in allFullyPopulatedRecentlyTraded.Where(ob => !ReferenceEquals(ob, populatedOrderBook)))
        {
            if (!WhollyContainedBy(subType[0].GetType(), populatedOrderBook[0].GetType())) continue;
            var newEmpty = new RecentlyTraded(populatedOrderBook);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            newEmpty.CopyFrom(subType);
            Assert.IsTrue(subType.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_CopyFromLessLastTrade_ReplicatesMissingValues()
    {
        var clonePopulated = simpleFullyPopulatedRecentlyTraded.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        Assert.AreEqual(MaxNumberOfEntries - 3, clonePopulated.Count);
        var notEmpty = new RecentlyTraded(simpleFullyPopulatedRecentlyTraded);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfEntries - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGap()
    {
        var clonePopulated = simpleFullyPopulatedRecentlyTraded.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[5]  = clonePopulated[5].ResetWithTracking();
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new RecentlyTraded(simpleFullyPopulatedRecentlyTraded);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleFullyPopulatedRecentlyTraded.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        clonePopulated[^1] = clonePopulated[^1].ResetWithTracking();
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new RecentlyTraded(simpleFullyPopulatedRecentlyTraded)
        {
            [5] = simpleFullyPopulatedRecentlyTraded[^5].Clone().ResetWithTracking()
        };
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void PQRecentlyTraded_CopyFromToEmptyRecentlyTraded_RecentlyTradedAreEqual()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var nonPQOrderBook = new PQRecentlyTraded(populatedRecentlyTraded);

            var newEmpty = CreateNewEmpty(populatedRecentlyTraded);
            newEmpty.CopyFrom(nonPQOrderBook);
            Assert.AreEqual(populatedRecentlyTraded, newEmpty);
        }
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var clone = (object)populatedRecentlyTraded.Clone();
            Assert.AreNotSame(clone, populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, clone);
            clone = ((ICloneable<IRecentlyTraded>)populatedRecentlyTraded).Clone();
            Assert.AreNotSame(clone, populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, clone);
            clone = ((ICloneable)populatedRecentlyTraded).Clone();
            Assert.AreNotSame(clone, populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, clone);
            clone = ((IMutableRecentlyTraded)populatedRecentlyTraded).Clone();
            Assert.AreNotSame(clone, populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, clone);
        }
    }

    [TestMethod]
    public void ClonedPopulatedRecentlyTraded_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var fullyPopulatedClone = (RecentlyTraded)((ICloneable)populatedRecentlyTraded).Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType
                (true, populatedRecentlyTraded, fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType
                (false, populatedRecentlyTraded, fullyPopulatedClone);
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
    public void FullyPopulatedRecentlyTrade_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var q        = populatedRecentlyTraded;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));

            Assert.IsTrue(toString.Contains($"LastTrades: [{string.Join(",", (IEnumerable<ILastTrade>)populatedRecentlyTraded)}]"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
        }
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = fullSupportLastTradesFullyPopulatedRecentlyTraded;
        Assert.AreEqual(MaxNumberOfEntries, rt.Count);
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<ILastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable)rt).OfType<ILastTrade>().Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable<IMutableLastTrade>)rt).Count());
        Assert.AreEqual(0, rt.OfType<ILastTrade>().Count());
    }

    private static RecentlyTraded CreateNewEmpty(RecentlyTraded populatedOrderBook)
    {
        var cloneGenesis = populatedOrderBook[0].Clone();
        cloneGenesis.StateReset();
        var clonedEmptyEntries = new List<ILastTrade>(MaxNumberOfEntries);
        for (var i = 0; i < MaxNumberOfEntries; i++) clonedEmptyEntries.Add(cloneGenesis.Clone());
        var newEmpty = new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, clonedEmptyEntries);
        return newEmpty;
    }

    private bool WhollyContainedBy(Type copySourceType, Type copyDestinationType)
    {
        if (copySourceType == typeof(LastTrade)) return true;
        if (copySourceType == typeof(LastPaidGivenTrade))
            return copyDestinationType == typeof(LastTrade) ||
                   copyDestinationType == typeof(LastPaidGivenTrade);
        if (copySourceType == typeof(LastExternalCounterPartyTrade))
            return copyDestinationType == typeof(LastTrade) ||
                   copyDestinationType == typeof(LastPaidGivenTrade) ||
                   copyDestinationType == typeof(LastExternalCounterPartyTrade);
        return false;
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableRecentlyTraded? original, IMutableRecentlyTraded? changingRecentlyTraded,
        IMutablePublishableLevel3Quote? originalQuote = null, IMutablePublishableLevel3Quote? changingQuote = null)
    {
        if (original == null && changingRecentlyTraded == null) return;

        Assert.IsTrue(changingRecentlyTraded!.AreEquivalent(original, exactComparison));

        Assert.AreEqual(original!.Count, changingRecentlyTraded.Count);

        for (var i = 0; i < original.Count; i++)
        {
            var originalEntry = original[i];
            var changingEntry = changingRecentlyTraded[i];
            LastTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (exactComparison, originalEntry, changingEntry, original, changingRecentlyTraded, originalQuote, changingQuote);
            if (originalEntry is IMutableLastPaidGivenTrade trade)
                LastPaidGivenTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, trade
                   , (IMutableLastPaidGivenTrade)changingEntry, original, changingRecentlyTraded, originalQuote, changingQuote);
            if (originalEntry is IMutableLastExternalCounterPartyTrade paidGivenTrade)
                LastExternalCounterPartyTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, paidGivenTrade, (IMutableLastExternalCounterPartyTrade)changingEntry
                   , original, changingRecentlyTraded, originalQuote, changingQuote);
        }
    }
}
