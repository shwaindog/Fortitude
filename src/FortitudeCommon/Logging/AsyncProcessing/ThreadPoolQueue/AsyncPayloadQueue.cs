// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

namespace FortitudeCommon.Logging.AsyncProcessing.ThreadPoolQueue;

public class AsyncPayloadQueue(string queueName, int capacity)
{
    private readonly BlockingStaticRing<FLogAsyncPayload> queue =
        new(queueName, capacity, ClaimStrategyType.MultiProducers);

    public string QueueName = queueName;

    public int Capacity => queue.RingSize;
    public int Count => queue.Count;

    public FLogAsyncPayload this[int slot] => queue[slot].Value!;

    public int Claim() => queue.Claim();

    public void Publish(int slot) => queue.Publish(slot);

    public FLogAsyncPayload Poll() => queue.Take();

    public FLogAsyncPayload? TryPoll() => queue.TryTake(out var maybeItem) ? maybeItem : null;
}
