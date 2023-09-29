using System;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    public class ValueDatePriceVolumeLayerFactory : IOrderBookLayerFactory
    {
        public virtual IPQPriceVolumeLayer CreateNewLayer()
        {
            return new PQValueDatePriceVolumeLayer();
        }

        public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original)
        {
            return new PQValueDatePriceVolumeLayer(original);
        }

        public virtual Type LayerCreationType => typeof(PQValueDatePriceVolumeLayer);
    }
}