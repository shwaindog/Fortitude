#region

using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IEnumerableBatchRingPoller<T> : IRingPoller<T> where T : class
{
    IRecycler Recycler { get; set; }
    new IEnumerableBatchPollingRing<T> Ring { get; }
}

public abstract class EnumerableBatchRingPoller<T> : IEnumerableBatchRingPoller<T> where T : class
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(EnumerableBatchRingPoller<T>));

    private readonly AutoResetEvent are = new(true);
    private readonly object initLock = new();
    private readonly IOSParallelController osParallelController;
    private readonly int timeoutMs;
    private volatile bool isRunning;
    private IOSThread? ringPollingThread;
    private Action? threadStartInitialization;

    // ReSharper disable once ConvertToPrimaryConstructor
    protected EnumerableBatchRingPoller(IEnumerableBatchPollingRing<T> ring, uint noDataPauseTimeoutMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
    {
        Ring = ring;
        timeoutMs = (int)noDataPauseTimeoutMs;
        osParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
        Name = Ring.Name + "-EnumerableBatchRingPoller";
        this.threadStartInitialization = threadStartInitialization;
    }

    public string Name { get; set; }

    public IEnumerableBatchPollingRing<T> Ring { get; }

    IPollingRing<T> IRingPoller<T>.Ring => Ring;

    public virtual int UsageCount { get; private set; }

    public bool IsRunning => isRunning;
    public IRecycler Recycler { get; set; } = new Recycler();

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
                threadStartInitialization = threadStartInitialize ?? threadStartInitialization;
                isRunning = true;
                ringPollingThread = osParallelController.CreateNewOSThread(ThreadStart);
                ringPollingThread.IsBackground = true;
                ringPollingThread.Name = Name;
                ringPollingThread.Start();
            }
        }
    }

    public void ForceStop()
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

public interface IEnumerableBatchPollSink<in T> where T : class
{
    void Processor(long sequence, long batchSize, T data, bool startOfBatch, bool endOfBatch);
}

public interface IEnumerableBatchRingPollerSink<T> : IEnumerableBatchRingPoller<T> where T : class
{
    IEnumerableBatchPollSink<T>? PollSink { get; set; }
    IRecycler Recycler { get; set; }
}

public class EnumerableBatchRingPollerSink<T>(IEnumerableBatchPollingRing<T> ring, uint timeoutMs, IEnumerableBatchPollSink<T>? pollSink = null
        , Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
    : EnumerableBatchRingPoller<T>(ring, timeoutMs, threadStartInitialization, parallelController), IEnumerableBatchRingPollerSink<T>
    where T : class
{
    private IRecycler? recycler;
    public IEnumerableBatchPollSink<T>? PollSink { get; set; } = pollSink;

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, T data, bool ringStartOfBatch
        , bool ringEndOfBatch)
    {
        PollSink!.Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
    }
}

public class EnumerableBatchRingPollerObservable<T>(IEnumerableBatchPollingRing<T> ring, uint timeoutMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
    : EnumerableBatchRingPoller<T>(ring, timeoutMs, threadStartInitialization, parallelController) where T : class
{
    private readonly Subject<T> eventSubject = new();
    private IObservable<T> Events => eventSubject.AsObservable();

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, T data, bool ringStartOfBatch
        , bool ringEndOfBatch)
    {
        eventSubject.OnNext(data);
    }
}
