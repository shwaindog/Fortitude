// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerSelector;

public class OrderBookLayerFactorySelector : LayerFlagsSelector<IPriceVolumeLayer>
{
    static OrderBookLayerFactorySelector()
    {
        foreach (var layerType in Enum.GetValues<LayerType>())
        {
            if (layerType == LayerType.None) continue;
            AllowedImplementations.Add(LayerFlagToImplementation(layerType).GetType());
        }
    }

    protected override IPriceVolumeLayer SelectSimplePriceVolumeLayer() => new PriceVolumeLayer();

    protected override IPriceVolumeLayer SelectValueDatePriceVolumeLayer() => new ValueDatePriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourcePriceVolumeLayer() => new SourcePriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourceQuoteRefPriceVolumeLayer() =>
        new SourceQuoteRefPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectOrdersCountPriceVolumeLayer() => new OrdersCountPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectAnonymousOrdersPriceVolumeLayer() => new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume);

    protected override IPriceVolumeLayer SelectCounterPartyOrdersPriceVolumeLayer () => new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume);

    protected override IPriceVolumeLayer SelectSourceQuoteRefTraderValueDatePriceVolumeLayer() =>
        new FullSupportPriceVolumeLayer();

    public override IPriceVolumeLayer CreateExpectedImplementation
    (LayerType desiredLayerType, IPriceVolumeLayer? copy = null
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var implementation = LayerFlagToImplementation(desiredLayerType);
        if (copy != null) implementation.CopyFrom(copy, copyMergeFlags);
        return implementation;
    }

    public static IPriceVolumeLayer LayerFlagToImplementation(LayerType desiredLayerType)
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
