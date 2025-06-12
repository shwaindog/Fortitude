// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Extensions;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.Availability;

public interface IWeeklyTimeTableConfig : IAvailability
{
    TimeZoneInfo DefaultTimeZone { get; set; }

    IWeeklyTimesConfig OpeningTimes { get; set; }
    IWeeklyTimesConfig ClosingTimes { get; set; }
}

public struct WeeklyTimeTable
{
    public WeeklyTimeTable(TimeZoneInfo defaultTimeZone, WeeklyTimes openingTimes, WeeklyTimes closingTimes)
    {
        DefaultTimeZone = defaultTimeZone;
        OpeningTimes    = openingTimes;
        ClosingTimes    = closingTimes;
    }

    public TimeZoneInfo DefaultTimeZone { get; }
    public WeeklyTimes  OpeningTimes    { get; }
    public WeeklyTimes  ClosingTimes    { get; }
}

public class WeeklyTimeTableConfig : ConfigSection, IWeeklyTimeTableConfig
{
    private static readonly WeekDayOpenClosePairComparer SortByOpenTimesComparer = new();

    // ReSharper disable once NotAccessedField.Local
    private static object? ignoreSuppressWarnings;

    private readonly List<WeekDayOpenClosePair> weeklyOpenAndCloseTimes = new();

    public WeeklyTimeTableConfig(IConfigurationRoot root, string path) : base(root, path)
    {
        GetSortedOpenCloseTimes();
    }

    public WeeklyTimeTableConfig() { }

    public WeeklyTimeTableConfig
    (TimeZoneInfo defaultTimeZone, IWeeklyTimesConfig openingTimes, IWeeklyTimesConfig closingTimes, IConfigurationRoot root
      , string path) : base(root, path)
    {
        DefaultTimeZone = defaultTimeZone;
        OpeningTimes    = openingTimes;
        ClosingTimes    = closingTimes;

        GetSortedOpenCloseTimes();
    }

    public WeeklyTimeTableConfig(TimeZoneInfo defaultTimeZone, IWeeklyTimesConfig openingTimes, IWeeklyTimesConfig closingTimes)
        : this(defaultTimeZone, openingTimes, closingTimes, InMemoryConfigRoot, InMemoryPath) { }

    public WeeklyTimeTableConfig(IWeeklyTimeTableConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        DefaultTimeZone = toClone.DefaultTimeZone;
        OpeningTimes    = toClone.OpeningTimes;
        ClosingTimes    = toClone.ClosingTimes;
        GetSortedOpenCloseTimes();
    }

    public WeeklyTimeTableConfig(IWeeklyTimeTableConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeZoneInfo DefaultTimeZone
    {
        get
        {
            var checkValue = this[nameof(DefaultTimeZone)];
            return checkValue != null ? TimeZoneInfo.FindSystemTimeZoneById(checkValue) : TimeZoneInfo.Utc;
        }
        set => this[nameof(DefaultTimeZone)] = value?.Id;
    }

    public IWeeklyTimesConfig OpeningTimes
    {
        get => new WeeklyTimesConfig(ConfigRoot, Path + ":" + nameof(OpeningTimes));
        set => ignoreSuppressWarnings = new WeeklyTimesConfig(value, ConfigRoot, Path + ":" + nameof(OpeningTimes));
    }

    public IWeeklyTimesConfig ClosingTimes
    {
        get => new WeeklyTimesConfig(ConfigRoot, Path + ":" + nameof(ClosingTimes));
        set => ignoreSuppressWarnings = new WeeklyTimesConfig(value, ConfigRoot, Path + ":" + nameof(ClosingTimes));
    }

    public bool ShouldBeUp(DateTimeOffset atThisDateTime)
    {
        if (weeklyOpenAndCloseTimes.Count == 0) GetSortedOpenCloseTimes();
        return weeklyOpenAndCloseTimes.Any(wdocp => wdocp.WithinOpeningHours(atThisDateTime));
    }

    public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
    {
        if (!ShouldBeUp(fromNow)) return TimeSpan.Zero;
        var currentOpeningHours = weeklyOpenAndCloseTimes.First(wdocp => wdocp.WithinOpeningHours(fromNow));
        var sinceUtcStartOfWeek = fromNow.TimeSinceUtcStartOfWeek();
        return currentOpeningHours.CloseTime.StartOfUtcWeekTimeSpan() - sinceUtcStartOfWeek;
    }

    public DateTimeOffset NextScheduledOpeningTime(DateTimeOffset fromNow)
    {
        if (weeklyOpenAndCloseTimes.Count == 0) GetSortedOpenCloseTimes();
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
        if (weeklyOpenAndCloseTimes.Count == 0) GetSortedOpenCloseTimes();
        if(weeklyOpenAndCloseTimes.Any(wdocp => wdocp.WithinOpeningHours(atThisDateTime)))
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

    private void GetSortedOpenCloseTimes()
    {
        var openTimesList  = OpeningTimes.TimeZonedWeekDayTimes(DefaultTimeZone).ToList();
        var closeTimesList = ClosingTimes.TimeZonedWeekDayTimes(DefaultTimeZone).ToList();

        if (openTimesList.Count != closeTimesList.Count) throw new ArgumentException("Expected opening times and closing times to match off");

        for (var i = 0; i < openTimesList.Count; i++)
        {
            var openTime  = openTimesList[i];
            var closeTime = openTimesList[i];
            if (openTime > closeTime) throw new ArgumentException("Expected opening time to be less than or equal to closing time");
            weeklyOpenAndCloseTimes.Add(new WeekDayOpenClosePair(openTime, closeTime));
        }
        weeklyOpenAndCloseTimes.Sort(0, weeklyOpenAndCloseTimes.Count, SortByOpenTimesComparer);
    }
}
