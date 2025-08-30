// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.OSWrapper.AsyncWrappers;

namespace FortitudeCommon.Logging.AsyncProcessing.Threaded;

internal class FLogEventPoller : IEnumerableBatchPollSink<FLogAsyncPayload>, IRingPoller
{
    private readonly EnumerableBatchRingPollerSink<FLogAsyncPayload> ringPoller;

    public FLogEventPoller(EnumerableBatchPollingRing<FLogAsyncPayload> ring, uint timeoutMs, IOSParallelController? osParallelController = null) =>
        ringPoller = new EnumerableBatchRingPollerSink<FLogAsyncPayload>(ring, timeoutMs, this, null
                                                                       , osParallelController ?? new OSParallelController());

    public void Processor(long sequence, long batchSize, FLogAsyncPayload data, bool startOfBatch,
        bool endOfBatch)
    {
        try
        {
            data.ReceiverExecuteRequest();
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine($"FlogEventPoller caught {ex}");
        }
        finally { }
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

    public void WaitUntilDrained()
    {
        ringPoller.WaitForBatchRunsToComplete(2);
    }
}
