// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Indicators.Pricing;
using FortitudeMarkets.Indicators.Pricing.MovingAverage;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Parameters;

public enum RequestExceedTimeRangeOptions
{
    GrowToHardLimitTimeRange
  , GrowUnlimited
  , RejectRequest
}

public struct LiveShortPeriodMovingAveragePublishParams
{
    public LiveShortPeriodMovingAveragePublishParams
    (PricingIndicatorId indicatorSourceTickerIdentifier
      , MovingAveragePublisherParams? initialPeriodsToPublish = null
      , CalculateMovingAverageOptions? calculateMovingAverageOptions = null
      , RequestExceedTimeRangeOptions requestExceedTimeRangeOptions = RequestExceedTimeRangeOptions.RejectRequest
      , UnboundedTimeSpanRange? initialTickTimeRange = null
      , TickGapOptions? markTipGapOptions = null
      , TimeSpan? trimExpiredTicksInterval = null)
    {
        IndicatorSourceTickerIdentifier      = indicatorSourceTickerIdentifier;
        DefaultCalculateMovingAverageOptions = calculateMovingAverageOptions ?? new CalculateMovingAverageOptions();

        TrimExpiredTicksInterval  = trimExpiredTicksInterval ?? TimeSpan.FromSeconds(23);
        TimeRangeExtensionOptions = requestExceedTimeRangeOptions;
        MarkTipGapOptions         = markTipGapOptions ?? new TickGapOptions();

        LimitMaxTicksInitialToHardTimeSpanRange
            = initialTickTimeRange ?? new UnboundedTimeSpanRange(TimeSpan.FromMinutes(31), TimeSpan.FromMinutes(31));
        if (initialPeriodsToPublish == null && LimitMaxTicksInitialToHardTimeSpanRange.LowerLimit != null)
        {
            var defaultPublishPeriods = new List<MovingAverageOffset>();
            foreach (var timeSeriesPeriod in LimitMaxTicksInitialToHardTimeSpanRange.LowerLimit.Value.NonTickTimeSeriesPeriodsSmallerThan())
            {
                var periodMovingAverage = new MovingAverageOffset(new DiscreetTimePeriod(timeSeriesPeriod));
                defaultPublishPeriods.Add(periodMovingAverage);
            }
            if (defaultPublishPeriods.Any())
            {
                var periodsToPublishParams = new MovingAveragePublisherParams
                    (new IndicatorPublishInterval(TimeBoundaryPeriod.OneSecond), null, calculateMovingAverageOptions
                   , defaultPublishPeriods.ToArray());
                InitialPeriodsToPublish = periodsToPublishParams;
            }
        }
        else
        {
            InitialPeriodsToPublish = initialPeriodsToPublish;
        }
    }

    public PricingIndicatorId IndicatorSourceTickerIdentifier { get; }

    public MovingAveragePublisherParams? InitialPeriodsToPublish { get; }

    public CalculateMovingAverageOptions DefaultCalculateMovingAverageOptions { get; }

    public UnboundedTimeSpanRange LimitMaxTicksInitialToHardTimeSpanRange { get; }

    public TickGapOptions MarkTipGapOptions { get; }

    public TimeSpan TrimExpiredTicksInterval { get; }

    public RequestExceedTimeRangeOptions TimeRangeExtensionOptions { get; }
}
