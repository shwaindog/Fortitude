#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.MessageBus.Tasks;

public interface ITaskPayload : IReusableObject<ITaskPayload>
{
    void Invoke();
}

public class TaskPayload : ReusableObject<ITaskPayload>, ITaskPayload
{
    public TaskPayload() { }

    private TaskPayload(TaskPayload toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public SendOrPostCallback Callback { get; set; } = null!;
    public object? State { get; set; }

    public void Invoke()
    {
        Callback(State);
    }

    public override void StateReset()
    {
        Callback = null!;
        State = null!;
        base.StateReset();
    }

    public override ITaskPayload CopyFrom(ITaskPayload source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is TaskPayload taskPayloadSource)
        {
            Callback = taskPayloadSource.Callback;
            State = taskPayloadSource.State;
        }

        return this;
    }

    public override ITaskPayload Clone() => Recycler?.Borrow<TaskPayload>().CopyFrom(this) ?? new TaskPayload(this);
}
