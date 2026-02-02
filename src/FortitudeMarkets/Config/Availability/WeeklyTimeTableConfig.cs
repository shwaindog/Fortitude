// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.Availability;

public interface IWeeklyTimeTableConfig : IWeeklyAvailability, IInterfacesComparable<IWeeklyTimeTableConfig>, IStringBearer
{
    TimeZoneInfo? OverrideTimeZone { get; set; }

    DayOfWeekFlags DaysOfWeek { get; set; }

    IWeeklyTimesConfig StartTimes { get; set; }

    IWeeklyTimesConfig StopTimes { get; set; }

    WeekdayStartStopPairList ToStartStopTimesInWeek(DateTimeOffset forTimeInWeek, DateTimeOffset toTimeInWeek);
}

public readonly struct WeeklyTimeTable
(
    WeeklyTimes startTimes
  , WeeklyTimes stopTimes
  , TimeZoneInfo? overrideTimeZone = null
  , DayOfWeekFlags? daysOfWeek = null)
{
    public WeeklyTimes StartTimes { get; } = startTimes;

    public WeeklyTimes StopTimes { get; } = stopTimes;

    public TimeZoneInfo? OverrideTimeZone { get; } = overrideTimeZone;

    public DayOfWeekFlags? DaysOfWeek { get; } = daysOfWeek;
}

public class WeeklyTimeTableConfig : ConfigSection, IWeeklyTimeTableConfig
{
    public WeeklyTimeTableConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public WeeklyTimeTableConfig() { }

    public WeeklyTimeTableConfig
    (TimeZoneInfo defaultTimeZone, IWeeklyTimesConfig openingTimes, IWeeklyTimesConfig closingTimes, IConfigurationRoot root
      , string path) : base(root, path)
    {
        OverrideTimeZone = defaultTimeZone;
        StartTimes       = openingTimes;
        StopTimes        = closingTimes;
    }

    public WeeklyTimeTableConfig(TimeZoneInfo defaultTimeZone, IWeeklyTimesConfig openingTimes, IWeeklyTimesConfig closingTimes)
        : this(defaultTimeZone, openingTimes, closingTimes, InMemoryConfigRoot, InMemoryPath) { }

    public WeeklyTimeTableConfig(IWeeklyTimeTableConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        OverrideTimeZone = toClone.OverrideTimeZone;
        StartTimes       = toClone.StartTimes;
        StopTimes        = toClone.StopTimes;
    }

    public WeeklyTimeTableConfig(IWeeklyTimeTableConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeZoneInfo? OverrideTimeZone
    {
        get
        {
            var checkValue = this[nameof(OverrideTimeZone)];
            return checkValue != null ? TimeZoneInfo.FindSystemTimeZoneById(checkValue) : ParentTimeZone;
        }
        set => this[nameof(OverrideTimeZone)] = value?.Id;
    }

    public TimeZoneInfo? ParentTimeZone { get; set; }

    public DayOfWeekFlags DaysOfWeek
    {
        get
        {
            var checkValue = this[nameof(DaysOfWeek)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<DayOfWeekFlags>(checkValue!) : DayOfWeekFlags.None;
        }
        set => this[nameof(DaysOfWeek)] = value.ToString();
    }

    public IWeeklyTimesConfig StartTimes
    {
        get =>
            new WeeklyTimesConfig(ConfigRoot, $"{Path}{Split}{nameof(StartTimes)}")
            {
                ParentDaysOfWeek = DaysOfWeek, ParentTimeZone = OverrideTimeZone
            };
        set => _ = new WeeklyTimesConfig(value, ConfigRoot, $"{Path}{Split}{nameof(StartTimes)}");
    }

    public IWeeklyTimesConfig StopTimes
    {
        get =>
            new WeeklyTimesConfig(ConfigRoot, $"{Path}{Split}{nameof(StopTimes)}")
            {
                ParentDaysOfWeek = DaysOfWeek, ParentTimeZone = OverrideTimeZone
            };
        set => _ = new WeeklyTimesConfig(value, ConfigRoot, $"{Path}{Split}{nameof(StopTimes)}");
    }

    public bool ShouldBeUp(DateTimeOffset atThisDateTime)
    {
        var weeklySchedule = WeeklySchedule(atThisDateTime);
        var marketState = weeklySchedule.CurrentActiveAvailabilityTransition(atThisDateTime).MarketState;
        weeklySchedule.DecrementRefCount();
        return marketState.IsOpen();
    }

    public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
    {
        if (!ShouldBeUp(fromNow)) return TimeSpan.Zero;
        var weeklySchedule = WeeklySchedule(fromNow);
        var nextTransition    = 
            weeklySchedule.NextTransitionMatching
                (fromNow, nextState => (nextState & TradingPeriodTypeFlags.IsMarketClosed) > 0);
        weeklySchedule.DecrementRefCount();
        return nextTransition.AtTime - fromNow;
    }

    public DateTimeOffset NextScheduledOpeningTime(DateTimeOffset fromNow)
    {
        var weeklySchedule = WeeklySchedule(fromNow);
        if (weeklySchedule.Count == 0) throw new ArgumentException("No configured up or down times in the WeeklyTimeTable");
        
        var nextTransition    = 
            weeklySchedule.NextTransitionMatching
                (fromNow, nextState => (nextState & TradingPeriodTypeFlags.IsOpen) > 0);
        weeklySchedule.DecrementRefCount();
        if (nextTransition.MarketState.IsOpen())
        {
            weeklySchedule.DecrementRefCount();
            return nextTransition.AtTime;
        }
        // else closed for this week
        var timeToNextWeek = fromNow.TimeToNextUtcStartOfWeek();
        return NextScheduledOpeningTime(fromNow + timeToNextWeek);
    }

    public TradingPeriodTypeFlags GetExpectedAvailability(DateTimeOffset atThisDateTime)
    {
        var weeklySchedule = WeeklySchedule(atThisDateTime);
        var marketState    = weeklySchedule.CurrentActiveAvailabilityTransition(atThisDateTime).MarketState;
        weeklySchedule.DecrementRefCount();
        return marketState;
    }

    public WeeklyTradingSchedule WeeklySchedule(DateTimeOffset forTimeInWeek)
    {
        var weeklySchedule = Recycler.Borrow<WeeklyTradingSchedule>();

        var timeInWeek = TimeZoneInfo.ConvertTime(forTimeInWeek.TruncToWeekBoundary().AddDays(3), OverrideTimeZone!);

        var startTimesList  = StartTimes.TimeZonedWeekDayTimes();
        var stopTimesList = StopTimes.TimeZonedWeekDayTimes();

        if (startTimesList.Count != stopTimesList.Count) throw new ArgumentException("Expected opening times and closing times to match off");

        for (var i = 0; i < startTimesList.Count; i++)
        {
            var openTime  = startTimesList[i];
            var closeTime = stopTimesList[i];
            if (openTime.TimeOfDay > closeTime.TimeOfDay)
                throw new ArgumentException("Expected opening time to be less than or equal to closing time");
            var closeReason = (i < stopTimesList.Count - 1)
                ? TradingPeriodTypeFlags.IsMarketClosed | TradingPeriodTypeFlags.IsOutOfHours
                : TradingPeriodTypeFlags.IsMarketClosed | TradingPeriodTypeFlags.IsWeekend;
            weeklySchedule.Add
                (new AvailabilityTransitionTime(openTime.ToTimeInWeek(timeInWeek), TradingPeriodTypeFlags.IsOpen));
            weeklySchedule.Add
                (new AvailabilityTransitionTime(closeTime.ToTimeInWeek(timeInWeek), closeReason));
        }
        startTimesList.DecrementRefCount();
        stopTimesList.DecrementRefCount();
        return weeklySchedule;
    }

    public WeekdayStartStopPairList ToStartStopTimesInWeek(DateTimeOffset forTimeInWeek, DateTimeOffset toTimeInWeek)
    {
        var weeklySchedule = Recycler.Borrow<WeekdayStartStopPairList>();

        var openTimesList  = StartTimes.TimeZonedWeekDayTimes();
        var closeTimesList = StopTimes.TimeZonedWeekDayTimes();

        if (openTimesList.Count != closeTimesList.Count) throw new ArgumentException("Expected opening times and closing times to match off");

        for (var i = 0; i < openTimesList.Count; i++)
        {
            var openTime  = openTimesList[i];
            var closeTime = closeTimesList[i];
            if (openTime.TimeOfDay > closeTime.TimeOfDay)
                throw new ArgumentException("Expected opening time to be less than or equal to closing time");


            var startTime      = openTime.ToTimeInWeek(forTimeInWeek);
            var stopTime = closeTime.ToTimeInWeek(forTimeInWeek);
            if (startTime < toTimeInWeek && stopTime > forTimeInWeek)
            {
                weeklySchedule.Add(new WeekdayStartStopPair(startTime, stopTime));
            }
        }
        openTimesList.DecrementRefCount();
        closeTimesList.DecrementRefCount();
        return weeklySchedule;
    }


    public bool AreEquivalent(IWeeklyTimeTableConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var overrideTzSame = Equals(OverrideTimeZone, other.OverrideTimeZone);
        var daysOfWeekSame = DaysOfWeek == other.DaysOfWeek;
        var startTimesSame = StartTimes.AreEquivalent(other.StartTimes, exactTypes);
        var stopTimesSame  = StopTimes.AreEquivalent(other.StopTimes, exactTypes);

        var allAreSame = overrideTzSame && daysOfWeekSame && startTimesSame && stopTimesSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IWeeklyTimeTableConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = StartTimes.GetHashCode();
            hashCode = (hashCode * 397) ^ StopTimes.GetHashCode();
            hashCode = (hashCode * 397) ^ DaysOfWeek.GetHashCode();
            hashCode = (hashCode * 397) ^ (OverrideTimeZone != null ? OverrideTimeZone.GetHashCode() : 0);
            return hashCode;
        }
    }

    public virtual AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(StartTimes), StartTimes)
           .Field.AlwaysAddObject(nameof(StopTimes), StopTimes)
           .Field.AlwaysAddObject(nameof(OverrideTimeZone), OverrideTimeZone)
           .Field.AlwaysAdd(nameof(DaysOfWeek), DaysOfWeek)
           .Field.AlwaysAddObject(nameof(ParentTimeZone), ParentTimeZone)
           .Complete();
    }
    
    public override string ToString() =>
        $"{nameof(StartTimes)}: {StartTimes}, {nameof(StopTimes)}: {StopTimes}, {nameof(OverrideTimeZone)}: {OverrideTimeZone}, " +
        $"{nameof(DaysOfWeek)}: {DaysOfWeek}, {nameof(ParentTimeZone)}: {ParentTimeZone}";
}