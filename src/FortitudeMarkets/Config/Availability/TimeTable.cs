// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;
using static FortitudeMarkets.Config.Availability.TradingPeriodTypeFlags;

#endregion

namespace FortitudeMarkets.Config.Availability;

public interface ITimeTableConfig : ICalendarAvailability, IInterfacesComparable<ITimeTableConfig>, IStringBearer
{
    TimeZoneInfo OperatingTimeZone { get; set; }

    IWeeklyTimeTableConfig WeeklyTimeTableConfig { get; set; }

    IReadOnlyList<ICalendarDateHolidayConfig> CalendarHolidays { get; set; }

    IReadOnlyList<NamedHoliday>? FollowsIrregularHolidays { get; set; }

    IReadOnlyList<ICalendarDateHolidayConfig>? UpcomingIrregularHolidays { get; set; }

    bool IsInHoliday(DateTimeOffset atThisDateTime);

    ITimeTableConfig Clone();
}

public readonly struct TimeTable
(
    TimeZoneInfo operatingTimeZone
  , WeeklyTimeTable weeklyTimeTableConfig
  , CalendarDateHoliday[]? consistentCalendarHolidays = null
  , NamedHoliday[]? followsIrregularHolidays = null
  , CalendarDateHoliday[]? upcomingIrregularHolidays = null)
{
    private static readonly CalendarDateHoliday[] EmptyReadonlyHoliday = [];

    public TimeZoneInfo OperatingTimeZone { get; } = operatingTimeZone;

    public WeeklyTimeTable WeeklyTimeTableConfig { get; } = weeklyTimeTableConfig;

    public CalendarDateHoliday[] CalendarHolidays { get; } = consistentCalendarHolidays ?? EmptyReadonlyHoliday;

    public NamedHoliday[]? FollowsIrregularHolidays { get; } = followsIrregularHolidays;

    public CalendarDateHoliday[] UpcomingIrregularHolidays { get; } = upcomingIrregularHolidays ?? EmptyReadonlyHoliday;
}

public class TimeTableConfig : ConfigSection, ITimeTableConfig
{
    public TimeTableConfig() { }

    public TimeTableConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public TimeTableConfig
    (TimeZoneInfo operatingTimeZone, IWeeklyTimeTableConfig regularTimeTable, IReadOnlyList<ICalendarDateHolidayConfig> consistentCalendarHolidays
      , IReadOnlyList<NamedHoliday>? followsIrregularHolidays = null, IReadOnlyList<ICalendarDateHolidayConfig>? upcomingIrregularHolidays = null)
        : this(operatingTimeZone, regularTimeTable, consistentCalendarHolidays, InMemoryConfigRoot, InMemoryPath, followsIrregularHolidays
             , upcomingIrregularHolidays) { }

    public TimeTableConfig
    (TimeZoneInfo operatingTimeZone, IWeeklyTimeTableConfig regularTimeTable, IReadOnlyList<ICalendarDateHolidayConfig> consistentCalendarHolidays
      , IConfigurationRoot root, string path
      , IReadOnlyList<NamedHoliday>? followsIrregularHolidays = null
      , IReadOnlyList<ICalendarDateHolidayConfig>? upcomingIrregularHolidays = null) : base(root, path)
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
            new WeeklyTimeTableConfig(ConfigRoot, $"{Path}{Split}{nameof(WeeklyTimeTableConfig)}")
            {
                ParentTimeZone = OperatingTimeZone
            };
        set
        {
            _ = new WeeklyTimeTableConfig(value, ConfigRoot, $"{Path}{Split}{nameof(WeeklyTimeTableConfig)}");
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

    public IReadOnlyList<ICalendarDateHolidayConfig> CalendarHolidays
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<ReusableList<ICalendarDateHolidayConfig>>();
            foreach (var calendarDay in GetSection(nameof(CalendarHolidays)).GetChildren())
                if (calendarDay["Month"] != null)
                    autoRecycleList.Add(new CalendarDateHolidayConfig(ConfigRoot, calendarDay.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = CalendarHolidays.Count;

            var i = 0;

            foreach (var calendarDay in value)
                _ = new CalendarDateMatchConfig(calendarDay, ConfigRoot, $"{Path}{Split}{nameof(CalendarHolidays)}{Split}{i++}");

            for (var j = i; j < oldCount; j++) CalendarDateHolidayConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(CalendarHolidays)}{Split}{j++}");
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

    public IReadOnlyList<ICalendarDateHolidayConfig>? UpcomingIrregularHolidays
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<ReusableList<ICalendarDateHolidayConfig>>();
            foreach (var calendarDay in GetSection(nameof(UpcomingIrregularHolidays)).GetChildren())
                if (calendarDay["Month"] != null)
                    autoRecycleList.Add(new CalendarDateHolidayConfig(ConfigRoot, calendarDay.Path));
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
                    _ = new CalendarDateHolidayConfig(calendarDay, ConfigRoot, $"{Path}{Split}{nameof(UpcomingIrregularHolidays)}{Split}{i}");
                }

            for (var j = i; j < oldCount; j++)
                CalendarDateHolidayConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(UpcomingIrregularHolidays)}{Split}{j}");
        }
    }

    public TradingPeriodTypeFlags GetExpectedAvailability(DateTimeOffset atThisDateTime)
    {
        var fullSchedule = WeeklySchedule(atThisDateTime);
        var marketState  = fullSchedule.CurrentActiveAvailabilityTransition(atThisDateTime).MarketState;
        fullSchedule.DecrementRefCount();
        return marketState;
    }

    public void AddWeeklyOnOffTradingState(WeeklyTradingSchedule tradingSchedule, TradingPeriodTypeFlags onState, TradingPeriodTypeFlags offState)
    {
        var prevTransition = tradingSchedule[0];
        for (var i = 0; i < tradingSchedule.Count; i++)
        {
            var currentTransition = tradingSchedule[i];
            if (prevTransition.IsOpenTransition(currentTransition))
            {
                var openingTransition = currentTransition;


                AvailabilityTransitionTime closeTransition = openingTransition;

                var untilClosePrev = openingTransition;
                for (int j = i + 1; j < tradingSchedule.Count; j++)
                {
                    var untilCloseCurrent = tradingSchedule[j];
                    if (untilClosePrev.IsClosedTransition(untilCloseCurrent))
                    {
                        closeTransition = untilCloseCurrent;
                        break;
                    }
                    untilClosePrev = untilCloseCurrent;
                }

                foreach (var startStopTimes in WeeklyTimeTableConfig.ToStartStopTimesInWeek(openingTransition.AtTime, closeTransition.AtTime))
                {
                    var startTime = startStopTimes.StartTime;
                    var stopTime  = startStopTimes.StopTime;
                    if (startTime < openingTransition.AtTime)
                    {
                        startTime = openingTransition.AtTime;
                    }

                    var foundIndex = tradingSchedule.FindTimeMatchAt(startTime);
                    if (foundIndex > 0)
                    {
                        tradingSchedule[foundIndex] = tradingSchedule[foundIndex].AddTradingState(onState);
                    }
                    else
                    {
                        var entryBeforeNew = tradingSchedule.FindEntryActiveAt(stopTime);
                        var highActivityOpenTransition = new AvailabilityTransitionTime
                            (startTime, (entryBeforeNew.MarketState & ~(offState)) | onState);
                        tradingSchedule.Add(highActivityOpenTransition);
                    }
                    foundIndex = tradingSchedule.FindTimeMatchAt(openingTransition.AtTime);
                    if (foundIndex > 0)
                    {
                        untilClosePrev = openingTransition;
                        for (int j = foundIndex; j < tradingSchedule.Count; j++)
                        {
                            var untilCloseCurrent = tradingSchedule[j];
                            if (untilCloseCurrent.AtTime < startTime)
                            {
                                tradingSchedule[j] = untilCloseCurrent.AddTradingState(offState);
                            }
                            else if (untilCloseCurrent.AtTime > startTime && untilCloseCurrent.AtTime < stopTime &&
                                     !untilClosePrev.IsClosedTransition(untilCloseCurrent))
                            {
                                tradingSchedule[j] = untilCloseCurrent.WithNewState
                                    ((untilCloseCurrent.MarketState & ~(offState)) | onState);
                            }
                            if (untilClosePrev.IsClosedTransition(untilCloseCurrent))
                            {
                                break;
                            }
                            untilClosePrev = untilCloseCurrent;
                        }
                    }
                    if (stopTime > closeTransition.AtTime)
                    {
                        stopTime = closeTransition.AtTime;
                    }

                    foundIndex = tradingSchedule.FindTimeMatchAt(stopTime);
                    if (foundIndex > 0)
                    {
                        tradingSchedule[foundIndex] = tradingSchedule[foundIndex]
                            .WithNewState((tradingSchedule[foundIndex].MarketState & ~(onState)) | offState);
                    }
                    else
                    {
                        var entryBeforeNew = tradingSchedule.FindEntryActiveAt(stopTime);
                        var highActivityOpenTransition = new AvailabilityTransitionTime
                            (stopTime, (entryBeforeNew.MarketState & ~(onState)) | offState);
                        tradingSchedule.Add(highActivityOpenTransition);
                    }
                    foundIndex = tradingSchedule.FindTimeMatchAt(openingTransition.AtTime);
                    if (foundIndex > 0)
                    {
                        untilClosePrev = openingTransition;
                        for (int j = foundIndex; j < tradingSchedule.Count; j++)
                        {
                            var untilCloseCurrent = tradingSchedule[j];
                            if (untilCloseCurrent.AtTime > stopTime && !untilClosePrev.IsClosedTransition(untilCloseCurrent))
                            {
                                tradingSchedule[j] = untilCloseCurrent.WithNewState
                                    ((untilCloseCurrent.MarketState & ~(onState)) | offState);
                            }
                            if (untilClosePrev.IsClosedTransition(untilCloseCurrent))
                            {
                                break;
                            }
                            untilClosePrev = untilCloseCurrent;
                        }
                    }
                }
            }
            prevTransition = currentTransition;
        }
    }

    public bool IsInHoliday(DateTimeOffset atThisDateTime) =>
        CalendarHolidays.Any(cdmc => cdmc.DateMatches(atThisDateTime))
     || (UpcomingIrregularHolidays?.Any(cdmc => cdmc.DateMatches(atThisDateTime)) ?? false);


    public WeeklyTradingSchedule WeeklySchedule(DateTimeOffset forTimeInWeek)
    {
        var rawWeeklyTradingSchedule = WeeklyTimeTableConfig.WeeklySchedule(forTimeInWeek);
        var publicHolidays           = CalculateCarriedPublicHolidays((ushort)forTimeInWeek.TruncToWeekBoundary().AddDays(1).Year);
        var fullWeeklySchedule       = Recycler.Borrow<WeeklyTradingSchedule>().Initialise(forTimeInWeek);

        var weekStartOffset = TimeZoneInfo.ConvertTime(forTimeInWeek, OperatingTimeZone).TruncToWeekBoundary();
        fullWeeklySchedule.Add(new AvailabilityTransitionTime(weekStartOffset, IsMarketClosed | IsWeekend));

        for (var i = 0; i < rawWeeklyTradingSchedule.Count; i++)
        {
            var availabilityTransition = rawWeeklyTradingSchedule[i];
            if (publicHolidays.Contains(availabilityTransition.AtTime))
            {
                if (availabilityTransition.MarketState.IsOpen())
                {
                    fullWeeklySchedule.Add(new AvailabilityTransitionTime(availabilityTransition.AtTime, IsMarketClosed | IsPublicHoliday));
                }
                else
                {
                    fullWeeklySchedule.Add(availabilityTransition);
                }
            }
            else
            {
                fullWeeklySchedule.Add(availabilityTransition);
            }
        }
        publicHolidays.DecrementRefCount();
        rawWeeklyTradingSchedule.DecrementRefCount();
        return fullWeeklySchedule;
    }

    public CalendarDatesList CalculateCarriedPublicHolidays(ushort year)
    {
        var weekendsInYear = AllWeekends(year);
        var publicHolidays = Recycler.Borrow<CalendarDatesList>();

        foreach (var calendarDateMatchConfig in CalendarHolidays)
        {
            var thisYearDate = new CalendarDate(year, (byte)calendarDateMatchConfig.Month, calendarDateMatchConfig.ResolveDateForYear(year));
            var isOnWeekend  = weekendsInYear.Contains(thisYearDate);
            if (isOnWeekend && calendarDateMatchConfig.CarryWeekendDirection != 0)
            {
                thisYearDate = calendarDateMatchConfig.CarryWeekendDirection > 0
                    ? weekendsInYear.NextDateNotInList(thisYearDate)
                    : weekendsInYear.PreviousDateNotInList(thisYearDate);
            }
            publicHolidays.Add(thisYearDate);
        }
        weekendsInYear.DecrementRefCount();
        return publicHolidays;
    }

    public CalendarDatesList AllWeekends(ushort year)
    {
        var weekendsInYear = Recycler.Borrow<CalendarDatesList>();
        weekendsInYear.Capacity = 128; // base 2 growth for lists
        var currentDayOfYear = new DateTime(year, 1, 1);

        var weekdays = WeeklyTimeTableConfig.DaysOfWeek;
        var weekends = ~(weekdays);

        for (int i = 0; i < 366 && currentDayOfYear.Year == year; i++)
        {
            if ((currentDayOfYear.DayOfWeek.ToDayOfWeekFlags() & weekends) > 0)
            {
                weekendsInYear.Add(new CalendarDate((ushort)currentDayOfYear.Year, (byte)currentDayOfYear.Month, (byte)currentDayOfYear.Day));
            }
            currentDayOfYear = currentDayOfYear.AddDays(1);
        }
        return weekendsInYear;
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(OperatingTimeZone), OperatingTimeZone.Id)
            .Field.AlwaysAdd(nameof(WeeklyTimeTableConfig), WeeklyTimeTableConfig)
            .CollectionField.AlwaysAddAll(nameof(CalendarHolidays), CalendarHolidays)
            .CollectionField.AlwaysAddAll(nameof(FollowsIrregularHolidays), FollowsIrregularHolidays)
            .CollectionField.AlwaysAddAll(nameof(UpcomingIrregularHolidays), UpcomingIrregularHolidays)
            .Complete();

    public override string ToString() =>
        $"{nameof(TimeTableConfig)}{{{nameof(OperatingTimeZone)}: {OperatingTimeZone}, {nameof(WeeklyTimeTableConfig)}: {WeeklyTimeTableConfig}, " +
        $"{nameof(CalendarHolidays)}: {CalendarHolidays}, {nameof(FollowsIrregularHolidays)}: {FollowsIrregularHolidays}, " +
        $"{nameof(UpcomingIrregularHolidays)}: {UpcomingIrregularHolidays}}}";
}
