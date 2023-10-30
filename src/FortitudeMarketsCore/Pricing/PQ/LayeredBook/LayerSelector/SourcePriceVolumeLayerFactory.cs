using System;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    public class SourcePriceVolumeLayerFactory : IOrderBookLayerFactory
    {
        public SourcePriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup)
        {
            SourceNameIdLookup = nameIdLookup;
        }

        public IPQNameIdLookupGenerator SourceNameIdLookup { get; set; }

        public virtual IPQPriceVolumeLayer CreateNewLayer()
        {
            return new PQSourcePriceVolumeLayer(0, 0, SourceNameIdLookup);
        }

        public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original)
        {
            return new PQSourcePriceVolumeLayer(original);
        }

        public virtual Type LayerCreationType => typeof(PQSourcePriceVolumeLayer);
    }
}