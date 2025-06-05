// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.MovingAverage;

public static class MovingAverageConstants
{
    public const string MovingAverageBase                     = $"{IndicatorServiceConstants.IndicatorsBase}.MovingAverage";
    public const string MovingAverageTimeWeightedLiveTemplate = $"{MovingAverageBase}.Live.{{0}}.{{1}}";
    public const string MovingAverageTimeWeightedLivePublish  = $"{MovingAverageBase}.Live.{{0}}.{{1}}.{{2}}";

    public const string MovingAverageTimeWeightedShortPeriodRequest = $"{MovingAverageBase}.Live.{{0}}.{{1}}.TimeWeighted.ShortPeriod";

    public static string MovingAverageTimeWeightedLiveShortPeriodRequest(this SourceTickerIdentifier sourceTicker) =>
        string.Format(MovingAverageTimeWeightedShortPeriodRequest, sourceTicker.SourceName, sourceTicker.InstrumentName);

    public static string MovingAverageTimeWeightedLiveShortPeriodPublishTemplate(this SourceTickerIdentifier sourceTicker) =>
        string.Format(MovingAverageTimeWeightedLiveTemplate, sourceTicker.SourceName, sourceTicker.InstrumentName) + ".{0}";

    public static string MovingAverageTimeWeightedLiveShortPeriodPublish
        (this SourceTickerIdentifier sourceTicker, DiscreetTimePeriod discreetTimePeriod) =>
        string.Format(sourceTicker.MovingAverageTimeWeightedLiveShortPeriodPublishTemplate(), discreetTimePeriod.ShortName());
}
