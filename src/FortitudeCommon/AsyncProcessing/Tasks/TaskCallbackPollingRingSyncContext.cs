#region

using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

#endregion

namespace FortitudeCommon.AsyncProcessing.Tasks;

public class TaskCallbackPollingRingSyncContext(ITaskCallbackPollingRing asyncEnabledPollingRing) : SynchronizationContext
{
    public override SynchronizationContext CreateCopy() => new TaskCallbackPollingRingSyncContext(asyncEnabledPollingRing);

    public override void Post(SendOrPostCallback d, object? state)
    {
        asyncEnabledPollingRing.EnqueueCallback(d, state);
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        if (Current == this)
            d.Invoke(state);
        else
            Post(d, state);
    }
}
