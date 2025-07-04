namespace FortitudeMarkets.Trading.Limits;

public enum LimitType
{
    None
  , TotalOpenPositionLimit
  , DailyTotalOpenPositionLimit
  , TickerOpenPositionLimit
  , DailyTickerOpenPositionLimit
  , AssetClassExposureLimit
  , TickerExposureLimit
  , TotalSubmitThroughputCountLimit
  , TotalSubmitThroughputVolumeLimit
  , TickerThroughputCountLimit
  , TickerThroughputVolumeLimit
  , MarginLimit
  , OrderSize
  , OnBehalfOfNotAuthorised
  , OnBehalfOfLimit
  , VolatilityLimit
  , TotalOpenInterestLimit
  , TickerOpenInterestLimit
  , TotalPeriodLossLimit
  , TickerPeriodLossLimit
  , TotalDailyLossLimit
  , TickerDailyLossLimit
  , MarketDistanceLimit
  , TotalLosingRunLimit
  , TickerLosingRunLimit
  , TotalAggressiveCountLimit 
  , TickerAggressiveCountLimit 
}