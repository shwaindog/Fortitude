#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public interface ITimerUpdate : IRecyclableObject<ITimerUpdate>
{
    bool IsFinished { get; }
    bool IsPaused { get; }
    DateTime NextScheduleDateTime { get; }
    ITimer RegisteredTimer { get; }

    bool Cancel();
    bool ExecuteNowOnThreadPool();
    bool ExecuteNowOnThisThread();
    bool UpdateWaitPeriod(int newWaitFromNowMs);
    bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan);
    bool Pause();
    bool Resume();
}
