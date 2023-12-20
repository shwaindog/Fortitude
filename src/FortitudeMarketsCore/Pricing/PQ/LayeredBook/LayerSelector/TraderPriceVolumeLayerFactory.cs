#region

using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;

public class TraderPriceVolumeLayerFactory : IOrderBookLayerFactory
{
    public TraderPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) => TraderNameIdLookup = nameIdLookup;

    public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }

    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQTraderPriceVolumeLayer(0, 0, TraderNameIdLookup);

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) =>
        new PQTraderPriceVolumeLayer(original);

    public virtual Type LayerCreationType => typeof(PQTraderPriceVolumeLayer);
}
