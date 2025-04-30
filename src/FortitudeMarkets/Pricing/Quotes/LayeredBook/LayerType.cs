// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using static FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerFlags;
using static FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerFlagsExtensions;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

[JsonConverter(typeof(JsonStringEnumConverter<LayerType>))]
public enum LayerType
{
    PriceVolume
  , SourcePriceVolume
  , SourceQuoteRefPriceVolume
  , ValueDatePriceVolume
  , OrdersCountPriceVolume
  , OrdersAnonymousPriceVolume
  , OrdersFullPriceVolume
  , SourceQuoteRefOrdersValueDatePriceVolume
  , None
}

public static class LayerTypeExtensions
{
    public static LayerType MostCompactLayerType(this LayerFlags layerFlags)
    {
        return layerFlags switch
               {
                   None => LayerType.None
                 , _ when layerFlags.HasAnyOf(PriceVolumeLayerFlags) &&
                          layerFlags.HasNoneOf(ValueDate | SourceQuoteReference | AdditionSourceLayerFlags | AdditionalOrdersCountFlags
                                             | AdditionalCounterPartyOrderFlags) =>
                       LayerType.PriceVolume
                 , _ when layerFlags.HasValueDate() &&
                          layerFlags.HasNoneOf(SourceQuoteReference | AdditionSourceLayerFlags | AdditionalOrdersCountFlags
                                             | AdditionalCounterPartyOrderFlags) =>
                       LayerType.ValueDatePriceVolume
                 , _ when layerFlags.HasAnyOf(AdditionSourceLayerFlags) &&
                          layerFlags.HasNoneOf(ValueDate | SourceQuoteReference | AdditionalOrdersCountFlags | AdditionalCounterPartyOrderFlags) =>
                       LayerType.SourcePriceVolume
                 , _ when layerFlags.HasSourceQuoteReference() &&
                          layerFlags.HasNoneOf(ValueDate | AdditionalOrdersCountFlags | AdditionalCounterPartyOrderFlags) =>
                       LayerType.SourceQuoteRefPriceVolume
                 , _ when layerFlags.HasAnyOf(AdditionalOrdersCountFlags) &&
                          layerFlags.HasNoneOf(AdditionSourceLayerFlags | SourceQuoteReference | ValueDate | AdditionalCounterPartyOrderFlags) =>
                       LayerType.OrdersCountPriceVolume
                 , _ when layerFlags.HasAnyOf(AdditionalAnonymousOrderFlags) &&
                          layerFlags.HasNoneOf(AdditionSourceLayerFlags | SourceQuoteReference | ValueDate | OrderCounterPartyName | OrderTraderName)
                       => LayerType.OrdersAnonymousPriceVolume
                 , _ when layerFlags.HasAnyOf(OrderCounterPartyName | OrderTraderName) &&
                          layerFlags.HasNoneOf(AdditionSourceLayerFlags | SourceQuoteReference | ValueDate) =>
                       LayerType.OrdersFullPriceVolume
                 , _ => LayerType.SourceQuoteRefOrdersValueDatePriceVolume
               };
    }

    public static bool IsJustPriceVolume(this LayerType layerType)                => layerType == LayerType.PriceVolume;
    public static bool IsJustSourcePriceVolume(this LayerType layerType)          => layerType == LayerType.SourcePriceVolume;
    public static bool IsJustSourceQuoteRefPriceVolume(this LayerType layerType)  => layerType == LayerType.SourceQuoteRefPriceVolume;
    public static bool IsJustValueDatePriceVolume(this LayerType layerType)       => layerType == LayerType.ValueDatePriceVolume;
    public static bool IsJustOrdersCountPriceVolume(this LayerType layerType)     => layerType == LayerType.OrdersCountPriceVolume;
    public static bool IsJustOrdersAnonymousPriceVolume(this LayerType layerType) => layerType == LayerType.OrdersAnonymousPriceVolume;
    public static bool IsJustOrdersFullPriceVolume(this LayerType layerType)      => layerType == LayerType.OrdersFullPriceVolume;

    public static bool IsSourceQuoteRefOrdersValueDatePriceVolume
        (this LayerType layerType) =>
        layerType == LayerType.SourceQuoteRefOrdersValueDatePriceVolume;

    public static bool SupportsSourcePriceVolume
        (this LayerType layerType) =>
        layerType.IsJustSourcePriceVolume() || layerType.IsSourceQuoteRefOrdersValueDatePriceVolume();

    public static bool SupportsSourceQuoteRefPriceVolume
        (this LayerType layerType) =>
        layerType.IsJustSourceQuoteRefPriceVolume() || layerType.IsSourceQuoteRefOrdersValueDatePriceVolume();

    public static bool SupportsValueDatePriceVolume
        (this LayerType layerType) =>
        layerType.IsJustValueDatePriceVolume() || layerType.IsSourceQuoteRefOrdersValueDatePriceVolume();

    public static bool SupportsOrdersCountPriceVolume
        (this LayerType layerType) =>
        layerType.IsJustOrdersCountPriceVolume() || layerType.IsSourceQuoteRefOrdersValueDatePriceVolume();

    public static bool SupportsOrdersAnonymousPriceVolume
        (this LayerType layerType) =>
        layerType.IsJustOrdersAnonymousPriceVolume() || layerType.IsSourceQuoteRefOrdersValueDatePriceVolume();

    public static bool SupportsOrdersFullPriceVolume
        (this LayerType layerType) =>
        layerType.IsJustOrdersFullPriceVolume() || layerType.IsSourceQuoteRefOrdersValueDatePriceVolume();
}
