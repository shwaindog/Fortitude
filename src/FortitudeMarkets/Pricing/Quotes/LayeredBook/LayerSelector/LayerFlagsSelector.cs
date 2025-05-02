// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerSelector;

public interface ILayerFlagsSelector<T, Tu> where T : class where Tu : ISourceTickerInfo
{
    bool OriginalCanWhollyContain(LayerFlags copySourceRequiredFlags, LayerFlags copyDestinationSupportedFlags);

    T FindForLayerFlags(Tu sourceTickerInfo);

    IPriceVolumeLayer CreateExpectedImplementation
    (LayerType desiredLayerType, IPriceVolumeLayer? copy = null,
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    IPriceVolumeLayer UpgradeExistingLayer
    (IPriceVolumeLayer? original, LayerType desiredLayerType,
        IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public abstract class LayerFlagsSelector<T, Tu> : ILayerFlagsSelector<T, Tu>
    where T : class
    where Tu : ISourceTickerInfo
{
    protected static readonly List<Type> AllowedImplementations = new();

    public T FindForLayerFlags(Tu sourceTickerInfo)
    {
        var layerFlags           = sourceTickerInfo.LayerFlags;
        var mostCompactLayerType = layerFlags.MostCompactLayerType();
        return mostCompactLayerType switch
               {
                   LayerType.PriceVolume                => SelectSimplePriceVolumeLayer(sourceTickerInfo)
                 , LayerType.SourcePriceVolume          => SelectSourcePriceVolumeLayer(sourceTickerInfo)
                 , LayerType.SourceQuoteRefPriceVolume  => SelectSourceQuoteRefPriceVolumeLayer(sourceTickerInfo)
                 , LayerType.ValueDatePriceVolume       => SelectValueDatePriceVolumeLayer(sourceTickerInfo)
                 , LayerType.OrdersCountPriceVolume     => SelectOrdersCountPriceVolumeLayer(sourceTickerInfo)
                 , LayerType.OrdersAnonymousPriceVolume => SelectAnonymousOrdersPriceVolumeLayer(sourceTickerInfo)
                 , LayerType.OrdersFullPriceVolume      => SelectCounterPartyOrdersPriceVolumeLayer(sourceTickerInfo)

                 , LayerType.SourceQuoteRefOrdersValueDatePriceVolume => SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(sourceTickerInfo)

                 , _ => SelectSimplePriceVolumeLayer(sourceTickerInfo)
               };
    }

    public bool OriginalCanWhollyContain(LayerFlags copySourceRequiredFlags, LayerFlags copyDestinationSupportedFlags) =>
        copyDestinationSupportedFlags.HasAllOf(copySourceRequiredFlags);

    public virtual IPriceVolumeLayer UpgradeExistingLayer
    (IPriceVolumeLayer? original, LayerType desiredLayerType,
        IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (original == null)
        {
            var cloneOfSrc = CreateExpectedImplementation(desiredLayerType);
            if (copy != null) cloneOfSrc.CopyFrom(copy, copyMergeFlags);
            return cloneOfSrc;
        }

        if ((original.LayerType != desiredLayerType && !OriginalCanWhollyContain(desiredLayerType.SupportedLayerFlags(), original.SupportsLayerFlags))
         || !AllowedImplementations.Contains(original.GetType()))
        {
            var mergeOrginalDesiredLayerFlags = original.SupportsLayerFlags | desiredLayerType.SupportedLayerFlags();
            var mostCompatibleSupportsBoth    = mergeOrginalDesiredLayerFlags.MostCompactLayerType();
            var upgradedLayer                 = CreateExpectedImplementation(mostCompatibleSupportsBoth);
            upgradedLayer.CopyFrom(original);
            if (copy != null) upgradedLayer.CopyFrom(copy, copyMergeFlags);
            return upgradedLayer;
        }

        if (copy != null) original.CopyFrom(copy, copyMergeFlags);
        return original;
    }

    public abstract IPriceVolumeLayer CreateExpectedImplementation
    (LayerType desiredLayerType, IPriceVolumeLayer? copy = null,
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    protected abstract T SelectSimplePriceVolumeLayer(Tu sourceTickerInfo);
    protected abstract T SelectValueDatePriceVolumeLayer(Tu sourceTickerInfo);
    protected abstract T SelectSourcePriceVolumeLayer(Tu sourceTickerInfo);
    protected abstract T SelectSourceQuoteRefPriceVolumeLayer(Tu sourceTickerInfo);
    protected abstract T SelectOrdersCountPriceVolumeLayer(Tu sourceTickerInfo);
    protected abstract T SelectAnonymousOrdersPriceVolumeLayer(Tu sourceTickerInfo);
    protected abstract T SelectCounterPartyOrdersPriceVolumeLayer(Tu sourceTickerInfo);
    protected abstract T SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(Tu sourceTickerInfo);
}
