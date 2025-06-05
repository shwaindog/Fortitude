// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

public class PQOrdersPriceVolumeLayerFactory : IPQOrderBookLayerFactory
{
    public PQOrdersPriceVolumeLayerFactory(LayerType layerType, IPQNameIdLookupGenerator nameIdLookup, QuoteLayerInstantBehaviorFlags layerBehavior)
    {
        LayerType     = layerType;
        NameIdLookup  = nameIdLookup;
        LayerBehavior = layerBehavior;
    }

    public LayerType LayerType { get; }

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public QuoteLayerInstantBehaviorFlags LayerBehavior { get; }

    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public virtual IPQPriceVolumeLayer CreateNewLayer() => new PQOrdersPriceVolumeLayer(LayerType, NameIdLookup, LayerBehavior);

    public virtual IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQOrdersPriceVolumeLayer(original, LayerType, NameIdLookup);

    public virtual Type LayerCreationType => typeof(PQOrdersPriceVolumeLayer);
}
