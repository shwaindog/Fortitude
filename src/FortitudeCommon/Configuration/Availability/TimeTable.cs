// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeCommon.Configuration.Availability;

public interface ITimeTableConfig : IAvailability
{
    IWeeklyTimeTableConfig            WeeklyTimeTableConfig  { get; set; }
    IEnumerable<ICalendarDayConfig>?  FixedHolidays          { get; set; }
    IEnumerable<ICalendarDateConfig>? YearlyVariableHolidays { get; set; }

    bool IsInHoliday(DateTimeOffset atThisDateTime);

    ITimeTableConfig Clone();
}

public struct TimeTable
{
    public TimeTable
        (WeeklyTimeTable weeklyTimeTableConfig, List<CalendarDay>? fixedHolidays = null, List<CalendarDate>? yearlyVariableHolidays = null)
    {
        FixedHolidays          = fixedHolidays;
        WeeklyTimeTableConfig  = weeklyTimeTableConfig;
        YearlyVariableHolidays = yearlyVariableHolidays;
    }

    public WeeklyTimeTable     WeeklyTimeTableConfig  { get; }
    public List<CalendarDay>?  FixedHolidays          { get; }
    public List<CalendarDate>? YearlyVariableHolidays { get; }
}

public class TimeTableConfig : ConfigSection, ITimeTableConfig
{
    // ReSharper disable once NotAccessedField.Local
    private static object? ignoreSuppressWarnings;

    public TimeTableConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public TimeTableConfig
    (IWeeklyTimeTableConfig? regularTimeTable = null, List<ICalendarDayConfig>? fixedHolidays = null
      , List<ICalendarDateConfig>? yearlyVariableHolidays = null)
    {
        WeeklyTimeTableConfig =
            regularTimeTable
         ?? new WeeklyTimeTableConfig
                (TimeZoneInfoExtensions.NewZealand, new WeeklyTimesConfig(new List<DayOfWeek>
                 {
                     DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
                 }, TimeSpan.FromHours(9)), new WeeklyTimesConfig(TimeZoneInfoExtensions.NewYork, new List<DayOfWeek>
                 {
                     DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
                 }, TimeSpan.FromHours(17))
                );
        FixedHolidays          = fixedHolidays ?? new List<ICalendarDayConfig>();
        YearlyVariableHolidays = yearlyVariableHolidays ?? new List<ICalendarDateConfig>();
    }

    public TimeTableConfig
    (IWeeklyTimeTableConfig regularTimeTable, IConfigurationRoot root, string path, List<ICalendarDayConfig>? fixedHolidays = null
      , List<ICalendarDateConfig>? yearlyVariableHolidays = null) : base(root, path)
    {
        WeeklyTimeTableConfig  = regularTimeTable;
        FixedHolidays          = fixedHolidays ?? new List<ICalendarDayConfig>();
        YearlyVariableHolidays = yearlyVariableHolidays ?? new List<ICalendarDateConfig>();
    }

    public TimeTableConfig(ITimeTableConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        WeeklyTimeTableConfig  = toClone.WeeklyTimeTableConfig;
        FixedHolidays          = toClone.FixedHolidays;
        YearlyVariableHolidays = toClone.YearlyVariableHolidays;
    }

    public TimeTableConfig(ITimeTableConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IWeeklyTimeTableConfig WeeklyTimeTableConfig
    {
        get => new WeeklyTimeTableConfig(ConfigRoot, Path + ":" + nameof(WeeklyTimeTableConfig));
        set => ignoreSuppressWarnings = new WeeklyTimeTableConfig(value, ConfigRoot, Path + ":" + nameof(WeeklyTimeTableConfig));
    }

    public IEnumerable<ICalendarDayConfig>? FixedHolidays
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<ICalendarDayConfig>>();
            foreach (var calendarDay in GetSection(nameof(FixedHolidays)).GetChildren())
                if (calendarDay["Month"] != null)
                    autoRecycleList.Add(new CalendarDayConfig(ConfigRoot, calendarDay.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = FixedHolidays?.Count() ?? 0;

            var i = 0;

            if (value != null)
                foreach (var calendarDay in value)
                    ignoreSuppressWarnings = new CalendarDayConfig(calendarDay, ConfigRoot, Path + ":" + nameof(FixedHolidays) + ":" + i);

            for (var j = i; j < oldCount; j++) CalendarDayConfig.ClearValues(ConfigRoot, Path + ":" + nameof(FixedHolidays) + $":{j}");
        }
    }

    public IEnumerable<ICalendarDateConfig>? YearlyVariableHolidays
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<ICalendarDateConfig>>();
            foreach (var calendarDay in GetSection(nameof(YearlyVariableHolidays)).GetChildren())
                if (calendarDay["Month"] != null)
                    autoRecycleList.Add(new CalendarDateConfig(ConfigRoot, calendarDay.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = YearlyVariableHolidays?.Count() ?? 0;

            var i = 0;

            if (value != null)
                foreach (var calendarDay in value)
                    ignoreSuppressWarnings = new CalendarDateConfig(calendarDay, ConfigRoot, Path + ":" + nameof(YearlyVariableHolidays) + ":" + i);

            for (var j = i; j < oldCount; j++) CalendarDateConfig.ClearValues(ConfigRoot, Path + ":" + nameof(YearlyVariableHolidays) + $":{j}");
        }
    }

    public bool ShouldBeUp(DateTimeOffset atThisDateTime) => WeeklyTimeTableConfig.ShouldBeUp(atThisDateTime) && !IsInHoliday(atThisDateTime);

    public bool IsInHoliday(DateTimeOffset atThisDateTime) =>
        (FixedHolidays?.Any(fcd => fcd.DateMatches(atThisDateTime)) ?? false)
     || (YearlyVariableHolidays?.Any(dtp => dtp.DateMatches(atThisDateTime)) ?? false);

    public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
    {
        if (!ShouldBeUp(fromNow)) return TimeSpan.Zero;
        return WeeklyTimeTableConfig.ExpectedRemainingUpTime(fromNow);
    }

    public DateTimeOffset NextScheduledOpeningTime(DateTimeOffset fromNow)
    {
        var nextScheduledOpeningTime = WeeklyTimeTableConfig.NextScheduledOpeningTime(fromNow);

        while (!IsInHoliday(nextScheduledOpeningTime))
        {
            var atClose = nextScheduledOpeningTime + ExpectedRemainingUpTime(nextScheduledOpeningTime);
            nextScheduledOpeningTime = WeeklyTimeTableConfig.NextScheduledOpeningTime(atClose);
        }
        return nextScheduledOpeningTime;
    }

    public ITimeTableConfig Clone() => new TimeTableConfig(this);

    protected bool Equals(TimeTableConfig other) =>
        Equals(WeeklyTimeTableConfig, other.WeeklyTimeTableConfig) && Equals(FixedHolidays, other.FixedHolidays) &&
        Equals(YearlyVariableHolidays, other.YearlyVariableHolidays);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TimeTableConfig)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = WeeklyTimeTableConfig != null ? WeeklyTimeTableConfig.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (FixedHolidays != null ? FixedHolidays.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (YearlyVariableHolidays != null ? YearlyVariableHolidays.GetHashCode() : 0);
            return hashCode;
        }
    }
}
