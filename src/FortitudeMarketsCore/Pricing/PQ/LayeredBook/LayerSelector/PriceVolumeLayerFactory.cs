using System;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    public class PriceVolumeLayerFactory : IOrderBookLayerFactory
    {
        public virtual IPQPriceVolumeLayer CreateNewLayer()
        {
            return new PQPriceVolumeLayer();
        }

        public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original)
        {
            return new PQPriceVolumeLayer(original);
        }

        public virtual Type LayerCreationType => typeof(PQPriceVolumeLayer);
    }
}