// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

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
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging;

[NoMatchingProductionClass]
public class MultipleOfEachMessageQueueTypeTestSetup
{
    public const int AsyncRingPollerSize = 255;

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MultipleOfEachMessageQueueTypeTestSetup));

    public MessageQueue CustomQueue1 = null!;


    protected RouteSelectionResult   CustomQueue1SelectionResult;
    public    MessageQueue           CustomQueue2 = null!;
    protected RouteSelectionResult   CustomQueue2SelectionResult;
    public    MessageQueue           CustomQueue3 = null!;
    protected RouteSelectionResult   CustomQueue3SelectionResult;
    public    MessageQueue           EventQueue1 = null!;
    protected RouteSelectionResult   EventQueue1SelectionResult;
    public    MessageQueue           EventQueue2 = null!;
    protected RouteSelectionResult   EventQueue2SelectionResult;
    public    MessageQueue           EventQueue3 = null!;
    protected RouteSelectionResult   EventQueue3SelectionResult;
    public    IOInboundMessageQueue  IOInboundQueue1 = null!;
    protected RouteSelectionResult   IOInboundQueue1SelectionResult;
    public    IOInboundMessageQueue  IOInboundQueue2 = null!;
    protected RouteSelectionResult   IOInboundQueue2SelectionResult;
    public    IOInboundMessageQueue  IOInboundQueue3 = null!;
    protected RouteSelectionResult   IOInboundQueue3SelectionResult;
    public    IOOutboundMessageQueue IOOutboundQueue1 = null!;
    protected RouteSelectionResult   IOOutboundQueue1SelectionResult;
    public    IOOutboundMessageQueue IOOutboundQueue2 = null!;
    protected RouteSelectionResult   IOOutboundQueue2SelectionResult;
    public    IOOutboundMessageQueue IOOutboundQueue3 = null!;
    protected RouteSelectionResult   IOOutboundQueue3SelectionResult;
    public    MessageBus             MessageBus   = null!;
    public    MessageQueue           WorkerQueue1 = null!;
    protected RouteSelectionResult   WorkerQueue1SelectionResult;
    public    MessageQueue           WorkerQueue2 = null!;
    protected RouteSelectionResult   WorkerQueue2SelectionResult;
    public    MessageQueue           WorkerQueue3 = null!;
    protected RouteSelectionResult   WorkerQueue3SelectionResult;

    [TestInitialize]
    public void SetupEventQueue()
    {
        OneOfQueueTypeMessageBus();
    }

    public MessageBus OneOfQueueTypeMessageBus()
    {
        var defaultQueuesConfig = new QueuesConfig();
        defaultQueuesConfig.DefaultQueueSize = AsyncRingPollerSize;
        defaultQueuesConfig.EventQueueSize   = AsyncRingPollerSize;
        var timer = TimerContext.CreateUpdateableTimer
            ("FortitudeTests.FortitudeBusRules.BusMessaging.Pipelines.MultipleOfEachEventQueueTypeTestSetup.ThreadPoolTimer");

        MessageBus = new MessageBus(evtBus =>
        {
            EventQueue1  = new MessageQueue(evtBus, MessageQueueType.Event, 1, RingPoller("EventQueue1"));
            EventQueue2  = new MessageQueue(evtBus, MessageQueueType.Event, 2, RingPoller("EventQueue2"));
            EventQueue3  = new MessageQueue(evtBus, MessageQueueType.Event, 3, RingPoller("EventQueue2"));
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

            var eventGroupContainer = new MessageQueueGroupContainer
                (evtBus, evBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Event, new Recycler(), defaultQueuesConfig)
                     { EventQueue1, EventQueue2, EventQueue3 }
               , wrkBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Worker, new Recycler(), defaultQueuesConfig)
                     { WorkerQueue1, WorkerQueue2, WorkerQueue3 }
               , ioInBus => new SocketListenerMessageQueueGroup(evtBus, MessageQueueType.IOInbound, new Recycler(), defaultQueuesConfig)
                     { IOInboundQueue1, IOInboundQueue2, IOInboundQueue3 }
               , ioOutBus => new SocketSenderMessageQueueGroup(evtBus, MessageQueueType.IOOutbound, new Recycler(), defaultQueuesConfig)
                     { IOOutboundQueue1, IOOutboundQueue2, IOOutboundQueue3 }
               , cstBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Custom, new Recycler(), defaultQueuesConfig)
                     { CustomQueue1, CustomQueue2, CustomQueue3 });
            return eventGroupContainer;
        });


        EventQueue1SelectionResult  = new RouteSelectionResult(EventQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        EventQueue2SelectionResult  = new RouteSelectionResult(EventQueue2, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        EventQueue3SelectionResult  = new RouteSelectionResult(EventQueue3, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        WorkerQueue1SelectionResult = new RouteSelectionResult(WorkerQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        WorkerQueue2SelectionResult = new RouteSelectionResult(WorkerQueue2, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        WorkerQueue3SelectionResult = new RouteSelectionResult(WorkerQueue3, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        CustomQueue1SelectionResult = new RouteSelectionResult(CustomQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        CustomQueue2SelectionResult = new RouteSelectionResult(CustomQueue2, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        CustomQueue3SelectionResult = new RouteSelectionResult(CustomQueue3, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOOutboundQueue1SelectionResult
            = new RouteSelectionResult(IOOutboundQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOOutboundQueue2SelectionResult
            = new RouteSelectionResult(IOOutboundQueue2, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOOutboundQueue3SelectionResult
            = new RouteSelectionResult(IOOutboundQueue3, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOInboundQueue1SelectionResult = new RouteSelectionResult(IOInboundQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOInboundQueue2SelectionResult = new RouteSelectionResult(IOInboundQueue2, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOInboundQueue3SelectionResult = new RouteSelectionResult(IOInboundQueue3, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        MessageBus.Start();
        return MessageBus;
    }

    protected IAsyncValueTaskRingPoller<BusMessage> RingPoller(string name) => new AsyncValueTaskRingPoller<BusMessage>(PollingRing(name), 1);

    protected ISocketSenderMessageQueueRingPoller SocketSenderRingPoller(string name) =>
        new SocketAsyncValueTaskEventQueueSender(PollingRing(name), 1);

    protected ISocketListenerMessageQueueRingPoller SocketListenerRingPoller(string name, IUpdateableTimer timer) =>
        new SocketAsyncValueTaskEventQueueListener(PollingRing(name), 1, new SocketSelector(1), timer);

    protected IAsyncValueTaskPollingRing<BusMessage> PollingRing(string name, int size = AsyncRingPollerSize)
    {
        return new AsyncValueValueTaskPollingRing<BusMessage>(name, size, () => new BusMessage(), ClaimStrategyType.MultiProducers);
    }

    [TestCleanup]
    public void TearDownMessageBus()
    {
        MessageBus.Stop();
        Logger.Info("MultipleOfEachMessageQueueTypeTestSetup MessageBus stopped");
    }
}
