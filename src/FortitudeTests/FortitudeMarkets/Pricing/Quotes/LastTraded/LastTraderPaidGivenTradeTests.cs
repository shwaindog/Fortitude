// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LastTraded;

[TestClass]
public class LastTraderPaidGivenTradeTests
{
    private const string WellKnownTraderName = "TestTraderName";

    private LastTraderPaidGivenTrade emptyLt               = null!;
    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;
    private LastTraderPaidGivenTrade populatedLt           = null!;
    private DateTime                 testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);

        emptyLt      = new LastTraderPaidGivenTrade();
        testDateTime = new DateTime(2017, 12, 17, 16, 11, 52);
        populatedLt = new LastTraderPaidGivenTrade
            (4.2949_672m, testDateTime, 42_949_672.95m, true, true, WellKnownTraderName);
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt = new LastTraderPaidGivenTrade
            (20, testDateTime, 42_949_672.95m, true, true, WellKnownTraderName);
        Assert.AreEqual(20m, newLt.TradePrice);
        Assert.AreEqual(testDateTime, newLt.TradeTime);
        Assert.AreEqual(42_949_672.95m, newLt.TradeVolume);
        Assert.IsTrue(newLt.WasGiven);
        Assert.IsTrue(newLt.WasPaid);
        Assert.AreEqual(WellKnownTraderName, newLt.TraderName);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(default, emptyLt.TradeTime);
        Assert.AreEqual(0m, emptyLt.TradeVolume);
        Assert.IsFalse(emptyLt.WasGiven);
        Assert.IsFalse(emptyLt.WasPaid);
        Assert.IsNull(emptyLt.TraderName);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new LastTraderPaidGivenTrade
            (20, testDateTime, 42_949_672.95m, true, true, WellKnownTraderName);
        var fromPQInstance = new LastTraderPaidGivenTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromPQInstance.TraderName);

        var nonPQLt = new PQLastTraderPaidGivenTrade(nameIdLookupGenerator, 1.23456m, testDateTime, 42_949_672.95m, true, true)
        {
            TraderName = WellKnownTraderName
        };
        var fromNonPqInstance = new LastTraderPaidGivenTrade(nonPQLt);
        Assert.AreEqual(1.23456m, fromNonPqInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromNonPqInstance.TradeTime);
        Assert.AreEqual(42_949_672.95m, fromNonPqInstance.TradeVolume);
        Assert.IsTrue(fromNonPqInstance.WasGiven);
        Assert.IsTrue(fromNonPqInstance.WasPaid);
        Assert.AreEqual(WellKnownTraderName, fromNonPqInstance.TraderName);

        var newEmptyLt = new LastTraderPaidGivenTrade(emptyLt);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(default, newEmptyLt.TradeTime);
        Assert.AreEqual(0m, newEmptyLt.TradeVolume);
        Assert.IsFalse(newEmptyLt.WasGiven);
        Assert.IsFalse(newEmptyLt.WasPaid);
        Assert.IsNull(newEmptyLt.TraderName);
    }

    [TestMethod]
    public void EmptyEntry_Mutate_UpdatesFields()
    {
        const decimal expectedPrice      = 3.45678m;
        const decimal expectedVolume     = 2345.345m;
        const string  expectedTraderName = "Toly";

        var expectedTradeTime = new DateTime(2018, 3, 4, 11, 34, 5);

        emptyLt.TradePrice  = expectedPrice;
        emptyLt.TradeTime   = expectedTradeTime;
        emptyLt.TradeVolume = expectedVolume;
        emptyLt.WasGiven    = true;
        emptyLt.WasPaid     = true;
        emptyLt.TraderName  = expectedTraderName;

        Assert.AreEqual(expectedPrice, emptyLt.TradePrice);
        Assert.AreEqual(expectedTradeTime, emptyLt.TradeTime);
        Assert.AreEqual(expectedVolume, emptyLt.TradeVolume);
        Assert.IsTrue(emptyLt.WasPaid);
        Assert.IsTrue(emptyLt.WasGiven);
        Assert.AreEqual(expectedTraderName, emptyLt.TraderName);
    }

    [TestMethod]
    public void PopulatedLt_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.AreNotEqual(0m, populatedLt.TradePrice);
        Assert.AreNotEqual(DateTimeConstants.UnixEpoch, populatedLt.TradeTime);
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
        var nonPQLt = new LastTraderPaidGivenTrade(populatedLt);
        emptyLt.CopyFrom(nonPQLt);
        Assert.AreEqual(populatedLt, emptyLt);
    }

    [TestMethod]
    public void PQPopulatedLt_CopyFromToEmptyPvl_QuotesEquivalentToEachOther()
    {
        var pqLastTrade = new PQLastTraderPaidGivenTrade(populatedLt, nameIdLookupGenerator);
        var newEmpty    = new LastTraderPaidGivenTrade();
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
        clone = ((ILastTraderPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IMutableLastTraderPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
    }

    [TestMethod]
    public void FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (LastTraderPaidGivenTrade)((ICloneable)populatedLt).Clone();
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradePrice)}: {populatedLt.TradePrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeTime)}: {populatedLt.TradeTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeVolume)}: {populatedLt.TradeVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasGiven)}: {populatedLt.WasGiven}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasPaid)}: {populatedLt.WasPaid}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TraderName)}: {populatedLt.TraderName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (
        bool exactComparison,
        IMutableLastTraderPaidGivenTrade original,
        IMutableLastTraderPaidGivenTrade changingLastTraderPaidGivenTrade,
        IMutableRecentlyTraded? originalRecentlyTraded = null,
        IMutableRecentlyTraded? changingRecentlyTraded = null,
        IMutableLevel3Quote? originalQuote = null,
        IMutableLevel3Quote? changingQuote = null)
    {
        LastPaidGivenTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingLastTraderPaidGivenTrade, originalRecentlyTraded, changingRecentlyTraded, originalQuote
           , changingQuote);

        if (original.GetType() == typeof(LastTraderPaidGivenTrade))
            Assert.IsTrue
                (original.AreEquivalent(new LastTraderPaidGivenTrade(changingLastTraderPaidGivenTrade), exactComparison));

        changingLastTraderPaidGivenTrade.TraderName = "Changed Trader Name";
        Assert.IsFalse(original.AreEquivalent(changingLastTraderPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse
                (originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTraderPaidGivenTrade.TraderName = original.TraderName;
        Assert.IsTrue(changingLastTraderPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue
                (originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
