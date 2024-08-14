// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public abstract class SocketAsyncValueTaskRingPollerListener<T> : AsyncValueTaskRingPoller<T>, ISocketDispatcherListener
    where T : class, ICanCarryTaskCallbackPayload, ICanCarrySocketReceiverPayload
{
    protected readonly IFLogger Logger;

    private readonly   IIntraOSThreadSignal     manualResetEvent;
    protected readonly IOSParallelController    ParallelController;
    private readonly   SocketsPollerAndDecoding socketsPollerAndDecoding;

    protected SocketAsyncValueTaskRingPollerListener
    (IAsyncValueTaskPollingRing<T> queueMessageRing, uint noDataPauseTimeoutMs, ISocketSelector selector,
        IUpdateableTimer updateable, Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : base(queueMessageRing, noDataPauseTimeoutMs, threadStartInitialization, parallelController)
    {
        queueMessageRing.InterceptHandler = RingPollerHandledMessage;
        ParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
        manualResetEvent = ParallelController.AllWaitingOSThreadActivateSignal(false);
        Name = Ring.Name + "-SocketAsyncValueTaskRingPollerListener";
        socketsPollerAndDecoding = new SocketsPollerAndDecoding(Name, selector, manualResetEvent, new ActionListTimer(updateable, Recycler));
        Logger = FLoggerFactory.Instance.GetLogger("FortitudeIO.Transports.Network.Dispatcher.SocketAsyncValueTaskRingPollerListener." + Name);
    }

    public override void Stop()
    {
        manualResetEvent.Set();
        base.Stop();
    }

    public void RegisterForListen(ISocketReceiver receiver)
    {
        EnqueueSocketReceiver(receiver, true);
    }

    public void UnregisterForListen(ISocketReceiver receiver)
    {
        EnqueueSocketReceiver(receiver, false);
    }

    public override int UsageCount => socketsPollerAndDecoding.CountRegisteredReceivers;

    public void RegisterForListen(IStreamListener receiver)
    {
        if (receiver is ISocketReceiver socketReceiver) RegisterForListen(socketReceiver);
    }

    public void UnregisterForListen(IStreamListener receiver)
    {
        if (receiver is ISocketReceiver socketReceiver)
        {
            socketReceiver.ListenActive = false;
            UnregisterForListen(socketReceiver);
        }
    }

    protected void RunPolling()
    {
        if (!IsRunning) Start();
        manualResetEvent.Set();
    }

    protected abstract void EnqueueSocketReceiver(ISocketReceiver receiver, bool isAdd);

    protected override bool RingPollerHandledMessage(T data)
    {
        if (!data.IsSocketReceiverItem) return false;
        try
        {
            var socketReceiver = data.SocketReceiver;
            if (socketReceiver != null)
            {
                if (data.IsSocketAdd)
                {
                    // Logger.Info("Processed socket receiver add {0} on {1}", socketReceiver.Name, Ring.Name);
                    socketsPollerAndDecoding.AddForListen(socketReceiver);
                }
                else
                {
                    socketsPollerAndDecoding.RemoveFromListen(socketReceiver);
                    socketReceiver.UnregisteredHandler();
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Warn($"SocketRingPollerListener '{Ring.Name}' caught exception while processing event: {data}.  {ex}");
        }

        return true;
    }

    protected override void BeforeProcessingEvents()
    {
        try
        {
            socketsPollerAndDecoding.PollSocketsAndDecodeData();
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception polling sockets on {0}. Got {1}", Ring.Name, ex);
        }
    }
}

public enum AsyncTaskSocketReceiverCommand
{
    NotSet
  , Add
  , Remove
  , TaskCallback
}

public class SimpleSocketAsyncValueTaskRingPollerListener : SocketAsyncValueTaskRingPollerListener<SimpleSocketReceiverPayload>
{
    public SimpleSocketAsyncValueTaskRingPollerListener
    (IAsyncValueTaskPollingRing<SimpleSocketReceiverPayload> queueMessageRing
      , uint noDataPauseTimeoutMs
      , ISocketSelector selector, IUpdateableTimer timer, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
        : base(queueMessageRing, noDataPauseTimeoutMs, selector, timer, threadStartInitialization, parallelController) { }

    public SimpleSocketAsyncValueTaskRingPollerListener
    (string name, uint noDataPauseTimeoutMs, ISocketSelector selector
      , IUpdateableTimer timer, Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : base(new AsyncValueTaskPollingRing<SimpleSocketReceiverPayload>(name, 10_000
                                                                        , () => new SimpleSocketReceiverPayload(),
                                                                          ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, selector, timer
             , threadStartInitialization, parallelController) { }


    protected override void EnqueueSocketReceiver(ISocketReceiver receiver, bool isAdd)
    {
        var seqId                = Ring.Claim();
        var socketReceiverUpdate = Ring[seqId];
        socketReceiverUpdate.SetAsSocketReceiverItem(receiver, isAdd);
        Ring.Publish(seqId);
        RunPolling();
    }
}
