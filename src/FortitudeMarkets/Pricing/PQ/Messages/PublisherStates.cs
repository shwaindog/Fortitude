using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Pricing.PQ.Messages;

[Flags]
public enum PublisherStates : uint
{
    None                                = 0x00_00_00_00
  , TickerPriceSubscriptionFailed       = 0x00_00_00_01
  , SourceNotAcceptingOrders            = 0x00_00_00_02
  , AdapterNotAcceptingOrders           = 0x00_00_00_04
  , PricingIsStale                      = 0x00_00_00_08
  , RequestingSnapshot                  = 0x00_00_00_10
  , MarketReportingTradingHalt          = 0x00_00_00_20
  , AdapterReportingTradingHalt         = 0x00_00_00_40
  , SourceReportingMarketConnectionLoss = 0x00_00_00_80
  , WaitingForFirstPriceUpdate          = 0x00_00_01_00
  , ReportingOnPricingSession           = 0x00_00_02_00
  , ReportingOnTradingSession           = 0x00_00_04_00
  , ReportingSessionHighLatencyOn       = 0x00_00_08_00
  , ReportingSessionSlow                = 0x00_00_10_00
  , ReportingSessionNotResponding       = 0x00_00_20_00
  , ReportingUnexpectedSessionLost      = 0x00_00_40_00
  , ReportingSessionRestarting          = 0x00_00_80_00
  , ReportingRequestRejection           = 0x00_01_00_00 // include pricing FailedTickerSubscription
  , ReportingOrdersAreUnknown           = 0x00_02_00_00
  , IsPreferredNonTradingPeriod         = 0x00_04_00_00
  , MarketClosingSoon                   = 0x00_08_00_00
  , MarketClosed                        = 0x00_10_00_00
  , MarketOutOfHours                    = 0x00_20_00_00
  , MarketOpening                       = 0x00_40_00_00
  , AwaitingConnectionStart             = 0x00_80_00_00
  , WaitingForMarketOpen                = 0x01_00_00_00
  , ConnectingToSource                  = 0x02_00_00_00
  , DisconnectionImmanent               = 0x04_00_00_00
  , DisconnectedFromSource              = 0x08_00_00_00
  , DisconnectionUnexpected             = 0x10_00_00_00
  , PublisherAboutToRestart             = 0x20_00_00_00
  , PublisherStopping                   = 0x40_00_00_00
}
