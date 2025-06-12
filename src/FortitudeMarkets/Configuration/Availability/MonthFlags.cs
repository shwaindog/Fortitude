// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Configuration.Availability;

[Flags]
public enum MonthFlags : ushort
{
    None   = 0
  , January   = 0x00_01
  , February  = 0x00_02
  , March     = 0x00_04
  , April     = 0x00_08
  , May       = 0x00_10
  , June      = 0x00_20
  , July      = 0x00_40
  , August    = 0x00_80
  , September = 0x01_00
  , October   = 0x02_00
  , November  = 0x04_00
  , December  = 0x08_00
  , AllMonths = 0x0F_FF
}

public static class MonthFlagsExtensions
{
    public static bool HasJanuary(this MonthFlags flags)   => (flags & MonthFlags.January) > 0;
    public static bool HasFebruary(this MonthFlags flags)  => (flags & MonthFlags.February) > 0;
    public static bool HasMarch(this MonthFlags flags)     => (flags & MonthFlags.March) > 0;
    public static bool HasApril(this MonthFlags flags)     => (flags & MonthFlags.April) > 0;
    public static bool HasMay(this MonthFlags flags)       => (flags & MonthFlags.May) > 0;
    public static bool HasJune(this MonthFlags flags)      => (flags & MonthFlags.June) > 0;
    public static bool HasJuly(this MonthFlags flags)      => (flags & MonthFlags.July) > 0;
    public static bool HasAugust(this MonthFlags flags)    => (flags & MonthFlags.August) > 0;
    public static bool HasSeptember(this MonthFlags flags) => (flags & MonthFlags.September) > 0;
    public static bool HasOctober(this MonthFlags flags)   => (flags & MonthFlags.October) > 0;
    public static bool HasNovember(this MonthFlags flags)  => (flags & MonthFlags.November) > 0;
    public static bool HasDecember(this MonthFlags flags)  => (flags & MonthFlags.December) > 0;

    public static int ToCalendarMonth(this MonthFlags toConvert)
    {
        if (toConvert.HasJanuary())
        {
            if ((toConvert & ~MonthFlags.January) > 0) throw new ArgumentException("Has more than one month in flags");
            return 1;
        }
        if (toConvert.HasFebruary())
        {
            if ((toConvert & ~MonthFlags.February) > 0) throw new ArgumentException("Has more than one month in flags");
            return 2;
        }
        if (toConvert.HasMarch())
        {
            if ((toConvert & ~MonthFlags.March) > 0) throw new ArgumentException("Has more than one month in flags");
            return 3;
        }
        if (toConvert.HasApril())
        {
            if ((toConvert & ~MonthFlags.April) > 0) throw new ArgumentException("Has more than one month in flags");
            return 4;
        }
        if (toConvert.HasMay())
        {
            if ((toConvert & ~MonthFlags.May) > 0) throw new ArgumentException("Has more than one month in flags");
            return 5;
        }
        if (toConvert.HasJune())
        {
            if ((toConvert & ~MonthFlags.June) > 0) throw new ArgumentException("Has more than one month in flags");
            return 6;
        }
        if (toConvert.HasJuly())
        {
            if ((toConvert & ~MonthFlags.July) > 0) throw new ArgumentException("Has more than one month in flags");
            return 7;
        }
        if (toConvert.HasAugust())
        {
            if ((toConvert & ~MonthFlags.August) > 0) throw new ArgumentException("Has more than one month in flags");
            return 8;
        }
        if (toConvert.HasSeptember())
        {
            if ((toConvert & ~MonthFlags.September) > 0) throw new ArgumentException("Has more than one month in flags");
            return 9;
        }
        if (toConvert.HasOctober())
        {
            if ((toConvert & ~MonthFlags.October) > 0) throw new ArgumentException("Has more than one month in flags");
            return 10;
        }
        if (toConvert.HasNovember())
        {
            if ((toConvert & ~MonthFlags.November) > 0) throw new ArgumentException("Has more than one month in flags");
            return 11;
        }
        if (toConvert.HasDecember())
        {
            if ((toConvert & ~MonthFlags.December) > 0) throw new ArgumentException("Has more than one month in flags");
            return 12;
        }
        throw new ArgumentException("Has No months to convert");
    }

    public static MonthFlags ToMonthsFlags(this int calendarMonth)
    {
        return calendarMonth switch
               {
                   1  => MonthFlags.January
                 , 2  => MonthFlags.February
                 , 3  => MonthFlags.March
                 , 4  => MonthFlags.April
                 , 5  => MonthFlags.May
                 , 6  => MonthFlags.June
                 , 7  => MonthFlags.July
                 , 8  => MonthFlags.August
                 , 9  => MonthFlags.September
                 , 10 => MonthFlags.October
                 , 11 => MonthFlags.November
                 , 12 => MonthFlags.December
                 , _  => throw new ArgumentOutOfRangeException(nameof(calendarMonth), calendarMonth, null)
               };
    }

    public static IEnumerable<int> ExpandToCalendarMonths(this MonthFlags monthsFlags, IList<int>? addToThs = null)
    {
        addToThs?.Clear();
        addToThs ??= new List<int>();
        if (monthsFlags.HasJanuary()) addToThs.Add(1);
        if (monthsFlags.HasFebruary()) addToThs.Add(2);
        if (monthsFlags.HasMarch()) addToThs.Add(3);
        if (monthsFlags.HasApril()) addToThs.Add(4);
        if (monthsFlags.HasMay()) addToThs.Add(5);
        if (monthsFlags.HasJune()) addToThs.Add(6);
        if (monthsFlags.HasJuly()) addToThs.Add(7);
        if (monthsFlags.HasAugust()) addToThs.Add(8);
        if (monthsFlags.HasSeptember()) addToThs.Add(9);
        if (monthsFlags.HasOctober()) addToThs.Add(10);
        if (monthsFlags.HasNovember()) addToThs.Add(11);
        if (monthsFlags.HasDecember()) addToThs.Add(12);

        return addToThs;
    }

    public static bool HasAllOf(this MonthFlags flags, MonthFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this MonthFlags flags, MonthFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this MonthFlags flags, MonthFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this MonthFlags flags, MonthFlags checkAllFound)   => flags == checkAllFound;
}