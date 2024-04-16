#region

using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IRingPoller : IDisposable
{
    IOSThread? ExecutingThread { get; }
    bool IsRunning { get; }
    int UsageCount { get; }
    void WakeIfAsleep();
    void Start(Action? threadStartInitialize = null);
    void Stop();

    event Action<QueueEventTime> QueueEntryStart;
    event Action<QueueEventTime> QueueEntryComplete;
}

public interface IRingPoller<out T> : IRingPoller where T : class
{
    IPollingRing<T> Ring { get; }
}
