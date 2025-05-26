// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;

[TestClass]
public class LastTradedLastTradeEntrySelectorTests
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

    private readonly LastTradedLastTradeEntrySelector entrySelector = new();

    private LastPaidGivenTrade lastPaidGivenTrade = null!;
    private LastTrade          lastTrade          = null!;

    private LastExternalCounterPartyTrade lastExternalCounterPartyTrade = null!;
    private IPQNameIdLookupGenerator      nameIdLookupGenerator         = null!;
    private PQLastPaidGivenTrade          pqLastPaidGivenTrade          = null!;

    private PQLastTrade                     pqLastTrade                     = null!;
    private PQLastExternalCounterPartyTrade pqLastExternalCounterPartyTrade = null!;
    private ISourceTickerInfo               sourceTickerInfo                = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);

        lastTrade = new LastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        lastPaidGivenTrade =
            new LastPaidGivenTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime);
        lastExternalCounterPartyTrade =
            new LastExternalCounterPartyTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedCounterPartyId
               , ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid, ExpectedWasGiven
               , ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);

        pqLastTrade = new PQLastTrade(ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
                                    , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        pqLastPaidGivenTrade =
            new PQLastPaidGivenTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime);
        pqLastExternalCounterPartyTrade =
            new PQLastExternalCounterPartyTrade
                (nameIdLookupGenerator.Clone(), ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
                {
                    ExternalTraderName = ExpectedTraderName
                };

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m);
    }

    [TestMethod]
    public void VariousLastTradeFlags_FindForLastTradeFlags_ReturnsLastTrade()
    {
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.None;
        var pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastTrade));
    }

    [TestMethod]
    public void VariousLastTradeFlags_FindForLastTradeFlags_ReturnsLastPaidGivenTrade()
    {
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        var pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice
                                                                            | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice
                                                                       | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.PaidOrGiven;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedPrice;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastPaidGivenTrade));
    }

    [TestMethod]
    public void VariousLastTradeFlags_FindForLastTradeFlags_ReturnsLastTraderPaidGivenTrade()
    {
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName;
        var pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedTime;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.LastTradedPrice;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven;
        pvl                              = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedPrice;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                           LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.PaidOrGiven;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice |
                                           LastTradedFlags.LastTradedTime;
        pvl = entrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(pvl.GetType(), typeof(LastExternalCounterPartyTrade));
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ConvertsToNonPQPriceVolumeLayerType()
    {
        var lt = entrySelector.ConvertToExpectedImplementation(pqLastTrade);
        Assert.AreEqual(lt.GetType(), typeof(LastTrade));
        Assert.AreEqual(ExpectedTradePrice, lt.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, lt.TradeTime);

        lt = entrySelector.ConvertToExpectedImplementation(pqLastPaidGivenTrade);
        var lsPdGvnTrd = lt as LastPaidGivenTrade;
        Assert.IsNotNull(lsPdGvnTrd);
        Assert.AreEqual(ExpectedTradePrice, lsPdGvnTrd.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, lsPdGvnTrd.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, lsPdGvnTrd.TradeVolume);
        Assert.AreEqual(ExpectedWasPaid, lsPdGvnTrd.WasPaid);
        Assert.AreEqual(ExpectedWasGiven, lsPdGvnTrd.WasGiven);

        lt = entrySelector.ConvertToExpectedImplementation(pqLastExternalCounterPartyTrade);
        var lastExtCpTrade = lt as LastExternalCounterPartyTrade;
        Assert.IsNotNull(lastExtCpTrade);
        Assert.AreEqual(ExpectedTradePrice, lastExtCpTrade.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, lastExtCpTrade.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, lastExtCpTrade.TradeVolume);
        Assert.AreEqual(ExpectedWasPaid, lastExtCpTrade.WasPaid);
        Assert.AreEqual(ExpectedWasGiven, lastExtCpTrade.WasGiven);
        Assert.AreEqual(ExpectedTraderName, lastExtCpTrade.ExternalTraderName);
    }

    [TestMethod]
    public void NonPQLastTradeTypes_ConvertToExpectedImplementation_ClonesPQPriceVolumeLayerType()
    {
        var lt = entrySelector.ConvertToExpectedImplementation(lastTrade, true);
        Assert.AreEqual(lt.GetType(), typeof(LastTrade));
        Assert.AreNotSame(lastTrade, lt);
        Assert.AreEqual(ExpectedTradePrice, lt.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, lt.TradeTime);

        lt = entrySelector.ConvertToExpectedImplementation(lastPaidGivenTrade, true);
        var lsPdGvnTrd = lt as LastPaidGivenTrade;
        Assert.IsNotNull(lsPdGvnTrd);
        Assert.AreNotSame(lastPaidGivenTrade, lsPdGvnTrd);
        Assert.AreEqual(ExpectedTradePrice, lsPdGvnTrd.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, lsPdGvnTrd.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, lsPdGvnTrd.TradeVolume);
        Assert.AreEqual(ExpectedWasPaid, lsPdGvnTrd.WasPaid);
        Assert.AreEqual(ExpectedWasGiven, lsPdGvnTrd.WasGiven);

        lt = entrySelector.ConvertToExpectedImplementation(lastExternalCounterPartyTrade, true);
        var lastExtCpTrade = lt as LastExternalCounterPartyTrade;
        Assert.IsNotNull(lastExtCpTrade);
        Assert.AreNotSame(lastExternalCounterPartyTrade, lastExtCpTrade);
        Assert.AreEqual(ExpectedTradePrice, lastExtCpTrade.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, lastExtCpTrade.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, lastExtCpTrade.TradeVolume);
        Assert.AreEqual(ExpectedWasPaid, lastExtCpTrade.WasPaid);
        Assert.AreEqual(ExpectedWasGiven, lastExtCpTrade.WasGiven);
        Assert.AreEqual(ExpectedTraderName, lastExtCpTrade.ExternalTraderName);
    }

    [TestMethod]
    public void PriceVolumeLayerTypes_ConvertToExpectedImplementation_ReturnsSamePriceVolumeLayerType()
    {
        var lt = entrySelector.ConvertToExpectedImplementation(lastTrade);
        Assert.AreEqual(lt.GetType(), typeof(LastTrade));
        Assert.AreSame(lastTrade, lt);

        lt = entrySelector.ConvertToExpectedImplementation(lastPaidGivenTrade);
        var pqSrcPvl = lt as LastPaidGivenTrade;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreSame(lastPaidGivenTrade, lt);

        lt = entrySelector.ConvertToExpectedImplementation(lastExternalCounterPartyTrade);
        var pqSrcQtRefPvl = lt as LastExternalCounterPartyTrade;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreSame(lastExternalCounterPartyTrade, lt);
    }
}
