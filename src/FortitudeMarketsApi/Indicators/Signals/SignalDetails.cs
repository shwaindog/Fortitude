// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Indicators.Signals;

public enum SignalType : byte
{
    Oscillator
  , OscillatorPeakThough
  , TrendLifeTime
  , MeanReversionLifeTime
  , BreakoutLifeTime
  , AggregatedSignal
  , TrendLineSet
  , TrendLineTripped
  , TrendLineFormed
  , TrendLineBounce
  , RangeBreached
  , NewRange
  , MeanCalculation
  , MeanVelocity
  , TickCountRange
  , LongTermDirection
  , MidTermDirection
  , ShortTermDirection
  , KeyPricePointReached
  , RecentHighLowPullback
  , HeadAndShoulders
  , FlagRange
  , SignalStrengthAdjustment
  , PriceMeanDeviation
  , LastPriceTimeSpan
  , CorrelatedSymbolDivergence
  , LeadingInstrumentReverted
  , MovingAverage
  , MovingAverageVelocity
  , MovingAveragesCrossed
  , PeakTroughPoints
  , ShortTermOscillationPeriod
  , MidTermOscillationPeriod
  , TradingMultiplierChange
  , TradingStatus
  , SignalSettings
  , TickerStatus
  , TickerActivity
  , MarketEvent
  , MarketNews
  , MarketSentiment
  , VolatilityInstrument
  , VolatilityIndex
  , InterestRateSentiment
  , AnalystPredictions
  , ConfiguredSignal
  , LastTrade
  , BookTilt
}

public interface ISignalDetails
{
    int        DetailsId           { get; }
    string     Name                { get; }
    string     Description         { get; }
    Indicator  PublishingIndicator { get; }
    SignalType SignalType          { get; }
}

public class SignalDetails : ISignalDetails
{
    public int        DetailsId           { get; set; }
    public string     Name                { get; set; }
    public string     Description         { get; set; }
    public Indicator  PublishingIndicator { get; set; }
    public SignalType SignalType          { get; set; }
}
