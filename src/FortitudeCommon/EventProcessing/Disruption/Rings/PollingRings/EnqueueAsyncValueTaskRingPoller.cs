// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IEnqueueAsyncValueTaskRingPoller<T> : IAsyncValueTaskRingPoller<T>
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload
{
    new IEnqueueAsyncValueTaskPollingRing<T> Ring { get; }
}

public interface IEnqueueAsyncValueTaskRingPollerLong<T> : IAsyncValueTaskRingPollerLong<T>
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload
{
    new IEnqueueAsyncValueTaskPollingRingLong<T> Ring { get; }
}

public class EnqueueAsyncValueTaskRingPoller<T> : AsyncValueTaskRingPollerBase<T, IEnqueueAsyncValueTaskPollingRing<T>>
  , IEnqueueAsyncValueTaskRingPoller<T>
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public EnqueueAsyncValueTaskRingPoller
    (IEnqueueAsyncValueTaskPollingRing<T> ring, uint emptyQueueMaxSleepMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
        : base(ring, emptyQueueMaxSleepMs, threadStartInitialization, parallelController) { }

    IPollingRing<T> IRingPoller<T>.Ring => base.Ring;

    IAsyncValueTaskPollingRing<T> IAsyncValueTaskRingPoller<T>.Ring => base.Ring;
}

public class EnqueueAsyncValueTaskRingPollerLong<T> : AsyncValueTaskRingPollerBase<T, IEnqueueAsyncValueTaskPollingRingLong<T>>
  , IEnqueueAsyncValueTaskRingPollerLong<T>
    where T : class, ITransferState<T>, ICanCarryTaskCallbackPayload
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public EnqueueAsyncValueTaskRingPollerLong
    (IEnqueueAsyncValueTaskPollingRingLong<T> ring, uint emptyQueueMaxSleepMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
        : base(ring, emptyQueueMaxSleepMs, threadStartInitialization, parallelController) { }

    IPollingRingLong<T> IRingPollerLong<T>.Ring => base.Ring;

    IAsyncValueTaskPollingRingLong<T> IAsyncValueTaskRingPollerLong<T>.Ring => base.Ring;
}
