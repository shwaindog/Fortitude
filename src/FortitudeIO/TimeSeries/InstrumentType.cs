// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries;

public enum InstrumentType : ushort
{
    Custom
  , Price
  , PriceSummaryPeriod
  , Indicator
  , AlgoState
  , AlgoSignal
}

public readonly struct PeriodInstrumentTypePair(InstrumentType instrumentType, TimeSeriesPeriod period)
{
    public TimeSeriesPeriod Period { get; } = period;

    public InstrumentType InstrumentType { get; } = instrumentType;
}
