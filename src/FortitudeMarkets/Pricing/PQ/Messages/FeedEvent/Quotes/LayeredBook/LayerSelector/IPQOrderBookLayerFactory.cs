// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public interface IPQOrdersCountLayerFactory
{
    Type                LayerCreationType { get; }
    IPQPriceVolumeLayer CreateNewLayer();
    IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original);
}

public interface IPQOrderBookLayerFactory : ISupportsPQNameIdLookupGenerator
{
    Type                LayerCreationType { get; }
    IPQPriceVolumeLayer CreateNewLayer();
    IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original);
}
