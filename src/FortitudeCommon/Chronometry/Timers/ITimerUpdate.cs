#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public interface ITimerUpdate : IRecyclableObject<ITimerUpdate>
{
    Timer RegisteredTimer { get; }

    bool Cancel();
    bool ExecuteNowOnThreadPool();
    bool ExecuteNowOnThisThread();
    bool UpdateWaitPeriod(int newWaitFromNowMs);
    bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan);
    bool Pause();
    bool Resume();
}
