// Licensed under the MIT license.
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
public class OnTickLastTradedTests
{
    private const int MaxNumberOfEntries = QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades - 1;

    private IList<OnTickLastTraded> allFullyPopulatedROnTickLastTraded = null!;

    private List<IReadOnlyList<ILastTrade>>   allPopulatedEntries  = null!;
    private IList<IMutableLastPaidGivenTrade> lastPaidGivenEntries = null!;

    private IList<IMutableLastTraderPaidGivenTrade> lastTraderPaidGivenEntries = null!;

    private OnTickLastTraded paidGivenVolumeFullyPopulatedOnTickLastTraded = null!;

    private IList<IMutableLastTrade> simpleEntries = null!;

    private OnTickLastTraded simpleFullyPopulatedOnTickLastTraded = null!;

    private OnTickLastTraded fullSupportLastTradesFullyPopulatedOnTickLastTraded = null!;
    // test being less than max.

    [TestInitialize]
    public void SetUp()
    {
        simpleEntries        = new List<IMutableLastTrade>(MaxNumberOfEntries);
        lastPaidGivenEntries = new List<IMutableLastPaidGivenTrade>(MaxNumberOfEntries);

        lastTraderPaidGivenEntries = new List<IMutableLastTraderPaidGivenTrade>(MaxNumberOfEntries);

        allPopulatedEntries =
        [
            (IReadOnlyList<ILastTrade>)simpleEntries, (IReadOnlyList<ILastTrade>)lastPaidGivenEntries
          , (IReadOnlyList<ILastTrade>)lastTraderPaidGivenEntries
        ];

        for (var i = 0; i < MaxNumberOfEntries; i++)
        {
            simpleEntries.Add(new LastTrade(1.234567m, new DateTime(2018, 1, 2, 22, 52, 59)));
            lastPaidGivenEntries.Add
                (new LastPaidGivenTrade
                    (1.234567m, new DateTime(2018, 1, 2, 22, 52, 59), 40_111_222m, true, true));
            lastTraderPaidGivenEntries.Add
                (new LastTraderPaidGivenTrade
                    (1.234567m, new DateTime(2018, 1, 2, 22, 52, 59), 40_111_222m
                   , true, true, "TestTraderName"));
        }

        simpleFullyPopulatedOnTickLastTraded          = new OnTickLastTraded(simpleEntries);
        paidGivenVolumeFullyPopulatedOnTickLastTraded = new OnTickLastTraded(lastPaidGivenEntries);

        fullSupportLastTradesFullyPopulatedOnTickLastTraded = new OnTickLastTraded(lastTraderPaidGivenEntries);

        allFullyPopulatedROnTickLastTraded = new List<OnTickLastTraded>
        {
            simpleFullyPopulatedOnTickLastTraded, paidGivenVolumeFullyPopulatedOnTickLastTraded
          , fullSupportLastTradesFullyPopulatedOnTickLastTraded
        };
    }


    [TestMethod]
    public void NewOnTickLastTraded_InitializedWithEntries_ContainsSameInstanceEntryAsInitialized()
    {
        for (var i = 0; i < allFullyPopulatedROnTickLastTraded.Count; i++)
        {
            var populatedOnTickLastTraded = allFullyPopulatedROnTickLastTraded[i];
            var populatedEntries          = allPopulatedEntries[i];
            for (var j = 0; j < MaxNumberOfEntries; j++)
            {
                Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Count);
                Assert.AreSame(populatedEntries[j], populatedOnTickLastTraded[j]);
            }
        }
    }

    [TestMethod]
    public void NewOnTickLastTraded_InitializedFromOnTickLastTraded_ClonesAllEntries()
    {
        for (var i = 0; i < allFullyPopulatedROnTickLastTraded.Count; i++)
        {
            IOnTickLastTraded populatedOnTickLastTraded = allFullyPopulatedROnTickLastTraded[i];

            var clonedOnTickLastTraded = new PQOnTickLastTraded(populatedOnTickLastTraded);
            for (var j = 0; j < MaxNumberOfEntries; j++)
            {
                Assert.AreEqual(MaxNumberOfEntries, clonedOnTickLastTraded.Count);
                Assert.AreNotSame(populatedOnTickLastTraded[j], clonedOnTickLastTraded[j]);
            }
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_AccessIndexerVariousInterfaces_GetsAndSetsLastTradeRemovesLastEntryIfNull()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
            for (var i = 0; i < MaxNumberOfEntries; i++)
            {
                var lastTrade       = ((IOnTickLastTraded)populatedOnTickLastTraded)[i];
                var clonedLastTrade = lastTrade?.Clone() as IMutableLastTrade;
                populatedOnTickLastTraded[i] = clonedLastTrade;
                Assert.AreNotSame(lastTrade, ((IMutableOnTickLastTraded)populatedOnTickLastTraded)[i]);
                Assert.AreSame(clonedLastTrade, populatedOnTickLastTraded[i]);
                if (i == populatedOnTickLastTraded.Count - 1)
                {
                    ((IMutableOnTickLastTraded)populatedOnTickLastTraded)[i] = null;
                    Assert.AreEqual(MaxNumberOfEntries - 1, populatedOnTickLastTraded.Count);
                }
            }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_Capacity_ShowMaxPossibleNumberOfEntriesNotNull()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            Assert.AreEqual(populatedOnTickLastTraded.Count, populatedOnTickLastTraded.Capacity);
            Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Capacity);
            populatedOnTickLastTraded[MaxNumberOfEntries - 1] = null;
            Assert.AreEqual(MaxNumberOfEntries - 1, populatedOnTickLastTraded.Capacity);
            Assert.AreEqual(populatedOnTickLastTraded.Count, populatedOnTickLastTraded.Capacity);
        }
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_Count_UpdatesWhenPricesChanged()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
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
    public void StaticDefault_EntryConverter_IsPQLastTradeEntrySelector()
    {
        Assert.IsInstanceOfType
            (LastTradedList.LastTradeEntrySelector, typeof(LastTradedLastTradeEntrySelector));
    }

    [TestMethod]
    public void PopulatedOnTickLastTraded_Reset_ResetsAllEntries()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
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
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Count);
            populatedOnTickLastTraded.Add(populatedOnTickLastTraded[0]!.Clone());
            Assert.AreEqual(MaxNumberOfEntries + 1, populatedOnTickLastTraded.Count);
            populatedOnTickLastTraded[MaxNumberOfEntries] = null;
            Assert.AreEqual(MaxNumberOfEntries, populatedOnTickLastTraded.Count);
            populatedOnTickLastTraded.Add(populatedOnTickLastTraded[0]!.Clone());
            Assert.AreEqual(MaxNumberOfEntries + 1, populatedOnTickLastTraded.Count);
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromToEmptyOnTickLastTraded_EqualEachOther()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            var newEmpty = CreateNewEmpty(populatedOnTickLastTraded);
            newEmpty.CopyFrom(populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromSubTypes_SubTypeSaysIsEquivalent()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        foreach (var subType in allFullyPopulatedROnTickLastTraded.Where(ob => !ReferenceEquals(ob, populatedOnTickLastTraded)))
        {
            if (!WhollyContainedBy(subType[0]!.GetType(), populatedOnTickLastTraded[0]!.GetType())) continue;
            var newEmpty = new OnTickLastTraded(populatedOnTickLastTraded);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOnTickLastTraded, newEmpty);
            newEmpty.CopyFrom(subType);
            Assert.IsTrue(subType.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromLessLastTrade_ReplicatesMissingValues()
    {
        var clonePopulated = simpleFullyPopulatedOnTickLastTraded.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[^1] = null;
        clonePopulated[^1] = null;
        clonePopulated[^1] = null;
        Assert.AreEqual(MaxNumberOfEntries - 3, clonePopulated.Count);
        var notEmpty = new OnTickLastTraded(simpleFullyPopulatedOnTickLastTraded);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfEntries - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromWithNull_ReplicatesGap()
    {
        var clonePopulated = simpleFullyPopulatedOnTickLastTraded.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[^1] = null;
        clonePopulated[^1] = null;
        clonePopulated[5]  = null;
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new OnTickLastTraded(simpleFullyPopulatedOnTickLastTraded);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleFullyPopulatedOnTickLastTraded.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[^1] = null;
        clonePopulated[^1] = null;
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new OnTickLastTraded(simpleFullyPopulatedOnTickLastTraded) { [5] = null };
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void PQOnTickLastTraded_CopyFromToEmptyOnTickLastTraded_AreEqual()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            var nonPQOnTickLastTraded = new PQOnTickLastTraded(populatedOnTickLastTraded);

            var newEmpty = CreateNewEmpty(populatedOnTickLastTraded);
            newEmpty.CopyFrom(nonPQOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, newEmpty);
        }
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            var clone = (object)populatedOnTickLastTraded.Clone();
            Assert.AreNotSame(clone, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clone);
            clone = ((ICloneable<IOnTickLastTraded>)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clone, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clone);
            clone = ((ICloneable)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clone, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clone);
            clone = ((IOnTickLastTraded)populatedOnTickLastTraded).Clone();
            Assert.AreNotSame(clone, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, clone);
        }
    }

    [TestMethod]
    public void ClonedPopulatedOnTickLastTraded_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            var fullyPopulatedClone = (OnTickLastTraded)((ICloneable)populatedOnTickLastTraded).Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType
                (true, populatedOnTickLastTraded, fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType
                (false, populatedOnTickLastTraded, fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTradedSameObj_Equals_ReturnsTrue()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            Assert.AreEqual(populatedOnTickLastTraded, populatedOnTickLastTraded);
            Assert.AreEqual(populatedOnTickLastTraded, ((ICloneable)populatedOnTickLastTraded).Clone());
            Assert.AreEqual(populatedOnTickLastTraded, ((ICloneable<IOnTickLastTraded>)populatedOnTickLastTraded).Clone());
            Assert.AreEqual(populatedOnTickLastTraded, ((IMutableOnTickLastTraded)populatedOnTickLastTraded).Clone());
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_GetHashCode_ReturnNumberNoException()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            var hashCode = populatedOnTickLastTraded.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }
    }

    [TestMethod]
    public void FullyPopulatedOnTickLastTraded_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedOnTickLastTraded in allFullyPopulatedROnTickLastTraded)
        {
            var q        = populatedOnTickLastTraded;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));

            Assert.IsTrue(toString.Contains($"LastTrades: [{string.Join(",", (IEnumerable<ILastTrade>)populatedOnTickLastTraded)}]"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
        }
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = fullSupportLastTradesFullyPopulatedOnTickLastTraded;
        Assert.AreEqual(MaxNumberOfEntries, rt.Count);
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<ILastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable)rt).OfType<ILastTrade>().Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable<IMutableLastTrade>)rt).Count());
        Assert.AreEqual(0, rt.OfType<ILastTrade>().Count());
    }

    private static OnTickLastTraded CreateNewEmpty(OnTickLastTraded populatedOnTickLastTraded)
    {
        var cloneGenesis = populatedOnTickLastTraded[0]!.Clone();
        cloneGenesis.StateReset();
        var clonedEmptyEntries = new List<ILastTrade>(MaxNumberOfEntries);
        for (var i = 0; i < MaxNumberOfEntries; i++) clonedEmptyEntries.Add(cloneGenesis.Clone());
        var newEmpty = new OnTickLastTraded(clonedEmptyEntries);
        return newEmpty;
    }

    private bool WhollyContainedBy(Type copySourceType, Type copyDestinationType)
    {
        if (copySourceType == typeof(LastTrade)) return true;
        if (copySourceType == typeof(LastPaidGivenTrade))
            return copyDestinationType == typeof(LastTrade) ||
                   copyDestinationType == typeof(LastPaidGivenTrade);
        if (copySourceType == typeof(LastTraderPaidGivenTrade))
            return copyDestinationType == typeof(LastTrade) ||
                   copyDestinationType == typeof(LastPaidGivenTrade) ||
                   copyDestinationType == typeof(LastTraderPaidGivenTrade);
        return false;
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableLastTradedList? original, IMutableLastTradedList? changingOnTickLastTraded,
        IMutablePublishableLevel3Quote? originalQuote = null, IMutablePublishableLevel3Quote? changingQuote = null)
    {
        if (original == null && changingOnTickLastTraded == null) return;

        Assert.IsTrue(changingOnTickLastTraded!.AreEquivalent(original, exactComparison));

        Assert.AreEqual(original!.Count, changingOnTickLastTraded.Count);

        for (var i = 0; i < original.Count; i++)
        {
            var originalEntry = original[i];
            var changingEntry = changingOnTickLastTraded[i];
            LastTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (exactComparison, originalEntry, changingEntry, original, changingOnTickLastTraded, originalQuote, changingQuote);
            if (originalEntry is IMutableLastPaidGivenTrade trade)
                LastPaidGivenTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, trade
                   , (IMutableLastPaidGivenTrade)changingEntry!, original, changingOnTickLastTraded, originalQuote, changingQuote);
            if (originalEntry is IMutableLastTraderPaidGivenTrade paidGivenTrade)
                LastTraderPaidGivenTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, paidGivenTrade, (IMutableLastTraderPaidGivenTrade)changingEntry!
                   , original, changingOnTickLastTraded, originalQuote, changingQuote);
        }
    }
}
