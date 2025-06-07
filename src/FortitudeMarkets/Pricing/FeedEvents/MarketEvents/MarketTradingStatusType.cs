// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

[Flags]
public enum MarketTradingStatusType
{
    None                       = 0x00_00_00_00
  , PreMarketOpen              = 0x00_00_00_01
  , NewDayStarted              = 0x00_00_00_02
  , MarketOpen                 = 0x00_00_00_04
  , ClosingSoon                = 0x00_00_00_08
  , TradingDayEnded            = 0x00_00_00_10
  , MarketClosed               = 0x00_00_00_20
  , OutOfHours                 = 0x00_00_00_40
  , Pricing                    = 0x00_00_00_80
  , TakingOrders               = 0x00_00_01_00
  , ExpiryDay                  = 0x00_00_02_00
  , Trading                    = 0x00_00_04_00
  , TradingHalt                = 0x00_00_08_00
  , LimitUp                    = 0x00_00_10_00
  , LimitDown                  = 0x00_00_20_00
  , BuyCreditLimitReached      = 0x00_00_40_00
  , SellCreditLimitReached     = 0x00_00_80_00
  , MainRegionPublicHoliday    = 0x00_01_00_00
  , InstrumentHoliday          = 0x00_02_00_00
  , FixingPeriod               = 0x00_04_00_00
  , FixingTime                 = 0x00_08_00_00
  , PendingMarketAnnouncement  = 0x00_10_00_00
  , MarketAnnouncement         = 0x00_20_00_00
  , AuctionPeriod              = 0x00_40_00_00
  , AcceptingNextOpeningOrders = 0x00_80_00_00
  , GreyMarket                 = 0x01_00_00_00
  , LowActivityPeriod          = 0x02_00_00_00
  , HighActivityPeriod         = 0x04_00_00_00
  , LowLiquidityPeriod         = 0x08_00_00_00
  , AverageLiquidityPeriod     = 0x10_00_00_00
  , HighLiquidityPeriod        = 0x20_00_00_00
}

public static class MarketTradingStatusTypeExtensions
{
    public static bool HasPreMarketOpenFlag(this MarketTradingStatusType flags)   => (flags & MarketTradingStatusType.PreMarketOpen) > 0;
    public static bool HasNewDayStartedFlag(this MarketTradingStatusType flags)   => (flags & MarketTradingStatusType.NewDayStarted) > 0;
    public static bool HasMarketOpenFlag(this MarketTradingStatusType flags)      => (flags & MarketTradingStatusType.MarketOpen) > 0;
    public static bool HasClosingSoonFlag(this MarketTradingStatusType flags)     => (flags & MarketTradingStatusType.ClosingSoon) > 0;
    public static bool HasTradingDayEndedFlag(this MarketTradingStatusType flags) => (flags & MarketTradingStatusType.TradingDayEnded) > 0;
    public static bool HasMarketClosedFlag(this MarketTradingStatusType flags)    => (flags & MarketTradingStatusType.MarketClosed) > 0;
    public static bool HasOutOfHoursFlag(this MarketTradingStatusType flags)      => (flags & MarketTradingStatusType.OutOfHours) > 0;
    public static bool HasPricingFlag(this MarketTradingStatusType flags)         => (flags & MarketTradingStatusType.Pricing) > 0;
    public static bool HasTakingOrdersFlag(this MarketTradingStatusType flags)    => (flags & MarketTradingStatusType.TakingOrders) > 0;
    public static bool HasExpiryDayFlag(this MarketTradingStatusType flags)       => (flags & MarketTradingStatusType.ExpiryDay) > 0;
    public static bool HasTradingFlag(this MarketTradingStatusType flags)         => (flags & MarketTradingStatusType.Trading) > 0;
    public static bool HasTradingHaltFlag(this MarketTradingStatusType flags)     => (flags & MarketTradingStatusType.TradingHalt) > 0;
    public static bool HasLimitUpFlag(this MarketTradingStatusType flags)         => (flags & MarketTradingStatusType.LimitUp) > 0;
    public static bool HasLimitDownFlag(this MarketTradingStatusType flags)       => (flags & MarketTradingStatusType.LimitDown) > 0;

    public static bool HasBuyCreditLimitReachedFlag(this MarketTradingStatusType flags) =>
        (flags & MarketTradingStatusType.BuyCreditLimitReached) > 0;

    public static bool HasSellCreditLimitReachedFlag(this MarketTradingStatusType flags) =>
        (flags & MarketTradingStatusType.SellCreditLimitReached) > 0;

    public static bool HasMainRegionPublicHolidayFlag(this MarketTradingStatusType flags) =>
        (flags & MarketTradingStatusType.MainRegionPublicHoliday) > 0;

    public static bool HasInstrumentHolidayFlag(this MarketTradingStatusType flags) => (flags & MarketTradingStatusType.InstrumentHoliday) > 0;
    public static bool HasFixingPeriodFlag(this MarketTradingStatusType flags)      => (flags & MarketTradingStatusType.FixingPeriod) > 0;
    public static bool HasFixingTimeFlag(this MarketTradingStatusType flags)        => (flags & MarketTradingStatusType.FixingTime) > 0;

    public static bool HasPendingMarketAnnouncementFlag(this MarketTradingStatusType flags) =>
        (flags & MarketTradingStatusType.PendingMarketAnnouncement) > 0;

    public static bool HasMarketAnnouncementFlag(this MarketTradingStatusType flags) => (flags & MarketTradingStatusType.MarketAnnouncement) > 0;
    public static bool HasAuctionPeriodFlag(this MarketTradingStatusType flags)      => (flags & MarketTradingStatusType.AuctionPeriod) > 0;

    public static bool HasAcceptingNextOpeningOrdersFlag(this MarketTradingStatusType flags) =>
        (flags & MarketTradingStatusType.AcceptingNextOpeningOrders) > 0;

    public static bool HasGreyMarketFlag(this MarketTradingStatusType flags)         => (flags & MarketTradingStatusType.GreyMarket) > 0;
    public static bool HasLowActivityPeriodFlag(this MarketTradingStatusType flags)  => (flags & MarketTradingStatusType.LowActivityPeriod) > 0;
    public static bool HasHighActivityPeriodFlag(this MarketTradingStatusType flags) => (flags & MarketTradingStatusType.HighActivityPeriod) > 0;
    public static bool HasLowLiquidityPeriodFlag(this MarketTradingStatusType flags) => (flags & MarketTradingStatusType.LowLiquidityPeriod) > 0;

    public static bool HasAverageLiquidityPeriodFlag(this MarketTradingStatusType flags) =>
        (flags & MarketTradingStatusType.AverageLiquidityPeriod) > 0;

    public static bool HasHighLiquidityPeriodFlag(this MarketTradingStatusType flags) => (flags & MarketTradingStatusType.HighLiquidityPeriod) > 0;
}
