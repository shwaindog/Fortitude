#region

using static FortitudeMarketsApi.Pricing.LayeredBook.LayerFlags;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public enum LayerType
{
    None
    , PriceVolume
    , SourcePriceVolume
    , SourceQuoteRefPriceVolume
    , TraderPriceVolume
    , SourceQuoteRefTraderValueDatePriceVolume
    , ValueDatePriceVolume
}

public static class LayerTypeExtensions
{
    public static LayerType MostCompactLayerType(this LayerFlags layerFlags)
    {
        return layerFlags switch
        {
            None => LayerType.None
            , _ when layerFlags.HasAnyOf(Price | Volume) &&
                     layerFlags.HasNoneOf(ValueDate | SourceQuoteReference | TraderCount | TraderName | TraderSize | SourceName | Executable) =>
                LayerType.PriceVolume
            , _ when layerFlags.HasValueDate() &&
                     layerFlags.HasNoneOf(SourceQuoteReference | TraderCount | TraderName | TraderSize | SourceName | Executable) =>
                LayerType.ValueDatePriceVolume
            , _ when layerFlags.HasAnyOf(SourceName | Executable) &&
                     layerFlags.HasNoneOf(SourceQuoteReference | TraderCount | TraderName | TraderSize | ValueDate) =>
                LayerType.SourcePriceVolume
            , _ when layerFlags.HasSourceQuoteReference() && layerFlags.HasNoneOf(TraderCount | TraderName | TraderSize | ValueDate) =>
                LayerType.SourceQuoteRefPriceVolume
            , _ when layerFlags.HasAnyOf(TraderCount | TraderName | TraderSize) &&
                     layerFlags.HasNoneOf(SourceName | Executable | SourceQuoteReference | ValueDate) => LayerType.TraderPriceVolume
            , _ => LayerType.SourceQuoteRefTraderValueDatePriceVolume
        };
    }
}
