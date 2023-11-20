#region

using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

public class MessagePumpTaskScheduler : TaskScheduler
{
    private readonly Action<Task> threadPoolQueueTask =
        NonPublicInvocator.GetInstanceMethodAction<Task>(Default, "QueueTask");

    private readonly Func<Task, bool, bool> threadPoolTryExecuteTaskInline =
        NonPublicInvocator.GetInstanceMethodFunc<Task, bool, bool>(Default, "TryExecuteTaskInline");

    private SynchronizationContext messagePumpSyncContext = SynchronizationContext.Current!;

    public override int MaximumConcurrencyLevel => 1;

    private void TryExecuteCallback(object? state)
    {
        TryExecuteTask((Task)state!);
    }

    protected override void QueueTask(Task task)
    {
        if (SynchronizationContext.Current == messagePumpSyncContext)
            messagePumpSyncContext.Post(TryExecuteCallback, task);
        else if (SynchronizationContext.Current != null)
            SynchronizationContext.Current.Post(TryExecuteCallback, task);
        else
            threadPoolQueueTask(task);
    }


    public void EnqueueTask(Task task)
    {
        QueueTask(task);
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) =>
        SynchronizationContext.Current == messagePumpSyncContext ?
            TryExecuteTask(task) :
            threadPoolTryExecuteTaskInline(task, taskWasPreviouslyQueued);

    protected override IEnumerable<Task>? GetScheduledTasks() => null;
}
