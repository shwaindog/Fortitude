// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.Availability;

public interface IWeeklyTimeTableConfig : IAvailability, IInterfacesComparable<IWeeklyTimeTableConfig>
{
    TimeZoneInfo? OverrideTimeZone { get; set; }

    DayOfWeekFlags DaysOfWeek { get; set; }

    IWeeklyTimesConfig StartTimes { get; set; }

    IWeeklyTimesConfig StopTimes { get; set; }
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
    private static readonly WeekDayOpenClosePairComparer SortByOpenTimesComparer = new();

    private List<WeekdayStartStopPair>? weeklyOpenAndCloseTimes;

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
            return StringExtensions.IsNotNullOrEmpty(checkValue) ? Enum.Parse<DayOfWeekFlags>(checkValue!) : DayOfWeekFlags.None;
        }
        set => this[nameof(DaysOfWeek)] = value.ToString();
    }

    public IWeeklyTimesConfig StartTimes
    {
        get =>
            new WeeklyTimesConfig(ConfigRoot, Path + ":" + nameof(StartTimes))
            {
                ParentDaysOfWeek = DaysOfWeek, ParentTimeZone = OverrideTimeZone
            };
        set => _ = new WeeklyTimesConfig(value, ConfigRoot, Path + ":" + nameof(StartTimes));
    }

    public IWeeklyTimesConfig StopTimes
    {
        get =>
            new WeeklyTimesConfig(ConfigRoot, Path + ":" + nameof(StopTimes))
            {
                ParentDaysOfWeek = DaysOfWeek, ParentTimeZone = OverrideTimeZone
            };
        set => _ = new WeeklyTimesConfig(value, ConfigRoot, Path + ":" + nameof(StopTimes));
    }

    public bool ShouldBeUp(DateTimeOffset atThisDateTime)
    {
        weeklyOpenAndCloseTimes ??= GetSortedOpenCloseTimes();
        return weeklyOpenAndCloseTimes.Any(wssp => wssp.WithinOpeningHours(atThisDateTime));
    }

    public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
    {
        if (!ShouldBeUp(fromNow)) return TimeSpan.Zero;
        weeklyOpenAndCloseTimes ??= GetSortedOpenCloseTimes();
        var currentOpeningHours = weeklyOpenAndCloseTimes.First(wssp => wssp.WithinOpeningHours(fromNow));
        var sinceUtcStartOfWeek = fromNow.TimeSinceUtcStartOfWeek();
        return currentOpeningHours.CloseTime.StartOfUtcWeekTimeSpan() - sinceUtcStartOfWeek;
    }

    public DateTimeOffset NextScheduledOpeningTime(DateTimeOffset fromNow)
    {
        weeklyOpenAndCloseTimes ??= GetSortedOpenCloseTimes();
        if (weeklyOpenAndCloseTimes.Count == 0) throw new ArgumentException("No configured up or down times in the WeeklyTimeTable");
        var sinceStartOfUtcWeek = fromNow.TimeSinceUtcStartOfWeek();
        int i;
        for (i = 0; i < weeklyOpenAndCloseTimes.Count; i++)
        {
            var currentOpenCloseTime = weeklyOpenAndCloseTimes[i];
            var weekStartOpen        = currentOpenCloseTime.OpenTime.StartOfUtcWeekTimeSpan();
            if (sinceStartOfUtcWeek < weekStartOpen) return fromNow + weekStartOpen - sinceStartOfUtcWeek;
            if (sinceStartOfUtcWeek > weekStartOpen && i + 1 < weeklyOpenAndCloseTimes.Count)
            {
                var nextOpenCloseTime = weeklyOpenAndCloseTimes[i + 1];
                return fromNow + nextOpenCloseTime.OpenTime.StartOfUtcWeekTimeSpan() - sinceStartOfUtcWeek;
            }
        }
        // else closed for this week
        var timeToNextWeek = fromNow.TimeToNextUtcStartOfWeek();
        var nextWeekOpen   = weeklyOpenAndCloseTimes[0];
        return fromNow + timeToNextWeek + nextWeekOpen.OpenTime.StartOfUtcWeekTimeSpan();
    }

    public TradingPeriodTypeFlags GetExpectedAvailability(DateTimeOffset atThisDateTime)
    {
        TradingPeriodTypeFlags tradingFlags = TradingPeriodTypeFlags.None;
        weeklyOpenAndCloseTimes ??= GetSortedOpenCloseTimes();
        if (weeklyOpenAndCloseTimes.Any(wssp => wssp.WithinOpeningHours(atThisDateTime)))
        {
            tradingFlags |= TradingPeriodTypeFlags.Open;
        }
        else
        {
            tradingFlags |= TradingPeriodTypeFlags.MarketClosed;
        }
        // TODO add other conditions
        return tradingFlags;
    }

    private List<WeekdayStartStopPair> GetSortedOpenCloseTimes()
    {
        var result = new List<WeekdayStartStopPair>();

        var openTimesList  = StartTimes.TimeZonedWeekDayTimes(OverrideTimeZone!).ToList();
        var closeTimesList = StopTimes.TimeZonedWeekDayTimes(OverrideTimeZone!).ToList();

        if (openTimesList.Count != closeTimesList.Count) throw new ArgumentException("Expected opening times and closing times to match off");

        for (var i = 0; i < openTimesList.Count; i++)
        {
            var openTime  = openTimesList[i];
            var closeTime = openTimesList[i];
            if (openTime.TimeOfDay > closeTime.TimeOfDay)
                throw new ArgumentException("Expected opening time to be less than or equal to closing time");
            result.Add(new WeekdayStartStopPair(openTime, closeTime));
        }
        result.Sort(0, result.Count, SortByOpenTimesComparer);
        return result;
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

    public override string ToString() =>
        $"{nameof(StartTimes)}: {StartTimes}, {nameof(StopTimes)}: {StopTimes}, {nameof(OverrideTimeZone)}: {OverrideTimeZone}, " +
        $"{nameof(DaysOfWeek)}: {DaysOfWeek}, {nameof(ParentTimeZone)}: {ParentTimeZone}";
}
