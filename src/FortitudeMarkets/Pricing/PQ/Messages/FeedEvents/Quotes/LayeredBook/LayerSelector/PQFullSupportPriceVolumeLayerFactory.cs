// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

public class PQFullSupportPriceVolumeLayerFactory : PQOrdersPriceVolumeLayerFactory
{
    public PQFullSupportPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup, QuoteLayerInstantBehaviorFlags layerBehavior)
        : base(LayerType.FullSupportPriceVolume, nameIdLookup, layerBehavior) { }

    public override Type LayerCreationType => typeof(PQFullSupportPriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() => 
        new PQFullSupportPriceVolumeLayer(NameIdLookup, LayerBehavior);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) =>
        new PQFullSupportPriceVolumeLayer(original, NameIdLookup);
}
