#region

using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.MessageBus.Tasks;

public class MessagePumpSyncContext(EventContext pumpContext) : SynchronizationContext
{
    private readonly MessagePumpSyncContextRule sendingRule = new(pumpContext);

    public override SynchronizationContext CreateCopy() => new MessagePumpSyncContext(pumpContext);

    public override void Post(SendOrPostCallback d, object? state)
    {
        var taskPayload = pumpContext.PooledRecycler.Borrow<TaskPayload>();
        taskPayload.Callback = d;
        taskPayload.State = state;

        pumpContext.RegisteredOn.EnqueuePayload(taskPayload, sendingRule, null, MessageType.TaskAction);
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        if (Current == this)
            d.Invoke(state);
        else
            Post(d, state);
    }

    private class MessagePumpSyncContextRule(EventContext pumpContext) :
        Rule($"MessagePumpSyncContextRule_{pumpContext.RegisteredOn.Name}"
            , $"MessagePumpSyncContextRule_{pumpContext.RegisteredOn.Name}");
}
