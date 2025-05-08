// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LastTraded.EntrySelector;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerInfo.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LastTraded.EntrySelector;

[TestClass]
public class RecentlyTradedLastTradeEntrySelectorTests
{
    private const decimal ExpectedTradePrice  = 2.3456m;
    private const decimal ExpectedTradeVolume = 42_000_111m;

    private readonly RecentlyTradedLastTradeEntrySelector entrySelector = new();

    private readonly bool expectedWasGiven = true;
    private readonly bool expectedWasPaid  = true;

    private string             expectedTraderName = null!;
    private DateTime           expectedTradeTime;
    private LastPaidGivenTrade lastPaidGivenTrade = null!;
    private LastTrade          lastTrade          = null!;

    private LastTraderPaidGivenTrade lastTraderPaidGivenTrade = null!;
    private IPQNameIdLookupGenerator nameIdLookupGenerator    = null!;
    private PQLastPaidGivenTrade     pqLastPaidGivenTrade     = null!;

    private PQLastTrade                pqLastTrade                = null!;
    private PQLastTraderPaidGivenTrade pqLastTraderPaidGivenTrade = null!;
    private ISourceTickerInfo          sourceTickerInfo           = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);
        expectedTraderName    = "TraderName-Helen";
        expectedTradeTime     = new DateTime(2018, 03, 2, 14, 40, 30);

        lastTrade = new LastTrade(ExpectedTradePrice, expectedTradeTime);
        lastPaidGivenTrade = new LastPaidGivenTrade(ExpectedTradePrice, expectedTradeTime, ExpectedTradeVolume,
                                                    expectedWasPaid, expectedWasGiven);
        lastTraderPaidGivenTrade = new LastTraderPaidGivenTrade(ExpectedTradePrice, expectedTradeTime,
                                                                ExpectedTradeVolume, expectedWasPaid, expectedWasGiven, expectedTraderName);

        pqLastTrade = new PQLastTrade(ExpectedTradePrice, expectedTradeTime);
        pqLastPaidGivenTrade = new PQLastPaidGivenTrade(ExpectedTradePrice, expectedTradeTime, ExpectedTradeVolume,
                                                        expectedWasPaid, expectedWasGiven);
        pqLastTraderPaidGivenTrade = new PQLastTraderPaidGivenTrade(nameIdLookupGenerator.Clone(), ExpectedTradePrice, expectedTradeTime,
                                                                    ExpectedTradeVolume, expectedWasPaid, expectedWasGiven)
        {
            TraderName = expectedTraderName
        };

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1);
    }

    [TestMethod]
    public void VariosLastTradeFlags_FindForLastTradeFlags_ReturnsLastTrade()
    {
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.None;
        var pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTrade));
    }

    [TestMethod]
    public void VariosLastTradeFlags_FindForLastTradeFlags_ReturnsLastPaidGivenTrade()
    {
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        var pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice
                                                                            | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice
                                                                       | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.PaidOrGiven;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedPrice;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
    }

    [TestMethod]
    public void VariosLastTradeFlags_FindForLastTradeFlags_ReturnsLastTraderPaidGivenTrade()
    {
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName;
        var pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.LastTradedPrice;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedPrice;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.PaidOrGiven;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags)!;
        Assert.AreEqual(pvl.GetType(), typeof(LastTraderPaidGivenTrade));
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ConvertsToNonPQPriceVolumeLayerType()
    {
        var lt = entrySelector.ConvertToExpectedImplementation(pqLastTrade)!;
        Assert.AreEqual(lt.GetType(), typeof(LastTrade));
        Assert.AreEqual(ExpectedTradePrice, lt.TradePrice);
        Assert.AreEqual(expectedTradeTime, lt.TradeTime);

        lt = entrySelector.ConvertToExpectedImplementation(pqLastPaidGivenTrade);
        var lsPdGvnTrd = lt as LastPaidGivenTrade;
        Assert.IsNotNull(lsPdGvnTrd);
        Assert.AreEqual(ExpectedTradePrice, lsPdGvnTrd.TradePrice);
        Assert.AreEqual(expectedTradeTime, lsPdGvnTrd.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, lsPdGvnTrd.TradeVolume);
        Assert.AreEqual(expectedWasPaid, lsPdGvnTrd.WasPaid);
        Assert.AreEqual(expectedWasGiven, lsPdGvnTrd.WasGiven);

        lt = entrySelector.ConvertToExpectedImplementation(pqLastTraderPaidGivenTrade);
        var lsTrdrPdGvnTrd = lt as LastTraderPaidGivenTrade;
        Assert.IsNotNull(lsTrdrPdGvnTrd);
        Assert.AreEqual(ExpectedTradePrice, lsTrdrPdGvnTrd.TradePrice);
        Assert.AreEqual(expectedTradeTime, lsTrdrPdGvnTrd.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, lsTrdrPdGvnTrd.TradeVolume);
        Assert.AreEqual(expectedWasPaid, lsTrdrPdGvnTrd.WasPaid);
        Assert.AreEqual(expectedWasGiven, lsTrdrPdGvnTrd.WasGiven);
        Assert.AreEqual(expectedTraderName, lsTrdrPdGvnTrd.TraderName);
    }

    [TestMethod]
    public void NonPQLastTradeTypes_ConvertToExpectedImplementation_ClonesPQPriceVolumeLayerType()
    {
        var lt = entrySelector.ConvertToExpectedImplementation(lastTrade, true)!;
        Assert.AreEqual(lt.GetType(), typeof(LastTrade));
        Assert.AreNotSame(lastTrade, lt);
        Assert.AreEqual(ExpectedTradePrice, lt.TradePrice);
        Assert.AreEqual(expectedTradeTime, lt.TradeTime);

        lt = entrySelector.ConvertToExpectedImplementation(lastPaidGivenTrade, true)!;
        var lsPdGvnTrd = lt as LastPaidGivenTrade;
        Assert.IsNotNull(lsPdGvnTrd);
        Assert.AreNotSame(lastPaidGivenTrade, lsPdGvnTrd);
        Assert.AreEqual(ExpectedTradePrice, lsPdGvnTrd.TradePrice);
        Assert.AreEqual(expectedTradeTime, lsPdGvnTrd.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, lsPdGvnTrd.TradeVolume);
        Assert.AreEqual(expectedWasPaid, lsPdGvnTrd.WasPaid);
        Assert.AreEqual(expectedWasGiven, lsPdGvnTrd.WasGiven);

        lt = entrySelector.ConvertToExpectedImplementation(lastTraderPaidGivenTrade, true)!;
        var lsTrdrPdGvnTrd = lt as LastTraderPaidGivenTrade;
        Assert.IsNotNull(lsTrdrPdGvnTrd);
        Assert.AreNotSame(lastTraderPaidGivenTrade, lsTrdrPdGvnTrd);
        Assert.AreEqual(ExpectedTradePrice, lsTrdrPdGvnTrd.TradePrice);
        Assert.AreEqual(expectedTradeTime, lsTrdrPdGvnTrd.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, lsTrdrPdGvnTrd.TradeVolume);
        Assert.AreEqual(expectedWasPaid, lsTrdrPdGvnTrd.WasPaid);
        Assert.AreEqual(expectedWasGiven, lsTrdrPdGvnTrd.WasGiven);
        Assert.AreEqual(expectedTraderName, lsTrdrPdGvnTrd.TraderName);
    }

    [TestMethod]
    public void PriceVolumeLayerTypes_ConvertToExpectedImplementation_ReturnsSamePriceVolumeLayerType()
    {
        var lt = entrySelector.ConvertToExpectedImplementation(lastTrade)!;
        Assert.AreEqual(lt.GetType(), typeof(LastTrade));
        Assert.AreSame(lastTrade, lt);

        lt = entrySelector.ConvertToExpectedImplementation(lastPaidGivenTrade)!;
        var pqSrcPvl = lt as LastPaidGivenTrade;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreSame(lastPaidGivenTrade, lt);

        lt = entrySelector.ConvertToExpectedImplementation(lastTraderPaidGivenTrade)!;
        var pqSrcQtRefPvl = lt as LastTraderPaidGivenTrade;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreSame(lastTraderPaidGivenTrade, lt);
    }
}
