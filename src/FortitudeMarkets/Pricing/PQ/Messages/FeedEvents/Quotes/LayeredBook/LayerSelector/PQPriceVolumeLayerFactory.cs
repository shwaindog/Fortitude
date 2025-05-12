#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

public class PQPriceVolumeLayerFactory : IPQOrderBookLayerFactory
{
    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; } = null!;

    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQPriceVolumeLayer();

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQPriceVolumeLayer(original);

    public virtual Type LayerCreationType => typeof(PQPriceVolumeLayer);
}
