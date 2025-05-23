// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.LastTraded;

[TestClass]
public class LastExternalCounterPartyTradeTests
{
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

    private LastExternalCounterPartyTrade emptyLt               = null!;
    private IPQNameIdLookupGenerator      nameIdLookupGenerator = null!;
    private LastExternalCounterPartyTrade populatedLt           = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);

        emptyLt      = new LastExternalCounterPartyTrade();
        populatedLt = new LastExternalCounterPartyTrade
            (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedCounterPartyId
           , ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid, ExpectedWasGiven
           , ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt =
            new LastExternalCounterPartyTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedCounterPartyId
               , ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid, ExpectedWasGiven
               , ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        Assert.AreEqual(ExpectedTradePrice, newLt.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, newLt.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, newLt.TradeVolume);
        Assert.IsTrue(newLt.WasGiven);
        Assert.IsTrue(newLt.WasPaid);
        Assert.AreEqual(ExpectedTraderName, newLt.ExternalTraderName);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(default, emptyLt.TradeTime);
        Assert.AreEqual(0m, emptyLt.TradeVolume);
        Assert.IsFalse(emptyLt.WasGiven);
        Assert.IsFalse(emptyLt.WasPaid);
        Assert.IsNull(emptyLt.ExternalTraderName);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt = new LastExternalCounterPartyTrade
            (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedCounterPartyId
           , ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid, ExpectedWasGiven
           , ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        var fromPQInstance = new LastExternalCounterPartyTrade(newPopulatedLt);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);

        var nonPQLt = new PQLastExternalCounterPartyTrade
            (nameIdLookupGenerator, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
           , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
           , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
           , ExpectedUpdateTime)
            {
                ExternalTraderName = ExpectedTraderName
            };
        var fromNonPqInstance = new LastExternalCounterPartyTrade(nonPQLt);
        Assert.AreEqual(ExpectedTradePrice, fromNonPqInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromNonPqInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromNonPqInstance.TradeVolume);
        Assert.IsTrue(fromNonPqInstance.WasGiven);
        Assert.IsTrue(fromNonPqInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromNonPqInstance.ExternalTraderName);

        var newEmptyLt = new LastExternalCounterPartyTrade(emptyLt);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(default, newEmptyLt.TradeTime);
        Assert.AreEqual(0m, newEmptyLt.TradeVolume);
        Assert.IsFalse(newEmptyLt.WasGiven);
        Assert.IsFalse(newEmptyLt.WasPaid);
        Assert.IsNull(newEmptyLt.ExternalTraderName);
    }

    [TestMethod]
    public void EmptyEntry_Mutate_UpdatesFields()
    {
        const decimal expectedPrice      = 3.45678m;
        const decimal expectedVolume     = 2345.345m;
        const string  expectedTraderName = "Toly";

        var expectedTradeTime = new DateTime(2018, 3, 4, 11, 34, 5);

        emptyLt.TradePrice         = expectedPrice;
        emptyLt.TradeTime          = expectedTradeTime;
        emptyLt.TradeVolume        = expectedVolume;
        emptyLt.WasGiven           = true;
        emptyLt.WasPaid            = true;
        emptyLt.ExternalTraderName = expectedTraderName;

        Assert.AreEqual(expectedPrice, emptyLt.TradePrice);
        Assert.AreEqual(expectedTradeTime, emptyLt.TradeTime);
        Assert.AreEqual(expectedVolume, emptyLt.TradeVolume);
        Assert.IsTrue(emptyLt.WasPaid);
        Assert.IsTrue(emptyLt.WasGiven);
        Assert.AreEqual(expectedTraderName, emptyLt.ExternalTraderName);
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
        var nonPQLt = new LastExternalCounterPartyTrade(populatedLt);
        emptyLt.CopyFrom(nonPQLt);
        Assert.AreEqual(populatedLt, emptyLt);
    }

    [TestMethod]
    public void PQPopulatedLt_CopyFromToEmptyPvl_QuotesEquivalentToEachOther()
    {
        var pqLastTrade = new PQLastExternalCounterPartyTrade(populatedLt, nameIdLookupGenerator);
        var newEmpty    = new LastExternalCounterPartyTrade();
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
        clone = ((ILastExternalCounterPartyTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IMutableLastExternalCounterPartyTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
    }

    [TestMethod]
    public void FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (LastExternalCounterPartyTrade)((ICloneable)populatedLt).Clone();
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.OrderId)}: {populatedLt.OrderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeVolume)}: {populatedLt.TradeVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasGiven)}: {populatedLt.WasGiven}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasPaid)}: {populatedLt.WasPaid}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.ExternalCounterPartyId)}: {populatedLt.ExternalCounterPartyId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.ExternalCounterPartyName)}: {populatedLt.ExternalCounterPartyName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.ExternalTraderId)}: {populatedLt.ExternalTraderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.ExternalTraderName)}: {populatedLt.ExternalTraderName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (
        bool exactComparison,
        IMutableLastExternalCounterPartyTrade original,
        IMutableLastExternalCounterPartyTrade changingExtCpTrade,
        IMutableLastTradedList? originalLastTradedList = null,
        IMutableLastTradedList? changingLastTradedList = null,
        IMutablePublishableLevel3Quote? originalQuote = null,
        IMutablePublishableLevel3Quote? changingQuote = null)
    {
        LastPaidGivenTradeTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingExtCpTrade, originalLastTradedList, changingLastTradedList, originalQuote
           , changingQuote);

        if (original.GetType() == typeof(LastExternalCounterPartyTrade))
            Assert.IsTrue
                (original.AreEquivalent(new LastExternalCounterPartyTrade(changingExtCpTrade), exactComparison));

        changingExtCpTrade.ExternalCounterPartyId = 1_992_102;
        Assert.IsFalse(original.AreEquivalent(changingExtCpTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingExtCpTrade.ExternalCounterPartyId = original.ExternalCounterPartyId;
        Assert.IsTrue(changingExtCpTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingExtCpTrade.ExternalCounterPartyName = "Changed CounterParty Name";
        Assert.IsFalse(original.AreEquivalent(changingExtCpTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingExtCpTrade.ExternalCounterPartyName = original.ExternalCounterPartyName;
        Assert.IsTrue(changingExtCpTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingExtCpTrade.ExternalTraderId = 992_102;
        Assert.IsFalse(original.AreEquivalent(changingExtCpTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingExtCpTrade.ExternalTraderId = original.ExternalTraderId;
        Assert.IsTrue(changingExtCpTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingExtCpTrade.ExternalTraderName = "Changed Trader Name";
        Assert.IsFalse(original.AreEquivalent(changingExtCpTrade, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsFalse
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingExtCpTrade.ExternalTraderName = original.ExternalTraderName;
        Assert.IsTrue(changingExtCpTrade.AreEquivalent(original, exactComparison));
        if (originalLastTradedList != null)
            Assert.IsTrue
                (originalLastTradedList.AreEquivalent(changingLastTradedList, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
