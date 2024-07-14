// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries;

public enum InstrumentType : byte
{
    Unknown
  , Any
  , Custom
  , Price
  , PriceSummaryPeriod
  , Indicator
  , AlgoState
  , AlgoSignal
}

public readonly struct PeriodInstrumentTypePair(InstrumentType instrumentType, TimeSeriesPeriod period)
{
    public TimeSeriesPeriod EntryPeriod { get; } = period;

    public InstrumentType InstrumentType { get; } = instrumentType;
}
