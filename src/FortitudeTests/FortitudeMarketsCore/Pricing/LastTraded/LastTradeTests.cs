#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded;

[TestClass]
public class LastTradeTests
{
    private LastTrade emptyLt = null!;
    private LastTrade populatedLt = null!;
    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyLt = new LastTrade();
        testDateTime = new DateTime(2017, 12, 17, 16, 11, 52);
        populatedLt = new LastTrade(4.2949_672m, testDateTime);
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt = new LastTrade(20, testDateTime);
        Assert.AreEqual(20m, newLt.TradePrice);
        Assert.AreEqual(testDateTime, newLt.TradeTime);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyLt.TradeTime);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new LastTrade(20, testDateTime);
        var fromPQInstance = new LastTrade(newPopulatedLt);
        Assert.AreEqual(20m, fromPQInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPQInstance.TradeTime);

        var pqLt = new PQLastTrade(1.23456m, testDateTime);
        var fromPqInstance = new LastTrade(pqLt);
        Assert.AreEqual(1.23456m, fromPqInstance.TradePrice);
        Assert.AreEqual(testDateTime, fromPqInstance.TradeTime);

        var newEmptyLt = new LastTrade(emptyLt);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, newEmptyLt.TradeTime);
    }

    [TestMethod]
    public void EmptyEntry_Mutate_UpdatesFields()
    {
        const decimal expectedPrice = 3.45678m;
        var expectedTradeTime = new DateTime(2018, 3, 4, 11, 34, 5);

        emptyLt.TradePrice = expectedPrice;
        emptyLt.TradeTime = expectedTradeTime;

        Assert.AreEqual(expectedPrice, emptyLt.TradePrice);
        Assert.AreEqual(expectedTradeTime, emptyLt.TradeTime);
    }

    [TestMethod]
    public void EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.IsTrue(emptyLt.IsEmpty);
    }

    [TestMethod]
    public void PopulatedLt_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.AreNotEqual(0m, populatedLt.TradePrice);
        Assert.AreNotEqual(DateTimeConstants.UnixEpoch, populatedLt.TradeTime);
        populatedLt.StateReset();
        Assert.IsTrue(populatedLt.IsEmpty);
        Assert.AreEqual(0m, populatedLt.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, populatedLt.TradeTime);
    }

    [TestMethod]
    public void FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther()
    {
        var lastTrade = new LastTrade(populatedLt);
        emptyLt.CopyFrom(lastTrade);
        Assert.AreEqual(populatedLt, emptyLt);
    }

    [TestMethod]
    public void PQPopulatedLt_CopyFromToEmptyPvl_QuotesEquivalentToEachOther()
    {
        var pqLastTrade = new PQLastTrade(populatedLt);
        var newEmpty = new LastTrade();
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
    }

    [TestMethod]
    public void FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (LastTrade)((ICloneable)populatedLt).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedLt,
            fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedLt,
            fullyPopulatedClone);
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
    }


    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IMutableLastTrade? original, IMutableLastTrade? changingLastTrade,
        IMutableRecentlyTraded? originalRecentlyTraded = null, IMutableRecentlyTraded? changingRecentlyTraded = null,
        IMutableLevel3Quote? originalQuote = null, IMutableLevel3Quote? changingQuote = null)
    {
        if (original == null || changingLastTrade == null) return;

        if (original.GetType() == typeof(LastTrade))
            Assert.IsTrue(original.AreEquivalent(new LastTrade(changingLastTrade), exactComparison));

        changingLastTrade.TradePrice = 12.34567m;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradePrice = original.TradePrice;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradeTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeTime = original.TradeTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
