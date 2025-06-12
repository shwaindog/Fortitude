using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Pricing.FeedEvents;

public enum FeedEventUpdateFlags : uint
{
    None                         = 0
  , SourceTriggeredUpdate        = 0x00_00_00_01
  , SourcePricingChangeUpdate    = 0x00_00_00_02
  , SourceOrderOrExecutionUpdate = 0x00_00_00_04
  , AdapterTriggeredUpdate       = 0x00_00_00_08
  , OtherTriggeredUpdate         = 0x00_00_00_10
  , ClientSnapshotTriggered      = 0x00_00_00_20
  , ConnectivityUpdate           = 0x00_00_00_40
  , IsPreviousCloseValues        = 0x00_00_00_80
  , QuoteNoLongerValid           = 0x00_00_01_00
  , QuoteChangeUpdate            = 0x00_00_04_00
  , LastTradedUpdate             = 0x00_00_08_00
  , MarketVolumesUpdate          = 0x00_00_10_00
  , AccountsUpdate               = 0x00_00_20_00
  , MarketNewsUpdate             = 0x00_00_40_00
  , MarketCalendarUpdate         = 0x00_00_80_00
  , MarketTradingStateUpdate     = 0x00_01_00_00
  , InternalOrdersUpdate         = 0x00_02_00_00
  , InternalOrderTradeUpdate     = 0x00_04_00_00
  , LimitsUpdate                 = 0x00_08_00_00
  , LimitAlertsUpdate            = 0x00_10_00_00
  , LimitBreachUpdate            = 0x00_20_00_00
  , IndicatorsUpdate             = 0x00_40_00_00
  , TradingDayUpdated            = 0x00_80_00_00
  , SignalsUpdated               = 0x01_00_00_00
  , StrategiesUpdated            = 0x02_00_00_00
  , SimulatorInjected            = 0x04_00_00_00
  , SimulatorModified            = 0x08_00_00_00
  , NoDataYetReceived            = 0x10_00_00_00
  , HasConflationSummary         = 0x20_00_00_00
}

public static class FeedEventUpdateFlagsExtensions { }
