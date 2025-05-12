#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

public class PQSourcePriceVolumeLayerFactory : IPQOrderBookLayerFactory
{
    public PQSourcePriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) => NameIdLookup = nameIdLookup;

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQSourcePriceVolumeLayer(NameIdLookup);

    public virtual Type LayerCreationType => typeof(PQSourcePriceVolumeLayer);

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQSourcePriceVolumeLayer(original, NameIdLookup);
}
