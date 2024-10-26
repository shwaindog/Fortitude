// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeMarkets.Indicators.Pricing.MovingAverage;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Parameters;

public struct MovingAveragePublisherParams
{
    public MovingAveragePublisherParams
    (IndicatorPublishInterval publishFrequency, ResponsePublishParams? publishParams = null
      , CalculateMovingAverageOptions? calculateMovingAverageOptions = null
      , params MovingAverageOffset[] periodsToPublish)
    {
        PublishFrequency = publishFrequency;
        PeriodsToPublish = periodsToPublish;

        PublishParams = publishParams ?? new ResponsePublishParams();

        CalculateMovingAverageOptions = calculateMovingAverageOptions;

        RequestId = Interlocked.Increment(ref MovingAveragePublisherParamsExtensions.AllRequestIds);
    }

    public int RequestId { get; }

    public MovingAverageOffset[] PeriodsToPublish { get; }

    public CalculateMovingAverageOptions? CalculateMovingAverageOptions { get; }

    public IndicatorPublishInterval PublishFrequency { get; }

    public ResponsePublishParams PublishParams { get; }
}
