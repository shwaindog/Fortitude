using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public class EnqueueBatchPollingRing<T> : EnumerableBatchPollingRing<T>, IEnqueuePollingRing<T> where T : class, ITransferState<T>
{
    public EnqueueBatchPollingRing
        (string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType, bool logErrors = true)
        : base(name, size, dataFactory, claimStrategyType, logErrors)
    {
    }

    public EnqueueBatchPollingRing(string name, T[] existing, ClaimStrategyType claimStrategyType, bool logErrors = true)
        : base(name, existing, claimStrategyType, logErrors)
    {
    }

    public int Enqueue(T toQueue)
    {
        var seqId = Claim();
        var evt   = this[seqId];

        evt.CopyFrom(toQueue);

        Publish(seqId);
        return seqId;
    }
}

public class EnqueueBatchPollingRingLong<T> : EnumerableBatchPollingRingLong<T>, IEnqueuePollingRingLong<T> where T : class, ITransferState<T>
{
    public EnqueueBatchPollingRingLong
        (string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType, bool logErrors = true)
        : base(name, size, dataFactory, claimStrategyType, logErrors)
    {
    }

    public EnqueueBatchPollingRingLong(string name, T[] existing, ClaimStrategyType claimStrategyType, bool logErrors = true)
        : base(name, existing, claimStrategyType, logErrors)
    {
    }

    public long Enqueue(T toQueue)
    {
        var seqId = Claim();
        var evt   = this[seqId];

        evt.CopyFrom(toQueue);

        Publish(seqId);
        return seqId;
    }
}