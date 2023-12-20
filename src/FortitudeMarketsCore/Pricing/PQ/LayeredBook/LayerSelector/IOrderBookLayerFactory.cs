namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;

public interface IOrderBookLayerFactory
{
    Type LayerCreationType { get; }
    IPQPriceVolumeLayer CreateNewLayer();
    IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original);
}
