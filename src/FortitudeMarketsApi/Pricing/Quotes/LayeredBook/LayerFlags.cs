// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

[Flags]
public enum LayerFlags : uint
{
    None                 = 0x00_00
  , Ladder               = 0x00_01
  , Price                = 0x00_02
  , Volume               = 0x00_04
  , SourceQuoteReference = 0x00_08
  , SourceName           = 0x00_10
  , TraderCount          = 0x00_20
  , TraderName           = 0x00_40
  , TraderSize           = 0x00_80
  , ValueDate            = 0x01_00
  , Executable           = 0x02_00
}

public static class LayerFlagsExtensions
{
    public static LayerFlags SupportedLayerFlags(this LayerType layerType)
    {
        switch (layerType)
        {
            case LayerType.PriceVolume:          return LayerFlags.Price | LayerFlags.Volume;
            case LayerType.ValueDatePriceVolume: return LayerFlags.ValueDate | LayerFlags.Price | LayerFlags.Volume;
            case LayerType.SourcePriceVolume:    return LayerFlags.SourceName | LayerFlags.Price | LayerFlags.Volume;
            case LayerType.SourceQuoteRefPriceVolume:
                return LayerFlags.SourceQuoteReference | LayerFlags.SourceName | LayerFlags.Price | LayerFlags.Volume;
            case LayerType.TraderPriceVolume:
                return LayerFlags.TraderCount | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.Price | LayerFlags.Volume;
            case LayerType.SourceQuoteRefTraderValueDatePriceVolume:
                return LayerFlags.ValueDate | LayerFlags.SourceQuoteReference | LayerFlags.SourceName | LayerFlags.TraderCount
                     | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.Price | LayerFlags.Volume;
            default: return LayerFlags.None;
        }
    }

    public static bool HasLadder(this LayerFlags flags)                             => (flags & LayerFlags.Ladder) > 0;
    public static bool HasPrice(this LayerFlags flags)                              => (flags & LayerFlags.Price) > 0;
    public static bool HasVolume(this LayerFlags flags)                             => (flags & LayerFlags.Volume) > 0;
    public static bool HasSourceName(this LayerFlags flags)                         => (flags & LayerFlags.SourceName) > 0;
    public static bool HasExecutable(this LayerFlags flags)                         => (flags & LayerFlags.Executable) > 0;
    public static bool HasSourceQuoteReference(this LayerFlags flags)               => (flags & LayerFlags.SourceQuoteReference) > 0;
    public static bool HasTraderCount(this LayerFlags flags)                        => (flags & LayerFlags.TraderCount) > 0;
    public static bool HasTraderName(this LayerFlags flags)                         => (flags & LayerFlags.TraderName) > 0;
    public static bool HasTraderSize(this LayerFlags flags)                         => (flags & LayerFlags.TraderSize) > 0;
    public static bool HasValueDate(this LayerFlags flags)                          => (flags & LayerFlags.ValueDate) > 0;
    public static bool HasAllOf(this LayerFlags flags, LayerFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this LayerFlags flags, LayerFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this LayerFlags flags, LayerFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this LayerFlags flags, LayerFlags checkAllFound)   => flags == checkAllFound;
}
