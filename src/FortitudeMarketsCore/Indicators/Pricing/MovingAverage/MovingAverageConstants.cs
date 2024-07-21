// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public static class MovingAverageConstants
{
    public const string MovingAverageBase         = $"{IndicatorServiceConstants.IndicatorsBase}.MovingAverage";
    public const string MovingAverageLiveTemplate = $"{MovingAverageBase}.Live.{{0}}.{{1}}.{{2}}";
    public const string MovingAverageShortPeriod  = $"{MovingAverageBase}.Live.{{0}}.{{1}}.ShortPeriod";


    public static string MovingAverageLiveShortPeriodRequest
        (this SourceTickerIdentifier sourceTicker) =>
        string.Format(MovingAverageShortPeriod, sourceTicker.Source, sourceTicker.Ticker);
}
