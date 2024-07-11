// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class NoOpTimerUpdate : ReusableObject<ITimerUpdate>, ITimerUpdate
{
    public override ITimerUpdate CopyFrom(ITimerUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => this;

    public override ITimerUpdate Clone() => this;

    public ValueTask DisposeAwaitValueTask { get; set; }

    public ValueTask Dispose()      => ValueTask.CompletedTask;
    public ValueTask DisposeAsync() => Dispose();

    public bool IsFinished => true;
    public bool IsPaused   => true;

    public DateTime NextScheduleDateTime => DateTime.UtcNow + UpdateableTimer.MaxTimerSpan;

    public IRunContextTimer RegisteredTimer { get; set; } = null!;

    public bool Cancel() => true;

    public bool ExecuteNowOnThisThread() => false;

    public bool UpdateWaitPeriod(int newWaitFromNowMs) => true;

    public bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan) => true;

    public bool Pause()  => true;
    public bool Resume() => true;
}
