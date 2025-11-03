// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public interface ITimerUpdate : IReusableObject<ITimerUpdate>, IAsyncValueTaskDisposable
{
    bool IsFinished { get; }
    bool IsPaused   { get; }

    DateTime         NextScheduleDateTime { get; }
    IRunContextTimer RegisteredTimer      { get; }

    bool Cancel();
    bool ExecuteNowOnThisThread();
    bool UpdateWaitPeriod(int newWaitFromNowMs);
    bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan);
    bool Pause();
    bool Resume();
}

public interface IThreadPoolTimerUpdate : ITimerUpdate
{
    bool ExecuteNowOnThreadPool();
}
