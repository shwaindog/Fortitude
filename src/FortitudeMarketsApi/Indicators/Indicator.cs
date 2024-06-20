// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Indicators;

public enum IndicatorType : byte
{
    Oscillator
  , Momentum
  , SignalAggregating
  , SignalFiltering
  , MarketBehaviorChange
  , TrendReversing
  , MeanReverting
  , RangeChange
  , CorrelatedInstrument
  , BreakoutDetecting
  , VolatilityStatus
  , EconomicCalendar
  , MarketAnnouncement
  , MarketOutlook
  , CurrentSentiment
  , MarketEvent
  , MarketNews
  , MarketAnalysis
  , AggregateUpdate
  , SupportResistance
  , KeyPriceLevel
  , TickerActivity
  , TradingSize
  , TradingStatus
  , SignalModification
  , SignalMonitoring
}

public interface IIndicator
{
    int    Id          { get; }
    string Name        { get; }
    string Description { get; }

    IndicatorType IndicatorType { get; }
}

public class Indicator : IIndicator
{
    public Indicator(int id, string name, string description, IndicatorType indicatorType)
    {
        Id            = id;
        Name          = name;
        Description   = description;
        IndicatorType = indicatorType;
    }

    public int    Id          { get; set; }
    public string Name        { get; set; }
    public string Description { get; set; }

    public IndicatorType IndicatorType { get; set; }
}
