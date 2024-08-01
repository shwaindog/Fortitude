// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsApi.Indicators.Signals;

public struct VolatilityPeriod
{
    public decimal            Volatility { get; set; }
    public TimeBoundaryPeriod Period     { get; set; }

    public TimeSpan FromSignal { get; set; }
}

public struct MovingAveragePeriods
{
    public BidAskInstantPair  MarketAveragePrice { get; set; }
    public TimeBoundaryPeriod AveragePeriod      { get; set; }
    public decimal            Velocity           { get; set; }
}

public enum PricePointType
{
    HorizontalSupport
  , HorizontalResistance
  , SupportStart
  , SupportEnd
  , ResistanceStart
  , ResistanceEnd
}

public struct KeyPricePoints
{
    public decimal        PricePoint     { get; set; }
    public DateTime       AtTime         { get; set; }
    public PricePointType PricePointType { get; set; }
}
