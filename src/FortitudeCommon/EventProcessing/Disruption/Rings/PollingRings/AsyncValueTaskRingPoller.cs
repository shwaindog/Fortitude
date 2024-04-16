#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IAsyncValueTaskRingPoller<T> : IRingPoller<T> where T : class, ICanCarryTaskCallbackPayload
{
    SyncContextTaskScheduler RingPollerTaskScheduler { get; }
    new IAsyncValueTaskPollingRing<T> Ring { get; }
    ValueTaskProcessEvent<T> ProcessEvent { get; set; }
    IRecycler Recycler { get; set; }
}

public class AsyncValueTaskRingPoller<T> : IAsyncValueTaskRingPoller<T> where T : class, ICanCarryTaskCallbackPayload
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(AsyncValueTaskRingPoller<T>));
    private readonly object initLock = new();
    private readonly IOSParallelController osParallelController;

    private readonly IIntraOSThreadSignal spinPauseSignal;
    private readonly int timeoutMs;
    private volatile bool isRunning;
    private IRecycler? recycler;
    private Action? threadStartInitialization;

    // ReSharper disable once ConvertToPrimaryConstructor
    public AsyncValueTaskRingPoller(IAsyncValueTaskPollingRing<T> ring, uint noDataPauseTimeoutMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
    {
        Ring = ring;
        Ring.ProcessEvent = ExceptionSafeProcessEvent;
        Ring.InterceptHandler = RingPollerHandledMessage;
        timeoutMs = (int)noDataPauseTimeoutMs;
        osParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
        spinPauseSignal = osParallelController.SingleOSThreadActivateSignal(false);
        Name = Ring.Name + "-AsyncValueTaskPoller";
        this.threadStartInitialization = threadStartInitialization;
    }

    public string Name { get; set; }

    public virtual int UsageCount { get; private set; }

    public bool IsRunning => isRunning;

    public IOSThread? ExecutingThread { get; private set; }

    public ValueTaskProcessEvent<T> ProcessEvent
    {
        get => Ring.ProcessEvent;
        set => Ring.ProcessEvent = value;
    }

    public IAsyncValueTaskPollingRing<T> Ring { get; }

    IPollingRing<T> IRingPoller<T>.Ring => Ring;

    public SyncContextTaskScheduler RingPollerTaskScheduler { get; } = new();

    public IRecycler Recycler
    {
        get => recycler ?? new Recycler();
        set => recycler = value;
    }

    public event Action<QueueEventTime>? QueueEntryStart
    {
        add => Ring.QueueEntryStart += value;
        remove => Ring.QueueEntryStart -= value;
    }

    public event Action<QueueEventTime>? QueueEntryComplete
    {
        add => Ring.QueueEntryComplete += value;
        remove => Ring.QueueEntryComplete -= value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        ForceStop();
    }

    public virtual void Stop()
    {
        lock (initLock)
        {
            --UsageCount;
            if (UsageCount != 0 || !isRunning) return;
            ForceStop();
        }
    }

    public void WakeIfAsleep()
    {
        spinPauseSignal.Set();
    }

    public void Start(Action? threadStartInitialize = null)
    {
        lock (initLock)
        {
            ++UsageCount;
            if (!isRunning)
            {
                threadStartInitialization = threadStartInitialize ?? threadStartInitialization;
                isRunning = true;
                ExecutingThread = osParallelController.CreateNewOSThread(ThreadStart);
                ExecutingThread.IsBackground = true;
                ExecutingThread.Name = Name;
                ExecutingThread.Start();
            }
        }
    }

    protected void ForceStop()
    {
        try
        {
            isRunning = false;
            spinPauseSignal.Set();
            ExecutingThread?.Join();
            Poll();
        }
        catch (Exception ex)
        {
            Logger.Warn($"RingPoller '{Ring.Name}' caught exception when trying to stop.  {ex}");
        }
    }

    protected void ThreadStart()
    {
        threadStartInitialization?.Invoke();
        var syncContext = new TaskCallbackPollingRingSyncContext(Ring);
        SynchronizationContext.SetSynchronizationContext(syncContext);
        while (isRunning) Poll();
    }

    private void Poll()
    {
        spinPauseSignal.WaitOne(timeoutMs);
        try
        {
            var keepPolling = true;
            BeforeProcessingEvents();
            while (keepPolling)
            {
                var seqValueTask = Ring.Poll();
                keepPolling = !seqValueTask.IsCompleted || seqValueTask.Result > 0;
            }
        }
        catch (Exception ex)
        {
            Logger.Warn($"RingPoller '{Ring.Name}' caught exception while processing event.  {ex}");
        }
    }

    protected virtual void BeforeProcessingEvents() { }

    protected virtual bool RingPollerHandledMessage(T data) => false;

    protected virtual async ValueTask<long> ExceptionSafeProcessEvent(long sequenceId, T data)
    {
        try
        {
            await ProcessEventMessage(sequenceId, data);
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught Exception processing data {0}. Got {1}", data, ex);
        }

        return sequenceId;
    }

    protected virtual ValueTask<long> ProcessEventMessage(long sequenceId, T data)
    {
        Logger.Warn("You should override this method to process the event");
        return new ValueTask<long>(sequenceId);
    }
}
