#region

using System.Reactive.Linq;
using System.Reactive.Subjects;
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
    void WakeIfAsleep();
    void StartPolling(Action? threadStartInitialize = null);
}

public interface IRingPollerSink<T> : IRingPoller where T : class
{
    IPollSink<T>? PollSink { get; set; }
}

public abstract class RingPollerBase<T>(IPollingRing<T> ring, uint timeoutMs,
        IOSParallelController? parallelController = null)
    : IRingPoller where T : class
{
    private readonly AutoResetEvent are = new(true);
    private readonly IOSParallelController osParallelController = parallelController ?? new OSParallelController();
    protected readonly IPollingRing<T> Ring = ring;
    private readonly int timeoutMs = (int)timeoutMs;
    private volatile bool isRunning;
    private IOSThread? thread;

    public bool IsRunning => isRunning;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        isRunning = false;
        are.Set();
        thread?.Join();
        foreach (var data in Ring)
            Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
    }

    public void WakeIfAsleep()
    {
        are.Set();
    }

    public void StartPolling(Action? threadStartInitialize = null)
    {
        isRunning = true;
        thread = osParallelController.CreateNewOSThread(delegate()
        {
            threadStartInitialize?.Invoke();
            while (isRunning) Poll();
        });
        thread.IsBackground = true;
        thread.Name = Ring.Name + "-Poller";
        thread.Start();
    }

    protected virtual void Poll()
    {
        are.WaitOne(timeoutMs);
        foreach (var data in Ring)
            Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
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

public class RingPollerObservable<T>(IPollingRing<T> ring, uint timeoutMs
        , IOSParallelController? parallelController = null)
    : RingPollerBase<T>(ring, timeoutMs, parallelController)
    where T : class
{
    private readonly Subject<T> eventSubject = new();
    private IObservable<T> Events => eventSubject.AsObservable();

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, T data, bool ringStartOfBatch
        , bool ringEndOfBatch)
    {
        eventSubject.OnNext(data);
    }
}
