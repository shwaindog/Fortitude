// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeCommon.Configuration.Availability;

public interface IWeeklyTimesConfig
{
    TimeZoneInfo?                     OverrideTimeZone    { get; set; }
    IEnumerable<DayOfWeek>            DaysOfWeek          { get; set; }
    TimeSpan                          TimeOfDay           { get; set; }
    IEnumerable<IWeekDayTimeConfig>?  OverrideWeeklyTimes { get; set; }
    IEnumerable<TimeZonedWeekDayTime> TimeZonedWeekDayTimes(TimeZoneInfo defaultTimeZone);
}

public struct WeeklyTimes
{
    public WeeklyTimes
        (List<DayOfWeek> daysOfWeek, TimeSpan timeOfDay, TimeZoneInfo? overrideTimeZone = null, List<WeekDayTime>? overrideWeeklyTimes = null)
    {
        DaysOfWeek          = daysOfWeek;
        TimeOfDay           = timeOfDay;
        OverrideTimeZone    = overrideTimeZone;
        OverrideWeeklyTimes = overrideWeeklyTimes;
    }

    public TimeZoneInfo?      OverrideTimeZone    { get; }
    public List<DayOfWeek>    DaysOfWeek          { get; }
    public TimeSpan           TimeOfDay           { get; }
    public List<WeekDayTime>? OverrideWeeklyTimes { get; }
}

public class WeeklyTimesConfig : ConfigSection, IWeeklyTimesConfig
{
    private static object? ignoreSuppressWarnings;

    public WeeklyTimesConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public WeeklyTimesConfig() { }

    public WeeklyTimesConfig
    (IEnumerable<DayOfWeek> daysOfWeek, TimeSpan timeOfDay, IConfigurationRoot root, string path,
        IEnumerable<IWeekDayTimeConfig>? overrideWeeklyTimes = null) : base(root, path)
    {
        DaysOfWeek = daysOfWeek;
        TimeOfDay  = timeOfDay;

        if (overrideWeeklyTimes != null) OverrideWeeklyTimes = overrideWeeklyTimes;
    }

    public WeeklyTimesConfig(IEnumerable<DayOfWeek> daysOfWeek, TimeSpan timeOfDay, IEnumerable<IWeekDayTimeConfig>? overrideWeeklyTimes = null)
        : this(daysOfWeek, timeOfDay, InMemoryConfigRoot, InMemoryPath, overrideWeeklyTimes) { }

    public WeeklyTimesConfig
    (TimeZoneInfo overrideTimeZone, IEnumerable<DayOfWeek> daysOfWeek, TimeSpan timeOfDay
      , IEnumerable<IWeekDayTimeConfig>? overrideWeeklyTimes = null)
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

    public IEnumerable<DayOfWeek> DaysOfWeek
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<DayOfWeek>>();
            foreach (var dayOfWeek in GetSection(nameof(DaysOfWeek)).GetChildren())
                if (dayOfWeek.Value != null)
                    autoRecycleList.Add(Enum.Parse<DayOfWeek>(dayOfWeek.Value!));
            return autoRecycleList;
        }
        set
        {
            var oldCount                                                                       = DaysOfWeek.Count();
            var i                                                                              = 0;
            foreach (var dayOfWeek in value) this[Path + ":" + nameof(DaysOfWeek) + $":{i++}"] = dayOfWeek.ToString();

            for (var j = i; j < oldCount; j++) this[Path + ":" + nameof(DaysOfWeek) + $":{i++}"] = null;
        }
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

    public IEnumerable<IWeekDayTimeConfig>? OverrideWeeklyTimes
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<IWeekDayTimeConfig>>();
            foreach (var configurationSection in GetSection(nameof(OverrideWeeklyTimes)).GetChildren())
                if (configurationSection["DayOfWeek"] != null)
                    autoRecycleList.Add(new WeekDayTimeConfig(ConfigRoot, configurationSection.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = OverrideWeeklyTimes?.Count() ?? 0;
            var i        = 0;
            if (value != null)
                foreach (var remoteServiceConfig in value)
                {
                    ignoreSuppressWarnings = new WeekDayTimeConfig(remoteServiceConfig, ConfigRoot
                                                                 , Path + ":" + nameof(OverrideWeeklyTimes) + $":{i}");
                    i++;
                }

            for (var j = i; j < oldCount; j++) WeekDayTimeConfig.ClearValues(ConfigRoot, Path + ":" + nameof(OverrideWeeklyTimes) + $":{i}");
        }
    }

    public IEnumerable<TimeZonedWeekDayTime> TimeZonedWeekDayTimes(TimeZoneInfo defaultTimeZone)
    {
        foreach (var dayOfWeek in DaysOfWeek)
            if (OverrideWeeklyTimes?.Any(wdtc => wdtc.DayOfWeek == dayOfWeek) ?? false)
                foreach (var weekDayOverride in OverrideWeeklyTimes.Where(wdtc => wdtc.DayOfWeek == dayOfWeek))
                    yield return new TimeZonedWeekDayTime(OverrideTimeZone ?? defaultTimeZone, dayOfWeek, weekDayOverride.TimeOfDay);
            else
                yield return new TimeZonedWeekDayTime(OverrideTimeZone ?? defaultTimeZone, dayOfWeek, TimeOfDay);
    }
}
