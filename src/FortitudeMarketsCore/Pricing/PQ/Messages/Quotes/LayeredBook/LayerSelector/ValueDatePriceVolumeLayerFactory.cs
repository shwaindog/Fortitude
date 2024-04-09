namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class ValueDatePriceVolumeLayerFactory : IOrderBookLayerFactory
{
    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQValueDatePriceVolumeLayer();

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQValueDatePriceVolumeLayer(original);

    public virtual Type LayerCreationType => typeof(PQValueDatePriceVolumeLayer);
}
