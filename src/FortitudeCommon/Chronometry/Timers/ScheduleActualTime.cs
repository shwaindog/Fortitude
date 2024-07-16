// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public interface IScheduleActualTime : IRecyclableObject
{
    ITimerUpdate? TimerUpdate   { get; }
    DateTime      ScheduleTime  { get; }
    DateTime      TimerTrigger  { get; }
    int           IntervalCount { get; }
    DateTime      ReceivedAt    { get; set; }
}

public interface IScheduleActualTime<TState> : IScheduleActualTime
{
    TState? State { get; set; }
}

public class ScheduleActualTime : RecyclableObject, IScheduleActualTime
{
    public ITimerUpdate? TimerUpdate { get; private set; }

    public int IntervalCount { get; private set; }

    public DateTime ScheduleTime { get; private set; }

    public DateTime TimerTrigger { get; private set; }

    public DateTime ReceivedAt { get; set; }

    public override void StateReset()
    {
        TimerUpdate   = null!;
        IntervalCount = 0;
        ScheduleTime  = default;
        TimerTrigger  = default;
        ReceivedAt    = default;
        base.StateReset();
    }

    public void Configure(ITimerUpdate timerUpdate, DateTime scheduleTime, DateTime timerTrigger, int intervalCount = 1)
    {
        TimerUpdate   = timerUpdate;
        ScheduleTime  = scheduleTime;
        TimerTrigger  = timerTrigger;
        IntervalCount = intervalCount;
    }
}

public class ScheduleActualTime<TState> : ScheduleActualTime, IScheduleActualTime<TState>
{
    public TState? State { get; set; }

    public override void StateReset()
    {
        State = default;
        base.StateReset();
    }

    public void Configure(ITimerUpdate timerUpdate, DateTime scheduleTime, DateTime timerTrigger, TState? state, int intervalCount = 1)
    {
        base.Configure(timerUpdate, scheduleTime, timerTrigger, intervalCount);
        State = state;
    }
}
