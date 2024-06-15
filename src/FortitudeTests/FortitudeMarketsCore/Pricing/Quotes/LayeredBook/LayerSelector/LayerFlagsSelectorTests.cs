// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class LayerFlagsSelectorTests
{
    private DummyLayerFlagsSelectorTests layerSelector         = null!;
    private ISourceTickerQuoteInfo       sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        layerSelector = new DummyLayerFlagsSelectorTests();

        sourceTickerQuoteInfo = new SourceTickerQuoteInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , LayerFlags.Volume | LayerFlags.Price
           , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSimplePriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectSimplePriceVolumeLayer";
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.None;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSourcePriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectSourcePriceVolumeLayer";
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceName;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                           LayerFlags.Executable | LayerFlags.SourceName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSourceQuoteRefPriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectSourceQuoteRefPriceVolumeLayer";
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceQuoteReference;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceQuoteReference;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                           LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                           LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                           LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                           LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                           LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                           LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                           LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsValueDatePriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectValueDatePriceVolumeLayer";
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.ValueDate;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.ValueDate;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.ValueDate;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                           LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsTraderPriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectTraderPriceVolumeLayer";
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.TraderName;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.TraderName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.TraderName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                           LayerFlags.TraderName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSrcQtRefTrdrVlDtSelector()
    {
        var expectedSelectorCalled = "SelectSourceQuoteRefTraderValueDatePriceVolumeLayer";
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.TraderName;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                           LayerFlags.ValueDate | LayerFlags.SourceName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable |
                                           LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable | LayerFlags.ValueDate;
        selectorCalled                   = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                           LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                           LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName |
                                           LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName |
                                           LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                           LayerFlags.SourceName | LayerFlags.SourceQuoteReference |
                                           LayerFlags.ValueDate | LayerFlags.TraderName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerQuoteInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }


    internal class DummyLayerFlagsSelectorTests : LayerFlagsSelector<string, ISourceTickerQuoteInfo>
    {
        protected override string SelectSimplePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => nameof(SelectSimplePriceVolumeLayer);

        protected override string SelectValueDatePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
            nameof(SelectValueDatePriceVolumeLayer);

        protected override string SelectSourcePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => nameof(SelectSourcePriceVolumeLayer);

        protected override string SelectSourceQuoteRefPriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
            nameof(SelectSourceQuoteRefPriceVolumeLayer);

        protected override string SelectTraderPriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => nameof(SelectTraderPriceVolumeLayer);

        protected override string SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(
            ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
            nameof(SelectSourceQuoteRefTraderValueDatePriceVolumeLayer);

        public override IPriceVolumeLayer CreateExpectedImplementation(LayerType desiredLayerType, IPriceVolumeLayer? copy = null,
            CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            throw new NotImplementedException();
    }
}
