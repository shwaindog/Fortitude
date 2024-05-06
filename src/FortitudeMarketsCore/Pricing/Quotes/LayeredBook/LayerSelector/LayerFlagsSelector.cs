#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

public interface ILayerFlagsSelector<T, Tu> where T : class where Tu : ISourceTickerQuoteInfo
{
    bool OriginalCanWhollyContain(LayerFlags copySourceRequiredFlags, LayerFlags copyDestinationSupportedFlags);
    T FindForLayerFlags(Tu sourceTickerQuoteInfo);

    IPriceVolumeLayer CreateExpectedImplementation(LayerType desiredLayerType, IPriceVolumeLayer? copy = null,
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    IPriceVolumeLayer UpgradeExistingLayer(IPriceVolumeLayer? original, LayerType desiredLayerType,
        IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public abstract class LayerFlagsSelector<T, Tu> : ILayerFlagsSelector<T, Tu>
    where T : class
    where Tu : ISourceTickerQuoteInfo
{
    protected static readonly List<Type> AllowedImplementations = new();

    public T FindForLayerFlags(Tu sourceTickerQuoteInfo)
    {
        var layerFlags = sourceTickerQuoteInfo.LayerFlags;
        var mostCompactLayerType = layerFlags.MostCompactLayerType();
        return mostCompactLayerType switch
        {
            LayerType.PriceVolume => SelectSimplePriceVolumeLayer(sourceTickerQuoteInfo)
            , LayerType.SourceQuoteRefTraderValueDatePriceVolume => SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(sourceTickerQuoteInfo)
            , LayerType.TraderPriceVolume => SelectTraderPriceVolumeLayer(sourceTickerQuoteInfo)
            , LayerType.ValueDatePriceVolume => SelectValueDatePriceVolumeLayer(sourceTickerQuoteInfo)
            , LayerType.SourceQuoteRefPriceVolume => SelectSourceQuoteRefPriceVolumeLayer(sourceTickerQuoteInfo)
            , LayerType.SourcePriceVolume => SelectSourcePriceVolumeLayer(sourceTickerQuoteInfo)
            , _ => SelectSimplePriceVolumeLayer(sourceTickerQuoteInfo)
        };
    }

    public bool OriginalCanWhollyContain(LayerFlags copySourceRequiredFlags, LayerFlags copyDestinationSupportedFlags) =>
        copyDestinationSupportedFlags.HasAllOf(copySourceRequiredFlags);

    public virtual IPriceVolumeLayer UpgradeExistingLayer(IPriceVolumeLayer? original, LayerType desiredLayerType,
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
            var mostCompatibleSupportsBoth = mergeOrginalDesiredLayerFlags.MostCompactLayerType();
            var upgradedLayer = CreateExpectedImplementation(mostCompatibleSupportsBoth);
            upgradedLayer.CopyFrom(original);
            if (copy != null) upgradedLayer.CopyFrom(copy, copyMergeFlags);
            return upgradedLayer;
        }

        if (copy != null) original.CopyFrom(copy, copyMergeFlags);
        return original;
    }

    public abstract IPriceVolumeLayer CreateExpectedImplementation(LayerType desiredLayerType, IPriceVolumeLayer? copy = null,
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    protected abstract T SelectSimplePriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectValueDatePriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectSourcePriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectSourceQuoteRefPriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectTraderPriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(Tu sourceTickerQuoteInfo);
}
