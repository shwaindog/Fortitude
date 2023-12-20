namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;

public class PriceVolumeLayerFactory : IOrderBookLayerFactory
{
    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQPriceVolumeLayer();

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQPriceVolumeLayer(original);

    public virtual Type LayerCreationType => typeof(PQPriceVolumeLayer);
}
