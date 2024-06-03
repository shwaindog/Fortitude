// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LastTraded.EntrySelector;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LastTraded.EntrySelector;

[TestClass]
public class LastTradeEntryFlagsSelectorTests
{
    private DummyLastTradeEntryFlagsSelector layerSelector         = null!;
    private ISourceTickerQuoteInfo           sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        layerSelector = new DummyLastTradeEntryFlagsSelector();

        sourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
                                                          "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                                                          LayerFlags.Volume | LayerFlags.Price
                                                        , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                                          LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSimpleLastTradeSelector()
    {
        var expectedSelectorCalled = "SelectSimpleLastTradeEntry";
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.None;
        var selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedTime;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsLastPaidGivenTradeSelector()
    {
        var expectedSelectorCalled = "SelectLastPaidGivenTradeEntry";
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume;
        var selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedPrice;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime |
                                                LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume |
                                                LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume |
                                                LastTradedFlags.LastTradedTime;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume |
                                                LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsLastTraderPaidGivenTradeSelector()
    {
        var expectedSelectorCalled = "SelectTraderLastTradeEntry";

        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName;
        var selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                                LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                                LastTradedFlags.LastTradedTime;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven;
        selectorCalled                        = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                                LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                                LastTradedFlags.LastTradedTime;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                                LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                                LastTradedFlags.LastTradedVolume;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                                LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                                LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven |
                                                LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime |
                                                LastTradedFlags.LastTradedPrice;
        selectorCalled = layerSelector.FindForLastTradeFlags(sourceTickerQuoteInfo.LastTradedFlags);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    internal class DummyLastTradeEntryFlagsSelector : LastTradeEntryFlagsSelector<string>
    {
        protected override string SelectSimpleLastTradeEntry() => nameof(SelectSimpleLastTradeEntry);

        protected override string SelectLastPaidGivenTradeEntry() => nameof(SelectLastPaidGivenTradeEntry);

        protected override string SelectTraderLastTradeEntry() => nameof(SelectTraderLastTradeEntry);

        public override IMutableLastTrade? ConvertToExpectedImplementation(ILastTrade? checkLastTrade, bool clone = false) =>
            throw new NotImplementedException();
    }
}
