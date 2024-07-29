// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeCommon.Configuration.Availability;

public interface IWeekDayTimeConfig
{
    DayOfWeek DayOfWeek { get; set; }
    TimeSpan  TimeOfDay { get; set; }
}

public struct WeekDayTime
{
    public WeekDayTime(DayOfWeek dayOfWeek, TimeSpan timeOfDay)
    {
        DayOfWeek = dayOfWeek;
        TimeOfDay = timeOfDay;
    }

    private DayOfWeek DayOfWeek { get; }
    private TimeSpan  TimeOfDay { get; }
}

public class WeekDayTimeConfig : ConfigSection, IWeekDayTimeConfig
{
    public WeekDayTimeConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public WeekDayTimeConfig() { }

    public WeekDayTimeConfig(DayOfWeek dayOfWeek, TimeSpan timeOfDay, IConfigurationRoot root, string path) : base(root, path)
    {
        DayOfWeek = dayOfWeek;
        TimeOfDay = timeOfDay;
    }

    public WeekDayTimeConfig(DayOfWeek dayOfWeek, TimeSpan timeOfDay) : this(dayOfWeek, timeOfDay, InMemoryConfigRoot, InMemoryPath) { }

    public WeekDayTimeConfig
        (DayOfWeek dayOfWeek, byte hour, byte minute = 0, byte second = 0) :
        this(dayOfWeek, hour, minute, second, InMemoryConfigRoot, InMemoryPath) { }

    public WeekDayTimeConfig(DayOfWeek dayOfWeek, byte hour, byte minute, IConfigurationRoot root, string path) : base(root, path)
    {
        DayOfWeek = dayOfWeek;
        TimeOfDay = TimeSpan.FromHours(hour) + TimeSpan.FromMinutes(minute);
    }

    public WeekDayTimeConfig(DayOfWeek dayOfWeek, byte hour, byte minute, byte second, IConfigurationRoot root, string path) : base(root, path)
    {
        DayOfWeek = dayOfWeek;
        TimeOfDay = TimeSpan.FromHours(hour) + TimeSpan.FromMinutes(minute) + TimeSpan.FromSeconds(second);
    }

    public WeekDayTimeConfig(IWeekDayTimeConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        DayOfWeek = toClone.DayOfWeek;
        TimeOfDay = toClone.TimeOfDay;
    }

    public WeekDayTimeConfig(IWeekDayTimeConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public DayOfWeek DayOfWeek
    {
        get => Enum.TryParse<DayOfWeek>(this[nameof(DayOfWeek)]!, out var month) ? month : DayOfWeek.Sunday;
        set => this[nameof(Month)] = value.ToString();
    }

    public TimeSpan TimeOfDay
    {
        get => TimeSpan.TryParse(this[nameof(TimeOfDay)]!, out var timeOfDay) ? timeOfDay : TimeSpan.Zero;
        set => this[nameof(TimeOfDay)] = value.ToString();
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(DayOfWeek)] = null;
        root[path + ":" + nameof(TimeOfDay)] = null;
    }
}
