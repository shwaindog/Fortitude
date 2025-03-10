// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using static FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerFlags;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

[JsonConverter(typeof(JsonStringEnumConverter<LayerType>))]
public enum LayerType
{
    PriceVolume
  , SourcePriceVolume
  , SourceQuoteRefPriceVolume
  , TraderPriceVolume
  , SourceQuoteRefTraderValueDatePriceVolume
  , ValueDatePriceVolume
  , None
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
