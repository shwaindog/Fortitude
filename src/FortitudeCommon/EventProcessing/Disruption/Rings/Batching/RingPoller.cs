#region

using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Batching;

public interface IPollSink<in T> where T : class
{
    void Processor(long sequence, long batchSize, T data, bool startOfBatch, bool endOfBatch);
}

public interface IRingPoller : IDisposable
{
    bool IsRunning { get; }
    int UsageCount { get; }
    void WakeIfAsleep();
    void Start(Action? threadStartInitialize = null);
    void Stop();
}

public interface IRingPollerSink<T> : IRingPoller where T : class
{
    IPollSink<T>? PollSink { get; set; }
}

public abstract class RingPollerBase<T>
    : IRingPoller where T : class
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(RingPollerBase<T>));

    private readonly AutoResetEvent are = new(true);
    private readonly object initLock = new();
    private readonly IOSParallelController osParallelController;
    private readonly int timeoutMs;
    private volatile bool isRunning;
    private string name;
    protected IPollingRing<T> Ring;
    private IOSThread? ringPollingThread;
    private Action? threadStartInitialization;

    // ReSharper disable once ConvertToPrimaryConstructor
    protected RingPollerBase(IPollingRing<T> ring, uint noDataPauseTimeoutMs,
        IOSParallelController? parallelController = null)
    {
        Ring = ring;
        timeoutMs = (int)noDataPauseTimeoutMs;
        osParallelController = parallelController ?? new OSParallelController();
        name = Ring.Name + "-Poller";
    }

    public virtual string Name
    {
        get => name;
        set => name = value;
    }

    public int UsageCount { get; private set; }

    public bool IsRunning => isRunning;

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
        are.Set();
    }

    public void Start(Action? threadStartInitialize = null)
    {
        lock (initLock)
        {
            ++UsageCount;
            if (!isRunning)
            {
                threadStartInitialization = threadStartInitialize;
                isRunning = true;
                ringPollingThread = osParallelController.CreateNewOSThread(ThreadStart);
                ringPollingThread.IsBackground = true;
                ringPollingThread.Name = Name;
                ringPollingThread.Start();
            }
        }
    }

    protected void ForceStop()
    {
        try
        {
            isRunning = false;
            are.Set();
            ringPollingThread?.Join();
            foreach (var data in Ring)
                try
                {
                    Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch
                        , Ring.EndOfBatch);
                }
                catch (Exception ex)
                {
                    Logger.Warn($"RingPoller '{Ring.Name}' caught exception while " +
                                $"stopping and processing event: {data}.  {ex}");
                }
        }
        catch (Exception ex)
        {
            Logger.Warn($"RingPoller '{Ring.Name}' caught exception when trying to stop.  {ex}");
        }
    }

    protected void ThreadStart()
    {
        threadStartInitialization?.Invoke();
        while (isRunning) PollAttempt();
    }

    protected virtual void PollAttempt()
    {
        are.WaitOne(timeoutMs);
        foreach (var data in Ring)
            try
            {
                Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
            }
            catch (Exception ex)
            {
                Logger.Warn($"RingPoller '{Ring.Name}' caught exception while processing event: {data}.  {ex}");
            }
    }

    protected abstract void Processor(long ringCurrentSequence, long ringCurrentBatchSize, T data, bool ringStartOfBatch
        , bool ringEndOfBatch);
}

public class RingPollerSink<T>(IPollingRing<T> ring, uint timeoutMs, IPollSink<T>? pollSink = null
        , IOSParallelController? parallelController = null)
    : RingPollerBase<T>(ring, timeoutMs, parallelController), IRingPollerSink<T>
    where T : class
{
    public IPollSink<T>? PollSink { get; set; } = pollSink;

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, T data, bool ringStartOfBatch
        , bool ringEndOfBatch)
    {
        PollSink!.Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
    }
}

public class RingPollerObservable<T>(IPollingRing<T> ring, uint timeoutMs,
        IOSParallelController? parallelController = null)
    : RingPollerBase<T>(ring, timeoutMs, parallelController) where T : class
{
    private readonly Subject<T> eventSubject = new();
    private IObservable<T> Events => eventSubject.AsObservable();

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, T data, bool ringStartOfBatch
        , bool ringEndOfBatch)
    {
        eventSubject.OnNext(data);
    }
}
