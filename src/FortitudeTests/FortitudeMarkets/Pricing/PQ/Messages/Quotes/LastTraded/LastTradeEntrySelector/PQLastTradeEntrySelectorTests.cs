// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

[TestClass]
public class PQLastTradeEntrySelectorTests
{
    private const decimal ExpectedPrice  = 2.3456m;
    private const decimal ExpectedVolume = 42_000_111m;

    private PQLastTradeEntrySelector entrySelector = null!;

    private string               expectedTradeName = null!;
    private DateTime             expectedTradeTime;
    private LastPaidGivenTrade   paidGivenLastTrade   = null!;
    private PQLastPaidGivenTrade pqPaidGivenLastTrade = null!;

    private PQLastTrade                pqSimpleLastTrade          = null!;
    private IPQSourceTickerInfo        pqSourceTickerInfo         = null!;
    private PQLastTraderPaidGivenTrade pqTraderPaidGivenLastTrade = null!;

    private LastTrade simpleLastTrade = null!;

    private IPQNameIdLookupGenerator traderNameIdGenerator    = null!;
    private LastTraderPaidGivenTrade traderPaidGivenLastTrade = null!;

    [TestInitialize]
    public void SetUp()
    {
        traderNameIdGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
        entrySelector         = new PQLastTradeEntrySelector(traderNameIdGenerator);
        expectedTradeName     = "TraderName-Anatoly";
        expectedTradeTime     = new DateTime(2018, 01, 9, 22, 50, 59);
        simpleLastTrade       = new LastTrade(ExpectedPrice, expectedTradeTime);
        paidGivenLastTrade    = new LastPaidGivenTrade(ExpectedPrice, expectedTradeTime, ExpectedVolume, true, true);
        traderPaidGivenLastTrade = new LastTraderPaidGivenTrade(ExpectedPrice, expectedTradeTime,
                                                                ExpectedVolume, true, true, expectedTradeName);
        pqSimpleLastTrade = new PQLastTrade(ExpectedPrice, expectedTradeTime);
        pqPaidGivenLastTrade = new PQLastPaidGivenTrade(ExpectedPrice, expectedTradeTime, ExpectedVolume,
                                                        true, true);
        pqTraderPaidGivenLastTrade = new PQLastTraderPaidGivenTrade(traderNameIdGenerator, ExpectedPrice, expectedTradeTime,
                                                                    ExpectedVolume, true, true, expectedTradeName);
        pqSourceTickerInfo =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
                   , 20, 0.00001m, 30000m, 50000000m, 1000m, 1, 100
                   , 10_000, true, true, LayerFlags.Volume | LayerFlags.Price
                   , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime));
        pqSourceTickerInfo.NameIdLookup = traderNameIdGenerator;
    }

    [TestMethod]
    public void VariosLastTradeFlags_Select_ReturnsSimpleLastTradeEntryFactory()
    {
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.None;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
    }

    [TestMethod]
    public void VariosLastTradeFlags_Select_ReturnsSimpleLastPaidGivenTradeEntryFactory()
    {
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory            = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                             LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                             LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                             LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                             LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
    }

    [TestMethod]
    public void VariosLastTradeFlags_Select_ReturnsSimpleLastTraderPaidGivenTradeEntryFactory()
    {
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume |
                                             LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven |
                                             LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                             LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                             LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                             LastTradedFlags.LastTradedVolume | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                             LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
    }

    [TestMethod]
    public void NonPQLastTradeTypes_ConvertIfNonPQLastTrade_ConvertsToPQLastTradeType()
    {
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(simpleLastTrade, traderNameIdGenerator.Clone())!;
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(paidGivenLastTrade, traderNameIdGenerator.Clone())!;
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedVolume, pqLastPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasGiven);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(traderPaidGivenLastTrade, traderNameIdGenerator.Clone())!;
        var pqLastTraderPaidGivenEntry = pqLastTradeEntry as PQLastTraderPaidGivenTrade;
        Assert.IsNotNull(pqLastTraderPaidGivenEntry);
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedVolume, pqLastTraderPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastTraderPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastTraderPaidGivenEntry.WasGiven);
        Assert.AreEqual(expectedTradeName, pqLastTraderPaidGivenEntry.TraderName);
    }

    [TestMethod]
    public void PQLastTradeTypes_ConvertIfNonPQLastTrade_ClonesPQLastTradeType()
    {
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqSimpleLastTrade, traderNameIdGenerator.Clone(), true)!;
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreNotSame(pqLastTradeEntry, pqSimpleLastTrade);
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), true)!;
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreNotSame(pqLastTradeEntry, pqPaidGivenLastTrade);
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedVolume, pqLastPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasGiven);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqTraderPaidGivenLastTrade, traderNameIdGenerator.Clone(), true)!;
        var pqLastTraderPaidGivenEntry = pqLastTradeEntry as PQLastTraderPaidGivenTrade;
        Assert.IsNotNull(pqLastTraderPaidGivenEntry);
        Assert.AreNotSame(pqLastTradeEntry, pqTraderPaidGivenLastTrade);
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedVolume, pqLastTraderPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastTraderPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastTraderPaidGivenEntry.WasGiven);
        Assert.AreEqual(expectedTradeName, pqLastTraderPaidGivenEntry.TraderName);
    }

    [TestMethod]
    public void PQLastTradeTypes_ConvertIfNonPQLastTrade_ReturnsSamePQLastTradeType()
    {
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqSimpleLastTrade, traderNameIdGenerator.Clone())!;
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreSame(pqLastTradeEntry, pqSimpleLastTrade);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqPaidGivenLastTrade, traderNameIdGenerator.Clone());
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreSame(pqLastTradeEntry, pqPaidGivenLastTrade);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqTraderPaidGivenLastTrade, traderNameIdGenerator);
        var pqLastTraderPaidGivenEntry = pqLastTradeEntry as PQLastTraderPaidGivenTrade;
        Assert.IsNotNull(pqLastTraderPaidGivenEntry);
        Assert.AreNotSame(pqLastTradeEntry, pqTraderPaidGivenLastTrade);
    }

    [TestMethod]
    public void NonPQLastTradeTypes_TypeCanWholeyContain_ReturnsAsExpected()
    {
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(typeof(LastTrade), typeof(PQLastTrade)));
        Assert.IsFalse(entrySelector.TypeCanWholeyContain(typeof(LastPaidGivenTrade), typeof(PQLastTrade)));
        Assert.IsFalse(entrySelector.TypeCanWholeyContain(typeof(LastTraderPaidGivenTrade), typeof(PQLastTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(typeof(LastTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(
                                                         typeof(LastPaidGivenTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsFalse(entrySelector.TypeCanWholeyContain(
                                                          typeof(LastTraderPaidGivenTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(typeof(LastTrade), typeof(PQLastTraderPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(
                                                         typeof(LastPaidGivenTrade), typeof(PQLastTraderPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(
                                                         typeof(LastTraderPaidGivenTrade), typeof(PQLastTraderPaidGivenTrade)));
    }

    [TestMethod]
    public void PQLastTradeTypes_TypeCanWholeyContain_ReturnsAsExpected()
    {
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(typeof(PQLastTrade), typeof(PQLastTrade)));
        Assert.IsFalse(entrySelector.TypeCanWholeyContain(typeof(PQLastPaidGivenTrade), typeof(PQLastTrade)));
        Assert.IsFalse(entrySelector.TypeCanWholeyContain(typeof(PQLastTraderPaidGivenTrade),
                                                          typeof(PQLastTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(typeof(PQLastTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(
                                                         typeof(PQLastPaidGivenTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsFalse(entrySelector.TypeCanWholeyContain(
                                                          typeof(PQLastTraderPaidGivenTrade), typeof(PQLastPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(typeof(PQLastTrade), typeof(PQLastTraderPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(
                                                         typeof(PQLastPaidGivenTrade), typeof(PQLastTraderPaidGivenTrade)));
        Assert.IsTrue(entrySelector.TypeCanWholeyContain(
                                                         typeof(PQLastTraderPaidGivenTrade), typeof(PQLastTraderPaidGivenTrade)));
    }

    [TestMethod]
    public void NonPQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToTraderLastPaidEntryIfCantContain()
    {
        var result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), simpleLastTrade);
        Assert.AreSame(result, pqSimpleLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), simpleLastTrade);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, traderNameIdGenerator.Clone(), simpleLastTrade);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), paidGivenLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), paidGivenLastTrade);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, traderNameIdGenerator.Clone(), paidGivenLastTrade);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), traderPaidGivenLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), traderPaidGivenLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqPaidGivenLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, traderNameIdGenerator.Clone(), traderPaidGivenLastTrade);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
    }

    [TestMethod]
    public void PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToTraderLastPaidEntryIfCantContain()
    {
        var desiredType = pqSimpleLastTrade.Clone();
        var result      = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqSimpleLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
        desiredType = paidGivenLastTrade.Clone();
        result      = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
        desiredType = traderPaidGivenLastTrade.Clone();
        result      = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqPaidGivenLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, traderNameIdGenerator.Clone(), desiredType);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
    }

    [TestMethod]
    public void NullLastTradeEntries_SelectLastTradeEntry_HandlesEmptyValues()
    {
        var result = entrySelector.SelectLastTradeEntry(null, traderNameIdGenerator.Clone(), simpleLastTrade)!;
        Assert.AreEqual(typeof(PQLastTrade), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderNameIdGenerator.Clone(), null);
        Assert.AreSame(result, pqSimpleLastTrade);
    }
}
