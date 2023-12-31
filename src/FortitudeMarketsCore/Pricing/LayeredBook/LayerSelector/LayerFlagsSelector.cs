﻿#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector;

public abstract class LayerFlagsSelector<T, Tu> : ILayerFlagsSelector<T, Tu>
    where T : class
    where Tu : ISourceTickerQuoteInfo
{
    public T FindForLayerFlags(Tu sourceTickerQuoteInfo)
    {
        var layerFlags = sourceTickerQuoteInfo.LayerFlags;
        const LayerFlags priceVolumeFlags = LayerFlags.Price | LayerFlags.Volume;
        var testCondition = priceVolumeFlags;

        var onlyPriceVolume = (layerFlags & testCondition) == layerFlags;
        if (onlyPriceVolume) return SelectSimplePriceVolumeLayer(sourceTickerQuoteInfo);

        testCondition |= LayerFlags.ValueDate;
        var onlyValueDatePriceVolumeLayer = (layerFlags & testCondition) == layerFlags;
        if (onlyValueDatePriceVolumeLayer) return SelectValueDatePriceVolumeLayer(sourceTickerQuoteInfo);

        testCondition = priceVolumeFlags | LayerFlags.SourceName | LayerFlags.Executable;
        var onlySourcePriceVolume = (layerFlags & testCondition) == layerFlags;
        if (onlySourcePriceVolume) return SelectSourcePriceVolumeLayer(sourceTickerQuoteInfo);

        testCondition |= LayerFlags.SourceQuoteReference;
        var onlySourceQuotRefPriceVolume = (layerFlags & testCondition) == layerFlags;
        if (onlySourceQuotRefPriceVolume) return SelectSourceQuoteRefPriceVolumeLayer(sourceTickerQuoteInfo);

        testCondition = priceVolumeFlags | LayerFlags.TraderCount;
        var traderCtPriceVolume = (layerFlags & testCondition) == layerFlags;
        testCondition |= priceVolumeFlags | LayerFlags.TraderName;
        var traderName = (layerFlags & testCondition) == layerFlags;
        testCondition |= priceVolumeFlags | LayerFlags.TraderSize;
        var traderSize = (layerFlags & testCondition) == layerFlags;
        if (traderCtPriceVolume || traderName || traderSize) return SelectTraderPriceVolumeLayer(sourceTickerQuoteInfo);

        return SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(sourceTickerQuoteInfo);
    }

    public abstract IPriceVolumeLayer ConvertToExpectedImplementation(IPriceVolumeLayer? priceVolumeLayer,
        bool clone = false);

    protected abstract T SelectSimplePriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectValueDatePriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectSourcePriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectSourceQuoteRefPriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectTraderPriceVolumeLayer(Tu sourceTickerQuoteInfo);
    protected abstract T SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(Tu sourceTickerQuoteInfo);
}
