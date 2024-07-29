// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Indicators.Pricing.Parameters;

public readonly struct CalculateMovingAverageOptions
{
    public CalculateMovingAverageOptions()
    {
        IncludeOpenMarketInvalidValues  = true;
        IncludeOpenMarketDataGapsValues = true;
    }

    public CalculateMovingAverageOptions
    (TimeLengthFlags timeLengthFlags = TimeLengthFlags.UseValidAndInvalidTime, bool includeOpenMarketInvalidValues = true
      , bool includeOpenMarketDataGapsValues = true)
    {
        IncludeOpenMarketInvalidValues  = includeOpenMarketInvalidValues || timeLengthFlags.HasIncludeOpenMarketInvalidPeriodsFlag();
        IncludeOpenMarketDataGapsValues = includeOpenMarketDataGapsValues || timeLengthFlags.HasIncludeOpenMarketDataGapPeriodsFlag();

        TimeLengthFlags = timeLengthFlags
                        | (IncludeOpenMarketInvalidValues ? TimeLengthFlags.IncludeOpenMarketInvalidPeriods : 0)
                        | (IncludeOpenMarketDataGapsValues ? TimeLengthFlags.IncludeOpenMarketDataGapPeriods : 0);
    }

    public bool IncludeOpenMarketInvalidValues  { get; }
    public bool IncludeOpenMarketDataGapsValues { get; }

    public TimeLengthFlags TimeLengthFlags { get; }
}
