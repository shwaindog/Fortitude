// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Extensions;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.Availability;

public interface IWeeklyTimesConfig
{
    TimeZoneInfo?  OverrideTimeZone { get; set; }
    DayOfWeekFlags DaysOfWeek       { get; set; }
    TimeSpan       TimeOfDay        { get; set; }

    IEnumerable<IWeeklyTimesConfig>?  OverrideWeeklyTimes { get; set; }
    IEnumerable<TimeZonedWeekDayTime> TimeZonedWeekDayTimes(TimeZoneInfo defaultTimeZone, IList<TimeZonedWeekDayTime>? addToThis = null);
}

public struct WeeklyTimes
{
    public WeeklyTimes(DayOfWeekFlags daysOfWeek, TimeSpan timeOfDay, TimeZoneInfo? overrideTimeZone = null
      , List<WeeklyTimes>? overrideWeeklyTimes = null)
    {
        DaysOfWeek          = daysOfWeek;
        TimeOfDay           = timeOfDay;
        OverrideTimeZone    = overrideTimeZone;
        OverrideWeeklyTimes = overrideWeeklyTimes;
    }

    public TimeZoneInfo?      OverrideTimeZone    { get; }
    public DayOfWeekFlags     DaysOfWeek          { get; }
    public TimeSpan           TimeOfDay           { get; }
    public List<WeeklyTimes>? OverrideWeeklyTimes { get; }
}

public class WeeklyTimesConfig : ConfigSection, IWeeklyTimesConfig
{
    private static object? ignoreSuppressWarnings;

    public WeeklyTimesConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public WeeklyTimesConfig() { }

    public WeeklyTimesConfig
    (DayOfWeekFlags daysOfWeek, TimeSpan timeOfDay, IConfigurationRoot root, string path,
        IEnumerable<IWeeklyTimesConfig>? overrideWeeklyTimes = null) : base(root, path)
    {
        DaysOfWeek = daysOfWeek;
        TimeOfDay  = timeOfDay;

        if (overrideWeeklyTimes != null) OverrideWeeklyTimes = overrideWeeklyTimes;
    }

    public WeeklyTimesConfig(DayOfWeekFlags daysOfWeek, TimeSpan timeOfDay, IEnumerable<IWeeklyTimesConfig>? overrideWeeklyTimes = null)
        : this(daysOfWeek, timeOfDay, InMemoryConfigRoot, InMemoryPath, overrideWeeklyTimes) { }

    public WeeklyTimesConfig
    (TimeZoneInfo overrideTimeZone, DayOfWeekFlags daysOfWeek, TimeSpan timeOfDay
      , IEnumerable<IWeeklyTimesConfig>? overrideWeeklyTimes = null)
        : this(daysOfWeek, timeOfDay, InMemoryConfigRoot, InMemoryPath, overrideWeeklyTimes) =>
        OverrideTimeZone = overrideTimeZone;

    public WeeklyTimesConfig(IWeeklyTimesConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        DaysOfWeek = toClone.DaysOfWeek;
        TimeOfDay  = toClone.TimeOfDay;

        OverrideWeeklyTimes = toClone.OverrideWeeklyTimes;
    }

    public WeeklyTimesConfig(IWeeklyTimesConfig toClone) : base(InMemoryConfigRoot, InMemoryPath)
    {
        DaysOfWeek = toClone.DaysOfWeek;
        TimeOfDay  = toClone.TimeOfDay;

        OverrideWeeklyTimes = toClone.OverrideWeeklyTimes;
    }

    public TimeZoneInfo? OverrideTimeZone
    {
        get
        {
            var checkValue = this[nameof(OverrideTimeZone)];
            return checkValue != null ? TimeZoneInfo.FindSystemTimeZoneById(checkValue) : null;
        }
        set => this[nameof(OverrideTimeZone)] = value?.Id;
    }

    public DayOfWeekFlags DaysOfWeek
    {
        get
        {
            var checkValue = this[nameof(DaysOfWeek)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<DayOfWeekFlags>(checkValue!) : DayOfWeekFlags.None;
        }
        set => this[nameof(DaysOfWeek)] = value.ToString();
    }

    public TimeSpan TimeOfDay
    {
        get
        {
            var checkValue = this[nameof(TimeOfDay)];
            return checkValue != null ? TimeSpan.Parse(checkValue) : TimeSpan.Zero;
        }
        set => this[nameof(TimeOfDay)] = value.ToString();
    }

    public IEnumerable<IWeeklyTimesConfig>? OverrideWeeklyTimes
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<IWeeklyTimesConfig>>();
            foreach (var configurationSection in GetSection(nameof(OverrideWeeklyTimes)).GetChildren())
                if (configurationSection["DayOfWeek"] != null)
                    autoRecycleList.Add(new WeeklyTimesConfig(ConfigRoot, configurationSection.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = OverrideWeeklyTimes?.Count() ?? 0;
            var i        = 0;
            if (value != null)
                foreach (var remoteServiceConfig in value)
                {
                    ignoreSuppressWarnings = new WeeklyTimesConfig(remoteServiceConfig, ConfigRoot
                                                                 , Path + ":" + nameof(OverrideWeeklyTimes) + $":{i}");
                    i++;
                }

            for (var j = i; j < oldCount; j++) WeekDayTimeConfig.ClearValues(ConfigRoot, Path + ":" + nameof(OverrideWeeklyTimes) + $":{i}");
        }
    }

    public TimeZonedWeekDayTime TimeZonedDayOfWeekWeekTime(TimeZoneInfo defaultTimeZone, DayOfWeekFlags dayOfWeekRequested)
    {
        var foundOverride = OverrideWeeklyTimes?.FirstOrDefault(owt => owt.DaysOfWeek.HasAnyOf(dayOfWeekRequested));
        if (foundOverride != null)
        {
            return new TimeZonedWeekDayTime(foundOverride.OverrideTimeZone ?? defaultTimeZone, dayOfWeekRequested, foundOverride.TimeOfDay);
        }

        return new TimeZonedWeekDayTime(OverrideTimeZone ?? defaultTimeZone, dayOfWeekRequested, TimeOfDay);
    }

    public IEnumerable<TimeZonedWeekDayTime> TimeZonedWeekDayTimes(TimeZoneInfo defaultTimeZone, IList<TimeZonedWeekDayTime>? addToThis = null)
    {
        addToThis?.Clear();
        addToThis ??= new List<TimeZonedWeekDayTime>();
        if (DaysOfWeek.HasSunday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(defaultTimeZone, DayOfWeekFlags.Sunday));
        if (DaysOfWeek.HasMonday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(defaultTimeZone, DayOfWeekFlags.Monday));
        if (DaysOfWeek.HasTuesday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(defaultTimeZone, DayOfWeekFlags.Tuesday));
        if (DaysOfWeek.HasWednesday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(defaultTimeZone, DayOfWeekFlags.Wednesday));
        if (DaysOfWeek.HasThursday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(defaultTimeZone, DayOfWeekFlags.Thursday));
        if (DaysOfWeek.HasFriday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(defaultTimeZone, DayOfWeekFlags.Friday));
        if (DaysOfWeek.HasSaturday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(defaultTimeZone, DayOfWeekFlags.Saturday));

        return addToThis;
    }
}
