// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Candles;

public static class CandleExtensions
{
    public static TimeBoundaryPeriod CalcTimeFrame(this ICandle candle)
    {
        if (candle.PeriodStartTime.Equals(DateTime.MinValue)
         || candle.PeriodEndTime < candle.PeriodStartTime)
            return TimeBoundaryPeriod.Tick;
        if (candle.PeriodEndTime.Equals(DateTime.MinValue)) return TimeBoundaryPeriod.Tick;
        if (candle.PeriodStartTime.Equals(candle.PeriodEndTime)) return TimeBoundaryPeriod.Tick;
        var diffTimeSpan = candle.PeriodEndTime - candle.PeriodStartTime;
        var totalSeconds = (int)diffTimeSpan.TotalSeconds;
        if (totalSeconds == 1) return TimeBoundaryPeriod.OneSecond;
        if (totalSeconds == 60) return TimeBoundaryPeriod.OneMinute;
        if (totalSeconds == 300) return TimeBoundaryPeriod.FiveMinutes;
        if (totalSeconds == 600) return TimeBoundaryPeriod.TenMinutes;
        if (totalSeconds == 900) return TimeBoundaryPeriod.FifteenMinutes;
        if (totalSeconds == 1800) return TimeBoundaryPeriod.ThirtyMinutes;
        if (totalSeconds == 3600) return TimeBoundaryPeriod.OneHour;
        if (totalSeconds == 14400) return TimeBoundaryPeriod.FourHours;
        if (totalSeconds == 3600 * 24) return TimeBoundaryPeriod.OneDay;
        if (totalSeconds >= 3600 * 24 * 5
         && totalSeconds <= 7 * 24 * 3600)
            return TimeBoundaryPeriod.OneWeek;
        if (totalSeconds >= 28 * 24 * 3600
         && totalSeconds <= 31 * 24 * 3600)
            return TimeBoundaryPeriod.OneMonth;
        if (totalSeconds >= 365 * 24 * 3600
         && totalSeconds <= 366 * 24 * 3600)
            return TimeBoundaryPeriod.OneYear;

        return TimeBoundaryPeriod.Tick;
    }

    public static IChannelLimitedEventFactory<Candle> CreateChannelFactory
        (this IListeningRule rule, Func<ChannelEvent<Candle>, bool> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
    {
        var limitedEventChannelFactory
            = rule.Context.PooledRecycler.Borrow<ChannelWrappingLimitedEventFactory<Candle, Candle>>();

        return limitedEventChannelFactory.Configure(new InterQueueChannel<Candle>(rule, receiveQuoteHandler), limitedRecycler);
    }

    public static IChannelLimitedEventFactory<Candle> CreateChannelFactory
        (this IListeningRule rule, Func<ChannelEvent<Candle>, ValueTask<bool>> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
    {
        var limitedEventChannelFactory
            = rule.Context.PooledRecycler.Borrow<ChannelWrappingLimitedEventFactory<Candle, Candle>>();
        return limitedEventChannelFactory.Configure(new InterQueueChannel<Candle>(rule, receiveQuoteHandler), limitedRecycler);
    }

    public static ChannelPublishRequest<Candle> ToChannelPublishRequest
        (this IChannel<Candle> channelFactory, int resultLimit = -1, int batchSize = 1) =>
        new(channelFactory, resultLimit, batchSize);

    public static int AddReplace
        (this IDoublyLinkedList<Candle> existing, IDoublyLinkedList<Candle> toAdd)
    {
        var removedCount = 0;
        var currentAdd   = toAdd.Head;

        while (currentAdd != null)
        {
            removedCount += AddReplace(existing, currentAdd);

            currentAdd = currentAdd.Next;
        }
        return removedCount;
    }

    public static int AddReplace
        (this IDoublyLinkedList<Candle> existing, Candle toAddReplace)
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
            if (toAddReplace.IsWhollyBoundedBy(currentExisting)) break;
            if (currentExisting.PeriodStartTime > toAddReplace.PeriodStartTime)
            {
                var insertAfter = currentExisting.Previous;
                while (currentExisting != null && currentExisting.IsWhollyBoundedBy(toAddReplace))
                {
                    if (ReferenceEquals(currentExisting, toAddReplace)) return removedCount;
                    var removed = existing.Remove(currentExisting);
                    removed.DecrementRefCount();
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
