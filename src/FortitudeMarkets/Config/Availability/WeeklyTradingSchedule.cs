// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Config.Availability;

public readonly struct AvailabilityTransitionTime(DateTimeOffset atTime , TradingPeriodTypeFlags currentStateState)
{
    public static readonly AvailabilityTransitionTimeComparer Comparer = new();

    public DateTimeOffset AtTime { get; } = atTime;

    public TradingPeriodTypeFlags MarketState { get; } = currentStateState;

    public override string ToString() => $"{nameof(AvailabilityTransitionTime)}{{{nameof(AtTime)}: {AtTime}, {nameof(MarketState)}: {MarketState}}}";
}

public static class AvailabilityTransitionTimeExtensions
{
    public static AvailabilityTransitionTime WithNewState(this AvailabilityTransitionTime toReplaceState, TradingPeriodTypeFlags newTradingState) =>
        new(toReplaceState.AtTime, newTradingState);

    public static AvailabilityTransitionTime AddTradingState(this AvailabilityTransitionTime toAmend, TradingPeriodTypeFlags addTradingStateFlags) =>
        new(toAmend.AtTime, toAmend.MarketState | addTradingStateFlags);

    public static bool IsOpenTransition(this AvailabilityTransitionTime previousTransition, AvailabilityTransitionTime nextTransition) =>
        previousTransition.MarketState.IsMarketClose() && nextTransition.MarketState.IsOpen();

    public static bool IsClosedTransition(this AvailabilityTransitionTime previousTransition, AvailabilityTransitionTime nextTransition) =>
        previousTransition.MarketState.IsOpen() && nextTransition.MarketState.IsMarketClose();
}

public class AvailabilityTransitionTimeComparer : IComparer<AvailabilityTransitionTime>
{
    private static readonly int TicksInOneMilli = (int)TimeSpan.FromMilliseconds(1).Ticks;

    public int Compare(AvailabilityTransitionTime lhs, AvailabilityTransitionTime rhs)
    {
        var diff       = (lhs.AtTime.UtcTicks) - (rhs.AtTime.UtcTicks);
        var diffOverMs = (int)(diff / TicksInOneMilli);
        if (diffOverMs != 0) return diffOverMs;
        var diffUnderMs = diff % TicksInOneMilli;
        return (int)diffUnderMs;
    }

    public int Compare(DateTimeOffset lhs, AvailabilityTransitionTime rhs)
    {
        var diff       = (lhs.UtcTicks) - (rhs.AtTime.UtcTicks);
        var diffOverMs = (int)(diff / TicksInOneMilli);
        if (diffOverMs != 0) return diffOverMs;
        var diffUnderMs = diff % TicksInOneMilli;
        return (int)diffUnderMs;
    }
}

public class WeeklyTradingSchedule : ReusableList<AvailabilityTransitionTime>, ICloneable<WeeklyTradingSchedule>
{
    public WeeklyTradingSchedule()
    {
        RequestTimeZoneInfo = TimeZoneInfo.Utc;
    }

    public WeeklyTradingSchedule(IRecycler recycler, int size = 16) : base(recycler, size)
    {
        RequestTimeZoneInfo = TimeZoneInfo.Utc;
    }

    protected WeeklyTradingSchedule(WeeklyTradingSchedule toClone) : base(toClone)
    {
        RequestTime         = toClone.RequestTime;
        RequestTimeZoneInfo = toClone.RequestTimeZoneInfo;
    }

    public WeeklyTradingSchedule Initialise(DateTime requestTime, TimeZoneInfo? forTimeZone = null)
    {
        Clear();
        RequestTime         = new DateTimeOffset(requestTime);
        RequestTimeZoneInfo = forTimeZone ?? TimeZoneInfo.Local;
        return this;
    }

    public WeeklyTradingSchedule Initialise(DateTimeOffset requestTime, TimeZoneInfo? forTimeZone = null)
    {
        Clear();
        RequestTime         = requestTime;
        RequestTimeZoneInfo = forTimeZone ?? TimeZoneInfo.Local;
        return this;
    }

    public DateTimeOffset RequestTime { get; private set; }

    public DateTimeOffset WeekStartingTime => RequestTime.TruncToWeekBoundary();

    public TimeZoneInfo RequestTimeZoneInfo { get; private set; }

    public override void Add(AvailabilityTransitionTime item)
    {
        var comparer = AvailabilityTransitionTime.Comparer;

        for (var i = 0; i < Count; i++)
        {
            var next = this[i];
            if (comparer.Compare(item, next) >= 0) continue;
            Insert(i, item);
            return;
        }
        base.Add(item);
    }

    public int SortInsertionIndex(int fromIndex, DateTimeOffset atTime)
    {
        var comparer = AvailabilityTransitionTime.Comparer;
        for (int i = fromIndex; i < Count; i++)
        {
            var checkAtIndex = this[i];
            if (comparer.Compare(atTime, checkAtIndex) >= 0) continue;
            return i;
        }
        return Count;
    }

    public AvailabilityTransitionTime FindEntryActiveAt(DateTimeOffset atTime)
    {
        var comparer = AvailabilityTransitionTime.Comparer;

        AvailabilityTransitionTime previous = this[0];
        for (var i = 1; i < Count; i++)
        {
            var current = this[i];
            if (comparer.Compare(atTime, current) < 0)
            {
                return previous;
            }
            previous = current;
        }
        return previous;
    }

    public int FindTimeMatchAt(DateTimeOffset atTime)
    {
        var comparer = AvailabilityTransitionTime.Comparer;
        for (var i = 0; i < Count; i++)
        {
            var checkAtIndex = this[i];
            if (comparer.Compare(atTime, checkAtIndex) == 0) return i;
        }
        return -1;
    }

    public AvailabilityTransitionTime CurrentActiveAvailabilityTransition(DateTimeOffset atTime)
    {
        AvailabilityTransitionTime lastAvailability
            = new AvailabilityTransitionTime(atTime.TruncToWeekBoundary(), TradingPeriodTypeFlags.IsMarketClosed | TradingPeriodTypeFlags.IsWeekend);
        for (int i = 0; i < Count; i++)
        {
            var currentAvailability = this[i];
            if (currentAvailability.AtTime > atTime)
            {
                return lastAvailability;
            }
            lastAvailability = currentAvailability;
        }
        return lastAvailability;
    }

    private static readonly Func<TradingPeriodTypeFlags, bool> AlwaysMatch = _ => true;

    public AvailabilityTransitionTime NextTransitionMatching(DateTimeOffset atTime, Func<TradingPeriodTypeFlags, bool>? predicate = null)
    {
        predicate ??= AlwaysMatch;
        AvailabilityTransitionTime lastAvailability
            = new AvailabilityTransitionTime(atTime.TruncToWeekBoundary(), TradingPeriodTypeFlags.IsMarketClosed | TradingPeriodTypeFlags.IsWeekend);
        for (int i = 0; i < Count; i++)
        {
            var currentAvailability = this[i];
            if (currentAvailability.AtTime > atTime && predicate(currentAvailability.MarketState))
            {
                return currentAvailability;
            }
            lastAvailability = currentAvailability;
        }
        return lastAvailability;
    }

    public override void StateReset()
    {
        RequestTime         = default;
        RequestTimeZoneInfo = TimeZoneInfo.Utc;
        base.StateReset();
    }

    public override WeeklyTradingSchedule Clone() =>
        Recycler?.Borrow<WeeklyTradingSchedule>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new WeeklyTradingSchedule(this);

    public override WeeklyTradingSchedule CopyFrom(IReusableList<AvailabilityTransitionTime> source, 
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is WeeklyTradingSchedule weeklyTradingSchedule)
        {
            RequestTime         = weeklyTradingSchedule.RequestTime;
            RequestTimeZoneInfo = weeklyTradingSchedule.RequestTimeZoneInfo;
        }
        return this;
    }

    public override string ToString() => 
        $"{nameof(WeeklyTradingSchedule)}{{{nameof(RequestTime)}: {RequestTime}, {nameof(WeekStartingTime)}: {WeekStartingTime}, " +
        $"{nameof(RequestTimeZoneInfo)}: {RequestTimeZoneInfo}, {ReusableListItemsOnNewLineToString} }}";
}
