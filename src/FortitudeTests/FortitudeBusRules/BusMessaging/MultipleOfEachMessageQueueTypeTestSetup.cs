// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Config;
using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeCommon.Chronometry.Timers;
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

    protected RouteSelectionResult CustomQueue1SelectionResult;
    public    MessageQueue         CustomQueue2 = null!;
    protected RouteSelectionResult CustomQueue2SelectionResult;
    public    MessageQueue         CustomQueue3 = null!;
    protected RouteSelectionResult CustomQueue3SelectionResult;
    public    MessageQueue         EventQueue1 = null!;
    protected RouteSelectionResult EventQueue1SelectionResult;
    public    MessageQueue         EventQueue2 = null!;
    protected RouteSelectionResult EventQueue2SelectionResult;
    public    MessageQueue         EventQueue3 = null!;
    protected RouteSelectionResult EventQueue3SelectionResult;
    public    MessageBus           MessageBus = null!;

    public    NetworkInboundMessageQueue  NetworkInboundQueue1 = null!;
    protected RouteSelectionResult        NetworkInboundQueue1SelectionResult;
    public    NetworkInboundMessageQueue  NetworkInboundQueue2 = null!;
    protected RouteSelectionResult        NetworkInboundQueue2SelectionResult;
    public    NetworkInboundMessageQueue  NetworkInboundQueue3 = null!;
    protected RouteSelectionResult        NetworkInboundQueue3SelectionResult;
    public    NetworkOutboundMessageQueue NetworkOutboundQueue1 = null!;
    protected RouteSelectionResult        NetworkOutboundQueue1SelectionResult;
    public    NetworkOutboundMessageQueue NetworkOutboundQueue2 = null!;
    protected RouteSelectionResult        NetworkOutboundQueue2SelectionResult;
    public    NetworkOutboundMessageQueue NetworkOutboundQueue3 = null!;
    protected RouteSelectionResult        NetworkOutboundQueue3SelectionResult;

    public    MessageQueue         WorkerQueue1 = null!;
    protected RouteSelectionResult WorkerQueue1SelectionResult;
    public    MessageQueue         WorkerQueue2 = null!;
    protected RouteSelectionResult WorkerQueue2SelectionResult;
    public    MessageQueue         WorkerQueue3 = null!;
    protected RouteSelectionResult WorkerQueue3SelectionResult;

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

            NetworkOutboundQueue1
                = new NetworkOutboundMessageQueue(evtBus, MessageQueueType.NetworkOutbound, 1, SocketSenderRingPoller("SocketSenderQueue1"));
            NetworkOutboundQueue2
                = new NetworkOutboundMessageQueue(evtBus, MessageQueueType.NetworkOutbound, 2, SocketSenderRingPoller("SocketSenderQueue2"));
            NetworkOutboundQueue3
                = new NetworkOutboundMessageQueue(evtBus, MessageQueueType.NetworkOutbound, 3, SocketSenderRingPoller("SocketSenderQueue3"));

            NetworkInboundQueue1
                = new NetworkInboundMessageQueue(evtBus, MessageQueueType.NetworkInbound, 1, SocketListenerRingPoller("SocketListenerQueue1", timer));
            NetworkInboundQueue2
                = new NetworkInboundMessageQueue(evtBus, MessageQueueType.NetworkInbound, 2, SocketListenerRingPoller("SocketListenerQueue2", timer));
            NetworkInboundQueue3
                = new NetworkInboundMessageQueue(evtBus, MessageQueueType.NetworkInbound, 3, SocketListenerRingPoller("SocketListenerQueue3", timer));

            var eventGroupContainer = new MessageQueueGroupContainer
                (evtBus, evBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Event, defaultQueuesConfig)
                     { EventQueue1, EventQueue2, EventQueue3 }
               , wrkBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Worker, defaultQueuesConfig)
                     { WorkerQueue1, WorkerQueue2, WorkerQueue3 }
               , ioInBus => new SocketListenerMessageQueueGroup(evtBus, MessageQueueType.NetworkInbound, defaultQueuesConfig)
                     { NetworkInboundQueue1, NetworkInboundQueue2, NetworkInboundQueue3 }
               , ioOutBus => new SocketSenderMessageQueueGroup(evtBus, MessageQueueType.NetworkOutbound, defaultQueuesConfig)
                     { NetworkOutboundQueue1, NetworkOutboundQueue2, NetworkOutboundQueue3 }
               , cstBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Custom, defaultQueuesConfig)
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
        NetworkOutboundQueue1SelectionResult
            = new RouteSelectionResult(NetworkOutboundQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        NetworkOutboundQueue2SelectionResult
            = new RouteSelectionResult(NetworkOutboundQueue2, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        NetworkOutboundQueue3SelectionResult
            = new RouteSelectionResult(NetworkOutboundQueue3, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        NetworkInboundQueue1SelectionResult
            = new RouteSelectionResult(NetworkInboundQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        NetworkInboundQueue2SelectionResult
            = new RouteSelectionResult(NetworkInboundQueue2, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        NetworkInboundQueue3SelectionResult
            = new RouteSelectionResult(NetworkInboundQueue3, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        MessageBus.Start();
        return MessageBus;
    }

    protected IMessagePump RingPoller(string name) => new MessagePump(PollingRing(name), 1);

    protected ISocketSenderMessageQueueRingPoller SocketSenderRingPoller(string name) =>
        new SocketAsyncValueTaskEventQueueSender(PollingRing(name), 1);

    protected ISocketListenerMessageQueueRingPoller SocketListenerRingPoller(string name, IUpdateableTimer timer) =>
        new SocketAsyncValueTaskEventQueueListener(PollingRing(name), 1, new SocketSelector(1), timer);

    protected IQueueMessageRing PollingRing(string name, int size = AsyncRingPollerSize) => new QueueMessageRing(name, size);

    [TestCleanup]
    public void TearDownMessageBus()
    {
        MessageBus.Stop();
        Logger.Info("MultipleOfEachMessageQueueTypeTestSetup MessageBus stopped");
    }
}
