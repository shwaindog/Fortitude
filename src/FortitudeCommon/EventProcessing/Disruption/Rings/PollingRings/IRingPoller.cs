namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IRingPoller : IDisposable
{
    bool IsRunning { get; }
    int UsageCount { get; }
    void WakeIfAsleep();
    void Start(Action? threadStartInitialize = null);
    void Stop();
}

public interface IRingPoller<out T> : IRingPoller where T : class
{
    IPollingRing<T> Ring { get; }
}
