#region

using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Batching;

public abstract class RingPoller<T> : IDisposable where T : class
{
    private readonly AutoResetEvent are = new(true);
    private readonly ISubject<T> eventSubject = new Subject<T>();
    protected readonly IPollingRing<T> Ring;
    private readonly IOSThread thread;
    private readonly int timeoutMs;
    private volatile bool isRunning;

    protected RingPoller(IPollingRing<T> ring, uint timeoutMs, IOSParallelController? parallelController = null)
    {
        Ring = ring;
        var osParallelController = parallelController ?? new OSParallelController();
        this.timeoutMs = (int)timeoutMs;
        thread = osParallelController.CreateNewOSThread(delegate()
        {
            InitializeInPollingThread();
            while (isRunning)
            {
                are.WaitOne(this.timeoutMs);
                foreach (var data in Ring)
                {
                    eventSubject.OnNext(data);
                    Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
                }

                CurrentPollingIterationComplete();
            }

            ShuttingDownPollingThread();
        });
        thread.IsBackground = true;
        thread.Name = Ring.Name + "-Poller";
    }

    private IObservable<T> Events => eventSubject.AsObservable();

    public bool IsRunning => isRunning;

    public void Dispose()
    {
        isRunning = false;
        are.Set();
        thread.Join();
        foreach (var data in Ring)
        {
            eventSubject.OnNext(data);
            Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
        }
    }

    public void WakeIfAsleep()
    {
        are.Set();
    }

    public void StartPolling()
    {
        isRunning = true;
        thread.Start();
    }

    protected virtual void InitializeInPollingThread() { }

    protected virtual void ShuttingDownPollingThread() { }
    protected virtual void CurrentPollingIterationComplete() { }

    protected virtual void Processor(long sequence, long batchSize, T data, bool startOfBatch, bool endOfBatch) { }
}
