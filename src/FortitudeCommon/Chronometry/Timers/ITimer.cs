namespace FortitudeCommon.Chronometry.Timers;

public interface ITimer
{
    ITimerUpdate RunIn(TimeSpan waitTimeSpan, WaitCallback callback);
    ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback);
    ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T> callback) where T : class;
    ITimerUpdate RunIn(int waitMs, WaitCallback callback);
    ITimerUpdate RunIn(int waitMs, Action callback);
    ITimerUpdate RunIn<T>(int waitMs, T state, Action<T> callback) where T : class;
    ITimerUpdate RunEvery(int intervalMs, WaitCallback callback);
    ITimerUpdate RunEvery(int intervalMs, Action callback);
    ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T> callback) where T : class;
    ITimerUpdate RunEvery(TimeSpan periodTimeSpan, WaitCallback callback);
    ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback);
    ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T> callback) where T : class;
    ITimerUpdate RunAt(DateTime futureDateTime, WaitCallback callback);
    ITimerUpdate RunAt(DateTime futureDateTime, Action callback);
    ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T> callback) where T : class;
    void PauseAllTimers();
    void ResumeAllTimers();
    TimerCallBackRunInfo? RunNextScheduledOneOffCallbackNowOnThreadPool();
    TimerCallBackRunInfo? RunNextScheduledIntervalCallbackNowOnThreadPool();
    TimerCallBackRunInfo? RunNextScheduledOneOffCallbackNowOnThisThread();
    TimerCallBackRunInfo? RunNextScheduledIntervalCallbackNowThisThread();
}
