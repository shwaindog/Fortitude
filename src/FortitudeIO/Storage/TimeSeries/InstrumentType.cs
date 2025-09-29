// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types.StringsOfPower;

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
    
    public static PalantírReveal<PeriodInstrumentTypePair> Styler { get; } =
        (pitp, stsa) =>
            stsa.StartComplexType(pitp, nameof(pitp))
                .Field.AlwaysAdd(nameof(pitp.InstrumentType), pitp.InstrumentType)
                .Field.AlwaysReveal(nameof(pitp.CoveringPeriod), pitp.CoveringPeriod, DiscreetTimePeriod.Styler)
                .Complete();
}
