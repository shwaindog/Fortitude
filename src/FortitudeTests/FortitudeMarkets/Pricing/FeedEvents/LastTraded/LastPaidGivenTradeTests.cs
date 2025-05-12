// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.LastTraded;

[TestClass]
public class LastPaidGivenTradeTests
{
    private LastPaidGivenTrade emptyLt     = null!;
    private LastPaidGivenTrade populatedLt = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyLt      = new LastPaidGivenTrade();
        testDateTime = new DateTime(2017, 12, 17, 16, 11, 52);
        populatedLt  = new LastPaidGivenTrade(4.2949_672m, testDateTime, 42_949_672.95m, true, true);
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt = new LastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true);
        Assert.AreEqual(20m, newLt.TradePrice);
        Assert.AreEqual(testDateTime, newLt.TradeTime);
        Assert.AreEqual(42_949_672.95m, newLt.TradeVolume);
        Assert.IsTrue(newLt.WasGiven);
        Assert.IsTrue(newLt.WasPaid);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(default, emptyLt.TradeTime);
        Assert.AreEqual(0m, emptyLt.TradeVolume);
        Assert.IsFalse(emptyLt.WasGiven);
        Assert.IsFalse(emptyLt.WasPaid);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new LastPaidGivenTrade(20, testDateTime, 42_949_672.95m, true, true);
        var fromPQInstance = new LastPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);

        var pqLt           = new PQLastPaidGivenTrade(1.23456m, testDateTime, 42_949_672.95m, true, true);
        var fromPqInstance = new LastPaidGivenTrade(pqLt);
        Assert.AreEqual(1.23456m, fromPqInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPqInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPqInstance.TradeVolume);
        Assert.IsTrue(fromPqInstance.WasGiven);
        Assert.IsTrue(fromPqInstance.WasPaid);

        var newEmptyLt = new LastPaidGivenTrade(emptyLt);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(default, newEmptyLt.TradeTime);
        Assert.AreEqual(0m, newEmptyLt.TradeVolume);
        Assert.IsFalse(newEmptyLt.WasGiven);
        Assert.IsFalse(newEmptyLt.WasPaid);
    }

    [TestMethod]
    public void EmptyEntry_Mutate_UpdatesFields()
    {
        const decimal expectedPrice  = 3.45678m;
        const decimal expectedVolume = 2345.345m;

        var expectedTradeTime = new DateTime(2018, 3, 4, 11, 34, 5);

        emptyLt.TradePrice  = expectedPrice;
        emptyLt.TradeTime   = expectedTradeTime;
        emptyLt.TradeVolume = expectedVolume;
        emptyLt.WasGiven    = true;
        emptyLt.WasPaid     = true;

        Assert.AreEqual(expectedPrice, emptyLt.TradePrice);
        Assert.AreEqual(expectedTradeTime, emptyLt.TradeTime);
        Assert.AreEqual(expectedVolume, emptyLt.TradeVolume);
        Assert.IsTrue(emptyLt.WasPaid);
        Assert.IsTrue(emptyLt.WasGiven);
    }

    [TestMethod]
    public void PopulatedLt_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.AreNotEqual(0m, populatedLt.TradePrice);
        Assert.AreNotEqual(DateTime.MinValue, populatedLt.TradeTime);
        Assert.AreNotEqual(0m, populatedLt.TradeVolume);
        Assert.IsTrue(populatedLt.WasGiven);
        Assert.IsTrue(populatedLt.WasPaid);
        populatedLt.StateReset();
        Assert.IsTrue(populatedLt.IsEmpty);
        Assert.AreEqual(0m, populatedLt.TradePrice);
        Assert.AreEqual(default, populatedLt.TradeTime);
        Assert.AreEqual(0m, populatedLt.TradeVolume);
        Assert.IsFalse(populatedLt.WasGiven);
        Assert.IsFalse(populatedLt.WasPaid);
    }

    [TestMethod]
    public void EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.IsTrue(emptyLt.IsEmpty);
    }

    [TestMethod]
    public void FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther()
    {
        var nonPQLt = new LastPaidGivenTrade(populatedLt);
        emptyLt.CopyFrom(nonPQLt);
        Assert.AreEqual(populatedLt, emptyLt);
    }

    [TestMethod]
    public void PQPopulatedLt_CopyFromToEmptyPvl_QuotesEquivalentToEachOther()
    {
        var pqLastTrade = new PQLastPaidGivenTrade(populatedLt);
        var newEmpty    = new LastPaidGivenTrade();
        newEmpty.CopyFrom(pqLastTrade);
        Assert.IsTrue(populatedLt.AreEquivalent(newEmpty));
        Assert.AreEqual(populatedLt, newEmpty);
    }

    [TestMethod]
    public void FromInterfacePopulatedLastTrade_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = populatedLt.Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((ICloneable<ILastTrade>)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((ILastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IMutableLastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
    }

    [TestMethod]
    public void FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (LastPaidGivenTrade)((ICloneable)populatedLt).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedLt, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedLt, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedLt, populatedLt);
        Assert.AreEqual(populatedLt, ((ICloneable)populatedLt).Clone());
        Assert.AreEqual(populatedLt, ((ILastPaidGivenTrade)populatedLt).Clone());
        Assert.AreEqual(populatedLt, ((IMutableLastPaidGivenTrade)populatedLt).Clone());
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeVolume)}: {populatedLt.TradeVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasGiven)}: {populatedLt.WasGiven}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasPaid)}: {populatedLt.WasPaid}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableLastPaidGivenTrade original, IMutableLastPaidGivenTrade changingLastPaidGivenTrade,
        IMutableRecentlyTraded? originalRecentlyTraded = null, IMutableRecentlyTraded? changingRecentlyTraded = null,
        IMutablePublishableLevel3Quote? originalQuote = null, IMutablePublishableLevel3Quote? changingQuote = null)
    {
        LastTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingLastPaidGivenTrade, originalRecentlyTraded, changingRecentlyTraded, originalQuote, changingQuote);

        if (original.GetType() == typeof(LastPaidGivenTrade))
            Assert.IsTrue
                (original.AreEquivalent(new LastPaidGivenTrade(changingLastPaidGivenTrade), exactComparison));

        changingLastPaidGivenTrade.TradeVolume = 1_234_567m;
        Assert.IsFalse(original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse
                (originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.TradeVolume = original.TradeVolume;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue
                (originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastPaidGivenTrade.WasGiven = !changingLastPaidGivenTrade.WasGiven;
        Assert.IsFalse(original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse
                (originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.WasGiven = original.WasGiven;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue
                (originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastPaidGivenTrade.WasPaid = !changingLastPaidGivenTrade.WasPaid;
        Assert.IsFalse(original.AreEquivalent(changingLastPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse
                (originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastPaidGivenTrade.WasPaid = original.WasPaid;
        Assert.IsTrue(changingLastPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue
                (originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
