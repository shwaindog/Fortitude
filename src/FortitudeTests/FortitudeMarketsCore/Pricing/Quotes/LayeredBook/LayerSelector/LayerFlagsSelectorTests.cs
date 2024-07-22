// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class LayerFlagsSelectorTests
{
    private DummyLayerFlagsSelectorTests layerSelector = null!;

    private ISourceTickerInfo sourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        layerSelector = new DummyLayerFlagsSelectorTests();

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSimplePriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectSimplePriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.None;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSourcePriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectSourcePriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.SourceName;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LayerFlags = LayerFlags.Executable;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.Executable | LayerFlags.SourceName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSourceQuoteRefPriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectSourceQuoteRefPriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceQuoteReference;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsValueDatePriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectValueDatePriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.ValueDate;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.ValueDate;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsTraderPriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectTraderPriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.TraderName;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.TraderName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.TraderName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.TraderName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSrcQtRefTrdrVlDtSelector()
    {
        var expectedSelectorCalled = "SelectSourceQuoteRefTraderValueDatePriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.TraderName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.ValueDate | LayerFlags.SourceName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable | LayerFlags.ValueDate;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference |
                                      LayerFlags.ValueDate | LayerFlags.TraderName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }


    internal class DummyLayerFlagsSelectorTests : LayerFlagsSelector<string, ISourceTickerInfo>
    {
        protected override string SelectSimplePriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => nameof(SelectSimplePriceVolumeLayer);

        protected override string SelectValueDatePriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => nameof(SelectValueDatePriceVolumeLayer);

        protected override string SelectSourcePriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => nameof(SelectSourcePriceVolumeLayer);

        protected override string SelectSourceQuoteRefPriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) =>
            nameof(SelectSourceQuoteRefPriceVolumeLayer);

        protected override string SelectTraderPriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => nameof(SelectTraderPriceVolumeLayer);

        protected override string SelectSourceQuoteRefTraderValueDatePriceVolumeLayer
        (
            ISourceTickerInfo sourceTickerInfo) =>
            nameof(SelectSourceQuoteRefTraderValueDatePriceVolumeLayer);

        public override IPriceVolumeLayer CreateExpectedImplementation
        (LayerType desiredLayerType, IPriceVolumeLayer? copy = null,
            CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            throw new NotImplementedException();
    }
}
