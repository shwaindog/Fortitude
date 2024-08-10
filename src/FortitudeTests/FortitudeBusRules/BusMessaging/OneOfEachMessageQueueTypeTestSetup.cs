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
using FortitudeBusRules.Messages;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging;

[NoMatchingProductionClass]
public class OneOfEachMessageQueueTypeTestSetup
{
    public const int DefaultRingPollerSize = 2550;

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(OneOfEachMessageQueueTypeTestSetup));

    public    MessageQueue         CustomQueue1 = null!;
    protected RouteSelectionResult CustomQueue1SelectionResult;
    public    MessageQueue         EventQueue1 = null!;
    protected RouteSelectionResult EventQueue1SelectionResult;
    public    MessageBus           MessageBus = null!;

    public    NetworkInboundMessageQueue  NetworkInboundQueue1 = null!;
    protected RouteSelectionResult        NetworkInboundQueue1SelectionResult;
    public    NetworkOutboundMessageQueue NetworkOutboundQueue1 = null!;
    protected RouteSelectionResult        NetworkOutboundQueue1SelectionResult;

    public    MessageQueue         WorkerQueue1 = null!;
    protected RouteSelectionResult WorkerQueue1SelectionResult;

    protected virtual int RingPollerSize => DefaultRingPollerSize;


    [TestInitialize]
    public void SetupMessageBus()
    {
        Logger.Info("Creating test message bus.");
        OneOfQueueTypeMessageBus();
    }

    public virtual ITimerProvider ResolverTimerProvider() => new RealTimerProvider();

    public MessageBus OneOfQueueTypeMessageBus()
    {
        var defaultQueuesConfig = new QueuesConfig
        {
            DefaultQueueSize = RingPollerSize
          , EventQueueSize   = RingPollerSize

          , RequiredNetworkInboundQueues  = 1
          , RequiredNetworkOutboundQueues = 1
        };
        TimerContext.Provider = ResolverTimerProvider();
        var timer
            = TimerContext
                .CreateUpdateableTimer
                    ("FortitudeTests.FortitudeBusRules.BusMessaging.Pipelines.OneOfEachEventQueueTypeTestSetup.ThreadPoolTimer");

        MessageBus = new MessageBus(evtBus =>
        {
            EventQueue1  = new MessageQueue(evtBus, MessageQueueType.Event, 1, RingPoller("EventQueue"));
            WorkerQueue1 = new MessageQueue(evtBus, MessageQueueType.Worker, 1, RingPoller("WorkerQueue"));
            CustomQueue1 = new MessageQueue(evtBus, MessageQueueType.Custom, 1, RingPoller("CustomQueue"));

            NetworkOutboundQueue1 = new NetworkOutboundMessageQueue(evtBus, MessageQueueType.NetworkOutbound, 1, SocketSenderRingPoller());
            NetworkInboundQueue1  = new NetworkInboundMessageQueue(evtBus, MessageQueueType.NetworkInbound, 1, SocketListenerRingPoller(timer));

            var eventGroupContainer = new MessageQueueGroupContainer
                (evtBus, evBus => new MessageQueueTypeGroup
                     (evtBus, MessageQueueType.Event, defaultQueuesConfig)
                     { EventQueue1 }
               , wrkBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Worker, defaultQueuesConfig)
                     { WorkerQueue1 }
               , ioInBus => new SocketListenerMessageQueueGroup(evtBus, MessageQueueType.NetworkInbound, defaultQueuesConfig)
                     { NetworkInboundQueue1 }
               , ioOutBus => new SocketSenderMessageQueueGroup(evtBus, MessageQueueType.NetworkOutbound, defaultQueuesConfig)
                     { NetworkOutboundQueue1 }
               , cstBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Custom, defaultQueuesConfig)
                     { CustomQueue1 }
                );
            return eventGroupContainer;
        });
        EventQueue1SelectionResult  = new RouteSelectionResult(EventQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        WorkerQueue1SelectionResult = new RouteSelectionResult(WorkerQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        CustomQueue1SelectionResult = new RouteSelectionResult(CustomQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        NetworkOutboundQueue1SelectionResult
            = new RouteSelectionResult(NetworkOutboundQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        NetworkInboundQueue1SelectionResult
            = new RouteSelectionResult(NetworkInboundQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        MessageBus.Start();
        return MessageBus;
    }

    [TestCleanup]
    public void TearDownMessageBus()
    {
        MessageBus.Stop();
        Logger.Info("OneOfEachMessageQueueTypeTestSetup MessageBus stopped");
    }

    protected IAsyncValueTaskRingPoller<BusMessage> RingPoller
        (string name) =>
        new AsyncValueTaskRingPoller<BusMessage>(PollingRing(name, RingPollerSize), 1);

    protected ISocketSenderMessageQueueRingPoller SocketSenderRingPoller() =>
        new SocketAsyncValueTaskEventQueueSender(PollingRing("MessageBusSocketSender", RingPollerSize), 1);

    protected ISocketListenerMessageQueueRingPoller SocketListenerRingPoller(IUpdateableTimer timer) =>
        new SocketAsyncValueTaskEventQueueListener(PollingRing("MessageBusSocketListener", RingPollerSize), 1, new SocketSelector(1), timer);

    protected IAsyncValueTaskPollingRing<BusMessage> PollingRing(string name, int size)
    {
        return new AsyncValueTaskPollingRing<BusMessage>
            (name, size, () => new BusMessage(), ClaimStrategyType.MultiProducers, null, false);
    }
}
