#region

using FortitudeCommon.AsyncProcessing.Tasks;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IPollingRing<out T> where T : class
{
    string Name { get; }
    T this[long sequence] { get; }
    long Claim();
    void Publish(long sequence);
}

public interface ITaskCallbackPollingRing
{
    void EnqueueCallback(SendOrPostCallback d, object? state);
}

public interface ITaskCallbackPollingRing<out T> : ITaskCallbackPollingRing, IPollingRing<T> where T : class, ICanCarryTaskCallbackPayload { }
