using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Pricing.FeedEvents;

[Flags]
public enum FeedConnectivityStatusFlags : uint
{
    None
  , OutOfHours                            = 0x00_00_00_01
  , MarketNotTakingOrders                 = 0x00_00_00_02
  , TickerNoLongerPricing                 = 0x00_00_00_04
  , MarketClosing                         = 0x00_00_00_08
  , MarketOpened                          = 0x00_00_00_10
  , SourceReportedTradingHalt             = 0x00_00_00_20
  , SourceReportedMarketDisconnection     = 0x00_00_00_40
  , AdapterYetToConnectToSource           = 0x00_00_00_80
  , AdapterConnecting                     = 0x00_00_01_00
  , AdapterCanNotSubscribeToTicker        = 0x00_00_02_00
  , AdapterPreparingForMarketClose        = 0x00_00_04_00
  , AdapterReportingNonResponsiveSource   = 0x00_00_08_00
  , AdapterReportingSlowResponsiveSource  = 0x00_00_10_00
  , AdapterReportingHighPricingLatency    = 0x00_00_20_00
  , AdapterReportingHasOrdersGoneUnknown  = 0x00_00_40_00
  , AdapterReportingTradingSessionDown    = 0x00_00_80_00
  , AdapterReportingPricingSessionDown    = 0x00_01_00_00
  , AdapterGracefullyDisconnecting        = 0x00_02_00_00
  , AdapterAbruptlyDisconnectedFromSource = 0x00_04_00_00
  , AdapterAboutToRestart                 = 0x00_08_00_00
  , AwaitClientConnectionStart            = 0x00_10_00_00
  , ClientConnecting                      = 0x00_20_00_00
  , ClientRequestingSnapshot              = 0x00_40_00_00
  , ClientReportedStale                   = 0x00_80_00_00
  , ClientReportingHighPricingLatency     = 0x01_00_00_00
  , ClientReportedAdapterUnresponsive     = 0x02_00_00_00
}
