#region

using FortitudeCommon.DataStructures.Maps;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

public class MessagePumpSyncContext : SynchronizationContext
{
    private IMap<Task, SendOrPostCallback> enqueueTasks;
    private IMap<Type, SendOrPostCallback> enqueueValueTasks;


    public MessagePumpSyncContext() => enqueueTasks = new ConcurrentMap<Task, SendOrPostCallback>();


    public MessagePumpSyncContext(IMap<Task, SendOrPostCallback> enqueueTasks) => this.enqueueTasks = enqueueTasks;

    public void RunQueuedTasks()
    {
        // TODO make this a member and clear and insert
        var enquedTasks = enqueueTasks.ToList();
        foreach (var keyValuePair in enquedTasks) keyValuePair.Value(keyValuePair.Key);
        foreach (var keyValuePair in enquedTasks)
        {
            var task = keyValuePair.Key;
            if (task.IsCompleted || task.IsCanceled || task.IsFaulted || task.IsCompletedSuccessfully)
                enqueueTasks.Remove(keyValuePair.Key);
        }
    }

    public override SynchronizationContext CreateCopy() => new MessagePumpSyncContext(enqueueTasks);

    public override void OperationCompleted()
    {
        base.OperationCompleted();
    }

    public override void OperationStarted()
    {
        base.OperationStarted();
    }

    public override void Post(SendOrPostCallback d, object? state)
    {
        var task = (Task)state!;
        enqueueTasks.Add(task, d);
    }

    public void Post<T>(ValueTask<T> toRun) { }

    public override void Send(SendOrPostCallback d, object? state)
    {
        var task = (Task)state;
        if (Current == this)
            d.Invoke(task);
        else
            Post(d, state);
        task.Wait(600_000);
    }
}
