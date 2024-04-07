#region

using FortitudeBusRules.Messaging;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Dispatcher;

public class SocketEventQueueRingPollerListener : SocketRingPollerListener<Message>, IRingPollerSink<Message>
{
    private IRecycler? recycler;

    public SocketEventQueueRingPollerListener(IPollingRing<Message> ring, uint noDataPauseTimeoutMs, ISocketSelector selector
        , IRecycler? recycler = null, IPollSink<Message>? pollSink = null, Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, selector, threadStartInitialization, parallelController)
    {
        PollSink = pollSink;
        this.recycler = recycler;
    }

    public SocketEventQueueRingPollerListener(string name, int size, uint noDataPauseTimeoutMs, ISocketSelector selector
        , IRecycler? recycler = null, IPollSink<Message>? pollSink = null, Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : this(new PollingRing<Message>(name, size, () => new Message(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, selector, recycler, pollSink, threadStartInitialization, parallelController) { }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public IPollSink<Message>? PollSink { get; set; }

    public override void RegisterForListen(ISocketReceiver receiver)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        var payLoadContainer = Recycler.Borrow<PayLoad<ISocketReceiver>>();
        payLoadContainer.Body = receiver;
        evt.Type = MessageType.SendToRemote;
        evt.PayLoad = payLoadContainer;
        evt.DestinationAddress = "NotRequired";
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = null;
        evt.SentTime = TimeContext.UtcNow;
        evt.ProcessorRegistry = null;
        evt.RuleFilter = Message.AppliesToAll;
        Ring.Publish(seqId);
        RunPolling();
    }

    public override void UnregisterForListen(ISocketReceiver receiver)
    {
        receiver.ListenActive = false;
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        var payLoadContainer = Recycler.Borrow<PayLoad<ISocketReceiver>>();
        payLoadContainer.Body = receiver;
        evt.Type = MessageType.SendToRemote;
        evt.PayLoad = payLoadContainer;
        evt.DestinationAddress = "NotRequired";
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = null;
        evt.SentTime = TimeContext.UtcNow;
        evt.ProcessorRegistry = null;
        evt.RuleFilter = Message.AppliesToAll;
        Ring.Publish(seqId);
        RunPolling();
    }

    protected override bool IsSocketReceiverInRingData(Message ringData) =>
        ringData.Type is MessageType.AddWatchSocket or MessageType.RemoveWatchSocket;

    protected override bool IsSocketReceiverAdd(Message ringData) => ringData.Type == MessageType.AddWatchSocket;

    protected override ISocketReceiver? ExtractSocketReceiverInRingData(Message ringData) => (ISocketReceiver)ringData.PayLoad!.BodyObj!;

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, Message data, bool ringStartOfBatch, bool ringEndOfBatch)
    {
        PollSink?.Processor(ringCurrentSequence, ringCurrentBatchSize, data, ringStartOfBatch, ringEndOfBatch);
    }
}
