#region

using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.Monitoring.Logging;

internal class FLogEventPoller : IEnumerableBatchPollSink<FLogEvent>, IRingPoller
{
    private readonly EnumerableBatchRingPollerSink<FLogEvent> ringPoller;

    public FLogEventPoller(EnumerableBatchPollingRing<FLogEvent> ring, uint timeoutMs, IOSParallelController? osParallelController = null) =>
        ringPoller = new EnumerableBatchRingPollerSink<FLogEvent>(ring, timeoutMs, this, null, osParallelController ?? new OSParallelController());

    public void Processor(long sequence, long batchSize, FLogEvent data, bool startOfBatch,
        bool endOfBatch)
    {
        try
        {
            data.Logger.OnLogEvent(data);
        }
        catch { }
        finally
        {
            data.Exception = null;
        }
    }

    public int UsageCount => ringPoller.UsageCount;

    public IOSThread? ExecutingThread => ringPoller.ExecutingThread;

    event Action<QueueEventTime>? IRingPoller.QueueEntryStart
    {
        add => ringPoller.QueueEntryStart += value;
        remove => ringPoller.QueueEntryStart -= value;
    }

    event Action<QueueEventTime>? IRingPoller.QueueEntryComplete
    {
        add => ringPoller.QueueEntryComplete += value;
        remove => ringPoller.QueueEntryComplete -= value;
    }

    public void Dispose()
    {
        ringPoller.ForceStop();
    }

    public void Stop()
    {
        ringPoller.Stop();
    }

    public bool IsRunning => ringPoller.IsRunning;

    public void WakeIfAsleep()
    {
        ringPoller.WakeIfAsleep();
    }

    public void Start(Action? threadStartInitialize = null)
    {
        ringPoller.Start(threadStartInitialize);
    }
}
