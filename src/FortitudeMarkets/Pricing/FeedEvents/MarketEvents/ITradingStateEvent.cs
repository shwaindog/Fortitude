using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

[Flags]
public enum TradingEventType
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

public interface ITradingStateEvent : IReusableObject<ITradingStateEvent>, IInterfacesComparable<ITradingStateEvent>
{
    uint EventSequenceId { get; }

    TradingEventType TradingEvent { get; }

    DateTime StartedAtTime { get; }

    int EstimatedLengthSeconds { get; }
}

public interface IMutableTradingStateEvent : ITradingStateEvent, ICloneable<IMutableTradingStateEvent>
{
    new uint EventSequenceId { get; set; }

    new TradingEventType TradingEvent { get; set; }

    new DateTime StartedAtTime { get; set; }

    new int EstimatedLengthSeconds { get; set; }
    
    new IMutableTradingStateEvent Clone();
}