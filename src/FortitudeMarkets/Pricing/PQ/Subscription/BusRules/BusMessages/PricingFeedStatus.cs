// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Subscription.BusRules.BusMessages;

public enum PricingFeedStatus
{
    NotStarted
  , Starting
  , AvailableForSubscription
  , Ticking
  , NothingTicking
  , Stopping
}

public struct PricingFeedStatusRequest
{
    public string FeedName { get; set; }
}

public class PricingFeedStatusResponse : RecyclableObject
{
    public string   FeedName  { get; set; } = null!;
    public DateTime StartTime { get; set; }

    public PricingFeedStatus FeedStatus { get; set; }

    public DateTime LastTickerRefreshTime { get; set; }

    public List<ISourceTickerInfo> AvailableTickersTickers    { get; set; } = new();
    public List<ISourceTickerInfo> HealthySubscribedTickers   { get; set; } = new();
    public List<ISourceTickerInfo> UnhealthySubscribedTickers { get; set; } = new();

    public override void StateReset()
    {
        FeedName  = null!;
        StartTime = DateTime.MaxValue;

        LastTickerRefreshTime = DateTime.MinValue;
        AvailableTickersTickers.Clear();
        HealthySubscribedTickers.Clear();
        UnhealthySubscribedTickers.Clear();
        base.StateReset();
    }

    public override string ToString() =>
        $"{nameof(PricingFeedStatusResponse)}({nameof(FeedName)}: {FeedName}, {nameof(StartTime)}: {StartTime}, {nameof(FeedStatus)}: {FeedStatus}, " +
        $"{nameof(LastTickerRefreshTime)}: {LastTickerRefreshTime}, {nameof(AvailableTickersTickers)}: {AvailableTickersTickers}, " +
        $"{nameof(HealthySubscribedTickers)}: {HealthySubscribedTickers}, {nameof(UnhealthySubscribedTickers)}: {UnhealthySubscribedTickers})";
}
