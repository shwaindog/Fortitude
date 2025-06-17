// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeMarkets.Config.Availability;

public interface ITradingAvailability
{
    bool HasTradingSchedule(string instrumentName);

    WeeklyTradingSchedule? TickerWeeklyTradingSchedule(DateTimeOffset forTimeInWeek, string instrumentName);

    IRecyclableReadOnlyDictionary<string, WeeklyTradingSchedule>? AllTickersWeeklyTradingSchedules(DateTime forTimeInWeek);
}

public interface IWeeklyAvailability
{
    WeeklyTradingSchedule WeeklySchedule(DateTimeOffset forTimeInWeek);
}

public interface IAlterWeeklyAvailability
{
    WeeklyTradingSchedule WeeklySchedule(DateTimeOffset forTimeInWeek, WeeklyTradingSchedule original);
}

public interface ICalendarAvailability : IWeeklyAvailability
{
    void AddWeeklyOnOffTradingState(WeeklyTradingSchedule tradingSchedule, TradingPeriodTypeFlags onState, TradingPeriodTypeFlags offState);

    CalendarDatesList CalculateCarriedPublicHolidays(ushort year);

    CalendarDatesList AllWeekends(ushort year);
}
