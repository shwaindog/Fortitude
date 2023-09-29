using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector
{
    public class OrderBookLayerFactorySelector : LayerFlagsSelector<IPriceVolumeLayer, ISourceTickerQuoteInfo> 
    {
        protected override IPriceVolumeLayer SelectSimplePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new PriceVolumeLayer();
        }

        protected override IPriceVolumeLayer SelectValueDatePriceVolumeLayer(
            ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new ValueDatePriceVolumeLayer();
        }

        protected override IPriceVolumeLayer SelectSourcePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new SourcePriceVolumeLayer();
        }

        protected override IPriceVolumeLayer SelectSourceQuoteRefPriceVolumeLayer(
            ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new SourceQuoteRefPriceVolumeLayer();
        }

        protected override IPriceVolumeLayer SelectTraderPriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new TraderPriceVolumeLayer();
        }

        protected override IPriceVolumeLayer SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(
            ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new SourceQuoteRefTraderValueDatePriceVolumeLayer();
        }
        
        public override IPriceVolumeLayer ConvertToExpectedImplementation(IPriceVolumeLayer priceVolumeLayer, 
            bool clone = false)
        {
            if (priceVolumeLayer == null) return null;
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
                    return new PriceVolumeLayer(priceVolumeLayer);
            }
        }
    }
}