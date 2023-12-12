#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.LastTraded.EntrySelector;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded;

[TestClass]
public class RecentlyTradedTests
{
    private const int MaxNumberOfEntries = PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades - 1;

    private IList<RecentlyTraded> allFullyPopulatedRecentlyTraded = null!;

    private List<IReadOnlyList<ILastTrade>> allPopulatedEntries = null!;
    private IList<IMutableLastPaidGivenTrade> lastPaidGivenEntries = null!;
    private IList<IMutableLastTraderPaidGivenTrade> lastTraderPaidGivenEntries = null!;
    private RecentlyTraded paidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;

    private IList<IMutableLastTrade> simpleEntries = null!;
    private RecentlyTraded simpleRecentlyTradedFullyPopulatedQuote = null!;

    private RecentlyTraded traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;
    // test being less than max.

    [TestInitialize]
    public void SetUp()
    {
        simpleEntries = new List<IMutableLastTrade>(MaxNumberOfEntries);
        lastPaidGivenEntries = new List<IMutableLastPaidGivenTrade>(MaxNumberOfEntries);
        lastTraderPaidGivenEntries = new List<IMutableLastTraderPaidGivenTrade>(MaxNumberOfEntries);

        allPopulatedEntries = new List<IReadOnlyList<ILastTrade>>
        {
            (IReadOnlyList<ILastTrade>)simpleEntries, (IReadOnlyList<ILastTrade>)lastPaidGivenEntries
            , (IReadOnlyList<ILastTrade>)lastTraderPaidGivenEntries
        };

        for (var i = 0; i < MaxNumberOfEntries; i++)
        {
            simpleEntries.Add(new LastTrade(1.234567m, new DateTime(2018, 1, 2, 22, 52, 59)));
            lastPaidGivenEntries.Add(new LastPaidGivenTrade(1.234567m, new DateTime(2018, 1, 2, 22, 52, 59),
                40_111_222m, true, true));
            lastTraderPaidGivenEntries.Add(new LastTraderPaidGivenTrade(1.234567m,
                new DateTime(2018, 1, 2, 22, 52, 59), 40_111_222m, true, true, "TestTraderName"));
        }

        simpleRecentlyTradedFullyPopulatedQuote = new RecentlyTraded(simpleEntries);
        paidGivenVolumeRecentlyTradedFullyPopulatedQuote = new RecentlyTraded(lastPaidGivenEntries);
        traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote = new RecentlyTraded(lastTraderPaidGivenEntries);

        allFullyPopulatedRecentlyTraded = new List<RecentlyTraded>
        {
            simpleRecentlyTradedFullyPopulatedQuote, paidGivenVolumeRecentlyTradedFullyPopulatedQuote
            , traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote
        };
    }


    [TestMethod]
    public void NewRecentlyTraded_InitializedWithEntries_ContainsSameInstanceEntryAsInitialized()
    {
        for (var i = 0; i < allFullyPopulatedRecentlyTraded.Count; i++)
        {
            var populatedRecentlyTraded = allFullyPopulatedRecentlyTraded[i];
            var populatedEntries = allPopulatedEntries[i];
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
    public void PopulatedRecentlyTraded_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull()
    {
        foreach (var populatedOrderBook in allFullyPopulatedRecentlyTraded)
            for (var i = 0; i < MaxNumberOfEntries; i++)
            {
                var layer = ((IRecentlyTraded)populatedOrderBook)[i];
                var clonedLayer = layer?.Clone() as IMutableLastTrade;
                populatedOrderBook[i] = clonedLayer;
                Assert.AreNotSame(layer, ((IMutableRecentlyTraded)populatedOrderBook)[i]);
                Assert.AreSame(clonedLayer, populatedOrderBook[i]);
                if (i == populatedOrderBook.Count - 1)
                {
                    ((IMutableRecentlyTraded)populatedOrderBook)[i] = null;
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
            populatedRecentlyTraded[MaxNumberOfEntries - 1] = null;
            Assert.AreEqual(MaxNumberOfEntries - 1, populatedRecentlyTraded.Capacity);
            Assert.AreEqual(populatedRecentlyTraded.Count, populatedRecentlyTraded.Capacity);
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
    public void StaticDefault_EntryConverter_IsPQLastTradeEntySelector()
    {
        Assert.IsInstanceOfType(RecentlyTraded.LastTradeEntrySelector,
            typeof(RecentlyTradedLastTradeEntrySelector));
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
            populatedRecentlyTraded[MaxNumberOfEntries] = null;
            Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Count);
            populatedRecentlyTraded.Add(populatedRecentlyTraded[0]!.Clone());
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
            if (!WholeyContainedBy(subType[0]!.GetType(), populatedOrderBook[0]!.GetType())) continue;
            var newEmpty = new RecentlyTraded(populatedOrderBook);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            newEmpty.CopyFrom(subType);
            Assert.IsTrue(subType.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedRecentlyTraded_CopyFromLessLayers_ReplicatesMissingValues()
    {
        var clonePopulated = simpleRecentlyTradedFullyPopulatedQuote.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        Assert.AreEqual(MaxNumberOfEntries - 3, clonePopulated.Count);
        var notEmpty = new RecentlyTraded(simpleRecentlyTradedFullyPopulatedQuote);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfEntries - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGap()
    {
        var clonePopulated = simpleRecentlyTradedFullyPopulatedQuote.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[5] = null;
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new RecentlyTraded(simpleRecentlyTradedFullyPopulatedQuote);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleRecentlyTradedFullyPopulatedQuote.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new RecentlyTraded(simpleRecentlyTradedFullyPopulatedQuote) { [5] = null };
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
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedQuote in allFullyPopulatedRecentlyTraded)
        {
            var q = populatedQuote;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));

            Assert.IsTrue(toString.Contains($"LastTrades: [{string.Join(",", populatedQuote)}]"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
        }
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote;
        Assert.AreEqual(MaxNumberOfEntries, rt.Count);
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<ILastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable)rt).OfType<ILastTrade>().Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, rt.Count());
        Assert.AreEqual(0, rt.OfType<ILastTrade>().Count());
    }

    private static RecentlyTraded CreateNewEmpty(RecentlyTraded populatedOrderBook)
    {
        var cloneGensis = populatedOrderBook[0]!.Clone();
        cloneGensis.StateReset();
        var clonedEmptyEntries = new List<ILastTrade>(MaxNumberOfEntries);
        for (var i = 0; i < MaxNumberOfEntries; i++) clonedEmptyEntries.Add(cloneGensis.Clone());
        var newEmpty = new RecentlyTraded(clonedEmptyEntries);
        return newEmpty;
    }

    private bool WholeyContainedBy(Type copySourceType, Type copyDestinationType)
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

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IMutableRecentlyTraded? original, IMutableRecentlyTraded? changingRecentlyTraded,
        IMutableLevel3Quote? originalQuote = null, IMutableLevel3Quote? changingQuote = null)
    {
        if (original == null && changingRecentlyTraded == null) return;

        Assert.IsTrue(changingRecentlyTraded!.AreEquivalent(original, exactComparison));

        Assert.AreEqual(original!.Count, changingRecentlyTraded.Count);

        for (var i = 0; i < original.Count; i++)
        {
            var originalEntry = original[i];
            var changingEntry = changingRecentlyTraded[i];
            LastTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                exactComparison, originalEntry,
                changingEntry, original,
                changingRecentlyTraded, originalQuote, changingQuote);
            if (originalEntry is IMutableLastPaidGivenTrade)
                LastPaidGivenTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, (IMutableLastPaidGivenTrade)originalEntry!,
                    (IMutableLastPaidGivenTrade)changingEntry!, original,
                    changingRecentlyTraded, originalQuote, changingQuote);
            if (originalEntry is IMutableLastTraderPaidGivenTrade)
                LastTraderPaidGivenTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, (IMutableLastTraderPaidGivenTrade)originalEntry!,
                    (IMutableLastTraderPaidGivenTrade)changingEntry!, original,
                    changingRecentlyTraded, originalQuote, changingQuote);
        }
    }
}
