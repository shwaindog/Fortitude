// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

[TestClass]
public class PQLastTradeEntrySelectorTests
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

    private const string ExpectedTraderName       = "TraderName-Anatoly";
    private const string ExpectedCounterPartyName = "CounterPartyName-Valcopp";

    private LastPaidGivenTrade   paidGivenLastTrade   = null!;
    private PQLastPaidGivenTrade pqPaidGivenLastTrade = null!;

    private PQLastTradeEntrySelector        entrySelector                   = null!;
    private PQLastTrade                     pqSimpleLastTrade               = null!;
    private IPQSourceTickerInfo             pqSourceTickerInfo              = null!;
    private PQLastExternalCounterPartyTrade pqExternalCounterPartyLastTrade = null!;

    private LastTrade simpleLastTrade = null!;

    private IPQNameIdLookupGenerator      traderNameIdGenerator         = null!;
    private LastExternalCounterPartyTrade externalCounterPartyLastTrade = null!;

    [TestInitialize]
    public void SetUp()
    {
        traderNameIdGenerator = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        entrySelector         = new PQLastTradeEntrySelector(traderNameIdGenerator);

        simpleLastTrade = new LastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                      , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        paidGivenLastTrade =
            new LastPaidGivenTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime);
        externalCounterPartyLastTrade =
            new LastExternalCounterPartyTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedCounterPartyId
               , ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid, ExpectedWasGiven
               , ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        pqSimpleLastTrade = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                          , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        pqPaidGivenLastTrade =
            new PQLastPaidGivenTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime);
        pqExternalCounterPartyLastTrade =
            new PQLastExternalCounterPartyTrade
                (traderNameIdGenerator, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime);
        pqSourceTickerInfo =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, MarketClassification.Unknown
                   , AUinMEL, AUinMEL, AUinMEL
                   , 20, 0.00001m, 30000m, 50000000m, 1000m, 1, 100
                   , 10_000, true, true, LayerFlags.Volume | LayerFlags.Price
                   , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime));
        pqSourceTickerInfo.NameIdLookup = traderNameIdGenerator;
    }

    [TestMethod]
    public void VariousLastTradeFlags_Select_ReturnsSimpleLastTradeEntryFactory()
    {
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.None;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
    }

    [TestMethod]
    public void VariousLastTradeFlags_Select_ReturnsSimpleLastPaidGivenTradeEntryFactory()
    {
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                             LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                             LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                             LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                             LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
    }

    [TestMethod]
    public void VariousLastTradeFlags_Select_ReturnsSimpleLastTraderPaidGivenTradeEntryFactory()
    {
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume |
                                             LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven |
                                             LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                             LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                             LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                             LastTradedFlags.LastTradedVolume | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                             LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
    }

    [TestMethod]
    public void NonPQLastTradeTypes_ConvertIfNonPQLastTrade_ConvertsToPQLastTradeType()
    {
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(simpleLastTrade, traderNameIdGenerator.Clone());
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreEqual(ExpectedTradePrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, pqLastTradeEntry.TradeTime);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(paidGivenLastTrade, traderNameIdGenerator.Clone());
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreEqual(ExpectedTradePrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, pqLastPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasGiven);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(externalCounterPartyLastTrade, traderNameIdGenerator.Clone());
        var pqLastTraderPaidGivenEntry = pqLastTradeEntry as PQLastExternalCounterPartyTrade;
        Assert.IsNotNull(pqLastTraderPaidGivenEntry);
        Assert.AreEqual(ExpectedTradePrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, pqLastTraderPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastTraderPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastTraderPaidGivenEntry.WasGiven);
        Assert.AreEqual(ExpectedTraderName, pqLastTraderPaidGivenEntry.ExternalTraderName);
    }

    [TestMethod]
    public void PQLastTradeTypes_ConvertIfNonPQLastTrade_ClonesPQLastTradeType()
    {
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqSimpleLastTrade, traderNameIdGenerator.Clone(), true);
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreNotSame(pqLastTradeEntry, pqSimpleLastTrade);
        Assert.AreEqual(ExpectedTradePrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, pqLastTradeEntry.TradeTime);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), true);
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreNotSame(pqLastTradeEntry, pqPaidGivenLastTrade);
        Assert.AreEqual(ExpectedTradePrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, pqLastPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasGiven);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqExternalCounterPartyLastTrade, traderNameIdGenerator.Clone(), true);
        var pqLastTraderPaidGivenEntry = pqLastTradeEntry as PQLastExternalCounterPartyTrade;
        Assert.IsNotNull(pqLastTraderPaidGivenEntry);
        Assert.AreNotSame(pqLastTradeEntry, pqExternalCounterPartyLastTrade);
        Assert.AreEqual(ExpectedTradePrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, pqLastTraderPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastTraderPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastTraderPaidGivenEntry.WasGiven);
        Assert.AreEqual(ExpectedTraderName, pqLastTraderPaidGivenEntry.ExternalTraderName);
    }

    [TestMethod]
    public void PQLastTradeTypes_ConvertIfNonPQLastTrade_ReturnsSamePQLastTradeType()
    {
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqSimpleLastTrade, traderNameIdGenerator.Clone());
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreSame(pqLastTradeEntry, pqSimpleLastTrade);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqPaidGivenLastTrade, traderNameIdGenerator.Clone());
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreSame(pqLastTradeEntry, pqPaidGivenLastTrade);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqExternalCounterPartyLastTrade, traderNameIdGenerator, true);
        var pqLastTraderPaidGivenEntry = pqLastTradeEntry as PQLastExternalCounterPartyTrade;
        Assert.IsNotNull(pqLastTraderPaidGivenEntry);
        Assert.AreNotSame(pqLastTradeEntry, pqExternalCounterPartyLastTrade);
    }

    [TestMethod]
    public void NonPQLastTradeTypes_TypeCanWhollyContain_ReturnsAsExpected()
    {
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(typeof(LastTrade), typeof(PQLastTrade)));
        Assert.IsFalse(entrySelector.TypeCanWhollyContain(typeof(LastPaidGivenTrade), typeof(PQLastTrade)));
        Assert.IsFalse(entrySelector.TypeCanWhollyContain(typeof(LastExternalCounterPartyTrade), typeof(PQLastTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(typeof(LastTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(
                                                         typeof(LastPaidGivenTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsFalse(entrySelector.TypeCanWhollyContain(
                                                          typeof(LastExternalCounterPartyTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(typeof(LastTrade), typeof(PQLastExternalCounterPartyTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(
                                                         typeof(LastPaidGivenTrade), typeof(PQLastExternalCounterPartyTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(
                                                         typeof(LastExternalCounterPartyTrade), typeof(PQLastExternalCounterPartyTrade)));
    }

    [TestMethod]
    public void PQLastTradeTypes_TypeCanWhollyContain_ReturnsAsExpected()
    {
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(typeof(PQLastTrade), typeof(PQLastTrade)));
        Assert.IsFalse(entrySelector.TypeCanWhollyContain(typeof(PQLastPaidGivenTrade), typeof(PQLastTrade)));
        Assert.IsFalse(entrySelector.TypeCanWhollyContain(typeof(PQLastExternalCounterPartyTrade),
                                                          typeof(PQLastTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(typeof(PQLastTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(
                                                         typeof(PQLastPaidGivenTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsFalse(entrySelector.TypeCanWhollyContain(
                                                          typeof(PQLastExternalCounterPartyTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(typeof(PQLastTrade), typeof(PQLastExternalCounterPartyTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(
                                                         typeof(PQLastPaidGivenTrade), typeof(PQLastExternalCounterPartyTrade)));
        Assert.IsTrue(entrySelector.TypeCanWhollyContain(
                                                         typeof(PQLastExternalCounterPartyTrade), typeof(PQLastExternalCounterPartyTrade)));
    }

    [TestMethod]
    public void NonPQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToTraderLastPaidEntryIfCantContain()
    {
        var result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), simpleLastTrade);
        Assert.AreSame(result, pqSimpleLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), simpleLastTrade);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqExternalCounterPartyLastTrade, traderNameIdGenerator.Clone(), simpleLastTrade);
        Assert.AreSame(result, pqExternalCounterPartyLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), paidGivenLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), paidGivenLastTrade);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqExternalCounterPartyLastTrade, traderNameIdGenerator.Clone(), paidGivenLastTrade);
        Assert.AreSame(result, pqExternalCounterPartyLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), externalCounterPartyLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastExternalCounterPartyTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), externalCounterPartyLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastExternalCounterPartyTrade));
        Assert.IsTrue(pqPaidGivenLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqExternalCounterPartyLastTrade, traderNameIdGenerator.Clone(), externalCounterPartyLastTrade);
        Assert.AreSame(result, pqExternalCounterPartyLastTrade);
    }

    [TestMethod]
    public void PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToTraderLastPaidEntryIfCantContain()
    {
        ILastTrade desiredType = pqSimpleLastTrade.Clone();
        var          result      = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqSimpleLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqExternalCounterPartyLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqExternalCounterPartyLastTrade);
        desiredType = paidGivenLastTrade.Clone();
        result      = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqExternalCounterPartyLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqExternalCounterPartyLastTrade);
        desiredType = externalCounterPartyLastTrade.Clone();
        result      = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastExternalCounterPartyTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastExternalCounterPartyTrade));
        Assert.IsTrue(pqPaidGivenLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqExternalCounterPartyLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqExternalCounterPartyLastTrade);
    }

    [TestMethod]
    public void CreateNewLastTradeEntries_SelectLastTradeEntry_ExpectedTypeIsReturned()
    {
        var result = entrySelector.FindForLastTradeFlags(LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedTime).CreateNewLastTradeEntry();
        Assert.AreEqual(typeof(PQLastTrade), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), null);
        Assert.AreSame(result, pqSimpleLastTrade);
    }
}
