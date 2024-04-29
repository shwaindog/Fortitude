#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQSourcePriceVolumeLayerFactory : IPQOrderBookLayerFactory
{
    public PQSourcePriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) => NameIdLookup = nameIdLookup;

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQSourcePriceVolumeLayer(NameIdLookup);

    public virtual Type LayerCreationType => typeof(PQSourcePriceVolumeLayer);

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQSourcePriceVolumeLayer(original, NameIdLookup);
}
