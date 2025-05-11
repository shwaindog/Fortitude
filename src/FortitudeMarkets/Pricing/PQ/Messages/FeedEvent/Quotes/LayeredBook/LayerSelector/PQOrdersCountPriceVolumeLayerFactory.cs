// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQOrdersCountPriceVolumeLayerFactory : PQPriceVolumeLayerFactory
{
    public override Type LayerCreationType => typeof(PQOrdersCountPriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() => new PQOrdersCountPriceVolumeLayer();

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQOrdersCountPriceVolumeLayer(original);
}
