﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public abstract class SocketAsyncValueTaskRingPollerSender<T> : AsyncValueTaskRingPoller<T>, ISocketDispatcherSender
    where T : class, ICanCarryTaskCallbackPayload, ICanCarrySocketSenderPayload
{
    protected readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SocketBatchEnumerableRingPollerSender<>));

    protected readonly IOSParallelController ParallelController;

    protected SocketAsyncValueTaskRingPollerSender
    (IAsyncValueTaskPollingRing<T> ring, uint emptyQueueMaxSleepMs
      , Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : base(ring, emptyQueueMaxSleepMs, threadStartInitialization, parallelController)
    {
        ring.InterceptHandler = RingPollerHandledMessage;

        Name = Ring.Name + "-SocketRingPollerSender";

        ParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
    }

    public abstract void EnqueueSocketSender(ISocketSender socketSender);

    protected override bool RingPollerHandledMessage(T data)
    {
        if (!data.IsSocketSenderItem) return false;
        if (UsageCount <= 0) return true;

        var ss = data.SocketSender!;
        try
        {
            var sent = ss.SendQueued();
            if (ss.AttemptCloseOnSendComplete)
                ss.SendExpectSessionCloseMessageAndClose();
            else if (!sent) EnqueueSocketSender(ss);
        }
        catch (SocketSendException se)
        {
            se.SocketSessionContext.OnSessionFailure($"Caught socket send error {se}");
        }

        return true;
    }
}

public class SimpleAsyncValueTaskSocketRingPollerSender : SocketAsyncValueTaskRingPollerSender<SimpleSocketSenderPayload>
{
    public SimpleAsyncValueTaskSocketRingPollerSender
    (IAsyncValueTaskPollingRing<SimpleSocketSenderPayload> ring
      , uint emptyQueueMaxSleepMs
      , Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : base(ring, emptyQueueMaxSleepMs, threadStartInitialization, parallelController) { }

    public SimpleAsyncValueTaskSocketRingPollerSender
    (string name, uint emptyQueueMaxSleepMs, Action? threadStartInitialization = null
      , IOSParallelController? parallelController = null)
        : base(new AsyncValueTaskPollingRing<SimpleSocketSenderPayload>
                   (name, 10_000, () => new SimpleSocketSenderPayload(), ClaimStrategyType.MultiProducers)
             , emptyQueueMaxSleepMs, threadStartInitialization, parallelController) { }

    public override void EnqueueSocketSender(ISocketSender socketSender)
    {
        var seqId = Ring.Claim();
        var evt   = Ring[seqId];
        evt.SetAsSocketSenderItem(socketSender);
        Ring.Publish(seqId);
        WakeIfAsleep();
        if (!IsRunning) Start();
    }
}
