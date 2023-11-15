#region

using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.EventBus.Tasks;

public class MessagePumpTaskScheduler : TaskScheduler
{
    private readonly Action<Task> threadPoolQueueTask =
        NonPublicInvocator.GetInstanceMethodAction<Task>(Default, "QueueTask");

    private readonly Func<Task, bool, bool> threadPoolTryExecuteTaskInline =
        NonPublicInvocator.GetInstanceMethodFunc<Task, bool, bool>(Default, "TryExecuteTaskInline");

    private SynchronizationContext messagePumptSyncContext = SynchronizationContext.Current;

    /// <summary>
    ///     Implements the <see cref="TaskScheduler.MaximumConcurrencyLevel" /> property for
    ///     this scheduler class.
    ///     By default it returns 1, because a <see cref="SynchronizationContext" /> based
    ///     scheduler only supports execution on a single thread.
    /// </summary>
    public override int MaximumConcurrencyLevel => 1;


    private void TryExecuteCallback(object? state)
    {
        TryExecuteTask((Task)state);
    }

    /// <summary>
    ///     Implementation of <see cref="TaskScheduler.QueueTask" /> for this scheduler class.
    ///     Simply posts the tasks to be executed on the associated <see cref="SynchronizationContext" />.
    /// </summary>
    /// <param name="task"></param>
    protected override void QueueTask(Task task)
    {
        if (SynchronizationContext.Current == messagePumptSyncContext)
            messagePumptSyncContext.Post(TryExecuteCallback, task);
        else if (SynchronizationContext.Current != null)
            SynchronizationContext.Current.Post(TryExecuteCallback, task);
        else
            threadPoolQueueTask(task);
    }


    public void EnqueueTask(Task task)
    {
        QueueTask(task);
    }

    /// <summary>
    ///     Implementation of <see cref="TaskScheduler.TryExecuteTaskInline" />  for this scheduler
    ///     class.
    ///     The task will be executed inline only if the call happens within
    ///     the associated <see cref="SynchronizationContext" />.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="taskWasPreviouslyQueued"></param>
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) =>
        SynchronizationContext.Current == messagePumptSyncContext ?
            TryExecuteTask(task) :
            threadPoolTryExecuteTaskInline(task, taskWasPreviouslyQueued);

    protected override IEnumerable<Task>? GetScheduledTasks() => null;
}
