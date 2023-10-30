using System;
using FortitudeCommon.Chronometry;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    public class SourceQuoteRefTraderValueDatePriceVolumeLayerFactory : TraderPriceVolumeLayerFactory
    {
        public SourceQuoteRefTraderValueDatePriceVolumeLayerFactory(
            IPQNameIdLookupGenerator sourceNameIdLookup, IPQNameIdLookupGenerator traderNameIdLookupGenerator) 
            : base(traderNameIdLookupGenerator)
        {
            SourceNameIdLookup = sourceNameIdLookup;
        }

        public IPQNameIdLookupGenerator SourceNameIdLookup { get; set; }

        public override IPQPriceVolumeLayer CreateNewLayer()
        {
            return new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(0, 0, SourceNameIdLookup, 
                TraderNameIdLookup);
        }

        public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original)
        {
            return new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(original);
        }

        public override Type LayerCreationType => typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
    }
}