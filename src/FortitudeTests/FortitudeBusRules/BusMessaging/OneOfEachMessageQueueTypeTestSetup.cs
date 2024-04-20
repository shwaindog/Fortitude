#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
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
public class OneOfEachMessageQueueTypeTestSetup
{
    public MessageQueue CustomQueue = null!;
    protected RouteSelectionResult CustomQueueSelectionResult;
    public MessageQueue EventQueue = null!;
    protected RouteSelectionResult EventQueueSelectionResult;
    public IOInboundMessageQueue IOInboundQueue = null!;
    protected RouteSelectionResult IOInboundQueueSelectionResult;
    public IOOutboundMessageQueue IOOutboundQueue = null!;
    protected RouteSelectionResult IOOutboundQueueSelectionResult;
    public MessageBus MessageBus = null!;
    public MessageQueue WorkerQueue = null!;
    protected RouteSelectionResult WorkerQueueSelectionResult;

    [TestInitialize]
    public void SetupMessageBus()
    {
        OneOfQueueTypeMessageBus();
    }

    public MessageBus OneOfQueueTypeMessageBus()
    {
        var defaultQueuesConfig = new QueuesConfig();
        defaultQueuesConfig.DefaultQueueSize = 32;
        defaultQueuesConfig.EventQueueSize = 32;
        defaultQueuesConfig.RequiredIOInboundQueues = 1;
        defaultQueuesConfig.RequiredIOOutboundQueues = 1;
        var timer = new UpdateableTimer("FortitudeTests.FortitudeBusRules.BusMessaging.Pipelines.OneOfEachEventQueueTypeTestSetup.ThreadPoolTimer");

        MessageBus = new MessageBus(evtBus =>
        {
            EventQueue = new MessageQueue(evtBus, MessageQueueType.Event, 1, RingPoller("EventQueue"));
            WorkerQueue = new MessageQueue(evtBus, MessageQueueType.Worker, 1, RingPoller("WorkerQueue"));
            CustomQueue = new MessageQueue(evtBus, MessageQueueType.Custom, 1, RingPoller("CustomQueue"));
            IOOutboundQueue = new IOOutboundMessageQueue(evtBus, MessageQueueType.IOOutbound, 1, SocketSenderRingPoller());
            IOInboundQueue = new IOInboundMessageQueue(evtBus, MessageQueueType.IOInbound, 1, SocketListenerRingPoller(timer));

            var eventGroupContainer = new MessageQueueGroupContainer(evtBus,
                evBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Event, new Recycler(), defaultQueuesConfig)
                    { EventQueue },
                wrkBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Worker, new Recycler(), defaultQueuesConfig)
                    { WorkerQueue },
                ioInBus => new SocketListenerMessageQueueGroup(evtBus, MessageQueueType.IOInbound, new Recycler(), defaultQueuesConfig)
                    { IOInboundQueue },
                ioOutBus => new SocketSenderMessageQueueGroup(evtBus, MessageQueueType.IOOutbound, new Recycler(), defaultQueuesConfig)
                    { IOOutboundQueue },
                cstBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Custom, new Recycler(), defaultQueuesConfig)
                    { CustomQueue }
            );
            return eventGroupContainer;
        });
        EventQueueSelectionResult = new RouteSelectionResult(EventQueue, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        WorkerQueueSelectionResult = new RouteSelectionResult(WorkerQueue, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        CustomQueueSelectionResult = new RouteSelectionResult(CustomQueue, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOOutboundQueueSelectionResult = new RouteSelectionResult(IOOutboundQueue, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOInboundQueueSelectionResult = new RouteSelectionResult(IOInboundQueue, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        MessageBus.Start();
        return MessageBus;
    }

    [TestCleanup]
    public void TearDownMessageQueue()
    {
        MessageBus.Stop();
    }

    protected IAsyncValueTaskRingPoller<BusMessage> RingPoller(string name) => new AsyncValueTaskRingPoller<BusMessage>(PollingRing(name), 1);

    protected ISocketSenderMessageQueueRingPoller SocketSenderRingPoller() =>
        new SocketAsyncValueTaskEventQueueSender(PollingRing("SocketSender"), 1);

    protected ISocketListenerMessageQueueRingPoller SocketListenerRingPoller(IUpdateableTimer timer) =>
        new SocketAsyncValueTaskEventQueueListener(PollingRing("SocketListener"), 1, new SocketSelector(1), timer);

    protected IAsyncValueTaskPollingRing<BusMessage> PollingRing(string name, int size = 32)
    {
        return new AsyncValueValueTaskPollingRing<BusMessage>(
            name,
            12,
            () => new BusMessage(),
            ClaimStrategyType.MultiProducers, null, null, false);
    }
}
