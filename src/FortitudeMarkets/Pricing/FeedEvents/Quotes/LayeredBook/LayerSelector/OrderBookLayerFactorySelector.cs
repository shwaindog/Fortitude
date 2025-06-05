// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;

public class OrderBookLayerFactorySelector : LayerFlagsSelector<IMutablePriceVolumeLayer>
{
    static OrderBookLayerFactorySelector()
    {
        foreach (var layerType in Enum.GetValues<LayerType>())
        {
            if (layerType == LayerType.None) continue;
            AllowedImplementations.Add(LayerFlagToImplementation(layerType, QuoteInstantBehaviorFlags.None).GetType());
        }
    }

    protected override IMutablePriceVolumeLayer SelectSimplePriceVolumeLayer() => new PriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectValueDatePriceVolumeLayer() => new ValueDatePriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectSourcePriceVolumeLayer() => new SourcePriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectSourceQuoteRefPriceVolumeLayer() =>
        new SourceQuoteRefPriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectOrdersCountPriceVolumeLayer() => new OrdersCountPriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectAnonymousOrdersPriceVolumeLayer(QuoteLayerInstantBehaviorFlags layerBehavior) =>
        new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, layerBehavior);

    protected override IMutablePriceVolumeLayer SelectCounterPartyOrdersPriceVolumeLayer (QuoteLayerInstantBehaviorFlags layerBehavior) => 
        new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, layerBehavior);

    protected override IMutablePriceVolumeLayer SelectFullSupportPriceVolumeLayer(QuoteLayerInstantBehaviorFlags layerBehavior) =>
        new FullSupportPriceVolumeLayer(layerBehavior);

    public override IMutablePriceVolumeLayer CreateExpectedImplementation
    (LayerType desiredLayerType, QuoteLayerInstantBehaviorFlags layerBehavior, IPriceVolumeLayer? copy = null
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var implementation = LayerFlagToImplementation(desiredLayerType, (QuoteInstantBehaviorFlags)layerBehavior);
        if (copy != null) implementation.CopyFrom(copy, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);
        return implementation;
    }

    public static IMutablePriceVolumeLayer LayerFlagToImplementation(LayerType desiredLayerType, QuoteInstantBehaviorFlags quoteBehavior)
    {
        var newLayer =
            desiredLayerType switch
            {
                LayerType.PriceVolume                => new PriceVolumeLayer()
              , LayerType.SourcePriceVolume          => new SourcePriceVolumeLayer()
              , LayerType.SourceQuoteRefPriceVolume  => new SourceQuoteRefPriceVolumeLayer()
              , LayerType.ValueDatePriceVolume       => new ValueDatePriceVolumeLayer()
              , LayerType.OrdersCountPriceVolume     => new OrdersCountPriceVolumeLayer()
              , LayerType.OrdersAnonymousPriceVolume => new OrdersPriceVolumeLayer(desiredLayerType)
              , LayerType.OrdersFullPriceVolume      => new OrdersPriceVolumeLayer(desiredLayerType)

              , LayerType.FullSupportPriceVolume => new FullSupportPriceVolumeLayer()

              , _ => new PriceVolumeLayer()
            };
        return newLayer;
    }
}
