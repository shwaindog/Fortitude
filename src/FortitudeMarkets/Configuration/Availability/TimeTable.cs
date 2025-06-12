// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.DataStructures.Lists;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.Availability;

public interface ITimeTableConfig : IAvailability
{
    IWeeklyTimeTableConfig                 WeeklyTimeTableConfig  { get; set; }
    IEnumerable<ICalendarDateMatchConfig>? MarketHolidays          { get; set; }

    bool IsInHoliday(DateTimeOffset atThisDateTime);

    ITimeTableConfig Clone();
}

public struct TimeTable
{
    public TimeTable
        (WeeklyTimeTable weeklyTimeTableConfig, List<CalendarDateMatch>? marketHolidays = null)
    {
        MarketHolidays          = marketHolidays;
        WeeklyTimeTableConfig  = weeklyTimeTableConfig;
    }

    public WeeklyTimeTable          WeeklyTimeTableConfig  { get; }
    public List<CalendarDateMatch>? MarketHolidays          { get; }
}

public class TimeTableConfig : ConfigSection, ITimeTableConfig
{
    // ReSharper disable once NotAccessedField.Local
    private static object? ignoreSuppressWarnings;

    public TimeTableConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public TimeTableConfig(IWeeklyTimeTableConfig? regularTimeTable = null, List<ICalendarDateMatchConfig>? fixedHolidays = null)
    {
        WeeklyTimeTableConfig =
            regularTimeTable
         ?? new WeeklyTimeTableConfig
                (TimeZoneInfoExtensions.NewZealand, new WeeklyTimesConfig
                     (
                      DayOfWeekFlags.Monday | DayOfWeekFlags.Tuesday | DayOfWeekFlags.Wednesday | DayOfWeekFlags.Thursday | DayOfWeekFlags.Friday
                 , TimeSpan.FromHours(9))
               , new WeeklyTimesConfig(TimeZoneInfoExtensions.NewYork 
                    , DayOfWeekFlags.Monday | DayOfWeekFlags.Tuesday | DayOfWeekFlags.Wednesday | DayOfWeekFlags.Thursday | DayOfWeekFlags.Friday
                 , TimeSpan.FromHours(17))
                );
        MarketHolidays          = fixedHolidays ?? new List<ICalendarDateMatchConfig>();
    }

    public TimeTableConfig(IWeeklyTimeTableConfig regularTimeTable, IConfigurationRoot root, string path
      , List<ICalendarDateMatchConfig>? fixedHolidays = null) 
        : base(root, path)
    {
        WeeklyTimeTableConfig  = regularTimeTable;
        MarketHolidays          = fixedHolidays ?? new List<ICalendarDateMatchConfig>();
    }

    public TimeTableConfig(ITimeTableConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        WeeklyTimeTableConfig  = toClone.WeeklyTimeTableConfig;
        MarketHolidays          = toClone.MarketHolidays;
    }

    public TimeTableConfig(ITimeTableConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IWeeklyTimeTableConfig WeeklyTimeTableConfig
    {
        get => new WeeklyTimeTableConfig(ConfigRoot, Path + ":" + nameof(WeeklyTimeTableConfig));
        set => ignoreSuppressWarnings = new WeeklyTimeTableConfig(value, ConfigRoot, Path + ":" + nameof(WeeklyTimeTableConfig));
    }

    public IEnumerable<ICalendarDateMatchConfig>? MarketHolidays
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<ICalendarDateMatchConfig>>();
            foreach (var calendarDay in GetSection(nameof(MarketHolidays)).GetChildren())
                if (calendarDay["Month"] != null)
                    autoRecycleList.Add(new CalendarDateMatchConfig(ConfigRoot, calendarDay.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = MarketHolidays?.Count() ?? 0;

            var i = 0;

            if (value != null)
                foreach (var calendarDay in value)
                    ignoreSuppressWarnings = new CalendarDateMatchConfig(calendarDay, ConfigRoot, Path + ":" + nameof(MarketHolidays) + ":" + i);

            for (var j = i; j < oldCount; j++) CalendarDateMatchConfig.ClearValues(ConfigRoot, Path + ":" + nameof(MarketHolidays) + $":{j}");
        }
    }

    // public bool ShouldBeUp(DateTimeOffset atThisDateTime) => WeeklyTimeTableConfig.GetExpectedAvailability(atThisDateTime) && !IsInHoliday(atThisDateTime);

    public TradingPeriodTypeFlags GetExpectedAvailability(DateTimeOffset atThisDateTime) => throw new NotImplementedException();

    public bool IsInHoliday(DateTimeOffset atThisDateTime) =>
        (MarketHolidays?.Any(fcd => fcd.DateMatches(atThisDateTime)) ?? false);

    public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
    {
        if (WeeklyTimeTableConfig.GetExpectedAvailability(fromNow).IsMarketClose()) return TimeSpan.Zero;
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
        Equals(WeeklyTimeTableConfig, other.WeeklyTimeTableConfig) && Equals(MarketHolidays, other.MarketHolidays);

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
            var hashCode = WeeklyTimeTableConfig.GetHashCode();
            hashCode = (hashCode * 397) ^ (MarketHolidays != null ? MarketHolidays.GetHashCode() : 0);
            return hashCode;
        }
    }
}
