// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

public class PQFullSupportPriceVolumeLayerFactory : PQOrdersPriceVolumeLayerFactory
{
    public PQFullSupportPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup)
        : base(LayerType.FullSupportPriceVolume, nameIdLookup) { }

    public override Type LayerCreationType => typeof(PQFullSupportPriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() => new PQFullSupportPriceVolumeLayer(NameIdLookup, 0, 0);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) =>
        new PQFullSupportPriceVolumeLayer(original, NameIdLookup);
}
