// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
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
         || pricePeriodSummary.PeriodEndTime < pricePeriodSummary.PeriodStartTime)
            return TimeSeriesPeriod.None;
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
         && totalSeconds <= 7 * 24 * 3600)
            return TimeSeriesPeriod.OneWeek;
        if (totalSeconds >= 28 * 24 * 3600
         && totalSeconds <= 31 * 24 * 3600)
            return TimeSeriesPeriod.OneMonth;
        if (totalSeconds >= 365 * 24 * 3600
         && totalSeconds <= 366 * 24 * 3600)
            return TimeSeriesPeriod.OneYear;

        return TimeSeriesPeriod.None;
    }

    public static IChannelLimitedEventFactory<PricePeriodSummary> CreateChannelFactory
        (this IListeningRule rule, Func<ChannelEvent<PricePeriodSummary>, bool> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
    {
        var limitedEventChannelFactory
            = rule.Context.PooledRecycler.Borrow<ChannelWrappingLimitedEventFactory<PricePeriodSummary, PricePeriodSummary>>();

        return limitedEventChannelFactory.Configure(new InterQueueChannel<PricePeriodSummary>(rule, receiveQuoteHandler), limitedRecycler);
    }

    public static IChannelLimitedEventFactory<PricePeriodSummary> CreateChannelFactory
        (this IListeningRule rule, Func<ChannelEvent<PricePeriodSummary>, ValueTask<bool>> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
    {
        var limitedEventChannelFactory
            = rule.Context.PooledRecycler.Borrow<ChannelWrappingLimitedEventFactory<PricePeriodSummary, PricePeriodSummary>>();
        return limitedEventChannelFactory.Configure(new InterQueueChannel<PricePeriodSummary>(rule, receiveQuoteHandler), limitedRecycler);
    }

    public static ChannelPublishRequest<PricePeriodSummary> ToChannelPublishRequest
        (this IChannel<PricePeriodSummary> channelFactory, int resultLimit = -1, int batchSize = 1) =>
        new(channelFactory, resultLimit, batchSize);

    public static int AddReplace
        (this IDoublyLinkedList<PricePeriodSummary> existing, IDoublyLinkedList<PricePeriodSummary> toAdd, IRecycler? recycler = null)
    {
        var removedCount = 0;
        var currentAdd   = toAdd.Head;

        while (currentAdd != null)
        {
            removedCount += AddReplace(existing, currentAdd, recycler);

            currentAdd = currentAdd.Next;
        }
        return removedCount;
    }

    public static int AddReplace
        (this IDoublyLinkedList<PricePeriodSummary> existing, PricePeriodSummary toAddReplace, IRecycler? recycler = null)
    {
        var currentExisting = existing.Head;
        if (currentExisting == null)
        {
            existing.AddFirst(toAddReplace);
            return 0;
        }
        var removedCount = 0;
        while (currentExisting != null)
        {
            if (ReferenceEquals(currentExisting, toAddReplace)) break;
            if (currentExisting.PeriodStartTime > toAddReplace.PeriodStartTime)
            {
                var insertAfter = currentExisting.Previous;
                while (currentExisting != null && currentExisting.IsWhollyBoundedBy(toAddReplace))
                {
                    if (ReferenceEquals(currentExisting, toAddReplace)) return removedCount;
                    var removed = existing.Remove(currentExisting);
                    recycler?.Recycle(removed);
                    removedCount++;
                    currentExisting = currentExisting.Next;
                }
                if (ReferenceEquals(insertAfter, toAddReplace)) break;
                if (insertAfter != null)
                {
                    insertAfter.Next      = toAddReplace;
                    toAddReplace.Previous = insertAfter;
                }
                else
                {
                    existing.AddFirst(toAddReplace);
                }
                if (currentExisting != null)
                {
                    currentExisting.Previous = toAddReplace;
                    toAddReplace.Next        = currentExisting;
                }
                else
                {
                    toAddReplace.Next = null;
                }
            }
            if (currentExisting?.Next == null)
            {
                existing.AddLast(toAddReplace);
                return removedCount;
            }

            currentExisting = currentExisting.Next;
        }
        return removedCount;
    }
}
