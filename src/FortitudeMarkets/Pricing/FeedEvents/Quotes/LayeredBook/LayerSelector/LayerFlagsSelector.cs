// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;

public interface ILayerFlagsSelector<out T> where T : class
{
    bool OriginalCanWhollyContain(LayerFlags copySourceRequiredFlags, LayerFlags copyDestinationSupportedFlags);

    T FindForLayerFlags(ISourceTickerInfo sourceTickerInfo);
    T FindForLayerFlags(LayerFlags layerFlags, QuoteLayerInstantBehaviorFlags layerBehavior);

    IMutablePriceVolumeLayer CreateExpectedImplementation
    (LayerType desiredLayerType, QuoteLayerInstantBehaviorFlags layerBehavior, IPriceVolumeLayer? copy = null,
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    IMutablePriceVolumeLayer UpgradeExistingLayer
    (IPriceVolumeLayer? original, LayerType desiredLayerType, QuoteLayerInstantBehaviorFlags layerBehavior
       , IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public abstract class LayerFlagsSelector<T> : ILayerFlagsSelector<T> where T : class
{
    protected static readonly List<Type> AllowedImplementations = new();

    public T FindForLayerFlags(ISourceTickerInfo sourceTickerInfo)
    {
        return FindForLayerFlags(sourceTickerInfo.LayerFlags, (QuoteLayerInstantBehaviorFlags)sourceTickerInfo.QuoteBehaviorFlags);
    }

    public T FindForLayerFlags(LayerFlags layerFlags, QuoteLayerInstantBehaviorFlags layerBehavior)
    {
        var mostCompactLayerType = layerFlags.MostCompactLayerType();
        return mostCompactLayerType switch
               {
                   LayerType.PriceVolume                => SelectSimplePriceVolumeLayer()
                 , LayerType.SourcePriceVolume          => SelectSourcePriceVolumeLayer()
                 , LayerType.SourceQuoteRefPriceVolume  => SelectSourceQuoteRefPriceVolumeLayer()
                 , LayerType.ValueDatePriceVolume       => SelectValueDatePriceVolumeLayer()
                 , LayerType.OrdersCountPriceVolume     => SelectOrdersCountPriceVolumeLayer()
                 , LayerType.OrdersAnonymousPriceVolume => SelectAnonymousOrdersPriceVolumeLayer(layerBehavior)
                 , LayerType.OrdersFullPriceVolume      => SelectCounterPartyOrdersPriceVolumeLayer(layerBehavior)
                 , LayerType.FullSupportPriceVolume => SelectFullSupportPriceVolumeLayer(layerBehavior)

                 , _ => SelectSimplePriceVolumeLayer()
               };
    }

    public bool OriginalCanWhollyContain(LayerFlags copySourceRequiredFlags, LayerFlags copyDestinationSupportedFlags) =>
        copyDestinationSupportedFlags.HasAllOf(copySourceRequiredFlags);

    public virtual IMutablePriceVolumeLayer UpgradeExistingLayer
    (IPriceVolumeLayer? original, LayerType desiredLayerType, QuoteLayerInstantBehaviorFlags layerBehavior
       , IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (original == null)
        {
            var cloneOfSrc = CreateExpectedImplementation(desiredLayerType, layerBehavior);
            if (copy != null) cloneOfSrc.CopyFrom(copy, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);
            return cloneOfSrc;
        }

        if ((original.LayerType != desiredLayerType && !OriginalCanWhollyContain(desiredLayerType.SupportedLayerFlags(), original.SupportsLayerFlags))
         || !AllowedImplementations.Contains(original.GetType()))
        {
            var mergeOriginalDesiredLayerFlags = original.SupportsLayerFlags | desiredLayerType.SupportedLayerFlags();
            var mostCompatibleSupportsBoth    = mergeOriginalDesiredLayerFlags.MostCompactLayerType();
            var upgradedLayer                 = CreateExpectedImplementation(mostCompatibleSupportsBoth, layerBehavior);
            upgradedLayer.CopyFrom(original, (QuoteInstantBehaviorFlags)layerBehavior);
            if (copy != null) upgradedLayer.CopyFrom(copy, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);
            return upgradedLayer;
        }

        if (copy != null) original.CopyFrom(copy, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);
        return (IMutablePriceVolumeLayer)original;
    }

    public abstract IMutablePriceVolumeLayer CreateExpectedImplementation
    (LayerType desiredLayerType, QuoteLayerInstantBehaviorFlags layerBehavior, IPriceVolumeLayer? copy = null,
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    protected abstract T SelectSimplePriceVolumeLayer();
    protected abstract T SelectValueDatePriceVolumeLayer();
    protected abstract T SelectSourcePriceVolumeLayer();
    protected abstract T SelectSourceQuoteRefPriceVolumeLayer();
    protected abstract T SelectOrdersCountPriceVolumeLayer();
    protected abstract T SelectAnonymousOrdersPriceVolumeLayer(QuoteLayerInstantBehaviorFlags layerBehavior);
    protected abstract T SelectCounterPartyOrdersPriceVolumeLayer(QuoteLayerInstantBehaviorFlags layerBehavior);
    protected abstract T SelectFullSupportPriceVolumeLayer(QuoteLayerInstantBehaviorFlags layerBehavior);
}
