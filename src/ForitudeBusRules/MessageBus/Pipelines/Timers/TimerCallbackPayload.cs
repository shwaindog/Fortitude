#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Pipelines.Timers;

public interface ITimerCallbackPayload : IRecyclableObject
{
    void Invoke();
}

public class TimerCallbackPayload<T> : ReusableObject<TimerCallbackPayload<T>>, ITimerCallbackPayload where T : class
{
    public TimerCallbackPayload() { }

    private TimerCallbackPayload(TimerCallbackPayload<T> toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Action? Action { get; set; }
    public Action<T?>? ActionState { get; set; }
    public T? State { get; set; }

    public void Invoke()
    {
        if (Action != null && ActionState != null)
            throw new InvalidDataException("Only expect Action or ActionState to be set");
        if (Action != null)
            Action();
        else if (ActionState != null)
            ActionState(State);
        else
            throw new InvalidDataException("Expected either Action or ActionState to be set");
    }

    public override void Reset()
    {
        Action = null;
        ActionState = null;
        State = null;
        base.Reset();
    }

    public override TimerCallbackPayload<T> CopyFrom(TimerCallbackPayload<T> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Action = source.Action;
        ActionState = source.ActionState;
        State = source.State;
        return this;
    }

    public override TimerCallbackPayload<T> Clone() =>
        Recycler?.Borrow<TimerCallbackPayload<T>>().CopyFrom(this) ?? new TimerCallbackPayload<T>(this);
}
