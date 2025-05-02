#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeBusRules.BusMessaging.Tasks;

public class TaskPayload : ReusableObject<IInvokeablePayload>, IInvokeablePayload
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TaskPayload));
    public TaskPayload() { }

    private TaskPayload(TaskPayload toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public SendOrPostCallback Callback { get; set; } = null!;
    public object? State { get; set; }
    public bool IsAsyncInvoke => false;

    public ValueTask InvokeAsync()
    {
        Invoke();
        return ValueTask.CompletedTask;
    }

    public void Invoke()
    {
        try
        {
            Callback(State);
        }
        catch (Exception ex)
        {
            Logger.Warn("Async Task, Value Task or Thread Pool callback threw exception {0}", ex);
        }
    }

    public override void StateReset()
    {
        Callback = null!;
        State = null!;
        base.StateReset();
    }

    public override IInvokeablePayload CopyFrom(IInvokeablePayload source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is TaskPayload taskPayloadSource)
        {
            Callback = taskPayloadSource.Callback;
            State = taskPayloadSource.State;
        }

        return this;
    }

    public override IInvokeablePayload Clone() => Recycler?.Borrow<TaskPayload>().CopyFrom(this) ?? new TaskPayload(this);
}
