// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IAsyncValueTaskRingPoller<T> : IRingPoller<T> where T : class, ICanCarryTaskCallbackPayload
{
    SyncContextTaskScheduler RingPollerTaskScheduler { get; }

    new IAsyncValueTaskPollingRing<T> Ring { get; }

    IRecycler Recycler { get; set; }
}

public interface IAsyncValueTaskRingPollerLong<T> : IRingPollerLong<T> where T : class, ICanCarryTaskCallbackPayload
{
    SyncContextTaskScheduler RingPollerTaskScheduler { get; }

    new IAsyncValueTaskPollingRingLong<T> Ring { get; }

    IRecycler Recycler { get; set; }
}

public class AsyncValueTaskRingPollerBase<T, TRingPoller>
    where T : class, ICanCarryTaskCallbackPayload
    where TRingPoller : class, IAsyncValueTaskRing<T>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(AsyncValueTaskRingPoller<T>));

    private readonly object initLock = new();

    private readonly IOSParallelController osParallelController;
    private readonly IIntraOSThreadSignal  spinPauseSignal;

    private readonly int timeoutMs;

    private bool gracefulShutdown = true;

    private volatile bool isRunning;

    private   IRecycler? recycler;
    protected Action?    ThreadStartInitialization;

    // ReSharper disable once ConvertToPrimaryConstructor
    public AsyncValueTaskRingPollerBase
    (TRingPoller ring, uint emptyQueueMaxSleepMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
    {
        Ring      = ring;
        Name      = Ring.Name + "-AsyncValueTaskPoller";
        timeoutMs = (int)emptyQueueMaxSleepMs;

        Ring.InterceptHandler = RingPollerHandledMessage;
        osParallelController  = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
        spinPauseSignal       = osParallelController.SingleOSThreadActivateSignal(false);

        ThreadStartInitialization = threadStartInitialization;
    }

    public string Name { get; set; }

    public virtual int UsageCount { get; private set; }

    public bool IsRunning => isRunning;

    public IOSThread? ExecutingThread { get; private set; }

    public virtual TRingPoller Ring { get; }

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
            if (UsageCount != 0 || !isRunning)
                // Logger.Debug("Ring {0} has {1} registered usages", Ring.Name, UsageCount);
                return;

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
                ThreadStartInitialization = threadStartInitialize ?? ThreadStartInitialization;

                isRunning       = true;
                ExecutingThread = osParallelController.CreateNewOSThread(ThreadStart);

                ExecutingThread.IsBackground = true;

                ExecutingThread.Name = Name;
                ExecutingThread.Start();
            }
        }
    }

    public void StopImmediate()
    {
        gracefulShutdown = false;
        ForceStop();
    }

    protected void ForceStop()
    {
        try
        {
            UsageCount = 0;
            isRunning  = false;
            if (gracefulShutdown)
            {
                if (ExecutingThread?.IsAlive == true)
                {
                    spinPauseSignal.Set();
                    ExecutingThread?.Join();
                }

                Poll(Ring.Size);
            }
        }
        catch (Exception ex)
        {
            Logger.Warn($"RingPoller '{Ring.Name}' caught exception when trying to stop.  {ex}");
        }

        // Logger.Debug("Ring {0} has stopped", Ring.Name);
    }

    protected void ThreadStart()
    {
        ThreadStartInitialization?.Invoke();
        var syncContext = new TaskCallbackPollingRingSyncContext(Ring);
        SynchronizationContext.SetSynchronizationContext(syncContext);
        while (isRunning)
        {
            if (timeoutMs > 0) spinPauseSignal.WaitOne(timeoutMs);
            Poll(Ring.Size);
        }
    }

    private void Poll(int maxMessagesPerPoll)
    {
        try
        {
            var keepPolling   = true;
            var countMessages = 0;
            BeforeProcessingEvents();
            while (keepPolling)
            {
                var processedAMessage = Ring.Poll();
                keepPolling = processedAMessage && countMessages++ < maxMessagesPerPoll;
            }
        }
        catch (Exception ex)
        {
            Logger.Warn($"RingPoller '{Ring.Name}' caught exception while processing event.  {ex}");
        }
    }

    protected virtual void BeforeProcessingEvents() { }

    protected virtual bool RingPollerHandledMessage(T data) => false;

    protected virtual async ValueTask ExceptionSafeProcessEvent(T data)
    {
        try
        {
            await ProcessEventMessage(data);
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught Exception processing data {0}. Got {1}", data, ex);
        }
    }

    protected virtual ValueTask ProcessEventMessage(T data)
    {
        Logger.Warn("You should override this method to process the event");
        return ValueTask.CompletedTask;
    }
}


public class AsyncValueTaskRingPoller<T> : AsyncValueTaskRingPollerBase<T, IAsyncValueTaskPollingRing<T>>, IAsyncValueTaskRingPoller<T>  where T : class, ICanCarryTaskCallbackPayload
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public AsyncValueTaskRingPoller
    (IAsyncValueTaskPollingRing<T> ring, uint emptyQueueMaxSleepMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : base(ring, emptyQueueMaxSleepMs, threadStartInitialization, parallelController)
    {
    }

    IPollingRing<T> IRingPoller<T>.Ring => Ring;
}


public class AsyncValueTaskRingPollerLong<T> : AsyncValueTaskRingPollerBase<T, IAsyncValueTaskPollingRingLong<T>>, IAsyncValueTaskRingPollerLong<T>  where T : class, ICanCarryTaskCallbackPayload
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public AsyncValueTaskRingPollerLong
    (IAsyncValueTaskPollingRingLong<T> ring, uint emptyQueueMaxSleepMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : base(ring, emptyQueueMaxSleepMs, threadStartInitialization, parallelController)
    {
    }

    IPollingRingLong<T> IRingPollerLong<T>.Ring => Ring;
}
