using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Pricing.FeedEvents;

[Flags]
public enum FeedConnectivityStatusFlags : uint
{
    None                             = 0x00_00_00_00
  , SourceReporting                  = 0x00_00_00_01
  , AdapterReporting                 = 0x00_00_00_02
  , ClientReporting                  = 0x00_00_00_04
  , TickerPriceSubscriptionFailed    = 0x00_00_00_08
  , NotAcceptingOrders               = 0x00_00_00_10
  , PricingIsStale                   = 0x00_00_00_20
  , RequestingSnapshot               = 0x00_00_00_40
  , TradingHalt                      = 0x00_00_00_80
  , MarketDisconnection              = 0x00_00_01_00
  , FromSourceSnapshot               = 0x00_00_02_00
  , FromAdapterSnapshot              = 0x00_00_04_00
  , FromStorage                      = 0x00_00_08_00
  , IsSourceReplay                   = 0x00_00_10_00
  , IsAdapterReplay                  = 0x00_00_28_00
  , ReportingOnPricing               = 0x00_00_40_00
  , ReportingOnTrading               = 0x00_00_80_00
  , ReportingConnectionNonResponsive = 0x00_01_00_00
  , ReportingSlowResponses           = 0x00_02_00_00
  , ReportingHighLatency             = 0x00_04_00_00
  , ReportingSessionDown             = 0x00_08_00_00
  , ReportingOrdersGoneUnknown       = 0x00_10_00_00
  , InPreferredNonTradingPeriod      = 0x00_20_00_00
  , ClosingSoon                      = 0x00_40_00_00
  , ClosedOutOfHours                 = 0x00_80_00_00
  , Opening                          = 0x01_00_00_00
  , AwaitingConnectionStart          = 0x02_00_00_00
  , Connecting                       = 0x04_00_00_00
  , Disconnecting                    = 0x08_00_00_00
  , Disconnected                     = 0x10_00_00_00
  , Reconnecting                     = 0x20_00_00_00
  , AboutToRestart                   = 0x40_00_00_00
  , AboutToStop                      = 0x80_00_00_00
}


public static class FeedConnectivityStatusFlagsExtensions
{
    public const FeedConnectivityStatusFlags ClientDefaultConnectionState = FeedConnectivityStatusFlags.AwaitingConnectionStart;


    public static bool HasIsSourceReplay (this FeedConnectivityStatusFlags connectivityStatus) =>
        (connectivityStatus & FeedConnectivityStatusFlags.IsSourceReplay) > 0;

    public static bool HasIsAdapterReplay (this FeedConnectivityStatusFlags connectivityStatus) =>
        (connectivityStatus & FeedConnectivityStatusFlags.IsAdapterReplay) > 0;

    public static bool HasFromSourceSnapshot (this FeedConnectivityStatusFlags connectivityStatus) =>
        (connectivityStatus & FeedConnectivityStatusFlags.FromSourceSnapshot) > 0;

    public static bool HasFromAdapterSnapshot (this FeedConnectivityStatusFlags connectivityStatus) =>
        (connectivityStatus & FeedConnectivityStatusFlags.FromAdapterSnapshot) > 0;

    
    public static FeedConnectivityStatusFlags Unset(this FeedConnectivityStatusFlags flags, FeedConnectivityStatusFlags toUnset) => flags & ~toUnset;
    public static FeedConnectivityStatusFlags Set(this FeedConnectivityStatusFlags flags, FeedConnectivityStatusFlags toSet) => flags | toSet;

    public static bool HasAllOf(this FeedConnectivityStatusFlags flags, FeedConnectivityStatusFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this FeedConnectivityStatusFlags flags, FeedConnectivityStatusFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this FeedConnectivityStatusFlags flags, FeedConnectivityStatusFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this FeedConnectivityStatusFlags flags, FeedConnectivityStatusFlags checkAllFound)   => flags == checkAllFound;
}