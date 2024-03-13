#region

using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;

#endregion

namespace FortitudeCommon.Monitoring.Logging;

internal class FLogEventPoller : IPollSink<FLogEvent>, IRingPoller
{
    private readonly RingPollerSink<FLogEvent> ringPoller;

    public FLogEventPoller(PollingRing<FLogEvent> ring, uint timeoutMs) =>
        ringPoller = new RingPollerSink<FLogEvent>(ring, timeoutMs, this);

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

    public void Dispose()
    {
        ringPoller.Dispose();
    }

    public bool IsRunning => ringPoller.IsRunning;

    public void WakeIfAsleep()
    {
        ringPoller.WakeIfAsleep();
    }

    public void StartPolling(Action? threadStartInitialize = null)
    {
        ringPoller.StartPolling(threadStartInitialize);
    }
}
