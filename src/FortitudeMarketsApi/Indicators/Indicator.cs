// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsApi.Indicators;

public enum IndicatorType : byte
{
    Oscillator
  , Averaging
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

public interface IIndicator : IInstrument
{
    ushort IndicatorId   { get; }
    string IndicatorName { get; }
    string Description   { get; }

    IndicatorType IndicatorType { get; }
}

public class Indicator : Instrument, IIndicator
{
    public Indicator
    (ushort indicatorId, string indicatorName, string instrumentSource, IndicatorType indicatorType
      , TimeSeriesPeriod indicatorPeriod, string description = "No Description Given")
        : base(indicatorName, instrumentSource, InstrumentType.Indicator, indicatorPeriod)
    {
        IndicatorId   = indicatorId;
        IndicatorType = indicatorType;
        Description   = description;
    }

    public Indicator
    (ushort indicatorId, string instrumentName, string instrumentSource, IndicatorType indicatorType
      , TimeSeriesPeriod entryPeriod, string description = "No Description Given", params KeyValuePair<string, string>[] requiredValues)
        : base(instrumentName, instrumentSource, InstrumentType.Indicator, entryPeriod, requiredValues)
    {
        IndicatorId   = indicatorId;
        IndicatorType = indicatorType;
        Description   = description;
    }

    public ushort IndicatorId   { get; set; }
    public string IndicatorName => InstrumentName;
    public string Description   { get; set; }

    public IndicatorType IndicatorType { get; set; }
}
