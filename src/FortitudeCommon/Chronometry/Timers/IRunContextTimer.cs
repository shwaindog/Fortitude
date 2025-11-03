// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public interface IActionTimer
{
    IRecycler? Recycler { get; set; }

    ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<ValueTask> callback);
    ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<IScheduleActualTime?, ValueTask> callback);
    ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<T?, ValueTask> callback) where T : class;
    ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class;
    ITimerUpdate RunIn(int waitMs, Func<ValueTask> callback);
    ITimerUpdate RunIn(int waitMs, Func<IScheduleActualTime?, ValueTask> callback);
    ITimerUpdate RunIn<T>(int waitMs, T state, Func<T?, ValueTask> callback) where T : class;
    ITimerUpdate RunIn<T>(int waitMs, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class;
    ITimerUpdate RunEvery(int intervalMs, Func<ValueTask> callback);
    ITimerUpdate RunEvery(int intervalMs, Func<IScheduleActualTime?, ValueTask> callback);
    ITimerUpdate RunEvery<T>(int intervalMs, T state, Func<T?, ValueTask> callback) where T : class;
    ITimerUpdate RunEvery<T>(int intervalMs, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class;
    ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<ValueTask> callback);
    ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<IScheduleActualTime?, ValueTask> callback);
    ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<T?, ValueTask> callback) where T : class;
    ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class;
    ITimerUpdate RunAt(DateTime futureDateTime, Func<IScheduleActualTime?, ValueTask> callback);
    ITimerUpdate RunAt(DateTime futureDateTime, Func<ValueTask> callback);
    ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<T?, ValueTask> callback) where T : class;
    ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class;
    ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback);
    ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action<IScheduleActualTime?> callback);
    ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class;
    ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<IScheduleActualTime<T>?> callback) where T : class;
    ITimerUpdate RunIn(int waitMs, Action callback);
    ITimerUpdate RunIn(int waitMs, Action<IScheduleActualTime?> callback);
    ITimerUpdate RunIn<T>(int waitMs, T state, Action<T?> callback) where T : class;
    ITimerUpdate RunIn<T>(int waitMs, T state, Action<IScheduleActualTime<T>?> callback) where T : class;
    ITimerUpdate RunEvery(int intervalMs, Action callback);
    ITimerUpdate RunEvery(int intervalMs, Action<IScheduleActualTime?> callback);
    ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T?> callback) where T : class;
    ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<IScheduleActualTime<T>?> callback) where T : class;
    ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback);
    ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action<IScheduleActualTime?> callback);
    ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T?> callback) where T : class;
    ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<IScheduleActualTime<T>?> callback) where T : class;
    ITimerUpdate RunAt(DateTime futureDateTime, Action callback);
    ITimerUpdate RunAt(DateTime futureDateTime, Action<IScheduleActualTime?> callback);
    ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T?> callback) where T : class;
    ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<IScheduleActualTime<T>?> callback) where T : class;

    void PauseAllTimers();
    void ResumeAllTimers();
    void StopAllTimers();
}

public interface IThreadPoolTimer
{
    ITimerUpdate RunIn(TimeSpan waitTimeSpan, WaitCallback callback);
    ITimerUpdate RunIn(TimeSpan waitTimeSpan, object? state, WaitCallback callback);
    ITimerUpdate RunIn(int waitMs, WaitCallback callback);
    ITimerUpdate RunIn(int waitMs, object? state, WaitCallback callback);
    ITimerUpdate RunEvery(int intervalMs, WaitCallback callback);
    ITimerUpdate RunEvery(int intervalMs, object? state, WaitCallback callback);
    ITimerUpdate RunEvery(TimeSpan periodTimeSpan, WaitCallback callback);
    ITimerUpdate RunEvery(TimeSpan periodTimeSpan, object? state, WaitCallback callback);
    ITimerUpdate RunAt(DateTime futureDateTime, WaitCallback callback);
    ITimerUpdate RunAt(DateTime futureDateTime, object? state, WaitCallback callback);
}

public interface IRunContextTimer : IActionTimer, IThreadPoolTimer { }

public interface IUpdateableTimer : IRunContextTimer
{
    bool Remove(ITimerCallBackRunInfo toRemove);
    void CheckNextOneOffLaunchTimeStillCorrect(ITimerCallBackRunInfo toCheck);

    ITimerCallBackRunInfo? RunNextScheduledOneOffCallbackNow();
    ITimerCallBackRunInfo? RunNextScheduledIntervalCallbackNow();
    ITimerCallBackRunInfo? RunNextScheduledOneOffCallbackNowOnThreadPool();
    ITimerCallBackRunInfo? RunNextScheduledIntervalCallbackNowOnThreadPool();
}
