// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;

public class OrderBookLayerFactorySelector : LayerFlagsSelector<IMutablePriceVolumeLayer>
{
    static OrderBookLayerFactorySelector()
    {
        foreach (var layerType in Enum.GetValues<LayerType>())
        {
            if (layerType == LayerType.None) continue;
            AllowedImplementations.Add(LayerFlagToImplementation(layerType).GetType());
        }
    }

    protected override IMutablePriceVolumeLayer SelectSimplePriceVolumeLayer() => new PriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectValueDatePriceVolumeLayer() => new ValueDatePriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectSourcePriceVolumeLayer() => new SourcePriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectSourceQuoteRefPriceVolumeLayer() =>
        new SourceQuoteRefPriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectOrdersCountPriceVolumeLayer() => new OrdersCountPriceVolumeLayer();

    protected override IMutablePriceVolumeLayer SelectAnonymousOrdersPriceVolumeLayer() => new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume);

    protected override IMutablePriceVolumeLayer SelectCounterPartyOrdersPriceVolumeLayer () => new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume);

    protected override IMutablePriceVolumeLayer SelectSourceQuoteRefTraderValueDatePriceVolumeLayer() =>
        new FullSupportPriceVolumeLayer();

    public override IMutablePriceVolumeLayer CreateExpectedImplementation
    (LayerType desiredLayerType, IPriceVolumeLayer? copy = null
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var implementation = LayerFlagToImplementation(desiredLayerType);
        if (copy != null) implementation.CopyFrom(copy, copyMergeFlags);
        return implementation;
    }

    public static IMutablePriceVolumeLayer LayerFlagToImplementation(LayerType desiredLayerType)
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
