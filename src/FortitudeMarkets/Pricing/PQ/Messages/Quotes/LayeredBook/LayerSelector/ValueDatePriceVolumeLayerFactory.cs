#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class ValueDatePriceVolumeLayerFactory : IPQOrderBookLayerFactory
{
    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQValueDatePriceVolumeLayer();

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; } = null!;

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQValueDatePriceVolumeLayer(original);

    public virtual Type LayerCreationType => typeof(PQValueDatePriceVolumeLayer);
}
