// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter<LayerFlags>))]
public enum LayerFlags : uint
{
    None                  = 0x00_00
  , Ladder                = 0x00_01
  , Price                 = 0x00_02
  , Volume                = 0x00_04
  , SourceQuoteReference  = 0x00_08
  , SourceName            = 0x00_10
  , ValueDate             = 0x00_20
  , Executable            = 0x00_40
  , InternalVolume        = 0x00_80
  , OrdersCount           = 0x01_00
  , OrderId               = 0x02_00
  , OrderCreated          = 0x04_00
  , OrderUpdated          = 0x08_00
  , OrderSize             = 0x10_00
  , OrderRemainingSize    = 0x20_00
  , OrderCounterPartyName = 0x40_00
  , OrderTraderName       = 0x80_00
}

public static class LayerFlagsExtensions
{
    public const LayerFlags PriceVolumeLayerFlags = LayerFlags.Price | LayerFlags.Volume;

    public const LayerFlags AdditionSourceLayerFlags = LayerFlags.SourceName | LayerFlags.Executable;

    public const LayerFlags AdditionalSourceQuoteRefFlags = LayerFlags.SourceQuoteReference;

    public const LayerFlags AdditionalValueDateFlags = LayerFlags.ValueDate;

    public const LayerFlags AdditionalOrdersCountFlags = LayerFlags.OrdersCount | LayerFlags.InternalVolume;

    public const LayerFlags AdditionalAnonymousOrderFlags =
        LayerFlags.OrderId | LayerFlags.OrderCreated | LayerFlags.OrderUpdated | LayerFlags.OrderSize | LayerFlags.OrderRemainingSize;

    public const LayerFlags AdditionalCounterPartyOrderFlags =
        LayerFlags.OrderCounterPartyName | LayerFlags.OrderTraderName | AdditionalAnonymousOrderFlags;

    public const LayerFlags AdditionalFullSupportLayerFlags =
        AdditionalValueDateFlags | AdditionalSourceQuoteRefFlags | AdditionSourceLayerFlags;

    public const LayerFlags FullSourceFlags =
        AdditionSourceLayerFlags | PriceVolumeLayerFlags;

    public const LayerFlags FullSourceQuoteRefFlags =
        AdditionalSourceQuoteRefFlags | FullSourceFlags;

    public const LayerFlags FullValueDateFlags =
        AdditionalValueDateFlags | PriceVolumeLayerFlags;

    public const LayerFlags FullOrdersCountFlags =
        AdditionalOrdersCountFlags | PriceVolumeLayerFlags;

    public const LayerFlags FullAnonymousOrderFlags =
        AdditionalAnonymousOrderFlags | FullOrdersCountFlags;

    public const LayerFlags FullCounterPartyOrdersFlags =
        AdditionalCounterPartyOrderFlags | FullAnonymousOrderFlags;

    public const LayerFlags FullSupportLayerFlags =
        AdditionalFullSupportLayerFlags | FullCounterPartyOrdersFlags;

    public static LayerFlags SupportedLayerFlags(this LayerType layerType)
    {
        switch (layerType)
        {
            case LayerType.PriceVolume:                              return PriceVolumeLayerFlags;
            case LayerType.ValueDatePriceVolume:                     return FullValueDateFlags;
            case LayerType.SourcePriceVolume:                        return FullSourceFlags;
            case LayerType.SourceQuoteRefPriceVolume:                return FullSourceQuoteRefFlags;
            case LayerType.OrdersCountPriceVolume:                   return FullOrdersCountFlags;
            case LayerType.OrdersAnonymousPriceVolume:               return FullAnonymousOrderFlags;
            case LayerType.OrdersFullPriceVolume:                    return FullCounterPartyOrdersFlags;

            case LayerType.FullSupportPriceVolume: return FullSupportLayerFlags;

            default:                                                 return LayerFlags.None;
        }
    }

    public static bool HasLadder(this LayerFlags flags)                => (flags & LayerFlags.Ladder) > 0;
    public static bool HasPrice(this LayerFlags flags)                 => (flags & LayerFlags.Price) > 0;
    public static bool HasVolume(this LayerFlags flags)                => (flags & LayerFlags.Volume) > 0;
    public static bool HasSourceName(this LayerFlags flags)            => (flags & LayerFlags.SourceName) > 0;
    public static bool HasExecutable(this LayerFlags flags)            => (flags & LayerFlags.Executable) > 0;
    public static bool HasSourceQuoteReference(this LayerFlags flags)  => (flags & LayerFlags.SourceQuoteReference) > 0;
    public static bool HasValueDate(this LayerFlags flags)             => (flags & LayerFlags.ValueDate) > 0;
    public static bool HasInternalVolume(this LayerFlags flags)        => (flags & LayerFlags.InternalVolume) > 0;
    public static bool HasOrderCount(this LayerFlags flags)            => (flags & LayerFlags.OrdersCount) > 0;
    public static bool HasOrderId(this LayerFlags flags)               => (flags & LayerFlags.OrderId) > 0;
    public static bool HasOrderCreated(this LayerFlags flags)          => (flags & LayerFlags.OrderCreated) > 0;
    public static bool HasOrderUpdated(this LayerFlags flags)          => (flags & LayerFlags.OrderUpdated) > 0;
    public static bool HasOrderSize(this LayerFlags flags)             => (flags & LayerFlags.OrderSize) > 0;
    public static bool HasOrderRemainingSize(this LayerFlags flags)    => (flags & LayerFlags.OrderRemainingSize) > 0;
    public static bool HasOrderCounterPartyName(this LayerFlags flags) => (flags & LayerFlags.OrderCounterPartyName) > 0;
    public static bool HasOrderTraderName(this LayerFlags flags)       => (flags & LayerFlags.OrderTraderName) > 0;

    public static LayerFlags Unset(this LayerFlags flags, LayerFlags toUnset) => flags & ~toUnset;

    public static bool HasAllOf(this LayerFlags flags, LayerFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this LayerFlags flags, LayerFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this LayerFlags flags, LayerFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this LayerFlags flags, LayerFlags checkAllFound)   => flags == checkAllFound;
}
