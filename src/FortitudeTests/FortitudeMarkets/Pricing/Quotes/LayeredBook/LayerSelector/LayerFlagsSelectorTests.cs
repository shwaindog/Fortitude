// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerSelector;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerSelector;

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
    public void VariousLayerFlags_Select_CallsValueDatePriceVolumeSelector()
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
    public void VariousLayerFlags_Select_CallsOrdersCountPriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectOrdersCountPriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.OrdersCount;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.InternalVolume;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.OrdersCount;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.InternalVolume;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariousLayerFlags_Select_CallsAnonymousOrdersPriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectAnonymousOrdersPriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.OrderCreated;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.OrderId;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.OrderUpdated;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.OrderSize;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariousLayerFlags_Select_CallsCounterPartyOrdersPriceVolumeSelector()
    {
        var expectedSelectorCalled = "SelectCounterPartyOrdersPriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.OrderTraderName;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.OrderCounterPartyName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.OrderTraderName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.OrderCounterPartyName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }

    [TestMethod]
    public void VariosLayerFlags_Select_CallsSrcQtRefTrdrVlDtSelector()
    {
        var expectedSelectorCalled = "SelectSourceQuoteRefTraderValueDatePriceVolumeLayer";
        sourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.OrderTraderName;
        var selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.OrderTraderName;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                      LayerFlags.ValueDate | LayerFlags.SourceName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);

        sourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
        selectorCalled              = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.OrderTraderName | LayerFlags.Executable |
                                      LayerFlags.SourceQuoteReference;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.OrderTraderName | LayerFlags.Executable | LayerFlags.ValueDate;
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
                                      LayerFlags.SourceQuoteReference | LayerFlags.OrderTraderName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName |
                                      LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
        sourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                      LayerFlags.SourceName | LayerFlags.SourceQuoteReference |
                                      LayerFlags.ValueDate | LayerFlags.OrderTraderName;
        selectorCalled = layerSelector.FindForLayerFlags(sourceTickerInfo);
        Assert.AreEqual(expectedSelectorCalled, selectorCalled);
    }


    internal class DummyLayerFlagsSelectorTests : LayerFlagsSelector<string>
    {
        protected override string SelectSimplePriceVolumeLayer() => nameof(SelectSimplePriceVolumeLayer);

        protected override string SelectValueDatePriceVolumeLayer() => nameof(SelectValueDatePriceVolumeLayer);

        protected override string SelectSourcePriceVolumeLayer() => nameof(SelectSourcePriceVolumeLayer);

        protected override string SelectSourceQuoteRefPriceVolumeLayer() => nameof(SelectSourceQuoteRefPriceVolumeLayer);

        protected override string SelectOrdersCountPriceVolumeLayer() => nameof(SelectOrdersCountPriceVolumeLayer);

        protected override string SelectAnonymousOrdersPriceVolumeLayer() => nameof(SelectAnonymousOrdersPriceVolumeLayer);

        protected override string SelectCounterPartyOrdersPriceVolumeLayer() => nameof(SelectCounterPartyOrdersPriceVolumeLayer);

        protected override string SelectSourceQuoteRefTraderValueDatePriceVolumeLayer() => nameof(SelectSourceQuoteRefTraderValueDatePriceVolumeLayer);

        public override IPriceVolumeLayer CreateExpectedImplementation
        (LayerType desiredLayerType, IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
            throw new NotImplementedException();
    }
}
