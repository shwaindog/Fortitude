#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.MessageBus.Tasks;

internal class TaskExecuteAction : ReusableObject<TaskExecuteAction>
{
    public TaskExecuteAction() { }

    private TaskExecuteAction(TaskExecuteAction toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public SendOrPostCallback TaskAction { get; set; } = null!;
    public object? State { get; set; }

    public override void StateReset()
    {
        TaskAction = null!;
        State = null;
    }

    public override TaskExecuteAction CopyFrom(TaskExecuteAction source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TaskAction = source.TaskAction;
        State = source.State;
        return this;
    }

    public void Invoke()
    {
        TaskAction(State);
    }

    public override TaskExecuteAction Clone() =>
        Recycler?.Borrow<TaskExecuteAction>().CopyFrom(this) ?? new TaskExecuteAction(this);
}
