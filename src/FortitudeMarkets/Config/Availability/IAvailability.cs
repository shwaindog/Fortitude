// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Config.Availability;

[Flags]
public enum TradingPeriodTypeFlags : ushort
{
    None                      = 0x00_00
  , MarketClosed              = 0x00_01
  , PreOpen                   = 0x00_02
  , Open                      = 0x00_04
  , ClosingSoon               = 0x00_08
  , AcceptingOrders           = 0x00_10
  , Pricing                   = 0x00_20
  , Trading                   = 0x00_40
  , NonPreferredTradingPeriod = 0x00_80
  , PreferredTradingPeriod    = 0x01_00
  , HighLiquidity             = 0x02_00
  , NormalLiquidity           = 0x04_00
  , LowLiquidity              = 0x08_00
}

public static class TradingPeriodTypeFlagsExtensions
{
    public static bool IsMarketClose(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.MarketClosed) > 0;

    public static bool IsOpen(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.Open) > 0;

    public static bool IsPreOpen(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.PreOpen) > 0;

    public static bool IsClosingSoon(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.ClosingSoon) > 0;

    public static bool IsAcceptingOrders(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.AcceptingOrders) > 0;

    public static bool IsPricing(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.Pricing) > 0;

    public static bool IsTrading(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.Trading) > 0;

    public static bool IsNonPreferredTradingPeriod(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.NonPreferredTradingPeriod) > 0;

    public static bool IsPreferredTradingPeriod(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.PreferredTradingPeriod) > 0;

    public static bool IsHighLiquidity(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.HighLiquidity) > 0;

    public static bool IsNormalLiquidity(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.NormalLiquidity) > 0;

    public static bool IsLowLiquidity(this TradingPeriodTypeFlags flags) => (flags & TradingPeriodTypeFlags.LowLiquidity) > 0;
}

public interface IAvailability
{
    TradingPeriodTypeFlags           GetExpectedAvailability(DateTimeOffset atThisDateTime);

    TimeSpan       ExpectedRemainingUpTime(DateTimeOffset fromNow);

    DateTimeOffset NextScheduledOpeningTime(DateTimeOffset fromNow);
}
