#region

using System.Diagnostics;

#endregion

namespace FortitudeCommon.Chronometry;

public sealed class DateTimeEx
{
    private const long ClockTickFrequency = 10000000;
    private readonly long syncPeriodStopwatchTicks;
    private DateTimeExData oldDt;
    public Stopwatch Stopwatch;

    public DateTimeEx() : this(10) { }

    public DateTimeEx(long syncPeriodSeconds)
    {
        Stopwatch = Stopwatch.StartNew();

        var now = DateTime.UtcNow;
        oldDt = new DateTimeExData(now, now, Stopwatch.ElapsedTicks, Stopwatch.Frequency);

        syncPeriodStopwatchTicks = syncPeriodSeconds * Stopwatch.Frequency;
    }

    public DateTime UtcNow
    {
        get
        {
            var elapsedTicks = Stopwatch.ElapsedTicks;
            var newDt = oldDt;
            var now = DateTime.UtcNow;

            if ((now - newDt.ObservedTime).TotalMinutes > 1)
                newDt = oldDt = new DateTimeExData(now, now, Stopwatch.ElapsedTicks, Stopwatch.Frequency);

            DateTime preciseNow;
            if (elapsedTicks < newDt.ObservedTicks + syncPeriodStopwatchTicks)
            {
                preciseNow =
                    newDt.BaseTime.AddTicks((elapsedTicks - newDt.ObservedTicks) * ClockTickFrequency /
                                            newDt.Frequency);
            }
            else
            {
                var baseTime =
                    newDt.BaseTime.AddTicks((elapsedTicks - newDt.ObservedTicks) * ClockTickFrequency /
                                            newDt.Frequency);
                oldDt = new DateTimeExData(now, baseTime, elapsedTicks,
                    (elapsedTicks - newDt.ObservedTicks) * ClockTickFrequency * 2 /
                    (now.Ticks - newDt.ObservedTime.Ticks + now.Ticks - baseTime.Ticks + now.Ticks -
                     newDt.ObservedTime.Ticks));
                preciseNow = baseTime;
            }

            return preciseNow;
        }
    }

    private struct DateTimeExData
    {
        public readonly DateTime BaseTime;
        public readonly long Frequency;
        public readonly long ObservedTicks;
        public readonly DateTime ObservedTime;

        public DateTimeExData(DateTime observedTime, DateTime baseTime,
            long observedTicks, long frequency)
        {
            ObservedTime = observedTime;
            BaseTime = baseTime;
            ObservedTicks = observedTicks;
            Frequency = frequency;
        }
    }
}
