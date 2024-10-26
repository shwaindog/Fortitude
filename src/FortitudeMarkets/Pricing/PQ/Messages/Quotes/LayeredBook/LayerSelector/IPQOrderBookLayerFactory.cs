#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public interface IPQOrderBookLayerFactory : ISupportsPQNameIdLookupGenerator
{
    Type LayerCreationType { get; }
    IPQPriceVolumeLayer CreateNewLayer();
    IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original);
}
