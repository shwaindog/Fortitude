// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.Availability;

public interface ITimeTableConfig : IAvailability, IInterfacesComparable<ITimeTableConfig>
{
    TimeZoneInfo OperatingTimeZone { get; set; }

    IWeeklyTimeTableConfig WeeklyTimeTableConfig { get; set; }

    IReadOnlyList<ICalendarDateMatchConfig> CalendarHolidays { get; set; }

    IReadOnlyList<NamedHoliday>? FollowsIrregularHolidays { get; set; }

    IReadOnlyList<ICalendarDateMatchConfig>? UpcomingIrregularHolidays { get; set; }

    bool IsInHoliday(DateTimeOffset atThisDateTime);

    ITimeTableConfig Clone();
}

public readonly struct TimeTable
(
    TimeZoneInfo operatingTimeZone
  , WeeklyTimeTable weeklyTimeTableConfig
  , CalendarDateMatch[]? consistentCalendarHolidays = null
  , NamedHoliday[]? followsIrregularHolidays = null
  , CalendarDateMatch[]? upcomingIrregularHolidays = null)
{
    private static readonly CalendarDateMatch[] EmptyReadonlyHoliday = [];

    public TimeZoneInfo OperatingTimeZone { get; } = operatingTimeZone;

    public WeeklyTimeTable WeeklyTimeTableConfig { get; } = weeklyTimeTableConfig;

    public CalendarDateMatch[] CalendarHolidays { get; } = consistentCalendarHolidays ?? EmptyReadonlyHoliday;

    public NamedHoliday[]? FollowsIrregularHolidays { get; } = followsIrregularHolidays;

    public CalendarDateMatch[] UpcomingIrregularHolidays { get; } = upcomingIrregularHolidays ?? EmptyReadonlyHoliday;
}

public class TimeTableConfig : ConfigSection, ITimeTableConfig
{
    public TimeTableConfig() { }

    public TimeTableConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public TimeTableConfig
    (TimeZoneInfo operatingTimeZone, IWeeklyTimeTableConfig regularTimeTable, IReadOnlyList<ICalendarDateMatchConfig> consistentCalendarHolidays
      , IReadOnlyList<NamedHoliday>? followsIrregularHolidays = null, IReadOnlyList<ICalendarDateMatchConfig>? upcomingIrregularHolidays = null)
        : this(operatingTimeZone, regularTimeTable, consistentCalendarHolidays, InMemoryConfigRoot, InMemoryPath, followsIrregularHolidays
             , upcomingIrregularHolidays) { }

    public TimeTableConfig
    (TimeZoneInfo operatingTimeZone, IWeeklyTimeTableConfig regularTimeTable, IReadOnlyList<ICalendarDateMatchConfig> consistentCalendarHolidays
      , IConfigurationRoot root, string path
      , IReadOnlyList<NamedHoliday>? followsIrregularHolidays = null
      , IReadOnlyList<ICalendarDateMatchConfig>? upcomingIrregularHolidays = null) : base(root, path)
    {
        OperatingTimeZone         = operatingTimeZone;
        WeeklyTimeTableConfig     = regularTimeTable;
        CalendarHolidays          = consistentCalendarHolidays;
        FollowsIrregularHolidays  = followsIrregularHolidays;
        UpcomingIrregularHolidays = upcomingIrregularHolidays;
    }

    public TimeTableConfig(ITimeTableConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        OperatingTimeZone         = toClone.OperatingTimeZone;
        WeeklyTimeTableConfig     = toClone.WeeklyTimeTableConfig;
        CalendarHolidays          = toClone.CalendarHolidays;
        FollowsIrregularHolidays  = toClone.FollowsIrregularHolidays;
        UpcomingIrregularHolidays = toClone.UpcomingIrregularHolidays;
    }

    public TimeTableConfig(ITimeTableConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IWeeklyTimeTableConfig WeeklyTimeTableConfig
    {
        get =>
            new WeeklyTimeTableConfig(ConfigRoot, Path + ":" + nameof(WeeklyTimeTableConfig))
            {
                ParentTimeZone = OperatingTimeZone
            };
        set
        {
            _ = new WeeklyTimeTableConfig(value, ConfigRoot, Path + ":" + nameof(WeeklyTimeTableConfig));
            if (value is WeeklyTimeTableConfig valueWeeklyTimeTableConfig)
            {
                valueWeeklyTimeTableConfig.ParentTimeZone = OperatingTimeZone;
            }
        }
    }

    public TimeZoneInfo OperatingTimeZone
    {
        get
        {
            var checkValue = this[nameof(OperatingTimeZone)];
            return checkValue != null ? TimeZoneInfo.FindSystemTimeZoneById(checkValue) : TimeZoneInfo.Utc;
        }
        set => this[nameof(OperatingTimeZone)] = value.Id;
    }

    public IReadOnlyList<ICalendarDateMatchConfig> CalendarHolidays
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<ReusableList<ICalendarDateMatchConfig>>();
            foreach (var calendarDay in GetSection(nameof(CalendarHolidays)).GetChildren())
                if (calendarDay["Month"] != null)
                    autoRecycleList.Add(new CalendarDateMatchConfig(ConfigRoot, calendarDay.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = CalendarHolidays.Count;

            var i = 0;

            foreach (var calendarDay in value)
                _ = new CalendarDateMatchConfig(calendarDay, ConfigRoot, Path + ":" + nameof(CalendarHolidays) + ":" + i++);

            for (var j = i; j < oldCount; j++) CalendarDateMatchConfig.ClearValues(ConfigRoot, Path + ":" + nameof(CalendarHolidays) + $":{j++}");
        }
    }

    public IReadOnlyList<NamedHoliday>? FollowsIrregularHolidays
    {
        get
        {
            var checkValue =
                GetSection(nameof(FollowsIrregularHolidays))
                    .GetChildren()
                    .Where(cs => cs.Value.IsNotNullOrEmpty())
                    .Select(cs => Enum.Parse<NamedHoliday>(cs.Value!)).ToList();
            return checkValue.Count > 0 ? checkValue.AsReadOnly() : null;
        }
        set
        {
            var previousCount = FollowsIrregularHolidays?.Count ?? 0;

            int i = 0;
            foreach (var namedHoliday in value ?? [])
            {
                this[nameof(FollowsIrregularHolidays) + ConfigurationPath.KeyDelimiter + i++] = namedHoliday.ToString();   
            }

            for (int j = 0; j < previousCount; j++)
            {
                this[nameof(FollowsIrregularHolidays) + ConfigurationPath.KeyDelimiter + i++] = "";   
            }
        }
    }

    public IReadOnlyList<ICalendarDateMatchConfig>? UpcomingIrregularHolidays
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<ReusableList<ICalendarDateMatchConfig>>();
            foreach (var calendarDay in GetSection(nameof(UpcomingIrregularHolidays)).GetChildren())
                if (calendarDay["Month"] != null)
                    autoRecycleList.Add(new CalendarDateMatchConfig(ConfigRoot, calendarDay.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = UpcomingIrregularHolidays?.Count ?? 0;

            var i = 0;

            if (value != null)
                foreach (var calendarDay in value)
                {
                    if (calendarDay.Year == null)
                    {
                        throw new ArgumentException($"{nameof(UpcomingIrregularHolidays)} must have Year set on CalendarDateMatchConfig");
                    }
                    _ = new CalendarDateMatchConfig(calendarDay, ConfigRoot, Path + ":" + nameof(UpcomingIrregularHolidays) + ":" + i);
                }

            for (var j = i; j < oldCount; j++)
                CalendarDateMatchConfig.ClearValues(ConfigRoot, Path + ":" + nameof(UpcomingIrregularHolidays) + $":{j}");
        }
    }

    // public bool ShouldBeUp(DateTimeOffset atThisDateTime) => WeeklyTimeTableConfig.GetExpectedAvailability(atThisDateTime) && !IsInHoliday(atThisDateTime);

    public TradingPeriodTypeFlags GetExpectedAvailability(DateTimeOffset atThisDateTime) => throw new NotImplementedException();

    public bool IsInHoliday(DateTimeOffset atThisDateTime) => CalendarHolidays.Any(fcd => fcd.DateMatches(atThisDateTime));

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

    public bool AreEquivalent(ITimeTableConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var operatingTzSame         = OperatingTimeZone.Equals(other.OperatingTimeZone);
        var weekTimeTableSame       = WeeklyTimeTableConfig.AreEquivalent(other.WeeklyTimeTableConfig, exactTypes);
        var thisConsistentHolidays  = CalendarHolidays;
        var otherConsistentHolidays = other.CalendarHolidays;
        var consistentCount         = thisConsistentHolidays.Count;
        var consistentCountSame     = consistentCount == otherConsistentHolidays.Count;
        var allConsistentAreSame    = consistentCountSame;
        for (var i = 0; i < consistentCount && allConsistentAreSame; i++)
        {
            allConsistentAreSame &= thisConsistentHolidays[i].AreEquivalent(otherConsistentHolidays[i], exactTypes);
        }
        var thisFollowsIrregularHolidays  = FollowsIrregularHolidays;
        var otherFollowsIrregularHolidays = other.FollowsIrregularHolidays;
        var followsIrregularCount         = thisFollowsIrregularHolidays?.Count ?? 0;
        var followsIrregularSame          = followsIrregularCount == (otherFollowsIrregularHolidays?.Count ?? 0);
        var followsIrregularAreSame       = followsIrregularSame;
        for (var i = 0; i < followsIrregularCount && followsIrregularAreSame; i++)
        {
            followsIrregularAreSame &= thisFollowsIrregularHolidays![i] == otherFollowsIrregularHolidays![i];
        }
        var thisUpcomingHolidays  = UpcomingIrregularHolidays;
        var otherUpcomingHolidays = other.UpcomingIrregularHolidays;
        var upcomingCount         = thisUpcomingHolidays?.Count ?? 0;
        var upcomingCountSame     = upcomingCount == (otherUpcomingHolidays?.Count ?? 0);
        var allUpcomingAreSame    = upcomingCountSame;
        for (var i = 0; i < upcomingCount && allUpcomingAreSame; i++)
        {
            allUpcomingAreSame &= thisUpcomingHolidays![i].AreEquivalent(otherUpcomingHolidays![i], exactTypes);
        }

        var allAreSame = operatingTzSame && weekTimeTableSame && allConsistentAreSame && followsIrregularAreSame && allUpcomingAreSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITimeTableConfig, true);


    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = WeeklyTimeTableConfig.GetHashCode();
            hashCode = (hashCode * 397) ^ CalendarHolidays.GetHashCode();
            hashCode = (hashCode * 397) ^ (FollowsIrregularHolidays != null ? FollowsIrregularHolidays.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (UpcomingIrregularHolidays != null ? UpcomingIrregularHolidays.GetHashCode() : 0);
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(TimeTableConfig)}{{{nameof(OperatingTimeZone)}: {OperatingTimeZone}, {nameof(WeeklyTimeTableConfig)}: {WeeklyTimeTableConfig}, " +
        $"{nameof(CalendarHolidays)}: {CalendarHolidays}, {nameof(FollowsIrregularHolidays)}: {FollowsIrregularHolidays}, " +
        $"{nameof(UpcomingIrregularHolidays)}: {UpcomingIrregularHolidays}}}";
}
