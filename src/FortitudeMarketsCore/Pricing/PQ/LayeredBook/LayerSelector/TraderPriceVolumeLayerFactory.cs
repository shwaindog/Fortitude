using System;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    public class TraderPriceVolumeLayerFactory : IOrderBookLayerFactory
    {
        public TraderPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup)
        {
            TraderNameIdLookup = nameIdLookup;
        }

        public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }

        public virtual IPQPriceVolumeLayer CreateNewLayer()
        {
            return new PQTraderPriceVolumeLayer(0, 0, TraderNameIdLookup);
        }

        public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original)
        {
            return new PQTraderPriceVolumeLayer(original);
        }

        public virtual Type LayerCreationType => typeof(PQTraderPriceVolumeLayer);
    }
}