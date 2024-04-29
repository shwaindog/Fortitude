#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQPriceVolumeLayerFactory : IPQOrderBookLayerFactory
{
    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; } = null!;

    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQPriceVolumeLayer();

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQPriceVolumeLayer(original);

    public virtual Type LayerCreationType => typeof(PQPriceVolumeLayer);
}
