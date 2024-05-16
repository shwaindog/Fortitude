namespace FortitudeCommon.Extensions;

public static class DateTimeExtensions
{
    private static long fiveMinuteTicks = TimeSpan.TicksPerMinute * 5;
    private static long tenMinuteTicks = TimeSpan.TicksPerMinute * 10;
    private static long fifteenMinuteTicks = TimeSpan.TicksPerMinute * 15;
    private static long thirtyMinuteTicks = TimeSpan.TicksPerMinute * 30;
    private static long fourHourTicks = TimeSpan.TicksPerHour * 4;

    public static DateTime TruncToSecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerSecond));

    public static DateTime TruncToMinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerMinute));

    public static DateTime TruncTo5MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % fiveMinuteTicks));

    public static DateTime TruncTo10MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % tenMinuteTicks));

    public static DateTime TruncTo15MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % fifteenMinuteTicks));

    public static DateTime TruncTo30MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % thirtyMinuteTicks));

    public static DateTime TruncTo1HourBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerHour));

    public static DateTime TruncTo4HourBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % fourHourTicks));

    public static DateTime TruncToDayBoundary(this DateTime allTicks) => allTicks.Date;

    public static DateTime TruncToWeekBoundary(this DateTime allTicks)
    {
        var currentDay = allTicks.Date;
        return currentDay.DayOfWeek switch
        {
            DayOfWeek.Monday => currentDay.AddDays(-1), DayOfWeek.Tuesday => currentDay.AddDays(-2), DayOfWeek.Wednesday => currentDay.AddDays(-3)
            , DayOfWeek.Thursday => currentDay.AddDays(-4), DayOfWeek.Friday => currentDay.AddDays(-5), DayOfWeek.Saturday => currentDay.AddDays(-6)
            , _ => currentDay
        };
    }

    public static DateTime TruncToMonthBoundary(this DateTime allTicks) => allTicks.Date.AddDays(-allTicks.Date.Day);

    public static DateTime TruncToYearBoundary(this DateTime allTicks) => allTicks.Date.AddDays(-allTicks.Date.DayOfYear);
}
