using System;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    public class SourceQuoteRefPriceVolumeLayerFactory : SourcePriceVolumeLayerFactory
    {
        public SourceQuoteRefPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) : base(nameIdLookup) { }
        public override IPQPriceVolumeLayer CreateNewLayer()
        {
            return new PQSourceQuoteRefPriceVolumeLayer(0, 0, SourceNameIdLookup);
        }

        public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original)
        {
            return new PQSourceQuoteRefPriceVolumeLayer(original);
        }

        public override Type LayerCreationType => typeof(PQSourceQuoteRefPriceVolumeLayer);
    }
}