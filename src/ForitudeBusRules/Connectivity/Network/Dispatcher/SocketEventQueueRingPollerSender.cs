#region

using FortitudeBusRules.Messaging;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Dispatcher;

public class SocketEventQueueRingPollerSender : SocketRingPollerSender<Message>, IRingPollerSink<Message>
{
    private IRecycler? recycler;

    public SocketEventQueueRingPollerSender(IPollingRing<Message> ring, uint noDataPauseTimeoutMs, IRecycler? recycler = null
        , IPollSink<Message>? pollSink = null,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, threadStartInitialization, parallelController)
    {
        PollSink = pollSink;
        this.recycler = recycler;
    }

    public SocketEventQueueRingPollerSender(string name, int size, uint noDataPauseTimeoutMs, IRecycler? recycler = null
        , IPollSink<Message>? pollSink = null,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : this(new PollingRing<Message>(name, size, () => new Message(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, recycler, pollSink, threadStartInitialization, parallelController) { }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public IPollSink<Message>? PollSink { get; set; }

    public override void AddToSendQueue(ISocketSender ss)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        var payLoadContainer = Recycler.Borrow<PayLoad<ISocketSender>>();
        payLoadContainer.Body = ss;
        evt.Type = MessageType.SendToRemote;
        evt.PayLoad = payLoadContainer;
        evt.DestinationAddress = "NotRequired";
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = null;
        evt.SentTime = TimeContext.UtcNow;
        evt.ProcessorRegistry = null;
        evt.RuleFilter = Message.AppliesToAll;
        Ring.Publish(seqId);
        WakeIfAsleep();
        if (!IsRunning) Start();
    }

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, Message data
        , bool ringStartOfBatch, bool ringEndOfBatch)
    {
        if (data.Type == MessageType.SendToRemote)
            PutQueuedSenderMessageOnSocket((ISocketSender)data.PayLoad!.BodyObj!);
        else
            PollSink?.Processor(ringCurrentSequence, ringCurrentBatchSize, data, ringStartOfBatch, ringEndOfBatch);
    }
}
