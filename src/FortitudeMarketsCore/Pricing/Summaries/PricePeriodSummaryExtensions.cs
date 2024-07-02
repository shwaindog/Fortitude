// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.Summaries;

public static class PricePeriodSummaryExtensions
{
    public static TimeSeriesPeriod CalcTimeFrame(this IPricePeriodSummary pricePeriodSummary)
    {
        if (pricePeriodSummary.PeriodStartTime.Equals(DateTimeConstants.UnixEpoch)
         || pricePeriodSummary.PeriodEndTime < pricePeriodSummary.PeriodStartTime) return TimeSeriesPeriod.None;
        if (pricePeriodSummary.PeriodEndTime.Equals(DateTimeConstants.UnixEpoch)) return TimeSeriesPeriod.None;
        if (pricePeriodSummary.PeriodStartTime.Equals(pricePeriodSummary.PeriodEndTime)) return TimeSeriesPeriod.Tick;
        var diffTimeSpan = pricePeriodSummary.PeriodEndTime - pricePeriodSummary.PeriodStartTime;
        var totalSeconds = (int)diffTimeSpan.TotalSeconds;
        if (totalSeconds == 1) return TimeSeriesPeriod.OneSecond;
        if (totalSeconds == 60) return TimeSeriesPeriod.OneMinute;
        if (totalSeconds == 300) return TimeSeriesPeriod.FiveMinutes;
        if (totalSeconds == 600) return TimeSeriesPeriod.TenMinutes;
        if (totalSeconds == 900) return TimeSeriesPeriod.FifteenMinutes;
        if (totalSeconds == 1800) return TimeSeriesPeriod.ThirtyMinutes;
        if (totalSeconds == 3600) return TimeSeriesPeriod.OneHour;
        if (totalSeconds == 14400) return TimeSeriesPeriod.FourHours;
        if (totalSeconds == 3600 * 24) return TimeSeriesPeriod.OneDay;
        if (totalSeconds >= 3600 * 24 * 5
         && totalSeconds <= 7 * 24 * 3600) return TimeSeriesPeriod.OneWeek;
        if (totalSeconds >= 28 * 24 * 3600
         && totalSeconds <= 31 * 24 * 3600) return TimeSeriesPeriod.OneMonth;
        if (totalSeconds >= 365 * 24 * 3600
         && totalSeconds <= 366 * 24 * 3600) return TimeSeriesPeriod.OneYear;

        return TimeSeriesPeriod.None;
    }

    public static IChannelLimitedEventFactory<IPricePeriodSummary> CreateChannelFactory
        (this IListeningRule rule, Func<ChannelEvent<IPricePeriodSummary>, bool> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
    {
        var limitedEventChannelFactory
            = rule.Context.PooledRecycler.Borrow<ChannelWrappingLimitedEventFactory<IPricePeriodSummary, PricePeriodSummary>>();

        return limitedEventChannelFactory.Configure(new InterQueueChannel<IPricePeriodSummary>(rule, receiveQuoteHandler), limitedRecycler);
    }

    public static IChannelLimitedEventFactory<IPricePeriodSummary> CreateChannelFactory
        (this IListeningRule rule, Func<ChannelEvent<IPricePeriodSummary>, ValueTask<bool>> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
    {
        var limitedEventChannelFactory
            = rule.Context.PooledRecycler.Borrow<ChannelWrappingLimitedEventFactory<IPricePeriodSummary, PricePeriodSummary>>();
        return limitedEventChannelFactory.Configure(new InterQueueChannel<IPricePeriodSummary>(rule, receiveQuoteHandler), limitedRecycler);
    }

    public static ChannelPublishRequest<IPricePeriodSummary> ToChannelPublishRequest
        (this IChannel<IPricePeriodSummary> channelFactory, int resultLimit = -1, int batchSize = 1) =>
        new(channelFactory, resultLimit, batchSize);
}
