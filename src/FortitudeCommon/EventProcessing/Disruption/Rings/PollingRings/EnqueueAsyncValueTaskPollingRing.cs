// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IEnqueueAsyncValueTaskPollingRing<T> : IAsyncValueTaskPollingRing<T>, IEnqueueTaskCallbackPollingRing<T> 
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload
{
}
public interface IEnqueueAsyncValueTaskPollingRingLong<T> : IAsyncValueTaskPollingRingLong<T>, IEnqueueTaskCallbackPollingRingLong<T> 
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload
{
}

public class EnqueueAsyncValueTaskPollingRing<T>
    (string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType, bool logErrors = true)
    : AsyncValueTaskPollingRing<T>(name, size, dataFactory, claimStrategyType, logErrors), IEnqueueAsyncValueTaskPollingRing<T>
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload
{
    public int Enqueue(T toQueue)
    {
        var seqId = Claim();
        var evt   = this[seqId];

        evt.CopyFrom(toQueue);

        Publish(seqId);
        return seqId;
    }
}

public class EnqueueAsyncValueTaskPollingRingLong<T>
    (string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType, bool logErrors = true)
    : AsyncValueTaskPollingRingLong<T>(name, size, dataFactory, claimStrategyType, logErrors), IEnqueueAsyncValueTaskPollingRingLong<T>
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload
{
    public long Enqueue(T toQueue)
    {
        var seqId = Claim();
        var evt   = this[seqId];

        evt.CopyFrom(toQueue);

        Publish(seqId);
        return seqId;
    }
}
