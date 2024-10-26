// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

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

    protected override IPriceVolumeLayer SelectTraderPriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) => new TraderPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(ISourceTickerInfo sourceTickerInfo) =>
        new SourceQuoteRefTraderValueDatePriceVolumeLayer();

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
                LayerType.PriceVolume => new PriceVolumeLayer()

              , LayerType.SourceQuoteRefTraderValueDatePriceVolume => new SourceQuoteRefTraderValueDatePriceVolumeLayer()

              , LayerType.SourceQuoteRefPriceVolume => new SourceQuoteRefPriceVolumeLayer()
              , LayerType.SourcePriceVolume         => new SourcePriceVolumeLayer()
              , LayerType.ValueDatePriceVolume      => new ValueDatePriceVolumeLayer()
              , LayerType.TraderPriceVolume         => new TraderPriceVolumeLayer()

              , _ => new PriceVolumeLayer()
            };
        return newLayer;
    }
}
