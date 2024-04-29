#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQTraderPriceVolumeLayerFactory : IPQOrderBookLayerFactory
{
    public PQTraderPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) => NameIdLookup = nameIdLookup;

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQTraderPriceVolumeLayer(NameIdLookup, 0);

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQTraderPriceVolumeLayer(original, NameIdLookup);

    public virtual Type LayerCreationType => typeof(PQTraderPriceVolumeLayer);
}
