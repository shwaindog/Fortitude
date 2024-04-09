#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

public class OrderBookLayerFactorySelector : LayerFlagsSelector<IPriceVolumeLayer, ISourceTickerQuoteInfo>
{
    protected override IPriceVolumeLayer SelectSimplePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new PriceVolumeLayer();

    protected override IPriceVolumeLayer SelectValueDatePriceVolumeLayer(
        ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new ValueDatePriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourcePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new SourcePriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourceQuoteRefPriceVolumeLayer(
        ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new SourceQuoteRefPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectTraderPriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new TraderPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(
        ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new SourceQuoteRefTraderValueDatePriceVolumeLayer();

    public override IPriceVolumeLayer ConvertToExpectedImplementation(IPriceVolumeLayer? priceVolumeLayer,
        bool clone = false)
    {
        switch (priceVolumeLayer)
        {
            case PriceVolumeLayer _:
                return clone ? priceVolumeLayer.Clone() : priceVolumeLayer;
            case ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdVlDtPvLayer:
                return new SourceQuoteRefTraderValueDatePriceVolumeLayer(srcQtRefTrdVlDtPvLayer);
            case ISourceQuoteRefPriceVolumeLayer srcQtRefPvLayer:
                return new SourceQuoteRefPriceVolumeLayer(srcQtRefPvLayer);
            case ISourcePriceVolumeLayer sourcePvLayer:
                return new SourcePriceVolumeLayer(sourcePvLayer);
            case IValueDatePriceVolumeLayer valueDatePvLayer:
                return new ValueDatePriceVolumeLayer(valueDatePvLayer);
            case ITraderPriceVolumeLayer traderPvLayer:
                return new TraderPriceVolumeLayer(traderPvLayer);
            default:
                return new PriceVolumeLayer(priceVolumeLayer!);
        }
    }
}
