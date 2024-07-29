// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.Parameters;

public enum RequestExceedTimeRangeOptions
{
    GrowToHardLimitTimeRange
  , GrowUnlimited
  , RejectRequest
}

public struct LiveShortPeriodMovingAveragePublishParams
{
    public LiveShortPeriodMovingAveragePublishParams
    (IndicatorSourceTickerIdentifier indicatorSourceTickerIdentifier, UnboundedTimeSpanRange? initialTickTimeRange = null
      , MovingAveragePublisherParams? initialPeriodsToPublish = null
      , RequestExceedTimeRangeOptions requestExceedTimeRangeOptions = RequestExceedTimeRangeOptions.RejectRequest
      , CalculateMovingAverageOptions? calculateMovingAverageOptions = null
      , TickGapOptions? markTipGapOptions = null)
    {
        IndicatorSourceTickerIdentifier      = indicatorSourceTickerIdentifier;
        DefaultCalculateMovingAverageOptions = calculateMovingAverageOptions ?? new CalculateMovingAverageOptions();

        TimeRangeExtensionOptions = requestExceedTimeRangeOptions;
        MarkTipGapOptions         = markTipGapOptions ?? new TickGapOptions();

        LimitMaxTicksInitialToHardTimeSpanRange
            = initialTickTimeRange ?? new UnboundedTimeSpanRange(TimeSpan.FromMinutes(31), TimeSpan.FromMinutes(31));
        if (initialPeriodsToPublish == null && LimitMaxTicksInitialToHardTimeSpanRange.LowerLimit != null)
        {
            var defaultPublishPeriods = new List<MovingAverageOffset>();
            foreach (var timeSeriesPeriod in LimitMaxTicksInitialToHardTimeSpanRange.LowerLimit.Value.TimeSeriesPeriodsSmallerThan())
            {
                var periodMovingAverage = new MovingAverageOffset(new TimePeriod(timeSeriesPeriod));
                defaultPublishPeriods.Add(periodMovingAverage);
            }
            if (defaultPublishPeriods.Any())
            {
                var periodsToPublishParams = new MovingAveragePublisherParams
                    (new IndicatorPublishInterval(TimeSeriesPeriod.OneSecond), null, calculateMovingAverageOptions
                   , defaultPublishPeriods.ToArray());
                InitialPeriodsToPublish = periodsToPublishParams;
            }
        }
        else
        {
            InitialPeriodsToPublish = initialPeriodsToPublish;
        }
    }

    public IndicatorSourceTickerIdentifier IndicatorSourceTickerIdentifier { get; }

    public MovingAveragePublisherParams? InitialPeriodsToPublish { get; }

    public CalculateMovingAverageOptions DefaultCalculateMovingAverageOptions { get; }

    public UnboundedTimeSpanRange LimitMaxTicksInitialToHardTimeSpanRange { get; }

    public TickGapOptions MarkTipGapOptions { get; }

    public RequestExceedTimeRangeOptions TimeRangeExtensionOptions { get; }
}
