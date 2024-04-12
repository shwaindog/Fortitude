#region

using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

#endregion

namespace FortitudeCommon.AsyncProcessing.Tasks;

public class TaskCallbackPollingRingSyncContext(ITaskCallbackPollingRing asyncEnabledPollingRing) : SynchronizationContext
{
    public override SynchronizationContext CreateCopy() => this;

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
