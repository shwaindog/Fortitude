#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IPollingRing
{
    string Name { get; }
}

public interface IPollingRing<out T> : IPollingRing where T : class
{
    T this[int sequence] { get; }
    int Claim();
    void Publish(int sequence);
}

public interface IPollingRingLong<out T> : IPollingRing  where T : class
{
    T this[long sequence] { get; }
    long Claim();
    void Publish(long sequence);
}

public interface ITaskCallbackPollingRing : IPollingRing
{
    void EnqueueCallback(SendOrPostCallback d, object? state);
}

public interface ITaskCallbackPollingRing<out T> : ITaskCallbackPollingRing, IPollingRing<T> where T : class, ICanCarryTaskCallbackPayload { }

public interface ITaskCallbackPollingRingLong<out T> : ITaskCallbackPollingRing, IPollingRingLong<T> where T : class, ICanCarryTaskCallbackPayload { }


public interface IEnqueuePollingRing<T> : IPollingRing<T> where T : class, ITransferState<T>
{
    int Enqueue(T toQueue);
}

public interface IEnqueuePollingRingLong<T> : IPollingRingLong<T> where T : class, ITransferState<T>
{
    long Enqueue(T toQueue);
}

public interface IEnqueueTaskCallbackPollingRing<T> : ITaskCallbackPollingRing, IEnqueuePollingRing<T> 
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload { }

public interface IEnqueueTaskCallbackPollingRingLong<T> : ITaskCallbackPollingRing, IEnqueuePollingRingLong<T> 
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload { }