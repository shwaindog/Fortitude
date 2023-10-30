#region

using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector;

[TestClass]
public class PQLastTradeEntrySelectorTests
{
    private const decimal ExpectedPrice = 2.3456m;
    private const decimal ExpectedVolume = 42_000_111m;

    private readonly PQLastTradeEntrySelector entrySelector = new();

    private readonly IPQNameIdLookupGenerator tradeNnameIdGenerator = new PQNameIdLookupGenerator(
        PQFieldKeys.LastTraderDictionaryUpsertCommand, PQFieldFlags.TraderNameIdLookupSubDictionaryKey);

    private string expectedTradeName = null!;
    private DateTime expectedTradeTime;
    private LastPaidGivenTrade paidGivenLastTrade = null!;
    private PQLastPaidGivenTrade pqPaidGivenLastTrade = null!;

    private PQLastTrade pqSimpleLastTrade = null!;
    private IPQSourceTickerQuoteInfo pqSourceTickerQuoteInfo = null!;
    private PQLastTraderPaidGivenTrade pqTraderPaidGivenLastTrade = null!;

    private LastTrade simpleLastTrade = null!;
    private LastTraderPaidGivenTrade traderPaidGivenLastTrade = null!;

    [TestInitialize]
    public void SetUp()
    {
        expectedTradeName = "TraderName-Anatoly";
        expectedTradeTime = new DateTime(2018, 01, 9, 22, 50, 59);
        simpleLastTrade = new LastTrade(ExpectedPrice, expectedTradeTime);
        paidGivenLastTrade = new LastPaidGivenTrade(ExpectedPrice, expectedTradeTime, ExpectedVolume, true, true);
        traderPaidGivenLastTrade = new LastTraderPaidGivenTrade(ExpectedPrice, expectedTradeTime,
            ExpectedVolume, true, true, expectedTradeName);
        pqSimpleLastTrade = new PQLastTrade(ExpectedPrice, expectedTradeTime);
        pqPaidGivenLastTrade = new PQLastPaidGivenTrade(ExpectedPrice, expectedTradeTime, ExpectedVolume,
            true, true);
        pqTraderPaidGivenLastTrade = new PQLastTraderPaidGivenTrade(ExpectedPrice, expectedTradeTime,
            ExpectedVolume, true, true, tradeNnameIdGenerator, expectedTradeName);
        pqSourceTickerQuoteInfo = new PQSourceTickerQuoteInfo(new SourceTickerQuoteInfo(uint.MaxValue, "TestSource",
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                                  LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime));
        pqSourceTickerQuoteInfo.TraderNameIdLookup = tradeNnameIdGenerator;
    }

    [TestMethod]
    public void VariosLastTradeFlags_Select_ReturnsSimpleLastTradeEntryFactory()
    {
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.None;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedTime;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTradeFactory));
    }

    [TestMethod]
    public void VariosLastTradeFlags_Select_ReturnsSimpleLastPaidGivenTradeEntryFactory()
    {
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                                  LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                                  LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                                  LastTradedFlags.LastTradedVolume;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                                  LastTradedFlags.PaidOrGiven;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastPaidGivenTradeFactory));
    }

    [TestMethod]
    public void VariosLastTradeFlags_Select_ReturnsSimpleLastTraderPaidGivenTradeEntryFactory()
    {
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName;
        var pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume |
                                                  LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven |
                                                  LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                                  LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedTime |
                                                  LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                                  LastTradedFlags.LastTradedVolume | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
        pqSourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice |
                                                  LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName;
        pqRecentlyTradedFactory = entrySelector.FindForLastTradeFlags(pqSourceTickerQuoteInfo)!;
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQLastTraderPaidGivenTradeFactory));
    }

    [TestMethod]
    public void NonPQLastTradeTypes_ConvertIfNonPQLastTrade_ConvertsToPQLastTradeType()
    {
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(simpleLastTrade)!;
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(paidGivenLastTrade)!;
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedVolume, pqLastPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasGiven);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(traderPaidGivenLastTrade)!;
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
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqSimpleLastTrade, true)!;
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreNotSame(pqLastTradeEntry, pqSimpleLastTrade);
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqPaidGivenLastTrade, true)!;
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreNotSame(pqLastTradeEntry, pqPaidGivenLastTrade);
        Assert.AreEqual(ExpectedPrice, pqLastTradeEntry.TradePrice);
        Assert.AreEqual(expectedTradeTime, pqLastTradeEntry.TradeTime);
        Assert.AreEqual(ExpectedVolume, pqLastPaidGivenEntry.TradeVolume);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasPaid);
        Assert.AreEqual(true, pqLastPaidGivenEntry.WasGiven);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqTraderPaidGivenLastTrade, true)!;
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
        var pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqSimpleLastTrade)!;
        Assert.AreEqual(pqLastTradeEntry.GetType(), typeof(PQLastTrade));
        Assert.AreSame(pqLastTradeEntry, pqSimpleLastTrade);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqPaidGivenLastTrade);
        var pqLastPaidGivenEntry = pqLastTradeEntry as PQLastPaidGivenTrade;
        Assert.IsNotNull(pqLastPaidGivenEntry);
        Assert.AreSame(pqLastTradeEntry, pqPaidGivenLastTrade);
        pqLastTradeEntry = entrySelector.ConvertToExpectedImplementation(pqTraderPaidGivenLastTrade);
        var pqLastTraderPaidGivenEntry = pqLastTradeEntry as PQLastTraderPaidGivenTrade;
        Assert.IsNotNull(pqLastTraderPaidGivenEntry);
        Assert.AreSame(pqLastTradeEntry, pqTraderPaidGivenLastTrade);
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
        var result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, simpleLastTrade);
        Assert.AreSame(result, pqSimpleLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, simpleLastTrade);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, simpleLastTrade);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, paidGivenLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, paidGivenLastTrade);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, paidGivenLastTrade);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, traderPaidGivenLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, traderPaidGivenLastTrade);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqPaidGivenLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, traderPaidGivenLastTrade);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
    }

    [TestMethod]
    public void PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToTraderLastPaidEntryIfCantContain()
    {
        var desiredType = pqSimpleLastTrade.Clone();
        var result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, desiredType);
        Assert.AreSame(result, pqSimpleLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, desiredType);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, desiredType);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
        desiredType = paidGivenLastTrade.Clone();
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, desiredType);
        Assert.AreSame(result, pqPaidGivenLastTrade);
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, desiredType);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
        desiredType = traderPaidGivenLastTrade.Clone();
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqSimpleLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqPaidGivenLastTrade, desiredType);
        Assert.AreNotSame(result, pqSimpleLastTrade);
        Assert.IsInstanceOfType(result, typeof(PQLastTraderPaidGivenTrade));
        Assert.IsTrue(pqPaidGivenLastTrade.AreEquivalent(result));
        result = entrySelector.SelectLastTradeEntry(pqTraderPaidGivenLastTrade, desiredType);
        Assert.AreSame(result, pqTraderPaidGivenLastTrade);
    }

    [TestMethod]
    public void NullLastTradeEntries_SelectLastTradeEntry_HandlesEmptyValues()
    {
        var result = entrySelector.SelectLastTradeEntry(null, simpleLastTrade)!;
        Assert.AreEqual(typeof(PQLastTrade), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = entrySelector.SelectLastTradeEntry(pqSimpleLastTrade, null);
        Assert.AreSame(result, pqSimpleLastTrade);
    }
}
