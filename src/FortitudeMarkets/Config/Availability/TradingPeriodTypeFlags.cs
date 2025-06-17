// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Config.Availability;

[Flags]
public enum TradingPeriodTypeFlags : uint
{
    None                      = 0x00_00
  , IsOpen                      = 0x00_01
  , IsMarketClosed              = 0x00_02
  , IsOutOfHours                = 0x00_04
  , IsPublicHoliday             = 0x00_08
  , IsWeekend                   = 0x00_10
  , IsPricing                   = 0x00_20
  , IsTrading                   = 0x00_40
  , IsPreOpen                   = 0x00_80
  , IsClosingSoon               = 0x01_00
  , IsAcceptingOrders           = 0x02_00
  , IsPreferredTradingPeriod    = 0x04_00
  , IsNonPreferredTradingPeriod = 0x08_00
  , IsHighActivityPeriod        = 0x10_00
  , IsLowActivityPeriod         = 0x20_00
  , IsMainTradingPeriod         = 0x40_00
  , IsGreyMarketTradingPeriod   = 0x80_00

  , AllFlagsMask = 0xFF_FF
}

public static class TradingPeriodTypeFlagsExtensions
{
    public const TradingPeriodTypeFlags OpenAllowFlagsMask = TradingPeriodTypeFlags.AllFlagsMask;

    public const TradingPeriodTypeFlags MarketClosedAllowFlagsMask =
        TradingPeriodTypeFlags.IsMarketClosed | TradingPeriodTypeFlags.IsOutOfHours | TradingPeriodTypeFlags.IsPublicHoliday |
        TradingPeriodTypeFlags.IsWeekend;

    public static bool IsMarketClose(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsMarketClosed) > 0;

    public static bool IsOutOfHours(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsOutOfHours) > 0;

    public static bool IsOpen(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsOpen) > 0;

    public static bool IsPublicHoliday(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsPublicHoliday) > 0;

    public static bool IsWeekend(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsWeekend) > 0;

    public static bool IsPricing(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsPricing) > 0;

    public static bool IsTrading(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsTrading) > 0;

    public static bool IsPreOpen(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsPreOpen) > 0;

    public static bool IsClosingSoon(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsClosingSoon) > 0;

    public static bool IsAcceptingOrders(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsAcceptingOrders) > 0;

    public static bool IsNonPreferredTradingPeriod
        (this TradingPeriodTypeFlags flags) =>
        (flags & TradingPeriodTypeFlags.IsNonPreferredTradingPeriod) > 0;

    public static bool IsPreferredTradingPeriod(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsPreferredTradingPeriod) > 0;

    public static bool IsHighActivityPeriod(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsHighActivityPeriod) > 0;

    public static bool IsLowActivityPeriod(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsLowActivityPeriod) > 0;

    public static bool IsMainTradingPeriod(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsMainTradingPeriod) > 0;

    public static bool IsGreyMarketTradingPeriod(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.IsGreyMarketTradingPeriod) > 0;
}
