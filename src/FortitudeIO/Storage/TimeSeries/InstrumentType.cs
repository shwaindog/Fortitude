// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeIO.Storage.TimeSeries;

public enum InstrumentType : byte
{
    Price = 0
  , Unknown
  , Any
  , Custom
  , Candle
  , Indicator
  , AlgoState
  , AlgoSignal
}

public readonly struct PeriodInstrumentTypePair(InstrumentType instrumentType, DiscreetTimePeriod period)
{
    public DiscreetTimePeriod CoveringPeriod { get; } = period;

    public InstrumentType InstrumentType { get; } = instrumentType;
}
