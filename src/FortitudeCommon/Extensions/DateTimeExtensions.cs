namespace FortitudeCommon.Extensions;

public static class DateTimeExtensions
{
    public static DateTime TruncToSecond(this DateTime allTicks) =>
        allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerSecond));
}
