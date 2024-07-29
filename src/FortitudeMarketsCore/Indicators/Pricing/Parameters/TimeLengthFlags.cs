// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using static FortitudeMarketsCore.Indicators.Pricing.Parameters.TimeLengthFlags;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.Parameters;

[Flags]
public enum TimeLengthFlags
{
    None                                = 0
  , WallTime                            = 1
  , IncludeOpenMarketInvalidPeriods     = 2
  , IncludeOpenMarketDataGapPeriods     = 4
  , UseWallTimeWithGapsAndInvalidValues = 7
  , ValidPeriodTime                     = 8
  , UseValidAndInvalidTime              = 10
  , UseValidAndInvalidAndGapsTime       = 14
}

public static class TimeLengthFlagsExtensions
{
    public static bool IsUseWallTime(this TimeLengthFlags flags) => (flags & WallTime) == WallTime;

    public static bool HasIncludeOpenMarketInvalidPeriodsFlag(this TimeLengthFlags flags) => (flags & IncludeOpenMarketInvalidPeriods) > 0;
    public static bool HasIncludeOpenMarketDataGapPeriodsFlag(this TimeLengthFlags flags) => (flags & IncludeOpenMarketDataGapPeriods) > 0;

    public static bool IsValidPeriodTime(this TimeLengthFlags flags) => (flags & ValidPeriodTime) == ValidPeriodTime;
}
