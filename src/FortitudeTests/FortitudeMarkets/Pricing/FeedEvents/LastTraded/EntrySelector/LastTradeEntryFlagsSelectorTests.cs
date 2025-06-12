// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Configuration;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;

[TestClass]
public class LastTradeEntryFlagsSelectorTests
{
    private DummyLastTradeEntryFlagsSelector layerSelector = null!;

    private ISourceTickerInfo sourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        layerSelector = new DummyLastTradeEntryFlagsSelector();

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", TickerQuoteDetailLevel.Level3Quote
           , MarketClassification.Unknown, AUinMEL, AUinMEL, AUinMEL
           , 20, 0.00001m, 30000m, 50000000m, 1000m
           , layerFlags: LayerFlags.Volume | LayerFlags.Price
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void VariousLayerFlags_Select_CallsSimpleLastTradeSelector()
    {
        var expectedSelectorCalled = "SelectSimpleLastTradeEntry";
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.None;
        var selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedTime;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariousLayerFlags_Select_CallsLastPaidGivenTradeSelector()
    {
        var expectedSelectorCalled = "SelectLastPaidGivenTradeEntry";
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        var selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariousLayerFlags_Select_CallsLastTraderPaidGivenTradeSelector()
    {
        var expectedSelectorCalled = "SelectTraderLastTradeEntry";

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName;
        var selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                           LastTradedFlags.LastTradedTime;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven;
        selectorCalled                   = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime
          | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    internal class DummyLastTradeEntryFlagsSelector : LastTradeEntryFlagsSelector<string>
    {
        protected override string SelectSimpleLastTradeEntry() => nameof(SelectSimpleLastTradeEntry);

        protected override string SelectLastPaidGivenTradeEntry() => nameof(SelectLastPaidGivenTradeEntry);

        protected override string SelectTraderLastTradeEntry() => nameof(SelectTraderLastTradeEntry);

        public override IMutableLastTrade ConvertToExpectedImplementation(ILastTrade? checkLastTrade, bool clone = false) =>
            throw new NotImplementedException();
    }
}
