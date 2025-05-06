// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;

[TestClass]
public class PQRecentlyTradedTests
{
    private const int MaxNumberOfEntries = PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades;

    private IList<PQRecentlyTraded>            allFullyPopulatedRecentlyTraded = null!;
    private List<IReadOnlyList<IPQLastTrade>>  allPopulatedEntries             = null!;
    private IList<IPQLastPaidGivenTrade>       lastPaidGivenEntries            = null!;
    private IList<IPQLastTraderPaidGivenTrade> lastTraderPaidGivenEntries      = null!;

    private PQRecentlyTraded paidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;

    private IList<IPQLastTrade> simpleEntries = null!;

    private PQRecentlyTraded        simpleRecentlyTradedFullyPopulatedQuote                = null!;
    private PQNameIdLookupGenerator traderNameIdLookupGenerator                            = null!;
    private PQRecentlyTraded        traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;
    // test being less than max.

    [TestInitialize]
    public void SetUp()
    {
        traderNameIdLookupGenerator = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);

        simpleEntries              = new List<IPQLastTrade>(MaxNumberOfEntries);
        lastPaidGivenEntries       = new List<IPQLastPaidGivenTrade>(MaxNumberOfEntries);
        lastTraderPaidGivenEntries = new List<IPQLastTraderPaidGivenTrade>(MaxNumberOfEntries);

        allPopulatedEntries = new List<IReadOnlyList<IPQLastTrade>>
        {
            (IReadOnlyList<IPQLastTrade>)simpleEntries, (IReadOnlyList<IPQLastTrade>)lastPaidGivenEntries
          , (IReadOnlyList<IPQLastTrade>)lastTraderPaidGivenEntries
        };

        for (var i = 0; i < MaxNumberOfEntries; i++)
        {
            simpleEntries.Add(new PQLastTrade(1.234567m, new DateTime(2018, 1, 2, 22, 52, 59)));
            lastPaidGivenEntries.Add(new PQLastPaidGivenTrade(1.234567m, new DateTime(2018, 1, 2, 22, 52, 59),
                                                              40_111_222m, true, true));
            lastTraderPaidGivenEntries.Add(new PQLastTraderPaidGivenTrade(traderNameIdLookupGenerator, 1.234567m,
                                                                          new DateTime(2018, 1, 2, 22, 52, 59), 40_111_222m, true, true)
            {
                TraderName = "TestTraderName"
            });
        }

        simpleRecentlyTradedFullyPopulatedQuote                = new PQRecentlyTraded(simpleEntries!);
        paidGivenVolumeRecentlyTradedFullyPopulatedQuote       = new PQRecentlyTraded(lastPaidGivenEntries);
        traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote = new PQRecentlyTraded(lastTraderPaidGivenEntries);

        allFullyPopulatedRecentlyTraded = new List<PQRecentlyTraded>
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
            var populatedEntries        = allPopulatedEntries[i];
            for (var j = 0; j < MaxNumberOfEntries; j++)
            {
                Assert.AreEqual(MaxNumberOfEntries, populatedRecentlyTraded.Count);
                Assert.AreNotSame(populatedEntries[j], populatedRecentlyTraded[j]);
            }
        }
    }

    [TestMethod]
    public void NewRecentlyTraded_InitializedFromRecentlyTraded_ClonesAllEntries()
    {
        for (var i = 0; i < allFullyPopulatedRecentlyTraded.Count; i++)
        {
            IRecentlyTraded populatedOrderBook = allFullyPopulatedRecentlyTraded[i];
            var             clonedOrderBook    = new PQRecentlyTraded(populatedOrderBook);
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
                var layer       = ((IRecentlyTraded)populatedOrderBook)[i];
                var clonedLayer = (IPQLastTrade)layer!.Clone();
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

                if (lt is IPQLastTraderPaidGivenTrade traderPaidGivenTrader)
                {
                    traderPaidGivenTrader.TraderName = "TestChangedTraderName";
                    Assert.IsTrue(populatedRecentlyTraded.HasUpdates);
                    Assert.IsTrue(traderPaidGivenTrader.HasUpdates);
                    traderPaidGivenTrader.IsTraderNameUpdated     = false;
                    traderPaidGivenTrader.NameIdLookup.HasUpdates = false;
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
            populatedRecentlyTraded[MaxNumberOfEntries] = null;
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
        foreach (var populatedL3Quote in allFullyPopulatedRecentlyTraded)
        {
            populatedL3Quote.HasUpdates = false;
            var pqFieldUpdates =
                populatedL3Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            var pqStringUpdates =
                populatedL3Quote.GetStringUpdates
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
        foreach (var populatedOrderBook in allFullyPopulatedRecentlyTraded)
        foreach (var subType in allFullyPopulatedRecentlyTraded.Where(ob => !ReferenceEquals(ob, populatedOrderBook)))
        {
            if (!WholeyContainedBy(subType[0]!.GetType(), populatedOrderBook[0]!.GetType())) continue;
            var newEmpty = new PQRecentlyTraded((IRecentlyTraded)populatedOrderBook);
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
        var notEmpty = new PQRecentlyTraded((IRecentlyTraded)simpleRecentlyTradedFullyPopulatedQuote);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfEntries - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGapAsEmpty()
    {
        var clonePopulated = simpleRecentlyTradedFullyPopulatedQuote.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);

        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;

        clonePopulated[5] = null;
        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new PQRecentlyTraded((IRecentlyTraded)simpleRecentlyTradedFullyPopulatedQuote);
        Assert.AreEqual(MaxNumberOfEntries, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(new PQLastTrade(), notEmpty[5]);
        Assert.AreEqual(MaxNumberOfEntries - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleRecentlyTradedFullyPopulatedQuote.Clone();
        Assert.AreEqual(MaxNumberOfEntries, clonePopulated.Count);

        clonePopulated[MaxNumberOfEntries - 1] = null;
        clonePopulated[MaxNumberOfEntries - 2] = null;

        Assert.AreEqual(MaxNumberOfEntries - 2, clonePopulated.Count);
        var notEmpty = new PQRecentlyTraded((IRecentlyTraded)simpleRecentlyTradedFullyPopulatedQuote)
        {
            [5] = null
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
                Assert.AreEqual(DateTimeConstants.UnixEpoch, pvl.TradeTime);
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

                if (pvl is IPQLastTraderPaidGivenTrade traderTrade)
                {
                    Assert.IsNull(traderTrade.TraderName);
                    Assert.IsFalse(traderTrade.IsTraderNameUpdated);
                }
            }
        }
    }

    [TestMethod]
    public void NonPQRecentlyTraded_CopyFromToEmptyRecentlyTraded_RecentlyTradedAreEqual()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var nonPQOrderBook = new RecentlyTraded(populatedRecentlyTraded);
            var newEmpty       = CreateNewEmpty(populatedRecentlyTraded);
            newEmpty.CopyFrom(nonPQOrderBook);
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
            var emptyOriginalTypeOrderBook = CreateNewEmpty(originalRecentlyTraded);
            AssertAllLayersAreOfTypeAndEquivalentTo(emptyOriginalTypeOrderBook, originalRecentlyTraded,
                                                    originalRecentlyTraded[0]!.GetType(), false);
            emptyOriginalTypeOrderBook.CopyFrom(otherRecentlyTraded);
            AssertAllLayersAreOfTypeAndEquivalentTo(emptyOriginalTypeOrderBook, otherRecentlyTraded,
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
            var clonedPopulatedOrderBook = (PQRecentlyTraded)originalRecentlyTraded.Clone();
            AssertAllLayersAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, originalRecentlyTraded,
                                                    originalRecentlyTraded[0]!.GetType(), false);
            clonedPopulatedOrderBook.CopyFrom(otherRecentlyTraded);
            AssertAllLayersAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, otherRecentlyTraded,
                                                    GetExpectedType(originalRecentlyTraded[0]!.GetType(),
                                                                    otherRecentlyTraded[0]!.GetType()));
            AssertAllLayersAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, originalRecentlyTraded,
                                                    GetExpectedType(originalRecentlyTraded[0]!.GetType(),
                                                                    otherRecentlyTraded[0]!.GetType()));
        }
    }

    [TestMethod]
    public void FullyPopulateRecentlyTraded_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedRecentlyTraded in allFullyPopulatedRecentlyTraded)
        {
            var clonedOrderBook = ((ICloneable<IRecentlyTraded>)populatedRecentlyTraded).Clone();
            Assert.AreNotSame(clonedOrderBook, populatedRecentlyTraded);
            Assert.AreEqual(populatedRecentlyTraded, clonedOrderBook);

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
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedQuote in allFullyPopulatedRecentlyTraded)
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
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote;
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
        if (originalType == typeof(PQLastPaidGivenTrade) && copyType == typeof(PQLastPaidGivenTrade)) return typeof(PQLastPaidGivenTrade);
        return typeof(PQLastTraderPaidGivenTrade);
    }

    private void AssertAllLayersAreOfTypeAndEquivalentTo
    (PQRecentlyTraded upgradedRecentlyTraded,
        PQRecentlyTraded equivalentTo, Type expectedType, bool compareForEquivalence = true,
        bool exactlyEquals = false)
    {
        for (var i = 0; i < upgradedRecentlyTraded.Capacity; i++)
        {
            var upgradedLayer = upgradedRecentlyTraded[i];
            var copyFromLayer = equivalentTo[i];

            Assert.IsInstanceOfType(upgradedLayer, expectedType);
            if (compareForEquivalence) Assert.IsTrue(copyFromLayer!.AreEquivalent(upgradedLayer, exactlyEquals));
        }
    }

    private bool WholeyContainedBy(Type copySourceType, Type copyDestinationType)
    {
        if (copySourceType == typeof(PQLastTrade)) return true;
        if (copySourceType == typeof(PQLastPaidGivenTrade))
            return copyDestinationType == typeof(PQLastTrade) ||
                   copyDestinationType == typeof(PQLastPaidGivenTrade);
        if (copySourceType == typeof(PQLastTraderPaidGivenTrade))
            return copyDestinationType == typeof(PQLastTrade) ||
                   copyDestinationType == typeof(PQLastPaidGivenTrade) ||
                   copyDestinationType == typeof(PQLastTraderPaidGivenTrade);
        return false;
    }

    private static PQRecentlyTraded CreateNewEmpty(PQRecentlyTraded populatedOrderBook)
    {
        var cloneGensis = populatedOrderBook[0]!.Clone();
        cloneGensis.StateReset();
        if (cloneGensis is IPQLastTraderPaidGivenTrade traderLastTrade)
            traderLastTrade.NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);
        var clonedEmptyEntries = new List<IPQLastTrade>(MaxNumberOfEntries);
        for (var i = 0; i < MaxNumberOfEntries; i++) clonedEmptyEntries.Add(cloneGensis.Clone());
        var newEmpty = new PQRecentlyTraded(clonedEmptyEntries!);
        return newEmpty;
    }

    public static void AssertContainsAllLevelRecentlyTradedFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        IPQRecentlyTraded recentlyTraded, PQBooleanValues expectedBooleanFlags = PQBooleanValuesExtensions.AllFields
      ,  PQFieldFlags priceScale = (PQFieldFlags)1,  PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        for (var i = 0; i < MaxNumberOfEntries; i++)
        {
            var lastTrade = recentlyTraded[i]!;
            var depthId   = (PQDepthKey)i;


            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, depthId, PQSubFieldKeys.LastTradedAtPrice, lastTrade.TradePrice, priceScale),
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedTickTrades, depthId, 
                                                                        PQSubFieldKeys.LastTradedAtPrice, priceScale),
                            $"For lastTradeType {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, depthId, PQSubFieldKeys.LastTradedTradeTimeDate,
                                              lastTrade.TradeTime.Get2MinIntervalsFromUnixEpoch()),
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedTickTrades, depthId,
                                                                        PQSubFieldKeys.LastTradedTradeTimeDate),
                            $"For bidlayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var extended = lastTrade.TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBase);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, depthId, PQSubFieldKeys.LastTradedTradeSub2MinTime, subHourBase, extended)
                          , PQTickInstantTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQQuoteFields.LastTradedTickTrades, depthId, PQSubFieldKeys.LastTradedTradeSub2MinTime, extended),
                            $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (lastTrade is IPQLastPaidGivenTrade pqPaidGivenTrade)
            {
                var lastTradedBoolFlags = pqPaidGivenTrade.WasGiven ? LastTradeBooleanFlags.WasGiven : LastTradeBooleanFlags.None;
                lastTradedBoolFlags |= pqPaidGivenTrade.WasPaid ? LastTradeBooleanFlags.WasPaid : LastTradeBooleanFlags.None;


                Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, depthId, PQSubFieldKeys.LastTradedBooleanFlags, (uint)lastTradedBoolFlags),
                                PQTickInstantTests.ExtractFieldUpdateWithId
                                    (checkFieldUpdates, PQQuoteFields.LastTradedTickTrades, depthId, PQSubFieldKeys.LastTradedBooleanFlags),
                                $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }

            if (lastTrade is IPQLastTraderPaidGivenTrade pqTraderPaidGivenTrade)
            {
                var lastTradedTraderId = (uint)pqTraderPaidGivenTrade.TraderId;
                Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, depthId, PQSubFieldKeys.LastTradedTraderId, lastTradedTraderId),
                                PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedTickTrades, depthId, 
                                                                            PQSubFieldKeys.LastTradedTraderId),
                                $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }
        }
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQRecentlyTraded original, PQRecentlyTraded changingRecentlyTraded, PQLevel3Quote? originalQuote = null,
        PQLevel3Quote? changingQuote = null)
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
            PQLastTraderPaidGivenTradeTests
                .AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalEntry as PQLastTraderPaidGivenTrade,
                     changingEntry as PQLastTraderPaidGivenTrade, original,
                     changingRecentlyTraded, originalQuote, changingQuote);
        }
    }
}
