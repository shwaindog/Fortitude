// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerSelector;

public class OrderBookLayerFactorySelector : LayerFlagsSelector<IPriceVolumeLayer, ISourceTickerInfo>
{
    static OrderBookLayerFactorySelector()
    {
        foreach (var layerType in Enum.GetValues<LayerType>())
        {
            if (layerType == LayerType.None) continue;
            AllowedImplementations.Add(LayerFlagToImplementation(layerType).GetType());
        }
    }

    protected override IPriceVolumeLayer SelectSimplePriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => new PriceVolumeLayer();

    protected override IPriceVolumeLayer SelectValueDatePriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => new ValueDatePriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourcePriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => new SourcePriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourceQuoteRefPriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) =>
        new SourceQuoteRefPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectOrdersCountPriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => new OrdersCountPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectAnonymousOrdersPriceVolumeLayer
        (ISourceTickerInfo sourceTickerInfo) =>
        new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume);

    protected override IPriceVolumeLayer SelectCounterPartyOrdersPriceVolumeLayer
        (ISourceTickerInfo sourceTickerInfo) =>
        new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume);

    protected override IPriceVolumeLayer SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) =>
        new SourceQuoteRefOrdersValueDatePriceVolumeLayer();

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

              , LayerType.SourceQuoteRefOrdersValueDatePriceVolume => new SourceQuoteRefOrdersValueDatePriceVolumeLayer()

              , _ => new PriceVolumeLayer()
            };
        return newLayer;
    }
}
