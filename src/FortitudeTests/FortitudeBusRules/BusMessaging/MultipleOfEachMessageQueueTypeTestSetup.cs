#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.Config;
using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeBusRules.Messages;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging;

[NoMatchingProductionClass]
public class MultipleOfEachMessageQueueTypeTestSetup
{
    public MessageQueue CustomQueue1 = null!;
    public MessageQueue CustomQueue2 = null!;
    public MessageQueue CustomQueue3 = null!;
    public MessageQueue EventQueue1 = null!;
    public MessageQueue EventQueue2 = null!;
    public MessageQueue EventQueue3 = null!;
    public IOInboundMessageQueue IOInboundQueue1 = null!;
    public IOInboundMessageQueue IOInboundQueue2 = null!;
    public IOInboundMessageQueue IOInboundQueue3 = null!;
    public IOOutboundMessageQueue IOOutboundQueue1 = null!;
    public IOOutboundMessageQueue IOOutboundQueue2 = null!;
    public IOOutboundMessageQueue IOOutboundQueue3 = null!;
    public MessageBus MessageBus = null!;
    public MessageQueue WorkerQueue1 = null!;
    public MessageQueue WorkerQueue2 = null!;
    public MessageQueue WorkerQueue3 = null!;

    [TestInitialize]
    public void SetupEventQueue()
    {
        OneOfQueueTypeMessageBus();
    }

    public MessageBus OneOfQueueTypeMessageBus()
    {
        var defaultQueuesConfig = new QueuesConfig();
        defaultQueuesConfig.DefaultQueueSize = 32;
        defaultQueuesConfig.EventQueueSize = 32;
        var timer = new UpdateableTimer(
            "FortitudeTests.FortitudeBusRules.BusMessaging.Pipelines.MultipleOfEachEventQueueTypeTestSetup.ThreadPoolTimer");

        MessageBus = new MessageBus(evtBus =>
        {
            EventQueue1 = new MessageQueue(evtBus, MessageQueueType.Event, 1, RingPoller("EventQueue1"));
            EventQueue2 = new MessageQueue(evtBus, MessageQueueType.Event, 2, RingPoller("EventQueue2"));
            EventQueue3 = new MessageQueue(evtBus, MessageQueueType.Event, 3, RingPoller("EventQueue2"));
            WorkerQueue1 = new MessageQueue(evtBus, MessageQueueType.Worker, 1, RingPoller("WorkerQueue1"));
            WorkerQueue2 = new MessageQueue(evtBus, MessageQueueType.Worker, 2, RingPoller("WorkerQueue2"));
            WorkerQueue3 = new MessageQueue(evtBus, MessageQueueType.Worker, 3, RingPoller("WorkerQueue3"));
            CustomQueue1 = new MessageQueue(evtBus, MessageQueueType.Custom, 1, RingPoller("CustomQueue1"));
            CustomQueue2 = new MessageQueue(evtBus, MessageQueueType.Custom, 2, RingPoller("CustomQueue2"));
            CustomQueue3 = new MessageQueue(evtBus, MessageQueueType.Custom, 3, RingPoller("CustomQueue3"));
            IOOutboundQueue1 = new IOOutboundMessageQueue(evtBus, MessageQueueType.IOOutbound, 1, SocketSenderRingPoller("SocketSenderQueue1"));
            IOOutboundQueue2 = new IOOutboundMessageQueue(evtBus, MessageQueueType.IOOutbound, 2, SocketSenderRingPoller("SocketSenderQueue2"));
            IOOutboundQueue3 = new IOOutboundMessageQueue(evtBus, MessageQueueType.IOOutbound, 3, SocketSenderRingPoller("SocketSenderQueue3"));
            IOInboundQueue1
                = new IOInboundMessageQueue(evtBus, MessageQueueType.IOInbound, 1, SocketListenerRingPoller("SocketListenerQueue1", timer));
            IOInboundQueue2
                = new IOInboundMessageQueue(evtBus, MessageQueueType.IOInbound, 2, SocketListenerRingPoller("SocketListenerQueue2", timer));
            IOInboundQueue3
                = new IOInboundMessageQueue(evtBus, MessageQueueType.IOInbound, 3, SocketListenerRingPoller("SocketListenerQueue3", timer));

            var eventGroupContainer = new MessageQueueGroupContainer(evtBus,
                evBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Event, new Recycler(), defaultQueuesConfig)
                    { EventQueue1, EventQueue2, EventQueue3 },
                wrkBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Worker, new Recycler(), defaultQueuesConfig)
                    { WorkerQueue1, WorkerQueue2, WorkerQueue3 },
                ioInBus => new SocketListenerMessageQueueGroup(evtBus, MessageQueueType.IOInbound, new Recycler(), defaultQueuesConfig)
                    { IOInboundQueue1, IOInboundQueue2, IOInboundQueue3 },
                ioOutBus => new SocketSenderMessageQueueGroup(evtBus, MessageQueueType.IOOutbound, new Recycler(), defaultQueuesConfig)
                    { IOOutboundQueue1, IOOutboundQueue2, IOOutboundQueue3 },
                cstBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Custom, new Recycler(), defaultQueuesConfig)
                    { CustomQueue1, CustomQueue2, CustomQueue3 });
            return eventGroupContainer;
        });
        MessageBus.Start();
        return MessageBus;
    }

    protected IAsyncValueTaskRingPoller<BusMessage> RingPoller(string name) => new AsyncValueTaskRingPoller<BusMessage>(PollingRing(name), 1);

    protected ISocketSenderMessageQueueRingPoller SocketSenderRingPoller(string name) =>
        new SocketAsyncValueTaskEventQueueSender(PollingRing(name), 1);

    protected ISocketListenerMessageQueueRingPoller SocketListenerRingPoller(string name, IUpdateableTimer timer) =>
        new SocketAsyncValueTaskEventQueueListener(PollingRing(name), 1, new SocketSelector(1), timer);

    protected IAsyncValueTaskPollingRing<BusMessage> PollingRing(string name, int size = 32)
    {
        return new AsyncValueValueTaskPollingRing<BusMessage>(
            name,
            12,
            () => new BusMessage(),
            ClaimStrategyType.MultiProducers, null, null, false);
    }
}
