// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Config.Availability;

public readonly struct CalendarDate(ushort year, byte month = 1, byte day = 1) : IEquatable<CalendarDate>
{
    public ushort Year  { get; } = year;
    public byte   Month { get; } = month;
    public byte   Day   { get; } = day;

    public static bool operator ==(CalendarDate lhs, CalendarDate rhs)
    {
        return lhs.Year == rhs.Year && lhs.Month == rhs.Month && lhs.Day == rhs.Day;
    }

    public static bool operator !=(CalendarDate lhs, CalendarDate rhs)
    {
        return !(lhs == rhs);
    }

    public bool Equals(CalendarDate other) => Year == other.Year && Month == other.Month && Day == other.Day;

    public override bool Equals(object? obj) => obj is CalendarDate other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Year.GetHashCode();
            hashCode = (hashCode * 397) ^ Month.GetHashCode();
            hashCode = (hashCode * 397) ^ Day.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => 
        $"{nameof(CalendarDate)}{{{nameof(Year)}: {Year}, {nameof(Month)}: {Month}, {nameof(Day)}: {Day}}}";
}


public class CalendarDatesList : ReusableList<CalendarDate>, ICloneable<CalendarDatesList>
{
    public CalendarDatesList() { }
    public CalendarDatesList(IRecycler recycler, int size = 16) : base(recycler, size) { }

    public CalendarDatesList(CalendarDatesList toClone) : base(toClone) { }

    public CalendarDate NextDateNotInList(CalendarDate fromThisDate)
    {
        var indexOfThis = IndexOf(fromThisDate);
        if (indexOfThis < 0) return fromThisDate;
        var previous = fromThisDate;
        for (var i = indexOfThis; i < Count; i++)
        {
            var next = this[i];
            if (previous.ToDaysApart(next) > 1)
            {
                return previous.AddDays(1);
            }
            previous = next;
        }
        return previous.AddDays();
    }

    public CalendarDate PreviousDateNotInList(CalendarDate fromThisDate)
    {
        var indexOfThis = IndexOf(fromThisDate);
        if (indexOfThis < 0) return fromThisDate;
        var previous = fromThisDate;
        for (var i = indexOfThis - 1; i > 0; i--)
        {
            var next = this[i];
            if (previous.ToDaysApart(next) < 1)
            {
                return previous.AddDays(-1);
            }
            previous = next;
        }
        return previous.AddDays();
    }

    public bool Contains(DateTimeOffset checkIsContained)
    {
        return Contains(new CalendarDate((ushort)checkIsContained.Year, (byte)checkIsContained.Month, (byte)checkIsContained.Day));
    }
    
    public override void Add(CalendarDate item)
    {
        var comparer = CalendarDateExtensions.CalendarDateComparer;

        for (var i = 0; i < Count; i++)
        {
            var next = this[i];
            if (comparer.Compare(item, next) >= 0) continue;
            Insert(i, item);
            return;
        }
        base.Add(item);
    }

    public override CalendarDatesList Clone() => 
        Recycler?.Borrow<CalendarDatesList>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new CalendarDatesList(this);

    public override CalendarDatesList CopyFrom(IReusableList<CalendarDate> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        return this;
    }
}

public class CalendarDateComparer : IComparer<CalendarDate>
{
    public int Compare(CalendarDate lhs, CalendarDate rhs) => (lhs.Year * 10000 + lhs.Month * 100 + lhs.Day) - (rhs.Year * 10000 + rhs.Month * 100 + rhs.Day);
}


public static class CalendarDateExtensions
{
    public static  CalendarDateComparer CalendarDateComparer { get; } = new ();

    public static DateTime ToDateTime(this CalendarDate calendarDate) => new (calendarDate.Year, calendarDate.Month, calendarDate.Day);

    public static ushort ToDaysApart(this CalendarDate fromCalendarDate, CalendarDate toCalendarDate) =>
        (ushort)(new DateTime(toCalendarDate.Year, toCalendarDate.Month, toCalendarDate.Day) 
               - new DateTime(fromCalendarDate.Year, fromCalendarDate.Month, fromCalendarDate.Day)).TotalDays;

    public static CalendarDate AddDays(this CalendarDate fromCalendarDate, int daysToAdd = 1)
    {
        var nextDateTime = new DateTime(fromCalendarDate.Year, fromCalendarDate.Month, fromCalendarDate.Day).AddDays(daysToAdd);
        return new CalendarDate((ushort)nextDateTime.Year, (byte)nextDateTime.Month, (byte)nextDateTime.Day);
    }

    public static DateTimeOffset ToDateTimeOffset(this CalendarDate calendarDate) => new (calendarDate.ToDateTime());
}
