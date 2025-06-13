// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Config.Availability;

[Flags]
public enum DayOfWeekFlags : byte
{
    None
  , Sunday    = 0x01
  , Monday    = 0x02
  , Tuesday   = 0x04
  , Wednesday = 0x08
  , Thursday  = 0x10
  , Friday    = 0x20
  , Saturday  = 0x40
}

public static class DayOfWeekFlagsExtensions
{
    public static bool HasSunday(this DayOfWeekFlags flags)    => (flags & DayOfWeekFlags.Sunday) > 0;
    public static bool HasMonday(this DayOfWeekFlags flags)    => (flags & DayOfWeekFlags.Monday) > 0;
    public static bool HasTuesday(this DayOfWeekFlags flags)   => (flags & DayOfWeekFlags.Tuesday) > 0;
    public static bool HasWednesday(this DayOfWeekFlags flags) => (flags & DayOfWeekFlags.Wednesday) > 0;
    public static bool HasThursday(this DayOfWeekFlags flags)  => (flags & DayOfWeekFlags.Thursday) > 0;
    public static bool HasFriday(this DayOfWeekFlags flags)    => (flags & DayOfWeekFlags.Friday) > 0;
    public static bool HasSaturday(this DayOfWeekFlags flags)  => (flags & DayOfWeekFlags.Saturday) > 0;

    public static DayOfWeekFlags ToDayOfWeekFlags(this DayOfWeek systemDayOfWeek) =>
        systemDayOfWeek switch
        {
            DayOfWeek.Sunday    => DayOfWeekFlags.Sunday
          , DayOfWeek.Monday    => DayOfWeekFlags.Monday
          , DayOfWeek.Tuesday   => DayOfWeekFlags.Tuesday
          , DayOfWeek.Wednesday => DayOfWeekFlags.Wednesday
          , DayOfWeek.Thursday  => DayOfWeekFlags.Thursday
          , DayOfWeek.Friday    => DayOfWeekFlags.Friday
          , DayOfWeek.Saturday  => DayOfWeekFlags.Saturday

          , _ => throw new ArgumentOutOfRangeException(nameof(systemDayOfWeek), systemDayOfWeek, null)
        };

    public static DayOfWeek ToSystemDayOfWeek(this DayOfWeekFlags dayOfWeekFlags)
    {
        if (dayOfWeekFlags.HasSunday())
        {
            if ((dayOfWeekFlags & ~DayOfWeekFlags.Sunday) > 0) throw new ArgumentException("Has more than one day in flags");
            return DayOfWeek.Sunday;
        }
        if (dayOfWeekFlags.HasMonday())
        {
            if ((dayOfWeekFlags & ~DayOfWeekFlags.Monday) > 0) throw new ArgumentException("Has more than one day in flags");
            return DayOfWeek.Monday;
        }
        if (dayOfWeekFlags.HasTuesday()) 
        {
            if ((dayOfWeekFlags & ~DayOfWeekFlags.Tuesday) > 0) throw new ArgumentException("Has more than one day in flags");
            return DayOfWeek.Tuesday;
        }
        if (dayOfWeekFlags.HasWednesday())
        {
            if ((dayOfWeekFlags & ~DayOfWeekFlags.Wednesday) > 0) throw new ArgumentException("Has more than one day in flags");
            return DayOfWeek.Wednesday;
        }
        if (dayOfWeekFlags.HasThursday())
        {
            if ((dayOfWeekFlags & ~DayOfWeekFlags.Thursday) > 0) throw new ArgumentException("Has more than one day in flags");
            return DayOfWeek.Thursday;
        }
        if (dayOfWeekFlags.HasFriday())
        {
            if ((dayOfWeekFlags & ~DayOfWeekFlags.Friday) > 0) throw new ArgumentException("Has more than one day in flags");
            return DayOfWeek.Friday;
        }
        if (dayOfWeekFlags.HasSaturday()) 
        {
            if ((dayOfWeekFlags & ~DayOfWeekFlags.Saturday) > 0) throw new ArgumentException("Has more than one day in flags");
            return DayOfWeek.Saturday;
        }
        throw new ArgumentException("Has No days to convert");
    }

    public static DayOfWeekFlags CollapseToDayOfWeekFlags(this IEnumerable<DayOfWeek> systemDayOfWeekCollection)
    {
        DayOfWeekFlags collapsed = DayOfWeekFlags.None;
        foreach (DayOfWeek dayOfWeek in systemDayOfWeekCollection)
        {
            collapsed |= dayOfWeek.ToDayOfWeekFlags();
        }
        return collapsed;
    }

    public static IEnumerable<DayOfWeek> ExpandToSystemDayOfWeek(this DayOfWeekFlags dayOfWeekflags, IList<DayOfWeek>? addToThs = null)
    {
        addToThs?.Clear();
        addToThs ??= new List<DayOfWeek>();
        if (dayOfWeekflags.HasSunday()) addToThs.Add(DayOfWeek.Sunday);
        if (dayOfWeekflags.HasMonday()) addToThs.Add(DayOfWeek.Monday);
        if (dayOfWeekflags.HasTuesday()) addToThs.Add(DayOfWeek.Tuesday);
        if (dayOfWeekflags.HasWednesday()) addToThs.Add(DayOfWeek.Wednesday);
        if (dayOfWeekflags.HasThursday()) addToThs.Add(DayOfWeek.Thursday);
        if (dayOfWeekflags.HasFriday()) addToThs.Add(DayOfWeek.Friday);
        if (dayOfWeekflags.HasSaturday()) addToThs.Add(DayOfWeek.Saturday);

        return addToThs;
    }

    public static bool HasAllOf(this DayOfWeekFlags flags, DayOfWeekFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this DayOfWeekFlags flags, DayOfWeekFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this DayOfWeekFlags flags, DayOfWeekFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this DayOfWeekFlags flags, DayOfWeekFlags checkAllFound)   => flags == checkAllFound;
}