// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeMarkets.Config.Availability;

public readonly struct WeekdayStartStopPair(DateTimeOffset startTime, DateTimeOffset stopTime) : IEquatable<WeekdayStartStopPair>
{
    public DateTimeOffset StartTime { get; } = startTime;
    public DateTimeOffset StopTime  { get; } = stopTime;

    public bool Equals(WeekdayStartStopPair other) => StartTime.Equals(other.StartTime) && StopTime.Equals(other.StopTime);

    public override bool Equals(object? obj) => obj is WeekdayStartStopPair other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (StartTime.GetHashCode() * 397) ^ StopTime.GetHashCode();
        }
    }

    public static bool operator ==(WeekdayStartStopPair lhs, WeekdayStartStopPair rhs) =>
       lhs.StartTime == rhs.StartTime && lhs.StopTime == rhs.StopTime;

    public static bool operator !=(WeekdayStartStopPair lhs, WeekdayStartStopPair rhs) => !(lhs == rhs);
}


public class WeekdayStartStopPairList : ReusableList<WeekdayStartStopPair>
{
    public WeekdayStartStopPairList() { }
    public WeekdayStartStopPairList(IRecycler recycler, int size = 16) : base(recycler, size) { }
    protected WeekdayStartStopPairList(ReusableList<WeekdayStartStopPair> toClone) : base(toClone) { }

    public WeekdayStartStopPair? NextStartStopPairStartingAfter(DateTimeOffset fromTime)
    {
        if (!this.Any()) return null;
        var previous = this[0];
        if (fromTime < previous.StartTime) return previous;
        for (int i = 1; i < Count; i++)
        {
            var current = this[i];
            if (fromTime < current.StartTime) return current;
        }
        return null;
    }

    public WeekdayStartStopPair? NextStartStopPairEndingAfter(DateTimeOffset fromTime)
    {
        if (!this.Any()) return null;
        var previous = this[0];
        if (fromTime < previous.StopTime) return previous;
        for (int i = 1; i < Count; i++)
        {
            var current = this[i];
            if (fromTime < current.StopTime) return current;
        }
        return null;
    }

    public IEnumerable<WeekdayStartStopPair> WeekdaysStartingOrEndingAfter(DateTimeOffset fromTime)
    {
        var ending = NextStartStopPairEndingAfter(fromTime);
        if (ending != null && ending.Value.WithinOpeningHours(fromTime))
        {
            yield return ending.Value;
        }
        var starting = NextStartStopPairStartingAfter(fromTime);
        if (starting != null && starting != ending && starting.Value.WithinOpeningHours(fromTime))
        {
            yield return starting.Value;
        }
    }
}


public class WeekDayOpenClosePairComparer : IComparer<WeekdayStartStopPair>
{
    public int Compare(WeekdayStartStopPair x, WeekdayStartStopPair y) =>
        x.StartTime.CompareTo(y.StartTime);
}

public static class WeekDayOpenClosePairExtensions
{
    public static bool WithinOpeningHours(this WeekdayStartStopPair weekdayStartStopPair, DateTimeOffset dateTime) => 
        dateTime >= weekdayStartStopPair.StartTime && dateTime < weekdayStartStopPair.StopTime;

    public static bool WithinOpeningHours(this WeekdayStartStopPair weekdayStartStopPair, DateTime dateTime) => 
        dateTime >= weekdayStartStopPair.StartTime && dateTime < weekdayStartStopPair.StopTime;
}
