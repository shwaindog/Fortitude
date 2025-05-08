// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LastTraded;

[JsonConverter(typeof(JsonStringEnumConverter<LastTradedFlags>))]
[Flags]
public enum LastTradedFlags : ushort
{
    None             = 0x00
  , LastTradedPrice  = 0x01
  , LastTradedTime   = 0x02
  , LastTradedVolume = 0x04
  , PaidOrGiven      = 0x08
  , TraderName       = 0x10
}

public static class LastTradedFlagsExtensions
{
    public const LastTradedFlags LastTradedPriceAndTimeFlags = LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;

    public const LastTradedFlags AdditionalPaidGivenFlags = LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume;

    public const LastTradedFlags AdditionalTraderNameFlags = LastTradedFlags.TraderName;

    public const LastTradedFlags FullPaidOrGivenFlags = AdditionalPaidGivenFlags | LastTradedPriceAndTimeFlags;

    public const LastTradedFlags FullTraderNamePaidOrGivenFlags = AdditionalTraderNameFlags | FullPaidOrGivenFlags;

    public static LastTradedFlags SupportedLastTradedFlags(this LastTradeType lastTradeType)
    {
        switch (lastTradeType)
        {
            case LastTradeType.Price:                  return LastTradedPriceAndTimeFlags;
            case LastTradeType.PricePaidOrGivenVolume: return FullPaidOrGivenFlags;
            case LastTradeType.PriceLastTraderName:    return FullTraderNamePaidOrGivenFlags;

            default: return LastTradedFlags.None;
        }
    }

    public static bool HasLastTradedPrice(this LastTradedFlags flags)  => (flags & LastTradedFlags.LastTradedPrice) > 0;
    public static bool HasLastTradedTime(this LastTradedFlags flags)   => (flags & LastTradedFlags.LastTradedTime) > 0;
    public static bool HasPaidOrGiven(this LastTradedFlags flags)      => (flags & LastTradedFlags.PaidOrGiven) > 0;
    public static bool HasLastTradedVolume(this LastTradedFlags flags) => (flags & LastTradedFlags.LastTradedVolume) > 0;
    public static bool HaTraderName(this LastTradedFlags flags)        => (flags & LastTradedFlags.TraderName) > 0;

    public static LastTradedFlags Unset(this LastTradedFlags flags, LastTradedFlags toUnset) => flags & ~toUnset;

    public static bool HasAllOf(this LastTradedFlags flags, LastTradedFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this LastTradedFlags flags, LastTradedFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this LastTradedFlags flags, LastTradedFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this LastTradedFlags flags, LastTradedFlags checkAllFound)   => flags == checkAllFound;
}
