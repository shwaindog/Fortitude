// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

public class OrderBookLayerFactorySelector : LayerFlagsSelector<IPriceVolumeLayer, ISourceTickerQuoteInfo>
{
    static OrderBookLayerFactorySelector()
    {
        foreach (var layerType in Enum.GetValues<LayerType>())
        {
            if (layerType == LayerType.None) continue;
            AllowedImplementations.Add(LayerFlagToImplementation(layerType).GetType());
        }
    }

    protected override IPriceVolumeLayer SelectSimplePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new PriceVolumeLayer();

    protected override IPriceVolumeLayer SelectValueDatePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new ValueDatePriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourcePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new SourcePriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourceQuoteRefPriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new SourceQuoteRefPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectTraderPriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new TraderPriceVolumeLayer();

    protected override IPriceVolumeLayer SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(ISourceTickerQuoteInfo sourceTickerQuoteInfo) =>
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
