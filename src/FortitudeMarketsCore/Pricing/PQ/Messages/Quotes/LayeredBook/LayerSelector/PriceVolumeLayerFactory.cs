namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PriceVolumeLayerFactory : IOrderBookLayerFactory
{
    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQPriceVolumeLayer();

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQPriceVolumeLayer(original);

    public virtual Type LayerCreationType => typeof(PQPriceVolumeLayer);
}
