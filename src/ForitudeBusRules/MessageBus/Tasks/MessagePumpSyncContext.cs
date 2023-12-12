#region

using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.MessageBus.Tasks;

public class MessagePumpSyncContext : SynchronizationContext
{
    private readonly EventContext pumpContext;
    private readonly MessagePumpSyncContextRule sendingRule;

    public MessagePumpSyncContext(EventContext pumpContext)
    {
        this.pumpContext = pumpContext;
        sendingRule = new MessagePumpSyncContextRule(pumpContext);
    }

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

    private class MessagePumpSyncContextRule : Rule
    {
        public MessagePumpSyncContextRule(EventContext pumpContext) : base(
            $"MessagePumpSyncContextRule_{pumpContext.RegisteredOn.Name}"
            , $"MessagePumpSyncContextRule_{pumpContext.RegisteredOn.Name}") { }
    }
}
