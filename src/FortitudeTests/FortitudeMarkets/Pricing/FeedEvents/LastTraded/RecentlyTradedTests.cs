// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
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


    private IList<RecentlyTraded> allFullyPopulatedRecentlyTraded = null!;

    private List<IReadOnlyList<ILastTrade>>   allPopulatedEntries  = null!;
    private IList<IMutableLastPaidGivenTrade> lastPaidGivenEntries = null!;

    private IList<IMutableLastExternalCounterPartyTrade> lastTraderPaidGivenEntries = null!;

    private RecentlyTraded paidGivenVolumeFullyPopulatedRecentlyTraded = null!;

    private IList<IMutableLastTrade> simpleEntries = null!;

    private RecentlyTraded simpleFullyPopulatedRecentlyTraded = null!;

    private RecentlyTraded fullSupportRecentlyTradedFullyPopulated = null!;
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

        for (uint i = 0; i < MaxNumberOfEntries; i++)
        {
            simpleEntries.Add(new LastTrade(ExpectedTradeId + i, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                          , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime));
            lastPaidGivenEntries.Add
                (new LastPaidGivenTrade
                    (ExpectedTradeId + i, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId, ExpectedWasPaid
                   , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
                   , ExpectedUpdateTime));
            lastTraderPaidGivenEntries.Add
                (new LastExternalCounterPartyTrade
                    (ExpectedTradeId + i, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedCounterPartyId
                   , ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid, ExpectedWasGiven
                   , ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime));
        }

        simpleFullyPopulatedRecentlyTraded          = new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, simpleEntries);
        paidGivenVolumeFullyPopulatedRecentlyTraded = new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, lastPaidGivenEntries);

        fullSupportRecentlyTradedFullyPopulated = new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags, lastTraderPaidGivenEntries);

        allFullyPopulatedRecentlyTraded = new List<RecentlyTraded>
        {
            simpleFullyPopulatedRecentlyTraded, paidGivenVolumeFullyPopulatedRecentlyTraded
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
            var rt        = populatedRecentlyTraded;
            var toString = rt.ToString();

            Assert.IsTrue(toString.Contains(rt.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(rt.MaxAllowedSize)}: {rt.MaxAllowedSize:N0}"));
            Assert.IsTrue(toString.Contains($"LastTrades: [{rt.EachLastTradeByIndexOnNewLines()}]"));
            Assert.IsTrue(toString.Contains($"{nameof(rt.Count)}: {rt.Count}"));
        }
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = fullSupportRecentlyTradedFullyPopulated;
        Assert.AreEqual(MaxNumberOfEntries, rt.Count);
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable<ILastTrade>)rt).Count());
        Assert.AreEqual(MaxNumberOfEntries, ((IEnumerable)rt).OfType<ILastTrade>().Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable<IMutableLastTrade>)rt).Count());
        Assert.AreEqual(0, rt.OfType<ILastTrade>().Count());
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_SmallerToLargerCalculateShifts_ShiftRightCommandsExpected()
    {
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
    }

    [TestMethod]
    public void PopulatedRecentlyTraded_LargerToSmallerCalculateShiftsWithNewEntryInMiddle_CalculateShiftLeftCommandsReturnsExpected()
    {
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

        var newLastTrade = new LastExternalCounterPartyTrade
            (ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_InsertNewElementAtStart_RemainingElementsShiftRightByOne()
    {
        var newLastTrade = new LastExternalCounterPartyTrade
            (ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries + 1, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxedSizeRecentlyTraded_DeleteMiddleElement_RemainingElementsShiftLeftByOne()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;

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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_InsertNewElementAtStart_RemainingElementsShiftRightExceptLastIsRemoved()
    {
        var newLastTrade = new LastExternalCounterPartyTrade
            (ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

        fullSupportRecentlyTradedFullyPopulated.MaxAllowedSize = MaxNumberOfEntries;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_InsertNewElementAtEnd_NewElementAppearsAtTheEnd()
    {
        var newLastTrade = new LastExternalCounterPartyTrade
            (ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries + 1, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachRecentlyTraded_AttemptInsertNewElementAtEnd_ReturnsFalseAndNoElementIsAdded()
    {
        var newLastTrade = new LastExternalCounterPartyTrade
            (ExpectedTradeId + 13, ExpectedBatchId + 13, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime);

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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleToStart_FormerMiddleElementIsAtStart()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_MoveMiddleToEnd_FormerMiddleElementIsAtTheEnd()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleToEnd_FormerMiddleElementIsAtTheEnd()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_MoveMiddleRightByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleRightByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_MoveMiddleLeftByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleLeftByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftLeftFromMiddleByOne_DeletesEntryFirstEntryCreatesEmptyOneBelowPinIndex()
    {
        var pinAt = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftLeftFromMiddleByOne_DeletesEntryFirstEntryCreatesEmptyOneBelowPinIndex()
    {
        var pinAt = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftRightFromMiddleByOne_CreatesEmptyOneAbovePinIndexAndExtendsList()
    {
        var pinAt = MaxNumberOfEntries / 2 - 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftRightFromMiddleByOne_CreatesEmptyOneAbovePinIndexAndDeletesLastEntry()
    {
        var pinAt = MaxNumberOfEntries / 2 - 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftLeftTowardMiddleByOne_DeletesPreMiddleEntryCreatesEmptyAtEnd()
    {
        var pinAt = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries - 1, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedCacheMaxAllowedSizeReachedRecentlyTraded_ShiftLeftTowardMiddleByOne_DeletesPreMiddleEntryCreatesEmptyAtEnd()
    {
        var pinAt = MaxNumberOfEntries / 2 + 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count + 1);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftRightTowardMiddleByOne_CreatesEmptyAtStartDeletesPreMiddleEntry()
    {
        var pinAt = MaxNumberOfEntries / 2 - 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftRightTowardMiddleByOne_CreatesEmptyAtStartDeletesPreMiddleEntry()
    {
        var pinAt = MaxNumberOfEntries / 2 - 1;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = MaxNumberOfEntries / 2;
        var toShift = fullSupportRecentlyTradedFullyPopulated.Clone();
        Assert.AreEqual(fullSupportRecentlyTradedFullyPopulated, toShift);

        toShift.ShiftElementsFrom(-halfListSize, short.MaxValue);

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], fullSupportRecentlyTradedFullyPopulated[i + halfListSize]);
        }
        Assert.AreEqual(halfListSize, toShift.Count);

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = MaxNumberOfEntries / 2;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeRecentlyTraded_ShiftRightFromStart_CreatesEmptyAtStartAndExtendsListByHalf()
    {
        var halfListSize = MaxNumberOfEntries / 2;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries + halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_ShiftRightFromStartByHalfList_CreatesEmptyItemsAtStartAndTruncatesLastHalfOfOrders()
    {
        var halfListSize = MaxNumberOfEntries / 2;
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

        var shiftCopyFrom = fullSupportRecentlyTradedFullyPopulated.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(MaxNumberOfEntries, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
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
