// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public static class MovingAverageConstants
{
    public const string MovingAverageBase  = $"{IndicatorServiceConstants.IndicatorsBase}.MovingAverage";
    public const string MovingLiveTemplate = $"{MovingAverageBase}.Live.{{0}}.{{1}}.{{2}}";
}
