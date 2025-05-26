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
public class LastTradeTests
{
    private const uint    ExpectedTradeId    = 42;
    private const uint    ExpectedBatchId    = 24_942;
    private const decimal ExpectedTradePrice = 2.3456m;

    private const LastTradedTypeFlags      ExpectedTradedTypeFlags     = LastTradedTypeFlags.HasPaidGivenDetails;
    private const LastTradedLifeCycleFlags ExpectedTradeLifeCycleFlags = LastTradedLifeCycleFlags.Confirmed;

    private static readonly DateTime ExpectedTradeTime           = new(2018, 03, 2, 14, 40, 30);
    private static readonly DateTime ExpectedFirstNotifiedTime   = new(2018, 03, 2, 14, 40, 31);
    private static readonly DateTime ExpectedAdapterReceivedTime = new(2018, 03, 2, 14, 40, 41);
    private static readonly DateTime ExpectedUpdateTime          = new(2018, 03, 2, 14, 40, 42);

    private LastTrade emptyLt     = null!;
    private LastTrade populatedLt = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyLt      = new LastTrade();
        populatedLt = new LastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                  , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt = new LastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        Assert.AreEqual(ExpectedTradePrice, newLt.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, newLt.TradeTime);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(default, emptyLt.TradeTime);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new LastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                         , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        var fromPQInstance = new LastTrade(newPopulatedLt);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);

        var pqLt = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                 , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        var fromPqInstance = new LastTrade(pqLt);
        Assert.AreEqual(ExpectedTradePrice, fromPqInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPqInstance.TradeTime);

        var newEmptyLt = new LastTrade(emptyLt);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(default, newEmptyLt.TradeTime);
    }

    [TestMethod]
    public void EmptyEntry_Mutate_UpdatesFields()
    {
        const decimal expectedPrice = 3.45678m;

        var expectedTradeTime = new DateTime(2018, 3, 4, 11, 34, 5);

        emptyLt.TradePrice = expectedPrice;
        emptyLt.TradeTime  = expectedTradeTime;

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
        Assert.AreNotEqual(default, populatedLt.TradeTime);
        populatedLt.StateReset();
        Assert.IsTrue(populatedLt.IsEmpty);
        Assert.AreEqual(0m, populatedLt.TradePrice);
        Assert.AreEqual(default, populatedLt.TradeTime);
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
        var newEmpty    = new LastTrade();
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeId)}: {populatedLt.TradeId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.BatchId)}: {populatedLt.BatchId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradePrice)}: {populatedLt.TradePrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeTime)}: {populatedLt.TradeTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeTypeFlags)}: {populatedLt.TradeTypeFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeLifeCycleStatus)}: {populatedLt.TradeLifeCycleStatus}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.FirstNotifiedTime)}: {populatedLt.FirstNotifiedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.AdapterReceivedTime)}: {populatedLt.AdapterReceivedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.UpdateTime)}: {populatedLt.UpdateTime:O}"));
    }


    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableLastTrade? original, IMutableLastTrade? changingLastTrade,
        IMutableLastTradedList? originalLastTradedList = null, IMutableLastTradedList? changingLastTradedList = null,
        IMutablePublishableLevel3Quote? originalQuote = null, IMutablePublishableLevel3Quote? changingQuote = null)
    {
        if (original == null || changingLastTrade == null) return;

        if (original.GetType() == typeof(LastTrade)) Assert.IsTrue(original.AreEquivalent(new LastTrade(changingLastTrade), exactComparison));

        changingLastTrade.TradeId = 77_889;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeId = original.TradeId;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.BatchId = 277_889;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.BatchId = original.BatchId;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradePrice = 12.34567m;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradePrice = original.TradePrice;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradeTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeTime = original.TradeTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradeTypeFlags = LastTradedTypeFlags.HasPaidGivenDetails | LastTradedTypeFlags.IsInternalOrderTradeUpdate;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeTypeFlags = original.TradeTypeFlags;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.TradeLifeCycleStatus = LastTradedLifeCycleFlags.Confirmed | LastTradedLifeCycleFlags.Rejected;
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.TradeLifeCycleStatus = original.TradeLifeCycleStatus;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.FirstNotifiedTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.FirstNotifiedTime = original.FirstNotifiedTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.AdapterReceivedTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.AdapterReceivedTime = original.AdapterReceivedTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTrade.UpdateTime = new DateTime(2018, 1, 02, 20, 22, 50);
        Assert.IsFalse(original.AreEquivalent(changingLastTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse(
                           originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTrade.UpdateTime = original.UpdateTime;
        Assert.IsTrue(changingLastTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue(
                          originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
