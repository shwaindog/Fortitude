#region

using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;

#endregion

namespace FortitudeCommon.Monitoring.Logging;

internal class FLogEventPoller : RingPoller<FLogEvent>
{
    public FLogEventPoller(PollingRing<FLogEvent> ring, uint timeoutMs)
        : base(ring, timeoutMs) { }

    protected override void Processor(long sequence, long batchSize, FLogEvent data, bool startOfBatch,
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
}
